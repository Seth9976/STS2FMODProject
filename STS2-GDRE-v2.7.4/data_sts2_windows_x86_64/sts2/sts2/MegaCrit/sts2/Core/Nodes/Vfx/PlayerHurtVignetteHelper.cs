using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Creates VFX warning that the player is low.
	/// Uses NLowHpBorderVfx, but avoids creating more than one at a time.
	/// </summary>
	// Token: 0x02000210 RID: 528
	public static class PlayerHurtVignetteHelper
	{
		// Token: 0x06001B57 RID: 6999 RVA: 0x000B1A0C File Offset: 0x000AFC0C
		public static void Play()
		{
			if (PlayerHurtVignetteHelper._currentVfx != null && GodotObject.IsInstanceValid(PlayerHurtVignetteHelper._currentVfx))
			{
				PlayerHurtVignetteHelper._currentVfx.Play();
				return;
			}
			PlayerHurtVignetteHelper._currentVfx = NLowHpBorderVfx.Create();
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.GlobalUi.AddChildSafely(PlayerHurtVignetteHelper._currentVfx);
			}
			NLowHpBorderVfx currentVfx = PlayerHurtVignetteHelper._currentVfx;
			if (currentVfx == null)
			{
				return;
			}
			currentVfx.Play();
		}

		// Token: 0x04000A34 RID: 2612
		[Nullable(2)]
		private static NLowHpBorderVfx _currentVfx;
	}
}
