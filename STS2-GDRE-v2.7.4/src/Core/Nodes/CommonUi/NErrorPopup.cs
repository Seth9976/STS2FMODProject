using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

/// <summary>
/// A popup to let the player know a terrible bug has occurred, and to upload a bug report.
/// The wording should be changed before we release.
/// Renders above the capstone screens (above top bar).
/// </summary>
public partial class NErrorPopup : NVerticalPopup, IScreenContext
{
	private NVerticalPopup _verticalPopup;

	private string _title;

	private string _body;

	private LocString? _cancel;

	private bool _showReportBugButton;

	private static readonly string _scenePath = SceneHelper.GetScenePath("ui/error_popup");

	public Control? DefaultFocusedControl => null;

	public new static IEnumerable<string> AssetPaths => new global::_003C_003Ez__ReadOnlySingleElementList<string>(_scenePath);

	public override void _Ready()
	{
		_verticalPopup = GetNode<NVerticalPopup>("VerticalPopup");
		_verticalPopup.SetText(_title, _body);
		if (_showReportBugButton)
		{
			_verticalPopup.InitYesButton(new LocString("main_menu_ui", "NETWORK_ERROR.report_bug"), OnReportBugButtonPressed);
		}
		else
		{
			_verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.ok"), OnOkButtonPressed);
		}
		if (_cancel != null)
		{
			_verticalPopup.InitNoButton(_cancel, OnCancelButtonPressed);
		}
		else
		{
			_verticalPopup.HideNoButton();
		}
	}

	public static NErrorPopup? Create(NetErrorInfo info)
	{
		if (TestMode.IsOn)
		{
			return null;
		}
		if (info.SelfInitiated && info.GetReason() == NetError.Quit)
		{
			return null;
		}
		bool showReportBugButton;
		return Create(new LocString("main_menu_ui", "NETWORK_ERROR.header"), LocStringFromNetError(info, out showReportBugButton), null, showReportBugButton);
	}

	public static NErrorPopup? Create(LocString title, LocString body, LocString? cancel, bool showReportBugButton)
	{
		if (TestMode.IsOn)
		{
			return null;
		}
		NErrorPopup nErrorPopup = PreloadManager.Cache.GetScene(_scenePath).Instantiate<NErrorPopup>(PackedScene.GenEditState.Disabled);
		nErrorPopup._title = title.GetFormattedText();
		nErrorPopup._body = body.GetFormattedText();
		nErrorPopup._showReportBugButton = showReportBugButton;
		nErrorPopup._cancel = cancel;
		return nErrorPopup;
	}

	/// <summary>
	/// Creates an error popup with hardcoded English text (bypassing localization).
	/// Use this when localization may be broken (e.g., showing localization errors).
	/// </summary>
	public static NErrorPopup? Create(string title, string body, bool showReportBugButton)
	{
		if (TestMode.IsOn)
		{
			return null;
		}
		NErrorPopup nErrorPopup = PreloadManager.Cache.GetScene(_scenePath).Instantiate<NErrorPopup>(PackedScene.GenEditState.Disabled);
		nErrorPopup._title = title;
		nErrorPopup._body = body;
		nErrorPopup._showReportBugButton = showReportBugButton;
		return nErrorPopup;
	}

	public static LocString LocStringFromNetError(NetErrorInfo info, out bool showReportBugButton)
	{
		NetError reason = info.GetReason();
		LocString locString = GetLocStringForReason(reason, info.ConnectionExtraInfo?.localIsHost ?? false);
		if (locString == null)
		{
			ConnectionFailureExtraInfo? connectionExtraInfo = info.ConnectionExtraInfo;
			if ((object)connectionExtraInfo != null && connectionExtraInfo.localIsHost)
			{
				locString = GetLocStringForReason(reason, isHost: false);
			}
		}
		bool flag = !info.IsModded;
		bool flag2 = flag;
		if (flag2)
		{
			bool flag3 = ((reason == NetError.None || reason == NetError.StateDivergence || (uint)(reason - 202) <= 1u) ? true : false);
			flag2 = flag3;
		}
		showReportBugButton = flag2;
		if (locString == null)
		{
			Log.Error($"Invalid net error passed to {"NErrorPopup"}: {info}!");
			locString = new LocString("main_menu_ui", "NETWORK_ERROR.INTERNAL_ERROR.body");
			showReportBugButton = !info.IsModded;
		}
		locString.Add("info", info.GetErrorString());
		return locString;
	}

	public static LocString? GetLocStringForReason(NetError reason, bool isHost)
	{
		string text = (isHost ? "NETWORK_ERROR.HOST." : "NETWORK_ERROR.");
		string text2 = reason switch
		{
			NetError.None => null, 
			NetError.QuitGameOver => null, 
			NetError.CancelledJoin => null, 
			NetError.LobbyFull => "LOBBY_FULL.body", 
			NetError.Quit => "QUIT.body", 
			NetError.HostAbandoned => "HOST_ABANDONED.body", 
			NetError.Kicked => "KICKED.body", 
			NetError.InvalidJoin => "INVALID_JOIN.body", 
			NetError.RunInProgress => "RUN_IN_PROGRESS.body", 
			NetError.StateDivergence => "STATE_DIVERGENCE.body", 
			NetError.ModMismatch => "MOD_MISMATCH.body", 
			NetError.JoinBlockedByUser => "JOIN_BLOCKED_BY_USER.body", 
			NetError.NoInternet => "NO_INTERNET.body", 
			NetError.Timeout => "TIMEOUT.body", 
			NetError.HandshakeTimeout => "TIMEOUT.body", 
			NetError.InternalError => "INTERNAL_ERROR.body", 
			NetError.UnknownNetworkError => "UNKNOWN_ERROR.body", 
			NetError.RateLimited => "RATE_LIMITED.body", 
			NetError.TryAgainLater => "TRY_AGAIN_LATER.body", 
			NetError.SecureConnectionFailed => "SECURE_CONNECTION_FAILED.body", 
			NetError.FailedToHost => "FAILED_TO_HOST.body", 
			NetError.NotInSaveGame => "NOT_IN_SAVE_GAME.body", 
			NetError.VersionMismatch => "VERSION_MISMATCH.body", 
			NetError.InvalidHandshake => "VERSION_MISMATCH.body", 
			NetError.LobbyJoinTimeout => "TIMEOUT.body", 
		};
		string text3 = ((text2 != null) ? (text + text2) : null);
		if (text3 != null)
		{
			LocString locString = new LocString("main_menu_ui", text3);
			if (locString.Exists())
			{
				return locString;
			}
		}
		return null;
	}

	private void OnOkButtonPressed(NButton _)
	{
		this.QueueFreeSafely();
	}

	private void OnCancelButtonPressed(NButton _)
	{
		this.QueueFreeSafely();
	}

	private void OnReportBugButtonPressed(NButton _)
	{
		TaskHelper.RunSafely(OpenFeedbackScreen());
	}

	private async Task OpenFeedbackScreen()
	{
		SceneTree sceneTree = GetTree();
		this.QueueFreeSafely();
		await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
		await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
		await NFeedbackScreenOpener.Instance.OpenFeedbackScreen();
	}
}
