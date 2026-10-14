using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.ControllerInput
{
	// Token: 0x02000D21 RID: 3361
	[NullableContext(1)]
	[Nullable(0)]
	public static class MegaInput
	{
		// Token: 0x17002565 RID: 9573
		// (get) Token: 0x06008792 RID: 34706 RVA: 0x002D9794 File Offset: 0x002D7994
		public static string[] AllInputs
		{
			get
			{
				return new string[]
				{
					MegaInput.up,
					MegaInput.down,
					MegaInput.left,
					MegaInput.right,
					MegaInput.altUp,
					MegaInput.altDown,
					MegaInput.altLeft,
					MegaInput.altRight,
					MegaInput.select,
					MegaInput.cancel,
					MegaInput.accept,
					MegaInput.topPanel,
					MegaInput.viewDeckAndTabLeft,
					MegaInput.viewExhaustPileAndTabRight,
					MegaInput.viewDiscardPile,
					MegaInput.viewDrawPile,
					MegaInput.pauseAndBack,
					MegaInput.viewMap,
					MegaInput.peek
				};
			}
		}

		// Token: 0x04002F25 RID: 12069
		public static readonly StringName up = "ui_up";

		// Token: 0x04002F26 RID: 12070
		public static readonly StringName down = "ui_down";

		// Token: 0x04002F27 RID: 12071
		public static readonly StringName left = "ui_left";

		// Token: 0x04002F28 RID: 12072
		public static readonly StringName right = "ui_right";

		// Token: 0x04002F29 RID: 12073
		public static readonly StringName altUp = "ui_alt_up";

		// Token: 0x04002F2A RID: 12074
		public static readonly StringName altDown = "ui_alt_down";

		// Token: 0x04002F2B RID: 12075
		public static readonly StringName altLeft = "ui_alt_left";

		// Token: 0x04002F2C RID: 12076
		public static readonly StringName altRight = "ui_alt_right";

		// Token: 0x04002F2D RID: 12077
		public static readonly StringName accept = "ui_accept";

		// Token: 0x04002F2E RID: 12078
		public static readonly StringName select = "ui_select";

		// Token: 0x04002F2F RID: 12079
		public static readonly StringName cancel = "ui_cancel";

		// Token: 0x04002F30 RID: 12080
		public static readonly StringName selectCard1 = "mega_select_card_1";

		// Token: 0x04002F31 RID: 12081
		public static readonly StringName selectCard2 = "mega_select_card_2";

		// Token: 0x04002F32 RID: 12082
		public static readonly StringName selectCard3 = "mega_select_card_3";

		// Token: 0x04002F33 RID: 12083
		public static readonly StringName selectCard4 = "mega_select_card_4";

		// Token: 0x04002F34 RID: 12084
		public static readonly StringName selectCard5 = "mega_select_card_5";

		// Token: 0x04002F35 RID: 12085
		public static readonly StringName selectCard6 = "mega_select_card_6";

		// Token: 0x04002F36 RID: 12086
		public static readonly StringName selectCard7 = "mega_select_card_7";

		// Token: 0x04002F37 RID: 12087
		public static readonly StringName selectCard8 = "mega_select_card_8";

		// Token: 0x04002F38 RID: 12088
		public static readonly StringName selectCard9 = "mega_select_card_9";

		// Token: 0x04002F39 RID: 12089
		public static readonly StringName selectCard10 = "mega_select_card_10";

		// Token: 0x04002F3A RID: 12090
		public static readonly StringName releaseCard = "mega_release_card";

		// Token: 0x04002F3B RID: 12091
		public static readonly StringName topPanel = "mega_top_panel";

		// Token: 0x04002F3C RID: 12092
		public static readonly StringName viewDrawPile = "mega_view_draw_pile";

		// Token: 0x04002F3D RID: 12093
		public static readonly StringName viewDiscardPile = "mega_view_discard_pile";

		// Token: 0x04002F3E RID: 12094
		public static readonly StringName viewDeckAndTabLeft = "mega_view_deck_and_tab_left";

		// Token: 0x04002F3F RID: 12095
		public static readonly StringName viewExhaustPileAndTabRight = "mega_view_exhaust_pile_and_tab_right";

		// Token: 0x04002F40 RID: 12096
		public static readonly StringName viewMap = "mega_view_map";

		// Token: 0x04002F41 RID: 12097
		public static readonly StringName pauseAndBack = "mega_pause_and_back";

		// Token: 0x04002F42 RID: 12098
		public static readonly StringName back = "mega_back";

		// Token: 0x04002F43 RID: 12099
		public static readonly StringName peek = "mega_peek";
	}
}
