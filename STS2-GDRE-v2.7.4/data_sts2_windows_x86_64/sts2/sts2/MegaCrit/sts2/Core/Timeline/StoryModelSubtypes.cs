using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Timeline.Stories;

namespace MegaCrit.Sts2.Core.Timeline
{
	// Token: 0x0200003E RID: 62
	public static class StoryModelSubtypes
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600027D RID: 637 RVA: 0x000131EC File Offset: 0x000113EC
		public static int Count
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600027E RID: 638 RVA: 0x000131EF File Offset: 0x000113EF
		public static IReadOnlyList<Type> All
		{
			get
			{
				return StoryModelSubtypes._subtypes;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000131F6 File Offset: 0x000113F6
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		public static Type Get(int i)
		{
			return StoryModelSubtypes._subtypes[i];
		}

		// Token: 0x0400014D RID: 333
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t0 = typeof(DefectStory);

		// Token: 0x0400014E RID: 334
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t1 = typeof(IroncladStory);

		// Token: 0x0400014F RID: 335
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t2 = typeof(MagnumOpusStory);

		// Token: 0x04000150 RID: 336
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t3 = typeof(NecrobinderStory);

		// Token: 0x04000151 RID: 337
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t4 = typeof(RegentStory);

		// Token: 0x04000152 RID: 338
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t5 = typeof(SilentStory);

		// Token: 0x04000153 RID: 339
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t6 = typeof(TalesFromTheSpireStory);

		// Token: 0x04000154 RID: 340
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
		private static readonly Type _t7 = typeof(TheReopeningStory);

		// Token: 0x04000155 RID: 341
		private static readonly Type[] _subtypes = new Type[]
		{
			StoryModelSubtypes._t0,
			StoryModelSubtypes._t1,
			StoryModelSubtypes._t2,
			StoryModelSubtypes._t3,
			StoryModelSubtypes._t4,
			StoryModelSubtypes._t5,
			StoryModelSubtypes._t6,
			StoryModelSubtypes._t7
		};
	}
}
