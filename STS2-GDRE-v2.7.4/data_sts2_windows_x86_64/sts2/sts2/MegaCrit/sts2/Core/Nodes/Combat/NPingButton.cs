using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Ping button. Flies in from off-screen after a short delay when the local player is ready but remote players are not.
	/// </summary>
	// Token: 0x02000407 RID: 1031
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NPingButton.cs")]
	public class NPingButton : NButton
	{
		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x001F0710 File Offset: 0x001EE910
		private Vector2 ShowPos
		{
			get
			{
				return NPingButton._showPosRatio * this._viewport.GetVisibleRect().Size;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x001F073C File Offset: 0x001EE93C
		private Vector2 HidePos
		{
			get
			{
				return NPingButton._hidePosRatio * this._viewport.GetVisibleRect().Size;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06004073 RID: 16499 RVA: 0x001F0766 File Offset: 0x001EE966
		protected override string[] Hotkeys
		{
			get
			{
				return new string[] { MegaInput.select };
			}
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x001F077C File Offset: 0x001EE97C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._visuals = base.GetNode<Control>("Visuals");
			this._image = base.GetNode<TextureRect>("Visuals/Image");
			this._label = base.GetNode<MegaLabel>("Visuals/Label");
			this._viewport = base.GetViewport();
			this._hsv = (ShaderMaterial)this._image.Material;
			LocString locString = new LocString("gameplay_ui", "PING_BUTTON");
			this._label.SetTextAutoSize(locString.GetFormattedText());
			base.Position = this.HidePos;
			base.Disable();
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x001F0828 File Offset: 0x001EEA28
		public override void _EnterTree()
		{
			base._EnterTree();
			CombatManager.Instance.AboutToSwitchToEnemyTurn += this.OnAboutToSwitchToEnemyTurn;
			CombatManager.Instance.PlayerEndedTurn += this.AfterPlayerEndedTurn;
			CombatManager.Instance.PlayerUnendedTurn += this.AfterPlayerUnendedTurn;
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x001F0880 File Offset: 0x001EEA80
		public override void _ExitTree()
		{
			base._ExitTree();
			CancellationTokenSource showCancelTokenSource = this._showCancelTokenSource;
			if (showCancelTokenSource != null)
			{
				showCancelTokenSource.Cancel();
			}
			this._showCancelTokenSource = null;
			CombatManager.Instance.AboutToSwitchToEnemyTurn -= this.OnAboutToSwitchToEnemyTurn;
			CombatManager.Instance.PlayerEndedTurn -= this.AfterPlayerEndedTurn;
			CombatManager.Instance.PlayerUnendedTurn -= this.AfterPlayerUnendedTurn;
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x001F08ED File Offset: 0x001EEAED
		private void AfterPlayerEndedTurn(Player player, bool _)
		{
			if (CombatManager.Instance.AllPlayersReadyToEndTurn())
			{
				this.SetState(NPingButton.State.Disabled);
				return;
			}
			if (LocalContext.IsMe(player))
			{
				this._showCancelTokenSource = new CancellationTokenSource();
				TaskHelper.RunSafely(this.AnimInAfterDelay());
			}
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x001F0922 File Offset: 0x001EEB22
		private void AfterPlayerUnendedTurn(Player player)
		{
			if (!LocalContext.IsMe(player))
			{
				return;
			}
			CancellationTokenSource showCancelTokenSource = this._showCancelTokenSource;
			if (showCancelTokenSource != null)
			{
				showCancelTokenSource.Cancel();
			}
			this.SetState(NPingButton.State.Hidden);
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x001F0948 File Offset: 0x001EEB48
		private async Task AnimInAfterDelay()
		{
			await Task.Delay(500, this._showCancelTokenSource.Token);
			if (!this._showCancelTokenSource.IsCancellationRequested)
			{
				this.SetState(NPingButton.State.Enabled);
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x001F098B File Offset: 0x001EEB8B
		private void OnAboutToSwitchToEnemyTurn(CombatState _)
		{
			this.SetState(NPingButton.State.Hidden);
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x001F0994 File Offset: 0x001EEB94
		protected override void OnRelease()
		{
			if (this._state != NPingButton.State.Enabled)
			{
				return;
			}
			RunManager.Instance.FlavorSynchronizer.SendEndTurnPing();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPingButton._v), base.IsFocused ? 1.5f : 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", Vector2.Zero, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._label, "modulate", base.IsEnabled ? StsColors.cream : StsColors.gray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x001F0AC2 File Offset: 0x001EECC2
		protected override void OnEnable()
		{
			this._image.Modulate = Colors.White;
			this._label.Modulate = StsColors.cream;
		}

		/// <summary>
		/// Called when the player presses End Turn or combat ends.
		/// </summary>
		// Token: 0x0600407D RID: 16509 RVA: 0x001F0AE4 File Offset: 0x001EECE4
		protected override void OnDisable()
		{
			NHoverTipSet.Remove(this);
			this._image.Modulate = StsColors.gray;
			this._label.Modulate = StsColors.gray;
		}

		/// <summary>
		/// Animates the button out off the screen and disables the button if it is currently enabled.
		/// </summary>
		// Token: 0x0600407E RID: 16510 RVA: 0x001F0B0C File Offset: 0x001EED0C
		private void AnimOut()
		{
			CancellationTokenSource showCancelTokenSource = this._showCancelTokenSource;
			if (showCancelTokenSource != null)
			{
				showCancelTokenSource.Cancel();
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this, "position", this.HidePos, 0.5).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Animates the button in from off the screen.
		/// </summary>
		// Token: 0x0600407F RID: 16511 RVA: 0x001F0B94 File Offset: 0x001EED94
		private void AnimIn()
		{
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this, "position", this.ShowPos, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x001F0BF7 File Offset: 0x001EEDF7
		public void OnCombatEnded()
		{
			this.SetState(NPingButton.State.Hidden);
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x001F0C00 File Offset: 0x001EEE00
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hsv.SetShaderParameter(NPingButton._v, 1.5);
			this._visuals.Position = new Vector2(0f, -2f);
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x001F0C5C File Offset: 0x001EEE5C
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPingButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", Vector2.Zero, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._label, "modulate", base.IsEnabled ? StsColors.cream : StsColors.gray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x001F0D6C File Offset: 0x001EEF6C
		protected override void OnPress()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPingButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._visuals, "position", new Vector2(0f, 4f), 0.5).SetEase(1L).SetTrans(7L);
			this._hoverTween.TweenProperty(this._label, "modulate", Colors.DarkGray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x001F0E6E File Offset: 0x001EF06E
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NPingButton._v, value);
		}

		/// <summary>
		/// Set the state of the button.
		/// Use this instead of calling Enable or Disable directly, as the enabled/disabled state of the button can also be
		/// controlled by the state of the UI above the button (see RefreshEnabled).
		/// </summary>
		// Token: 0x06004085 RID: 16517 RVA: 0x001F0E86 File Offset: 0x001EF086
		private void SetState(NPingButton.State newState)
		{
			if (this._state != newState)
			{
				if (newState == NPingButton.State.Hidden)
				{
					this.AnimOut();
				}
				if (newState == NPingButton.State.Enabled && this._state == NPingButton.State.Hidden)
				{
					this.AnimIn();
				}
				this._state = newState;
				this.RefreshEnabled();
			}
		}

		/// <summary>
		/// Refreshes whether the button is enabled or not. Should be called when a screen appears/disappears.
		/// Use this instead of calling Enable or Disable directly! The enabled/disabled state of the button should be
		/// controlled by the button itself.
		/// </summary>
		// Token: 0x06004086 RID: 16518 RVA: 0x001F0EBC File Offset: 0x001EF0BC
		public void RefreshEnabled()
		{
			bool flag = NCombatRoom.Instance == null || NCombatRoom.Instance.Mode != CombatRoomMode.ActiveCombat || !ActiveScreenContext.Instance.IsCurrent(NCombatRoom.Instance) || NCombatRoom.Instance.Ui.Hand.IsInCardSelection;
			bool flag2 = this._state == NPingButton.State.Enabled && !flag;
			if (flag2)
			{
				base.Enable();
				return;
			}
			base.Disable();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004087 RID: 16519 RVA: 0x001F0F28 File Offset: 0x001EF128
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(15)
			{
				new MethodInfo(NPingButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.OnCombatEnded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPingButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPingButton.MethodName.SetState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "newState", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPingButton.MethodName.RefreshEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004088 RID: 16520 RVA: 0x001F124C File Offset: 0x001EF44C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPingButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.OnCombatEnded && args.Count == 0)
			{
				this.OnCombatEnded();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.SetState && args.Count == 1)
			{
				this.SetState(VariantUtils.ConvertTo<NPingButton.State>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPingButton.MethodName.RefreshEnabled && args.Count == 0)
			{
				this.RefreshEnabled();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06004089 RID: 16521 RVA: 0x001F14AC File Offset: 0x001EF6AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPingButton.MethodName._Ready || (ref method) == NPingButton.MethodName._EnterTree || (ref method) == NPingButton.MethodName._ExitTree || (ref method) == NPingButton.MethodName.OnRelease || (ref method) == NPingButton.MethodName.OnEnable || (ref method) == NPingButton.MethodName.OnDisable || (ref method) == NPingButton.MethodName.AnimOut || (ref method) == NPingButton.MethodName.AnimIn || (ref method) == NPingButton.MethodName.OnCombatEnded || (ref method) == NPingButton.MethodName.OnFocus || (ref method) == NPingButton.MethodName.OnUnfocus || (ref method) == NPingButton.MethodName.OnPress || (ref method) == NPingButton.MethodName.UpdateShaderV || (ref method) == NPingButton.MethodName.SetState || (ref method) == NPingButton.MethodName.RefreshEnabled || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600408A RID: 16522 RVA: 0x001F15A4 File Offset: 0x001EF7A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPingButton.PropertyName._state)
			{
				this._state = VariantUtils.ConvertTo<NPingButton.State>(ref value);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._visuals)
			{
				this._visuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._viewport)
			{
				this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._positionTween)
			{
				this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600408B RID: 16523 RVA: 0x001F1694 File Offset: 0x001EF894
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPingButton.PropertyName.ShowPos)
			{
				Vector2 vector = this.ShowPos;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName.HidePos)
			{
				Vector2 vector = this.HidePos;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._state)
			{
				value = VariantUtils.CreateFrom<NPingButton.State>(ref this._state);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._visuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visuals);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._viewport)
			{
				value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._positionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
				return true;
			}
			if ((ref name) == NPingButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600408C RID: 16524 RVA: 0x001F1814 File Offset: 0x001EFA14
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NPingButton.PropertyName._state, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPingButton.PropertyName._visuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPingButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPingButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPingButton.PropertyName._viewport, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPingButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(5L, NPingButton.PropertyName.ShowPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NPingButton.PropertyName.HidePos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPingButton.PropertyName._positionTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPingButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(34L, NPingButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600408D RID: 16525 RVA: 0x001F1990 File Offset: 0x001EFB90
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPingButton.PropertyName._state, Variant.From<NPingButton.State>(ref this._state));
			info.AddProperty(NPingButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
			info.AddProperty(NPingButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NPingButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NPingButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
			info.AddProperty(NPingButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NPingButton.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
			info.AddProperty(NPingButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x0600408E RID: 16526 RVA: 0x001F1A54 File Offset: 0x001EFC54
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPingButton.PropertyName._state, ref variant))
			{
				this._state = variant.As<NPingButton.State>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPingButton.PropertyName._visuals, ref variant2))
			{
				this._visuals = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPingButton.PropertyName._image, ref variant3))
			{
				this._image = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPingButton.PropertyName._label, ref variant4))
			{
				this._label = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPingButton.PropertyName._viewport, ref variant5))
			{
				this._viewport = variant5.As<Viewport>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPingButton.PropertyName._hsv, ref variant6))
			{
				this._hsv = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPingButton.PropertyName._positionTween, ref variant7))
			{
				this._positionTween = variant7.As<Tween>();
			}
			Variant variant8;
			if (info.TryGetProperty(NPingButton.PropertyName._hoverTween, ref variant8))
			{
				this._hoverTween = variant8.As<Tween>();
			}
		}

		// Token: 0x04001915 RID: 6421
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001916 RID: 6422
		private const double _flyInOutDuration = 0.5;

		// Token: 0x04001917 RID: 6423
		private NPingButton.State _state = NPingButton.State.Hidden;

		// Token: 0x04001918 RID: 6424
		private Control _visuals;

		// Token: 0x04001919 RID: 6425
		private TextureRect _image;

		// Token: 0x0400191A RID: 6426
		private MegaLabel _label;

		// Token: 0x0400191B RID: 6427
		private Viewport _viewport;

		// Token: 0x0400191C RID: 6428
		private ShaderMaterial _hsv;

		// Token: 0x0400191D RID: 6429
		[Nullable(2)]
		private CancellationTokenSource _showCancelTokenSource;

		// Token: 0x0400191E RID: 6430
		private static readonly Vector2 _showPosRatio = new Vector2(1536f, 932f) / NGame.devResolution;

		// Token: 0x0400191F RID: 6431
		private static readonly Vector2 _hidePosRatio = NPingButton._showPosRatio + new Vector2(0f, 250f) / NGame.devResolution;

		// Token: 0x04001920 RID: 6432
		[Nullable(2)]
		private Tween _positionTween;

		// Token: 0x04001921 RID: 6433
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x02001903 RID: 6403
		[NullableContext(0)]
		private enum State
		{
			// Token: 0x0400623D RID: 25149
			Enabled,
			// Token: 0x0400623E RID: 25150
			Disabled,
			// Token: 0x0400623F RID: 25151
			Hidden
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001904 RID: 6404
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006240 RID: 25152
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04006241 RID: 25153
			public new static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006242 RID: 25154
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04006243 RID: 25155
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04006244 RID: 25156
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04006245 RID: 25157
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x04006246 RID: 25158
			public static readonly StringName AnimOut = "AnimOut";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x04006247 RID: 25159
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'OnCombatEnded' method.
			/// </summary>
			// Token: 0x04006248 RID: 25160
			public static readonly StringName OnCombatEnded = "OnCombatEnded";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04006249 RID: 25161
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400624A RID: 25162
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400624B RID: 25163
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x0400624C RID: 25164
			public static readonly StringName UpdateShaderV = "UpdateShaderV";

			/// <summary>
			/// Cached name for the 'SetState' method.
			/// </summary>
			// Token: 0x0400624D RID: 25165
			public static readonly StringName SetState = "SetState";

			/// <summary>
			/// Cached name for the 'RefreshEnabled' method.
			/// </summary>
			// Token: 0x0400624E RID: 25166
			public static readonly StringName RefreshEnabled = "RefreshEnabled";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001905 RID: 6405
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ShowPos' property.
			/// </summary>
			// Token: 0x0400624F RID: 25167
			public static readonly StringName ShowPos = "ShowPos";

			/// <summary>
			/// Cached name for the 'HidePos' property.
			/// </summary>
			// Token: 0x04006250 RID: 25168
			public static readonly StringName HidePos = "HidePos";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04006251 RID: 25169
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_state' field.
			/// </summary>
			// Token: 0x04006252 RID: 25170
			public static readonly StringName _state = "_state";

			/// <summary>
			/// Cached name for the '_visuals' field.
			/// </summary>
			// Token: 0x04006253 RID: 25171
			public static readonly StringName _visuals = "_visuals";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04006254 RID: 25172
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04006255 RID: 25173
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_viewport' field.
			/// </summary>
			// Token: 0x04006256 RID: 25174
			public static readonly StringName _viewport = "_viewport";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04006257 RID: 25175
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_positionTween' field.
			/// </summary>
			// Token: 0x04006258 RID: 25176
			public static readonly StringName _positionTween = "_positionTween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04006259 RID: 25177
			public static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001906 RID: 6406
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
