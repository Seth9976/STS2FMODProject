using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B28 RID: 2856
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Underworld : CardModel
	{
		// Token: 0x06007945 RID: 31045 RVA: 0x0029B787 File Offset: 0x00299987
		public Underworld()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x1700211F RID: 8479
		// (get) Token: 0x06007946 RID: 31046 RVA: 0x0029B794 File Offset: 0x00299994
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17002120 RID: 8480
		// (get) Token: 0x06007947 RID: 31047 RVA: 0x0029B797 File Offset: 0x00299997
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17002121 RID: 8481
		// (get) Token: 0x06007948 RID: 31048 RVA: 0x0029B7A0 File Offset: 0x002999A0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06007949 RID: 31049 RVA: 0x0029B7C0 File Offset: 0x002999C0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<UnderworldPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x0600794A RID: 31050 RVA: 0x0029B80B File Offset: 0x00299A0B
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
