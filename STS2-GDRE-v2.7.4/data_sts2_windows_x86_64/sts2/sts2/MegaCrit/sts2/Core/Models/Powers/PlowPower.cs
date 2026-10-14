using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
	// Token: 0x020006CC RID: 1740
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlowPower : PowerModel
	{
		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x06005A37 RID: 23095 RVA: 0x00251D89 File Offset: 0x0024FF89
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x06005A38 RID: 23096 RVA: 0x00251D8C File Offset: 0x0024FF8C
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x06005A39 RID: 23097 RVA: 0x00251D90 File Offset: 0x0024FF90
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Stun, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromPower<StrengthPower>(null)
				});
			}
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06005A3A RID: 23098 RVA: 0x00251DC7 File Offset: 0x0024FFC7
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x00251DCC File Offset: 0x0024FFCC
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage > 0)
				{
					if (target.CurrentHp <= base.Amount)
					{
						base.Flash();
						List<TemporaryStrengthPower> list = base.Owner.GetPowerInstances<TemporaryStrengthPower>().ToList<TemporaryStrengthPower>();
						foreach (TemporaryStrengthPower temporaryStrengthPower in list)
						{
							await PowerCmd.Remove(temporaryStrengthPower);
						}
						List<TemporaryStrengthPower>.Enumerator enumerator = default(List<TemporaryStrengthPower>.Enumerator);
						await PowerCmd.Remove<StrengthPower>(base.Owner);
						CeremonialBeast ceremonialBeast = base.Owner.Monster as CeremonialBeast;
						if (ceremonialBeast != null)
						{
							await ceremonialBeast.SetStunned();
							await CreatureCmd.Stun(base.Owner, new Func<IReadOnlyList<Creature>, Task>(ceremonialBeast.StunnedMove), ceremonialBeast.BeastCryState.StateId);
						}
						else
						{
							await CreatureCmd.Stun(base.Owner, null);
						}
						await PowerCmd.Remove(this);
					}
				}
			}
		}
	}
}
