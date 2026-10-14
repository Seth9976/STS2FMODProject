using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C6A RID: 3178
	[NullableContext(1)]
	[Nullable(0)]
	public class KindleRestSiteOption : RestSiteOption
	{
		// Token: 0x1700239C RID: 9116
		// (get) Token: 0x06008269 RID: 33385 RVA: 0x002C1517 File Offset: 0x002BF717
		public override string OptionId
		{
			get
			{
				return "KINDLE";
			}
		}

		// Token: 0x0600826A RID: 33386 RVA: 0x002C151E File Offset: 0x002BF71E
		public KindleRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x1700239D RID: 9117
		// (get) Token: 0x0600826B RID: 33387 RVA: 0x002C1528 File Offset: 0x002BF728
		public override LocString Description
		{
			get
			{
				LocString description = base.Description;
				description.Add("RelicName", ModelDb.Relic<PumpkinCandle>().Title.GetFormattedText());
				description.Add("RekindleAmount", 5m);
				return description;
			}
		}

		// Token: 0x0600826C RID: 33388 RVA: 0x002C1568 File Offset: 0x002BF768
		public override Task<bool> OnSelect()
		{
			PumpkinCandle relic = base.Owner.GetRelic<PumpkinCandle>();
			if (relic != null)
			{
				relic.Rekindle();
			}
			return Task.FromResult<bool>(true);
		}

		// Token: 0x0600826D RID: 33389 RVA: 0x002C1586 File Offset: 0x002BF786
		public override Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			this.PlayKindleVfx();
			return Task.CompletedTask;
		}

		// Token: 0x0600826E RID: 33390 RVA: 0x002C1593 File Offset: 0x002BF793
		public override Task DoRemotePostSelectVfx()
		{
			this.PlayKindleVfx();
			return Task.CompletedTask;
		}

		// Token: 0x0600826F RID: 33391 RVA: 0x002C15A0 File Offset: 0x002BF7A0
		private void PlayKindleVfx()
		{
			SfxCmd.Play("event:/sfx/characters/attack_fire", 1f);
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			NRestSiteCharacter nrestSiteCharacter = ((instance != null) ? instance.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player == base.Owner) : null);
			if (nrestSiteCharacter != null)
			{
				nrestSiteCharacter.Shake();
			}
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(ModelDb.Relic<PumpkinCandle>());
			if (nrelicFlashVfx == null)
			{
				return;
			}
			if (nrestSiteCharacter != null)
			{
				nrestSiteCharacter.AddChildSafely(nrelicFlashVfx);
			}
			nrelicFlashVfx.Position = Vector2.Zero;
		}
	}
}
