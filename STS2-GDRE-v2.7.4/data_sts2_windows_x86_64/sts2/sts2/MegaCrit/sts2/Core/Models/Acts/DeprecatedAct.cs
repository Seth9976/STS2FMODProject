using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.Acts
{
	// Token: 0x02000B80 RID: 2944
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedAct : ActModel
	{
		// Token: 0x06007B1E RID: 31518 RVA: 0x002A03A6 File Offset: 0x0029E5A6
		public override IEnumerable<EncounterModel> GenerateAllEncounters()
		{
			return Array.Empty<EncounterModel>();
		}

		// Token: 0x06007B1F RID: 31519 RVA: 0x002A03AD File Offset: 0x0029E5AD
		public override bool IsUnlocked(UnlockState unlockState)
		{
			return true;
		}

		// Token: 0x170021FF RID: 8703
		// (get) Token: 0x06007B20 RID: 31520 RVA: 0x002A03B0 File Offset: 0x0029E5B0
		public override string ChestOpenSfx
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002200 RID: 8704
		// (get) Token: 0x06007B21 RID: 31521 RVA: 0x002A03B7 File Offset: 0x0029E5B7
		public override IEnumerable<EncounterModel> BossDiscoveryOrder
		{
			get
			{
				return Array.Empty<EncounterModel>();
			}
		}

		// Token: 0x17002201 RID: 8705
		// (get) Token: 0x06007B22 RID: 31522 RVA: 0x002A03BE File Offset: 0x0029E5BE
		public override IEnumerable<AncientEventModel> AllAncients
		{
			get
			{
				return Array.Empty<AncientEventModel>();
			}
		}

		// Token: 0x06007B23 RID: 31523 RVA: 0x002A03C5 File Offset: 0x0029E5C5
		public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
		{
			return Array.Empty<AncientEventModel>();
		}

		// Token: 0x17002202 RID: 8706
		// (get) Token: 0x06007B24 RID: 31524 RVA: 0x002A03CC File Offset: 0x0029E5CC
		public override IEnumerable<EventModel> AllEvents
		{
			get
			{
				return Array.Empty<EventModel>();
			}
		}

		// Token: 0x17002203 RID: 8707
		// (get) Token: 0x06007B25 RID: 31525 RVA: 0x002A03D3 File Offset: 0x0029E5D3
		protected override int NumberOfWeakEncounters
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002204 RID: 8708
		// (get) Token: 0x06007B26 RID: 31526 RVA: 0x002A03D6 File Offset: 0x0029E5D6
		protected override int BaseNumberOfRooms
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002205 RID: 8709
		// (get) Token: 0x06007B27 RID: 31527 RVA: 0x002A03D9 File Offset: 0x0029E5D9
		public override string[] BgMusicOptions
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		// Token: 0x17002206 RID: 8710
		// (get) Token: 0x06007B28 RID: 31528 RVA: 0x002A03E0 File Offset: 0x0029E5E0
		public override string[] MusicBankPaths
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		// Token: 0x17002207 RID: 8711
		// (get) Token: 0x06007B29 RID: 31529 RVA: 0x002A03E7 File Offset: 0x0029E5E7
		public override string AmbientSfx
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002208 RID: 8712
		// (get) Token: 0x06007B2A RID: 31530 RVA: 0x002A03EE File Offset: 0x0029E5EE
		public override string ChestSpineResourcePath
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002209 RID: 8713
		// (get) Token: 0x06007B2B RID: 31531 RVA: 0x002A03F5 File Offset: 0x0029E5F5
		public override string ChestSpineSkinNameNormal
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700220A RID: 8714
		// (get) Token: 0x06007B2C RID: 31532 RVA: 0x002A03FC File Offset: 0x0029E5FC
		public override string ChestSpineSkinNameStroke
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06007B2D RID: 31533 RVA: 0x002A0403 File Offset: 0x0029E603
		protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
		{
		}

		// Token: 0x1700220B RID: 8715
		// (get) Token: 0x06007B2E RID: 31534 RVA: 0x002A0405 File Offset: 0x0029E605
		public override int Index
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700220C RID: 8716
		// (get) Token: 0x06007B2F RID: 31535 RVA: 0x002A0408 File Offset: 0x0029E608
		public override bool IsDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700220D RID: 8717
		// (get) Token: 0x06007B30 RID: 31536 RVA: 0x002A040B File Offset: 0x0029E60B
		public override Color MapTraveledColor
		{
			get
			{
				return new Color("27221C");
			}
		}

		// Token: 0x1700220E RID: 8718
		// (get) Token: 0x06007B31 RID: 31537 RVA: 0x002A0417 File Offset: 0x0029E617
		public override Color MapUntraveledColor
		{
			get
			{
				return new Color("6E7750");
			}
		}

		// Token: 0x1700220F RID: 8719
		// (get) Token: 0x06007B32 RID: 31538 RVA: 0x002A0423 File Offset: 0x0029E623
		public override Color MapBgColor
		{
			get
			{
				return new Color("9B9562");
			}
		}

		// Token: 0x06007B33 RID: 31539 RVA: 0x002A042F File Offset: 0x0029E62F
		public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
		{
			return new MapPointTypeCounts(0, 0);
		}
	}
}
