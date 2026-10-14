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
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000946 RID: 2374
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CaptureSpirit : CardModel
	{
		// Token: 0x06006F15 RID: 28437 RVA: 0x00286B0E File Offset: 0x00284D0E
		public CaptureSpirit()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CD6 RID: 7382
		// (get) Token: 0x06006F16 RID: 28438 RVA: 0x00286B1B File Offset: 0x00284D1B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(3m, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x17001CD7 RID: 7383
		// (get) Token: 0x06006F17 RID: 28439 RVA: 0x00286B41 File Offset: 0x00284D41
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x00286B50 File Offset: 0x00284D50
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.Damage(choiceContext, cardPlay.Target, base.DynamicVars.Damage, this, cardPlay);
			List<Soul> list = Soul.Create(base.Owner, base.DynamicVars.Cards.IntValue, base.CombatState).ToList<Soul>();
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Draw, base.Owner, CardPilePosition.Random);
			CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x06006F19 RID: 28441 RVA: 0x00286BA3 File Offset: 0x00284DA3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
