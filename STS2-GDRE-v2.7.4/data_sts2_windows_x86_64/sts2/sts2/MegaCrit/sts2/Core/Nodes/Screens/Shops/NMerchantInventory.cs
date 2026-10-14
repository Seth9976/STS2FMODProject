using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	/// <summary>
	/// Manages the shop items for a shop.
	/// </summary>
	// Token: 0x0200027F RID: 639
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantInventory.cs")]
	public class NMerchantInventory : Control, IScreenContext
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x000E7085 File Offset: 0x000E5285
		// (set) Token: 0x060021DF RID: 8671 RVA: 0x000E708D File Offset: 0x000E528D
		[Nullable(2)]
		public MerchantInventory Inventory
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x000E7096 File Offset: 0x000E5296
		// (set) Token: 0x060021E1 RID: 8673 RVA: 0x000E709E File Offset: 0x000E529E
		public bool IsOpen { get; private set; }

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x000E70A7 File Offset: 0x000E52A7
		// (set) Token: 0x060021E3 RID: 8675 RVA: 0x000E70AF File Offset: 0x000E52AF
		[Nullable(1)]
		public NMerchantHand MerchantHand
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000E70B8 File Offset: 0x000E52B8
		public override void _Ready()
		{
			this._merchantDialogue = base.GetNode<NMerchantDialogue>("%Dialogue");
			this._merchantDialogue.Modulate = Colors.Transparent;
			this._slotsContainer = base.GetNode<Control>("%SlotsContainer");
			this._slotsContainer.Position = new Vector2(this._slotsContainer.Position.X, -1000f);
			this._backstop = base.GetNode<ColorRect>("Backstop");
			this.MerchantHand = base.GetNode<NMerchantHand>("%MerchantHand");
			this._characterCardContainer = base.GetNodeOrNull<Control>("%CharacterCards");
			this._colorlessCardContainer = base.GetNodeOrNull<Control>("%ColorlessCards");
			this._relicContainer = base.GetNodeOrNull<Control>("%Relics");
			this._potionContainer = base.GetNodeOrNull<Control>("%Potions");
			this._cardRemovalNode = base.GetNodeOrNull<NMerchantCardRemoval>("%MerchantCardRemoval");
			this._backButton = base.GetNode<NBackButton>("%BackButton");
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.Close();
			}), 0U);
			this._backButton.Disable();
			this._inputBlocker = base.GetNode<Control>("%InputBlocker");
			NGame.Instance.SetScreenShakeTarget(this);
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x000E7225 File Offset: 0x000E5425
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenUpdated;
			this.SubscribeToEntries();
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000E7244 File Offset: 0x000E5444
		public override void _ExitTree()
		{
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenUpdated;
			if (this.Inventory != null)
			{
				foreach (MerchantEntry merchantEntry in this.Inventory.AllEntries)
				{
					merchantEntry.PurchaseCompleted -= this.OnPurchaseCompleted;
					merchantEntry.PurchaseFailed -= this._merchantDialogue.ShowForPurchaseAttempt;
				}
			}
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000E72D8 File Offset: 0x000E54D8
		[NullableContext(1)]
		public void Initialize(MerchantInventory inventory, MerchantDialogueSet dialogue)
		{
			if (this.Inventory != null)
			{
				throw new InvalidOperationException("Merchant inventory already populated.");
			}
			this.Inventory = inventory;
			for (int i = 0; i < this.Inventory.CharacterCardEntries.Count; i++)
			{
				NMerchantCard child = this._characterCardContainer.GetChild<NMerchantCard>(i, false);
				child.Initialize(this);
				child.FillSlot(this.Inventory.CharacterCardEntries[i]);
			}
			for (int j = 0; j < this.Inventory.ColorlessCardEntries.Count; j++)
			{
				NMerchantCard child2 = this._colorlessCardContainer.GetChild<NMerchantCard>(j, false);
				child2.Initialize(this);
				child2.FillSlot(this.Inventory.ColorlessCardEntries[j]);
			}
			for (int k = 0; k < this.Inventory.RelicEntries.Count; k++)
			{
				NMerchantRelic child3 = this._relicContainer.GetChild<NMerchantRelic>(k, false);
				child3.Initialize(this);
				child3.FillSlot(this.Inventory.RelicEntries[k]);
			}
			for (int l = 0; l < this.Inventory.PotionEntries.Count; l++)
			{
				NMerchantPotion child4 = this._potionContainer.GetChild<NMerchantPotion>(l, false);
				child4.Initialize(this);
				child4.FillSlot(this.Inventory.PotionEntries[l]);
			}
			if (this.Inventory.CardRemovalEntry != null)
			{
				this._cardRemovalNode.Initialize(this);
				this._cardRemovalNode.FillSlot(this.Inventory.CardRemovalEntry);
			}
			this.SubscribeToEntries();
			this.UpdateNavigation();
			using (IEnumerator<NMerchantSlot> enumerator = this.GetAllSlots().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NMerchantSlot slot = enumerator.Current;
					slot.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
					{
						this._lastSlot = slot;
					}), 0U);
				}
			}
			this._merchantDialogue.Initialize(dialogue);
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000E74EC File Offset: 0x000E56EC
		public void Open()
		{
			if (!SaveManager.Instance.SeenFtue("merchant_ftue"))
			{
				SaveManager.Instance.MarkFtueAsComplete("merchant_ftue");
			}
			TaskHelper.RunSafely(this.DoOpenAnimation());
			base.MouseFilter = 0L;
			if (!this._isInputBlocked)
			{
				this._backButton.Enable();
			}
			foreach (NMerchantCard nmerchantCard in this.GetCardSlots())
			{
				nmerchantCard.OnInventoryOpened();
			}
			SfxCmd.Play("event:/sfx/npcs/merchant/merchant_welcome", 1f);
			this.IsOpen = true;
			ActiveScreenContext.Instance.Update();
			this._merchantDialogue.ShowOnInventoryOpen();
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000E75AC File Offset: 0x000E57AC
		private void SubscribeToEntries()
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			if (this.Inventory == null)
			{
				return;
			}
			foreach (MerchantEntry merchantEntry in this.Inventory.AllEntries)
			{
				merchantEntry.PurchaseCompleted += this.OnPurchaseCompleted;
				merchantEntry.PurchaseFailed += this._merchantDialogue.ShowForPurchaseAttempt;
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000E7634 File Offset: 0x000E5834
		[NullableContext(1)]
		private async Task DoOpenAnimation()
		{
			Tween inventoryTween = this._inventoryTween;
			if (inventoryTween != null)
			{
				inventoryTween.Kill();
			}
			this._inventoryTween = base.CreateTween().SetParallel(true);
			this._inventoryTween.TweenProperty(this._backstop, "modulate:a", 0.8f, 1.0).SetEase(2L).SetTrans(1L)
				.FromCurrent();
			this._inventoryTween.TweenProperty(this._slotsContainer, "position:y", 80f, 0.699999988079071).SetEase(1L).SetTrans(2L)
				.FromCurrent();
			bool flag = await this._inventoryTween.AwaitFinished(this);
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000E7678 File Offset: 0x000E5878
		private void Close()
		{
			this.MerchantHand.StopPointing(0f);
			Tween inventoryTween = this._inventoryTween;
			if (inventoryTween != null)
			{
				inventoryTween.Kill();
			}
			this._inventoryTween = base.CreateTween().SetParallel(true);
			this._inventoryTween.TweenProperty(this._backstop, "modulate:a", 0f, 0.800000011920929).SetEase(2L).SetTrans(1L)
				.FromCurrent();
			this._inventoryTween.TweenProperty(this._slotsContainer, "position:y", -1000f, 0.5).SetEase(1L).SetTrans(7L)
				.FromCurrent();
			base.MouseFilter = 2L;
			this._backButton.Disable();
			this._lastSlot = null;
			this.IsOpen = false;
			ActiveScreenContext.Instance.Update();
			this.EmitSignalInventoryClosed();
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000E7770 File Offset: 0x000E5970
		[NullableContext(1)]
		private void OnPurchaseCompleted(PurchaseStatus status, MerchantEntry entry)
		{
			this.UpdateNavigation();
			NMerchantSlot lastSlot = this.GetAllSlots().FirstOrDefault<NMerchantSlot>((NMerchantSlot s) => s.Entry == entry);
			if (lastSlot != null && !this._isInputBlocked)
			{
				NMerchantSlot nmerchantSlot = (from s in this.GetAllSlots()
					where s.Visible && s != lastSlot
					select s).MinBy<NMerchantSlot, float>((NMerchantSlot s) => (s.GlobalPosition - lastSlot.GlobalPosition).Length());
				if (nmerchantSlot != null)
				{
					nmerchantSlot.TryGrabFocus();
				}
			}
			SfxCmd.Play("event:/sfx/npcs/merchant/merchant_thank_yous", 1f);
			this._merchantDialogue.ShowForPurchaseAttempt(status);
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000E780A File Offset: 0x000E5A0A
		public void OnCardRemovalUsed()
		{
			this._cardRemovalNode.OnCardRemovalUsed();
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000E7818 File Offset: 0x000E5A18
		[NullableContext(1)]
		public IEnumerable<NMerchantSlot> GetAllSlots()
		{
			List<NMerchantSlot> list = new List<NMerchantSlot>();
			list.AddRange(this.GetCardSlots());
			if (this._relicContainer != null)
			{
				list.AddRange(this._relicContainer.GetChildren(false).OfType<NMerchantRelic>());
			}
			if (this._potionContainer != null)
			{
				list.AddRange(this._potionContainer.GetChildren(false).OfType<NMerchantPotion>());
			}
			if (this._cardRemovalNode != null)
			{
				list.Add(this._cardRemovalNode);
			}
			return list;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000E788C File Offset: 0x000E5A8C
		[NullableContext(1)]
		private IEnumerable<NMerchantCard> GetCardSlots()
		{
			IEnumerable<Node>[] array = new IEnumerable<Node>[2];
			int num = 0;
			Control characterCardContainer = this._characterCardContainer;
			array[num] = ((characterCardContainer != null) ? characterCardContainer.GetChildren(false) : null) ?? new Array<Node>();
			int num2 = 1;
			Control colorlessCardContainer = this._colorlessCardContainer;
			array[num2] = ((colorlessCardContainer != null) ? colorlessCardContainer.GetChildren(false) : null) ?? new Array<Node>();
			return array.SelectMany<IEnumerable<Node>, Node>((IEnumerable<Node> n) => n).OfType<NMerchantCard>();
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000E7906 File Offset: 0x000E5B06
		protected virtual void UpdateNavigation()
		{
			this.UpdateHorizontalNavigation();
			this.UpdateVerticalNavigation();
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x000E7914 File Offset: 0x000E5B14
		private void UpdateHorizontalNavigation()
		{
			Control characterCardContainer = this._characterCardContainer;
			List<NMerchantSlot> list;
			if (characterCardContainer == null)
			{
				list = null;
			}
			else
			{
				list = (from c in characterCardContainer.GetChildren(false).OfType<NMerchantSlot>()
					where c.Visible
					select c).ToList<NMerchantSlot>();
			}
			List<NMerchantSlot> list2 = list ?? new List<NMerchantSlot>();
			Control colorlessCardContainer = this._colorlessCardContainer;
			List<NMerchantSlot> list3;
			if (colorlessCardContainer == null)
			{
				list3 = null;
			}
			else
			{
				list3 = (from c in colorlessCardContainer.GetChildren(false).OfType<NMerchantSlot>()
					where c.Visible
					select c).ToList<NMerchantSlot>();
			}
			List<NMerchantSlot> list4 = list3 ?? new List<NMerchantSlot>();
			Control relicContainer = this._relicContainer;
			List<NMerchantSlot> list5;
			if (relicContainer == null)
			{
				list5 = null;
			}
			else
			{
				list5 = (from c in relicContainer.GetChildren(false).OfType<NMerchantSlot>()
					where c.Visible
					select c).ToList<NMerchantSlot>();
			}
			List<NMerchantSlot> list6 = list5 ?? new List<NMerchantSlot>();
			Control potionContainer = this._potionContainer;
			List<NMerchantSlot> list7;
			if (potionContainer == null)
			{
				list7 = null;
			}
			else
			{
				list7 = (from c in potionContainer.GetChildren(false).OfType<NMerchantSlot>()
					where c.Visible
					select c).ToList<NMerchantSlot>();
			}
			List<NMerchantSlot> list8 = list7 ?? new List<NMerchantSlot>();
			List<NMerchantSlot> list9 = list2.ToList<NMerchantSlot>();
			IEnumerable<NMerchantSlot> enumerable = list4.Concat<NMerchantSlot>(list6);
			IEnumerable<NMerchantSlot> enumerable3;
			if (this._cardRemovalNode == null)
			{
				IEnumerable<NMerchantSlot> enumerable2 = Array.Empty<NMerchantSlot>();
				enumerable3 = enumerable2;
			}
			else
			{
				IEnumerable<NMerchantSlot> enumerable2 = new <>z__ReadOnlySingleElementList<NMerchantSlot>(this._cardRemovalNode);
				enumerable3 = enumerable2;
			}
			List<NMerchantSlot> list10 = enumerable.Concat<NMerchantSlot>(enumerable3).ToList<NMerchantSlot>();
			IEnumerable<NMerchantSlot> enumerable4 = list4.Concat<NMerchantSlot>(list8);
			IEnumerable<NMerchantSlot> enumerable5;
			if (this._cardRemovalNode == null)
			{
				IEnumerable<NMerchantSlot> enumerable2 = Array.Empty<NMerchantSlot>();
				enumerable5 = enumerable2;
			}
			else
			{
				IEnumerable<NMerchantSlot> enumerable2 = new <>z__ReadOnlySingleElementList<NMerchantSlot>(this._cardRemovalNode);
				enumerable5 = enumerable2;
			}
			List<NMerchantSlot> list11 = enumerable4.Concat<NMerchantSlot>(enumerable5).ToList<NMerchantSlot>();
			for (int i = 0; i < list9.Count; i++)
			{
				list9[i].FocusNeighborLeft = ((i > 0) ? list9[i - 1].GetPath() : list9[i].GetPath());
				list9[i].FocusNeighborRight = ((i < list9.Count - 1) ? list9[i + 1].GetPath() : list9[i].GetPath());
			}
			for (int j = 0; j < list11.Count; j++)
			{
				list11[j].FocusNeighborLeft = ((j > 0) ? list11[j - 1].GetPath() : list11[j].GetPath());
				list11[j].FocusNeighborRight = ((j < list11.Count - 1) ? list11[j + 1].GetPath() : list11[j].GetPath());
			}
			for (int k = 0; k < list10.Count; k++)
			{
				list10[k].FocusNeighborLeft = ((k > 0) ? list10[k - 1].GetPath() : list10[k].GetPath());
				list10[k].FocusNeighborRight = ((k < list10.Count - 1) ? list10[k + 1].GetPath() : list10[k].GetPath());
			}
			if (list6.Count == 0 && list8.Count > 0)
			{
				if (this._cardRemovalNode != null)
				{
					this._cardRemovalNode.FocusNeighborLeft = list8.Last<NMerchantSlot>().GetPath();
				}
				if (list4.Count > 0)
				{
					list4.Last<NMerchantSlot>().FocusNeighborRight = list8.First<NMerchantSlot>().GetPath();
				}
			}
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x000E7C9C File Offset: 0x000E5E9C
		private void UpdateVerticalNavigation()
		{
			Control characterCardContainer = this._characterCardContainer;
			List<NMerchantSlot> list = ((characterCardContainer != null) ? characterCardContainer.GetChildren(false).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : null) ?? new List<NMerchantSlot>();
			Control colorlessCardContainer = this._colorlessCardContainer;
			List<NMerchantSlot> list2 = ((colorlessCardContainer != null) ? colorlessCardContainer.GetChildren(false).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : null) ?? new List<NMerchantSlot>();
			Control relicContainer = this._relicContainer;
			List<NMerchantSlot> list3 = ((relicContainer != null) ? relicContainer.GetChildren(false).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : null) ?? new List<NMerchantSlot>();
			Control potionContainer = this._potionContainer;
			List<NMerchantSlot> list4 = ((potionContainer != null) ? potionContainer.GetChildren(false).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : null) ?? new List<NMerchantSlot>();
			List<NMerchantSlot> list5 = list.ToList<NMerchantSlot>();
			IEnumerable<NMerchantSlot> enumerable = list2.Concat<NMerchantSlot>(list3);
			IEnumerable<NMerchantSlot> enumerable3;
			if (this._cardRemovalNode == null)
			{
				IEnumerable<NMerchantSlot> enumerable2 = Array.Empty<NMerchantSlot>();
				enumerable3 = enumerable2;
			}
			else
			{
				IEnumerable<NMerchantSlot> enumerable2 = new <>z__ReadOnlySingleElementList<NMerchantSlot>(this._cardRemovalNode);
				enumerable3 = enumerable2;
			}
			List<NMerchantSlot> list6 = enumerable.Concat<NMerchantSlot>(enumerable3).ToList<NMerchantSlot>();
			IEnumerable<NMerchantSlot> enumerable4 = list2.Concat<NMerchantSlot>(list4);
			IEnumerable<NMerchantSlot> enumerable5;
			if (this._cardRemovalNode == null)
			{
				IEnumerable<NMerchantSlot> enumerable2 = Array.Empty<NMerchantSlot>();
				enumerable5 = enumerable2;
			}
			else
			{
				IEnumerable<NMerchantSlot> enumerable2 = new <>z__ReadOnlySingleElementList<NMerchantSlot>(this._cardRemovalNode);
				enumerable5 = enumerable2;
			}
			List<NMerchantSlot> list7 = enumerable4.Concat<NMerchantSlot>(enumerable5).ToList<NMerchantSlot>();
			int i = 0;
			while (i < list5.Count)
			{
				list5[i].FocusNeighborTop = list5[i].GetPath();
				if (list6.Count <= 0)
				{
					goto IL_0163;
				}
				Control closestStockedSlot = this.GetClosestStockedSlot(i, list6);
				if (closestStockedSlot == null)
				{
					goto IL_0163;
				}
				list5[i].FocusNeighborBottom = closestStockedSlot.GetPath();
				IL_01A6:
				i++;
				continue;
				IL_0163:
				Control closestStockedSlot2 = this.GetClosestStockedSlot(i, list7);
				if (closestStockedSlot2 != null)
				{
					list5[i].FocusNeighborBottom = closestStockedSlot2.GetPath();
					goto IL_01A6;
				}
				list5[i].FocusNeighborBottom = list5[i].GetPath();
				goto IL_01A6;
			}
			int j = 0;
			while (j < list7.Count)
			{
				if (list6.Count <= 0)
				{
					goto IL_01F3;
				}
				Control closestStockedSlot3 = this.GetClosestStockedSlot(j, list6);
				if (closestStockedSlot3 == null)
				{
					goto IL_01F3;
				}
				list7[j].FocusNeighborTop = closestStockedSlot3.GetPath();
				IL_0236:
				list7[j].FocusNeighborBottom = list7[j].GetPath();
				j++;
				continue;
				IL_01F3:
				Control closestStockedSlot4 = this.GetClosestStockedSlot(j, list5);
				if (closestStockedSlot4 != null)
				{
					list7[j].FocusNeighborTop = closestStockedSlot4.GetPath();
					goto IL_0236;
				}
				list7[j].FocusNeighborTop = list7[j].GetPath();
				goto IL_0236;
			}
			int k = 0;
			while (k < list6.Count)
			{
				if (list5.Count <= 0)
				{
					goto IL_029F;
				}
				Control closestStockedSlot5 = this.GetClosestStockedSlot(k, list5);
				if (closestStockedSlot5 == null)
				{
					goto IL_029F;
				}
				list6[k].FocusNeighborTop = closestStockedSlot5.GetPath();
				IL_02BB:
				if (list7.Count <= 0)
				{
					goto IL_02EC;
				}
				Control closestStockedSlot6 = this.GetClosestStockedSlot(k, list7);
				if (closestStockedSlot6 == null)
				{
					goto IL_02EC;
				}
				list6[k].FocusNeighborBottom = closestStockedSlot6.GetPath();
				IL_0308:
				k++;
				continue;
				IL_02EC:
				list6[k].FocusNeighborBottom = list6[k].GetPath();
				goto IL_0308;
				IL_029F:
				list6[k].FocusNeighborTop = list6[k].GetPath();
				goto IL_02BB;
			}
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000E7FC5 File Offset: 0x000E61C5
		public void BlockInput()
		{
			this._isInputBlocked = true;
			this._inputBlocker.MouseFilter = 0L;
			NHotkeyManager.Instance.AddBlockingScreen(this._inputBlocker);
			if (this._backButton.IsEnabled)
			{
				this._backButton.Disable();
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000E8003 File Offset: 0x000E6203
		public void UnblockInput()
		{
			this._isInputBlocked = false;
			this._inputBlocker.MouseFilter = 2L;
			NHotkeyManager.Instance.RemoveBlockingScreen(this._inputBlocker);
			if (!this._backButton.IsEnabled)
			{
				this._backButton.Enable();
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000E8044 File Offset: 0x000E6244
		[NullableContext(1)]
		[return: Nullable(2)]
		private Control GetClosestStockedSlot(int idx, List<NMerchantSlot> row)
		{
			NMerchantSlot nmerchantSlot = row[Math.Min(idx, row.Count - 1)];
			if (nmerchantSlot.Entry.IsStocked)
			{
				return nmerchantSlot;
			}
			int num = row.IndexOf(nmerchantSlot);
			int num2 = num - 1;
			int num3 = num + 1;
			while (num2 >= 0 || num3 < row.Count)
			{
				if (num3 < row.Count)
				{
					if (row[num3].Entry.IsStocked)
					{
						return row[num3];
					}
					num3++;
				}
				if (num2 >= 0)
				{
					if (row[num2].Entry.IsStocked)
					{
						return row[num2];
					}
					num2--;
				}
			}
			return null;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000E80E0 File Offset: 0x000E62E0
		private void OnActiveScreenUpdated()
		{
			this.UpdateControllerNavEnabled<NMerchantInventory>();
			if (ActiveScreenContext.Instance.IsCurrent(this))
			{
				if (this._characterCardContainer != null && NControllerManager.Instance.IsUsingController && this._inventoryTween != null && this._inventoryTween.IsRunning())
				{
					float num = 80f - this._slotsContainer.Position.Y;
					this.MerchantHand.PointAtTarget(this._characterCardContainer.GetChild<NMerchantCard>(0, false), Vector2.Down * num);
				}
				if (!this._isInputBlocked)
				{
					this._backButton.Enable();
					return;
				}
			}
			else
			{
				this._backButton.Disable();
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060021F7 RID: 8695 RVA: 0x000E8184 File Offset: 0x000E6384
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._isInputBlocked)
				{
					return this._inputBlocker;
				}
				NMerchantSlot lastSlot = this._lastSlot;
				if (lastSlot != null)
				{
					MerchantEntry entry = lastSlot.Entry;
					if (entry != null && entry.IsStocked)
					{
						return this._lastSlot;
					}
				}
				return this.GetAllSlots().FirstOrDefault<NMerchantSlot>((NMerchantSlot s) => s.Entry.IsStocked);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021F8 RID: 8696 RVA: 0x000E81F0 File Offset: 0x000E63F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NMerchantInventory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.SubscribeToEntries, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.OnCardRemovalUsed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.UpdateHorizontalNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.UpdateVerticalNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.BlockInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.UnblockInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantInventory.MethodName.OnActiveScreenUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021F9 RID: 8697 RVA: 0x000E846C File Offset: 0x000E666C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantInventory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.SubscribeToEntries && args.Count == 0)
			{
				this.SubscribeToEntries();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.Close && args.Count == 0)
			{
				this.Close();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.OnCardRemovalUsed && args.Count == 0)
			{
				this.OnCardRemovalUsed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.UpdateHorizontalNavigation && args.Count == 0)
			{
				this.UpdateHorizontalNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.UpdateVerticalNavigation && args.Count == 0)
			{
				this.UpdateVerticalNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.BlockInput && args.Count == 0)
			{
				this.BlockInput();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.UnblockInput && args.Count == 0)
			{
				this.UnblockInput();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantInventory.MethodName.OnActiveScreenUpdated && args.Count == 0)
			{
				this.OnActiveScreenUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060021FA RID: 8698 RVA: 0x000E8664 File Offset: 0x000E6864
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantInventory.MethodName._Ready || (ref method) == NMerchantInventory.MethodName._EnterTree || (ref method) == NMerchantInventory.MethodName._ExitTree || (ref method) == NMerchantInventory.MethodName.Open || (ref method) == NMerchantInventory.MethodName.SubscribeToEntries || (ref method) == NMerchantInventory.MethodName.Close || (ref method) == NMerchantInventory.MethodName.OnCardRemovalUsed || (ref method) == NMerchantInventory.MethodName.UpdateNavigation || (ref method) == NMerchantInventory.MethodName.UpdateHorizontalNavigation || (ref method) == NMerchantInventory.MethodName.UpdateVerticalNavigation || (ref method) == NMerchantInventory.MethodName.BlockInput || (ref method) == NMerchantInventory.MethodName.UnblockInput || (ref method) == NMerchantInventory.MethodName.OnActiveScreenUpdated || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060021FB RID: 8699 RVA: 0x000E873C File Offset: 0x000E693C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantInventory.PropertyName.IsOpen)
			{
				this.IsOpen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName.MerchantHand)
			{
				this.MerchantHand = VariantUtils.ConvertTo<NMerchantHand>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._characterCardContainer)
			{
				this._characterCardContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._colorlessCardContainer)
			{
				this._colorlessCardContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._relicContainer)
			{
				this._relicContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._potionContainer)
			{
				this._potionContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._cardRemovalNode)
			{
				this._cardRemovalNode = VariantUtils.ConvertTo<NMerchantCardRemoval>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._merchantDialogue)
			{
				this._merchantDialogue = VariantUtils.ConvertTo<NMerchantDialogue>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._inventoryTween)
			{
				this._inventoryTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._slotsContainer)
			{
				this._slotsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._inputBlocker)
			{
				this._inputBlocker = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._isInputBlocked)
			{
				this._isInputBlocked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._lastSlot)
			{
				this._lastSlot = VariantUtils.ConvertTo<NMerchantSlot>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060021FC RID: 8700 RVA: 0x000E88E8 File Offset: 0x000E6AE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantInventory.PropertyName.IsOpen)
			{
				bool isOpen = this.IsOpen;
				value = VariantUtils.CreateFrom<bool>(ref isOpen);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName.MerchantHand)
			{
				NMerchantHand merchantHand = this.MerchantHand;
				value = VariantUtils.CreateFrom<NMerchantHand>(ref merchantHand);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._characterCardContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterCardContainer);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._colorlessCardContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._colorlessCardContainer);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._relicContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicContainer);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._potionContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionContainer);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._cardRemovalNode)
			{
				value = VariantUtils.CreateFrom<NMerchantCardRemoval>(ref this._cardRemovalNode);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._merchantDialogue)
			{
				value = VariantUtils.CreateFrom<NMerchantDialogue>(ref this._merchantDialogue);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._inventoryTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._inventoryTween);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._slotsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._slotsContainer);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._inputBlocker)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._inputBlocker);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._isInputBlocked)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isInputBlocked);
				return true;
			}
			if ((ref name) == NMerchantInventory.PropertyName._lastSlot)
			{
				value = VariantUtils.CreateFrom<NMerchantSlot>(ref this._lastSlot);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021FD RID: 8701 RVA: 0x000E8B08 File Offset: 0x000E6D08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMerchantInventory.PropertyName._characterCardContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._colorlessCardContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._relicContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._potionContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._cardRemovalNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._merchantDialogue, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._inventoryTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._slotsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._inputBlocker, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMerchantInventory.PropertyName._isInputBlocked, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName._lastSlot, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMerchantInventory.PropertyName.IsOpen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName.MerchantHand, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantInventory.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021FE RID: 8702 RVA: 0x000E8D2C File Offset: 0x000E6F2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isOpen = NMerchantInventory.PropertyName.IsOpen;
			bool isOpen2 = this.IsOpen;
			info.AddProperty(isOpen, Variant.From<bool>(ref isOpen2));
			StringName merchantHand = NMerchantInventory.PropertyName.MerchantHand;
			NMerchantHand merchantHand2 = this.MerchantHand;
			info.AddProperty(merchantHand, Variant.From<NMerchantHand>(ref merchantHand2));
			info.AddProperty(NMerchantInventory.PropertyName._characterCardContainer, Variant.From<Control>(ref this._characterCardContainer));
			info.AddProperty(NMerchantInventory.PropertyName._colorlessCardContainer, Variant.From<Control>(ref this._colorlessCardContainer));
			info.AddProperty(NMerchantInventory.PropertyName._relicContainer, Variant.From<Control>(ref this._relicContainer));
			info.AddProperty(NMerchantInventory.PropertyName._potionContainer, Variant.From<Control>(ref this._potionContainer));
			info.AddProperty(NMerchantInventory.PropertyName._cardRemovalNode, Variant.From<NMerchantCardRemoval>(ref this._cardRemovalNode));
			info.AddProperty(NMerchantInventory.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NMerchantInventory.PropertyName._merchantDialogue, Variant.From<NMerchantDialogue>(ref this._merchantDialogue));
			info.AddProperty(NMerchantInventory.PropertyName._inventoryTween, Variant.From<Tween>(ref this._inventoryTween));
			info.AddProperty(NMerchantInventory.PropertyName._slotsContainer, Variant.From<Control>(ref this._slotsContainer));
			info.AddProperty(NMerchantInventory.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
			info.AddProperty(NMerchantInventory.PropertyName._inputBlocker, Variant.From<Control>(ref this._inputBlocker));
			info.AddProperty(NMerchantInventory.PropertyName._isInputBlocked, Variant.From<bool>(ref this._isInputBlocked));
			info.AddProperty(NMerchantInventory.PropertyName._lastSlot, Variant.From<NMerchantSlot>(ref this._lastSlot));
			info.AddSignalEventDelegate(NMerchantInventory.SignalName.InventoryClosed, this.backing_InventoryClosed);
		}

		/// <inheritdoc />
		// Token: 0x060021FF RID: 8703 RVA: 0x000E8EA4 File Offset: 0x000E70A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantInventory.PropertyName.IsOpen, ref variant))
			{
				this.IsOpen = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantInventory.PropertyName.MerchantHand, ref variant2))
			{
				this.MerchantHand = variant2.As<NMerchantHand>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._characterCardContainer, ref variant3))
			{
				this._characterCardContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._colorlessCardContainer, ref variant4))
			{
				this._colorlessCardContainer = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._relicContainer, ref variant5))
			{
				this._relicContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._potionContainer, ref variant6))
			{
				this._potionContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._cardRemovalNode, ref variant7))
			{
				this._cardRemovalNode = variant7.As<NMerchantCardRemoval>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._backButton, ref variant8))
			{
				this._backButton = variant8.As<NBackButton>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._merchantDialogue, ref variant9))
			{
				this._merchantDialogue = variant9.As<NMerchantDialogue>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._inventoryTween, ref variant10))
			{
				this._inventoryTween = variant10.As<Tween>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._slotsContainer, ref variant11))
			{
				this._slotsContainer = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._backstop, ref variant12))
			{
				this._backstop = variant12.As<ColorRect>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._inputBlocker, ref variant13))
			{
				this._inputBlocker = variant13.As<Control>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._isInputBlocked, ref variant14))
			{
				this._isInputBlocked = variant14.As<bool>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMerchantInventory.PropertyName._lastSlot, ref variant15))
			{
				this._lastSlot = variant15.As<NMerchantSlot>();
			}
			NMerchantInventory.InventoryClosedEventHandler inventoryClosedEventHandler;
			if (info.TryGetSignalEventDelegate<NMerchantInventory.InventoryClosedEventHandler>(NMerchantInventory.SignalName.InventoryClosed, ref inventoryClosedEventHandler))
			{
				this.backing_InventoryClosed = inventoryClosedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002200 RID: 8704 RVA: 0x000E9074 File Offset: 0x000E7274
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMerchantInventory.SignalName.InventoryClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantInventory.InventoryClosedEventHandler" />
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06002201 RID: 8705 RVA: 0x000E90B8 File Offset: 0x000E72B8
		// (remove) Token: 0x06002202 RID: 8706 RVA: 0x000E90D1 File Offset: 0x000E72D1
		public event NMerchantInventory.InventoryClosedEventHandler InventoryClosed
		{
			add
			{
				this.backing_InventoryClosed = (NMerchantInventory.InventoryClosedEventHandler)Delegate.Combine(this.backing_InventoryClosed, value);
			}
			remove
			{
				this.backing_InventoryClosed = (NMerchantInventory.InventoryClosedEventHandler)Delegate.Remove(this.backing_InventoryClosed, value);
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000E90EA File Offset: 0x000E72EA
		protected void EmitSignalInventoryClosed()
		{
			base.EmitSignal(NMerchantInventory.SignalName.InventoryClosed, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06002204 RID: 8708 RVA: 0x000E90FD File Offset: 0x000E72FD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NMerchantInventory.SignalName.InventoryClosed) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NMerchantInventory.InventoryClosedEventHandler inventoryClosedEventHandler = this.backing_InventoryClosed;
			if (inventoryClosedEventHandler == null)
			{
				return;
			}
			inventoryClosedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06002205 RID: 8709 RVA: 0x000E912E File Offset: 0x000E732E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NMerchantInventory.SignalName.InventoryClosed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000D3C RID: 3388
		private const float _openPosition = 80f;

		// Token: 0x04000D3D RID: 3389
		private const float _closedPosition = -1000f;

		// Token: 0x04000D3E RID: 3390
		[Nullable(2)]
		private Control _characterCardContainer;

		// Token: 0x04000D3F RID: 3391
		[Nullable(2)]
		private Control _colorlessCardContainer;

		// Token: 0x04000D40 RID: 3392
		[Nullable(2)]
		protected Control _relicContainer;

		// Token: 0x04000D41 RID: 3393
		[Nullable(2)]
		private Control _potionContainer;

		// Token: 0x04000D42 RID: 3394
		[Nullable(2)]
		private NMerchantCardRemoval _cardRemovalNode;

		// Token: 0x04000D43 RID: 3395
		[Nullable(1)]
		private NBackButton _backButton;

		// Token: 0x04000D44 RID: 3396
		[Nullable(1)]
		private NMerchantDialogue _merchantDialogue;

		// Token: 0x04000D45 RID: 3397
		[Nullable(2)]
		private Tween _inventoryTween;

		// Token: 0x04000D46 RID: 3398
		[Nullable(1)]
		private Control _slotsContainer;

		// Token: 0x04000D47 RID: 3399
		[Nullable(1)]
		private ColorRect _backstop;

		// Token: 0x04000D48 RID: 3400
		[Nullable(1)]
		private Control _inputBlocker;

		// Token: 0x04000D49 RID: 3401
		private bool _isInputBlocked;

		// Token: 0x04000D4A RID: 3402
		[Nullable(2)]
		private NMerchantSlot _lastSlot;

		// Token: 0x04000D4E RID: 3406
		private NMerchantInventory.InventoryClosedEventHandler backing_InventoryClosed;

		// Token: 0x02001328 RID: 4904
		// (Invoke) Token: 0x06009E7B RID: 40571
		[Signal]
		public delegate void InventoryClosedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001329 RID: 4905
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A6C RID: 19052
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004A6D RID: 19053
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004A6E RID: 19054
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x04004A6F RID: 19055
			public static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'SubscribeToEntries' method.
			/// </summary>
			// Token: 0x04004A70 RID: 19056
			public static readonly StringName SubscribeToEntries = "SubscribeToEntries";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x04004A71 RID: 19057
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'OnCardRemovalUsed' method.
			/// </summary>
			// Token: 0x04004A72 RID: 19058
			public static readonly StringName OnCardRemovalUsed = "OnCardRemovalUsed";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x04004A73 RID: 19059
			public static readonly StringName UpdateNavigation = "UpdateNavigation";

			/// <summary>
			/// Cached name for the 'UpdateHorizontalNavigation' method.
			/// </summary>
			// Token: 0x04004A74 RID: 19060
			public static readonly StringName UpdateHorizontalNavigation = "UpdateHorizontalNavigation";

			/// <summary>
			/// Cached name for the 'UpdateVerticalNavigation' method.
			/// </summary>
			// Token: 0x04004A75 RID: 19061
			public static readonly StringName UpdateVerticalNavigation = "UpdateVerticalNavigation";

			/// <summary>
			/// Cached name for the 'BlockInput' method.
			/// </summary>
			// Token: 0x04004A76 RID: 19062
			public static readonly StringName BlockInput = "BlockInput";

			/// <summary>
			/// Cached name for the 'UnblockInput' method.
			/// </summary>
			// Token: 0x04004A77 RID: 19063
			public static readonly StringName UnblockInput = "UnblockInput";

			/// <summary>
			/// Cached name for the 'OnActiveScreenUpdated' method.
			/// </summary>
			// Token: 0x04004A78 RID: 19064
			public static readonly StringName OnActiveScreenUpdated = "OnActiveScreenUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200132A RID: 4906
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsOpen' property.
			/// </summary>
			// Token: 0x04004A79 RID: 19065
			public static readonly StringName IsOpen = "IsOpen";

			/// <summary>
			/// Cached name for the 'MerchantHand' property.
			/// </summary>
			// Token: 0x04004A7A RID: 19066
			public static readonly StringName MerchantHand = "MerchantHand";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004A7B RID: 19067
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_characterCardContainer' field.
			/// </summary>
			// Token: 0x04004A7C RID: 19068
			public static readonly StringName _characterCardContainer = "_characterCardContainer";

			/// <summary>
			/// Cached name for the '_colorlessCardContainer' field.
			/// </summary>
			// Token: 0x04004A7D RID: 19069
			public static readonly StringName _colorlessCardContainer = "_colorlessCardContainer";

			/// <summary>
			/// Cached name for the '_relicContainer' field.
			/// </summary>
			// Token: 0x04004A7E RID: 19070
			public static readonly StringName _relicContainer = "_relicContainer";

			/// <summary>
			/// Cached name for the '_potionContainer' field.
			/// </summary>
			// Token: 0x04004A7F RID: 19071
			public static readonly StringName _potionContainer = "_potionContainer";

			/// <summary>
			/// Cached name for the '_cardRemovalNode' field.
			/// </summary>
			// Token: 0x04004A80 RID: 19072
			public static readonly StringName _cardRemovalNode = "_cardRemovalNode";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04004A81 RID: 19073
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_merchantDialogue' field.
			/// </summary>
			// Token: 0x04004A82 RID: 19074
			public static readonly StringName _merchantDialogue = "_merchantDialogue";

			/// <summary>
			/// Cached name for the '_inventoryTween' field.
			/// </summary>
			// Token: 0x04004A83 RID: 19075
			public static readonly StringName _inventoryTween = "_inventoryTween";

			/// <summary>
			/// Cached name for the '_slotsContainer' field.
			/// </summary>
			// Token: 0x04004A84 RID: 19076
			public static readonly StringName _slotsContainer = "_slotsContainer";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x04004A85 RID: 19077
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_inputBlocker' field.
			/// </summary>
			// Token: 0x04004A86 RID: 19078
			public static readonly StringName _inputBlocker = "_inputBlocker";

			/// <summary>
			/// Cached name for the '_isInputBlocked' field.
			/// </summary>
			// Token: 0x04004A87 RID: 19079
			public static readonly StringName _isInputBlocked = "_isInputBlocked";

			/// <summary>
			/// Cached name for the '_lastSlot' field.
			/// </summary>
			// Token: 0x04004A88 RID: 19080
			public static readonly StringName _lastSlot = "_lastSlot";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200132B RID: 4907
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'InventoryClosed' signal.
			/// </summary>
			// Token: 0x04004A89 RID: 19081
			public static readonly StringName InventoryClosed = "InventoryClosed";
		}
	}
}
