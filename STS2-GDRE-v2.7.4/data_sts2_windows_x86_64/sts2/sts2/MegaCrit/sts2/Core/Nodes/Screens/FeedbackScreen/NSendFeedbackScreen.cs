using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using Sentry;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	// Token: 0x02000328 RID: 808
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackScreen.cs")]
	public class NSendFeedbackScreen : Control, IScreenContext
	{
		// Token: 0x06002D0F RID: 11535 RVA: 0x00141638 File Offset: 0x0013F838
		[NullableContext(2)]
		public static NSendFeedbackScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NSendFeedbackScreen._scenePath).Instantiate<NSendFeedbackScreen>(0L);
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x0014165C File Offset: 0x0013F85C
		public unsafe override void _Ready()
		{
			this._mainPanel = base.GetNode<Control>("%MainPanel");
			this._descriptionInput = base.GetNode<NMegaTextEdit>("%DescriptionInput");
			this._emojiLabel = base.GetNode<MegaLabel>("%EmojiLabel");
			this._sendButton = base.GetNode<NButton>("%SendButton");
			this._returnToGameButton = base.GetNode<NButton>("%ReturnToGameButton");
			this._returnToGameLabel = this._returnToGameButton.GetNode<MegaLabel>("Label");
			this._returnToGameHoverLabel = this._returnToGameButton.GetNode<MegaLabel>("%ReturnToGameHoverLabel");
			this._sendLabel = this._sendButton.GetNode<MegaLabel>("Label");
			this._categoryLabel = base.GetNode<MegaLabel>("%CategoryLabel");
			this._categoryDropdown = base.GetNode<NFeedbackCategoryDropdown>("%CategoryDropdown");
			this._sendBackstop = base.GetNode<Control>("%SendBackstop");
			this._sendPanel = base.GetNode<Control>("%SendPanel");
			this._successLabel = base.GetNode<MegaLabel>("%SuccessLabel");
			this._failedLabel = base.GetNode<MegaLabel>("%FailedLabel");
			this._sendingLabel = base.GetNode<MegaRichTextLabel>("%SendingLabel");
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._successLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SEND_SUCCESS_LABEL").GetFormattedText());
			this._failedLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SEND_FAILED_LABEL").GetFormattedText());
			this._sendingLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SENDING_LABEL").GetFormattedText());
			this._returnToGameLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_RUN_IN_BACKGROUND_LABEL").GetFormattedText());
			this._returnToGameHoverLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_RUN_IN_BACKGROUND_HOVER_LABEL").GetFormattedText());
			this._originalSuccessPosition = this._sendPanel.Position;
			int num = 3;
			List<NSendFeedbackCartoon> list = new List<NSendFeedbackCartoon>(num);
			CollectionsMarshal.SetCount<NSendFeedbackCartoon>(list, num);
			Span<NSendFeedbackCartoon> span = CollectionsMarshal.AsSpan<NSendFeedbackCartoon>(list);
			int num2 = 0;
			*span[num2] = base.GetNode<NSendFeedbackCartoon>("Sun");
			num2++;
			*span[num2] = base.GetNode<NSendFeedbackCartoon>("Cupcake");
			num2++;
			*span[num2] = base.GetNode<NSendFeedbackCartoon>("FlowerContainer/Flower");
			this._cartoons = list;
			this._flower = base.GetNode<NSendFeedbackFlower>("FlowerContainer");
			foreach (Node node in base.GetNode("%EmojiButtonContainer").GetChildren(false))
			{
				NSendFeedbackEmojiButton nsendFeedbackEmojiButton = node as NSendFeedbackEmojiButton;
				if (nsendFeedbackEmojiButton != null)
				{
					this._emojiButtons.Add(nsendFeedbackEmojiButton);
					nsendFeedbackEmojiButton.PivotOffset = nsendFeedbackEmojiButton.Size * 0.5f;
					nsendFeedbackEmojiButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.EmojiButtonSelected)), 0U);
				}
			}
			this._sendButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SendButtonSelected)), 0U);
			this._sendButton.Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.SendButtonFocused)), 0U);
			this._sendButton.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.SendButtonUnfocused)), 0U);
			this._returnToGameButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ReturnToGameSelected)), 0U);
			this._returnToGameButton.Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.ReturnToGameFocused)), 0U);
			this._returnToGameButton.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.ReturnToGameUnfocused)), 0U);
			this._descriptionInput.Connect(TextEdit.SignalName.TextChanged, Callable.From(new Action(this.OnDescriptionChanged)), 0U);
			this._returnToGameHoverLabel.Visible = false;
			this._sendButton.FocusNeighborTop = this._categoryDropdown.GetPath();
			this._sendButton.FocusNeighborLeft = this._emojiButtons.Last<NSendFeedbackEmojiButton>().GetPath();
			this._sendButton.FocusNeighborBottom = this._sendButton.GetPath();
			this._sendButton.FocusNeighborRight = this._sendButton.GetPath();
			this._emojiButtons.Last<NSendFeedbackEmojiButton>().FocusNeighborRight = this._sendButton.GetPath();
			foreach (NSendFeedbackEmojiButton nsendFeedbackEmojiButton2 in this._emojiButtons)
			{
				nsendFeedbackEmojiButton2.FocusNeighborTop = this._categoryDropdown.GetPath();
				nsendFeedbackEmojiButton2.FocusNeighborBottom = nsendFeedbackEmojiButton2.GetPath();
			}
			this._categoryDropdown.FocusNeighborRight = this._sendButton.GetPath();
			this._categoryDropdown.FocusNeighborBottom = this._emojiButtons.First<NSendFeedbackEmojiButton>().GetPath();
			this._categoryDropdown.FocusNeighborTop = this._descriptionInput.GetPath();
			this._descriptionInput.FocusNeighborTop = this._descriptionInput.GetPath();
			this._descriptionInput.FocusNeighborLeft = this._descriptionInput.GetPath();
			this._descriptionInput.FocusNeighborRight = this._descriptionInput.GetPath();
			this._descriptionInput.FocusNeighborBottom = this._categoryDropdown.GetPath();
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.Close();
			}), 0U);
			base.Visible = false;
			base.MouseFilter = 2L;
			this._backButton.Disable();
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x00141C58 File Offset: 0x0013FE58
		public void Relocalize()
		{
			this._descriptionInput.PlaceholderText = new LocString("settings_ui", "FEEDBACK_DESCRIPTION_PLACEHOLDER").GetFormattedText();
			this._categoryLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_CATEGORY_LABEL").GetFormattedText());
			this._emojiLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_EMOJI_LABEL").GetFormattedText());
			this._sendLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SEND_BUTTON_LABEL").GetFormattedText());
			this._descriptionInput.RefreshFont();
			this._categoryLabel.RefreshFont();
			this._emojiLabel.RefreshFont();
			this._sendLabel.RefreshFont();
		}

		/// <summary>
		/// Used to limit the number of characters that can be typed into the description box.
		/// </summary>
		// Token: 0x06002D12 RID: 11538 RVA: 0x00141D10 File Offset: 0x0013FF10
		private void OnDescriptionChanged()
		{
			if (this._descriptionInput.Text.Length > 8000)
			{
				this._descriptionInput.Text = this._descriptionText;
				this._descriptionInput.SetCaretLine(this._descriptionCaretLine, true, true, 0, 0);
				this._descriptionInput.SetCaretColumn(this._descriptionCaretColumn, true, 0);
				return;
			}
			this._descriptionText = this._descriptionInput.Text;
			this._descriptionCaretLine = this._descriptionInput.GetCaretLine(0);
			this._descriptionCaretColumn = this._descriptionInput.GetCaretColumn(0);
		}

		/// <summary>
		/// Set the screenshot that will be uploaded with the feedback. The screenshot will be automatically scaled to a
		/// smaller size so that the upload isn't massive.
		/// </summary>
		/// <param name="screenshot"></param>
		// Token: 0x06002D13 RID: 11539 RVA: 0x00141DA4 File Offset: 0x0013FFA4
		public void SetScreenshot(Image screenshot)
		{
			int width = screenshot.GetWidth();
			int height = screenshot.GetHeight();
			float num = (float)width / (float)height;
			if (width > 1280)
			{
				screenshot.Resize(1280, Mathf.RoundToInt(1280f / num), 1L);
			}
			if (height > 720)
			{
				screenshot.Resize(Mathf.RoundToInt(720f * num), 720, 1L);
			}
			this._screenshotBytes = screenshot.SavePngToBuffer();
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x00141E13 File Offset: 0x00140013
		private void EmojiButtonSelected(NButton button)
		{
			this.SetSelectedEmoji((NSendFeedbackEmojiButton)button);
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x00141E21 File Offset: 0x00140021
		private void SendButtonFocused(NClickableControl _)
		{
			this._flower.SetState(NSendFeedbackFlower.State.Anticipation);
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x00141E2F File Offset: 0x0014002F
		private void SendButtonUnfocused(NClickableControl _)
		{
			if (this._flower.MyState == NSendFeedbackFlower.State.Anticipation && !this._sendBackstop.Visible)
			{
				this._flower.SetState(NSendFeedbackFlower.State.None);
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x00141E58 File Offset: 0x00140058
		public void Open()
		{
			if (base.Visible)
			{
				return;
			}
			Log.Info("Feedback screen opened", 2);
			if (Time.GetTicksMsec() - this._lastClosedMsec > 60000UL)
			{
				this.ClearInput();
			}
			this._screenClosedCancelToken = new CancellationTokenSource();
			base.Visible = true;
			this._flower.SetState(NSendFeedbackFlower.State.None);
			this._sendBackstop.Visible = false;
			this._sendButton.Enable();
			base.MouseFilter = 0L;
			NHotkeyManager.Instance.AddBlockingScreen(this);
			ActiveScreenContext.Instance.Update();
			this._backButton.Enable();
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x00141EF0 File Offset: 0x001400F0
		private void Close()
		{
			Log.Info("Feedback screen closed", 2);
			this._flower.SetState(NSendFeedbackFlower.State.None);
			this._sendBackstop.Visible = false;
			this._mainPanel.Modulate = Colors.White;
			Tween wiggleTween = this._wiggleTween;
			if (wiggleTween != null)
			{
				wiggleTween.Kill();
			}
			base.Visible = false;
			this._lastClosedMsec = Time.GetTicksMsec();
			CancellationTokenSource screenClosedCancelToken = this._screenClosedCancelToken;
			if (screenClosedCancelToken != null)
			{
				screenClosedCancelToken.Cancel();
			}
			base.MouseFilter = 2L;
			this._backButton.Disable();
			NHotkeyManager.Instance.RemoveBlockingScreen(this);
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x00141F8C File Offset: 0x0014018C
		private void ClearInput()
		{
			this._descriptionInput.Text = string.Empty;
			this._descriptionText = string.Empty;
			this.SetSelectedEmoji(null);
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x00141FB0 File Offset: 0x001401B0
		[NullableContext(2)]
		private void SetSelectedEmoji(NSendFeedbackEmojiButton button)
		{
			NSendFeedbackEmojiButton selectedEmoteButton = this._selectedEmoteButton;
			NSendFeedbackEmojiButton selectedEmoteButton2 = this._selectedEmoteButton;
			if (selectedEmoteButton2 != null)
			{
				selectedEmoteButton2.SetSelected(false);
			}
			if (selectedEmoteButton != button)
			{
				this._selectedEmoteButton = button;
				NSendFeedbackEmojiButton selectedEmoteButton3 = this._selectedEmoteButton;
				if (selectedEmoteButton3 == null)
				{
					return;
				}
				selectedEmoteButton3.SetSelected(true);
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x00141FF2 File Offset: 0x001401F2
		private void SendButtonSelected(NButton _)
		{
			TaskHelper.RunSafely(this.SendFeedbackWrapper());
			this._sendButton.Disable();
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x0014200B File Offset: 0x0014020B
		private void ReturnToGameSelected(NButton _)
		{
			TaskCompletionSource runInBackgroundTaskSource = this._runInBackgroundTaskSource;
			if (runInBackgroundTaskSource == null)
			{
				return;
			}
			runInBackgroundTaskSource.TrySetResult();
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x0014201E File Offset: 0x0014021E
		private void ReturnToGameFocused(NClickableControl _)
		{
			this._returnToGameHoverLabel.Visible = true;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x0014202C File Offset: 0x0014022C
		private void ReturnToGameUnfocused(NClickableControl _)
		{
			this._returnToGameHoverLabel.Visible = false;
		}

		/// <summary>
		/// Send feedback, allowing the player to exit the menu if they want to get back to playing.
		/// </summary>
		// Token: 0x06002D1F RID: 11551 RVA: 0x0014203C File Offset: 0x0014023C
		private async Task SendFeedbackWrapper()
		{
			if (!string.IsNullOrEmpty(this._descriptionText))
			{
				Log.Info("Beginning asynchronous feedback send at " + Log.Timestamp + ": " + this._descriptionText, 2);
				ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
				string text = ((releaseInfo != null) ? releaseInfo.Commit : null) ?? GitHelper.ShortCommitId;
				FeedbackData feedbackData = new FeedbackData
				{
					description = this._descriptionText,
					category = this._categoryDropdown.CurrentCategory,
					gameVersion = (((releaseInfo != null) ? releaseInfo.Version : null) ?? "v0.0.1"),
					uniqueId = SaveManager.Instance.Progress.UniqueId,
					commit = (text ?? "unknown"),
					platformBranch = PlatformUtil.GetPlatformBranch().ToName(),
					sessionId = SentryService.SessionId,
					isModded = (ModManager.IsRunningModded() || ModManager.HasHarmonyPatches()),
					isFullConsole = SaveManager.Instance.SettingsSave.FullConsole,
					lang = LocManager.Instance.Language
				};
				MemoryStream memoryStream = new MemoryStream(this._screenshotBytes);
				int currentProfileId = SaveManager.Instance.CurrentProfileId;
				MemoryStream memoryStream2 = new MemoryStream();
				GetLogsConsoleCmd.ZipFeedbackLogs(memoryStream2, currentProfileId);
				memoryStream2.Seek(0L, SeekOrigin.Begin);
				this._runInBackgroundTaskSource = new TaskCompletionSource();
				Task<bool> sendTask = NSendFeedbackScreen.SendFeedback(feedbackData, memoryStream, memoryStream2);
				this._sendBackstop.Visible = true;
				this._sendingLabel.Visible = true;
				this._failedLabel.Visible = false;
				this._successLabel.Visible = false;
				this._sendPanel.Modulate = Colors.Transparent;
				Control sendPanel = this._sendPanel;
				Vector2 position = this._sendPanel.Position;
				position.Y = this._originalSuccessPosition.Y + 20f;
				sendPanel.Position = position;
				Tween tween = base.GetTree().CreateTween().Parallel();
				tween.TweenProperty(this._mainPanel, "modulate", new Color(0.1f, 0.1f, 0.1f, 1f), 0.15000000596046448);
				tween.TweenProperty(this._sendPanel, "modulate", Colors.White, 0.15000000596046448);
				tween.TweenProperty(this._sendPanel, "position:y", this._originalSuccessPosition.Y, 0.15000000596046448).SetEase(1L).SetTrans(4L);
				tween.Chain().TweenProperty(this._successLabel, "position:y", this._successLabel.Position.Y - 10f, 0.10000000149011612).SetEase(1L)
					.SetTrans(4L);
				await TaskHelper.WhenAny(new Task[]
				{
					sendTask,
					this._runInBackgroundTaskSource.Task
				});
				if (this.IsValid())
				{
					if (this._runInBackgroundTaskSource.Task.IsCompleted)
					{
						this.Close();
					}
					else if (await sendTask)
					{
						await this.OnFeedbackSuccess();
					}
					else
					{
						await this.OnFeedbackFailed();
					}
				}
			}
		}

		/// <summary>
		/// Send feedback and log after close.
		/// This is purposefully static so that we ensure that nothing from the screen is used, as it can be deallocated
		/// before the method completes.
		/// </summary>
		// Token: 0x06002D20 RID: 11552 RVA: 0x00142080 File Offset: 0x00140280
		private static async Task<bool> SendFeedback(FeedbackData data, Stream screenshotStream, Stream logsMemoryStream)
		{
			bool flag;
			try
			{
				using (MultipartFormDataContent formContent = NSendFeedbackScreen.BuildMultipartContent(data, screenshotStream, logsMemoryStream))
				{
					int[] delaysMs = new int[] { 500, 1000, 2000 };
					string sentryMessage = null;
					for (int attempt = 0; attempt < 3; attempt++)
					{
						try
						{
							HttpResponseMessage httpResponseMessage = await NSendFeedbackScreen._httpClient.PutAsync(NSendFeedbackScreen._url, formContent);
							using (HttpResponseMessage response = httpResponseMessage)
							{
								if (response.IsSuccessStatusCode)
								{
									Log.Info("Feedback successfully posted!", 2);
									return true;
								}
								int statusCode = (int)response.StatusCode;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
								if (statusCode >= 400 && statusCode < 500 && statusCode != 429)
								{
									string text = await response.Content.ReadAsStringAsync();
									defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
									defaultInterpolatedStringHandler.AppendLiteral("Feedback rejected (");
									defaultInterpolatedStringHandler.AppendFormatted<HttpStatusCode>(response.StatusCode);
									defaultInterpolatedStringHandler.AppendLiteral("): ");
									defaultInterpolatedStringHandler.AppendFormatted(text);
									Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
									defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
									defaultInterpolatedStringHandler.AppendLiteral("Feedback rejected: Response status code ");
									defaultInterpolatedStringHandler.AppendFormatted<HttpStatusCode>(response.StatusCode);
									SentrySdk.CaptureMessage(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
									return false;
								}
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
								defaultInterpolatedStringHandler.AppendLiteral("Response status code ");
								defaultInterpolatedStringHandler.AppendFormatted<HttpStatusCode>(response.StatusCode);
								sentryMessage = defaultInterpolatedStringHandler.ToStringAndClear();
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Feedback attempt ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(attempt + 1);
								defaultInterpolatedStringHandler.AppendLiteral("/");
								defaultInterpolatedStringHandler.AppendFormatted<int>(3);
								defaultInterpolatedStringHandler.AppendLiteral(" failed: ");
								defaultInterpolatedStringHandler.AppendFormatted<HttpStatusCode>(response.StatusCode);
								Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							}
							HttpResponseMessage response = null;
						}
						catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 4);
							defaultInterpolatedStringHandler.AppendLiteral("Feedback attempt ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(attempt + 1);
							defaultInterpolatedStringHandler.AppendLiteral("/");
							defaultInterpolatedStringHandler.AppendFormatted<int>(3);
							defaultInterpolatedStringHandler.AppendLiteral(" network error: ");
							defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
							defaultInterpolatedStringHandler.AppendLiteral("\n Inner messages: ");
							defaultInterpolatedStringHandler.AppendFormatted(NSendFeedbackScreen.ExceptionMessageWithInner(ex));
							string text2 = defaultInterpolatedStringHandler.ToStringAndClear();
							HttpRequestException ex2 = ex as HttpRequestException;
							if (ex2 != null && ex2.HttpRequestError != HttpRequestError.NameResolutionError)
							{
								sentryMessage = ex.GetType().Name + ": " + NSendFeedbackScreen.ExceptionMessageWithInner(ex);
							}
							Log.Warn(text2, 2);
						}
						if (attempt < 2)
						{
							await Task.Delay(delaysMs[attempt]);
							screenshotStream.Seek(0L, SeekOrigin.Begin);
							logsMemoryStream.Seek(0L, SeekOrigin.Begin);
						}
					}
					Log.Warn("Feedback send failed after all retry attempts", 2);
					if (sentryMessage != null)
					{
						SentrySdk.CaptureMessage("Feedback failed to send: " + sentryMessage, 1);
					}
					flag = false;
				}
			}
			finally
			{
				screenshotStream.Close();
				logsMemoryStream.Close();
			}
			return flag;
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x001420D3 File Offset: 0x001402D3
		private static string ExceptionMessageWithInner(Exception ex)
		{
			if (ex.InnerException == null)
			{
				return ex.Message;
			}
			return ex.Message + " | " + NSendFeedbackScreen.ExceptionMessageWithInner(ex.InnerException);
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x00142100 File Offset: 0x00140300
		private static MultipartFormDataContent BuildMultipartContent(FeedbackData data, Stream screenshotStream, Stream logsStream)
		{
			string text = JsonSerializer.Serialize<FeedbackData>(data, JsonSerializationUtility.GetTypeInfo<FeedbackData>());
			return new MultipartFormDataContent
			{
				new StringContent(text)
				{
					Headers = 
					{
						ContentType = new MediaTypeHeaderValue("application/json"),
						ContentDisposition = new ContentDispositionHeaderValue("form-data")
						{
							Name = "payload_json"
						}
					}
				},
				new StreamContent(logsStream)
				{
					Headers = 
					{
						ContentType = new MediaTypeHeaderValue("application/zip"),
						ContentDisposition = new ContentDispositionHeaderValue("form-data")
						{
							Name = "logs"
						}
					}
				},
				new StreamContent(screenshotStream)
				{
					Headers = 
					{
						ContentType = new MediaTypeHeaderValue("image/png"),
						ContentDisposition = new ContentDispositionHeaderValue("form-data")
						{
							Name = "screenshot"
						}
					}
				}
			};
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x001421F0 File Offset: 0x001403F0
		private async Task OnFeedbackFailed()
		{
			this._sendingLabel.Visible = false;
			this._failedLabel.Visible = true;
			this._successLabel.Visible = false;
			Tween tween = base.GetTree().CreateTween().Chain();
			tween.TweenProperty(this._failedLabel, "position:x", this._successLabel.Position.X - 10f, 0.02500000037252903).SetEase(0L).SetTrans(4L);
			tween.TweenProperty(this._failedLabel, "position:x", this._successLabel.Position.X + 10f, 0.05000000074505806).SetEase(2L).SetTrans(4L);
			tween.TweenProperty(this._failedLabel, "position:x", this._successLabel.Position.X - 10f, 0.05000000074505806).SetEase(2L).SetTrans(4L);
			tween.TweenProperty(this._failedLabel, "position:x", this._successLabel.Position.X + 10f, 0.05000000074505806).SetEase(2L).SetTrans(4L);
			tween.TweenProperty(this._failedLabel, "position:x", this._successLabel.Position.X, 0.02500000037252903).SetEase(1L).SetTrans(4L);
			Tween wiggleTween = this._wiggleTween;
			if (wiggleTween != null)
			{
				wiggleTween.Kill();
			}
			this._flower.SetState(NSendFeedbackFlower.State.None);
			int num = 2000;
			CancellationTokenSource screenClosedCancelToken = this._screenClosedCancelToken;
			await Task.Delay(num, (screenClosedCancelToken != null) ? screenClosedCancelToken.Token : default(CancellationToken));
			this._sendBackstop.Visible = false;
			this._mainPanel.Modulate = Colors.White;
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x00142234 File Offset: 0x00140434
		private async Task OnFeedbackSuccess()
		{
			this.ClearInput();
			this._screenshotBytes = null;
			this._sendingLabel.Visible = false;
			this._failedLabel.Visible = false;
			this._successLabel.Visible = true;
			CanvasItem successLabel = this._successLabel;
			Color modulate = this._successLabel.Modulate;
			modulate.A = 0f;
			successLabel.Modulate = modulate;
			Tween tween = base.GetTree().CreateTween().Parallel();
			tween.TweenProperty(this._successLabel, "modulate:a", 1f, 0.15000000596046448);
			tween.TweenProperty(this._successLabel, "position:y", this._successLabel.Position.Y - 10f, 0.10000000149011612).SetEase(1L).SetTrans(4L);
			tween.Chain().TweenProperty(this._successLabel, "position:y", this._successLabel.Position.Y, 0.10000000149011612).SetEase(0L)
				.SetTrans(4L);
			Tween wiggleTween = this._wiggleTween;
			if (wiggleTween != null)
			{
				wiggleTween.Kill();
			}
			this._wiggleTween = base.CreateTween();
			this._wiggleTween.TweenCallback(Callable.From(new Action(this.WiggleCartoons1)));
			this._wiggleTween.TweenInterval(0.25);
			this._wiggleTween.TweenCallback(Callable.From(new Action(this.WiggleCartoons2)));
			this._wiggleTween.TweenInterval(0.25);
			this._wiggleTween.SetLoops(0);
			string scenePath = SceneHelper.GetScenePath("vfx/vfx_dramatic_entrance_fullscreen");
			Node2D node2D = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>(0L);
			this.AddChildSafely(node2D);
			this.MoveChildSafely(node2D, 1);
			node2D.GlobalPosition = NGame.Instance.GetViewportRect().Size * 0.5f;
			this._flower.SetState(NSendFeedbackFlower.State.NoddingFast);
			int num = 2000;
			CancellationTokenSource screenClosedCancelToken = this._screenClosedCancelToken;
			await Task.Delay(num, (screenClosedCancelToken != null) ? screenClosedCancelToken.Token : default(CancellationToken));
			this.Close();
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x00142278 File Offset: 0x00140478
		private void WiggleCartoons1()
		{
			foreach (NSendFeedbackCartoon nsendFeedbackCartoon in this._cartoons)
			{
				if (this._flower.MyState == NSendFeedbackFlower.State.None || nsendFeedbackCartoon != this._flower.Cartoon)
				{
					nsendFeedbackCartoon.SetRotation1();
				}
			}
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x001422E8 File Offset: 0x001404E8
		private void WiggleCartoons2()
		{
			foreach (NSendFeedbackCartoon nsendFeedbackCartoon in this._cartoons)
			{
				if (this._flower.MyState == NSendFeedbackFlower.State.None || nsendFeedbackCartoon != this._flower.Cartoon)
				{
					nsendFeedbackCartoon.SetRotation2();
				}
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x00142358 File Offset: 0x00140558
		public Control DefaultFocusedControl
		{
			get
			{
				return this._descriptionInput;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D28 RID: 11560 RVA: 0x00142360 File Offset: 0x00140560
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NSendFeedbackScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.Relocalize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.OnDescriptionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.SetScreenshot, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "screenshot", 0L, "", 6L, new StringName("Image"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.EmojiButtonSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.SendButtonFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.SendButtonUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.ClearInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.SetSelectedEmoji, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.SendButtonSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.ReturnToGameSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.ReturnToGameFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.ReturnToGameUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.WiggleCartoons1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackScreen.MethodName.WiggleCartoons2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D29 RID: 11561 RVA: 0x00142880 File Offset: 0x00140A80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSendFeedbackScreen.MethodName.Create && args.Count == 0)
			{
				NSendFeedbackScreen nsendFeedbackScreen = NSendFeedbackScreen.Create();
				ret = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref nsendFeedbackScreen);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.Relocalize && args.Count == 0)
			{
				this.Relocalize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.OnDescriptionChanged && args.Count == 0)
			{
				this.OnDescriptionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.SetScreenshot && args.Count == 1)
			{
				this.SetScreenshot(VariantUtils.ConvertTo<Image>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.EmojiButtonSelected && args.Count == 1)
			{
				this.EmojiButtonSelected(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.SendButtonFocused && args.Count == 1)
			{
				this.SendButtonFocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.SendButtonUnfocused && args.Count == 1)
			{
				this.SendButtonUnfocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.Close && args.Count == 0)
			{
				this.Close();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.ClearInput && args.Count == 0)
			{
				this.ClearInput();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.SetSelectedEmoji && args.Count == 1)
			{
				this.SetSelectedEmoji(VariantUtils.ConvertTo<NSendFeedbackEmojiButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.SendButtonSelected && args.Count == 1)
			{
				this.SendButtonSelected(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.ReturnToGameSelected && args.Count == 1)
			{
				this.ReturnToGameSelected(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.ReturnToGameFocused && args.Count == 1)
			{
				this.ReturnToGameFocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.ReturnToGameUnfocused && args.Count == 1)
			{
				this.ReturnToGameUnfocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.WiggleCartoons1 && args.Count == 0)
			{
				this.WiggleCartoons1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackScreen.MethodName.WiggleCartoons2 && args.Count == 0)
			{
				this.WiggleCartoons2();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x00142BB4 File Offset: 0x00140DB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSendFeedbackScreen.MethodName.Create && args.Count == 0)
			{
				NSendFeedbackScreen nsendFeedbackScreen = NSendFeedbackScreen.Create();
				ret = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref nsendFeedbackScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002D2B RID: 11563 RVA: 0x00142BF4 File Offset: 0x00140DF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSendFeedbackScreen.MethodName.Create || (ref method) == NSendFeedbackScreen.MethodName._Ready || (ref method) == NSendFeedbackScreen.MethodName.Relocalize || (ref method) == NSendFeedbackScreen.MethodName.OnDescriptionChanged || (ref method) == NSendFeedbackScreen.MethodName.SetScreenshot || (ref method) == NSendFeedbackScreen.MethodName.EmojiButtonSelected || (ref method) == NSendFeedbackScreen.MethodName.SendButtonFocused || (ref method) == NSendFeedbackScreen.MethodName.SendButtonUnfocused || (ref method) == NSendFeedbackScreen.MethodName.Open || (ref method) == NSendFeedbackScreen.MethodName.Close || (ref method) == NSendFeedbackScreen.MethodName.ClearInput || (ref method) == NSendFeedbackScreen.MethodName.SetSelectedEmoji || (ref method) == NSendFeedbackScreen.MethodName.SendButtonSelected || (ref method) == NSendFeedbackScreen.MethodName.ReturnToGameSelected || (ref method) == NSendFeedbackScreen.MethodName.ReturnToGameFocused || (ref method) == NSendFeedbackScreen.MethodName.ReturnToGameUnfocused || (ref method) == NSendFeedbackScreen.MethodName.WiggleCartoons1 || (ref method) == NSendFeedbackScreen.MethodName.WiggleCartoons2 || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002D2C RID: 11564 RVA: 0x00142D18 File Offset: 0x00140F18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSendFeedbackScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._mainPanel)
			{
				this._mainPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionInput)
			{
				this._descriptionInput = VariantUtils.ConvertTo<NMegaTextEdit>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._emojiLabel)
			{
				this._emojiLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendButton)
			{
				this._sendButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendLabel)
			{
				this._sendLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._categoryLabel)
			{
				this._categoryLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._returnToGameButton)
			{
				this._returnToGameButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._returnToGameLabel)
			{
				this._returnToGameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._returnToGameHoverLabel)
			{
				this._returnToGameHoverLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._categoryDropdown)
			{
				this._categoryDropdown = VariantUtils.ConvertTo<NFeedbackCategoryDropdown>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendBackstop)
			{
				this._sendBackstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendPanel)
			{
				this._sendPanel = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._successLabel)
			{
				this._successLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._failedLabel)
			{
				this._failedLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendingLabel)
			{
				this._sendingLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._flower)
			{
				this._flower = VariantUtils.ConvertTo<NSendFeedbackFlower>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._selectedEmoteButton)
			{
				this._selectedEmoteButton = VariantUtils.ConvertTo<NSendFeedbackEmojiButton>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._screenshotBytes)
			{
				this._screenshotBytes = VariantUtils.ConvertTo<byte[]>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._originalSuccessPosition)
			{
				this._originalSuccessPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._lastClosedMsec)
			{
				this._lastClosedMsec = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionText)
			{
				this._descriptionText = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionCaretLine)
			{
				this._descriptionCaretLine = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionCaretColumn)
			{
				this._descriptionCaretColumn = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._wiggleTween)
			{
				this._wiggleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002D2D RID: 11565 RVA: 0x00142FD0 File Offset: 0x001411D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSendFeedbackScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._mainPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._mainPanel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionInput)
			{
				value = VariantUtils.CreateFrom<NMegaTextEdit>(ref this._descriptionInput);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._emojiLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._emojiLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._sendButton);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._sendLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._categoryLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._categoryLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._returnToGameButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._returnToGameButton);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._returnToGameLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._returnToGameLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._returnToGameHoverLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._returnToGameHoverLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._categoryDropdown)
			{
				value = VariantUtils.CreateFrom<NFeedbackCategoryDropdown>(ref this._categoryDropdown);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendBackstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._sendBackstop);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendPanel)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._sendPanel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._successLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._successLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._failedLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._failedLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._sendingLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._sendingLabel);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._flower)
			{
				value = VariantUtils.CreateFrom<NSendFeedbackFlower>(ref this._flower);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._selectedEmoteButton)
			{
				value = VariantUtils.CreateFrom<NSendFeedbackEmojiButton>(ref this._selectedEmoteButton);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._screenshotBytes)
			{
				value = VariantUtils.CreateFrom<byte[]>(ref this._screenshotBytes);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._originalSuccessPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalSuccessPosition);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._lastClosedMsec)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._lastClosedMsec);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionText)
			{
				value = VariantUtils.CreateFrom<string>(ref this._descriptionText);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionCaretLine)
			{
				value = VariantUtils.CreateFrom<int>(ref this._descriptionCaretLine);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._descriptionCaretColumn)
			{
				value = VariantUtils.CreateFrom<int>(ref this._descriptionCaretColumn);
				return true;
			}
			if ((ref name) == NSendFeedbackScreen.PropertyName._wiggleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._wiggleTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D2E RID: 11566 RVA: 0x00143328 File Offset: 0x00141528
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._mainPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._descriptionInput, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._emojiLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._sendButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._sendLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._categoryLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._returnToGameButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._returnToGameLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._returnToGameHoverLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._categoryDropdown, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._sendBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._sendPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._successLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._failedLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._sendingLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._flower, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._selectedEmoteButton, 0L, "", 4096L, false),
				new PropertyInfo(29L, NSendFeedbackScreen.PropertyName._screenshotBytes, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSendFeedbackScreen.PropertyName._originalSuccessPosition, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSendFeedbackScreen.PropertyName._lastClosedMsec, 0L, "", 4096L, false),
				new PropertyInfo(4L, NSendFeedbackScreen.PropertyName._descriptionText, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSendFeedbackScreen.PropertyName._descriptionCaretLine, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSendFeedbackScreen.PropertyName._descriptionCaretColumn, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName._wiggleTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D2F RID: 11567 RVA: 0x00143694 File Offset: 0x00141894
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSendFeedbackScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NSendFeedbackScreen.PropertyName._mainPanel, Variant.From<Control>(ref this._mainPanel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionInput, Variant.From<NMegaTextEdit>(ref this._descriptionInput));
			info.AddProperty(NSendFeedbackScreen.PropertyName._emojiLabel, Variant.From<MegaLabel>(ref this._emojiLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._sendButton, Variant.From<NButton>(ref this._sendButton));
			info.AddProperty(NSendFeedbackScreen.PropertyName._sendLabel, Variant.From<MegaLabel>(ref this._sendLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._categoryLabel, Variant.From<MegaLabel>(ref this._categoryLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._returnToGameButton, Variant.From<NButton>(ref this._returnToGameButton));
			info.AddProperty(NSendFeedbackScreen.PropertyName._returnToGameLabel, Variant.From<MegaLabel>(ref this._returnToGameLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._returnToGameHoverLabel, Variant.From<MegaLabel>(ref this._returnToGameHoverLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._categoryDropdown, Variant.From<NFeedbackCategoryDropdown>(ref this._categoryDropdown));
			info.AddProperty(NSendFeedbackScreen.PropertyName._sendBackstop, Variant.From<Control>(ref this._sendBackstop));
			info.AddProperty(NSendFeedbackScreen.PropertyName._sendPanel, Variant.From<Control>(ref this._sendPanel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._successLabel, Variant.From<MegaLabel>(ref this._successLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._failedLabel, Variant.From<MegaLabel>(ref this._failedLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._sendingLabel, Variant.From<MegaRichTextLabel>(ref this._sendingLabel));
			info.AddProperty(NSendFeedbackScreen.PropertyName._flower, Variant.From<NSendFeedbackFlower>(ref this._flower));
			info.AddProperty(NSendFeedbackScreen.PropertyName._selectedEmoteButton, Variant.From<NSendFeedbackEmojiButton>(ref this._selectedEmoteButton));
			info.AddProperty(NSendFeedbackScreen.PropertyName._screenshotBytes, Variant.From<byte[]>(ref this._screenshotBytes));
			info.AddProperty(NSendFeedbackScreen.PropertyName._originalSuccessPosition, Variant.From<Vector2>(ref this._originalSuccessPosition));
			info.AddProperty(NSendFeedbackScreen.PropertyName._lastClosedMsec, Variant.From<ulong>(ref this._lastClosedMsec));
			info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionText, Variant.From<string>(ref this._descriptionText));
			info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionCaretLine, Variant.From<int>(ref this._descriptionCaretLine));
			info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionCaretColumn, Variant.From<int>(ref this._descriptionCaretColumn));
			info.AddProperty(NSendFeedbackScreen.PropertyName._wiggleTween, Variant.From<Tween>(ref this._wiggleTween));
		}

		/// <inheritdoc />
		// Token: 0x06002D30 RID: 11568 RVA: 0x001438D0 File Offset: 0x00141AD0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._backButton, ref variant))
			{
				this._backButton = variant.As<NBackButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._mainPanel, ref variant2))
			{
				this._mainPanel = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionInput, ref variant3))
			{
				this._descriptionInput = variant3.As<NMegaTextEdit>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._emojiLabel, ref variant4))
			{
				this._emojiLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendButton, ref variant5))
			{
				this._sendButton = variant5.As<NButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendLabel, ref variant6))
			{
				this._sendLabel = variant6.As<MegaLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._categoryLabel, ref variant7))
			{
				this._categoryLabel = variant7.As<MegaLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._returnToGameButton, ref variant8))
			{
				this._returnToGameButton = variant8.As<NButton>();
			}
			Variant variant9;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._returnToGameLabel, ref variant9))
			{
				this._returnToGameLabel = variant9.As<MegaLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._returnToGameHoverLabel, ref variant10))
			{
				this._returnToGameHoverLabel = variant10.As<MegaLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._categoryDropdown, ref variant11))
			{
				this._categoryDropdown = variant11.As<NFeedbackCategoryDropdown>();
			}
			Variant variant12;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendBackstop, ref variant12))
			{
				this._sendBackstop = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendPanel, ref variant13))
			{
				this._sendPanel = variant13.As<Control>();
			}
			Variant variant14;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._successLabel, ref variant14))
			{
				this._successLabel = variant14.As<MegaLabel>();
			}
			Variant variant15;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._failedLabel, ref variant15))
			{
				this._failedLabel = variant15.As<MegaLabel>();
			}
			Variant variant16;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendingLabel, ref variant16))
			{
				this._sendingLabel = variant16.As<MegaRichTextLabel>();
			}
			Variant variant17;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._flower, ref variant17))
			{
				this._flower = variant17.As<NSendFeedbackFlower>();
			}
			Variant variant18;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._selectedEmoteButton, ref variant18))
			{
				this._selectedEmoteButton = variant18.As<NSendFeedbackEmojiButton>();
			}
			Variant variant19;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._screenshotBytes, ref variant19))
			{
				this._screenshotBytes = variant19.As<byte[]>();
			}
			Variant variant20;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._originalSuccessPosition, ref variant20))
			{
				this._originalSuccessPosition = variant20.As<Vector2>();
			}
			Variant variant21;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._lastClosedMsec, ref variant21))
			{
				this._lastClosedMsec = variant21.As<ulong>();
			}
			Variant variant22;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionText, ref variant22))
			{
				this._descriptionText = variant22.As<string>();
			}
			Variant variant23;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionCaretLine, ref variant23))
			{
				this._descriptionCaretLine = variant23.As<int>();
			}
			Variant variant24;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionCaretColumn, ref variant24))
			{
				this._descriptionCaretColumn = variant24.As<int>();
			}
			Variant variant25;
			if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._wiggleTween, ref variant25))
			{
				this._wiggleTween = variant25.As<Tween>();
			}
		}

		// Token: 0x04001117 RID: 4375
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/feedback_screen/feedback_screen");

		// Token: 0x04001118 RID: 4376
		private const float _superWiggleTime = 0.25f;

		// Token: 0x04001119 RID: 4377
		private const string _defaultUrl = "https://feedback.sts2.megacrit.com/feedback";

		// Token: 0x0400111A RID: 4378
		private static readonly string _url = Environment.GetEnvironmentVariable("STS2_FEEDBACK_URL") ?? "https://feedback.sts2.megacrit.com/feedback";

		// Token: 0x0400111B RID: 4379
		private static readonly HttpClient _httpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(10L)
		};

		// Token: 0x0400111C RID: 4380
		private const int _maxDescriptionChars = 8000;

		// Token: 0x0400111D RID: 4381
		private NBackButton _backButton;

		// Token: 0x0400111E RID: 4382
		private Control _mainPanel;

		// Token: 0x0400111F RID: 4383
		private NMegaTextEdit _descriptionInput;

		// Token: 0x04001120 RID: 4384
		private MegaLabel _emojiLabel;

		// Token: 0x04001121 RID: 4385
		private NButton _sendButton;

		// Token: 0x04001122 RID: 4386
		private MegaLabel _sendLabel;

		// Token: 0x04001123 RID: 4387
		private MegaLabel _categoryLabel;

		// Token: 0x04001124 RID: 4388
		private NButton _returnToGameButton;

		// Token: 0x04001125 RID: 4389
		private MegaLabel _returnToGameLabel;

		// Token: 0x04001126 RID: 4390
		private MegaLabel _returnToGameHoverLabel;

		// Token: 0x04001127 RID: 4391
		private NFeedbackCategoryDropdown _categoryDropdown;

		// Token: 0x04001128 RID: 4392
		private Control _sendBackstop;

		// Token: 0x04001129 RID: 4393
		private Control _sendPanel;

		// Token: 0x0400112A RID: 4394
		private MegaLabel _successLabel;

		// Token: 0x0400112B RID: 4395
		private MegaLabel _failedLabel;

		// Token: 0x0400112C RID: 4396
		private MegaRichTextLabel _sendingLabel;

		// Token: 0x0400112D RID: 4397
		private List<NSendFeedbackCartoon> _cartoons = new List<NSendFeedbackCartoon>();

		// Token: 0x0400112E RID: 4398
		private NSendFeedbackFlower _flower;

		// Token: 0x0400112F RID: 4399
		[Nullable(2)]
		private CancellationTokenSource _screenClosedCancelToken;

		// Token: 0x04001130 RID: 4400
		[Nullable(2)]
		private TaskCompletionSource _runInBackgroundTaskSource;

		// Token: 0x04001131 RID: 4401
		private readonly List<NSendFeedbackEmojiButton> _emojiButtons = new List<NSendFeedbackEmojiButton>();

		// Token: 0x04001132 RID: 4402
		[Nullable(2)]
		private NSendFeedbackEmojiButton _selectedEmoteButton;

		// Token: 0x04001133 RID: 4403
		[Nullable(2)]
		private byte[] _screenshotBytes;

		// Token: 0x04001134 RID: 4404
		private Vector2 _originalSuccessPosition;

		// Token: 0x04001135 RID: 4405
		private ulong _lastClosedMsec;

		// Token: 0x04001136 RID: 4406
		private string _descriptionText = string.Empty;

		// Token: 0x04001137 RID: 4407
		private int _descriptionCaretLine;

		// Token: 0x04001138 RID: 4408
		private int _descriptionCaretColumn;

		// Token: 0x04001139 RID: 4409
		[Nullable(2)]
		private Tween _wiggleTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001582 RID: 5506
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005250 RID: 21072
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005251 RID: 21073
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Relocalize' method.
			/// </summary>
			// Token: 0x04005252 RID: 21074
			public static readonly StringName Relocalize = "Relocalize";

			/// <summary>
			/// Cached name for the 'OnDescriptionChanged' method.
			/// </summary>
			// Token: 0x04005253 RID: 21075
			public static readonly StringName OnDescriptionChanged = "OnDescriptionChanged";

			/// <summary>
			/// Cached name for the 'SetScreenshot' method.
			/// </summary>
			// Token: 0x04005254 RID: 21076
			public static readonly StringName SetScreenshot = "SetScreenshot";

			/// <summary>
			/// Cached name for the 'EmojiButtonSelected' method.
			/// </summary>
			// Token: 0x04005255 RID: 21077
			public static readonly StringName EmojiButtonSelected = "EmojiButtonSelected";

			/// <summary>
			/// Cached name for the 'SendButtonFocused' method.
			/// </summary>
			// Token: 0x04005256 RID: 21078
			public static readonly StringName SendButtonFocused = "SendButtonFocused";

			/// <summary>
			/// Cached name for the 'SendButtonUnfocused' method.
			/// </summary>
			// Token: 0x04005257 RID: 21079
			public static readonly StringName SendButtonUnfocused = "SendButtonUnfocused";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x04005258 RID: 21080
			public static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x04005259 RID: 21081
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'ClearInput' method.
			/// </summary>
			// Token: 0x0400525A RID: 21082
			public static readonly StringName ClearInput = "ClearInput";

			/// <summary>
			/// Cached name for the 'SetSelectedEmoji' method.
			/// </summary>
			// Token: 0x0400525B RID: 21083
			public static readonly StringName SetSelectedEmoji = "SetSelectedEmoji";

			/// <summary>
			/// Cached name for the 'SendButtonSelected' method.
			/// </summary>
			// Token: 0x0400525C RID: 21084
			public static readonly StringName SendButtonSelected = "SendButtonSelected";

			/// <summary>
			/// Cached name for the 'ReturnToGameSelected' method.
			/// </summary>
			// Token: 0x0400525D RID: 21085
			public static readonly StringName ReturnToGameSelected = "ReturnToGameSelected";

			/// <summary>
			/// Cached name for the 'ReturnToGameFocused' method.
			/// </summary>
			// Token: 0x0400525E RID: 21086
			public static readonly StringName ReturnToGameFocused = "ReturnToGameFocused";

			/// <summary>
			/// Cached name for the 'ReturnToGameUnfocused' method.
			/// </summary>
			// Token: 0x0400525F RID: 21087
			public static readonly StringName ReturnToGameUnfocused = "ReturnToGameUnfocused";

			/// <summary>
			/// Cached name for the 'WiggleCartoons1' method.
			/// </summary>
			// Token: 0x04005260 RID: 21088
			public static readonly StringName WiggleCartoons1 = "WiggleCartoons1";

			/// <summary>
			/// Cached name for the 'WiggleCartoons2' method.
			/// </summary>
			// Token: 0x04005261 RID: 21089
			public static readonly StringName WiggleCartoons2 = "WiggleCartoons2";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001583 RID: 5507
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005262 RID: 21090
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04005263 RID: 21091
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_mainPanel' field.
			/// </summary>
			// Token: 0x04005264 RID: 21092
			public static readonly StringName _mainPanel = "_mainPanel";

			/// <summary>
			/// Cached name for the '_descriptionInput' field.
			/// </summary>
			// Token: 0x04005265 RID: 21093
			public static readonly StringName _descriptionInput = "_descriptionInput";

			/// <summary>
			/// Cached name for the '_emojiLabel' field.
			/// </summary>
			// Token: 0x04005266 RID: 21094
			public static readonly StringName _emojiLabel = "_emojiLabel";

			/// <summary>
			/// Cached name for the '_sendButton' field.
			/// </summary>
			// Token: 0x04005267 RID: 21095
			public static readonly StringName _sendButton = "_sendButton";

			/// <summary>
			/// Cached name for the '_sendLabel' field.
			/// </summary>
			// Token: 0x04005268 RID: 21096
			public static readonly StringName _sendLabel = "_sendLabel";

			/// <summary>
			/// Cached name for the '_categoryLabel' field.
			/// </summary>
			// Token: 0x04005269 RID: 21097
			public static readonly StringName _categoryLabel = "_categoryLabel";

			/// <summary>
			/// Cached name for the '_returnToGameButton' field.
			/// </summary>
			// Token: 0x0400526A RID: 21098
			public static readonly StringName _returnToGameButton = "_returnToGameButton";

			/// <summary>
			/// Cached name for the '_returnToGameLabel' field.
			/// </summary>
			// Token: 0x0400526B RID: 21099
			public static readonly StringName _returnToGameLabel = "_returnToGameLabel";

			/// <summary>
			/// Cached name for the '_returnToGameHoverLabel' field.
			/// </summary>
			// Token: 0x0400526C RID: 21100
			public static readonly StringName _returnToGameHoverLabel = "_returnToGameHoverLabel";

			/// <summary>
			/// Cached name for the '_categoryDropdown' field.
			/// </summary>
			// Token: 0x0400526D RID: 21101
			public static readonly StringName _categoryDropdown = "_categoryDropdown";

			/// <summary>
			/// Cached name for the '_sendBackstop' field.
			/// </summary>
			// Token: 0x0400526E RID: 21102
			public static readonly StringName _sendBackstop = "_sendBackstop";

			/// <summary>
			/// Cached name for the '_sendPanel' field.
			/// </summary>
			// Token: 0x0400526F RID: 21103
			public static readonly StringName _sendPanel = "_sendPanel";

			/// <summary>
			/// Cached name for the '_successLabel' field.
			/// </summary>
			// Token: 0x04005270 RID: 21104
			public static readonly StringName _successLabel = "_successLabel";

			/// <summary>
			/// Cached name for the '_failedLabel' field.
			/// </summary>
			// Token: 0x04005271 RID: 21105
			public static readonly StringName _failedLabel = "_failedLabel";

			/// <summary>
			/// Cached name for the '_sendingLabel' field.
			/// </summary>
			// Token: 0x04005272 RID: 21106
			public static readonly StringName _sendingLabel = "_sendingLabel";

			/// <summary>
			/// Cached name for the '_flower' field.
			/// </summary>
			// Token: 0x04005273 RID: 21107
			public static readonly StringName _flower = "_flower";

			/// <summary>
			/// Cached name for the '_selectedEmoteButton' field.
			/// </summary>
			// Token: 0x04005274 RID: 21108
			public static readonly StringName _selectedEmoteButton = "_selectedEmoteButton";

			/// <summary>
			/// Cached name for the '_screenshotBytes' field.
			/// </summary>
			// Token: 0x04005275 RID: 21109
			public static readonly StringName _screenshotBytes = "_screenshotBytes";

			/// <summary>
			/// Cached name for the '_originalSuccessPosition' field.
			/// </summary>
			// Token: 0x04005276 RID: 21110
			public static readonly StringName _originalSuccessPosition = "_originalSuccessPosition";

			/// <summary>
			/// Cached name for the '_lastClosedMsec' field.
			/// </summary>
			// Token: 0x04005277 RID: 21111
			public static readonly StringName _lastClosedMsec = "_lastClosedMsec";

			/// <summary>
			/// Cached name for the '_descriptionText' field.
			/// </summary>
			// Token: 0x04005278 RID: 21112
			public static readonly StringName _descriptionText = "_descriptionText";

			/// <summary>
			/// Cached name for the '_descriptionCaretLine' field.
			/// </summary>
			// Token: 0x04005279 RID: 21113
			public static readonly StringName _descriptionCaretLine = "_descriptionCaretLine";

			/// <summary>
			/// Cached name for the '_descriptionCaretColumn' field.
			/// </summary>
			// Token: 0x0400527A RID: 21114
			public static readonly StringName _descriptionCaretColumn = "_descriptionCaretColumn";

			/// <summary>
			/// Cached name for the '_wiggleTween' field.
			/// </summary>
			// Token: 0x0400527B RID: 21115
			public static readonly StringName _wiggleTween = "_wiggleTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001584 RID: 5508
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
