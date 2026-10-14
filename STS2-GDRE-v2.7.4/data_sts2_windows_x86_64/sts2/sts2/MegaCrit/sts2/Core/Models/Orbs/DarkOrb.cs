using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Orbs
{
	// Token: 0x0200078E RID: 1934
	[NullableContext(1)]
	[Nullable(0)]
	public class DarkOrb : OrbModel
	{
		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x06005EFB RID: 24315 RVA: 0x00259DE6 File Offset: 0x00257FE6
		protected override string ChannelSfx
		{
			get
			{
				return "event:/sfx/characters/defect/defect_dark_channel";
			}
		}

		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x06005EFC RID: 24316 RVA: 0x00259DED File Offset: 0x00257FED
		public override Color DarkenedColor
		{
			get
			{
				return new Color("9001d3");
			}
		}

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x06005EFD RID: 24317 RVA: 0x00259DF9 File Offset: 0x00257FF9
		public override decimal PassiveVal
		{
			get
			{
				return base.ModifyOrbValue(6m);
			}
		}

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x06005EFE RID: 24318 RVA: 0x00259E07 File Offset: 0x00258007
		public override decimal EvokeVal
		{
			get
			{
				return this._evokeVal;
			}
		}

		// Token: 0x06005EFF RID: 24319 RVA: 0x00259E10 File Offset: 0x00258010
		public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
		{
			await base.TriggerPassive(choiceContext, null);
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x00259E5C File Offset: 0x0025805C
		public override Task Passive(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			if (target != null)
			{
				throw new InvalidOperationException("Dark orbs cannot target creatures.");
			}
			base.ActivatePassive();
			this._evokeVal += this.PassiveVal;
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(base.Owner.Creature);
				if (creatureNode != null)
				{
					NOrbManager orbManager = creatureNode.OrbManager;
					if (orbManager != null)
					{
						orbManager.UpdateVisuals(OrbEvokeType.None);
					}
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005F01 RID: 24321 RVA: 0x00259ECC File Offset: 0x002580CC
		public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
		{
			IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
			IEnumerable<Creature> enumerable;
			if (hittableEnemies.Count == 0)
			{
				enumerable = Array.Empty<Creature>();
			}
			else
			{
				base.PlayEvokeSfx();
				Creature weakestEnemy = hittableEnemies.MinBy<Creature, int>((Creature c) => c.CurrentHp);
				base.ActivateEvoke(new Creature[] { weakestEnemy });
				await CreatureCmd.Damage(playerChoiceContext, weakestEnemy, this.EvokeVal, ValueProp.Unpowered, base.Owner.Creature);
				enumerable = new <>z__ReadOnlySingleElementList<Creature>(weakestEnemy);
			}
			return enumerable;
		}

		// Token: 0x04002536 RID: 9526
		private decimal _evokeVal = 6m;
	}
}
