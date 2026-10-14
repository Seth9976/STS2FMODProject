using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Creates fullscreen VFX that occurs when healing at an event.
	/// </summary>
	// Token: 0x0200020F RID: 527
	[NullableContext(1)]
	[Nullable(0)]
	public static class PlayerFullscreenHealVfx
	{
		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001B54 RID: 6996 RVA: 0x000B18A8 File Offset: 0x000AFAA8
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(PlayerFullscreenHealVfx._scenePath);
			}
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000B18B4 File Offset: 0x000AFAB4
		public static void Play(Player player, decimal healAmount, [Nullable(2)] Control vfxContainer)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (healAmount < 1m)
			{
				return;
			}
			if (vfxContainer == null)
			{
				return;
			}
			float num = Ease.QuadOut((float)(healAmount / player.Creature.MaxHp));
			Color green = StsColors.green;
			green.A = Mathf.Max(num * 0.8f, 0.4f);
			NSmokyVignetteVfx nsmokyVignetteVfx = NSmokyVignetteVfx.Create(green, new Color(0f, 1f, 0f, 0.33f));
			if (nsmokyVignetteVfx != null)
			{
				vfxContainer.AddChildSafely(nsmokyVignetteVfx);
			}
			NVfxParticleSystem nvfxParticleSystem = PreloadManager.Cache.GetScene(PlayerFullscreenHealVfx._scenePath).Instantiate<NVfxParticleSystem>(0L);
			Rect2 viewportRect = NGame.Instance.GetViewportRect();
			nvfxParticleSystem.GlobalPosition = viewportRect.Size * 0.5f;
			GpuParticles2D node = nvfxParticleSystem.GetNode<GpuParticles2D>("beam");
			ParticleProcessMaterial particleProcessMaterial = (ParticleProcessMaterial)node.ProcessMaterial;
			particleProcessMaterial.EmissionBoxExtents = new Vector3(viewportRect.Size.X / 2f, viewportRect.Size.Y / 2f, 1f);
			int num2 = Mathf.RoundToInt(num * 40f + 10f);
			node.Amount = num2;
			vfxContainer.AddChildSafely(nvfxParticleSystem);
		}

		// Token: 0x04000A33 RID: 2611
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_cross_heal_fullscreen");
	}
}
