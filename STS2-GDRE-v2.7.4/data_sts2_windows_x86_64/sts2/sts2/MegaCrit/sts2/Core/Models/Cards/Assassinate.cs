using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000906 RID: 2310
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Assassinate : CardModel
	{
		// Token: 0x06006DCF RID: 28111 RVA: 0x00284096 File Offset: 0x00282296
		public Assassinate()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C51 RID: 7249
		// (get) Token: 0x06006DD0 RID: 28112 RVA: 0x002840A3 File Offset: 0x002822A3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new PowerVar<VulnerablePower>(1m)
				});
			}
		}

		// Token: 0x17001C52 RID: 7250
		// (get) Token: 0x06006DD1 RID: 28113 RVA: 0x002840D0 File Offset: 0x002822D0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x17001C53 RID: 7251
		// (get) Token: 0x06006DD2 RID: 28114 RVA: 0x002840F0 File Offset: 0x002822F0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Innate,
					CardKeyword.Exhaust
				});
			}
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x00284108 File Offset: 0x00282308
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_dramatic_stab", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x0028415B File Offset: 0x0028235B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
			base.DynamicVars.Vulnerable.UpgradeValueBy(1m);
		}
	}
}
