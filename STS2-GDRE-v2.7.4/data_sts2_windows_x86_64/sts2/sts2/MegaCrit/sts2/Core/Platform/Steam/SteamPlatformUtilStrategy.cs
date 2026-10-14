using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	// Token: 0x02000162 RID: 354
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamPlatformUtilStrategy : IPlatformUtilStrategy
	{
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x00062FFB File Offset: 0x000611FB
		public bool SupportsInviteDialog
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x00063002 File Offset: 0x00061202
		public bool IsPlatformOverlayOpen
		{
			get
			{
				return this._isPlatformOverlayOpen;
			}
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0006300A File Offset: 0x0006120A
		public SteamPlatformUtilStrategy()
		{
			this._steamOverlayCallback = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnSteamOverlayToggled));
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0006302C File Offset: 0x0006122C
		protected override void Finalize()
		{
			try
			{
				Callback<GameOverlayActivated_t> steamOverlayCallback = this._steamOverlayCallback;
				if (steamOverlayCallback != null)
				{
					steamOverlayCallback.Dispose();
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00063064 File Offset: 0x00061264
		public string GetPlayerName(ulong playerId)
		{
			string text;
			if (playerId == SteamUser.GetSteamID().m_SteamID)
			{
				text = SteamFriends.GetPersonaName();
			}
			else
			{
				text = SteamFriends.GetFriendPersonaName(new CSteamID(playerId));
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return playerId.ToString();
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x000630A3 File Offset: 0x000612A3
		public ulong GetLocalPlayerId()
		{
			return SteamUser.GetSteamID().m_SteamID;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x000630B0 File Offset: 0x000612B0
		public Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies()
		{
			List<ulong> list = new List<ulong>();
			int friendCount = SteamFriends.GetFriendCount(4);
			for (int i = 0; i < friendCount; i++)
			{
				CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, 4);
				FriendGameInfo_t friendGameInfo_t;
				if (SteamFriends.GetFriendGamePlayed(friendByIndex, ref friendGameInfo_t) && friendGameInfo_t.m_gameID.m_GameID == 2868840UL && friendGameInfo_t.m_steamIDLobby.IsValid())
				{
					list.Add(friendByIndex.m_SteamID);
				}
			}
			return Task.FromResult<IEnumerable<ulong>>(list);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0006311C File Offset: 0x0006131C
		public void OpenInviteDialog(INetGameService netService)
		{
			if (!SteamUtils.IsOverlayEnabled())
			{
				Log.Error("Tried to open invite dialog, but the player has disabled the steam overlay", 2);
				return;
			}
			INetHostGameService netHostGameService = netService as INetHostGameService;
			CSteamID csteamID;
			if (netHostGameService != null)
			{
				SteamHost steamHost = netHostGameService.NetHost as SteamHost;
				if (steamHost != null)
				{
					if (steamHost.LobbyId == null)
					{
						Log.Warn("Tried to open invite dialog but steam host is not yet in a lobby", 2);
						return;
					}
					csteamID = steamHost.LobbyId.Value;
					goto IL_00D1;
				}
			}
			INetClientGameService netClientGameService = netService as INetClientGameService;
			if (netClientGameService != null)
			{
				SteamClient steamClient = netClientGameService.NetClient as SteamClient;
				if (steamClient != null)
				{
					if (steamClient.LobbyId == null)
					{
						Log.Warn("Tried to open invite dialog but steam host is not yet in a lobby", 2);
						return;
					}
					csteamID = steamClient.LobbyId.Value;
					goto IL_00D1;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Tried to open invite dialog for non-steam net service ");
			defaultInterpolatedStringHandler.AppendFormatted<INetGameService>(netService);
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			IL_00D1:
			SteamFriends.ActivateGameOverlayInviteDialog(csteamID);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00063200 File Offset: 0x00061400
		public void OpenUrl(string url)
		{
			if (SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, 0);
				return;
			}
			OS.ShellOpen(url);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00063218 File Offset: 0x00061418
		public void OpenVirtualKeyboard()
		{
			SteamUtils.ShowFloatingGamepadTextInput(0, 100, 200, 300, 50);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0006322F File Offset: 0x0006142F
		public void CloseVirtualKeyboard()
		{
			SteamUtils.DismissFloatingGamepadTextInput();
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00063238 File Offset: 0x00061438
		public void SetRichPresence(string token, [Nullable(2)] string playerGroup, int? groupSize)
		{
			SteamFriends.SetRichPresence("steam_display", "#" + token);
			SteamFriends.SetRichPresence("steam_player_group", playerGroup);
			SteamFriends.SetRichPresence("steam_player_group_size", (groupSize != null) ? groupSize.GetValueOrDefault().ToString() : null);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0006328D File Offset: 0x0006148D
		public void SetRichPresenceValue(string key, [Nullable(2)] string value)
		{
			SteamFriends.SetRichPresence(key, value);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00063297 File Offset: 0x00061497
		public void ClearRichPresence()
		{
			SteamFriends.ClearRichPresence();
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x000632A0 File Offset: 0x000614A0
		[NullableContext(2)]
		public string GetThreeLetterLanguageCode()
		{
			string rawLanguage = this.GetRawLanguage();
			if (rawLanguage != null)
			{
				switch (rawLanguage.Length)
				{
				case 4:
					if (rawLanguage == "thai")
					{
						return "tha";
					}
					break;
				case 5:
				{
					char c = rawLanguage[0];
					switch (c)
					{
					case 'c':
						if (rawLanguage == "czech")
						{
							return "cze";
						}
						break;
					case 'd':
						if (rawLanguage == "dutch")
						{
							return "dut";
						}
						break;
					case 'e':
					case 'f':
						break;
					case 'g':
						if (rawLanguage == "greek")
						{
							return "gre";
						}
						break;
					default:
						if (c == 'l')
						{
							if (rawLanguage == "latam")
							{
								return "esp";
							}
						}
						break;
					}
					break;
				}
				case 6:
				{
					char c = rawLanguage[0];
					if (c <= 'f')
					{
						if (c != 'a')
						{
							if (c == 'f')
							{
								if (rawLanguage == "french")
								{
									return "fra";
								}
							}
						}
						else if (rawLanguage == "arabic")
						{
							return "ara";
						}
					}
					else if (c != 'g')
					{
						if (c != 'k')
						{
							if (c == 'p')
							{
								if (rawLanguage == "polish")
								{
									return "pol";
								}
							}
						}
						else if (rawLanguage == "korean")
						{
							return "kor";
						}
					}
					else if (rawLanguage == "german")
					{
						return "deu";
					}
					break;
				}
				case 7:
				{
					char c = rawLanguage[0];
					if (c != 'e')
					{
						if (c != 'i')
						{
							switch (c)
							{
							case 'r':
								if (rawLanguage == "russian")
								{
									return "rus";
								}
								break;
							case 's':
								if (rawLanguage == "spanish")
								{
									return "spa";
								}
								if (rawLanguage == "swedish")
								{
									return "swe";
								}
								break;
							case 't':
								if (rawLanguage == "turkish")
								{
									return "tur";
								}
								break;
							}
						}
						else if (rawLanguage == "italian")
						{
							return "ita";
						}
					}
					else if (rawLanguage == "english")
					{
						return "eng";
					}
					break;
				}
				case 8:
				{
					char c = rawLanguage[0];
					if (c != 'j')
					{
						if (c != 's')
						{
							if (c == 't')
							{
								if (rawLanguage == "tchinese")
								{
									return "zht";
								}
							}
						}
						else if (rawLanguage == "schinese")
						{
							return "zhs";
						}
					}
					else if (rawLanguage == "japanese")
					{
						return "jpn";
					}
					break;
				}
				case 9:
				{
					char c = rawLanguage[0];
					if (c != 'b')
					{
						if (c != 'n')
						{
							if (c == 'u')
							{
								if (rawLanguage == "ukrainian")
								{
									return "ukr";
								}
							}
						}
						else if (rawLanguage == "norwegian")
						{
							return "nor";
						}
					}
					else if (rawLanguage == "brazilian")
					{
						return "ptb";
					}
					break;
				}
				case 10:
				{
					char c = rawLanguage[0];
					if (c != 'i')
					{
						if (c != 'p')
						{
							if (c == 'v')
							{
								if (rawLanguage == "vietnamese")
								{
									return "vie";
								}
							}
						}
						else if (rawLanguage == "portuguese")
						{
							return "por";
						}
					}
					else if (rawLanguage == "indonesian")
					{
						return "ind";
					}
					break;
				}
				}
			}
			return null;
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x000636EC File Offset: 0x000618EC
		public PlatformBranch GetPlatformBranch()
		{
			if (this._branch != null)
			{
				return this._branch.Value;
			}
			string text;
			if (!SteamApps.GetCurrentBetaName(ref text, 128))
			{
				this._branch = new PlatformBranch?(PlatformBranch.Production);
			}
			else
			{
				PlatformBranch? platformBranch = PlatformBranchExtensions.FromName(text);
				if (platformBranch != null)
				{
					this._branch = platformBranch;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 2);
					defaultInterpolatedStringHandler.AppendLiteral("User is on Steam beta branch ");
					defaultInterpolatedStringHandler.AppendFormatted(text);
					defaultInterpolatedStringHandler.AppendLiteral(" which could not be mapped to any ");
					defaultInterpolatedStringHandler.AppendFormatted("PlatformBranch");
					defaultInterpolatedStringHandler.AppendLiteral("! Defaulting to None");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this._branch = new PlatformBranch?(PlatformBranch.None);
				}
			}
			return this._branch.Value;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x000637AD File Offset: 0x000619AD
		public string GetRawLanguage()
		{
			return SteamUtils.GetSteamUILanguage();
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x000637B4 File Offset: 0x000619B4
		public SupportedWindowMode GetSupportedWindowMode()
		{
			if (SteamUtils.IsSteamInBigPictureMode())
			{
				return SupportedWindowMode.FullscreenOnlyDisplayToggle;
			}
			return SupportedWindowMode.Any;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x000637C0 File Offset: 0x000619C0
		private void OnSteamOverlayToggled(GameOverlayActivated_t callback)
		{
			this._isPlatformOverlayOpen = callback.m_bActive == 1;
		}

		// Token: 0x040005FA RID: 1530
		private const string _richPresenceDisplayKey = "steam_display";

		// Token: 0x040005FB RID: 1531
		private const string _richPresenceGroupKey = "steam_player_group";

		// Token: 0x040005FC RID: 1532
		private const string _richPresenceGroupSizeKey = "steam_player_group_size";

		// Token: 0x040005FD RID: 1533
		private PlatformBranch? _branch;

		// Token: 0x040005FE RID: 1534
		private bool _isPlatformOverlayOpen;

		// Token: 0x040005FF RID: 1535
		[Nullable(2)]
		private Callback<GameOverlayActivated_t> _steamOverlayCallback;
	}
}
