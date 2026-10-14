using System;
using Godot;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput
{
	// Token: 0x020004AD RID: 1197
	public interface INetCursorPositionTranslator
	{
		// Token: 0x0600480E RID: 18446
		Vector2 GetNetPositionFromScreenPosition(Vector2 screenPosition);

		// Token: 0x0600480F RID: 18447
		Vector2 GetScreenPositionFromNetPosition(Vector2 netPosition);
	}
}
