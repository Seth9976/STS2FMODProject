using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F0 RID: 1776
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SkittishPower : PowerModel
	{
		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x06005B17 RID: 23319 RVA: 0x00253A3F File Offset: 0x00251C3F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x06005B18 RID: 23320 RVA: 0x00253A42 File Offset: 0x00251C42
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x06005B19 RID: 23321 RVA: 0x00253A45 File Offset: 0x00251C45
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06005B1A RID: 23322 RVA: 0x00253A48 File Offset: 0x00251C48
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06005B1B RID: 23323 RVA: 0x00253A5A File Offset: 0x00251C5A
		// (set) Token: 0x06005B1C RID: 23324 RVA: 0x00253A67 File Offset: 0x00251C67
		public bool HasGainedBlockThisTurn
		{
			get
			{
				return base.GetInternalData<SkittishPower.Data>().hasGainedBlockThisTurn;
			}
			private set
			{
				base.AssertMutable();
				base.GetInternalData<SkittishPower.Data>().hasGainedBlockThisTurn = value;
			}
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x00253A7B File Offset: 0x00251C7B
		protected override object InitInternalData()
		{
			return new SkittishPower.Data();
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x00253A84 File Offset: 0x00251C84
		public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			if (!this.HasGainedBlockThisTurn)
			{
				if (command.DamageProps.HasFlag(ValueProp.Move))
				{
					if (command.ModelSource is CardModel)
					{
						DamageResult damageResult = command.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).FirstOrDefault<DamageResult>((DamageResult r) => r.Receiver == base.Owner);
						if (damageResult != null)
						{
							if (damageResult.UnblockedDamage != 0)
							{
								this.HasGainedBlockThisTurn = true;
								SfxCmd.Play("event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_retract", 1f);
								await CreatureCmd.TriggerAnim(base.Owner, "BlockStart", 0.3f);
								await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x00253AD0 File Offset: 0x00251CD0
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side != base.Owner.Side)
			{
				if (this.HasGainedBlockThisTurn)
				{
					SfxCmd.Play("event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_extend", 1f);
					await CreatureCmd.TriggerAnim(base.Owner, "BlockEnd", 0.15f);
				}
				this.HasGainedBlockThisTurn = false;
			}
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x00253B1B File Offset: 0x00251D1B
		public override decimal GetScaledAmountForMultiplayer(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource)
		{
			return amount * (1m + (combatState.Players.Count - 1) * 0.5m);
		}

		// Token: 0x04002518 RID: 9496
		private const string _extendSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_extend";

		// Token: 0x04002519 RID: 9497
		private const string _retractSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_retract";

		// Token: 0x02001C63 RID: 7267
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Has the owner gained block from Skittish this turn?
			/// Skittish should only trigger once per turn, so we can skip when this is true.
			/// </summary>
			// Token: 0x040071CC RID: 29132
			public bool hasGainedBlockThisTurn;
		}
	}
}
