using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005ED RID: 1517
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StoneHumidifier : RelicModel
	{
		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x0600552B RID: 21803 RVA: 0x0024818B File Offset: 0x0024638B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x0600552C RID: 21804 RVA: 0x0024818E File Offset: 0x0024638E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(5m));
			}
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x002481A0 File Offset: 0x002463A0
		public override async Task AfterRestSiteHeal(Player player, bool isMimicked)
		{
			if (player == base.Owner)
			{
				base.Flash();
				await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
			}
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x002481EC File Offset: 0x002463EC
		public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(Player player, IReadOnlyList<LocString> currentExtraText)
		{
			if (!LocalContext.IsMe(base.Owner))
			{
				return currentExtraText;
			}
			int num = 0;
			LocString[] array = new LocString[1 + currentExtraText.Count];
			foreach (LocString locString in currentExtraText)
			{
				array[num] = locString;
				num++;
			}
			array[num] = base.AdditionalRestSiteHealText;
			return new <>z__ReadOnlyArray<LocString>(array);
		}
	}
}
