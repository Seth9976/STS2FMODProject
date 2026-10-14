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
	// Token: 0x02000B1F RID: 2847
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tremble : CardModel
	{
		// Token: 0x06007917 RID: 30999 RVA: 0x0029B26D File Offset: 0x0029946D
		public Tremble()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700210C RID: 8460
		// (get) Token: 0x06007918 RID: 31000 RVA: 0x0029B27A File Offset: 0x0029947A
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x1700210D RID: 8461
		// (get) Token: 0x06007919 RID: 31001 RVA: 0x0029B282 File Offset: 0x00299482
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VulnerablePower>(3m));
			}
		}

		// Token: 0x1700210E RID: 8462
		// (get) Token: 0x0600791A RID: 31002 RVA: 0x0029B294 File Offset: 0x00299494
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x0600791B RID: 31003 RVA: 0x0029B2B4 File Offset: 0x002994B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600791C RID: 31004 RVA: 0x0029B307 File Offset: 0x00299507
		protected override void OnUpgrade()
		{
			base.DynamicVars.Vulnerable.UpgradeValueBy(1m);
		}
	}
}
