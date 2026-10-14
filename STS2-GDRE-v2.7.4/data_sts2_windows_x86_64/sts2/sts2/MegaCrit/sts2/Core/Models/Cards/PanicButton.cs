using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A69 RID: 2665
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PanicButton : CardModel
	{
		// Token: 0x0600753D RID: 30013 RVA: 0x002932A4 File Offset: 0x002914A4
		public PanicButton()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F7D RID: 8061
		// (get) Token: 0x0600753E RID: 30014 RVA: 0x002932B1 File Offset: 0x002914B1
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F7E RID: 8062
		// (get) Token: 0x0600753F RID: 30015 RVA: 0x002932B4 File Offset: 0x002914B4
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F7F RID: 8063
		// (get) Token: 0x06007540 RID: 30016 RVA: 0x002932BC File Offset: 0x002914BC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(30m, ValueProp.Move),
					new DynamicVar("Turns", 2m)
				});
			}
		}

		// Token: 0x06007541 RID: 30017 RVA: 0x002932EC File Offset: 0x002914EC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<NoBlockPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Turns"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007542 RID: 30018 RVA: 0x0029333F File Offset: 0x0029153F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(10m);
		}

		// Token: 0x040028AD RID: 10413
		private const string _turnsKey = "Turns";
	}
}
