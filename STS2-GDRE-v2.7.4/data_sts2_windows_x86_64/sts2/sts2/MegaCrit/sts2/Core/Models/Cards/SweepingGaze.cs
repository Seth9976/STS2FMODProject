using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AFF RID: 2815
	[NullableContext(1)]
	[Nullable(0)]
	public class SweepingGaze : CardModel
	{
		// Token: 0x06007864 RID: 30820 RVA: 0x00299BD7 File Offset: 0x00297DD7
		public SweepingGaze()
			: base(0, CardType.Attack, CardRarity.Token, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x170020C4 RID: 8388
		// (get) Token: 0x06007865 RID: 30821 RVA: 0x00299BE4 File Offset: 0x00297DE4
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x170020C5 RID: 8389
		// (get) Token: 0x06007866 RID: 30822 RVA: 0x00299BF1 File Offset: 0x00297DF1
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x170020C6 RID: 8390
		// (get) Token: 0x06007867 RID: 30823 RVA: 0x00299C00 File Offset: 0x00297E00
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Ethereal,
					CardKeyword.Exhaust
				});
			}
		}

		// Token: 0x170020C7 RID: 8391
		// (get) Token: 0x06007868 RID: 30824 RVA: 0x00299C15 File Offset: 0x00297E15
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new OstyDamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x06007869 RID: 30825 RVA: 0x00299C2C File Offset: 0x00297E2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).TargetingRandomOpponents(base.CombatState, true)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
		}

		// Token: 0x0600786A RID: 30826 RVA: 0x00299C7F File Offset: 0x00297E7F
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(5m);
		}
	}
}
