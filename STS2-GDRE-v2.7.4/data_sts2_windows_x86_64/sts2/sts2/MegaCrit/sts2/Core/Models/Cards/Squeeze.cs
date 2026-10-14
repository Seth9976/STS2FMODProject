using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AE6 RID: 2790
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Squeeze : CardModel
	{
		// Token: 0x060077EC RID: 30700 RVA: 0x00298AD4 File Offset: 0x00296CD4
		public Squeeze()
			: base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700209A RID: 8346
		// (get) Token: 0x060077ED RID: 30701 RVA: 0x00298AE1 File Offset: 0x00296CE1
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x1700209B RID: 8347
		// (get) Token: 0x060077EE RID: 30702 RVA: 0x00298AEE File Offset: 0x00296CEE
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x1700209C RID: 8348
		// (get) Token: 0x060077EF RID: 30703 RVA: 0x00298B00 File Offset: 0x00296D00
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(25m);
				array[1] = new ExtraDamageVar(5m).FromOsty();
				array[2] = new CalculatedDamageVar(ValueProp.Move).FromOsty().WithMultiplier((CardModel card, [Nullable(2)] Creature _) => card.Owner.PlayerCombatState.AllCards.Count<CardModel>((CardModel c) => c.Tags.Contains(CardTag.OstyAttack) && c != card));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060077F0 RID: 30704 RVA: 0x00298B6C File Offset: 0x00296D6C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
		}

		// Token: 0x060077F1 RID: 30705 RVA: 0x00298BBF File Offset: 0x00296DBF
		protected override void OnUpgrade()
		{
			base.DynamicVars.CalculationBase.UpgradeValueBy(5m);
			base.DynamicVars.ExtraDamage.UpgradeValueBy(1m);
		}
	}
}
