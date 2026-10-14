using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// A popup to let the player know a terrible bug has occurred, and to upload a bug report.
	/// The wording should be changed before we release.
	/// Renders above the capstone screens (above top bar).
	/// </summary>
	// Token: 0x020003DD RID: 989
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NErrorPopup.cs")]
	public class NErrorPopup : NVerticalPopup, IScreenContext
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003C15 RID: 15381 RVA: 0x001CC0F4 File Offset: 0x001CA2F4
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06003C16 RID: 15382 RVA: 0x001CC0F7 File Offset: 0x001CA2F7
		public new static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NErrorPopup._scenePath);
			}
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x001CC104 File Offset: 0x001CA304
		public override void _Ready()
		{
			this._verticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this._verticalPopup.SetText(this._title, this._body);
			if (this._showReportBugButton)
			{
				this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "NETWORK_ERROR.report_bug"), new Action<NButton>(this.OnReportBugButtonPressed));
			}
			else
			{
				this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.ok"), new Action<NButton>(this.OnOkButtonPressed));
			}
			if (this._cancel != null)
			{
				this._verticalPopup.InitNoButton(this._cancel, new Action<NButton>(this.OnCancelButtonPressed));
				return;
			}
			this._verticalPopup.HideNoButton();
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x001CC1C8 File Offset: 0x001CA3C8
		[NullableContext(2)]
		public static NErrorPopup Create(NetErrorInfo info)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (info.SelfInitiated && info.GetReason() == NetError.Quit)
			{
				return null;
			}
			bool flag;
			return NErrorPopup.Create(new LocString("main_menu_ui", "NETWORK_ERROR.header"), NErrorPopup.LocStringFromNetError(info, out flag), null, flag);
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x001CC214 File Offset: 0x001CA414
		[return: Nullable(2)]
		public static NErrorPopup Create(LocString title, LocString body, [Nullable(2)] LocString cancel, bool showReportBugButton)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NErrorPopup nerrorPopup = PreloadManager.Cache.GetScene(NErrorPopup._scenePath).Instantiate<NErrorPopup>(0L);
			nerrorPopup._title = title.GetFormattedText();
			nerrorPopup._body = body.GetFormattedText();
			nerrorPopup._showReportBugButton = showReportBugButton;
			nerrorPopup._cancel = cancel;
			return nerrorPopup;
		}

		/// <summary>
		/// Creates an error popup with hardcoded English text (bypassing localization).
		/// Use this when localization may be broken (e.g., showing localization errors).
		/// </summary>
		// Token: 0x06003C1A RID: 15386 RVA: 0x001CC268 File Offset: 0x001CA468
		[return: Nullable(2)]
		public static NErrorPopup Create(string title, string body, bool showReportBugButton)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NErrorPopup nerrorPopup = PreloadManager.Cache.GetScene(NErrorPopup._scenePath).Instantiate<NErrorPopup>(0L);
			nerrorPopup._title = title;
			nerrorPopup._body = body;
			nerrorPopup._showReportBugButton = showReportBugButton;
			return nerrorPopup;
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x001CC2AC File Offset: 0x001CA4AC
		private static LocString LocStringFromNetError(NetErrorInfo info, out bool showReportBugButton)
		{
			NetError reason = info.GetReason();
			string text;
			switch (reason)
			{
			case NetError.None:
				text = null;
				break;
			case NetError.Quit:
				text = "NETWORK_ERROR.QUIT.body";
				break;
			case NetError.QuitGameOver:
				text = null;
				break;
			case NetError.HostAbandoned:
				text = "NETWORK_ERROR.HOST_ABANDONED.body";
				break;
			case NetError.Kicked:
				text = "NETWORK_ERROR.KICKED.body";
				break;
			case NetError.InvalidJoin:
				text = "NETWORK_ERROR.INVALID_JOIN.body";
				break;
			case NetError.CancelledJoin:
				text = null;
				break;
			case NetError.LobbyFull:
				text = "NETWORK_ERROR.LOBBY_FULL.body";
				break;
			case NetError.RunInProgress:
				text = "NETWORK_ERROR.RUN_IN_PROGRESS.body";
				break;
			case NetError.NotInSaveGame:
				text = "NETWORK_ERROR.NOT_IN_SAVE_GAME.body";
				break;
			case NetError.VersionMismatch:
				text = "NETWORK_ERROR.VERSION_MISMATCH.body";
				break;
			case NetError.JoinBlockedByUser:
				text = "NETWORK_ERROR.JOIN_BLOCKED_BY_USER.body";
				break;
			case NetError.StateDivergence:
				text = "NETWORK_ERROR.STATE_DIVERGENCE.body";
				break;
			case NetError.HandshakeTimeout:
				text = "NETWORK_ERROR.TIMEOUT.body";
				break;
			case NetError.ModMismatch:
				text = "NETWORK_ERROR.MOD_MISMATCH.body";
				break;
			case NetError.NoInternet:
				text = "NETWORK_ERROR.NO_INTERNET.body";
				break;
			case NetError.Timeout:
				text = "NETWORK_ERROR.TIMEOUT.body";
				break;
			case NetError.InternalError:
				text = "NETWORK_ERROR.INTERNAL_ERROR.body";
				break;
			case NetError.UnknownNetworkError:
				text = "NETWORK_ERROR.UNKNOWN_ERROR.body";
				break;
			case NetError.RateLimited:
				text = "NETWORK_ERROR.RATE_LIMITED.body";
				break;
			case NetError.TryAgainLater:
				text = "NETWORK_ERROR.TRY_AGAIN_LATER.body";
				break;
			case NetError.FailedToHost:
				text = "NETWORK_ERROR.FAILED_TO_HOST.body";
				break;
			case NetError.SecureConnectionFailed:
				text = "NETWORK_ERROR.SECURE_CONNECTION_FAILED.body";
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(reason);
				break;
			}
			string text2 = text;
			bool flag = reason == NetError.None || reason == NetError.StateDivergence || reason - NetError.InternalError <= 1;
			showReportBugButton = flag;
			if (text2 == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid net error passed to ");
				defaultInterpolatedStringHandler.AppendFormatted("NErrorPopup");
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<NetErrorInfo>(info);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				text2 = "NETWORK_ERROR.INTERNAL_ERROR.body";
				showReportBugButton = true;
			}
			LocString locString = new LocString("main_menu_ui", text2);
			locString.Add("info", info.GetErrorString());
			return locString;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x001CC48E File Offset: 0x001CA68E
		private void OnOkButtonPressed(NButton _)
		{
			this.QueueFreeSafely();
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x001CC496 File Offset: 0x001CA696
		private void OnCancelButtonPressed(NButton _)
		{
			this.QueueFreeSafely();
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x001CC49E File Offset: 0x001CA69E
		private void OnReportBugButtonPressed(NButton _)
		{
			TaskHelper.RunSafely(this.OpenFeedbackScreen());
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x001CC4AC File Offset: 0x001CA6AC
		private async Task OpenFeedbackScreen()
		{
			SceneTree sceneTree = base.GetTree();
			this.QueueFreeSafely();
			await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
			await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
			await NFeedbackScreenOpener.Instance.OpenFeedbackScreen();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C20 RID: 15392 RVA: 0x001CC4F0 File Offset: 0x001CA6F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NErrorPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NErrorPopup.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "title", 0L, "", 6L, false),
					new PropertyInfo(4L, "body", 0L, "", 6L, false),
					new PropertyInfo(1L, "showReportBugButton", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NErrorPopup.MethodName.OnOkButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NErrorPopup.MethodName.OnCancelButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NErrorPopup.MethodName.OnReportBugButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C21 RID: 15393 RVA: 0x001CC6F4 File Offset: 0x001CA8F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NErrorPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NErrorPopup.MethodName.Create && args.Count == 3)
			{
				NErrorPopup nerrorPopup = NErrorPopup.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NErrorPopup>(ref nerrorPopup);
				return true;
			}
			if ((ref method) == NErrorPopup.MethodName.OnOkButtonPressed && args.Count == 1)
			{
				this.OnOkButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NErrorPopup.MethodName.OnCancelButtonPressed && args.Count == 1)
			{
				this.OnCancelButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NErrorPopup.MethodName.OnReportBugButtonPressed && args.Count == 1)
			{
				this.OnReportBugButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x001CC81C File Offset: 0x001CAA1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NErrorPopup.MethodName.Create && args.Count == 3)
			{
				NErrorPopup nerrorPopup = NErrorPopup.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NErrorPopup>(ref nerrorPopup);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003C23 RID: 15395 RVA: 0x001CC884 File Offset: 0x001CAA84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NErrorPopup.MethodName._Ready || (ref method) == NErrorPopup.MethodName.Create || (ref method) == NErrorPopup.MethodName.OnOkButtonPressed || (ref method) == NErrorPopup.MethodName.OnCancelButtonPressed || (ref method) == NErrorPopup.MethodName.OnReportBugButtonPressed || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003C24 RID: 15396 RVA: 0x001CC8E4 File Offset: 0x001CAAE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NErrorPopup.PropertyName._verticalPopup)
			{
				this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			if ((ref name) == NErrorPopup.PropertyName._title)
			{
				this._title = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NErrorPopup.PropertyName._body)
			{
				this._body = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NErrorPopup.PropertyName._showReportBugButton)
			{
				this._showReportBugButton = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003C25 RID: 15397 RVA: 0x001CC968 File Offset: 0x001CAB68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NErrorPopup.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NErrorPopup.PropertyName._verticalPopup)
			{
				value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
				return true;
			}
			if ((ref name) == NErrorPopup.PropertyName._title)
			{
				value = VariantUtils.CreateFrom<string>(ref this._title);
				return true;
			}
			if ((ref name) == NErrorPopup.PropertyName._body)
			{
				value = VariantUtils.CreateFrom<string>(ref this._body);
				return true;
			}
			if ((ref name) == NErrorPopup.PropertyName._showReportBugButton)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._showReportBugButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C26 RID: 15398 RVA: 0x001CCA20 File Offset: 0x001CAC20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NErrorPopup.PropertyName._verticalPopup, 0L, "", 4096L, false),
				new PropertyInfo(4L, NErrorPopup.PropertyName._title, 0L, "", 4096L, false),
				new PropertyInfo(4L, NErrorPopup.PropertyName._body, 0L, "", 4096L, false),
				new PropertyInfo(1L, NErrorPopup.PropertyName._showReportBugButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NErrorPopup.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C27 RID: 15399 RVA: 0x001CCAD8 File Offset: 0x001CACD8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NErrorPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
			info.AddProperty(NErrorPopup.PropertyName._title, Variant.From<string>(ref this._title));
			info.AddProperty(NErrorPopup.PropertyName._body, Variant.From<string>(ref this._body));
			info.AddProperty(NErrorPopup.PropertyName._showReportBugButton, Variant.From<bool>(ref this._showReportBugButton));
		}

		/// <inheritdoc />
		// Token: 0x06003C28 RID: 15400 RVA: 0x001CCB44 File Offset: 0x001CAD44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NErrorPopup.PropertyName._verticalPopup, ref variant))
			{
				this._verticalPopup = variant.As<NVerticalPopup>();
			}
			Variant variant2;
			if (info.TryGetProperty(NErrorPopup.PropertyName._title, ref variant2))
			{
				this._title = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NErrorPopup.PropertyName._body, ref variant3))
			{
				this._body = variant3.As<string>();
			}
			Variant variant4;
			if (info.TryGetProperty(NErrorPopup.PropertyName._showReportBugButton, ref variant4))
			{
				this._showReportBugButton = variant4.As<bool>();
			}
		}

		// Token: 0x0400177A RID: 6010
		private NVerticalPopup _verticalPopup;

		// Token: 0x0400177B RID: 6011
		private string _title;

		// Token: 0x0400177C RID: 6012
		private string _body;

		// Token: 0x0400177D RID: 6013
		[Nullable(2)]
		private LocString _cancel;

		// Token: 0x0400177E RID: 6014
		private bool _showReportBugButton;

		// Token: 0x0400177F RID: 6015
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/error_popup");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001846 RID: 6214
		[NullableContext(0)]
		public new class MethodName : NVerticalPopup.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F0D RID: 24333
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005F0E RID: 24334
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'OnOkButtonPressed' method.
			/// </summary>
			// Token: 0x04005F0F RID: 24335
			public static readonly StringName OnOkButtonPressed = "OnOkButtonPressed";

			/// <summary>
			/// Cached name for the 'OnCancelButtonPressed' method.
			/// </summary>
			// Token: 0x04005F10 RID: 24336
			public static readonly StringName OnCancelButtonPressed = "OnCancelButtonPressed";

			/// <summary>
			/// Cached name for the 'OnReportBugButtonPressed' method.
			/// </summary>
			// Token: 0x04005F11 RID: 24337
			public static readonly StringName OnReportBugButtonPressed = "OnReportBugButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001847 RID: 6215
		[NullableContext(0)]
		public new class PropertyName : NVerticalPopup.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005F12 RID: 24338
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_verticalPopup' field.
			/// </summary>
			// Token: 0x04005F13 RID: 24339
			public static readonly StringName _verticalPopup = "_verticalPopup";

			/// <summary>
			/// Cached name for the '_title' field.
			/// </summary>
			// Token: 0x04005F14 RID: 24340
			public static readonly StringName _title = "_title";

			/// <summary>
			/// Cached name for the '_body' field.
			/// </summary>
			// Token: 0x04005F15 RID: 24341
			public static readonly StringName _body = "_body";

			/// <summary>
			/// Cached name for the '_showReportBugButton' field.
			/// </summary>
			// Token: 0x04005F16 RID: 24342
			public static readonly StringName _showReportBugButton = "_showReportBugButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001848 RID: 6216
		[NullableContext(0)]
		public new class SignalName : NVerticalPopup.SignalName
		{
		}
	}
}
