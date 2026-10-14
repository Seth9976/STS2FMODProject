using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200053B RID: 1339
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmotionChip : RelicModel
	{
		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005072 RID: 20594 RVA: 0x0023F24E File Offset: 0x0023D44E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005073 RID: 20595 RVA: 0x0023F251 File Offset: 0x0023D451
		private bool LostHpInPreviousTurn
		{
			get
			{
				return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Any<DamageReceivedEntry>((DamageReceivedEntry e) => e.Receiver == base.Owner.Creature && !e.Result.WasFullyBlocked && e.HappenedLastPlayerTurn(base.Owner));
			}
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x0023F278 File Offset: 0x0023D478
		public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return Task.CompletedTask;
			}
			if (target != base.Owner.Creature)
			{
				return Task.CompletedTask;
			}
			if (result.UnblockedDamage <= 0)
			{
				return Task.CompletedTask;
			}
			base.Status = RelicStatus.Active;
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x0023F2CC File Offset: 0x0023D4CC
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				base.Status = RelicStatus.Normal;
				if (this.LostHpInPreviousTurn)
				{
					base.Flash();
					foreach (OrbModel orbModel in base.Owner.PlayerCombatState.OrbQueue.Orbs)
					{
						await OrbCmd.Passive(choiceContext, orbModel, null, true);
						await Cmd.Wait(0.25f, false);
					}
					IEnumerator<OrbModel> enumerator = null;
				}
			}
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x0023F31F File Offset: 0x0023D51F
		public override Task AfterCombatEnd(CombatRoom room)
		{
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}
	}
}
