using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006B6 RID: 1718
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NightmarePower : PowerModel
	{
		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x060059AC RID: 22956 RVA: 0x00250DC3 File Offset: 0x0024EFC3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x060059AD RID: 22957 RVA: 0x00250DC6 File Offset: 0x0024EFC6
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x060059AE RID: 22958 RVA: 0x00250DC9 File Offset: 0x0024EFC9
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x00250DCC File Offset: 0x0024EFCC
		protected override object InitInternalData()
		{
			return new NightmarePower.Data();
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x060059B0 RID: 22960 RVA: 0x00250DD3 File Offset: 0x0024EFD3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("Card", ""));
			}
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x00250DEC File Offset: 0x0024EFEC
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				CardModel card = base.GetInternalData<NightmarePower.Data>().selectedCard;
				for (int i = 0; i < base.Amount; i++)
				{
					CardModel cardModel = card.CreateClone();
					await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
				}
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x00250E38 File Offset: 0x0024F038
		public void SetSelectedCard(CardModel card)
		{
			CardModel cardModel = card.CreateClone();
			CardCmd.ClearAffliction(cardModel);
			base.GetInternalData<NightmarePower.Data>().selectedCard = cardModel;
			((StringVar)base.DynamicVars["Card"]).StringValue = cardModel.Title;
		}

		// Token: 0x04002508 RID: 9480
		private const string _cardKey = "Card";

		// Token: 0x02001C0F RID: 7183
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// This will be null for the moment after this power is applied but before this is set by Nightmare.OnPlay.
			/// For all current use cases, this means we should never see this being null.
			/// However, if we needed to override AfterApplied in here in the future, this would be null in it, so let's
			/// leave this nullable for future-proofing.
			/// </summary>
			// Token: 0x04007014 RID: 28692
			[Nullable(2)]
			public CardModel selectedCard;
		}
	}
}
