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
	// Token: 0x020009AD RID: 2477
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Equilibrium : CardModel
	{
		// Token: 0x06007137 RID: 28983 RVA: 0x0028AC93 File Offset: 0x00288E93
		public Equilibrium()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DCB RID: 7627
		// (get) Token: 0x06007138 RID: 28984 RVA: 0x0028ACA0 File Offset: 0x00288EA0
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DCC RID: 7628
		// (get) Token: 0x06007139 RID: 28985 RVA: 0x0028ACA3 File Offset: 0x00288EA3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(13m, ValueProp.Move),
					new DynamicVar("Equilibrium", 1m)
				});
			}
		}

		// Token: 0x17001DCD RID: 7629
		// (get) Token: 0x0600713A RID: 28986 RVA: 0x0028ACD2 File Offset: 0x00288ED2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
			}
		}

		// Token: 0x0600713B RID: 28987 RVA: 0x0028ACE0 File Offset: 0x00288EE0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<RetainHandPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Equilibrium"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600713C RID: 28988 RVA: 0x0028AD33 File Offset: 0x00288F33
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}

		// Token: 0x04002867 RID: 10343
		private const string _equilibriumKey = "Equilibrium";
	}
}
