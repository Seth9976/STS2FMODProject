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
	// Token: 0x02000982 RID: 2434
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefendIronclad : CardModel
	{
		// Token: 0x0600704B RID: 28747 RVA: 0x0028921B File Offset: 0x0028741B
		public DefendIronclad()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17001D5D RID: 7517
		// (get) Token: 0x0600704C RID: 28748 RVA: 0x00289228 File Offset: 0x00287428
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D5E RID: 7518
		// (get) Token: 0x0600704D RID: 28749 RVA: 0x0028922B File Offset: 0x0028742B
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Defend };
			}
		}

		// Token: 0x17001D5F RID: 7519
		// (get) Token: 0x0600704E RID: 28750 RVA: 0x0028923A File Offset: 0x0028743A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x0600704F RID: 28751 RVA: 0x00289250 File Offset: 0x00287450
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06007050 RID: 28752 RVA: 0x0028929B File Offset: 0x0028749B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
