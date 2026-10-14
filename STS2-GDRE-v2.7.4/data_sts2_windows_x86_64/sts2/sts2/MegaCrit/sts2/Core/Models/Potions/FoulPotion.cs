using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Events.Custom;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200075E RID: 1886
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FoulPotion : PotionModel
	{
		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06005DD5 RID: 24021 RVA: 0x002583A3 File Offset: 0x002565A3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Event;
			}
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06005DD6 RID: 24022 RVA: 0x002583A6 File Offset: 0x002565A6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.AnyTime;
			}
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x06005DD7 RID: 24023 RVA: 0x002583A9 File Offset: 0x002565A9
		public override TargetType TargetType
		{
			get
			{
				if (!CombatManager.Instance.IsInProgress)
				{
					return TargetType.TargetedNoCreature;
				}
				return TargetType.AllEnemies;
			}
		}

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x06005DD8 RID: 24024 RVA: 0x002583BA File Offset: 0x002565BA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(12m, ValueProp.Unpowered),
					new GoldVar(100)
				});
			}
		}

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x06005DD9 RID: 24025 RVA: 0x002583E4 File Offset: 0x002565E4
		public override bool PassesCustomUsabilityCheck
		{
			get
			{
				if (CombatManager.Instance.IsInProgress)
				{
					return true;
				}
				AbstractRoom currentRoom = base.Owner.RunState.CurrentRoom;
				bool flag;
				if (!(currentRoom is MerchantRoom))
				{
					EventRoom eventRoom = currentRoom as EventRoom;
					if (eventRoom != null)
					{
						EventModel canonicalEvent = eventRoom.CanonicalEvent;
						if (canonicalEvent is FakeMerchant)
						{
							goto IL_0040;
						}
					}
					flag = false;
					goto IL_0046;
				}
				IL_0040:
				flag = true;
				IL_0046:
				return flag && FoulPotion.GetFoulPotionMerchantTarget(currentRoom).Item1 != null;
			}
		}

		// Token: 0x06005DDA RID: 24026 RVA: 0x0025844C File Offset: 0x0025664C
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				Creature creature = base.Owner.Creature;
				DamageVar damage = base.DynamicVars.Damage;
				await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.Creatures.Where<Creature>((Creature c) => !c.IsPet), damage.BaseValue, damage.Props, creature, null, null);
			}
			else if (base.Owner.RunState.CurrentRoom is MerchantRoom)
			{
				NRun instance = NRun.Instance;
				NMerchantRoom nmerchantRoom = ((instance != null) ? instance.MerchantRoom : null);
				if (nmerchantRoom != null)
				{
					this.ShowPotionVfx(nmerchantRoom.MerchantButton);
					nmerchantRoom.FoulPotionThrown(this);
				}
				await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
			}
			else
			{
				EventRoom eventRoom = base.Owner.RunState.CurrentRoom as EventRoom;
				if (eventRoom != null && eventRoom.CanonicalEvent is FakeMerchant)
				{
					EventModel localMutableEvent = eventRoom.LocalMutableEvent;
					if (localMutableEvent != null)
					{
						NFakeMerchant nfakeMerchant = localMutableEvent.Node as NFakeMerchant;
						if (nfakeMerchant != null)
						{
							this.ShowPotionVfx(nfakeMerchant.MerchantButton);
							List<Task> list = new List<Task>();
							foreach (Player player in base.Owner.RunState.Players)
							{
								list.Add(((FakeMerchant)RunManager.Instance.EventSynchronizer.GetEventForPlayer(player)).FoulPotionThrown(this));
							}
							await Task.WhenAll(list);
						}
					}
				}
			}
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x00258498 File Offset: 0x00256698
		[NullableContext(2)]
		private void ShowPotionVfx(NMerchantButton merchantButton)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (merchantButton == null)
			{
				return;
			}
			string scenePath = SceneHelper.GetScenePath("vfx/vfx_slime_impact");
			Node2D node2D = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>(0L);
			merchantButton.GetParent().AddChildSafely(node2D);
			node2D.GlobalPosition = merchantButton.GlobalPosition;
		}

		/// <summary>
		/// Tries to grab the merchant button and its parent screen context from the Shop/Fake shop room.
		/// Used for Foul Potion targeting.
		/// </summary>
		// Token: 0x06005DDC RID: 24028 RVA: 0x002584E8 File Offset: 0x002566E8
		[return: TupleElementNames(new string[] { "button", "screenContext" })]
		[return: Nullable(new byte[] { 0, 2, 2 })]
		public static ValueTuple<NMerchantButton, Control> GetFoulPotionMerchantTarget(AbstractRoom room)
		{
			if (room.RoomType == RoomType.Shop)
			{
				NMerchantRoom instance = NMerchantRoom.Instance;
				if (instance != null)
				{
					NMerchantInventory nmerchantInventory = instance.Inventory;
					if (nmerchantInventory != null && !nmerchantInventory.IsOpen)
					{
						return new ValueTuple<NMerchantButton, Control>(instance.MerchantButton, instance);
					}
				}
			}
			EventRoom eventRoom = room as EventRoom;
			if (eventRoom != null && eventRoom.CanonicalEvent is FakeMerchant)
			{
				EventModel localMutableEvent = eventRoom.LocalMutableEvent;
				if (localMutableEvent != null)
				{
					NFakeMerchant nfakeMerchant = localMutableEvent.Node as NFakeMerchant;
					if (nfakeMerchant != null)
					{
						NMerchantInventory nmerchantInventory = nfakeMerchant.Inventory;
						if (nmerchantInventory != null && !nmerchantInventory.IsOpen)
						{
							return new ValueTuple<NMerchantButton, Control>(nfakeMerchant.MerchantButton, nfakeMerchant);
						}
					}
				}
			}
			return new ValueTuple<NMerchantButton, Control>(null, null);
		}
	}
}
