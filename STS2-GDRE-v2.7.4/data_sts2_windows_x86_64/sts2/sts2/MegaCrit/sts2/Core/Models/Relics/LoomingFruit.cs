using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200057C RID: 1404
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LoomingFruit : RelicModel
	{
		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06005238 RID: 21048 RVA: 0x002427DF File Offset: 0x002409DF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x002427E2 File Offset: 0x002409E2
		protected override string IconBaseName
		{
			get
			{
				if (!LoomingFruit.HasCornucopia())
				{
					return base.IconBaseName + "_2";
				}
				return base.IconBaseName;
			}
		}

		/// <summary>
		/// 50% chance of Looming Fruit having the cornucopia. This chance is LOCKED for each player indefinitely.
		/// Players will only know if there's a cornucopia or not by seeing OTHER player's Looming Fruit.
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600523A RID: 21050 RVA: 0x00242804 File Offset: 0x00240A04
		private static bool HasCornucopia()
		{
			string uniqueId = SaveManager.Instance.Progress.UniqueId;
			if (string.IsNullOrEmpty(uniqueId))
			{
				return false;
			}
			string text = uniqueId;
			return text[text.Length - 1] % '\u0002' == '\0';
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x0600523B RID: 21051 RVA: 0x0024283E File Offset: 0x00240A3E
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x00242841 File Offset: 0x00240A41
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(31m));
			}
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x00242854 File Offset: 0x00240A54
		public override async Task AfterObtained()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
