using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E1 RID: 1249
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class EventModel : AbstractModel
	{
		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004C8C RID: 19596 RVA: 0x0022C10E File Offset: 0x0022A30E
		public virtual Color ButtonColor
		{
			get
			{
				return new Color(1f, 1f, 1f, 0.9f);
			}
		}

		/// <summary>
		/// Deterministic events send out a checksum at their end. It's skipped for non-deterministic ones.
		/// - All standard events should be deterministic
		/// - All shared events are combat-related, and we don't trust end-of-combat rewards to be deterministic
		/// - One-off events which grant rewards are non-deterministic, such as Crystal Sphere
		/// </summary>
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06004C8D RID: 19597 RVA: 0x0022C129 File Offset: 0x0022A329
		public virtual bool IsDeterministic
		{
			get
			{
				return !this.IsShared;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06004C8E RID: 19598 RVA: 0x0022C134 File Offset: 0x0022A334
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06004C8F RID: 19599 RVA: 0x0022C137 File Offset: 0x0022A337
		public virtual string LocTable
		{
			get
			{
				return "events";
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x0022C13E File Offset: 0x0022A33E
		public LocString Title
		{
			get
			{
				return this.L10NLookup(base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06004C91 RID: 19601 RVA: 0x0022C15B File Offset: 0x0022A35B
		public virtual LocString InitialDescription
		{
			get
			{
				return this.L10NLookup(base.Id.Entry + ".pages.INITIAL.description");
			}
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x0022C178 File Offset: 0x0022A378
		[return: Nullable(2)]
		public LocString GetOptionTitle(string key)
		{
			return LocString.GetIfExists(this.LocTable, key + ".title");
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x0022C190 File Offset: 0x0022A390
		[return: Nullable(2)]
		public LocString GetOptionDescription(string key)
		{
			return LocString.GetIfExists(this.LocTable, key + ".description");
		}

		/// <summary>
		/// Get the owner of this particular event instance.
		/// When players enter an event in a multiplayer run, a separate EventModel instance is created with each player as
		/// the owner. Only the event owned by the local player is displayed in <see cref="T:MegaCrit.Sts2.Core.Nodes.Rooms.NEventRoom" />, but the other
		/// instances still exist on each player's device, so keep that in mind when using this property.
		/// Will be null in canonical events.
		/// </summary>
		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x0022C1A8 File Offset: 0x0022A3A8
		// (set) Token: 0x06004C95 RID: 19605 RVA: 0x0022C1B0 File Offset: 0x0022A3B0
		[Nullable(2)]
		public Player Owner
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// In non-shared events, multiplayer players may choose options independently.
		/// In shared events, multiplayer players vote on an option. Then, the top-voted option is executed for all players.
		/// There is no difference in singleplayer.
		/// All events that transition to other rooms (e.g. Dense Vegetation) must be shared.
		/// </summary>
		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x0022C1B9 File Offset: 0x0022A3B9
		public virtual bool IsShared
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x0022C1BC File Offset: 0x0022A3BC
		// (set) Token: 0x06004C98 RID: 19608 RVA: 0x0022C1C4 File Offset: 0x0022A3C4
		[Nullable(2)]
		public LocString Description
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// The encounter that should be displayed when entering this event.
		/// Only relevant when <see cref="P:MegaCrit.Sts2.Core.Models.EventModel.LayoutType" /> is <see cref="F:MegaCrit.Sts2.Core.Events.EventLayoutType.Combat" />.
		/// </summary>
		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06004C99 RID: 19609 RVA: 0x0022C1CD File Offset: 0x0022A3CD
		[Nullable(2)]
		public virtual EncounterModel CanonicalEncounter
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x0022C1D0 File Offset: 0x0022A3D0
		public async Task BeginEvent(Player player, [Nullable(2)] EventCombatSynchronizer combatSynchronizer, bool isPreFinished)
		{
			base.AssertMutable();
			if (this.Owner != null)
			{
				throw new InvalidOperationException("Tried to begin event, but it already has an owner!");
			}
			this.Owner = player;
			this.Rng = new Rng(this.Owner.RunState.Rng.Seed + (ulong)(this.IsShared ? 0L : ((long)this.Owner.RunState.GetPlayerSlotIndex(this.Owner))) + StringHelper.GetDeterministicHashCode(base.Id.Entry));
			if (this.CanonicalEncounter != null && combatSynchronizer == null)
			{
				throw new InvalidOperationException("Combat synchronizer must be passed to events that may transition to combat!");
			}
			this._combatSynchronizer = combatSynchronizer;
			try
			{
				await this.BeforeEventStarted(isPreFinished);
				this.CalculateVars();
				if (player.Creature.IsDead)
				{
					Log.Error("The generic event death message should not appear!", 2);
					this.SetEventFinished(this.L10NLookup("GENERIC.youAreDead.description"));
				}
				else
				{
					this.SetInitialEventState(isPreFinished);
				}
			}
			catch
			{
				this.EnsureCleanup();
				throw;
			}
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x0022C22C File Offset: 0x0022A42C
		protected virtual void SetInitialEventState(bool isPreFinished)
		{
			if (isPreFinished && !(this is AncientEventModel))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(80, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to load into pre-finished event ");
				defaultInterpolatedStringHandler.AppendFormatted<EventModel>(this);
				defaultInterpolatedStringHandler.AppendLiteral("! Only ancient events can be pre-finished.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			IReadOnlyList<EventOption> readOnlyList = this.GenerateInitialOptionsWrapper();
			this.SetEventState(this.InitialDescription, readOnlyList);
		}

		/// <summary>
		/// Wrapper around abstract GenerateInitialOptions, similar to <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.OnPlayWrapper(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Boolean,MegaCrit.Sts2.Core.Entities.Cards.ResourceInfo,System.Boolean)" />.
		/// Making this virtual lets us change initial option generation behavior in entire categories of events (like
		/// Ancient events), while still letting concrete event subclasses override the inner GenerateInitialOptions method.
		/// </summary>
		// Token: 0x06004C9C RID: 19612 RVA: 0x0022C290 File Offset: 0x0022A490
		protected virtual IReadOnlyList<EventOption> GenerateInitialOptionsWrapper()
		{
			base.AssertMutable();
			List<EventOption> list = this.GenerateInitialOptions().ToList<EventOption>();
			this.ReplaceNullOptions(list);
			return list;
		}

		/// <summary>
		/// Defensively protect against misconfigured events with null options.
		/// </summary>
		// Token: 0x06004C9D RID: 19613 RVA: 0x0022C2B8 File Offset: 0x0022A4B8
		protected void ReplaceNullOptions(List<EventOption> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				if (options[i] == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Event ");
					defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry);
					defaultInterpolatedStringHandler.AppendLiteral(" has a null option at index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					string text = defaultInterpolatedStringHandler.ToStringAndClear();
					Log.Error(text, 2);
					SentryService.CaptureException(new NullReferenceException(text));
					EventOption eventOption = new EventOption(this, null, "ERROR", Array.Empty<IHoverTip>());
					options[i] = eventOption;
				}
			}
		}

		// Token: 0x06004C9E RID: 19614
		protected abstract IReadOnlyList<EventOption> GenerateInitialOptions();

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x0022C363 File Offset: 0x0022A563
		// (set) Token: 0x06004CA0 RID: 19616 RVA: 0x0022C36B File Offset: 0x0022A56B
		public bool IsFinished
		{
			get
			{
				return this._isFinished;
			}
			private set
			{
				base.AssertMutable();
				this._isFinished = value;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06004CA1 RID: 19617 RVA: 0x0022C37A File Offset: 0x0022A57A
		public IReadOnlyList<EventOption> CurrentOptions
		{
			get
			{
				base.AssertMutable();
				if (this._currentOptions == null)
				{
					this._currentOptions = new List<EventOption>();
				}
				return this._currentOptions;
			}
		}

		/// <summary>
		/// Used by <see cref="T:MegaCrit.Sts2.Core.Models.Events.TheArchitect" /> to stop the initial options from showing until the initial animations have
		/// played. Probably worth coming up with a more robust solution after EA release.
		/// </summary>
		// Token: 0x06004CA2 RID: 19618 RVA: 0x0022C39B File Offset: 0x0022A59B
		protected void ClearCurrentOptions()
		{
			base.AssertMutable();
			if (this._currentOptions == null)
			{
				this._currentOptions = new List<EventOption>();
			}
			this._currentOptions.Clear();
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06004CA3 RID: 19619 RVA: 0x0022C3C4 File Offset: 0x0022A5C4
		// (remove) Token: 0x06004CA4 RID: 19620 RVA: 0x0022C3FC File Offset: 0x0022A5FC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<EventModel> StateChanged;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06004CA5 RID: 19621 RVA: 0x0022C434 File Offset: 0x0022A634
		// (remove) Token: 0x06004CA6 RID: 19622 RVA: 0x0022C46C File Offset: 0x0022A66C
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action EnteringEventCombat;

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06004CA7 RID: 19623 RVA: 0x0022C4A1 File Offset: 0x0022A6A1
		public DynamicVarSet DynamicVars
		{
			get
			{
				if (this._dynamicVars != null)
				{
					return this._dynamicVars;
				}
				this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
				this._dynamicVars.InitializeWithOwner(this);
				return this._dynamicVars;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06004CA8 RID: 19624 RVA: 0x0022C4D5 File Offset: 0x0022A6D5
		protected virtual IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		/// <summary>
		/// Whether or not this event is allowed to be entered based on the specified combat state.
		/// This will usually just return true, but some events require special conditions.
		/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Events.RelicTrader" /> can only be entered if you have 5+ relics and you're on act 2+.
		/// </summary>
		// Token: 0x06004CA9 RID: 19625 RVA: 0x0022C4DC File Offset: 0x0022A6DC
		public virtual bool IsAllowed(IRunState runState)
		{
			return true;
		}

		/// <summary>
		/// A per-event RNG that we can use to do random rolls in the event independently of the run's centralized RNG.
		/// This is safe to do in events because we don't need to keep track of a given event's RNG state once it's over.
		/// Null in canonical events, but we should never be using it there, so we mark it as non-nullable.
		/// </summary>
		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06004CAA RID: 19626 RVA: 0x0022C4DF File Offset: 0x0022A6DF
		// (set) Token: 0x06004CAB RID: 19627 RVA: 0x0022C4E7 File Offset: 0x0022A6E7
		public Rng Rng { get; private set; }

		/// <summary>
		/// Get all the EventOption LocStrings shown on the initial page.
		/// Used by NGameInfoUploader to upload info about the game for lookup elsewhere.
		/// Override in events with unusually dynamic options like <see cref="T:MegaCrit.Sts2.Core.Models.Events.TheFutureOfPotions" />.
		/// </summary>
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06004CAC RID: 19628 RVA: 0x0022C4F0 File Offset: 0x0022A6F0
		public virtual IEnumerable<LocString> GameInfoOptions
		{
			get
			{
				List<LocString> list = (from k in LocManager.Instance.GetTable(this.LocTable).Keys
					where k.StartsWith(base.Id.Entry + ".pages.INITIAL.options")
					select new LocString(this.LocTable, k)).ToList<LocString>();
				if (list.Count == 0)
				{
					throw new LocException("Event Loc for " + base.Id.Entry + " does not conform to the common format");
				}
				foreach (LocString locString in list)
				{
					this.DynamicVars.AddTo(locString);
				}
				return list;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004CAD RID: 19629 RVA: 0x0022C5AC File Offset: 0x0022A7AC
		public virtual EventLayoutType LayoutType
		{
			get
			{
				return EventLayoutType.Default;
			}
		}

		/// <summary>
		/// Create the scene that will be used for this event.
		/// </summary>
		// Token: 0x06004CAE RID: 19630 RVA: 0x0022C5AF File Offset: 0x0022A7AF
		public PackedScene CreateScene()
		{
			return PreloadManager.Cache.GetScene(this.LayoutScenePath);
		}

		/// <summary>
		/// The node that is being shown for this event.
		/// Null for canonical events, and before a mutable event has been initialized.
		/// </summary>
		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004CAF RID: 19631 RVA: 0x0022C5C1 File Offset: 0x0022A7C1
		// (set) Token: 0x06004CB0 RID: 19632 RVA: 0x0022C5C9 File Offset: 0x0022A7C9
		[Nullable(2)]
		public Control Node
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x0022C5D2 File Offset: 0x0022A7D2
		public void SetNode(Control node)
		{
			base.AssertMutable();
			if (this.Node != null)
			{
				throw new InvalidOperationException("Tried to set node, but it has already been set!");
			}
			this.Node = node;
			if (this.LayoutType == EventLayoutType.Custom)
			{
				((ICustomEventNode)this.Node).Initialize(this);
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x0022C610 File Offset: 0x0022A810
		private string LayoutScenePath
		{
			get
			{
				string text;
				switch (this.LayoutType)
				{
				case EventLayoutType.Default:
					text = "res://scenes/events/default_event_layout.tscn";
					break;
				case EventLayoutType.Combat:
					text = "res://scenes/events/combat_event_layout.tscn";
					break;
				case EventLayoutType.Ancient:
					text = "res://scenes/events/ancient_event_layout.tscn";
					break;
				case EventLayoutType.Custom:
					text = SceneHelper.GetScenePath("events/custom/" + base.Id.Entry.ToLowerInvariant());
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				return text;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x0022C67D File Offset: 0x0022A87D
		// (set) Token: 0x06004CB4 RID: 19636 RVA: 0x0022C68F File Offset: 0x0022A88F
		public EventModel CanonicalInstance
		{
			get
			{
				if (!base.IsMutable)
				{
					return this;
				}
				return this._canonicalInstance;
			}
			private set
			{
				base.AssertMutable();
				this._canonicalInstance = value;
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x0022C69E File Offset: 0x0022A89E
		private string InitialPortraitPath
		{
			get
			{
				return ImageHelper.GetImagePath("events/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x0022C6C4 File Offset: 0x0022A8C4
		private string InitialPhobiaModePortraitPath
		{
			get
			{
				return ImageHelper.GetImagePath("events/" + base.Id.Entry.ToLowerInvariant() + "_phobia_mode.png");
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x0022C6EA File Offset: 0x0022A8EA
		public bool HasPhobiaModePortrait
		{
			get
			{
				return ResourceLoader.Exists(this.InitialPhobiaModePortraitPath, "");
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x0022C6FC File Offset: 0x0022A8FC
		private string BackgroundScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("events/background_scenes/" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x0022C71D File Offset: 0x0022A91D
		private string VfxPath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/events/" + base.Id.Entry.ToLowerInvariant() + "_vfx");
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x0022C743 File Offset: 0x0022A943
		public bool HasVfx
		{
			get
			{
				return ResourceLoader.Exists(this.VfxPath, "");
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x0022C755 File Offset: 0x0022A955
		public static Vector2 VfxOffset
		{
			get
			{
				return new Vector2(268f, 49f);
			}
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x0022C766 File Offset: 0x0022A966
		public Texture2D CreateInitialPortrait()
		{
			return PreloadManager.Cache.GetTexture2D(this.InitialPortraitPath);
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x0022C778 File Offset: 0x0022A978
		public Texture2D CreateInitialPhobiaModePortrait()
		{
			return PreloadManager.Cache.GetTexture2D(this.InitialPhobiaModePortraitPath);
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0022C78A File Offset: 0x0022A98A
		public PackedScene CreateBackgroundScene()
		{
			return PreloadManager.Cache.GetScene(this.BackgroundScenePath);
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0022C79C File Offset: 0x0022A99C
		public Node2D CreateVfx()
		{
			return PreloadManager.Cache.GetScene(this.VfxPath).Instantiate<Node2D>(0L);
		}

		/// <summary>
		/// Create the visuals for the combat room that will be used for this event.
		/// Only relevant to <see cref="F:MegaCrit.Sts2.Core.Events.EventLayoutType.Combat" /> events.
		/// </summary>
		// Token: 0x06004CC0 RID: 19648 RVA: 0x0022C7B5 File Offset: 0x0022A9B5
		public ICombatRoomVisuals CreateCombatRoomVisuals(IEnumerable<Player> players, ActModel act)
		{
			if (this.LayoutType != EventLayoutType.Combat)
			{
				throw new InvalidOperationException("Tried to create combat room visuals for non-combat event!");
			}
			return new CombatEventVisuals(this._combatSynchronizer.MutableEncounterForLayout, players, act);
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x0022C7E0 File Offset: 0x0022A9E0
		public EventModel ToMutable()
		{
			base.AssertCanonical();
			EventModel eventModel = (EventModel)base.MutableClone();
			eventModel.CanonicalInstance = this;
			return eventModel;
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x0022C807 File Offset: 0x0022AA07
		protected override void DeepCloneFields()
		{
			base.DeepCloneFields();
			this._dynamicVars = this.DynamicVars.Clone(this);
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0022C821 File Offset: 0x0022AA21
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this.StateChanged = null;
			this.EnteringEventCombat = null;
			this._currentOptions = null;
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x0022C83E File Offset: 0x0022AA3E
		public virtual void CalculateVars()
		{
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x0022C840 File Offset: 0x0022AA40
		protected LocString L10NLookup(string entryName)
		{
			return new LocString(this.LocTable, entryName);
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0022C850 File Offset: 0x0022AA50
		public unsafe virtual IEnumerable<string> GetAssetPaths(IRunState runState)
		{
			if (TestMode.IsOn)
			{
				return Array.Empty<string>();
			}
			int num = 1;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = this.LayoutScenePath;
			List<string> list2 = list;
			switch (this.LayoutType)
			{
			case EventLayoutType.Default:
				list2.Add(this.InitialPortraitPath);
				if (this.HasPhobiaModePortrait)
				{
					list2.Add(this.InitialPhobiaModePortraitPath);
				}
				if (this.HasVfx)
				{
					list2.Add(this.VfxPath);
				}
				break;
			case EventLayoutType.Combat:
			{
				list2.AddRange(NCombatRoom.AssetPaths);
				EventCombatSynchronizer combatSynchronizer = this._combatSynchronizer;
				if (((combatSynchronizer != null) ? combatSynchronizer.MutableEncounterForLayout : null) != null)
				{
					list2.AddRange(this._combatSynchronizer.MutableEncounterForLayout.GetAssetPaths(runState));
				}
				break;
			}
			case EventLayoutType.Ancient:
				list2.Add(this.BackgroundScenePath);
				break;
			}
			return list2;
		}

		/// <summary>
		/// Gets called when entering an Event. Useful for setting ambient vfx, changing music, etc
		/// </summary>
		// Token: 0x06004CC7 RID: 19655 RVA: 0x0022C92E File Offset: 0x0022AB2E
		public virtual void OnRoomEnter()
		{
		}

		/// <summary>
		/// Gets called when an event is resumed. This should handle setting up the appropriate next page.
		/// This happens when the player was in an event, entered a new room (NOT a new <see cref="T:MegaCrit.Sts2.Core.Map.MapPoint" />), and then
		/// finished that room, causing them to resume the event room.
		/// This is most common in events where a choice can start a combat, like Dense Vegetation's Rest + Fight choice.
		/// </summary>
		/// <param name="exitedRoom">The room that was exited before this was resumed.</param>
		// Token: 0x06004CC8 RID: 19656 RVA: 0x0022C930 File Offset: 0x0022AB30
		public virtual Task Resume(AbstractRoom exitedRoom)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Call this from an event option function when the event is all finished and the player should be able to proceed.
		/// </summary>
		/// <param name="description">The description to set on the event room.</param>
		// Token: 0x06004CC9 RID: 19657 RVA: 0x0022C937 File Offset: 0x0022AB37
		protected void SetEventFinished(LocString description)
		{
			this.SetEventState(description, Array.Empty<EventOption>());
			this.IsFinished = true;
			this.EnsureCleanup();
		}

		/// <summary>
		/// Virtual function for events to hook onto if they need to do stuff before the event starts.
		/// </summary>
		// Token: 0x06004CCA RID: 19658 RVA: 0x0022C952 File Offset: 0x0022AB52
		protected virtual Task BeforeEventStarted(bool isPreFinished)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Virtual function for events to hook onto if they need to do stuff after the event starts.
		/// ONLY called for the locally owned event in multiplayer.
		/// </summary>
		// Token: 0x06004CCB RID: 19659 RVA: 0x0022C959 File Offset: 0x0022AB59
		public virtual Task AfterEventStarted()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Virtual function for events to hook onto if they need to do stuff when the event ends.
		/// </summary>
		// Token: 0x06004CCC RID: 19660 RVA: 0x0022C960 File Offset: 0x0022AB60
		protected virtual void OnEventFinished()
		{
		}

		/// <summary>
		/// Ensures OnEventFinished is called exactly once, preventing double-cleanup.
		/// </summary>
		// Token: 0x06004CCD RID: 19661 RVA: 0x0022C962 File Offset: 0x0022AB62
		public void EnsureCleanup()
		{
			if (!this._cleanupCalled)
			{
				this._cleanupCalled = true;
				this.OnEventFinished();
			}
		}

		/// <summary>
		/// Call this from an event option function when the event's description and options change to new ones.
		/// </summary>
		/// <param name="description">The new description to set on the event room.</param>
		/// <param name="eventOptions">The new choices to present to the player.</param>
		// Token: 0x06004CCE RID: 19662 RVA: 0x0022C97C File Offset: 0x0022AB7C
		protected virtual void SetEventState(LocString description, IEnumerable<EventOption> eventOptions)
		{
			base.AssertMutable();
			if (this._currentOptions == null)
			{
				this._currentOptions = new List<EventOption>();
			}
			this._currentOptions.Clear();
			this._currentOptions.AddRange(eventOptions);
			this.Description = description;
			if (this._currentOptions.Count == 0)
			{
				if (this._isFinished)
				{
					throw new InvalidOperationException("Tried to set event options after event was finished!");
				}
				this._isFinished = true;
			}
			Action<EventModel> stateChanged = this.StateChanged;
			if (stateChanged == null)
			{
				return;
			}
			stateChanged(this);
		}

		/// <summary>
		/// Enter an encounter, then return to this event once the encounter is finished.
		/// </summary>
		/// <param name="extraRewards">Extra rewards to give the player in addition to the encounter's normal rewards.</param>
		/// <param name="shouldResumeAfterCombat">
		/// Whether to resume this event after the encounter is finished.
		/// Usually true, but some events (like <see cref="T:MegaCrit.Sts2.Core.Models.Events.DenseVegetation" />) have no more options after combat ends, so
		/// they pass false to directly proceed to the next map point after combat ends.
		/// </param>
		// Token: 0x06004CCF RID: 19663 RVA: 0x0022C9F8 File Offset: 0x0022ABF8
		protected void EnterCombatWithoutExitingEvent<[Nullable(0)] T>(IReadOnlyList<Reward> extraRewards, bool shouldResumeAfterCombat) where T : EncounterModel
		{
			this.EnterCombatWithoutExitingEvent(ModelDb.Encounter<T>(), extraRewards, shouldResumeAfterCombat);
		}

		/// <summary>
		/// Enter an encounter, then return to this event once the encounter is finished.
		/// </summary>
		/// <param name="canonicalEncounter">The canonical encounter to enter.</param>
		/// <param name="extraRewards">Extra rewards to give the player in addition to the encounter's normal rewards.</param>
		/// <param name="shouldResumeAfterCombat">
		/// Whether to resume this event after the encounter is finished.
		/// Usually true, but some events (like <see cref="T:MegaCrit.Sts2.Core.Models.Events.DenseVegetation" />) have no more options after combat ends, so
		/// they pass false to directly proceed to the next map point after combat ends.
		/// </param>
		// Token: 0x06004CD0 RID: 19664 RVA: 0x0022CA0C File Offset: 0x0022AC0C
		protected void EnterCombatWithoutExitingEvent(EncounterModel canonicalEncounter, IReadOnlyList<Reward> extraRewards, bool shouldResumeAfterCombat)
		{
			if (!this.IsShared)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to enter combat in non-shared event ");
				defaultInterpolatedStringHandler.AppendFormatted<EventModel>(this);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (shouldResumeAfterCombat && this.LayoutType == EventLayoutType.Combat)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot resume event ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(base.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" after combat because it has a Combat layout — ");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear() + "there is no event layout to return to.");
			}
			Action enteringEventCombat = this.EnteringEventCombat;
			if (enteringEventCombat != null)
			{
				enteringEventCombat();
			}
			if (LocalContext.IsMe(this.Owner))
			{
				this.Node = null;
			}
			this._combatSynchronizer.ReadyToEnterCombat(canonicalEncounter, this.Owner, extraRewards, shouldResumeAfterCombat);
		}

		/// <summary>
		/// Generate an event option from a relic.
		/// </summary>
		/// <remarks>
		/// By default, the passed relic's title and description will be used as the title/description for this option.
		/// However, if events.json contains a {optionKey}.title and/or {optionKey}.description entry, they'll override the
		/// relic's associated field.
		///
		/// For example, at the time of this writing, Pael's "Liquify" option grants the <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PaelsClaw" /> relic. We
		/// want this option's title to be "Liquify" (not <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PaelsClaw" />), but we want its description to be
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PaelsClaw" />'s description.
		/// So, we pass textKey=PAEL.pages.INITIAL.options.LIQUIFY, and we add
		/// "PAEL.pages.INITIAL.options.LIQUIFY.title": "Liquify" to events.json, but we don't add a ".description" entry.
		/// </remarks>
		/// <param name="onChosen">Function to execute when this option is chosen</param>
		/// <param name="pageName">Name of the page that this option is in, for metrics/gameinfo and loc. INITIAL by default.</param>
		/// <typeparam name="T">Relic whose title, description, and HoverTips should be used for the event option.</typeparam>
		// Token: 0x06004CD1 RID: 19665 RVA: 0x0022CAE4 File Offset: 0x0022ACE4
		protected EventOption RelicOption<[Nullable(0)] T>([Nullable(new byte[] { 2, 1 })] Func<Task> onChosen, string pageName = "INITIAL") where T : RelicModel
		{
			RelicModel relicModel = ModelDb.Relic<T>().ToMutable();
			return this.RelicOption(relicModel, onChosen, pageName);
		}

		/// <summary>
		/// Generate an event option from a relic. See <see cref="M:MegaCrit.Sts2.Core.Models.EventModel.RelicOption``1(System.Func{System.Threading.Tasks.Task},System.String)" /> for more details.
		/// </summary>
		// Token: 0x06004CD2 RID: 19666 RVA: 0x0022CB0C File Offset: 0x0022AD0C
		protected EventOption RelicOption(RelicModel relic, [Nullable(new byte[] { 2, 1 })] Func<Task> onChosen, string pageName = "INITIAL")
		{
			relic.AssertMutable();
			relic.Owner = this.Owner;
			string text = this.OptionKey(pageName, relic.Id.Entry);
			return EventOption.FromRelic(relic, this, onChosen, text);
		}

		/// <summary>
		/// Generate a key for the specified option.
		/// </summary>
		/// <param name="optionName">Name of this option, for metrics/gameinfo and loc.</param>
		// Token: 0x06004CD3 RID: 19667 RVA: 0x0022CB47 File Offset: 0x0022AD47
		protected string InitialOptionKey(string optionName)
		{
			return this.OptionKey("INITIAL", optionName);
		}

		/// <summary>
		/// Generate a key for the specified page and option.
		/// </summary>
		/// <param name="optionName">Name of this option, for metrics/gameinfo and loc.</param>
		/// <param name="pageName">Name of the page that this option is in, for metrics/gameinfo and loc.</param>
		// Token: 0x06004CD4 RID: 19668 RVA: 0x0022CB58 File Offset: 0x0022AD58
		private string OptionKey(string pageName, string optionName)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 3);
			defaultInterpolatedStringHandler.AppendFormatted(StringHelper.Slugify(base.GetType().Name));
			defaultInterpolatedStringHandler.AppendLiteral(".pages.");
			defaultInterpolatedStringHandler.AppendFormatted(pageName);
			defaultInterpolatedStringHandler.AppendLiteral(".options.");
			defaultInterpolatedStringHandler.AppendFormatted(optionName);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Helper function for events that have you choose cards from a grid.
		/// Adds the unselected cards as cards skipped in the run history.
		/// </summary>
		/// <param name="cards">Care creation results to choose from</param>
		/// <param name="prefs">Card Selector prefs for the choice</param>
		// Token: 0x06004CD5 RID: 19669 RVA: 0x0022CBB8 File Offset: 0x0022ADB8
		protected async Task SelectCardsToAddToDeckFromGrid(List<CardCreationResult> cards, CardSelectorPrefs prefs)
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromSimpleGridForRewards(new BlockingPlayerChoiceContext(), cards, this.Owner, prefs);
			IEnumerable<CardModel> selectedCards = enumerable;
			IEnumerable<CardCreationResult> unselectedCards = cards.Where<CardCreationResult>((CardCreationResult c) => !selectedCards.Contains(c.Card));
			foreach (CardModel cardModel in selectedCards)
			{
				CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false), 1.2f, CardPreviewStyle.HorizontalLayout);
			}
			IEnumerator<CardModel> enumerator = null;
			foreach (CardCreationResult cardCreationResult in unselectedCards)
			{
				this.Owner.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Owner.NetId).CardChoices.Add(new CardChoiceHistoryEntry(cardCreationResult.Card, false));
			}
		}

		// Token: 0x04001D36 RID: 7478
		protected const string _initialPageKey = "INITIAL";

		// Token: 0x04001D39 RID: 7481
		[Nullable(2)]
		protected EventCombatSynchronizer _combatSynchronizer;

		// Token: 0x04001D3A RID: 7482
		[Nullable(new byte[] { 2, 1 })]
		private List<EventOption> _currentOptions;

		// Token: 0x04001D3B RID: 7483
		private bool _isFinished;

		// Token: 0x04001D3C RID: 7484
		private bool _cleanupCalled;

		// Token: 0x04001D3F RID: 7487
		[Nullable(2)]
		private DynamicVarSet _dynamicVars;

		// Token: 0x04001D42 RID: 7490
		private EventModel _canonicalInstance;
	}
}
