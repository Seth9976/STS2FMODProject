using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A4 RID: 2468
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Eidolon : CardModel
	{
		// Token: 0x06007108 RID: 28936 RVA: 0x0028A797 File Offset: 0x00288997
		public Eidolon()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001DB5 RID: 7605
		// (get) Token: 0x06007109 RID: 28937 RVA: 0x0028A7A4 File Offset: 0x002889A4
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001DB6 RID: 7606
		// (get) Token: 0x0600710A RID: 28938 RVA: 0x0028A7AC File Offset: 0x002889AC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x0600710B RID: 28939 RVA: 0x0028A7BC File Offset: 0x002889BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			List<CardModel> list = base.Owner.PlayerCombatState.ExhaustPile.Cards.Where<CardModel>((CardModel c) => c.Keywords.Contains(CardKeyword.Ethereal) && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				await CardCmd.AutoPlay(choiceContext, cardModel, null, AutoPlayType.Default, false, false);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
		}

		// Token: 0x0600710C RID: 28940 RVA: 0x0028A807 File Offset: 0x00288A07
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
