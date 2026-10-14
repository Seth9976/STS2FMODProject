using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	// Token: 0x0200033D RID: 829
	[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NAscensionPanel.cs")]
	public class NAscensionPanel : Control
	{
		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x0015312A File Offset: 0x0015132A
		// (set) Token: 0x06002ED6 RID: 11990 RVA: 0x00153132 File Offset: 0x00151332
		public int Ascension { get; private set; }

		// Token: 0x06002ED7 RID: 11991 RVA: 0x0015313C File Offset: 0x0015133C
		public override void _Ready()
		{
			this._leftTriggerIcon = base.GetNode<TextureRect>("%LeftTriggerIcon");
			this._rightTriggerIcon = base.GetNode<TextureRect>("%RightTriggerIcon");
			this._leftArrow = base.GetNode<NButton>("HBoxContainer/LeftArrowContainer/LeftArrow");
			this._rightArrow = base.GetNode<NButton>("HBoxContainer/RightArrowContainer/RightArrow");
			this._ascensionLevel = base.GetNode<MegaLabel>("HBoxContainer/AscensionIconContainer/AscensionIcon/AscensionLevel");
			this._info = base.GetNode<MegaRichTextLabel>("HBoxContainer/AscensionDescription/Description");
			this._iconHsv = (ShaderMaterial)base.GetNode<Control>("%AscensionIcon").Material;
			this._leftArrow.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.DecrementAscension();
			}), 0U);
			this._rightArrow.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.IncrementAscension();
			}), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			this.UpdateControllerButton();
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x001532A0 File Offset: 0x001514A0
		public void Initialize(MultiplayerUiMode mode)
		{
			this._mode = mode;
			if (this._mode == MultiplayerUiMode.Host)
			{
				this.SetFireBlue();
				this._arrowsVisible = true;
				this.SetMaxAscension(SaveManager.Instance.Progress.MaxMultiplayerAscension);
				this.SetAscensionLevel(Math.Min(this._maxAscension, SaveManager.Instance.Progress.PreferredMultiplayerAscension));
				NHotkeyManager.Instance.PushHotkeyPressedBinding(NAscensionPanel._tabLeftHotkey, new Action(this.DecrementAscension));
				NHotkeyManager.Instance.PushHotkeyPressedBinding(NAscensionPanel._tabRightHotkey, new Action(this.IncrementAscension));
				return;
			}
			if (this._mode == MultiplayerUiMode.Singleplayer)
			{
				this.SetFireRed();
				this._arrowsVisible = true;
				this.SetMaxAscension(0);
				this.SetAscensionLevel(0);
				NHotkeyManager.Instance.PushHotkeyPressedBinding(NAscensionPanel._tabLeftHotkey, new Action(this.DecrementAscension));
				NHotkeyManager.Instance.PushHotkeyPressedBinding(NAscensionPanel._tabRightHotkey, new Action(this.IncrementAscension));
				return;
			}
			MultiplayerUiMode mode2 = this._mode;
			bool flag = mode2 - MultiplayerUiMode.Client <= 1;
			if (flag)
			{
				this.SetFireBlue();
				this._arrowsVisible = false;
				this.SetMaxAscension(0);
			}
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x001533D4 File Offset: 0x001515D4
		private void SetFireBlue()
		{
			this._iconHsv.SetShaderParameter(NAscensionPanel._h, 0.52f);
			this._iconHsv.SetShaderParameter(NAscensionPanel._v, 1.2f);
			this._ascensionLevel.AddThemeColorOverride(NAscensionPanel._fontOutlineTheme, NAscensionPanel._blueLabelOutline);
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x0015342C File Offset: 0x0015162C
		private void SetFireRed()
		{
			this._iconHsv.SetShaderParameter(NAscensionPanel._h, 1f);
			this._iconHsv.SetShaderParameter(NAscensionPanel._v, 1f);
			this._ascensionLevel.AddThemeColorOverride(NAscensionPanel._fontOutlineTheme, NAscensionPanel._redLabelOutline);
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x00153484 File Offset: 0x00151684
		public void Cleanup()
		{
			MultiplayerUiMode mode = this._mode;
			bool flag = mode - MultiplayerUiMode.Singleplayer <= 1;
			if (flag)
			{
				NHotkeyManager.Instance.RemoveHotkeyPressedBinding(NAscensionPanel._tabLeftHotkey, new Action(this.DecrementAscension));
				NHotkeyManager.Instance.RemoveHotkeyPressedBinding(NAscensionPanel._tabRightHotkey, new Action(this.IncrementAscension));
			}
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x001534E7 File Offset: 0x001516E7
		public void SetAscensionLevel(int ascension)
		{
			if (this.Ascension != ascension)
			{
				this.Ascension = ascension;
				base.EmitSignal(NAscensionPanel.SignalName.AscensionLevelChanged, Array.Empty<Variant>());
			}
			this.RefreshAscensionText();
			this.RefreshArrowVisibility();
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x00153516 File Offset: 0x00151716
		private void IncrementAscension()
		{
			if (this.Ascension < this._maxAscension)
			{
				this.SetAscensionLevel(this.Ascension + 1);
			}
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x00153534 File Offset: 0x00151734
		private void DecrementAscension()
		{
			if (this.Ascension > 0)
			{
				this.SetAscensionLevel(this.Ascension - 1);
			}
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x00153550 File Offset: 0x00151750
		private void RefreshArrowVisibility()
		{
			this._leftArrow.Visible = this._arrowsVisible && this.Ascension != 0;
			this._rightArrow.Visible = this._arrowsVisible && this.Ascension != this._maxAscension;
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x001535A4 File Offset: 0x001517A4
		public void SetMaxAscension(int maxAscension)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Max ascension changed to ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(maxAscension);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._maxAscension = maxAscension;
			if (this.Ascension >= this._maxAscension)
			{
				this.SetAscensionLevel(this._maxAscension);
			}
			base.Visible = this._maxAscension > 0;
			this.RefreshArrowVisibility();
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x00153614 File Offset: 0x00151814
		private void RefreshAscensionText()
		{
			this._ascensionLevel.SetTextAutoSize(this.Ascension.ToString());
			string formattedText = AscensionHelper.GetTitle(this.Ascension).GetFormattedText();
			string formattedText2 = AscensionHelper.GetDescription(this.Ascension).GetFormattedText();
			this._info.Text = "[b][gold]" + formattedText + "[/gold][/b]\n" + formattedText2;
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x00153678 File Offset: 0x00151878
		public void AnimIn()
		{
			if (!base.Visible)
			{
				return;
			}
			Color modulate = base.Modulate;
			modulate.A = 0f;
			base.Modulate = modulate;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.2);
			this._tween.TweenProperty(this, "position:y", base.Position.Y, 0.3).From(base.Position.Y + 30f).SetEase(1L)
				.SetTrans(10L);
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00153750 File Offset: 0x00151950
		private void UpdateControllerButton()
		{
			MultiplayerUiMode mode = this._mode;
			bool flag = mode - MultiplayerUiMode.Singleplayer <= 1;
			if (flag)
			{
				this._leftTriggerIcon.Visible = NControllerManager.Instance.IsUsingController;
				this._rightTriggerIcon.Visible = NControllerManager.Instance.IsUsingController;
				this._leftTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.viewDeckAndTabLeft);
				this._rightTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.viewExhaustPileAndTabRight);
				return;
			}
			this._leftTriggerIcon.Visible = false;
			this._rightTriggerIcon.Visible = false;
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x001537F4 File Offset: 0x001519F4
		public void Disable()
		{
			this._leftArrow.Disable();
			this._rightArrow.Disable();
			this._leftTriggerIcon.Visible = false;
			this._rightTriggerIcon.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002EE5 RID: 12005 RVA: 0x00153824 File Offset: 0x00151A24
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NAscensionPanel.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "mode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAscensionPanel.MethodName.SetFireBlue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.SetFireRed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.SetAscensionLevel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "ascension", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAscensionPanel.MethodName.IncrementAscension, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.DecrementAscension, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.RefreshArrowVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.SetMaxAscension, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "maxAscension", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAscensionPanel.MethodName.RefreshAscensionText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.UpdateControllerButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionPanel.MethodName.Disable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002EE6 RID: 12006 RVA: 0x00153B3C File Offset: 0x00151D3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAscensionPanel.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.Initialize && args.Count == 1)
			{
				this.Initialize(VariantUtils.ConvertTo<MultiplayerUiMode>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.SetFireBlue && args.Count == 0)
			{
				this.SetFireBlue();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.SetFireRed && args.Count == 0)
			{
				this.SetFireRed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.SetAscensionLevel && args.Count == 1)
			{
				this.SetAscensionLevel(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.IncrementAscension && args.Count == 0)
			{
				this.IncrementAscension();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.DecrementAscension && args.Count == 0)
			{
				this.DecrementAscension();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.RefreshArrowVisibility && args.Count == 0)
			{
				this.RefreshArrowVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.SetMaxAscension && args.Count == 1)
			{
				this.SetMaxAscension(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.RefreshAscensionText && args.Count == 0)
			{
				this.RefreshAscensionText();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.UpdateControllerButton && args.Count == 0)
			{
				this.UpdateControllerButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionPanel.MethodName.Disable && args.Count == 0)
			{
				this.Disable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002EE7 RID: 12007 RVA: 0x00153D84 File Offset: 0x00151F84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAscensionPanel.MethodName._Ready || (ref method) == NAscensionPanel.MethodName.Initialize || (ref method) == NAscensionPanel.MethodName.SetFireBlue || (ref method) == NAscensionPanel.MethodName.SetFireRed || (ref method) == NAscensionPanel.MethodName.Cleanup || (ref method) == NAscensionPanel.MethodName.SetAscensionLevel || (ref method) == NAscensionPanel.MethodName.IncrementAscension || (ref method) == NAscensionPanel.MethodName.DecrementAscension || (ref method) == NAscensionPanel.MethodName.RefreshArrowVisibility || (ref method) == NAscensionPanel.MethodName.SetMaxAscension || (ref method) == NAscensionPanel.MethodName.RefreshAscensionText || (ref method) == NAscensionPanel.MethodName.AnimIn || (ref method) == NAscensionPanel.MethodName.UpdateControllerButton || (ref method) == NAscensionPanel.MethodName.Disable || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002EE8 RID: 12008 RVA: 0x00153E6C File Offset: 0x0015206C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAscensionPanel.PropertyName.Ascension)
			{
				this.Ascension = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._maxAscension)
			{
				this._maxAscension = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._leftArrow)
			{
				this._leftArrow = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._rightArrow)
			{
				this._rightArrow = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._ascensionLevel)
			{
				this._ascensionLevel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._info)
			{
				this._info = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._leftTriggerIcon)
			{
				this._leftTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._rightTriggerIcon)
			{
				this._rightTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._iconHsv)
			{
				this._iconHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._arrowsVisible)
			{
				this._arrowsVisible = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._mode)
			{
				this._mode = VariantUtils.ConvertTo<MultiplayerUiMode>(ref value);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002EE9 RID: 12009 RVA: 0x00153FC8 File Offset: 0x001521C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAscensionPanel.PropertyName.Ascension)
			{
				int ascension = this.Ascension;
				value = VariantUtils.CreateFrom<int>(ref ascension);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._maxAscension)
			{
				value = VariantUtils.CreateFrom<int>(ref this._maxAscension);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._leftArrow)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._leftArrow);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._rightArrow)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._rightArrow);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._ascensionLevel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLevel);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._info)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._info);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._leftTriggerIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._leftTriggerIcon);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._rightTriggerIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._rightTriggerIcon);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._iconHsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._iconHsv);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._arrowsVisible)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._arrowsVisible);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._mode)
			{
				value = VariantUtils.CreateFrom<MultiplayerUiMode>(ref this._mode);
				return true;
			}
			if ((ref name) == NAscensionPanel.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002EEA RID: 12010 RVA: 0x00154160 File Offset: 0x00152360
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NAscensionPanel.PropertyName.Ascension, 0L, "", 4096L, false),
				new PropertyInfo(2L, NAscensionPanel.PropertyName._maxAscension, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._leftArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._rightArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._ascensionLevel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._info, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._leftTriggerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._rightTriggerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._iconHsv, 0L, "", 4096L, false),
				new PropertyInfo(1L, NAscensionPanel.PropertyName._arrowsVisible, 0L, "", 4096L, false),
				new PropertyInfo(2L, NAscensionPanel.PropertyName._mode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAscensionPanel.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002EEB RID: 12011 RVA: 0x001542FC File Offset: 0x001524FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName ascension = NAscensionPanel.PropertyName.Ascension;
			int ascension2 = this.Ascension;
			info.AddProperty(ascension, Variant.From<int>(ref ascension2));
			info.AddProperty(NAscensionPanel.PropertyName._maxAscension, Variant.From<int>(ref this._maxAscension));
			info.AddProperty(NAscensionPanel.PropertyName._leftArrow, Variant.From<NButton>(ref this._leftArrow));
			info.AddProperty(NAscensionPanel.PropertyName._rightArrow, Variant.From<NButton>(ref this._rightArrow));
			info.AddProperty(NAscensionPanel.PropertyName._ascensionLevel, Variant.From<MegaLabel>(ref this._ascensionLevel));
			info.AddProperty(NAscensionPanel.PropertyName._info, Variant.From<MegaRichTextLabel>(ref this._info));
			info.AddProperty(NAscensionPanel.PropertyName._leftTriggerIcon, Variant.From<TextureRect>(ref this._leftTriggerIcon));
			info.AddProperty(NAscensionPanel.PropertyName._rightTriggerIcon, Variant.From<TextureRect>(ref this._rightTriggerIcon));
			info.AddProperty(NAscensionPanel.PropertyName._iconHsv, Variant.From<ShaderMaterial>(ref this._iconHsv));
			info.AddProperty(NAscensionPanel.PropertyName._arrowsVisible, Variant.From<bool>(ref this._arrowsVisible));
			info.AddProperty(NAscensionPanel.PropertyName._mode, Variant.From<MultiplayerUiMode>(ref this._mode));
			info.AddProperty(NAscensionPanel.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddSignalEventDelegate(NAscensionPanel.SignalName.AscensionLevelChanged, this.backing_AscensionLevelChanged);
		}

		/// <inheritdoc />
		// Token: 0x06002EEC RID: 12012 RVA: 0x0015442C File Offset: 0x0015262C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAscensionPanel.PropertyName.Ascension, ref variant))
			{
				this.Ascension = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._maxAscension, ref variant2))
			{
				this._maxAscension = variant2.As<int>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._leftArrow, ref variant3))
			{
				this._leftArrow = variant3.As<NButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._rightArrow, ref variant4))
			{
				this._rightArrow = variant4.As<NButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._ascensionLevel, ref variant5))
			{
				this._ascensionLevel = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._info, ref variant6))
			{
				this._info = variant6.As<MegaRichTextLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._leftTriggerIcon, ref variant7))
			{
				this._leftTriggerIcon = variant7.As<TextureRect>();
			}
			Variant variant8;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._rightTriggerIcon, ref variant8))
			{
				this._rightTriggerIcon = variant8.As<TextureRect>();
			}
			Variant variant9;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._iconHsv, ref variant9))
			{
				this._iconHsv = variant9.As<ShaderMaterial>();
			}
			Variant variant10;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._arrowsVisible, ref variant10))
			{
				this._arrowsVisible = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._mode, ref variant11))
			{
				this._mode = variant11.As<MultiplayerUiMode>();
			}
			Variant variant12;
			if (info.TryGetProperty(NAscensionPanel.PropertyName._tween, ref variant12))
			{
				this._tween = variant12.As<Tween>();
			}
			NAscensionPanel.AscensionLevelChangedEventHandler ascensionLevelChangedEventHandler;
			if (info.TryGetSignalEventDelegate<NAscensionPanel.AscensionLevelChangedEventHandler>(NAscensionPanel.SignalName.AscensionLevelChanged, ref ascensionLevelChangedEventHandler))
			{
				this.backing_AscensionLevelChanged = ascensionLevelChangedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002EED RID: 12013 RVA: 0x001545A8 File Offset: 0x001527A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NAscensionPanel.SignalName.AscensionLevelChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NAscensionPanel.AscensionLevelChangedEventHandler" />
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06002EEE RID: 12014 RVA: 0x001545EC File Offset: 0x001527EC
		// (remove) Token: 0x06002EEF RID: 12015 RVA: 0x00154605 File Offset: 0x00152805
		public event NAscensionPanel.AscensionLevelChangedEventHandler AscensionLevelChanged
		{
			add
			{
				this.backing_AscensionLevelChanged = (NAscensionPanel.AscensionLevelChangedEventHandler)Delegate.Combine(this.backing_AscensionLevelChanged, value);
			}
			remove
			{
				this.backing_AscensionLevelChanged = (NAscensionPanel.AscensionLevelChangedEventHandler)Delegate.Remove(this.backing_AscensionLevelChanged, value);
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x0015461E File Offset: 0x0015281E
		protected void EmitSignalAscensionLevelChanged()
		{
			base.EmitSignal(NAscensionPanel.SignalName.AscensionLevelChanged, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06002EF1 RID: 12017 RVA: 0x00154631 File Offset: 0x00152831
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NAscensionPanel.SignalName.AscensionLevelChanged) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NAscensionPanel.AscensionLevelChangedEventHandler ascensionLevelChangedEventHandler = this.backing_AscensionLevelChanged;
			if (ascensionLevelChangedEventHandler == null)
			{
				return;
			}
			ascensionLevelChangedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06002EF2 RID: 12018 RVA: 0x00154662 File Offset: 0x00152862
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NAscensionPanel.SignalName.AscensionLevelChanged || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040011D9 RID: 4569
		[Nullable(1)]
		private static readonly StringName _tabLeftHotkey = MegaInput.viewDeckAndTabLeft;

		// Token: 0x040011DA RID: 4570
		[Nullable(1)]
		private static readonly StringName _tabRightHotkey = MegaInput.viewExhaustPileAndTabRight;

		// Token: 0x040011DB RID: 4571
		[Nullable(1)]
		private static readonly StringName _fontOutlineTheme = "font_outline_color";

		// Token: 0x040011DC RID: 4572
		[Nullable(1)]
		private static readonly StringName _h = new StringName("h");

		// Token: 0x040011DD RID: 4573
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040011DE RID: 4574
		private static readonly Color _redLabelOutline = new Color("593400");

		// Token: 0x040011DF RID: 4575
		private static readonly Color _blueLabelOutline = new Color("004759");

		// Token: 0x040011E1 RID: 4577
		private int _maxAscension;

		// Token: 0x040011E2 RID: 4578
		[Nullable(1)]
		private NButton _leftArrow;

		// Token: 0x040011E3 RID: 4579
		[Nullable(1)]
		private NButton _rightArrow;

		// Token: 0x040011E4 RID: 4580
		[Nullable(1)]
		private MegaLabel _ascensionLevel;

		// Token: 0x040011E5 RID: 4581
		[Nullable(1)]
		private MegaRichTextLabel _info;

		// Token: 0x040011E6 RID: 4582
		[Nullable(1)]
		private TextureRect _leftTriggerIcon;

		// Token: 0x040011E7 RID: 4583
		[Nullable(1)]
		private TextureRect _rightTriggerIcon;

		// Token: 0x040011E8 RID: 4584
		[Nullable(1)]
		private ShaderMaterial _iconHsv;

		// Token: 0x040011E9 RID: 4585
		private bool _arrowsVisible = true;

		// Token: 0x040011EA RID: 4586
		private MultiplayerUiMode _mode = MultiplayerUiMode.Singleplayer;

		// Token: 0x040011EB RID: 4587
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040011EC RID: 4588
		private NAscensionPanel.AscensionLevelChangedEventHandler backing_AscensionLevelChanged;

		// Token: 0x020015D5 RID: 5589
		// (Invoke) Token: 0x0600A375 RID: 41845
		[Signal]
		public delegate void AscensionLevelChangedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015D6 RID: 5590
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005426 RID: 21542
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x04005427 RID: 21543
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'SetFireBlue' method.
			/// </summary>
			// Token: 0x04005428 RID: 21544
			public static readonly StringName SetFireBlue = "SetFireBlue";

			/// <summary>
			/// Cached name for the 'SetFireRed' method.
			/// </summary>
			// Token: 0x04005429 RID: 21545
			public static readonly StringName SetFireRed = "SetFireRed";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x0400542A RID: 21546
			public static readonly StringName Cleanup = "Cleanup";

			/// <summary>
			/// Cached name for the 'SetAscensionLevel' method.
			/// </summary>
			// Token: 0x0400542B RID: 21547
			public static readonly StringName SetAscensionLevel = "SetAscensionLevel";

			/// <summary>
			/// Cached name for the 'IncrementAscension' method.
			/// </summary>
			// Token: 0x0400542C RID: 21548
			public static readonly StringName IncrementAscension = "IncrementAscension";

			/// <summary>
			/// Cached name for the 'DecrementAscension' method.
			/// </summary>
			// Token: 0x0400542D RID: 21549
			public static readonly StringName DecrementAscension = "DecrementAscension";

			/// <summary>
			/// Cached name for the 'RefreshArrowVisibility' method.
			/// </summary>
			// Token: 0x0400542E RID: 21550
			public static readonly StringName RefreshArrowVisibility = "RefreshArrowVisibility";

			/// <summary>
			/// Cached name for the 'SetMaxAscension' method.
			/// </summary>
			// Token: 0x0400542F RID: 21551
			public static readonly StringName SetMaxAscension = "SetMaxAscension";

			/// <summary>
			/// Cached name for the 'RefreshAscensionText' method.
			/// </summary>
			// Token: 0x04005430 RID: 21552
			public static readonly StringName RefreshAscensionText = "RefreshAscensionText";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x04005431 RID: 21553
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'UpdateControllerButton' method.
			/// </summary>
			// Token: 0x04005432 RID: 21554
			public static readonly StringName UpdateControllerButton = "UpdateControllerButton";

			/// <summary>
			/// Cached name for the 'Disable' method.
			/// </summary>
			// Token: 0x04005433 RID: 21555
			public static readonly StringName Disable = "Disable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015D7 RID: 5591
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Ascension' property.
			/// </summary>
			// Token: 0x04005434 RID: 21556
			public static readonly StringName Ascension = "Ascension";

			/// <summary>
			/// Cached name for the '_maxAscension' field.
			/// </summary>
			// Token: 0x04005435 RID: 21557
			public static readonly StringName _maxAscension = "_maxAscension";

			/// <summary>
			/// Cached name for the '_leftArrow' field.
			/// </summary>
			// Token: 0x04005436 RID: 21558
			public static readonly StringName _leftArrow = "_leftArrow";

			/// <summary>
			/// Cached name for the '_rightArrow' field.
			/// </summary>
			// Token: 0x04005437 RID: 21559
			public static readonly StringName _rightArrow = "_rightArrow";

			/// <summary>
			/// Cached name for the '_ascensionLevel' field.
			/// </summary>
			// Token: 0x04005438 RID: 21560
			public static readonly StringName _ascensionLevel = "_ascensionLevel";

			/// <summary>
			/// Cached name for the '_info' field.
			/// </summary>
			// Token: 0x04005439 RID: 21561
			public static readonly StringName _info = "_info";

			/// <summary>
			/// Cached name for the '_leftTriggerIcon' field.
			/// </summary>
			// Token: 0x0400543A RID: 21562
			public static readonly StringName _leftTriggerIcon = "_leftTriggerIcon";

			/// <summary>
			/// Cached name for the '_rightTriggerIcon' field.
			/// </summary>
			// Token: 0x0400543B RID: 21563
			public static readonly StringName _rightTriggerIcon = "_rightTriggerIcon";

			/// <summary>
			/// Cached name for the '_iconHsv' field.
			/// </summary>
			// Token: 0x0400543C RID: 21564
			public static readonly StringName _iconHsv = "_iconHsv";

			/// <summary>
			/// Cached name for the '_arrowsVisible' field.
			/// </summary>
			// Token: 0x0400543D RID: 21565
			public static readonly StringName _arrowsVisible = "_arrowsVisible";

			/// <summary>
			/// Cached name for the '_mode' field.
			/// </summary>
			// Token: 0x0400543E RID: 21566
			public static readonly StringName _mode = "_mode";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400543F RID: 21567
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015D8 RID: 5592
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'AscensionLevelChanged' signal.
			/// </summary>
			// Token: 0x04005440 RID: 21568
			public static readonly StringName AscensionLevelChanged = "AscensionLevelChanged";
		}
	}
}
