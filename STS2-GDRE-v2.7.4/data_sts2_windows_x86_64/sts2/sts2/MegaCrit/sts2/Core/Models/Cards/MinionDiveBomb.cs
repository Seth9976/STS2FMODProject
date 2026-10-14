using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A42 RID: 2626
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MinionDiveBomb : CardModel
	{
		// Token: 0x06007472 RID: 29810 RVA: 0x002918FB File Offset: 0x0028FAFB
		public MinionDiveBomb()
			: base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F26 RID: 7974
		// (get) Token: 0x06007473 RID: 29811 RVA: 0x00291908 File Offset: 0x0028FB08
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Minion };
			}
		}

		// Token: 0x17001F27 RID: 7975
		// (get) Token: 0x06007474 RID: 29812 RVA: 0x00291917 File Offset: 0x0028FB17
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(13m, ValueProp.Move));
			}
		}

		// Token: 0x17001F28 RID: 7976
		// (get) Token: 0x06007475 RID: 29813 RVA: 0x0029192B File Offset: 0x0028FB2B
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007476 RID: 29814 RVA: 0x00291934 File Offset: 0x0028FB34
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).WithAttackerAnim("Cast", 1f, null)
				.Targeting(cardPlay.Target)
				.WithAttackerFx(() => NMinionDiveBombVfx.Create(this.Owner.Creature, cardPlay.Target))
				.Execute(choiceContext);
		}

		// Token: 0x06007477 RID: 29815 RVA: 0x00291987 File Offset: 0x0028FB87
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
