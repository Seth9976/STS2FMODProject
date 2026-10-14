using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x0200032B RID: 811
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLeaderboard.cs")]
	public class NDailyRunLeaderboard : Control
	{
		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x001442DA File Offset: 0x001424DA
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NDailyRunLeaderboard._scenePath };
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x001442EC File Offset: 0x001424EC
		public override void _Ready()
		{
			this._paginator = base.GetNode<NLeaderboardDayPaginator>("Paginator");
			VBoxContainer vboxContainer;
			if ((vboxContainer = base.GetNodeOrNull<VBoxContainer>("%ScoreContainer")) == null && (vboxContainer = base.GetNodeOrNull<VBoxContainer>("%LeaderboardScoreContainer")) == null)
			{
				throw new InvalidOperationException("Couldn't find score container");
			}
			this._scoreContainer = vboxContainer;
			this._leftArrow = base.GetNode<NLeaderboardPageArrow>("%LeftArrow");
			this._rightArrow = base.GetNode<NLeaderboardPageArrow>("%RightArrow");
			this._loadingIndicator = base.GetNode<MegaRichTextLabel>("%LoadingText");
			this._noScoresIndicator = base.GetNode<MegaLabel>("%NoScoresIndicator");
			this._noFriendsIndicator = base.GetNode<MegaLabel>("%NoFriendsIndicator");
			this._noScoreUploadIndicator = base.GetNodeOrNull<Control>("%ScoreWarning");
			this._separators = base.GetNode<Control>("%Separators");
			this._globalTickbox = base.GetNode<NGlobalRankTickbox>("%GlobalTickbox");
			this._globalTickbox.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleGlobalRank)), 0U);
			this._globalTickbox.SetLabel(new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.globalTickbox").GetRawText());
			base.CallDeferred(NDailyRunLeaderboard.MethodName.SetLocalizedText, Array.Empty<Variant>());
			this._loadingIndicator.SetTextAutoSize(NDailyRunLeaderboard._fetchingScoreLoc.GetFormattedText());
			this._rightArrow.Connect(delegate
			{
				this.ChangePage(this._currentIndex += 10);
			});
			this._leftArrow.Connect(delegate
			{
				this.ChangePage(this._currentIndex -= 10);
			});
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x00144490 File Offset: 0x00142690
		private void ToggleGlobalRank(NTickbox tickbox)
		{
			if (!tickbox.IsTicked)
			{
				this.SetPage(0);
				return;
			}
			DateTimeOffset? leaderboardTime = this._leaderboardTime;
			if (leaderboardTime != null)
			{
				DateTimeOffset valueOrDefault = leaderboardTime.GetValueOrDefault();
				this._currentIndex = -4;
				TaskHelper.RunSafely(this.LoadLeaderboard(valueOrDefault, this._currentPage, LeaderboardQueryType.AroundUser));
				return;
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x001444E4 File Offset: 0x001426E4
		private void SetLocalizedText()
		{
			MegaLabel nodeOrNull = base.GetNodeOrNull<MegaLabel>("%Title");
			if (nodeOrNull != null)
			{
				nodeOrNull.SetTextAutoSize(NDailyRunLeaderboard._titleLoc.GetRawText());
			}
			this._noScoresIndicator.SetTextAutoSize(NDailyRunLeaderboard._scoreLoc.GetRawText());
			this._noFriendsIndicator.SetTextAutoSize(NDailyRunLeaderboard._friendsLoc.GetRawText());
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x00144540 File Offset: 0x00142740
		public void Cleanup()
		{
			CancellationTokenSource loadCts = this._loadCts;
			if (loadCts != null)
			{
				loadCts.Cancel();
			}
			this._loadCts = null;
			this._leftArrow.Visible = false;
			this._rightArrow.Visible = false;
			this._loadingIndicator.Visible = false;
			this._noScoresIndicator.Visible = false;
			this._noFriendsIndicator.Visible = false;
			this._paginator.Visible = false;
			this._globalTickbox.Visible = false;
			this._currentIndex = -4;
			this._leaderboardTime = null;
			if (this._noScoreUploadIndicator != null)
			{
				this._noScoreUploadIndicator.Visible = false;
			}
			this.ClearEntries();
		}

		/// <summary>
		/// Does first-time initialization with the given day. Should only be called when the page loads and when a new
		/// player joins the run.
		/// </summary>
		// Token: 0x06002D46 RID: 11590 RVA: 0x001445E8 File Offset: 0x001427E8
		public void Initialize(DateTimeOffset dateTime, IEnumerable<ulong> playersInRun, bool allowPagination)
		{
			this._playersInRun.Clear();
			this._playersInRun.AddRange(playersInRun);
			this._paginator.Initialize(this, dateTime, allowPagination);
			this._paginator.Visible = true;
			this._todaysDailyTime = dateTime;
			this.SetDay(dateTime);
		}

		/// <summary>
		/// Called by the paginator when the day in the paginator changes.
		/// </summary>
		// Token: 0x06002D47 RID: 11591 RVA: 0x00144634 File Offset: 0x00142834
		public void SetDay(DateTimeOffset dateTime)
		{
			this._leaderboardTime = new DateTimeOffset?(dateTime);
			if (!this._globalTickbox.IsTicked)
			{
				this.SetPage(0);
				return;
			}
			TaskHelper.RunSafely(this.LoadLeaderboard(dateTime, this._currentPage, LeaderboardQueryType.AroundUser));
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x0014466B File Offset: 0x0014286B
		private void ChangePage(int _ = 0)
		{
			this.NavigateGlobalRank();
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x00144674 File Offset: 0x00142874
		private void SetPage(int page)
		{
			DateTimeOffset? leaderboardTime = this._leaderboardTime;
			if (leaderboardTime != null)
			{
				DateTimeOffset valueOrDefault = leaderboardTime.GetValueOrDefault();
				this._currentPage = page;
				TaskHelper.RunSafely(this.LoadLeaderboard(valueOrDefault, this._currentPage, LeaderboardQueryType.FriendsOnly));
				return;
			}
		}

		/// <summary>
		/// Called when we press left/right arrows when viewing Global Rank.
		/// Lets other players view the scores and names of other players ranked above/below them.
		/// </summary>
		// Token: 0x06002D4A RID: 11594 RVA: 0x001446B8 File Offset: 0x001428B8
		private void NavigateGlobalRank()
		{
			DateTimeOffset? leaderboardTime = this._leaderboardTime;
			if (leaderboardTime != null)
			{
				DateTimeOffset valueOrDefault = leaderboardTime.GetValueOrDefault();
				TaskHelper.RunSafely(this.LoadLeaderboard(valueOrDefault, this._currentPage, LeaderboardQueryType.AroundUser));
				return;
			}
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x001446F4 File Offset: 0x001428F4
		private Task LoadLeaderboard(DateTimeOffset dateTime, int page, LeaderboardQueryType queryType)
		{
			NDailyRunLeaderboard.<LoadLeaderboard>d__35 <LoadLeaderboard>d__;
			<LoadLeaderboard>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadLeaderboard>d__.<>4__this = this;
			<LoadLeaderboard>d__.dateTime = dateTime;
			<LoadLeaderboard>d__.page = page;
			<LoadLeaderboard>d__.queryType = queryType;
			<LoadLeaderboard>d__.<>1__state = -1;
			<LoadLeaderboard>d__.<>t__builder.Start<NDailyRunLeaderboard.<LoadLeaderboard>d__35>(ref <LoadLeaderboard>d__);
			return <LoadLeaderboard>d__.<>t__builder.Task;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x00144750 File Offset: 0x00142950
		private async Task QueryFriendScores(ILeaderboardHandle handle, int page, CancellationToken ct)
		{
			List<LeaderboardEntry> list = await LeaderboardManager.QueryLeaderboard(handle, LeaderboardQueryType.FriendsOnly, page * 10, 10, ct);
			List<LeaderboardEntry> list2 = list;
			if (!ct.IsCancellationRequested)
			{
				if (list2.Count > 0)
				{
					this._noScoresIndicator.Visible = false;
					this._globalTickbox.Visible = true;
					this._separators.Visible = true;
				}
				else
				{
					this._noScoresIndicator.Visible = true;
					this._globalTickbox.Visible = false;
					this._separators.Visible = false;
				}
				this.FillEntries(list2);
				this._leftArrow.Visible = false;
				this._rightArrow.Visible = false;
			}
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x001447AC File Offset: 0x001429AC
		private async Task QueryGlobalScores(ILeaderboardHandle handle, CancellationToken ct)
		{
			List<LeaderboardEntry> list = await LeaderboardManager.QueryLeaderboard(handle, LeaderboardQueryType.AroundUser, this._currentIndex, 10, ct);
			List<LeaderboardEntry> list2 = list;
			if (!ct.IsCancellationRequested)
			{
				if (list2.Count > 0)
				{
					this._noScoresIndicator.Visible = false;
					this._globalTickbox.Visible = true;
					this._separators.Visible = true;
				}
				else
				{
					this._noScoresIndicator.Visible = true;
					this._globalTickbox.Visible = false;
					this._separators.Visible = false;
				}
				this.FillEntries(list2);
				if (list2.Count > 0)
				{
					int leaderboardEntryCount = LeaderboardManager.GetLeaderboardEntryCount(handle);
					this._leftArrow.Visible = true;
					this._rightArrow.Visible = true;
					if (list2[0].rank > 0)
					{
						this._leftArrow.Enable();
					}
					else
					{
						this._leftArrow.Disable();
					}
					List<LeaderboardEntry> list3 = list2;
					if (list3[list3.Count - 1].rank < leaderboardEntryCount - 1 && !this._hasNegativeScore)
					{
						this._rightArrow.Enable();
					}
					else
					{
						this._rightArrow.Disable();
					}
				}
			}
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x00144800 File Offset: 0x00142A00
		private void FillEntries(List<LeaderboardEntry> entries)
		{
			if (entries.Count == 0)
			{
				return;
			}
			this._hasNegativeScore = false;
			NDailyRunLeaderboardHeader ndailyRunLeaderboardHeader = NDailyRunLeaderboardHeader.Create();
			this._scoreContainer.AddChildSafely(ndailyRunLeaderboardHeader);
			NDailyRunLeaderboardSeparator ndailyRunLeaderboardSeparator = NDailyRunLeaderboardSeparator.Create();
			this._scoreContainer.AddChildSafely(ndailyRunLeaderboardSeparator);
			ulong localPlayerId = PlatformUtil.GetLocalPlayerId(LeaderboardManager.CurrentPlatform);
			foreach (LeaderboardEntry leaderboardEntry in entries)
			{
				if (leaderboardEntry.score < 0)
				{
					this._hasNegativeScore = true;
				}
				else
				{
					this._scoreContainer.AddChildSafely(NDailyRunLeaderboardRow.Create(leaderboardEntry, leaderboardEntry.userIds.Contains(localPlayerId)));
					this._scoreContainer.AddChildSafely(NDailyRunLeaderboardSeparator.Create());
				}
			}
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x001448CC File Offset: 0x00142ACC
		private void ClearEntries()
		{
			this._noScoresIndicator.Visible = false;
			this._noFriendsIndicator.Visible = false;
			foreach (Node node in this._scoreContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x00144938 File Offset: 0x00142B38
		public override void _ExitTree()
		{
			CancellationTokenSource loadCts = this._loadCts;
			if (loadCts != null)
			{
				loadCts.Cancel();
			}
			this._loadCts = null;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D51 RID: 11601 RVA: 0x00144954 File Offset: 0x00142B54
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NDailyRunLeaderboard.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName.ToggleGlobalRank, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName.SetLocalizedText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName.ChangePage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName.SetPage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "page", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName.NavigateGlobalRank, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName.ClearEntries, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunLeaderboard.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D52 RID: 11602 RVA: 0x00144B8C File Offset: 0x00142D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunLeaderboard.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName.ToggleGlobalRank && args.Count == 1)
			{
				this.ToggleGlobalRank(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName.SetLocalizedText && args.Count == 0)
			{
				this.SetLocalizedText();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName.ChangePage && args.Count == 1)
			{
				this.ChangePage(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName.SetPage && args.Count == 1)
			{
				this.SetPage(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName.NavigateGlobalRank && args.Count == 0)
			{
				this.NavigateGlobalRank();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName.ClearEntries && args.Count == 0)
			{
				this.ClearEntries();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunLeaderboard.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002D53 RID: 11603 RVA: 0x00144D1C File Offset: 0x00142F1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunLeaderboard.MethodName._Ready || (ref method) == NDailyRunLeaderboard.MethodName.ToggleGlobalRank || (ref method) == NDailyRunLeaderboard.MethodName.SetLocalizedText || (ref method) == NDailyRunLeaderboard.MethodName.Cleanup || (ref method) == NDailyRunLeaderboard.MethodName.ChangePage || (ref method) == NDailyRunLeaderboard.MethodName.SetPage || (ref method) == NDailyRunLeaderboard.MethodName.NavigateGlobalRank || (ref method) == NDailyRunLeaderboard.MethodName.ClearEntries || (ref method) == NDailyRunLeaderboard.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002D54 RID: 11604 RVA: 0x00144DB8 File Offset: 0x00142FB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDailyRunLeaderboard.PropertyName._paginator)
			{
				this._paginator = VariantUtils.ConvertTo<NLeaderboardDayPaginator>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._scoreContainer)
			{
				this._scoreContainer = VariantUtils.ConvertTo<VBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._leftArrow)
			{
				this._leftArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._rightArrow)
			{
				this._rightArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._loadingIndicator)
			{
				this._loadingIndicator = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._noScoresIndicator)
			{
				this._noScoresIndicator = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._noFriendsIndicator)
			{
				this._noFriendsIndicator = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator)
			{
				this._noScoreUploadIndicator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._separators)
			{
				this._separators = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._globalTickbox)
			{
				this._globalTickbox = VariantUtils.ConvertTo<NGlobalRankTickbox>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._currentPage)
			{
				this._currentPage = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._currentIndex)
			{
				this._currentIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._hasNegativeScore)
			{
				this._hasNegativeScore = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002D55 RID: 11605 RVA: 0x00144F2C File Offset: 0x0014312C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDailyRunLeaderboard.PropertyName._paginator)
			{
				value = VariantUtils.CreateFrom<NLeaderboardDayPaginator>(ref this._paginator);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._scoreContainer)
			{
				value = VariantUtils.CreateFrom<VBoxContainer>(ref this._scoreContainer);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._leftArrow)
			{
				value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._leftArrow);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._rightArrow)
			{
				value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._rightArrow);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._loadingIndicator)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._loadingIndicator);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._noScoresIndicator)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._noScoresIndicator);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._noFriendsIndicator)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._noFriendsIndicator);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._noScoreUploadIndicator);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._separators)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._separators);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._globalTickbox)
			{
				value = VariantUtils.CreateFrom<NGlobalRankTickbox>(ref this._globalTickbox);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._currentPage)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentPage);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._currentIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentIndex);
				return true;
			}
			if ((ref name) == NDailyRunLeaderboard.PropertyName._hasNegativeScore)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._hasNegativeScore);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D56 RID: 11606 RVA: 0x001450E4 File Offset: 0x001432E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._paginator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._scoreContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._leftArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._rightArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._loadingIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._noScoresIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._noFriendsIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._separators, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunLeaderboard.PropertyName._globalTickbox, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDailyRunLeaderboard.PropertyName._currentPage, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDailyRunLeaderboard.PropertyName._currentIndex, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDailyRunLeaderboard.PropertyName._hasNegativeScore, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D57 RID: 11607 RVA: 0x001452A4 File Offset: 0x001434A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDailyRunLeaderboard.PropertyName._paginator, Variant.From<NLeaderboardDayPaginator>(ref this._paginator));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._scoreContainer, Variant.From<VBoxContainer>(ref this._scoreContainer));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._leftArrow, Variant.From<NLeaderboardPageArrow>(ref this._leftArrow));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._rightArrow, Variant.From<NLeaderboardPageArrow>(ref this._rightArrow));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._loadingIndicator, Variant.From<MegaRichTextLabel>(ref this._loadingIndicator));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._noScoresIndicator, Variant.From<MegaLabel>(ref this._noScoresIndicator));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._noFriendsIndicator, Variant.From<MegaLabel>(ref this._noFriendsIndicator));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator, Variant.From<Control>(ref this._noScoreUploadIndicator));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._separators, Variant.From<Control>(ref this._separators));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._globalTickbox, Variant.From<NGlobalRankTickbox>(ref this._globalTickbox));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._currentPage, Variant.From<int>(ref this._currentPage));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._currentIndex, Variant.From<int>(ref this._currentIndex));
			info.AddProperty(NDailyRunLeaderboard.PropertyName._hasNegativeScore, Variant.From<bool>(ref this._hasNegativeScore));
		}

		/// <inheritdoc />
		// Token: 0x06002D58 RID: 11608 RVA: 0x001453D8 File Offset: 0x001435D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._paginator, ref variant))
			{
				this._paginator = variant.As<NLeaderboardDayPaginator>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._scoreContainer, ref variant2))
			{
				this._scoreContainer = variant2.As<VBoxContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._leftArrow, ref variant3))
			{
				this._leftArrow = variant3.As<NLeaderboardPageArrow>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._rightArrow, ref variant4))
			{
				this._rightArrow = variant4.As<NLeaderboardPageArrow>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._loadingIndicator, ref variant5))
			{
				this._loadingIndicator = variant5.As<MegaRichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._noScoresIndicator, ref variant6))
			{
				this._noScoresIndicator = variant6.As<MegaLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._noFriendsIndicator, ref variant7))
			{
				this._noFriendsIndicator = variant7.As<MegaLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator, ref variant8))
			{
				this._noScoreUploadIndicator = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._separators, ref variant9))
			{
				this._separators = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._globalTickbox, ref variant10))
			{
				this._globalTickbox = variant10.As<NGlobalRankTickbox>();
			}
			Variant variant11;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._currentPage, ref variant11))
			{
				this._currentPage = variant11.As<int>();
			}
			Variant variant12;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._currentIndex, ref variant12))
			{
				this._currentIndex = variant12.As<int>();
			}
			Variant variant13;
			if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._hasNegativeScore, ref variant13))
			{
				this._hasNegativeScore = variant13.As<bool>();
			}
		}

		// Token: 0x04001146 RID: 4422
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_leaderboard");

		// Token: 0x04001147 RID: 4423
		private const int _maxEntries = 10;

		// Token: 0x04001148 RID: 4424
		private NLeaderboardDayPaginator _paginator;

		// Token: 0x04001149 RID: 4425
		private VBoxContainer _scoreContainer;

		// Token: 0x0400114A RID: 4426
		private NLeaderboardPageArrow _leftArrow;

		// Token: 0x0400114B RID: 4427
		private NLeaderboardPageArrow _rightArrow;

		// Token: 0x0400114C RID: 4428
		private MegaRichTextLabel _loadingIndicator;

		// Token: 0x0400114D RID: 4429
		private MegaLabel _noScoresIndicator;

		// Token: 0x0400114E RID: 4430
		private MegaLabel _noFriendsIndicator;

		// Token: 0x0400114F RID: 4431
		[Nullable(2)]
		private Control _noScoreUploadIndicator;

		// Token: 0x04001150 RID: 4432
		private Control _separators;

		// Token: 0x04001151 RID: 4433
		private NGlobalRankTickbox _globalTickbox;

		// Token: 0x04001152 RID: 4434
		private const int _defaultGlobalIndex = -4;

		// Token: 0x04001153 RID: 4435
		private int _currentPage;

		// Token: 0x04001154 RID: 4436
		private int _currentIndex = -4;

		// Token: 0x04001155 RID: 4437
		private DateTimeOffset _todaysDailyTime;

		// Token: 0x04001156 RID: 4438
		private DateTimeOffset? _leaderboardTime;

		// Token: 0x04001157 RID: 4439
		private readonly List<ulong> _playersInRun = new List<ulong>();

		// Token: 0x04001158 RID: 4440
		private bool _hasNegativeScore;

		// Token: 0x04001159 RID: 4441
		[Nullable(2)]
		private CancellationTokenSource _loadCts;

		// Token: 0x0400115A RID: 4442
		private static readonly LocString _titleLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.title");

		// Token: 0x0400115B RID: 4443
		private static readonly LocString _scoreLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.noScore");

		// Token: 0x0400115C RID: 4444
		private static readonly LocString _fetchingScoreLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.fetchingScores");

		// Token: 0x0400115D RID: 4445
		private static readonly LocString _friendsLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.noFriends");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200158C RID: 5516
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400529F RID: 21151
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ToggleGlobalRank' method.
			/// </summary>
			// Token: 0x040052A0 RID: 21152
			public static readonly StringName ToggleGlobalRank = "ToggleGlobalRank";

			/// <summary>
			/// Cached name for the 'SetLocalizedText' method.
			/// </summary>
			// Token: 0x040052A1 RID: 21153
			public static readonly StringName SetLocalizedText = "SetLocalizedText";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x040052A2 RID: 21154
			public static readonly StringName Cleanup = "Cleanup";

			/// <summary>
			/// Cached name for the 'ChangePage' method.
			/// </summary>
			// Token: 0x040052A3 RID: 21155
			public static readonly StringName ChangePage = "ChangePage";

			/// <summary>
			/// Cached name for the 'SetPage' method.
			/// </summary>
			// Token: 0x040052A4 RID: 21156
			public static readonly StringName SetPage = "SetPage";

			/// <summary>
			/// Cached name for the 'NavigateGlobalRank' method.
			/// </summary>
			// Token: 0x040052A5 RID: 21157
			public static readonly StringName NavigateGlobalRank = "NavigateGlobalRank";

			/// <summary>
			/// Cached name for the 'ClearEntries' method.
			/// </summary>
			// Token: 0x040052A6 RID: 21158
			public static readonly StringName ClearEntries = "ClearEntries";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040052A7 RID: 21159
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200158D RID: 5517
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_paginator' field.
			/// </summary>
			// Token: 0x040052A8 RID: 21160
			public static readonly StringName _paginator = "_paginator";

			/// <summary>
			/// Cached name for the '_scoreContainer' field.
			/// </summary>
			// Token: 0x040052A9 RID: 21161
			public static readonly StringName _scoreContainer = "_scoreContainer";

			/// <summary>
			/// Cached name for the '_leftArrow' field.
			/// </summary>
			// Token: 0x040052AA RID: 21162
			public static readonly StringName _leftArrow = "_leftArrow";

			/// <summary>
			/// Cached name for the '_rightArrow' field.
			/// </summary>
			// Token: 0x040052AB RID: 21163
			public static readonly StringName _rightArrow = "_rightArrow";

			/// <summary>
			/// Cached name for the '_loadingIndicator' field.
			/// </summary>
			// Token: 0x040052AC RID: 21164
			public static readonly StringName _loadingIndicator = "_loadingIndicator";

			/// <summary>
			/// Cached name for the '_noScoresIndicator' field.
			/// </summary>
			// Token: 0x040052AD RID: 21165
			public static readonly StringName _noScoresIndicator = "_noScoresIndicator";

			/// <summary>
			/// Cached name for the '_noFriendsIndicator' field.
			/// </summary>
			// Token: 0x040052AE RID: 21166
			public static readonly StringName _noFriendsIndicator = "_noFriendsIndicator";

			/// <summary>
			/// Cached name for the '_noScoreUploadIndicator' field.
			/// </summary>
			// Token: 0x040052AF RID: 21167
			public static readonly StringName _noScoreUploadIndicator = "_noScoreUploadIndicator";

			/// <summary>
			/// Cached name for the '_separators' field.
			/// </summary>
			// Token: 0x040052B0 RID: 21168
			public static readonly StringName _separators = "_separators";

			/// <summary>
			/// Cached name for the '_globalTickbox' field.
			/// </summary>
			// Token: 0x040052B1 RID: 21169
			public static readonly StringName _globalTickbox = "_globalTickbox";

			/// <summary>
			/// Cached name for the '_currentPage' field.
			/// </summary>
			// Token: 0x040052B2 RID: 21170
			public static readonly StringName _currentPage = "_currentPage";

			/// <summary>
			/// Cached name for the '_currentIndex' field.
			/// </summary>
			// Token: 0x040052B3 RID: 21171
			public static readonly StringName _currentIndex = "_currentIndex";

			/// <summary>
			/// Cached name for the '_hasNegativeScore' field.
			/// </summary>
			// Token: 0x040052B4 RID: 21172
			public static readonly StringName _hasNegativeScore = "_hasNegativeScore";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200158E RID: 5518
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
