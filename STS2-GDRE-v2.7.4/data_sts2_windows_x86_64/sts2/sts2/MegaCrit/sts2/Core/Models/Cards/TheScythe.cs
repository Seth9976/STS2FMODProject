using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B10 RID: 2832
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheScythe : CardModel
	{
		// Token: 0x060078C0 RID: 30912 RVA: 0x0029A857 File Offset: 0x00298A57
		public TheScythe()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020EA RID: 8426
		// (get) Token: 0x060078C1 RID: 30913 RVA: 0x0029A86C File Offset: 0x00298A6C
		// (set) Token: 0x060078C2 RID: 30914 RVA: 0x0029A874 File Offset: 0x00298A74
		[SavedProperty]
		public int CurrentDamage
		{
			get
			{
				return this._currentDamage;
			}
			set
			{
				base.AssertMutable();
				this._currentDamage = value;
				base.DynamicVars.Damage.BaseValue = this._currentDamage;
			}
		}

		// Token: 0x170020EB RID: 8427
		// (get) Token: 0x060078C3 RID: 30915 RVA: 0x0029A89E File Offset: 0x00298A9E
		// (set) Token: 0x060078C4 RID: 30916 RVA: 0x0029A8A6 File Offset: 0x00298AA6
		[SavedProperty]
		public int IncreasedDamage
		{
			get
			{
				return this._increasedDamage;
			}
			set
			{
				base.AssertMutable();
				this._increasedDamage = value;
			}
		}

		// Token: 0x170020EC RID: 8428
		// (get) Token: 0x060078C5 RID: 30917 RVA: 0x0029A8B5 File Offset: 0x00298AB5
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x170020ED RID: 8429
		// (get) Token: 0x060078C6 RID: 30918 RVA: 0x0029A8BD File Offset: 0x00298ABD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(this.CurrentDamage, ValueProp.Move),
					new IntVar("Increase", 4m)
				});
			}
		}

		// Token: 0x060078C7 RID: 30919 RVA: 0x0029A8F4 File Offset: 0x00298AF4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			int intValue = base.DynamicVars["Increase"].IntValue;
			this.BuffFromPlay(intValue);
			TheScythe theScythe = base.DeckVersion as TheScythe;
			if (theScythe != null)
			{
				theScythe.BuffFromPlay(intValue);
			}
		}

		// Token: 0x060078C8 RID: 30920 RVA: 0x0029A947 File Offset: 0x00298B47
		protected override void OnUpgrade()
		{
			base.DynamicVars["Increase"].UpgradeValueBy(1m);
		}

		// Token: 0x060078C9 RID: 30921 RVA: 0x0029A963 File Offset: 0x00298B63
		protected override void AfterDowngraded()
		{
			this.UpdateDamage();
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x0029A96B File Offset: 0x00298B6B
		private void BuffFromPlay(int extraDamage)
		{
			this.IncreasedDamage += extraDamage;
			this.UpdateDamage();
		}

		// Token: 0x060078CB RID: 30923 RVA: 0x0029A981 File Offset: 0x00298B81
		private void UpdateDamage()
		{
			this.CurrentDamage = 13 + this.IncreasedDamage;
		}

		// Token: 0x040028D1 RID: 10449
		private const string _increaseKey = "Increase";

		// Token: 0x040028D2 RID: 10450
		private const int _baseDamage = 13;

		// Token: 0x040028D3 RID: 10451
		private int _currentDamage = 13;

		// Token: 0x040028D4 RID: 10452
		private int _increasedDamage;
	}
}
