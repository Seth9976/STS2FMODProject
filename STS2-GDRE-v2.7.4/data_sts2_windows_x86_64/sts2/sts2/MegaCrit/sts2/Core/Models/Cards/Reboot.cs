using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A97 RID: 2711
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Reboot : CardModel
	{
		// Token: 0x06007628 RID: 30248 RVA: 0x0029524F File Offset: 0x0029344F
		public Reboot()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001FDA RID: 8154
		// (get) Token: 0x06007629 RID: 30249 RVA: 0x0029525C File Offset: 0x0029345C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(4));
			}
		}

		// Token: 0x17001FDB RID: 8155
		// (get) Token: 0x0600762A RID: 30250 RVA: 0x00295269 File Offset: 0x00293469
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x0600762B RID: 30251 RVA: 0x00295274 File Offset: 0x00293474
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			foreach (CardModel cardModel in PileType.Hand.GetPile(base.Owner).Cards.ToList<CardModel>())
			{
				await CardPileCmd.Add(cardModel, PileType.Draw, CardPilePosition.Bottom, null, false);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			await CardPileCmd.Shuffle(choiceContext, base.Owner);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x0600762C RID: 30252 RVA: 0x002952BF File Offset: 0x002934BF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(2m);
		}
	}
}
