using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B37 RID: 2871
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WasteAway : CardModel, KnowledgeDemon.IChoosable
	{
		// Token: 0x06007992 RID: 31122 RVA: 0x0029C0F4 File Offset: 0x0029A2F4
		public WasteAway()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x1700213F RID: 8511
		// (get) Token: 0x06007993 RID: 31123 RVA: 0x0029C101 File Offset: 0x0029A301
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002140 RID: 8512
		// (get) Token: 0x06007994 RID: 31124 RVA: 0x0029C104 File Offset: 0x0029A304
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002141 RID: 8513
		// (get) Token: 0x06007995 RID: 31125 RVA: 0x0029C107 File Offset: 0x0029A307
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17002142 RID: 8514
		// (get) Token: 0x06007996 RID: 31126 RVA: 0x0029C114 File Offset: 0x0029A314
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<WasteAwayPower>(1m));
			}
		}

		// Token: 0x06007997 RID: 31127 RVA: 0x0029C128 File Offset: 0x0029A328
		public async Task OnChosen()
		{
			await PowerCmd.Apply<WasteAwayPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["WasteAwayPower"].IntValue, base.Owner.Creature, this, false);
		}
	}
}
