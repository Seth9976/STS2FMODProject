using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004FB RID: 1275
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BeatingRemnant : RelicModel
	{
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06004EFE RID: 20222 RVA: 0x0023C788 File Offset: 0x0023A988
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06004EFF RID: 20223 RVA: 0x0023C78B File Offset: 0x0023A98B
		// (set) Token: 0x06004F00 RID: 20224 RVA: 0x0023C793 File Offset: 0x0023A993
		private decimal DamageReceivedThisTurn
		{
			get
			{
				return this._damageReceivedThisTurn;
			}
			set
			{
				base.AssertMutable();
				this._damageReceivedThisTurn = value;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06004F01 RID: 20225 RVA: 0x0023C7A2 File Offset: 0x0023A9A2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("MaxHpLoss", 20m));
			}
		}

		/// <summary>
		/// Note: We can't use <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyDamageCap(MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" /> because we may be taking "trample" damage from Osty.
		/// The ModifyDamageX hooks will have already run for that initial damage to Osty, and we currently can't re-run
		/// them for the trample damage.
		/// </summary>
		// Token: 0x06004F02 RID: 20226 RVA: 0x0023C7BC File Offset: 0x0023A9BC
		[NullableContext(2)]
		public override decimal ModifyHpLostAfterOsty([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return amount;
			}
			if (target != base.Owner.Creature)
			{
				return amount;
			}
			return Math.Min(amount, base.DynamicVars["MaxHpLoss"].BaseValue - this.DamageReceivedThisTurn);
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x0023C80D File Offset: 0x0023AA0D
		public override Task AfterModifyingHpLostAfterOsty()
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x0023C81C File Offset: 0x0023AA1C
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
			this.DamageReceivedThisTurn += result.UnblockedDamage;
			return Task.CompletedTask;
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x0023C870 File Offset: 0x0023AA70
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.DamageReceivedThisTurn = 0m;
			return Task.CompletedTask;
		}

		// Token: 0x04002422 RID: 9250
		private const string _maxHpLossKey = "MaxHpLoss";

		// Token: 0x04002423 RID: 9251
		private decimal _damageReceivedThisTurn;
	}
}
