using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C65 RID: 3173
	[NullableContext(1)]
	[Nullable(0)]
	public class CloneRestSiteOption : RestSiteOption
	{
		// Token: 0x17002392 RID: 9106
		// (get) Token: 0x06008245 RID: 33349 RVA: 0x002C0E71 File Offset: 0x002BF071
		public override string OptionId
		{
			get
			{
				return "CLONE";
			}
		}

		// Token: 0x06008246 RID: 33350 RVA: 0x002C0E78 File Offset: 0x002BF078
		public CloneRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x17002393 RID: 9107
		// (get) Token: 0x06008247 RID: 33351 RVA: 0x002C0E84 File Offset: 0x002BF084
		public override LocString Description
		{
			get
			{
				LocString description = base.Description;
				description.Add("EnchantmentName", ModelDb.Enchantment<Clone>().Title.GetFormattedText());
				return description;
			}
		}

		// Token: 0x06008248 RID: 33352 RVA: 0x002C0EB4 File Offset: 0x002BF0B4
		public override async Task<bool> OnSelect()
		{
			IEnumerable<CardModel> enumerable = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c.Enchantment is Clone).ToList<CardModel>();
			List<CardPileAddResult> results = new List<CardPileAddResult>();
			foreach (CardModel cardModel in enumerable)
			{
				CardModel cardModel2 = base.Owner.RunState.CloneCard(cardModel);
				List<CardPileAddResult> list = results;
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel2, PileType.Deck, CardPilePosition.Bottom, null, false);
				list.Add(cardPileAddResult);
				list = null;
			}
			IEnumerator<CardModel> enumerator = null;
			CardCmd.PreviewCardPileAdd(results, 1.2f, CardPreviewStyle.MessyLayout);
			return true;
		}

		// Token: 0x06008249 RID: 33353 RVA: 0x002C0EF7 File Offset: 0x002BF0F7
		public override Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short, -1f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600824A RID: 33354 RVA: 0x002C0F18 File Offset: 0x002BF118
		public override Task DoRemotePostSelectVfx()
		{
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			NRestSiteCharacter nrestSiteCharacter = ((instance != null) ? instance.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player == base.Owner) : null);
			if (nrestSiteCharacter != null)
			{
				nrestSiteCharacter.Shake();
			}
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(ModelDb.Relic<PaelsGrowth>());
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
