using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F6 RID: 1270
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArchaicTooth : RelicModel
	{
		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06004ED3 RID: 20179 RVA: 0x0023C18C File Offset: 0x0023A38C
		private static Dictionary<ModelId, CardModel> TranscendenceUpgrades
		{
			get
			{
				return new Dictionary<ModelId, CardModel>
				{
					{
						ModelDb.Card<Bash>().Id,
						ModelDb.Card<Break>()
					},
					{
						ModelDb.Card<Neutralize>().Id,
						ModelDb.Card<Suppress>()
					},
					{
						ModelDb.Card<Unleash>().Id,
						ModelDb.Card<Protector>()
					},
					{
						ModelDb.Card<FallingStar>().Id,
						ModelDb.Card<MeteorShower>()
					},
					{
						ModelDb.Card<Dualcast>().Id,
						ModelDb.Card<Quadcast>()
					}
				};
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004ED4 RID: 20180 RVA: 0x0023C207 File Offset: 0x0023A407
		public static List<CardModel> TranscendenceCards
		{
			get
			{
				return ArchaicTooth.TranscendenceUpgrades.Values.ToList<CardModel>();
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06004ED5 RID: 20181 RVA: 0x0023C218 File Offset: 0x0023A418
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06004ED6 RID: 20182 RVA: 0x0023C21B File Offset: 0x0023A41B
		// (set) Token: 0x06004ED7 RID: 20183 RVA: 0x0023C223 File Offset: 0x0023A423
		[Nullable(2)]
		[SavedProperty]
		public SerializableCard StarterCard
		{
			[NullableContext(2)]
			get
			{
				return this._serializableStarterCard;
			}
			[NullableContext(2)]
			private set
			{
				base.AssertMutable();
				this._serializableStarterCard = value;
				this.UpdateHoverTips();
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004ED8 RID: 20184 RVA: 0x0023C238 File Offset: 0x0023A438
		// (set) Token: 0x06004ED9 RID: 20185 RVA: 0x0023C240 File Offset: 0x0023A440
		[Nullable(2)]
		[SavedProperty]
		public SerializableCard AncientCard
		{
			[NullableContext(2)]
			get
			{
				return this._serializableAncientCard;
			}
			[NullableContext(2)]
			private set
			{
				base.AssertMutable();
				this._serializableAncientCard = value;
				this.UpdateHoverTips();
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06004EDA RID: 20186 RVA: 0x0023C255 File Offset: 0x0023A455
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("StarterCard", ""),
					new StringVar("AncientCard", "")
				});
			}
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x0023C286 File Offset: 0x0023A486
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this._extraHoverTips = new List<IHoverTip>();
		}

		/// <summary>
		/// Sets up transcendent starter card for given player.
		/// </summary>
		/// <param name="player">Player we are getting the trasncendent card for.</param>
		/// <returns>Returns false if the player doesn't have the base starter card to transcend.</returns>
		// Token: 0x06004EDC RID: 20188 RVA: 0x0023C29C File Offset: 0x0023A49C
		public bool SetupForPlayer(Player player)
		{
			base.AssertMutable();
			CardModel transcendenceStarterCard = this.GetTranscendenceStarterCard(player);
			if (transcendenceStarterCard != null)
			{
				this.StarterCard = transcendenceStarterCard.ToSerializable();
				this.AncientCard = this.GetTranscendenceTransformedCard(transcendenceStarterCard).ToSerializable();
				this.UpdateHoverTips();
				return true;
			}
			return false;
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x0023C2E1 File Offset: 0x0023A4E1
		public void SetupForTests(SerializableCard starterCard, SerializableCard ancientCard)
		{
			base.AssertMutable();
			this.StarterCard = starterCard;
			this.AncientCard = ancientCard;
			this.UpdateHoverTips();
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x0023C300 File Offset: 0x0023A500
		private void UpdateHoverTips()
		{
			this._extraHoverTips.Clear();
			if (this.StarterCard != null)
			{
				CardModel cardModel = CardModel.FromSerializable(this.StarterCard);
				this._extraHoverTips.AddRange(cardModel.HoverTips);
				this._extraHoverTips.Add(HoverTipFactory.FromCard(cardModel, false));
				((StringVar)base.DynamicVars["StarterCard"]).StringValue = cardModel.Title;
			}
			if (this.AncientCard != null)
			{
				CardModel cardModel2 = CardModel.FromSerializable(this.AncientCard);
				this._extraHoverTips.AddRange(cardModel2.HoverTips);
				this._extraHoverTips.Add(HoverTipFactory.FromCard(cardModel2, false));
				((StringVar)base.DynamicVars["AncientCard"]).StringValue = cardModel2.Title;
			}
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x0023C3C6 File Offset: 0x0023A5C6
		[return: Nullable(2)]
		private CardModel GetTranscendenceStarterCard(Player player)
		{
			return player.Deck.Cards.FirstOrDefault<CardModel>((CardModel c) => ArchaicTooth.TranscendenceUpgrades.ContainsKey(c.Id));
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x0023C3F8 File Offset: 0x0023A5F8
		private CardModel GetTranscendenceTransformedCard(CardModel starterCard)
		{
			CardModel cardModel;
			if (ArchaicTooth.TranscendenceUpgrades.TryGetValue(starterCard.Id, out cardModel))
			{
				CardModel cardModel2 = starterCard.Owner.RunState.CreateCard(cardModel, starterCard.Owner);
				if (starterCard.IsUpgraded)
				{
					CardCmd.Upgrade(cardModel2, CardPreviewStyle.HorizontalLayout);
				}
				if (starterCard.Enchantment != null)
				{
					EnchantmentModel enchantmentModel = (EnchantmentModel)starterCard.Enchantment.MutableClone();
					CardCmd.Enchant(enchantmentModel, cardModel2, enchantmentModel.Amount);
				}
				return cardModel2;
			}
			return base.Owner.RunState.CreateCard<Doubt>(starterCard.Owner);
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06004EE1 RID: 20193 RVA: 0x0023C484 File Offset: 0x0023A684
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return this._extraHoverTips;
			}
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x0023C48C File Offset: 0x0023A68C
		public override async Task AfterObtained()
		{
			CardModel transcendenceStarterCard = this.GetTranscendenceStarterCard(base.Owner);
			await CardCmd.Transform(transcendenceStarterCard, this.GetTranscendenceTransformedCard(transcendenceStarterCard), CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x0400241B RID: 9243
		private const string _starterCardKey = "StarterCard";

		// Token: 0x0400241C RID: 9244
		private const string _ancientCardKey = "AncientCard";

		// Token: 0x0400241D RID: 9245
		[Nullable(2)]
		private SerializableCard _serializableStarterCard;

		// Token: 0x0400241E RID: 9246
		[Nullable(2)]
		private SerializableCard _serializableAncientCard;

		// Token: 0x0400241F RID: 9247
		private List<IHoverTip> _extraHoverTips = new List<IHoverTip>();
	}
}
