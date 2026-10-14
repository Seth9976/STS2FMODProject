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
	// Token: 0x02000A1E RID: 2590
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KinglyPunch : CardModel
	{
		// Token: 0x0600739C RID: 29596 RVA: 0x0028FB11 File Offset: 0x0028DD11
		public KinglyPunch()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001ECF RID: 7887
		// (get) Token: 0x0600739D RID: 29597 RVA: 0x0028FB1E File Offset: 0x0028DD1E
		// (set) Token: 0x0600739E RID: 29598 RVA: 0x0028FB26 File Offset: 0x0028DD26
		private decimal ExtraDamage
		{
			get
			{
				return this._extraDamage;
			}
			set
			{
				base.AssertMutable();
				this._extraDamage = value;
			}
		}

		// Token: 0x17001ED0 RID: 7888
		// (get) Token: 0x0600739F RID: 29599 RVA: 0x0028FB35 File Offset: 0x0028DD35
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(8m, ValueProp.Move),
					new DynamicVar("Increase", 4m)
				});
			}
		}

		// Token: 0x060073A0 RID: 29600 RVA: 0x0028FB64 File Offset: 0x0028DD64
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x060073A1 RID: 29601 RVA: 0x0028FBB8 File Offset: 0x0028DDB8
		public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card != this)
			{
				return Task.CompletedTask;
			}
			decimal baseValue = base.DynamicVars["Increase"].BaseValue;
			base.DynamicVars.Damage.BaseValue += baseValue;
			this.ExtraDamage += baseValue;
			return Task.CompletedTask;
		}

		// Token: 0x060073A2 RID: 29602 RVA: 0x0028FC18 File Offset: 0x0028DE18
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars["Increase"].UpgradeValueBy(2m);
		}

		// Token: 0x060073A3 RID: 29603 RVA: 0x0028FC4B File Offset: 0x0028DE4B
		protected override void AfterDowngraded()
		{
			base.AfterDowngraded();
			base.DynamicVars.Damage.BaseValue += this.ExtraDamage;
		}

		// Token: 0x04002882 RID: 10370
		private const string _increaseKey = "Increase";

		// Token: 0x04002883 RID: 10371
		private decimal _extraDamage;
	}
}
