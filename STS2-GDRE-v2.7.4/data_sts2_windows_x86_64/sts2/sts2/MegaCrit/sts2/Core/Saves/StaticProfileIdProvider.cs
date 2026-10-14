using System;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000B6 RID: 182
	public class StaticProfileIdProvider : IProfileIdProvider
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x000417B0 File Offset: 0x0003F9B0
		public int CurrentProfileId
		{
			get
			{
				return this._profileId;
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x000417B8 File Offset: 0x0003F9B8
		public StaticProfileIdProvider(int profileId)
		{
			this._profileId = profileId;
		}

		// Token: 0x0400030A RID: 778
		private int _profileId;
	}
}
