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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B0C RID: 2828
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheBall : CardModel
	{
		// Token: 0x060078A6 RID: 30886 RVA: 0x0029A4BA File Offset: 0x002986BA
		public TheBall()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020E0 RID: 8416
		// (get) Token: 0x060078A7 RID: 30887 RVA: 0x0029A4C7 File Offset: 0x002986C7
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		/// <summary>
		/// Required so we can restore the extra damage amount after a downgrade (ie Magiknight)
		/// </summary>
		// Token: 0x170020E1 RID: 8417
		// (get) Token: 0x060078A8 RID: 30888 RVA: 0x0029A4CA File Offset: 0x002986CA
		// (set) Token: 0x060078A9 RID: 30889 RVA: 0x0029A4D2 File Offset: 0x002986D2
		private decimal ExtraDamageFromPlays
		{
			get
			{
				return this._extraDamageFromPlays;
			}
			set
			{
				base.AssertMutable();
				this._extraDamageFromPlays = value;
			}
		}

		// Token: 0x170020E2 RID: 8418
		// (get) Token: 0x060078AA RID: 30890 RVA: 0x0029A4E1 File Offset: 0x002986E1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new DynamicVar("Increase", 10m)
				});
			}
		}

		// Token: 0x060078AB RID: 30891 RVA: 0x0029A514 File Offset: 0x00298714
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			base.DynamicVars.Damage.BaseValue += base.DynamicVars["Increase"].BaseValue;
			this.ExtraDamageFromPlays += base.DynamicVars["Increase"].BaseValue;
		}

		// Token: 0x060078AC RID: 30892 RVA: 0x0029A568 File Offset: 0x00298768
		protected override CardLocation GetResultLocationForCardPlay()
		{
			CardLocation resultLocationForCardPlay = base.GetResultLocationForCardPlay();
			if (base.CombatState == null)
			{
				return resultLocationForCardPlay;
			}
			List<Creature> list = (from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
				where c != null && c.IsAlive && c.IsPlayer && c.Player != base.Owner
				select c).ToList<Creature>();
			if (list.Count == 0)
			{
				return resultLocationForCardPlay;
			}
			resultLocationForCardPlay.player = base.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(list).Player;
			if (resultLocationForCardPlay.pileType == PileType.Discard)
			{
				resultLocationForCardPlay.pileType = PileType.Draw;
				resultLocationForCardPlay.position = CardPilePosition.Random;
			}
			return resultLocationForCardPlay;
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x0029A5FE File Offset: 0x002987FE
		protected override void AfterDowngraded()
		{
			base.AfterDowngraded();
			base.DynamicVars.Damage.BaseValue += this.ExtraDamageFromPlays;
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x0029A627 File Offset: 0x00298827
		protected override void OnUpgrade()
		{
			base.DynamicVars["Increase"].UpgradeValueBy(5m);
		}

		// Token: 0x040028CD RID: 10445
		private const string _increaseKey = "Increase";

		// Token: 0x040028CE RID: 10446
		private decimal _extraDamageFromPlays;
	}
}
