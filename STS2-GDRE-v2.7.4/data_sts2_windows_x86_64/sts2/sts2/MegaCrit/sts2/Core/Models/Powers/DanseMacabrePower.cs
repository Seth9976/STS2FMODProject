using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000659 RID: 1625
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DanseMacabrePower : PowerModel
	{
		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x0024D4E8 File Offset: 0x0024B6E8
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x060057AD RID: 22445 RVA: 0x0024D4EB File Offset: 0x0024B6EB
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x060057AE RID: 22446 RVA: 0x0024D4EE File Offset: 0x0024B6EE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x060057AF RID: 22447 RVA: 0x0024D4FB File Offset: 0x0024B6FB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x0024D508 File Offset: 0x0024B708
		public override async Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature == base.Owner)
			{
				if (cardPlay.Card.EnergyCost.GetResolved() >= base.DynamicVars.Energy.IntValue)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
				}
			}
		}
	}
}
