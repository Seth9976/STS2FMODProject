using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.sts2.Core.Nodes.TopBar
{
	// Token: 0x0200001D RID: 29
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarModifier.cs")]
	public class NTopBarModifier : NClickableControl
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x000086A0 File Offset: 0x000068A0
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NTopBarModifier Create(ModifierModel modifier)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NTopBarModifier ntopBarModifier = PreloadManager.Cache.GetScene(NTopBarModifier._scenePath).Instantiate<NTopBarModifier>(0L);
			ntopBarModifier._modifier = modifier;
			return ntopBarModifier;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000086D8 File Offset: 0x000068D8
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("Icon");
			this._icon.Texture = this._modifier.Icon;
			this._hoverTip = new HoverTip(this._modifier.Title, this._modifier.Description, null);
			this.ConnectSignals();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000873C File Offset: 0x0000693C
		protected override void OnFocus()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000878C File Offset: 0x0000698C
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000FB RID: 251 RVA: 0x00008794 File Offset: 0x00006994
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NTopBarModifier.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarModifier.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarModifier.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000FC RID: 252 RVA: 0x00008838 File Offset: 0x00006A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarModifier.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarModifier.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarModifier.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060000FD RID: 253 RVA: 0x000088BD File Offset: 0x00006ABD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarModifier.MethodName._Ready || (ref method) == NTopBarModifier.MethodName.OnFocus || (ref method) == NTopBarModifier.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060000FE RID: 254 RVA: 0x000088F3 File Offset: 0x00006AF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarModifier.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060000FF RID: 255 RVA: 0x00008918 File Offset: 0x00006B18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarModifier.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000100 RID: 256 RVA: 0x00008944 File Offset: 0x00006B44
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTopBarModifier.PropertyName._icon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000101 RID: 257 RVA: 0x00008979 File Offset: 0x00006B79
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarModifier.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
		}

		/// <inheritdoc />
		// Token: 0x06000102 RID: 258 RVA: 0x00008998 File Offset: 0x00006B98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarModifier.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
		}

		// Token: 0x0400005D RID: 93
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/top_bar/top_bar_modifier");

		// Token: 0x0400005E RID: 94
		private HoverTip _hoverTip;

		// Token: 0x0400005F RID: 95
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000060 RID: 96
		[Nullable(1)]
		private ModifierModel _modifier;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE7 RID: 3559
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040031D3 RID: 12755
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040031D4 RID: 12756
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040031D5 RID: 12757
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE8 RID: 3560
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x040031D6 RID: 12758
			public static readonly StringName _icon = "_icon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE9 RID: 3561
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
