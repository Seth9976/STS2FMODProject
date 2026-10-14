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
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	// Token: 0x0200036B RID: 875
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Rooms/NEventRoom.cs")]
	public class NEventRoom : Control, IScreenContext
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x0017CF4C File Offset: 0x0017B14C
		[Nullable(2)]
		public static NEventRoom Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.EventRoom;
			}
		}

		/// <summary>
		/// The event layout node.
		/// Null in custom-layout events like <see cref="T:MegaCrit.Sts2.Core.Models.Events.FakeMerchant" />.
		/// </summary>
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x0017CF5E File Offset: 0x0017B15E
		[Nullable(2)]
		public NEventLayout Layout
		{
			[NullableContext(2)]
			get
			{
				return this._eventContainer.CurrentScene as NEventLayout;
			}
		}

		/// <summary>
		/// The Custom Event node.
		/// </summary>
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x0017CF70 File Offset: 0x0017B170
		[Nullable(2)]
		public ICustomEventNode CustomEventNode
		{
			[NullableContext(2)]
			get
			{
				return this._eventContainer.CurrentScene as ICustomEventNode;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x0017CF82 File Offset: 0x0017B182
		[Nullable(2)]
		public NCombatRoom EmbeddedCombatRoom
		{
			[NullableContext(2)]
			get
			{
				NCombatEventLayout ncombatEventLayout = this.Layout as NCombatEventLayout;
				if (ncombatEventLayout == null)
				{
					return null;
				}
				return ncombatEventLayout.EmbeddedCombatRoom;
			}
		}

		/// <summary>
		/// Container node for event VFX.
		/// Null in custom-layout events like <see cref="T:MegaCrit.Sts2.Core.Models.Events.FakeMerchant" />.
		/// </summary>
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x0017CF9A File Offset: 0x0017B19A
		// (set) Token: 0x0600331A RID: 13082 RVA: 0x0017CFA2 File Offset: 0x0017B1A2
		[Nullable(2)]
		public Control VfxContainer
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x0600331B RID: 13083 RVA: 0x0017CFAB File Offset: 0x0017B1AB
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/rooms/event_room.tscn");
			}
		}

		/// <summary>
		/// Creates an event room node for the given event.
		/// </summary>
		/// <param name="eventModel">The event to create the room node for.</param>
		/// <param name="runState">The state of the run that the event is in.</param>
		/// <param name="isPreFinished">Whether or not the event is pre-finished (i.e. loaded from a save after being finished).</param>
		// Token: 0x0600331C RID: 13084 RVA: 0x0017CFB8 File Offset: 0x0017B1B8
		[NullableContext(2)]
		public static NEventRoom Create([Nullable(1)] EventModel eventModel, IRunState runState, bool isPreFinished)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NEventRoom neventRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/event_room.tscn").Instantiate<NEventRoom>(0L);
			neventRoom._event = eventModel;
			neventRoom._isPreFinished = isPreFinished;
			if (runState != null)
			{
				neventRoom._runState = runState;
			}
			return neventRoom;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x0017D000 File Offset: 0x0017B200
		public override void _Ready()
		{
			if (this._event.Node != null)
			{
				throw new InvalidOperationException("Tried to create event room, but event already has a node!");
			}
			this._eventContainer = base.GetNode<NSceneContainer>("%EventContainer");
			NGame.Instance.SetScreenShakeTarget(this._eventContainer);
			Control control = this._event.CreateScene().Instantiate<Control>(0L);
			this._event.SetNode(control);
			this._eventContainer.SetCurrentScene(control);
			NEventLayout layout = this.Layout;
			this.VfxContainer = ((layout != null) ? layout.VfxContainer : null);
			TaskHelper.RunSafely(this.SetupLayout());
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x0017D09A File Offset: 0x0017B29A
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenUpdated;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x0017D0B4 File Offset: 0x0017B2B4
		public override void _ExitTree()
		{
			this._cts.Cancel();
			NGame.Instance.ClearScreenShakeTarget();
			this._event.StateChanged -= this.RefreshEventState;
			this._event.EnteringEventCombat -= this.OnEnteringEventCombat;
			foreach (EventOption eventOption in this._connectedOptions)
			{
				eventOption.BeforeChosen -= this.BeforeOptionChosen;
			}
			this._connectedOptions.Clear();
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenUpdated;
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x0017D178 File Offset: 0x0017B378
		private async Task SetupLayout()
		{
			if (this._event.Owner == null)
			{
				throw new InvalidOperationException("Event must be started before passed to NEventRoom!");
			}
			if (this.Layout != null)
			{
				this.Layout.SetEvent(this._event);
				this.SetTitle(this._event.Title);
				this._event.StateChanged += this.RefreshEventState;
				this._event.EnteringEventCombat += this.OnEnteringEventCombat;
				await Cmd.Wait(0.2f, this._cts.Token, false);
				this.SetDescription(this.GetDescriptionOrFallback());
				AncientEventModel ancientEventModel = this._event as AncientEventModel;
				if (ancientEventModel != null && !this._isPreFinished)
				{
					ModelId id = this._event.Owner.Character.Id;
					AncientStats statsForAncient = SaveManager.Instance.Progress.GetStatsForAncient(ancientEventModel.Id);
					int num = ((statsForAncient != null) ? statsForAncient.GetVisitsAs(id) : 0);
					int num2 = ((statsForAncient != null) ? statsForAncient.TotalVisits : 0);
					IEnumerable<AncientDialogue> validDialogues = ancientEventModel.DialogueSet.GetValidDialogues(id, num, num2, !ancientEventModel.AnyCharacterDialogueBlacklist.Contains(this._event.Owner.Character));
					AncientDialogue ancientDialogue = Rng.Chaotic.NextItem<AncientDialogue>(validDialogues);
					foreach (AncientDialogueLine ancientDialogueLine in ancientDialogue.Lines)
					{
						LocString lineText = ancientDialogueLine.LineText;
						if (lineText != null)
						{
							lineText.Add("Act1Name", this._runState.Acts[0].Title);
						}
					}
					((NAncientEventLayout)this.Layout).SetDialogue(ancientDialogue.Lines);
				}
				this.SetOptions(this._event);
				this.Layout.OnSetupComplete();
			}
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x0017D1BB File Offset: 0x0017B3BB
		public void SetPortrait(Texture2D portrait)
		{
			this.Layout.SetPortrait(portrait, null);
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x0017D1CA File Offset: 0x0017B3CA
		private void SetTitle(LocString title)
		{
			this.Layout.SetTitle(title.GetFormattedText());
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x0017D1E0 File Offset: 0x0017B3E0
		private void SetDescription(LocString description)
		{
			if (!description.Exists())
			{
				return;
			}
			CharacterModel character = this._event.Owner.Character;
			character.AddDetailsTo(description);
			description.Add("IsMultiplayer", this._event.Owner.RunState.Players.Count > 1);
			this._event.DynamicVars.AddTo(description);
			this.Layout.SetDescription(description.GetFormattedText());
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x0017D258 File Offset: 0x0017B458
		private void SetOptions(EventModel eventModel)
		{
			this.Layout.ClearOptions();
			IReadOnlyList<EventOption> readOnlyList = eventModel.CurrentOptions;
			if (eventModel.IsFinished)
			{
				Func<Task> func;
				if ((func = NEventRoom.<>O.<0>__Proceed) == null)
				{
					func = (NEventRoom.<>O.<0>__Proceed = new Func<Task>(NEventRoom.Proceed));
				}
				readOnlyList = new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(eventModel, func, "PROCEED", false, true, Array.Empty<IHoverTip>()));
			}
			foreach (EventOption eventOption in readOnlyList)
			{
				eventOption.BeforeChosen += this.BeforeOptionChosen;
				this._connectedOptions.Add(eventOption);
			}
			this.Layout.AddOptions(readOnlyList);
			Control defaultFocusedControl = this.DefaultFocusedControl;
			if (defaultFocusedControl == null)
			{
				return;
			}
			defaultFocusedControl.TryGrabFocus();
		}

		/// <summary>
		/// Occurs when the local player clicks an event option.
		/// </summary>
		// Token: 0x06003325 RID: 13093 RVA: 0x0017D320 File Offset: 0x0017B520
		public void OptionButtonClicked(EventOption option, int index)
		{
			if (option.IsLocked)
			{
				return;
			}
			if (option.IsProceed)
			{
				TaskHelper.RunSafely(option.Chosen());
				return;
			}
			if (!this._event.IsShared)
			{
				this.Layout.ClearOptions();
			}
			RunManager.Instance.EventSynchronizer.ChooseLocalOption(index);
		}

		/// <summary>
		/// Occurs after an option is executed. This may not occur at the same time as the local player clicking the button
		/// if we are in a shared multiplayer event.
		/// </summary>
		// Token: 0x06003326 RID: 13094 RVA: 0x0017D374 File Offset: 0x0017B574
		private async Task BeforeOptionChosen(EventOption option)
		{
			if (this._event.Owner.RunState.Players.Count > 1 && RunManager.Instance.EventSynchronizer.IsShared && !option.IsProceed)
			{
				await this.Layout.BeforeSharedOptionChosen(option);
			}
			else if (!option.IsProceed)
			{
				this.DisableOptionButtons();
			}
		}

		/// <summary>
		/// Called whenever an event's state changes - i.e. when an option is chosen.
		/// Note that, in multiplayer, this does not necessarily occur at the same time the local player chooses an option.
		/// For shared events, this is called after all players vote on an option.
		/// </summary>
		// Token: 0x06003327 RID: 13095 RVA: 0x0017D3BF File Offset: 0x0017B5BF
		private void RefreshEventState(EventModel eventModel)
		{
			this.SetDescription(this.GetDescriptionOrFallback());
			if (eventModel is AncientEventModel)
			{
				((NAncientEventLayout)this.Layout).ClearDialogue();
			}
			this.SetOptions(this._event);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x0017D3F1 File Offset: 0x0017B5F1
		private void DisableOptionButtons()
		{
			NEventLayout layout = this.Layout;
			if (layout == null)
			{
				return;
			}
			layout.DisableEventOptions();
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x0017D404 File Offset: 0x0017B604
		private void OnEnteringEventCombat()
		{
			this.DisableOptionButtons();
			NCombatEventLayout ncombatEventLayout = this.Layout as NCombatEventLayout;
			if (ncombatEventLayout != null)
			{
				ncombatEventLayout.HideEventVisuals();
			}
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x0017D42C File Offset: 0x0017B62C
		public static Task Proceed()
		{
			NMapScreen.Instance.SetTravelEnabled(true);
			NMapScreen.Instance.Open(false);
			return Task.CompletedTask;
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x0600332B RID: 13099 RVA: 0x0017D44C File Offset: 0x0017B64C
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				IScreenContext customEventNode = this.CustomEventNode;
				if (customEventNode != null)
				{
					return customEventNode.DefaultFocusedControl;
				}
				NEventLayout layout = this.Layout;
				if (layout == null)
				{
					return null;
				}
				return layout.DefaultFocusedControl;
			}
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x0017D47B File Offset: 0x0017B67B
		private LocString GetDescriptionOrFallback()
		{
			return this._event.Description ?? new LocString("events", "ERROR.description");
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x0017D49B File Offset: 0x0017B69B
		private void OnActiveScreenUpdated()
		{
			this.UpdateControllerNavEnabled<NEventRoom>();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600332E RID: 13102 RVA: 0x0017D4A4 File Offset: 0x0017B6A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NEventRoom.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventRoom.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventRoom.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventRoom.MethodName.SetPortrait, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "portrait", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null),
				new MethodInfo(NEventRoom.MethodName.DisableOptionButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventRoom.MethodName.OnEnteringEventCombat, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600332F RID: 13103 RVA: 0x0017D634 File Offset: 0x0017B834
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEventRoom.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventRoom.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventRoom.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventRoom.MethodName.SetPortrait && args.Count == 1)
			{
				this.SetPortrait(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventRoom.MethodName.DisableOptionButtons && args.Count == 0)
			{
				this.DisableOptionButtons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventRoom.MethodName.OnEnteringEventCombat && args.Count == 0)
			{
				this.OnEnteringEventCombat();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventRoom.MethodName.OnActiveScreenUpdated && args.Count == 0)
			{
				this.OnActiveScreenUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003330 RID: 13104 RVA: 0x0017D75C File Offset: 0x0017B95C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEventRoom.MethodName._Ready || (ref method) == NEventRoom.MethodName._EnterTree || (ref method) == NEventRoom.MethodName._ExitTree || (ref method) == NEventRoom.MethodName.SetPortrait || (ref method) == NEventRoom.MethodName.DisableOptionButtons || (ref method) == NEventRoom.MethodName.OnEnteringEventCombat || (ref method) == NEventRoom.MethodName.OnActiveScreenUpdated || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003331 RID: 13105 RVA: 0x0017D7DC File Offset: 0x0017B9DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEventRoom.PropertyName.VfxContainer)
			{
				this.VfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEventRoom.PropertyName._isPreFinished)
			{
				this._isPreFinished = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEventRoom.PropertyName._eventContainer)
			{
				this._eventContainer = VariantUtils.ConvertTo<NSceneContainer>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003332 RID: 13106 RVA: 0x0017D844 File Offset: 0x0017BA44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEventRoom.PropertyName.Layout)
			{
				NEventLayout layout = this.Layout;
				value = VariantUtils.CreateFrom<NEventLayout>(ref layout);
				return true;
			}
			if ((ref name) == NEventRoom.PropertyName.EmbeddedCombatRoom)
			{
				NCombatRoom embeddedCombatRoom = this.EmbeddedCombatRoom;
				value = VariantUtils.CreateFrom<NCombatRoom>(ref embeddedCombatRoom);
				return true;
			}
			if ((ref name) == NEventRoom.PropertyName.VfxContainer)
			{
				Control control = this.VfxContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NEventRoom.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NEventRoom.PropertyName._isPreFinished)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isPreFinished);
				return true;
			}
			if ((ref name) == NEventRoom.PropertyName._eventContainer)
			{
				value = VariantUtils.CreateFrom<NSceneContainer>(ref this._eventContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003333 RID: 13107 RVA: 0x0017D928 File Offset: 0x0017BB28
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NEventRoom.PropertyName._isPreFinished, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventRoom.PropertyName._eventContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventRoom.PropertyName.Layout, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventRoom.PropertyName.EmbeddedCombatRoom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventRoom.PropertyName.VfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventRoom.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003334 RID: 13108 RVA: 0x0017DA04 File Offset: 0x0017BC04
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName vfxContainer = NEventRoom.PropertyName.VfxContainer;
			Control vfxContainer2 = this.VfxContainer;
			info.AddProperty(vfxContainer, Variant.From<Control>(ref vfxContainer2));
			info.AddProperty(NEventRoom.PropertyName._isPreFinished, Variant.From<bool>(ref this._isPreFinished));
			info.AddProperty(NEventRoom.PropertyName._eventContainer, Variant.From<NSceneContainer>(ref this._eventContainer));
		}

		/// <inheritdoc />
		// Token: 0x06003335 RID: 13109 RVA: 0x0017DA60 File Offset: 0x0017BC60
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEventRoom.PropertyName.VfxContainer, ref variant))
			{
				this.VfxContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEventRoom.PropertyName._isPreFinished, ref variant2))
			{
				this._isPreFinished = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEventRoom.PropertyName._eventContainer, ref variant3))
			{
				this._eventContainer = variant3.As<NSceneContainer>();
			}
		}

		// Token: 0x040013D8 RID: 5080
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040013D9 RID: 5081
		private EventModel _event;

		// Token: 0x040013DA RID: 5082
		private IRunState _runState = NullRunState.Instance;

		// Token: 0x040013DB RID: 5083
		private bool _isPreFinished;

		// Token: 0x040013DC RID: 5084
		private NSceneContainer _eventContainer;

		// Token: 0x040013DD RID: 5085
		private const string _scenePath = "res://scenes/rooms/event_room.tscn";

		// Token: 0x040013DE RID: 5086
		private readonly List<EventOption> _connectedOptions = new List<EventOption>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200168A RID: 5770
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040057CD RID: 22477
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040057CE RID: 22478
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040057CF RID: 22479
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SetPortrait' method.
			/// </summary>
			// Token: 0x040057D0 RID: 22480
			public static readonly StringName SetPortrait = "SetPortrait";

			/// <summary>
			/// Cached name for the 'DisableOptionButtons' method.
			/// </summary>
			// Token: 0x040057D1 RID: 22481
			public static readonly StringName DisableOptionButtons = "DisableOptionButtons";

			/// <summary>
			/// Cached name for the 'OnEnteringEventCombat' method.
			/// </summary>
			// Token: 0x040057D2 RID: 22482
			public static readonly StringName OnEnteringEventCombat = "OnEnteringEventCombat";

			/// <summary>
			/// Cached name for the 'OnActiveScreenUpdated' method.
			/// </summary>
			// Token: 0x040057D3 RID: 22483
			public static readonly StringName OnActiveScreenUpdated = "OnActiveScreenUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200168B RID: 5771
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Layout' property.
			/// </summary>
			// Token: 0x040057D4 RID: 22484
			public static readonly StringName Layout = "Layout";

			/// <summary>
			/// Cached name for the 'EmbeddedCombatRoom' property.
			/// </summary>
			// Token: 0x040057D5 RID: 22485
			public static readonly StringName EmbeddedCombatRoom = "EmbeddedCombatRoom";

			/// <summary>
			/// Cached name for the 'VfxContainer' property.
			/// </summary>
			// Token: 0x040057D6 RID: 22486
			public static readonly StringName VfxContainer = "VfxContainer";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040057D7 RID: 22487
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_isPreFinished' field.
			/// </summary>
			// Token: 0x040057D8 RID: 22488
			public static readonly StringName _isPreFinished = "_isPreFinished";

			/// <summary>
			/// Cached name for the '_eventContainer' field.
			/// </summary>
			// Token: 0x040057D9 RID: 22489
			public static readonly StringName _eventContainer = "_eventContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200168C RID: 5772
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}

		// Token: 0x0200168D RID: 5773
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040057DA RID: 22490
			[Nullable(new byte[] { 0, 1 })]
			public static Func<Task> <0>__Proceed;
		}
	}
}
