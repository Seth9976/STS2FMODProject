using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Afflictions
{
	/// <summary>
	/// Most of this Affliction's logic lives in <see cref="T:MegaCrit.Sts2.Core.Models.Powers.HexPower" />.
	/// </summary>
	// Token: 0x02000B79 RID: 2937
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hexed : AfflictionModel
	{
		// Token: 0x170021F5 RID: 8693
		// (get) Token: 0x06007B0A RID: 31498 RVA: 0x002A02AB File Offset: 0x0029E4AB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x06007B0B RID: 31499 RVA: 0x002A02B8 File Offset: 0x0029E4B8
		public override Task AfterCardEnteredCombat(CardModel card)
		{
			if (card != base.Card)
			{
				return Task.CompletedTask;
			}
			if (card.Owner.Creature.HasPower<HexPower>())
			{
				return Task.CompletedTask;
			}
			CardCmd.ClearAffliction(base.Card);
			return Task.CompletedTask;
		}
	}
}
