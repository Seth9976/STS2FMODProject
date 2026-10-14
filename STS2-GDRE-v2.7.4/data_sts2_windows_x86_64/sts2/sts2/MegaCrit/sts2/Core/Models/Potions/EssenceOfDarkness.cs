using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000757 RID: 1879
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EssenceOfDarkness : PotionModel
	{
		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06005DA7 RID: 23975 RVA: 0x00257FEB File Offset: 0x002561EB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x06005DA8 RID: 23976 RVA: 0x00257FEE File Offset: 0x002561EE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x06005DA9 RID: 23977 RVA: 0x00257FF1 File Offset: 0x002561F1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x06005DAA RID: 23978 RVA: 0x00257FF4 File Offset: 0x002561F4
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<DarkOrb>()
				});
			}
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x00258018 File Offset: 0x00256218
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player targetPlayer = target.Player;
			int count = targetPlayer.PlayerCombatState.OrbQueue.Capacity;
			for (int i = 0; i < count; i++)
			{
				await OrbCmd.Channel<DarkOrb>(choiceContext, targetPlayer);
			}
		}
	}
}
