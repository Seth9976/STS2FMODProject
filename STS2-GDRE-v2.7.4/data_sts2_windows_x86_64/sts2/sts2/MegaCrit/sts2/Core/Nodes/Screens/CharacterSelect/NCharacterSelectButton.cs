using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	/// <summary>
	/// The button you click to select a character before starting a run.
	/// </summary>
	// Token: 0x0200033E RID: 830
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NCharacterSelectButton.cs")]
	public class NCharacterSelectButton : NButton
	{
		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002EF7 RID: 12023 RVA: 0x0015470C File Offset: 0x0015290C
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NCharacterSelectButton._playerIconScenePath,
					NCharacterSelectButton._unlockedIconPath
				});
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x00154729 File Offset: 0x00152929
		// (set) Token: 0x06002EF9 RID: 12025 RVA: 0x00154731 File Offset: 0x00152931
		public bool IsRandom { get; private set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x0015473A File Offset: 0x0015293A
		public IReadOnlyCollection<ulong> RemoteSelectedPlayers
		{
			get
			{
				return this._remoteSelectedPlayers;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002EFB RID: 12027 RVA: 0x00154742 File Offset: 0x00152942
		public CharacterModel Character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x0015474A File Offset: 0x0015294A
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002EFD RID: 12029 RVA: 0x00154752 File Offset: 0x00152952
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x0015475C File Offset: 0x0015295C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._iconAdd = base.GetNode<TextureRect>("%IconAdd");
			this._lock = base.GetNode<TextureRect>("%Lock");
			this._outlineLocal = base.GetNode<Control>("%OutlineLocal");
			this._outlineRemote = base.GetNode<Control>("%OutlineRemote");
			this._outlineMixed = base.GetNode<Control>("%OutlineMixed");
			this._shadow = base.GetNode<Control>("%Shadow");
			this._playerIconContainer = base.GetNode<Control>("%PlayerIconContainer");
			this._hsv = (ShaderMaterial)this._icon.Material;
			this._hsv.SetShaderParameter(NCharacterSelectButton._s, 0.2f);
			this._hsv.SetShaderParameter(NCharacterSelectButton._v, 0.4f);
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.Select)), 0U);
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x00154888 File Offset: 0x00152A88
		public void Init(CharacterModel character, ICharacterSelectButtonDelegate del)
		{
			this._delegate = del;
			this._character = character;
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			if (character is RandomCharacter)
			{
				this.IsRandom = true;
				this._isLocked = ModelDb.AllCharacters.Any<CharacterModel>((CharacterModel c) => !unlockState.Characters.Contains(c));
			}
			else
			{
				this._isLocked = !unlockState.Characters.Contains(this._character);
			}
			if (this._isLocked)
			{
				this._icon.Texture = character.CharacterSelectLockedIcon;
				this._lock.Visible = true;
				return;
			}
			this._icon.Texture = character.CharacterSelectIcon;
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x0015493C File Offset: 0x00152B3C
		protected override void OnFocus()
		{
			if (this._isSelected)
			{
				return;
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			base.Scale = NCharacterSelectButton._hoverScale;
			this._hsv.SetShaderParameter(NCharacterSelectButton._s, 1f);
			this._hsv.SetShaderParameter(NCharacterSelectButton._v, 1.1f);
			if (this._isLocked)
			{
				HoverTip hoverTip = new HoverTip(new LocString("main_menu_ui", "CHARACTER_SELECT.locked.title"), this._character.GetUnlockText(), null);
				NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
				if (nhoverTipSet != null)
				{
					nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NCharacterSelectButton._hoverTipOffset, false);
				}
			}
			SfxCmd.Play("event:/sfx/ui/clicks/ui_hover", 1f);
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x00154A04 File Offset: 0x00152C04
		protected override void OnPress()
		{
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x00154A08 File Offset: 0x00152C08
		protected override void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
			this.AnimateSaturationToCurrentState(this._hoverTween);
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x00154A84 File Offset: 0x00152C84
		public override void _Process(double delta)
		{
			if (this._currentOutline == null)
			{
				return;
			}
			Color color;
			if (this._isSelected)
			{
				float num = Mathf.Lerp(0.35f, 1f, (Mathf.Cos(Time.GetTicksMsec() * 0.001f * 1.6f * 3.1415927f) + 1f) * 0.5f);
				CanvasItem currentOutline = this._currentOutline;
				color = this._currentOutline.Modulate;
				color.A = num;
				currentOutline.Modulate = color;
				return;
			}
			CanvasItem currentOutline2 = this._currentOutline;
			color = this._currentOutline.Modulate;
			color.A = 0.5f;
			currentOutline2.Modulate = color;
		}

		/// <summary>
		/// There's a small delay before the unlock animation starts, and in that time, we want the button to look locked
		/// even though it's not.
		/// </summary>
		// Token: 0x06002F04 RID: 12036 RVA: 0x00154B24 File Offset: 0x00152D24
		public void LockForAnimation()
		{
			this._icon.Texture = this._character.CharacterSelectLockedIcon;
			this._lock.Visible = true;
			base.ZIndex = 1;
			this._lock.Modulate = Colors.White;
			base.Disable();
		}

		/// <summary>
		/// Plays the unlock animation.
		/// </summary>
		// Token: 0x06002F05 RID: 12037 RVA: 0x00154B70 File Offset: 0x00152D70
		public async Task AnimateUnlock()
		{
			GpuParticles2D chargeParticles = base.GetNode<GpuParticles2D>("%UnlockChargeParticles");
			chargeParticles.Emitting = true;
			float num = 1f;
			Vector2 originalLockPosition = this._lock.Position;
			float num2 = 0f;
			NDebugAudioManager.Instance.Play("character_unlock_charge.mp3", 1f, PitchVariance.None);
			while (num2 < 1f)
			{
				Vector2 vector = Vector2.Right.Rotated(Rng.Chaotic.NextFloat(6.2831855f)) * num;
				this._lock.Position = originalLockPosition + vector;
				float num3 = num2;
				float num4 = await this.AwaitProcessFrame(default(CancellationToken));
				num2 = num3 + num4;
				num = Mathf.Lerp(1f, 5f, Ease.QuadOut(num2));
			}
			NDebugAudioManager.Instance.Play("character_unlock.mp3", 1f, PitchVariance.None);
			this._lock.Position = originalLockPosition;
			this._lock.Texture = PreloadManager.Cache.GetTexture2D(NCharacterSelectButton._unlockedIconPath);
			this._icon.Texture = this._character.CharacterSelectIcon;
			this._iconAdd.Texture = this._icon.Texture;
			this._iconAdd.Visible = true;
			GpuParticles2D node = base.GetNode<GpuParticles2D>("%UnlockParticles");
			node.Emitting = true;
			chargeParticles.Emitting = false;
			Tween tween = base.CreateTween();
			tween.SetParallel(true);
			tween.TweenProperty(this._iconAdd, "scale", Vector2.One * 1.5f, 1.0);
			tween.TweenProperty(this._iconAdd, "modulate:a", 0f, 1.0).SetEase(0L).SetTrans(5L);
			tween.TweenProperty(this._lock, "modulate:a", 0f, 0.5).SetDelay(0.5);
			base.ZIndex = 0;
			base.Enable();
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x00154BB4 File Offset: 0x00152DB4
		public void Reset()
		{
			foreach (Node node in this._playerIconContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			this._remoteSelectedPlayers.Clear();
			this.Deselect();
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x00154C18 File Offset: 0x00152E18
		public void OnRemotePlayerSelected(ulong playerId)
		{
			this._remoteSelectedPlayers.Add(playerId);
			this.RefreshState();
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x00154C2D File Offset: 0x00152E2D
		public void OnRemotePlayerDeselected(ulong playerId)
		{
			this._remoteSelectedPlayers.Remove(playerId);
			this.RefreshState();
		}

		/// <summary>
		/// When this button is clicked on (or navigated to via controller), this button is considered Selected!
		/// </summary>
		// Token: 0x06002F09 RID: 12041 RVA: 0x00154C42 File Offset: 0x00152E42
		public void Select()
		{
			if (this._isSelected)
			{
				return;
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._isSelected = true;
			this._delegate.SelectCharacter(this, this._character);
			this.RefreshState();
		}

		/// <summary>
		/// When a player selects a button that's NOT this one, selected buttons become deselected, and this is called.
		/// </summary>
		// Token: 0x06002F0A RID: 12042 RVA: 0x00154C7D File Offset: 0x00152E7D
		public void Deselect()
		{
			this._isSelected = false;
			this.RefreshState();
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x00154C8C File Offset: 0x00152E8C
		private void RefreshState()
		{
			NCharacterSelectButton.State state;
			if (this._isSelected)
			{
				state = NCharacterSelectButton.State.SelectedLocally;
			}
			else if (this._remoteSelectedPlayers.Count > 0)
			{
				state = NCharacterSelectButton.State.SelectedRemotely;
			}
			else
			{
				state = NCharacterSelectButton.State.NotSelected;
			}
			NCharacterSelectButton.State state2 = this._state;
			if (state2 != state)
			{
				this._state = state;
				if (state2 == NCharacterSelectButton.State.NotSelected)
				{
					this._hsv.SetShaderParameter(NCharacterSelectButton._s, this.GetSaturationForCurrentState());
					this._hsv.SetShaderParameter(NCharacterSelectButton._v, this.GetValueForCurrentState());
				}
				else
				{
					Tween hoverTween = this._hoverTween;
					if (hoverTween != null)
					{
						hoverTween.Kill();
					}
					this._hoverTween = base.CreateTween().SetParallel(true);
					this.AnimateSaturationToCurrentState(this._hoverTween);
				}
			}
			this.RefreshOutline();
			this.RefreshPlayerIcons();
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x00154D44 File Offset: 0x00152F44
		private float GetSaturationForCurrentState()
		{
			float num;
			switch (this._state)
			{
			case NCharacterSelectButton.State.NotSelected:
				num = 0.2f;
				break;
			case NCharacterSelectButton.State.SelectedLocally:
				num = 1f;
				break;
			case NCharacterSelectButton.State.SelectedRemotely:
				num = 0.8f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return num;
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x00154D8C File Offset: 0x00152F8C
		private float GetValueForCurrentState()
		{
			float num;
			switch (this._state)
			{
			case NCharacterSelectButton.State.NotSelected:
				num = 0.8f;
				break;
			case NCharacterSelectButton.State.SelectedLocally:
				num = 1.1f;
				break;
			case NCharacterSelectButton.State.SelectedRemotely:
				num = 0.4f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return num;
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x00154DD4 File Offset: 0x00152FD4
		private void AnimateSaturationToCurrentState(Tween tween)
		{
			tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCharacterSelectButton._s), this.GetSaturationForCurrentState(), 0.5).SetEase(1L).SetTrans(5L);
			tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCharacterSelectButton._v), this.GetValueForCurrentState(), 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x00154E78 File Offset: 0x00153078
		private void RefreshOutline()
		{
			if (this._currentOutline != null)
			{
				this._currentOutline.Visible = false;
			}
			if (this._isSelected && this._remoteSelectedPlayers.Count > 0)
			{
				this._currentOutline = this._outlineMixed;
			}
			else if (this._isSelected)
			{
				this._currentOutline = this._outlineLocal;
			}
			else if (this._remoteSelectedPlayers.Count > 0)
			{
				this._currentOutline = this._outlineRemote;
			}
			else
			{
				this._currentOutline = null;
			}
			if (this._currentOutline != null)
			{
				this._currentOutline.Visible = true;
			}
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x00154F0C File Offset: 0x0015310C
		private void RefreshPlayerIcons()
		{
			if (this._delegate == null || this._delegate.Lobby.NetService.Type == NetGameType.Singleplayer)
			{
				return;
			}
			int num = this._remoteSelectedPlayers.Count + ((this._isSelected > false) ? 1 : 0);
			for (int i = this._playerIconContainer.GetChildCount(false); i < num; i++)
			{
				TextureRect textureRect = PreloadManager.Cache.GetScene(NCharacterSelectButton._playerIconScenePath).Instantiate<TextureRect>(0L);
				this._playerIconContainer.AddChildSafely(textureRect);
			}
			while (this._playerIconContainer.GetChildCount(false) > num)
			{
				Control child = this._playerIconContainer.GetChild<Control>(0, false);
				this._playerIconContainer.RemoveChildSafely(child);
				child.QueueFreeSafely();
			}
			for (int j = 0; j < this._playerIconContainer.GetChildCount(false); j++)
			{
				Control child2 = this._playerIconContainer.GetChild<Control>(j, false);
				child2.Modulate = ((this._isSelected && j == 0) ? StsColors.gold : StsColors.blue);
			}
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x00155005 File Offset: 0x00153205
		public void DebugUnlock()
		{
			this._icon.Texture = this._character.CharacterSelectIcon;
			this._isLocked = false;
			this._lock.Visible = false;
			base.Enable();
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x00155038 File Offset: 0x00153238
		public void UnlockIfPossible()
		{
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			if (unlockState.Characters.Contains(this._character))
			{
				this._icon.Texture = this._character.CharacterSelectIcon;
				this._isLocked = false;
				this._lock.Visible = false;
				base.Enable();
			}
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x00155092 File Offset: 0x00153292
		private void UpdateShaderH(float value)
		{
			this._hsv.SetShaderParameter(NCharacterSelectButton._h, value);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x001550AA File Offset: 0x001532AA
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NCharacterSelectButton._s, value);
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x001550C2 File Offset: 0x001532C2
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NCharacterSelectButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F16 RID: 12054 RVA: 0x001550DC File Offset: 0x001532DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(22)
			{
				new MethodInfo(NCharacterSelectButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCharacterSelectButton.MethodName.LockForAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.Reset, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.OnRemotePlayerSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCharacterSelectButton.MethodName.OnRemotePlayerDeselected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCharacterSelectButton.MethodName.Select, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.Deselect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.RefreshState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.GetSaturationForCurrentState, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.GetValueForCurrentState, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.AnimateSaturationToCurrentState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tween", 0L, "", 6L, new StringName("Tween"), false)
				}, null),
				new MethodInfo(NCharacterSelectButton.MethodName.RefreshOutline, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.RefreshPlayerIcons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.DebugUnlock, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.UnlockIfPossible, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectButton.MethodName.UpdateShaderH, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCharacterSelectButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCharacterSelectButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F17 RID: 12055 RVA: 0x0015560C File Offset: 0x0015380C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCharacterSelectButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.LockForAnimation && args.Count == 0)
			{
				this.LockForAnimation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.Reset && args.Count == 0)
			{
				this.Reset();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.OnRemotePlayerSelected && args.Count == 1)
			{
				this.OnRemotePlayerSelected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.OnRemotePlayerDeselected && args.Count == 1)
			{
				this.OnRemotePlayerDeselected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.Select && args.Count == 0)
			{
				this.Select();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.Deselect && args.Count == 0)
			{
				this.Deselect();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.RefreshState && args.Count == 0)
			{
				this.RefreshState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.GetSaturationForCurrentState && args.Count == 0)
			{
				float saturationForCurrentState = this.GetSaturationForCurrentState();
				ret = VariantUtils.CreateFrom<float>(ref saturationForCurrentState);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.GetValueForCurrentState && args.Count == 0)
			{
				float valueForCurrentState = this.GetValueForCurrentState();
				ret = VariantUtils.CreateFrom<float>(ref valueForCurrentState);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.AnimateSaturationToCurrentState && args.Count == 1)
			{
				this.AnimateSaturationToCurrentState(VariantUtils.ConvertTo<Tween>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.RefreshOutline && args.Count == 0)
			{
				this.RefreshOutline();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.RefreshPlayerIcons && args.Count == 0)
			{
				this.RefreshPlayerIcons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.DebugUnlock && args.Count == 0)
			{
				this.DebugUnlock();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.UnlockIfPossible && args.Count == 0)
			{
				this.UnlockIfPossible();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.UpdateShaderH && args.Count == 1)
			{
				this.UpdateShaderH(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002F18 RID: 12056 RVA: 0x001559C0 File Offset: 0x00153BC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCharacterSelectButton.MethodName._Ready || (ref method) == NCharacterSelectButton.MethodName.OnFocus || (ref method) == NCharacterSelectButton.MethodName.OnPress || (ref method) == NCharacterSelectButton.MethodName.OnUnfocus || (ref method) == NCharacterSelectButton.MethodName._Process || (ref method) == NCharacterSelectButton.MethodName.LockForAnimation || (ref method) == NCharacterSelectButton.MethodName.Reset || (ref method) == NCharacterSelectButton.MethodName.OnRemotePlayerSelected || (ref method) == NCharacterSelectButton.MethodName.OnRemotePlayerDeselected || (ref method) == NCharacterSelectButton.MethodName.Select || (ref method) == NCharacterSelectButton.MethodName.Deselect || (ref method) == NCharacterSelectButton.MethodName.RefreshState || (ref method) == NCharacterSelectButton.MethodName.GetSaturationForCurrentState || (ref method) == NCharacterSelectButton.MethodName.GetValueForCurrentState || (ref method) == NCharacterSelectButton.MethodName.AnimateSaturationToCurrentState || (ref method) == NCharacterSelectButton.MethodName.RefreshOutline || (ref method) == NCharacterSelectButton.MethodName.RefreshPlayerIcons || (ref method) == NCharacterSelectButton.MethodName.DebugUnlock || (ref method) == NCharacterSelectButton.MethodName.UnlockIfPossible || (ref method) == NCharacterSelectButton.MethodName.UpdateShaderH || (ref method) == NCharacterSelectButton.MethodName.UpdateShaderS || (ref method) == NCharacterSelectButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002F19 RID: 12057 RVA: 0x00155B20 File Offset: 0x00153D20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCharacterSelectButton.PropertyName.IsRandom)
			{
				this.IsRandom = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._iconAdd)
			{
				this._iconAdd = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._lock)
			{
				this._lock = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._outlineLocal)
			{
				this._outlineLocal = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._outlineRemote)
			{
				this._outlineRemote = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._outlineMixed)
			{
				this._outlineMixed = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._shadow)
			{
				this._shadow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._playerIconContainer)
			{
				this._playerIconContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._isLocked)
			{
				this._isLocked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._currentOutline)
			{
				this._currentOutline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._isSelected)
			{
				this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._state)
			{
				this._state = VariantUtils.ConvertTo<NCharacterSelectButton.State>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._hsvTween)
			{
				this._hsvTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002F1A RID: 12058 RVA: 0x00155CE8 File Offset: 0x00153EE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCharacterSelectButton.PropertyName.IsRandom)
			{
				bool flag = this.IsRandom;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName.IsLocked)
			{
				bool flag = this.IsLocked;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName.IsSelected)
			{
				bool flag = this.IsSelected;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._iconAdd)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._iconAdd);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._lock)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._lock);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._outlineLocal)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outlineLocal);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._outlineRemote)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outlineRemote);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._outlineMixed)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outlineMixed);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._shadow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._shadow);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._playerIconContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._playerIconContainer);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._isLocked)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isLocked);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._currentOutline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._currentOutline);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._isSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._state)
			{
				value = VariantUtils.CreateFrom<NCharacterSelectButton.State>(ref this._state);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NCharacterSelectButton.PropertyName._hsvTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hsvTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F1B RID: 12059 RVA: 0x00155F48 File Offset: 0x00154148
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._iconAdd, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._lock, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._outlineLocal, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._outlineRemote, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._outlineMixed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._shadow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._playerIconContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCharacterSelectButton.PropertyName._isLocked, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCharacterSelectButton.PropertyName.IsRandom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._currentOutline, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCharacterSelectButton.PropertyName._isSelected, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCharacterSelectButton.PropertyName._state, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterSelectButton.PropertyName._hsvTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCharacterSelectButton.PropertyName.IsLocked, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCharacterSelectButton.PropertyName.IsSelected, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F1C RID: 12060 RVA: 0x001561A8 File Offset: 0x001543A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isRandom = NCharacterSelectButton.PropertyName.IsRandom;
			bool isRandom2 = this.IsRandom;
			info.AddProperty(isRandom, Variant.From<bool>(ref isRandom2));
			info.AddProperty(NCharacterSelectButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NCharacterSelectButton.PropertyName._iconAdd, Variant.From<TextureRect>(ref this._iconAdd));
			info.AddProperty(NCharacterSelectButton.PropertyName._lock, Variant.From<TextureRect>(ref this._lock));
			info.AddProperty(NCharacterSelectButton.PropertyName._outlineLocal, Variant.From<Control>(ref this._outlineLocal));
			info.AddProperty(NCharacterSelectButton.PropertyName._outlineRemote, Variant.From<Control>(ref this._outlineRemote));
			info.AddProperty(NCharacterSelectButton.PropertyName._outlineMixed, Variant.From<Control>(ref this._outlineMixed));
			info.AddProperty(NCharacterSelectButton.PropertyName._shadow, Variant.From<Control>(ref this._shadow));
			info.AddProperty(NCharacterSelectButton.PropertyName._playerIconContainer, Variant.From<Control>(ref this._playerIconContainer));
			info.AddProperty(NCharacterSelectButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NCharacterSelectButton.PropertyName._isLocked, Variant.From<bool>(ref this._isLocked));
			info.AddProperty(NCharacterSelectButton.PropertyName._currentOutline, Variant.From<Control>(ref this._currentOutline));
			info.AddProperty(NCharacterSelectButton.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
			info.AddProperty(NCharacterSelectButton.PropertyName._state, Variant.From<NCharacterSelectButton.State>(ref this._state));
			info.AddProperty(NCharacterSelectButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NCharacterSelectButton.PropertyName._hsvTween, Variant.From<Tween>(ref this._hsvTween));
		}

		/// <inheritdoc />
		// Token: 0x06002F1D RID: 12061 RVA: 0x00156320 File Offset: 0x00154520
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName.IsRandom, ref variant))
			{
				this.IsRandom = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._iconAdd, ref variant3))
			{
				this._iconAdd = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._lock, ref variant4))
			{
				this._lock = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._outlineLocal, ref variant5))
			{
				this._outlineLocal = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._outlineRemote, ref variant6))
			{
				this._outlineRemote = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._outlineMixed, ref variant7))
			{
				this._outlineMixed = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._shadow, ref variant8))
			{
				this._shadow = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._playerIconContainer, ref variant9))
			{
				this._playerIconContainer = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._hsv, ref variant10))
			{
				this._hsv = variant10.As<ShaderMaterial>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._isLocked, ref variant11))
			{
				this._isLocked = variant11.As<bool>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._currentOutline, ref variant12))
			{
				this._currentOutline = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._isSelected, ref variant13))
			{
				this._isSelected = variant13.As<bool>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._state, ref variant14))
			{
				this._state = variant14.As<NCharacterSelectButton.State>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._hoverTween, ref variant15))
			{
				this._hoverTween = variant15.As<Tween>();
			}
			Variant variant16;
			if (info.TryGetProperty(NCharacterSelectButton.PropertyName._hsvTween, ref variant16))
			{
				this._hsvTween = variant16.As<Tween>();
			}
		}

		// Token: 0x040011ED RID: 4589
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040011EE RID: 4590
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040011EF RID: 4591
		private static readonly StringName _h = new StringName("h");

		// Token: 0x040011F0 RID: 4592
		private static readonly string _playerIconScenePath = SceneHelper.GetScenePath("screens/char_select/char_select_player_icon");

		// Token: 0x040011F1 RID: 4593
		private static readonly string _unlockedIconPath = ImageHelper.GetImagePath("packed/character_select/char_select_lock3_unlocked.png");

		// Token: 0x040011F2 RID: 4594
		private TextureRect _icon;

		// Token: 0x040011F3 RID: 4595
		private TextureRect _iconAdd;

		// Token: 0x040011F4 RID: 4596
		private TextureRect _lock;

		// Token: 0x040011F5 RID: 4597
		private Control _outlineLocal;

		// Token: 0x040011F6 RID: 4598
		private Control _outlineRemote;

		// Token: 0x040011F7 RID: 4599
		private Control _outlineMixed;

		// Token: 0x040011F8 RID: 4600
		private Control _shadow;

		// Token: 0x040011F9 RID: 4601
		private Control _playerIconContainer;

		// Token: 0x040011FA RID: 4602
		private CharacterModel _character;

		// Token: 0x040011FB RID: 4603
		private ShaderMaterial _hsv;

		// Token: 0x040011FC RID: 4604
		private bool _isLocked;

		// Token: 0x040011FE RID: 4606
		private static readonly Vector2 _hoverTipOffset = new Vector2(-90f, -180f);

		// Token: 0x040011FF RID: 4607
		[Nullable(2)]
		private ICharacterSelectButtonDelegate _delegate;

		// Token: 0x04001200 RID: 4608
		[Nullable(2)]
		private Control _currentOutline;

		// Token: 0x04001201 RID: 4609
		private bool _isSelected;

		// Token: 0x04001202 RID: 4610
		private readonly HashSet<ulong> _remoteSelectedPlayers = new HashSet<ulong>();

		// Token: 0x04001203 RID: 4611
		private NCharacterSelectButton.State _state;

		// Token: 0x04001204 RID: 4612
		private static readonly Vector2 _hoverScale = Vector2.One * 1.1f;

		// Token: 0x04001205 RID: 4613
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04001206 RID: 4614
		[Nullable(2)]
		private Tween _hsvTween;

		// Token: 0x04001207 RID: 4615
		private const float _unhoverDuration = 0.5f;

		// Token: 0x04001208 RID: 4616
		private const float _glowSpeed = 1.6f;

		// Token: 0x04001209 RID: 4617
		private const float _selectedSaturation = 1f;

		// Token: 0x0400120A RID: 4618
		private const float _selectedValue = 1.1f;

		// Token: 0x0400120B RID: 4619
		private const float _remotelySelectedSaturation = 0.8f;

		// Token: 0x0400120C RID: 4620
		private const float _remotelySelectedValue = 0.4f;

		// Token: 0x0400120D RID: 4621
		private const float _notSelectedSaturation = 0.2f;

		// Token: 0x0400120E RID: 4622
		private const float _notSelectedValue = 0.4f;

		// Token: 0x020015D9 RID: 5593
		[NullableContext(0)]
		private enum State
		{
			/// <summary> This button is not selected by anyone. </summary>
			// Token: 0x04005442 RID: 21570
			NotSelected,
			/// <summary> This button is selected by the local player (and maybe also remote players). </summary>
			// Token: 0x04005443 RID: 21571
			SelectedLocally,
			/// <summary> This button is not selected locally, but is selected by at least one remote player. </summary>
			// Token: 0x04005444 RID: 21572
			SelectedRemotely
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015DA RID: 5594
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005445 RID: 21573
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005446 RID: 21574
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005447 RID: 21575
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005448 RID: 21576
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005449 RID: 21577
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'LockForAnimation' method.
			/// </summary>
			// Token: 0x0400544A RID: 21578
			public static readonly StringName LockForAnimation = "LockForAnimation";

			/// <summary>
			/// Cached name for the 'Reset' method.
			/// </summary>
			// Token: 0x0400544B RID: 21579
			public static readonly StringName Reset = "Reset";

			/// <summary>
			/// Cached name for the 'OnRemotePlayerSelected' method.
			/// </summary>
			// Token: 0x0400544C RID: 21580
			public static readonly StringName OnRemotePlayerSelected = "OnRemotePlayerSelected";

			/// <summary>
			/// Cached name for the 'OnRemotePlayerDeselected' method.
			/// </summary>
			// Token: 0x0400544D RID: 21581
			public static readonly StringName OnRemotePlayerDeselected = "OnRemotePlayerDeselected";

			/// <summary>
			/// Cached name for the 'Select' method.
			/// </summary>
			// Token: 0x0400544E RID: 21582
			public static readonly StringName Select = "Select";

			/// <summary>
			/// Cached name for the 'Deselect' method.
			/// </summary>
			// Token: 0x0400544F RID: 21583
			public static readonly StringName Deselect = "Deselect";

			/// <summary>
			/// Cached name for the 'RefreshState' method.
			/// </summary>
			// Token: 0x04005450 RID: 21584
			public static readonly StringName RefreshState = "RefreshState";

			/// <summary>
			/// Cached name for the 'GetSaturationForCurrentState' method.
			/// </summary>
			// Token: 0x04005451 RID: 21585
			public static readonly StringName GetSaturationForCurrentState = "GetSaturationForCurrentState";

			/// <summary>
			/// Cached name for the 'GetValueForCurrentState' method.
			/// </summary>
			// Token: 0x04005452 RID: 21586
			public static readonly StringName GetValueForCurrentState = "GetValueForCurrentState";

			/// <summary>
			/// Cached name for the 'AnimateSaturationToCurrentState' method.
			/// </summary>
			// Token: 0x04005453 RID: 21587
			public static readonly StringName AnimateSaturationToCurrentState = "AnimateSaturationToCurrentState";

			/// <summary>
			/// Cached name for the 'RefreshOutline' method.
			/// </summary>
			// Token: 0x04005454 RID: 21588
			public static readonly StringName RefreshOutline = "RefreshOutline";

			/// <summary>
			/// Cached name for the 'RefreshPlayerIcons' method.
			/// </summary>
			// Token: 0x04005455 RID: 21589
			public static readonly StringName RefreshPlayerIcons = "RefreshPlayerIcons";

			/// <summary>
			/// Cached name for the 'DebugUnlock' method.
			/// </summary>
			// Token: 0x04005456 RID: 21590
			public static readonly StringName DebugUnlock = "DebugUnlock";

			/// <summary>
			/// Cached name for the 'UnlockIfPossible' method.
			/// </summary>
			// Token: 0x04005457 RID: 21591
			public static readonly StringName UnlockIfPossible = "UnlockIfPossible";

			/// <summary>
			/// Cached name for the 'UpdateShaderH' method.
			/// </summary>
			// Token: 0x04005458 RID: 21592
			public static readonly StringName UpdateShaderH = "UpdateShaderH";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04005459 RID: 21593
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x0400545A RID: 21594
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015DB RID: 5595
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsRandom' property.
			/// </summary>
			// Token: 0x0400545B RID: 21595
			public static readonly StringName IsRandom = "IsRandom";

			/// <summary>
			/// Cached name for the 'IsLocked' property.
			/// </summary>
			// Token: 0x0400545C RID: 21596
			public static readonly StringName IsLocked = "IsLocked";

			/// <summary>
			/// Cached name for the 'IsSelected' property.
			/// </summary>
			// Token: 0x0400545D RID: 21597
			public static readonly StringName IsSelected = "IsSelected";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x0400545E RID: 21598
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_iconAdd' field.
			/// </summary>
			// Token: 0x0400545F RID: 21599
			public static readonly StringName _iconAdd = "_iconAdd";

			/// <summary>
			/// Cached name for the '_lock' field.
			/// </summary>
			// Token: 0x04005460 RID: 21600
			public static readonly StringName _lock = "_lock";

			/// <summary>
			/// Cached name for the '_outlineLocal' field.
			/// </summary>
			// Token: 0x04005461 RID: 21601
			public static readonly StringName _outlineLocal = "_outlineLocal";

			/// <summary>
			/// Cached name for the '_outlineRemote' field.
			/// </summary>
			// Token: 0x04005462 RID: 21602
			public static readonly StringName _outlineRemote = "_outlineRemote";

			/// <summary>
			/// Cached name for the '_outlineMixed' field.
			/// </summary>
			// Token: 0x04005463 RID: 21603
			public static readonly StringName _outlineMixed = "_outlineMixed";

			/// <summary>
			/// Cached name for the '_shadow' field.
			/// </summary>
			// Token: 0x04005464 RID: 21604
			public static readonly StringName _shadow = "_shadow";

			/// <summary>
			/// Cached name for the '_playerIconContainer' field.
			/// </summary>
			// Token: 0x04005465 RID: 21605
			public static readonly StringName _playerIconContainer = "_playerIconContainer";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005466 RID: 21606
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_isLocked' field.
			/// </summary>
			// Token: 0x04005467 RID: 21607
			public static readonly StringName _isLocked = "_isLocked";

			/// <summary>
			/// Cached name for the '_currentOutline' field.
			/// </summary>
			// Token: 0x04005468 RID: 21608
			public static readonly StringName _currentOutline = "_currentOutline";

			/// <summary>
			/// Cached name for the '_isSelected' field.
			/// </summary>
			// Token: 0x04005469 RID: 21609
			public static readonly StringName _isSelected = "_isSelected";

			/// <summary>
			/// Cached name for the '_state' field.
			/// </summary>
			// Token: 0x0400546A RID: 21610
			public static readonly StringName _state = "_state";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x0400546B RID: 21611
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_hsvTween' field.
			/// </summary>
			// Token: 0x0400546C RID: 21612
			public static readonly StringName _hsvTween = "_hsvTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015DC RID: 5596
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
