using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A91 RID: 2705
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rampage : CardModel
	{
		// Token: 0x06007607 RID: 30215 RVA: 0x00294DDF File Offset: 0x00292FDF
		public Rampage()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FCD RID: 8141
		// (get) Token: 0x06007608 RID: 30216 RVA: 0x00294DEC File Offset: 0x00292FEC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(9m, ValueProp.Move),
					new DynamicVar("Increase", 5m)
				});
			}
		}

		/// <summary>
		/// Required so we can restore the extra damage amount after a downgrade (ie Magiknight)
		/// </summary>
		// Token: 0x17001FCE RID: 8142
		// (get) Token: 0x06007609 RID: 30217 RVA: 0x00294E1C File Offset: 0x0029301C
		// (set) Token: 0x0600760A RID: 30218 RVA: 0x00294E24 File Offset: 0x00293024
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

		// Token: 0x0600760B RID: 30219 RVA: 0x00294E34 File Offset: 0x00293034
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			base.DynamicVars.Damage.BaseValue += base.DynamicVars["Increase"].BaseValue;
			this.ExtraDamageFromPlays += base.DynamicVars["Increase"].BaseValue;
		}

		// Token: 0x0600760C RID: 30220 RVA: 0x00294E87 File Offset: 0x00293087
		protected override void AfterDowngraded()
		{
			base.AfterDowngraded();
			base.DynamicVars.Damage.BaseValue += this.ExtraDamageFromPlays;
		}

		// Token: 0x0600760D RID: 30221 RVA: 0x00294EB0 File Offset: 0x002930B0
		protected override void OnUpgrade()
		{
			base.DynamicVars["Increase"].UpgradeValueBy(4m);
		}

		// Token: 0x040028B4 RID: 10420
		private const string _increaseKey = "Increase";

		// Token: 0x040028B5 RID: 10421
		private decimal _extraDamageFromPlays;
	}
}
