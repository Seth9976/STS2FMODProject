using System;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D0A RID: 3338
	public static class DebugSettings
	{
		/// <summary>
		/// Skip intro logo and timeline requirement for faster dev iteration.
		/// Set via STS2_DEV_SKIP environment variable.
		/// </summary>
		// Token: 0x17002525 RID: 9509
		// (get) Token: 0x060086B6 RID: 34486 RVA: 0x002D3598 File Offset: 0x002D1798
		public static bool DevSkip { get; } = Environment.GetEnvironmentVariable("STS2_DEV_SKIP") != null;

		/// <summary>
		/// Should packed images be ignored in favor of the original image version?
		/// Set this to true to enable, but make sure to set it back to false before committing, or else you will get a test
		/// failure.
		/// </summary>
		// Token: 0x17002526 RID: 9510
		// (get) Token: 0x060086B7 RID: 34487 RVA: 0x002D359F File Offset: 0x002D179F
		public static bool IgnorePackedImages
		{
			get
			{
				return false;
			}
		}
	}
}
