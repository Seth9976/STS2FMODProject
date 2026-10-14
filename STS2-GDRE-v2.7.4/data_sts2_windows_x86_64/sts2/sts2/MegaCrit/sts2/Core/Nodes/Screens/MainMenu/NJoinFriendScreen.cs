using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000304 RID: 772
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendScreen.cs")]
	public class NJoinFriendScreen : NSubmenu
	{
		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x00127DC5 File Offset: 0x00125FC5
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._buttonContainer.GetChildCount(false) <= 0)
				{
					return this._refreshButton;
				}
				return this._buttonContainer.GetChild<Control>(0, false);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x00127DEA File Offset: 0x00125FEA
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NJoinFriendScreen._scenePath,
					NJoinFriendButton.scenePath
				});
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002A58 RID: 10840 RVA: 0x00127E07 File Offset: 0x00126007
		public bool DebugFriendsButtons
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x00127E0A File Offset: 0x0012600A
		[NullableContext(2)]
		public static NJoinFriendScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NJoinFriendScreen._scenePath).Instantiate<NJoinFriendScreen>(0L);
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x00127E2C File Offset: 0x0012602C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._buttonContainer = base.GetNode<Control>("%ButtonContainer");
			this._loadingOverlay = base.GetNode<Control>("%LoadingOverlay");
			this._loadingFriendsIndicator = base.GetNode<Control>("%LoadingIndicator");
			this._noFriendsLabel = base.GetNode<MegaLabel>("%NoFriendsText");
			this._refreshButton = base.GetNode<NJoinFriendRefreshButton>("%RefreshButton");
			base.GetNode<MegaLabel>("TitleLabel").SetTextAutoSize(new LocString("main_menu_ui", "JOIN_FRIENDS_MENU.title").GetFormattedText());
			this._noFriendsLabel.SetTextAutoSize(new LocString("main_menu_ui", "JOIN_FRIENDS_MENU.noFriends").GetFormattedText());
			this._refreshButton.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				this.RefreshButtonClicked();
			}), 0U);
			this._loadingFriendsIndicator.Visible = false;
			this._noFriendsLabel.Visible = false;
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x00127F30 File Offset: 0x00126130
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			this._loadingOverlay.Visible = false;
			if ((!SteamInitializer.Initialized || CommandLineHelper.HasArg("fastmp")) && !this.DebugFriendsButtons)
			{
				TaskHelper.RunSafely(this.FastMpJoin());
				return;
			}
			this._refreshTask = TaskHelper.RunSafely(this.ShowFriends());
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x00127F88 File Offset: 0x00126188
		public override void OnSubmenuClosed()
		{
			JoinFlow currentJoinFlow = this._currentJoinFlow;
			if (currentJoinFlow == null)
			{
				return;
			}
			currentJoinFlow.CancelToken.Cancel();
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x00127FA0 File Offset: 0x001261A0
		private async Task FastMpJoin()
		{
			ulong num = 1000UL;
			string text;
			if (CommandLineHelper.TryGetValue("clientId", out text))
			{
				num = ulong.Parse(text);
			}
			DisplayServer.WindowSetTitle("Slay The Spire 2 (Client)", 0);
			await this.JoinGameAsync(new ENetClientConnectionInitializer(num, "127.0.0.1", 33771));
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x00127FE4 File Offset: 0x001261E4
		private void RefreshButtonClicked()
		{
			Task refreshTask = this._refreshTask;
			if (refreshTask != null && !refreshTask.IsCompleted)
			{
				return;
			}
			this._refreshTask = TaskHelper.RunSafely(this.RefreshButtonClickedAsync());
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x00128018 File Offset: 0x00126218
		private async Task RefreshButtonClickedAsync()
		{
			this._noFriendsLabel.Visible = false;
			if (SteamInitializer.Initialized)
			{
				this._loadingFriendsIndicator.Visible = true;
				await Cmd.Wait(0.5f, false);
				this._loadingFriendsIndicator.Visible = false;
			}
			await this.ShowFriends();
			Control initialFocusedControl = this.InitialFocusedControl;
			if (initialFocusedControl != null)
			{
				initialFocusedControl.TryGrabFocus();
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x0012805C File Offset: 0x0012625C
		private async Task ShowFriends()
		{
			this._loadingFriendsIndicator.Visible = true;
			foreach (Node node in this._buttonContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			if (SteamInitializer.Initialized)
			{
				IEnumerable<ulong> enumerable = await PlatformUtil.GetFriendsWithOpenLobbies(PlatformType.Steam);
				IEnumerable<ulong> enumerable2 = enumerable;
				NButton nbutton = null;
				foreach (ulong num in enumerable2)
				{
					NJoinFriendButton njoinFriendButton = NJoinFriendButton.Create(num);
					this._buttonContainer.AddChildSafely(njoinFriendButton);
					SteamClientConnectionInitializer connInitializer = SteamClientConnectionInitializer.FromPlayer(num);
					njoinFriendButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
					{
						this.JoinGame(connInitializer);
					}), 0U);
					if (nbutton == null)
					{
						nbutton = njoinFriendButton;
					}
				}
			}
			if (this.DebugFriendsButtons)
			{
				for (int i = 0; i < Rng.Chaotic.NextInt(5, 20); i++)
				{
					NJoinFriendButton njoinFriendButton2 = NJoinFriendButton.Create((ulong)((long)((i == 0) ? 1 : (i * 1000))));
					this._buttonContainer.AddChildSafely(njoinFriendButton2);
				}
			}
			ActiveScreenContext.Instance.Update();
			this._loadingFriendsIndicator.Visible = false;
			this._noFriendsLabel.Visible = this._buttonContainer.GetChildCount(false) == 0;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x0012809F File Offset: 0x0012629F
		private void JoinGame(IClientConnectionInitializer connInitializer)
		{
			TaskHelper.RunSafely(this.JoinGameAsync(connInitializer));
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x001280B0 File Offset: 0x001262B0
		public async Task JoinGameAsync(IClientConnectionInitializer connInitializer)
		{
			JoinFlow currentJoinFlow = this._currentJoinFlow;
			if (currentJoinFlow != null && currentJoinFlow.NetService.IsConnected)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(95, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to join game with connection ");
				defaultInterpolatedStringHandler.AppendFormatted<IClientConnectionInitializer>(connInitializer);
				defaultInterpolatedStringHandler.AppendLiteral(" while we were already joining a game! Ignoring this attempt");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				this._loadingOverlay.Visible = true;
				this._currentJoinFlow = new JoinFlow(new NetClientGameService(), null);
				try
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Attempting to join game with connection initializer ");
					defaultInterpolatedStringHandler.AppendFormatted<IClientConnectionInitializer>(connInitializer);
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					JoinResult joinResult = await this._currentJoinFlow.Begin(connInitializer, base.GetTree());
					JoinResult joinResult2 = joinResult;
					if (joinResult2.sessionState.GetValueOrDefault() == RunSessionState.InLobby)
					{
						if (joinResult2.gameMode == GameMode.Standard)
						{
							NCharacterSelectScreen submenuType = this._stack.GetSubmenuType<NCharacterSelectScreen>();
							submenuType.InitializeMultiplayerAsClient(this._currentJoinFlow.NetService, joinResult2.joinResponse.Value);
							this._stack.Push(submenuType);
						}
						else if (joinResult2.gameMode == GameMode.Daily)
						{
							NDailyRunScreen submenuType2 = this._stack.GetSubmenuType<NDailyRunScreen>();
							submenuType2.InitializeMultiplayerAsClient(this._currentJoinFlow.NetService, joinResult2.joinResponse.Value);
							this._stack.Push(submenuType2);
						}
						else
						{
							if (joinResult2.gameMode != GameMode.Custom)
							{
								throw new ArgumentOutOfRangeException("gameMode", joinResult2.gameMode, "Invalid game mode!");
							}
							NCustomRunScreen submenuType3 = this._stack.GetSubmenuType<NCustomRunScreen>();
							submenuType3.InitializeMultiplayerAsClient(this._currentJoinFlow.NetService, joinResult2.joinResponse.Value);
							this._stack.Push(submenuType3);
						}
					}
					else if (joinResult2.sessionState.GetValueOrDefault() == RunSessionState.InLoadedLobby)
					{
						if (joinResult2.gameMode == GameMode.Standard)
						{
							NMultiplayerLoadGameScreen submenuType4 = this._stack.GetSubmenuType<NMultiplayerLoadGameScreen>();
							submenuType4.InitializeAsClient(this._currentJoinFlow.NetService, joinResult2.loadJoinResponse.Value);
							this._stack.Push(submenuType4);
						}
						else if (joinResult2.gameMode == GameMode.Daily)
						{
							NDailyRunLoadScreen submenuType5 = this._stack.GetSubmenuType<NDailyRunLoadScreen>();
							submenuType5.InitializeAsClient(this._currentJoinFlow.NetService, joinResult2.loadJoinResponse.Value);
							this._stack.Push(submenuType5);
						}
						else if (joinResult2.gameMode == GameMode.Custom)
						{
							NCustomRunLoadScreen submenuType6 = this._stack.GetSubmenuType<NCustomRunLoadScreen>();
							submenuType6.InitializeAsClient(this._currentJoinFlow.NetService, joinResult2.loadJoinResponse.Value);
							this._stack.Push(submenuType6);
						}
					}
					else if (joinResult2.sessionState.GetValueOrDefault() == RunSessionState.Running)
					{
						NErrorPopup nerrorPopup = NErrorPopup.Create(new NetErrorInfo(NetError.RunInProgress, false));
						if (nerrorPopup != null)
						{
							NModalContainer.Instance.Add(nerrorPopup, true);
						}
						this._currentJoinFlow.NetService.Disconnect(NetError.RunInProgress, false);
					}
				}
				catch (ClientConnectionFailedException ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Received connection failed exception while joining game: ");
					defaultInterpolatedStringHandler.AppendFormatted<ClientConnectionFailedException>(ex);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					NErrorPopup nerrorPopup2 = NErrorPopup.Create(ex.info);
					if (nerrorPopup2 != null)
					{
						NModalContainer.Instance.Add(nerrorPopup2, true);
					}
					this._currentJoinFlow.NetService.Disconnect(ex.info.GetReason(), false);
				}
				catch (OperationCanceledException)
				{
					Log.Warn("Joining was canceled by user", 2);
				}
				catch (Exception ex2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Received unexpected exception ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(ex2.GetType());
					defaultInterpolatedStringHandler.AppendLiteral(" while joining game! Disconnecting with InternalError");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					NErrorPopup nerrorPopup3 = NErrorPopup.Create(new NetErrorInfo(NetError.InternalError, false));
					if (nerrorPopup3 != null)
					{
						NModalContainer.Instance.Add(nerrorPopup3, true);
					}
					this._currentJoinFlow.NetService.Disconnect(NetError.InternalError, false);
					throw;
				}
				finally
				{
					if (GodotObject.IsInstanceValid(this))
					{
						this._loadingOverlay.Visible = false;
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A63 RID: 10851 RVA: 0x001280FC File Offset: 0x001262FC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NJoinFriendScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NJoinFriendScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendScreen.MethodName.RefreshButtonClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A64 RID: 10852 RVA: 0x00128208 File Offset: 0x00126408
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NJoinFriendScreen.MethodName.Create && args.Count == 0)
			{
				NJoinFriendScreen njoinFriendScreen = NJoinFriendScreen.Create();
				ret = VariantUtils.CreateFrom<NJoinFriendScreen>(ref njoinFriendScreen);
				return true;
			}
			if ((ref method) == NJoinFriendScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendScreen.MethodName.RefreshButtonClicked && args.Count == 0)
			{
				this.RefreshButtonClicked();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x001282E0 File Offset: 0x001264E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NJoinFriendScreen.MethodName.Create && args.Count == 0)
			{
				NJoinFriendScreen njoinFriendScreen = NJoinFriendScreen.Create();
				ret = VariantUtils.CreateFrom<NJoinFriendScreen>(ref njoinFriendScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002A66 RID: 10854 RVA: 0x00128320 File Offset: 0x00126520
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NJoinFriendScreen.MethodName.Create || (ref method) == NJoinFriendScreen.MethodName._Ready || (ref method) == NJoinFriendScreen.MethodName.OnSubmenuOpened || (ref method) == NJoinFriendScreen.MethodName.OnSubmenuClosed || (ref method) == NJoinFriendScreen.MethodName.RefreshButtonClicked || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002A67 RID: 10855 RVA: 0x00128380 File Offset: 0x00126580
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NJoinFriendScreen.PropertyName._buttonContainer)
			{
				this._buttonContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._loadingOverlay)
			{
				this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._loadingFriendsIndicator)
			{
				this._loadingFriendsIndicator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._noFriendsLabel)
			{
				this._noFriendsLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._refreshButton)
			{
				this._refreshButton = VariantUtils.ConvertTo<NJoinFriendRefreshButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002A68 RID: 10856 RVA: 0x0012841C File Offset: 0x0012661C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NJoinFriendScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName.DebugFriendsButtons)
			{
				bool debugFriendsButtons = this.DebugFriendsButtons;
				value = VariantUtils.CreateFrom<bool>(ref debugFriendsButtons);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._buttonContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonContainer);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._loadingOverlay)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._loadingOverlay);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._loadingFriendsIndicator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._loadingFriendsIndicator);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._noFriendsLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._noFriendsLabel);
				return true;
			}
			if ((ref name) == NJoinFriendScreen.PropertyName._refreshButton)
			{
				value = VariantUtils.CreateFrom<NJoinFriendRefreshButton>(ref this._refreshButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A69 RID: 10857 RVA: 0x00128518 File Offset: 0x00126718
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NJoinFriendScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NJoinFriendScreen.PropertyName._buttonContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NJoinFriendScreen.PropertyName._loadingOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NJoinFriendScreen.PropertyName._loadingFriendsIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NJoinFriendScreen.PropertyName._noFriendsLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NJoinFriendScreen.PropertyName._refreshButton, 0L, "", 4096L, false),
				new PropertyInfo(1L, NJoinFriendScreen.PropertyName.DebugFriendsButtons, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A6A RID: 10858 RVA: 0x00128614 File Offset: 0x00126814
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NJoinFriendScreen.PropertyName._buttonContainer, Variant.From<Control>(ref this._buttonContainer));
			info.AddProperty(NJoinFriendScreen.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
			info.AddProperty(NJoinFriendScreen.PropertyName._loadingFriendsIndicator, Variant.From<Control>(ref this._loadingFriendsIndicator));
			info.AddProperty(NJoinFriendScreen.PropertyName._noFriendsLabel, Variant.From<MegaLabel>(ref this._noFriendsLabel));
			info.AddProperty(NJoinFriendScreen.PropertyName._refreshButton, Variant.From<NJoinFriendRefreshButton>(ref this._refreshButton));
		}

		/// <inheritdoc />
		// Token: 0x06002A6B RID: 10859 RVA: 0x00128698 File Offset: 0x00126898
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NJoinFriendScreen.PropertyName._buttonContainer, ref variant))
			{
				this._buttonContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NJoinFriendScreen.PropertyName._loadingOverlay, ref variant2))
			{
				this._loadingOverlay = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NJoinFriendScreen.PropertyName._loadingFriendsIndicator, ref variant3))
			{
				this._loadingFriendsIndicator = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NJoinFriendScreen.PropertyName._noFriendsLabel, ref variant4))
			{
				this._noFriendsLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NJoinFriendScreen.PropertyName._refreshButton, ref variant5))
			{
				this._refreshButton = variant5.As<NJoinFriendRefreshButton>();
			}
		}

		// Token: 0x04000FD1 RID: 4049
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/join_friend_submenu");

		// Token: 0x04000FD2 RID: 4050
		private Control _buttonContainer;

		// Token: 0x04000FD3 RID: 4051
		private Control _loadingOverlay;

		// Token: 0x04000FD4 RID: 4052
		private Control _loadingFriendsIndicator;

		// Token: 0x04000FD5 RID: 4053
		private MegaLabel _noFriendsLabel;

		// Token: 0x04000FD6 RID: 4054
		private NJoinFriendRefreshButton _refreshButton;

		// Token: 0x04000FD7 RID: 4055
		[Nullable(2)]
		private Task _refreshTask;

		// Token: 0x04000FD8 RID: 4056
		[Nullable(2)]
		private JoinFlow _currentJoinFlow;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014F4 RID: 5364
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004FD1 RID: 20433
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004FD2 RID: 20434
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004FD3 RID: 20435
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04004FD4 RID: 20436
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'RefreshButtonClicked' method.
			/// </summary>
			// Token: 0x04004FD5 RID: 20437
			public static readonly StringName RefreshButtonClicked = "RefreshButtonClicked";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014F5 RID: 5365
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004FD6 RID: 20438
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the 'DebugFriendsButtons' property.
			/// </summary>
			// Token: 0x04004FD7 RID: 20439
			public static readonly StringName DebugFriendsButtons = "DebugFriendsButtons";

			/// <summary>
			/// Cached name for the '_buttonContainer' field.
			/// </summary>
			// Token: 0x04004FD8 RID: 20440
			public static readonly StringName _buttonContainer = "_buttonContainer";

			/// <summary>
			/// Cached name for the '_loadingOverlay' field.
			/// </summary>
			// Token: 0x04004FD9 RID: 20441
			public static readonly StringName _loadingOverlay = "_loadingOverlay";

			/// <summary>
			/// Cached name for the '_loadingFriendsIndicator' field.
			/// </summary>
			// Token: 0x04004FDA RID: 20442
			public static readonly StringName _loadingFriendsIndicator = "_loadingFriendsIndicator";

			/// <summary>
			/// Cached name for the '_noFriendsLabel' field.
			/// </summary>
			// Token: 0x04004FDB RID: 20443
			public static readonly StringName _noFriendsLabel = "_noFriendsLabel";

			/// <summary>
			/// Cached name for the '_refreshButton' field.
			/// </summary>
			// Token: 0x04004FDC RID: 20444
			public static readonly StringName _refreshButton = "_refreshButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014F6 RID: 5366
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
