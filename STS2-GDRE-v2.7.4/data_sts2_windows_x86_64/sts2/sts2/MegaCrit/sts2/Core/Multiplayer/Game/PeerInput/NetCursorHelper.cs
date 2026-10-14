using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput
{
	/// <summary>
	/// Utilities to help with transforming mouse position from screen coordinates to a normalized position suitable for
	/// syncing mouse positions across various aspect ratios.
	/// </summary>
	// Token: 0x020004AF RID: 1199
	[NullableContext(2)]
	[Nullable(0)]
	public static class NetCursorHelper
	{
		// Token: 0x06004810 RID: 18448 RVA: 0x002206FC File Offset: 0x0021E8FC
		public static Vector2 GetNormalizedPosition(Vector2 mouseScreenPos, Control rootNode)
		{
			if (rootNode != null)
			{
				Vector2 vector = rootNode.GetGlobalTransformWithCanvas() * mouseScreenPos;
				Vector2 vector2;
				vector2..ctor(960f, 540f);
				return (vector - rootNode.Size / 2f) / vector2;
			}
			if (TestMode.IsOn)
			{
				return mouseScreenPos;
			}
			throw new InvalidOperationException("Root node should only be null in tests!");
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0022075C File Offset: 0x0021E95C
		public static Vector2 GetControlSpacePosition(Vector2 normalizedCursorPosition, Control rootNode)
		{
			if (rootNode != null)
			{
				Vector2 vector;
				vector..ctor(960f, 540f);
				return normalizedCursorPosition * vector + rootNode.Size / 2f;
			}
			if (TestMode.IsOn)
			{
				return normalizedCursorPosition;
			}
			throw new InvalidOperationException("Root node should only be null in tests!");
		}

		// Token: 0x04001C57 RID: 7255
		public static readonly QuantizeParams quantizeParams = new QuantizeParams(-3f, 3f, 16);
	}
}
