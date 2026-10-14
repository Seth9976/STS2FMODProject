using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	/// <summary>
	/// The Timeline screen allows players to reveal Epochs they've obtained to see lore and unlock content.
	/// When accessed:
	///  1. Load progress data
	///  2. Set up the Epoch slots
	///  3. Control screen state logic for the SingleEpochViewScreen and UnlockScreen
	///  4. Play any bespoke animations for Timeline expansion, etc
	/// </summary>
	// Token: 0x02000265 RID: 613
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NTimelineScreen.cs")]
	public class NTimelineScreen : NSubmenu
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x000D8904 File Offset: 0x000D6B04
		public static string[] AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(NTimelineScreen._scenePath);
				list.Add("res://scenes/timeline_screen/place_epoch_sparks.tscn");
				list.Add(NEpochHighlightVfx.scenePath);
				list.Add(NEpochOffscreenVfx.scenePath);
				list.Add(NEpochInspectScreen.lockedImagePath);
				list.AddRange(NUnlockTimelineScreen.AssetPaths);
				list.AddRange(NUnlockPotionsScreen.AssetPaths);
				list.AddRange(NUnlockRelicsScreen.AssetPaths);
				list.AddRange(NUnlockCardsScreen.AssetPaths);
				list.AddRange(NUnlockMiscScreen.AssetPaths);
				list.AddRange(NUnlockCharacterScreen.AssetPaths);
				list.AddRange(NEraColumn.assetPaths);
				list.AddRange(NTimelineScreen.GetAllEraTexturePaths());
				return list.ToArray();
			}
		}

		/// <summary>
		/// This is a convenience reference for static access and avoiding re-initialization and NOT a Singleton.
		/// </summary>
		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x000D89AA File Offset: 0x000D6BAA
		public static NTimelineScreen Instance
		{
			get
			{
				return NGame.Instance.MainMenu.SubmenuStack.GetSubmenuType<NTimelineScreen>();
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x000D89C0 File Offset: 0x000D6BC0
		// (set) Token: 0x06002015 RID: 8213 RVA: 0x000D89C8 File Offset: 0x000D6BC8
		[Nullable(2)]
		public NUnlockScreen CurrentUnlockScreen
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000D89D1 File Offset: 0x000D6BD1
		[NullableContext(2)]
		public static NTimelineScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NTimelineScreen._scenePath).Instantiate<NTimelineScreen>(0L);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000D89F4 File Offset: 0x000D6BF4
		public override void OnSubmenuOpened()
		{
			this.ResetScreen();
			this.DisableInput();
			SerializableEpoch serializableEpoch = SaveManager.Instance.Progress.Epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == EpochModel.GetId<NeowEpoch>());
			bool flag = serializableEpoch == null;
			bool flag2 = flag;
			if (!flag2)
			{
				EpochState state = serializableEpoch.State;
				bool flag3 = state - EpochState.NoSlot <= 1;
				flag2 = flag3;
			}
			if (flag2)
			{
				SaveManager.Instance.Progress.ObtainEpoch(EpochModel.GetId<NeowEpoch>());
			}
			if (SaveManager.Instance.IsNeowDiscovered())
			{
				TaskHelper.RunSafely(this.FirstTimeLogic());
			}
			else
			{
				SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_open", 1f);
				TaskHelper.RunSafely(this.InitScreen());
			}
			this.SetScreenDraggability();
			AchievementsHelper.CheckTimelineComplete();
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000D8ABA File Offset: 0x000D6CBA
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			this.ResetScreen();
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x000D8AC8 File Offset: 0x000D6CC8
		protected override void OnSubmenuShown()
		{
			base.ProcessMode = 0L;
			this.RefreshBackButton();
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x000D8AD8 File Offset: 0x000D6CD8
		protected override void OnSubmenuHidden()
		{
			base.ProcessMode = 4L;
			NMainMenu mainMenu = NGame.Instance.MainMenu;
			if (mainMenu == null)
			{
				return;
			}
			mainMenu.RefreshButtons();
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000D8AF8 File Offset: 0x000D6CF8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._epochSlotContainer = base.GetNode<HBoxContainer>("%EpochSlots");
			this._reminderText = base.GetNode<NEpochReminderText>("%EpochReminderText");
			this._reminderVfxHolder = base.GetNode<Control>("%ReminderVfxHolder");
			this._inputBlocker = base.GetNode<Control>("%InputBlocker");
			this._backstop = base.GetNode<ColorRect>("%SharedBackstop");
			this._inspectScreen = base.GetNode<NEpochInspectScreen>("%EpochInspectScreen");
			this._line = base.GetNode<Control>("%Line");
			this._lineContainer = base.GetNode<Control>("%LineContainer");
			this._slotsContainer = base.GetNode<NSlotsContainer>("%SlotsContainer");
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._unlockScreenHolder = base.GetNode<Control>("%UnlockScreenHolder");
			GodotObject backButton = this._backButton;
			StringName released = NClickableControl.SignalName.Released;
			Action<NButton> action;
			if ((action = NTimelineScreen.<>O.<0>__OnBackButtonPressed) == null)
			{
				action = (NTimelineScreen.<>O.<0>__OnBackButtonPressed = new Action<NButton>(NTimelineScreen.OnBackButtonPressed));
			}
			backButton.Connect(released, Callable.From<NButton>(action), 0U);
			this._save = SaveManager.Instance.Progress;
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._slotsContainer, "modulate:a", 1f, 1.0);
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x000D8C70 File Offset: 0x000D6E70
		private static void OnBackButtonPressed(NButton obj)
		{
			SfxCmd.Play("event:/sfx/ui/map/map_close", 1f);
		}

		/// <summary>
		/// Only called the first time you enter the Timeline (hence the name).
		/// </summary>
		// Token: 0x0600201D RID: 8221 RVA: 0x000D8C84 File Offset: 0x000D6E84
		private async Task FirstTimeLogic()
		{
			await this.AwaitProcessFrame(default(CancellationToken));
			this._tutorial = SceneHelper.Instantiate<NTimelineTutorial>("timeline_screen/timeline_tutorial");
			this.AddChildSafely(this._tutorial);
			this._tutorial.Init(this);
		}

		/// <summary>
		/// Called from NTimelineTutorial once the player completes it.
		/// </summary>
		// Token: 0x0600201E RID: 8222 RVA: 0x000D8CC8 File Offset: 0x000D6EC8
		public async Task SpawnFirstTimeTimeline()
		{
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_open", 1f);
			Log.Info("Running first time logic", 2);
			List<EpochSlotData> list = new List<EpochSlotData>(1)
			{
				new EpochSlotData(EpochModel.GetId<NeowEpoch>(), EpochSlotState.Obtained)
			};
			await this.AddEpochSlots(list, true);
			SaveManager.Instance.UnlockSlot(EpochModel.GetId<NeowEpoch>());
			this.EnableInput();
		}

		/// <summary>
		/// Called every time this screen is opened.
		/// The Timeline is recreated from the save file each time.
		/// NOTE: If you're opening the screen for the first time,
		/// FirstTimeLogic() is called instead.
		/// </summary>
		// Token: 0x0600201F RID: 8223 RVA: 0x000D8D0C File Offset: 0x000D6F0C
		private async Task InitScreen()
		{
			Log.Info("Initializing Timeline:", 2);
			List<EpochSlotData> list = new List<EpochSlotData>();
			Tween lineGrowTween = this._lineGrowTween;
			if (lineGrowTween != null)
			{
				lineGrowTween.Kill();
			}
			this._lineGrowTween = base.CreateTween();
			this._lineGrowTween.TweenProperty(this._lineContainer, "modulate:a", 1f, 0.5);
			foreach (SerializableEpoch serializableEpoch in this._save.Epochs)
			{
				if (serializableEpoch.State != EpochState.ObtainedNoSlot)
				{
					list.Add(new EpochSlotData(serializableEpoch.Id, EpochSlotState.NotObtained));
				}
			}
			list = list.OrderBy<EpochSlotData, int>((EpochSlotData a) => a.EraPosition).ToList<EpochSlotData>();
			await this.AddEpochSlots(list, false);
			int num = 0;
			foreach (SerializableEpoch serializableEpoch2 in this._save.Epochs)
			{
				EpochModel epochModel = EpochModel.Get(serializableEpoch2.Id);
				if (serializableEpoch2.State > EpochState.ObtainedNoSlot)
				{
					foreach (Node node in this._uniqueEpochEras[epochModel.Era].GetChildren(false))
					{
						NEpochSlot nepochSlot = node as NEpochSlot;
						if (nepochSlot != null && nepochSlot.eraPosition == epochModel.EraPosition)
						{
							num++;
							nepochSlot.SetState((serializableEpoch2.State >= EpochState.Revealed) ? EpochSlotState.Complete : EpochSlotState.Obtained);
							TaskHelper.RunSafely(node.GetParent<NEraColumn>().SpawnNameAndYear());
						}
					}
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" Epochs are complete");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			TaskHelper.RunSafely(this.NavigateToRevealableSlot());
		}

		/// <summary>
		/// When the Timeline is opened, if there is an epoch to unlock, we lerp to it.
		/// </summary>
		// Token: 0x06002020 RID: 8224 RVA: 0x000D8D50 File Offset: 0x000D6F50
		private async Task NavigateToRevealableSlot()
		{
			if (SaveManager.Instance.GetDiscoveredEpochCount() == 0)
			{
				this.EnableInput();
			}
			else
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				float getInitX = this._slotsContainer.GetInitX;
				float num = 0f;
				float num2 = float.MaxValue;
				foreach (NEraColumn neraColumn in this._uniqueEpochEras.Values)
				{
					foreach (NEpochSlot nepochSlot in neraColumn.GetChildrenRecursive<NEpochSlot>())
					{
						if (nepochSlot.State == EpochSlotState.Obtained)
						{
							float num3 = Math.Abs(getInitX - nepochSlot.GlobalPosition.X);
							if (num3 < num2)
							{
								num = nepochSlot.GlobalPosition.X;
								num2 = num3;
							}
						}
					}
				}
				await TaskHelper.RunSafely(this._slotsContainer.LerpToSlot(num));
				this.EnableInput();
			}
		}

		/// <summary>
		/// Creates Epoch slots and the Era columns required to hold them.
		/// This is called both when opening the screen and for Timeline expansion events (isAnimated).
		/// </summary>
		// Token: 0x06002021 RID: 8225 RVA: 0x000D8D94 File Offset: 0x000D6F94
		public async Task AddEpochSlots(List<EpochSlotData> slotsToAdd, bool isAnimated)
		{
			List<NEraColumn> list = new List<NEraColumn>();
			if (isAnimated)
			{
				foreach (NEraColumn neraColumn in this._uniqueEpochEras.Values)
				{
					TaskHelper.RunSafely(neraColumn.SaveBeforeAnimationPosition());
				}
			}
			foreach (EpochSlotData epochSlotData in slotsToAdd)
			{
				NEraColumn neraColumn2;
				if (!this._uniqueEpochEras.TryGetValue(epochSlotData.Era, out neraColumn2))
				{
					NEraColumn neraColumn3 = NEraColumn.Create(epochSlotData);
					this._epochSlotContainer.AddChildSafely(neraColumn3);
					int num = 0;
					foreach (Node node in this._epochSlotContainer.GetChildren(false))
					{
						NEraColumn neraColumn4 = node as NEraColumn;
						if (neraColumn4 != null && neraColumn3.era > neraColumn4.era)
						{
							num = neraColumn4.GetIndex(false) + 1;
						}
					}
					this._epochSlotContainer.MoveChildSafely(neraColumn3, num);
					list.Add(neraColumn3);
					this._uniqueEpochEras.Add(epochSlotData.Era, neraColumn3);
				}
				else
				{
					neraColumn2.AddSlot(epochSlotData);
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral(" Created ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(slotsToAdd.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Epoch slots");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral(" Created ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Era columns");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (isAnimated)
			{
				List<Vector2> list2 = this.PredictHBoxLayout(this._epochSlotContainer);
				foreach (NEraColumn neraColumn5 in this._uniqueEpochEras.Values)
				{
					neraColumn5.SetPredictedPosition(list2[neraColumn5.GetIndex(false)]);
				}
				await this.GrowTimelineAndAddEraIcons(list);
			}
			else
			{
				this.InitLineAndIcons(list);
			}
		}

		/// <summary>
		/// Exists solely to calculate the expected position for a column during timeline expansion.
		/// This position is cached to prevent a 1 frame flicker of moving columns when the Timeline expands.
		/// </summary>
		// Token: 0x06002022 RID: 8226 RVA: 0x000D8DE8 File Offset: 0x000D6FE8
		private List<Vector2> PredictHBoxLayout(HBoxContainer hbox)
		{
			float num = 0f;
			float num2 = (float)hbox.GetThemeConstant(ThemeConstants.BoxContainer.Separation, "HBoxContainer");
			List<Control> list = (from c in hbox.GetChildren(false).OfType<Control>()
				where c.Visible
				select c).ToList<Control>();
			int num3 = 0;
			foreach (Control control in list)
			{
				num += control.CustomMinimumSize.X;
				if ((control.SizeFlagsHorizontal & 2L) != null)
				{
					num3++;
				}
			}
			num += num2 * (float)Math.Max(list.Count - 1, 0);
			float num4 = hbox.Size.X - num;
			float num5 = ((num3 > 0) ? (num4 / (float)num3) : 0f);
			float num6 = 0f;
			List<Vector2> list2 = new List<Vector2>();
			foreach (Control control2 in list)
			{
				float num7 = control2.CustomMinimumSize.X;
				if ((control2.SizeFlagsHorizontal & 2L) != null)
				{
					num7 += num5;
				}
				list2.Add(new Vector2(num6, 0f));
				num6 += num7 + num2;
			}
			return list2;
		}

		/// <summary>
		/// Called whenever we add new epoch slots to this screen.
		/// Expands the Timeline "line" and pops in the new Era icons if applicable.
		/// </summary>
		/// <param name="newlyCreatedColumns"></param>
		// Token: 0x06002023 RID: 8227 RVA: 0x000D8F60 File Offset: 0x000D7160
		private async Task GrowTimelineAndAddEraIcons(List<NEraColumn> newlyCreatedColumns)
		{
			if (newlyCreatedColumns.Count > 0)
			{
				Tween lineGrowTween = this._lineGrowTween;
				if (lineGrowTween != null)
				{
					lineGrowTween.Kill();
				}
				this._lineGrowTween = base.CreateTween().SetParallel(true);
				this._lineGrowTween.TweenProperty(this._lineContainer, "modulate:a", 1f, 0.5);
				this._lineGrowTween.TweenProperty(this._line, "custom_minimum_size:x", (float)this._uniqueEpochEras.Count * 226f, 2.0).SetEase(2L).SetTrans(7L);
				TaskAwaiter<bool> taskAwaiter = this._lineGrowTween.AwaitFinished(this).GetAwaiter();
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
				Log.Info("Spawning slots...", 2);
				foreach (NEraColumn neraColumn in newlyCreatedColumns)
				{
					neraColumn.SpawnIcon();
				}
				newlyCreatedColumns.Clear();
			}
			foreach (NEraColumn neraColumn2 in this._uniqueEpochEras.Values)
			{
				TaskHelper.RunSafely(neraColumn2.SpawnSlots(true));
			}
		}

		/// <summary>
		/// This is the non-animated version of GrowTimelineAndAddEraIcons()
		/// </summary>
		/// <param name="newlyCreatedColumns"></param>
		// Token: 0x06002024 RID: 8228 RVA: 0x000D8FAC File Offset: 0x000D71AC
		private void InitLineAndIcons(List<NEraColumn> newlyCreatedColumns)
		{
			if (newlyCreatedColumns.Count == 0)
			{
				return;
			}
			this._line.CustomMinimumSize = new Vector2((float)this._uniqueEpochEras.Count * 226f, this._line.CustomMinimumSize.Y);
			foreach (NEraColumn neraColumn in newlyCreatedColumns)
			{
				neraColumn.SpawnIcon();
			}
			foreach (NEraColumn neraColumn2 in this._uniqueEpochEras.Values)
			{
				TaskHelper.RunSafely(neraColumn2.SpawnSlots(false));
			}
			newlyCreatedColumns.Clear();
		}

		/// <summary>
		/// Lookup table to get the icon and name associated with a given EpochEra.
		/// Utilizes Tuples. Fancy.
		/// </summary>
		/// <param name="era"></param>
		// Token: 0x06002025 RID: 8229 RVA: 0x000D9088 File Offset: 0x000D7288
		[return: TupleElementNames(new string[] { "Texture", "Name" })]
		[return: Nullable(new byte[] { 0, 1, 1 })]
		public static ValueTuple<Texture2D, string> GetEraIcon(EpochEra era)
		{
			return new ValueTuple<Texture2D, string>(PreloadManager.Cache.GetTexture2D(NTimelineScreen.GetEraTexturePath(era)), StringHelper.Slugify(era.ToString()));
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x000D90B4 File Offset: 0x000D72B4
		private static string GetEraTexturePath(EpochEra era)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (era < EpochEra.Seeds0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("res://images/atlases/era_atlas.sprites/era_minus_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Math.Abs((int)era));
				defaultInterpolatedStringHandler.AppendLiteral(".tres");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("res://images/atlases/era_atlas.sprites/era_");
			defaultInterpolatedStringHandler.AppendFormatted<int>((int)era);
			defaultInterpolatedStringHandler.AppendLiteral(".tres");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000D912D File Offset: 0x000D732D
		private static IEnumerable<string> GetAllEraTexturePaths()
		{
			return new NTimelineScreen.<GetAllEraTexturePaths>d__46(-2);
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x000D9138 File Offset: 0x000D7338
		[NullableContext(2)]
		private NEpochSlot GetSlot(EpochEra era, int position)
		{
			foreach (KeyValuePair<EpochEra, NEraColumn> keyValuePair in this._uniqueEpochEras)
			{
				if (keyValuePair.Key == era)
				{
					return (NEpochSlot)keyValuePair.Value.GetChild(keyValuePair.Value.GetChildCount(false) - position - 2, false);
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find Epoch slot: ");
			defaultInterpolatedStringHandler.AppendFormatted<EpochEra>(era);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(position);
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			return null;
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x000D91F8 File Offset: 0x000D73F8
		public void OpenInspectScreen(NEpochSlot slot, bool playAnimation)
		{
			if (playAnimation)
			{
				TaskHelper.RunSafely(slot.GetParent<NEraColumn>().SpawnNameAndYear());
			}
			this._lastFocusedControl = slot;
			TaskHelper.RunSafely(this._inspectScreen.Open(slot, slot.model, playAnimation));
		}

		/// <summary>
		/// Misc unlock screens just show "text"
		/// </summary>
		/// <param name="text"></param>
		// Token: 0x0600202A RID: 8234 RVA: 0x000D9230 File Offset: 0x000D7430
		public void QueueMiscUnlock(string text)
		{
			NUnlockMiscScreen nunlockMiscScreen = NUnlockMiscScreen.Create();
			nunlockMiscScreen.SetUnlocks(text);
			this._unlockScreens.Enqueue(nunlockMiscScreen);
		}

		/// <summary>
		/// Character unlock screen shows text and the idle animation of the character
		/// </summary>
		// Token: 0x0600202B RID: 8235 RVA: 0x000D9256 File Offset: 0x000D7456
		public void QueueCharacterUnlock<[Nullable(0)] T>(EpochModel epoch) where T : CharacterModel
		{
			this._unlockScreens.Enqueue(NUnlockCharacterScreen.Create(epoch, ModelDb.Character<T>()));
		}

		/// <summary>
		/// Used by EpochModels to queue up a "Card Unlock Screen".
		/// See: IroncladUnlock1Epoch.cs
		/// </summary>
		/// <param name="cards"></param>
		// Token: 0x0600202C RID: 8236 RVA: 0x000D9274 File Offset: 0x000D7474
		public void QueueCardUnlock(IReadOnlyList<CardModel> cards)
		{
			NUnlockCardsScreen nunlockCardsScreen = NUnlockCardsScreen.Create();
			nunlockCardsScreen.SetCards(cards);
			this._unlockScreens.Enqueue(nunlockCardsScreen);
		}

		/// <summary>
		/// Used by EpochModels to queue up a "Relic Unlock Screen".
		/// See: Silent3Epoch.cs
		/// </summary>
		/// <param name="relics"></param>
		// Token: 0x0600202D RID: 8237 RVA: 0x000D929C File Offset: 0x000D749C
		public void QueueRelicUnlock(List<RelicModel> relics)
		{
			NUnlockRelicsScreen nunlockRelicsScreen = NUnlockRelicsScreen.Create();
			nunlockRelicsScreen.SetRelics(relics);
			this._unlockScreens.Enqueue(nunlockRelicsScreen);
		}

		/// <summary>
		/// Used by EpochModels to queue up a "Potion Unlock Screen".
		/// </summary>
		/// <param name="potions"></param>
		// Token: 0x0600202E RID: 8238 RVA: 0x000D92C4 File Offset: 0x000D74C4
		public void QueuePotionUnlock(List<PotionModel> potions)
		{
			NUnlockPotionsScreen nunlockPotionsScreen = NUnlockPotionsScreen.Create();
			nunlockPotionsScreen.SetPotions(potions);
			this._unlockScreens.Enqueue(nunlockPotionsScreen);
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000D92EC File Offset: 0x000D74EC
		public void QueueTimelineExpansion(List<EpochSlotData> eraData)
		{
			NUnlockTimelineScreen nunlockTimelineScreen = NUnlockTimelineScreen.Create();
			nunlockTimelineScreen.SetUnlocks(eraData);
			this._unlockScreens.Enqueue(nunlockTimelineScreen);
		}

		/// <summary>
		/// Allows the user to pan the screen left/right if there are 4+ Epochs
		/// </summary>
		// Token: 0x06002030 RID: 8240 RVA: 0x000D9312 File Offset: 0x000D7512
		public void SetScreenDraggability()
		{
			this._slotsContainer.MouseFilter = ((this._save.Epochs.Count <= 4) ? 2L : 0L);
		}

		/// <summary>
		/// Called when we open an Epoch Inspect screen.
		/// Because the backstop is transparent we do some fancy trickery.
		/// </summary>
		// Token: 0x06002031 RID: 8241 RVA: 0x000D9338 File Offset: 0x000D7538
		public void ShowBackstopAndHideUi()
		{
			this._backstop.Visible = true;
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.Kill();
			}
			this._backstopTween = base.CreateTween().SetParallel(true);
			this._backstopTween.TweenProperty(this._slotsContainer, "modulate:a", 0.1f, 0.4);
			this._backstopTween.TweenProperty(this._backstop, "modulate:a", 0.5f, 0.4);
			this._backButton.Disable();
			this._reminderText.AnimateOut();
		}

		/// <summary>
		/// Called when we close the Epoch Inspect screen.
		/// </summary>
		/// <param name="showBackButton"></param>
		// Token: 0x06002032 RID: 8242 RVA: 0x000D93E8 File Offset: 0x000D75E8
		public async Task HideBackstopAndShowUi(bool showBackButton)
		{
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.FastForwardToCompletion();
			}
			this._backstopTween = base.CreateTween().SetParallel(true);
			this._backstopTween.TweenProperty(this._slotsContainer, "modulate:a", 1f, 0.4).SetEase(1L).SetTrans(7L);
			this._backstopTween.TweenProperty(this._backstop, "modulate:a", 0f, 0.4).SetEase(1L).SetTrans(7L);
			if (showBackButton)
			{
				this.RefreshBackButton();
			}
			TaskAwaiter<bool> taskAwaiter = this._backstopTween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this._backstop.Visible = false;
			}
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000D9434 File Offset: 0x000D7634
		public void OpenQueuedScreen()
		{
			NUnlockScreen nunlockScreen = this._unlockScreens.Dequeue();
			this._unlockScreenHolder.AddChildSafely(nunlockScreen);
			nunlockScreen.Open();
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000D945F File Offset: 0x000D765F
		public bool IsScreenQueued()
		{
			return this._unlockScreens.Count > 0;
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000D946F File Offset: 0x000D766F
		private bool IsInspectScreenQueued()
		{
			return this._queuedInspectScreen != null;
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000D947C File Offset: 0x000D767C
		private async Task SpawnEraLabel(EpochEra era)
		{
			await this._uniqueEpochEras[era].SpawnNameAndYear();
		}

		/// <summary>
		/// Animates in the Header and Actions Available UI.
		/// Called during the first timeline expansion or on screen open (if any Epochs are slotted)
		/// </summary>
		// Token: 0x06002037 RID: 8247 RVA: 0x000D94C7 File Offset: 0x000D76C7
		public void ShowHeaderAndActionsUi()
		{
			if (this._isUiVisible)
			{
				return;
			}
			this._isUiVisible = true;
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000D94D9 File Offset: 0x000D76D9
		public void DisableInput()
		{
			this._inputBlocker.Visible = true;
			this._inputBlocker.MouseFilter = 0L;
			this._slotsContainer.SetEnabled(false);
			base.GetViewport().GuiReleaseFocus();
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000D950C File Offset: 0x000D770C
		public void EnableInput()
		{
			if (this._queuedInspectScreen == null && this._unlockScreens.Count == 0)
			{
				this.RefreshBackButton();
				this._inputBlocker.Visible = false;
				this._inputBlocker.MouseFilter = 2L;
				this._slotsContainer.SetEnabled(true);
				ActiveScreenContext.Instance.Update();
			}
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x000D9563 File Offset: 0x000D7763
		private void RefreshBackButton()
		{
			if (SaveManager.Instance.GetDiscoveredEpochCount() > 0)
			{
				if (this._epochSlotContainer.GetChildCount(false) > 1)
				{
					this._reminderText.AnimateIn();
				}
				this._backButton.Disable();
				return;
			}
			this._backButton.Enable();
		}

		/// <summary>
		/// Resets all vars and nodes on this screen. Used when players back out to main menu and reopen this screen.
		/// While less safe than destroying and recreating the scene, it's more efficient.
		/// </summary>
		// Token: 0x0600203B RID: 8251 RVA: 0x000D95A4 File Offset: 0x000D77A4
		private void ResetScreen()
		{
			CanvasItem lineContainer = this._lineContainer;
			Color modulate = this._lineContainer.Modulate;
			modulate.A = 0f;
			lineContainer.Modulate = modulate;
			Log.Info("Cleaning up Timeline screen...", 2);
			this._uniqueEpochEras = new Dictionary<EpochEra, NEraColumn>();
			this._queuedInspectScreen = null;
			this._unlockScreens = new Queue<NUnlockScreen>();
			this._epochSlotContainer.FreeChildren();
			this._reminderVfxHolder.FreeChildren();
			this._slotsContainer.Reset();
			if (this._tutorial != null)
			{
				this._tutorial.QueueFreeSafely();
				this._tutorial = null;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x000D9638 File Offset: 0x000D7838
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._epochSlotContainer.GetChildren(false).SelectMany<Node, NEpochSlot>((Node c) => c.GetChildren(false).OfType<NEpochSlot>()).FirstOrDefault<NEpochSlot>((NEpochSlot s) => s.model is NeowEpoch);
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000D9699 File Offset: 0x000D7899
		public Control GetReminderVfxHolder()
		{
			return this._reminderVfxHolder;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600203E RID: 8254 RVA: 0x000D96A4 File Offset: 0x000D78A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(22)
			{
				new MethodInfo(NTimelineScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.OnSubmenuShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.OnSubmenuHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.OnBackButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "obj", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTimelineScreen.MethodName.GetEraTexturePath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "era", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTimelineScreen.MethodName.GetSlot, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "era", 0L, "", 6L, false),
					new PropertyInfo(2L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTimelineScreen.MethodName.OpenInspectScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "slot", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(1L, "playAnimation", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTimelineScreen.MethodName.QueueMiscUnlock, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTimelineScreen.MethodName.SetScreenDraggability, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.ShowBackstopAndHideUi, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.OpenQueuedScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.IsScreenQueued, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.IsInspectScreenQueued, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.ShowHeaderAndActionsUi, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.DisableInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.EnableInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.RefreshBackButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.ResetScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineScreen.MethodName.GetReminderVfxHolder, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600203F RID: 8255 RVA: 0x000D9BFC File Offset: 0x000D7DFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTimelineScreen.MethodName.Create && args.Count == 0)
			{
				NTimelineScreen ntimelineScreen = NTimelineScreen.Create();
				ret = VariantUtils.CreateFrom<NTimelineScreen>(ref ntimelineScreen);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OnSubmenuShown && args.Count == 0)
			{
				this.OnSubmenuShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OnSubmenuHidden && args.Count == 0)
			{
				this.OnSubmenuHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OnBackButtonPressed && args.Count == 1)
			{
				NTimelineScreen.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.GetEraTexturePath && args.Count == 1)
			{
				string eraTexturePath = NTimelineScreen.GetEraTexturePath(VariantUtils.ConvertTo<EpochEra>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref eraTexturePath);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.GetSlot && args.Count == 2)
			{
				NEpochSlot slot = this.GetSlot(VariantUtils.ConvertTo<EpochEra>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = VariantUtils.CreateFrom<NEpochSlot>(ref slot);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OpenInspectScreen && args.Count == 2)
			{
				this.OpenInspectScreen(VariantUtils.ConvertTo<NEpochSlot>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.QueueMiscUnlock && args.Count == 1)
			{
				this.QueueMiscUnlock(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.SetScreenDraggability && args.Count == 0)
			{
				this.SetScreenDraggability();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.ShowBackstopAndHideUi && args.Count == 0)
			{
				this.ShowBackstopAndHideUi();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OpenQueuedScreen && args.Count == 0)
			{
				this.OpenQueuedScreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.IsScreenQueued && args.Count == 0)
			{
				bool flag = this.IsScreenQueued();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.IsInspectScreenQueued && args.Count == 0)
			{
				bool flag2 = this.IsInspectScreenQueued();
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.ShowHeaderAndActionsUi && args.Count == 0)
			{
				this.ShowHeaderAndActionsUi();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.DisableInput && args.Count == 0)
			{
				this.DisableInput();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.EnableInput && args.Count == 0)
			{
				this.EnableInput();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.RefreshBackButton && args.Count == 0)
			{
				this.RefreshBackButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.ResetScreen && args.Count == 0)
			{
				this.ResetScreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.GetReminderVfxHolder && args.Count == 0)
			{
				Control reminderVfxHolder = this.GetReminderVfxHolder();
				ret = VariantUtils.CreateFrom<Control>(ref reminderVfxHolder);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000D9FCC File Offset: 0x000D81CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTimelineScreen.MethodName.Create && args.Count == 0)
			{
				NTimelineScreen ntimelineScreen = NTimelineScreen.Create();
				ret = VariantUtils.CreateFrom<NTimelineScreen>(ref ntimelineScreen);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.OnBackButtonPressed && args.Count == 1)
			{
				NTimelineScreen.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineScreen.MethodName.GetEraTexturePath && args.Count == 1)
			{
				string eraTexturePath = NTimelineScreen.GetEraTexturePath(VariantUtils.ConvertTo<EpochEra>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref eraTexturePath);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002041 RID: 8257 RVA: 0x000DA078 File Offset: 0x000D8278
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTimelineScreen.MethodName.Create || (ref method) == NTimelineScreen.MethodName.OnSubmenuOpened || (ref method) == NTimelineScreen.MethodName.OnSubmenuClosed || (ref method) == NTimelineScreen.MethodName.OnSubmenuShown || (ref method) == NTimelineScreen.MethodName.OnSubmenuHidden || (ref method) == NTimelineScreen.MethodName._Ready || (ref method) == NTimelineScreen.MethodName.OnBackButtonPressed || (ref method) == NTimelineScreen.MethodName.GetEraTexturePath || (ref method) == NTimelineScreen.MethodName.GetSlot || (ref method) == NTimelineScreen.MethodName.OpenInspectScreen || (ref method) == NTimelineScreen.MethodName.QueueMiscUnlock || (ref method) == NTimelineScreen.MethodName.SetScreenDraggability || (ref method) == NTimelineScreen.MethodName.ShowBackstopAndHideUi || (ref method) == NTimelineScreen.MethodName.OpenQueuedScreen || (ref method) == NTimelineScreen.MethodName.IsScreenQueued || (ref method) == NTimelineScreen.MethodName.IsInspectScreenQueued || (ref method) == NTimelineScreen.MethodName.ShowHeaderAndActionsUi || (ref method) == NTimelineScreen.MethodName.DisableInput || (ref method) == NTimelineScreen.MethodName.EnableInput || (ref method) == NTimelineScreen.MethodName.RefreshBackButton || (ref method) == NTimelineScreen.MethodName.ResetScreen || (ref method) == NTimelineScreen.MethodName.GetReminderVfxHolder || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002042 RID: 8258 RVA: 0x000DA1D8 File Offset: 0x000D83D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTimelineScreen.PropertyName.CurrentUnlockScreen)
			{
				this.CurrentUnlockScreen = VariantUtils.ConvertTo<NUnlockScreen>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._inspectScreen)
			{
				this._inspectScreen = VariantUtils.ConvertTo<NEpochInspectScreen>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._reminderText)
			{
				this._reminderText = VariantUtils.ConvertTo<NEpochReminderText>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._reminderVfxHolder)
			{
				this._reminderVfxHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._inputBlocker)
			{
				this._inputBlocker = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._lineContainer)
			{
				this._lineContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._line)
			{
				this._line = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._epochSlotContainer)
			{
				this._epochSlotContainer = VariantUtils.ConvertTo<HBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._slotsContainer)
			{
				this._slotsContainer = VariantUtils.ConvertTo<NSlotsContainer>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._unlockScreenHolder)
			{
				this._unlockScreenHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._tutorial)
			{
				this._tutorial = VariantUtils.ConvertTo<NTimelineTutorial>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._isUiVisible)
			{
				this._isUiVisible = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._queuedInspectScreen)
			{
				this._queuedInspectScreen = VariantUtils.ConvertTo<NEpochSlot>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._lineGrowTween)
			{
				this._lineGrowTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._backstopTween)
			{
				this._backstopTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002043 RID: 8259 RVA: 0x000DA3B8 File Offset: 0x000D85B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTimelineScreen.PropertyName.CurrentUnlockScreen)
			{
				NUnlockScreen currentUnlockScreen = this.CurrentUnlockScreen;
				value = VariantUtils.CreateFrom<NUnlockScreen>(ref currentUnlockScreen);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._inspectScreen)
			{
				value = VariantUtils.CreateFrom<NEpochInspectScreen>(ref this._inspectScreen);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._reminderText)
			{
				value = VariantUtils.CreateFrom<NEpochReminderText>(ref this._reminderText);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._reminderVfxHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._reminderVfxHolder);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._inputBlocker)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._inputBlocker);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._lineContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._lineContainer);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._line)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._line);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._epochSlotContainer)
			{
				value = VariantUtils.CreateFrom<HBoxContainer>(ref this._epochSlotContainer);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._slotsContainer)
			{
				value = VariantUtils.CreateFrom<NSlotsContainer>(ref this._slotsContainer);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._unlockScreenHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._unlockScreenHolder);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._tutorial)
			{
				value = VariantUtils.CreateFrom<NTimelineTutorial>(ref this._tutorial);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._isUiVisible)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isUiVisible);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._queuedInspectScreen)
			{
				value = VariantUtils.CreateFrom<NEpochSlot>(ref this._queuedInspectScreen);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._lineGrowTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._lineGrowTween);
				return true;
			}
			if ((ref name) == NTimelineScreen.PropertyName._backstopTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._backstopTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002044 RID: 8260 RVA: 0x000DA614 File Offset: 0x000D8814
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTimelineScreen.PropertyName._inspectScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._reminderText, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._reminderVfxHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._inputBlocker, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._lineContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._line, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._epochSlotContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._slotsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._unlockScreenHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._tutorial, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTimelineScreen.PropertyName._isUiVisible, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._queuedInspectScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName.CurrentUnlockScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._lineGrowTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName._backstopTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002045 RID: 8261 RVA: 0x000DA87C File Offset: 0x000D8A7C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName currentUnlockScreen = NTimelineScreen.PropertyName.CurrentUnlockScreen;
			NUnlockScreen currentUnlockScreen2 = this.CurrentUnlockScreen;
			info.AddProperty(currentUnlockScreen, Variant.From<NUnlockScreen>(ref currentUnlockScreen2));
			info.AddProperty(NTimelineScreen.PropertyName._inspectScreen, Variant.From<NEpochInspectScreen>(ref this._inspectScreen));
			info.AddProperty(NTimelineScreen.PropertyName._reminderText, Variant.From<NEpochReminderText>(ref this._reminderText));
			info.AddProperty(NTimelineScreen.PropertyName._reminderVfxHolder, Variant.From<Control>(ref this._reminderVfxHolder));
			info.AddProperty(NTimelineScreen.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
			info.AddProperty(NTimelineScreen.PropertyName._inputBlocker, Variant.From<Control>(ref this._inputBlocker));
			info.AddProperty(NTimelineScreen.PropertyName._lineContainer, Variant.From<Control>(ref this._lineContainer));
			info.AddProperty(NTimelineScreen.PropertyName._line, Variant.From<Control>(ref this._line));
			info.AddProperty(NTimelineScreen.PropertyName._epochSlotContainer, Variant.From<HBoxContainer>(ref this._epochSlotContainer));
			info.AddProperty(NTimelineScreen.PropertyName._slotsContainer, Variant.From<NSlotsContainer>(ref this._slotsContainer));
			info.AddProperty(NTimelineScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NTimelineScreen.PropertyName._unlockScreenHolder, Variant.From<Control>(ref this._unlockScreenHolder));
			info.AddProperty(NTimelineScreen.PropertyName._tutorial, Variant.From<NTimelineTutorial>(ref this._tutorial));
			info.AddProperty(NTimelineScreen.PropertyName._isUiVisible, Variant.From<bool>(ref this._isUiVisible));
			info.AddProperty(NTimelineScreen.PropertyName._queuedInspectScreen, Variant.From<NEpochSlot>(ref this._queuedInspectScreen));
			info.AddProperty(NTimelineScreen.PropertyName._lineGrowTween, Variant.From<Tween>(ref this._lineGrowTween));
			info.AddProperty(NTimelineScreen.PropertyName._backstopTween, Variant.From<Tween>(ref this._backstopTween));
		}

		/// <inheritdoc />
		// Token: 0x06002046 RID: 8262 RVA: 0x000DAA0C File Offset: 0x000D8C0C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTimelineScreen.PropertyName.CurrentUnlockScreen, ref variant))
			{
				this.CurrentUnlockScreen = variant.As<NUnlockScreen>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._inspectScreen, ref variant2))
			{
				this._inspectScreen = variant2.As<NEpochInspectScreen>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._reminderText, ref variant3))
			{
				this._reminderText = variant3.As<NEpochReminderText>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._reminderVfxHolder, ref variant4))
			{
				this._reminderVfxHolder = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._backstop, ref variant5))
			{
				this._backstop = variant5.As<ColorRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._inputBlocker, ref variant6))
			{
				this._inputBlocker = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._lineContainer, ref variant7))
			{
				this._lineContainer = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._line, ref variant8))
			{
				this._line = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._epochSlotContainer, ref variant9))
			{
				this._epochSlotContainer = variant9.As<HBoxContainer>();
			}
			Variant variant10;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._slotsContainer, ref variant10))
			{
				this._slotsContainer = variant10.As<NSlotsContainer>();
			}
			Variant variant11;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._backButton, ref variant11))
			{
				this._backButton = variant11.As<NBackButton>();
			}
			Variant variant12;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._unlockScreenHolder, ref variant12))
			{
				this._unlockScreenHolder = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._tutorial, ref variant13))
			{
				this._tutorial = variant13.As<NTimelineTutorial>();
			}
			Variant variant14;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._isUiVisible, ref variant14))
			{
				this._isUiVisible = variant14.As<bool>();
			}
			Variant variant15;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._queuedInspectScreen, ref variant15))
			{
				this._queuedInspectScreen = variant15.As<NEpochSlot>();
			}
			Variant variant16;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._lineGrowTween, ref variant16))
			{
				this._lineGrowTween = variant16.As<Tween>();
			}
			Variant variant17;
			if (info.TryGetProperty(NTimelineScreen.PropertyName._backstopTween, ref variant17))
			{
				this._backstopTween = variant17.As<Tween>();
			}
		}

		// Token: 0x04000C8A RID: 3210
		private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/timeline_screen");

		// Token: 0x04000C8B RID: 3211
		private const string _placeEpochSparksPath = "res://scenes/timeline_screen/place_epoch_sparks.tscn";

		// Token: 0x04000C8C RID: 3212
		private NEpochInspectScreen _inspectScreen;

		// Token: 0x04000C8D RID: 3213
		private NEpochReminderText _reminderText;

		// Token: 0x04000C8E RID: 3214
		private Control _reminderVfxHolder;

		// Token: 0x04000C8F RID: 3215
		private ColorRect _backstop;

		// Token: 0x04000C90 RID: 3216
		private Control _inputBlocker;

		// Token: 0x04000C91 RID: 3217
		private Control _lineContainer;

		// Token: 0x04000C92 RID: 3218
		private Control _line;

		// Token: 0x04000C93 RID: 3219
		private HBoxContainer _epochSlotContainer;

		// Token: 0x04000C94 RID: 3220
		private NSlotsContainer _slotsContainer;

		// Token: 0x04000C95 RID: 3221
		private NBackButton _backButton;

		// Token: 0x04000C96 RID: 3222
		private Control _unlockScreenHolder;

		// Token: 0x04000C97 RID: 3223
		[Nullable(2)]
		private NTimelineTutorial _tutorial;

		// Token: 0x04000C98 RID: 3224
		private ProgressState _save;

		// Token: 0x04000C99 RID: 3225
		private bool _isUiVisible;

		// Token: 0x04000C9A RID: 3226
		private Dictionary<EpochEra, NEraColumn> _uniqueEpochEras = new Dictionary<EpochEra, NEraColumn>();

		// Token: 0x04000C9B RID: 3227
		[Nullable(2)]
		private NEpochSlot _queuedInspectScreen;

		// Token: 0x04000C9C RID: 3228
		private Queue<NUnlockScreen> _unlockScreens = new Queue<NUnlockScreen>();

		// Token: 0x04000C9E RID: 3230
		[Nullable(2)]
		private Tween _lineGrowTween;

		// Token: 0x04000C9F RID: 3231
		[Nullable(2)]
		private Tween _backstopTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012C1 RID: 4801
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004902 RID: 18690
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004903 RID: 18691
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04004904 RID: 18692
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnSubmenuShown' method.
			/// </summary>
			// Token: 0x04004905 RID: 18693
			public new static readonly StringName OnSubmenuShown = "OnSubmenuShown";

			/// <summary>
			/// Cached name for the 'OnSubmenuHidden' method.
			/// </summary>
			// Token: 0x04004906 RID: 18694
			public new static readonly StringName OnSubmenuHidden = "OnSubmenuHidden";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004907 RID: 18695
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnBackButtonPressed' method.
			/// </summary>
			// Token: 0x04004908 RID: 18696
			public static readonly StringName OnBackButtonPressed = "OnBackButtonPressed";

			/// <summary>
			/// Cached name for the 'GetEraTexturePath' method.
			/// </summary>
			// Token: 0x04004909 RID: 18697
			public static readonly StringName GetEraTexturePath = "GetEraTexturePath";

			/// <summary>
			/// Cached name for the 'GetSlot' method.
			/// </summary>
			// Token: 0x0400490A RID: 18698
			public static readonly StringName GetSlot = "GetSlot";

			/// <summary>
			/// Cached name for the 'OpenInspectScreen' method.
			/// </summary>
			// Token: 0x0400490B RID: 18699
			public static readonly StringName OpenInspectScreen = "OpenInspectScreen";

			/// <summary>
			/// Cached name for the 'QueueMiscUnlock' method.
			/// </summary>
			// Token: 0x0400490C RID: 18700
			public static readonly StringName QueueMiscUnlock = "QueueMiscUnlock";

			/// <summary>
			/// Cached name for the 'SetScreenDraggability' method.
			/// </summary>
			// Token: 0x0400490D RID: 18701
			public static readonly StringName SetScreenDraggability = "SetScreenDraggability";

			/// <summary>
			/// Cached name for the 'ShowBackstopAndHideUi' method.
			/// </summary>
			// Token: 0x0400490E RID: 18702
			public static readonly StringName ShowBackstopAndHideUi = "ShowBackstopAndHideUi";

			/// <summary>
			/// Cached name for the 'OpenQueuedScreen' method.
			/// </summary>
			// Token: 0x0400490F RID: 18703
			public static readonly StringName OpenQueuedScreen = "OpenQueuedScreen";

			/// <summary>
			/// Cached name for the 'IsScreenQueued' method.
			/// </summary>
			// Token: 0x04004910 RID: 18704
			public static readonly StringName IsScreenQueued = "IsScreenQueued";

			/// <summary>
			/// Cached name for the 'IsInspectScreenQueued' method.
			/// </summary>
			// Token: 0x04004911 RID: 18705
			public static readonly StringName IsInspectScreenQueued = "IsInspectScreenQueued";

			/// <summary>
			/// Cached name for the 'ShowHeaderAndActionsUi' method.
			/// </summary>
			// Token: 0x04004912 RID: 18706
			public static readonly StringName ShowHeaderAndActionsUi = "ShowHeaderAndActionsUi";

			/// <summary>
			/// Cached name for the 'DisableInput' method.
			/// </summary>
			// Token: 0x04004913 RID: 18707
			public static readonly StringName DisableInput = "DisableInput";

			/// <summary>
			/// Cached name for the 'EnableInput' method.
			/// </summary>
			// Token: 0x04004914 RID: 18708
			public static readonly StringName EnableInput = "EnableInput";

			/// <summary>
			/// Cached name for the 'RefreshBackButton' method.
			/// </summary>
			// Token: 0x04004915 RID: 18709
			public static readonly StringName RefreshBackButton = "RefreshBackButton";

			/// <summary>
			/// Cached name for the 'ResetScreen' method.
			/// </summary>
			// Token: 0x04004916 RID: 18710
			public static readonly StringName ResetScreen = "ResetScreen";

			/// <summary>
			/// Cached name for the 'GetReminderVfxHolder' method.
			/// </summary>
			// Token: 0x04004917 RID: 18711
			public static readonly StringName GetReminderVfxHolder = "GetReminderVfxHolder";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012C2 RID: 4802
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CurrentUnlockScreen' property.
			/// </summary>
			// Token: 0x04004918 RID: 18712
			public static readonly StringName CurrentUnlockScreen = "CurrentUnlockScreen";

			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004919 RID: 18713
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_inspectScreen' field.
			/// </summary>
			// Token: 0x0400491A RID: 18714
			public static readonly StringName _inspectScreen = "_inspectScreen";

			/// <summary>
			/// Cached name for the '_reminderText' field.
			/// </summary>
			// Token: 0x0400491B RID: 18715
			public static readonly StringName _reminderText = "_reminderText";

			/// <summary>
			/// Cached name for the '_reminderVfxHolder' field.
			/// </summary>
			// Token: 0x0400491C RID: 18716
			public static readonly StringName _reminderVfxHolder = "_reminderVfxHolder";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x0400491D RID: 18717
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_inputBlocker' field.
			/// </summary>
			// Token: 0x0400491E RID: 18718
			public static readonly StringName _inputBlocker = "_inputBlocker";

			/// <summary>
			/// Cached name for the '_lineContainer' field.
			/// </summary>
			// Token: 0x0400491F RID: 18719
			public static readonly StringName _lineContainer = "_lineContainer";

			/// <summary>
			/// Cached name for the '_line' field.
			/// </summary>
			// Token: 0x04004920 RID: 18720
			public static readonly StringName _line = "_line";

			/// <summary>
			/// Cached name for the '_epochSlotContainer' field.
			/// </summary>
			// Token: 0x04004921 RID: 18721
			public static readonly StringName _epochSlotContainer = "_epochSlotContainer";

			/// <summary>
			/// Cached name for the '_slotsContainer' field.
			/// </summary>
			// Token: 0x04004922 RID: 18722
			public static readonly StringName _slotsContainer = "_slotsContainer";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04004923 RID: 18723
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_unlockScreenHolder' field.
			/// </summary>
			// Token: 0x04004924 RID: 18724
			public static readonly StringName _unlockScreenHolder = "_unlockScreenHolder";

			/// <summary>
			/// Cached name for the '_tutorial' field.
			/// </summary>
			// Token: 0x04004925 RID: 18725
			public static readonly StringName _tutorial = "_tutorial";

			/// <summary>
			/// Cached name for the '_isUiVisible' field.
			/// </summary>
			// Token: 0x04004926 RID: 18726
			public static readonly StringName _isUiVisible = "_isUiVisible";

			/// <summary>
			/// Cached name for the '_queuedInspectScreen' field.
			/// </summary>
			// Token: 0x04004927 RID: 18727
			public static readonly StringName _queuedInspectScreen = "_queuedInspectScreen";

			/// <summary>
			/// Cached name for the '_lineGrowTween' field.
			/// </summary>
			// Token: 0x04004928 RID: 18728
			public static readonly StringName _lineGrowTween = "_lineGrowTween";

			/// <summary>
			/// Cached name for the '_backstopTween' field.
			/// </summary>
			// Token: 0x04004929 RID: 18729
			public static readonly StringName _backstopTween = "_backstopTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012C3 RID: 4803
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}

		// Token: 0x020012C4 RID: 4804
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400492A RID: 18730
			[Nullable(new byte[] { 0, 1 })]
			public static Action<NButton> <0>__OnBackButtonPressed;
		}
	}
}
