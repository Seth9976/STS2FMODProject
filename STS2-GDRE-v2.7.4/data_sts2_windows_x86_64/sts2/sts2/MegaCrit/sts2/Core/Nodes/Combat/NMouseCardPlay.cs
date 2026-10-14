using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
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
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000405 RID: 1029
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NMouseCardPlay.cs")]
	public class NMouseCardPlay : NCardPlay
	{
		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06004030 RID: 16432 RVA: 0x001EE56C File Offset: 0x001EC76C
		private float PlayZoneThreshold
		{
			get
			{
				float num = this._viewport.GetVisibleRect().Size.Y * 0.75f;
				if (this._skipStartCardDrag)
				{
					return num + 100f;
				}
				if (this._dragStartYPosition > num)
				{
					return Mathf.Max(num, this._dragStartYPosition - 100f);
				}
				return Mathf.Min(num, this._dragStartYPosition - 50f);
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x001EE5D8 File Offset: 0x001EC7D8
		private float CancelZoneThreshold
		{
			get
			{
				return this._viewport.GetVisibleRect().Size.Y * 0.95f;
			}
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x001EE604 File Offset: 0x001EC804
		public static NMouseCardPlay Create(NHandCardHolder holder, StringName cancelShortcut, bool wasStartedWithShortcut)
		{
			return new NMouseCardPlay
			{
				Holder = holder,
				Player = holder.CardModel.Owner,
				_cancelShortcut = cancelShortcut,
				_skipStartCardDrag = wasStartedWithShortcut
			};
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x001EE640 File Offset: 0x001EC840
		public override void _Input(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
				if (buttonIndex != 1L)
				{
					if (buttonIndex == 2L)
					{
						if (inputEventMouseButton.IsPressed())
						{
							base.CancelPlayCard();
						}
					}
				}
				else if (inputEventMouseButton.IsPressed())
				{
					this._isLeftMouseDown = true;
				}
				else if (inputEventMouseButton.IsReleased())
				{
					this._isLeftMouseDown = false;
				}
			}
			if (inputEvent.IsActionPressed(this._cancelShortcut, false, false) || inputEvent.IsActionPressed(MegaInput.releaseCard, false, false))
			{
				base.CancelPlayCard();
				Viewport viewport = base.GetViewport();
				if (viewport == null)
				{
					return;
				}
				viewport.SetInputAsHandled();
			}
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x001EE6D0 File Offset: 0x001EC8D0
		public override void Start()
		{
			this._isLeftMouseDown = !this._skipStartCardDrag;
			base.Holder.Hitbox.MouseFilter = 2L;
			this._cancellationTokenSource = new CancellationTokenSource();
			this._onCreatureHoverCallable = Callable.From<NCreature>(new Action<NCreature>(base.OnCreatureHover));
			this._onCreatureUnhoverCallable = Callable.From<NCreature>(new Action<NCreature>(base.OnCreatureUnhover));
			TaskHelper.RunSafely(this.StartAsync());
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x001EE744 File Offset: 0x001EC944
		public override void _EnterTree()
		{
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(base.CancelPlayCard)), 0U);
				NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(base.CancelPlayCard)), 0U);
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x001EE79C File Offset: 0x001EC99C
		public override void _ExitTree()
		{
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(base.CancelPlayCard)));
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(base.CancelPlayCard)));
			}
			this._cancellationTokenSource.Cancel();
			this.DisconnectTargetingSignals();
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x001EE804 File Offset: 0x001ECA04
		private async Task StartAsync()
		{
			if (base.Card != null && base.CardNode != null)
			{
				await this.StartCardDrag();
				if (!this._cancellationTokenSource.IsCancellationRequested)
				{
					UnplayableReason unplayableReason;
					AbstractModel abstractModel;
					if (!base.Card.CanPlay(out unplayableReason, out abstractModel))
					{
						base.CannotPlayThisCardFtueCheck(base.Card);
						base.CancelPlayCard();
						LocString playerDialogueLine = unplayableReason.GetPlayerDialogueLine(abstractModel);
						if (playerDialogueLine != null)
						{
							NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(NThoughtBubbleVfx.Create(playerDialogueLine.GetFormattedText(), base.Card.Owner.Creature, new double?((double)1f)));
						}
					}
					else
					{
						base.CardNode.CardHighlight.AnimFlash();
						TargetMode targetMode;
						if (this._skipStartCardDrag)
						{
							targetMode = TargetMode.ClickMouseToTarget;
						}
						else
						{
							targetMode = (this._isLeftMouseDown ? TargetMode.ReleaseMouseToTarget : TargetMode.ClickMouseToTarget);
						}
						await this.TargetSelection(targetMode);
						if (!this._cancellationTokenSource.IsCancellationRequested)
						{
							if (!this.IsCardInPlayZone())
							{
								base.CancelPlayCard();
							}
							if (!this._cancellationTokenSource.IsCancellationRequested)
							{
								base.TryPlayCard(this._target);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x001EE848 File Offset: 0x001ECA48
		private async Task StartCardDrag()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("card_select.mp3", 0.5f, PitchVariance.None);
			}
			NHoverTipSet.Remove(base.Holder);
			this._dragStartYPosition = this._viewport.GetMousePosition().Y;
			if (!this._skipStartCardDrag)
			{
				do
				{
					await this.LerpToMouse(base.Holder);
				}
				while (!this.IsCardInPlayZone() && !this._cancellationTokenSource.IsCancellationRequested);
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x001EE88C File Offset: 0x001ECA8C
		private async Task TargetSelection(TargetMode targetMode)
		{
			if (base.Card != null)
			{
				base.TryShowEvokingOrbs();
				NCard cardNode = base.CardNode;
				if (cardNode != null)
				{
					cardNode.CardHighlight.AnimFlash();
				}
				TargetType targetType = base.Card.TargetType;
				bool flag = targetType == TargetType.AnyEnemy || targetType == TargetType.AnyAlly;
				if (flag)
				{
					await this.SingleCreatureTargeting(targetMode, base.Card.TargetType);
				}
				else
				{
					await this.MultiCreatureTargeting(targetMode);
				}
			}
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x001EE8D8 File Offset: 0x001ECAD8
		private async Task SingleCreatureTargeting(TargetMode targetMode, TargetType targetType)
		{
			if (!this._cancellationTokenSource.IsCancellationRequested)
			{
				base.CenterCard();
				NTargetManager instance = NTargetManager.Instance;
				instance.Connect(NTargetManager.SignalName.CreatureHovered, this._onCreatureHoverCallable, 0U);
				instance.Connect(NTargetManager.SignalName.CreatureUnhovered, this._onCreatureUnhoverCallable, 0U);
				this._signalsConnected = true;
				try
				{
					instance.StartTargeting(targetType, base.CardNode, targetMode, () => this.IsCardInCancelZone() || this._cancellationTokenSource.IsCancellationRequested, null);
					Node node = await instance.SelectionFinished();
					Node node2 = node;
					if (!this._cancellationTokenSource.IsCancellationRequested)
					{
						if (node2 != null)
						{
							NCreature ncreature = node2 as NCreature;
							Creature creature;
							if (ncreature == null)
							{
								NMultiplayerPlayerState nmultiplayerPlayerState = node2 as NMultiplayerPlayerState;
								if (nmultiplayerPlayerState == null)
								{
									throw new ArgumentOutOfRangeException("target", node2, null);
								}
								creature = nmultiplayerPlayerState.Player.Creature;
							}
							else
							{
								creature = ncreature.Entity;
							}
							this._target = creature;
						}
					}
				}
				finally
				{
					this.DisconnectTargetingSignals();
				}
			}
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x001EE92C File Offset: 0x001ECB2C
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

		// Token: 0x0600403C RID: 16444 RVA: 0x001EE97C File Offset: 0x001ECB7C
		private async Task MultiCreatureTargeting(TargetMode targetMode)
		{
			bool isShowingTargetingVisuals = false;
			Func<bool> shouldFinishTargeting = ((targetMode == TargetMode.ReleaseMouseToTarget) ? (() => !this._isLeftMouseDown) : (() => this._isLeftMouseDown));
			do
			{
				if (isShowingTargetingVisuals)
				{
					if (!this.IsCardInPlayZone())
					{
						base.HideTargetingVisuals();
						isShowingTargetingVisuals = false;
					}
				}
				else if (this.IsCardInPlayZone())
				{
					base.ShowMultiCreatureTargetingVisuals();
					isShowingTargetingVisuals = true;
				}
				await this.LerpToMouse(base.Holder);
			}
			while (!shouldFinishTargeting() && !this._cancellationTokenSource.IsCancellationRequested && !this.IsCardInCancelZone());
			if (!this._cancellationTokenSource.IsCancellationRequested)
			{
				if (this.IsCardInCancelZone())
				{
					base.CancelPlayCard();
				}
			}
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x001EE9C7 File Offset: 0x001ECBC7
		protected override void OnCancelPlayCard()
		{
			if (!GodotObject.IsInstanceValid(this) || !base.IsInsideTree())
			{
				return;
			}
			base.Holder.Hitbox.MouseFilter = 0L;
			this._cancellationTokenSource.Cancel();
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x001EE9F8 File Offset: 0x001ECBF8
		private async Task LerpToMouse(NHandCardHolder cardHolder)
		{
			cardHolder.SetTargetPosition(this._viewport.GetMousePosition());
			await this.AwaitProcessFrame(default(CancellationToken));
		}

		/// <summary>
		/// Has the card moved up into the Play Zone?
		/// This happens when the card was near the bottom of the screen (meaning you just started dragging it) and then you
		/// drag it up higher to start targeting it.
		/// </summary>
		// Token: 0x0600403F RID: 16447 RVA: 0x001EEA43 File Offset: 0x001ECC43
		private bool IsCardInPlayZone()
		{
			return this._viewport.GetMousePosition().Y < this.PlayZoneThreshold;
		}

		/// <summary>
		/// Has the card moved down into the Cancel Zone?
		/// This happens when the card was in the Play Zone (meaning you were targeting it) and then you drag it back down
		/// to cancel the play.
		///
		/// We use a greater value (further down the screen) for the Cancel Zone than the Play Zone because:
		/// 1. We want you to have to move the card down a meaningful amount before we stop targeting.
		/// 2. For multi-target cards, we want an "intermediate" zone where the card doesn't show targeting visuals but we
		///    haven't canceled yet.
		/// </summary>
		// Token: 0x06004040 RID: 16448 RVA: 0x001EEA60 File Offset: 0x001ECC60
		private bool IsCardInCancelZone()
		{
			this._hasLeftCardCancelZoneOnce |= this._viewport.GetMousePosition().Y <= this.CancelZoneThreshold;
			return this._viewport.GetMousePosition().Y > this.CancelZoneThreshold && this._hasLeftCardCancelZoneOnce;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004041 RID: 16449 RVA: 0x001EEAB8 File Offset: 0x001ECCB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NMouseCardPlay.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(21L, "cancelShortcut", 0L, "", 6L, false),
					new PropertyInfo(1L, "wasStartedWithShortcut", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMouseCardPlay.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMouseCardPlay.MethodName.Start, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMouseCardPlay.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMouseCardPlay.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMouseCardPlay.MethodName.DisconnectTargetingSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMouseCardPlay.MethodName.OnCancelPlayCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMouseCardPlay.MethodName.IsCardInPlayZone, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMouseCardPlay.MethodName.IsCardInCancelZone, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004042 RID: 16450 RVA: 0x001EED24 File Offset: 0x001ECF24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMouseCardPlay.MethodName.Create && args.Count == 3)
			{
				NMouseCardPlay nmouseCardPlay = NMouseCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(args[0]), VariantUtils.ConvertTo<StringName>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NMouseCardPlay>(ref nmouseCardPlay);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName.Start && args.Count == 0)
			{
				this.Start();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName.DisconnectTargetingSignals && args.Count == 0)
			{
				this.DisconnectTargetingSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName.OnCancelPlayCard && args.Count == 0)
			{
				this.OnCancelPlayCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName.IsCardInPlayZone && args.Count == 0)
			{
				bool flag = this.IsCardInPlayZone();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NMouseCardPlay.MethodName.IsCardInCancelZone && args.Count == 0)
			{
				bool flag2 = this.IsCardInCancelZone();
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x001EEED4 File Offset: 0x001ED0D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMouseCardPlay.MethodName.Create && args.Count == 3)
			{
				NMouseCardPlay nmouseCardPlay = NMouseCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(args[0]), VariantUtils.ConvertTo<StringName>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NMouseCardPlay>(ref nmouseCardPlay);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06004044 RID: 16452 RVA: 0x001EEF3C File Offset: 0x001ED13C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMouseCardPlay.MethodName.Create || (ref method) == NMouseCardPlay.MethodName._Input || (ref method) == NMouseCardPlay.MethodName.Start || (ref method) == NMouseCardPlay.MethodName._EnterTree || (ref method) == NMouseCardPlay.MethodName._ExitTree || (ref method) == NMouseCardPlay.MethodName.DisconnectTargetingSignals || (ref method) == NMouseCardPlay.MethodName.OnCancelPlayCard || (ref method) == NMouseCardPlay.MethodName.IsCardInPlayZone || (ref method) == NMouseCardPlay.MethodName.IsCardInCancelZone || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06004045 RID: 16453 RVA: 0x001EEFD8 File Offset: 0x001ED1D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce)
			{
				this._hasLeftCardCancelZoneOnce = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._dragStartYPosition)
			{
				this._dragStartYPosition = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._isLeftMouseDown)
			{
				this._isLeftMouseDown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._onCreatureHoverCallable)
			{
				this._onCreatureHoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._onCreatureUnhoverCallable)
			{
				this._onCreatureUnhoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._signalsConnected)
			{
				this._signalsConnected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._cancelShortcut)
			{
				this._cancelShortcut = VariantUtils.ConvertTo<StringName>(ref value);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._skipStartCardDrag)
			{
				this._skipStartCardDrag = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06004046 RID: 16454 RVA: 0x001EF0C8 File Offset: 0x001ED2C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMouseCardPlay.PropertyName.PlayZoneThreshold)
			{
				float num = this.PlayZoneThreshold;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName.CancelZoneThreshold)
			{
				float num = this.CancelZoneThreshold;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._hasLeftCardCancelZoneOnce);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._dragStartYPosition)
			{
				value = VariantUtils.CreateFrom<float>(ref this._dragStartYPosition);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._isLeftMouseDown)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isLeftMouseDown);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._onCreatureHoverCallable)
			{
				value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureHoverCallable);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._onCreatureUnhoverCallable)
			{
				value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureUnhoverCallable);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._signalsConnected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._signalsConnected);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._cancelShortcut)
			{
				value = VariantUtils.CreateFrom<StringName>(ref this._cancelShortcut);
				return true;
			}
			if ((ref name) == NMouseCardPlay.PropertyName._skipStartCardDrag)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._skipStartCardDrag);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004047 RID: 16455 RVA: 0x001EF224 File Offset: 0x001ED424
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMouseCardPlay.PropertyName.PlayZoneThreshold, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMouseCardPlay.PropertyName.CancelZoneThreshold, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMouseCardPlay.PropertyName._dragStartYPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMouseCardPlay.PropertyName._isLeftMouseDown, 0L, "", 4096L, false),
				new PropertyInfo(25L, NMouseCardPlay.PropertyName._onCreatureHoverCallable, 0L, "", 4096L, false),
				new PropertyInfo(25L, NMouseCardPlay.PropertyName._onCreatureUnhoverCallable, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMouseCardPlay.PropertyName._signalsConnected, 0L, "", 4096L, false),
				new PropertyInfo(21L, NMouseCardPlay.PropertyName._cancelShortcut, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMouseCardPlay.PropertyName._skipStartCardDrag, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004048 RID: 16456 RVA: 0x001EF37C File Offset: 0x001ED57C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce, Variant.From<bool>(ref this._hasLeftCardCancelZoneOnce));
			info.AddProperty(NMouseCardPlay.PropertyName._dragStartYPosition, Variant.From<float>(ref this._dragStartYPosition));
			info.AddProperty(NMouseCardPlay.PropertyName._isLeftMouseDown, Variant.From<bool>(ref this._isLeftMouseDown));
			info.AddProperty(NMouseCardPlay.PropertyName._onCreatureHoverCallable, Variant.From<Callable>(ref this._onCreatureHoverCallable));
			info.AddProperty(NMouseCardPlay.PropertyName._onCreatureUnhoverCallable, Variant.From<Callable>(ref this._onCreatureUnhoverCallable));
			info.AddProperty(NMouseCardPlay.PropertyName._signalsConnected, Variant.From<bool>(ref this._signalsConnected));
			info.AddProperty(NMouseCardPlay.PropertyName._cancelShortcut, Variant.From<StringName>(ref this._cancelShortcut));
			info.AddProperty(NMouseCardPlay.PropertyName._skipStartCardDrag, Variant.From<bool>(ref this._skipStartCardDrag));
		}

		/// <inheritdoc />
		// Token: 0x06004049 RID: 16457 RVA: 0x001EF440 File Offset: 0x001ED640
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce, ref variant))
			{
				this._hasLeftCardCancelZoneOnce = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._dragStartYPosition, ref variant2))
			{
				this._dragStartYPosition = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._isLeftMouseDown, ref variant3))
			{
				this._isLeftMouseDown = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._onCreatureHoverCallable, ref variant4))
			{
				this._onCreatureHoverCallable = variant4.As<Callable>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._onCreatureUnhoverCallable, ref variant5))
			{
				this._onCreatureUnhoverCallable = variant5.As<Callable>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._signalsConnected, ref variant6))
			{
				this._signalsConnected = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._cancelShortcut, ref variant7))
			{
				this._cancelShortcut = variant7.As<StringName>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMouseCardPlay.PropertyName._skipStartCardDrag, ref variant8))
			{
				this._skipStartCardDrag = variant8.As<bool>();
			}
		}

		// Token: 0x040018FC RID: 6396
		private const float _fakeLowerEnterPlayZoneDistance = 100f;

		// Token: 0x040018FD RID: 6397
		private const float _fakeUpperEnterPlayZoneDistance = 50f;

		// Token: 0x040018FE RID: 6398
		private const float _playZoneScreenProportion = 0.75f;

		// Token: 0x040018FF RID: 6399
		private const float _cancelZoneScreenProportion = 0.95f;

		// Token: 0x04001900 RID: 6400
		private bool _hasLeftCardCancelZoneOnce;

		// Token: 0x04001901 RID: 6401
		private float _dragStartYPosition;

		// Token: 0x04001902 RID: 6402
		[Nullable(2)]
		private Creature _target;

		// Token: 0x04001903 RID: 6403
		private bool _isLeftMouseDown;

		// Token: 0x04001904 RID: 6404
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04001905 RID: 6405
		private Callable _onCreatureHoverCallable;

		// Token: 0x04001906 RID: 6406
		private Callable _onCreatureUnhoverCallable;

		// Token: 0x04001907 RID: 6407
		private bool _signalsConnected;

		/// <summary>
		/// shortcut to cancel the card play. Usually we set this to be the index of the card holder in the hand,
		/// </summary>
		// Token: 0x04001908 RID: 6408
		private StringName _cancelShortcut;

		// Token: 0x04001909 RID: 6409
		private bool _skipStartCardDrag;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018F5 RID: 6389
		[NullableContext(0)]
		public new class MethodName : NCardPlay.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040061F4 RID: 25076
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x040061F5 RID: 25077
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'Start' method.
			/// </summary>
			// Token: 0x040061F6 RID: 25078
			public new static readonly StringName Start = "Start";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040061F7 RID: 25079
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040061F8 RID: 25080
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'DisconnectTargetingSignals' method.
			/// </summary>
			// Token: 0x040061F9 RID: 25081
			public static readonly StringName DisconnectTargetingSignals = "DisconnectTargetingSignals";

			/// <summary>
			/// Cached name for the 'OnCancelPlayCard' method.
			/// </summary>
			// Token: 0x040061FA RID: 25082
			public new static readonly StringName OnCancelPlayCard = "OnCancelPlayCard";

			/// <summary>
			/// Cached name for the 'IsCardInPlayZone' method.
			/// </summary>
			// Token: 0x040061FB RID: 25083
			public static readonly StringName IsCardInPlayZone = "IsCardInPlayZone";

			/// <summary>
			/// Cached name for the 'IsCardInCancelZone' method.
			/// </summary>
			// Token: 0x040061FC RID: 25084
			public static readonly StringName IsCardInCancelZone = "IsCardInCancelZone";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018F6 RID: 6390
		[NullableContext(0)]
		public new class PropertyName : NCardPlay.PropertyName
		{
			/// <summary>
			/// Cached name for the 'PlayZoneThreshold' property.
			/// </summary>
			// Token: 0x040061FD RID: 25085
			public static readonly StringName PlayZoneThreshold = "PlayZoneThreshold";

			/// <summary>
			/// Cached name for the 'CancelZoneThreshold' property.
			/// </summary>
			// Token: 0x040061FE RID: 25086
			public static readonly StringName CancelZoneThreshold = "CancelZoneThreshold";

			/// <summary>
			/// Cached name for the '_hasLeftCardCancelZoneOnce' field.
			/// </summary>
			// Token: 0x040061FF RID: 25087
			public static readonly StringName _hasLeftCardCancelZoneOnce = "_hasLeftCardCancelZoneOnce";

			/// <summary>
			/// Cached name for the '_dragStartYPosition' field.
			/// </summary>
			// Token: 0x04006200 RID: 25088
			public static readonly StringName _dragStartYPosition = "_dragStartYPosition";

			/// <summary>
			/// Cached name for the '_isLeftMouseDown' field.
			/// </summary>
			// Token: 0x04006201 RID: 25089
			public static readonly StringName _isLeftMouseDown = "_isLeftMouseDown";

			/// <summary>
			/// Cached name for the '_onCreatureHoverCallable' field.
			/// </summary>
			// Token: 0x04006202 RID: 25090
			public static readonly StringName _onCreatureHoverCallable = "_onCreatureHoverCallable";

			/// <summary>
			/// Cached name for the '_onCreatureUnhoverCallable' field.
			/// </summary>
			// Token: 0x04006203 RID: 25091
			public static readonly StringName _onCreatureUnhoverCallable = "_onCreatureUnhoverCallable";

			/// <summary>
			/// Cached name for the '_signalsConnected' field.
			/// </summary>
			// Token: 0x04006204 RID: 25092
			public static readonly StringName _signalsConnected = "_signalsConnected";

			/// <summary>
			/// Cached name for the '_cancelShortcut' field.
			/// </summary>
			// Token: 0x04006205 RID: 25093
			public static readonly StringName _cancelShortcut = "_cancelShortcut";

			/// <summary>
			/// Cached name for the '_skipStartCardDrag' field.
			/// </summary>
			// Token: 0x04006206 RID: 25094
			public static readonly StringName _skipStartCardDrag = "_skipStartCardDrag";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018F7 RID: 6391
		[NullableContext(0)]
		public new class SignalName : NCardPlay.SignalName
		{
		}
	}
}
