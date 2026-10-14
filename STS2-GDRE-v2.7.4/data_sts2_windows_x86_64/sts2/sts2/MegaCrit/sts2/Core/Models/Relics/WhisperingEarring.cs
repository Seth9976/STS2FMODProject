using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000615 RID: 1557
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WhisperingEarring : RelicModel
	{
		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x0600562D RID: 22061 RVA: 0x0024A0A0 File Offset: 0x002482A0
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x0600562E RID: 22062 RVA: 0x0024A0A3 File Offset: 0x002482A3
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x0600562F RID: 22063 RVA: 0x0024A0B0 File Offset: 0x002482B0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x0024A0BD File Offset: 0x002482BD
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.BaseValue;
		}

		/// <remarks>
		/// We do this in Late because we want this to trigger after all other triggers fire (i.e. <see cref="T:MegaCrit.Sts2.Core.Models.Enchantments.Imbued" />).
		/// </remarks>
		// Token: 0x06005631 RID: 22065 RVA: 0x0024A0E0 File Offset: 0x002482E0
		public override async Task AfterAutoPrePlayPhaseEnteredLate(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				ICombatState combatState = player.Creature.CombatState;
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					bool flag;
					using (CardSelectCmd.PushSelector(new VakuuCardSelector(), false))
					{
						int cardsPlayed = 0;
						int startTurn = base.Owner.PlayerCombatState.TurnNumber;
						while (cardsPlayed < 13 && !CombatManager.Instance.IsOverOrEnding && !CombatManager.Instance.IsPlayerReadyToEndTurn(player) && base.Owner.PlayerCombatState.TurnNumber == startTurn)
						{
							CardPile pile = PileType.Hand.GetPile(base.Owner);
							CardModel card = pile.Cards.FirstOrDefault<CardModel>((CardModel c) => c.CanPlay());
							if (card == null)
							{
								break;
							}
							Creature target = this.GetTarget(card, combatState);
							await card.SpendResources();
							await CardCmd.AutoPlay(choiceContext, card, target, AutoPlayType.Default, true, false);
							cardsPlayed++;
							card = null;
							target = null;
						}
						flag = cardsPlayed >= 13;
						if (cardsPlayed == 0)
						{
							return;
						}
					}
					IDisposable disposable = null;
					LocString locString = (flag ? new LocString("relics", "WHISPERING_EARRING.warning") : new LocString("relics", "WHISPERING_EARRING.approval"));
					TalkCmd.Play(locString, base.Owner.Creature, VfxColor.Purple, VfxDuration.Custom);
				}
			}
		}

		/// <summary>
		/// Gets the target for a card during Vakuu's auto-play.
		/// Enemies: leftmost first. Allies: random.
		/// </summary>
		// Token: 0x06005632 RID: 22066 RVA: 0x0024A134 File Offset: 0x00248334
		[return: Nullable(2)]
		private Creature GetTarget(CardModel card, ICombatState combatState)
		{
			Rng combatTargets = base.Owner.RunState.Rng.CombatTargets;
			switch (card.TargetType)
			{
			case TargetType.AnyEnemy:
				return combatState.HittableEnemies.FirstOrDefault<Creature>();
			case TargetType.AnyPlayer:
				return base.Owner.Creature;
			case TargetType.AnyAlly:
				return combatTargets.NextItem<Creature>(combatState.Allies.Where<Creature>((Creature c) => c != null && c.IsAlive && c.IsPlayer && c != base.Owner.Creature));
			}
			return null;
		}

		// Token: 0x040024E4 RID: 9444
		public const int maxCardsToPlay = 13;
	}
}
