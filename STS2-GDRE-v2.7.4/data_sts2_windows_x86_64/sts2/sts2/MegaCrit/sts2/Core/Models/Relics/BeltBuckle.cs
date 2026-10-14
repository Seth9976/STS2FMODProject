using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004FE RID: 1278
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BeltBuckle : RelicModel
	{
		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06004F0F RID: 20239 RVA: 0x0023C99B File Offset: 0x0023AB9B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06004F10 RID: 20240 RVA: 0x0023C99E File Offset: 0x0023AB9E
		// (set) Token: 0x06004F11 RID: 20241 RVA: 0x0023C9A6 File Offset: 0x0023ABA6
		private bool DexterityApplied
		{
			get
			{
				return this._dexterityApplied;
			}
			set
			{
				base.AssertMutable();
				this._dexterityApplied = value;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06004F12 RID: 20242 RVA: 0x0023C9B5 File Offset: 0x0023ABB5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(2m));
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06004F13 RID: 20243 RVA: 0x0023C9C8 File Offset: 0x0023ABC8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x0023C9E8 File Offset: 0x0023ABE8
		public override async Task AfterObtained()
		{
			if (CombatManager.Instance.IsInProgress)
			{
				if (!base.Owner.Potions.Any<PotionModel>())
				{
					await this.ApplyDexterity();
				}
			}
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x0023CA2C File Offset: 0x0023AC2C
		public override async Task BeforeCombatStart()
		{
			this.DexterityApplied = false;
			this.RefreshStatus();
			if (!base.Owner.Potions.Any<PotionModel>())
			{
				await this.ApplyDexterity();
			}
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x0023CA6F File Offset: 0x0023AC6F
		public override Task AfterCombatEnd(CombatRoom room)
		{
			this.RefreshStatus();
			return Task.CompletedTask;
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x0023CA7C File Offset: 0x0023AC7C
		public override async Task AfterPotionProcured(PotionModel potion)
		{
			this.RefreshStatus();
			if (CombatManager.Instance.IsInProgress)
			{
				if (base.Owner.Potions.Any<PotionModel>())
				{
					await this.RemoveDexterity();
				}
			}
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x0023CAC0 File Offset: 0x0023ACC0
		public override async Task AfterPotionDiscarded(PotionModel potion)
		{
			this.RefreshStatus();
			if (CombatManager.Instance.IsInProgress)
			{
				if (!base.Owner.Potions.Any<PotionModel>())
				{
					await this.ApplyDexterity();
				}
			}
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x0023CB04 File Offset: 0x0023AD04
		public override async Task AfterPotionUsed(PotionModel potion, [Nullable(2)] Creature target)
		{
			this.RefreshStatus();
			if (CombatManager.Instance.IsInProgress)
			{
				if (!base.Owner.Potions.Any<PotionModel>())
				{
					await this.ApplyDexterity();
				}
			}
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x0023CB47 File Offset: 0x0023AD47
		public override Task AfterCombatVictory(CombatRoom room)
		{
			this.DexterityApplied = false;
			this.RefreshStatus();
			return Task.CompletedTask;
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x0023CB5C File Offset: 0x0023AD5C
		private async Task ApplyDexterity()
		{
			if (!this.DexterityApplied)
			{
				this.DexterityApplied = true;
				base.Flash();
				await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, null, null, false);
			}
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x0023CBA0 File Offset: 0x0023ADA0
		private async Task RemoveDexterity()
		{
			if (this.DexterityApplied)
			{
				this.DexterityApplied = false;
				base.Flash();
				await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, -base.DynamicVars.Dexterity.BaseValue, null, null, false);
			}
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x0023CBE3 File Offset: 0x0023ADE3
		private void RefreshStatus()
		{
			if (CombatManager.Instance.IsInProgress && !base.Owner.Potions.Any<PotionModel>())
			{
				base.Status = RelicStatus.Active;
				return;
			}
			base.Status = RelicStatus.Normal;
		}

		// Token: 0x04002425 RID: 9253
		private bool _dexterityApplied;
	}
}
