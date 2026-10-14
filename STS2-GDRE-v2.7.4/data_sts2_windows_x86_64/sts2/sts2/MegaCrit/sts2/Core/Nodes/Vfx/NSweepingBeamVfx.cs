using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000202 RID: 514
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSweepingBeamVfx.cs")]
	public class NSweepingBeamVfx : Node2D
	{
		// Token: 0x06001A7E RID: 6782 RVA: 0x000A99B4 File Offset: 0x000A7BB4
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000A99C8 File Offset: 0x000A7BC8
		[return: Nullable(2)]
		public static NSweepingBeamVfx Create(Creature owner, List<Creature> targets)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(owner) : null);
			if (ncreature != null)
			{
				Vector2 vector = ncreature.VfxSpawnPosition;
				Player player = owner.Player;
				if (player != null)
				{
					Defect defect = player.Character as Defect;
					if (defect != null)
					{
						vector += Defect.EyelineOffset;
					}
				}
				Array<Vector2> array = new Array<Vector2>();
				foreach (Creature creature in targets)
				{
					NCombatRoom instance2 = NCombatRoom.Instance;
					NCreature ncreature2 = ((instance2 != null) ? instance2.GetCreatureNode(creature) : null);
					if (ncreature2 != null)
					{
						array.Add(ncreature2.VfxSpawnPosition);
					}
				}
				return NSweepingBeamVfx.Create(vector, array);
			}
			return null;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000A9A98 File Offset: 0x000A7C98
		[return: Nullable(2)]
		public static NSweepingBeamVfx Create(Vector2 defectEyeCenter, Array<Vector2> targetCenterPositions)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSweepingBeamVfx nsweepingBeamVfx = PreloadManager.Cache.GetScene(NSweepingBeamVfx.scenePath).Instantiate<NSweepingBeamVfx>(0L);
			nsweepingBeamVfx.GlobalPosition = defectEyeCenter;
			nsweepingBeamVfx._targetCenterPositions = targetCenterPositions;
			return nsweepingBeamVfx;
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x000A9AD4 File Offset: 0x000A7CD4
		public override void _Ready()
		{
			for (int i = 0; i < this._emittingParticles.Count; i++)
			{
				this._emittingParticles[i].Emitting = false;
			}
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x000A9B18 File Offset: 0x000A7D18
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			double timer = 0.0;
			bool playedImpactParticles = false;
			for (int i = 0; i < this._startParticles.Count; i++)
			{
				this._startParticles[i].Restart();
			}
			for (int j = 0; j < this._emittingParticles.Count; j++)
			{
				this._emittingParticles[j].Restart();
				this._emittingParticles[j].Emitting = true;
			}
			int previousSweepIndex = -1;
			while (timer < (double)this._sweepDuration)
			{
				double processDeltaTime = base.GetProcessDeltaTime();
				float num = (float)(timer / (double)this._sweepDuration);
				int num2 = Mathf.FloorToInt(this._sweepingIndexCurve.Sample(num));
				if (previousSweepIndex != num2 && num2 >= 0 && num2 < this._sweepingParticles.Count)
				{
					this._sweepingParticles[num2].Restart();
					previousSweepIndex = num2;
				}
				if (num >= 0.5f && !playedImpactParticles)
				{
					playedImpactParticles = true;
					NGame instance = NGame.Instance;
					if (instance != null)
					{
						instance.ScreenShake(ShakeStrength.Medium, ShakeDuration.Normal, -1f);
					}
					for (int k = 0; k < this._targetCenterPositions.Count; k++)
					{
						NSweepingBeamImpactVfx nsweepingBeamImpactVfx = NSweepingBeamImpactVfx.Create(this._targetCenterPositions[k]);
						base.GetTree().Root.AddChildSafely(nsweepingBeamImpactVfx);
					}
				}
				timer += processDeltaTime;
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			for (int l = 0; l < this._endParticles.Count; l++)
			{
				this._endParticles[l].Restart();
			}
			for (int m = 0; m < this._emittingParticles.Count; m++)
			{
				this._emittingParticles[m].Emitting = false;
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A83 RID: 6787 RVA: 0x000A9B5C File Offset: 0x000A7D5C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSweepingBeamVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSweepingBeamVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "defectEyeCenter", 0L, "", 6L, false),
					new PropertyInfo(28L, "targetCenterPositions", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSweepingBeamVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A84 RID: 6788 RVA: 0x000A9C54 File Offset: 0x000A7E54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSweepingBeamVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSweepingBeamVfx.MethodName.Create && args.Count == 2)
			{
				NSweepingBeamVfx nsweepingBeamVfx = NSweepingBeamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertToArray<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NSweepingBeamVfx>(ref nsweepingBeamVfx);
				return true;
			}
			if ((ref method) == NSweepingBeamVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000A9CFC File Offset: 0x000A7EFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSweepingBeamVfx.MethodName.Create && args.Count == 2)
			{
				NSweepingBeamVfx nsweepingBeamVfx = NSweepingBeamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertToArray<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NSweepingBeamVfx>(ref nsweepingBeamVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001A86 RID: 6790 RVA: 0x000A9D57 File Offset: 0x000A7F57
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSweepingBeamVfx.MethodName._ExitTree || (ref method) == NSweepingBeamVfx.MethodName.Create || (ref method) == NSweepingBeamVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A87 RID: 6791 RVA: 0x000A9D90 File Offset: 0x000A7F90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSweepingBeamVfx.PropertyName._emittingParticles)
			{
				this._emittingParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._startParticles)
			{
				this._startParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._endParticles)
			{
				this._endParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._sweepingParticles)
			{
				this._sweepingParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._sweepingIndexCurve)
			{
				this._sweepingIndexCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._sweepDuration)
			{
				this._sweepDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._targetCenterPositions)
			{
				this._targetCenterPositions = VariantUtils.ConvertToArray<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A88 RID: 6792 RVA: 0x000A9E64 File Offset: 0x000A8064
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSweepingBeamVfx.PropertyName._emittingParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._emittingParticles);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._startParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._startParticles);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._endParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._endParticles);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._sweepingParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._sweepingParticles);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._sweepingIndexCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._sweepingIndexCurve);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._sweepDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._sweepDuration);
				return true;
			}
			if ((ref name) == NSweepingBeamVfx.PropertyName._targetCenterPositions)
			{
				value = VariantUtils.CreateFromArray<Vector2>(this._targetCenterPositions);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A89 RID: 6793 RVA: 0x000A9F5C File Offset: 0x000A815C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NSweepingBeamVfx.PropertyName._emittingParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSweepingBeamVfx.PropertyName._startParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSweepingBeamVfx.PropertyName._endParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSweepingBeamVfx.PropertyName._sweepingParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NSweepingBeamVfx.PropertyName._sweepingIndexCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(3L, NSweepingBeamVfx.PropertyName._sweepDuration, 0L, "", 4102L, true),
				new PropertyInfo(28L, NSweepingBeamVfx.PropertyName._targetCenterPositions, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A8A RID: 6794 RVA: 0x000AA05C File Offset: 0x000A825C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSweepingBeamVfx.PropertyName._emittingParticles, Variant.CreateFrom<GpuParticles2D>(this._emittingParticles));
			info.AddProperty(NSweepingBeamVfx.PropertyName._startParticles, Variant.CreateFrom<GpuParticles2D>(this._startParticles));
			info.AddProperty(NSweepingBeamVfx.PropertyName._endParticles, Variant.CreateFrom<GpuParticles2D>(this._endParticles));
			info.AddProperty(NSweepingBeamVfx.PropertyName._sweepingParticles, Variant.CreateFrom<GpuParticles2D>(this._sweepingParticles));
			info.AddProperty(NSweepingBeamVfx.PropertyName._sweepingIndexCurve, Variant.From<Curve>(ref this._sweepingIndexCurve));
			info.AddProperty(NSweepingBeamVfx.PropertyName._sweepDuration, Variant.From<float>(ref this._sweepDuration));
			info.AddProperty(NSweepingBeamVfx.PropertyName._targetCenterPositions, Variant.CreateFrom<Vector2>(this._targetCenterPositions));
		}

		/// <inheritdoc />
		// Token: 0x06001A8B RID: 6795 RVA: 0x000AA10C File Offset: 0x000A830C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._emittingParticles, ref variant))
			{
				this._emittingParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._startParticles, ref variant2))
			{
				this._startParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._endParticles, ref variant3))
			{
				this._endParticles = variant3.AsGodotArray<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._sweepingParticles, ref variant4))
			{
				this._sweepingParticles = variant4.AsGodotArray<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._sweepingIndexCurve, ref variant5))
			{
				this._sweepingIndexCurve = variant5.As<Curve>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._sweepDuration, ref variant6))
			{
				this._sweepDuration = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NSweepingBeamVfx.PropertyName._targetCenterPositions, ref variant7))
			{
				this._targetCenterPositions = variant7.AsGodotArray<Vector2>();
			}
		}

		// Token: 0x040009D0 RID: 2512
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_sweeping_beam");

		// Token: 0x040009D1 RID: 2513
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _emittingParticles = new Array<GpuParticles2D>();

		// Token: 0x040009D2 RID: 2514
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _startParticles = new Array<GpuParticles2D>();

		// Token: 0x040009D3 RID: 2515
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _endParticles = new Array<GpuParticles2D>();

		// Token: 0x040009D4 RID: 2516
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _sweepingParticles = new Array<GpuParticles2D>();

		// Token: 0x040009D5 RID: 2517
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _sweepingIndexCurve;

		// Token: 0x040009D6 RID: 2518
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _sweepDuration = 0.65f;

		// Token: 0x040009D7 RID: 2519
		private Array<Vector2> _targetCenterPositions = new Array<Vector2>();

		// Token: 0x040009D8 RID: 2520
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001176 RID: 4470
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004410 RID: 17424
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004411 RID: 17425
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004412 RID: 17426
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001177 RID: 4471
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_emittingParticles' field.
			/// </summary>
			// Token: 0x04004413 RID: 17427
			public static readonly StringName _emittingParticles = "_emittingParticles";

			/// <summary>
			/// Cached name for the '_startParticles' field.
			/// </summary>
			// Token: 0x04004414 RID: 17428
			public static readonly StringName _startParticles = "_startParticles";

			/// <summary>
			/// Cached name for the '_endParticles' field.
			/// </summary>
			// Token: 0x04004415 RID: 17429
			public static readonly StringName _endParticles = "_endParticles";

			/// <summary>
			/// Cached name for the '_sweepingParticles' field.
			/// </summary>
			// Token: 0x04004416 RID: 17430
			public static readonly StringName _sweepingParticles = "_sweepingParticles";

			/// <summary>
			/// Cached name for the '_sweepingIndexCurve' field.
			/// </summary>
			// Token: 0x04004417 RID: 17431
			public static readonly StringName _sweepingIndexCurve = "_sweepingIndexCurve";

			/// <summary>
			/// Cached name for the '_sweepDuration' field.
			/// </summary>
			// Token: 0x04004418 RID: 17432
			public static readonly StringName _sweepDuration = "_sweepDuration";

			/// <summary>
			/// Cached name for the '_targetCenterPositions' field.
			/// </summary>
			// Token: 0x04004419 RID: 17433
			public static readonly StringName _targetCenterPositions = "_targetCenterPositions";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001178 RID: 4472
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
