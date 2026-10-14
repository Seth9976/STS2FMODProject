using System;

namespace MegaCrit.Sts2.Core.Saves.Validation
{
	/// <summary>
	/// Severity levels for save validation errors.
	/// </summary>
	// Token: 0x020000B9 RID: 185
	public enum ValidationSeverity
	{
		/// <summary>
		/// The save loaded but data was degraded (e.g. deprecated content replaced with fallback).
		/// </summary>
		// Token: 0x0400030F RID: 783
		Warning,
		/// <summary>
		/// The save cannot be loaded due to invalid or missing data.
		/// </summary>
		// Token: 0x04000310 RID: 784
		Fatal
	}
}
