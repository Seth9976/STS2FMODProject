using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AAE RID: 2734
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scourge : CardModel
	{
		// Token: 0x060076AA RID: 30378 RVA: 0x0029627A File Offset: 0x0029447A
		public Scourge()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002013 RID: 8211
		// (get) Token: 0x060076AB RID: 30379 RVA: 0x00296287 File Offset: 0x00294487
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<DoomPower>(13m),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x17002014 RID: 8212
		// (get) Token: 0x060076AC RID: 30380 RVA: 0x002962AC File Offset: 0x002944AC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x060076AD RID: 30381 RVA: 0x002962CC File Offset: 0x002944CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, base.DynamicVars.Doom.BaseValue, base.Owner.Creature, this, false);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x060076AE RID: 30382 RVA: 0x0029631F File Offset: 0x0029451F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Doom.UpgradeValueBy(3m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
