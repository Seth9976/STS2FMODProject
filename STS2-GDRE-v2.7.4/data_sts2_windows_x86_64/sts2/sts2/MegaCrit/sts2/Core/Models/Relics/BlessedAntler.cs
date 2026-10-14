using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000505 RID: 1285
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlessedAntler : RelicModel
	{
		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06004F3D RID: 20285 RVA: 0x0023CF52 File Offset: 0x0023B152
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06004F3E RID: 20286 RVA: 0x0023CF55 File Offset: 0x0023B155
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06004F3F RID: 20287 RVA: 0x0023CF74 File Offset: 0x0023B174
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.Add(HoverTipFactory.ForEnergy(this));
				list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Dazed>(false));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x0023CF98 File Offset: 0x0023B198
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.IntValue;
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x0023CFC0 File Offset: 0x0023B1C0
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					base.Flash();
					List<CardModel> list = new List<CardModel>();
					for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
					{
						list.Add(combatState.CreateCard<Dazed>(base.Owner));
					}
					IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Draw, base.Owner, CardPilePosition.Random);
					IReadOnlyList<CardPileAddResult> readOnlyList2 = readOnlyList;
					CardCmd.PreviewCardPileAdd(readOnlyList2, 1.2f, CardPreviewStyle.HorizontalLayout);
					await Cmd.Wait(3f, false);
				}
			}
		}
	}
}
