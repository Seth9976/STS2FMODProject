using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.TestSupport
{
	// Token: 0x02000087 RID: 135
	public static class TestRngInjector
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x00015C72 File Offset: 0x00013E72
		public static void SetRelicOverride<T>() where T : RelicModel
		{
			TestRngInjector._relicOverride = ModelDb.Relic<T>();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00015C84 File Offset: 0x00013E84
		[NullableContext(2)]
		public static RelicModel ConsumeRelicOverride()
		{
			RelicModel relicOverride = TestRngInjector._relicOverride;
			TestRngInjector._relicOverride = null;
			return relicOverride;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00015C9E File Offset: 0x00013E9E
		public static void SetRelicRarityOverride(RelicRarity relicRarity)
		{
			TestRngInjector._relicRarityOverride = new RelicRarity?(relicRarity);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00015CAB File Offset: 0x00013EAB
		public static RelicRarity? GetRelicRarityOverride()
		{
			return TestRngInjector._relicRarityOverride;
		}

		/// <summary>
		/// Force specific cards to show up for CardFactory.GetDistinctForCombat.
		/// </summary>
		// Token: 0x0600046F RID: 1135 RVA: 0x00015CB2 File Offset: 0x00013EB2
		[NullableContext(1)]
		public static void SetCombatCardGenerationOverride(List<CardModel> cards)
		{
			TestRngInjector._combatCardGenerationOverride = cards;
		}

		/// <summary>
		/// Consume the combat cards generation override and clear it out.
		/// </summary>
		// Token: 0x06000470 RID: 1136 RVA: 0x00015CBC File Offset: 0x00013EBC
		[return: Nullable(new byte[] { 2, 1 })]
		public static List<CardModel> ConsumeCombatCardGenerationOverride()
		{
			List<CardModel> combatCardGenerationOverride = TestRngInjector._combatCardGenerationOverride;
			TestRngInjector._combatCardGenerationOverride = null;
			return combatCardGenerationOverride;
		}

		/// <summary>
		/// Force a specific order for the initial shuffle.
		/// </summary>
		// Token: 0x06000471 RID: 1137 RVA: 0x00015CD6 File Offset: 0x00013ED6
		[NullableContext(1)]
		public static void SetInitialShuffleOverride(Action<List<CardModel>> reorder)
		{
			TestRngInjector._initialShuffleOverride = reorder;
		}

		/// <summary>
		/// Consume the initial shuffle override and clear it out.
		/// </summary>
		// Token: 0x06000472 RID: 1138 RVA: 0x00015CE0 File Offset: 0x00013EE0
		[return: Nullable(new byte[] { 2, 1, 1 })]
		public static Action<List<CardModel>> ConsumeInitialShuffleOverride()
		{
			Action<List<CardModel>> initialShuffleOverride = TestRngInjector._initialShuffleOverride;
			TestRngInjector._initialShuffleOverride = null;
			return initialShuffleOverride;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00015CFA File Offset: 0x00013EFA
		public static void Cleanup()
		{
			TestRngInjector._relicOverride = null;
			TestRngInjector._relicRarityOverride = null;
			TestRngInjector._initialShuffleOverride = null;
			TestRngInjector._combatCardGenerationOverride = null;
		}

		// Token: 0x04000163 RID: 355
		[Nullable(2)]
		private static RelicModel _relicOverride;

		// Token: 0x04000164 RID: 356
		private static RelicRarity? _relicRarityOverride;

		// Token: 0x04000165 RID: 357
		[Nullable(new byte[] { 2, 1, 1 })]
		private static Action<List<CardModel>> _initialShuffleOverride;

		// Token: 0x04000166 RID: 358
		[Nullable(new byte[] { 2, 1 })]
		private static List<CardModel> _combatCardGenerationOverride;
	}
}
