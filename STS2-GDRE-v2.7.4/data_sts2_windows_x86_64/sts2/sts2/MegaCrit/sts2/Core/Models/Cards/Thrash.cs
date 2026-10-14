using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B14 RID: 2836
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Thrash : CardModel
	{
		// Token: 0x060078DB RID: 30939 RVA: 0x0029AAF8 File Offset: 0x00298CF8
		public Thrash()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020F4 RID: 8436
		// (get) Token: 0x060078DC RID: 30940 RVA: 0x0029AB05 File Offset: 0x00298D05
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x170020F5 RID: 8437
		// (get) Token: 0x060078DD RID: 30941 RVA: 0x0029AB12 File Offset: 0x00298D12
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(4m, ValueProp.Move));
			}
		}

		// Token: 0x170020F6 RID: 8438
		// (get) Token: 0x060078DE RID: 30942 RVA: 0x0029AB25 File Offset: 0x00298D25
		// (set) Token: 0x060078DF RID: 30943 RVA: 0x0029AB2D File Offset: 0x00298D2D
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

		// Token: 0x060078E0 RID: 30944 RVA: 0x0029AB3C File Offset: 0x00298D3C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_thrash", null, null)
				.Execute(choiceContext);
			CardPile pile = PileType.Hand.GetPile(base.Owner);
			CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(pile.Cards.Where<CardModel>((CardModel c) => c.Type == CardType.Attack));
			if (cardModel != null)
			{
				decimal num = 0m;
				if (cardModel.DynamicVars.ContainsKey("CalculatedDamage"))
				{
					num = cardModel.DynamicVars.CalculatedDamage.Calculate(null);
				}
				else if (cardModel.DynamicVars.ContainsKey("Damage"))
				{
					num = cardModel.DynamicVars.Damage.BaseValue;
				}
				else if (cardModel.DynamicVars.ContainsKey("OstyDamage"))
				{
					num = cardModel.DynamicVars.OstyDamage.BaseValue;
				}
				else
				{
					Log.Warn(base.Id.Entry + " exhausted attack card " + cardModel.Id.Entry + " that did not have an appropriate damage var!", 2);
				}
				IEnumerable<AbstractModel> enumerable;
				num = Hook.ModifyDamage(base.Owner.RunState, base.Owner.Creature.CombatState, null, base.Owner.Creature, num, ValueProp.Move, cardModel, null, ModifyDamageHookType.All, CardPreviewMode.None, out enumerable);
				base.DynamicVars.Damage.BaseValue += num;
				this.ExtraDamage += num;
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
		}

		// Token: 0x060078E1 RID: 30945 RVA: 0x0029AB8F File Offset: 0x00298D8F
		protected override void AfterDowngraded()
		{
			base.AfterDowngraded();
			base.DynamicVars.Damage.BaseValue += this.ExtraDamage;
		}

		// Token: 0x060078E2 RID: 30946 RVA: 0x0029ABB8 File Offset: 0x00298DB8
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}

		// Token: 0x040028D5 RID: 10453
		private decimal _extraDamage;
	}
}
