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
	// Token: 0x02000745 RID: 1861
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BeetleJuice : PotionModel
	{
		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x06005D3C RID: 23868 RVA: 0x002577B3 File Offset: 0x002559B3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x06005D3D RID: 23869 RVA: 0x002577B6 File Offset: 0x002559B6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x06005D3E RID: 23870 RVA: 0x002577B9 File Offset: 0x002559B9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x06005D3F RID: 23871 RVA: 0x002577BC File Offset: 0x002559BC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("DamageDecrease", 30m),
					new RepeatVar(4)
				});
			}
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x002577E8 File Offset: 0x002559E8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("65cf81"));
			}
			await PowerCmd.Apply<ShrinkPower>(choiceContext, target, base.DynamicVars.Repeat.BaseValue, base.Owner.Creature, null, false);
		}

		// Token: 0x04002531 RID: 9521
		private const string _damageDecreaseKey = "DamageDecrease";
	}
}
