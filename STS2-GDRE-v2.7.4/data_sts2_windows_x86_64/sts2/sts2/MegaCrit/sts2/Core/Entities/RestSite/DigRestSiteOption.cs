using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C67 RID: 3175
	[NullableContext(1)]
	[Nullable(0)]
	public class DigRestSiteOption : RestSiteOption
	{
		// Token: 0x17002397 RID: 9111
		// (get) Token: 0x06008252 RID: 33362 RVA: 0x002C10A1 File Offset: 0x002BF2A1
		public override string OptionId
		{
			get
			{
				return "DIG";
			}
		}

		// Token: 0x06008253 RID: 33363 RVA: 0x002C10A8 File Offset: 0x002BF2A8
		public DigRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x06008254 RID: 33364 RVA: 0x002C10B4 File Offset: 0x002BF2B4
		public override async Task<bool> OnSelect()
		{
			await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable(), base.Owner, -1);
			return true;
		}

		// Token: 0x06008255 RID: 33365 RVA: 0x002C10F7 File Offset: 0x002BF2F7
		public override Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			NDebugAudioManager.Instance.Play("sts_sfx_shovel_v1.mp3", 1f, PitchVariance.Small);
			return Task.CompletedTask;
		}

		// Token: 0x06008256 RID: 33366 RVA: 0x002C1114 File Offset: 0x002BF314
		public override Task DoRemotePostSelectVfx()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("sts_sfx_shovel_v1.mp3", 0.5f, PitchVariance.Small);
			}
			NRestSiteRoom instance2 = NRestSiteRoom.Instance;
			NRestSiteCharacter nrestSiteCharacter = ((instance2 != null) ? instance2.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player == base.Owner) : null);
			if (nrestSiteCharacter != null)
			{
				nrestSiteCharacter.Shake();
			}
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(ModelDb.Relic<Shovel>());
			if (nrelicFlashVfx == null)
			{
				return Task.CompletedTask;
			}
			if (nrestSiteCharacter != null)
			{
				nrestSiteCharacter.AddChildSafely(nrelicFlashVfx);
			}
			nrelicFlashVfx.Position = Vector2.Zero;
			return Task.CompletedTask;
		}
	}
}
