using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Node script for DrawPile, DiscardPile, and ExhaustPile.
	/// Don't extend to more piles! Make bespoke scripts.
	/// </summary>
	// Token: 0x020003F3 RID: 1011
	[ScriptPath("res://src/Core/Nodes/Combat/NCombatCardPile.cs")]
	public abstract class NCombatCardPile : NButton
	{
		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003E18 RID: 15896
		protected abstract PileType Pile { get; }

		// Token: 0x06003E19 RID: 15897 RVA: 0x001DBCFC File Offset: 0x001D9EFC
		public override void _Ready()
		{
			if (base.GetType() != typeof(NCombatCardPile))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x001DBD54 File Offset: 0x001D9F54
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._icon = base.GetNode<Control>("Icon");
			this._countLabel = base.GetNode<MegaLabel>("CountContainer/Count");
			this.SetAnimInOutPositions();
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x001DBD90 File Offset: 0x001D9F90
		public override void _EnterTree()
		{
			base._EnterTree();
			if (this._pile != null)
			{
				this._pile.CardAddFinished -= this.AddCard;
				this._pile.CardAddFinished += this.AddCard;
				this._pile.CardRemoveFinished -= this.RemoveCard;
				this._pile.CardRemoveFinished += this.RemoveCard;
			}
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x001DBE0C File Offset: 0x001DA00C
		public override void _ExitTree()
		{
			base._ExitTree();
			if (this._pile != null)
			{
				this._pile.CardAddFinished -= this.AddCard;
				this._pile.CardRemoveFinished -= this.RemoveCard;
			}
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			Tween bumpTween = this._bumpTween;
			if (bumpTween == null)
			{
				return;
			}
			bumpTween.Kill();
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x001DBE78 File Offset: 0x001DA078
		[NullableContext(1)]
		public virtual void Initialize(Player player)
		{
			this._localPlayer = player;
			this._pile = this.Pile.GetPile(this._localPlayer);
			this._pile.CardAddFinished += this.AddCard;
			this._pile.CardRemoveFinished += this.RemoveCard;
			this._currentCount = this._pile.Cards.Count;
			this._countLabel.SetTextAutoSize(this._currentCount.ToString());
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x001DBEFE File Offset: 0x001DA0FE
		protected virtual void SetAnimInOutPositions()
		{
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x001DBF00 File Offset: 0x001DA100
		protected override void OnRelease()
		{
			Tween bumpTween = this._bumpTween;
			if (bumpTween != null)
			{
				bumpTween.Kill();
			}
			this._bumpTween = base.CreateTween();
			this._bumpTween.TweenProperty(this._icon, "scale", base.IsFocused ? NCombatCardPile._hoverScale : Vector2.One, 0.05);
			this._bumpTween.TweenProperty(this._icon, "modulate", Colors.White, 0.5).SetEase(1L).SetTrans(5L);
			if (this._pile == null || this._localPlayer == null)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			if (NTargetManager.Instance.IsInSelection)
			{
				NTargetManager.Instance.CancelTargeting();
			}
			if (this._pile.IsEmpty)
			{
				NCapstoneContainer instance = NCapstoneContainer.Instance;
				if (instance != null && instance.InUse)
				{
					NCapstoneContainer.Instance.Close();
				}
				NThoughtBubbleVfx nthoughtBubbleVfx = NThoughtBubbleVfx.Create(this._emptyPileMessage.GetFormattedText(), this._localPlayer.Creature, new double?((double)2f));
				NCombatRoom instance2 = NCombatRoom.Instance;
				if (instance2 == null)
				{
					return;
				}
				instance2.CombatVfxContainer.AddChildSafely(nthoughtBubbleVfx);
				return;
			}
			else
			{
				NCapstoneContainer instance3 = NCapstoneContainer.Instance;
				NCardPileScreen ncardPileScreen = ((instance3 != null) ? instance3.CurrentCapstoneScreen : null) as NCardPileScreen;
				if (ncardPileScreen != null && ncardPileScreen.Pile == this._pile)
				{
					NCapstoneContainer.Instance.Close();
					return;
				}
				NCardPileScreen.ShowScreen(this._pile, this.Hotkeys);
				return;
			}
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x001DC088 File Offset: 0x001DA288
		protected override void OnFocus()
		{
			if (this._pile == null)
			{
				return;
			}
			LocString locString;
			LocString locString2;
			switch (this._pile.Type)
			{
			case PileType.Draw:
				locString = new LocString("static_hover_tips", "DRAW_PILE.title");
				locString2 = new LocString("static_hover_tips", "DRAW_PILE.description");
				goto IL_009D;
			case PileType.Discard:
				locString = new LocString("static_hover_tips", "DISCARD_PILE.title");
				locString2 = new LocString("static_hover_tips", "DISCARD_PILE.description");
				goto IL_009D;
			case PileType.Exhaust:
				locString = new LocString("static_hover_tips", "EXHAUST_PILE.title");
				locString2 = new LocString("static_hover_tips", "EXHAUST_PILE.description");
				goto IL_009D;
			}
			throw new ArgumentOutOfRangeException();
			IL_009D:
			string text = this.Hotkeys.FirstOrDefault<string>();
			Key? key = null;
			if (text != null)
			{
				key = new Key?(NInputManager.Instance.GetShortcutKey(text));
			}
			if (key != null)
			{
				locString.Add("Hotkey", key.Value.ToString());
			}
			else
			{
				locString.Add("Hotkey", "None");
			}
			HoverTip hoverTip = new HoverTip(locString, locString2, null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				NHoverTipSet nhoverTipSet2 = nhoverTipSet;
				Vector2 vector;
				switch (this._pile.Type)
				{
				case PileType.Draw:
					vector = base.GlobalPosition + new Vector2(14f, -375f);
					goto IL_01B7;
				case PileType.Discard:
					vector = base.GlobalPosition + new Vector2(-320f, -370f);
					goto IL_01B7;
				case PileType.Exhaust:
					vector = base.GlobalPosition + new Vector2(-320f, -125f);
					goto IL_01B7;
				}
				vector = nhoverTipSet.GlobalPosition;
				IL_01B7:
				nhoverTipSet2.GlobalPosition = vector;
			}
			Tween bumpTween = this._bumpTween;
			if (bumpTween != null)
			{
				bumpTween.Kill();
			}
			this._bumpTween = base.CreateTween();
			this._bumpTween.TweenProperty(this._icon, "scale", NCombatCardPile._hoverScale, 0.05);
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x001DC2A4 File Offset: 0x001DA4A4
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
			Tween bumpTween = this._bumpTween;
			if (bumpTween != null)
			{
				bumpTween.Kill();
			}
			this._bumpTween = base.CreateTween().SetParallel(true);
			this._bumpTween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._bumpTween.TweenProperty(this._icon, "modulate", Colors.White, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x001DC354 File Offset: 0x001DA554
		protected override void OnPress()
		{
			Tween bumpTween = this._bumpTween;
			if (bumpTween != null)
			{
				bumpTween.Kill();
			}
			this._bumpTween = base.CreateTween().SetParallel(true);
			this._bumpTween.TweenProperty(this._icon, "scale", Vector2.One, 0.25).SetEase(1L).SetTrans(7L);
			this._bumpTween.TweenProperty(this._icon, "modulate", NCombatCardPile._downColor, 0.25).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x001DC400 File Offset: 0x001DA600
		protected virtual void AddCard()
		{
			if (this._pile == null)
			{
				return;
			}
			this._currentCount = Math.Min(this._currentCount + 1, this._pile.Cards.Count);
			this._countLabel.SetTextAutoSize(this._currentCount.ToString());
			this._countLabel.PivotOffset = this._countLabel.Size * 0.5f;
			Tween bumpTween = this._bumpTween;
			if (bumpTween != null)
			{
				bumpTween.Kill();
			}
			this._bumpTween = base.CreateTween().SetParallel(true);
			this._icon.Scale = NCombatCardPile._hoverScale;
			this._bumpTween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._countLabel.Scale = NCombatCardPile._hoverScale;
			this._bumpTween.TweenProperty(this._countLabel, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x001DC52C File Offset: 0x001DA72C
		private void RemoveCard()
		{
			if (this._pile == null)
			{
				return;
			}
			this._currentCount = Math.Max(this._currentCount - 1, this._pile.Cards.Count);
			this._countLabel.SetTextAutoSize(this._currentCount.ToString());
			this._countLabel.PivotOffset = this._countLabel.Size * 0.5f;
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x001DC59C File Offset: 0x001DA79C
		public virtual void AnimIn()
		{
			base.Position = this._hidePosition;
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this, "position", this._showPosition, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x001DC60C File Offset: 0x001DA80C
		public void AnimOut()
		{
			base.Position = this._showPosition;
			Tween positionTween = this._positionTween;
			if (positionTween != null)
			{
				positionTween.Kill();
			}
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this, "position", this._hidePosition, 0.5).SetEase(0L).SetTrans(10L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E27 RID: 15911 RVA: 0x001DC67C File Offset: 0x001DA87C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NCombatCardPile.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.SetAnimInOutPositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.AddCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.RemoveCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatCardPile.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E28 RID: 15912 RVA: 0x001DC8F8 File Offset: 0x001DAAF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatCardPile.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.SetAnimInOutPositions && args.Count == 0)
			{
				this.SetAnimInOutPositions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.AddCard && args.Count == 0)
			{
				this.AddCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.RemoveCard && args.Count == 0)
			{
				this.RemoveCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatCardPile.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003E29 RID: 15913 RVA: 0x001DCAF0 File Offset: 0x001DACF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatCardPile.MethodName._Ready || (ref method) == NCombatCardPile.MethodName.ConnectSignals || (ref method) == NCombatCardPile.MethodName._EnterTree || (ref method) == NCombatCardPile.MethodName._ExitTree || (ref method) == NCombatCardPile.MethodName.SetAnimInOutPositions || (ref method) == NCombatCardPile.MethodName.OnRelease || (ref method) == NCombatCardPile.MethodName.OnFocus || (ref method) == NCombatCardPile.MethodName.OnUnfocus || (ref method) == NCombatCardPile.MethodName.OnPress || (ref method) == NCombatCardPile.MethodName.AddCard || (ref method) == NCombatCardPile.MethodName.RemoveCard || (ref method) == NCombatCardPile.MethodName.AnimIn || (ref method) == NCombatCardPile.MethodName.AnimOut || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003E2A RID: 15914 RVA: 0x001DCBC8 File Offset: 0x001DADC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatCardPile.PropertyName._countLabel)
			{
				this._countLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._bumpTween)
			{
				this._bumpTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._currentCount)
			{
				this._currentCount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._positionTween)
			{
				this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._showPosition)
			{
				this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._hidePosition)
			{
				this._hidePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003E2B RID: 15915 RVA: 0x001DCC9C File Offset: 0x001DAE9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatCardPile.PropertyName.Pile)
			{
				PileType pile = this.Pile;
				value = VariantUtils.CreateFrom<PileType>(ref pile);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._countLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._countLabel);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._icon);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._bumpTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._bumpTween);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._currentCount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentCount);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._positionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._showPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
				return true;
			}
			if ((ref name) == NCombatCardPile.PropertyName._hidePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._hidePosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E2C RID: 15916 RVA: 0x001DCDB4 File Offset: 0x001DAFB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NCombatCardPile.PropertyName.Pile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatCardPile.PropertyName._countLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatCardPile.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatCardPile.PropertyName._bumpTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCombatCardPile.PropertyName._currentCount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatCardPile.PropertyName._positionTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCombatCardPile.PropertyName._showPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCombatCardPile.PropertyName._hidePosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E2D RID: 15917 RVA: 0x001DCECC File Offset: 0x001DB0CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatCardPile.PropertyName._countLabel, Variant.From<MegaLabel>(ref this._countLabel));
			info.AddProperty(NCombatCardPile.PropertyName._icon, Variant.From<Control>(ref this._icon));
			info.AddProperty(NCombatCardPile.PropertyName._bumpTween, Variant.From<Tween>(ref this._bumpTween));
			info.AddProperty(NCombatCardPile.PropertyName._currentCount, Variant.From<int>(ref this._currentCount));
			info.AddProperty(NCombatCardPile.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
			info.AddProperty(NCombatCardPile.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
			info.AddProperty(NCombatCardPile.PropertyName._hidePosition, Variant.From<Vector2>(ref this._hidePosition));
		}

		/// <inheritdoc />
		// Token: 0x06003E2E RID: 15918 RVA: 0x001DCF7C File Offset: 0x001DB17C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatCardPile.PropertyName._countLabel, ref variant))
			{
				this._countLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatCardPile.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatCardPile.PropertyName._bumpTween, ref variant3))
			{
				this._bumpTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCombatCardPile.PropertyName._currentCount, ref variant4))
			{
				this._currentCount = variant4.As<int>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCombatCardPile.PropertyName._positionTween, ref variant5))
			{
				this._positionTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCombatCardPile.PropertyName._showPosition, ref variant6))
			{
				this._showPosition = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCombatCardPile.PropertyName._hidePosition, ref variant7))
			{
				this._hidePosition = variant7.As<Vector2>();
			}
		}

		// Token: 0x04001823 RID: 6179
		[Nullable(2)]
		private CardPile _pile;

		// Token: 0x04001824 RID: 6180
		[Nullable(2)]
		private Player _localPlayer;

		// Token: 0x04001825 RID: 6181
		[Nullable(1)]
		private MegaLabel _countLabel;

		// Token: 0x04001826 RID: 6182
		[Nullable(1)]
		private Control _icon;

		// Token: 0x04001827 RID: 6183
		[Nullable(1)]
		protected LocString _emptyPileMessage;

		// Token: 0x04001828 RID: 6184
		[Nullable(2)]
		private Tween _bumpTween;

		// Token: 0x04001829 RID: 6185
		private int _currentCount;

		// Token: 0x0400182A RID: 6186
		private static readonly Vector2 _hoverScale = Vector2.One * 1.25f;

		// Token: 0x0400182B RID: 6187
		private const double _unhoverAnimDur = 0.5;

		// Token: 0x0400182C RID: 6188
		private const double _pressDownDur = 0.25;

		// Token: 0x0400182D RID: 6189
		private static readonly Color _downColor = Colors.DarkGray;

		// Token: 0x0400182E RID: 6190
		[Nullable(2)]
		private Tween _positionTween;

		// Token: 0x0400182F RID: 6191
		private const double _animDuration = 0.5;

		// Token: 0x04001830 RID: 6192
		protected Vector2 _showPosition = new Vector2(100f, 828f);

		// Token: 0x04001831 RID: 6193
		protected Vector2 _hidePosition = new Vector2(-160f, 860f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018A8 RID: 6312
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006057 RID: 24663
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04006058 RID: 24664
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04006059 RID: 24665
			public new static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400605A RID: 24666
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SetAnimInOutPositions' method.
			/// </summary>
			// Token: 0x0400605B RID: 24667
			public static readonly StringName SetAnimInOutPositions = "SetAnimInOutPositions";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400605C RID: 24668
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400605D RID: 24669
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400605E RID: 24670
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400605F RID: 24671
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'AddCard' method.
			/// </summary>
			// Token: 0x04006060 RID: 24672
			public static readonly StringName AddCard = "AddCard";

			/// <summary>
			/// Cached name for the 'RemoveCard' method.
			/// </summary>
			// Token: 0x04006061 RID: 24673
			public static readonly StringName RemoveCard = "RemoveCard";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x04006062 RID: 24674
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x04006063 RID: 24675
			public static readonly StringName AnimOut = "AnimOut";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018A9 RID: 6313
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Pile' property.
			/// </summary>
			// Token: 0x04006064 RID: 24676
			public static readonly StringName Pile = "Pile";

			/// <summary>
			/// Cached name for the '_countLabel' field.
			/// </summary>
			// Token: 0x04006065 RID: 24677
			public static readonly StringName _countLabel = "_countLabel";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04006066 RID: 24678
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_bumpTween' field.
			/// </summary>
			// Token: 0x04006067 RID: 24679
			public static readonly StringName _bumpTween = "_bumpTween";

			/// <summary>
			/// Cached name for the '_currentCount' field.
			/// </summary>
			// Token: 0x04006068 RID: 24680
			public static readonly StringName _currentCount = "_currentCount";

			/// <summary>
			/// Cached name for the '_positionTween' field.
			/// </summary>
			// Token: 0x04006069 RID: 24681
			public static readonly StringName _positionTween = "_positionTween";

			/// <summary>
			/// Cached name for the '_showPosition' field.
			/// </summary>
			// Token: 0x0400606A RID: 24682
			public static readonly StringName _showPosition = "_showPosition";

			/// <summary>
			/// Cached name for the '_hidePosition' field.
			/// </summary>
			// Token: 0x0400606B RID: 24683
			public static readonly StringName _hidePosition = "_hidePosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018AA RID: 6314
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
