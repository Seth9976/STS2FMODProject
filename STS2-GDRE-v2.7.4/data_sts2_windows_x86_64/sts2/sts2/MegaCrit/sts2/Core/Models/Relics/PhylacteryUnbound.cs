using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B8 RID: 1464
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhylacteryUnbound : RelicModel
	{
		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x0024579B File Offset: 0x0024399B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x060053D2 RID: 21458 RVA: 0x0024579E File Offset: 0x0024399E
		public override bool SpawnsPets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x002457A1 File Offset: 0x002439A1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new SummonVar("StartOfCombat", 5m),
					new SummonVar("StartOfTurn", 2m)
				});
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x060053D4 RID: 21460 RVA: 0x002457D4 File Offset: 0x002439D4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonStatic, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x002457E8 File Offset: 0x002439E8
		public override async Task BeforeCombatStart()
		{
			await OstyCmd.Summon(new ThrowingPlayerChoiceContext(), base.Owner, base.DynamicVars["StartOfCombat"].BaseValue, this);
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x0024582C File Offset: 0x00243A2C
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				await OstyCmd.Summon(new ThrowingPlayerChoiceContext(), base.Owner, base.DynamicVars["StartOfTurn"].BaseValue, this);
			}
		}

		// Token: 0x040024A0 RID: 9376
		private const string _startOfCombatKey = "StartOfCombat";

		// Token: 0x040024A1 RID: 9377
		private const string _startOfTurnKey = "StartOfTurn";
	}
}
