using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x0200081F RID: 2079
	[NullableContext(1)]
	[Nullable(0)]
	public class NightTerrors : ModifierModel
	{
		// Token: 0x0600674B RID: 26443 RVA: 0x002722E0 File Offset: 0x002704E0
		public override decimal ModifyRestSiteHealAmount(Creature creature, decimal amount)
		{
			return creature.MaxHp;
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x002722F0 File Offset: 0x002704F0
		public override async Task AfterRestSiteHeal(Player player, bool isMimicked)
		{
			if (!isMimicked)
			{
				int num = Mathf.Min(5, player.Creature.MaxHp - 1);
				await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), player.Creature, num, false);
			}
		}

		// Token: 0x0600674D RID: 26445 RVA: 0x0027233C File Offset: 0x0027053C
		public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(Player player, IReadOnlyList<LocString> currentExtraText)
		{
			LocString additionalRestSiteHealText = base.AdditionalRestSiteHealText;
			additionalRestSiteHealText.Add("Heal", player.Creature.MaxHp);
			additionalRestSiteHealText.Add("MaxHpLoss", 5m);
			int num = 0;
			LocString[] array = new LocString[1 + currentExtraText.Count];
			foreach (LocString locString in currentExtraText)
			{
				array[num] = locString;
				num++;
			}
			array[num] = additionalRestSiteHealText;
			return new <>z__ReadOnlyArray<LocString>(array);
		}

		// Token: 0x04002752 RID: 10066
		private const int _maxHpLoss = 5;
	}
}
