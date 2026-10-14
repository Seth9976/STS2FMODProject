using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Afflictions
{
	/// <summary>
	/// Most of this Affliction's logic lives in <see cref="T:MegaCrit.Sts2.Core.Models.Powers.TaintedPower" />.
	/// </summary>
	// Token: 0x02000B7C RID: 2940
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tainted : AfflictionModel
	{
		// Token: 0x170021F7 RID: 8695
		// (get) Token: 0x06007B10 RID: 31504 RVA: 0x002A030C File Offset: 0x0029E50C
		public override bool IsStackable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021F8 RID: 8696
		// (get) Token: 0x06007B11 RID: 31505 RVA: 0x002A030F File Offset: 0x0029E50F
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021F9 RID: 8697
		// (get) Token: 0x06007B12 RID: 31506 RVA: 0x002A0312 File Offset: 0x0029E512
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromPowerWithPowerHoverTips<TaintedPower>(new int?(base.Amount));
			}
		}

		// Token: 0x06007B13 RID: 31507 RVA: 0x002A0324 File Offset: 0x0029E524
		public override bool CanAfflictCardType(CardType cardType)
		{
			return cardType == CardType.Skill;
		}
	}
}
