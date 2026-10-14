using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000950 RID: 2384
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Clash : CardModel
	{
		// Token: 0x06006F43 RID: 28483 RVA: 0x002870C7 File Offset: 0x002852C7
		public Clash()
			: base(0, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CE8 RID: 7400
		// (get) Token: 0x06006F44 RID: 28484 RVA: 0x002870D4 File Offset: 0x002852D4
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<IroncladCardPool>();
			}
		}

		// Token: 0x17001CE9 RID: 7401
		// (get) Token: 0x06006F45 RID: 28485 RVA: 0x002870DB File Offset: 0x002852DB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(14m, ValueProp.Move));
			}
		}

		// Token: 0x17001CEA RID: 7402
		// (get) Token: 0x06006F46 RID: 28486 RVA: 0x002870EF File Offset: 0x002852EF
		protected override bool IsPlayable
		{
			get
			{
				return CardPile.GetCards(base.Owner, new PileType[] { PileType.Hand }).All<CardModel>((CardModel c) => c.Type == CardType.Attack);
			}
		}

		// Token: 0x17001CEB RID: 7403
		// (get) Token: 0x06006F47 RID: 28487 RVA: 0x0028712A File Offset: 0x0028532A
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.IsPlayable;
			}
		}

		// Token: 0x06006F48 RID: 28488 RVA: 0x00287134 File Offset: 0x00285334
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06006F49 RID: 28489 RVA: 0x00287187 File Offset: 0x00285387
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
