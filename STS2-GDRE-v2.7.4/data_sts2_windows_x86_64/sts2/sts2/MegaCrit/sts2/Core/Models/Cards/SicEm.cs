using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AC8 RID: 2760
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SicEm : CardModel
	{
		// Token: 0x06007739 RID: 30521 RVA: 0x0029743B File Offset: 0x0029563B
		public SicEm()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002050 RID: 8272
		// (get) Token: 0x0600773A RID: 30522 RVA: 0x00297448 File Offset: 0x00295648
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17002051 RID: 8273
		// (get) Token: 0x0600773B RID: 30523 RVA: 0x00297455 File Offset: 0x00295655
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17002052 RID: 8274
		// (get) Token: 0x0600773C RID: 30524 RVA: 0x00297464 File Offset: 0x00295664
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonStatic, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17002053 RID: 8275
		// (get) Token: 0x0600773D RID: 30525 RVA: 0x00297477 File Offset: 0x00295677
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new OstyDamageVar(5m, ValueProp.Move),
					new PowerVar<SicEmPower>(3m)
				});
			}
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x002974A4 File Offset: 0x002956A4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
			await PowerCmd.Apply<SicEmPower>(choiceContext, cardPlay.Target, base.DynamicVars["SicEmPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x002974F7 File Offset: 0x002956F7
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(1m);
			base.DynamicVars["SicEmPower"].UpgradeValueBy(1m);
		}
	}
}
