using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B25 RID: 2853
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UltimateDefend : CardModel
	{
		// Token: 0x06007935 RID: 31029 RVA: 0x0029B5CC File Offset: 0x002997CC
		public UltimateDefend()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002118 RID: 8472
		// (get) Token: 0x06007936 RID: 31030 RVA: 0x0029B5D9 File Offset: 0x002997D9
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Yes, Ultimate Defend counts as a Defend for the purposes of effects like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Fasten" />.
		/// </summary>
		// Token: 0x17002119 RID: 8473
		// (get) Token: 0x06007937 RID: 31031 RVA: 0x0029B5DC File Offset: 0x002997DC
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Defend };
			}
		}

		// Token: 0x1700211A RID: 8474
		// (get) Token: 0x06007938 RID: 31032 RVA: 0x0029B5EB File Offset: 0x002997EB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x0029B5FF File Offset: 0x002997FF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(4m);
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x0029B618 File Offset: 0x00299818
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}
	}
}
