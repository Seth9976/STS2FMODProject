using System;

namespace MegaCrit.Sts2.Core.Entities.Ancients
{
	// Token: 0x02000CD3 RID: 3283
	public enum AncientDialogueSpeaker
	{
		/// <summary>
		/// No speaker. This should never be set, it just gives us a neutral default.
		/// </summary>
		// Token: 0x04002E36 RID: 11830
		None,
		/// <summary>
		/// The Ancient (Neow, Darv, etc.) is speaking.
		/// </summary>
		// Token: 0x04002E37 RID: 11831
		Ancient,
		/// <summary>
		/// The player character (Ironclad, Silent, etc.) is speaking.
		/// </summary>
		// Token: 0x04002E38 RID: 11832
		Character
	}
}
