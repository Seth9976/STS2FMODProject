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
	// Token: 0x020004ED RID: 1261
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class RelicPoolModel : AbstractModel, IPoolModel
	{
		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06004EA5 RID: 20133
		public abstract string EnergyColorName { get; }

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06004EA6 RID: 20134 RVA: 0x00231013 File Offset: 0x0022F213
		public virtual Color LabOutlineColor
		{
			get
			{
				return StsColors.halfTransparentBlack;
			}
		}

		/// <summary>
		/// Get every relic in this pool (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x0023101A File Offset: 0x0022F21A
		public IEnumerable<RelicModel> AllRelics
		{
			get
			{
				if (this._relics == null)
				{
					this._relics = this.GenerateAllRelics();
					this._relics = ModHelper.ConcatModelsFromMods<RelicModel>(this, this._relics);
				}
				return this._relics;
			}
		}

		/// <summary>
		/// Get the ID of every relic in this pool (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x00231048 File Offset: 0x0022F248
		public HashSet<ModelId> AllRelicIds
		{
			get
			{
				HashSet<ModelId> hashSet;
				if ((hashSet = this._allRelicIds) == null)
				{
					hashSet = (this._allRelicIds = this.AllRelics.Select<RelicModel, ModelId>((RelicModel c) => c.Id).ToHashSet<ModelId>());
				}
				return hashSet;
			}
		}

		/// <summary>
		/// Generates every relic in this pool (ignores Unlocks/Epoch state).
		/// Overridden in subclasses, but should only be called once by <see cref="P:MegaCrit.Sts2.Core.Models.RelicPoolModel.AllRelics" /> so it can be cached.
		/// </summary>
		// Token: 0x06004EA9 RID: 20137
		protected abstract IEnumerable<RelicModel> GenerateAllRelics();

		/// <summary>
		/// Returns every relic in this pool that the player has unlocked.
		/// By default, this is just AllRelics, but can be overriden in subclasses to remove relics that should be locked
		/// under certain conditions.
		/// </summary>
		// Token: 0x06004EAA RID: 20138 RVA: 0x00231097 File Offset: 0x0022F297
		public virtual IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
		{
			return this.AllRelics;
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0023109F File Offset: 0x0022F29F
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001D9D RID: 7581
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<RelicModel> _relics;

		// Token: 0x04001D9E RID: 7582
		[Nullable(new byte[] { 2, 1 })]
		private HashSet<ModelId> _allRelicIds;
	}
}
