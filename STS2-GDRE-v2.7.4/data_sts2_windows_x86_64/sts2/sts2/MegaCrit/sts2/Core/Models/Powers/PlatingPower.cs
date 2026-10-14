using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006CB RID: 1739
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlatingPower : PowerModel
	{
		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06005A2C RID: 23084 RVA: 0x00251BE9 File Offset: 0x0024FDE9
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06005A2D RID: 23085 RVA: 0x00251BEC File Offset: 0x0024FDEC
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06005A2E RID: 23086 RVA: 0x00251BEF File Offset: 0x0024FDEF
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x06005A2F RID: 23087 RVA: 0x00251BF2 File Offset: 0x0024FDF2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x06005A30 RID: 23088 RVA: 0x00251C04 File Offset: 0x0024FE04
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Decrement", 1m));
			}
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x00251C1C File Offset: 0x0024FE1C
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			if (base.Owner.Side == CombatSide.Enemy)
			{
				base.DynamicVars["Decrement"].BaseValue = base.Owner.CombatState.RunState.Players.Count;
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// We want enemies that start with Plating to also start combat with block.
		/// </summary>
		// Token: 0x06005A32 RID: 23090 RVA: 0x00251C70 File Offset: 0x0024FE70
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (side != CombatSide.Player)
			{
				return Task.CompletedTask;
			}
			if (base.Owner.IsPlayer)
			{
				return Task.CompletedTask;
			}
			if (combatState.RoundNumber > 1)
			{
				return Task.CompletedTask;
			}
			return CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
		}

		/// <summary>
		/// We do this in early so that it triggers before end-of-turn damage effects.
		/// </summary>
		// Token: 0x06005A33 RID: 23091 RVA: 0x00251CC4 File Offset: 0x0024FEC4
		public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
			}
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x00251D10 File Offset: 0x0024FF10
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.Owner.Player == null || base.Owner.Player.PlayerCombatState.TurnNumber != 1)
				{
					if (base.Owner.Side != CombatSide.Enemy || combatState.RoundNumber != 1)
					{
						if (base.Owner.Side == CombatSide.Enemy)
						{
							await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), this, -base.DynamicVars["Decrement"].BaseValue, null, null, false);
						}
						else
						{
							await PowerCmd.Decrement(this);
						}
					}
				}
			}
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x00251D63 File Offset: 0x0024FF63
		public override decimal GetScaledAmountForMultiplayer(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource)
		{
			return ((combatState.Players.Count - 1) * 2 + 1) * amount;
		}

		// Token: 0x0400250E RID: 9486
		private const string _decrementKey = "Decrement";
	}
}
