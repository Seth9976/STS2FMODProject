using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009BF RID: 2495
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fetch : CardModel
	{
		// Token: 0x0600719D RID: 29085 RVA: 0x0028BA31 File Offset: 0x00289C31
		public Fetch()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DF9 RID: 7673
		// (get) Token: 0x0600719E RID: 29086 RVA: 0x0028BA3E File Offset: 0x00289C3E
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001DFA RID: 7674
		// (get) Token: 0x0600719F RID: 29087 RVA: 0x0028BA4D File Offset: 0x00289C4D
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001DFB RID: 7675
		// (get) Token: 0x060071A0 RID: 29088 RVA: 0x0028BA5A File Offset: 0x00289C5A
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return !this.HasBeenPlayedThisTurn;
			}
		}

		// Token: 0x17001DFC RID: 7676
		// (get) Token: 0x060071A1 RID: 29089 RVA: 0x0028BA65 File Offset: 0x00289C65
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new OstyDamageVar(3m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x060071A2 RID: 29090 RVA: 0x0028BA8C File Offset: 0x00289C8C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
				if (!this.HasBeenPlayedThisTurn)
				{
					await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
				}
			}
		}

		// Token: 0x060071A3 RID: 29091 RVA: 0x0028BADF File Offset: 0x00289CDF
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(3m);
		}

		// Token: 0x17001DFD RID: 7677
		// (get) Token: 0x060071A4 RID: 29092 RVA: 0x0028BAF7 File Offset: 0x00289CF7
		private bool HasBeenPlayedThisTurn
		{
			get
			{
				return CombatManager.Instance.History.CardPlaysFinished.Any<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.CardPlay.Card == this && e.HappenedThisTurn(base.CombatState));
			}
		}
	}
}
