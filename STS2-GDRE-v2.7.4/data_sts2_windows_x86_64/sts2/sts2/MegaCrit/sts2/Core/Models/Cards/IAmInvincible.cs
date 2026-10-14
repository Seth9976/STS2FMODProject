using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A09 RID: 2569
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IAmInvincible : CardModel
	{
		// Token: 0x06007333 RID: 29491 RVA: 0x0028EE97 File Offset: 0x0028D097
		public IAmInvincible()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001EA5 RID: 7845
		// (get) Token: 0x06007334 RID: 29492 RVA: 0x0028EEA4 File Offset: 0x0028D0A4
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EA6 RID: 7846
		// (get) Token: 0x06007335 RID: 29493 RVA: 0x0028EEA7 File Offset: 0x0028D0A7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x06007336 RID: 29494 RVA: 0x0028EEBC File Offset: 0x0028D0BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06007337 RID: 29495 RVA: 0x0028EF08 File Offset: 0x0028D108
		public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				CardPile pile = PileType.Draw.GetPile(base.Owner);
				if (pile.Cards.FirstOrDefault<CardModel>() == this)
				{
					await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner, 1, CardPilePosition.Top, false);
				}
			}
		}

		// Token: 0x06007338 RID: 29496 RVA: 0x0028EF5B File Offset: 0x0028D15B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
