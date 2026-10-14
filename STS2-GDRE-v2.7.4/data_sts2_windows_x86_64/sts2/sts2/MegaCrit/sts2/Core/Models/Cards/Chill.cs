using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200094E RID: 2382
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Chill : CardModel
	{
		// Token: 0x06006F39 RID: 28473 RVA: 0x00286F9F File Offset: 0x0028519F
		public Chill()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CE4 RID: 7396
		// (get) Token: 0x06006F3A RID: 28474 RVA: 0x00286FAC File Offset: 0x002851AC
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001CE5 RID: 7397
		// (get) Token: 0x06006F3B RID: 28475 RVA: 0x00286FB4 File Offset: 0x002851B4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<FrostOrb>()
				});
			}
		}

		// Token: 0x06006F3C RID: 28476 RVA: 0x00286FD8 File Offset: 0x002851D8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
			foreach (Creature creature in hittableEnemies)
			{
				await OrbCmd.Channel<FrostOrb>(choiceContext, base.Owner);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x06006F3D RID: 28477 RVA: 0x00287023 File Offset: 0x00285223
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
