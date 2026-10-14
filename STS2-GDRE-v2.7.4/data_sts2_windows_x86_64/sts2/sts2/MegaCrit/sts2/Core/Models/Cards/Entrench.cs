using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009AA RID: 2474
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Entrench : CardModel
	{
		// Token: 0x06007128 RID: 28968 RVA: 0x0028AAF9 File Offset: 0x00288CF9
		public Entrench()
			: base(2, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001DC5 RID: 7621
		// (get) Token: 0x06007129 RID: 28969 RVA: 0x0028AB06 File Offset: 0x00288D06
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<IroncladCardPool>();
			}
		}

		// Token: 0x17001DC6 RID: 7622
		// (get) Token: 0x0600712A RID: 28970 RVA: 0x0028AB0D File Offset: 0x00288D0D
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x0028AB10 File Offset: 0x00288D10
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.Owner.Creature.Block, ValueProp.Unpowered | ValueProp.Move, cardPlay, false);
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x0028AB5B File Offset: 0x00288D5B
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
