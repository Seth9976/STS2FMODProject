using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.addons.mega_text
{
	/// <summary>
	/// Central source of truth for Godot theme property names.
	/// These names are defined by Godot and must match exactly.
	/// Tests verify these constants are valid against the current Godot version.
	/// Using StringName for efficiency since Godot's theme methods take StringName parameters.
	/// </summary>
	// Token: 0x02000DA1 RID: 3489
	public static class ThemeConstants
	{
		/// <summary>
		/// Theme property names for Label nodes.
		/// </summary>
		// Token: 0x0200257A RID: 9594
		[NullableContext(1)]
		[Nullable(0)]
		public static class Label
		{
			// Token: 0x0400A075 RID: 41077
			public static readonly StringName FontSize = "font_size";

			// Token: 0x0400A076 RID: 41078
			public static readonly StringName Font = "font";

			// Token: 0x0400A077 RID: 41079
			public static readonly StringName LineSpacing = "line_spacing";

			// Token: 0x0400A078 RID: 41080
			public static readonly StringName OutlineSize = "outline_size";

			// Token: 0x0400A079 RID: 41081
			public static readonly StringName FontColor = "font_color";

			// Token: 0x0400A07A RID: 41082
			public static readonly StringName FontOutlineColor = "font_outline_color";

			// Token: 0x0400A07B RID: 41083
			public static readonly StringName FontShadowColor = "font_shadow_color";
		}

		/// <summary>
		/// Theme property names for RichTextLabel nodes.
		/// Note: RichTextLabel uses different names than Label (e.g., "line_separation" vs "line_spacing").
		/// </summary>
		// Token: 0x0200257B RID: 9595
		[NullableContext(1)]
		[Nullable(0)]
		public static class RichTextLabel
		{
			// Token: 0x0400A07C RID: 41084
			public static readonly StringName NormalFont = "normal_font";

			// Token: 0x0400A07D RID: 41085
			public static readonly StringName BoldFont = "bold_font";

			// Token: 0x0400A07E RID: 41086
			public static readonly StringName ItalicsFont = "italics_font";

			// Token: 0x0400A07F RID: 41087
			public static readonly StringName LineSpacing = "line_separation";

			// Token: 0x0400A080 RID: 41088
			public static readonly StringName NormalFontSize = "normal_font_size";

			// Token: 0x0400A081 RID: 41089
			public static readonly StringName BoldFontSize = "bold_font_size";

			// Token: 0x0400A082 RID: 41090
			public static readonly StringName BoldItalicsFontSize = "bold_italics_font_size";

			// Token: 0x0400A083 RID: 41091
			public static readonly StringName ItalicsFontSize = "italics_font_size";

			// Token: 0x0400A084 RID: 41092
			public static readonly StringName MonoFontSize = "mono_font_size";

			/// <summary>
			/// All font size property names for RichTextLabel.
			/// Used when setting all font sizes to the same value.
			/// </summary>
			// Token: 0x0400A085 RID: 41093
			public static readonly StringName[] AllFontSizes = new StringName[]
			{
				ThemeConstants.RichTextLabel.NormalFontSize,
				ThemeConstants.RichTextLabel.BoldFontSize,
				ThemeConstants.RichTextLabel.BoldItalicsFontSize,
				ThemeConstants.RichTextLabel.ItalicsFontSize,
				ThemeConstants.RichTextLabel.MonoFontSize
			};

			// Token: 0x0400A086 RID: 41094
			public static readonly StringName DefaultColor = "default_color";

			// Token: 0x0400A087 RID: 41095
			public static readonly StringName FontOutlineColor = "font_outline_color";

			// Token: 0x0400A088 RID: 41096
			public static readonly StringName FontShadowColor = "font_shadow_color";
		}

		/// <summary>
		/// Theme property names for Control nodes (shared across many control types).
		/// </summary>
		// Token: 0x0200257C RID: 9596
		public static class Control
		{
			// Token: 0x0400A089 RID: 41097
			[Nullable(1)]
			public static readonly StringName Focus = "focus";
		}

		/// <summary>
		/// Theme property names for MarginContainer nodes.
		/// </summary>
		// Token: 0x0200257D RID: 9597
		[NullableContext(1)]
		[Nullable(0)]
		public static class MarginContainer
		{
			// Token: 0x0400A08A RID: 41098
			public static readonly StringName MarginLeft = "margin_left";

			// Token: 0x0400A08B RID: 41099
			public static readonly StringName MarginRight = "margin_right";

			// Token: 0x0400A08C RID: 41100
			public static readonly StringName MarginTop = "margin_top";

			// Token: 0x0400A08D RID: 41101
			public static readonly StringName MarginBottom = "margin_bottom";
		}

		/// <summary>
		/// Theme property names for HBoxContainer and VBoxContainer nodes.
		/// </summary>
		// Token: 0x0200257E RID: 9598
		public static class BoxContainer
		{
			// Token: 0x0400A08E RID: 41102
			[Nullable(1)]
			public static readonly StringName Separation = "separation";
		}

		/// <summary>
		/// Theme property names for FlowContainer nodes.
		/// </summary>
		// Token: 0x0200257F RID: 9599
		[NullableContext(1)]
		[Nullable(0)]
		public static class FlowContainer
		{
			// Token: 0x0400A08F RID: 41103
			public static readonly StringName HSeparation = "h_separation";

			// Token: 0x0400A090 RID: 41104
			public static readonly StringName VSeparation = "v_separation";
		}

		/// <summary>
		/// Theme property names for TextEdit nodes.
		/// </summary>
		// Token: 0x02002580 RID: 9600
		public static class TextEdit
		{
			// Token: 0x0400A091 RID: 41105
			[Nullable(1)]
			public static readonly StringName Font = "font";
		}

		/// <summary>
		/// Theme property names for LineEdit nodes.
		/// </summary>
		// Token: 0x02002581 RID: 9601
		public static class LineEdit
		{
			// Token: 0x0400A092 RID: 41106
			[Nullable(1)]
			public static readonly StringName Font = "font";
		}
	}
}
