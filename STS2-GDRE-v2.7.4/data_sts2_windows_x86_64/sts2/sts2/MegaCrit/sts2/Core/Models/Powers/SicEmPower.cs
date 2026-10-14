using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006EE RID: 1774
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SicEmPower : PowerModel
	{
		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06005B0C RID: 23308 RVA: 0x002538FC File Offset: 0x00251AFC
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06005B0D RID: 23309 RVA: 0x002538FF File Offset: 0x00251AFF
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x06005B0E RID: 23310 RVA: 0x00253902 File Offset: 0x00251B02
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonStatic, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x00253918 File Offset: 0x00251B18
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			Osty osty = ((dealer != null) ? dealer.Monster : null) as Osty;
			if (osty != null)
			{
				if (osty.Creature.PetOwner != null && base.Applier != null)
				{
					if (osty.Creature.PetOwner.Creature == base.Applier)
					{
						if (target == base.Owner)
						{
							await OstyCmd.Summon(choiceContext, dealer.PetOwner, base.Amount, this);
						}
					}
				}
			}
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x00253974 File Offset: 0x00251B74
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
