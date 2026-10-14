using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Controls visual logic for the health bar! This includes the red part, the text, block icon, and handles Poison/Doom.
	/// This does NOT control the behavior/layout of Powers.
	/// </summary>
	// Token: 0x02000403 RID: 1027
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NHealthBar.cs")]
	public class NHealthBar : Control
	{
		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x001EB2A6 File Offset: 0x001E94A6
		private float MaxFgWidth
		{
			get
			{
				if (this._expectedMaxFgWidth <= 0f)
				{
					return this._hpForegroundContainer.Size.X;
				}
				return this._expectedMaxFgWidth;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x001EB2CC File Offset: 0x001E94CC
		// (set) Token: 0x06003FF9 RID: 16377 RVA: 0x001EB2D4 File Offset: 0x001E94D4
		public Control HpBarContainer { get; private set; }

		// Token: 0x06003FFA RID: 16378 RVA: 0x001EB2E0 File Offset: 0x001E94E0
		public void SetCreature(Creature creature)
		{
			if (this._creature != null)
			{
				throw new InvalidOperationException("Creature was already set.");
			}
			this._creature = creature;
			this._hpForeground.OffsetRight = this.GetFgWidth(this._creature.CurrentHp) - this.MaxFgWidth;
			this._hpMiddleground.OffsetRight = this._hpForeground.OffsetRight - 2f;
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x001EB348 File Offset: 0x001E9548
		public override void _Ready()
		{
			this.HpBarContainer = base.GetNode<Control>("%HpBarContainer");
			this._hpForegroundContainer = base.GetNode<Control>("%HpForegroundContainer");
			this._hpMiddleground = base.GetNode<Control>("%HpMiddleground");
			this._hpForeground = base.GetNode<Control>("%HpForeground");
			this._poisonForeground = base.GetNode<Control>("%PoisonForeground");
			this._doomForeground = base.GetNode<Control>("%DoomForeground");
			this._hpLabel = base.GetNode<MegaLabel>("%HpLabel");
			this._blockContainer = base.GetNode<Control>("%BlockContainer");
			this._blockLabel = base.GetNode<MegaLabel>("%BlockLabel");
			this._blockOutline = base.GetNode<Control>("%BlockOutline");
			this._infinityTex = base.GetNode<TextureRect>("%InfinityTex");
			this._originalBlockPosition = this._blockContainer.Position;
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x001EB458 File Offset: 0x001E9658
		private void DebugToggleVisibility()
		{
			base.Visible = !NCombatUi.IsDebugHidingHpBar;
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x001EB468 File Offset: 0x001E9668
		public void UpdateLayoutForCreatureBounds(Control bounds)
		{
			float num = 24f;
			MonsterModel monster = this._creature.Monster;
			float valueOrDefault = (num - ((monster != null) ? new float?(monster.HpBarSizeReduction) : null)).GetValueOrDefault();
			this.HpBarContainer.GlobalPosition = new Vector2(bounds.GlobalPosition.X - valueOrDefault * 0.5f, this.HpBarContainer.GlobalPosition.Y);
			float num2 = bounds.Size.X + valueOrDefault;
			this.SetHpBarContainerSizeWithOffsets(new Vector2(num2, this.HpBarContainer.Size.Y));
			float num3 = this._blockContainer.Size.X * 0.5f;
			this._blockContainer.GlobalPosition = new Vector2(bounds.GlobalPosition.X - num3, this._blockContainer.GlobalPosition.Y);
			this._originalBlockPosition = this._blockContainer.Position;
		}

		/// <summary>
		/// Linearly interpolates the width of the HP bar based on the reference values.
		/// </summary>
		/// <param name="refMaxHp">A reference max HP.</param>
		/// <param name="refWidth">The width of the HP bar when the max HP is equal to refMaxHp.</param>
		// Token: 0x06003FFE RID: 16382 RVA: 0x001EB584 File Offset: 0x001E9784
		public void UpdateWidthRelativeToReferenceValue(float refMaxHp, float refWidth)
		{
			Vector2 size = this.HpBarContainer.Size;
			size.X = (float)this._creature.MaxHp / refMaxHp * refWidth;
			this.SetHpBarContainerSizeWithOffsetsImmediately(size);
		}

		/// <summary>
		/// Sets the container size, deferring it to avoid issues.
		/// TODO: What issues? I'm not sure, it's from an old commit
		/// </summary>
		// Token: 0x06003FFF RID: 16383 RVA: 0x001EB5BC File Offset: 0x001E97BC
		private void SetHpBarContainerSizeWithOffsets(Vector2 size)
		{
			Callable.From(delegate
			{
				this.SetHpBarContainerSizeWithOffsetsImmediately(size);
			}).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x001EB5FC File Offset: 0x001E97FC
		private void SetHpBarContainerSizeWithOffsetsImmediately(Vector2 size)
		{
			if (this.HpBarContainer.Size.IsEqualApprox(size))
			{
				return;
			}
			Tween middlegroundTween = this._middlegroundTween;
			if (middlegroundTween != null)
			{
				middlegroundTween.Kill();
			}
			this.HpBarContainer.Size = size;
			this._expectedMaxFgWidth = size.X - 10f;
			this._hpForeground.OffsetRight = this.GetFgWidth(this._creature.CurrentHp, this._expectedMaxFgWidth) - this._expectedMaxFgWidth;
			this._hpMiddleground.OffsetRight = this._hpForeground.OffsetRight - 2f;
		}

		/// <summary>
		/// Forces the health bar to look right and start/stop animations.
		/// Called often, even if the health bar isn't affected.
		/// </summary>
		// Token: 0x06004001 RID: 16385 RVA: 0x001EB694 File Offset: 0x001E9894
		public void RefreshValues()
		{
			this.RefreshBlockUi();
			this.RefreshForeground();
			this.RefreshMiddleground();
			this.RefreshText();
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x001EB6B0 File Offset: 0x001E98B0
		private void RefreshMiddleground()
		{
			if (this._creature.CurrentHp <= 0)
			{
				this._hpMiddleground.Visible = false;
				return;
			}
			this._hpMiddleground.Visible = true;
			Control hpMiddleground = this._hpMiddleground;
			Vector2 position = this._hpMiddleground.Position;
			position.X = 1f;
			hpMiddleground.Position = position;
			int currentHp = this._creature.CurrentHp;
			int maxHp = this._creature.MaxHp;
			if (currentHp != this._currentHpOnLastRefresh || maxHp != this._maxHpOnLastRefresh)
			{
				this._currentHpOnLastRefresh = currentHp;
				this._maxHpOnLastRefresh = maxHp;
				float num = (this._poisonForeground.Visible ? this._poisonForeground.OffsetRight : this._hpForeground.OffsetRight);
				bool flag = num >= this._hpMiddleground.OffsetRight;
				this._hpMiddleground.OffsetRight += 1f;
				Tween middlegroundTween = this._middlegroundTween;
				if (middlegroundTween != null)
				{
					middlegroundTween.Kill();
				}
				this._middlegroundTween = base.CreateTween();
				this._middlegroundTween.TweenProperty(this._hpMiddleground, "offset_right", num - 2f, 1.0).SetDelay(flag ? 0.0 : 1.0).SetEase(1L)
					.SetTrans(5L);
			}
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x001EB80C File Offset: 0x001E9A0C
		private void RefreshForeground()
		{
			if (this._creature.CurrentHp <= 0)
			{
				this._poisonForeground.Visible = false;
				this._doomForeground.Visible = false;
				this._hpForeground.Visible = false;
				return;
			}
			this._hpForeground.Visible = true;
			float num = this.GetFgWidth(this._creature.CurrentHp) - this.MaxFgWidth;
			this._hpForeground.OffsetRight = num;
			if (this._creature.HpDisplay.IsInfinite())
			{
				this._hpForeground.SelfModulate = NHealthBar._invincibleForegroundColor;
				this._poisonForeground.Visible = false;
				this._doomForeground.Visible = false;
				return;
			}
			int powerAmount = this._creature.GetPowerAmount<DoomPower>();
			PoisonPower power = this._creature.GetPower<PoisonPower>();
			int num2 = ((power != null) ? power.CalculateTotalDamageNextTurn() : 0);
			if (this._creature.HasPower<PoisonPower>())
			{
				if (num2 > 0)
				{
					this._poisonForeground.Visible = true;
					if (this.IsPoisonLethal(num2))
					{
						this._poisonForeground.OffsetLeft = 0f;
						this._poisonForeground.OffsetRight = num;
						this._hpForeground.Visible = false;
					}
					else
					{
						float fgWidth = this.GetFgWidth(this._creature.CurrentHp - num2);
						this._hpForeground.OffsetRight = fgWidth - this.MaxFgWidth;
						this._hpForeground.Visible = true;
						int patchMarginLeft = ((NinePatchRect)this._poisonForeground).PatchMarginLeft;
						this._poisonForeground.OffsetLeft = Math.Max(0f, fgWidth - (float)patchMarginLeft);
						this._poisonForeground.OffsetRight = num;
					}
				}
				else
				{
					this._poisonForeground.Visible = false;
				}
			}
			else
			{
				this._poisonForeground.Visible = false;
				this._poisonForeground.OffsetLeft = 0f;
			}
			if (!this._creature.HasPower<DoomPower>())
			{
				this._doomForeground.Visible = false;
				return;
			}
			if (powerAmount <= 0)
			{
				this._doomForeground.Visible = false;
				return;
			}
			this._doomForeground.Visible = true;
			float num3 = this.GetFgWidth(powerAmount) - this.MaxFgWidth;
			if (!this.IsDoomLethal(powerAmount, num2))
			{
				int patchMarginRight = ((NinePatchRect)this._doomForeground).PatchMarginRight;
				this._doomForeground.OffsetRight = Math.Min(0f, num3 + (float)patchMarginRight);
				this._hpForeground.Visible = true;
				return;
			}
			if (!this.IsPoisonLethal(num2))
			{
				this._doomForeground.OffsetRight = this._hpForeground.OffsetRight;
				this._hpForeground.Visible = false;
				return;
			}
			this._hpForeground.Visible = false;
			this._doomForeground.Visible = false;
		}

		/// <summary>
		/// Determines whether the Block icon should be visible + the color of the health bar if you have Block.
		/// Note that the text on the health bar is handled elsewhere -&gt; RefreshText();
		/// </summary>
		// Token: 0x06004004 RID: 16388 RVA: 0x001EBAA4 File Offset: 0x001E9CA4
		private void RefreshBlockUi()
		{
			if (this._creature.Block <= 0)
			{
				Creature blockTrackingCreature = this._blockTrackingCreature;
				if (blockTrackingCreature == null || blockTrackingCreature.Block <= 0)
				{
					if (this._blockContainer.Visible)
					{
						NBlockBrokenVfx nblockBrokenVfx = NBlockBrokenVfx.Create();
						if (nblockBrokenVfx != null)
						{
							this.AddChildSafely(nblockBrokenVfx);
							nblockBrokenVfx.GlobalPosition = this._blockContainer.GlobalPosition + this._blockContainer.Size * 0.5f;
						}
					}
					this._blockContainer.Visible = false;
					this._blockOutline.Visible = false;
					this._hpForeground.SelfModulate = NHealthBar._redForegroundColor;
					return;
				}
			}
			this._blockOutline.Visible = true;
			this._hpForeground.SelfModulate = NHealthBar._blockHpForegroundColor;
			if (this._creature.Block > 0)
			{
				this._blockContainer.Visible = true;
				this._blockLabel.SetTextAutoSize(this._creature.Block.ToString());
				return;
			}
		}

		/// <summary>
		/// The text that displays HP. e.g. 72/72
		/// This logic controls this text's color and its outline as it's
		/// affected by having Block, being in lethal due to Poison/Doom, and Invincibility.
		/// </summary>
		// Token: 0x06004005 RID: 16389 RVA: 0x001EBB9C File Offset: 0x001E9D9C
		private void RefreshText()
		{
			if (this._creature.CurrentHp <= 0)
			{
				this._hpLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, NHealthBar._defaultFontColor);
				this._hpLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, NHealthBar._defaultFontOutlineColor);
				this._hpLabel.SetTextAutoSize(this._healthBarDead.GetRawText());
				return;
			}
			this._doomForeground.Modulate = (this._creature.HpDisplay.IsInfinite() ? Colors.Transparent : Colors.White);
			this._infinityTex.Visible = this._creature.HpDisplay == HpDisplay.InfiniteWithoutNumbers;
			this._hpLabel.Visible = this._creature.HpDisplay.ShowsNumbers();
			if (this._creature.HpDisplay.ShowsNumbers())
			{
				PoisonPower power = this._creature.GetPower<PoisonPower>();
				int num = ((power != null) ? power.CalculateTotalDamageNextTurn() : 0);
				int powerAmount = this._creature.GetPowerAmount<DoomPower>();
				Color color;
				Color color2;
				if (this._creature.HpDisplay == HpDisplay.InfiniteWithNumbers)
				{
					color = NHealthBar._defaultFontColor;
					color2 = NHealthBar._invincibleOutlineColor;
				}
				else if (this.IsPoisonLethal(num))
				{
					color..ctor("76FF40");
					color2..ctor("074700");
				}
				else if (this.IsDoomLethal(powerAmount, num))
				{
					color..ctor("FB8DFF");
					color2..ctor("2D1263");
				}
				else
				{
					if (this._creature.Block <= 0)
					{
						Creature blockTrackingCreature = this._blockTrackingCreature;
						if (blockTrackingCreature == null || blockTrackingCreature.Block <= 0)
						{
							color = NHealthBar._defaultFontColor;
							color2 = NHealthBar._defaultFontOutlineColor;
							goto IL_0189;
						}
					}
					color = NHealthBar._defaultFontColor;
					color2 = NHealthBar._blockOutlineColor;
				}
				IL_0189:
				this._hpLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, color);
				this._hpLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, color2);
				MegaLabel hpLabel = this._hpLabel;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._creature.CurrentHp);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._creature.MaxHp);
				hpLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x001EBD9F File Offset: 0x001E9F9F
		private bool IsPoisonLethal(int poisonDamage)
		{
			return poisonDamage > 0 && this._creature.HasPower<PoisonPower>() && poisonDamage >= this._creature.CurrentHp;
		}

		/// <summary>
		/// Checks if Doom is lethal when it's to trigger next. Poison damage is also incorporated as the
		/// creature's HP may drop at the start of their turn and their remaining HP may enter Doom range.
		/// </summary>
		// Token: 0x06004007 RID: 16391 RVA: 0x001EBDC5 File Offset: 0x001E9FC5
		private bool IsDoomLethal(int doomAmount, int poisonDamage)
		{
			return doomAmount > 0 && this._creature.HasPower<DoomPower>() && doomAmount >= this._creature.CurrentHp - poisonDamage;
		}

		/// <summary>
		/// Calculates the width of the foreground given an "amount" which is a ratio of this and the MaxHP.
		/// Example: If amount is currentHp. Then if you have 12 currentHP and 50 maxHp, this would return
		/// 0.24 (12/50) multiplied by the width of the foreground bar's total length. If the bar is 240px,
		/// then we would return 240 x 0.24 as a float.
		/// </summary>
		// Token: 0x06004008 RID: 16392 RVA: 0x001EBDED File Offset: 0x001E9FED
		private float GetFgWidth(int amount)
		{
			return this.GetFgWidth(amount, this.MaxFgWidth);
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x001EBDFC File Offset: 0x001E9FFC
		private float GetFgWidth(int amount, float maxFgWidth)
		{
			if (this._creature.MaxHp <= 0)
			{
				return 0f;
			}
			float num = (float)amount / (float)this._creature.MaxHp * maxFgWidth;
			return Math.Max(num, (this._creature.CurrentHp > 0) ? 12f : 0f);
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x001EBE50 File Offset: 0x001EA050
		public void FadeOutHpLabel(float duration, float finalAlpha)
		{
			Tween hpLabelFadeTween = this._hpLabelFadeTween;
			if (hpLabelFadeTween != null)
			{
				hpLabelFadeTween.Kill();
			}
			this._hpLabelFadeTween = base.CreateTween();
			this._hpLabelFadeTween.TweenProperty(this._hpLabel, "modulate:a", finalAlpha, (double)duration).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x001EBEAC File Offset: 0x001EA0AC
		public void FadeInHpLabel(float duration)
		{
			Tween hpLabelFadeTween = this._hpLabelFadeTween;
			if (hpLabelFadeTween != null)
			{
				hpLabelFadeTween.Kill();
			}
			this._hpLabelFadeTween = base.CreateTween();
			this._hpLabelFadeTween.TweenProperty(this._hpLabel, "modulate:a", 1f, (double)duration);
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x001EBF00 File Offset: 0x001EA100
		public void AnimateInBlock(int oldBlock, int blockGain)
		{
			if (oldBlock != 0 || blockGain == 0)
			{
				return;
			}
			this._blockContainer.Visible = true;
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				return;
			}
			this._blockContainer.Modulate = StsColors.transparentWhite;
			this._blockContainer.Position = this._originalBlockPosition - NHealthBar._blockAnimOffset;
			Tween blockTween = this._blockTween;
			if (blockTween != null)
			{
				blockTween.Kill();
			}
			this._blockTween = base.CreateTween().SetParallel(true);
			this._blockTween.TweenProperty(this._blockContainer, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(1L);
			this._blockTween.TweenProperty(this._blockContainer, "position", this._originalBlockPosition, 0.5).SetEase(1L).SetTrans(10L);
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Nodes.Combat.NCreature.TrackBlockStatus(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" /> for details.
		/// </summary>
		// Token: 0x0600400D RID: 16397 RVA: 0x001EBFFD File Offset: 0x001EA1FD
		public void TrackBlockStatus(Creature creature)
		{
			this._blockTrackingCreature = creature;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600400E RID: 16398 RVA: 0x001EC008 File Offset: 0x001EA208
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NHealthBar.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealthBar.MethodName.DebugToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealthBar.MethodName.UpdateLayoutForCreatureBounds, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "bounds", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.UpdateWidthRelativeToReferenceValue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "refMaxHp", 0L, "", 6L, false),
					new PropertyInfo(3L, "refWidth", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.SetHpBarContainerSizeWithOffsets, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "size", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.SetHpBarContainerSizeWithOffsetsImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "size", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.RefreshValues, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealthBar.MethodName.RefreshMiddleground, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealthBar.MethodName.RefreshForeground, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealthBar.MethodName.RefreshBlockUi, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealthBar.MethodName.RefreshText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealthBar.MethodName.IsPoisonLethal, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "poisonDamage", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.IsDoomLethal, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "doomAmount", 0L, "", 6L, false),
					new PropertyInfo(2L, "poisonDamage", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.GetFgWidth, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "amount", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.GetFgWidth, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "amount", 0L, "", 6L, false),
					new PropertyInfo(3L, "maxFgWidth", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.FadeOutHpLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "duration", 0L, "", 6L, false),
					new PropertyInfo(3L, "finalAlpha", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.FadeInHpLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "duration", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealthBar.MethodName.AnimateInBlock, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "oldBlock", 0L, "", 6L, false),
					new PropertyInfo(2L, "blockGain", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600400F RID: 16399 RVA: 0x001EC5B4 File Offset: 0x001EA7B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHealthBar.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.DebugToggleVisibility && args.Count == 0)
			{
				this.DebugToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.UpdateLayoutForCreatureBounds && args.Count == 1)
			{
				this.UpdateLayoutForCreatureBounds(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.UpdateWidthRelativeToReferenceValue && args.Count == 2)
			{
				this.UpdateWidthRelativeToReferenceValue(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.SetHpBarContainerSizeWithOffsets && args.Count == 1)
			{
				this.SetHpBarContainerSizeWithOffsets(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.SetHpBarContainerSizeWithOffsetsImmediately && args.Count == 1)
			{
				this.SetHpBarContainerSizeWithOffsetsImmediately(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.RefreshValues && args.Count == 0)
			{
				this.RefreshValues();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.RefreshMiddleground && args.Count == 0)
			{
				this.RefreshMiddleground();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.RefreshForeground && args.Count == 0)
			{
				this.RefreshForeground();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.RefreshBlockUi && args.Count == 0)
			{
				this.RefreshBlockUi();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.RefreshText && args.Count == 0)
			{
				this.RefreshText();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.IsPoisonLethal && args.Count == 1)
			{
				bool flag = this.IsPoisonLethal(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.IsDoomLethal && args.Count == 2)
			{
				bool flag2 = this.IsDoomLethal(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.GetFgWidth && args.Count == 1)
			{
				float fgWidth = this.GetFgWidth(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<float>(ref fgWidth);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.GetFgWidth && args.Count == 2)
			{
				float fgWidth2 = this.GetFgWidth(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = VariantUtils.CreateFrom<float>(ref fgWidth2);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.FadeOutHpLabel && args.Count == 2)
			{
				this.FadeOutHpLabel(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.FadeInHpLabel && args.Count == 1)
			{
				this.FadeInHpLabel(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealthBar.MethodName.AnimateInBlock && args.Count == 2)
			{
				this.AnimateInBlock(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004010 RID: 16400 RVA: 0x001EC95C File Offset: 0x001EAB5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHealthBar.MethodName._Ready || (ref method) == NHealthBar.MethodName.DebugToggleVisibility || (ref method) == NHealthBar.MethodName.UpdateLayoutForCreatureBounds || (ref method) == NHealthBar.MethodName.UpdateWidthRelativeToReferenceValue || (ref method) == NHealthBar.MethodName.SetHpBarContainerSizeWithOffsets || (ref method) == NHealthBar.MethodName.SetHpBarContainerSizeWithOffsetsImmediately || (ref method) == NHealthBar.MethodName.RefreshValues || (ref method) == NHealthBar.MethodName.RefreshMiddleground || (ref method) == NHealthBar.MethodName.RefreshForeground || (ref method) == NHealthBar.MethodName.RefreshBlockUi || (ref method) == NHealthBar.MethodName.RefreshText || (ref method) == NHealthBar.MethodName.IsPoisonLethal || (ref method) == NHealthBar.MethodName.IsDoomLethal || (ref method) == NHealthBar.MethodName.GetFgWidth || (ref method) == NHealthBar.MethodName.FadeOutHpLabel || (ref method) == NHealthBar.MethodName.FadeInHpLabel || (ref method) == NHealthBar.MethodName.AnimateInBlock || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004011 RID: 16401 RVA: 0x001ECA70 File Offset: 0x001EAC70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHealthBar.PropertyName.HpBarContainer)
			{
				this.HpBarContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpForegroundContainer)
			{
				this._hpForegroundContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpForeground)
			{
				this._hpForeground = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._poisonForeground)
			{
				this._poisonForeground = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._doomForeground)
			{
				this._doomForeground = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpMiddleground)
			{
				this._hpMiddleground = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpLabel)
			{
				this._hpLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockContainer)
			{
				this._blockContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockLabel)
			{
				this._blockLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockOutline)
			{
				this._blockOutline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._infinityTex)
			{
				this._infinityTex = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockTween)
			{
				this._blockTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpLabelFadeTween)
			{
				this._hpLabelFadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._middlegroundTween)
			{
				this._middlegroundTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._originalBlockPosition)
			{
				this._originalBlockPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._currentHpOnLastRefresh)
			{
				this._currentHpOnLastRefresh = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._maxHpOnLastRefresh)
			{
				this._maxHpOnLastRefresh = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._expectedMaxFgWidth)
			{
				this._expectedMaxFgWidth = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004012 RID: 16402 RVA: 0x001ECC6C File Offset: 0x001EAE6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHealthBar.PropertyName.MaxFgWidth)
			{
				float maxFgWidth = this.MaxFgWidth;
				value = VariantUtils.CreateFrom<float>(ref maxFgWidth);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName.HpBarContainer)
			{
				Control hpBarContainer = this.HpBarContainer;
				value = VariantUtils.CreateFrom<Control>(ref hpBarContainer);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpForegroundContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hpForegroundContainer);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpForeground)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hpForeground);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._poisonForeground)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._poisonForeground);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._doomForeground)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._doomForeground);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpMiddleground)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hpMiddleground);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._hpLabel);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._blockContainer);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._blockLabel);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockOutline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._blockOutline);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._infinityTex)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._infinityTex);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._blockTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._blockTween);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._hpLabelFadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hpLabelFadeTween);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._middlegroundTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._middlegroundTween);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._originalBlockPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalBlockPosition);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._currentHpOnLastRefresh)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentHpOnLastRefresh);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._maxHpOnLastRefresh)
			{
				value = VariantUtils.CreateFrom<int>(ref this._maxHpOnLastRefresh);
				return true;
			}
			if ((ref name) == NHealthBar.PropertyName._expectedMaxFgWidth)
			{
				value = VariantUtils.CreateFrom<float>(ref this._expectedMaxFgWidth);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004013 RID: 16403 RVA: 0x001ECEE8 File Offset: 0x001EB0E8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHealthBar.PropertyName._hpForegroundContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._hpForeground, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._poisonForeground, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._doomForeground, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._hpMiddleground, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._hpLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._blockContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._blockLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._blockOutline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._infinityTex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._blockTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._hpLabelFadeTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName._middlegroundTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHealthBar.PropertyName._originalBlockPosition, 0L, "", 4096L, false),
				new PropertyInfo(2L, NHealthBar.PropertyName._currentHpOnLastRefresh, 0L, "", 4096L, false),
				new PropertyInfo(2L, NHealthBar.PropertyName._maxHpOnLastRefresh, 0L, "", 4096L, false),
				new PropertyInfo(3L, NHealthBar.PropertyName._expectedMaxFgWidth, 0L, "", 4096L, false),
				new PropertyInfo(3L, NHealthBar.PropertyName.MaxFgWidth, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealthBar.PropertyName.HpBarContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004014 RID: 16404 RVA: 0x001ED16C File Offset: 0x001EB36C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hpBarContainer = NHealthBar.PropertyName.HpBarContainer;
			Control hpBarContainer2 = this.HpBarContainer;
			info.AddProperty(hpBarContainer, Variant.From<Control>(ref hpBarContainer2));
			info.AddProperty(NHealthBar.PropertyName._hpForegroundContainer, Variant.From<Control>(ref this._hpForegroundContainer));
			info.AddProperty(NHealthBar.PropertyName._hpForeground, Variant.From<Control>(ref this._hpForeground));
			info.AddProperty(NHealthBar.PropertyName._poisonForeground, Variant.From<Control>(ref this._poisonForeground));
			info.AddProperty(NHealthBar.PropertyName._doomForeground, Variant.From<Control>(ref this._doomForeground));
			info.AddProperty(NHealthBar.PropertyName._hpMiddleground, Variant.From<Control>(ref this._hpMiddleground));
			info.AddProperty(NHealthBar.PropertyName._hpLabel, Variant.From<MegaLabel>(ref this._hpLabel));
			info.AddProperty(NHealthBar.PropertyName._blockContainer, Variant.From<Control>(ref this._blockContainer));
			info.AddProperty(NHealthBar.PropertyName._blockLabel, Variant.From<MegaLabel>(ref this._blockLabel));
			info.AddProperty(NHealthBar.PropertyName._blockOutline, Variant.From<Control>(ref this._blockOutline));
			info.AddProperty(NHealthBar.PropertyName._infinityTex, Variant.From<TextureRect>(ref this._infinityTex));
			info.AddProperty(NHealthBar.PropertyName._blockTween, Variant.From<Tween>(ref this._blockTween));
			info.AddProperty(NHealthBar.PropertyName._hpLabelFadeTween, Variant.From<Tween>(ref this._hpLabelFadeTween));
			info.AddProperty(NHealthBar.PropertyName._middlegroundTween, Variant.From<Tween>(ref this._middlegroundTween));
			info.AddProperty(NHealthBar.PropertyName._originalBlockPosition, Variant.From<Vector2>(ref this._originalBlockPosition));
			info.AddProperty(NHealthBar.PropertyName._currentHpOnLastRefresh, Variant.From<int>(ref this._currentHpOnLastRefresh));
			info.AddProperty(NHealthBar.PropertyName._maxHpOnLastRefresh, Variant.From<int>(ref this._maxHpOnLastRefresh));
			info.AddProperty(NHealthBar.PropertyName._expectedMaxFgWidth, Variant.From<float>(ref this._expectedMaxFgWidth));
		}

		/// <inheritdoc />
		// Token: 0x06004015 RID: 16405 RVA: 0x001ED310 File Offset: 0x001EB510
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHealthBar.PropertyName.HpBarContainer, ref variant))
			{
				this.HpBarContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHealthBar.PropertyName._hpForegroundContainer, ref variant2))
			{
				this._hpForegroundContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHealthBar.PropertyName._hpForeground, ref variant3))
			{
				this._hpForeground = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHealthBar.PropertyName._poisonForeground, ref variant4))
			{
				this._poisonForeground = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NHealthBar.PropertyName._doomForeground, ref variant5))
			{
				this._doomForeground = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NHealthBar.PropertyName._hpMiddleground, ref variant6))
			{
				this._hpMiddleground = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NHealthBar.PropertyName._hpLabel, ref variant7))
			{
				this._hpLabel = variant7.As<MegaLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NHealthBar.PropertyName._blockContainer, ref variant8))
			{
				this._blockContainer = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NHealthBar.PropertyName._blockLabel, ref variant9))
			{
				this._blockLabel = variant9.As<MegaLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NHealthBar.PropertyName._blockOutline, ref variant10))
			{
				this._blockOutline = variant10.As<Control>();
			}
			Variant variant11;
			if (info.TryGetProperty(NHealthBar.PropertyName._infinityTex, ref variant11))
			{
				this._infinityTex = variant11.As<TextureRect>();
			}
			Variant variant12;
			if (info.TryGetProperty(NHealthBar.PropertyName._blockTween, ref variant12))
			{
				this._blockTween = variant12.As<Tween>();
			}
			Variant variant13;
			if (info.TryGetProperty(NHealthBar.PropertyName._hpLabelFadeTween, ref variant13))
			{
				this._hpLabelFadeTween = variant13.As<Tween>();
			}
			Variant variant14;
			if (info.TryGetProperty(NHealthBar.PropertyName._middlegroundTween, ref variant14))
			{
				this._middlegroundTween = variant14.As<Tween>();
			}
			Variant variant15;
			if (info.TryGetProperty(NHealthBar.PropertyName._originalBlockPosition, ref variant15))
			{
				this._originalBlockPosition = variant15.As<Vector2>();
			}
			Variant variant16;
			if (info.TryGetProperty(NHealthBar.PropertyName._currentHpOnLastRefresh, ref variant16))
			{
				this._currentHpOnLastRefresh = variant16.As<int>();
			}
			Variant variant17;
			if (info.TryGetProperty(NHealthBar.PropertyName._maxHpOnLastRefresh, ref variant17))
			{
				this._maxHpOnLastRefresh = variant17.As<int>();
			}
			Variant variant18;
			if (info.TryGetProperty(NHealthBar.PropertyName._expectedMaxFgWidth, ref variant18))
			{
				this._expectedMaxFgWidth = variant18.As<float>();
			}
		}

		// Token: 0x040018CA RID: 6346
		private Control _hpForegroundContainer;

		// Token: 0x040018CB RID: 6347
		private Control _hpForeground;

		// Token: 0x040018CC RID: 6348
		private Control _poisonForeground;

		// Token: 0x040018CD RID: 6349
		private Control _doomForeground;

		// Token: 0x040018CE RID: 6350
		private Control _hpMiddleground;

		// Token: 0x040018CF RID: 6351
		private MegaLabel _hpLabel;

		// Token: 0x040018D0 RID: 6352
		private Control _blockContainer;

		// Token: 0x040018D1 RID: 6353
		private MegaLabel _blockLabel;

		// Token: 0x040018D2 RID: 6354
		private Control _blockOutline;

		// Token: 0x040018D3 RID: 6355
		private Creature _creature;

		// Token: 0x040018D4 RID: 6356
		[Nullable(2)]
		private Creature _blockTrackingCreature;

		// Token: 0x040018D5 RID: 6357
		private readonly LocString _healthBarDead = new LocString("gameplay_ui", "HEALTH_BAR.DEAD");

		// Token: 0x040018D6 RID: 6358
		private TextureRect _infinityTex;

		// Token: 0x040018D7 RID: 6359
		[Nullable(2)]
		private Tween _blockTween;

		// Token: 0x040018D8 RID: 6360
		[Nullable(2)]
		private Tween _hpLabelFadeTween;

		// Token: 0x040018D9 RID: 6361
		[Nullable(2)]
		private Tween _middlegroundTween;

		// Token: 0x040018DA RID: 6362
		private Vector2 _originalBlockPosition;

		// Token: 0x040018DB RID: 6363
		private int _currentHpOnLastRefresh = -1;

		// Token: 0x040018DC RID: 6364
		private int _maxHpOnLastRefresh = -1;

		// Token: 0x040018DD RID: 6365
		private float _expectedMaxFgWidth = -1f;

		// Token: 0x040018DE RID: 6366
		private const float _minSize = 12f;

		// Token: 0x040018DF RID: 6367
		private static readonly Vector2 _blockAnimOffset = new Vector2(0f, 20f);

		// Token: 0x040018E0 RID: 6368
		private static readonly Color _defaultFontColor = StsColors.cream;

		// Token: 0x040018E1 RID: 6369
		private static readonly Color _defaultFontOutlineColor = new Color("900000");

		// Token: 0x040018E2 RID: 6370
		private static readonly Color _blockOutlineColor = new Color("1B3045");

		// Token: 0x040018E3 RID: 6371
		private static readonly Color _invincibleOutlineColor = new Color("4C4370");

		// Token: 0x040018E4 RID: 6372
		private static readonly Color _redForegroundColor = new Color("F1373E");

		// Token: 0x040018E5 RID: 6373
		private static readonly Color _blockHpForegroundColor = new Color("3B6FA3");

		// Token: 0x040018E6 RID: 6374
		private static readonly Color _invincibleForegroundColor = new Color("C5BBED");

		// Token: 0x040018E7 RID: 6375
		private const float _foregroundContainerInset = 10f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018EE RID: 6382
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040061BA RID: 25018
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'DebugToggleVisibility' method.
			/// </summary>
			// Token: 0x040061BB RID: 25019
			public static readonly StringName DebugToggleVisibility = "DebugToggleVisibility";

			/// <summary>
			/// Cached name for the 'UpdateLayoutForCreatureBounds' method.
			/// </summary>
			// Token: 0x040061BC RID: 25020
			public static readonly StringName UpdateLayoutForCreatureBounds = "UpdateLayoutForCreatureBounds";

			/// <summary>
			/// Cached name for the 'UpdateWidthRelativeToReferenceValue' method.
			/// </summary>
			// Token: 0x040061BD RID: 25021
			public static readonly StringName UpdateWidthRelativeToReferenceValue = "UpdateWidthRelativeToReferenceValue";

			/// <summary>
			/// Cached name for the 'SetHpBarContainerSizeWithOffsets' method.
			/// </summary>
			// Token: 0x040061BE RID: 25022
			public static readonly StringName SetHpBarContainerSizeWithOffsets = "SetHpBarContainerSizeWithOffsets";

			/// <summary>
			/// Cached name for the 'SetHpBarContainerSizeWithOffsetsImmediately' method.
			/// </summary>
			// Token: 0x040061BF RID: 25023
			public static readonly StringName SetHpBarContainerSizeWithOffsetsImmediately = "SetHpBarContainerSizeWithOffsetsImmediately";

			/// <summary>
			/// Cached name for the 'RefreshValues' method.
			/// </summary>
			// Token: 0x040061C0 RID: 25024
			public static readonly StringName RefreshValues = "RefreshValues";

			/// <summary>
			/// Cached name for the 'RefreshMiddleground' method.
			/// </summary>
			// Token: 0x040061C1 RID: 25025
			public static readonly StringName RefreshMiddleground = "RefreshMiddleground";

			/// <summary>
			/// Cached name for the 'RefreshForeground' method.
			/// </summary>
			// Token: 0x040061C2 RID: 25026
			public static readonly StringName RefreshForeground = "RefreshForeground";

			/// <summary>
			/// Cached name for the 'RefreshBlockUi' method.
			/// </summary>
			// Token: 0x040061C3 RID: 25027
			public static readonly StringName RefreshBlockUi = "RefreshBlockUi";

			/// <summary>
			/// Cached name for the 'RefreshText' method.
			/// </summary>
			// Token: 0x040061C4 RID: 25028
			public static readonly StringName RefreshText = "RefreshText";

			/// <summary>
			/// Cached name for the 'IsPoisonLethal' method.
			/// </summary>
			// Token: 0x040061C5 RID: 25029
			public static readonly StringName IsPoisonLethal = "IsPoisonLethal";

			/// <summary>
			/// Cached name for the 'IsDoomLethal' method.
			/// </summary>
			// Token: 0x040061C6 RID: 25030
			public static readonly StringName IsDoomLethal = "IsDoomLethal";

			/// <summary>
			/// Cached name for the 'GetFgWidth' method.
			/// </summary>
			// Token: 0x040061C7 RID: 25031
			public static readonly StringName GetFgWidth = "GetFgWidth";

			/// <summary>
			/// Cached name for the 'FadeOutHpLabel' method.
			/// </summary>
			// Token: 0x040061C8 RID: 25032
			public static readonly StringName FadeOutHpLabel = "FadeOutHpLabel";

			/// <summary>
			/// Cached name for the 'FadeInHpLabel' method.
			/// </summary>
			// Token: 0x040061C9 RID: 25033
			public static readonly StringName FadeInHpLabel = "FadeInHpLabel";

			/// <summary>
			/// Cached name for the 'AnimateInBlock' method.
			/// </summary>
			// Token: 0x040061CA RID: 25034
			public static readonly StringName AnimateInBlock = "AnimateInBlock";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018EF RID: 6383
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'MaxFgWidth' property.
			/// </summary>
			// Token: 0x040061CB RID: 25035
			public static readonly StringName MaxFgWidth = "MaxFgWidth";

			/// <summary>
			/// Cached name for the 'HpBarContainer' property.
			/// </summary>
			// Token: 0x040061CC RID: 25036
			public static readonly StringName HpBarContainer = "HpBarContainer";

			/// <summary>
			/// Cached name for the '_hpForegroundContainer' field.
			/// </summary>
			// Token: 0x040061CD RID: 25037
			public static readonly StringName _hpForegroundContainer = "_hpForegroundContainer";

			/// <summary>
			/// Cached name for the '_hpForeground' field.
			/// </summary>
			// Token: 0x040061CE RID: 25038
			public static readonly StringName _hpForeground = "_hpForeground";

			/// <summary>
			/// Cached name for the '_poisonForeground' field.
			/// </summary>
			// Token: 0x040061CF RID: 25039
			public static readonly StringName _poisonForeground = "_poisonForeground";

			/// <summary>
			/// Cached name for the '_doomForeground' field.
			/// </summary>
			// Token: 0x040061D0 RID: 25040
			public static readonly StringName _doomForeground = "_doomForeground";

			/// <summary>
			/// Cached name for the '_hpMiddleground' field.
			/// </summary>
			// Token: 0x040061D1 RID: 25041
			public static readonly StringName _hpMiddleground = "_hpMiddleground";

			/// <summary>
			/// Cached name for the '_hpLabel' field.
			/// </summary>
			// Token: 0x040061D2 RID: 25042
			public static readonly StringName _hpLabel = "_hpLabel";

			/// <summary>
			/// Cached name for the '_blockContainer' field.
			/// </summary>
			// Token: 0x040061D3 RID: 25043
			public static readonly StringName _blockContainer = "_blockContainer";

			/// <summary>
			/// Cached name for the '_blockLabel' field.
			/// </summary>
			// Token: 0x040061D4 RID: 25044
			public static readonly StringName _blockLabel = "_blockLabel";

			/// <summary>
			/// Cached name for the '_blockOutline' field.
			/// </summary>
			// Token: 0x040061D5 RID: 25045
			public static readonly StringName _blockOutline = "_blockOutline";

			/// <summary>
			/// Cached name for the '_infinityTex' field.
			/// </summary>
			// Token: 0x040061D6 RID: 25046
			public static readonly StringName _infinityTex = "_infinityTex";

			/// <summary>
			/// Cached name for the '_blockTween' field.
			/// </summary>
			// Token: 0x040061D7 RID: 25047
			public static readonly StringName _blockTween = "_blockTween";

			/// <summary>
			/// Cached name for the '_hpLabelFadeTween' field.
			/// </summary>
			// Token: 0x040061D8 RID: 25048
			public static readonly StringName _hpLabelFadeTween = "_hpLabelFadeTween";

			/// <summary>
			/// Cached name for the '_middlegroundTween' field.
			/// </summary>
			// Token: 0x040061D9 RID: 25049
			public static readonly StringName _middlegroundTween = "_middlegroundTween";

			/// <summary>
			/// Cached name for the '_originalBlockPosition' field.
			/// </summary>
			// Token: 0x040061DA RID: 25050
			public static readonly StringName _originalBlockPosition = "_originalBlockPosition";

			/// <summary>
			/// Cached name for the '_currentHpOnLastRefresh' field.
			/// </summary>
			// Token: 0x040061DB RID: 25051
			public static readonly StringName _currentHpOnLastRefresh = "_currentHpOnLastRefresh";

			/// <summary>
			/// Cached name for the '_maxHpOnLastRefresh' field.
			/// </summary>
			// Token: 0x040061DC RID: 25052
			public static readonly StringName _maxHpOnLastRefresh = "_maxHpOnLastRefresh";

			/// <summary>
			/// Cached name for the '_expectedMaxFgWidth' field.
			/// </summary>
			// Token: 0x040061DD RID: 25053
			public static readonly StringName _expectedMaxFgWidth = "_expectedMaxFgWidth";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018F0 RID: 6384
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
