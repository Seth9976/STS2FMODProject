using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000598 RID: 1432
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NinjaScroll : RelicModel
	{
		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x060052D2 RID: 21202 RVA: 0x002439AF File Offset: 0x00241BAF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x060052D3 RID: 21203 RVA: 0x002439B2 File Offset: 0x00241BB2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Shivs", 3m));
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x060052D4 RID: 21204 RVA: 0x002439C9 File Offset: 0x00241BC9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Shiv>(false);
			}
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x002439D4 File Offset: 0x00241BD4
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await Shiv.CreateInHand(base.Owner, base.DynamicVars["Shivs"].IntValue, combatState, null);
				}
			}
		}

		// Token: 0x0400247F RID: 9343
		private const string _shivKey = "Shivs";
	}
}
