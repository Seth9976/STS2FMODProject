using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200069B RID: 1691
	public sealed class ImprovementPower : PowerModel
	{
		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x0600591B RID: 22811 RVA: 0x0024FDBF File Offset: 0x0024DFBF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x0600591C RID: 22812 RVA: 0x0024FDC2 File Offset: 0x0024DFC2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x0024FDC8 File Offset: 0x0024DFC8
		[NullableContext(1)]
		public override Task AfterCombatEnd(CombatRoom room)
		{
			List<CardModel> list = PileType.Deck.GetPile(base.Owner.Player).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>();
			int num = 0;
			while (num < base.Amount && list.Count != 0)
			{
				CardModel cardModel = base.Owner.Player.RunState.Rng.CombatCardSelection.NextItem<CardModel>(list);
				list.Remove(cardModel);
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				num++;
			}
			return Task.CompletedTask;
		}
	}
}
