using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	// Token: 0x0200036D RID: 877
	[ScriptPath("res://src/Core/Nodes/Rooms/NMerchantButton.cs")]
	public class NMerchantButton : NButton
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06003346 RID: 13126 RVA: 0x0017DEC0 File Offset: 0x0017C0C0
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.select };
			}
		}

		/// <summary>
		/// Is the local player dead?
		/// Used to determine whether to open the merchant rug or play a dialogue line when clicked.
		/// </summary>
		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06003347 RID: 13127 RVA: 0x0017DED5 File Offset: 0x0017C0D5
		// (set) Token: 0x06003348 RID: 13128 RVA: 0x0017DEDD File Offset: 0x0017C0DD
		public bool IsLocalPlayerDead { get; set; }

		/// <summary>
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Merchant.MerchantDialogueSet.PlayerDeadLines" />.
		/// </summary>
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06003349 RID: 13129 RVA: 0x0017DEE6 File Offset: 0x0017C0E6
		// (set) Token: 0x0600334A RID: 13130 RVA: 0x0017DEEE File Offset: 0x0017C0EE
		[Nullable(1)]
		public IReadOnlyList<LocString> PlayerDeadLines
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = Array.Empty<LocString>();

		// Token: 0x0600334B RID: 13131 RVA: 0x0017DEF8 File Offset: 0x0017C0F8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._merchantSelectionReticle = base.GetNode<NSelectionReticle>("%MerchantSelectionReticle");
			MegaSprite sprite = new MegaSprite(base.GetNode("%MerchantVisual"));
			this.RunWhenSpineReady(sprite, delegate(MegaAnimationState animState)
			{
				this._merchantSkeleton = sprite.GetSkeleton();
				animState.SetAnimation("idle_loop", true, 0);
				if (this.IsFocused && !this._focusedWhileTargeting)
				{
					this._merchantSkeleton.SetSkinByName("outline");
					this._merchantSkeleton.SetSlotsToSetupPose();
				}
			});
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x0017DF66 File Offset: 0x0017C166
		protected override void OnFocus()
		{
			base.OnFocus();
			this.RefreshFocus();
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x0017DF74 File Offset: 0x0017C174
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._merchantSelectionReticle.OnDeselect();
			if (this._focusedWhileTargeting)
			{
				NTargetManager.Instance.OnNodeUnhovered(this);
			}
			else
			{
				MegaSkeleton merchantSkeleton = this._merchantSkeleton;
				if (merchantSkeleton != null)
				{
					merchantSkeleton.SetSkinByName("default");
				}
				MegaSkeleton merchantSkeleton2 = this._merchantSkeleton;
				if (merchantSkeleton2 != null)
				{
					merchantSkeleton2.SetSlotsToSetupPose();
				}
			}
			this._focusedWhileTargeting = false;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x0017DFD8 File Offset: 0x0017C1D8
		protected override void OnRelease()
		{
			if (this._focusedWhileTargeting)
			{
				this._merchantSelectionReticle.OnDeselect();
				this._focusedWhileTargeting = false;
				this.RefreshFocus();
				return;
			}
			if (this.IsLocalPlayerDead)
			{
				LocString locString = Rng.Chaotic.NextItem<LocString>(this.PlayerDeadLines);
				if (locString != null)
				{
					this.PlayDialogue(locString, 2.0);
					return;
				}
			}
			else
			{
				this.EmitSignalMerchantOpened(this);
			}
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x0017E03C File Offset: 0x0017C23C
		[NullableContext(2)]
		public NSpeechBubbleVfx PlayDialogue(LocString line, double duration = 2.0)
		{
			if (line == null)
			{
				return null;
			}
			NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.Create(line.GetFormattedText(), DialogueSide.Right, base.GlobalPosition + base.Size.X * Vector2.Left, duration, VfxColor.Blue);
			if (nspeechBubbleVfx != null)
			{
				base.GetParent().AddChildSafely(nspeechBubbleVfx);
			}
			return nspeechBubbleVfx;
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x0017E090 File Offset: 0x0017C290
		private void RefreshFocus()
		{
			if (NTargetManager.Instance.IsInSelection && NTargetManager.Instance.AllowedToTargetNode(this))
			{
				NTargetManager.Instance.OnNodeHovered(this);
				this._merchantSelectionReticle.OnSelect();
				this._focusedWhileTargeting = true;
				return;
			}
			MegaSkeleton merchantSkeleton = this._merchantSkeleton;
			if (merchantSkeleton != null)
			{
				merchantSkeleton.SetSkinByName("outline");
			}
			MegaSkeleton merchantSkeleton2 = this._merchantSkeleton;
			if (merchantSkeleton2 != null)
			{
				merchantSkeleton2.SetSlotsToSetupPose();
			}
			this._focusedWhileTargeting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003351 RID: 13137 RVA: 0x0017E104 File Offset: 0x0017C304
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMerchantButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantButton.MethodName.RefreshFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003352 RID: 13138 RVA: 0x0017E204 File Offset: 0x0017C404
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantButton.MethodName.RefreshFocus && args.Count == 0)
			{
				this.RefreshFocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003353 RID: 13139 RVA: 0x0017E2D4 File Offset: 0x0017C4D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantButton.MethodName._Ready || (ref method) == NMerchantButton.MethodName.OnFocus || (ref method) == NMerchantButton.MethodName.OnUnfocus || (ref method) == NMerchantButton.MethodName.OnRelease || (ref method) == NMerchantButton.MethodName.RefreshFocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003354 RID: 13140 RVA: 0x0017E334 File Offset: 0x0017C534
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantButton.PropertyName.IsLocalPlayerDead)
			{
				this.IsLocalPlayerDead = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMerchantButton.PropertyName._merchantSelectionReticle)
			{
				this._merchantSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NMerchantButton.PropertyName._focusedWhileTargeting)
			{
				this._focusedWhileTargeting = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003355 RID: 13141 RVA: 0x0017E39C File Offset: 0x0017C59C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NMerchantButton.PropertyName.IsLocalPlayerDead)
			{
				bool isLocalPlayerDead = this.IsLocalPlayerDead;
				value = VariantUtils.CreateFrom<bool>(ref isLocalPlayerDead);
				return true;
			}
			if ((ref name) == NMerchantButton.PropertyName._merchantSelectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._merchantSelectionReticle);
				return true;
			}
			if ((ref name) == NMerchantButton.PropertyName._focusedWhileTargeting)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._focusedWhileTargeting);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003356 RID: 13142 RVA: 0x0017E438 File Offset: 0x0017C638
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NMerchantButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantButton.PropertyName._merchantSelectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMerchantButton.PropertyName._focusedWhileTargeting, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMerchantButton.PropertyName.IsLocalPlayerDead, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003357 RID: 13143 RVA: 0x0017E4D0 File Offset: 0x0017C6D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isLocalPlayerDead = NMerchantButton.PropertyName.IsLocalPlayerDead;
			bool isLocalPlayerDead2 = this.IsLocalPlayerDead;
			info.AddProperty(isLocalPlayerDead, Variant.From<bool>(ref isLocalPlayerDead2));
			info.AddProperty(NMerchantButton.PropertyName._merchantSelectionReticle, Variant.From<NSelectionReticle>(ref this._merchantSelectionReticle));
			info.AddProperty(NMerchantButton.PropertyName._focusedWhileTargeting, Variant.From<bool>(ref this._focusedWhileTargeting));
			info.AddSignalEventDelegate(NMerchantButton.SignalName.MerchantOpened, this.backing_MerchantOpened);
		}

		/// <inheritdoc />
		// Token: 0x06003358 RID: 13144 RVA: 0x0017E53C File Offset: 0x0017C73C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantButton.PropertyName.IsLocalPlayerDead, ref variant))
			{
				this.IsLocalPlayerDead = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantButton.PropertyName._merchantSelectionReticle, ref variant2))
			{
				this._merchantSelectionReticle = variant2.As<NSelectionReticle>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantButton.PropertyName._focusedWhileTargeting, ref variant3))
			{
				this._focusedWhileTargeting = variant3.As<bool>();
			}
			NMerchantButton.MerchantOpenedEventHandler merchantOpenedEventHandler;
			if (info.TryGetSignalEventDelegate<NMerchantButton.MerchantOpenedEventHandler>(NMerchantButton.SignalName.MerchantOpened, ref merchantOpenedEventHandler))
			{
				this.backing_MerchantOpened = merchantOpenedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003359 RID: 13145 RVA: 0x0017E5BC File Offset: 0x0017C7BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMerchantButton.SignalName.MerchantOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "merchantButton", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Rooms.NMerchantButton.MerchantOpenedEventHandler" />
		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600335A RID: 13146 RVA: 0x0017E630 File Offset: 0x0017C830
		// (remove) Token: 0x0600335B RID: 13147 RVA: 0x0017E649 File Offset: 0x0017C849
		public event NMerchantButton.MerchantOpenedEventHandler MerchantOpened
		{
			add
			{
				this.backing_MerchantOpened = (NMerchantButton.MerchantOpenedEventHandler)Delegate.Combine(this.backing_MerchantOpened, value);
			}
			remove
			{
				this.backing_MerchantOpened = (NMerchantButton.MerchantOpenedEventHandler)Delegate.Remove(this.backing_MerchantOpened, value);
			}
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x0017E662 File Offset: 0x0017C862
		protected void EmitSignalMerchantOpened(NMerchantButton merchantButton)
		{
			base.EmitSignal(NMerchantButton.SignalName.MerchantOpened, new Variant[] { merchantButton });
		}

		/// <inheritdoc />
		// Token: 0x0600335D RID: 13149 RVA: 0x0017E683 File Offset: 0x0017C883
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NMerchantButton.SignalName.MerchantOpened) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NMerchantButton.MerchantOpenedEventHandler merchantOpenedEventHandler = this.backing_MerchantOpened;
			if (merchantOpenedEventHandler == null)
			{
				return;
			}
			merchantOpenedEventHandler(VariantUtils.ConvertTo<NMerchantButton>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x0600335E RID: 13150 RVA: 0x0017E6C2 File Offset: 0x0017C8C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NMerchantButton.SignalName.MerchantOpened || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x040013E3 RID: 5091
		[Nullable(2)]
		private MegaSkeleton _merchantSkeleton;

		// Token: 0x040013E4 RID: 5092
		[Nullable(1)]
		private NSelectionReticle _merchantSelectionReticle;

		/// <summary>
		/// Used to determine if the merchant was focused while in targeting mode, for things like <see cref="T:MegaCrit.Sts2.Core.Models.Potions.FoulPotion" />.
		/// </summary>
		// Token: 0x040013E5 RID: 5093
		private bool _focusedWhileTargeting;

		// Token: 0x040013E8 RID: 5096
		private NMerchantButton.MerchantOpenedEventHandler backing_MerchantOpened;

		// Token: 0x02001693 RID: 5779
		// (Invoke) Token: 0x0600A504 RID: 42244
		[Signal]
		public delegate void MerchantOpenedEventHandler(NMerchantButton merchantButton);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001694 RID: 5780
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040057E9 RID: 22505
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040057EA RID: 22506
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040057EB RID: 22507
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040057EC RID: 22508
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'RefreshFocus' method.
			/// </summary>
			// Token: 0x040057ED RID: 22509
			public new static readonly StringName RefreshFocus = "RefreshFocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001695 RID: 5781
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040057EE RID: 22510
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'IsLocalPlayerDead' property.
			/// </summary>
			// Token: 0x040057EF RID: 22511
			public static readonly StringName IsLocalPlayerDead = "IsLocalPlayerDead";

			/// <summary>
			/// Cached name for the '_merchantSelectionReticle' field.
			/// </summary>
			// Token: 0x040057F0 RID: 22512
			public static readonly StringName _merchantSelectionReticle = "_merchantSelectionReticle";

			/// <summary>
			/// Cached name for the '_focusedWhileTargeting' field.
			/// </summary>
			// Token: 0x040057F1 RID: 22513
			public static readonly StringName _focusedWhileTargeting = "_focusedWhileTargeting";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001696 RID: 5782
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'MerchantOpened' signal.
			/// </summary>
			// Token: 0x040057F2 RID: 22514
			public static readonly StringName MerchantOpened = "MerchantOpened";
		}
	}
}
