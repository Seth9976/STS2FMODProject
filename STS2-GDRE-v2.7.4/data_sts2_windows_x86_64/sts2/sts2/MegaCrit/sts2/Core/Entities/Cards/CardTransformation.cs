using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Represents a transformation of one card in a pile to another card which replaces the original card in the same pile.
	/// If Replacement is set, then it will be used as a direct replacement.
	/// If ReplacementOptions is set, then a random card will be chosen from one of those options.
	/// </summary>
	// Token: 0x02000CB9 RID: 3257
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct CardTransformation
	{
		// Token: 0x17002448 RID: 9288
		// (get) Token: 0x060084D2 RID: 34002 RVA: 0x002CAE26 File Offset: 0x002C9026
		public CardModel Original { get; }

		// Token: 0x17002449 RID: 9289
		// (get) Token: 0x060084D3 RID: 34003 RVA: 0x002CAE2E File Offset: 0x002C902E
		[Nullable(2)]
		public CardModel Replacement
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x1700244A RID: 9290
		// (get) Token: 0x060084D4 RID: 34004 RVA: 0x002CAE36 File Offset: 0x002C9036
		[Nullable(new byte[] { 2, 1 })]
		public IEnumerable<CardModel> ReplacementOptions
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
		}

		/// <summary>
		/// Whether the original card is in combat at construction time. Captured here because the card
		/// may be removed from its pile before GetReplacement is called, which would make CombatState null.
		/// </summary>
		// Token: 0x1700244B RID: 9291
		// (get) Token: 0x060084D5 RID: 34005 RVA: 0x002CAE3E File Offset: 0x002C903E
		public bool IsInCombat { get; }

		// Token: 0x060084D6 RID: 34006 RVA: 0x002CAE46 File Offset: 0x002C9046
		public CardTransformation(CardModel original)
		{
			CardTransformation.AssertTransformable(original);
			this.Original = original;
			this.ReplacementOptions = null;
			this.Replacement = null;
			this.IsInCombat = original.CombatState != null;
		}

		// Token: 0x060084D7 RID: 34007 RVA: 0x002CAE72 File Offset: 0x002C9072
		public CardTransformation(CardModel original, IEnumerable<CardModel> options)
		{
			CardTransformation.AssertTransformable(original);
			this.Original = original;
			this.ReplacementOptions = options;
			this.Replacement = null;
			this.IsInCombat = original.CombatState != null;
		}

		// Token: 0x060084D8 RID: 34008 RVA: 0x002CAE9E File Offset: 0x002C909E
		public CardTransformation(CardModel original, CardModel replacement)
		{
			CardTransformation.AssertTransformable(original);
			this.Original = original;
			this.Replacement = replacement;
			this.ReplacementOptions = null;
			this.IsInCombat = original.CombatState != null;
		}

		// Token: 0x060084D9 RID: 34009 RVA: 0x002CAECC File Offset: 0x002C90CC
		[NullableContext(2)]
		public CardModel GetReplacement(Rng rng)
		{
			if (this.Replacement != null)
			{
				return this.Replacement;
			}
			if (rng == null)
			{
				throw new ArgumentException("RNG must be passed when replacement options is set!");
			}
			if (!this.Original.IsTransformable)
			{
				return null;
			}
			if (this.ReplacementOptions == null)
			{
				return CardFactory.CreateRandomCardForTransform(this.Original, this.IsInCombat, rng);
			}
			return CardFactory.CreateRandomCardForTransform(this.Original, this.ReplacementOptions, this.IsInCombat, rng);
		}

		// Token: 0x060084DA RID: 34010 RVA: 0x002CAF38 File Offset: 0x002C9138
		public IEnumerable<CardTransformation> Yield()
		{
			CardTransformation.<Yield>d__16 <Yield>d__ = new CardTransformation.<Yield>d__16(-2);
			<Yield>d__.<>3__<>4__this = this;
			return <Yield>d__;
		}

		// Token: 0x060084DB RID: 34011 RVA: 0x002CAF4D File Offset: 0x002C914D
		private static void AssertTransformable(CardModel card)
		{
			if (!card.IsTransformable)
			{
				throw new InvalidOperationException("Non-removable cards cannot be transformed!");
			}
		}
	}
}
