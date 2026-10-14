using System;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000C1 RID: 193
	[AttributeUsage(AttributeTargets.Property)]
	public class SavedPropertyAttribute : Attribute
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x00043B64 File Offset: 0x00041D64
		public SavedPropertyAttribute()
		{
			this.defaultBehaviour = SerializationCondition.AlwaysSave;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00043B73 File Offset: 0x00041D73
		public SavedPropertyAttribute(SerializationCondition defaultBehaviour)
		{
			this.defaultBehaviour = defaultBehaviour;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00043B82 File Offset: 0x00041D82
		public SavedPropertyAttribute(SerializationCondition defaultBehaviour, int order)
		{
			this.defaultBehaviour = defaultBehaviour;
			this.order = order;
		}

		// Token: 0x04000327 RID: 807
		public readonly SerializationCondition defaultBehaviour;

		/// <summary>
		/// This controls the order in which properties are serialized and deserialized.
		/// It is very rarely necessary, but sometimes properties depend on each other.
		/// </summary>
		// Token: 0x04000328 RID: 808
		public readonly int order;
	}
}
