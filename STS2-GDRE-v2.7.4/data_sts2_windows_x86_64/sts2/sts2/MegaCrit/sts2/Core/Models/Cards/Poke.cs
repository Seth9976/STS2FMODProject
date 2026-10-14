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
	// Token: 0x02000A78 RID: 2680
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Poke : CardModel
	{
		// Token: 0x0600758E RID: 30094 RVA: 0x00293DAC File Offset: 0x00291FAC
		public Poke()
			: base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F9C RID: 8092
		// (get) Token: 0x0600758F RID: 30095 RVA: 0x00293DB9 File Offset: 0x00291FB9
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001F9D RID: 8093
		// (get) Token: 0x06007590 RID: 30096 RVA: 0x00293DC6 File Offset: 0x00291FC6
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001F9E RID: 8094
		// (get) Token: 0x06007591 RID: 30097 RVA: 0x00293DD5 File Offset: 0x00291FD5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new OstyDamageVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06007592 RID: 30098 RVA: 0x00293DE8 File Offset: 0x00291FE8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithAttackerAnim("attack_poke", 0.3f, null)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
		}

		// Token: 0x06007593 RID: 30099 RVA: 0x00293E3B File Offset: 0x0029203B
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(3m);
		}
	}
}
