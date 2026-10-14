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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002AE RID: 686
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResetTutorialsButton.cs")]
	public class NResetTutorialsButton : NSettingsButton
	{
		// Token: 0x0600244C RID: 9292 RVA: 0x000F6324 File Offset: 0x000F4524
		public override void _Ready()
		{
			if (!NGame.IsReleaseGame())
			{
				base.GetNode<Control>("%ResetTutorialDivider").Visible = true;
				base.GetNode<Control>("%ResetTutorials").Visible = true;
			}
			this.ConnectSignals();
			this._image = base.GetNode<TextureRect>("Image");
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.SetTextAutoSize(new LocString("settings_ui", "TUTORIAL_RESET_BUTTON_LABEL").GetFormattedText());
			base.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenPopup)), 0U);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000F63D3 File Offset: 0x000F45D3
		[NullableContext(1)]
		private void OpenPopup(NButton _)
		{
			NModalContainer.Instance.Add(NSettingsScreenPopup.Create(this._header, this._description), true);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600244E RID: 9294 RVA: 0x000F63F4 File Offset: 0x000F45F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NResetTutorialsButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResetTutorialsButton.MethodName.OpenPopup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600244F RID: 9295 RVA: 0x000F6498 File Offset: 0x000F4698
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResetTutorialsButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResetTutorialsButton.MethodName.OpenPopup && args.Count == 1)
			{
				this.OpenPopup(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002450 RID: 9296 RVA: 0x000F6506 File Offset: 0x000F4706
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NResetTutorialsButton.MethodName._Ready || (ref method) == NResetTutorialsButton.MethodName.OpenPopup || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002451 RID: 9297 RVA: 0x000F652D File Offset: 0x000F472D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NResetTutorialsButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NResetTutorialsButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002452 RID: 9298 RVA: 0x000F6570 File Offset: 0x000F4770
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NResetTutorialsButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NResetTutorialsButton.PropertyName._label)
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
		// Token: 0x06002453 RID: 9299 RVA: 0x000F65C8 File Offset: 0x000F47C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NResetTutorialsButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NResetTutorialsButton.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002454 RID: 9300 RVA: 0x000F661E File Offset: 0x000F481E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NResetTutorialsButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NResetTutorialsButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x06002455 RID: 9301 RVA: 0x000F6654 File Offset: 0x000F4854
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NResetTutorialsButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NResetTutorialsButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
		}

		// Token: 0x04000DB0 RID: 3504
		[Nullable(1)]
		private readonly LocString _header = new LocString("settings_ui", "TUTORIAL_RESET_POPUP_HEADER");

		// Token: 0x04000DB1 RID: 3505
		[Nullable(1)]
		private readonly LocString _description = new LocString("settings_ui", "TUTORIAL_RESET_POPUP_DESCRIPTION");

		// Token: 0x04000DB2 RID: 3506
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000DB3 RID: 3507
		[Nullable(1)]
		private MegaLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C2 RID: 5058
		public new class MethodName : NSettingsButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BB3 RID: 19379
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OpenPopup' method.
			/// </summary>
			// Token: 0x04004BB4 RID: 19380
			public static readonly StringName OpenPopup = "OpenPopup";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C3 RID: 5059
		public new class PropertyName : NSettingsButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004BB5 RID: 19381
			public new static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004BB6 RID: 19382
			public new static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C4 RID: 5060
		public new class SignalName : NSettingsButton.SignalName
		{
		}
	}
}
