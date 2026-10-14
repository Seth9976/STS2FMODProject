using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009E2 RID: 2530
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GlimpseBeyond : CardModel
	{
		// Token: 0x06007266 RID: 29286 RVA: 0x0028D41E File Offset: 0x0028B61E
		public GlimpseBeyond()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001E4D RID: 7757
		// (get) Token: 0x06007267 RID: 29287 RVA: 0x0028D42B File Offset: 0x0028B62B
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001E4E RID: 7758
		// (get) Token: 0x06007268 RID: 29288 RVA: 0x0028D42E File Offset: 0x0028B62E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17001E4F RID: 7759
		// (get) Token: 0x06007269 RID: 29289 RVA: 0x0028D43B File Offset: 0x0028B63B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x17001E50 RID: 7760
		// (get) Token: 0x0600726A RID: 29290 RVA: 0x0028D448 File Offset: 0x0028B648
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x0600726B RID: 29291 RVA: 0x0028D450 File Offset: 0x0028B650
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
				where c != null && c.IsAlive && c.IsPlayer
				select c;
			foreach (Creature creature in enumerable)
			{
				IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(Soul.Create(creature.Player, base.DynamicVars.Cards.IntValue, base.CombatState).ToList<Soul>(), PileType.Draw, base.Owner, CardPilePosition.Random);
				if (LocalContext.IsMe(creature))
				{
					CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.HorizontalLayout);
				}
				creature = null;
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x0600726C RID: 29292 RVA: 0x0028D493 File Offset: 0x0028B693
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
