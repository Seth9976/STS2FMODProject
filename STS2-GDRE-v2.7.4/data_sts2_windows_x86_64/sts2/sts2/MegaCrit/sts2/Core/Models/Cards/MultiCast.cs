using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A4C RID: 2636
	public sealed class MultiCast : CardModel
	{
		// Token: 0x060074A8 RID: 29864 RVA: 0x00291F40 File Offset: 0x00290140
		public MultiCast()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F3E RID: 7998
		// (get) Token: 0x060074A9 RID: 29865 RVA: 0x00291F4D File Offset: 0x0029014D
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F3F RID: 7999
		// (get) Token: 0x060074AA RID: 29866 RVA: 0x00291F50 File Offset: 0x00290150
		public override OrbEvokeType OrbEvokeType
		{
			get
			{
				return OrbEvokeType.All;
			}
		}

		// Token: 0x060074AB RID: 29867 RVA: 0x00291F54 File Offset: 0x00290154
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int evokeCount = base.ResolveEnergyXValue();
			if (base.IsUpgraded)
			{
				evokeCount++;
			}
			for (int i = 0; i < evokeCount; i++)
			{
				await OrbCmd.EvokeNext(choiceContext, base.Owner, i == evokeCount - 1);
				await Cmd.Wait(0.25f, false);
			}
		}
	}
}
