using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200082A RID: 2090
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AromaOfChaos : EventModel
	{
		// Token: 0x06006771 RID: 26481 RVA: 0x00272A64 File Offset: 0x00270C64
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.LetGo), "AROMA_OF_CHAOS.pages.INITIAL.options.LET_GO", new IHoverTip[] { HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()) }),
				new EventOption(this, new Func<Task>(this.MaintainControl), "AROMA_OF_CHAOS.pages.INITIAL.options.MAINTAIN_CONTROL", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x06006772 RID: 26482 RVA: 0x00272AC9 File Offset: 0x00270CC9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		// Token: 0x06006773 RID: 26483 RVA: 0x00272AD0 File Offset: 0x00270CD0
		private async Task LetGo()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForTransformation(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), null);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardCmd.TransformToRandom(cardModel, base.Rng, CardPreviewStyle.EventLayout);
			}
			base.SetEventFinished(base.L10NLookup("AROMA_OF_CHAOS.pages.LET_GO.description"));
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x00272B14 File Offset: 0x00270D14
		private async Task MaintainControl()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForUpgrade(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1));
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			LocString locString = base.L10NLookup("AROMA_OF_CHAOS.pages.MAINTAIN_CONTROL.description");
			locString.Add("AromaPrinciple", new LocString("characters", base.Owner.Character.Id.Entry + ".aromaPrinciple"));
			base.SetEventFinished(locString);
		}
	}
}
