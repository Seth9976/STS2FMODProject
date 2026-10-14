using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A8B RID: 2699
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Quadcast : CardModel
	{
		// Token: 0x060075EB RID: 30187 RVA: 0x00294A52 File Offset: 0x00292C52
		public Quadcast()
			: base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001FC2 RID: 8130
		// (get) Token: 0x060075EC RID: 30188 RVA: 0x00294A5F File Offset: 0x00292C5F
		public override OrbEvokeType OrbEvokeType
		{
			get
			{
				return OrbEvokeType.Front;
			}
		}

		// Token: 0x17001FC3 RID: 8131
		// (get) Token: 0x060075ED RID: 30189 RVA: 0x00294A62 File Offset: 0x00292C62
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(4));
			}
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x00294A70 File Offset: 0x00292C70
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (base.Owner.PlayerCombatState.OrbQueue.Orbs.Count > 0)
			{
				await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
				for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
				{
					await OrbCmd.EvokeNext(choiceContext, base.Owner, i == base.DynamicVars.Repeat.IntValue - 1);
					if (i != base.DynamicVars.Repeat.IntValue - 1)
					{
						await Cmd.CustomScaledWait(0.15f, 0.25f, false, default(CancellationToken));
					}
				}
			}
		}

		// Token: 0x060075EF RID: 30191 RVA: 0x00294ABB File Offset: 0x00292CBB
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
