using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using Steamworks;

namespace MegaCrit.Sts2.Core.Platform.Steam
{
	/// <summary>
	/// Responsible for handling Steam-related requests to join multiplayer sessions coming from outside the game.
	/// </summary>
	// Token: 0x0200015F RID: 351
	public class SteamJoinCallbackHandler : IDisposable
	{
		// Token: 0x060010A7 RID: 4263 RVA: 0x00062A2C File Offset: 0x00060C2C
		public SteamJoinCallbackHandler()
		{
			this._steamJoinCallback = new Callback<GameLobbyJoinRequested_t>(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.OnSteamLobbyJoinRequested), false);
		}

		/// <summary>
		/// If the game is not yet launched and the player accepts an invite, Steam launches the game with a specific command
		/// line argument. This method handles joining the multiplayer session. It should be called relatively early after
		/// the game is launched.
		/// </summary>
		// Token: 0x060010A8 RID: 4264 RVA: 0x00062A4C File Offset: 0x00060C4C
		public void CheckForCommandLineJoin()
		{
			string text;
			if (CommandLineHelper.TryGetValue("+connect_lobby", out text))
			{
				ulong num = ulong.Parse(text);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Joining to host via Steam invite that caused the game to launch. Lobby id: ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(num);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				TaskHelper.RunSafely(SteamJoinCallbackHandler.JoinToHost(num, null, null));
			}
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00062AAF File Offset: 0x00060CAF
		public void Dispose()
		{
			this._steamJoinCallback.Dispose();
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00062ABC File Offset: 0x00060CBC
		private void OnSteamLobbyJoinRequested(GameLobbyJoinRequested_t lobbyJoinRequest)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Joining to host via Steam invite. Lobby: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(lobbyJoinRequest.m_steamIDLobby.m_SteamID);
			defaultInterpolatedStringHandler.AppendLiteral(" player: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(lobbyJoinRequest.m_steamIDFriend.m_SteamID);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			TaskHelper.RunSafely(SteamJoinCallbackHandler.JoinToHost(lobbyJoinRequest.m_steamIDLobby.m_SteamID, new ulong?(lobbyJoinRequest.m_steamIDFriend.m_SteamID), null));
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00062B44 File Offset: 0x00060D44
		[NullableContext(1)]
		private static async Task JoinToHost(ulong lobbyId, ulong? playerId, [Nullable(2)] IClientConnectionInitializer connInitializer = null)
		{
			NMultiplayerTest nmultiplayerTest = NGame.Instance.RootSceneContainer.CurrentScene as NMultiplayerTest;
			if (nmultiplayerTest != null)
			{
				if (connInitializer == null)
				{
					connInitializer = SteamClientConnectionInitializer.FromLobby(lobbyId);
				}
				await nmultiplayerTest.JoinToHost(connInitializer);
			}
			else
			{
				if (RunManager.Instance.IsInProgress)
				{
					LocString locString = new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.body");
					playerId.GetValueOrDefault();
					if (playerId == null)
					{
						playerId = new ulong?(SteamMatchmaking.GetLobbyOwner(new CSteamID(lobbyId)).m_SteamID);
					}
					locString.Add("host", PlatformUtil.GetPlayerName(PlatformType.Steam, playerId.Value));
					NGenericPopup ngenericPopup = NGenericPopup.Create();
					NModalContainer.Instance.Add(ngenericPopup, true);
					TaskAwaiter<bool> taskAwaiter = ngenericPopup.WaitForConfirmation(locString, new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.header"), new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.cancel"), new LocString("gameplay_ui", "QUIT_AND_JOIN_CONFIRMATION.confirm")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						return;
					}
				}
				if (NGame.Instance.MainMenu == null)
				{
					await NGame.Instance.ReturnToMainMenu();
				}
				for (;;)
				{
					NMainMenu mainMenu = NGame.Instance.MainMenu;
					if (((mainMenu != null) ? mainMenu.SubmenuStack.Peek() : null) == null)
					{
						break;
					}
					NMainMenu mainMenu2 = NGame.Instance.MainMenu;
					if (mainMenu2 != null)
					{
						mainMenu2.SubmenuStack.Pop();
					}
				}
				if (connInitializer == null)
				{
					connInitializer = SteamClientConnectionInitializer.FromLobby(lobbyId);
				}
				await NGame.Instance.MainMenu.JoinGame(connInitializer);
			}
		}

		// Token: 0x040005F5 RID: 1525
		[Nullable(1)]
		private readonly Callback<GameLobbyJoinRequested_t> _steamJoinCallback;
	}
}
