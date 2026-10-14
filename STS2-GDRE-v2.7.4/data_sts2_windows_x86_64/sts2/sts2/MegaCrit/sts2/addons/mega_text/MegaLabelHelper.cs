using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Text;

namespace MegaCrit.Sts2.addons.mega_text
{
	// Token: 0x02000D9F RID: 3487
	[NullableContext(1)]
	[Nullable(0)]
	public static class MegaLabelHelper
	{
		/// <summary>
		/// This is the result of a very long investigation into what I believe is ultimately a Godot bug. Here's an
		/// explanation of the issue for future reference.
		///
		/// MegaLabel uses TextParagraph to auto-size. In order to use this method, MegaLabel must grab the current font
		/// whenever it auto-sizes. Auto-sizing happens in response to many different events, including when the game is
		/// quitting (this is caused by how Godot works, it's not intentional).
		///
		/// When we're auto-sizing in response to quitting and we try to grab the current font, if we have no font override
		/// (meaning the font is just defined in the theme, or nowhere at all), the game beach-balls on macOS. If we do have
		/// a font override (even if it's the exact same font as in the theme), it works perfectly.
		///
		/// This feels to me like either a reference counting bug, or an issue with the resource serialization lifecycle
		/// timing. I believe we can fix it with an engine patch, and once we do, we can remove this check. In the meantime
		/// though, this will keep us from creating a MegaLabel without a font override.
		/// </summary>
		// Token: 0x06008C40 RID: 35904 RVA: 0x002ED324 File Offset: 0x002EB524
		public static void AssertThemeFontOverride(Control control, StringName fontOverrideName)
		{
			if (control.HasThemeFontOverride(fontOverrideName))
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 2);
			defaultInterpolatedStringHandler.AppendFormatted(control.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral(" '");
			defaultInterpolatedStringHandler.AppendFormatted<NodePath>(control.GetPath());
			defaultInterpolatedStringHandler.AppendLiteral("' has no theme font override. Please set one to avoid a Godot engine bug.");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// This does a crude parsing of BBcode tags and text into a format usable by the EstimateTextSize function.
		/// The version of EsimateTextSize that takes a string can be used with RichTextLabel.GetParsedText. However, this
		/// will strip images and ignore their size, which is problematic when we are using energy icons in text.
		/// If EstimateTextSize is called multiple times, call this only once to minimize performance impact.
		/// Note that the return value of this is cached! We only expect this to be called once, for the results to be used,
		/// and then for it to be called again.
		/// </summary>
		/// <param name="bbcode">String with bbcode tags.</param>
		/// <returns>The parsed tags and text from the bbcode string.</returns>
		// Token: 0x06008C41 RID: 35905 RVA: 0x002ED388 File Offset: 0x002EB588
		public static List<BbcodeObject> ParseBbcode(string bbcode)
		{
			MegaLabelHelper._cachedBbcodeObjects.Clear();
			MegaLabelHelper.BbcodeParsingState bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.NotInTag;
			Stack<string> stack = new Stack<string>();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < bbcode.Length; i++)
			{
				if (bbcode[i] == '[' && bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.NotInTag)
				{
					if (stringBuilder.Length > 0)
					{
						MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject
						{
							text = stringBuilder.ToString(),
							type = BbcodeObjectType.Text
						});
						stringBuilder.Clear();
					}
					bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.InTag;
				}
				else if ((bbcode[i] == ' ' || bbcode[i] == '=') && bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag)
				{
					string text = stringBuilder.ToString();
					MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject
					{
						tag = text,
						type = BbcodeObjectType.BeginTag
					});
					stack.Push(text);
					bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.InTagEnvironment;
				}
				else if (bbcode[i] == '/' && bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag)
				{
					bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.InEndTag;
				}
				else if (bbcode[i] == ']' && (bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag || bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InEndTag || bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTagEnvironment))
				{
					if (bbcodeParsingState != MegaLabelHelper.BbcodeParsingState.InTagEnvironment)
					{
						string text2 = stringBuilder.ToString();
						if (text2 == "lb")
						{
							MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject
							{
								text = "[",
								type = BbcodeObjectType.Text
							});
						}
						else if (text2 == "rb")
						{
							MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject
							{
								text = "]",
								type = BbcodeObjectType.Text
							});
						}
						else
						{
							if (bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InEndTag)
							{
								if (stack.Count == 0)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
									defaultInterpolatedStringHandler.AppendLiteral("Found end tag ");
									defaultInterpolatedStringHandler.AppendFormatted(text2);
									defaultInterpolatedStringHandler.AppendLiteral(" with no tag on the stack. (");
									defaultInterpolatedStringHandler.AppendFormatted(bbcode);
									defaultInterpolatedStringHandler.AppendLiteral(")");
									throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
								}
								if (stack.Peek() != text2)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 3);
									defaultInterpolatedStringHandler.AppendLiteral("Found end tag ");
									defaultInterpolatedStringHandler.AppendFormatted(text2);
									defaultInterpolatedStringHandler.AppendLiteral(", expected ");
									defaultInterpolatedStringHandler.AppendFormatted(stack.Peek());
									defaultInterpolatedStringHandler.AppendLiteral(". (");
									defaultInterpolatedStringHandler.AppendFormatted(bbcode);
									defaultInterpolatedStringHandler.AppendLiteral(")");
									throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
								}
								stack.Pop();
							}
							else
							{
								stack.Push(text2);
							}
							MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject
							{
								tag = text2,
								type = ((bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag) ? BbcodeObjectType.BeginTag : BbcodeObjectType.EndTag)
							});
						}
					}
					bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.NotInTag;
					stringBuilder.Clear();
				}
				else if (bbcodeParsingState != MegaLabelHelper.BbcodeParsingState.InTagEnvironment)
				{
					stringBuilder.Append(bbcode[i]);
				}
			}
			if (bbcodeParsingState != MegaLabelHelper.BbcodeParsingState.NotInTag)
			{
				throw new InvalidOperationException("In tag at end of string");
			}
			if (stringBuilder.Length > 0)
			{
				MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject
				{
					text = stringBuilder.ToString(),
					type = BbcodeObjectType.Text
				});
			}
			return MegaLabelHelper._cachedBbcodeObjects;
		}

		/// <summary>
		/// Estimates the size of some text with bbcode in it.
		/// </summary>
		/// <param name="paragraph">Cached paragraph object that can be reused through multiple calls to this function.</param>
		/// <param name="objs">The parsed bbcode text obtained from ParseBbcode.</param>
		/// <param name="font">The font to use.</param>
		/// <param name="fontSize">The font size to use.</param>
		/// <param name="maxWidth">The maximum width of a line before wrapping.</param>
		/// <param name="lineSpacing">The spacing between lines.</param>
		/// <returns>Estimated size of the text.</returns>
		// Token: 0x06008C42 RID: 35906 RVA: 0x002ED664 File Offset: 0x002EB864
		public static Vector2 EstimateTextSize(TextParagraph paragraph, List<BbcodeObject> objs, Font font, int fontSize, float maxWidth, float lineSpacing)
		{
			paragraph.Clear();
			paragraph.Direction = 0L;
			paragraph.Orientation = 0L;
			Stack<string> stack = new Stack<string>();
			int num = 0;
			foreach (BbcodeObject bbcodeObject in objs)
			{
				if (bbcodeObject.type == BbcodeObjectType.BeginTag)
				{
					stack.Push(bbcodeObject.tag);
				}
				else if (bbcodeObject.type == BbcodeObjectType.EndTag)
				{
					stack.Pop();
				}
				else if (bbcodeObject.type == BbcodeObjectType.Text)
				{
					string text;
					if (stack.TryPeek(out text) && text == "img")
					{
						string text2 = bbcodeObject.text;
						Texture2D texture2D = PreloadManager.Cache.GetTexture2D(text2);
						paragraph.AddObject(num, texture2D.GetSize(), 5L, 1, 0f);
						num++;
					}
					else
					{
						paragraph.AddString(bbcodeObject.text, font, fontSize, "", default(Variant));
					}
				}
			}
			paragraph.Width = maxWidth;
			paragraph.BreakFlags = 3L;
			paragraph.JustificationFlags = 3L;
			paragraph.TextOverrunBehavior = 1L;
			paragraph.Alignment = 1L;
			paragraph.MaxLinesVisible = -1;
			int lineCount = paragraph.GetLineCount();
			return paragraph.GetSize() + Vector2.Down * (lineSpacing - -3f) * (float)(lineCount - 1);
		}

		/// <summary>
		/// Checks if some text with bbcode in it is too big for the passed size.
		/// </summary>
		/// <param name="paragraph">Cached paragraph object that can be reused through multiple calls to this function.</param>
		/// <param name="objs">The parsed bbcode text obtained from ParseBbcode.</param>
		/// <param name="font">The font to use.</param>
		/// <param name="fontSize">The font size to use.</param>
		/// <param name="lineSpacing">The spacing between lines.</param>
		/// <param name="rectSize">The size to check against.</param>
		/// <param name="horizontallyBound">If false, allows the size to be greater than the X size of the rect.</param>
		/// <param name="verticallyBound">If false, allows the size to be greater than the Y size of the rect.</param>
		/// <returns>Whether or not the text would overrun rectSize when rendered.</returns>
		// Token: 0x06008C43 RID: 35907 RVA: 0x002ED7D4 File Offset: 0x002EB9D4
		public static bool IsTooBig(TextParagraph paragraph, List<BbcodeObject> objs, Font font, int fontSize, float lineSpacing, Vector2 rectSize, bool horizontallyBound, bool verticallyBound)
		{
			Vector2 vector = MegaLabelHelper.EstimateTextSize(paragraph, objs, font, fontSize, rectSize.X, lineSpacing);
			float x = rectSize.X;
			float y = rectSize.Y;
			bool flag = vector.X > x;
			bool flag2 = vector.Y > y;
			return (flag && horizontallyBound) || (flag2 && verticallyBound);
		}

		/// <summary>
		/// Estimates the size of some text.
		/// Should be called only on text with no bbcode in it.
		/// </summary>
		/// <param name="paragraph">Cached paragraph object that can be reused through multiple calls to this function.</param>
		/// <param name="text">The text whose size will be estimated.</param>
		/// <param name="font">The font to use.</param>
		/// <param name="fontSize">The font size to use.</param>
		/// <param name="maxWidth">The maximum width of a line before wrapping.</param>
		/// <param name="lineSpacing">The spacing between lines.</param>
		/// <param name="wrap">If true, line wrapping is accounted for.</param>
		/// <returns>Estimated size of the text.</returns>
		// Token: 0x06008C44 RID: 35908 RVA: 0x002ED828 File Offset: 0x002EBA28
		public static Vector2 EstimateTextSize(TextParagraph paragraph, string text, Font font, int fontSize, float maxWidth, float lineSpacing, bool wrap = true)
		{
			paragraph.Clear();
			paragraph.Direction = 0L;
			paragraph.Orientation = 0L;
			paragraph.AddString(text, font, fontSize, "", default(Variant));
			paragraph.Width = maxWidth;
			paragraph.BreakFlags = (wrap ? 3L : 0L);
			paragraph.JustificationFlags = 3L;
			paragraph.TextOverrunBehavior = 0L;
			paragraph.Alignment = 1L;
			paragraph.MaxLinesVisible = -1;
			int lineCount = paragraph.GetLineCount();
			return paragraph.GetSize() + Vector2.Down * (lineSpacing - -3f) * (float)(lineCount - 1);
		}

		/// <summary>
		/// Checks if some text with no bbcode in it is too big for the passed size.
		/// </summary>
		/// <param name="paragraph">Cached paragraph object that can be reused through multiple calls to this function.</param>
		/// <param name="text">The text whose size will be estimated.</param>
		/// <param name="font">The font to use.</param>
		/// <param name="fontSize">The font size to use.</param>
		/// <param name="lineSpacing">The spacing between lines.</param>
		/// <param name="rectSize">The size to check against.</param>
		/// <param name="wrap">Whether or not we allow the text to be wrapped.</param>
		/// <returns>Whether or not the text would overrun rectSize when rendered.</returns>
		// Token: 0x06008C45 RID: 35909 RVA: 0x002ED8C8 File Offset: 0x002EBAC8
		public static bool IsTooBig(TextParagraph paragraph, string text, Font font, int fontSize, float lineSpacing, bool wrap, Vector2 rectSize)
		{
			Vector2 vector = MegaLabelHelper.EstimateTextSize(paragraph, text, font, fontSize, rectSize.X, lineSpacing, wrap);
			float x = rectSize.X;
			float y = rectSize.Y;
			bool flag = vector.X > x;
			bool flag2 = vector.Y > y;
			return flag || flag2;
		}

		// Token: 0x04003115 RID: 12565
		private const float _defaultLineSpacing = -3f;

		// Token: 0x04003116 RID: 12566
		private static readonly List<BbcodeObject> _cachedBbcodeObjects = new List<BbcodeObject>();

		// Token: 0x02002576 RID: 9590
		[NullableContext(0)]
		private enum BbcodeParsingState
		{
			// Token: 0x0400A058 RID: 41048
			NotInTag,
			// Token: 0x0400A059 RID: 41049
			InTag,
			// Token: 0x0400A05A RID: 41050
			InEndTag,
			// Token: 0x0400A05B RID: 41051
			InTagEnvironment
		}
	}
}
