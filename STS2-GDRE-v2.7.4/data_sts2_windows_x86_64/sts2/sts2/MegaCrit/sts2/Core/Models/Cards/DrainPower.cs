using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200099B RID: 2459
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DrainPower : CardModel
	{
		// Token: 0x060070D9 RID: 28889 RVA: 0x0028A1CB File Offset: 0x002883CB
		public DrainPower()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DA2 RID: 7586
		// (get) Token: 0x060070DA RID: 28890 RVA: 0x0028A1D8 File Offset: 0x002883D8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x060070DB RID: 28891 RVA: 0x0028A200 File Offset: 0x00288400
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			IEnumerable<CardModel> enumerable = PileType.Discard.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).TakeRandom<CardModel>(base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardSelection);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x060070DC RID: 28892 RVA: 0x0028A253 File Offset: 0x00288453
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
