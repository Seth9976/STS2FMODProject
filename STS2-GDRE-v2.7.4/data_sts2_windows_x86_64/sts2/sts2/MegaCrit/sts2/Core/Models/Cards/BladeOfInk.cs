using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200091D RID: 2333
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BladeOfInk : CardModel
	{
		// Token: 0x06006E45 RID: 28229 RVA: 0x00285072 File Offset: 0x00283272
		public BladeOfInk()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C82 RID: 7298
		// (get) Token: 0x06006E46 RID: 28230 RVA: 0x0028507F File Offset: 0x0028327F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.Add(HoverTipFactory.FromCard<Shiv>(false));
				list.AddRange(HoverTipFactory.FromEnchantment<Inky>(1));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x17001C83 RID: 7299
		// (get) Token: 0x06006E47 RID: 28231 RVA: 0x002850A3 File Offset: 0x002832A3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x002850B0 File Offset: 0x002832B0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> enumerable = await Shiv.CreateInHand(base.Owner, base.DynamicVars.Cards.IntValue, base.CombatState, null);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Enchant<Inky>(cardModel, 1m);
			}
		}

		// Token: 0x06006E49 RID: 28233 RVA: 0x002850F3 File Offset: 0x002832F3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
