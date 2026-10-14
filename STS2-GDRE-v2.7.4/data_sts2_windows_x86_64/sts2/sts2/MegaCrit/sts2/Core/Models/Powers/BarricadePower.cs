using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000633 RID: 1587
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BarricadePower : PowerModel
	{
		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x060056D0 RID: 22224 RVA: 0x0024BC5B File Offset: 0x00249E5B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x060056D1 RID: 22225 RVA: 0x0024BC5E File Offset: 0x00249E5E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x060056D2 RID: 22226 RVA: 0x0024BC61 File Offset: 0x00249E61
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("ApplierName", ""));
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x060056D3 RID: 22227 RVA: 0x0024BC77 File Offset: 0x00249E77
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060056D4 RID: 22228 RVA: 0x0024BC8C File Offset: 0x00249E8C
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			Creature applier2 = base.Applier;
			if (applier2 != null && applier2.IsMonster)
			{
				((StringVar)base.DynamicVars["ApplierName"]).StringValue = base.Applier.Monster.Title.GetFormattedText();
			}
			return Task.CompletedTask;
		}

		// Token: 0x060056D5 RID: 22229 RVA: 0x0024BCDF File Offset: 0x00249EDF
		public override bool ShouldClearBlock(Creature creature)
		{
			return base.Owner != creature;
		}

		// Token: 0x040024F1 RID: 9457
		private const string _applierNameKey = "ApplierName";
	}
}
