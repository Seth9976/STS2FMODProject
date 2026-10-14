using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000817 RID: 2071
	[NullableContext(1)]
	[Nullable(0)]
	public class DeadlyEvents : ModifierModel
	{
		// Token: 0x06006733 RID: 26419 RVA: 0x00271F5C File Offset: 0x0027015C
		protected override void AfterRunCreated(RunState runState)
		{
			foreach (Player player in runState.Players)
			{
				player.RelicGrabBag.Remove<JuzuBracelet>();
			}
			runState.SharedRelicGrabBag.Remove<JuzuBracelet>();
			runState.Odds.UnknownMapPoint.EliteOdds = 0.1f;
			runState.Odds.UnknownMapPoint.SetBaseOdds(RoomType.Elite, 0.1f);
		}

		// Token: 0x06006734 RID: 26420 RVA: 0x00271FE4 File Offset: 0x002701E4
		protected override void AfterRunLoaded(RunState runState)
		{
			runState.Odds.UnknownMapPoint.SetBaseOdds(RoomType.Elite, 0.1f);
		}

		// Token: 0x06006735 RID: 26421 RVA: 0x00271FFC File Offset: 0x002701FC
		public override float ModifyOddsIncreaseForUnrolledRoomType(RoomType roomType, float oddsIncrease)
		{
			if (roomType != RoomType.Treasure)
			{
				return oddsIncrease;
			}
			return oddsIncrease * 2f;
		}
	}
}
