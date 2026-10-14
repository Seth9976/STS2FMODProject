using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004EA RID: 1258
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PotionPoolModel : AbstractModel, IPoolModel
	{
		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06004DF6 RID: 19958
		public abstract string EnergyColorName { get; }

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x0022F7B8 File Offset: 0x0022D9B8
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x0022F7BB File Offset: 0x0022D9BB
		public virtual Color LabOutlineColor
		{
			get
			{
				return StsColors.halfTransparentBlack;
			}
		}

		/// <summary>
		/// Get every potion in this pool (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x0022F7C2 File Offset: 0x0022D9C2
		public IEnumerable<PotionModel> AllPotions
		{
			get
			{
				if (this._allPotions == null)
				{
					this._allPotions = this.GenerateAllPotions();
					this._allPotions = ModHelper.ConcatModelsFromMods<PotionModel>(this, this._allPotions);
				}
				return this._allPotions;
			}
		}

		/// <summary>
		/// Get the ID of every potion in this pool (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06004DFA RID: 19962 RVA: 0x0022F7F0 File Offset: 0x0022D9F0
		public IEnumerable<ModelId> AllPotionIds
		{
			get
			{
				HashSet<ModelId> hashSet;
				if ((hashSet = this._allPotionIds) == null)
				{
					hashSet = (this._allPotionIds = this.AllPotions.Select<PotionModel, ModelId>((PotionModel p) => p.Id).ToHashSet<ModelId>());
				}
				return hashSet;
			}
		}

		/// <summary>
		/// Generates every potion in this pool (ignores Unlocks/Epoch state).
		/// Overriden in subclasses, but should only be called once by <see cref="P:MegaCrit.Sts2.Core.Models.PotionPoolModel.AllPotions" /> so it can be cached.
		/// </summary>
		// Token: 0x06004DFB RID: 19963
		protected abstract IEnumerable<PotionModel> GenerateAllPotions();

		/// <summary>
		/// Returns every potion in this pool that the player has unlocked.
		/// By default, this is just AllPotions, but can be overriden in subclasses to remove potions that should be locked
		/// under certain conditions.
		/// </summary>
		// Token: 0x06004DFC RID: 19964 RVA: 0x0022F83F File Offset: 0x0022DA3F
		public virtual IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
		{
			return this.AllPotions;
		}

		// Token: 0x04001D78 RID: 7544
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<PotionModel> _allPotions;

		// Token: 0x04001D79 RID: 7545
		[Nullable(new byte[] { 2, 1 })]
		private HashSet<ModelId> _allPotionIds;
	}
}
