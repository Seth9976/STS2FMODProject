using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Commands
{
	/// <summary>
	/// Creates a speech bubble. Used when a creature talks/screams/whispers to you. Generally during combat
	/// </summary>
	// Token: 0x02000D3A RID: 3386
	[NullableContext(1)]
	[Nullable(0)]
	public static class TalkCmd
	{
		// Token: 0x0600887F RID: 34943 RVA: 0x002DF620 File Offset: 0x002DD820
		[return: Nullable(2)]
		public static NSpeechBubbleVfx Play(LocString line, Creature speaker, VfxColor vfxColor, VfxDuration duration = VfxDuration.Custom)
		{
			if (speaker.IsDead)
			{
				return null;
			}
			string formattedText = line.GetFormattedText();
			double num;
			if (duration == VfxDuration.Custom)
			{
				num = (double)TalkCmd.GetRawCharCount(formattedText) * ((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.1 : 0.12);
			}
			else
			{
				num = TalkCmd.GetDuration(duration);
				if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
				{
					num -= 0.5;
				}
			}
			num = Math.Max(0.5, num);
			NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.Create(formattedText, speaker, num, vfxColor);
			if (nspeechBubbleVfx != null)
			{
				Control vfxContainer = speaker.GetVfxContainer();
				if (vfxContainer != null)
				{
					vfxContainer.AddChildSafely(nspeechBubbleVfx);
				}
			}
			return nspeechBubbleVfx;
		}

		// Token: 0x06008880 RID: 34944 RVA: 0x002DF6C8 File Offset: 0x002DD8C8
		private static double GetDuration(VfxDuration duration)
		{
			switch (duration)
			{
			case VfxDuration.None:
				return 0.0;
			case VfxDuration.VeryShort:
				return 1.0;
			case VfxDuration.Short:
				return 1.5;
			case VfxDuration.Standard:
				return 1.75;
			case VfxDuration.Long:
				return 2.25;
			case VfxDuration.VeryLong:
				return 3.0;
			case VfxDuration.Forever:
				return 999999999.0;
			}
			return 0.0;
		}

		/// <summary>
		/// Calculates how many characters in a string without the bbcode. Also strips out new line, brackets, optionals.
		/// Example:
		/// [jitter][gold]Money![/gold][/jitter]
		/// [sine][orange]Money![/orange][/sine]
		/// returns Money!Money! -&gt; 12 characters -&gt; 1.2 second speech bubble
		/// </summary>
		// Token: 0x06008881 RID: 34945 RVA: 0x002DF75C File Offset: 0x002DD95C
		private static int GetRawCharCount(string bbcodeText)
		{
			string text = Regex.Replace(bbcodeText, "\\[/?[^\\]]+\\]", "");
			return text.Replace("\n", "").Replace("\r", "").Replace(" ", "")
				.Length;
		}
	}
}
