using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x020009B9 RID: 2489
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fasten : CardModel
	{
		// Token: 0x0600717D RID: 29053 RVA: 0x0028B583 File Offset: 0x00289783
		public Fasten()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DEB RID: 7659
		// (get) Token: 0x0600717E RID: 29054 RVA: 0x0028B590 File Offset: 0x00289790
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				CardModel cardModel = null;
				if (base.IsMutable && base.Owner != null)
				{
					cardModel = base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint).First<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Defend));
				}
				if (cardModel == null)
				{
					cardModel = ModelDb.Card<DefendIronclad>();
				}
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromCard(cardModel, false)
				});
			}
		}

		// Token: 0x17001DEC RID: 7660
		// (get) Token: 0x0600717F RID: 29055 RVA: 0x0028B630 File Offset: 0x00289830
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("ExtraBlock", 4m));
			}
		}

		// Token: 0x06007180 RID: 29056 RVA: 0x0028B648 File Offset: 0x00289848
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<FastenPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ExtraBlock"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007181 RID: 29057 RVA: 0x0028B693 File Offset: 0x00289893
		protected override void OnUpgrade()
		{
			base.DynamicVars["ExtraBlock"].UpgradeValueBy(2m);
		}

		// Token: 0x0400286B RID: 10347
		private const string _extraBlockKey = "ExtraBlock";
	}
}
