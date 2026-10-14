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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000187 RID: 391
	[ScriptPath("res://src/Core/Nodes/Vfx/NBigSlashImpactVfx.cs")]
	public class NBigSlashImpactVfx : Node2D
	{
		// Token: 0x060012E7 RID: 4839 RVA: 0x00071674 File Offset: 0x0006F874
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NBigSlashImpactVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NBigSlashImpactVfx.Create(ncreature.VfxSpawnPosition);
			}
			return null;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x000716AD File Offset: 0x0006F8AD
		[NullableContext(2)]
		public static NBigSlashImpactVfx Create(Vector2 targetCenterPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return NBigSlashImpactVfx.Create(targetCenterPosition, 60f, Color.FromHtml("#80dbff"));
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x000716D4 File Offset: 0x0006F8D4
		[NullableContext(2)]
		public static NBigSlashImpactVfx Create(Vector2 targetCenterPosition, float rotationDegrees, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NBigSlashImpactVfx nbigSlashImpactVfx = PreloadManager.Cache.GetScene(NBigSlashImpactVfx.scenePath).Instantiate<NBigSlashImpactVfx>(0L);
			nbigSlashImpactVfx.GlobalPosition = targetCenterPosition;
			nbigSlashImpactVfx.RotateCore(rotationDegrees);
			nbigSlashImpactVfx.ModulateParticles(tint);
			return nbigSlashImpactVfx;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00071717 File Offset: 0x0006F917
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00071725 File Offset: 0x0006F925
		private void RotateCore(float rotationDegrees)
		{
			if (this._corePivot != null)
			{
				this._corePivot.RotationDegrees = rotationDegrees;
			}
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0007173C File Offset: 0x0006F93C
		private void ModulateParticles(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].SelfModulate = tint;
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00071771 File Offset: 0x0006F971
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00071784 File Offset: 0x0006F984
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._anticipationParticles.Count; i++)
			{
				this._anticipationParticles[i].Restart();
			}
			await Cmd.Wait(0.1f, this._cts.Token, false);
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short, -1f);
			}
			for (int j = 0; j < this._impactParticles.Count; j++)
			{
				this._impactParticles[j].Restart();
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012EF RID: 4847 RVA: 0x000717C8 File Offset: 0x0006F9C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NBigSlashImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBigSlashImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(3L, "rotationDegrees", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBigSlashImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBigSlashImpactVfx.MethodName.RotateCore, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "rotationDegrees", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBigSlashImpactVfx.MethodName.ModulateParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBigSlashImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012F0 RID: 4848 RVA: 0x000719E8 File Offset: 0x0006FBE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBigSlashImpactVfx.MethodName.Create && args.Count == 1)
			{
				NBigSlashImpactVfx nbigSlashImpactVfx = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx);
				return true;
			}
			if ((ref method) == NBigSlashImpactVfx.MethodName.Create && args.Count == 3)
			{
				NBigSlashImpactVfx nbigSlashImpactVfx2 = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx2);
				return true;
			}
			if ((ref method) == NBigSlashImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBigSlashImpactVfx.MethodName.RotateCore && args.Count == 1)
			{
				this.RotateCore(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBigSlashImpactVfx.MethodName.ModulateParticles && args.Count == 1)
			{
				this.ModulateParticles(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBigSlashImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00071B3C File Offset: 0x0006FD3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBigSlashImpactVfx.MethodName.Create && args.Count == 1)
			{
				NBigSlashImpactVfx nbigSlashImpactVfx = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx);
				return true;
			}
			if ((ref method) == NBigSlashImpactVfx.MethodName.Create && args.Count == 3)
			{
				NBigSlashImpactVfx nbigSlashImpactVfx2 = NBigSlashImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NBigSlashImpactVfx>(ref nbigSlashImpactVfx2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060012F2 RID: 4850 RVA: 0x00071BE0 File Offset: 0x0006FDE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBigSlashImpactVfx.MethodName.Create || (ref method) == NBigSlashImpactVfx.MethodName._Ready || (ref method) == NBigSlashImpactVfx.MethodName.RotateCore || (ref method) == NBigSlashImpactVfx.MethodName.ModulateParticles || (ref method) == NBigSlashImpactVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060012F3 RID: 4851 RVA: 0x00071C40 File Offset: 0x0006FE40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBigSlashImpactVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NBigSlashImpactVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NBigSlashImpactVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NBigSlashImpactVfx.PropertyName._corePivot)
			{
				this._corePivot = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060012F4 RID: 4852 RVA: 0x00071CC4 File Offset: 0x0006FEC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBigSlashImpactVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
				return true;
			}
			if ((ref name) == NBigSlashImpactVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			if ((ref name) == NBigSlashImpactVfx.PropertyName._modulateParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
				return true;
			}
			if ((ref name) == NBigSlashImpactVfx.PropertyName._corePivot)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._corePivot);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012F5 RID: 4853 RVA: 0x00071D5C File Offset: 0x0006FF5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NBigSlashImpactVfx.PropertyName._anticipationParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NBigSlashImpactVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NBigSlashImpactVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NBigSlashImpactVfx.PropertyName._corePivot, 34L, "Node2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012F6 RID: 4854 RVA: 0x00071DF8 File Offset: 0x0006FFF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBigSlashImpactVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
			info.AddProperty(NBigSlashImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
			info.AddProperty(NBigSlashImpactVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
			info.AddProperty(NBigSlashImpactVfx.PropertyName._corePivot, Variant.From<Node2D>(ref this._corePivot));
		}

		/// <inheritdoc />
		// Token: 0x060012F7 RID: 4855 RVA: 0x00071E64 File Offset: 0x00070064
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBigSlashImpactVfx.PropertyName._anticipationParticles, ref variant))
			{
				this._anticipationParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBigSlashImpactVfx.PropertyName._impactParticles, ref variant2))
			{
				this._impactParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBigSlashImpactVfx.PropertyName._modulateParticles, ref variant3))
			{
				this._modulateParticles = variant3.AsGodotArray<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBigSlashImpactVfx.PropertyName._corePivot, ref variant4))
			{
				this._corePivot = variant4.As<Node2D>();
			}
		}

		// Token: 0x040006B5 RID: 1717
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_big_slash_impact");

		// Token: 0x040006B6 RID: 1718
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();

		// Token: 0x040006B7 RID: 1719
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x040006B8 RID: 1720
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x040006B9 RID: 1721
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _corePivot;

		// Token: 0x040006BA RID: 1722
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA6 RID: 4006
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003E67 RID: 15975
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E68 RID: 15976
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RotateCore' method.
			/// </summary>
			// Token: 0x04003E69 RID: 15977
			public static readonly StringName RotateCore = "RotateCore";

			/// <summary>
			/// Cached name for the 'ModulateParticles' method.
			/// </summary>
			// Token: 0x04003E6A RID: 15978
			public static readonly StringName ModulateParticles = "ModulateParticles";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003E6B RID: 15979
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA7 RID: 4007
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x04003E6C RID: 15980
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x04003E6D RID: 15981
			public static readonly StringName _impactParticles = "_impactParticles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x04003E6E RID: 15982
			public static readonly StringName _modulateParticles = "_modulateParticles";

			/// <summary>
			/// Cached name for the '_corePivot' field.
			/// </summary>
			// Token: 0x04003E6F RID: 15983
			public static readonly StringName _corePivot = "_corePivot";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA8 RID: 4008
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
