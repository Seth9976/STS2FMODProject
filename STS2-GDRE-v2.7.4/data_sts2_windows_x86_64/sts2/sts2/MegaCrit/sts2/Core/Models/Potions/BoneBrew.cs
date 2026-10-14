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
	// Token: 0x02000749 RID: 1865
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BoneBrew : PotionModel
	{
		// Token: 0x170014CF RID: 5327
		// (get) Token: 0x06005D54 RID: 23892 RVA: 0x002579D3 File Offset: 0x00255BD3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170014D0 RID: 5328
		// (get) Token: 0x06005D55 RID: 23893 RVA: 0x002579D6 File Offset: 0x00255BD6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014D1 RID: 5329
		// (get) Token: 0x06005D56 RID: 23894 RVA: 0x002579D9 File Offset: 0x00255BD9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x06005D57 RID: 23895 RVA: 0x002579DC File Offset: 0x00255BDC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(15m));
			}
		}

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x06005D58 RID: 23896 RVA: 0x002579EF File Offset: 0x00255BEF
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x00257A14 File Offset: 0x00255C14
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("6876bd"));
			}
			await OstyCmd.Summon(choiceContext, target.Player, base.DynamicVars.Summon.BaseValue, this);
		}
	}
}
