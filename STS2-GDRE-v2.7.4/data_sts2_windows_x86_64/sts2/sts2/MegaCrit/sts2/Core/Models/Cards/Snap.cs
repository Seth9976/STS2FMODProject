using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AD2 RID: 2770
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Snap : CardModel
	{
		// Token: 0x0600776A RID: 30570 RVA: 0x002979DF File Offset: 0x00295BDF
		public Snap()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002065 RID: 8293
		// (get) Token: 0x0600776B RID: 30571 RVA: 0x002979EC File Offset: 0x00295BEC
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17002066 RID: 8294
		// (get) Token: 0x0600776C RID: 30572 RVA: 0x002979F9 File Offset: 0x00295BF9
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17002067 RID: 8295
		// (get) Token: 0x0600776D RID: 30573 RVA: 0x00297A08 File Offset: 0x00295C08
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
			}
		}

		// Token: 0x17002068 RID: 8296
		// (get) Token: 0x0600776E RID: 30574 RVA: 0x00297A15 File Offset: 0x00295C15
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new OstyDamageVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x0600776F RID: 30575 RVA: 0x00297A28 File Offset: 0x00295C28
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, (CardModel c) => !c.Keywords.Contains(CardKeyword.Retain), this);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.ApplyKeyword(cardModel, new CardKeyword[] { CardKeyword.Retain });
			}
		}

		// Token: 0x06007770 RID: 30576 RVA: 0x00297A7B File Offset: 0x00295C7B
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(3m);
		}
	}
}
