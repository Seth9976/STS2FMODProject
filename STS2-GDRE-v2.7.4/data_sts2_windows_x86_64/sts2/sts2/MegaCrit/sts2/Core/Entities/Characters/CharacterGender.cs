using System;

namespace MegaCrit.Sts2.Core.Entities.Characters
{
	/// <summary>
	/// Represents a character's gender for grammatical purposes.
	/// These do not necessarily align with the character's gender identity.
	/// For example, in French, a nonbinary character may want to use any of these options, but there is no grammatical
	/// gender specifically for "nonbinary" in French.
	/// </summary>
	// Token: 0x02000CA6 RID: 3238
	public enum CharacterGender
	{
		// Token: 0x04002D69 RID: 11625
		Neutral,
		// Token: 0x04002D6A RID: 11626
		Feminine,
		// Token: 0x04002D6B RID: 11627
		Masculine
	}
}
