using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009FC RID: 2556
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HelloWorld : CardModel
	{
		// Token: 0x060072EC RID: 29420 RVA: 0x0028E593 File Offset: 0x0028C793
		public HelloWorld()
			: base(1, CardType.Power, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001E87 RID: 7815
		// (get) Token: 0x060072ED RID: 29421 RVA: 0x0028E5A0 File Offset: 0x0028C7A0
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<DefectCardPool>();
			}
		}

		// Token: 0x060072EE RID: 29422 RVA: 0x0028E5A8 File Offset: 0x0028C7A8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<HelloWorldPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x0028E5F3 File Offset: 0x0028C7F3
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
