using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	// Token: 0x020003C7 RID: 967
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Debug/NDevConsole.cs")]
	public class NDevConsole : Panel
	{
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06003A77 RID: 14967 RVA: 0x001BD3BC File Offset: 0x001BB5BC
		public static NDevConsole Instance
		{
			get
			{
				NDevConsole instance = NDevConsole._instance;
				if (instance == null)
				{
					throw new InvalidOperationException("Dev console used before being created.");
				}
				return instance;
			}
		}

		/// <summary>
		/// True if the dev console exists and is currently shown. Safe to call before the console is
		/// created (e.g. hotkeys pressed very early in initialization, or in TestMode where the console
		/// is never instantiated), unlike <see cref="P:MegaCrit.Sts2.Core.Nodes.Debug.NDevConsole.Instance" /> which throws when absent.
		/// </summary>
		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06003A78 RID: 14968 RVA: 0x001BD3D2 File Offset: 0x001BB5D2
		public static bool IsConsoleVisible
		{
			get
			{
				NDevConsole instance = NDevConsole._instance;
				return instance != null && instance.Visible;
			}
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x001BD3E4 File Offset: 0x001BB5E4
		[NullableContext(2)]
		public static CanvasLayer Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NDevConsole.assetPath).Instantiate<CanvasLayer>(0L);
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x001BD405 File Offset: 0x001BB605
		public override void _EnterTree()
		{
			if (TestMode.IsOn)
			{
				this.QueueFreeSafely();
				return;
			}
			if (NDevConsole._instance != null)
			{
				this.QueueFreeSafely();
				return;
			}
			NDevConsole._instance = this;
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x001BD42C File Offset: 0x001BB62C
		public override void _Ready()
		{
			this._outputBuffer = base.GetNode<RichTextLabel>("OutputContainer/OutputBuffer");
			this._tabBuffer = base.GetNode<RichTextLabel>("OutputContainer/TabBuffer");
			this._inputContainer = base.GetNode<Control>("InputContainer");
			this._inputBuffer = base.GetNode<LineEdit>("InputContainer/InputBufferContainer/InputBuffer");
			this._promptLabel = base.GetNode<Label>("InputContainer/PromptLabel");
			this._ghostTextLabel = base.GetNode<Label>("InputContainer/InputBufferContainer/GhostText");
			this.HideConsole();
			this.MakeHalfScreen();
			this.DisableTabBuffer();
			this.HideGhostText();
			this._inputBuffer.CaretBlink = true;
			Font themeFont = this._promptLabel.GetThemeFont(ThemeConstants.Label.Font, null);
			if (themeFont.HasChar(10140L))
			{
				this._symbolPrompt = "➜";
			}
			if (themeFont.HasChar(9888L))
			{
				this._symbolWarning = "⚠";
			}
			if (themeFont.HasChar(8593L))
			{
				this._symbolUp = "↑";
			}
			if (themeFont.HasChar(8595L))
			{
				this._symbolDown = "↓";
			}
			this.UpdatePromptStyle();
			this._inputBuffer.TextChanged += new LineEdit.TextChangedEventHandler(this.OnInputTextChanged);
			this.PrintUsage();
			bool flag = OS.HasFeature("editor") || TestMode.IsOn || ModManager.IsRunningModded() || SaveManager.Instance.SettingsSave.FullConsole;
			this._devConsole = new DevConsole(flag);
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x001BD5B3 File Offset: 0x001BB7B3
		public override void _ExitTree()
		{
			if (TestMode.IsOff)
			{
				this._inputBuffer.TextChanged -= new LineEdit.TextChangedEventHandler(this.OnInputTextChanged);
			}
			base._ExitTree();
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x001BD5D9 File Offset: 0x001BB7D9
		private void PrintUsage()
		{
			RichTextLabel outputBuffer = this._outputBuffer;
			outputBuffer.Text += "[color=#888888]Use 'F11' to toggle console fullscreen. Press 'up arrow' to use the last command. You can autocomplete commands with 'tab'.[/color]";
			RichTextLabel outputBuffer2 = this._outputBuffer;
			outputBuffer2.Text += "\n\n";
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x001BD614 File Offset: 0x001BB814
		public override void _Input(InputEvent inputEvent)
		{
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey == null)
			{
				return;
			}
			if (!inputEventKey.Pressed)
			{
				return;
			}
			Key keycode = inputEventKey.Keycode;
			if (keycode <= 42L)
			{
				if (keycode != 39L && keycode != 42L)
				{
					goto IL_003F;
				}
			}
			else if (keycode != 94L && keycode != 96L)
			{
				goto IL_003F;
			}
			bool flag = true;
			goto IL_0041;
			IL_003F:
			flag = false;
			IL_0041:
			if (flag || (inputEventKey.IsShiftPressed() && inputEventKey.Keycode == 56L))
			{
				if (base.Visible)
				{
					this.HideConsole();
				}
				else
				{
					Control control = base.GetViewport().GuiGetFocusOwner();
					if (!(control is TextEdit) && !(control is LineEdit))
					{
						this.ShowConsole();
					}
				}
			}
			if (!base.Visible)
			{
				return;
			}
			if (inputEventKey.Keycode == 4194305L)
			{
				if (this._tabCompletion.InSelectionMode)
				{
					this.ExitSelectionMode();
					return;
				}
				this.HideConsole();
				return;
			}
			else
			{
				if (inputEventKey.Keycode == 4194342L)
				{
					this.OnToggleMaximizeButtonPressed();
					return;
				}
				if (inputEventKey.Keycode == 4194306L)
				{
					if (this._tabCompletion.InSelectionMode)
					{
						this.NavigateSelection(1);
						return;
					}
					this.AutocompleteCommand();
					return;
				}
				else
				{
					if (inputEventKey.Keycode == 4194320L)
					{
						if (this._tabCompletion.InSelectionMode)
						{
							this.NavigateSelection(-1);
						}
						else if (this._devConsole.historyIndex < this._devConsole.history.Count)
						{
							this._tabCompletion.ProgrammaticTextChange = true;
							string text = this._devConsole.history[this._devConsole.historyIndex];
							this._inputBuffer.Text = text;
							if (this._devConsole.historyIndex < this._devConsole.history.Count - 1)
							{
								this._devConsole.historyIndex++;
								while (this._devConsole.historyIndex < this._devConsole.history.Count - 1 && this._devConsole.history[this._devConsole.historyIndex] == text)
								{
									this._devConsole.historyIndex++;
								}
							}
							this.MoveInputCursorToEndOfLine();
						}
						base.GetViewport().SetInputAsHandled();
						return;
					}
					if (inputEventKey.Keycode == 4194322L)
					{
						if (this._tabCompletion.InSelectionMode)
						{
							this.NavigateSelection(1);
						}
						else if (this._devConsole.historyIndex < this._devConsole.history.Count)
						{
							this._tabCompletion.ProgrammaticTextChange = true;
							string text2 = this._devConsole.history[this._devConsole.historyIndex];
							this._inputBuffer.Text = text2;
							if (this._devConsole.historyIndex > 0)
							{
								this._devConsole.historyIndex--;
								while (this._devConsole.historyIndex > 0 && this._devConsole.history[this._devConsole.historyIndex] == text2)
								{
									this._devConsole.historyIndex--;
								}
							}
							this.MoveInputCursorToEndOfLine();
						}
						base.GetViewport().SetInputAsHandled();
						return;
					}
					if (inputEventKey.Keycode != 4194309L)
					{
						if (inputEventKey.IsCtrlPressed())
						{
							this.HandleReadlineKeybinding(inputEventKey);
						}
						return;
					}
					if (this._tabCompletion.InSelectionMode)
					{
						this.AcceptSelection();
						return;
					}
					this.ProcessCommand();
					return;
				}
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x001BD978 File Offset: 0x001BBB78
		private void HandleReadlineKeybinding(InputEventKey keyEvent)
		{
			Key keycode = keyEvent.Keycode;
			if (keycode <= 75L)
			{
				Key key = keycode - 65L;
				if (key <= 4L)
				{
					switch (key)
					{
					case 0U:
						base.GetViewport().SetInputAsHandled();
						this._inputBuffer.CaretColumn = 0;
						this._inputBuffer.CallDeferred(LineEdit.MethodName.Deselect, Array.Empty<Variant>());
						return;
					case 1U:
						return;
					case 2U:
						base.GetViewport().SetInputAsHandled();
						this._inputBuffer.Text = string.Empty;
						this.ExitSelectionMode();
						return;
					case 3U:
						base.GetViewport().SetInputAsHandled();
						this.HideConsole();
						return;
					case 4U:
						base.GetViewport().SetInputAsHandled();
						this.MoveInputCursorToEndOfLine();
						return;
					}
				}
				if (keycode != 75L)
				{
					return;
				}
				base.GetViewport().SetInputAsHandled();
				this.KillToEndOfLine();
				return;
			}
			else
			{
				if (keycode == 76L)
				{
					base.GetViewport().SetInputAsHandled();
					this._outputBuffer.Text = string.Empty;
					return;
				}
				Key key2 = keycode - 85L;
				if (key2 > 4L)
				{
					return;
				}
				switch (key2)
				{
				case 0U:
					base.GetViewport().SetInputAsHandled();
					this._yankBuffer = this._inputBuffer.Text;
					this._inputBuffer.Text = string.Empty;
					this.ExitSelectionMode();
					return;
				case 1U:
				case 3U:
					break;
				case 2U:
					base.GetViewport().SetInputAsHandled();
					this.DeleteWordBackward();
					return;
				case 4U:
					base.GetViewport().SetInputAsHandled();
					this.Yank();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x001BDAEC File Offset: 0x001BBCEC
		private void DeleteWordBackward()
		{
			string text = this._inputBuffer.Text;
			int caretColumn = this._inputBuffer.CaretColumn;
			if (caretColumn == 0 || string.IsNullOrEmpty(text))
			{
				return;
			}
			int i;
			for (i = caretColumn - 1; i >= 0; i--)
			{
				if (!char.IsWhiteSpace(text[i]))
				{
					break;
				}
			}
			while (i >= 0 && !char.IsWhiteSpace(text[i]))
			{
				i--;
			}
			i++;
			this._yankBuffer = text.Substring(i, caretColumn - i);
			string text2 = text.Substring(0, i) + text.Substring(caretColumn);
			this._tabCompletion.ProgrammaticTextChange = true;
			this._inputBuffer.Text = text2;
			this._inputBuffer.CaretColumn = i;
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x001BDBA0 File Offset: 0x001BBDA0
		private void KillToEndOfLine()
		{
			string text = this._inputBuffer.Text;
			int caretColumn = this._inputBuffer.CaretColumn;
			if (caretColumn >= text.Length)
			{
				return;
			}
			this._yankBuffer = text.Substring(caretColumn);
			string text2 = text.Substring(0, caretColumn);
			this._tabCompletion.ProgrammaticTextChange = true;
			this._inputBuffer.Text = text2;
			this._inputBuffer.CaretColumn = caretColumn;
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x001BDC0C File Offset: 0x001BBE0C
		private void Yank()
		{
			if (string.IsNullOrEmpty(this._yankBuffer))
			{
				return;
			}
			string text = this._inputBuffer.Text;
			int caretColumn = this._inputBuffer.CaretColumn;
			string text2 = text.Insert(caretColumn, this._yankBuffer);
			this._tabCompletion.ProgrammaticTextChange = true;
			this._inputBuffer.Text = text2;
			this._inputBuffer.CaretColumn = caretColumn + this._yankBuffer.Length;
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x001BDC7D File Offset: 0x001BBE7D
		private void EnableTabBuffer()
		{
			this._outputBuffer.Visible = false;
			this._tabBuffer.Visible = true;
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x001BDC97 File Offset: 0x001BBE97
		private void DisableTabBuffer()
		{
			this._outputBuffer.Visible = true;
			this._tabBuffer.Visible = false;
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x001BDCB1 File Offset: 0x001BBEB1
		public void SetBackgroundColor(Color color)
		{
			base.Modulate = color;
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x001BDCBA File Offset: 0x001BBEBA
		private void HideGhostText()
		{
			this._ghostTextLabel.Visible = false;
			this._ghostTextLabel.Text = string.Empty;
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x001BDCD8 File Offset: 0x001BBED8
		private void ShowGhostText(string ghostText)
		{
			this._ghostTextLabel.Text = ghostText;
			this._ghostTextLabel.Visible = true;
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001BDCF4 File Offset: 0x001BBEF4
		private void UpdateGhostText()
		{
			if (this._tabCompletion.InSelectionMode)
			{
				this.HideGhostText();
				return;
			}
			string text = this._inputBuffer.Text;
			if (string.IsNullOrWhiteSpace(text))
			{
				this.HideGhostText();
				return;
			}
			CompletionResult completionResults = this._devConsole.GetCompletionResults(text);
			string text2 = DevConsole.CalculateGhostText(text, completionResults);
			if (text2 != null)
			{
				this.ShowGhostText(text2);
				return;
			}
			this.HideGhostText();
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001BDD58 File Offset: 0x001BBF58
		private void AutocompleteCommand()
		{
			string text = this._inputBuffer.Text;
			CompletionResult completionResult = this._devConsole.GetCompletionResults(text);
			this._tabCompletion.LastCompletionResult = completionResult;
			if (string.IsNullOrWhiteSpace(text) && completionResult.Candidates.Count == 0)
			{
				completionResult = this._devConsole.GetCompletionResults("");
				this._tabCompletion.LastCompletionResult = completionResult;
			}
			if (completionResult.Candidates.Count == 0)
			{
				this.ExitSelectionMode();
				return;
			}
			if (completionResult.Candidates.Count == 1)
			{
				this._tabCompletion.ProgrammaticTextChange = true;
				this._inputBuffer.Text = completionResult.CommonPrefix;
				this.MoveInputCursorToEndOfLine();
				this.ExitSelectionMode();
				return;
			}
			this._tabCompletion.CompletionCandidates.Clear();
			this._tabCompletion.CompletionCandidates.AddRange(completionResult.Candidates);
			this._tabCompletion.InSelectionMode = true;
			this._tabCompletion.SelectionIndex = 0;
			this.HideGhostText();
			this.RenderSelectionMenu();
			this.MoveInputCursorToEndOfLine();
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x001BDE58 File Offset: 0x001BC058
		private void RenderSelectionMenu()
		{
			if (!this._tabCompletion.InSelectionMode || this._tabCompletion.CompletionCandidates.Count == 0)
			{
				return;
			}
			List<string> list = new List<string>();
			CompletionResult lastCompletionResult = this._tabCompletion.LastCompletionResult;
			CompletionType? completionType = ((lastCompletionResult != null) ? new CompletionType?(lastCompletionResult.Type) : null);
			string text;
			if (completionType != null)
			{
				switch (completionType.GetValueOrDefault())
				{
				case CompletionType.Command:
					text = "Select command:";
					goto IL_00C6;
				case CompletionType.Subcommand:
					text = "Select " + this._tabCompletion.LastCompletionResult.ArgumentContext + " action:";
					goto IL_00C6;
				case CompletionType.Argument:
					text = "Select " + this._tabCompletion.LastCompletionResult.ArgumentContext + " argument:";
					goto IL_00C6;
				}
			}
			text = "Select option:";
			IL_00C6:
			string text2 = text;
			list.Add(text2);
			list.Add("[color=gray]Tab/" + this._symbolUp + this._symbolDown + ": navigate, Enter: accept, Esc: cancel, Type to filter[/color]");
			list.Add("");
			int count = this._tabCompletion.CompletionCandidates.Count;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (count <= 12)
			{
				for (int i = 0; i < count; i++)
				{
					this.AddCandidateToDisplay(list, i);
				}
			}
			else
			{
				int num = Math.Max(0, this._tabCompletion.SelectionIndex - 6);
				int num2 = Math.Min(count, num + 12);
				if (num2 - num < 12)
				{
					num = Math.Max(0, num2 - 12);
				}
				if (num > 0)
				{
					List<string> list2 = list;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
					defaultInterpolatedStringHandler.AppendLiteral("[color=gray]");
					defaultInterpolatedStringHandler.AppendFormatted(this._symbolUp);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(" more above ");
					defaultInterpolatedStringHandler.AppendFormatted(this._symbolUp);
					defaultInterpolatedStringHandler.AppendLiteral("[/color]");
					list2.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				for (int j = num; j < num2; j++)
				{
					this.AddCandidateToDisplay(list, j);
				}
				if (num2 < count)
				{
					int num3 = count - num2;
					List<string> list3 = list;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
					defaultInterpolatedStringHandler.AppendLiteral("[color=gray]");
					defaultInterpolatedStringHandler.AppendFormatted(this._symbolDown);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
					defaultInterpolatedStringHandler.AppendLiteral(" more below ");
					defaultInterpolatedStringHandler.AppendFormatted(this._symbolDown);
					defaultInterpolatedStringHandler.AppendLiteral("[/color]");
					list3.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			list.Add("");
			List<string> list4 = list;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("[color=gray](");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._tabCompletion.CompletionCandidates.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" matches)[/color]");
			list4.Add(defaultInterpolatedStringHandler.ToStringAndClear());
			this._tabBuffer.Text = string.Join("\n", list);
			this.EnableTabBuffer();
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x001BE138 File Offset: 0x001BC338
		private void AddCandidateToDisplay(List<string> displayLines, int index)
		{
			if (index < 0 || index >= this._tabCompletion.CompletionCandidates.Count)
			{
				return;
			}
			string text = this._tabCompletion.CompletionCandidates[index];
			string text2;
			if (index != this._tabCompletion.SelectionIndex)
			{
				text2 = "  " + text;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[color=yellow]");
				defaultInterpolatedStringHandler.AppendFormatted(this._symbolPrompt);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("[/color]");
				text2 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			string text3 = text2;
			displayLines.Add(text3);
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x001BE1E0 File Offset: 0x001BC3E0
		private void OnInputTextChanged(string newText)
		{
			bool programmaticTextChange = this._tabCompletion.ProgrammaticTextChange;
			this._tabCompletion.ProgrammaticTextChange = false;
			if (this._tabCompletion.InSelectionMode && !programmaticTextChange)
			{
				CompletionResult completionResults = this._devConsole.GetCompletionResults(newText);
				this._tabCompletion.LastCompletionResult = completionResults;
				if (completionResults.Candidates.Count == 0)
				{
					this.ExitSelectionMode();
				}
				else if (completionResults.Candidates.Count == 1)
				{
					this._tabCompletion.ProgrammaticTextChange = true;
					this._inputBuffer.Text = completionResults.CommonPrefix;
					this.MoveInputCursorToEndOfLine();
					this.ExitSelectionMode();
				}
				else
				{
					this._tabCompletion.CompletionCandidates.Clear();
					this._tabCompletion.CompletionCandidates.AddRange(completionResults.Candidates);
					this._tabCompletion.SelectionIndex = 0;
					this.RenderSelectionMenu();
				}
			}
			if (!programmaticTextChange)
			{
				this.UpdateGhostText();
			}
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x001BE2C3 File Offset: 0x001BC4C3
		private void ExitSelectionMode()
		{
			this._tabCompletion.Reset();
			this._tabBuffer.Text = string.Empty;
			this.DisableTabBuffer();
			this.UpdateGhostText();
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x001BE2EC File Offset: 0x001BC4EC
		private void NavigateSelection(int direction)
		{
			if (!this._tabCompletion.InSelectionMode || this._tabCompletion.CompletionCandidates.Count == 0)
			{
				return;
			}
			this._tabCompletion.SelectionIndex = (this._tabCompletion.SelectionIndex + direction + this._tabCompletion.CompletionCandidates.Count) % this._tabCompletion.CompletionCandidates.Count;
			this.RenderSelectionMenu();
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x001BE35C File Offset: 0x001BC55C
		private void AcceptSelection()
		{
			if (!this._tabCompletion.InSelectionMode || this._tabCompletion.SelectionIndex < 0 || this._tabCompletion.SelectionIndex >= this._tabCompletion.CompletionCandidates.Count)
			{
				return;
			}
			string text = this._tabCompletion.CompletionCandidates[this._tabCompletion.SelectionIndex];
			this._tabCompletion.ProgrammaticTextChange = true;
			string text2 = this._inputBuffer.Text;
			CompletionResult lastCompletionResult = this._tabCompletion.LastCompletionResult;
			if (lastCompletionResult != null)
			{
				CompletionType type = lastCompletionResult.Type;
				if (type != CompletionType.Command)
				{
					if (type - CompletionType.Subcommand <= 1)
					{
						string text3 = (text.Contains(' ') ? ("\"" + text + "\"") : text);
						if (text2.EndsWith(' '))
						{
							this._inputBuffer.Text = lastCompletionResult.CommandPrefix + text3 + " ";
						}
						else
						{
							this._inputBuffer.Text = lastCompletionResult.CommandPrefix + text3 + " ";
						}
					}
				}
				else
				{
					this._inputBuffer.Text = text + " ";
				}
			}
			else
			{
				string[] array = text2.Trim().Split(' ', StringSplitOptions.None);
				string[] array2 = ((array.Length > 1) ? array.Take<string>(array.Length - 1).ToArray<string>() : Array.Empty<string>());
				string text4 = ((array2.Length != 0) ? (string.Join(" ", array2) + " ") : string.Empty);
				this._inputBuffer.Text = text4 + text + " ";
			}
			this.MoveInputCursorToEndOfLine();
			this.ExitSelectionMode();
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x001BE4FC File Offset: 0x001BC6FC
		private void ProcessCommand()
		{
			if (string.IsNullOrWhiteSpace(this._inputBuffer.Text))
			{
				return;
			}
			RichTextLabel outputBuffer = this._outputBuffer;
			string text = outputBuffer.Text;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[color=#00ff00]");
			defaultInterpolatedStringHandler.AppendFormatted(this._symbolPrompt);
			defaultInterpolatedStringHandler.AppendLiteral("[/color] ");
			defaultInterpolatedStringHandler.AppendFormatted(this._inputBuffer.Text);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			outputBuffer.Text = text + defaultInterpolatedStringHandler.ToStringAndClear();
			if (this._inputBuffer.Text.Trim().Equals("clear"))
			{
				this._outputBuffer.Text = string.Empty;
				this._inputBuffer.Text = string.Empty;
				return;
			}
			if (this._inputBuffer.Text.Trim().Equals("exit"))
			{
				this.HideConsole();
				return;
			}
			Exception ex = null;
			CmdResult cmdResult;
			try
			{
				cmdResult = this._devConsole.ProcessCommand(this._inputBuffer.Text);
			}
			catch (Exception ex2)
			{
				bool flag = false;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("An exception occurred: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex2);
				cmdResult = new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
				ex = ex2;
			}
			if (cmdResult.success)
			{
				RichTextLabel outputBuffer2 = this._outputBuffer;
				outputBuffer2.Text = outputBuffer2.Text + cmdResult.msg + "\n";
			}
			else
			{
				RichTextLabel outputBuffer3 = this._outputBuffer;
				string text2 = outputBuffer3.Text;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[color=#ff5555]");
				defaultInterpolatedStringHandler.AppendFormatted(this._symbolWarning);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(cmdResult.msg);
				defaultInterpolatedStringHandler.AppendLiteral("[/color]\n");
				outputBuffer3.Text = text2 + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this._inputBuffer.Text = string.Empty;
			this._tabBuffer.Text = string.Empty;
			this.DisableTabBuffer();
			this.HideGhostText();
			if (ex != null)
			{
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x001BE710 File Offset: 0x001BC910
		public Task ProcessNetCommand([Nullable(2)] Player player, string netCommand)
		{
			Exception ex = null;
			CmdResult cmdResult;
			try
			{
				cmdResult = this._devConsole.ProcessNetCommand(player, netCommand);
			}
			catch (Exception ex2)
			{
				bool flag = false;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("An exception occurred: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex2);
				cmdResult = new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
				ex = ex2;
			}
			if (cmdResult.success)
			{
				RichTextLabel outputBuffer = this._outputBuffer;
				outputBuffer.Text = outputBuffer.Text + cmdResult.msg + "\n";
			}
			else
			{
				RichTextLabel outputBuffer2 = this._outputBuffer;
				string text = outputBuffer2.Text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[color=#ff5555]");
				defaultInterpolatedStringHandler.AppendFormatted(this._symbolWarning);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(cmdResult.msg);
				defaultInterpolatedStringHandler.AppendLiteral("[/color]\n");
				outputBuffer2.Text = text + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (ex != null)
			{
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			return cmdResult.task ?? Task.CompletedTask;
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x001BE820 File Offset: 0x001BCA20
		public void ShowConsole()
		{
			base.Visible = true;
			this._inputBuffer.CallDeferred(Control.MethodName.GrabFocus, Array.Empty<Variant>());
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x001BE83F File Offset: 0x001BCA3F
		public void HideConsole()
		{
			base.Visible = false;
			Viewport viewport = base.GetViewport();
			if (viewport == null)
			{
				return;
			}
			viewport.GuiReleaseFocus();
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x001BE858 File Offset: 0x001BCA58
		public void MakeHalfScreen()
		{
			Vector2 size = base.GetViewportRect().Size;
			float num = size.Y * 0.5f;
			this._inputContainer.SetSize(new Vector2(size.X, 40f), false);
			this._inputContainer.Position = new Vector2(0f, num - 40f);
			this._outputBuffer.SetSize(new Vector2(size.X, num), false);
			this._tabBuffer.SetSize(new Vector2(size.X, num), false);
			base.SetSize(new Vector2(size.X, size.Y / 2f), false);
			this._isFullscreen = false;
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x001BE910 File Offset: 0x001BCB10
		public void MakeFullScreen()
		{
			Vector2 size = base.GetViewportRect().Size;
			float y = size.Y;
			this._inputContainer.SetSize(new Vector2(size.X, 40f), false);
			this._inputContainer.Position = new Vector2(0f, y - 40f);
			this._outputBuffer.SetSize(new Vector2(this._outputBuffer.Size.X, y - 40f), false);
			this._tabBuffer.SetSize(new Vector2(this._outputBuffer.Size.X, y - 40f), false);
			base.SetSize(size, false);
			this._isFullscreen = true;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x001BE9CA File Offset: 0x001BCBCA
		private void OnToggleMaximizeButtonPressed()
		{
			if (!this._isFullscreen)
			{
				this.MakeFullScreen();
				return;
			}
			this.MakeHalfScreen();
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x001BE9E1 File Offset: 0x001BCBE1
		public void MoveInputCursorToEndOfLine()
		{
			this._inputBuffer.CaretColumn = this._inputBuffer.Text.Length;
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x001BEA00 File Offset: 0x001BCC00
		private void UpdatePromptStyle()
		{
			this._promptLabel.Text = this._symbolPrompt;
			this._promptLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, new Color(0f, 0.831f, 1f, 1f));
			this._promptLabel.AddThemeFontSizeOverride(ThemeConstants.Label.FontSize, 18);
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x001BEA59 File Offset: 0x001BCC59
		public void AddChildToTree(Node node)
		{
			this.AddChildSafely(node);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A9A RID: 15002 RVA: 0x001BEA64 File Offset: 0x001BCC64
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(31)
			{
				new MethodInfo(NDevConsole.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("CanvasLayer"), false), 33L, null, null),
				new MethodInfo(NDevConsole.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.PrintUsage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NDevConsole.MethodName.HandleReadlineKeybinding, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "keyEvent", 0L, "", 6L, new StringName("InputEventKey"), false)
				}, null),
				new MethodInfo(NDevConsole.MethodName.DeleteWordBackward, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.KillToEndOfLine, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.Yank, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.EnableTabBuffer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.DisableTabBuffer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.SetBackgroundColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "color", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDevConsole.MethodName.HideGhostText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.ShowGhostText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "ghostText", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDevConsole.MethodName.UpdateGhostText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.AutocompleteCommand, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.RenderSelectionMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.OnInputTextChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "newText", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDevConsole.MethodName.ExitSelectionMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.NavigateSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "direction", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDevConsole.MethodName.AcceptSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.ProcessCommand, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.ShowConsole, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.HideConsole, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.MakeHalfScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.MakeFullScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.OnToggleMaximizeButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.MoveInputCursorToEndOfLine, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.UpdatePromptStyle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevConsole.MethodName.AddChildToTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A9B RID: 15003 RVA: 0x001BF15C File Offset: 0x001BD35C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDevConsole.MethodName.Create && args.Count == 0)
			{
				CanvasLayer canvasLayer = NDevConsole.Create();
				ret = VariantUtils.CreateFrom<CanvasLayer>(ref canvasLayer);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.PrintUsage && args.Count == 0)
			{
				this.PrintUsage();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.HandleReadlineKeybinding && args.Count == 1)
			{
				this.HandleReadlineKeybinding(VariantUtils.ConvertTo<InputEventKey>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.DeleteWordBackward && args.Count == 0)
			{
				this.DeleteWordBackward();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.KillToEndOfLine && args.Count == 0)
			{
				this.KillToEndOfLine();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.Yank && args.Count == 0)
			{
				this.Yank();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.EnableTabBuffer && args.Count == 0)
			{
				this.EnableTabBuffer();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.DisableTabBuffer && args.Count == 0)
			{
				this.DisableTabBuffer();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.SetBackgroundColor && args.Count == 1)
			{
				this.SetBackgroundColor(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.HideGhostText && args.Count == 0)
			{
				this.HideGhostText();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.ShowGhostText && args.Count == 1)
			{
				this.ShowGhostText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.UpdateGhostText && args.Count == 0)
			{
				this.UpdateGhostText();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.AutocompleteCommand && args.Count == 0)
			{
				this.AutocompleteCommand();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.RenderSelectionMenu && args.Count == 0)
			{
				this.RenderSelectionMenu();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.OnInputTextChanged && args.Count == 1)
			{
				this.OnInputTextChanged(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.ExitSelectionMode && args.Count == 0)
			{
				this.ExitSelectionMode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.NavigateSelection && args.Count == 1)
			{
				this.NavigateSelection(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.AcceptSelection && args.Count == 0)
			{
				this.AcceptSelection();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.ProcessCommand && args.Count == 0)
			{
				this.ProcessCommand();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.ShowConsole && args.Count == 0)
			{
				this.ShowConsole();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.HideConsole && args.Count == 0)
			{
				this.HideConsole();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.MakeHalfScreen && args.Count == 0)
			{
				this.MakeHalfScreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.MakeFullScreen && args.Count == 0)
			{
				this.MakeFullScreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.OnToggleMaximizeButtonPressed && args.Count == 0)
			{
				this.OnToggleMaximizeButtonPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.MoveInputCursorToEndOfLine && args.Count == 0)
			{
				this.MoveInputCursorToEndOfLine();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.UpdatePromptStyle && args.Count == 0)
			{
				this.UpdatePromptStyle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevConsole.MethodName.AddChildToTree && args.Count == 1)
			{
				this.AddChildToTree(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x001BF658 File Offset: 0x001BD858
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDevConsole.MethodName.Create && args.Count == 0)
			{
				CanvasLayer canvasLayer = NDevConsole.Create();
				ret = VariantUtils.CreateFrom<CanvasLayer>(ref canvasLayer);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003A9D RID: 15005 RVA: 0x001BF698 File Offset: 0x001BD898
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDevConsole.MethodName.Create || (ref method) == NDevConsole.MethodName._EnterTree || (ref method) == NDevConsole.MethodName._Ready || (ref method) == NDevConsole.MethodName._ExitTree || (ref method) == NDevConsole.MethodName.PrintUsage || (ref method) == NDevConsole.MethodName._Input || (ref method) == NDevConsole.MethodName.HandleReadlineKeybinding || (ref method) == NDevConsole.MethodName.DeleteWordBackward || (ref method) == NDevConsole.MethodName.KillToEndOfLine || (ref method) == NDevConsole.MethodName.Yank || (ref method) == NDevConsole.MethodName.EnableTabBuffer || (ref method) == NDevConsole.MethodName.DisableTabBuffer || (ref method) == NDevConsole.MethodName.SetBackgroundColor || (ref method) == NDevConsole.MethodName.HideGhostText || (ref method) == NDevConsole.MethodName.ShowGhostText || (ref method) == NDevConsole.MethodName.UpdateGhostText || (ref method) == NDevConsole.MethodName.AutocompleteCommand || (ref method) == NDevConsole.MethodName.RenderSelectionMenu || (ref method) == NDevConsole.MethodName.OnInputTextChanged || (ref method) == NDevConsole.MethodName.ExitSelectionMode || (ref method) == NDevConsole.MethodName.NavigateSelection || (ref method) == NDevConsole.MethodName.AcceptSelection || (ref method) == NDevConsole.MethodName.ProcessCommand || (ref method) == NDevConsole.MethodName.ShowConsole || (ref method) == NDevConsole.MethodName.HideConsole || (ref method) == NDevConsole.MethodName.MakeHalfScreen || (ref method) == NDevConsole.MethodName.MakeFullScreen || (ref method) == NDevConsole.MethodName.OnToggleMaximizeButtonPressed || (ref method) == NDevConsole.MethodName.MoveInputCursorToEndOfLine || (ref method) == NDevConsole.MethodName.UpdatePromptStyle || (ref method) == NDevConsole.MethodName.AddChildToTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003A9E RID: 15006 RVA: 0x001BF880 File Offset: 0x001BDA80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDevConsole.PropertyName._outputBuffer)
			{
				this._outputBuffer = VariantUtils.ConvertTo<RichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._tabBuffer)
			{
				this._tabBuffer = VariantUtils.ConvertTo<RichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._inputContainer)
			{
				this._inputContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._inputBuffer)
			{
				this._inputBuffer = VariantUtils.ConvertTo<LineEdit>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._promptLabel)
			{
				this._promptLabel = VariantUtils.ConvertTo<Label>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._ghostTextLabel)
			{
				this._ghostTextLabel = VariantUtils.ConvertTo<Label>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._isFullscreen)
			{
				this._isFullscreen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._yankBuffer)
			{
				this._yankBuffer = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolPrompt)
			{
				this._symbolPrompt = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolWarning)
			{
				this._symbolWarning = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolUp)
			{
				this._symbolUp = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolDown)
			{
				this._symbolDown = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003A9F RID: 15007 RVA: 0x001BF9DC File Offset: 0x001BDBDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDevConsole.PropertyName._outputBuffer)
			{
				value = VariantUtils.CreateFrom<RichTextLabel>(ref this._outputBuffer);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._tabBuffer)
			{
				value = VariantUtils.CreateFrom<RichTextLabel>(ref this._tabBuffer);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._inputContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._inputContainer);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._inputBuffer)
			{
				value = VariantUtils.CreateFrom<LineEdit>(ref this._inputBuffer);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._promptLabel)
			{
				value = VariantUtils.CreateFrom<Label>(ref this._promptLabel);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._ghostTextLabel)
			{
				value = VariantUtils.CreateFrom<Label>(ref this._ghostTextLabel);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._isFullscreen)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFullscreen);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._yankBuffer)
			{
				value = VariantUtils.CreateFrom<string>(ref this._yankBuffer);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolPrompt)
			{
				value = VariantUtils.CreateFrom<string>(ref this._symbolPrompt);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolWarning)
			{
				value = VariantUtils.CreateFrom<string>(ref this._symbolWarning);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolUp)
			{
				value = VariantUtils.CreateFrom<string>(ref this._symbolUp);
				return true;
			}
			if ((ref name) == NDevConsole.PropertyName._symbolDown)
			{
				value = VariantUtils.CreateFrom<string>(ref this._symbolDown);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003AA0 RID: 15008 RVA: 0x001BFB74 File Offset: 0x001BDD74
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDevConsole.PropertyName._outputBuffer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDevConsole.PropertyName._tabBuffer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDevConsole.PropertyName._inputContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDevConsole.PropertyName._inputBuffer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDevConsole.PropertyName._promptLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDevConsole.PropertyName._ghostTextLabel, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDevConsole.PropertyName._isFullscreen, 0L, "", 4096L, false),
				new PropertyInfo(4L, NDevConsole.PropertyName._yankBuffer, 0L, "", 4096L, false),
				new PropertyInfo(4L, NDevConsole.PropertyName._symbolPrompt, 0L, "", 4096L, false),
				new PropertyInfo(4L, NDevConsole.PropertyName._symbolWarning, 0L, "", 4096L, false),
				new PropertyInfo(4L, NDevConsole.PropertyName._symbolUp, 0L, "", 4096L, false),
				new PropertyInfo(4L, NDevConsole.PropertyName._symbolDown, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003AA1 RID: 15009 RVA: 0x001BFD10 File Offset: 0x001BDF10
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDevConsole.PropertyName._outputBuffer, Variant.From<RichTextLabel>(ref this._outputBuffer));
			info.AddProperty(NDevConsole.PropertyName._tabBuffer, Variant.From<RichTextLabel>(ref this._tabBuffer));
			info.AddProperty(NDevConsole.PropertyName._inputContainer, Variant.From<Control>(ref this._inputContainer));
			info.AddProperty(NDevConsole.PropertyName._inputBuffer, Variant.From<LineEdit>(ref this._inputBuffer));
			info.AddProperty(NDevConsole.PropertyName._promptLabel, Variant.From<Label>(ref this._promptLabel));
			info.AddProperty(NDevConsole.PropertyName._ghostTextLabel, Variant.From<Label>(ref this._ghostTextLabel));
			info.AddProperty(NDevConsole.PropertyName._isFullscreen, Variant.From<bool>(ref this._isFullscreen));
			info.AddProperty(NDevConsole.PropertyName._yankBuffer, Variant.From<string>(ref this._yankBuffer));
			info.AddProperty(NDevConsole.PropertyName._symbolPrompt, Variant.From<string>(ref this._symbolPrompt));
			info.AddProperty(NDevConsole.PropertyName._symbolWarning, Variant.From<string>(ref this._symbolWarning));
			info.AddProperty(NDevConsole.PropertyName._symbolUp, Variant.From<string>(ref this._symbolUp));
			info.AddProperty(NDevConsole.PropertyName._symbolDown, Variant.From<string>(ref this._symbolDown));
		}

		/// <inheritdoc />
		// Token: 0x06003AA2 RID: 15010 RVA: 0x001BFE2C File Offset: 0x001BE02C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDevConsole.PropertyName._outputBuffer, ref variant))
			{
				this._outputBuffer = variant.As<RichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDevConsole.PropertyName._tabBuffer, ref variant2))
			{
				this._tabBuffer = variant2.As<RichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDevConsole.PropertyName._inputContainer, ref variant3))
			{
				this._inputContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDevConsole.PropertyName._inputBuffer, ref variant4))
			{
				this._inputBuffer = variant4.As<LineEdit>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDevConsole.PropertyName._promptLabel, ref variant5))
			{
				this._promptLabel = variant5.As<Label>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDevConsole.PropertyName._ghostTextLabel, ref variant6))
			{
				this._ghostTextLabel = variant6.As<Label>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDevConsole.PropertyName._isFullscreen, ref variant7))
			{
				this._isFullscreen = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDevConsole.PropertyName._yankBuffer, ref variant8))
			{
				this._yankBuffer = variant8.As<string>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDevConsole.PropertyName._symbolPrompt, ref variant9))
			{
				this._symbolPrompt = variant9.As<string>();
			}
			Variant variant10;
			if (info.TryGetProperty(NDevConsole.PropertyName._symbolWarning, ref variant10))
			{
				this._symbolWarning = variant10.As<string>();
			}
			Variant variant11;
			if (info.TryGetProperty(NDevConsole.PropertyName._symbolUp, ref variant11))
			{
				this._symbolUp = variant11.As<string>();
			}
			Variant variant12;
			if (info.TryGetProperty(NDevConsole.PropertyName._symbolDown, ref variant12))
			{
				this._symbolDown = variant12.As<string>();
			}
		}

		// Token: 0x040016D5 RID: 5845
		public static readonly string assetPath = SceneHelper.GetScenePath("debug/dev_console");

		// Token: 0x040016D6 RID: 5846
		[Nullable(2)]
		private static NDevConsole _instance;

		// Token: 0x040016D7 RID: 5847
		private RichTextLabel _outputBuffer;

		// Token: 0x040016D8 RID: 5848
		private RichTextLabel _tabBuffer;

		// Token: 0x040016D9 RID: 5849
		private Control _inputContainer;

		// Token: 0x040016DA RID: 5850
		private LineEdit _inputBuffer;

		// Token: 0x040016DB RID: 5851
		private Label _promptLabel;

		// Token: 0x040016DC RID: 5852
		private Label _ghostTextLabel;

		// Token: 0x040016DD RID: 5853
		private bool _isFullscreen;

		// Token: 0x040016DE RID: 5854
		private DevConsole _devConsole;

		// Token: 0x040016DF RID: 5855
		private const float _inputBufferSizeY = 40f;

		// Token: 0x040016E0 RID: 5856
		private readonly TabCompletionState _tabCompletion = new TabCompletionState();

		// Token: 0x040016E1 RID: 5857
		private string _yankBuffer = string.Empty;

		// Token: 0x040016E2 RID: 5858
		private string _symbolPrompt = ">";

		// Token: 0x040016E3 RID: 5859
		private string _symbolWarning = "[!]";

		// Token: 0x040016E4 RID: 5860
		private string _symbolUp = "^";

		// Token: 0x040016E5 RID: 5861
		private string _symbolDown = "v";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017FC RID: 6140
		[NullableContext(0)]
		public class MethodName : Panel.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005DA0 RID: 23968
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005DA1 RID: 23969
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005DA2 RID: 23970
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005DA3 RID: 23971
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'PrintUsage' method.
			/// </summary>
			// Token: 0x04005DA4 RID: 23972
			public static readonly StringName PrintUsage = "PrintUsage";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005DA5 RID: 23973
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'HandleReadlineKeybinding' method.
			/// </summary>
			// Token: 0x04005DA6 RID: 23974
			public static readonly StringName HandleReadlineKeybinding = "HandleReadlineKeybinding";

			/// <summary>
			/// Cached name for the 'DeleteWordBackward' method.
			/// </summary>
			// Token: 0x04005DA7 RID: 23975
			public static readonly StringName DeleteWordBackward = "DeleteWordBackward";

			/// <summary>
			/// Cached name for the 'KillToEndOfLine' method.
			/// </summary>
			// Token: 0x04005DA8 RID: 23976
			public static readonly StringName KillToEndOfLine = "KillToEndOfLine";

			/// <summary>
			/// Cached name for the 'Yank' method.
			/// </summary>
			// Token: 0x04005DA9 RID: 23977
			public static readonly StringName Yank = "Yank";

			/// <summary>
			/// Cached name for the 'EnableTabBuffer' method.
			/// </summary>
			// Token: 0x04005DAA RID: 23978
			public static readonly StringName EnableTabBuffer = "EnableTabBuffer";

			/// <summary>
			/// Cached name for the 'DisableTabBuffer' method.
			/// </summary>
			// Token: 0x04005DAB RID: 23979
			public static readonly StringName DisableTabBuffer = "DisableTabBuffer";

			/// <summary>
			/// Cached name for the 'SetBackgroundColor' method.
			/// </summary>
			// Token: 0x04005DAC RID: 23980
			public static readonly StringName SetBackgroundColor = "SetBackgroundColor";

			/// <summary>
			/// Cached name for the 'HideGhostText' method.
			/// </summary>
			// Token: 0x04005DAD RID: 23981
			public static readonly StringName HideGhostText = "HideGhostText";

			/// <summary>
			/// Cached name for the 'ShowGhostText' method.
			/// </summary>
			// Token: 0x04005DAE RID: 23982
			public static readonly StringName ShowGhostText = "ShowGhostText";

			/// <summary>
			/// Cached name for the 'UpdateGhostText' method.
			/// </summary>
			// Token: 0x04005DAF RID: 23983
			public static readonly StringName UpdateGhostText = "UpdateGhostText";

			/// <summary>
			/// Cached name for the 'AutocompleteCommand' method.
			/// </summary>
			// Token: 0x04005DB0 RID: 23984
			public static readonly StringName AutocompleteCommand = "AutocompleteCommand";

			/// <summary>
			/// Cached name for the 'RenderSelectionMenu' method.
			/// </summary>
			// Token: 0x04005DB1 RID: 23985
			public static readonly StringName RenderSelectionMenu = "RenderSelectionMenu";

			/// <summary>
			/// Cached name for the 'OnInputTextChanged' method.
			/// </summary>
			// Token: 0x04005DB2 RID: 23986
			public static readonly StringName OnInputTextChanged = "OnInputTextChanged";

			/// <summary>
			/// Cached name for the 'ExitSelectionMode' method.
			/// </summary>
			// Token: 0x04005DB3 RID: 23987
			public static readonly StringName ExitSelectionMode = "ExitSelectionMode";

			/// <summary>
			/// Cached name for the 'NavigateSelection' method.
			/// </summary>
			// Token: 0x04005DB4 RID: 23988
			public static readonly StringName NavigateSelection = "NavigateSelection";

			/// <summary>
			/// Cached name for the 'AcceptSelection' method.
			/// </summary>
			// Token: 0x04005DB5 RID: 23989
			public static readonly StringName AcceptSelection = "AcceptSelection";

			/// <summary>
			/// Cached name for the 'ProcessCommand' method.
			/// </summary>
			// Token: 0x04005DB6 RID: 23990
			public static readonly StringName ProcessCommand = "ProcessCommand";

			/// <summary>
			/// Cached name for the 'ShowConsole' method.
			/// </summary>
			// Token: 0x04005DB7 RID: 23991
			public static readonly StringName ShowConsole = "ShowConsole";

			/// <summary>
			/// Cached name for the 'HideConsole' method.
			/// </summary>
			// Token: 0x04005DB8 RID: 23992
			public static readonly StringName HideConsole = "HideConsole";

			/// <summary>
			/// Cached name for the 'MakeHalfScreen' method.
			/// </summary>
			// Token: 0x04005DB9 RID: 23993
			public static readonly StringName MakeHalfScreen = "MakeHalfScreen";

			/// <summary>
			/// Cached name for the 'MakeFullScreen' method.
			/// </summary>
			// Token: 0x04005DBA RID: 23994
			public static readonly StringName MakeFullScreen = "MakeFullScreen";

			/// <summary>
			/// Cached name for the 'OnToggleMaximizeButtonPressed' method.
			/// </summary>
			// Token: 0x04005DBB RID: 23995
			public static readonly StringName OnToggleMaximizeButtonPressed = "OnToggleMaximizeButtonPressed";

			/// <summary>
			/// Cached name for the 'MoveInputCursorToEndOfLine' method.
			/// </summary>
			// Token: 0x04005DBC RID: 23996
			public static readonly StringName MoveInputCursorToEndOfLine = "MoveInputCursorToEndOfLine";

			/// <summary>
			/// Cached name for the 'UpdatePromptStyle' method.
			/// </summary>
			// Token: 0x04005DBD RID: 23997
			public static readonly StringName UpdatePromptStyle = "UpdatePromptStyle";

			/// <summary>
			/// Cached name for the 'AddChildToTree' method.
			/// </summary>
			// Token: 0x04005DBE RID: 23998
			public static readonly StringName AddChildToTree = "AddChildToTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017FD RID: 6141
		[NullableContext(0)]
		public class PropertyName : Panel.PropertyName
		{
			/// <summary>
			/// Cached name for the '_outputBuffer' field.
			/// </summary>
			// Token: 0x04005DBF RID: 23999
			public static readonly StringName _outputBuffer = "_outputBuffer";

			/// <summary>
			/// Cached name for the '_tabBuffer' field.
			/// </summary>
			// Token: 0x04005DC0 RID: 24000
			public static readonly StringName _tabBuffer = "_tabBuffer";

			/// <summary>
			/// Cached name for the '_inputContainer' field.
			/// </summary>
			// Token: 0x04005DC1 RID: 24001
			public static readonly StringName _inputContainer = "_inputContainer";

			/// <summary>
			/// Cached name for the '_inputBuffer' field.
			/// </summary>
			// Token: 0x04005DC2 RID: 24002
			public static readonly StringName _inputBuffer = "_inputBuffer";

			/// <summary>
			/// Cached name for the '_promptLabel' field.
			/// </summary>
			// Token: 0x04005DC3 RID: 24003
			public static readonly StringName _promptLabel = "_promptLabel";

			/// <summary>
			/// Cached name for the '_ghostTextLabel' field.
			/// </summary>
			// Token: 0x04005DC4 RID: 24004
			public static readonly StringName _ghostTextLabel = "_ghostTextLabel";

			/// <summary>
			/// Cached name for the '_isFullscreen' field.
			/// </summary>
			// Token: 0x04005DC5 RID: 24005
			public static readonly StringName _isFullscreen = "_isFullscreen";

			/// <summary>
			/// Cached name for the '_yankBuffer' field.
			/// </summary>
			// Token: 0x04005DC6 RID: 24006
			public static readonly StringName _yankBuffer = "_yankBuffer";

			/// <summary>
			/// Cached name for the '_symbolPrompt' field.
			/// </summary>
			// Token: 0x04005DC7 RID: 24007
			public static readonly StringName _symbolPrompt = "_symbolPrompt";

			/// <summary>
			/// Cached name for the '_symbolWarning' field.
			/// </summary>
			// Token: 0x04005DC8 RID: 24008
			public static readonly StringName _symbolWarning = "_symbolWarning";

			/// <summary>
			/// Cached name for the '_symbolUp' field.
			/// </summary>
			// Token: 0x04005DC9 RID: 24009
			public static readonly StringName _symbolUp = "_symbolUp";

			/// <summary>
			/// Cached name for the '_symbolDown' field.
			/// </summary>
			// Token: 0x04005DCA RID: 24010
			public static readonly StringName _symbolDown = "_symbolDown";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017FE RID: 6142
		[NullableContext(0)]
		public class SignalName : Panel.SignalName
		{
		}
	}
}
