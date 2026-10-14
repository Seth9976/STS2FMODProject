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
	// Token: 0x02000AD7 RID: 2775
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Soulbound : CardModel
	{
		// Token: 0x06007788 RID: 30600 RVA: 0x00297D26 File Offset: 0x00295F26
		public Soulbound()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17002074 RID: 8308
		// (get) Token: 0x06007789 RID: 30601 RVA: 0x00297D33 File Offset: 0x00295F33
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17002075 RID: 8309
		// (get) Token: 0x0600778A RID: 30602 RVA: 0x00297D36 File Offset: 0x00295F36
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x0600778B RID: 30603 RVA: 0x00297D44 File Offset: 0x00295F44
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await CreatureCmd.TriggerAnim(cardPlay.Target, "PowerUp", cardPlay.Target.Player.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SoulboundPower>(choiceContext, cardPlay.Target, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x0600778C RID: 30604 RVA: 0x00297D97 File Offset: 0x00295F97
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
