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
	// Token: 0x02000A43 RID: 2627
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MinionSacrifice : CardModel
	{
		// Token: 0x06007478 RID: 29816 RVA: 0x0029199F File Offset: 0x0028FB9F
		public MinionSacrifice()
			: base(0, CardType.Skill, CardRarity.Token, TargetType.Self, true)
		{
		}

		// Token: 0x17001F29 RID: 7977
		// (get) Token: 0x06007479 RID: 29817 RVA: 0x002919AC File Offset: 0x0028FBAC
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F2A RID: 7978
		// (get) Token: 0x0600747A RID: 29818 RVA: 0x002919AF File Offset: 0x0028FBAF
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Minion };
			}
		}

		// Token: 0x17001F2B RID: 7979
		// (get) Token: 0x0600747B RID: 29819 RVA: 0x002919BE File Offset: 0x0028FBBE
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F2C RID: 7980
		// (get) Token: 0x0600747C RID: 29820 RVA: 0x002919C6 File Offset: 0x0028FBC6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x0600747D RID: 29821 RVA: 0x002919DC File Offset: 0x0028FBDC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x0600747E RID: 29822 RVA: 0x00291A27 File Offset: 0x0028FC27
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
