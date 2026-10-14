using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Orbs
{
	// Token: 0x02000792 RID: 1938
	[NullableContext(1)]
	[Nullable(0)]
	public class PlasmaOrb : OrbModel
	{
		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x06005F1E RID: 24350 RVA: 0x0025A300 File Offset: 0x00258500
		protected override string ChannelSfx
		{
			get
			{
				return "event:/sfx/characters/defect/defect_plasma_channel";
			}
		}

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x06005F1F RID: 24351 RVA: 0x0025A307 File Offset: 0x00258507
		public override Color DarkenedColor
		{
			get
			{
				return new Color("008585");
			}
		}

		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x06005F20 RID: 24352 RVA: 0x0025A313 File Offset: 0x00258513
		public override decimal PassiveVal
		{
			get
			{
				return 1m;
			}
		}

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x06005F21 RID: 24353 RVA: 0x0025A31A File Offset: 0x0025851A
		public override decimal EvokeVal
		{
			get
			{
				return 2m;
			}
		}

		// Token: 0x06005F22 RID: 24354 RVA: 0x0025A324 File Offset: 0x00258524
		public override async Task AfterTurnStartOrbTrigger(PlayerChoiceContext choiceContext)
		{
			await base.TriggerPassive(choiceContext, null);
		}

		// Token: 0x06005F23 RID: 24355 RVA: 0x0025A370 File Offset: 0x00258570
		public override async Task Passive(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			if (target != null)
			{
				throw new InvalidOperationException("Plasma orbs cannot target creatures.");
			}
			base.ActivatePassive();
			await PlayerCmd.GainEnergy(this.PassiveVal, base.Owner);
		}

		// Token: 0x06005F24 RID: 24356 RVA: 0x0025A3BC File Offset: 0x002585BC
		public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
		{
			base.PlayEvokeSfx();
			base.ActivateEvoke(new Creature[] { base.Owner.Creature });
			await PlayerCmd.GainEnergy(this.EvokeVal, base.Owner);
			return new <>z__ReadOnlySingleElementList<Creature>(base.Owner.Creature);
		}
	}
}
