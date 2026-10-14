using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000310 RID: 784
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NPatchNotesScreen.cs")]
	public class NPatchNotesScreen : Control, IScreenContext
	{
		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x0013156D File Offset: 0x0012F76D
		// (set) Token: 0x06002B53 RID: 11091 RVA: 0x00131575 File Offset: 0x0012F775
		public bool IsOpen { get; private set; }

		// Token: 0x06002B54 RID: 11092 RVA: 0x00131580 File Offset: 0x0012F780
		public override void _Ready()
		{
			this._cachedScene = ResourceLoader.Load<PackedScene>("res://scenes/screens/patch_screen_contents.tscn", null, 1L);
			this.CreateNewPatchEntry();
			this._prevButton = base.GetNode<NButton>("PrevButton");
			this._prevButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.PreviousPatchNote();
			}), 0U);
			this._nextButton = base.GetNode<NButton>("NextButton");
			this._nextButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.NextPatchNote();
			}), 0U);
			this._nextButton.Visible = false;
			this._patchNotesToggle = base.GetNode<NButton>("%PatchNotesToggle");
			this._patchNotesToggle.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.Close();
			}), 0U);
			this._patchNotesToggle.Disable();
			this._backButton = base.GetNode<NButton>("%BackButton");
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.Close();
			}), 0U);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x001316A4 File Offset: 0x0012F8A4
		private void CreateNewPatchEntry()
		{
			this._screenContents = this._cachedScene.Instantiate<NScrollableContainer>(0L);
			this.AddChildSafely(this._screenContents);
			this.MoveChildSafely(this._screenContents, 0);
			this._marginContainer = this._screenContents.GetNode<MarginContainer>("Content");
			this._patchText = this._screenContents.GetNode<MegaRichTextLabel>("Content/PatchText");
			this._dateLabel = this._patchText.GetNode<MegaLabel>("DateLabel");
			if (this._patchNotePaths != null)
			{
				string text = this._patchNotePaths[this._index];
				this.LoadPatchNoteText(text);
			}
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x00131750 File Offset: 0x0012F950
		private void NextPatchNote()
		{
			if (!this._nextButton.Visible)
			{
				return;
			}
			if (this._patchNotePaths == null)
			{
				Log.Error("NPatchNotesScreen: No patch paths available!", 2);
				return;
			}
			this._index--;
			this._prevButton.Visible = true;
			if (this._index == 0)
			{
				this._nextButton.Visible = false;
			}
			this._screenContents.QueueFreeSafely();
			this.CreateNewPatchEntry();
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x001317C0 File Offset: 0x0012F9C0
		private void PreviousPatchNote()
		{
			if (this._patchNotePaths == null)
			{
				Log.Error("NPatchNotesScreen: No patch paths available!", 2);
				return;
			}
			this._index++;
			this._nextButton.Visible = true;
			if (this._index == this._patchNotePaths.Count - 1)
			{
				this._prevButton.Visible = false;
			}
			this._screenContents.QueueFreeSafely();
			this.CreateNewPatchEntry();
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x00131830 File Offset: 0x0012FA30
		public void Open()
		{
			this.IsOpen = true;
			NMainMenu mainMenu = NGame.Instance.MainMenu;
			if (mainMenu != null)
			{
				mainMenu.EnableBackstop();
			}
			this._patchNotesToggle.Enable();
			this._backButton.Enable();
			base.Visible = true;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.25);
			if (this._patchNotePaths == null)
			{
				this._patchNotePaths = (from fileName in DirAccess.GetFilesAt("res://localization/eng/patch_notes")
					select "res://localization/eng/patch_notes/" + fileName).Reverse<string>().ToList<string>();
			}
			this.LoadPatchNoteText(this._patchNotePaths[this._index]);
			ActiveScreenContext.Instance.Update();
			NHotkeyManager.Instance.PushHotkeyReleasedBinding(MegaInput.left, new Action(this.PreviousPatchNote));
			NHotkeyManager.Instance.PushHotkeyReleasedBinding(MegaInput.right, new Action(this.NextPatchNote));
			NHotkeyManager.Instance.PushHotkeyReleasedBinding(MegaInput.pauseAndBack, new Action(this.Close));
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x0013198C File Offset: 0x0012FB8C
		private void Close()
		{
			NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(MegaInput.left, new Action(this.PreviousPatchNote));
			NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(MegaInput.right, new Action(this.NextPatchNote));
			NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(MegaInput.pauseAndBack, new Action(this.Close));
			this._patchNotesToggle.Disable();
			this._backButton.Disable();
			NMainMenu mainMenu = NGame.Instance.MainMenu;
			if (mainMenu != null)
			{
				mainMenu.DisableBackstop();
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.25);
			this._tween.TweenCallback(Callable.From(delegate
			{
				this.IsOpen = false;
				base.SetVisible(false);
				ActiveScreenContext.Instance.Update();
			}));
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x00131A90 File Offset: 0x0012FC90
		private void LoadPatchNoteText(string patchNotePath)
		{
			this._patchText.ScrollToLine(0);
			this._currentScrollLine = 0;
			string text = NPatchNotesScreen.ReadPatchNoteFile(patchNotePath);
			this._patchText.SetTextAutoSize(text);
			this.UpdateDateLabel(patchNotePath);
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x00131ACC File Offset: 0x0012FCCC
		private static string ReadPatchNoteFile(string engPatchNotePath)
		{
			string language = LocManager.Instance.Language;
			if (language != "eng")
			{
				string fileNameFromPath = NPatchNotesScreen.GetFileNameFromPath(engPatchNotePath);
				string text = "res://localization/" + language + "/patch_notes/" + fileNameFromPath;
				if (FileAccess.FileExists(text))
				{
					using (FileAccess fileAccess = FileAccess.Open(text, 1L))
					{
						if (fileAccess != null)
						{
							return fileAccess.GetAsText(false);
						}
						Log.Warn("Failed to open localized patch notes: " + text, 2);
					}
				}
			}
			string text2;
			using (FileAccess fileAccess2 = FileAccess.Open(engPatchNotePath, 1L))
			{
				if (fileAccess2 == null)
				{
					Log.Error("Failed to open patch notes: " + engPatchNotePath, 2);
					text2 = "";
				}
				else
				{
					text2 = fileAccess2.GetAsText(false);
				}
			}
			return text2;
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x00131BA4 File Offset: 0x0012FDA4
		private void UpdateDateLabel(string patchNotePath)
		{
			string fileNameFromPath = NPatchNotesScreen.GetFileNameFromPath(patchNotePath);
			string text = NPatchNotesScreen.RemoveFileExtension(fileNameFromPath);
			string text2;
			if (NPatchNotesScreen.TryParseDate(text, out text2))
			{
				this._dateLabel.SetTextAutoSize(text2);
				return;
			}
			Log.Error("Invalid date format in file name: " + text, 2);
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x00131BE8 File Offset: 0x0012FDE8
		private static string GetFileNameFromPath(string path)
		{
			int num = path.LastIndexOf('/') + 1;
			return path.Substring(num, path.Length - num);
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x00131C11 File Offset: 0x0012FE11
		private static string RemoveFileExtension(string fileName)
		{
			return fileName.Split('.', StringSplitOptions.None)[0];
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x00131C20 File Offset: 0x0012FE20
		private static bool TryParseDate(string dateString, out string formattedDate)
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(dateString, "yyyy_MM_d", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
			{
				formattedDate = dateTime.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);
				return true;
			}
			formattedDate = string.Empty;
			return false;
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002B60 RID: 11104 RVA: 0x00131C5F File Offset: 0x0012FE5F
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B61 RID: 11105 RVA: 0x00131C64 File Offset: 0x0012FE64
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NPatchNotesScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesScreen.MethodName.CreateNewPatchEntry, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesScreen.MethodName.NextPatchNote, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesScreen.MethodName.PreviousPatchNote, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesScreen.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesScreen.MethodName.LoadPatchNoteText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "patchNotePath", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPatchNotesScreen.MethodName.ReadPatchNoteFile, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "engPatchNotePath", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPatchNotesScreen.MethodName.UpdateDateLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "patchNotePath", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPatchNotesScreen.MethodName.GetFileNameFromPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPatchNotesScreen.MethodName.RemoveFileExtension, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "fileName", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B62 RID: 11106 RVA: 0x00131F3C File Offset: 0x0013013C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPatchNotesScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.CreateNewPatchEntry && args.Count == 0)
			{
				this.CreateNewPatchEntry();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.NextPatchNote && args.Count == 0)
			{
				this.NextPatchNote();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.PreviousPatchNote && args.Count == 0)
			{
				this.PreviousPatchNote();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.Close && args.Count == 0)
			{
				this.Close();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.LoadPatchNoteText && args.Count == 1)
			{
				this.LoadPatchNoteText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.ReadPatchNoteFile && args.Count == 1)
			{
				string text = NPatchNotesScreen.ReadPatchNoteFile(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.UpdateDateLabel && args.Count == 1)
			{
				this.UpdateDateLabel(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.GetFileNameFromPath && args.Count == 1)
			{
				string fileNameFromPath = NPatchNotesScreen.GetFileNameFromPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref fileNameFromPath);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.RemoveFileExtension && args.Count == 1)
			{
				string text2 = NPatchNotesScreen.RemoveFileExtension(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text2);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x00132144 File Offset: 0x00130344
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPatchNotesScreen.MethodName.ReadPatchNoteFile && args.Count == 1)
			{
				string text = NPatchNotesScreen.ReadPatchNoteFile(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.GetFileNameFromPath && args.Count == 1)
			{
				string fileNameFromPath = NPatchNotesScreen.GetFileNameFromPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref fileNameFromPath);
				return true;
			}
			if ((ref method) == NPatchNotesScreen.MethodName.RemoveFileExtension && args.Count == 1)
			{
				string text2 = NPatchNotesScreen.RemoveFileExtension(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002B64 RID: 11108 RVA: 0x00132204 File Offset: 0x00130404
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPatchNotesScreen.MethodName._Ready || (ref method) == NPatchNotesScreen.MethodName.CreateNewPatchEntry || (ref method) == NPatchNotesScreen.MethodName.NextPatchNote || (ref method) == NPatchNotesScreen.MethodName.PreviousPatchNote || (ref method) == NPatchNotesScreen.MethodName.Open || (ref method) == NPatchNotesScreen.MethodName.Close || (ref method) == NPatchNotesScreen.MethodName.LoadPatchNoteText || (ref method) == NPatchNotesScreen.MethodName.ReadPatchNoteFile || (ref method) == NPatchNotesScreen.MethodName.UpdateDateLabel || (ref method) == NPatchNotesScreen.MethodName.GetFileNameFromPath || (ref method) == NPatchNotesScreen.MethodName.RemoveFileExtension || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002B65 RID: 11109 RVA: 0x001322C0 File Offset: 0x001304C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPatchNotesScreen.PropertyName.IsOpen)
			{
				this.IsOpen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._screenContents)
			{
				this._screenContents = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._marginContainer)
			{
				this._marginContainer = VariantUtils.ConvertTo<MarginContainer>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._prevButton)
			{
				this._prevButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._nextButton)
			{
				this._nextButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._patchNotesToggle)
			{
				this._patchNotesToggle = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._dateLabel)
			{
				this._dateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._patchText)
			{
				this._patchText = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._cachedScene)
			{
				this._cachedScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._index)
			{
				this._index = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._currentScrollLine)
			{
				this._currentScrollLine = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002B66 RID: 11110 RVA: 0x00132434 File Offset: 0x00130634
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPatchNotesScreen.PropertyName.IsOpen)
			{
				bool isOpen = this.IsOpen;
				value = VariantUtils.CreateFrom<bool>(ref isOpen);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._screenContents)
			{
				value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._screenContents);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._marginContainer)
			{
				value = VariantUtils.CreateFrom<MarginContainer>(ref this._marginContainer);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._prevButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._prevButton);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._nextButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._nextButton);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._patchNotesToggle)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._patchNotesToggle);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._dateLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._dateLabel);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._patchText)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._patchText);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._cachedScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._cachedScene);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._index)
			{
				value = VariantUtils.CreateFrom<int>(ref this._index);
				return true;
			}
			if ((ref name) == NPatchNotesScreen.PropertyName._currentScrollLine)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentScrollLine);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B67 RID: 11111 RVA: 0x00132610 File Offset: 0x00130810
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._screenContents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._marginContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._prevButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._nextButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._patchNotesToggle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._dateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._patchText, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName._cachedScene, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPatchNotesScreen.PropertyName.IsOpen, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPatchNotesScreen.PropertyName._index, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPatchNotesScreen.PropertyName._currentScrollLine, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B68 RID: 11112 RVA: 0x001327F0 File Offset: 0x001309F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isOpen = NPatchNotesScreen.PropertyName.IsOpen;
			bool isOpen2 = this.IsOpen;
			info.AddProperty(isOpen, Variant.From<bool>(ref isOpen2));
			info.AddProperty(NPatchNotesScreen.PropertyName._screenContents, Variant.From<NScrollableContainer>(ref this._screenContents));
			info.AddProperty(NPatchNotesScreen.PropertyName._marginContainer, Variant.From<MarginContainer>(ref this._marginContainer));
			info.AddProperty(NPatchNotesScreen.PropertyName._prevButton, Variant.From<NButton>(ref this._prevButton));
			info.AddProperty(NPatchNotesScreen.PropertyName._nextButton, Variant.From<NButton>(ref this._nextButton));
			info.AddProperty(NPatchNotesScreen.PropertyName._patchNotesToggle, Variant.From<NButton>(ref this._patchNotesToggle));
			info.AddProperty(NPatchNotesScreen.PropertyName._backButton, Variant.From<NButton>(ref this._backButton));
			info.AddProperty(NPatchNotesScreen.PropertyName._dateLabel, Variant.From<MegaLabel>(ref this._dateLabel));
			info.AddProperty(NPatchNotesScreen.PropertyName._patchText, Variant.From<MegaRichTextLabel>(ref this._patchText));
			info.AddProperty(NPatchNotesScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NPatchNotesScreen.PropertyName._cachedScene, Variant.From<PackedScene>(ref this._cachedScene));
			info.AddProperty(NPatchNotesScreen.PropertyName._index, Variant.From<int>(ref this._index));
			info.AddProperty(NPatchNotesScreen.PropertyName._currentScrollLine, Variant.From<int>(ref this._currentScrollLine));
		}

		/// <inheritdoc />
		// Token: 0x06002B69 RID: 11113 RVA: 0x00132928 File Offset: 0x00130B28
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName.IsOpen, ref variant))
			{
				this.IsOpen = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._screenContents, ref variant2))
			{
				this._screenContents = variant2.As<NScrollableContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._marginContainer, ref variant3))
			{
				this._marginContainer = variant3.As<MarginContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._prevButton, ref variant4))
			{
				this._prevButton = variant4.As<NButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._nextButton, ref variant5))
			{
				this._nextButton = variant5.As<NButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._patchNotesToggle, ref variant6))
			{
				this._patchNotesToggle = variant6.As<NButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._backButton, ref variant7))
			{
				this._backButton = variant7.As<NButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._dateLabel, ref variant8))
			{
				this._dateLabel = variant8.As<MegaLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._patchText, ref variant9))
			{
				this._patchText = variant9.As<MegaRichTextLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._tween, ref variant10))
			{
				this._tween = variant10.As<Tween>();
			}
			Variant variant11;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._cachedScene, ref variant11))
			{
				this._cachedScene = variant11.As<PackedScene>();
			}
			Variant variant12;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._index, ref variant12))
			{
				this._index = variant12.As<int>();
			}
			Variant variant13;
			if (info.TryGetProperty(NPatchNotesScreen.PropertyName._currentScrollLine, ref variant13))
			{
				this._currentScrollLine = variant13.As<int>();
			}
		}

		// Token: 0x04001045 RID: 4165
		private NScrollableContainer _screenContents;

		// Token: 0x04001046 RID: 4166
		private MarginContainer _marginContainer;

		// Token: 0x04001047 RID: 4167
		private NButton _prevButton;

		// Token: 0x04001048 RID: 4168
		private NButton _nextButton;

		// Token: 0x04001049 RID: 4169
		private NButton _patchNotesToggle;

		// Token: 0x0400104A RID: 4170
		private NButton _backButton;

		// Token: 0x0400104B RID: 4171
		private MegaLabel _dateLabel;

		// Token: 0x0400104C RID: 4172
		private MegaRichTextLabel _patchText;

		// Token: 0x0400104D RID: 4173
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400104E RID: 4174
		private PackedScene _cachedScene;

		// Token: 0x04001050 RID: 4176
		private const string _engPatchNotesPath = "res://localization/eng/patch_notes";

		// Token: 0x04001051 RID: 4177
		[Nullable(new byte[] { 2, 1 })]
		private List<string> _patchNotePaths;

		// Token: 0x04001052 RID: 4178
		private int _index;

		// Token: 0x04001053 RID: 4179
		private int _currentScrollLine;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001529 RID: 5417
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040050CF RID: 20687
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'CreateNewPatchEntry' method.
			/// </summary>
			// Token: 0x040050D0 RID: 20688
			public static readonly StringName CreateNewPatchEntry = "CreateNewPatchEntry";

			/// <summary>
			/// Cached name for the 'NextPatchNote' method.
			/// </summary>
			// Token: 0x040050D1 RID: 20689
			public static readonly StringName NextPatchNote = "NextPatchNote";

			/// <summary>
			/// Cached name for the 'PreviousPatchNote' method.
			/// </summary>
			// Token: 0x040050D2 RID: 20690
			public static readonly StringName PreviousPatchNote = "PreviousPatchNote";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x040050D3 RID: 20691
			public static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x040050D4 RID: 20692
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'LoadPatchNoteText' method.
			/// </summary>
			// Token: 0x040050D5 RID: 20693
			public static readonly StringName LoadPatchNoteText = "LoadPatchNoteText";

			/// <summary>
			/// Cached name for the 'ReadPatchNoteFile' method.
			/// </summary>
			// Token: 0x040050D6 RID: 20694
			public static readonly StringName ReadPatchNoteFile = "ReadPatchNoteFile";

			/// <summary>
			/// Cached name for the 'UpdateDateLabel' method.
			/// </summary>
			// Token: 0x040050D7 RID: 20695
			public static readonly StringName UpdateDateLabel = "UpdateDateLabel";

			/// <summary>
			/// Cached name for the 'GetFileNameFromPath' method.
			/// </summary>
			// Token: 0x040050D8 RID: 20696
			public static readonly StringName GetFileNameFromPath = "GetFileNameFromPath";

			/// <summary>
			/// Cached name for the 'RemoveFileExtension' method.
			/// </summary>
			// Token: 0x040050D9 RID: 20697
			public static readonly StringName RemoveFileExtension = "RemoveFileExtension";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200152A RID: 5418
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsOpen' property.
			/// </summary>
			// Token: 0x040050DA RID: 20698
			public static readonly StringName IsOpen = "IsOpen";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040050DB RID: 20699
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_screenContents' field.
			/// </summary>
			// Token: 0x040050DC RID: 20700
			public static readonly StringName _screenContents = "_screenContents";

			/// <summary>
			/// Cached name for the '_marginContainer' field.
			/// </summary>
			// Token: 0x040050DD RID: 20701
			public static readonly StringName _marginContainer = "_marginContainer";

			/// <summary>
			/// Cached name for the '_prevButton' field.
			/// </summary>
			// Token: 0x040050DE RID: 20702
			public static readonly StringName _prevButton = "_prevButton";

			/// <summary>
			/// Cached name for the '_nextButton' field.
			/// </summary>
			// Token: 0x040050DF RID: 20703
			public static readonly StringName _nextButton = "_nextButton";

			/// <summary>
			/// Cached name for the '_patchNotesToggle' field.
			/// </summary>
			// Token: 0x040050E0 RID: 20704
			public static readonly StringName _patchNotesToggle = "_patchNotesToggle";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x040050E1 RID: 20705
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_dateLabel' field.
			/// </summary>
			// Token: 0x040050E2 RID: 20706
			public static readonly StringName _dateLabel = "_dateLabel";

			/// <summary>
			/// Cached name for the '_patchText' field.
			/// </summary>
			// Token: 0x040050E3 RID: 20707
			public static readonly StringName _patchText = "_patchText";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040050E4 RID: 20708
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_cachedScene' field.
			/// </summary>
			// Token: 0x040050E5 RID: 20709
			public static readonly StringName _cachedScene = "_cachedScene";

			/// <summary>
			/// Cached name for the '_index' field.
			/// </summary>
			// Token: 0x040050E6 RID: 20710
			public static readonly StringName _index = "_index";

			/// <summary>
			/// Cached name for the '_currentScrollLine' field.
			/// </summary>
			// Token: 0x040050E7 RID: 20711
			public static readonly StringName _currentScrollLine = "_currentScrollLine";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200152B RID: 5419
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
