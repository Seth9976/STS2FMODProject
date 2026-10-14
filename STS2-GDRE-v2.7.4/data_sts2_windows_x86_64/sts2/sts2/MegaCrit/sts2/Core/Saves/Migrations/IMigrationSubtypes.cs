using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Saves.Migrations.PrefsSaves;
using MegaCrit.Sts2.Core.Saves.Migrations.ProfileSaves;
using MegaCrit.Sts2.Core.Saves.Migrations.ProgressSaves;
using MegaCrit.Sts2.Core.Saves.Migrations.RunHistories;
using MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;
using MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	// Token: 0x020000E2 RID: 226
	public static class IMigrationSubtypes
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00047B3B File Offset: 0x00045D3B
		public static int Count
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00047B3F File Offset: 0x00045D3F
		public static IReadOnlyList<Type> All
		{
			get
			{
				return IMigrationSubtypes._subtypes;
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00047B46 File Offset: 0x00045D46
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		public static Type Get(int i)
		{
			return IMigrationSubtypes._subtypes[i];
		}

		// Token: 0x04000396 RID: 918
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t0 = typeof(PrefsSaveV1ToV2);

		// Token: 0x04000397 RID: 919
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t1 = typeof(ProfileSaveV1ToV2);

		// Token: 0x04000398 RID: 920
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t2 = typeof(ProgressSaveV20ToV21);

		// Token: 0x04000399 RID: 921
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t3 = typeof(ProgressSaveV21ToV22);

		// Token: 0x0400039A RID: 922
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t4 = typeof(RunHistoryV7ToV8);

		// Token: 0x0400039B RID: 923
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t5 = typeof(RunHistoryV8ToV9);

		// Token: 0x0400039C RID: 924
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t6 = typeof(SerializableRunV12ToV13);

		// Token: 0x0400039D RID: 925
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t7 = typeof(SerializableRunV13ToV14);

		// Token: 0x0400039E RID: 926
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t8 = typeof(SerializableRunV14ToV15);

		// Token: 0x0400039F RID: 927
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t9 = typeof(SerializableRunV15ToV16);

		// Token: 0x040003A0 RID: 928
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t10 = typeof(SerializableRunV16ToV17);

		// Token: 0x040003A1 RID: 929
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t11 = typeof(SerializableRunV17ToV18);

		// Token: 0x040003A2 RID: 930
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t12 = typeof(SerializableRunV18ToV19);

		// Token: 0x040003A3 RID: 931
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t13 = typeof(SettingsSaveV3ToV4);

		// Token: 0x040003A4 RID: 932
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t14 = typeof(SettingsSaveV4ToV5);

		// Token: 0x040003A5 RID: 933
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t15 = typeof(SettingsSaveV5ToV6);

		// Token: 0x040003A6 RID: 934
		private static readonly Type[] _subtypes = new Type[]
		{
			IMigrationSubtypes._t0,
			IMigrationSubtypes._t1,
			IMigrationSubtypes._t2,
			IMigrationSubtypes._t3,
			IMigrationSubtypes._t4,
			IMigrationSubtypes._t5,
			IMigrationSubtypes._t6,
			IMigrationSubtypes._t7,
			IMigrationSubtypes._t8,
			IMigrationSubtypes._t9,
			IMigrationSubtypes._t10,
			IMigrationSubtypes._t11,
			IMigrationSubtypes._t12,
			IMigrationSubtypes._t13,
			IMigrationSubtypes._t14,
			IMigrationSubtypes._t15
		};
	}
}
