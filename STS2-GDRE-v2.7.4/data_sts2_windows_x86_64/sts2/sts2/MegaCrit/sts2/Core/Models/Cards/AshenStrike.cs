using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000905 RID: 2309
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AshenStrike : CardModel
	{
		// Token: 0x06006DC9 RID: 28105 RVA: 0x00283F9F File Offset: 0x0028219F
		public AshenStrike()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C4E RID: 7246
		// (get) Token: 0x06006DCA RID: 28106 RVA: 0x00283FAC File Offset: 0x002821AC
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001C4F RID: 7247
		// (get) Token: 0x06006DCB RID: 28107 RVA: 0x00283FBC File Offset: 0x002821BC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(6m);
				array[1] = new ExtraDamageVar(3m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => PileType.Exhaust.GetPile(card.Owner).Cards.Count);
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x17001C50 RID: 7248
		// (get) Token: 0x06006DCC RID: 28108 RVA: 0x0028401D File Offset: 0x0028221D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06006DCD RID: 28109 RVA: 0x0028402C File Offset: 0x0028222C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06006DCE RID: 28110 RVA: 0x0028407F File Offset: 0x0028227F
		protected override void OnUpgrade()
		{
			base.DynamicVars.ExtraDamage.UpgradeValueBy(1m);
		}
	}
}
