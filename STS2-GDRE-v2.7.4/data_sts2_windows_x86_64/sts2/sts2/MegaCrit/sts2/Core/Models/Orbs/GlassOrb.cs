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
	// Token: 0x02000790 RID: 1936
	[NullableContext(1)]
	[Nullable(0)]
	public class GlassOrb : OrbModel
	{
		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x06005F0B RID: 24331 RVA: 0x0025A03F File Offset: 0x0025823F
		protected override string ChannelSfx
		{
			get
			{
				return "event:/sfx/characters/defect/defect_glass_channel";
			}
		}

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x06005F0C RID: 24332 RVA: 0x0025A046 File Offset: 0x00258246
		public override Color DarkenedColor
		{
			get
			{
				return new Color("008585");
			}
		}

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x06005F0D RID: 24333 RVA: 0x0025A052 File Offset: 0x00258252
		public override decimal PassiveVal
		{
			get
			{
				return base.ModifyOrbValue(this._passiveVal);
			}
		}

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06005F0E RID: 24334 RVA: 0x0025A060 File Offset: 0x00258260
		public override decimal EvokeVal
		{
			get
			{
				return this.PassiveVal * 2m;
			}
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x0025A074 File Offset: 0x00258274
		public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
		{
			await base.TriggerPassive(choiceContext, null);
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x0025A0C0 File Offset: 0x002582C0
		public override async Task Passive(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			List<Creature> list = base.CombatState.HittableEnemies.Where<Creature>((Creature e) => e.IsHittable).ToList<Creature>();
			decimal passiveVal = this.PassiveVal;
			if (!(passiveVal <= 0m))
			{
				base.ActivatePassive();
				base.PlayPassiveSfx();
				this._passiveVal = Math.Max(0m, this._passiveVal - 1m);
				await CreatureCmd.Damage(choiceContext, list, passiveVal, ValueProp.Unpowered, base.Owner.Creature);
			}
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x0025A10C File Offset: 0x0025830C
		public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
		{
			List<Creature> enemies = base.CombatState.HittableEnemies.Where<Creature>((Creature e) => e.IsHittable).ToList<Creature>();
			IEnumerable<Creature> enumerable;
			if (this.EvokeVal <= 0m)
			{
				enumerable = Array.Empty<Creature>();
			}
			else
			{
				base.ActivateEvoke(enemies.ToArray());
				await CreatureCmd.Damage(playerChoiceContext, enemies, this.EvokeVal, ValueProp.Unpowered, base.Owner.Creature);
				enumerable = enemies;
			}
			return enumerable;
		}

		// Token: 0x04002537 RID: 9527
		private decimal _passiveVal = 4m;
	}
}
