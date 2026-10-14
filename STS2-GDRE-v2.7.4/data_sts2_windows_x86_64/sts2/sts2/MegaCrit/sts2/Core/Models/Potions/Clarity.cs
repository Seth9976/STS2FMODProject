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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200074B RID: 1867
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Clarity : PotionModel
	{
		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x06005D61 RID: 23905 RVA: 0x00257AE3 File Offset: 0x00255CE3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x06005D62 RID: 23906 RVA: 0x00257AE6 File Offset: 0x00255CE6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x06005D63 RID: 23907 RVA: 0x00257AE9 File Offset: 0x00255CE9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x06005D64 RID: 23908 RVA: 0x00257AEC File Offset: 0x00255CEC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<ClarityPower>(3m),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x00257B10 File Offset: 0x00255D10
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("ac54b3"));
			}
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, target.Player, false);
			await PowerCmd.Apply<ClarityPower>(choiceContext, target, base.DynamicVars["ClarityPower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
