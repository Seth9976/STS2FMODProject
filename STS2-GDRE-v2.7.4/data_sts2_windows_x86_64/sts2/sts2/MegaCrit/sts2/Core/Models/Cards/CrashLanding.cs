using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000969 RID: 2409
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrashLanding : CardModel
	{
		// Token: 0x06006FCC RID: 28620 RVA: 0x002881F4 File Offset: 0x002863F4
		public CrashLanding()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001D25 RID: 7461
		// (get) Token: 0x06006FCD RID: 28621 RVA: 0x00288201 File Offset: 0x00286401
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Debris>(false));
			}
		}

		// Token: 0x17001D26 RID: 7462
		// (get) Token: 0x06006FCE RID: 28622 RVA: 0x0028820E File Offset: 0x0028640E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(21m, ValueProp.Move));
			}
		}

		// Token: 0x06006FCF RID: 28623 RVA: 0x00288224 File Offset: 0x00286424
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
			int num = CardPile.MaxCardsInHand - CardPile.GetCards(base.Owner, new PileType[] { PileType.Hand }).Count<CardModel>();
			List<CardModel> list = new List<CardModel>();
			for (int i = 0; i < num; i++)
			{
				list.Add(base.CombatState.CreateCard<Debris>(base.Owner));
			}
			await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner, CardPilePosition.Bottom);
		}

		// Token: 0x06006FD0 RID: 28624 RVA: 0x00288277 File Offset: 0x00286477
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
