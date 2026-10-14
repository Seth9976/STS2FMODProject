using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Entities.Ancients
{
	// Token: 0x02000CD0 RID: 3280
	[NullableContext(1)]
	[Nullable(0)]
	public class AncientDialogue
	{
		// Token: 0x1700245F RID: 9311
		// (get) Token: 0x06008516 RID: 34070 RVA: 0x002CB905 File Offset: 0x002C9B05
		public IReadOnlyList<AncientDialogueLine> Lines { get; }

		/// <summary>
		/// Should this dialogue be added to the pool of dialogues that can be repeated on future visits?
		/// Can be set manually or auto-populated from loc keys via the "r" suffix (e.g., "1-0r.ancient").
		/// </summary>
		// Token: 0x17002460 RID: 9312
		// (get) Token: 0x06008517 RID: 34071 RVA: 0x002CB90D File Offset: 0x002C9B0D
		// (set) Token: 0x06008518 RID: 34072 RVA: 0x002CB915 File Offset: 0x002C9B15
		public bool IsRepeating { get; set; }

		/// <summary>
		/// What visit should this dialogue be shown on?
		/// This can be combined with <see cref="P:MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogue.IsRepeating" /> to show a dialogue on the Nth visit, and then add it to the
		/// pool of repeating dialogues for visits after that.
		/// </summary>
		/// <example>
		/// 0: Show this dialogue on this character's first visit to this ancient.
		/// 1: Show this dialogue on this character's second visit to this ancient.
		/// 4: Show this dialogue on this character's fifth visit to this ancient.
		/// null: Show this dialogue on any visit to this ancient.
		/// </example>
		// Token: 0x17002461 RID: 9313
		// (get) Token: 0x06008519 RID: 34073 RVA: 0x002CB91E File Offset: 0x002C9B1E
		// (set) Token: 0x0600851A RID: 34074 RVA: 0x002CB926 File Offset: 0x002C9B26
		public int? VisitIndex { get; set; }

		/// <summary>
		/// For Architect dialogues, who should attack at the start?
		/// </summary>
		// Token: 0x17002462 RID: 9314
		// (get) Token: 0x0600851B RID: 34075 RVA: 0x002CB92F File Offset: 0x002C9B2F
		// (set) Token: 0x0600851C RID: 34076 RVA: 0x002CB937 File Offset: 0x002C9B37
		public ArchitectAttackers StartAttackers { get; set; }

		/// <summary>
		/// For Architect dialogues, who should attack at the end?
		/// </summary>
		// Token: 0x17002463 RID: 9315
		// (get) Token: 0x0600851D RID: 34077 RVA: 0x002CB940 File Offset: 0x002C9B40
		// (set) Token: 0x0600851E RID: 34078 RVA: 0x002CB948 File Offset: 0x002C9B48
		public ArchitectAttackers EndAttackers { get; set; }

		/// <summary>
		/// Create a new dialogue.
		/// The number of SFX you pass controls how many lines this dialogue has.
		/// </summary>
		/// <param name="sfxPaths">
		/// The paths to the SFX that should be played when this dialogue is shown.
		/// Each SFX corresponds to a line in the dialogue.
		/// Passing an empty string for a given line indicates that no dialogue should be played for it, but you must pass
		/// some string for every line.
		/// For example, if you have a single-line dialogue with no SFX, call `new AncientDialogue("")`.
		/// If you called `new AncientDialogue()`, you'd create a dialogue with 0 lines.
		/// </param>
		// Token: 0x0600851F RID: 34079 RVA: 0x002CB954 File Offset: 0x002C9B54
		public AncientDialogue(params string[] sfxPaths)
		{
			if (sfxPaths.Length == 0)
			{
				throw new ArgumentException("Requires at least 1 SFX path", "sfxPaths");
			}
			this.Lines = sfxPaths.Select<string, AncientDialogueLine>((string sfx) => new AncientDialogueLine(sfx)).ToList<AncientDialogueLine>();
		}

		/// <summary>
		/// Populate this dialogue's lines with the correct loc keys.
		/// IsRepeating is derived from the presence of the "r" suffix in the first line's loc key.
		/// Speaker is derived from which key suffix exists in the JSON: ".ancient" or ".char".
		/// </summary>
		/// <param name="ancientEntry">The ID entry for the ancient that this line is for.</param>
		/// <param name="charEntry">
		/// The ID entry for the character that this line is for.
		/// Can also be "ANY" for character-agnostic dialogues, or "firstVisitEver" for the dialogue that should be shown the
		/// first time you ever visit this ancient (regardless of character).
		/// </param>
		/// <param name="dialogueIndex">
		/// The index of this dialogue in the list of dialogues for this ancient and character.
		/// </param>
		// Token: 0x06008520 RID: 34080 RVA: 0x002CB9AC File Offset: 0x002C9BAC
		public void PopulateLines(string ancientEntry, string charEntry, int dialogueIndex)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 3);
			defaultInterpolatedStringHandler.AppendFormatted(ancientEntry);
			defaultInterpolatedStringHandler.AppendLiteral(".talk.");
			defaultInterpolatedStringHandler.AppendFormatted(charEntry);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<int>(dialogueIndex);
			defaultInterpolatedStringHandler.AppendLiteral("-0");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			bool flag = AncientDialogue.HasRepeatingSuffix(text);
			if (flag)
			{
				this.IsRepeating = true;
			}
			string text2 = (flag ? "r" : "");
			for (int i = 0; i < this.Lines.Count; i++)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 4);
				defaultInterpolatedStringHandler.AppendFormatted(ancientEntry);
				defaultInterpolatedStringHandler.AppendLiteral(".talk.");
				defaultInterpolatedStringHandler.AppendFormatted(charEntry);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				defaultInterpolatedStringHandler.AppendFormatted<int>(dialogueIndex);
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				string text3 = defaultInterpolatedStringHandler.ToStringAndClear();
				bool flag2 = AncientDialogue.HasRepeatingSuffix(text3);
				if (flag && !flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Dialogue ");
					defaultInterpolatedStringHandler.AppendFormatted(ancientEntry);
					defaultInterpolatedStringHandler.AppendLiteral(".talk.");
					defaultInterpolatedStringHandler.AppendFormatted(charEntry);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					defaultInterpolatedStringHandler.AppendFormatted<int>(dialogueIndex);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendLiteral("line 0 has 'r' suffix but line ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" does not.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (!flag && flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Dialogue ");
					defaultInterpolatedStringHandler.AppendFormatted(ancientEntry);
					defaultInterpolatedStringHandler.AppendLiteral(".talk.");
					defaultInterpolatedStringHandler.AppendFormatted(charEntry);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					defaultInterpolatedStringHandler.AppendFormatted<int>(dialogueIndex);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendLiteral("line 0 has no 'r' suffix but line ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" does.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string text4 = text3 + text2;
				string text5 = text4 + ".ancient";
				string text6 = text4 + ".char";
				if (LocString.Exists("ancients", text5))
				{
					this.Lines[i].LineText = new LocString("ancients", text5);
					this.Lines[i].Speaker = AncientDialogueSpeaker.Ancient;
				}
				else
				{
					this.Lines[i].LineText = new LocString("ancients", text6);
					this.Lines[i].Speaker = AncientDialogueSpeaker.Character;
				}
			}
		}

		// Token: 0x06008521 RID: 34081 RVA: 0x002CBC59 File Offset: 0x002C9E59
		private static bool HasRepeatingSuffix(string baseKey)
		{
			return LocString.Exists("ancients", baseKey + "r.ancient") || LocString.Exists("ancients", baseKey + "r.char");
		}

		// Token: 0x04002E27 RID: 11815
		private const string _locTable = "ancients";
	}
}
