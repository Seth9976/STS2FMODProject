using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Audio.Debug
{
	// Token: 0x02000D8C RID: 3468
	[NullableContext(1)]
	[Nullable(0)]
	public static class TmpSfx
	{
		// Token: 0x17002654 RID: 9812
		// (get) Token: 0x06008B78 RID: 35704 RVA: 0x002E8D7A File Offset: 0x002E6F7A
		public static IEnumerable<string> BattleStart
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "battle_start_1.mp3", "battle_start_2.mp3" });
			}
		}

		// Token: 0x17002655 RID: 9813
		// (get) Token: 0x06008B79 RID: 35705 RVA: 0x002E8D97 File Offset: 0x002E6F97
		public static IEnumerable<string> PotionSlosh
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "potion_slosh_1.mp3", "potion_slosh_2.mp3", "potion_slosh_3.mp3" });
			}
		}

		// Token: 0x06008B7A RID: 35706 RVA: 0x002E8DBC File Offset: 0x002E6FBC
		public static string GetPath(string sfx)
		{
			return "res://debug_audio/" + sfx;
		}

		// Token: 0x04003089 RID: 12425
		public const string gameOver = "event:/temp/sfx/game_over";

		// Token: 0x0400308A RID: 12426
		public static readonly IReadOnlyList<string> assetPaths = new List<string>
		{
			"doom_apply.mp3", "blunt_attack.mp3", "character_unlock.mp3", "character_unlock_charge.mp3", "card_deal.mp3", "card_exhaust.mp3", "card_select.mp3", "card_smith.mp3", "dagger_throw.mp3", "deny.mp3",
			"doll_room_amb.mp3", "gain_potion.mp3", "heavy_attack.mp3", "map_ping.mp3", "map_open.mp3", "SOTE_Logo_Echoing_ShortTail.mp3", "sts_sfx_shovel_v1.mp3", "relic_get.mp3", "SOTE_SFX_SleepBlanket_v1.mp3", "sleep.tres",
			"slash_attack.mp3", "enemy_turn.mp3", "player_turn.mp3", "victory.mp3"
		}.Concat<string>(TmpSfx.BattleStart).Concat<string>(TmpSfx.PotionSlosh).Select<string, string>(new Func<string, string>(TmpSfx.GetPath))
			.ToList<string>();

		// Token: 0x0400308B RID: 12427
		public const string gainPotion = "gain_potion.mp3";

		// Token: 0x0400308C RID: 12428
		public const string victory = "victory.mp3";

		// Token: 0x0400308D RID: 12429
		public const string startPlayerTurn = "player_turn.mp3";

		// Token: 0x0400308E RID: 12430
		public const string startEnemyTurn = "enemy_turn.mp3";

		// Token: 0x0400308F RID: 12431
		public const string openCardScreen = "map_open.mp3";

		// Token: 0x04003090 RID: 12432
		public const string mapPing = "map_ping.mp3";

		// Token: 0x04003091 RID: 12433
		public const string cardDeal = "card_deal.mp3";

		// Token: 0x04003092 RID: 12434
		public const string cardSelect = "card_select.mp3";

		// Token: 0x04003093 RID: 12435
		public const string cardExhaust = "card_exhaust.mp3";

		// Token: 0x04003094 RID: 12436
		public const string deny = "deny.mp3";

		// Token: 0x04003095 RID: 12437
		public const string mapSplitVoteTick = "map_split_tick.mp3";

		// Token: 0x04003096 RID: 12438
		public const string relicGet = "relic_get.mp3";

		// Token: 0x04003097 RID: 12439
		public const string shovel = "sts_sfx_shovel_v1.mp3";

		// Token: 0x04003098 RID: 12440
		public const string heavyAttack = "heavy_attack.mp3";

		// Token: 0x04003099 RID: 12441
		public const string slashAttack = "slash_attack.mp3";

		// Token: 0x0400309A RID: 12442
		public const string daggerThrow = "dagger_throw.mp3";

		// Token: 0x0400309B RID: 12443
		public const string applyDoom = "doom_apply.mp3";

		// Token: 0x0400309C RID: 12444
		public const string bluntAttack = "blunt_attack.mp3";

		// Token: 0x0400309D RID: 12445
		public const string lightningStrike = "lightning_orb_evoke.mp3";

		// Token: 0x0400309E RID: 12446
		public const string logo = "SOTE_Logo_Echoing_ShortTail.mp3";

		// Token: 0x0400309F RID: 12447
		public const string restSiteHealBlanket = "SOTE_SFX_SleepBlanket_v1.mp3";

		// Token: 0x040030A0 RID: 12448
		public const string restSiteHealJingle = "sleep.tres";

		// Token: 0x040030A1 RID: 12449
		public const string cardSmith = "card_smith.mp3";

		// Token: 0x040030A2 RID: 12450
		public const string characterUnlock = "character_unlock.mp3";

		// Token: 0x040030A3 RID: 12451
		public const string characterUnlockCharge = "character_unlock_charge.mp3";

		// Token: 0x040030A4 RID: 12452
		public const string shortDeathStinger = "STS_DeathStinger_v4_Short_SFX.mp3";

		// Token: 0x040030A5 RID: 12453
		public const string dollRoomAmbience = "doll_room_amb.mp3";

		// Token: 0x040030A6 RID: 12454
		public const string hey = "hey.mp3";

		// Token: 0x040030A7 RID: 12455
		public const string hiss = "hiss.mp3";
	}
}
