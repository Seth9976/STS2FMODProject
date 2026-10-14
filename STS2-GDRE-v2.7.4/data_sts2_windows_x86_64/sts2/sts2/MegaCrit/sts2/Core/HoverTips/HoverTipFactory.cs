using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.HoverTips
{
	// Token: 0x02000BFA RID: 3066
	[NullableContext(1)]
	[Nullable(0)]
	public static class HoverTipFactory
	{
		// Token: 0x06007EBA RID: 32442 RVA: 0x002AEC38 File Offset: 0x002ACE38
		public static IEnumerable<IHoverTip> FromEnchantment<[Nullable(0)] T>(int amount = 1) where T : EnchantmentModel
		{
			EnchantmentModel enchantmentModel = ModelDb.Enchantment<T>().ToMutable();
			enchantmentModel.Amount = amount;
			enchantmentModel.RecalculateValues();
			return enchantmentModel.HoverTips;
		}

		// Token: 0x06007EBB RID: 32443 RVA: 0x002AEC68 File Offset: 0x002ACE68
		public static IEnumerable<IHoverTip> FromAffliction<[Nullable(0)] T>(int amount = 1) where T : AfflictionModel
		{
			AfflictionModel afflictionModel = ModelDb.Affliction<T>().ToMutable();
			afflictionModel.Amount = amount;
			return afflictionModel.HoverTips;
		}

		// Token: 0x06007EBC RID: 32444 RVA: 0x002AEC92 File Offset: 0x002ACE92
		public static IHoverTip FromKeyword(CardKeyword keyword)
		{
			if (!HoverTipFactory._keywordHoverTips.ContainsKey(keyword))
			{
				HoverTipFactory._keywordHoverTips[keyword] = new HoverTip(keyword.GetTitle(), keyword.GetDescription(), null);
			}
			return HoverTipFactory._keywordHoverTips[keyword];
		}

		// Token: 0x06007EBD RID: 32445 RVA: 0x002AECD0 File Offset: 0x002ACED0
		public static IHoverTip FromPower<[Nullable(0)] T>(int? amount = null) where T : PowerModel
		{
			PowerModel powerModel = ModelDb.Power<T>();
			return HoverTipFactory.FromPower(powerModel, amount);
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x002AECEF File Offset: 0x002ACEEF
		public static IHoverTip FromPower(PowerModel model, int? amount = null)
		{
			return model.GetDumbHoverTip(amount);
		}

		/// <summary>
		/// Generate a HoverTip-sized preview of the specified power along with the power's hovertips.
		/// </summary>
		// Token: 0x06007EBF RID: 32447 RVA: 0x002AECFD File Offset: 0x002ACEFD
		public static IEnumerable<IHoverTip> FromPowerWithPowerHoverTips<[Nullable(0)] T>(int? amount = null) where T : PowerModel
		{
			return new IHoverTip[] { HoverTipFactory.FromPower<T>(amount) }.Concat<IHoverTip>(ModelDb.Power<T>().HoverTips);
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x002AED24 File Offset: 0x002ACF24
		public static IHoverTip FromPotion<[Nullable(0)] T>() where T : PotionModel
		{
			PotionModel potionModel = ModelDb.Potion<T>();
			return HoverTipFactory.FromPotion(potionModel);
		}

		// Token: 0x06007EC1 RID: 32449 RVA: 0x002AED42 File Offset: 0x002ACF42
		public static IHoverTip FromPotion(PotionModel model)
		{
			HoverTipFactory._potionHoverTips.TryAdd(model.Id, model.HoverTip);
			return HoverTipFactory._potionHoverTips[model.Id];
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x002AED70 File Offset: 0x002ACF70
		public static IHoverTip FromOrb<[Nullable(0)] T>() where T : OrbModel
		{
			return ModelDb.Orb<T>().DumbHoverTip;
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x002AED88 File Offset: 0x002ACF88
		public static IEnumerable<IHoverTip> FromRelic<[Nullable(0)] T>() where T : RelicModel
		{
			RelicModel relicModel = ModelDb.Relic<T>();
			return HoverTipFactory.FromRelic(relicModel);
		}

		// Token: 0x06007EC4 RID: 32452 RVA: 0x002AEDA8 File Offset: 0x002ACFA8
		public static IEnumerable<IHoverTip> FromRelicExcludingItself<[Nullable(0)] T>() where T : RelicModel
		{
			RelicModel relicModel = ModelDb.Relic<T>();
			return HoverTipFactory.FromRelicExcludingItself(relicModel);
		}

		// Token: 0x06007EC5 RID: 32453 RVA: 0x002AEDC6 File Offset: 0x002ACFC6
		public static IEnumerable<IHoverTip> FromRelic(RelicModel relic)
		{
			return relic.HoverTips;
		}

		// Token: 0x06007EC6 RID: 32454 RVA: 0x002AEDCE File Offset: 0x002ACFCE
		public static IEnumerable<IHoverTip> FromRelicExcludingItself(RelicModel relic)
		{
			return relic.HoverTipsExcludingRelic;
		}

		/// <summary>
		/// Generate a HoverTip-sized preview of the specified card along with the card's hovertips.
		/// </summary>
		/// <typeparam name="T">CardModel subclass</typeparam>
		/// <returns>HoverTip card preview</returns>
		// Token: 0x06007EC7 RID: 32455 RVA: 0x002AEDD6 File Offset: 0x002ACFD6
		public static IEnumerable<IHoverTip> FromCardWithCardHoverTips<[Nullable(0)] T>(bool upgrade = false) where T : CardModel
		{
			return new IHoverTip[] { HoverTipFactory.FromCard<T>(upgrade) }.Concat<IHoverTip>(ModelDb.Card<T>().HoverTips);
		}

		/// <summary>
		/// Generate a HoverTip-sized preview of the specified card.
		/// </summary>
		/// <param name="upgrade">Whether the card should appear upgraded.</param>
		/// <typeparam name="T">CardModel subclass</typeparam>
		/// <returns>HoverTip card preview</returns>
		// Token: 0x06007EC8 RID: 32456 RVA: 0x002AEDFB File Offset: 0x002ACFFB
		public static IHoverTip FromCard<[Nullable(0)] T>(bool upgrade = false) where T : CardModel
		{
			return HoverTipFactory.FromCard(ModelDb.Card<T>(), upgrade);
		}

		/// <summary>
		/// Generate a HoverTip-sized preview of the specified card.
		/// Note: You should usually use the generic version of this method. Only use this version if you need to modify
		/// the base version of the card (like to show an upgrade or something).
		/// </summary>
		/// <param name="card">CardModel to preview</param>
		/// <param name="upgrade">Whether the card should appear upgraded.</param>
		/// <returns>HoverTip card preview</returns>
		// Token: 0x06007EC9 RID: 32457 RVA: 0x002AEE0D File Offset: 0x002AD00D
		public static IHoverTip FromCard(CardModel card, bool upgrade = false)
		{
			if (upgrade)
			{
				card = (CardModel)card.MutableClone();
				card.UpgradeInternal();
				card.FinalizeUpgradeInternal();
			}
			return new CardHoverTip(card);
		}

		// Token: 0x06007ECA RID: 32458 RVA: 0x002AEE34 File Offset: 0x002AD034
		public static IHoverTip Static(StaticHoverTip tip, params DynamicVar[] vars)
		{
			string text = StringHelper.Slugify(tip.ToString());
			LocString locString = HoverTipFactory.L10NStatic(text + ".title");
			LocString locString2 = HoverTipFactory.L10NStatic(text + ".description");
			foreach (DynamicVar dynamicVar in vars)
			{
				locString.Add(dynamicVar);
				locString2.Add(dynamicVar);
			}
			return new HoverTip(locString, locString2, null);
		}

		// Token: 0x06007ECB RID: 32459 RVA: 0x002AEEAD File Offset: 0x002AD0AD
		public static IHoverTip ForEnergy(CardModel card)
		{
			return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath(card));
		}

		// Token: 0x06007ECC RID: 32460 RVA: 0x002AEEBA File Offset: 0x002AD0BA
		public static IHoverTip ForEnergy(PotionModel potion)
		{
			return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath(potion));
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x002AEEC7 File Offset: 0x002AD0C7
		public static IHoverTip ForEnergy(PowerModel power)
		{
			return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath(power));
		}

		// Token: 0x06007ECE RID: 32462 RVA: 0x002AEED4 File Offset: 0x002AD0D4
		public static IHoverTip ForEnergy(Player player)
		{
			return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath(player.Character.CardPool));
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x002AEEEB File Offset: 0x002AD0EB
		public static IHoverTip ForEnergy(RelicModel relic)
		{
			return HoverTipFactory.ForEnergyWithIconPath(EnergyIconHelper.GetPath(relic));
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x002AEEF8 File Offset: 0x002AD0F8
		private static IHoverTip ForEnergyWithIconPath(string path)
		{
			string localCharacterEnergyIconPrefix = RunManager.Instance.GetLocalCharacterEnergyIconPrefix();
			return new HoverTip(HoverTipFactory.L10NStatic("ENERGY.title"), HoverTipFactory.L10NStatic("ENERGY.description"), PreloadManager.Cache.GetTexture2D((localCharacterEnergyIconPrefix != null) ? EnergyIconHelper.GetPath(localCharacterEnergyIconPrefix) : path));
		}

		// Token: 0x06007ED1 RID: 32465 RVA: 0x002AEF44 File Offset: 0x002AD144
		public static IEnumerable<IHoverTip> FromForge()
		{
			List<IHoverTip> list = new List<IHoverTip>();
			list.Add(HoverTipFactory.Static(StaticHoverTip.Forge, Array.Empty<DynamicVar>()));
			list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<SovereignBlade>(false));
			return new <>z__ReadOnlyList<IHoverTip>(list);
		}

		// Token: 0x06007ED2 RID: 32466 RVA: 0x002AEF6E File Offset: 0x002AD16E
		private static LocString L10NStatic(string entry)
		{
			return new LocString("static_hover_tips", entry);
		}

		// Token: 0x04002A5E RID: 10846
		private static readonly Dictionary<CardKeyword, HoverTip> _keywordHoverTips = new Dictionary<CardKeyword, HoverTip>();

		// Token: 0x04002A5F RID: 10847
		private static readonly Dictionary<ModelId, HoverTip> _potionHoverTips = new Dictionary<ModelId, HoverTip>();
	}
}
