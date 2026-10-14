using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200066D RID: 1645
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EntropyPower : PowerModel
	{
		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x0600581C RID: 22556 RVA: 0x0024E163 File Offset: 0x0024C363
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x0600581D RID: 22557 RVA: 0x0024E166 File Offset: 0x0024C366
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x0600581E RID: 22558 RVA: 0x0024E169 File Offset: 0x0024C369
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x0024E17C File Offset: 0x0024C37C
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, base.Amount);
				IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, player, cardSelectorPrefs, null, this);
				List<CardModel> list = enumerable.ToList<CardModel>();
				foreach (CardModel cardModel in list)
				{
					await CardCmd.TransformToRandom(cardModel, player.RunState.Rng.CombatCardSelection, CardPreviewStyle.HorizontalLayout);
				}
				List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			}
		}
	}
}
