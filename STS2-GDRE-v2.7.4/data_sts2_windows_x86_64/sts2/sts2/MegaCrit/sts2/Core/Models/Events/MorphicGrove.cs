using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000842 RID: 2114
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MorphicGrove : EventModel
	{
		// Token: 0x17001988 RID: 6536
		// (get) Token: 0x0600682E RID: 26670 RVA: 0x00275E87 File Offset: 0x00274087
		public override bool IsShared
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x0600682F RID: 26671 RVA: 0x00275E8A File Offset: 0x0027408A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(5m));
			}
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x00275E9C File Offset: 0x0027409C
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>(delegate(Player p)
			{
				if (p.Gold >= 100)
				{
					return p.Deck.Cards.Count<CardModel>((CardModel c) => c.IsTransformable) >= 2;
				}
				return false;
			});
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x00275EC8 File Offset: 0x002740C8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Group), "MORPHIC_GROVE.pages.INITIAL.options.GROUP", new IHoverTip[] { HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()) }),
				new EventOption(this, new Func<Task>(this.Loner), "MORPHIC_GROVE.pages.INITIAL.options.LONER", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x00275F30 File Offset: 0x00274130
		private async Task Loner()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
			base.SetEventFinished(base.L10NLookup("MORPHIC_GROVE.pages.LONER.description"));
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x00275F74 File Offset: 0x00274174
		private async Task Group()
		{
			await PlayerCmd.LoseGold(base.Owner.Gold, base.Owner, GoldLossType.Stolen);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 2);
			List<CardModel> list = (await CardSelectCmd.FromDeckForTransformation(base.Owner, cardSelectorPrefs, null)).ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				await CardCmd.TransformToRandom(cardModel, base.Rng, CardPreviewStyle.EventLayout);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			base.SetEventFinished(base.L10NLookup("MORPHIC_GROVE.pages.GROUP.description"));
		}

		// Token: 0x0400278F RID: 10127
		private const int _transformCount = 2;
	}
}
