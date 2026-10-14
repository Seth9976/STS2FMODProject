using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009C8 RID: 2504
	public sealed class Flanking : CardModel
	{
		// Token: 0x060071D0 RID: 29136 RVA: 0x0028C16E File Offset: 0x0028A36E
		public Flanking()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E0F RID: 7695
		// (get) Token: 0x060071D1 RID: 29137 RVA: 0x0028C17B File Offset: 0x0028A37B
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x060071D2 RID: 29138 RVA: 0x0028C180 File Offset: 0x0028A380
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<FlankingPower>(choiceContext, cardPlay.Target, 2m, base.Owner.Creature, this, false);
		}

		// Token: 0x060071D3 RID: 29139 RVA: 0x0028C1D3 File Offset: 0x0028A3D3
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
