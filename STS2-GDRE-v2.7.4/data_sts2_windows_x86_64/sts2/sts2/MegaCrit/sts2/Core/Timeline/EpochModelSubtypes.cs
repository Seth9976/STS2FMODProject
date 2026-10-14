using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Timeline
{
	// Token: 0x0200003D RID: 61
	public static class EpochModelSubtypes
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00012C70 File Offset: 0x00010E70
		public static int Count
		{
			get
			{
				return 57;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00012C74 File Offset: 0x00010E74
		public static IReadOnlyList<Type> All
		{
			get
			{
				return EpochModelSubtypes._subtypes;
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00012C7B File Offset: 0x00010E7B
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		public static Type Get(int i)
		{
			return EpochModelSubtypes._subtypes[i];
		}

		// Token: 0x04000113 RID: 275
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t0 = typeof(Act2BEpoch);

		// Token: 0x04000114 RID: 276
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t1 = typeof(Act3BEpoch);

		// Token: 0x04000115 RID: 277
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t2 = typeof(Colorless1Epoch);

		// Token: 0x04000116 RID: 278
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t3 = typeof(Colorless2Epoch);

		// Token: 0x04000117 RID: 279
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t4 = typeof(Colorless3Epoch);

		// Token: 0x04000118 RID: 280
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t5 = typeof(Colorless4Epoch);

		// Token: 0x04000119 RID: 281
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t6 = typeof(Colorless5Epoch);

		// Token: 0x0400011A RID: 282
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t7 = typeof(CustomAndSeedsEpoch);

		// Token: 0x0400011B RID: 283
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t8 = typeof(DailyRunEpoch);

		// Token: 0x0400011C RID: 284
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t9 = typeof(DarvEpoch);

		// Token: 0x0400011D RID: 285
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t10 = typeof(Defect1Epoch);

		// Token: 0x0400011E RID: 286
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t11 = typeof(Defect2Epoch);

		// Token: 0x0400011F RID: 287
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t12 = typeof(Defect3Epoch);

		// Token: 0x04000120 RID: 288
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t13 = typeof(Defect4Epoch);

		// Token: 0x04000121 RID: 289
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t14 = typeof(Defect5Epoch);

		// Token: 0x04000122 RID: 290
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t15 = typeof(Defect6Epoch);

		// Token: 0x04000123 RID: 291
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t16 = typeof(Defect7Epoch);

		// Token: 0x04000124 RID: 292
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t17 = typeof(Event1Epoch);

		// Token: 0x04000125 RID: 293
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t18 = typeof(Event2Epoch);

		// Token: 0x04000126 RID: 294
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t19 = typeof(Event3Epoch);

		// Token: 0x04000127 RID: 295
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t20 = typeof(Ironclad2Epoch);

		// Token: 0x04000128 RID: 296
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t21 = typeof(Ironclad3Epoch);

		// Token: 0x04000129 RID: 297
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t22 = typeof(Ironclad4Epoch);

		// Token: 0x0400012A RID: 298
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t23 = typeof(Ironclad5Epoch);

		// Token: 0x0400012B RID: 299
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t24 = typeof(Ironclad6Epoch);

		// Token: 0x0400012C RID: 300
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t25 = typeof(Ironclad7Epoch);

		// Token: 0x0400012D RID: 301
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t26 = typeof(Necrobinder1Epoch);

		// Token: 0x0400012E RID: 302
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t27 = typeof(Necrobinder2Epoch);

		// Token: 0x0400012F RID: 303
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t28 = typeof(Necrobinder3Epoch);

		// Token: 0x04000130 RID: 304
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t29 = typeof(Necrobinder4Epoch);

		// Token: 0x04000131 RID: 305
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t30 = typeof(Necrobinder5Epoch);

		// Token: 0x04000132 RID: 306
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t31 = typeof(Necrobinder6Epoch);

		// Token: 0x04000133 RID: 307
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t32 = typeof(Necrobinder7Epoch);

		// Token: 0x04000134 RID: 308
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t33 = typeof(NeowEpoch);

		// Token: 0x04000135 RID: 309
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t34 = typeof(OrobasEpoch);

		// Token: 0x04000136 RID: 310
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t35 = typeof(Potion1Epoch);

		// Token: 0x04000137 RID: 311
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t36 = typeof(Potion2Epoch);

		// Token: 0x04000138 RID: 312
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t37 = typeof(Regent1Epoch);

		// Token: 0x04000139 RID: 313
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t38 = typeof(Regent2Epoch);

		// Token: 0x0400013A RID: 314
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t39 = typeof(Regent3Epoch);

		// Token: 0x0400013B RID: 315
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t40 = typeof(Regent4Epoch);

		// Token: 0x0400013C RID: 316
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t41 = typeof(Regent5Epoch);

		// Token: 0x0400013D RID: 317
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t42 = typeof(Regent6Epoch);

		// Token: 0x0400013E RID: 318
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t43 = typeof(Regent7Epoch);

		// Token: 0x0400013F RID: 319
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t44 = typeof(Relic1Epoch);

		// Token: 0x04000140 RID: 320
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t45 = typeof(Relic2Epoch);

		// Token: 0x04000141 RID: 321
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t46 = typeof(Relic3Epoch);

		// Token: 0x04000142 RID: 322
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t47 = typeof(Relic4Epoch);

		// Token: 0x04000143 RID: 323
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t48 = typeof(Relic5Epoch);

		// Token: 0x04000144 RID: 324
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t49 = typeof(Silent1Epoch);

		// Token: 0x04000145 RID: 325
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t50 = typeof(Silent2Epoch);

		// Token: 0x04000146 RID: 326
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t51 = typeof(Silent3Epoch);

		// Token: 0x04000147 RID: 327
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t52 = typeof(Silent4Epoch);

		// Token: 0x04000148 RID: 328
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t53 = typeof(Silent5Epoch);

		// Token: 0x04000149 RID: 329
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t54 = typeof(Silent6Epoch);

		// Token: 0x0400014A RID: 330
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t55 = typeof(Silent7Epoch);

		// Token: 0x0400014B RID: 331
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t56 = typeof(UnderdocksEpoch);

		// Token: 0x0400014C RID: 332
		private static readonly Type[] _subtypes = new Type[]
		{
			EpochModelSubtypes._t0,
			EpochModelSubtypes._t1,
			EpochModelSubtypes._t2,
			EpochModelSubtypes._t3,
			EpochModelSubtypes._t4,
			EpochModelSubtypes._t5,
			EpochModelSubtypes._t6,
			EpochModelSubtypes._t7,
			EpochModelSubtypes._t8,
			EpochModelSubtypes._t9,
			EpochModelSubtypes._t10,
			EpochModelSubtypes._t11,
			EpochModelSubtypes._t12,
			EpochModelSubtypes._t13,
			EpochModelSubtypes._t14,
			EpochModelSubtypes._t15,
			EpochModelSubtypes._t16,
			EpochModelSubtypes._t17,
			EpochModelSubtypes._t18,
			EpochModelSubtypes._t19,
			EpochModelSubtypes._t20,
			EpochModelSubtypes._t21,
			EpochModelSubtypes._t22,
			EpochModelSubtypes._t23,
			EpochModelSubtypes._t24,
			EpochModelSubtypes._t25,
			EpochModelSubtypes._t26,
			EpochModelSubtypes._t27,
			EpochModelSubtypes._t28,
			EpochModelSubtypes._t29,
			EpochModelSubtypes._t30,
			EpochModelSubtypes._t31,
			EpochModelSubtypes._t32,
			EpochModelSubtypes._t33,
			EpochModelSubtypes._t34,
			EpochModelSubtypes._t35,
			EpochModelSubtypes._t36,
			EpochModelSubtypes._t37,
			EpochModelSubtypes._t38,
			EpochModelSubtypes._t39,
			EpochModelSubtypes._t40,
			EpochModelSubtypes._t41,
			EpochModelSubtypes._t42,
			EpochModelSubtypes._t43,
			EpochModelSubtypes._t44,
			EpochModelSubtypes._t45,
			EpochModelSubtypes._t46,
			EpochModelSubtypes._t47,
			EpochModelSubtypes._t48,
			EpochModelSubtypes._t49,
			EpochModelSubtypes._t50,
			EpochModelSubtypes._t51,
			EpochModelSubtypes._t52,
			EpochModelSubtypes._t53,
			EpochModelSubtypes._t54,
			EpochModelSubtypes._t55,
			EpochModelSubtypes._t56
		};
	}
}
