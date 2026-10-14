using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D5 RID: 1749
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RavenousPower : PowerModel
	{
		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06005A6E RID: 23150 RVA: 0x0025242F File Offset: 0x0025062F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06005A6F RID: 23151 RVA: 0x00252432 File Offset: 0x00250632
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06005A70 RID: 23152 RVA: 0x00252438 File Offset: 0x00250638
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.Static(StaticHoverTip.Stun, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x00252470 File Offset: 0x00250670
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (target != base.Owner)
				{
					if (target.Side == base.Owner.Side)
					{
						if (!base.Owner.IsDead)
						{
							base.Flash();
							SfxCmd.Play("event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous", 1f);
							await CreatureCmd.TriggerAnim(base.Owner, "DevourStartTrigger", 0.5f);
							((CorpseSlug)base.Owner.Monster).IsRavenous = true;
							await CreatureCmd.Stun(base.Owner, new Func<IReadOnlyList<Creature>, Task>(this.StunnedMove), null);
							await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.Amount, base.Owner, null, false);
						}
					}
				}
			}
		}

		// Token: 0x06005A72 RID: 23154 RVA: 0x002524CC File Offset: 0x002506CC
		private async Task StunnedMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous_up_double", 1f);
			await CreatureCmd.TriggerAnim(base.Owner, "DevourEndkTrigger", 0.5f);
			((CorpseSlug)base.Owner.Monster).IsRavenous = false;
		}
	}
}
