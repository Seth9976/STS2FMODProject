using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Credits
{
	// Token: 0x0200033A RID: 826
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Credits/NCreditsScreen.cs")]
	public class NCreditsScreen : Control, IScreenContext
	{
		// Token: 0x06002E97 RID: 11927 RVA: 0x0015085E File Offset: 0x0014EA5E
		[NullableContext(2)]
		public static NCreditsScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCreditsScreen._scenePath).Instantiate<NCreditsScreen>(0L);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x00150880 File Offset: 0x0014EA80
		public override void _Ready()
		{
			NHotkeyManager.Instance.PushHotkeyReleasedBinding(MegaInput.back, new Action(this.CloseScreenDebug));
			this._screenContents = base.GetNode<Control>("%ScreenContents");
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._screenContents.Modulate = StsColors.transparentWhite;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._screenContents, "modulate", Colors.White, 2.0);
			this._targetPosition = this._screenContents.Position.Y;
			TaskHelper.RunSafely(this.EnableScreenExit());
			this.InitMegaCrit();
			this.InitComposer();
			this.InitAdditionalProgramming();
			this.InitAdditionalVfx();
			this.InitMarketingSupport();
			this.InitConsultants();
			this.InitVoices();
			this.InitLocalization();
			this.InitTwitchExtension();
			this.InitModdingSupport();
			this.InitPlaytesters();
			this.InitTrailer();
			this.InitFmod();
			this.InitSpine();
			this.InitGodot();
			this.InitExitMessage();
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x001509AC File Offset: 0x0014EBAC
		private void InitMegaCrit()
		{
			base.GetNode<MegaLabel>("%MegaCritHeader").Text = new LocString("credits", "MEGA_CRIT.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%CreatedByNames").Text = new LocString("credits", "MEGA_CRIT.names").GetRawText();
			ValueTuple<string, string> valueTuple = this.SplitTwoColumn(new LocString("credits", "MEGA_CRIT_TEAM.names").GetRawText());
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			base.GetNode<MegaRichTextLabel>("%MegaCritTeamRoles").Text = item;
			base.GetNode<MegaRichTextLabel>("%MegaCritTeamNames").Text = item2;
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x00150A60 File Offset: 0x0014EC60
		private void InitComposer()
		{
			base.GetNode<MegaLabel>("%ComposerHeader").Text = new LocString("credits", "COMPOSER.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%ComposerNames").Text = new LocString("credits", "COMPOSER.names").GetRawText();
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x00150AC0 File Offset: 0x0014ECC0
		private void InitAdditionalProgramming()
		{
			base.GetNode<MegaLabel>("%AdditionalProgrammingHeader").Text = new LocString("credits", "ADDITIONAL_PROGRAMMING.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%AdditionalProgrammingNames").Text = new LocString("credits", "ADDITIONAL_PROGRAMMING.names").GetRawText();
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x00150B20 File Offset: 0x0014ED20
		private void InitAdditionalVfx()
		{
			base.GetNode<MegaLabel>("%AdditionalVfxHeader").Text = new LocString("credits", "ADDITIONAL_VFX.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%AdditionalVfxNames").Text = new LocString("credits", "ADDITIONAL_VFX.names").GetRawText();
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x00150B80 File Offset: 0x0014ED80
		private void InitMarketingSupport()
		{
			base.GetNode<MegaLabel>("%MarketingSupportHeader").Text = new LocString("credits", "MARKETING_SUPPORT.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%MarketingSupportNames").Text = new LocString("credits", "MARKETING_SUPPORT.names").GetRawText();
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x00150BE0 File Offset: 0x0014EDE0
		private void InitConsultants()
		{
			base.GetNode<MegaLabel>("%ConsultantsHeader").Text = new LocString("credits", "CONSULTANTS.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%ConsultantsNames").Text = new LocString("credits", "CONSULTANTS.names").GetRawText();
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x00150C40 File Offset: 0x0014EE40
		private void InitVoices()
		{
			base.GetNode<MegaLabel>("%VoicesHeader").Text = new LocString("credits", "VOICES.header").GetRawText();
			ValueTuple<string, string> valueTuple = NCreditsScreen.SplitTwoColumnMultiRole(new LocString("credits", "VOICES.names").GetRawText());
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			base.GetNode<MegaRichTextLabel>("%VoicesRoles").Text = item;
			base.GetNode<MegaRichTextLabel>("%VoicesNames").Text = item2;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x00150CC8 File Offset: 0x0014EEC8
		private void InitLocalization()
		{
			base.GetNode<MegaLabel>("%LocalizationHeader").Text = new LocString("credits", "LOC.header").GetRawText();
			base.GetNode<MegaLabel>("%ptbHeader").Text = new LocString("credits", "LOC_PTB.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%ptbNames").Text = new LocString("credits", "LOC_PTB.names").GetRawText();
			base.GetNode<MegaLabel>("%zhsHeader").Text = new LocString("credits", "LOC_ZHS.header").GetRawText();
			ValueTuple<string, string> valueTuple = this.SplitTwoColumn(new LocString("credits", "LOC_ZHS.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%zhsRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%zhsNames").Text = valueTuple.Item2;
			base.GetNode<MegaLabel>("%fraHeader").Text = new LocString("credits", "LOC_FRA.header").GetRawText();
			valueTuple = this.SplitTwoColumn(new LocString("credits", "LOC_FRA.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%fraRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%fraNames").Text = valueTuple.Item2;
			base.GetNode<MegaLabel>("%deuHeader").Text = new LocString("credits", "LOC_DEU.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%deuNames").Text = new LocString("credits", "LOC_DEU.names").GetRawText();
			base.GetNode<MegaLabel>("%itaHeader").Text = new LocString("credits", "LOC_ITA.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%itaTeam").Text = new LocString("credits", "LOC_ITA.team").GetRawText();
			valueTuple = this.SplitTwoColumn(new LocString("credits", "LOC_ITA.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%itaRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%itaNames").Text = valueTuple.Item2;
			base.GetNode<MegaLabel>("%jpnHeader").Text = new LocString("credits", "LOC_JPN.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%jpnNames").Text = new LocString("credits", "LOC_JPN.names").GetRawText();
			base.GetNode<MegaLabel>("%korHeader").Text = new LocString("credits", "LOC_KOR.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%korNames").Text = new LocString("credits", "LOC_KOR.names").GetRawText();
			base.GetNode<MegaLabel>("%polHeader").Text = new LocString("credits", "LOC_POL.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%polNames").Text = new LocString("credits", "LOC_POL.names").GetRawText();
			base.GetNode<MegaLabel>("%rusHeader").Text = new LocString("credits", "LOC_RUS.header").GetRawText();
			valueTuple = this.SplitTwoColumn(new LocString("credits", "LOC_RUS.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%rusRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%rusNames").Text = valueTuple.Item2;
			base.GetNode<MegaLabel>("%spaHeader").Text = new LocString("credits", "LOC_SPA.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%spaNames").Text = new LocString("credits", "LOC_SPA.names").GetRawText();
			base.GetNode<MegaLabel>("%espHeader").Text = new LocString("credits", "LOC_ESP.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%espTeam").Text = new LocString("credits", "LOC_ESP.team").GetRawText();
			valueTuple = this.SplitTwoColumn(new LocString("credits", "LOC_ESP.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%espRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%espNames").Text = valueTuple.Item2;
			base.GetNode<MegaLabel>("%thaHeader").Text = new LocString("credits", "LOC_THA.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%thaNames").Text = new LocString("credits", "LOC_THA.names").GetRawText();
			base.GetNode<MegaLabel>("%turHeader").Text = new LocString("credits", "LOC_TUR.header").GetRawText();
			valueTuple = this.SplitTwoColumn(new LocString("credits", "LOC_TUR.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%turRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%turNames").Text = valueTuple.Item2;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x0015126C File Offset: 0x0014F46C
		private void InitTwitchExtension()
		{
			base.GetNode<MegaLabel>("%TwitchHeader").Text = new LocString("credits", "TWITCH.header").GetRawText();
			ValueTuple<string, string> valueTuple = this.SplitTwoColumn(new LocString("credits", "TWITCH.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%TwitchRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%TwitchNames").Text = valueTuple.Item2;
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x001512F4 File Offset: 0x0014F4F4
		private void InitModdingSupport()
		{
			base.GetNode<MegaLabel>("%ModdingSupportHeader").Text = new LocString("credits", "MODDING_SUPPORT.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%ModdingSupportNames").Text = NCreditsScreen.ShuffleOneColumn(new LocString("credits", "MODDING_SUPPORT.names").GetRawText());
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x00151358 File Offset: 0x0014F558
		private void InitPlaytesters()
		{
			base.GetNode<MegaLabel>("%PlaytestersHeader").Text = new LocString("credits", "PLAYTESTERS.header").GetRawText();
			ValueTuple<string, string, string> valueTuple = this.SplitThreeColumnPlaytesters(new LocString("credits", "PLAYTESTERS.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%PlaytesterNames1").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%PlaytesterNames2").Text = valueTuple.Item2;
			base.GetNode<MegaRichTextLabel>("%PlaytesterNames3").Text = valueTuple.Item3;
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x001513FC File Offset: 0x0014F5FC
		private void InitTrailer()
		{
			base.GetNode<MegaLabel>("%TrailerHeader").Text = new LocString("credits", "TRAILER.header").GetRawText();
			base.GetNode<MegaRichTextLabel>("%TrailerAnimationTeam").Text = new LocString("credits", "TRAILER_ANIMATION.team").GetRawText();
			base.GetNode<MegaLabel>("%TrailerAnimationHeader").Text = new LocString("credits", "TRAILER_ANIMATION.header").GetRawText();
			ValueTuple<string, string> valueTuple = this.SplitTwoColumn(new LocString("credits", "TRAILER_ANIMATION.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%TrailerAnimationRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%TrailerAnimationNames").Text = valueTuple.Item2;
			base.GetNode<MegaLabel>("%TrailerEditorHeader").Text = new LocString("credits", "TRAILER_EDITOR.header").GetRawText();
			valueTuple = this.SplitTwoColumn(new LocString("credits", "TRAILER_EDITOR.names").GetRawText());
			base.GetNode<MegaRichTextLabel>("%TrailerEditorRoles").Text = valueTuple.Item1;
			base.GetNode<MegaRichTextLabel>("%TrailerEditorNames").Text = valueTuple.Item2;
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x0015154F File Offset: 0x0014F74F
		private void InitFmod()
		{
			base.GetNode<MegaLabel>("%FmodHeader").Text = new LocString("credits", "FMOD").GetRawText();
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x0015157A File Offset: 0x0014F77A
		private void InitSpine()
		{
			base.GetNode<MegaLabel>("%SpineHeader").Text = new LocString("credits", "SPINE").GetRawText();
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x001515A5 File Offset: 0x0014F7A5
		private void InitGodot()
		{
			base.GetNode<MegaLabel>("%GodotHeader").Text = new LocString("credits", "GODOT").GetRawText();
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x001515D0 File Offset: 0x0014F7D0
		private void InitExitMessage()
		{
			base.GetNode<MegaRichTextLabel>("%ExitMessage").Text = new LocString("credits", "EXIT_MESSAGE").GetRawText();
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x001515FB File Offset: 0x0014F7FB
		public override void _EnterTree()
		{
			NHotkeyManager.Instance.AddBlockingScreen(this);
			NHotkeyManager.Instance.PushHotkeyReleasedBinding(MegaInput.cancel, new Action(this.CloseScreenDebug));
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x00151628 File Offset: 0x0014F828
		public override void _ExitTree()
		{
			NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(MegaInput.cancel, new Action(this.CloseScreenDebug));
			NHotkeyManager.Instance.RemoveBlockingScreen(this);
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002EAB RID: 11947 RVA: 0x00151655 File Offset: 0x0014F855
		public Control DefaultFocusedControl
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// The screen must be up for 2 seconds before you can exit.
		/// Otherwise, you can open it and close it before seeing it (confusion).
		/// </summary>
		// Token: 0x06002EAC RID: 11948 RVA: 0x00151658 File Offset: 0x0014F858
		private async Task EnableScreenExit()
		{
			await Task.Delay(2000);
			this._canClose = true;
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x0015169B File Offset: 0x0014F89B
		private void CloseScreenDebug()
		{
			if (!this._canClose)
			{
				return;
			}
			if (this._exitingScreen)
			{
				return;
			}
			this._exitingScreen = true;
			TaskHelper.RunSafely(this.FadeAndExitScreen());
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x001516C4 File Offset: 0x0014F8C4
		private async Task FadeAndExitScreen()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._screenContents, "modulate:a", 0f, 1.0);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				NModalContainer.Instance.Clear();
			}
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x00151708 File Offset: 0x0014F908
		public override void _GuiInput(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L && inputEventMouseButton.Pressed)
			{
				this.CloseScreenDebug();
			}
			this.ProcessScrollEvent(inputEvent);
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x00151740 File Offset: 0x0014F940
		public override void _Process(double delta)
		{
			float num = (float)delta;
			this._targetPosition -= 80f * num;
			this._screenContents.Position = this._screenContents.Position.Lerp(new Vector2(this._screenContents.Position.X, this._targetPosition), num * 20f);
		}

		/// <summary>
		/// Processes mouse wheel to zoom in and out of the Timeline screen.
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06002EB1 RID: 11953 RVA: 0x001517A4 File Offset: 0x0014F9A4
		private void ProcessScrollEvent(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				if (inputEventMouseButton.ButtonIndex == 4L)
				{
					this._targetPosition += 50f;
					return;
				}
				if (inputEventMouseButton.ButtonIndex == 5L)
				{
					this._targetPosition -= 50f;
					return;
				}
			}
			else
			{
				InputEventPanGesture inputEventPanGesture = inputEvent as InputEventPanGesture;
				if (inputEventPanGesture != null)
				{
					this._targetPosition += -inputEventPanGesture.Delta.Y * 20f;
				}
			}
		}

		/// <summary>
		/// When you want to randomize the order for a section with just names.
		/// Used for Modding Support section.
		/// </summary>
		// Token: 0x06002EB2 RID: 11954 RVA: 0x00151820 File Offset: 0x0014FA20
		private static string ShuffleOneColumn(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return string.Empty;
			}
			List<string> list = input.Split(new string[] { "||" }, StringSplitOptions.None).ToList<string>();
			for (int i = list.Count - 1; i > 0; i--)
			{
				int num = Rng.Chaotic.NextInt(i + 1);
				List<string> list2 = list;
				int num2 = i;
				List<string> list3 = list;
				int num3 = num;
				string text = list[num];
				string text2 = list[i];
				list2[num2] = text;
				list3[num3] = text2;
			}
			return string.Join("\n", list);
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x001518B0 File Offset: 0x0014FAB0
		[return: TupleElementNames(new string[] { "Roles", "Names" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		private ValueTuple<string, string> SplitTwoColumn(string input)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string[] array = input.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] array3 = (from p in text.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
					select p.Trim() into p
					where !string.IsNullOrWhiteSpace(p)
					select p).ToArray<string>();
				if (array3.Length == 2)
				{
					list.Add(array3[0]);
					list2.Add(array3[1]);
				}
			}
			return new ValueTuple<string, string>(string.Join("\n", list), string.Join("\n", list2));
		}

		/// <summary>
		/// Takes all the playtester credits, shuffles it
		/// </summary>
		// Token: 0x06002EB4 RID: 11956 RVA: 0x00151998 File Offset: 0x0014FB98
		[return: TupleElementNames(new string[] { "Column1", "Column2", "Column3" })]
		[return: Nullable(new byte[] { 0, 1, 1, 1 })]
		private ValueTuple<string, string, string> SplitThreeColumnPlaytesters(string input)
		{
			string[] array = (from p in input.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
				select p.Trim() into p
				where !string.IsNullOrWhiteSpace(p)
				select p).ToArray<string>();
			for (int i = array.Length - 1; i > 0; i--)
			{
				int num = Rng.Chaotic.NextInt(i + 1);
				ref string ptr = ref array[i];
				string[] array2 = array;
				int num2 = num;
				string text = array[num];
				string text2 = array[i];
				ptr = text;
				array2[num2] = text2;
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			for (int j = 0; j < array.Length; j++)
			{
				switch (j % 3)
				{
				case 0:
					list.Add(array[j]);
					break;
				case 1:
					list2.Add(array[j]);
					break;
				case 2:
					list3.Add(array[j]);
					break;
				}
			}
			return new ValueTuple<string, string, string>(string.Join("\n", list), string.Join("\n", list2), string.Join("\n", list3));
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x00151ADC File Offset: 0x0014FCDC
		[return: TupleElementNames(new string[] { "left", "right" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		private static ValueTuple<string, string> SplitTwoColumnMultiRole(string input)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string[] array = input.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new string[] { "||" }, StringSplitOptions.None);
				if (array2.Length == 2)
				{
					string text = array2[0].Trim();
					string text2 = array2[1].Trim();
					List<string> list3 = (from r in text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
						select r.Trim()).ToList<string>();
					for (int j = 0; j < list3.Count; j++)
					{
						list.Add(list3[j]);
						list2.Add((j == 0) ? text2 : "");
					}
					bool flag = list3.Count > 1;
					bool flag2 = i == array.Length - 1;
					if (flag && !flag2)
					{
						list.Add("");
						list2.Add("");
					}
				}
			}
			return new ValueTuple<string, string>(string.Join("\n", list), string.Join("\n", list2));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002EB6 RID: 11958 RVA: 0x00151C1C File Offset: 0x0014FE1C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(25)
			{
				new MethodInfo(NCreditsScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCreditsScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitMegaCrit, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitComposer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitAdditionalProgramming, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitAdditionalVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitMarketingSupport, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitConsultants, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitVoices, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitLocalization, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitTwitchExtension, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitModdingSupport, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitPlaytesters, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitTrailer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitFmod, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitSpine, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitGodot, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.InitExitMessage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName.CloseScreenDebug, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreditsScreen.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCreditsScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreditsScreen.MethodName.ProcessScrollEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCreditsScreen.MethodName.ShuffleOneColumn, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "input", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002EB7 RID: 11959 RVA: 0x00152180 File Offset: 0x00150380
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCreditsScreen.MethodName.Create && args.Count == 0)
			{
				NCreditsScreen ncreditsScreen = NCreditsScreen.Create();
				ret = VariantUtils.CreateFrom<NCreditsScreen>(ref ncreditsScreen);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitMegaCrit && args.Count == 0)
			{
				this.InitMegaCrit();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitComposer && args.Count == 0)
			{
				this.InitComposer();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitAdditionalProgramming && args.Count == 0)
			{
				this.InitAdditionalProgramming();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitAdditionalVfx && args.Count == 0)
			{
				this.InitAdditionalVfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitMarketingSupport && args.Count == 0)
			{
				this.InitMarketingSupport();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitConsultants && args.Count == 0)
			{
				this.InitConsultants();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitVoices && args.Count == 0)
			{
				this.InitVoices();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitLocalization && args.Count == 0)
			{
				this.InitLocalization();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitTwitchExtension && args.Count == 0)
			{
				this.InitTwitchExtension();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitModdingSupport && args.Count == 0)
			{
				this.InitModdingSupport();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitPlaytesters && args.Count == 0)
			{
				this.InitPlaytesters();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitTrailer && args.Count == 0)
			{
				this.InitTrailer();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitFmod && args.Count == 0)
			{
				this.InitFmod();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitSpine && args.Count == 0)
			{
				this.InitSpine();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitGodot && args.Count == 0)
			{
				this.InitGodot();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.InitExitMessage && args.Count == 0)
			{
				this.InitExitMessage();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.CloseScreenDebug && args.Count == 0)
			{
				this.CloseScreenDebug();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.ProcessScrollEvent && args.Count == 1)
			{
				this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.ShuffleOneColumn && args.Count == 1)
			{
				string text = NCreditsScreen.ShuffleOneColumn(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x00152578 File Offset: 0x00150778
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCreditsScreen.MethodName.Create && args.Count == 0)
			{
				NCreditsScreen ncreditsScreen = NCreditsScreen.Create();
				ret = VariantUtils.CreateFrom<NCreditsScreen>(ref ncreditsScreen);
				return true;
			}
			if ((ref method) == NCreditsScreen.MethodName.ShuffleOneColumn && args.Count == 1)
			{
				string text = NCreditsScreen.ShuffleOneColumn(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002EB9 RID: 11961 RVA: 0x001525F4 File Offset: 0x001507F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCreditsScreen.MethodName.Create || (ref method) == NCreditsScreen.MethodName._Ready || (ref method) == NCreditsScreen.MethodName.InitMegaCrit || (ref method) == NCreditsScreen.MethodName.InitComposer || (ref method) == NCreditsScreen.MethodName.InitAdditionalProgramming || (ref method) == NCreditsScreen.MethodName.InitAdditionalVfx || (ref method) == NCreditsScreen.MethodName.InitMarketingSupport || (ref method) == NCreditsScreen.MethodName.InitConsultants || (ref method) == NCreditsScreen.MethodName.InitVoices || (ref method) == NCreditsScreen.MethodName.InitLocalization || (ref method) == NCreditsScreen.MethodName.InitTwitchExtension || (ref method) == NCreditsScreen.MethodName.InitModdingSupport || (ref method) == NCreditsScreen.MethodName.InitPlaytesters || (ref method) == NCreditsScreen.MethodName.InitTrailer || (ref method) == NCreditsScreen.MethodName.InitFmod || (ref method) == NCreditsScreen.MethodName.InitSpine || (ref method) == NCreditsScreen.MethodName.InitGodot || (ref method) == NCreditsScreen.MethodName.InitExitMessage || (ref method) == NCreditsScreen.MethodName._EnterTree || (ref method) == NCreditsScreen.MethodName._ExitTree || (ref method) == NCreditsScreen.MethodName.CloseScreenDebug || (ref method) == NCreditsScreen.MethodName._GuiInput || (ref method) == NCreditsScreen.MethodName._Process || (ref method) == NCreditsScreen.MethodName.ProcessScrollEvent || (ref method) == NCreditsScreen.MethodName.ShuffleOneColumn || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002EBA RID: 11962 RVA: 0x00152780 File Offset: 0x00150980
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCreditsScreen.PropertyName._canClose)
			{
				this._canClose = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._exitingScreen)
			{
				this._exitingScreen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._screenContents)
			{
				this._screenContents = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._targetPosition)
			{
				this._targetPosition = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002EBB RID: 11963 RVA: 0x00152838 File Offset: 0x00150A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCreditsScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._canClose)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._canClose);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._exitingScreen)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._exitingScreen);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._screenContents)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._screenContents);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NCreditsScreen.PropertyName._targetPosition)
			{
				value = VariantUtils.CreateFrom<float>(ref this._targetPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002EBC RID: 11964 RVA: 0x00152930 File Offset: 0x00150B30
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCreditsScreen.PropertyName._canClose, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreditsScreen.PropertyName._exitingScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreditsScreen.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreditsScreen.PropertyName._screenContents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreditsScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCreditsScreen.PropertyName._targetPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreditsScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002EBD RID: 11965 RVA: 0x00152A28 File Offset: 0x00150C28
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCreditsScreen.PropertyName._canClose, Variant.From<bool>(ref this._canClose));
			info.AddProperty(NCreditsScreen.PropertyName._exitingScreen, Variant.From<bool>(ref this._exitingScreen));
			info.AddProperty(NCreditsScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCreditsScreen.PropertyName._screenContents, Variant.From<Control>(ref this._screenContents));
			info.AddProperty(NCreditsScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NCreditsScreen.PropertyName._targetPosition, Variant.From<float>(ref this._targetPosition));
		}

		/// <inheritdoc />
		// Token: 0x06002EBE RID: 11966 RVA: 0x00152AC0 File Offset: 0x00150CC0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCreditsScreen.PropertyName._canClose, ref variant))
			{
				this._canClose = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCreditsScreen.PropertyName._exitingScreen, ref variant2))
			{
				this._exitingScreen = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCreditsScreen.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCreditsScreen.PropertyName._screenContents, ref variant4))
			{
				this._screenContents = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCreditsScreen.PropertyName._backButton, ref variant5))
			{
				this._backButton = variant5.As<NBackButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCreditsScreen.PropertyName._targetPosition, ref variant6))
			{
				this._targetPosition = variant6.As<float>();
			}
		}

		// Token: 0x040011CB RID: 4555
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/credits_screen");

		// Token: 0x040011CC RID: 4556
		private bool _canClose;

		// Token: 0x040011CD RID: 4557
		private bool _exitingScreen;

		// Token: 0x040011CE RID: 4558
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040011CF RID: 4559
		private Control _screenContents;

		// Token: 0x040011D0 RID: 4560
		private NBackButton _backButton;

		// Token: 0x040011D1 RID: 4561
		private const string _table = "credits";

		// Token: 0x040011D2 RID: 4562
		private float _targetPosition;

		// Token: 0x040011D3 RID: 4563
		private const float _scrollSpeed = 50f;

		// Token: 0x040011D4 RID: 4564
		private const float _trackpadScrollSpeed = 20f;

		// Token: 0x040011D5 RID: 4565
		private const float _autoScrollSpeed = 80f;

		// Token: 0x040011D6 RID: 4566
		private const float _lerpSmoothness = 20f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015CC RID: 5580
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040053F0 RID: 21488
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040053F1 RID: 21489
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'InitMegaCrit' method.
			/// </summary>
			// Token: 0x040053F2 RID: 21490
			public static readonly StringName InitMegaCrit = "InitMegaCrit";

			/// <summary>
			/// Cached name for the 'InitComposer' method.
			/// </summary>
			// Token: 0x040053F3 RID: 21491
			public static readonly StringName InitComposer = "InitComposer";

			/// <summary>
			/// Cached name for the 'InitAdditionalProgramming' method.
			/// </summary>
			// Token: 0x040053F4 RID: 21492
			public static readonly StringName InitAdditionalProgramming = "InitAdditionalProgramming";

			/// <summary>
			/// Cached name for the 'InitAdditionalVfx' method.
			/// </summary>
			// Token: 0x040053F5 RID: 21493
			public static readonly StringName InitAdditionalVfx = "InitAdditionalVfx";

			/// <summary>
			/// Cached name for the 'InitMarketingSupport' method.
			/// </summary>
			// Token: 0x040053F6 RID: 21494
			public static readonly StringName InitMarketingSupport = "InitMarketingSupport";

			/// <summary>
			/// Cached name for the 'InitConsultants' method.
			/// </summary>
			// Token: 0x040053F7 RID: 21495
			public static readonly StringName InitConsultants = "InitConsultants";

			/// <summary>
			/// Cached name for the 'InitVoices' method.
			/// </summary>
			// Token: 0x040053F8 RID: 21496
			public static readonly StringName InitVoices = "InitVoices";

			/// <summary>
			/// Cached name for the 'InitLocalization' method.
			/// </summary>
			// Token: 0x040053F9 RID: 21497
			public static readonly StringName InitLocalization = "InitLocalization";

			/// <summary>
			/// Cached name for the 'InitTwitchExtension' method.
			/// </summary>
			// Token: 0x040053FA RID: 21498
			public static readonly StringName InitTwitchExtension = "InitTwitchExtension";

			/// <summary>
			/// Cached name for the 'InitModdingSupport' method.
			/// </summary>
			// Token: 0x040053FB RID: 21499
			public static readonly StringName InitModdingSupport = "InitModdingSupport";

			/// <summary>
			/// Cached name for the 'InitPlaytesters' method.
			/// </summary>
			// Token: 0x040053FC RID: 21500
			public static readonly StringName InitPlaytesters = "InitPlaytesters";

			/// <summary>
			/// Cached name for the 'InitTrailer' method.
			/// </summary>
			// Token: 0x040053FD RID: 21501
			public static readonly StringName InitTrailer = "InitTrailer";

			/// <summary>
			/// Cached name for the 'InitFmod' method.
			/// </summary>
			// Token: 0x040053FE RID: 21502
			public static readonly StringName InitFmod = "InitFmod";

			/// <summary>
			/// Cached name for the 'InitSpine' method.
			/// </summary>
			// Token: 0x040053FF RID: 21503
			public static readonly StringName InitSpine = "InitSpine";

			/// <summary>
			/// Cached name for the 'InitGodot' method.
			/// </summary>
			// Token: 0x04005400 RID: 21504
			public static readonly StringName InitGodot = "InitGodot";

			/// <summary>
			/// Cached name for the 'InitExitMessage' method.
			/// </summary>
			// Token: 0x04005401 RID: 21505
			public static readonly StringName InitExitMessage = "InitExitMessage";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005402 RID: 21506
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005403 RID: 21507
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'CloseScreenDebug' method.
			/// </summary>
			// Token: 0x04005404 RID: 21508
			public static readonly StringName CloseScreenDebug = "CloseScreenDebug";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005405 RID: 21509
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005406 RID: 21510
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'ProcessScrollEvent' method.
			/// </summary>
			// Token: 0x04005407 RID: 21511
			public static readonly StringName ProcessScrollEvent = "ProcessScrollEvent";

			/// <summary>
			/// Cached name for the 'ShuffleOneColumn' method.
			/// </summary>
			// Token: 0x04005408 RID: 21512
			public static readonly StringName ShuffleOneColumn = "ShuffleOneColumn";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015CD RID: 5581
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005409 RID: 21513
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_canClose' field.
			/// </summary>
			// Token: 0x0400540A RID: 21514
			public static readonly StringName _canClose = "_canClose";

			/// <summary>
			/// Cached name for the '_exitingScreen' field.
			/// </summary>
			// Token: 0x0400540B RID: 21515
			public static readonly StringName _exitingScreen = "_exitingScreen";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400540C RID: 21516
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_screenContents' field.
			/// </summary>
			// Token: 0x0400540D RID: 21517
			public static readonly StringName _screenContents = "_screenContents";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x0400540E RID: 21518
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_targetPosition' field.
			/// </summary>
			// Token: 0x0400540F RID: 21519
			public static readonly StringName _targetPosition = "_targetPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015CE RID: 5582
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
