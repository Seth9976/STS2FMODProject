using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AD8 RID: 2776
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoulStorm : CardModel
	{
		// Token: 0x0600778D RID: 30605 RVA: 0x00297DA0 File Offset: 0x00295FA0
		public SoulStorm()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002076 RID: 8310
		// (get) Token: 0x0600778E RID: 30606 RVA: 0x00297DAD File Offset: 0x00295FAD
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x17002077 RID: 8311
		// (get) Token: 0x0600778F RID: 30607 RVA: 0x00297DBC File Offset: 0x00295FBC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(9m);
				array[1] = new ExtraDamageVar(4m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, [Nullable(2)] Creature _)
				{
					PlayerCombatState playerCombatState = card.Owner.PlayerCombatState;
					int num;
					if (playerCombatState == null)
					{
						num = 0;
					}
					else
					{
						num = playerCombatState.ExhaustPile.Cards.Count<CardModel>((CardModel c) => c is Soul);
					}
					return num;
				});
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x00297E20 File Offset: 0x00296020
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x00297E73 File Offset: 0x00296073
		protected override void OnUpgrade()
		{
			base.DynamicVars.ExtraDamage.UpgradeValueBy(2m);
		}
	}
}
