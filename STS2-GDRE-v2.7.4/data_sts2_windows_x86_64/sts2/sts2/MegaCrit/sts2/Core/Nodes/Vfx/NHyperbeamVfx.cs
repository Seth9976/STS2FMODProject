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
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001BF RID: 447
	[ScriptPath("res://src/Core/Nodes/Vfx/NHyperbeamVfx.cs")]
	public class NHyperbeamVfx : Node2D
	{
		// Token: 0x0600164E RID: 5710 RVA: 0x00088447 File Offset: 0x00086647
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0008845C File Offset: 0x0008665C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NHyperbeamVfx Create(Creature owner, Creature target)
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
				return NHyperbeamVfx.Create(vector, ncreature2.VfxSpawnPosition);
			}
			return null;
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x000884DC File Offset: 0x000866DC
		[NullableContext(2)]
		public static NHyperbeamVfx Create(Vector2 defectEyePosition, Vector2 mainTargetCenterPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NHyperbeamVfx nhyperbeamVfx = PreloadManager.Cache.GetScene(NHyperbeamVfx.scenePath).Instantiate<NHyperbeamVfx>(0L);
			nhyperbeamVfx.GlobalPosition = defectEyePosition;
			nhyperbeamVfx.ApplyRotation(defectEyePosition, mainTargetCenterPosition);
			return nhyperbeamVfx;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0008851C File Offset: 0x0008671C
		public void ApplyRotation(Vector2 sourcePosition, Vector2 targetPosition)
		{
			Vector2 vector = targetPosition - sourcePosition;
			float num = Mathf.RadToDeg(Mathf.Atan2(vector.Y, vector.X));
			base.RotationDegrees = num;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0008854F File Offset: 0x0008674F
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00088560 File Offset: 0x00086760
		private void ShowLaser(bool showing)
		{
			for (int i = 0; i < this._laserParticles.Count; i++)
			{
				this._laserParticles[i].Visible = showing;
				if (showing)
				{
					this._laserParticles[i].Restart();
				}
			}
			this._laserLine.Visible = showing;
			this._laserContainer.Visible = showing;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000885C4 File Offset: 0x000867C4
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			SfxCmd.Play("event:/sfx/characters/defect/defect_hyperbeam", 1f);
			this.ShowLaser(false);
			for (int i = 0; i < this._anticipationParticles.Count; i++)
			{
				this._anticipationParticles[i].Restart();
			}
			await Cmd.Wait(NHyperbeamVfx.hyperbeamAnticipationDuration, this._cts.Token, false);
			this.ShowLaser(true);
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Medium, ShakeDuration.Normal, -1f);
			}
			await Cmd.Wait(NHyperbeamVfx.hyperbeamLaserDuration, this._cts.Token, false);
			this.ShowLaser(false);
			for (int j = 0; j < this._laserEndParticles.Count; j++)
			{
				this._laserEndParticles[j].Restart();
			}
			NGame instance2 = NGame.Instance;
			if (instance2 != null)
			{
				instance2.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short, -1f);
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001655 RID: 5717 RVA: 0x00088608 File Offset: 0x00086808
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NHyperbeamVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHyperbeamVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "defectEyePosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "mainTargetCenterPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHyperbeamVfx.MethodName.ApplyRotation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "sourcePosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHyperbeamVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHyperbeamVfx.MethodName.ShowLaser, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "showing", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001656 RID: 5718 RVA: 0x000887C8 File Offset: 0x000869C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHyperbeamVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHyperbeamVfx.MethodName.Create && args.Count == 2)
			{
				NHyperbeamVfx nhyperbeamVfx = NHyperbeamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NHyperbeamVfx>(ref nhyperbeamVfx);
				return true;
			}
			if ((ref method) == NHyperbeamVfx.MethodName.ApplyRotation && args.Count == 2)
			{
				this.ApplyRotation(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHyperbeamVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHyperbeamVfx.MethodName.ShowLaser && args.Count == 1)
			{
				this.ShowLaser(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x000888E4 File Offset: 0x00086AE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHyperbeamVfx.MethodName.Create && args.Count == 2)
			{
				NHyperbeamVfx nhyperbeamVfx = NHyperbeamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NHyperbeamVfx>(ref nhyperbeamVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001658 RID: 5720 RVA: 0x00088940 File Offset: 0x00086B40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHyperbeamVfx.MethodName._ExitTree || (ref method) == NHyperbeamVfx.MethodName.Create || (ref method) == NHyperbeamVfx.MethodName.ApplyRotation || (ref method) == NHyperbeamVfx.MethodName._Ready || (ref method) == NHyperbeamVfx.MethodName.ShowLaser || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001659 RID: 5721 RVA: 0x000889A0 File Offset: 0x00086BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHyperbeamVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserParticles)
			{
				this._laserParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserEndParticles)
			{
				this._laserEndParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserLine)
			{
				this._laserLine = VariantUtils.ConvertTo<Line2D>(ref value);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserContainer)
			{
				this._laserContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600165A RID: 5722 RVA: 0x00088A3C File Offset: 0x00086C3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHyperbeamVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._laserParticles);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserEndParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._laserEndParticles);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserLine)
			{
				value = VariantUtils.CreateFrom<Line2D>(ref this._laserLine);
				return true;
			}
			if ((ref name) == NHyperbeamVfx.PropertyName._laserContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._laserContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600165B RID: 5723 RVA: 0x00088AF4 File Offset: 0x00086CF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NHyperbeamVfx.PropertyName._anticipationParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NHyperbeamVfx.PropertyName._laserParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NHyperbeamVfx.PropertyName._laserEndParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NHyperbeamVfx.PropertyName._laserLine, 34L, "Line2D", 4102L, true),
				new PropertyInfo(24L, NHyperbeamVfx.PropertyName._laserContainer, 34L, "Node2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600165C RID: 5724 RVA: 0x00088BB4 File Offset: 0x00086DB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHyperbeamVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
			info.AddProperty(NHyperbeamVfx.PropertyName._laserParticles, Variant.CreateFrom<GpuParticles2D>(this._laserParticles));
			info.AddProperty(NHyperbeamVfx.PropertyName._laserEndParticles, Variant.CreateFrom<GpuParticles2D>(this._laserEndParticles));
			info.AddProperty(NHyperbeamVfx.PropertyName._laserLine, Variant.From<Line2D>(ref this._laserLine));
			info.AddProperty(NHyperbeamVfx.PropertyName._laserContainer, Variant.From<Node2D>(ref this._laserContainer));
		}

		/// <inheritdoc />
		// Token: 0x0600165D RID: 5725 RVA: 0x00088C38 File Offset: 0x00086E38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHyperbeamVfx.PropertyName._anticipationParticles, ref variant))
			{
				this._anticipationParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHyperbeamVfx.PropertyName._laserParticles, ref variant2))
			{
				this._laserParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHyperbeamVfx.PropertyName._laserEndParticles, ref variant3))
			{
				this._laserEndParticles = variant3.AsGodotArray<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHyperbeamVfx.PropertyName._laserLine, ref variant4))
			{
				this._laserLine = variant4.As<Line2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NHyperbeamVfx.PropertyName._laserContainer, ref variant5))
			{
				this._laserContainer = variant5.As<Node2D>();
			}
		}

		// Token: 0x040007E7 RID: 2023
		[Nullable(1)]
		private const string _hyperbeamSfxPath = "event:/sfx/characters/defect/defect_hyperbeam";

		// Token: 0x040007E8 RID: 2024
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_hyperbeam");

		// Token: 0x040007E9 RID: 2025
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();

		// Token: 0x040007EA RID: 2026
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _laserParticles = new Array<GpuParticles2D>();

		// Token: 0x040007EB RID: 2027
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _laserEndParticles = new Array<GpuParticles2D>();

		// Token: 0x040007EC RID: 2028
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Line2D _laserLine;

		// Token: 0x040007ED RID: 2029
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _laserContainer;

		// Token: 0x040007EE RID: 2030
		public static readonly float hyperbeamAnticipationDuration = 0.525f;

		// Token: 0x040007EF RID: 2031
		public static readonly float hyperbeamLaserDuration = 0.5f;

		// Token: 0x040007F0 RID: 2032
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001079 RID: 4217
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040040C8 RID: 16584
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040040C9 RID: 16585
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'ApplyRotation' method.
			/// </summary>
			// Token: 0x040040CA RID: 16586
			public static readonly StringName ApplyRotation = "ApplyRotation";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040CB RID: 16587
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ShowLaser' method.
			/// </summary>
			// Token: 0x040040CC RID: 16588
			public static readonly StringName ShowLaser = "ShowLaser";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200107A RID: 4218
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x040040CD RID: 16589
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_laserParticles' field.
			/// </summary>
			// Token: 0x040040CE RID: 16590
			public static readonly StringName _laserParticles = "_laserParticles";

			/// <summary>
			/// Cached name for the '_laserEndParticles' field.
			/// </summary>
			// Token: 0x040040CF RID: 16591
			public static readonly StringName _laserEndParticles = "_laserEndParticles";

			/// <summary>
			/// Cached name for the '_laserLine' field.
			/// </summary>
			// Token: 0x040040D0 RID: 16592
			public static readonly StringName _laserLine = "_laserLine";

			/// <summary>
			/// Cached name for the '_laserContainer' field.
			/// </summary>
			// Token: 0x040040D1 RID: 16593
			public static readonly StringName _laserContainer = "_laserContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200107B RID: 4219
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
