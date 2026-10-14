using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A25 RID: 2597
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LeadingStrike : CardModel
	{
		// Token: 0x060073BF RID: 29631 RVA: 0x00290047 File Offset: 0x0028E247
		public LeadingStrike()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001EDC RID: 7900
		// (get) Token: 0x060073C0 RID: 29632 RVA: 0x00290054 File Offset: 0x0028E254
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001EDD RID: 7901
		// (get) Token: 0x060073C1 RID: 29633 RVA: 0x00290063 File Offset: 0x0028E263
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar("Shivs", 2),
					new DamageVar(3m, ValueProp.Move)
				});
			}
		}

		// Token: 0x17001EDE RID: 7902
		// (get) Token: 0x060073C2 RID: 29634 RVA: 0x0029008D File Offset: 0x0028E28D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x060073C3 RID: 29635 RVA: 0x0029009C File Offset: 0x0028E29C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			for (int i = 0; i < base.DynamicVars["Shivs"].IntValue; i++)
			{
				await Shiv.CreateInHand(base.Owner, base.CombatState, null);
				await Cmd.Wait(0.25f, false);
			}
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x002900EF File Offset: 0x0028E2EF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}

		// Token: 0x04002886 RID: 10374
		private const string _shivsKey = "Shivs";
	}
}
