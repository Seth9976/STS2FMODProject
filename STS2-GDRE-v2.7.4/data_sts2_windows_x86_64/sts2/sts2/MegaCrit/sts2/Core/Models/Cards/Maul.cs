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
	// Token: 0x02000A37 RID: 2615
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Maul : CardModel
	{
		// Token: 0x06007432 RID: 29746 RVA: 0x00291039 File Offset: 0x0028F239
		public Maul()
			: base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F0D RID: 7949
		// (get) Token: 0x06007433 RID: 29747 RVA: 0x00291046 File Offset: 0x0028F246
		// (set) Token: 0x06007434 RID: 29748 RVA: 0x0029104E File Offset: 0x0028F24E
		private decimal ExtraDamageFromMaulPlays
		{
			get
			{
				return this._extraDamageFromMaulPlays;
			}
			set
			{
				base.AssertMutable();
				this._extraDamageFromMaulPlays = value;
			}
		}

		// Token: 0x17001F0E RID: 7950
		// (get) Token: 0x06007435 RID: 29749 RVA: 0x0029105D File Offset: 0x0028F25D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(5m, ValueProp.Move),
					new DynamicVar("Increase", 1m)
				});
			}
		}

		// Token: 0x06007436 RID: 29750 RVA: 0x0029108C File Offset: 0x0028F28C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitVfxNode((Creature t) => NScratchVfx.Create(t, true))
				.Execute(choiceContext);
			IEnumerable<Maul> enumerable = base.Owner.PlayerCombatState.AllCards.OfType<Maul>();
			decimal baseValue = base.DynamicVars["Increase"].BaseValue;
			foreach (Maul maul in enumerable)
			{
				maul.BuffFromMaulPlay(baseValue);
			}
		}

		// Token: 0x06007437 RID: 29751 RVA: 0x002910DF File Offset: 0x0028F2DF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars["Increase"].UpgradeValueBy(1m);
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x00291110 File Offset: 0x0028F310
		protected override void AfterDowngraded()
		{
			base.AfterDowngraded();
			base.DynamicVars.Damage.BaseValue += this.ExtraDamageFromMaulPlays;
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x00291139 File Offset: 0x0028F339
		private void BuffFromMaulPlay(decimal extraDamage)
		{
			base.DynamicVars.Damage.BaseValue += extraDamage;
			this.ExtraDamageFromMaulPlays += extraDamage;
		}

		// Token: 0x040028A1 RID: 10401
		private const string _increaseKey = "Increase";

		// Token: 0x040028A2 RID: 10402
		private decimal _extraDamageFromMaulPlays;
	}
}
