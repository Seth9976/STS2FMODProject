using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004D9 RID: 1241
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class AncientEventModel : EventModel
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06004AB1 RID: 19121 RVA: 0x00227A68 File Offset: 0x00225C68
		public override string LocTable
		{
			get
			{
				return "ancients";
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06004AB2 RID: 19122 RVA: 0x00227A6F File Offset: 0x00225C6F
		public LocString Epithet
		{
			get
			{
				return base.L10NLookup(base.Id.Entry + ".epithet");
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004AB3 RID: 19123 RVA: 0x00227A8C File Offset: 0x00225C8C
		public AncientDialogueSet DialogueSet
		{
			get
			{
				if (this._dialogueSet == null)
				{
					this._dialogueSet = this.DefineDialogues();
					this._dialogueSet.PopulateLocKeys(base.Id.Entry);
				}
				return this._dialogueSet;
			}
		}

		/// <summary>
		/// Define the dialogue structure for this ancient. Each subclass must implement this
		/// to specify dialogues for all categories (first meeting, repeating, etc.).
		/// </summary>
		// Token: 0x06004AB4 RID: 19124
		protected abstract AncientDialogueSet DefineDialogues();

		/// <summary>
		/// Convenience method to get a character's dictionary key from its type.
		/// Usage: CharKey&lt;Ironclad&gt;() returns "IRONCLAD".
		/// </summary>
		// Token: 0x06004AB5 RID: 19125 RVA: 0x00227ABE File Offset: 0x00225CBE
		protected static string CharKey<[Nullable(0)] T>() where T : CharacterModel
		{
			return ModelDb.Character<T>().Id.Entry;
		}

		/// <summary>
		/// The characters for whom the "any character" dialogue options shouldn't be shown.
		/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Events.Tezcatara" />'s "any character" dialogue is friendly, but they don't trust the
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Characters.Defect" />, so we blacklist them so we don't show these friendly dialogue options for them.
		/// </summary>
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004AB6 RID: 19126 RVA: 0x00227AD4 File Offset: 0x00225CD4
		public virtual IEnumerable<CharacterModel> AnyCharacterDialogueBlacklist
		{
			get
			{
				return Array.Empty<CharacterModel>();
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004AB7 RID: 19127 RVA: 0x00227ADB File Offset: 0x00225CDB
		public override Color ButtonColor
		{
			get
			{
				return new Color(0f, 0f, 0f, 0.35f);
			}
		}

		/// <summary>
		/// The color of the speech bubble in the Ancient events during Ancient Dialogue.
		/// </summary>
		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004AB8 RID: 19128 RVA: 0x00227AF6 File Offset: 0x00225CF6
		public virtual Color DialogueColor { get; } = new Color("28454f");

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06004AB9 RID: 19129 RVA: 0x00227AFE File Offset: 0x00225CFE
		// (set) Token: 0x06004ABA RID: 19130 RVA: 0x00227B06 File Offset: 0x00225D06
		[Nullable(2)]
		private string CustomDonePage
		{
			[NullableContext(2)]
			get
			{
				return this._customDonePage;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._customDonePage = value;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06004ABB RID: 19131 RVA: 0x00227B15 File Offset: 0x00225D15
		// (set) Token: 0x06004ABC RID: 19132 RVA: 0x00227B1D File Offset: 0x00225D1D
		[Nullable(2)]
		public string DebugOption
		{
			[NullableContext(2)]
			get
			{
				return this._debugOption;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._debugOption = value;
			}
		}

		/// <summary>
		/// The initial options generated when entering an instance of this Ancient event.
		/// Will always be null in canonical events.
		/// </summary>
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06004ABD RID: 19133 RVA: 0x00227B2C File Offset: 0x00225D2C
		// (set) Token: 0x06004ABE RID: 19134 RVA: 0x00227B34 File Offset: 0x00225D34
		[Nullable(new byte[] { 2, 1 })]
		private List<EventOption> GeneratedOptions
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._generatedOptions;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				base.AssertMutable();
				this._generatedOptions = value;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06004ABF RID: 19135 RVA: 0x00227B43 File Offset: 0x00225D43
		public override EventLayoutType LayoutType
		{
			get
			{
				return EventLayoutType.Ancient;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06004AC0 RID: 19136 RVA: 0x00227B46 File Offset: 0x00225D46
		private string MapIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/map/ancients/ancient_node_" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06004AC1 RID: 19137 RVA: 0x00227B6C File Offset: 0x00225D6C
		private string MapIconOutlinePath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/map/ancients/ancient_node_" + base.Id.Entry.ToLowerInvariant() + "_outline.png");
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004AC2 RID: 19138 RVA: 0x00227B92 File Offset: 0x00225D92
		public Texture2D MapIcon
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.MapIconPath);
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06004AC3 RID: 19139 RVA: 0x00227BA4 File Offset: 0x00225DA4
		public Texture2D MapIconOutline
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.MapIconOutlinePath);
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06004AC4 RID: 19140 RVA: 0x00227BB6 File Offset: 0x00225DB6
		public IEnumerable<string> MapNodeAssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { this.MapIconPath, this.MapIconOutlinePath });
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06004AC5 RID: 19141 RVA: 0x00227BD5 File Offset: 0x00225DD5
		public virtual string AmbientBgm
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06004AC6 RID: 19142 RVA: 0x00227BDC File Offset: 0x00225DDC
		public bool HasAmbientBgm
		{
			get
			{
				return this.AmbientBgm != "";
			}
		}

		/// <summary>
		/// The icon used for this Ancient in the Run History screen.
		/// Also used in other spots, like <see cref="T:MegaCrit.Sts2.Core.Nodes.Events.NAncientDialogueLine" />.
		/// </summary>
		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06004AC7 RID: 19143 RVA: 0x00227BEE File Offset: 0x00225DEE
		public Texture2D RunHistoryIcon
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(ImageHelper.GetRoomIconPath(MapPointType.Ancient, RoomType.Event, base.Id));
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06004AC8 RID: 19144 RVA: 0x00227C07 File Offset: 0x00225E07
		public Texture2D RunHistoryIconOutline
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.RunHistoryIconOutlinePath);
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06004AC9 RID: 19145 RVA: 0x00227C19 File Offset: 0x00225E19
		private string RunHistoryIconOutlinePath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/run_history/" + base.Id.Entry.ToLowerInvariant() + "_outline.png");
			}
		}

		/// <summary>
		/// The amount of health the player healed upon entering this ancient.
		/// </summary>
		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06004ACA RID: 19146 RVA: 0x00227C3F File Offset: 0x00225E3F
		// (set) Token: 0x06004ACB RID: 19147 RVA: 0x00227C47 File Offset: 0x00225E47
		public int HealedAmount { get; private set; }

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06004ACC RID: 19148
		public abstract IEnumerable<EventOption> AllPossibleOptions { get; }

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06004ACD RID: 19149 RVA: 0x00227C50 File Offset: 0x00225E50
		// (set) Token: 0x06004ACE RID: 19150 RVA: 0x00227C58 File Offset: 0x00225E58
		protected virtual Color EventButtonColor { get; set; } = new Color("00000059");

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06004ACF RID: 19151 RVA: 0x00227C61 File Offset: 0x00225E61
		public override LocString InitialDescription
		{
			get
			{
				if (!RunManager.Instance.IsInProgress || Hook.ShouldAllowAncient(base.Owner.RunState, base.Owner, this))
				{
					return base.InitialDescription;
				}
				return new LocString("relics", "WAX_CHOKER.blockMessage");
			}
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00227CA0 File Offset: 0x00225EA0
		protected override async Task BeforeEventStarted(bool isPreFinished)
		{
			if (!isPreFinished)
			{
				if (this is Neow)
				{
					base.Owner.Creature.SetCurrentHpInternal(0m);
				}
				int oldHp = base.Owner.Creature.CurrentHp;
				decimal num = base.Owner.Creature.MaxHp - base.Owner.Creature.CurrentHp;
				if (RunManager.Instance.HasAscension(AscensionLevel.WearyTraveler))
				{
					num *= 0.8m;
				}
				await CreatureCmd.Heal(base.Owner.Creature, num, false);
				if (NRun.Instance != null && this is Neow)
				{
					TaskHelper.RunSafely(NRun.Instance.GlobalUi.TopBar.Hp.LerpAtNeow());
				}
				this.HealedAmount = base.Owner.Creature.CurrentHp - oldHp;
			}
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x00227CEC File Offset: 0x00225EEC
		protected sealed override IReadOnlyList<EventOption> GenerateInitialOptionsWrapper()
		{
			if (Hook.ShouldAllowAncient(base.Owner.RunState, base.Owner, this))
			{
				this.GeneratedOptions = this.GenerateInitialOptions().ToList<EventOption>();
				if (this.DebugOption != null)
				{
					this.GeneratedOptions.RemoveAt(0);
					this.GeneratedOptions.Insert(0, this.AllPossibleOptions.First<EventOption>((EventOption c) => c.TextKey.Contains(this.DebugOption)));
				}
				base.ReplaceNullOptions(this.GeneratedOptions);
				return this.GeneratedOptions;
			}
			Func<Task> func;
			if ((func = AncientEventModel.<>O.<0>__Proceed) == null)
			{
				func = (AncientEventModel.<>O.<0>__Proceed = new Func<Task>(NEventRoom.Proceed));
			}
			return new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, func, "PROCEED", false, true, Array.Empty<IHoverTip>()));
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x00227DA0 File Offset: 0x00225FA0
		protected override void SetInitialEventState(bool isPreFinished)
		{
			IReadOnlyList<EventOption> readOnlyList = this.GenerateInitialOptionsWrapper();
			if (readOnlyList.Count == 0 || isPreFinished)
			{
				this.StartPreFinished();
				return;
			}
			this.SetEventState(this.InitialDescription, readOnlyList);
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x00227DD8 File Offset: 0x00225FD8
		private void UpdateRunHistory()
		{
			if (!RunManager.Instance.IsInProgress)
			{
				return;
			}
			foreach (EventOption eventOption in this.GeneratedOptions)
			{
				AncientChoiceHistoryEntry ancientChoiceHistoryEntry = new AncientChoiceHistoryEntry(eventOption.Title, eventOption.WasChosen);
				MapPointHistoryEntry currentMapPointHistoryEntry = base.Owner.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(base.Owner.NetId).AncientChoices.Add(ancientChoiceHistoryEntry);
				}
			}
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x00227E74 File Offset: 0x00226074
		public void StartPreFinished()
		{
			if (this.CustomDonePage == null)
			{
				base.SetEventFinished(base.L10NLookup(base.Id.Entry + ".pages.DONE.description"));
				return;
			}
			base.SetEventFinished(base.L10NLookup(this.CustomDonePage));
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x00227EB4 File Offset: 0x002260B4
		protected void Done()
		{
			this.UpdateRunHistory();
			if (this.CustomDonePage == null)
			{
				base.SetEventFinished(base.L10NLookup(base.Id.Entry + ".pages.DONE.description"));
				return;
			}
			base.SetEventFinished(base.L10NLookup(this.CustomDonePage));
		}

		/// <summary>
		/// Generate an event option from a relic.
		/// This overload also generates an onChosen callback that makes the player obtain the specified relic and then
		/// finishes the event.
		/// </summary>
		/// <param name="pageName">Name of the page that this option is in, for metrics/gameinfo and loc. INITIAL by default.</param>
		/// <param name="customDonePage">custom page path that this choice goes to next</param>
		/// <typeparam name="T">Relic whose title, description, and HoverTips should be used for the event option.</typeparam>
		// Token: 0x06004AD6 RID: 19158 RVA: 0x00227F03 File Offset: 0x00226103
		protected EventOption RelicOption<[Nullable(0)] T>(string pageName = "INITIAL", [Nullable(2)] string customDonePage = null) where T : RelicModel
		{
			return this.RelicOption(ModelDb.Relic<T>().ToMutable(), pageName, null);
		}

		/// <summary>
		/// Generate an event option from a relic.
		/// This overload also generates an onChosen callback that makes the player obtain the specified relic and then
		/// finishes the event.
		/// See <see cref="M:MegaCrit.Sts2.Core.Models.AncientEventModel.RelicOption``1(System.String,System.String)" /> for more details.
		/// </summary>
		// Token: 0x06004AD7 RID: 19159 RVA: 0x00227F1C File Offset: 0x0022611C
		protected EventOption RelicOption(RelicModel relic, string pageName = "INITIAL", [Nullable(2)] string customDonePage = null)
		{
			AncientEventModel.<>c__DisplayClass69_0 CS$<>8__locals1 = new AncientEventModel.<>c__DisplayClass69_0();
			CS$<>8__locals1.relic = relic;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.customDonePage = customDonePage;
			return base.RelicOption(CS$<>8__locals1.relic, new Func<Task>(CS$<>8__locals1.<RelicOption>g__OnChosen|0), pageName);
		}

		// Token: 0x04001CDF RID: 7391
		[Nullable(2)]
		private AncientDialogueSet _dialogueSet;

		// Token: 0x04001CE0 RID: 7392
		[Nullable(new byte[] { 2, 1 })]
		private List<EventOption> _generatedOptions;

		// Token: 0x04001CE1 RID: 7393
		[Nullable(2)]
		private string _customDonePage;

		// Token: 0x04001CE3 RID: 7395
		[Nullable(2)]
		private string _debugOption;

		// Token: 0x02001A06 RID: 6662
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04006687 RID: 26247
			[Nullable(new byte[] { 0, 1 })]
			public static Func<Task> <0>__Proceed;
		}
	}
}
