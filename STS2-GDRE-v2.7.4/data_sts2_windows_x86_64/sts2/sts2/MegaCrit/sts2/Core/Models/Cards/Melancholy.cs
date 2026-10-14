using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A39 RID: 2617
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Melancholy : CardModel
	{
		// Token: 0x0600743D RID: 29757 RVA: 0x002911D1 File Offset: 0x0028F3D1
		public Melancholy()
			: base(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F0F RID: 7951
		// (get) Token: 0x0600743E RID: 29758 RVA: 0x002911DE File Offset: 0x0028F3DE
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F10 RID: 7952
		// (get) Token: 0x0600743F RID: 29759 RVA: 0x002911E1 File Offset: 0x0028F3E1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(13m, ValueProp.Move),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001F11 RID: 7953
		// (get) Token: 0x06007440 RID: 29760 RVA: 0x00291207 File Offset: 0x0028F407
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007441 RID: 29761 RVA: 0x00291214 File Offset: 0x0028F414
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06007442 RID: 29762 RVA: 0x00291260 File Offset: 0x0028F460
		public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (wasRemovalPrevented)
			{
				return Task.CompletedTask;
			}
			CardPile pile = base.Pile;
			if (pile == null || !pile.IsCombatPile)
			{
				return Task.CompletedTask;
			}
			base.EnergyCost.AddThisCombat(-base.DynamicVars.Energy.IntValue, false);
			return Task.CompletedTask;
		}

		// Token: 0x06007443 RID: 29763 RVA: 0x002912B0 File Offset: 0x0028F4B0
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(4m);
		}
	}
}
