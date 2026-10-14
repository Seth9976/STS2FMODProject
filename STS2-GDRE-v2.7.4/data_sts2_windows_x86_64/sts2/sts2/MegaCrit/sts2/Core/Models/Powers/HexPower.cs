using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Afflictions;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000694 RID: 1684
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HexPower : PowerModel
	{
		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x060058E8 RID: 22760 RVA: 0x0024F776 File Offset: 0x0024D976
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x060058E9 RID: 22761 RVA: 0x0024F779 File Offset: 0x0024D979
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x060058EA RID: 22762 RVA: 0x0024F77C File Offset: 0x0024D97C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromAffliction<Hexed>(base.Amount);
			}
		}

		/// <summary>
		/// Ethereal is granted globally for as long as this power exists, gated on the card being afflicted with
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Afflictions.Hexed" />. When this power is removed, it stops contributing here automatically, so any Ethereal from
		/// another source (e.g. <see cref="T:MegaCrit.Sts2.Core.Models.Relics.MusicBox" />) is left untouched.
		/// </summary>
		// Token: 0x060058EB RID: 22763 RVA: 0x0024F789 File Offset: 0x0024D989
		public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
		{
			return card.Owner == base.Owner.Player && card.Affliction is Hexed && keywords.Add(CardKeyword.Ethereal);
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x0024F7B8 File Offset: 0x0024D9B8
		[NullableContext(2)]
		[return: Nullable(1)]
		public override async Task AfterApplied(Creature applier, CardModel cardSource)
		{
			foreach (CardModel cardModel in base.Owner.Player.PlayerCombatState.AllCards)
			{
				await this.Afflict(cardModel);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x0024F7FC File Offset: 0x0024D9FC
		public override async Task AfterCardEnteredCombat(CardModel card)
		{
			if (card.Owner == base.Owner.Player)
			{
				await this.Afflict(card);
			}
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x0024F848 File Offset: 0x0024DA48
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Applier)
				{
					await PowerCmd.Remove(this);
				}
			}
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x0024F89C File Offset: 0x0024DA9C
		public override Task AfterRemoved(Creature oldOwner)
		{
			foreach (CardModel cardModel in base.Owner.Player.PlayerCombatState.AllCards)
			{
				if (cardModel.Affliction is Hexed)
				{
					CardCmd.ClearAffliction(cardModel);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x0024F90C File Offset: 0x0024DB0C
		private async Task Afflict(CardModel card)
		{
			if (card.Affliction == null)
			{
				await CardCmd.Afflict<Hexed>(card, base.Amount);
			}
		}
	}
}
