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
	// Token: 0x02000AC1 RID: 2753
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SharedFate : CardModel
	{
		// Token: 0x0600770B RID: 30475 RVA: 0x00296E13 File Offset: 0x00295013
		public SharedFate()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700203B RID: 8251
		// (get) Token: 0x0600770C RID: 30476 RVA: 0x00296E20 File Offset: 0x00295020
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x1700203C RID: 8252
		// (get) Token: 0x0600770D RID: 30477 RVA: 0x00296E28 File Offset: 0x00295028
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("EnemyStrengthLoss", 2m),
					new DynamicVar("PlayerStrengthLoss", 2m)
				});
			}
		}

		// Token: 0x1700203D RID: 8253
		// (get) Token: 0x0600770E RID: 30478 RVA: 0x00296E5C File Offset: 0x0029505C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x0600770F RID: 30479 RVA: 0x00296E7C File Offset: 0x0029507C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, -base.DynamicVars["PlayerStrengthLoss"].BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -base.DynamicVars["EnemyStrengthLoss"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007710 RID: 30480 RVA: 0x00296ECF File Offset: 0x002950CF
		protected override void OnUpgrade()
		{
			base.DynamicVars["EnemyStrengthLoss"].UpgradeValueBy(1m);
		}

		// Token: 0x040028BC RID: 10428
		private const string _enemyStrengthLossKey = "EnemyStrengthLoss";

		// Token: 0x040028BD RID: 10429
		private const string _playerStrengthLossKey = "PlayerStrengthLoss";
	}
}
