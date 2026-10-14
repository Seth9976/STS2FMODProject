using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001E9 RID: 489
	[ScriptPath("res://src/Core/Nodes/Vfx/NRollingBoulderVfx.cs")]
	public class NRollingBoulderVfx : Node2D
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060018D3 RID: 6355 RVA: 0x0009BCE0 File Offset: 0x00099EE0
		[Nullable(1)]
		public static string[] AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new string[] { NRollingBoulderVfx._scenePath };
			}
		}

		/// <summary>
		/// Creates VFX for rolling boulder.
		/// </summary>
		/// <param name="creatures">The creatures that will be hit by the boulder.</param>
		/// <param name="damage">The amount of damage the boulder will do. Affects the size and speed of the boulder. This
		/// class does _not_ deal this damage to the creatures.</param>
		/// <param name="debugFinalPosition">If running in a test scene without NCombatRoom, this is where the creature to
		/// hit is located.</param>
		/// <returns>VFX, or null if in tests.</returns>
		// Token: 0x060018D4 RID: 6356 RVA: 0x0009BCF0 File Offset: 0x00099EF0
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NRollingBoulderVfx Create(IEnumerable<Creature> creatures, decimal damage, Vector2? debugFinalPosition = null)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRollingBoulderVfx nrollingBoulderVfx = PreloadManager.Cache.GetScene(NRollingBoulderVfx._scenePath).Instantiate<NRollingBoulderVfx>(0L);
			nrollingBoulderVfx._creatures = creatures.ToList<Creature>();
			nrollingBoulderVfx._damage = damage;
			nrollingBoulderVfx._debugFinalPosition = debugFinalPosition;
			return nrollingBoulderVfx;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0009BD38 File Offset: 0x00099F38
		public override void _Ready()
		{
			this._boulder = base.GetNode<Sprite2D>("Boulder");
			this._shadow = base.GetNode<Sprite2D>("Shadow");
			this._slamBehind = base.GetNode<GpuParticles2D>("SlamBehind");
			this._slamFront = base.GetNode<GpuParticles2D>("SlamFront");
			float num = (float)(this._damage / (this._damage + 40m));
			float num2 = num * 1.5f;
			float num3 = Mathf.Lerp(0.5f, 0.25f, num);
			float num4 = Mathf.Lerp(600f, 1000f, num);
			float num5 = Mathf.Lerp(600f, 1000f, num);
			base.Scale = Vector2.One * num2;
			foreach (GpuParticles2D gpuParticles2D in base.GetChildren(false).OfType<GpuParticles2D>())
			{
				if (!gpuParticles2D.LocalCoords)
				{
					ParticleProcessMaterial particleProcessMaterial = gpuParticles2D.ProcessMaterial as ParticleProcessMaterial;
					if (particleProcessMaterial != null)
					{
						particleProcessMaterial.Scale = Vector2.One * num2;
					}
				}
			}
			TaskHelper.RunSafely(this.PlayAnim(num3, num5, num4));
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0009BE90 File Offset: 0x0009A090
		[NullableContext(1)]
		private async Task PlayAnim(float timeToImpact, float xOffset, float rotationSpeed)
		{
			if (NCombatRoom.Instance == null || this._creatures.Count == 0)
			{
				this.CleanUpBeforeEarlyExit();
			}
			else
			{
				Vector2 initialBoulderPosition = base.GlobalPosition;
				Vector2 initialShadowOffset = this._shadow.Position;
				Vector2 initialShadowScale = this._shadow.Scale;
				Vector2 vector = Vector2.Zero;
				List<Creature> creaturesHit = new List<Creature>();
				int num = 0;
				foreach (Creature creature in this._creatures)
				{
					NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
					if (creatureNode != null)
					{
						vector += creatureNode.Visuals.GlobalPosition;
						num++;
					}
				}
				if (num == 0)
				{
					this.CleanUpBeforeEarlyExit();
				}
				else
				{
					vector /= (float)num;
					this._shadow.Reparent(NCombatRoom.Instance.BackCombatVfxContainer, true);
					if (vector == Vector2.Zero)
					{
						if (this._debugFinalPosition == null)
						{
							this.CleanUpBeforeEarlyExit();
							return;
						}
						vector = this._debugFinalPosition.Value;
					}
					Vector2 impactPoint = new Vector2(base.GetViewportRect().Size.X * 0.5f, vector.Y);
					Vector2 vector2;
					vector2..ctor(impactPoint.X - xOffset, -150f);
					float num2 = (impactPoint.X - vector2.X) / timeToImpact;
					float yAccel = 2f * (impactPoint.Y - vector2.Y) / (timeToImpact * timeToImpact);
					this._shadow.Scale = Vector2.Zero;
					base.GlobalPosition = vector2;
					Vector2 velocity = new Vector2(num2, 0f);
					float timer = 0f;
					bool firstImpact = false;
					while (timer <= 1f)
					{
						float num3 = await this.AwaitProcessFrame(default(CancellationToken));
						float num4 = num3;
						velocity.Y += yAccel * num4;
						base.GlobalPosition += velocity * num4;
						this._boulder.Rotation += Mathf.DegToRad(rotationSpeed) * num4;
						this._shadow.Scale = initialShadowScale * base.Scale * Mathf.InverseLerp(initialBoulderPosition.Y, impactPoint.Y, base.GlobalPosition.Y);
						this._shadow.GlobalPosition = new Vector2(base.GlobalPosition.X, impactPoint.Y) + initialShadowOffset * base.Scale;
						foreach (Creature creature2 in this._creatures)
						{
							if (!creaturesHit.Contains(creature2))
							{
								NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(creature2);
								if (creatureNode2 == null)
								{
									creaturesHit.Add(creature2);
								}
								else
								{
									float num5 = creatureNode2.Visuals.GlobalPosition.X - creatureNode2.Visuals.Bounds.Size.X * 0.5f;
									if (base.GlobalPosition.X >= num5)
									{
										this.EmitSignalHitCreature(creatureNode2);
										creaturesHit.Add(creature2);
									}
								}
							}
						}
						if (this._creatures.Count == creaturesHit.Count)
						{
							timer += num4;
						}
						if (base.GlobalPosition.Y >= impactPoint.Y)
						{
							if (!firstImpact)
							{
								this._slamBehind.GlobalPosition = impactPoint;
								this._slamFront.GlobalPosition = impactPoint;
								this._slamBehind.Emitting = true;
								this._slamFront.Emitting = true;
								firstImpact = true;
							}
							velocity.Y = -velocity.Y * 0.33f;
							if (Mathf.Abs(velocity.Y) < 1f)
							{
								velocity.Y = 0f;
								yAccel = 0f;
							}
							Vector2 globalPosition = base.GlobalPosition;
							globalPosition.Y = impactPoint.Y;
							base.GlobalPosition = globalPosition;
						}
					}
					base.EmitSignal(NRollingBoulderVfx.SignalName.Finished, Array.Empty<Variant>());
					this._shadow.QueueFreeSafely();
					this.QueueFreeSafely();
				}
			}
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0009BEEB File Offset: 0x0009A0EB
		private void CleanUpBeforeEarlyExit()
		{
			Log.Warn("Rolling boulder VFX spawned with no targets, disabling", 2);
			base.EmitSignal(NRollingBoulderVfx.SignalName.Finished, Array.Empty<Variant>());
			this._shadow.QueueFreeSafely();
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018D8 RID: 6360 RVA: 0x0009BF1C File Offset: 0x0009A11C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRollingBoulderVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRollingBoulderVfx.MethodName.CleanUpBeforeEarlyExit, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018D9 RID: 6361 RVA: 0x0009BF90 File Offset: 0x0009A190
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRollingBoulderVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRollingBoulderVfx.MethodName.CleanUpBeforeEarlyExit && args.Count == 0)
			{
				this.CleanUpBeforeEarlyExit();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060018DA RID: 6362 RVA: 0x0009BFF0 File Offset: 0x0009A1F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRollingBoulderVfx.MethodName._Ready || (ref method) == NRollingBoulderVfx.MethodName.CleanUpBeforeEarlyExit || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060018DB RID: 6363 RVA: 0x0009C018 File Offset: 0x0009A218
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRollingBoulderVfx.PropertyName._boulder)
			{
				this._boulder = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NRollingBoulderVfx.PropertyName._shadow)
			{
				this._shadow = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NRollingBoulderVfx.PropertyName._slamBehind)
			{
				this._slamBehind = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NRollingBoulderVfx.PropertyName._slamFront)
			{
				this._slamFront = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060018DC RID: 6364 RVA: 0x0009C09C File Offset: 0x0009A29C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRollingBoulderVfx.PropertyName._boulder)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._boulder);
				return true;
			}
			if ((ref name) == NRollingBoulderVfx.PropertyName._shadow)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._shadow);
				return true;
			}
			if ((ref name) == NRollingBoulderVfx.PropertyName._slamBehind)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slamBehind);
				return true;
			}
			if ((ref name) == NRollingBoulderVfx.PropertyName._slamFront)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slamFront);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018DD RID: 6365 RVA: 0x0009C134 File Offset: 0x0009A334
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRollingBoulderVfx.PropertyName._boulder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRollingBoulderVfx.PropertyName._shadow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRollingBoulderVfx.PropertyName._slamBehind, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRollingBoulderVfx.PropertyName._slamFront, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018DE RID: 6366 RVA: 0x0009C1CC File Offset: 0x0009A3CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRollingBoulderVfx.PropertyName._boulder, Variant.From<Sprite2D>(ref this._boulder));
			info.AddProperty(NRollingBoulderVfx.PropertyName._shadow, Variant.From<Sprite2D>(ref this._shadow));
			info.AddProperty(NRollingBoulderVfx.PropertyName._slamBehind, Variant.From<GpuParticles2D>(ref this._slamBehind));
			info.AddProperty(NRollingBoulderVfx.PropertyName._slamFront, Variant.From<GpuParticles2D>(ref this._slamFront));
			info.AddSignalEventDelegate(NRollingBoulderVfx.SignalName.HitCreature, this.backing_HitCreature);
			info.AddSignalEventDelegate(NRollingBoulderVfx.SignalName.Finished, this.backing_Finished);
		}

		/// <inheritdoc />
		// Token: 0x060018DF RID: 6367 RVA: 0x0009C25C File Offset: 0x0009A45C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._boulder, ref variant))
			{
				this._boulder = variant.As<Sprite2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._shadow, ref variant2))
			{
				this._shadow = variant2.As<Sprite2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._slamBehind, ref variant3))
			{
				this._slamBehind = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._slamFront, ref variant4))
			{
				this._slamFront = variant4.As<GpuParticles2D>();
			}
			NRollingBoulderVfx.HitCreatureEventHandler hitCreatureEventHandler;
			if (info.TryGetSignalEventDelegate<NRollingBoulderVfx.HitCreatureEventHandler>(NRollingBoulderVfx.SignalName.HitCreature, ref hitCreatureEventHandler))
			{
				this.backing_HitCreature = hitCreatureEventHandler;
			}
			NRollingBoulderVfx.FinishedEventHandler finishedEventHandler;
			if (info.TryGetSignalEventDelegate<NRollingBoulderVfx.FinishedEventHandler>(NRollingBoulderVfx.SignalName.Finished, ref finishedEventHandler))
			{
				this.backing_Finished = finishedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018E0 RID: 6368 RVA: 0x0009C310 File Offset: 0x0009A510
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRollingBoulderVfx.SignalName.HitCreature, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creature", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRollingBoulderVfx.SignalName.Finished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Vfx.NRollingBoulderVfx.HitCreatureEventHandler" />
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060018E1 RID: 6369 RVA: 0x0009C3B3 File Offset: 0x0009A5B3
		// (remove) Token: 0x060018E2 RID: 6370 RVA: 0x0009C3CC File Offset: 0x0009A5CC
		public event NRollingBoulderVfx.HitCreatureEventHandler HitCreature
		{
			add
			{
				this.backing_HitCreature = (NRollingBoulderVfx.HitCreatureEventHandler)Delegate.Combine(this.backing_HitCreature, value);
			}
			remove
			{
				this.backing_HitCreature = (NRollingBoulderVfx.HitCreatureEventHandler)Delegate.Remove(this.backing_HitCreature, value);
			}
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0009C3E5 File Offset: 0x0009A5E5
		protected void EmitSignalHitCreature(NCreature creature)
		{
			base.EmitSignal(NRollingBoulderVfx.SignalName.HitCreature, new Variant[] { creature });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Vfx.NRollingBoulderVfx.FinishedEventHandler" />
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060018E4 RID: 6372 RVA: 0x0009C406 File Offset: 0x0009A606
		// (remove) Token: 0x060018E5 RID: 6373 RVA: 0x0009C41F File Offset: 0x0009A61F
		public event NRollingBoulderVfx.FinishedEventHandler Finished
		{
			add
			{
				this.backing_Finished = (NRollingBoulderVfx.FinishedEventHandler)Delegate.Combine(this.backing_Finished, value);
			}
			remove
			{
				this.backing_Finished = (NRollingBoulderVfx.FinishedEventHandler)Delegate.Remove(this.backing_Finished, value);
			}
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x0009C438 File Offset: 0x0009A638
		protected void EmitSignalFinished()
		{
			base.EmitSignal(NRollingBoulderVfx.SignalName.Finished, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x060018E7 RID: 6375 RVA: 0x0009C44C File Offset: 0x0009A64C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NRollingBoulderVfx.SignalName.HitCreature && args.Count == 1)
			{
				NRollingBoulderVfx.HitCreatureEventHandler hitCreatureEventHandler = this.backing_HitCreature;
				if (hitCreatureEventHandler == null)
				{
					return;
				}
				hitCreatureEventHandler(VariantUtils.ConvertTo<NCreature>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NRollingBoulderVfx.SignalName.Finished) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NRollingBoulderVfx.FinishedEventHandler finishedEventHandler = this.backing_Finished;
				if (finishedEventHandler == null)
				{
					return;
				}
				finishedEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x060018E8 RID: 6376 RVA: 0x0009C4BD File Offset: 0x0009A6BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NRollingBoulderVfx.SignalName.HitCreature || (ref signal) == NRollingBoulderVfx.SignalName.Finished || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000901 RID: 2305
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_rolling_boulder");

		// Token: 0x04000902 RID: 2306
		private const float _maxScale = 1.5f;

		// Token: 0x04000903 RID: 2307
		private const decimal _halfScaleDamage = 40m;

		// Token: 0x04000904 RID: 2308
		private const float _maxTimeToImpact = 0.5f;

		// Token: 0x04000905 RID: 2309
		private const float _minTimeToImpact = 0.25f;

		// Token: 0x04000906 RID: 2310
		private const float _minRotationSpeed = 600f;

		// Token: 0x04000907 RID: 2311
		private const float _maxRotationSpeed = 1000f;

		// Token: 0x04000908 RID: 2312
		private const float _minXOffset = 600f;

		// Token: 0x04000909 RID: 2313
		private const float _maxXOffset = 1000f;

		// Token: 0x0400090A RID: 2314
		[Nullable(1)]
		private Sprite2D _boulder;

		// Token: 0x0400090B RID: 2315
		[Nullable(1)]
		private Sprite2D _shadow;

		// Token: 0x0400090C RID: 2316
		[Nullable(1)]
		private GpuParticles2D _slamBehind;

		// Token: 0x0400090D RID: 2317
		[Nullable(1)]
		private GpuParticles2D _slamFront;

		// Token: 0x0400090E RID: 2318
		[Nullable(1)]
		private List<Creature> _creatures;

		// Token: 0x0400090F RID: 2319
		private Vector2? _debugFinalPosition;

		// Token: 0x04000910 RID: 2320
		private decimal _damage;

		// Token: 0x04000911 RID: 2321
		private NRollingBoulderVfx.HitCreatureEventHandler backing_HitCreature;

		// Token: 0x04000912 RID: 2322
		private NRollingBoulderVfx.FinishedEventHandler backing_Finished;

		// Token: 0x02001112 RID: 4370
		// (Invoke) Token: 0x06009ABD RID: 39613
		[Signal]
		public delegate void HitCreatureEventHandler(NCreature creature);

		// Token: 0x02001113 RID: 4371
		// (Invoke) Token: 0x06009AC1 RID: 39617
		[Signal]
		public delegate void FinishedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001114 RID: 4372
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042AE RID: 17070
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'CleanUpBeforeEarlyExit' method.
			/// </summary>
			// Token: 0x040042AF RID: 17071
			public static readonly StringName CleanUpBeforeEarlyExit = "CleanUpBeforeEarlyExit";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001115 RID: 4373
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_boulder' field.
			/// </summary>
			// Token: 0x040042B0 RID: 17072
			public static readonly StringName _boulder = "_boulder";

			/// <summary>
			/// Cached name for the '_shadow' field.
			/// </summary>
			// Token: 0x040042B1 RID: 17073
			public static readonly StringName _shadow = "_shadow";

			/// <summary>
			/// Cached name for the '_slamBehind' field.
			/// </summary>
			// Token: 0x040042B2 RID: 17074
			public static readonly StringName _slamBehind = "_slamBehind";

			/// <summary>
			/// Cached name for the '_slamFront' field.
			/// </summary>
			// Token: 0x040042B3 RID: 17075
			public static readonly StringName _slamFront = "_slamFront";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001116 RID: 4374
		public class SignalName : Node2D.SignalName
		{
			/// <summary>
			/// Cached name for the 'HitCreature' signal.
			/// </summary>
			// Token: 0x040042B4 RID: 17076
			public static readonly StringName HitCreature = "HitCreature";

			/// <summary>
			/// Cached name for the 'Finished' signal.
			/// </summary>
			// Token: 0x040042B5 RID: 17077
			public static readonly StringName Finished = "Finished";
		}
	}
}
