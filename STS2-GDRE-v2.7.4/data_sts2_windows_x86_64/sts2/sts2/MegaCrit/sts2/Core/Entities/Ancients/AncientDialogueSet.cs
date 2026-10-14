using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Ancients
{
	/// <summary>
	/// Represents the complete dialogue configuration for an ancient, organized by category.
	/// Each ancient defines its dialogues explicitly in C# via <see cref="M:MegaCrit.Sts2.Core.Models.AncientEventModel.DefineDialogues" />.
	/// Keys use X-Y format where X is the dialogue index within a collection and Y is the line index within a dialogue.
	/// Each line key ends with ".ancient" or ".char" to indicate the speaker. The speaker is derived from which key
	/// exists in the JSON at PopulateLocKeys time.
	///
	/// Dialogues that should be added to the "repeating" pool encode their repeating status in the loc keys by appending
	/// "r" to the X-Y component.
	/// For example, "1-0r.ancient" indicates a repeating dialogue line. When <see cref="M:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogue.PopulateLines(System.String,System.String,System.Int32)" />
	/// detects the "r" suffix on the first line, it automatically sets <see cref="P:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogue.IsRepeating" /> to true.
	///
	/// For example:
	/// "DARV.talk.IRONCLAD.0-0.ancient": "Dialogue 1, line 1 (ancient speaks)",
	/// "DARV.talk.IRONCLAD.0-1.char": "Dialogue 1, line 2 (character speaks)",
	/// "DARV.talk.IRONCLAD.0-2.ancient": "Dialogue 1, line 3 (ancient speaks)",
	/// "DARV.talk.IRONCLAD.1-0r.ancient": "Dialogue 2, line 1 (ancient speaks, repeating)",
	///
	/// "Next" button text must also be supplied for every line except the last line in each dialogue.
	/// Repeating dialogues use the "r" suffix on their next keys as well.
	///
	/// For example:
	/// "DARV.talk.IRONCLAD.0-0.ancient": "Dialogue 1, line 1",
	/// "DARV.talk.IRONCLAD.0-0.next": "Respond",
	/// "DARV.talk.IRONCLAD.0-1.char": "Dialogue 1, line 2",
	/// "DARV.talk.IRONCLAD.0-1.next": "Continue",
	/// "DARV.talk.IRONCLAD.0-2.ancient": "Dialogue 1, line 3",
	/// "DARV.talk.IRONCLAD.1-0r.ancient": "Dialogue 2, line 1 (repeating)",
	/// "DARV.talk.IRONCLAD.1-0r.next": "Acknowledge",
	/// "DARV.talk.IRONCLAD.1-1r.ancient": "Dialogue 2, line 2 (repeating)",
	/// </summary>
	// Token: 0x02000CD2 RID: 3282
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class AncientDialogueSet
	{
		/// <summary>
		/// First time ANY character visits this ancient (global once-only).
		/// Shown when totalAncientVisits == 0.
		/// If null, this ancient has no first-visit-ever dialogue (for example, The Architect).
		/// Example key: "DARV.talk.firstVisitEver.0-0.ancient"
		/// </summary>
		// Token: 0x17002467 RID: 9319
		// (get) Token: 0x0600852A RID: 34090 RVA: 0x002CBCE6 File Offset: 0x002C9EE6
		// (set) Token: 0x0600852B RID: 34091 RVA: 0x002CBCEE File Offset: 0x002C9EEE
		[Nullable(2)]
		[RequiredMember]
		public AncientDialogue FirstVisitEverDialogue
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// Per-character dialogues.
		/// Each <see cref="T:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogue" /> carries its own <see cref="P:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogue.VisitIndex" /> and
		/// <see cref="P:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogue.IsRepeating" /> metadata to determine when it should be shown.
		/// Example key: "DARV.talk.IRONCLAD.0-0.ancient"
		/// </summary>
		// Token: 0x17002468 RID: 9320
		// (get) Token: 0x0600852C RID: 34092 RVA: 0x002CBCF7 File Offset: 0x002C9EF7
		// (set) Token: 0x0600852D RID: 34093 RVA: 0x002CBCFF File Offset: 0x002C9EFF
		[RequiredMember]
		public Dictionary<string, IReadOnlyList<AncientDialogue>> CharacterDialogues { get; set; }

		/// <summary>
		/// Character-agnostic dialogues. Used as fallback when no visit-specific dialogue applies.
		/// Example key: "DARV.talk.ANY.0-0.ancient"
		/// </summary>
		// Token: 0x17002469 RID: 9321
		// (get) Token: 0x0600852E RID: 34094 RVA: 0x002CBD08 File Offset: 0x002C9F08
		// (set) Token: 0x0600852F RID: 34095 RVA: 0x002CBD10 File Offset: 0x002C9F10
		[RequiredMember]
		public IReadOnlyList<AncientDialogue> AgnosticDialogues { get; set; } = Array.Empty<AncientDialogue>();

		/// <summary>
		/// Get every dialogue in this set.
		/// </summary>
		// Token: 0x06008530 RID: 34096 RVA: 0x002CBD19 File Offset: 0x002C9F19
		public IEnumerable<AncientDialogue> GetAllDialogues()
		{
			AncientDialogueSet.<GetAllDialogues>d__12 <GetAllDialogues>d__ = new AncientDialogueSet.<GetAllDialogues>d__12(-2);
			<GetAllDialogues>d__.<>4__this = this;
			return <GetAllDialogues>d__;
		}

		/// <summary>
		/// Get valid dialogues for the specified character and visit counts.
		/// Returns a collection of dialogue sequences; the caller picks one randomly.
		///
		/// Priority order:
		/// 1. FirstVisitEverDialogue (totalVisits == 0)
		/// 2. Visit-specific dialogues for this character (matching VisitIndex)
		/// 3. Visit-specific character-agnostic dialogues (matching VisitIndex)
		/// 4. Repeating pool (character-specific + character-agnostic IsRepeating)
		/// </summary>
		/// <param name="characterId">The ID of the character visiting this ancient.</param>
		/// <param name="charVisits">Number of times this character has previously visited this ancient.</param>
		/// <param name="totalVisits">Total number of visits to this ancient across all characters.</param>
		/// <param name="allowAnyCharacterDialogues">
		/// Whether character-agnostic repeating dialogues are allowed.
		/// See <see cref="P:MegaCrit.Sts2.Core.Models.AncientEventModel.AnyCharacterDialogueBlacklist" />.
		/// </param>
		/// <returns>A collection of valid dialogue sequences to choose from.</returns>
		// Token: 0x06008531 RID: 34097 RVA: 0x002CBD2C File Offset: 0x002C9F2C
		public IEnumerable<AncientDialogue> GetValidDialogues(ModelId characterId, int charVisits, int totalVisits, bool allowAnyCharacterDialogues)
		{
			if (totalVisits == 0 && this.FirstVisitEverDialogue != null)
			{
				return new <>z__ReadOnlySingleElementList<AncientDialogue>(this.FirstVisitEverDialogue);
			}
			IReadOnlyList<AncientDialogue> readOnlyList = null;
			IReadOnlyList<AncientDialogue> readOnlyList2;
			if (this.CharacterDialogues.TryGetValue(characterId.Entry, out readOnlyList2))
			{
				readOnlyList = readOnlyList2;
				List<AncientDialogue> list = readOnlyList.Where<AncientDialogue>(delegate(AncientDialogue d)
				{
					int? visitIndex = d.VisitIndex;
					int charVisits2 = charVisits;
					return (visitIndex.GetValueOrDefault() == charVisits2) & (visitIndex != null);
				}).ToList<AncientDialogue>();
				if (list.Count > 0)
				{
					return list;
				}
			}
			if (allowAnyCharacterDialogues)
			{
				List<AncientDialogue> list2 = this.AgnosticDialogues.Where<AncientDialogue>(delegate(AncientDialogue d)
				{
					int? visitIndex2 = d.VisitIndex;
					int charVisits3 = charVisits;
					return (visitIndex2.GetValueOrDefault() == charVisits3) & (visitIndex2 != null);
				}).ToList<AncientDialogue>();
				if (list2.Count > 0)
				{
					return list2;
				}
			}
			List<AncientDialogue> list3 = new List<AncientDialogue>();
			if (readOnlyList != null)
			{
				AncientDialogueSet.AddRepeatingDialogues(readOnlyList, list3, charVisits);
			}
			if (allowAnyCharacterDialogues)
			{
				AncientDialogueSet.AddRepeatingDialogues(this.AgnosticDialogues, list3, charVisits);
			}
			return list3;
		}

		/// <summary>
		/// Auto-derives <see cref="P:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogueLine.LineText" /> and <see cref="P:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogueLine.NextButtonText" />
		/// LocStrings for every line based on its position in the dialogue structure.
		/// Keys use X-Y format where X is the dialogue index within a collection and Y is the line index within a dialogue.
		/// </summary>
		// Token: 0x06008532 RID: 34098 RVA: 0x002CBDF8 File Offset: 0x002C9FF8
		public void PopulateLocKeys(string ancientEntry)
		{
			AncientDialogue firstVisitEverDialogue = this.FirstVisitEverDialogue;
			if (firstVisitEverDialogue != null)
			{
				firstVisitEverDialogue.PopulateLines(ancientEntry, "firstVisitEver", 0);
			}
			foreach (KeyValuePair<string, IReadOnlyList<AncientDialogue>> keyValuePair in this.CharacterDialogues)
			{
				string text;
				IReadOnlyList<AncientDialogue> readOnlyList;
				keyValuePair.Deconstruct(out text, out readOnlyList);
				string text2 = text;
				IReadOnlyList<AncientDialogue> readOnlyList2 = readOnlyList;
				for (int i = 0; i < readOnlyList2.Count; i++)
				{
					readOnlyList2[i].PopulateLines(ancientEntry, text2, i);
				}
			}
			for (int j = 0; j < this.AgnosticDialogues.Count; j++)
			{
				this.AgnosticDialogues[j].PopulateLines(ancientEntry, "ANY", j);
			}
			foreach (AncientDialogue ancientDialogue in this.GetAllDialogues())
			{
				for (int k = 0; k < ancientDialogue.Lines.Count - 1; k++)
				{
					AncientDialogueLine ancientDialogueLine = ancientDialogue.Lines[k];
					string locEntryKey = ancientDialogueLine.LineText.LocEntryKey;
					string text3 = locEntryKey.Substring(0, locEntryKey.LastIndexOf('.'));
					string text4 = text3 + ".next";
					ancientDialogueLine.NextButtonText = new LocString("ancients", text4);
				}
			}
		}

		// Token: 0x06008533 RID: 34099 RVA: 0x002CBF74 File Offset: 0x002CA174
		private static void AddRepeatingDialogues(IEnumerable<AncientDialogue> source, List<AncientDialogue> destination, int charVisits)
		{
			foreach (AncientDialogue ancientDialogue in source)
			{
				if (ancientDialogue.IsRepeating)
				{
					if (ancientDialogue.VisitIndex != null)
					{
						int? visitIndex = ancientDialogue.VisitIndex;
						if ((charVisits < visitIndex.GetValueOrDefault()) & (visitIndex != null))
						{
							continue;
						}
					}
					destination.Add(ancientDialogue);
				}
			}
		}

		// Token: 0x06008534 RID: 34100 RVA: 0x002CBFF0 File Offset: 0x002CA1F0
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public AncientDialogueSet()
		{
		}
	}
}
