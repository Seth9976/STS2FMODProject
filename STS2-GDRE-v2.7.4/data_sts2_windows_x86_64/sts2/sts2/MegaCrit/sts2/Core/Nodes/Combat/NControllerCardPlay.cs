using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x020003F8 RID: 1016
	[ScriptPath("res://src/Core/Nodes/Combat/NControllerCardPlay.cs")]
	public class NControllerCardPlay : NCardPlay
	{
		// Token: 0x06003EA3 RID: 16035 RVA: 0x001DFC14 File Offset: 0x001DDE14
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			InputEventAction inputEventAction = inputEvent as InputEventAction;
			if (inputEventAction == null)
			{
				return;
			}
			if (inputEventAction.IsActionPressed(MegaInput.select, false, false))
			{
				base.EmitSignal(NControllerCardPlay.SignalName.Confirmed, Array.Empty<Variant>());
				Viewport viewport = base.GetViewport();
				if (viewport != null)
				{
					viewport.SetInputAsHandled();
				}
			}
			if (inputEventAction.IsActionPressed(MegaInput.cancel, false, false) || inputEventAction.IsActionPressed(MegaInput.topPanel, false, false))
			{
				base.EmitSignal(NControllerCardPlay.SignalName.Canceled, Array.Empty<Variant>());
				if (inputEvent.IsActionPressed(MegaInput.cancel, false, false))
				{
					base.GetViewport().SetInputAsHandled();
				}
			}
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x001DFCA8 File Offset: 0x001DDEA8
		[NullableContext(1)]
		public static NControllerCardPlay Create(NHandCardHolder holder)
		{
			return new NControllerCardPlay
			{
				Holder = holder,
				Player = holder.CardModel.Owner
			};
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x001DFCD4 File Offset: 0x001DDED4
		public override void Start()
		{
			if (base.Card == null || base.CardNode == null)
			{
				return;
			}
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("card_select.mp3", 1f, PitchVariance.None);
			}
			NHoverTipSet.Remove(base.Holder);
			this._onCreatureHoverCallable = Callable.From<NCreature>(new Action<NCreature>(base.OnCreatureHover));
			this._onCreatureUnhoverCallable = Callable.From<NCreature>(new Action<NCreature>(base.OnCreatureUnhover));
			UnplayableReason unplayableReason;
			AbstractModel abstractModel;
			if (!base.Card.CanPlay(out unplayableReason, out abstractModel))
			{
				base.CannotPlayThisCardFtueCheck(base.Card);
				base.CancelPlayCard();
				LocString playerDialogueLine = unplayableReason.GetPlayerDialogueLine(abstractModel);
				if (playerDialogueLine != null)
				{
					NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(NThoughtBubbleVfx.Create(playerDialogueLine.GetFormattedText(), base.Card.Owner.Creature, new double?(1.0)));
					return;
				}
			}
			else
			{
				base.TryShowEvokingOrbs();
				base.CardNode.CardHighlight.AnimFlash();
				base.CenterCard();
				TargetType targetType = base.Card.TargetType;
				bool flag = targetType == TargetType.AnyEnemy || targetType == TargetType.AnyAlly;
				if (flag)
				{
					TaskHelper.RunSafely(this.SingleCreatureTargeting(base.Card.TargetType));
					return;
				}
				this.MultiCreatureTargeting();
			}
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x001DFE0C File Offset: 0x001DE00C
		[NullableContext(1)]
		private async Task SingleCreatureTargeting(TargetType targetType)
		{
			Creature owner = base.Card.Owner.Creature;
			List<Creature> list = new List<Creature>();
			if (targetType == TargetType.AnyEnemy)
			{
				list = (from c in owner.CombatState.GetOpponentsOf(owner)
					where c.IsHittable
					select c).ToList<Creature>();
			}
			else if (targetType == TargetType.AnyAlly)
			{
				list = base.Card.CombatState.PlayerCreatures.Where<Creature>((Creature c) => c.IsHittable && c != owner).ToList<Creature>();
			}
			if (list.Count == 0)
			{
				base.CancelPlayCard();
			}
			else
			{
				List<NCreature> list2 = list.Select<Creature, NCreature>((Creature c) => NCombatRoom.Instance.GetCreatureNode(c)).OfType<NCreature>().ToList<NCreature>();
				if (list2.Count == 0)
				{
					base.CancelPlayCard();
				}
				else
				{
					NTargetManager instance = NTargetManager.Instance;
					instance.Connect(NTargetManager.SignalName.CreatureHovered, this._onCreatureHoverCallable, 0U);
					instance.Connect(NTargetManager.SignalName.CreatureUnhovered, this._onCreatureUnhoverCallable, 0U);
					this._signalsConnected = true;
					try
					{
						instance.StartTargeting(targetType, base.CardNode, TargetMode.Controller, () => !GodotObject.IsInstanceValid(this) || !NControllerManager.Instance.IsUsingController, null);
						NCombatRoom.Instance.RestrictControllerNavigation(list2.Select<NCreature, Control>((NCreature n) => n.Hitbox));
						NCreature ncreature = list2.First<NCreature>();
						if (NCombatRoom.Instance.LastTargetedCreature != null && NCombatRoom.Instance.LastTargetedCreature.IsHittable)
						{
							NCreature ncreature2 = list2.FirstOrDefault<NCreature>((NCreature c) => c.Entity == NCombatRoom.Instance.LastTargetedCreature);
							if (ncreature2 != null)
							{
								ncreature = ncreature2;
							}
						}
						ncreature.Hitbox.TryGrabFocus();
						Node node = await instance.SelectionFinished();
						NCreature ncreature3 = (NCreature)node;
						NCombatRoom.Instance.EnableControllerNavigation();
						if (GodotObject.IsInstanceValid(this))
						{
							if (ncreature3 != null)
							{
								base.TryPlayCard(ncreature3.Entity);
							}
							else
							{
								base.CancelPlayCard();
							}
						}
					}
					finally
					{
						this.DisconnectTargetingSignals();
					}
				}
			}
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x001DFE57 File Offset: 0x001DE057
		public override void _ExitTree()
		{
			this.DisconnectTargetingSignals();
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x001DFE60 File Offset: 0x001DE060
		private void DisconnectTargetingSignals()
		{
			if (!this._signalsConnected)
			{
				return;
			}
			this._signalsConnected = false;
			if (NRun.Instance == null)
			{
				return;
			}
			NTargetManager instance = NTargetManager.Instance;
			instance.Disconnect(NTargetManager.SignalName.CreatureHovered, this._onCreatureHoverCallable);
			instance.Disconnect(NTargetManager.SignalName.CreatureUnhovered, this._onCreatureUnhoverCallable);
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x001DFEB0 File Offset: 0x001DE0B0
		private void MultiCreatureTargeting()
		{
			NCombatRoom.Instance.RestrictControllerNavigation(Array.Empty<Control>());
			base.ShowMultiCreatureTargetingVisuals();
			base.Connect(NControllerCardPlay.SignalName.Confirmed, Callable.From(delegate
			{
				NCombatRoom.Instance.EnableControllerNavigation();
				base.TryPlayCard(null);
			}), 0U);
			base.Connect(NControllerCardPlay.SignalName.Canceled, Callable.From(new Action(base.CancelPlayCard)), 0U);
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x001DFF0E File Offset: 0x001DE10E
		protected override void OnCancelPlayCard()
		{
			NCombatRoom.Instance.EnableControllerNavigation();
			base.Holder.TryGrabFocus();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003EAB RID: 16043 RVA: 0x001DFF28 File Offset: 0x001DE128
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NControllerCardPlay.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NControllerCardPlay.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NControllerCardPlay.MethodName.Start, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerCardPlay.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerCardPlay.MethodName.DisconnectTargetingSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerCardPlay.MethodName.MultiCreatureTargeting, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerCardPlay.MethodName.OnCancelPlayCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003EAC RID: 16044 RVA: 0x001E00F4 File Offset: 0x001DE2F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NControllerCardPlay.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerCardPlay.MethodName.Create && args.Count == 1)
			{
				NControllerCardPlay ncontrollerCardPlay = NControllerCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = VariantUtils.CreateFrom<NControllerCardPlay>(ref ncontrollerCardPlay);
				return true;
			}
			if ((ref method) == NControllerCardPlay.MethodName.Start && args.Count == 0)
			{
				this.Start();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerCardPlay.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerCardPlay.MethodName.DisconnectTargetingSignals && args.Count == 0)
			{
				this.DisconnectTargetingSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerCardPlay.MethodName.MultiCreatureTargeting && args.Count == 0)
			{
				this.MultiCreatureTargeting();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerCardPlay.MethodName.OnCancelPlayCard && args.Count == 0)
			{
				this.OnCancelPlayCard();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x001E0230 File Offset: 0x001DE430
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NControllerCardPlay.MethodName.Create && args.Count == 1)
			{
				NControllerCardPlay ncontrollerCardPlay = NControllerCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = VariantUtils.CreateFrom<NControllerCardPlay>(ref ncontrollerCardPlay);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003EAE RID: 16046 RVA: 0x001E0280 File Offset: 0x001DE480
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NControllerCardPlay.MethodName._Input || (ref method) == NControllerCardPlay.MethodName.Create || (ref method) == NControllerCardPlay.MethodName.Start || (ref method) == NControllerCardPlay.MethodName._ExitTree || (ref method) == NControllerCardPlay.MethodName.DisconnectTargetingSignals || (ref method) == NControllerCardPlay.MethodName.MultiCreatureTargeting || (ref method) == NControllerCardPlay.MethodName.OnCancelPlayCard || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003EAF RID: 16047 RVA: 0x001E0300 File Offset: 0x001DE500
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NControllerCardPlay.PropertyName._onCreatureHoverCallable)
			{
				this._onCreatureHoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
				return true;
			}
			if ((ref name) == NControllerCardPlay.PropertyName._onCreatureUnhoverCallable)
			{
				this._onCreatureUnhoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
				return true;
			}
			if ((ref name) == NControllerCardPlay.PropertyName._signalsConnected)
			{
				this._signalsConnected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003EB0 RID: 16048 RVA: 0x001E0368 File Offset: 0x001DE568
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NControllerCardPlay.PropertyName._onCreatureHoverCallable)
			{
				value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureHoverCallable);
				return true;
			}
			if ((ref name) == NControllerCardPlay.PropertyName._onCreatureUnhoverCallable)
			{
				value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureUnhoverCallable);
				return true;
			}
			if ((ref name) == NControllerCardPlay.PropertyName._signalsConnected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._signalsConnected);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003EB1 RID: 16049 RVA: 0x001E03E0 File Offset: 0x001DE5E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(25L, NControllerCardPlay.PropertyName._onCreatureHoverCallable, 0L, "", 4096L, false),
				new PropertyInfo(25L, NControllerCardPlay.PropertyName._onCreatureUnhoverCallable, 0L, "", 4096L, false),
				new PropertyInfo(1L, NControllerCardPlay.PropertyName._signalsConnected, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003EB2 RID: 16050 RVA: 0x001E0458 File Offset: 0x001DE658
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NControllerCardPlay.PropertyName._onCreatureHoverCallable, Variant.From<Callable>(ref this._onCreatureHoverCallable));
			info.AddProperty(NControllerCardPlay.PropertyName._onCreatureUnhoverCallable, Variant.From<Callable>(ref this._onCreatureUnhoverCallable));
			info.AddProperty(NControllerCardPlay.PropertyName._signalsConnected, Variant.From<bool>(ref this._signalsConnected));
			info.AddSignalEventDelegate(NControllerCardPlay.SignalName.Confirmed, this.backing_Confirmed);
			info.AddSignalEventDelegate(NControllerCardPlay.SignalName.Canceled, this.backing_Canceled);
		}

		/// <inheritdoc />
		// Token: 0x06003EB3 RID: 16051 RVA: 0x001E04D0 File Offset: 0x001DE6D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NControllerCardPlay.PropertyName._onCreatureHoverCallable, ref variant))
			{
				this._onCreatureHoverCallable = variant.As<Callable>();
			}
			Variant variant2;
			if (info.TryGetProperty(NControllerCardPlay.PropertyName._onCreatureUnhoverCallable, ref variant2))
			{
				this._onCreatureUnhoverCallable = variant2.As<Callable>();
			}
			Variant variant3;
			if (info.TryGetProperty(NControllerCardPlay.PropertyName._signalsConnected, ref variant3))
			{
				this._signalsConnected = variant3.As<bool>();
			}
			NControllerCardPlay.ConfirmedEventHandler confirmedEventHandler;
			if (info.TryGetSignalEventDelegate<NControllerCardPlay.ConfirmedEventHandler>(NControllerCardPlay.SignalName.Confirmed, ref confirmedEventHandler))
			{
				this.backing_Confirmed = confirmedEventHandler;
			}
			NControllerCardPlay.CanceledEventHandler canceledEventHandler;
			if (info.TryGetSignalEventDelegate<NControllerCardPlay.CanceledEventHandler>(NControllerCardPlay.SignalName.Canceled, ref canceledEventHandler))
			{
				this.backing_Canceled = canceledEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003EB4 RID: 16052 RVA: 0x001E0568 File Offset: 0x001DE768
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NControllerCardPlay.SignalName.Confirmed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerCardPlay.SignalName.Canceled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NControllerCardPlay.ConfirmedEventHandler" />
		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06003EB5 RID: 16053 RVA: 0x001E05DB File Offset: 0x001DE7DB
		// (remove) Token: 0x06003EB6 RID: 16054 RVA: 0x001E05F4 File Offset: 0x001DE7F4
		public event NControllerCardPlay.ConfirmedEventHandler Confirmed
		{
			add
			{
				this.backing_Confirmed = (NControllerCardPlay.ConfirmedEventHandler)Delegate.Combine(this.backing_Confirmed, value);
			}
			remove
			{
				this.backing_Confirmed = (NControllerCardPlay.ConfirmedEventHandler)Delegate.Remove(this.backing_Confirmed, value);
			}
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x001E060D File Offset: 0x001DE80D
		protected void EmitSignalConfirmed()
		{
			base.EmitSignal(NControllerCardPlay.SignalName.Confirmed, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NControllerCardPlay.CanceledEventHandler" />
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06003EB8 RID: 16056 RVA: 0x001E0620 File Offset: 0x001DE820
		// (remove) Token: 0x06003EB9 RID: 16057 RVA: 0x001E0639 File Offset: 0x001DE839
		public event NControllerCardPlay.CanceledEventHandler Canceled
		{
			add
			{
				this.backing_Canceled = (NControllerCardPlay.CanceledEventHandler)Delegate.Combine(this.backing_Canceled, value);
			}
			remove
			{
				this.backing_Canceled = (NControllerCardPlay.CanceledEventHandler)Delegate.Remove(this.backing_Canceled, value);
			}
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x001E0652 File Offset: 0x001DE852
		protected void EmitSignalCanceled()
		{
			base.EmitSignal(NControllerCardPlay.SignalName.Canceled, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06003EBB RID: 16059 RVA: 0x001E0668 File Offset: 0x001DE868
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NControllerCardPlay.SignalName.Confirmed && args.Count == 0)
			{
				NControllerCardPlay.ConfirmedEventHandler confirmedEventHandler = this.backing_Confirmed;
				if (confirmedEventHandler == null)
				{
					return;
				}
				confirmedEventHandler();
				return;
			}
			else
			{
				if (!((ref signal) == NControllerCardPlay.SignalName.Canceled) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(in signal, args);
					return;
				}
				NControllerCardPlay.CanceledEventHandler canceledEventHandler = this.backing_Canceled;
				if (canceledEventHandler == null)
				{
					return;
				}
				canceledEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06003EBC RID: 16060 RVA: 0x001E06CB File Offset: 0x001DE8CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NControllerCardPlay.SignalName.Confirmed || (ref signal) == NControllerCardPlay.SignalName.Canceled || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x0400185A RID: 6234
		private Callable _onCreatureHoverCallable;

		// Token: 0x0400185B RID: 6235
		private Callable _onCreatureUnhoverCallable;

		// Token: 0x0400185C RID: 6236
		private bool _signalsConnected;

		// Token: 0x0400185D RID: 6237
		private NControllerCardPlay.ConfirmedEventHandler backing_Confirmed;

		// Token: 0x0400185E RID: 6238
		private NControllerCardPlay.CanceledEventHandler backing_Canceled;

		// Token: 0x020018BB RID: 6331
		// (Invoke) Token: 0x0600A949 RID: 43337
		[Signal]
		public delegate void ConfirmedEventHandler();

		// Token: 0x020018BC RID: 6332
		// (Invoke) Token: 0x0600A94D RID: 43341
		[Signal]
		public delegate void CanceledEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018BD RID: 6333
		public new class MethodName : NCardPlay.MethodName
		{
			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x040060AD RID: 24749
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040060AE RID: 24750
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'Start' method.
			/// </summary>
			// Token: 0x040060AF RID: 24751
			public new static readonly StringName Start = "Start";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040060B0 RID: 24752
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'DisconnectTargetingSignals' method.
			/// </summary>
			// Token: 0x040060B1 RID: 24753
			public static readonly StringName DisconnectTargetingSignals = "DisconnectTargetingSignals";

			/// <summary>
			/// Cached name for the 'MultiCreatureTargeting' method.
			/// </summary>
			// Token: 0x040060B2 RID: 24754
			public static readonly StringName MultiCreatureTargeting = "MultiCreatureTargeting";

			/// <summary>
			/// Cached name for the 'OnCancelPlayCard' method.
			/// </summary>
			// Token: 0x040060B3 RID: 24755
			public new static readonly StringName OnCancelPlayCard = "OnCancelPlayCard";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018BE RID: 6334
		public new class PropertyName : NCardPlay.PropertyName
		{
			/// <summary>
			/// Cached name for the '_onCreatureHoverCallable' field.
			/// </summary>
			// Token: 0x040060B4 RID: 24756
			public static readonly StringName _onCreatureHoverCallable = "_onCreatureHoverCallable";

			/// <summary>
			/// Cached name for the '_onCreatureUnhoverCallable' field.
			/// </summary>
			// Token: 0x040060B5 RID: 24757
			public static readonly StringName _onCreatureUnhoverCallable = "_onCreatureUnhoverCallable";

			/// <summary>
			/// Cached name for the '_signalsConnected' field.
			/// </summary>
			// Token: 0x040060B6 RID: 24758
			public static readonly StringName _signalsConnected = "_signalsConnected";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018BF RID: 6335
		public new class SignalName : NCardPlay.SignalName
		{
			/// <summary>
			/// Cached name for the 'Confirmed' signal.
			/// </summary>
			// Token: 0x040060B7 RID: 24759
			public static readonly StringName Confirmed = "Confirmed";

			/// <summary>
			/// Cached name for the 'Canceled' signal.
			/// </summary>
			// Token: 0x040060B8 RID: 24760
			public static readonly StringName Canceled = "Canceled";
		}
	}
}
