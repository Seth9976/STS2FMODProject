using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	/// <summary>
	/// Manages the starting UI flow for a merchant room. i.e. opening the inventory and proceeding to the map.
	/// This does not include the merchant items themselves. Those are handled by <see cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantInventory" />.
	/// </summary>
	// Token: 0x0200036E RID: 878
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Rooms/NMerchantRoom.cs")]
	public class NMerchantRoom : Control, IScreenContext, IRoomWithProceedButton
	{
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06003360 RID: 13152 RVA: 0x0017E6ED File Offset: 0x0017C8ED
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NMerchantRoom._scenePath);
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06003361 RID: 13153 RVA: 0x0017E6F9 File Offset: 0x0017C8F9
		[Nullable(2)]
		public static NMerchantRoom Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.MerchantRoom;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003362 RID: 13154 RVA: 0x0017E70B File Offset: 0x0017C90B
		public NProceedButton ProceedButton
		{
			get
			{
				return this._proceedButton;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06003363 RID: 13155 RVA: 0x0017E713 File Offset: 0x0017C913
		// (set) Token: 0x06003364 RID: 13156 RVA: 0x0017E71B File Offset: 0x0017C91B
		public MerchantRoom Room { get; private set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06003365 RID: 13157 RVA: 0x0017E724 File Offset: 0x0017C924
		// (set) Token: 0x06003366 RID: 13158 RVA: 0x0017E72C File Offset: 0x0017C92C
		public NMerchantInventory Inventory { get; private set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06003367 RID: 13159 RVA: 0x0017E735 File Offset: 0x0017C935
		// (set) Token: 0x06003368 RID: 13160 RVA: 0x0017E73D File Offset: 0x0017C93D
		public NMerchantButton MerchantButton { get; private set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x0017E746 File Offset: 0x0017C946
		public IReadOnlyList<NMerchantCharacter> PlayerVisuals
		{
			get
			{
				return this._playerVisuals;
			}
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x0017E750 File Offset: 0x0017C950
		[return: Nullable(2)]
		public static NMerchantRoom Create(MerchantRoom room, IReadOnlyList<Player> players)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NMerchantRoom nmerchantRoom = PreloadManager.Cache.GetScene(NMerchantRoom._scenePath).Instantiate<NMerchantRoom>(0L);
			nmerchantRoom.Room = room;
			nmerchantRoom._players.AddRange(players);
			nmerchantRoom._dialogue = MerchantRoom.Dialogue;
			return nmerchantRoom;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x0017E79C File Offset: 0x0017C99C
		public override void _Ready()
		{
			this._proceedButton = base.GetNode<NProceedButton>("%ProceedButton");
			this._proceedButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.HideScreen)), 0U);
			this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
			this._proceedButton.SetPulseState(false);
			this._proceedButton.Enable();
			this.MerchantButton = base.GetNode<NMerchantButton>("%MerchantButton");
			this.MerchantButton.IsLocalPlayerDead = LocalContext.GetMe(this._players).Creature.IsDead;
			this.MerchantButton.PlayerDeadLines = this._dialogue.PlayerDeadLines;
			this.MerchantButton.Connect(NMerchantButton.SignalName.MerchantOpened, Callable.From<NMerchantButton>(new Action<NMerchantButton>(this.OnMerchantOpened)), 0U);
			this.Inventory = base.GetNode<NMerchantInventory>("%Inventory");
			this.Inventory.MouseFilter = 2L;
			this.Inventory.Initialize(this.Room.GetLocalInventory(), this._dialogue);
			this._characterContainer = base.GetNode<Control>("%CharacterContainer");
			NMapScreen.Instance.SetTravelEnabled(true);
			NGame.Instance.SetScreenShakeTarget(this);
			this.AfterRoomIsLoaded();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x0017E8EC File Offset: 0x0017CAEC
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenUpdated;
			NMapScreen.Instance.Connect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
			NMapScreen.Instance.Connect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x0017E954 File Offset: 0x0017CB54
		public override void _ExitTree()
		{
			NGame.Instance.ClearScreenShakeTarget();
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenUpdated;
			NMapScreen.Instance.Disconnect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)));
			NMapScreen.Instance.Disconnect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)));
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x0017E9C4 File Offset: 0x0017CBC4
		public void FoulPotionThrown(FoulPotion potion)
		{
			SfxCmd.Play("event:/sfx/npcs/merchant/merchant_thank_yous", 1f);
			LocString locString = Rng.Chaotic.NextItem<LocString>(this._dialogue.FoulPotionLines);
			if (locString == null)
			{
				return;
			}
			if (this.MerchantButton.PlayDialogue(locString, 2.0) == null)
			{
				return;
			}
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.ScreenRumble(ShakeStrength.Medium, ShakeDuration.Short, RumbleStyle.Rumble);
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x0017EA26 File Offset: 0x0017CC26
		private void ToggleMerchantTrack()
		{
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.ToggleMerchantTrack();
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x0017EA38 File Offset: 0x0017CC38
		private void AfterRoomIsLoaded()
		{
			Player me = LocalContext.GetMe(this._players);
			this._players.Remove(me);
			this._players.Insert(0, me);
			int num = Mathf.CeilToInt(Mathf.Sqrt((float)this._players.Count));
			for (int i = 0; i < num; i++)
			{
				float num2 = -140f * (float)i;
				for (int j = 0; j < num; j++)
				{
					int num3 = i * num + j;
					if (num3 >= this._players.Count)
					{
						break;
					}
					NMerchantCharacter nmerchantCharacter = PreloadManager.Cache.GetScene(this._players[num3].Character.MerchantAnimPath).Instantiate<NMerchantCharacter>(0L);
					this._characterContainer.AddChildSafely(nmerchantCharacter);
					this._characterContainer.MoveChildSafely(nmerchantCharacter, 0);
					nmerchantCharacter.Position = new Vector2(num2, -50f * (float)i);
					if (i > 0)
					{
						nmerchantCharacter.Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
					}
					num2 -= 275f;
					this._playerVisuals.Add(nmerchantCharacter);
				}
			}
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x0017EB60 File Offset: 0x0017CD60
		private void HideScreen(NButton _)
		{
			if (this.MerchantFtueCheck())
			{
				return;
			}
			NMapScreen.Instance.Open(false);
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x0017EB77 File Offset: 0x0017CD77
		private bool MerchantFtueCheck()
		{
			if (!SaveManager.Instance.SeenFtue("merchant_ftue"))
			{
				NModalContainer.Instance.Add(NMerchantFtue.Create(this), true);
				SaveManager.Instance.MarkFtueAsComplete("merchant_ftue");
				return true;
			}
			return false;
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x0017EBAD File Offset: 0x0017CDAD
		private void OnMerchantOpened(NMerchantButton _)
		{
			this.OpenInventory();
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x0017EBB8 File Offset: 0x0017CDB8
		public void OpenInventory()
		{
			if (this.Inventory.IsOpen)
			{
				return;
			}
			this._proceedButton.Disable();
			this.Inventory.Open();
			this.MerchantButton.Disable();
			this.Inventory.Connect(NMerchantInventory.SignalName.InventoryClosed, Callable.From(delegate
			{
				this.MerchantButton.Enable();
				this._proceedButton.Enable();
				this._proceedButton.SetPulseState(true);
			}), 4U);
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x0017EC18 File Offset: 0x0017CE18
		private void OnActiveScreenUpdated()
		{
			this.UpdateControllerNavEnabled<NMerchantRoom>();
			if (ActiveScreenContext.Instance.IsCurrent(this))
			{
				this.MerchantButton.Enable();
				this._proceedButton.Enable();
				return;
			}
			this.MerchantButton.Disable();
			this._proceedButton.Disable();
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06003376 RID: 13174 RVA: 0x0017EC65 File Offset: 0x0017CE65
		public Control DefaultFocusedControl
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003377 RID: 13175 RVA: 0x0017EC68 File Offset: 0x0017CE68
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NMerchantRoom.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRoom.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRoom.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRoom.MethodName.ToggleMerchantTrack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRoom.MethodName.AfterRoomIsLoaded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRoom.MethodName.HideScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantRoom.MethodName.MerchantFtueCheck, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRoom.MethodName.OnMerchantOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantRoom.MethodName.OpenInventory, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003378 RID: 13176 RVA: 0x0017EEB4 File Offset: 0x0017D0B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantRoom.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName.ToggleMerchantTrack && args.Count == 0)
			{
				this.ToggleMerchantTrack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName.AfterRoomIsLoaded && args.Count == 0)
			{
				this.AfterRoomIsLoaded();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName.HideScreen && args.Count == 1)
			{
				this.HideScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName.MerchantFtueCheck && args.Count == 0)
			{
				bool flag = this.MerchantFtueCheck();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName.OnMerchantOpened && args.Count == 1)
			{
				this.OnMerchantOpened(VariantUtils.ConvertTo<NMerchantButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName.OpenInventory && args.Count == 0)
			{
				this.OpenInventory();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRoom.MethodName.OnActiveScreenUpdated && args.Count == 0)
			{
				this.OnActiveScreenUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003379 RID: 13177 RVA: 0x0017F060 File Offset: 0x0017D260
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantRoom.MethodName._Ready || (ref method) == NMerchantRoom.MethodName._EnterTree || (ref method) == NMerchantRoom.MethodName._ExitTree || (ref method) == NMerchantRoom.MethodName.ToggleMerchantTrack || (ref method) == NMerchantRoom.MethodName.AfterRoomIsLoaded || (ref method) == NMerchantRoom.MethodName.HideScreen || (ref method) == NMerchantRoom.MethodName.MerchantFtueCheck || (ref method) == NMerchantRoom.MethodName.OnMerchantOpened || (ref method) == NMerchantRoom.MethodName.OpenInventory || (ref method) == NMerchantRoom.MethodName.OnActiveScreenUpdated || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600337A RID: 13178 RVA: 0x0017F10C File Offset: 0x0017D30C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantRoom.PropertyName.Inventory)
			{
				this.Inventory = VariantUtils.ConvertTo<NMerchantInventory>(ref value);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName.MerchantButton)
			{
				this.MerchantButton = VariantUtils.ConvertTo<NMerchantButton>(ref value);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName._proceedButton)
			{
				this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName._characterContainer)
			{
				this._characterContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600337B RID: 13179 RVA: 0x0017F190 File Offset: 0x0017D390
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantRoom.PropertyName.ProceedButton)
			{
				NProceedButton proceedButton = this.ProceedButton;
				value = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName.Inventory)
			{
				NMerchantInventory inventory = this.Inventory;
				value = VariantUtils.CreateFrom<NMerchantInventory>(ref inventory);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName.MerchantButton)
			{
				NMerchantButton merchantButton = this.MerchantButton;
				value = VariantUtils.CreateFrom<NMerchantButton>(ref merchantButton);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName._proceedButton)
			{
				value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
				return true;
			}
			if ((ref name) == NMerchantRoom.PropertyName._characterContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600337C RID: 13180 RVA: 0x0017F274 File Offset: 0x0017D474
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMerchantRoom.PropertyName._proceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantRoom.PropertyName.ProceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantRoom.PropertyName._characterContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantRoom.PropertyName.Inventory, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantRoom.PropertyName.MerchantButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantRoom.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600337D RID: 13181 RVA: 0x0017F350 File Offset: 0x0017D550
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName inventory = NMerchantRoom.PropertyName.Inventory;
			NMerchantInventory inventory2 = this.Inventory;
			info.AddProperty(inventory, Variant.From<NMerchantInventory>(ref inventory2));
			StringName merchantButton = NMerchantRoom.PropertyName.MerchantButton;
			NMerchantButton merchantButton2 = this.MerchantButton;
			info.AddProperty(merchantButton, Variant.From<NMerchantButton>(ref merchantButton2));
			info.AddProperty(NMerchantRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
			info.AddProperty(NMerchantRoom.PropertyName._characterContainer, Variant.From<Control>(ref this._characterContainer));
		}

		/// <inheritdoc />
		// Token: 0x0600337E RID: 13182 RVA: 0x0017F3C4 File Offset: 0x0017D5C4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantRoom.PropertyName.Inventory, ref variant))
			{
				this.Inventory = variant.As<NMerchantInventory>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantRoom.PropertyName.MerchantButton, ref variant2))
			{
				this.MerchantButton = variant2.As<NMerchantButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantRoom.PropertyName._proceedButton, ref variant3))
			{
				this._proceedButton = variant3.As<NProceedButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantRoom.PropertyName._characterContainer, ref variant4))
			{
				this._characterContainer = variant4.As<Control>();
			}
		}

		// Token: 0x040013E9 RID: 5097
		private const float _animVariance = 0.5f;

		// Token: 0x040013EA RID: 5098
		private static readonly string _scenePath = SceneHelper.GetScenePath("rooms/merchant_room");

		// Token: 0x040013EB RID: 5099
		private readonly List<Player> _players = new List<Player>();

		// Token: 0x040013EC RID: 5100
		private MerchantDialogueSet _dialogue;

		// Token: 0x040013ED RID: 5101
		private NProceedButton _proceedButton;

		// Token: 0x040013EE RID: 5102
		private Control _characterContainer;

		// Token: 0x040013F2 RID: 5106
		private readonly List<NMerchantCharacter> _playerVisuals = new List<NMerchantCharacter>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001698 RID: 5784
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040057F5 RID: 22517
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040057F6 RID: 22518
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040057F7 RID: 22519
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ToggleMerchantTrack' method.
			/// </summary>
			// Token: 0x040057F8 RID: 22520
			public static readonly StringName ToggleMerchantTrack = "ToggleMerchantTrack";

			/// <summary>
			/// Cached name for the 'AfterRoomIsLoaded' method.
			/// </summary>
			// Token: 0x040057F9 RID: 22521
			public static readonly StringName AfterRoomIsLoaded = "AfterRoomIsLoaded";

			/// <summary>
			/// Cached name for the 'HideScreen' method.
			/// </summary>
			// Token: 0x040057FA RID: 22522
			public static readonly StringName HideScreen = "HideScreen";

			/// <summary>
			/// Cached name for the 'MerchantFtueCheck' method.
			/// </summary>
			// Token: 0x040057FB RID: 22523
			public static readonly StringName MerchantFtueCheck = "MerchantFtueCheck";

			/// <summary>
			/// Cached name for the 'OnMerchantOpened' method.
			/// </summary>
			// Token: 0x040057FC RID: 22524
			public static readonly StringName OnMerchantOpened = "OnMerchantOpened";

			/// <summary>
			/// Cached name for the 'OpenInventory' method.
			/// </summary>
			// Token: 0x040057FD RID: 22525
			public static readonly StringName OpenInventory = "OpenInventory";

			/// <summary>
			/// Cached name for the 'OnActiveScreenUpdated' method.
			/// </summary>
			// Token: 0x040057FE RID: 22526
			public static readonly StringName OnActiveScreenUpdated = "OnActiveScreenUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001699 RID: 5785
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ProceedButton' property.
			/// </summary>
			// Token: 0x040057FF RID: 22527
			public static readonly StringName ProceedButton = "ProceedButton";

			/// <summary>
			/// Cached name for the 'Inventory' property.
			/// </summary>
			// Token: 0x04005800 RID: 22528
			public static readonly StringName Inventory = "Inventory";

			/// <summary>
			/// Cached name for the 'MerchantButton' property.
			/// </summary>
			// Token: 0x04005801 RID: 22529
			public static readonly StringName MerchantButton = "MerchantButton";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005802 RID: 22530
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_proceedButton' field.
			/// </summary>
			// Token: 0x04005803 RID: 22531
			public static readonly StringName _proceedButton = "_proceedButton";

			/// <summary>
			/// Cached name for the '_characterContainer' field.
			/// </summary>
			// Token: 0x04005804 RID: 22532
			public static readonly StringName _characterContainer = "_characterContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200169A RID: 5786
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
