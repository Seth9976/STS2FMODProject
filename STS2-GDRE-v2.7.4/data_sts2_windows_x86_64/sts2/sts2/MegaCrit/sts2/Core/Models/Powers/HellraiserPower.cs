using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000693 RID: 1683
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HellraiserPower : PowerModel
	{
		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x060058E0 RID: 22752 RVA: 0x0024F657 File Offset: 0x0024D857
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x060058E1 RID: 22753 RVA: 0x0024F65A File Offset: 0x0024D85A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x0024F65D File Offset: 0x0024D85D
		protected override object InitInternalData()
		{
			return new HellraiserPower.Data();
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x0024F664 File Offset: 0x0024D864
		public override async Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner.Creature == base.Owner)
			{
				if (card.Tags.Contains(CardTag.Strike))
				{
					HellraiserPower.Data data = base.GetInternalData<HellraiserPower.Data>();
					bool flag = true;
					if (base.Owner.CombatState.HittableEnemies.All<Creature>((Creature c) => c.HpDisplay.IsInfinite()))
					{
						if (data.infiniteAutoPlaysThisTurn >= 9)
						{
							flag = false;
							if (!data.showedCapReachedMessage)
							{
								ThinkCmd.Play(new LocString("powers", "HELLRAISER_POWER.infiniteAutoPlayCapReached"), base.Owner, -1.0);
								data.showedCapReachedMessage = true;
							}
						}
						data.infiniteAutoPlaysThisTurn++;
					}
					else
					{
						this.ResetInfiniteAutoPlayData();
					}
					if (flag)
					{
						data.autoPlayingCards.Add(card);
						await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.Default, false, false);
						data.autoPlayingCards.Remove(card);
					}
				}
			}
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x0024F6B7 File Offset: 0x0024D8B7
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			this.ResetInfiniteAutoPlayData();
			return Task.CompletedTask;
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x0024F6D8 File Offset: 0x0024D8D8
		public override Task BeforeAttack(AttackCommand command)
		{
			if (!base.GetInternalData<HellraiserPower.Data>().autoPlayingCards.Contains(command.ModelSource))
			{
				return Task.CompletedTask;
			}
			command.WithHitFx("vfx/hellraiser_attack_vfx", command.HitSfx, command.TmpHitSfx).WithAttackerAnim("Cast", command.Attacker.Player.Character.CastAnimDelay, null).SpawningHitVfxOnEachCreature()
				.WithHitVfxSpawnedAtBase();
			return Task.CompletedTask;
		}

		// Token: 0x060058E6 RID: 22758 RVA: 0x0024F74C File Offset: 0x0024D94C
		private void ResetInfiniteAutoPlayData()
		{
			HellraiserPower.Data internalData = base.GetInternalData<HellraiserPower.Data>();
			internalData.infiniteAutoPlaysThisTurn = 0;
			internalData.showedCapReachedMessage = false;
		}

		// Token: 0x040024FD RID: 9469
		private const int _infiniteAutoPlayCap = 9;

		// Token: 0x02001BE5 RID: 7141
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006F3D RID: 28477
			[Nullable(1)]
			public readonly HashSet<CardModel> autoPlayingCards = new HashSet<CardModel>();

			// Token: 0x04006F3E RID: 28478
			public int infiniteAutoPlaysThisTurn;

			// Token: 0x04006F3F RID: 28479
			public bool showedCapReachedMessage;
		}
	}
}
