using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A96 RID: 2710
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Reave : CardModel
	{
		// Token: 0x06007623 RID: 30243 RVA: 0x0029519C File Offset: 0x0029339C
		public Reave()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FD8 RID: 8152
		// (get) Token: 0x06007624 RID: 30244 RVA: 0x002951A9 File Offset: 0x002933A9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x17001FD9 RID: 8153
		// (get) Token: 0x06007625 RID: 30245 RVA: 0x002951CF File Offset: 0x002933CF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(base.IsUpgraded));
			}
		}

		// Token: 0x06007626 RID: 30246 RVA: 0x002951E4 File Offset: 0x002933E4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			IEnumerable<Soul> enumerable = Soul.Create(base.Owner, base.DynamicVars.Cards.IntValue, base.CombatState);
			if (base.IsUpgraded)
			{
				foreach (Soul soul in enumerable)
				{
					CardCmd.Upgrade(soul, CardPreviewStyle.HorizontalLayout);
				}
			}
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(enumerable, PileType.Draw, base.Owner, CardPilePosition.Random);
			CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x06007627 RID: 30247 RVA: 0x00295237 File Offset: 0x00293437
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
