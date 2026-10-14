using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.CardSelection
{
	// Token: 0x02000D5A RID: 3418
	[NullableContext(1)]
	[Nullable(0)]
	public struct CardSelectorPrefs
	{
		// Token: 0x170025FC RID: 9724
		// (get) Token: 0x06008A2F RID: 35375 RVA: 0x002E422C File Offset: 0x002E242C
		public static LocString TransformSelectionPrompt
		{
			get
			{
				return new LocString("card_selection", "TO_TRANSFORM");
			}
		}

		// Token: 0x170025FD RID: 9725
		// (get) Token: 0x06008A30 RID: 35376 RVA: 0x002E423D File Offset: 0x002E243D
		public static LocString ExhaustSelectionPrompt
		{
			get
			{
				return new LocString("card_selection", "TO_EXHAUST");
			}
		}

		// Token: 0x170025FE RID: 9726
		// (get) Token: 0x06008A31 RID: 35377 RVA: 0x002E424E File Offset: 0x002E244E
		public static LocString RemoveSelectionPrompt
		{
			get
			{
				return new LocString("card_selection", "TO_REMOVE");
			}
		}

		// Token: 0x170025FF RID: 9727
		// (get) Token: 0x06008A32 RID: 35378 RVA: 0x002E425F File Offset: 0x002E245F
		public static LocString EnchantSelectionPrompt
		{
			get
			{
				return new LocString("card_selection", "TO_ENCHANT");
			}
		}

		// Token: 0x17002600 RID: 9728
		// (get) Token: 0x06008A33 RID: 35379 RVA: 0x002E4270 File Offset: 0x002E2470
		public static LocString DiscardSelectionPrompt
		{
			get
			{
				return new LocString("card_selection", "TO_DISCARD");
			}
		}

		// Token: 0x17002601 RID: 9729
		// (get) Token: 0x06008A34 RID: 35380 RVA: 0x002E4281 File Offset: 0x002E2481
		public static LocString UpgradeSelectionPrompt
		{
			get
			{
				return new LocString("card_selection", "TO_UPGRADE");
			}
		}

		// Token: 0x17002602 RID: 9730
		// (get) Token: 0x06008A35 RID: 35381 RVA: 0x002E4292 File Offset: 0x002E2492
		public readonly LocString Prompt { get; }

		// Token: 0x17002603 RID: 9731
		// (get) Token: 0x06008A36 RID: 35382 RVA: 0x002E429A File Offset: 0x002E249A
		public readonly int MinSelect { get; }

		// Token: 0x17002604 RID: 9732
		// (get) Token: 0x06008A37 RID: 35383 RVA: 0x002E42A2 File Offset: 0x002E24A2
		public readonly int MaxSelect { get; }

		// Token: 0x17002605 RID: 9733
		// (get) Token: 0x06008A38 RID: 35384 RVA: 0x002E42AA File Offset: 0x002E24AA
		// (set) Token: 0x06008A39 RID: 35385 RVA: 0x002E42B2 File Offset: 0x002E24B2
		public bool RequireManualConfirmation { readonly get; set; }

		// Token: 0x17002606 RID: 9734
		// (get) Token: 0x06008A3A RID: 35386 RVA: 0x002E42BB File Offset: 0x002E24BB
		// (set) Token: 0x06008A3B RID: 35387 RVA: 0x002E42C3 File Offset: 0x002E24C3
		public bool Cancelable { readonly get; set; }

		/// <summary>
		/// If non-null, then the default sort when displayed in the selector will use this comparer. Otherwise, cards will
		/// be displayed as ordered in the list.
		/// In most cases the order passed to the CardSelectCmd must be stable across machines. Therefore, we have to do any
		/// culture-variant sorts on the frontend only.
		/// This is currently only supported when using FromSimpleGrid.
		/// </summary>
		// Token: 0x17002607 RID: 9735
		// (get) Token: 0x06008A3C RID: 35388 RVA: 0x002E42CC File Offset: 0x002E24CC
		// (set) Token: 0x06008A3D RID: 35389 RVA: 0x002E42D4 File Offset: 0x002E24D4
		[Nullable(new byte[] { 2, 1 })]
		public Comparison<CardModel> Comparison
		{
			[return: Nullable(new byte[] { 2, 1 })]
			readonly get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		/// <summary>
		/// If set, card previews displayed during the selection will be unaffected by any external powers or hooks. This
		/// includes enchantments and afflictions!
		/// Only applies to hand card selections.
		/// </summary>
		// Token: 0x17002608 RID: 9736
		// (get) Token: 0x06008A3E RID: 35390 RVA: 0x002E42DD File Offset: 0x002E24DD
		// (set) Token: 0x06008A3F RID: 35391 RVA: 0x002E42E5 File Offset: 0x002E24E5
		public bool UnpoweredPreviews { readonly get; set; }

		/// <summary>
		/// If set, energy/star cost color highlighting will be suppressed for cards that wouldn't be able to be played due
		/// to insufficient energy/stars.
		/// Only applicable to hand card selection.
		/// </summary>
		// Token: 0x17002609 RID: 9737
		// (get) Token: 0x06008A40 RID: 35392 RVA: 0x002E42EE File Offset: 0x002E24EE
		// (set) Token: 0x06008A41 RID: 35393 RVA: 0x002E42F6 File Offset: 0x002E24F6
		public bool PretendCardsCanBePlayed { readonly get; set; }

		/// <summary>
		/// If set, cards that pass the predicate will glow gold.
		/// Only applicable to hand card selection.
		/// </summary>
		// Token: 0x1700260A RID: 9738
		// (get) Token: 0x06008A42 RID: 35394 RVA: 0x002E42FF File Offset: 0x002E24FF
		// (set) Token: 0x06008A43 RID: 35395 RVA: 0x002E4307 File Offset: 0x002E2507
		[Nullable(new byte[] { 2, 1 })]
		public Func<CardModel, bool> ShouldGlowGold
		{
			[return: Nullable(new byte[] { 2, 1 })]
			readonly get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x06008A44 RID: 35396 RVA: 0x002E4310 File Offset: 0x002E2510
		public CardSelectorPrefs(LocString prompt, int selectCount)
		{
			this = new CardSelectorPrefs(prompt, selectCount, selectCount);
			this.Prompt.Add("Amount", selectCount);
		}

		// Token: 0x06008A45 RID: 35397 RVA: 0x002E4334 File Offset: 0x002E2534
		public CardSelectorPrefs(LocString prompt, int minCount, int maxCount)
		{
			this = default(CardSelectorPrefs);
			this.Prompt = prompt;
			this.Prompt.Add("Amount", maxCount);
			this.Prompt.Add("MinCount", minCount);
			this.Prompt.Add("MaxCount", maxCount);
			this.MaxSelect = maxCount;
			this.MinSelect = minCount;
			this.RequireManualConfirmation = this.MinSelect >= 0 && this.MinSelect != this.MaxSelect;
		}

		// Token: 0x04003011 RID: 12305
		private const string _cardSelectionLocFilePath = "card_selection";
	}
}
