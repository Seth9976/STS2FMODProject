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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001BE RID: 446
	[ScriptPath("res://src/Core/Nodes/Vfx/NHyperbeamImpactVfx.cs")]
	public class NHyperbeamImpactVfx : Node2D
	{
		// Token: 0x0600163D RID: 5693 RVA: 0x00087E48 File Offset: 0x00086048
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00087E5C File Offset: 0x0008605C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NHyperbeamImpactVfx Create(Creature owner, Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(owner) : null);
			NCombatRoom instance2 = NCombatRoom.Instance;
			NCreature ncreature2 = ((instance2 != null) ? instance2.GetCreatureNode(target) : null);
			if (ncreature2 != null && ncreature != null)
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
				return NHyperbeamImpactVfx.Create(vector, ncreature2.VfxSpawnPosition);
			}
			return null;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00087EDC File Offset: 0x000860DC
		[NullableContext(2)]
		public static NHyperbeamImpactVfx Create(Vector2 hyperbeamSourcePosition, Vector2 targetCenterPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NHyperbeamImpactVfx nhyperbeamImpactVfx = PreloadManager.Cache.GetScene(NHyperbeamImpactVfx.scenePath).Instantiate<NHyperbeamImpactVfx>(0L);
			nhyperbeamImpactVfx.GlobalPosition = targetCenterPosition;
			nhyperbeamImpactVfx.ApplyRotation(hyperbeamSourcePosition, targetCenterPosition);
			return nhyperbeamImpactVfx;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00087F1C File Offset: 0x0008611C
		public void ApplyRotation(Vector2 sourcePosition, Vector2 targetPosition)
		{
			Vector2 vector = targetPosition - sourcePosition;
			float num = Mathf.RadToDeg(Mathf.Atan2(vector.Y, vector.X));
			base.RotationDegrees = num;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00087F4F File Offset: 0x0008614F
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00087F60 File Offset: 0x00086160
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._impactStartParticles.Count; i++)
			{
				this._impactStartParticles[i].Visible = true;
				this._impactStartParticles[i].Restart();
			}
			await Cmd.Wait(NHyperbeamVfx.hyperbeamLaserDuration, this._cts.Token, false);
			for (int j = 0; j < this._impactStartParticles.Count; j++)
			{
				this._impactStartParticles[j].Visible = false;
			}
			for (int k = 0; k < this._impactEndParticles.Count; k++)
			{
				this._impactEndParticles[k].Restart();
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001643 RID: 5699 RVA: 0x00087FA4 File Offset: 0x000861A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NHyperbeamImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHyperbeamImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "hyperbeamSourcePosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHyperbeamImpactVfx.MethodName.ApplyRotation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "sourcePosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHyperbeamImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001644 RID: 5700 RVA: 0x00088110 File Offset: 0x00086310
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHyperbeamImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHyperbeamImpactVfx.MethodName.Create && args.Count == 2)
			{
				NHyperbeamImpactVfx nhyperbeamImpactVfx = NHyperbeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NHyperbeamImpactVfx>(ref nhyperbeamImpactVfx);
				return true;
			}
			if ((ref method) == NHyperbeamImpactVfx.MethodName.ApplyRotation && args.Count == 2)
			{
				this.ApplyRotation(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHyperbeamImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x000881F8 File Offset: 0x000863F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHyperbeamImpactVfx.MethodName.Create && args.Count == 2)
			{
				NHyperbeamImpactVfx nhyperbeamImpactVfx = NHyperbeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NHyperbeamImpactVfx>(ref nhyperbeamImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001646 RID: 5702 RVA: 0x00088254 File Offset: 0x00086454
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHyperbeamImpactVfx.MethodName._ExitTree || (ref method) == NHyperbeamImpactVfx.MethodName.Create || (ref method) == NHyperbeamImpactVfx.MethodName.ApplyRotation || (ref method) == NHyperbeamImpactVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001647 RID: 5703 RVA: 0x000882A4 File Offset: 0x000864A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHyperbeamImpactVfx.PropertyName._impactStartParticles)
			{
				this._impactStartParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHyperbeamImpactVfx.PropertyName._impactEndParticles)
			{
				this._impactEndParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001648 RID: 5704 RVA: 0x000882E4 File Offset: 0x000864E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHyperbeamImpactVfx.PropertyName._impactStartParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactStartParticles);
				return true;
			}
			if ((ref name) == NHyperbeamImpactVfx.PropertyName._impactEndParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactEndParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001649 RID: 5705 RVA: 0x0008833C File Offset: 0x0008653C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NHyperbeamImpactVfx.PropertyName._impactStartParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NHyperbeamImpactVfx.PropertyName._impactEndParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600164A RID: 5706 RVA: 0x00088394 File Offset: 0x00086594
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHyperbeamImpactVfx.PropertyName._impactStartParticles, Variant.CreateFrom<GpuParticles2D>(this._impactStartParticles));
			info.AddProperty(NHyperbeamImpactVfx.PropertyName._impactEndParticles, Variant.CreateFrom<GpuParticles2D>(this._impactEndParticles));
		}

		/// <inheritdoc />
		// Token: 0x0600164B RID: 5707 RVA: 0x000883CC File Offset: 0x000865CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHyperbeamImpactVfx.PropertyName._impactStartParticles, ref variant))
			{
				this._impactStartParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHyperbeamImpactVfx.PropertyName._impactEndParticles, ref variant2))
			{
				this._impactEndParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x040007E3 RID: 2019
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_hyperbeam_impact");

		// Token: 0x040007E4 RID: 2020
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactStartParticles = new Array<GpuParticles2D>();

		// Token: 0x040007E5 RID: 2021
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactEndParticles = new Array<GpuParticles2D>();

		// Token: 0x040007E6 RID: 2022
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001075 RID: 4213
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040040BE RID: 16574
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040040BF RID: 16575
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'ApplyRotation' method.
			/// </summary>
			// Token: 0x040040C0 RID: 16576
			public static readonly StringName ApplyRotation = "ApplyRotation";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040C1 RID: 16577
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001076 RID: 4214
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_impactStartParticles' field.
			/// </summary>
			// Token: 0x040040C2 RID: 16578
			public static readonly StringName _impactStartParticles = "_impactStartParticles";

			/// <summary>
			/// Cached name for the '_impactEndParticles' field.
			/// </summary>
			// Token: 0x040040C3 RID: 16579
			public static readonly StringName _impactEndParticles = "_impactEndParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001077 RID: 4215
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
