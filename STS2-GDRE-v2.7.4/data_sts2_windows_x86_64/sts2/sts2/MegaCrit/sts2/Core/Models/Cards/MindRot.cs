using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A41 RID: 2625
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MindRot : CardModel, KnowledgeDemon.IChoosable
	{
		// Token: 0x0600746D RID: 29805 RVA: 0x00291891 File Offset: 0x0028FA91
		public MindRot()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17001F23 RID: 7971
		// (get) Token: 0x0600746E RID: 29806 RVA: 0x0029189E File Offset: 0x0028FA9E
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F24 RID: 7972
		// (get) Token: 0x0600746F RID: 29807 RVA: 0x002918A1 File Offset: 0x0028FAA1
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001F25 RID: 7973
		// (get) Token: 0x06007470 RID: 29808 RVA: 0x002918A4 File Offset: 0x0028FAA4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<MindRotPower>(1m));
			}
		}

		// Token: 0x06007471 RID: 29809 RVA: 0x002918B8 File Offset: 0x0028FAB8
		public async Task OnChosen()
		{
			await PowerCmd.Apply<MindRotPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["MindRotPower"].IntValue, base.Owner.Creature, this, false);
		}
	}
}
