using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Commands
{
	/// <summary>
	/// Creates a thought bubble
	/// </summary>
	// Token: 0x02000D3B RID: 3387
	public static class ThinkCmd
	{
		// Token: 0x06008882 RID: 34946 RVA: 0x002DF7B0 File Offset: 0x002DD9B0
		[NullableContext(1)]
		public static void Play(LocString line, Creature speaker, double secondsToDisplay = -1.0)
		{
			string formattedText = line.GetFormattedText();
			if (secondsToDisplay < 0.0)
			{
				secondsToDisplay = (double)formattedText.Length * 0.08;
			}
			if (secondsToDisplay < 1.5)
			{
				secondsToDisplay = 1.5;
			}
			NThoughtBubbleVfx nthoughtBubbleVfx = NThoughtBubbleVfx.Create(formattedText, speaker, new double?(secondsToDisplay));
			if (nthoughtBubbleVfx != null)
			{
				Control vfxContainer = speaker.GetVfxContainer();
				if (vfxContainer == null)
				{
					return;
				}
				vfxContainer.AddChildSafely(nthoughtBubbleVfx);
			}
		}

		// Token: 0x04002F60 RID: 12128
		private const double _defaultTimePerCharacter = 0.08;

		// Token: 0x04002F61 RID: 12129
		private const double _minTimeToDisplay = 1.5;
	}
}
