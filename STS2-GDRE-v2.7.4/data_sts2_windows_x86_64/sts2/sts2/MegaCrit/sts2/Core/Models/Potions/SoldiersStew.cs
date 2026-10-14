using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200077A RID: 1914
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoldiersStew : PotionModel
	{
		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x06005E90 RID: 24208 RVA: 0x00259423 File Offset: 0x00257623
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x06005E91 RID: 24209 RVA: 0x00259426 File Offset: 0x00257626
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x06005E92 RID: 24210 RVA: 0x00259429 File Offset: 0x00257629
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x06005E93 RID: 24211 RVA: 0x0025942C File Offset: 0x0025762C
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayStatic, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00259440 File Offset: 0x00257640
		protected override Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, new Color("e6a045"));
			}
			List<CardModel> list = target.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Strike)).ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				CardModel cardModel2 = cardModel;
				int baseReplayCount = cardModel2.BaseReplayCount;
				cardModel2.BaseReplayCount = baseReplayCount + 1;
			}
			return Task.CompletedTask;
		}
	}
}
