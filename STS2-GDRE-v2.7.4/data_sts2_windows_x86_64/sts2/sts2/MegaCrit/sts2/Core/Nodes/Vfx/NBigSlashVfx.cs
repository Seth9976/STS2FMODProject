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
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000188 RID: 392
	[ScriptPath("res://src/Core/Nodes/Vfx/NBigSlashVfx.cs")]
	public class NBigSlashVfx : Node2D
	{
		// Token: 0x060012FA RID: 4858 RVA: 0x00071F24 File Offset: 0x00070124
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NBigSlashVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NBigSlashVfx.Create(ncreature.VfxSpawnPosition, creature.IsEnemy, new Color("50b598"));
			}
			return null;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00071F6D File Offset: 0x0007016D
		[NullableContext(2)]
		public static NBigSlashVfx Create(Vector2 targetCenterPosition, bool facingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return NBigSlashVfx.Create(targetCenterPosition, facingRight, Color.FromHtml("#a380ff"));
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00071F90 File Offset: 0x00070190
		[NullableContext(2)]
		public static NBigSlashVfx Create(Vector2 targetCenterPosition, bool facingRight, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NBigSlashVfx nbigSlashVfx = PreloadManager.Cache.GetScene(NBigSlashVfx.scenePath).Instantiate<NBigSlashVfx>(0L);
			nbigSlashVfx.GlobalPosition = targetCenterPosition;
			nbigSlashVfx.Scale = new Vector2(facingRight ? 1f : (-1f), 1f);
			nbigSlashVfx.ModulateParticles(tint);
			return nbigSlashVfx;
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00071FEB File Offset: 0x000701EB
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00071FF9 File Offset: 0x000701F9
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0007200C File Offset: 0x0007020C
		private void ModulateParticles(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].SelfModulate = tint;
			}
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00072044 File Offset: 0x00070244
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._slashParticles.Count; i++)
			{
				this._slashParticles[i].Restart();
			}
			await Cmd.Wait(1f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001301 RID: 4865 RVA: 0x00072088 File Offset: 0x00070288
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NBigSlashVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(1L, "facingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBigSlashVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(1L, "facingRight", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBigSlashVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBigSlashVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBigSlashVfx.MethodName.ModulateParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001302 RID: 4866 RVA: 0x00072274 File Offset: 0x00070474
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBigSlashVfx.MethodName.Create && args.Count == 2)
			{
				NBigSlashVfx nbigSlashVfx = NBigSlashVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NBigSlashVfx>(ref nbigSlashVfx);
				return true;
			}
			if ((ref method) == NBigSlashVfx.MethodName.Create && args.Count == 3)
			{
				NBigSlashVfx nbigSlashVfx2 = NBigSlashVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NBigSlashVfx>(ref nbigSlashVfx2);
				return true;
			}
			if ((ref method) == NBigSlashVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBigSlashVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBigSlashVfx.MethodName.ModulateParticles && args.Count == 1)
			{
				this.ModulateParticles(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000723A0 File Offset: 0x000705A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBigSlashVfx.MethodName.Create && args.Count == 2)
			{
				NBigSlashVfx nbigSlashVfx = NBigSlashVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NBigSlashVfx>(ref nbigSlashVfx);
				return true;
			}
			if ((ref method) == NBigSlashVfx.MethodName.Create && args.Count == 3)
			{
				NBigSlashVfx nbigSlashVfx2 = NBigSlashVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NBigSlashVfx>(ref nbigSlashVfx2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001304 RID: 4868 RVA: 0x00072450 File Offset: 0x00070650
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBigSlashVfx.MethodName.Create || (ref method) == NBigSlashVfx.MethodName._Ready || (ref method) == NBigSlashVfx.MethodName._ExitTree || (ref method) == NBigSlashVfx.MethodName.ModulateParticles || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001305 RID: 4869 RVA: 0x000724A0 File Offset: 0x000706A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBigSlashVfx.PropertyName._slashParticles)
			{
				this._slashParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NBigSlashVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001306 RID: 4870 RVA: 0x000724E0 File Offset: 0x000706E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBigSlashVfx.PropertyName._slashParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._slashParticles);
				return true;
			}
			if ((ref name) == NBigSlashVfx.PropertyName._modulateParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001307 RID: 4871 RVA: 0x00072538 File Offset: 0x00070738
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NBigSlashVfx.PropertyName._slashParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NBigSlashVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001308 RID: 4872 RVA: 0x00072590 File Offset: 0x00070790
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBigSlashVfx.PropertyName._slashParticles, Variant.CreateFrom<GpuParticles2D>(this._slashParticles));
			info.AddProperty(NBigSlashVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001309 RID: 4873 RVA: 0x000725C8 File Offset: 0x000707C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBigSlashVfx.PropertyName._slashParticles, ref variant))
			{
				this._slashParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBigSlashVfx.PropertyName._modulateParticles, ref variant2))
			{
				this._modulateParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x040006BB RID: 1723
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_big_slash");

		// Token: 0x040006BC RID: 1724
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _slashParticles = new Array<GpuParticles2D>();

		// Token: 0x040006BD RID: 1725
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x040006BE RID: 1726
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FAA RID: 4010
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003E74 RID: 15988
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E75 RID: 15989
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003E76 RID: 15990
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ModulateParticles' method.
			/// </summary>
			// Token: 0x04003E77 RID: 15991
			public static readonly StringName ModulateParticles = "ModulateParticles";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FAB RID: 4011
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_slashParticles' field.
			/// </summary>
			// Token: 0x04003E78 RID: 15992
			public static readonly StringName _slashParticles = "_slashParticles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x04003E79 RID: 15993
			public static readonly StringName _modulateParticles = "_modulateParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FAC RID: 4012
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
