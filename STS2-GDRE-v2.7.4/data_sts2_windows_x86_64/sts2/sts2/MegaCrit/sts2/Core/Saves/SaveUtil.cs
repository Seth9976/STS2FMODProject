using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000AD RID: 173
	[NullableContext(1)]
	[Nullable(0)]
	public static class SaveUtil
	{
		/// <summary>
		/// Obtains an enchantment by ID, or falls back to the deprecated event if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x060007FA RID: 2042 RVA: 0x000401A7 File Offset: 0x0003E3A7
		public static EventModel EventOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<EventModel>(id) ?? ModelDb.Event<DeprecatedEvent>();
		}

		/// <summary>
		/// Obtains an ancient event by ID, or falls back to the deprecated ancient event if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x060007FB RID: 2043 RVA: 0x000401B8 File Offset: 0x0003E3B8
		public static AncientEventModel AncientEventOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<AncientEventModel>(id) ?? ModelDb.Event<DeprecatedAncientEvent>();
		}

		/// <summary>
		/// Obtains an encounter by ID, or falls back to the deprecated encounter if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x060007FC RID: 2044 RVA: 0x000401C9 File Offset: 0x0003E3C9
		public static EncounterModel EncounterOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<EncounterModel>(id) ?? ModelDb.Encounter<DeprecatedEncounter>();
		}

		/// <summary>
		/// Obtains a card by ID, or falls back to the deprecated card if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x060007FD RID: 2045 RVA: 0x000401DA File Offset: 0x0003E3DA
		public static CardModel CardOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<CardModel>(id) ?? ModelDb.Card<DeprecatedCard>();
		}

		/// <summary>
		/// Obtains a relic by ID, or falls back to the deprecated relic if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x060007FE RID: 2046 RVA: 0x000401EB File Offset: 0x0003E3EB
		public static RelicModel RelicOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<RelicModel>(id) ?? ModelDb.Relic<DeprecatedRelic>();
		}

		/// <summary>
		/// Obtains a potion by ID, or falls back to the deprecated potion if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x060007FF RID: 2047 RVA: 0x000401FC File Offset: 0x0003E3FC
		public static PotionModel PotionOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<PotionModel>(id) ?? ModelDb.Potion<DeprecatedPotion>();
		}

		/// <summary>
		/// Obtains a modifier by ID, or falls back to the deprecated modifier if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x06000800 RID: 2048 RVA: 0x0004020D File Offset: 0x0003E40D
		public static ModifierModel ModifierOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<ModifierModel>(id) ?? ModelDb.Modifier<DeprecatedModifier>();
		}

		/// <summary>
		/// Obtains an enchantment by ID, or falls back to the deprecated enchantment if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x06000801 RID: 2049 RVA: 0x0004021E File Offset: 0x0003E41E
		public static EnchantmentModel EnchantmentOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<EnchantmentModel>(id) ?? ModelDb.Enchantment<DeprecatedEnchantment>();
		}

		/// <summary>
		/// Obtains a monster by ID, or falls back to the deprecated monster if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x06000802 RID: 2050 RVA: 0x0004022F File Offset: 0x0003E42F
		public static MonsterModel MonsterOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<MonsterModel>(id) ?? ModelDb.Monster<DeprecatedMonster>();
		}

		/// <summary>
		/// Obtains a character by ID, or falls back to the deprecated character if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x06000803 RID: 2051 RVA: 0x00040240 File Offset: 0x0003E440
		public static CharacterModel CharacterOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<CharacterModel>(id) ?? ModelDb.Character<DeprecatedCharacter>();
		}

		/// <summary>
		/// Obtains a act by ID, or falls back to the deprecated act if it's not found.
		/// This should only be used in save loading where it's okay to fall back to deprecated content.
		/// </summary>
		// Token: 0x06000804 RID: 2052 RVA: 0x00040251 File Offset: 0x0003E451
		public static ActModel ActOrDeprecated(ModelId id)
		{
			return ModelDb.GetByIdOrNull<ActModel>(id) ?? ModelDb.Act<DeprecatedAct>();
		}
	}
}
