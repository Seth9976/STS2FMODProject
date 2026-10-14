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
	// Token: 0x02000763 RID: 1891
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GigantificationPotion : PotionModel
	{
		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x06005DF8 RID: 24056 RVA: 0x002587BF File Offset: 0x002569BF
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x06005DF9 RID: 24057 RVA: 0x002587C2 File Offset: 0x002569C2
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x06005DFA RID: 24058 RVA: 0x002587C5 File Offset: 0x002569C5
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x06005DFB RID: 24059 RVA: 0x002587C8 File Offset: 0x002569C8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<GigantificationPower>(1m));
			}
		}

		// Token: 0x06005DFC RID: 24060 RVA: 0x002587DC File Offset: 0x002569DC
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color(Colors.Red, 1f));
			}
			await PowerCmd.Apply<GigantificationPower>(choiceContext, target, base.DynamicVars["GigantificationPower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
