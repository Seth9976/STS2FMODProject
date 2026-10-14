using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems
{
	// Token: 0x02000C57 RID: 3159
	[NullableContext(1)]
	[Nullable(0)]
	public class CrystalSphereRelic : CrystalSphereItem
	{
		// Token: 0x1700238E RID: 9102
		// (get) Token: 0x0600822F RID: 33327 RVA: 0x002C0A35 File Offset: 0x002BEC35
		public override Vector2I Size
		{
			get
			{
				return new Vector2I(4, 4);
			}
		}

		// Token: 0x1700238F RID: 9103
		// (get) Token: 0x06008230 RID: 33328 RVA: 0x002C0A3E File Offset: 0x002BEC3E
		public override bool IsGood
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002390 RID: 9104
		// (get) Token: 0x06008231 RID: 33329 RVA: 0x002C0A41 File Offset: 0x002BEC41
		protected override string TexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_relic.png");
			}
		}

		// Token: 0x06008232 RID: 33330 RVA: 0x002C0A4D File Offset: 0x002BEC4D
		public override Reward ToReward(Player owner, Rng rng)
		{
			return new RelicReward(owner).SetRng(rng);
		}

		// Token: 0x06008233 RID: 33331 RVA: 0x002C0A5B File Offset: 0x002BEC5B
		public override SerializableCrystalSphereItem ToSerializable()
		{
			return new SerializableCrystalSphereItem
			{
				type = CrystalSphereItemType.Relic
			};
		}
	}
}
