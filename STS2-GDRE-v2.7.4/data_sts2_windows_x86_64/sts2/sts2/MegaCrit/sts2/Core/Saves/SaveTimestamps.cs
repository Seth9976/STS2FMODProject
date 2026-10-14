using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using Sentry;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Helpers for turning raw filesystem / cloud last-modified timestamps into <see cref="T:System.DateTimeOffset" />s.
	/// </summary>
	// Token: 0x020000AC RID: 172
	public static class SaveTimestamps
	{
		/// <summary>
		/// Converts a Unix-seconds last-modified timestamp into a <see cref="T:System.DateTimeOffset" />, returning
		/// <see cref="F:System.DateTimeOffset.UnixEpoch" /> for values outside the representable range instead of throwing
		/// <see cref="T:System.ArgumentOutOfRangeException" />.
		///
		/// Steam Cloud (<c>SteamRemoteStorage.GetFileTimestamp</c>) and the OS (<c>FileAccess.GetModifiedTime</c>) can
		/// report corrupt timestamps, for example during cloud-conflict resolution. Such a value used to crash the
		/// startup cloud sync (PRG-7045). Degrading a bad mtime to the epoch makes the file look "very old", so the sync
		/// treats it as stale and re-copies the valid cloud/local content instead of aborting the whole file's sync.
		/// </summary>
		// Token: 0x060007F8 RID: 2040 RVA: 0x000400CC File Offset: 0x0003E2CC
		[NullableContext(1)]
		public static DateTimeOffset FromUnixTimeSecondsOrEpoch(long seconds, string path)
		{
			if (seconds < SaveTimestamps._minUnixSeconds || seconds > SaveTimestamps._maxUnixSeconds)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(143, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Last-modified timestamp ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(seconds);
				defaultInterpolatedStringHandler.AppendLiteral(" for ");
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral(" is outside the representable DateTimeOffset range; treating it as the Unix epoch so the file re-syncs. (PRG-7045)");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureMessage("Save-store last-modified timestamp out of DateTimeOffset range", 2, delegate(Scope scope)
				{
					scope.SetExtra("save.timestamp.raw_seconds", seconds);
					scope.SetExtra("save.timestamp.path", path);
				});
				return DateTimeOffset.UnixEpoch;
			}
			return DateTimeOffset.FromUnixTimeSeconds(seconds);
		}

		// Token: 0x040002AF RID: 687
		private static readonly long _minUnixSeconds = DateTimeOffset.MinValue.ToUnixTimeSeconds();

		// Token: 0x040002B0 RID: 688
		private static readonly long _maxUnixSeconds = DateTimeOffset.MaxValue.ToUnixTimeSeconds();
	}
}
