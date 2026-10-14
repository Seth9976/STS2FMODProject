using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000582 RID: 1410
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Mango : RelicModel
	{
		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06005255 RID: 21077 RVA: 0x00242AC7 File Offset: 0x00240CC7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06005256 RID: 21078 RVA: 0x00242ACA File Offset: 0x00240CCA
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06005257 RID: 21079 RVA: 0x00242ACD File Offset: 0x00240CCD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(14m));
			}
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x00242AE0 File Offset: 0x00240CE0
		public override async Task AfterObtained()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
