using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000684 RID: 1668
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GalvanicPower : PowerModel
	{
		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x0600588C RID: 22668 RVA: 0x0024ECE7 File Offset: 0x0024CEE7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x0600588D RID: 22669 RVA: 0x0024ECEA File Offset: 0x0024CEEA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x0600588E RID: 22670 RVA: 0x0024ECED File Offset: 0x0024CEED
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("AfflictionTitle", ModelDb.Affliction<Galvanized>().Title.GetFormattedText()));
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x0600588F RID: 22671 RVA: 0x0024ED0D File Offset: 0x0024CF0D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromAffliction<Galvanized>(base.Amount);
			}
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x0024ED1C File Offset: 0x0024CF1C
		public override async Task BeforeCombatStart()
		{
			foreach (Creature creature in base.Owner.CombatState.Allies.ToList<Creature>())
			{
				if (creature.IsPlayer)
				{
					IEnumerable<CardModel> enumerable = creature.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Type == CardType.Power);
					foreach (CardModel cardModel in enumerable)
					{
						await CardCmd.Afflict<Galvanized>(cardModel, base.Amount);
					}
					IEnumerator<CardModel> enumerator2 = null;
				}
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x0024ED60 File Offset: 0x0024CF60
		public override async Task AfterCardEnteredCombat(CardModel card)
		{
			if (card.Affliction == null)
			{
				if (card.Type == CardType.Power)
				{
					await CardCmd.Afflict<Galvanized>(card, base.Amount);
				}
			}
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x0024EDAC File Offset: 0x0024CFAC
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Affliction is Galvanized)
			{
				VfxCmd.PlayOnCreature(cardPlay.Card.Owner.Creature, "vfx/vfx_attack_lightning");
				await CreatureCmd.Damage(choiceContext, cardPlay.Card.Owner.Creature, base.Amount, ValueProp.Unpowered | ValueProp.Move, null, null);
			}
		}
	}
}
