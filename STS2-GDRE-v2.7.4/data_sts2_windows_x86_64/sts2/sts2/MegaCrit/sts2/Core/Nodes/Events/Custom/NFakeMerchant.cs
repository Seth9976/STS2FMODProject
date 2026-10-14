using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Events.Custom
{
	// Token: 0x020003BF RID: 959
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/Custom/NFakeMerchant.cs")]
	public class NFakeMerchant : Control, ICustomEventNode, IScreenContext
	{
		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x001B7BE9 File Offset: 0x001B5DE9
		// (set) Token: 0x060039E2 RID: 14818 RVA: 0x001B7BF1 File Offset: 0x001B5DF1
		public NMerchantInventory Inventory { get; private set; }

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060039E3 RID: 14819 RVA: 0x001B7BFA File Offset: 0x001B5DFA
		// (set) Token: 0x060039E4 RID: 14820 RVA: 0x001B7C02 File Offset: 0x001B5E02
		public NMerchantButton MerchantButton { get; private set; }

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x001B7C0C File Offset: 0x001B5E0C
		public IScreenContext CurrentScreenContext
		{
			get
			{
				if (!this.Inventory.IsOpen)
				{
					return this;
				}
				return this.Inventory;
			}
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x001B7C32 File Offset: 0x001B5E32
		public void Initialize(EventModel eventModel)
		{
			this._event = (FakeMerchant)eventModel;
			this._dialogue = FakeMerchant.Dialogue;
			this._players.AddRange(this._event.Owner.RunState.Players);
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x001B7C6C File Offset: 0x001B5E6C
		public override void _Ready()
		{
			this._proceedButton = base.GetNode<NProceedButton>("%ProceedButton");
			this._proceedButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.HideScreen)), 0U);
			this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
			this._proceedButton.SetPulseState(false);
			this._proceedButton.Enable();
			this.MerchantButton = base.GetNode<NMerchantButton>("%MerchantButton");
			if (this._event.StartedFight)
			{
				this.MerchantButton.Hide();
				Player me = LocalContext.GetMe(this._players);
				if (me.GetRelic<FakeMerchantsRug>() != null)
				{
					MegaSprite megaSprite = new MegaSprite(base.GetNode<Node2D>("%FakeMerchantBackground"));
					MegaSkeleton skeleton = megaSprite.GetSkeleton();
					if (skeleton != null)
					{
						MegaBone megaBone = skeleton.FindBone("rug");
						if (megaBone != null)
						{
							megaBone.Hide();
						}
					}
				}
			}
			else
			{
				this.MerchantButton.IsLocalPlayerDead = LocalContext.GetMe(this._players).Creature.IsDead;
				this.MerchantButton.PlayerDeadLines = this._dialogue.PlayerDeadLines;
				this.MerchantButton.Connect(NMerchantButton.SignalName.MerchantOpened, Callable.From<NMerchantButton>(new Action<NMerchantButton>(this.OnMerchantOpened)), 0U);
			}
			this.Inventory = base.GetNode<NMerchantInventory>("%Inventory");
			this.Inventory.MouseFilter = 2L;
			this.Inventory.Initialize(this._event.Inventory, this._dialogue);
			this._characterContainer = base.GetNode<Control>("%CharacterContainer");
			this._inputBlocker = base.GetNode<Control>("%InputBlocker");
			NMapScreen.Instance.SetTravelEnabled(true);
			NGame.Instance.SetScreenShakeTarget(this);
			this.AfterRoomIsLoaded();
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x001B7E3C File Offset: 0x001B603C
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenUpdated;
			NMapScreen.Instance.Connect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
			NMapScreen.Instance.Connect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x001B7EB0 File Offset: 0x001B60B0
		public override void _ExitTree()
		{
			this._cts.Cancel();
			NGame.Instance.ClearScreenShakeTarget();
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenUpdated;
			NMapScreen.Instance.Disconnect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)));
			NMapScreen.Instance.Disconnect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)));
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x001B7F28 File Offset: 0x001B6128
		public async Task FoulPotionThrown()
		{
			this.MerchantButton.Disable();
			NSpeechBubbleVfx nspeechBubbleVfx = this.MerchantButton.PlayDialogue(Rng.Chaotic.NextItem<LocString>(this._dialogue.FoulPotionLines), 2.0);
			if (nspeechBubbleVfx != null)
			{
				await Cmd.Wait((float)nspeechBubbleVfx.SecondsToDisplay - 1f, this._cts.Token, false);
			}
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x001B7F6B File Offset: 0x001B616B
		private void ToggleMerchantTrack()
		{
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x001B7F70 File Offset: 0x001B6170
		private void AfterRoomIsLoaded()
		{
			Player me = LocalContext.GetMe(this._players);
			this._players.Remove(me);
			this._players.Insert(0, me);
			int num = Mathf.CeilToInt(Mathf.Sqrt((float)this._players.Count));
			for (int i = 0; i < num; i++)
			{
				float num2 = -75f * (float)i;
				for (int j = 0; j < num; j++)
				{
					int num3 = i * num + j;
					if (num3 >= this._players.Count)
					{
						break;
					}
					NCreatureVisuals ncreatureVisuals = this._players[num3].Character.CreateVisuals();
					this._characterContainer.AddChildSafely(ncreatureVisuals);
					this.StartCharacterAnimation(ncreatureVisuals);
					this._characterContainer.MoveChildSafely(ncreatureVisuals, 0);
					ncreatureVisuals.Position = new Vector2(num2, -50f * (float)i);
					if (i > 0)
					{
						ncreatureVisuals.Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
					}
					num2 -= ncreatureVisuals.Bounds.Size.X * 0.5f + 25f;
				}
			}
			if (!this._event.StartedFight)
			{
				TaskHelper.RunSafely(this.ShowWelcomeDialogue());
			}
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x001B80B4 File Offset: 0x001B62B4
		private async Task ShowWelcomeDialogue()
		{
			LocString line = Rng.Chaotic.NextItem<LocString>(this._dialogue.WelcomeLines);
			if (line != null)
			{
				await Cmd.Wait(0.75f, this._cts.Token, false);
				SfxCmd.Play("event:/sfx/npcs/reverse_merchant/reverse_merchant_laugh", 1f);
				this.MerchantButton.PlayDialogue(line, 4.0);
			}
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001B80F8 File Offset: 0x001B62F8
		private void StartCharacterAnimation(NCreatureVisuals visuals)
		{
			visuals.SpineAnimation.SetAnimation("relaxed_loop", true, 0);
			using (MegaTrackEntry currentTrack = visuals.SpineAnimation.GetCurrentTrack(0))
			{
				if (currentTrack != null)
				{
					currentTrack.SetLoop(true);
					currentTrack.SetTimeScale(Rng.Chaotic.NextFloat(0.9f, 1.1f));
					float animationEnd = currentTrack.GetAnimationEnd();
					currentTrack.SetTrackTime((animationEnd + Rng.Chaotic.NextFloat(-0.5f, 0.5f)) % animationEnd);
				}
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x001B8194 File Offset: 0x001B6394
		private void HideScreen(NButton _)
		{
			NMapScreen.Instance.Open(false);
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x001B81A2 File Offset: 0x001B63A2
		private void OnMerchantOpened(NMerchantButton _)
		{
			this.OpenInventory();
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x001B81AC File Offset: 0x001B63AC
		private void OpenInventory()
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
				this.ShowProceedButton();
			}), 4U);
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x001B820B File Offset: 0x001B640B
		private void ShowProceedButton()
		{
			this._proceedButton.Enable();
			this._proceedButton.SetPulseState(true);
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x001B8224 File Offset: 0x001B6424
		private void OnActiveScreenUpdated()
		{
			this.UpdateControllerNavEnabled<NFakeMerchant>();
			if (ActiveScreenContext.Instance.IsCurrent(this))
			{
				this.MerchantButton.Enable();
				if (!this._proceedButton.IsEnabled)
				{
					this._proceedButton.Enable();
					return;
				}
			}
			else
			{
				this.MerchantButton.Disable();
				this._proceedButton.Disable();
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x001B827E File Offset: 0x001B647E
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x001B8281 File Offset: 0x001B6481
		public void BlockInput()
		{
			this._inputBlocker.MouseFilter = 0L;
			NHotkeyManager.Instance.AddBlockingScreen(this._inputBlocker);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x001B82A0 File Offset: 0x001B64A0
		public void UnblockInput()
		{
			this._inputBlocker.MouseFilter = 2L;
			NHotkeyManager.Instance.RemoveBlockingScreen(this._inputBlocker);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060039F7 RID: 14839 RVA: 0x001B82C0 File Offset: 0x001B64C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NFakeMerchant.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName.ToggleMerchantTrack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName.AfterRoomIsLoaded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName.StartCharacterAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "visuals", 0L, "", 6L, new StringName("Node2D"), false)
				}, null),
				new MethodInfo(NFakeMerchant.MethodName.HideScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NFakeMerchant.MethodName.OnMerchantOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NFakeMerchant.MethodName.OpenInventory, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName.ShowProceedButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName.OnActiveScreenUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName.BlockInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchant.MethodName.UnblockInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060039F8 RID: 14840 RVA: 0x001B85CC File Offset: 0x001B67CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFakeMerchant.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.ToggleMerchantTrack && args.Count == 0)
			{
				this.ToggleMerchantTrack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.AfterRoomIsLoaded && args.Count == 0)
			{
				this.AfterRoomIsLoaded();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.StartCharacterAnimation && args.Count == 1)
			{
				this.StartCharacterAnimation(VariantUtils.ConvertTo<NCreatureVisuals>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.HideScreen && args.Count == 1)
			{
				this.HideScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.OnMerchantOpened && args.Count == 1)
			{
				this.OnMerchantOpened(VariantUtils.ConvertTo<NMerchantButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.OpenInventory && args.Count == 0)
			{
				this.OpenInventory();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.ShowProceedButton && args.Count == 0)
			{
				this.ShowProceedButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.OnActiveScreenUpdated && args.Count == 0)
			{
				this.OnActiveScreenUpdated();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.BlockInput && args.Count == 0)
			{
				this.BlockInput();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchant.MethodName.UnblockInput && args.Count == 0)
			{
				this.UnblockInput();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060039F9 RID: 14841 RVA: 0x001B87F0 File Offset: 0x001B69F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFakeMerchant.MethodName._Ready || (ref method) == NFakeMerchant.MethodName._EnterTree || (ref method) == NFakeMerchant.MethodName._ExitTree || (ref method) == NFakeMerchant.MethodName.ToggleMerchantTrack || (ref method) == NFakeMerchant.MethodName.AfterRoomIsLoaded || (ref method) == NFakeMerchant.MethodName.StartCharacterAnimation || (ref method) == NFakeMerchant.MethodName.HideScreen || (ref method) == NFakeMerchant.MethodName.OnMerchantOpened || (ref method) == NFakeMerchant.MethodName.OpenInventory || (ref method) == NFakeMerchant.MethodName.ShowProceedButton || (ref method) == NFakeMerchant.MethodName.OnActiveScreenUpdated || (ref method) == NFakeMerchant.MethodName.BlockInput || (ref method) == NFakeMerchant.MethodName.UnblockInput || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060039FA RID: 14842 RVA: 0x001B88C8 File Offset: 0x001B6AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFakeMerchant.PropertyName.Inventory)
			{
				this.Inventory = VariantUtils.ConvertTo<NMerchantInventory>(ref value);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName.MerchantButton)
			{
				this.MerchantButton = VariantUtils.ConvertTo<NMerchantButton>(ref value);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName._proceedButton)
			{
				this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName._characterContainer)
			{
				this._characterContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName._inputBlocker)
			{
				this._inputBlocker = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060039FB RID: 14843 RVA: 0x001B8964 File Offset: 0x001B6B64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFakeMerchant.PropertyName.Inventory)
			{
				NMerchantInventory inventory = this.Inventory;
				value = VariantUtils.CreateFrom<NMerchantInventory>(ref inventory);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName.MerchantButton)
			{
				NMerchantButton merchantButton = this.MerchantButton;
				value = VariantUtils.CreateFrom<NMerchantButton>(ref merchantButton);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName._proceedButton)
			{
				value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName._characterContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterContainer);
				return true;
			}
			if ((ref name) == NFakeMerchant.PropertyName._inputBlocker)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._inputBlocker);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060039FC RID: 14844 RVA: 0x001B8A44 File Offset: 0x001B6C44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFakeMerchant.PropertyName._proceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFakeMerchant.PropertyName._characterContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFakeMerchant.PropertyName._inputBlocker, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFakeMerchant.PropertyName.Inventory, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFakeMerchant.PropertyName.MerchantButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFakeMerchant.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060039FD RID: 14845 RVA: 0x001B8B20 File Offset: 0x001B6D20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName inventory = NFakeMerchant.PropertyName.Inventory;
			NMerchantInventory inventory2 = this.Inventory;
			info.AddProperty(inventory, Variant.From<NMerchantInventory>(ref inventory2));
			StringName merchantButton = NFakeMerchant.PropertyName.MerchantButton;
			NMerchantButton merchantButton2 = this.MerchantButton;
			info.AddProperty(merchantButton, Variant.From<NMerchantButton>(ref merchantButton2));
			info.AddProperty(NFakeMerchant.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
			info.AddProperty(NFakeMerchant.PropertyName._characterContainer, Variant.From<Control>(ref this._characterContainer));
			info.AddProperty(NFakeMerchant.PropertyName._inputBlocker, Variant.From<Control>(ref this._inputBlocker));
		}

		/// <inheritdoc />
		// Token: 0x060039FE RID: 14846 RVA: 0x001B8BA8 File Offset: 0x001B6DA8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFakeMerchant.PropertyName.Inventory, ref variant))
			{
				this.Inventory = variant.As<NMerchantInventory>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFakeMerchant.PropertyName.MerchantButton, ref variant2))
			{
				this.MerchantButton = variant2.As<NMerchantButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFakeMerchant.PropertyName._proceedButton, ref variant3))
			{
				this._proceedButton = variant3.As<NProceedButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NFakeMerchant.PropertyName._characterContainer, ref variant4))
			{
				this._characterContainer = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NFakeMerchant.PropertyName._inputBlocker, ref variant5))
			{
				this._inputBlocker = variant5.As<Control>();
			}
		}

		// Token: 0x0400168E RID: 5774
		private const float _animVariance = 0.5f;

		// Token: 0x0400168F RID: 5775
		private readonly List<Player> _players = new List<Player>();

		// Token: 0x04001690 RID: 5776
		private FakeMerchant _event;

		// Token: 0x04001691 RID: 5777
		private MerchantDialogueSet _dialogue;

		// Token: 0x04001692 RID: 5778
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04001693 RID: 5779
		private NProceedButton _proceedButton;

		// Token: 0x04001694 RID: 5780
		private Control _characterContainer;

		// Token: 0x04001695 RID: 5781
		private Control _inputBlocker;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017DC RID: 6108
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D21 RID: 23841
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005D22 RID: 23842
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005D23 RID: 23843
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ToggleMerchantTrack' method.
			/// </summary>
			// Token: 0x04005D24 RID: 23844
			public static readonly StringName ToggleMerchantTrack = "ToggleMerchantTrack";

			/// <summary>
			/// Cached name for the 'AfterRoomIsLoaded' method.
			/// </summary>
			// Token: 0x04005D25 RID: 23845
			public static readonly StringName AfterRoomIsLoaded = "AfterRoomIsLoaded";

			/// <summary>
			/// Cached name for the 'StartCharacterAnimation' method.
			/// </summary>
			// Token: 0x04005D26 RID: 23846
			public static readonly StringName StartCharacterAnimation = "StartCharacterAnimation";

			/// <summary>
			/// Cached name for the 'HideScreen' method.
			/// </summary>
			// Token: 0x04005D27 RID: 23847
			public static readonly StringName HideScreen = "HideScreen";

			/// <summary>
			/// Cached name for the 'OnMerchantOpened' method.
			/// </summary>
			// Token: 0x04005D28 RID: 23848
			public static readonly StringName OnMerchantOpened = "OnMerchantOpened";

			/// <summary>
			/// Cached name for the 'OpenInventory' method.
			/// </summary>
			// Token: 0x04005D29 RID: 23849
			public static readonly StringName OpenInventory = "OpenInventory";

			/// <summary>
			/// Cached name for the 'ShowProceedButton' method.
			/// </summary>
			// Token: 0x04005D2A RID: 23850
			public static readonly StringName ShowProceedButton = "ShowProceedButton";

			/// <summary>
			/// Cached name for the 'OnActiveScreenUpdated' method.
			/// </summary>
			// Token: 0x04005D2B RID: 23851
			public static readonly StringName OnActiveScreenUpdated = "OnActiveScreenUpdated";

			/// <summary>
			/// Cached name for the 'BlockInput' method.
			/// </summary>
			// Token: 0x04005D2C RID: 23852
			public static readonly StringName BlockInput = "BlockInput";

			/// <summary>
			/// Cached name for the 'UnblockInput' method.
			/// </summary>
			// Token: 0x04005D2D RID: 23853
			public static readonly StringName UnblockInput = "UnblockInput";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017DD RID: 6109
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Inventory' property.
			/// </summary>
			// Token: 0x04005D2E RID: 23854
			public static readonly StringName Inventory = "Inventory";

			/// <summary>
			/// Cached name for the 'MerchantButton' property.
			/// </summary>
			// Token: 0x04005D2F RID: 23855
			public static readonly StringName MerchantButton = "MerchantButton";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005D30 RID: 23856
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_proceedButton' field.
			/// </summary>
			// Token: 0x04005D31 RID: 23857
			public static readonly StringName _proceedButton = "_proceedButton";

			/// <summary>
			/// Cached name for the '_characterContainer' field.
			/// </summary>
			// Token: 0x04005D32 RID: 23858
			public static readonly StringName _characterContainer = "_characterContainer";

			/// <summary>
			/// Cached name for the '_inputBlocker' field.
			/// </summary>
			// Token: 0x04005D33 RID: 23859
			public static readonly StringName _inputBlocker = "_inputBlocker";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017DE RID: 6110
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
