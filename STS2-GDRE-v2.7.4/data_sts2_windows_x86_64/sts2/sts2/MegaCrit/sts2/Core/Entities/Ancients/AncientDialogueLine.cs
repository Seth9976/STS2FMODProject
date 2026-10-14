using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Entities.Ancients
{
	// Token: 0x02000CD1 RID: 3281
	[NullableContext(2)]
	[Nullable(0)]
	public class AncientDialogueLine
	{
		/// <summary>
		/// The text that should be shown for this line of dialogue.
		/// </summary>
		// Token: 0x17002464 RID: 9316
		// (get) Token: 0x06008522 RID: 34082 RVA: 0x002CBC89 File Offset: 0x002C9E89
		// (set) Token: 0x06008523 RID: 34083 RVA: 0x002CBC91 File Offset: 0x002C9E91
		public LocString LineText { get; set; }

		/// <summary>
		/// The text that should be shown on the "Next" button that goes with this line of dialogue.
		/// Null for the last line in a dialogue.
		/// </summary>
		// Token: 0x17002465 RID: 9317
		// (get) Token: 0x06008524 RID: 34084 RVA: 0x002CBC9A File Offset: 0x002C9E9A
		// (set) Token: 0x06008525 RID: 34085 RVA: 0x002CBCA2 File Offset: 0x002C9EA2
		public LocString NextButtonText { get; set; }

		/// <summary>
		/// The speaker of this line of dialogue.
		/// </summary>
		// Token: 0x17002466 RID: 9318
		// (get) Token: 0x06008526 RID: 34086 RVA: 0x002CBCAB File Offset: 0x002C9EAB
		// (set) Token: 0x06008527 RID: 34087 RVA: 0x002CBCB3 File Offset: 0x002C9EB3
		public AncientDialogueSpeaker Speaker { get; set; }

		// Token: 0x06008528 RID: 34088 RVA: 0x002CBCBC File Offset: 0x002C9EBC
		[NullableContext(1)]
		public AncientDialogueLine(string sfxPath)
		{
			this._sfxPath = sfxPath;
		}

		/// <summary>
		/// Get the path to the SFX to play for this line of dialogue.
		/// Returns a fallback if the SFX path is empty.
		/// </summary>
		// Token: 0x06008529 RID: 34089 RVA: 0x002CBCCB File Offset: 0x002C9ECB
		[NullableContext(1)]
		public string GetSfxOrFallbackPath()
		{
			if (!string.IsNullOrEmpty(this._sfxPath))
			{
				return this._sfxPath;
			}
			return "event:/sfx/ui/enchant_simple";
		}

		// Token: 0x04002E2D RID: 11821
		[Nullable(1)]
		public const string sfxFallbackPath = "event:/sfx/ui/enchant_simple";

		/// <summary>
		/// The path to the SFX that should be played when this line of dialogue is shown.
		/// Empty string means no SFX.
		/// </summary>
		// Token: 0x04002E31 RID: 11825
		[Nullable(1)]
		private readonly string _sfxPath;
	}
}
