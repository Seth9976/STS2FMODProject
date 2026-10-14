using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000709 RID: 1801
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SuckPower : PowerModel
	{
		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x06005BA9 RID: 23465 RVA: 0x0025498B File Offset: 0x00252B8B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x06005BAA RID: 23466 RVA: 0x0025498E File Offset: 0x00252B8E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06005BAB RID: 23467 RVA: 0x00254994 File Offset: 0x00252B94
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005BAC RID: 23468 RVA: 0x002549B4 File Offset: 0x00252BB4
		public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			if (command.Attacker == base.Owner)
			{
				if (command.TargetSide != base.Owner.Side)
				{
					if (command.DamageProps.IsPoweredAttack())
					{
						List<List<DamageResult>> list = command.Results.ToList<List<DamageResult>>();
						int num = 0;
						foreach (List<DamageResult> list2 in list)
						{
							List<DamageResult> list3 = list2.Where<DamageResult>((DamageResult r) => r.Receiver.IsPet).ToList<DamageResult>();
							using (List<DamageResult>.Enumerator enumerator2 = list3.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									DamageResult petHit = enumerator2.Current;
									list2.RemoveAll(delegate(DamageResult r)
									{
										Creature receiver = r.Receiver;
										Player petOwner = petHit.Receiver.PetOwner;
										return receiver == ((petOwner != null) ? petOwner.Creature : null);
									});
								}
							}
							if (list2.Any<DamageResult>((DamageResult r) => r.UnblockedDamage > 0))
							{
								num++;
							}
						}
						if (num > 0)
						{
							base.Flash();
							await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.Amount * num, base.Owner, null, false);
						}
					}
				}
			}
		}
	}
}
