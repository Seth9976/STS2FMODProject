using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	/// <summary>
	/// Contains information about why a network operation failed, or why we disconnected from a multiplayer session.
	/// Prefer passing this instead of NetError, as this contains more information about the underlying error that might
	/// have been mapped to our NetError.
	/// </summary>
	// Token: 0x02000C8A RID: 3210
	[NullableContext(2)]
	[Nullable(0)]
	public readonly struct NetErrorInfo
	{
		/// <summary>
		/// Set to true if the disconnection was initiated by the local peer. For example, if the reason is Quit and this is
		/// true, then the disconnection was caused by a quit initiated locally. If the reason is Quit and this is false, then
		/// the disconnection was caused by a quit initiated remotely.
		/// </summary>
		// Token: 0x170023E7 RID: 9191
		// (get) Token: 0x06008375 RID: 33653 RVA: 0x002C4E22 File Offset: 0x002C3022
		public bool SelfInitiated { get; }

		/// <summary>
		/// Extra info about the disconnection.
		/// You usually do not need to access this directly; use GetErrorString and show that to the user.
		/// </summary>
		// Token: 0x170023E8 RID: 9192
		// (get) Token: 0x06008376 RID: 33654 RVA: 0x002C4E2A File Offset: 0x002C302A
		public ConnectionFailureExtraInfo ConnectionExtraInfo { get; }

		// Token: 0x06008377 RID: 33655 RVA: 0x002C4E34 File Offset: 0x002C3034
		public NetErrorInfo(NetError reason, bool selfInitiated)
		{
			this._connectionReason = null;
			this._steamReason = null;
			this._lobbyCreationResult = null;
			this._lobbyEnterResponse = null;
			this._debugReason = null;
			this._godotError = null;
			this.ConnectionExtraInfo = null;
			this._reason = new NetError?(reason);
			this.SelfInitiated = selfInitiated;
		}

		// Token: 0x06008378 RID: 33656 RVA: 0x002C4EA0 File Offset: 0x002C30A0
		public NetErrorInfo(ConnectionFailureReason reason, ConnectionFailureExtraInfo extraInfo = null)
		{
			this._reason = null;
			this._steamReason = null;
			this._lobbyCreationResult = null;
			this._lobbyEnterResponse = null;
			this._debugReason = null;
			this._godotError = null;
			this._connectionReason = new ConnectionFailureReason?(reason);
			this.ConnectionExtraInfo = extraInfo;
			this.SelfInitiated = false;
		}

		// Token: 0x06008379 RID: 33657 RVA: 0x002C4F0C File Offset: 0x002C310C
		public NetErrorInfo(SteamDisconnectionReason steamReason, string debugReason, bool selfInitiated)
		{
			this._reason = null;
			this._connectionReason = null;
			this._lobbyCreationResult = null;
			this._lobbyEnterResponse = null;
			this._godotError = null;
			this.ConnectionExtraInfo = null;
			this._steamReason = new SteamDisconnectionReason?(steamReason);
			this._debugReason = debugReason;
			this.SelfInitiated = selfInitiated;
		}

		// Token: 0x0600837A RID: 33658 RVA: 0x002C4F78 File Offset: 0x002C3178
		public NetErrorInfo(EChatRoomEnterResponse lobbyEnterResponse)
		{
			this._reason = null;
			this._connectionReason = null;
			this._steamReason = null;
			this._lobbyCreationResult = null;
			this._debugReason = null;
			this._godotError = null;
			this.ConnectionExtraInfo = null;
			this._lobbyEnterResponse = new EChatRoomEnterResponse?(lobbyEnterResponse);
			this.SelfInitiated = true;
		}

		// Token: 0x0600837B RID: 33659 RVA: 0x002C4FE4 File Offset: 0x002C31E4
		public NetErrorInfo(EResult lobbyCreationResult)
		{
			this._reason = null;
			this._connectionReason = null;
			this._steamReason = null;
			this._lobbyEnterResponse = null;
			this._debugReason = null;
			this._godotError = null;
			this.ConnectionExtraInfo = null;
			this._lobbyCreationResult = new EResult?(lobbyCreationResult);
			this.SelfInitiated = true;
		}

		// Token: 0x0600837C RID: 33660 RVA: 0x002C5050 File Offset: 0x002C3250
		public NetErrorInfo(Error error)
		{
			this._reason = null;
			this._connectionReason = null;
			this._steamReason = null;
			this._lobbyCreationResult = null;
			this._lobbyEnterResponse = null;
			this._debugReason = null;
			this.ConnectionExtraInfo = null;
			this._godotError = new Error?(error);
			this.SelfInitiated = true;
		}

		/// <summary>
		/// Returns a disconnection reason mapped from the underlying transport disconnection reason.
		/// </summary>
		// Token: 0x0600837D RID: 33661 RVA: 0x002C50BC File Offset: 0x002C32BC
		public NetError GetReason()
		{
			if (this._reason != null)
			{
				return this._reason.Value;
			}
			NetError netError;
			if (this._connectionReason != null)
			{
				ConnectionFailureReason value = this._connectionReason.Value;
				switch (value)
				{
				case ConnectionFailureReason.None:
					netError = NetError.None;
					break;
				case ConnectionFailureReason.LobbyFull:
					netError = NetError.LobbyFull;
					break;
				case ConnectionFailureReason.NotInSaveGame:
					netError = NetError.NotInSaveGame;
					break;
				case ConnectionFailureReason.RunInProgress:
					netError = NetError.RunInProgress;
					break;
				case ConnectionFailureReason.VersionMismatch:
					netError = NetError.VersionMismatch;
					break;
				case ConnectionFailureReason.ModMismatch:
					netError = NetError.ModMismatch;
					break;
				default:
					<PrivateImplementationDetails>.ThrowSwitchExpressionException(value);
					break;
				}
				return netError;
			}
			if (this._steamReason != null)
			{
				return this._steamReason.Value.ToApp();
			}
			if (this._lobbyCreationResult != null)
			{
				return NetError.FailedToHost;
			}
			if (this._lobbyEnterResponse != null)
			{
				EChatRoomEnterResponse value2 = this._lobbyEnterResponse.Value;
				switch (value2)
				{
				case 1:
					return NetError.None;
				case 2:
					return NetError.InvalidJoin;
				case 3:
					return NetError.InternalError;
				case 4:
					return NetError.LobbyFull;
				case 5:
					return NetError.UnknownNetworkError;
				case 6:
					return NetError.JoinBlockedByUser;
				case 7:
					return NetError.UnknownNetworkError;
				case 8:
					return NetError.JoinBlockedByUser;
				case 9:
					return NetError.JoinBlockedByUser;
				case 10:
					return NetError.JoinBlockedByUser;
				case 11:
					return NetError.JoinBlockedByUser;
				case 15:
					return NetError.RateLimited;
				}
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(value2);
				return netError;
			}
			if (this._godotError != null)
			{
				return NetError.FailedToHost;
			}
			throw new InvalidOperationException("Tried to get DisconnectionReason from DisconnectionInfo without any assigned errors");
		}

		/// <summary>
		/// Returns a string with more human-readable details about the underlying cause of the disconnection.
		/// This is suitable for displaying to users as long as platform requirements allow plain error codes in messages.
		/// </summary>
		// Token: 0x0600837E RID: 33662 RVA: 0x002C5234 File Offset: 0x002C3434
		[NullableContext(1)]
		public string GetErrorString()
		{
			if (this._reason != null)
			{
				return this._reason.Value.ToString();
			}
			if (this._connectionReason != null)
			{
				if (this._connectionReason.GetValueOrDefault() == ConnectionFailureReason.ModMismatch)
				{
					StringBuilder stringBuilder = new StringBuilder();
					ConnectionFailureExtraInfo connectionExtraInfo = this.ConnectionExtraInfo;
					List<string> list = ((connectionExtraInfo != null) ? connectionExtraInfo.missingModsOnHost : null);
					if (list != null && list.Count > 0)
					{
						LocString locString = new LocString("main_menu_ui", "NETWORK_ERROR.MOD_MISMATCH.description.missingOnHost");
						locString.Add("mods", string.Join(", ", this.ConnectionExtraInfo.missingModsOnHost));
						stringBuilder.AppendLine(locString.GetFormattedText());
					}
					ConnectionFailureExtraInfo connectionExtraInfo2 = this.ConnectionExtraInfo;
					list = ((connectionExtraInfo2 != null) ? connectionExtraInfo2.missingModsOnLocal : null);
					if (list != null && list.Count > 0)
					{
						LocString locString2 = new LocString("main_menu_ui", "NETWORK_ERROR.MOD_MISMATCH.description.missingOnLocal");
						locString2.Add("mods", string.Join(", ", this.ConnectionExtraInfo.missingModsOnLocal));
						stringBuilder.AppendLine(locString2.GetFormattedText());
					}
					return stringBuilder.ToString();
				}
				if (this._connectionReason.GetValueOrDefault() == ConnectionFailureReason.VersionMismatch)
				{
					ConnectionFailureExtraInfo connectionExtraInfo3 = this.ConnectionExtraInfo;
					string text = ((connectionExtraInfo3 != null) ? connectionExtraInfo3.hostVersion : null);
					ConnectionFailureExtraInfo connectionExtraInfo4 = this.ConnectionExtraInfo;
					if (text != ((connectionExtraInfo4 != null) ? connectionExtraInfo4.localVersion : null))
					{
						ConnectionFailureExtraInfo connectionExtraInfo5 = this.ConnectionExtraInfo;
						PlatformBranch? platformBranch = ((connectionExtraInfo5 != null) ? connectionExtraInfo5.hostBranch : null);
						ConnectionFailureExtraInfo connectionExtraInfo6 = this.ConnectionExtraInfo;
						PlatformBranch? platformBranch2 = ((connectionExtraInfo6 != null) ? connectionExtraInfo6.localBranch : null);
						if (!((platformBranch.GetValueOrDefault() == platformBranch2.GetValueOrDefault()) & (platformBranch != null == (platformBranch2 != null))))
						{
							LocString locString3 = new LocString("main_menu_ui", "NETWORK_ERROR.VERSION_MISMATCH.description.branchMismatch");
							LocString locString4 = locString3;
							string text2 = "hostBranch";
							ConnectionFailureExtraInfo connectionExtraInfo7 = this.ConnectionExtraInfo;
							locString4.Add(text2, ((connectionExtraInfo7 != null) ? ((connectionExtraInfo7.hostBranch != null) ? connectionExtraInfo7.hostBranch.GetValueOrDefault().ToName() : null) : null) ?? "<null>");
							LocString locString5 = locString3;
							string text3 = "localBranch";
							ConnectionFailureExtraInfo connectionExtraInfo8 = this.ConnectionExtraInfo;
							locString5.Add(text3, ((connectionExtraInfo8 != null) ? ((connectionExtraInfo8.localBranch != null) ? connectionExtraInfo8.localBranch.GetValueOrDefault().ToName() : null) : null) ?? "<null>");
							return locString3.GetFormattedText();
						}
						LocString locString6 = new LocString("main_menu_ui", "NETWORK_ERROR.VERSION_MISMATCH.description.versionMismatch");
						LocString locString7 = locString6;
						string text4 = "hostVersion";
						ConnectionFailureExtraInfo connectionExtraInfo9 = this.ConnectionExtraInfo;
						locString7.Add(text4, ((connectionExtraInfo9 != null) ? connectionExtraInfo9.hostVersion : null) ?? "<null>");
						LocString locString8 = locString6;
						string text5 = "localVersion";
						ConnectionFailureExtraInfo connectionExtraInfo10 = this.ConnectionExtraInfo;
						locString8.Add(text5, ((connectionExtraInfo10 != null) ? connectionExtraInfo10.localVersion : null) ?? "<null>");
						return locString6.GetFormattedText();
					}
					else
					{
						ConnectionFailureExtraInfo connectionExtraInfo11 = this.ConnectionExtraInfo;
						ulong? num = ((connectionExtraInfo11 != null) ? connectionExtraInfo11.hostHash : null);
						ConnectionFailureExtraInfo connectionExtraInfo12 = this.ConnectionExtraInfo;
						ulong? num2 = ((connectionExtraInfo12 != null) ? connectionExtraInfo12.localHash : null);
						if (!((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null))))
						{
							LocString locString9 = new LocString("main_menu_ui", "NETWORK_ERROR.VERSION_MISMATCH.description.modelDbMismatch");
							LocString locString10 = locString9;
							string text6 = "hostHash";
							ConnectionFailureExtraInfo connectionExtraInfo13 = this.ConnectionExtraInfo;
							locString10.Add(text6, ((connectionExtraInfo13 != null) ? ((connectionExtraInfo13.hostHash != null) ? connectionExtraInfo13.hostHash.GetValueOrDefault().ToString() : null) : null) ?? "<null>");
							LocString locString11 = locString9;
							string text7 = "localHash";
							ConnectionFailureExtraInfo connectionExtraInfo14 = this.ConnectionExtraInfo;
							locString11.Add(text7, ((connectionExtraInfo14 != null) ? ((connectionExtraInfo14.localHash != null) ? connectionExtraInfo14.localHash.GetValueOrDefault().ToString() : null) : null) ?? "<null>");
							return locString9.GetFormattedText();
						}
					}
				}
				return this._connectionReason.Value.ToString();
			}
			else
			{
				if (this._steamReason != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
					defaultInterpolatedStringHandler.AppendFormatted<SteamDisconnectionReason?>(this._steamReason);
					defaultInterpolatedStringHandler.AppendLiteral(" - ");
					defaultInterpolatedStringHandler.AppendFormatted(this._debugReason);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				if (this._lobbyCreationResult != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Lobby creation failed: ");
					defaultInterpolatedStringHandler.AppendFormatted<EResult>(this._lobbyCreationResult.Value);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				if (this._lobbyEnterResponse != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Lobby join failed: ");
					defaultInterpolatedStringHandler.AppendFormatted<EChatRoomEnterResponse>(this._lobbyEnterResponse.Value);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				if (this._godotError != null)
				{
					return this._godotError.Value.ToString();
				}
				return "<null>";
			}
		}

		/// <summary>
		/// Returns a string that should not be displayed to the user. Use this in debug logs.
		/// </summary>
		// Token: 0x0600837F RID: 33663 RVA: 0x002C56EC File Offset: 0x002C38EC
		[NullableContext(1)]
		public override string ToString()
		{
			if (this._reason != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 2);
				defaultInterpolatedStringHandler.AppendLiteral("DisconnectionReason ");
				defaultInterpolatedStringHandler.AppendFormatted<NetError>(this._reason.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.SelfInitiated);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (this._connectionReason != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendLiteral("ConnectionFailureReason ");
				defaultInterpolatedStringHandler.AppendFormatted<ConnectionFailureReason>(this._connectionReason.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.SelfInitiated);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (this._steamReason != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 3);
				defaultInterpolatedStringHandler.AppendLiteral("SteamDisconnectionReason ");
				defaultInterpolatedStringHandler.AppendFormatted<SteamDisconnectionReason>(this._steamReason.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(this._debugReason);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.SelfInitiated);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (this._lobbyCreationResult != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
				defaultInterpolatedStringHandler.AppendLiteral("EResult ");
				defaultInterpolatedStringHandler.AppendFormatted<EResult>(this._lobbyCreationResult.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.SelfInitiated);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (this._lobbyEnterResponse != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("EChatRoomEnterResponse ");
				defaultInterpolatedStringHandler.AppendFormatted<EChatRoomEnterResponse>(this._lobbyEnterResponse.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.SelfInitiated);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (this._godotError != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Godot.Error ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(this._godotError.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.SelfInitiated);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return "<null>";
		}

		/// <summary>
		/// Used for errors that occur above the platform level, in our transport layers.
		/// </summary>
		// Token: 0x04002CAE RID: 11438
		private readonly NetError? _reason;

		/// <summary>
		/// Used when the disconnection was raised from the JoinFlow.
		/// </summary>
		// Token: 0x04002CAF RID: 11439
		private readonly ConnectionFailureReason? _connectionReason;

		/// <summary>
		/// Used when the disconnection was raised from the Steam transport.
		/// </summary>
		// Token: 0x04002CB0 RID: 11440
		private readonly SteamDisconnectionReason? _steamReason;

		/// <summary>
		/// Used for Steam hosting when the lobby creation fails.
		/// </summary>
		// Token: 0x04002CB1 RID: 11441
		private readonly EResult? _lobbyCreationResult;

		/// <summary>
		/// Use for Steam joining when entering the lobby fails. Even though it refers to "chat room", it really means "lobby".
		/// </summary>
		// Token: 0x04002CB2 RID: 11442
		private readonly EChatRoomEnterResponse? _lobbyEnterResponse;

		/// <summary>
		/// String that accompanies the disconnection. Currently only used on the Steam platform.
		/// </summary>
		// Token: 0x04002CB3 RID: 11443
		private readonly string _debugReason;

		/// <summary>
		/// Used specifically for ENet hosting when lobby creation fails.
		/// If Godot errors occur elsewhere, expand the mapping for this error.
		/// </summary>
		// Token: 0x04002CB4 RID: 11444
		private readonly Error? _godotError;
	}
}
