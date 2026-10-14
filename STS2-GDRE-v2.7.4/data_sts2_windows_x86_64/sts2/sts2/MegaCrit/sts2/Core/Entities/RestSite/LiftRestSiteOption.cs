using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C6B RID: 3179
	[NullableContext(1)]
	[Nullable(0)]
	public class LiftRestSiteOption : RestSiteOption
	{
		// Token: 0x1700239E RID: 9118
		// (get) Token: 0x06008271 RID: 33393 RVA: 0x002C161C File Offset: 0x002BF81C
		public override LocString Description
		{
			get
			{
				LocString description = base.Description;
				Girya relic = base.Owner.GetRelic<Girya>();
				int num = 3 - relic.TimesLifted;
				description.Add("LiftsLeft", num);
				return description;
			}
		}

		// Token: 0x1700239F RID: 9119
		// (get) Token: 0x06008272 RID: 33394 RVA: 0x002C1657 File Offset: 0x002BF857
		public override string OptionId
		{
			get
			{
				return "LIFT";
			}
		}

		// Token: 0x06008273 RID: 33395 RVA: 0x002C165E File Offset: 0x002BF85E
		public LiftRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x06008274 RID: 33396 RVA: 0x002C1668 File Offset: 0x002BF868
		public override async Task<bool> OnSelect()
		{
			base.Owner.GetRelic<Girya>().TimesLifted++;
			return await Task.FromResult<bool>(true);
		}

		// Token: 0x06008275 RID: 33397 RVA: 0x002C16AB File Offset: 0x002BF8AB
		public override Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short, -1f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06008276 RID: 33398 RVA: 0x002C16CC File Offset: 0x002BF8CC
		public override Task DoRemotePostSelectVfx()
		{
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			NRestSiteCharacter nrestSiteCharacter = ((instance != null) ? instance.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player == base.Owner) : null);
			if (nrestSiteCharacter != null)
			{
				nrestSiteCharacter.Shake();
			}
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(ModelDb.Relic<Girya>());
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
