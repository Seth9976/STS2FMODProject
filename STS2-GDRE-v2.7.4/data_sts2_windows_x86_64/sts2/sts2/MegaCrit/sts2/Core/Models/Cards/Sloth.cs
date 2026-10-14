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
	// Token: 0x02000ACF RID: 2767
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Sloth : CardModel, KnowledgeDemon.IChoosable
	{
		// Token: 0x0600775B RID: 30555 RVA: 0x0029783B File Offset: 0x00295A3B
		public Sloth()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x1700205E RID: 8286
		// (get) Token: 0x0600775C RID: 30556 RVA: 0x00297848 File Offset: 0x00295A48
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700205F RID: 8287
		// (get) Token: 0x0600775D RID: 30557 RVA: 0x0029784B File Offset: 0x00295A4B
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002060 RID: 8288
		// (get) Token: 0x0600775E RID: 30558 RVA: 0x0029784E File Offset: 0x00295A4E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SlothPower>(3m));
			}
		}

		// Token: 0x0600775F RID: 30559 RVA: 0x00297860 File Offset: 0x00295A60
		public async Task OnChosen()
		{
			await PowerCmd.Apply<SlothPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["SlothPower"].IntValue, base.Owner.Creature, this, false);
		}
	}
}
