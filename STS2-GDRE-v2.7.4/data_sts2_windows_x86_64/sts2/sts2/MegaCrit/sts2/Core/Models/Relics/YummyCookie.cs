using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200061C RID: 1564
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YummyCookie : RelicModel
	{
		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x0600565D RID: 22109 RVA: 0x0024A643 File Offset: 0x00248843
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x0600565E RID: 22110 RVA: 0x0024A646 File Offset: 0x00248846
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x0600565F RID: 22111 RVA: 0x0024A64C File Offset: 0x0024884C
		protected override string IconBaseName
		{
			get
			{
				CharacterModel characterModel;
				if (base.IsCanonical || base.Owner == null)
				{
					if (YummyCookie._cachedRandomCharacter == null)
					{
						YummyCookie._cachedRandomCharacter = Rng.Chaotic.NextItem<CharacterModel>(ModelDb.AllCharacters);
					}
					characterModel = YummyCookie._cachedRandomCharacter;
				}
				else
				{
					characterModel = base.Owner.Character;
				}
				string text;
				if (!(characterModel is Ironclad))
				{
					if (!(characterModel is Silent))
					{
						if (!(characterModel is Regent))
						{
							if (!(characterModel is Necrobinder))
							{
								if (!(characterModel is Defect))
								{
									text = "yummy_cookie_ironclad";
								}
								else
								{
									text = "yummy_cookie_defect";
								}
							}
							else
							{
								text = "yummy_cookie_necro";
							}
						}
						else
						{
							text = "yummy_cookie_regent";
						}
					}
					else
					{
						text = "yummy_cookie_silent";
					}
				}
				else
				{
					text = "yummy_cookie_ironclad";
				}
				return text;
			}
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x0024A6F1 File Offset: 0x002488F1
		protected override void AfterCloned()
		{
			base.AfterCloned();
			base.RelicIconChanged();
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x06005661 RID: 22113 RVA: 0x0024A6FF File Offset: 0x002488FF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(4));
			}
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x0024A70C File Offset: 0x0024890C
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForUpgrade(base.Owner, cardSelectorPrefs);
			List<CardModel> list = enumerable.ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x040024EE RID: 9454
		[Nullable(2)]
		private static CharacterModel _cachedRandomCharacter;
	}
}
