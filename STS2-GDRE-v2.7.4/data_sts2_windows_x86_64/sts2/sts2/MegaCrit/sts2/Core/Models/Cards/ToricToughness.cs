using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B1A RID: 2842
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToricToughness : CardModel
	{
		// Token: 0x060078FC RID: 30972 RVA: 0x0029AF95 File Offset: 0x00299195
		public ToricToughness()
			: base(2, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x170020FF RID: 8447
		// (get) Token: 0x060078FD RID: 30973 RVA: 0x0029AFA2 File Offset: 0x002991A2
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002100 RID: 8448
		// (get) Token: 0x060078FE RID: 30974 RVA: 0x0029AFA5 File Offset: 0x002991A5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Turns", 2m),
					new BlockVar(5m, ValueProp.Move)
				});
			}
		}

		// Token: 0x17002101 RID: 8449
		// (get) Token: 0x060078FF RID: 30975 RVA: 0x0029AFD4 File Offset: 0x002991D4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x0029AFE8 File Offset: 0x002991E8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			decimal num = await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			decimal blockAmount = num;
			ToricToughnessPower toricToughnessPower = await PowerCmd.Apply<ToricToughnessPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Turns"].BaseValue, base.Owner.Creature, this, false);
			if (toricToughnessPower != null)
			{
				toricToughnessPower.SetBlock(blockAmount);
			}
		}

		// Token: 0x06007901 RID: 30977 RVA: 0x0029B03B File Offset: 0x0029923B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}

		// Token: 0x040028D6 RID: 10454
		private const string _turnsKey = "Turns";
	}
}
