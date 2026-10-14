using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020008FC RID: 2300
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AllForOne : CardModel
	{
		// Token: 0x06006D9F RID: 28063 RVA: 0x00283ABA File Offset: 0x00281CBA
		public AllForOne()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C3D RID: 7229
		// (get) Token: 0x06006DA0 RID: 28064 RVA: 0x00283AC7 File Offset: 0x00281CC7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x00283ADC File Offset: 0x00281CDC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "blunt_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
			IEnumerable<CardModel> enumerable = PileType.Discard.GetPile(base.Owner).Cards.Where<CardModel>(new Func<CardModel, bool>(this.Filter)).ToList<CardModel>();
			foreach (CardModel cardModel in enumerable)
			{
				await CardPileCmd.Add(cardModel, PileType.Hand, CardPilePosition.Bottom, null, false);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x00283B30 File Offset: 0x00281D30
		private bool Filter(CardModel card)
		{
			bool flag = card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX;
			bool flag2 = flag;
			if (flag2)
			{
				CardType type = card.Type;
				bool flag3 = type - CardType.Attack <= 2;
				flag2 = flag3;
			}
			return flag2;
		}

		// Token: 0x06006DA3 RID: 28067 RVA: 0x00283B78 File Offset: 0x00281D78
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
