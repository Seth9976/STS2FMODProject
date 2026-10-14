using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Platform.Null;
using MegaCrit.Sts2.Core.Platform.Steam;

namespace MegaCrit.Sts2.Core.Platform
{
	// Token: 0x02000158 RID: 344
	[NullableContext(1)]
	[Nullable(0)]
	public static class PlatformUtil
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x000622F6 File Offset: 0x000604F6
		public static PlatformType PrimaryPlatform
		{
			get
			{
				if (SteamInitializer.Initialized)
				{
					return PlatformType.Steam;
				}
				return PlatformType.None;
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00062304 File Offset: 0x00060504
		private static IPlatformUtilStrategy GetPlatformUtil(PlatformType platformType)
		{
			IPlatformUtilStrategy platformUtilStrategy;
			if (platformType != PlatformType.None)
			{
				if (platformType != PlatformType.Steam)
				{
					throw new ArgumentOutOfRangeException("platformType", platformType, null);
				}
				platformUtilStrategy = PlatformUtil._steam;
			}
			else
			{
				platformUtilStrategy = PlatformUtil._null;
			}
			return platformUtilStrategy;
		}

		/// <summary>
		/// Obtains the player name for the given player ID, with bracket escaping for safe use in BBCode contexts.
		/// Use <see cref="M:MegaCrit.Sts2.Core.Platform.PlatformUtil.GetPlayerNameRaw(MegaCrit.Sts2.Core.Platform.PlatformType,System.UInt64)" /> when displaying in plain text controls.
		/// </summary>
		/// <remarks>
		/// Player names are untrusted external input (e.g. Steam display names) and may contain square brackets that
		/// would crash or corrupt the BBCode parser. This method escapes by default so that callers don't need to
		/// remember to escape individually. The majority of call sites render through MegaRichTextLabel or HoverTip
		/// (both BBCode), so safe-by-default prevents the class of bugs where a new call site forgets to escape.
		/// </remarks>
		/// <param name="platformType">The platform type to use. Must be passed for cases where multiple platforms are
		/// supported; for instance, on PC we can play on LAN (for debug) or on Steam.</param>
		/// <param name="playerId">The ID of the player whose name is retrieved.</param>
		// Token: 0x0600107F RID: 4223 RVA: 0x0006233D File Offset: 0x0006053D
		public static string GetPlayerName(PlatformType platformType, ulong playerId)
		{
			return PlatformUtil.GetPlayerNameRaw(platformType, playerId).EscapeBbcodeTags();
		}

		/// <summary>
		/// Obtains the raw player name for the given player ID, without any escaping.
		/// Only use this for plain text contexts (e.g. <see cref="T:Godot.Label" />, <see cref="T:MegaCrit.Sts2.addons.mega_text.MegaLabel" />).
		/// For BBCode contexts (e.g. <see cref="T:MegaCrit.Sts2.addons.mega_text.MegaRichTextLabel" />, HoverTip), use
		/// <see cref="M:MegaCrit.Sts2.Core.Platform.PlatformUtil.GetPlayerName(MegaCrit.Sts2.Core.Platform.PlatformType,System.UInt64)" /> instead.
		/// </summary>
		/// <param name="platformType">The platform type to use. Must be passed for cases where multiple platforms are
		/// supported; for instance, on PC we can play on LAN (for debug) or on Steam.</param>
		/// <param name="playerId">The ID of the player whose name is retrieved.</param>
		// Token: 0x06001080 RID: 4224 RVA: 0x0006234B File Offset: 0x0006054B
		public static string GetPlayerNameRaw(PlatformType platformType, ulong playerId)
		{
			return PlatformUtil.GetPlatformUtil(platformType).GetPlayerName(playerId);
		}

		/// <summary>
		/// Returns the ID of the player playing on this machine.
		/// </summary>
		/// <param name="platformType">The platform type to use. Must be passed for cases where multiple platforms are
		/// supported; for instance, on PC we can play on LAN (for debug) or on Steam.</param>
		// Token: 0x06001081 RID: 4225 RVA: 0x00062359 File Offset: 0x00060559
		public static ulong GetLocalPlayerId(PlatformType platformType)
		{
			return PlatformUtil.GetPlatformUtil(platformType).GetLocalPlayerId();
		}

		/// <summary>
		/// Returns IDs of friends who are currently in lobbies that can be joined.
		/// </summary>
		/// <param name="platformType">The platform type to use. Must be passed for cases where multiple platforms are
		/// supported; for instance, on PC we can play on LAN (for debug) or on Steam.</param>
		// Token: 0x06001082 RID: 4226 RVA: 0x00062366 File Offset: 0x00060566
		public static Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies(PlatformType platformType)
		{
			return PlatformUtil.GetPlatformUtil(platformType).GetFriendsWithOpenLobbies();
		}

		/// <summary>
		/// Returns true if OpenInviteDialog may be called on the given platform; false otherwise.
		/// </summary>
		/// <param name="platformType">The platform type to use. Must be passed for cases where multiple platforms are
		/// supported; for instance, on PC we can play on LAN (for debug) or on Steam.</param>
		// Token: 0x06001083 RID: 4227 RVA: 0x00062373 File Offset: 0x00060573
		public static bool SupportsInviteDialog(PlatformType platformType)
		{
			return PlatformUtil.GetPlatformUtil(platformType).SupportsInviteDialog;
		}

		/// <summary>
		/// Opens the platform invite dialog for the given net service.
		/// </summary>
		// Token: 0x06001084 RID: 4228 RVA: 0x00062380 File Offset: 0x00060580
		public static void OpenInviteDialog(INetGameService netService)
		{
			PlatformUtil.GetPlatformUtil(netService.Platform).OpenInviteDialog(netService);
		}

		/// <summary>
		/// Opens a URL in a platform-specific browser.
		/// For example, on Steam, the URL is opened in the game overlay if available. On non-Steam PC, the URL is opened
		/// in the OS' default browser.
		/// </summary>
		// Token: 0x06001085 RID: 4229 RVA: 0x00062393 File Offset: 0x00060593
		public static void OpenUrl(string url)
		{
			PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).OpenUrl(url);
		}

		/// <summary>
		/// Opens the virtual keyboard.
		/// Does nothing on PC.
		/// </summary>
		// Token: 0x06001086 RID: 4230 RVA: 0x000623A5 File Offset: 0x000605A5
		public static void OpenVirtualKeyboard()
		{
			PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).OpenVirtualKeyboard();
		}

		/// <summary>
		/// Closes the virtual keyboard.
		/// </summary>
		// Token: 0x06001087 RID: 4231 RVA: 0x000623B6 File Offset: 0x000605B6
		public static void CloseVirtualKeyboard()
		{
			PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).CloseVirtualKeyboard();
		}

		/// <summary>
		/// Returns which platform branch the player is playing on.
		/// </summary>
		// Token: 0x06001088 RID: 4232 RVA: 0x000623C7 File Offset: 0x000605C7
		public static PlatformBranch GetPlatformBranch()
		{
			return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetPlatformBranch();
		}

		/// <summary>
		/// Returns the three-letter language code for the language that the player currently has set for the primary platform.
		/// </summary>
		// Token: 0x06001089 RID: 4233 RVA: 0x000623D8 File Offset: 0x000605D8
		[NullableContext(2)]
		public static string GetThreeLetterLanguageCode()
		{
			return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetThreeLetterLanguageCode();
		}

		/// <summary>
		/// Returns the raw string identifier of the language code that the player currently has set for primary platform.
		/// </summary>
		// Token: 0x0600108A RID: 4234 RVA: 0x000623E9 File Offset: 0x000605E9
		public static string GetRawLanguage()
		{
			return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetRawLanguage();
		}

		/// <summary>
		/// Sets the primary rich presence display data for the player.
		/// This API is currently rather Steam-centric, so feel free to refactor later if necessary.
		/// </summary>
		/// <param name="token">The key in the rich presence localization to display as the primary string. Additional
		/// key/value pairs used for substitutions can be provided through <see cref="M:MegaCrit.Sts2.Core.Platform.PlatformUtil.SetRichPresenceValue(System.String,System.String)" />.</param>
		/// <param name="playerGroup">The group to associate with. This is an arbitrary string that is used to associate
		/// players in the same multiplayer group together. Pass null if there is no multiplayer group.</param>
		/// <param name="groupSize">The size of the multiplayer group.</param>
		// Token: 0x0600108B RID: 4235 RVA: 0x000623FA File Offset: 0x000605FA
		public static void SetRichPresence(string token, [Nullable(2)] string playerGroup, int? groupSize)
		{
			PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).SetRichPresence(token, playerGroup, groupSize);
		}

		/// <summary>
		/// Sets a key-value pair used in rich presence loc string substitution.
		/// This API is currently rather Steam-centric, so feel free to refactor later if necessary.
		/// </summary>
		// Token: 0x0600108C RID: 4236 RVA: 0x0006240E File Offset: 0x0006060E
		public static void SetRichPresenceValue(string key, [Nullable(2)] string value)
		{
			PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).SetRichPresenceValue(key, value);
		}

		/// <summary>
		/// Clears all rich presence data set for the player, including the primary display data and associated key-value pairs.
		/// </summary>
		// Token: 0x0600108D RID: 4237 RVA: 0x00062421 File Offset: 0x00060621
		public static void ClearRichPresence()
		{
			PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).ClearRichPresence();
		}

		/// <summary>
		/// Returns the kind of fullscreening this platform allows for.
		/// See <see cref="T:MegaCrit.Sts2.Core.Platform.SupportedWindowMode" /> for the values.
		/// </summary>
		// Token: 0x0600108E RID: 4238 RVA: 0x00062432 File Offset: 0x00060632
		public static SupportedWindowMode GetSupportedWindowMode()
		{
			return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetSupportedWindowMode();
		}

		/// <summary>
		/// Returns whether or not the platform overlay (ie steam overlay) is currently open
		/// </summary>
		// Token: 0x0600108F RID: 4239 RVA: 0x00062443 File Offset: 0x00060643
		public static bool IsPlatformOverlayOpen()
		{
			return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).IsPlatformOverlayOpen;
		}

		// Token: 0x040005CE RID: 1486
		private static readonly NullPlatformUtilStrategy _null = new NullPlatformUtilStrategy();

		// Token: 0x040005CF RID: 1487
		private static readonly SteamPlatformUtilStrategy _steam = new SteamPlatformUtilStrategy();
	}
}
