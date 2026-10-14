using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000298 RID: 664
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NInputSettingsEntry.cs")]
	public class NInputSettingsEntry : NButton
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x000EF8CC File Offset: 0x000EDACC
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/screens/settings_screen/input_settings_entry.tscn");
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x000EF8D8 File Offset: 0x000EDAD8
		// (set) Token: 0x0600233C RID: 9020 RVA: 0x000EF8E0 File Offset: 0x000EDAE0
		public StringName InputName { get; private set; }

		// Token: 0x0600233D RID: 9021 RVA: 0x000EF8EC File Offset: 0x000EDAEC
		public static NInputSettingsEntry Create(string commandName)
		{
			NInputSettingsEntry ninputSettingsEntry = ResourceLoader.Load<PackedScene>("res://scenes/screens/settings_screen/input_settings_entry.tscn", null, 1L).Instantiate<NInputSettingsEntry>(0L);
			ninputSettingsEntry.InputName = commandName;
			return ninputSettingsEntry;
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000EF91C File Offset: 0x000EDB1C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._inputLabel = base.GetNode<MegaRichTextLabel>("%InputLabel");
			this._keyBindingLabel = base.GetNode<MegaRichTextLabel>("%KeyBindingInputLabel");
			this._controllerBindingIcon = base.GetNode<TextureRect>("%ControllerBindingIcon");
			this._bg = base.GetNode<Control>("%Bg");
			string text = NInputSettingsEntry._commandToLocTitle[this.InputName];
			this._inputLabel.Text = new LocString("settings_ui", "INPUT_SETTINGS.INPUT_TITLE." + text).GetFormattedText();
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateInput)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateInput)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateInput)), 0U);
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.UpdateInput)), 0U);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000EFA44 File Offset: 0x000EDC44
		public override void _ExitTree()
		{
			base._ExitTree();
			NInputManager.Instance.Disconnect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateInput)));
			NControllerManager.Instance.Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateInput)));
			NControllerManager.Instance.Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateInput)));
			base.Disconnect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.UpdateInput)));
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000EFAD4 File Offset: 0x000EDCD4
		private void UpdateInput()
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (NInputManager.remappableKeyboardInputs.Contains(this.InputName))
			{
				Key shortcutKey = NInputManager.Instance.GetShortcutKey(this.InputName);
				this._keyBindingLabel.Text = ((shortcutKey != null) ? shortcutKey.ToString() : "");
			}
			else
			{
				this._keyBindingLabel.Text = "";
			}
			if (NInputManager.remappableControllerInputs.Contains(this.InputName))
			{
				this._controllerBindingIcon.Texture = NInputManager.Instance.GetHotkeyIcon(this.InputName);
			}
			this._controllerBindingIcon.Modulate = (NControllerManager.Instance.ShouldAllowControllerRebinding ? Colors.White : new Color(1f, 1f, 1f, 0.15f));
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000EFBAA File Offset: 0x000EDDAA
		protected override void OnFocus()
		{
			this._bg.Visible = true;
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000EFBB8 File Offset: 0x000EDDB8
		protected override void OnUnfocus()
		{
			this._bg.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002343 RID: 9027 RVA: 0x000EFBC8 File Offset: 0x000EDDC8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NInputSettingsEntry.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "commandName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInputSettingsEntry.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputSettingsEntry.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputSettingsEntry.MethodName.UpdateInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputSettingsEntry.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputSettingsEntry.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002344 RID: 9028 RVA: 0x000EFD28 File Offset: 0x000EDF28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInputSettingsEntry.MethodName.Create && args.Count == 1)
			{
				NInputSettingsEntry ninputSettingsEntry = NInputSettingsEntry.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NInputSettingsEntry>(ref ninputSettingsEntry);
				return true;
			}
			if ((ref method) == NInputSettingsEntry.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsEntry.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsEntry.MethodName.UpdateInput && args.Count == 0)
			{
				this.UpdateInput();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsEntry.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsEntry.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000EFE30 File Offset: 0x000EE030
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInputSettingsEntry.MethodName.Create && args.Count == 1)
			{
				NInputSettingsEntry ninputSettingsEntry = NInputSettingsEntry.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NInputSettingsEntry>(ref ninputSettingsEntry);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002346 RID: 9030 RVA: 0x000EFE80 File Offset: 0x000EE080
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NInputSettingsEntry.MethodName.Create || (ref method) == NInputSettingsEntry.MethodName._Ready || (ref method) == NInputSettingsEntry.MethodName._ExitTree || (ref method) == NInputSettingsEntry.MethodName.UpdateInput || (ref method) == NInputSettingsEntry.MethodName.OnFocus || (ref method) == NInputSettingsEntry.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002347 RID: 9031 RVA: 0x000EFEF0 File Offset: 0x000EE0F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NInputSettingsEntry.PropertyName.InputName)
			{
				this.InputName = VariantUtils.ConvertTo<StringName>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._bg)
			{
				this._bg = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._inputLabel)
			{
				this._inputLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._keyBindingLabel)
			{
				this._keyBindingLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._controllerBindingIcon)
			{
				this._controllerBindingIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002348 RID: 9032 RVA: 0x000EFF8C File Offset: 0x000EE18C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NInputSettingsEntry.PropertyName.InputName)
			{
				StringName inputName = this.InputName;
				value = VariantUtils.CreateFrom<StringName>(ref inputName);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._bg)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bg);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._inputLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._inputLabel);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._keyBindingLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._keyBindingLabel);
				return true;
			}
			if ((ref name) == NInputSettingsEntry.PropertyName._controllerBindingIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._controllerBindingIcon);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002349 RID: 9033 RVA: 0x000F0044 File Offset: 0x000EE244
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(21L, NInputSettingsEntry.PropertyName.InputName, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsEntry.PropertyName._bg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsEntry.PropertyName._inputLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsEntry.PropertyName._keyBindingLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsEntry.PropertyName._controllerBindingIcon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600234A RID: 9034 RVA: 0x000F0100 File Offset: 0x000EE300
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName inputName = NInputSettingsEntry.PropertyName.InputName;
			StringName inputName2 = this.InputName;
			info.AddProperty(inputName, Variant.From<StringName>(ref inputName2));
			info.AddProperty(NInputSettingsEntry.PropertyName._bg, Variant.From<Control>(ref this._bg));
			info.AddProperty(NInputSettingsEntry.PropertyName._inputLabel, Variant.From<MegaRichTextLabel>(ref this._inputLabel));
			info.AddProperty(NInputSettingsEntry.PropertyName._keyBindingLabel, Variant.From<MegaRichTextLabel>(ref this._keyBindingLabel));
			info.AddProperty(NInputSettingsEntry.PropertyName._controllerBindingIcon, Variant.From<TextureRect>(ref this._controllerBindingIcon));
		}

		/// <inheritdoc />
		// Token: 0x0600234B RID: 9035 RVA: 0x000F0188 File Offset: 0x000EE388
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NInputSettingsEntry.PropertyName.InputName, ref variant))
			{
				this.InputName = variant.As<StringName>();
			}
			Variant variant2;
			if (info.TryGetProperty(NInputSettingsEntry.PropertyName._bg, ref variant2))
			{
				this._bg = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NInputSettingsEntry.PropertyName._inputLabel, ref variant3))
			{
				this._inputLabel = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NInputSettingsEntry.PropertyName._keyBindingLabel, ref variant4))
			{
				this._keyBindingLabel = variant4.As<MegaRichTextLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NInputSettingsEntry.PropertyName._controllerBindingIcon, ref variant5))
			{
				this._controllerBindingIcon = variant5.As<TextureRect>();
			}
		}

		// Token: 0x04000D79 RID: 3449
		private static readonly Dictionary<StringName, string> _commandToLocTitle = new Dictionary<StringName, string>
		{
			{
				MegaInput.accept,
				"endTurn"
			},
			{
				MegaInput.select,
				"confirmCard"
			},
			{
				MegaInput.viewDiscardPile,
				"viewDiscard"
			},
			{
				MegaInput.viewDrawPile,
				"viewDraw"
			},
			{
				MegaInput.viewDeckAndTabLeft,
				"viewDeck"
			},
			{
				MegaInput.viewExhaustPileAndTabRight,
				"viewExhaust"
			},
			{
				MegaInput.viewMap,
				"viewMap"
			},
			{
				MegaInput.cancel,
				"cancel"
			},
			{
				MegaInput.peek,
				"peek"
			},
			{
				MegaInput.up,
				"up"
			},
			{
				MegaInput.topPanel,
				"topPanel"
			},
			{
				MegaInput.down,
				"down"
			},
			{
				MegaInput.left,
				"left"
			},
			{
				MegaInput.right,
				"right"
			},
			{
				MegaInput.selectCard1,
				"selectCard1"
			},
			{
				MegaInput.selectCard2,
				"selectCard2"
			},
			{
				MegaInput.selectCard3,
				"selectCard3"
			},
			{
				MegaInput.selectCard4,
				"selectCard4"
			},
			{
				MegaInput.selectCard5,
				"selectCard5"
			},
			{
				MegaInput.selectCard6,
				"selectCard6"
			},
			{
				MegaInput.selectCard7,
				"selectCard7"
			},
			{
				MegaInput.selectCard8,
				"selectCard8"
			},
			{
				MegaInput.selectCard9,
				"selectCard9"
			},
			{
				MegaInput.selectCard10,
				"selectCard10"
			},
			{
				MegaInput.releaseCard,
				"releaseCard"
			}
		};

		// Token: 0x04000D7A RID: 3450
		private const string _scenePath = "res://scenes/screens/settings_screen/input_settings_entry.tscn";

		// Token: 0x04000D7C RID: 3452
		private Control _bg;

		// Token: 0x04000D7D RID: 3453
		private MegaRichTextLabel _inputLabel;

		// Token: 0x04000D7E RID: 3454
		private MegaRichTextLabel _keyBindingLabel;

		// Token: 0x04000D7F RID: 3455
		private TextureRect _controllerBindingIcon;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200137B RID: 4987
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004B24 RID: 19236
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B25 RID: 19237
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004B26 RID: 19238
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateInput' method.
			/// </summary>
			// Token: 0x04004B27 RID: 19239
			public static readonly StringName UpdateInput = "UpdateInput";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004B28 RID: 19240
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004B29 RID: 19241
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200137C RID: 4988
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InputName' property.
			/// </summary>
			// Token: 0x04004B2A RID: 19242
			public static readonly StringName InputName = "InputName";

			/// <summary>
			/// Cached name for the '_bg' field.
			/// </summary>
			// Token: 0x04004B2B RID: 19243
			public static readonly StringName _bg = "_bg";

			/// <summary>
			/// Cached name for the '_inputLabel' field.
			/// </summary>
			// Token: 0x04004B2C RID: 19244
			public static readonly StringName _inputLabel = "_inputLabel";

			/// <summary>
			/// Cached name for the '_keyBindingLabel' field.
			/// </summary>
			// Token: 0x04004B2D RID: 19245
			public static readonly StringName _keyBindingLabel = "_keyBindingLabel";

			/// <summary>
			/// Cached name for the '_controllerBindingIcon' field.
			/// </summary>
			// Token: 0x04004B2E RID: 19246
			public static readonly StringName _controllerBindingIcon = "_controllerBindingIcon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200137D RID: 4989
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
