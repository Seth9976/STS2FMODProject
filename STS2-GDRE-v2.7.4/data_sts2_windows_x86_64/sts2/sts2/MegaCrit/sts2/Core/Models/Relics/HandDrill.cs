using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200055F RID: 1375
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HandDrill : RelicModel
	{
		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x0600515D RID: 20829 RVA: 0x00240D33 File Offset: 0x0023EF33
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x0600515E RID: 20830 RVA: 0x00240D36 File Offset: 0x0023EF36
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VulnerablePower>(2m));
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x0600515F RID: 20831 RVA: 0x00240D48 File Offset: 0x0023EF48
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x00240D68 File Offset: 0x0023EF68
		public override async Task AfterBlockBroken(PlayerChoiceContext choiceContext, Creature target, [Nullable(2)] Creature breaker)
		{
			if (breaker == base.Owner.Creature || ((breaker != null) ? breaker.PetOwner : null) == base.Owner)
			{
				if (!target.IsPlayer)
				{
					base.Flash();
					await PowerCmd.Apply<VulnerablePower>(choiceContext, target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
