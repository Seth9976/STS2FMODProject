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
	// Token: 0x02000993 RID: 2451
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Disintegration : CardModel, KnowledgeDemon.IChoosable
	{
		// Token: 0x060070AB RID: 28843 RVA: 0x00289CDB File Offset: 0x00287EDB
		public Disintegration()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17001D8A RID: 7562
		// (get) Token: 0x060070AC RID: 28844 RVA: 0x00289CE8 File Offset: 0x00287EE8
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D8B RID: 7563
		// (get) Token: 0x060070AD RID: 28845 RVA: 0x00289CEB File Offset: 0x00287EEB
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D8C RID: 7564
		// (get) Token: 0x060070AE RID: 28846 RVA: 0x00289CEE File Offset: 0x00287EEE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DisintegrationPower>(6m));
			}
		}

		// Token: 0x060070AF RID: 28847 RVA: 0x00289D00 File Offset: 0x00287F00
		public async Task OnChosen()
		{
			await PowerCmd.Apply<DisintegrationPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["DisintegrationPower"].BaseValue, base.Owner.Creature, this, false);
		}
	}
}
