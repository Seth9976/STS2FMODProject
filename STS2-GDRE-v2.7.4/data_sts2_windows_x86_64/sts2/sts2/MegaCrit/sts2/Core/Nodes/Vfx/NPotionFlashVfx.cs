using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001DD RID: 477
	[ScriptPath("res://src/Core/Nodes/Vfx/NPotionFlashVfx.cs")]
	public class NPotionFlashVfx : Node2D
	{
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x000970A4 File Offset: 0x000952A4
		[Nullable(1)]
		public static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_potion_flash");
			}
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x000970B0 File Offset: 0x000952B0
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NPotionFlashVfx Create(NPotion originPotion)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NPotionFlashVfx npotionFlashVfx = PreloadManager.Cache.GetScene(NPotionFlashVfx.ScenePath).Instantiate<NPotionFlashVfx>(0L);
			npotionFlashVfx._flash = (Control)originPotion.Duplicate(15);
			ulong instanceId = npotionFlashVfx._flash.GetInstanceId();
			npotionFlashVfx._flash.SetScript(default(Variant));
			npotionFlashVfx._flash = (Control)GodotObject.InstanceFromId(instanceId);
			return npotionFlashVfx;
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00097122 File Offset: 0x00095322
		public override void _Ready()
		{
			base.GetNode<SubViewport>("SubViewport").AddChildSafely(this._flash);
			this._flash.Position = Vector2.Zero;
			TaskHelper.RunSafely(this.FlashAndFree());
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0009715C File Offset: 0x0009535C
		[NullableContext(1)]
		private async Task FlashAndFree()
		{
			CpuParticles2D node = base.GetNode<CpuParticles2D>("%Flash");
			node.Emitting = true;
			await node.AwaitSignal(CpuParticles2D.SignalName.Finished, this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001826 RID: 6182 RVA: 0x000971A0 File Offset: 0x000953A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NPotionFlashVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "originPotion", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPotionFlashVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001827 RID: 6183 RVA: 0x00097250 File Offset: 0x00095450
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionFlashVfx.MethodName.Create && args.Count == 1)
			{
				NPotionFlashVfx npotionFlashVfx = NPotionFlashVfx.Create(VariantUtils.ConvertTo<NPotion>(args[0]));
				ret = VariantUtils.CreateFrom<NPotionFlashVfx>(ref npotionFlashVfx);
				return true;
			}
			if ((ref method) == NPotionFlashVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x000972C4 File Offset: 0x000954C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionFlashVfx.MethodName.Create && args.Count == 1)
			{
				NPotionFlashVfx npotionFlashVfx = NPotionFlashVfx.Create(VariantUtils.ConvertTo<NPotion>(args[0]));
				ret = VariantUtils.CreateFrom<NPotionFlashVfx>(ref npotionFlashVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001829 RID: 6185 RVA: 0x00097312 File Offset: 0x00095512
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionFlashVfx.MethodName.Create || (ref method) == NPotionFlashVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600182A RID: 6186 RVA: 0x00097339 File Offset: 0x00095539
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotionFlashVfx.PropertyName._flash)
			{
				this._flash = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600182B RID: 6187 RVA: 0x0009735E File Offset: 0x0009555E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionFlashVfx.PropertyName._flash)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._flash);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600182C RID: 6188 RVA: 0x00097388 File Offset: 0x00095588
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPotionFlashVfx.PropertyName._flash, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600182D RID: 6189 RVA: 0x000973BD File Offset: 0x000955BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPotionFlashVfx.PropertyName._flash, Variant.From<Control>(ref this._flash));
		}

		/// <inheritdoc />
		// Token: 0x0600182E RID: 6190 RVA: 0x000973DC File Offset: 0x000955DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotionFlashVfx.PropertyName._flash, ref variant))
			{
				this._flash = variant.As<Control>();
			}
		}

		// Token: 0x040008B5 RID: 2229
		[Nullable(1)]
		private Control _flash;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E7 RID: 4327
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004235 RID: 16949
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004236 RID: 16950
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E8 RID: 4328
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_flash' field.
			/// </summary>
			// Token: 0x04004237 RID: 16951
			public static readonly StringName _flash = "_flash";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E9 RID: 4329
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
