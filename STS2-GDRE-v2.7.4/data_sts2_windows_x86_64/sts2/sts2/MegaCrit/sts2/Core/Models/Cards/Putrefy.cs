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

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A89 RID: 2697
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Putrefy : CardModel
	{
		// Token: 0x060075DF RID: 30175 RVA: 0x002948AB File Offset: 0x00292AAB
		public Putrefy()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FBD RID: 8125
		// (get) Token: 0x060075E0 RID: 30176 RVA: 0x002948B8 File Offset: 0x00292AB8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 2m));
			}
		}

		// Token: 0x17001FBE RID: 8126
		// (get) Token: 0x060075E1 RID: 30177 RVA: 0x002948CF File Offset: 0x00292ACF
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001FBF RID: 8127
		// (get) Token: 0x060075E2 RID: 30178 RVA: 0x002948D8 File Offset: 0x00292AD8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x00294914 File Offset: 0x00292B14
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int amount = base.DynamicVars["Power"].IntValue;
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, amount, base.Owner.Creature, this, false);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, amount, base.Owner.Creature, this, false);
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x00294967 File Offset: 0x00292B67
		protected override void OnUpgrade()
		{
			base.DynamicVars["Power"].UpgradeValueBy(1m);
		}

		// Token: 0x040028B1 RID: 10417
		private const string _powerKey = "Power";
	}
}
