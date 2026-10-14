using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C6E RID: 3182
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SmithRestSiteOption : RestSiteOption
	{
		// Token: 0x170023AA RID: 9130
		// (get) Token: 0x06008294 RID: 33428 RVA: 0x002C1B34 File Offset: 0x002BFD34
		public override string OptionId
		{
			get
			{
				return "SMITH";
			}
		}

		// Token: 0x170023AB RID: 9131
		// (get) Token: 0x06008295 RID: 33429 RVA: 0x002C1B3B File Offset: 0x002BFD3B
		public override IEnumerable<string> AssetPaths
		{
			get
			{
				return base.AssetPaths.Concat<string>(NCardSmithVfx.AssetPaths);
			}
		}

		// Token: 0x170023AC RID: 9132
		// (get) Token: 0x06008296 RID: 33430 RVA: 0x002C1B4D File Offset: 0x002BFD4D
		// (set) Token: 0x06008297 RID: 33431 RVA: 0x002C1B55 File Offset: 0x002BFD55
		public int SmithCount { get; set; } = 1;

		// Token: 0x170023AD RID: 9133
		// (get) Token: 0x06008298 RID: 33432 RVA: 0x002C1B60 File Offset: 0x002BFD60
		public override LocString Description
		{
			get
			{
				LocString locString;
				if (this.IsEnabled)
				{
					locString = new LocString("rest_site_ui", "OPTION_" + this.OptionId + ".description");
					locString.Add("Count", this.SmithCount);
				}
				else
				{
					locString = new LocString("rest_site_ui", "OPTION_" + this.OptionId + ".descriptionDisabled");
				}
				return locString;
			}
		}

		// Token: 0x170023AE RID: 9134
		// (get) Token: 0x06008299 RID: 33433 RVA: 0x002C1BCE File Offset: 0x002BFDCE
		public override bool IsEnabled
		{
			get
			{
				return base.Owner.Deck.UpgradableCardCount != 0;
			}
		}

		// Token: 0x0600829A RID: 33434 RVA: 0x002C1BE3 File Offset: 0x002BFDE3
		public SmithRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x0600829B RID: 33435 RVA: 0x002C1BF4 File Offset: 0x002BFDF4
		public override async Task<bool> OnSelect()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, this.SmithCount)
			{
				Cancelable = true,
				RequireManualConfirmation = true
			};
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForUpgrade(base.Owner, cardSelectorPrefs);
			this._selection = enumerable;
			bool flag;
			if (!this._selection.Any<CardModel>())
			{
				flag = false;
			}
			else
			{
				foreach (CardModel cardModel in this._selection)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.None);
				}
				await Hook.AfterRestSiteSmith(base.Owner.RunState, base.Owner);
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600829C RID: 33436 RVA: 0x002C1C38 File Offset: 0x002BFE38
		public override async Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.GlobalUi.CardPreviewContainer.AddChildSafely(NCardSmithVfx.Create(this._selection.ToArray<CardModel>(), true));
			}
			await Cmd.CustomScaledWait(1f, 2f, false, ct);
		}

		// Token: 0x0600829D RID: 33437 RVA: 0x002C1C84 File Offset: 0x002BFE84
		public override Task DoRemotePostSelectVfx()
		{
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			NRestSiteCharacter nrestSiteCharacter = ((instance != null) ? instance.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player == base.Owner) : null);
			NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create();
			if (ncardSmithVfx == null)
			{
				return Task.CompletedTask;
			}
			if (nrestSiteCharacter != null)
			{
				nrestSiteCharacter.AddChildSafely(ncardSmithVfx);
			}
			ncardSmithVfx.Position = Vector2.Zero;
			return Task.CompletedTask;
		}

		// Token: 0x04002BEC RID: 11244
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<CardModel> _selection;
	}
}
