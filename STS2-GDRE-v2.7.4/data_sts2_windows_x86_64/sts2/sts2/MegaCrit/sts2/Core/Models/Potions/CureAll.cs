using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200074F RID: 1871
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CureAll : PotionModel
	{
		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x06005D79 RID: 23929 RVA: 0x00257CB3 File Offset: 0x00255EB3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x06005D7A RID: 23930 RVA: 0x00257CB6 File Offset: 0x00255EB6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x06005D7B RID: 23931 RVA: 0x00257CB9 File Offset: 0x00255EB9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x06005D7C RID: 23932 RVA: 0x00257CBC File Offset: 0x00255EBC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x06005D7D RID: 23933 RVA: 0x00257CDB File Offset: 0x00255EDB
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x00257CE8 File Offset: 0x00255EE8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("a296a3"));
			}
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, target.Player);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, target.Player, false);
		}
	}
}
