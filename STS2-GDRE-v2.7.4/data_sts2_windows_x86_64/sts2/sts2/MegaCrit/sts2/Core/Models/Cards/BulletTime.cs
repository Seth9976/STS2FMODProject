using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000935 RID: 2357
	public sealed class BulletTime : CardModel
	{
		// Token: 0x06006EC7 RID: 28359 RVA: 0x002861F3 File Offset: 0x002843F3
		public BulletTime()
			: base(3, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x06006EC8 RID: 28360 RVA: 0x00286200 File Offset: 0x00284400
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			foreach (CardModel cardModel in PileType.Hand.GetPile(base.Owner).Cards)
			{
				if (!cardModel.EnergyCost.CostsX)
				{
					cardModel.SetToFreeThisTurn();
				}
			}
			await PowerCmd.Apply<NoDrawPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06006EC9 RID: 28361 RVA: 0x0028624B File Offset: 0x0028444B
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
