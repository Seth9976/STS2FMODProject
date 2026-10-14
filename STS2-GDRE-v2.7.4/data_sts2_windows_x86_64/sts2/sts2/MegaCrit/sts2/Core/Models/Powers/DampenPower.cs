using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000658 RID: 1624
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DampenPower : PowerModel
	{
		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x060057A4 RID: 22436 RVA: 0x0024D347 File Offset: 0x0024B547
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x060057A5 RID: 22437 RVA: 0x0024D34A File Offset: 0x0024B54A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.None;
			}
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x0024D34D File Offset: 0x0024B54D
		protected override object InitInternalData()
		{
			return new DampenPower.Data();
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x0024D354 File Offset: 0x0024B554
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			IEnumerable<CardModel> enumerable = base.Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.IsUpgraded);
			foreach (CardModel cardModel in enumerable)
			{
				base.GetInternalData<DampenPower.Data>().downgradedCardsToOldUpgradeLevels.Add(cardModel, cardModel.CurrentUpgradeLevel);
				CardCmd.Downgrade(cardModel);
			}
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x0024D3F8 File Offset: 0x0024B5F8
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				DampenPower.Data internalData = base.GetInternalData<DampenPower.Data>();
				if (internalData.casters.Contains(creature))
				{
					internalData.casters.Remove(creature);
					if (internalData.casters.Count == 0)
					{
						await PowerCmd.Remove(this);
					}
				}
			}
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x0024D44C File Offset: 0x0024B64C
		public override Task AfterRemoved(Creature oldOwner)
		{
			foreach (KeyValuePair<CardModel, int> keyValuePair in base.GetInternalData<DampenPower.Data>().downgradedCardsToOldUpgradeLevels)
			{
				CardModel cardModel;
				int num;
				keyValuePair.Deconstruct(out cardModel, out num);
				CardModel cardModel2 = cardModel;
				int num2 = num;
				for (int i = 0; i < num2; i++)
				{
					CardCmd.Upgrade(cardModel2, CardPreviewStyle.HorizontalLayout);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x0024D4CC File Offset: 0x0024B6CC
		public void AddCaster(Creature creature)
		{
			base.GetInternalData<DampenPower.Data>().casters.Add(creature);
		}

		// Token: 0x02001BAA RID: 7082
		[Nullable(0)]
		private class Data
		{
			// Token: 0x04006E1B RID: 28187
			public readonly HashSet<Creature> casters = new HashSet<Creature>();

			// Token: 0x04006E1C RID: 28188
			public readonly Dictionary<CardModel, int> downgradedCardsToOldUpgradeLevels = new Dictionary<CardModel, int>();
		}
	}
}
