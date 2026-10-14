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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000951 RID: 2385
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Claw : CardModel
	{
		// Token: 0x06006F4A RID: 28490 RVA: 0x0028719F File Offset: 0x0028539F
		public Claw()
			: base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CEC RID: 7404
		// (get) Token: 0x06006F4B RID: 28491 RVA: 0x002871AC File Offset: 0x002853AC
		// (set) Token: 0x06006F4C RID: 28492 RVA: 0x002871B4 File Offset: 0x002853B4
		private decimal ExtraDamageFromClawPlays
		{
			get
			{
				return this._extraDamageFromClawPlays;
			}
			set
			{
				base.AssertMutable();
				this._extraDamageFromClawPlays = value;
			}
		}

		// Token: 0x17001CED RID: 7405
		// (get) Token: 0x06006F4D RID: 28493 RVA: 0x002871C3 File Offset: 0x002853C3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(3m, ValueProp.Move),
					new DynamicVar("Increase", 2m)
				});
			}
		}

		// Token: 0x06006F4E RID: 28494 RVA: 0x002871F4 File Offset: 0x002853F4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitVfxNode((Creature t) => NScratchVfx.Create(t, true))
				.Execute(choiceContext);
			IEnumerable<Claw> enumerable = base.Owner.PlayerCombatState.AllCards.OfType<Claw>();
			decimal baseValue = base.DynamicVars["Increase"].BaseValue;
			foreach (Claw claw in enumerable)
			{
				claw.BuffFromClawPlay(baseValue);
			}
		}

		// Token: 0x06006F4F RID: 28495 RVA: 0x00287247 File Offset: 0x00285447
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars["Increase"].UpgradeValueBy(1m);
		}

		// Token: 0x06006F50 RID: 28496 RVA: 0x00287278 File Offset: 0x00285478
		protected override void AfterDowngraded()
		{
			base.AfterDowngraded();
			base.DynamicVars.Damage.BaseValue += this.ExtraDamageFromClawPlays;
		}

		// Token: 0x06006F51 RID: 28497 RVA: 0x002872A1 File Offset: 0x002854A1
		private void BuffFromClawPlay(decimal extraDamage)
		{
			base.DynamicVars.Damage.BaseValue += extraDamage;
			this.ExtraDamageFromClawPlays += extraDamage;
		}

		// Token: 0x04002853 RID: 10323
		private const string _increaseKey = "Increase";

		// Token: 0x04002854 RID: 10324
		private decimal _extraDamageFromClawPlays;
	}
}
