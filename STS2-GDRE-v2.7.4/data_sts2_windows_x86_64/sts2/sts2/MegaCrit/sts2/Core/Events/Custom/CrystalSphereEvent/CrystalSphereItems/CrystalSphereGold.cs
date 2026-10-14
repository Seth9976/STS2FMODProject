using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems
{
	// Token: 0x02000C55 RID: 3157
	[NullableContext(1)]
	[Nullable(0)]
	public class CrystalSphereGold : CrystalSphereItem
	{
		// Token: 0x17002387 RID: 9095
		// (get) Token: 0x06008221 RID: 33313 RVA: 0x002C08DE File Offset: 0x002BEADE
		protected override string TexturePath
		{
			get
			{
				if (!this._isBig)
				{
					return ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_gold.png");
				}
				return ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_big_gold.png");
			}
		}

		// Token: 0x17002388 RID: 9096
		// (get) Token: 0x06008222 RID: 33314 RVA: 0x002C08FD File Offset: 0x002BEAFD
		private int Amount
		{
			get
			{
				if (!this._isBig)
				{
					return 10;
				}
				return 30;
			}
		}

		// Token: 0x17002389 RID: 9097
		// (get) Token: 0x06008223 RID: 33315 RVA: 0x002C090C File Offset: 0x002BEB0C
		public override Vector2I Size
		{
			get
			{
				if (!this._isBig)
				{
					return Vector2I.One;
				}
				return new Vector2I(2, 1);
			}
		}

		// Token: 0x1700238A RID: 9098
		// (get) Token: 0x06008224 RID: 33316 RVA: 0x002C0923 File Offset: 0x002BEB23
		public override bool IsGood
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008225 RID: 33317 RVA: 0x002C0926 File Offset: 0x002BEB26
		public CrystalSphereGold(bool isBig)
		{
			this._isBig = isBig;
		}

		// Token: 0x06008226 RID: 33318 RVA: 0x002C0935 File Offset: 0x002BEB35
		public override Reward ToReward(Player owner, Rng rng)
		{
			return new GoldReward(this.Amount, owner, false).SetRng(rng);
		}

		// Token: 0x06008227 RID: 33319 RVA: 0x002C094A File Offset: 0x002BEB4A
		public override SerializableCrystalSphereItem ToSerializable()
		{
			return new SerializableCrystalSphereItem
			{
				type = CrystalSphereItemType.Gold,
				isBigGold = this._isBig
			};
		}

		// Token: 0x04002BA2 RID: 11170
		private readonly bool _isBig;

		// Token: 0x04002BA3 RID: 11171
		private const int _smallAmount = 10;

		// Token: 0x04002BA4 RID: 11172
		private const int _largeAmount = 30;
	}
}
