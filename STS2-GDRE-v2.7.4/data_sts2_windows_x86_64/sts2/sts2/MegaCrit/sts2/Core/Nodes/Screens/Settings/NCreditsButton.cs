using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Credits;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x0200028D RID: 653
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NCreditsButton.cs")]
	public class NCreditsButton : NSettingsButton
	{
		// Token: 0x060022BE RID: 8894 RVA: 0x000ED474 File Offset: 0x000EB674
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<TextureRect>("Image");
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.SetTextAutoSize(new LocString("settings_ui", "CREDITS_BUTTON_LABEL").GetFormattedText());
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000ED4D4 File Offset: 0x000EB6D4
		protected override void OnRelease()
		{
			base.OnRelease();
			NCreditsScreen ncreditsScreen = NCreditsScreen.Create();
			if (ncreditsScreen != null)
			{
				NModalContainer.Instance.Add(ncreditsScreen, false);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022C0 RID: 8896 RVA: 0x000ED4FC File Offset: 0x000EB6FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCreditsButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022C1 RID: 8897 RVA: 0x000ED570 File Offset: 0x000EB770
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCreditsButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060022C2 RID: 8898 RVA: 0x000ED5D0 File Offset: 0x000EB7D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCreditsButton.MethodName._Ready || (ref method) == NCreditsButton.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060022C3 RID: 8899 RVA: 0x000ED5F7 File Offset: 0x000EB7F7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCreditsButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCreditsButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060022C4 RID: 8900 RVA: 0x000ED638 File Offset: 0x000EB838
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCreditsButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NCreditsButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022C5 RID: 8901 RVA: 0x000ED690 File Offset: 0x000EB890
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCreditsButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreditsButton.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022C6 RID: 8902 RVA: 0x000ED6E6 File Offset: 0x000EB8E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCreditsButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NCreditsButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x060022C7 RID: 8903 RVA: 0x000ED71C File Offset: 0x000EB91C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCreditsButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCreditsButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
		}

		// Token: 0x04000D6D RID: 3437
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000D6E RID: 3438
		[Nullable(1)]
		private MegaLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200135A RID: 4954
		public new class MethodName : NSettingsButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004AF8 RID: 19192
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004AF9 RID: 19193
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200135B RID: 4955
		public new class PropertyName : NSettingsButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004AFA RID: 19194
			public new static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004AFB RID: 19195
			public new static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200135C RID: 4956
		public new class SignalName : NSettingsButton.SignalName
		{
		}
	}
}
