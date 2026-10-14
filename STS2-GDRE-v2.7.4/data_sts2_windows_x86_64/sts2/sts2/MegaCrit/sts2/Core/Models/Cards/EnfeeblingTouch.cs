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
	// Token: 0x020009A7 RID: 2471
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EnfeeblingTouch : CardModel
	{
		// Token: 0x06007119 RID: 28953 RVA: 0x0028A94A File Offset: 0x00288B4A
		public EnfeeblingTouch()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DBD RID: 7613
		// (get) Token: 0x0600711A RID: 28954 RVA: 0x0028A957 File Offset: 0x00288B57
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("StrengthLoss", 8m));
			}
		}

		// Token: 0x17001DBE RID: 7614
		// (get) Token: 0x0600711B RID: 28955 RVA: 0x0028A96E File Offset: 0x00288B6E
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x17001DBF RID: 7615
		// (get) Token: 0x0600711C RID: 28956 RVA: 0x0028A978 File Offset: 0x00288B78
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x0028A998 File Offset: 0x00288B98
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<EnfeeblingTouchPower>(choiceContext, cardPlay.Target, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x0028A9EB File Offset: 0x00288BEB
		protected override void OnUpgrade()
		{
			base.DynamicVars["StrengthLoss"].UpgradeValueBy(3m);
		}

		// Token: 0x04002866 RID: 10342
		private const string _strengthLossKey = "StrengthLoss";
	}
}
