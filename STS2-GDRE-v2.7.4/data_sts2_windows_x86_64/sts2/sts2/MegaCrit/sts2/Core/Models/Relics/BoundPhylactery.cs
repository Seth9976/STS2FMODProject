using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200050E RID: 1294
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BoundPhylactery : RelicModel
	{
		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06004F78 RID: 20344 RVA: 0x0023D6CB File Offset: 0x0023B8CB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06004F79 RID: 20345 RVA: 0x0023D6CE File Offset: 0x0023B8CE
		public override bool SpawnsPets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06004F7A RID: 20346 RVA: 0x0023D6D1 File Offset: 0x0023B8D1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(1m));
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06004F7B RID: 20347 RVA: 0x0023D6E2 File Offset: 0x0023B8E2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x0023D704 File Offset: 0x0023B904
		public override async Task BeforeCombatStart()
		{
			await this.SummonPet();
		}

		/// <summary>
		/// We use AfterEnergyResetLate instead of <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterEnergyReset(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Entities.Players.Player)" /> or
		/// <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.BeforeSideTurnStart(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" /> because we want to allow effects that check for Osty's existence (like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Friendship" />) to run before we summon Osty.
		/// </summary>
		// Token: 0x06004F7D RID: 20349 RVA: 0x0023D748 File Offset: 0x0023B948
		public override async Task AfterEnergyResetLate(Player player)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber != 1)
				{
					await this.SummonPet();
				}
			}
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x0023D794 File Offset: 0x0023B994
		private async Task SummonPet()
		{
			await OstyCmd.Summon(new ThrowingPlayerChoiceContext(), base.Owner, base.DynamicVars.Summon.BaseValue, this);
		}
	}
}
