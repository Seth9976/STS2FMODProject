using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Orbs
{
	// Token: 0x0200078F RID: 1935
	[NullableContext(1)]
	[Nullable(0)]
	public class FrostOrb : OrbModel
	{
		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x06005F03 RID: 24323 RVA: 0x00259F2B File Offset: 0x0025812B
		protected override string ChannelSfx
		{
			get
			{
				return "event:/sfx/characters/defect/defect_frost_channel";
			}
		}

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x06005F04 RID: 24324 RVA: 0x00259F32 File Offset: 0x00258132
		public override Color DarkenedColor
		{
			get
			{
				return new Color("7860a7");
			}
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x06005F05 RID: 24325 RVA: 0x00259F3E File Offset: 0x0025813E
		public override decimal PassiveVal
		{
			get
			{
				return base.ModifyOrbValue(2m);
			}
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x06005F06 RID: 24326 RVA: 0x00259F4C File Offset: 0x0025814C
		public override decimal EvokeVal
		{
			get
			{
				return base.ModifyOrbValue(5m);
			}
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x00259F5C File Offset: 0x0025815C
		public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
		{
			await base.TriggerPassive(choiceContext, null);
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x00259FA8 File Offset: 0x002581A8
		public override async Task Passive(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			if (target != null)
			{
				throw new InvalidOperationException("Frost orbs cannot target creatures.");
			}
			base.ActivatePassive();
			base.PlayPassiveSfx();
			await CreatureCmd.GainBlock(base.Owner.Creature, this.PassiveVal, ValueProp.Unpowered, null, false);
			if (base.Owner.Creature.HasPower<HibernatePower>())
			{
				foreach (Player player in base.CombatState.Players)
				{
					if (player != base.Owner)
					{
						await CreatureCmd.GainBlock(player.Creature, this.PassiveVal, ValueProp.Unpowered, null, false);
					}
				}
				IEnumerator<Player> enumerator = null;
			}
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x00259FF4 File Offset: 0x002581F4
		public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
		{
			base.PlayEvokeSfx();
			base.ActivateEvoke(new Creature[] { base.Owner.Creature });
			await CreatureCmd.GainBlock(base.Owner.Creature, this.EvokeVal, ValueProp.Unpowered, null, false);
			IEnumerable<Creature> enumerable;
			if (base.Owner.Creature.HasPower<HibernatePower>())
			{
				foreach (Player player in base.CombatState.Players)
				{
					if (player != base.Owner)
					{
						await CreatureCmd.GainBlock(player.Creature, this.EvokeVal, ValueProp.Unpowered, null, false);
					}
				}
				IEnumerator<Player> enumerator = null;
				enumerable = base.CombatState.Players.Select<Player, Creature>((Player p) => p.Creature);
			}
			else
			{
				enumerable = new <>z__ReadOnlySingleElementList<Creature>(base.Owner.Creature);
			}
			return enumerable;
		}
	}
}
