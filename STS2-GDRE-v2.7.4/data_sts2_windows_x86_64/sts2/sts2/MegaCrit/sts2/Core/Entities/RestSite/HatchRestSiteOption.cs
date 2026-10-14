using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C68 RID: 3176
	[NullableContext(1)]
	[Nullable(0)]
	public class HatchRestSiteOption : RestSiteOption
	{
		// Token: 0x17002398 RID: 9112
		// (get) Token: 0x06008258 RID: 33368 RVA: 0x002C11A7 File Offset: 0x002BF3A7
		public override string OptionId
		{
			get
			{
				return "HATCH";
			}
		}

		// Token: 0x06008259 RID: 33369 RVA: 0x002C11AE File Offset: 0x002BF3AE
		public HatchRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x0600825A RID: 33370 RVA: 0x002C11B8 File Offset: 0x002BF3B8
		public override async Task<bool> OnSelect()
		{
			await RelicCmd.Obtain<Byrdpip>(base.Owner);
			return true;
		}

		// Token: 0x0600825B RID: 33371 RVA: 0x002C11FB File Offset: 0x002BF3FB
		public override Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack", 1f);
			return Task.CompletedTask;
		}

		// Token: 0x0600825C RID: 33372 RVA: 0x002C1214 File Offset: 0x002BF414
		public override Task DoRemotePostSelectVfx()
		{
			SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack", 1f);
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			NRestSiteCharacter nrestSiteCharacter = ((instance != null) ? instance.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player == base.Owner) : null);
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(ModelDb.Relic<Byrdpip>());
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
