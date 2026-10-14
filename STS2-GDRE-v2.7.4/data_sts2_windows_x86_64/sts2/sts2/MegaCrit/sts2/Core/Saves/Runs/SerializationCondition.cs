using System;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000D4 RID: 212
	public enum SerializationCondition
	{
		/// <summary>
		/// Property is always serialized and deserialized no matter what.
		/// </summary>
		// Token: 0x04000387 RID: 903
		AlwaysSave,
		/// <summary>
		/// Property is only serialized and deserialized if Object.Equals returns false when comparing the current value of
		/// the property and the default value of the property when the declaring type's default constructor is called.
		/// </summary>
		// Token: 0x04000388 RID: 904
		SaveIfNotPropertyDefault,
		/// <summary>
		/// Property is only serialized and deserialized if Object.Equals returns false when comparing the current value of
		/// the property and default(T), where T is the property of the type.
		/// </summary>
		// Token: 0x04000389 RID: 905
		SaveIfNotTypeDefault,
		/// <summary>
		/// Property is only serialized and deserialized if it is a collection that is not empty, or if it is not null.
		/// </summary>
		// Token: 0x0400038A RID: 906
		SaveIfNotCollectionEmptyOrNull
	}
}
