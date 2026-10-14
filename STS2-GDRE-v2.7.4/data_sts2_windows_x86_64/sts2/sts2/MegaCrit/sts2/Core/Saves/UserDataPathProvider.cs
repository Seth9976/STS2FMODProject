using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	[Nullable(0)]
	public static class UserDataPathProvider
	{
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000417C7 File Offset: 0x0003F9C7
		public static string SavesDir
		{
			get
			{
				return "saves";
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x000417CE File Offset: 0x0003F9CE
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x000417D5 File Offset: 0x0003F9D5
		public static bool IsRunningModded { get; set; }

		// Token: 0x060008E0 RID: 2272 RVA: 0x000417E0 File Offset: 0x0003F9E0
		public static string GetProfileScopedPath(int profileId, string dataType, PlatformType? platformOverride = null, ulong? userIdOverride = null)
		{
			PlatformType platformType = platformOverride ?? PlatformUtil.PrimaryPlatform;
			ulong num = userIdOverride ?? PlatformUtil.GetLocalPlayerId(platformType);
			string platformDirectoryName = UserDataPathProvider.GetPlatformDirectoryName(platformType);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 4);
			defaultInterpolatedStringHandler.AppendLiteral("user://");
			defaultInterpolatedStringHandler.AppendFormatted(platformDirectoryName);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(num);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted(UserDataPathProvider.GetProfileDir(profileId));
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted(dataType);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00041890 File Offset: 0x0003FA90
		public static string GetProfileScopedBasePath(int profileId, PlatformType? platformOverride = null, ulong? userIdOverride = null)
		{
			PlatformType platformType = platformOverride ?? PlatformUtil.PrimaryPlatform;
			ulong num = userIdOverride ?? PlatformUtil.GetLocalPlayerId(platformType);
			string platformDirectoryName = UserDataPathProvider.GetPlatformDirectoryName(platformType);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 3);
			defaultInterpolatedStringHandler.AppendLiteral("user://");
			defaultInterpolatedStringHandler.AppendFormatted(platformDirectoryName);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(num);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted(UserDataPathProvider.GetProfileDir(profileId));
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0004192C File Offset: 0x0003FB2C
		public static string GetAccountScopedBasePath([Nullable(2)] string dataType, PlatformType? platformOverride = null, ulong? userIdOverride = null)
		{
			PlatformType platformType = platformOverride ?? PlatformUtil.PrimaryPlatform;
			ulong num = userIdOverride ?? PlatformUtil.GetLocalPlayerId(platformType);
			string platformDirectoryName = UserDataPathProvider.GetPlatformDirectoryName(platformType);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
			defaultInterpolatedStringHandler.AppendLiteral("user://");
			defaultInterpolatedStringHandler.AppendFormatted(platformDirectoryName);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(num);
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (dataType != null)
			{
				text = StringExtensions.PathJoin(text, dataType);
			}
			return text;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x000419BC File Offset: 0x0003FBBC
		public static string GetAccountDir(bool? forceModState = null)
		{
			if (!(forceModState ?? UserDataPathProvider.IsRunningModded))
			{
				return "";
			}
			return "modded";
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000419F4 File Offset: 0x0003FBF4
		public static string GetProfileDir(int profileId)
		{
			return UserDataPathProvider.GetProfileDir(profileId, null);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00041A10 File Offset: 0x0003FC10
		public static string GetProfileDir(int profileId, bool? forceModState)
		{
			string accountDir = UserDataPathProvider.GetAccountDir(forceModState);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
			defaultInterpolatedStringHandler.AppendLiteral("profile");
			defaultInterpolatedStringHandler.AppendFormatted<int>(profileId);
			return StringExtensions.PathJoin(accountDir, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00041A4C File Offset: 0x0003FC4C
		public static string GetLegacyPreAccountPath(string dataType)
		{
			return "user://" + dataType;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00041A5C File Offset: 0x0003FC5C
		public static string GetPlatformDirectoryName(PlatformType platform)
		{
			string text;
			if (platform == PlatformType.Steam)
			{
				text = "steam";
			}
			else
			{
				text = (OS.HasFeature("editor") ? "editor" : "default");
			}
			return text;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00041A8F File Offset: 0x0003FC8F
		public static bool IsLegacyPath(string path)
		{
			return !path.Contains("/steam/") && !path.Contains("/default/") && !path.Contains("/editor/");
		}
	}
}
