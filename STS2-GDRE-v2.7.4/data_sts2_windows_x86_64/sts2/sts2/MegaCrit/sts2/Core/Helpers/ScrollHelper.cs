using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C13 RID: 3091
	public static class ScrollHelper
	{
		/// <summary>
		/// Get the amount of scroll drag that should be added for a given scroll event.
		/// </summary>
		// Token: 0x06007FE0 RID: 32736 RVA: 0x002B6968 File Offset: 0x002B4B68
		[NullableContext(1)]
		public static float GetDragForScrollEvent(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
				if (buttonIndex == 4L)
				{
					return 40f;
				}
				if (buttonIndex == 5L)
				{
					return -40f;
				}
			}
			else
			{
				InputEventPanGesture inputEventPanGesture = inputEvent as InputEventPanGesture;
				if (inputEventPanGesture != null)
				{
					return -inputEventPanGesture.Delta.Y * 50f;
				}
			}
			return 0f;
		}

		/// <summary>
		/// How much we travel per scrollWheel tick.
		/// Probably better to get the expected scroll distance from the OS.
		/// </summary>
		// Token: 0x04002A93 RID: 10899
		private const float _scrollAmount = 40f;

		/// <summary>
		/// How much we travel per touchpad gesture tick.
		/// </summary>
		// Token: 0x04002A94 RID: 10900
		private const float _panScrollSpeed = 50f;

		/// <summary>
		/// How fast we lerp to the target position when dragging.
		/// </summary>
		// Token: 0x04002A95 RID: 10901
		public const float dragLerpSpeed = 15f;

		/// <summary>
		/// How close we need to be to the target position before we snap to it.
		/// </summary>
		// Token: 0x04002A96 RID: 10902
		public const float snapThreshold = 0.5f;

		/// <summary>
		/// How fast we bounce back after scrolling past the edge of the screen.
		/// </summary>
		// Token: 0x04002A97 RID: 10903
		public const float bounceBackStrength = 12f;
	}
}
