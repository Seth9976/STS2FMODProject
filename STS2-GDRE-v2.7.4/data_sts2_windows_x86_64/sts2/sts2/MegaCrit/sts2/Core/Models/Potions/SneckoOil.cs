using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000779 RID: 1913
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SneckoOil : PotionModel
	{
		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x06005E87 RID: 24199 RVA: 0x0025935F File Offset: 0x0025755F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x06005E88 RID: 24200 RVA: 0x00259362 File Offset: 0x00257562
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x06005E89 RID: 24201 RVA: 0x00259365 File Offset: 0x00257565
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x06005E8A RID: 24202 RVA: 0x00259368 File Offset: 0x00257568
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(7));
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x06005E8B RID: 24203 RVA: 0x00259375 File Offset: 0x00257575
		// (set) Token: 0x06005E8C RID: 24204 RVA: 0x0025937D File Offset: 0x0025757D
		public int TestEnergyCostOverride
		{
			get
			{
				return this._testEnergyCostOverride;
			}
			set
			{
				TestMode.AssertOn();
				base.AssertMutable();
				this._testEnergyCostOverride = value;
			}
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x00259394 File Offset: 0x00257594
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("6ec46f"));
			}
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, target.Player, false);
			IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(target.Player).Cards.Where<CardModel>((CardModel c) => !c.EnergyCost.CostsX);
			foreach (CardModel cardModel in enumerable)
			{
				if (cardModel.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
				{
					cardModel.EnergyCost.SetThisTurnOrUntilPlayed(this.NextEnergyCost(), false);
					NCard ncard = NCard.FindOnTable(cardModel, null);
					if (ncard != null)
					{
						ncard.PlayRandomizeCostAnim();
					}
				}
			}
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x002593E7 File Offset: 0x002575E7
		private int NextEnergyCost()
		{
			if (this.TestEnergyCostOverride >= 0)
			{
				return this.TestEnergyCostOverride;
			}
			return base.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
		}

		// Token: 0x04002534 RID: 9524
		private int _testEnergyCostOverride = -1;
	}
}
