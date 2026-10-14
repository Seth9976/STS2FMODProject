using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200069C RID: 1692
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfernoPower : PowerModel
	{
		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x0024FE6B File Offset: 0x0024E06B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06005920 RID: 22816 RVA: 0x0024FE6E File Offset: 0x0024E06E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06005921 RID: 22817 RVA: 0x0024FE71 File Offset: 0x0024E071
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered));
			}
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x0024FE88 File Offset: 0x0024E088
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(base.Owner));
				}
				await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
				DamageVar damageVar = (DamageVar)base.DynamicVars["SelfDamage"];
				await CreatureCmd.Damage(choiceContext, base.Owner, damageVar.BaseValue, damageVar.Props, base.Owner);
			}
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x0024FEDC File Offset: 0x0024E0DC
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage > 0)
				{
					if (base.Owner.CombatState.CurrentSide == base.Owner.Side)
					{
						foreach (Creature creature in base.CombatState.HittableEnemies)
						{
							NFireBurstVfx nfireBurstVfx = NFireBurstVfx.Create(creature, 0.75f);
							NCombatRoom instance = NCombatRoom.Instance;
							if (instance != null)
							{
								instance.CombatVfxContainer.AddChildSafely(nfireBurstVfx);
							}
						}
						await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
					}
				}
			}
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x0024FF38 File Offset: 0x0024E138
		public void IncrementSelfDamage()
		{
			base.AssertMutable();
			DynamicVar dynamicVar = base.DynamicVars["SelfDamage"];
			decimal baseValue = dynamicVar.BaseValue;
			dynamicVar.BaseValue = baseValue + 1m;
		}

		// Token: 0x04002503 RID: 9475
		private const string _selfDamageKey = "SelfDamage";
	}
}
