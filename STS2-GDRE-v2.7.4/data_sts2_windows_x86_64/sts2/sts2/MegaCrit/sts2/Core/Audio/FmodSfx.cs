using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Audio
{
	/// <summary>
	/// Set of FMOD sfx that are used more globally (vs being tied to a specific creature of vfx).
	/// Any Temporary Sfx or Sfx not using FMOD should be put into TmpSfx.cs
	/// </summary>
	// Token: 0x02000D89 RID: 3465
	[NullableContext(1)]
	[Nullable(0)]
	public static class FmodSfx
	{
		// Token: 0x04003051 RID: 12369
		public const string block = "event:/sfx/block_gain";

		// Token: 0x04003052 RID: 12370
		public const string blockBreak = "event:/sfx/block_break";

		// Token: 0x04003053 RID: 12371
		public const string blockHit = "event:/sfx/block_hit";

		// Token: 0x04003054 RID: 12372
		public const string buff = "event:/sfx/buff";

		// Token: 0x04003055 RID: 12373
		public const string debuff = "event:/sfx/debuff";

		// Token: 0x04003056 RID: 12374
		public const string heal = "event:/sfx/heal";

		// Token: 0x04003057 RID: 12375
		public const string fire = "event:/sfx/characters/attack_fire";

		// Token: 0x04003058 RID: 12376
		public const string backButton = "event:/sfx/ui/clicks/ui_back";

		// Token: 0x04003059 RID: 12377
		public const string uiHover = "event:/sfx/ui/clicks/ui_hover";

		// Token: 0x0400305A RID: 12378
		public const string uiClick = "event:/sfx/ui/clicks/ui_click";

		// Token: 0x0400305B RID: 12379
		public const string timelineHover = "event:/sfx/ui/timeline/ui_timeline_hover";

		// Token: 0x0400305C RID: 12380
		public const string timelineHoverLocked = "event:/sfx/ui/timeline/ui_timeline_hover_locked";

		// Token: 0x0400305D RID: 12381
		public const string timelineClick = "event:/sfx/ui/timeline/ui_timeline_click";

		// Token: 0x0400305E RID: 12382
		public const string timelineOpenEpoch = "event:/sfx/ui/timeline/ui_timeline_open_epoch";

		// Token: 0x0400305F RID: 12383
		public const string timelineCloseEpoch = "event:/sfx/ui/timeline/ui_timeline_close_epoch";

		// Token: 0x04003060 RID: 12384
		public const string timelineOpen = "event:/sfx/ui/timeline/ui_timeline_open";

		// Token: 0x04003061 RID: 12385
		public const string timelineUnlock = "event:/sfx/ui/timeline/ui_timeline_unlock";

		// Token: 0x04003062 RID: 12386
		public const string uiTickboxOn = "event:/sfx/ui/clicks/ui_checkbox_on";

		// Token: 0x04003063 RID: 12387
		public const string uiTickboxOff = "event:/sfx/ui/clicks/ui_checkbox_off";

		// Token: 0x04003064 RID: 12388
		public const string wipeMap = "event:/sfx/ui/wipe_map";

		// Token: 0x04003065 RID: 12389
		public const string closeMap = "event:/sfx/ui/map/map_close";

		// Token: 0x04003066 RID: 12390
		public const string openMap = "event:/sfx/ui/map/map_open";

		// Token: 0x04003067 RID: 12391
		public const string selectMapNode = "event:/sfx/ui/map/map_select";

		// Token: 0x04003068 RID: 12392
		public const string eraseMapSketch = "event:/sfx/ui/map/map_erase";

		// Token: 0x04003069 RID: 12393
		public const string openPauseMenu = "event:/sfx/ui/pause_open";

		// Token: 0x0400306A RID: 12394
		public const string closePauseMenu = "event:/sfx/ui/pause_close";

		// Token: 0x0400306B RID: 12395
		public const string relicFlashGeneral = "event:/sfx/ui/relic_activate_general";

		// Token: 0x0400306C RID: 12396
		public const string relicFlashDraw = "event:/sfx/ui/relic_activate_draw";

		// Token: 0x0400306D RID: 12397
		public const string gainEnergy = "event:/sfx/ui/gain_energy";

		// Token: 0x0400306E RID: 12398
		public const string enchant = "event:/sfx/ui/enchant_shimmer";

		// Token: 0x0400306F RID: 12399
		public const string merchantWelcome = "event:/sfx/npcs/merchant/merchant_welcome";

		// Token: 0x04003070 RID: 12400
		public const string merchantPassive = "event:/sfx/npcs/merchant/merchant_passive";

		// Token: 0x04003071 RID: 12401
		public const string merchantThankYou = "event:/sfx/npcs/merchant/merchant_thank_yous";

		// Token: 0x04003072 RID: 12402
		public const string merchantDisappointment = "event:/sfx/npcs/merchant/merchant_dissapointment";

		// Token: 0x04003073 RID: 12403
		public const string fakeMerchantLaugh = "event:/sfx/npcs/reverse_merchant/reverse_merchant_laugh";

		// Token: 0x04003074 RID: 12404
		public const string transform = "event:/sfx/ui/cards/card_transform";

		// Token: 0x04003075 RID: 12405
		public const string doomKill = "event:/sfx/characters/necrobinder/necrobinder_doom_kill";

		// Token: 0x04003076 RID: 12406
		public const string necrobinderSummon = "event:/sfx/characters/necrobinder/necrobinder_summon";

		// Token: 0x04003077 RID: 12407
		public const string cardImpactIntoMulti = "event:/sfx/ui/cards/card_impact_into_multi";

		// Token: 0x04003078 RID: 12408
		public const string cardImpactIntoSingle = "event:/sfx/ui/cards/card_impact_into_single";

		// Token: 0x04003079 RID: 12409
		public const string cardMovementIntoDeck = "event:/sfx/ui/cards/card_movement_B_into_deck";

		// Token: 0x0400307A RID: 12410
		public const string cardMovementIntoDiscard = "event:/sfx/ui/cards/card_movement_B_into_discard";

		// Token: 0x0400307B RID: 12411
		public const string cardMovementIntoDraw = "event:/sfx/ui/cards/card_movement_B_into_draw";

		// Token: 0x0400307C RID: 12412
		public const string cardMovementPlayIntoDiscard = "event:/sfx/ui/cards/card_movement_B_play_into_discard";

		// Token: 0x0400307D RID: 12413
		public const string cardMovementIntoPower = "event:/sfx/ui/cards/card_movement_B_power";
	}
}
