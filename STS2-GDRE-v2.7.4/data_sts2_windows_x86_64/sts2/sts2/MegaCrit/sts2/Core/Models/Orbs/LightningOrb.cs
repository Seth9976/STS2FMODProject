using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Orbs
{
	// Token: 0x02000791 RID: 1937
	[NullableContext(1)]
	[Nullable(0)]
	public class LightningOrb : OrbModel
	{
		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x06005F13 RID: 24339 RVA: 0x0025A16B File Offset: 0x0025836B
		protected override string PassiveSfx
		{
			get
			{
				return "event:/sfx/characters/defect/defect_lightning_passive";
			}
		}

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x06005F14 RID: 24340 RVA: 0x0025A172 File Offset: 0x00258372
		protected override string EvokeSfx
		{
			get
			{
				return "event:/sfx/characters/defect/defect_lightning_evoke";
			}
		}

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x06005F15 RID: 24341 RVA: 0x0025A179 File Offset: 0x00258379
		protected override string ChannelSfx
		{
			get
			{
				return "event:/sfx/characters/defect/defect_lightning_channel";
			}
		}

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x06005F16 RID: 24342 RVA: 0x0025A180 File Offset: 0x00258380
		public override Color DarkenedColor
		{
			get
			{
				return new Color("796606");
			}
		}

		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x06005F17 RID: 24343 RVA: 0x0025A18C File Offset: 0x0025838C
		public override decimal PassiveVal
		{
			get
			{
				return base.ModifyOrbValue(3m);
			}
		}

		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x06005F18 RID: 24344 RVA: 0x0025A19A File Offset: 0x0025839A
		public override decimal EvokeVal
		{
			get
			{
				return base.ModifyOrbValue(8m);
			}
		}

		// Token: 0x06005F19 RID: 24345 RVA: 0x0025A1A8 File Offset: 0x002583A8
		public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
		{
			await base.TriggerPassive(choiceContext, null);
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x0025A1F4 File Offset: 0x002583F4
		public override async Task Passive(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			base.ActivatePassive();
			await this.ApplyLightningDamage(this.PassiveVal, target, choiceContext, false);
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x0025A248 File Offset: 0x00258448
		public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
		{
			return await this.ApplyLightningDamage(this.EvokeVal, null, playerChoiceContext, true);
		}

		// Token: 0x06005F1C RID: 24348 RVA: 0x0025A294 File Offset: 0x00258494
		private async Task<IEnumerable<Creature>> ApplyLightningDamage(decimal value, [Nullable(2)] Creature target, PlayerChoiceContext choiceContext, bool isEvoke)
		{
			List<Creature> list = (from e in base.CombatState.GetOpponentsOf(base.Owner.Creature)
				where e.IsHittable
				select e).ToList<Creature>();
			IEnumerable<Creature> enumerable;
			if (list.Count == 0)
			{
				enumerable = Array.Empty<Creature>();
			}
			else
			{
				IReadOnlyList<Creature> targets;
				if (target != null)
				{
					targets = new <>z__ReadOnlySingleElementList<Creature>(target);
				}
				else
				{
					targets = new <>z__ReadOnlySingleElementList<Creature>(base.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(list));
				}
				if (isEvoke)
				{
					base.ActivateEvoke(targets.ToArray<Creature>());
				}
				foreach (Creature creature in targets)
				{
					VfxCmd.PlayOnCreature(creature, "vfx/vfx_attack_lightning");
				}
				base.PlayEvokeSfx();
				await CreatureCmd.Damage(choiceContext, targets, value, ValueProp.Unpowered, base.Owner.Creature);
				enumerable = targets;
			}
			return enumerable;
		}
	}
}
