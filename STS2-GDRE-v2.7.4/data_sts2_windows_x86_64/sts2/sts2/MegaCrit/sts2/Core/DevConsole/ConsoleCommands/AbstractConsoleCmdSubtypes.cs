using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000D06 RID: 3334
	public static class AbstractConsoleCmdSubtypes
	{
		// Token: 0x17002523 RID: 9507
		// (get) Token: 0x060086A7 RID: 34471 RVA: 0x002D2D4B File Offset: 0x002D0F4B
		public static int Count
		{
			get
			{
				return 39;
			}
		}

		// Token: 0x17002524 RID: 9508
		// (get) Token: 0x060086A8 RID: 34472 RVA: 0x002D2D4F File Offset: 0x002D0F4F
		public static IReadOnlyList<Type> All
		{
			get
			{
				return AbstractConsoleCmdSubtypes._subtypes;
			}
		}

		// Token: 0x060086A9 RID: 34473 RVA: 0x002D2D56 File Offset: 0x002D0F56
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		public static Type Get(int i)
		{
			return AbstractConsoleCmdSubtypes._subtypes[i];
		}

		// Token: 0x04002E70 RID: 11888
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t0 = typeof(AchievementConsoleCmd);

		// Token: 0x04002E71 RID: 11889
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t1 = typeof(ActConsoleCmd);

		// Token: 0x04002E72 RID: 11890
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t2 = typeof(AfflictConsoleCmd);

		// Token: 0x04002E73 RID: 11891
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t3 = typeof(AncientConsoleCmd);

		// Token: 0x04002E74 RID: 11892
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t4 = typeof(ApplyPowerConsoleCmd);

		// Token: 0x04002E75 RID: 11893
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t5 = typeof(ArtConsoleCmd);

		// Token: 0x04002E76 RID: 11894
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t6 = typeof(BestiaryConsoleCmd);

		// Token: 0x04002E77 RID: 11895
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t7 = typeof(BlockConsoleCmd);

		// Token: 0x04002E78 RID: 11896
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t8 = typeof(CardConsoleCmd);

		// Token: 0x04002E79 RID: 11897
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t9 = typeof(CloudConsoleCmd);

		// Token: 0x04002E7A RID: 11898
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t10 = typeof(DamageConsoleCmd);

		// Token: 0x04002E7B RID: 11899
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t11 = typeof(DieConsoleCmd);

		// Token: 0x04002E7C RID: 11900
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t12 = typeof(DrawConsoleCmd);

		// Token: 0x04002E7D RID: 11901
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t13 = typeof(DumpConsoleCmd);

		// Token: 0x04002E7E RID: 11902
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t14 = typeof(EnchantConsoleCmd);

		// Token: 0x04002E7F RID: 11903
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t15 = typeof(EnergyConsoleCmd);

		// Token: 0x04002E80 RID: 11904
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t16 = typeof(EventConsoleCmd);

		// Token: 0x04002E81 RID: 11905
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t17 = typeof(FightConsoleCmd);

		// Token: 0x04002E82 RID: 11906
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t18 = typeof(GetLogsConsoleCmd);

		// Token: 0x04002E83 RID: 11907
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t19 = typeof(GodModeConsoleCmd);

		// Token: 0x04002E84 RID: 11908
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t20 = typeof(GoldConsoleCmd);

		// Token: 0x04002E85 RID: 11909
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t21 = typeof(HealConsoleCmd);

		// Token: 0x04002E86 RID: 11910
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t22 = typeof(InstantConsoleCmd);

		// Token: 0x04002E87 RID: 11911
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t23 = typeof(KillConsoleCmd);

		// Token: 0x04002E88 RID: 11912
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t24 = typeof(LeaderboardConsoleCmd);

		// Token: 0x04002E89 RID: 11913
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t25 = typeof(LogConsoleCmd);

		// Token: 0x04002E8A RID: 11914
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t26 = typeof(MultiplayerConsoleCmd);

		// Token: 0x04002E8B RID: 11915
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t27 = typeof(OpenConsoleCmd);

		// Token: 0x04002E8C RID: 11916
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t28 = typeof(PotionConsoleCmd);

		// Token: 0x04002E8D RID: 11917
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t29 = typeof(RelicConsoleCmd);

		// Token: 0x04002E8E RID: 11918
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t30 = typeof(RemoveCardConsoleCmd);

		// Token: 0x04002E8F RID: 11919
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t31 = typeof(RoomConsoleCmd);

		// Token: 0x04002E90 RID: 11920
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t32 = typeof(SentryConsoleCmd);

		// Token: 0x04002E91 RID: 11921
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t33 = typeof(StarsConsoleCmd);

		// Token: 0x04002E92 RID: 11922
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t34 = typeof(TrailerConsoleCmd);

		// Token: 0x04002E93 RID: 11923
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t35 = typeof(TravelConsoleCmd);

		// Token: 0x04002E94 RID: 11924
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t36 = typeof(UnlockConsoleCmd);

		// Token: 0x04002E95 RID: 11925
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t37 = typeof(UpgradeCardConsoleCmd);

		// Token: 0x04002E96 RID: 11926
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static readonly Type _t38 = typeof(WinConsoleCmd);

		// Token: 0x04002E97 RID: 11927
		private static readonly Type[] _subtypes = new Type[]
		{
			AbstractConsoleCmdSubtypes._t0,
			AbstractConsoleCmdSubtypes._t1,
			AbstractConsoleCmdSubtypes._t2,
			AbstractConsoleCmdSubtypes._t3,
			AbstractConsoleCmdSubtypes._t4,
			AbstractConsoleCmdSubtypes._t5,
			AbstractConsoleCmdSubtypes._t6,
			AbstractConsoleCmdSubtypes._t7,
			AbstractConsoleCmdSubtypes._t8,
			AbstractConsoleCmdSubtypes._t9,
			AbstractConsoleCmdSubtypes._t10,
			AbstractConsoleCmdSubtypes._t11,
			AbstractConsoleCmdSubtypes._t12,
			AbstractConsoleCmdSubtypes._t13,
			AbstractConsoleCmdSubtypes._t14,
			AbstractConsoleCmdSubtypes._t15,
			AbstractConsoleCmdSubtypes._t16,
			AbstractConsoleCmdSubtypes._t17,
			AbstractConsoleCmdSubtypes._t18,
			AbstractConsoleCmdSubtypes._t19,
			AbstractConsoleCmdSubtypes._t20,
			AbstractConsoleCmdSubtypes._t21,
			AbstractConsoleCmdSubtypes._t22,
			AbstractConsoleCmdSubtypes._t23,
			AbstractConsoleCmdSubtypes._t24,
			AbstractConsoleCmdSubtypes._t25,
			AbstractConsoleCmdSubtypes._t26,
			AbstractConsoleCmdSubtypes._t27,
			AbstractConsoleCmdSubtypes._t28,
			AbstractConsoleCmdSubtypes._t29,
			AbstractConsoleCmdSubtypes._t30,
			AbstractConsoleCmdSubtypes._t31,
			AbstractConsoleCmdSubtypes._t32,
			AbstractConsoleCmdSubtypes._t33,
			AbstractConsoleCmdSubtypes._t34,
			AbstractConsoleCmdSubtypes._t35,
			AbstractConsoleCmdSubtypes._t36,
			AbstractConsoleCmdSubtypes._t37,
			AbstractConsoleCmdSubtypes._t38
		};
	}
}
