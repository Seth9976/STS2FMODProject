using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.Timeline
{
	/// <summary>
	/// An abstract class which contains data for a single Epoch.
	/// </summary>
	// Token: 0x02000038 RID: 56
	[NullableContext(1)]
	[Nullable(0)]
	[GenerateSubtypes(DynamicallyAccessedMemberTypes = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
	public abstract class EpochModel
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00011A63 File Offset: 0x0000FC63
		public static IReadOnlyList<Type> AllEpochs
		{
			get
			{
				return EpochModel._allEpochs;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00011A6A File Offset: 0x0000FC6A
		public static IReadOnlyList<string> AllEpochIds
		{
			get
			{
				List<string> list;
				if ((list = EpochModel._allEpochIds) == null)
				{
					IEnumerable<Type> allEpochs = EpochModel._allEpochs;
					Func<Type, string> func;
					if ((func = EpochModel.<>O.<0>__GetId) == null)
					{
						func = (EpochModel.<>O.<0>__GetId = new Func<Type, string>(EpochModel.GetId));
					}
					list = (EpochModel._allEpochIds = allEpochs.Select<Type, string>(func).ToList<string>());
				}
				return list;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00011AA5 File Offset: 0x0000FCA5
		public static IReadOnlySet<string> EpochIdsHashSet
		{
			get
			{
				HashSet<string> hashSet;
				if ((hashSet = EpochModel._epochIdsHashSet) == null)
				{
					hashSet = (EpochModel._epochIdsHashSet = EpochModel.AllEpochIds.ToHashSet<string>());
				}
				return hashSet;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000241 RID: 577
		public abstract string Id { get; }

		/// <summary>
		/// Not used as of this time except for the header of the hovertip when peeking at the unlock requirement.
		/// </summary>
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00011AC0 File Offset: 0x0000FCC0
		public LocString Title
		{
			get
			{
				return new LocString("epochs", this.Id + ".title");
			}
		}

		/// <summary>
		/// The fancy lore text.
		/// </summary>
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00011ADC File Offset: 0x0000FCDC
		public string Description
		{
			get
			{
				return new LocString("epochs", this.Id + ".description").GetFormattedText();
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00011AFD File Offset: 0x0000FCFD
		[Nullable(2)]
		public string StoryTitle
		{
			[NullableContext(2)]
			get
			{
				if (this.StoryId == null)
				{
					return null;
				}
				return new LocString("epochs", "STORY_" + this.StoryId.ToUpperInvariant()).GetRawText();
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00011B2D File Offset: 0x0000FD2D
		[Nullable(2)]
		public virtual string StoryId
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// The text shown in the hovertip when you hover this Epoch while it's not yet obtained.
		/// </summary>
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00011B30 File Offset: 0x0000FD30
		public LocString UnlockInfo
		{
			get
			{
				return new LocString("epochs", this.Id + ".unlockInfo");
			}
		}

		/// <summary>
		/// The text which shows up at the bottom of the Epoch Inspect Screen, describing what had been unlocked.
		/// </summary>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00011B4C File Offset: 0x0000FD4C
		public virtual string UnlockText
		{
			get
			{
				return new LocString("epochs", this.Id + ".unlockText").GetFormattedText();
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000248 RID: 584
		public abstract EpochEra Era { get; }

		/// <summary>
		/// The "row" which this Era supposedly resides in. 0 is the bottom, 4 is the top!
		/// </summary>
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000249 RID: 585
		public abstract int EraPosition { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00011B6D File Offset: 0x0000FD6D
		public Texture2D Portrait
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.PackedPortraitPath, null, 1L);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00011B7D File Offset: 0x0000FD7D
		private string PackedPortraitPath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/epoch_atlas.sprites/" + this.Id.ToLowerInvariant() + ".tres");
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00011B9E File Offset: 0x0000FD9E
		public Texture2D RealPortrait
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.ResolvedPortraitPath, null, 1L);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00011BAE File Offset: 0x0000FDAE
		public bool HasRealPortrait
		{
			get
			{
				return ResourceLoader.Exists(this.RealPortraitPath, "");
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00011BC0 File Offset: 0x0000FDC0
		private string RealPortraitPath
		{
			get
			{
				return ImageHelper.GetImagePath("timeline/epoch_portraits/" + this.Id.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00011BE1 File Offset: 0x0000FDE1
		private string PlaceholderPortraitPath
		{
			get
			{
				return ImageHelper.GetImagePath("timeline/epoch_portraits/placeholder/" + this.Id.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00011C04 File Offset: 0x0000FE04
		public string ResolvedPortraitPath
		{
			get
			{
				if (this._resolvedPortraitPath != null)
				{
					return this._resolvedPortraitPath;
				}
				if (ResourceLoader.Exists(this.RealPortraitPath, ""))
				{
					this._resolvedPortraitPath = this.RealPortraitPath;
				}
				else
				{
					this._resolvedPortraitPath = this.PlaceholderPortraitPath;
				}
				return this._resolvedPortraitPath;
			}
		}

		/// <summary>
		/// Grabs the index of a given Epoch. If invalid, returns -1
		/// </summary>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00011C54 File Offset: 0x0000FE54
		public int ChapterIndex
		{
			get
			{
				if (this.StoryId == null)
				{
					return -1;
				}
				EpochModel[] epochs = StoryModel.Get(StringHelper.Slugify(this.StoryId)).Epochs;
				for (int i = 0; i < epochs.Length; i++)
				{
					if (epochs[i].Id == this.Id)
					{
						return i + 1;
					}
				}
				return -1;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00011CAC File Offset: 0x0000FEAC
		unsafe static EpochModel()
		{
			int num = 57;
			List<Type> list = new List<Type>(num);
			CollectionsMarshal.SetCount<Type>(list, num);
			Span<Type> span = CollectionsMarshal.AsSpan<Type>(list);
			int num2 = 0;
			*span[num2] = typeof(Act2BEpoch);
			num2++;
			*span[num2] = typeof(Act3BEpoch);
			num2++;
			*span[num2] = typeof(Colorless1Epoch);
			num2++;
			*span[num2] = typeof(Colorless2Epoch);
			num2++;
			*span[num2] = typeof(Colorless3Epoch);
			num2++;
			*span[num2] = typeof(Colorless4Epoch);
			num2++;
			*span[num2] = typeof(Colorless5Epoch);
			num2++;
			*span[num2] = typeof(CustomAndSeedsEpoch);
			num2++;
			*span[num2] = typeof(DailyRunEpoch);
			num2++;
			*span[num2] = typeof(DarvEpoch);
			num2++;
			*span[num2] = typeof(Defect1Epoch);
			num2++;
			*span[num2] = typeof(Defect2Epoch);
			num2++;
			*span[num2] = typeof(Defect3Epoch);
			num2++;
			*span[num2] = typeof(Defect4Epoch);
			num2++;
			*span[num2] = typeof(Defect5Epoch);
			num2++;
			*span[num2] = typeof(Defect6Epoch);
			num2++;
			*span[num2] = typeof(Defect7Epoch);
			num2++;
			*span[num2] = typeof(Event1Epoch);
			num2++;
			*span[num2] = typeof(Event2Epoch);
			num2++;
			*span[num2] = typeof(Event3Epoch);
			num2++;
			*span[num2] = typeof(Ironclad2Epoch);
			num2++;
			*span[num2] = typeof(Ironclad3Epoch);
			num2++;
			*span[num2] = typeof(Ironclad4Epoch);
			num2++;
			*span[num2] = typeof(Ironclad5Epoch);
			num2++;
			*span[num2] = typeof(Ironclad6Epoch);
			num2++;
			*span[num2] = typeof(Ironclad7Epoch);
			num2++;
			*span[num2] = typeof(Necrobinder1Epoch);
			num2++;
			*span[num2] = typeof(Necrobinder2Epoch);
			num2++;
			*span[num2] = typeof(Necrobinder3Epoch);
			num2++;
			*span[num2] = typeof(Necrobinder4Epoch);
			num2++;
			*span[num2] = typeof(Necrobinder5Epoch);
			num2++;
			*span[num2] = typeof(Necrobinder6Epoch);
			num2++;
			*span[num2] = typeof(Necrobinder7Epoch);
			num2++;
			*span[num2] = typeof(NeowEpoch);
			num2++;
			*span[num2] = typeof(OrobasEpoch);
			num2++;
			*span[num2] = typeof(Potion1Epoch);
			num2++;
			*span[num2] = typeof(Potion2Epoch);
			num2++;
			*span[num2] = typeof(Regent1Epoch);
			num2++;
			*span[num2] = typeof(Regent2Epoch);
			num2++;
			*span[num2] = typeof(Regent3Epoch);
			num2++;
			*span[num2] = typeof(Regent4Epoch);
			num2++;
			*span[num2] = typeof(Regent5Epoch);
			num2++;
			*span[num2] = typeof(Regent6Epoch);
			num2++;
			*span[num2] = typeof(Regent7Epoch);
			num2++;
			*span[num2] = typeof(Relic1Epoch);
			num2++;
			*span[num2] = typeof(Relic2Epoch);
			num2++;
			*span[num2] = typeof(Relic3Epoch);
			num2++;
			*span[num2] = typeof(Relic4Epoch);
			num2++;
			*span[num2] = typeof(Relic5Epoch);
			num2++;
			*span[num2] = typeof(Silent1Epoch);
			num2++;
			*span[num2] = typeof(Silent2Epoch);
			num2++;
			*span[num2] = typeof(Silent3Epoch);
			num2++;
			*span[num2] = typeof(Silent4Epoch);
			num2++;
			*span[num2] = typeof(Silent5Epoch);
			num2++;
			*span[num2] = typeof(Silent6Epoch);
			num2++;
			*span[num2] = typeof(Silent7Epoch);
			num2++;
			*span[num2] = typeof(UnderdocksEpoch);
			EpochModel._allEpochs = list;
			EpochModel._epochTypeDictionary = new Dictionary<string, Type>();
			EpochModel._typeToIdDictionary = new Dictionary<Type, string>();
			for (int i = 0; i < EpochModelSubtypes.Count; i++)
			{
				Type type = EpochModelSubtypes.Get(i);
				EpochModel epochModel = (EpochModel)Activator.CreateInstance(type);
				EpochModel._epochTypeDictionary[epochModel.Id] = type;
				EpochModel._typeToIdDictionary[type] = epochModel.Id;
			}
		}

		/// <summary>
		/// Returns the list of epochs whose slots are revealed when this epoch is revealed on the timeline.
		/// Used by <see cref="M:MegaCrit.Sts2.Core.Timeline.EpochModel.QueueTimelineExpansion(MegaCrit.Sts2.Core.Timeline.EpochModel[])" /> at runtime and by save validation to detect missing slots.
		/// </summary>
		// Token: 0x06000253 RID: 595 RVA: 0x00012259 File Offset: 0x00010459
		public virtual EpochModel[] GetTimelineExpansion()
		{
			return Array.Empty<EpochModel>();
		}

		/// <summary>
		/// WARN: Currently, every Epoch MUST unlock something. Whether it be information, cards, relics, etc.
		/// Without an unlock, if the player slots 2 Epochs at once, the game will function incorrectly.
		/// </summary>
		// Token: 0x06000254 RID: 596 RVA: 0x00012260 File Offset: 0x00010460
		public virtual void QueueUnlocks()
		{
		}

		/// <summary>
		/// Static method to get the Id for a given type
		/// </summary>
		// Token: 0x06000255 RID: 597 RVA: 0x00012262 File Offset: 0x00010462
		public static string GetId<[Nullable(0)] T>() where T : EpochModel
		{
			return EpochModel._typeToIdDictionary[typeof(T)];
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00012278 File Offset: 0x00010478
		public static string GetId(Type t)
		{
			return EpochModel._typeToIdDictionary[t];
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00012285 File Offset: 0x00010485
		public static bool IsValid(string id)
		{
			return EpochModel.EpochIdsHashSet.Contains(id);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00012294 File Offset: 0x00010494
		public static EpochModel Get(string id)
		{
			Type type;
			if (EpochModel._epochTypeDictionary.TryGetValue(id, out type))
			{
				return (EpochModel)Activator.CreateInstance(type);
			}
			throw new ArgumentException("Epoch with id '" + id + "' does not exist.");
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000122D1 File Offset: 0x000104D1
		public static EpochModel Get<[Nullable(0)] T>() where T : EpochModel
		{
			return EpochModel.Get(EpochModel.GetId<T>());
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000122E0 File Offset: 0x000104E0
		protected static void QueueTimelineExpansion(EpochModel[] epochs)
		{
			Log.Info("Queueing a Timeline expansion...", 2);
			List<EpochSlotData> list = new List<EpochSlotData>();
			for (int i = 0; i < epochs.Length; i++)
			{
				EpochModel epoch = epochs[i];
				SerializableEpoch serializableEpoch = SaveManager.Instance.Progress.Epochs.FirstOrDefault<SerializableEpoch>((SerializableEpoch e) => e.Id == epoch.Id);
				if (serializableEpoch != null && serializableEpoch.State == EpochState.ObtainedNoSlot)
				{
					Log.Info("We have it already Yay: " + serializableEpoch.Id, 2);
					list.Add(new EpochSlotData(epoch, EpochSlotState.Obtained));
				}
				else
				{
					list.Add(new EpochSlotData(epoch, EpochSlotState.NotObtained));
				}
			}
			NTimelineScreen.Instance.QueueTimelineExpansion(list);
			foreach (EpochModel epochModel in epochs)
			{
				SaveManager.Instance.UnlockSlot(epochModel.Id);
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000123CC File Offset: 0x000105CC
		protected string CreateCardUnlockText(List<CardModel> cards)
		{
			LocString locString = new LocString("timeline", "UNLOCK_TEXT.cards");
			cards = cards.OrderBy<CardModel, CardRarity>((CardModel c) => c.Rarity).ToList<CardModel>();
			for (int i = 0; i < 3; i++)
			{
				LocString locString2 = locString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Card");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
				locString2.Add(defaultInterpolatedStringHandler.ToStringAndClear(), this.GetColoredCardName(cards[i]));
			}
			return locString.GetFormattedText();
		}

		/// <summary>
		/// Little helper method for coloring the text.
		/// </summary>
		// Token: 0x0600025C RID: 604 RVA: 0x00012460 File Offset: 0x00010660
		private string GetColoredCardName(CardModel card)
		{
			if (card.Rarity == CardRarity.Common)
			{
				return card.TitleLocString.GetRawText();
			}
			if (card.Rarity == CardRarity.Uncommon)
			{
				return "[blue]" + card.TitleLocString.GetRawText() + "[/blue]";
			}
			if (card.Rarity == CardRarity.Rare)
			{
				return "[gold]" + card.TitleLocString.GetRawText() + "[/gold]";
			}
			return "ERROR";
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000124D0 File Offset: 0x000106D0
		protected string CreateRelicUnlockText(List<RelicModel> relics)
		{
			LocString locString = new LocString("timeline", "UNLOCK_TEXT.relics");
			relics = relics.OrderBy<RelicModel, RelicRarity>((RelicModel r) => r.Rarity).ToList<RelicModel>();
			for (int i = 0; i < 3; i++)
			{
				LocString locString2 = locString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Relic");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
				locString2.Add(defaultInterpolatedStringHandler.ToStringAndClear(), this.GetColoredRelicName(relics[i]));
			}
			return locString.GetFormattedText();
		}

		/// <summary>
		/// Little helper method for coloring the text.
		/// </summary>
		// Token: 0x0600025E RID: 606 RVA: 0x00012564 File Offset: 0x00010764
		private string GetColoredRelicName(RelicModel relic)
		{
			if (relic.Rarity == RelicRarity.Common)
			{
				return relic.Title.GetRawText();
			}
			if (relic.Rarity == RelicRarity.Uncommon)
			{
				return "[blue]" + relic.Title.GetRawText() + "[/blue]";
			}
			if (relic.Rarity == RelicRarity.Rare)
			{
				return "[gold]" + relic.Title.GetRawText() + "[/gold]";
			}
			return "ERROR";
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000125D4 File Offset: 0x000107D4
		protected string CreatePotionUnlockText(List<PotionModel> potions)
		{
			LocString locString = new LocString("timeline", "UNLOCK_TEXT.potions");
			potions = potions.OrderBy<PotionModel, PotionRarity>((PotionModel r) => r.Rarity).ToList<PotionModel>();
			for (int i = 0; i < 3; i++)
			{
				LocString locString2 = locString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Potion");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
				locString2.Add(defaultInterpolatedStringHandler.ToStringAndClear(), this.GetColoredPotionName(potions[i]));
			}
			return locString.GetFormattedText();
		}

		/// <summary>
		/// Little helper method for coloring the text.
		/// </summary>
		// Token: 0x06000260 RID: 608 RVA: 0x00012668 File Offset: 0x00010868
		private string GetColoredPotionName(PotionModel potion)
		{
			if (potion.Rarity == PotionRarity.Common)
			{
				return potion.Title.GetRawText();
			}
			if (potion.Rarity == PotionRarity.Uncommon)
			{
				return "[blue]" + potion.Title.GetRawText() + "[/blue]";
			}
			if (potion.Rarity == PotionRarity.Rare)
			{
				return "[gold]" + potion.Title.GetRawText() + "[/gold]";
			}
			return "ERROR";
		}

		/// <summary>
		/// List of all valid epochs currently in the game
		/// </summary>
		// Token: 0x04000108 RID: 264
		private static readonly List<Type> _allEpochs;

		// Token: 0x04000109 RID: 265
		[Nullable(new byte[] { 2, 1 })]
		private static List<string> _allEpochIds;

		// Token: 0x0400010A RID: 266
		[Nullable(new byte[] { 2, 1 })]
		private static HashSet<string> _epochIdsHashSet;

		// Token: 0x0400010B RID: 267
		[Nullable(2)]
		private string _resolvedPortraitPath;

		// Token: 0x0400010C RID: 268
		private static readonly Dictionary<string, Type> _epochTypeDictionary;

		// Token: 0x0400010D RID: 269
		private static readonly Dictionary<Type, string> _typeToIdDictionary;

		// Token: 0x02000E06 RID: 3590
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040032F1 RID: 13041
			[Nullable(0)]
			public static Func<Type, string> <0>__GetId;
		}
	}
}
