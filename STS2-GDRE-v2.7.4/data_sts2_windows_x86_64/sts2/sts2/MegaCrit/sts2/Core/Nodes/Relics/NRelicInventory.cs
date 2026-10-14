using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Relics
{
	/// <summary>
	/// UI for holding relics. Is located under the top bar.
	/// Positioning is handled by the HFlowContainer.
	/// </summary>
	// Token: 0x02000378 RID: 888
	[ScriptPath("res://src/Core/Nodes/Relics/NRelicInventory.cs")]
	public class NRelicInventory : FlowContainer
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x00186C6D File Offset: 0x00184E6D
		[Nullable(1)]
		public IReadOnlyList<NRelicInventoryHolder> RelicNodes
		{
			[NullableContext(1)]
			get
			{
				return this._relicNodes;
			}
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x00186C75 File Offset: 0x00184E75
		public override void _Ready()
		{
			this._originalPos = base.Position;
			base.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._relicNodes[0].TryGrabFocus();
			}), 0U);
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x00186CA1 File Offset: 0x00184EA1
		public override void _EnterTree()
		{
			base._EnterTree();
			this.ConnectPlayerEvents();
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x00186CAF File Offset: 0x00184EAF
		public override void _ExitTree()
		{
			base._ExitTree();
			this.DisconnectPlayerEvents();
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x00186CC0 File Offset: 0x00184EC0
		[NullableContext(1)]
		public void Initialize(RunState runState)
		{
			this.DisconnectPlayerEvents();
			this._player = LocalContext.GetMe(runState);
			this.ConnectPlayerEvents();
			foreach (RelicModel relicModel in this._player.Relics)
			{
				this.Add(relicModel, true, -1);
			}
			this.UpdateNavigation();
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x00186D34 File Offset: 0x00184F34
		private void ConnectPlayerEvents()
		{
			if (this._player == null)
			{
				return;
			}
			this._player.RelicObtained += this.OnRelicObtained;
			this._player.RelicRemoved += this.OnRelicRemoved;
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x00186D6D File Offset: 0x00184F6D
		private void DisconnectPlayerEvents()
		{
			if (this._player == null)
			{
				return;
			}
			this._player.RelicObtained -= this.OnRelicObtained;
			this._player.RelicRemoved -= this.OnRelicRemoved;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x00186DA8 File Offset: 0x00184FA8
		[NullableContext(1)]
		private void Add(RelicModel relic, bool startsShown, int index = -1)
		{
			NRelicInventoryHolder nrelicInventoryHolder = NRelicInventoryHolder.Create(relic);
			nrelicInventoryHolder.Inventory = this;
			if (index < 0)
			{
				this._relicNodes.Add(nrelicInventoryHolder);
			}
			else
			{
				this._relicNodes.Insert(index, nrelicInventoryHolder);
			}
			this.AddChildSafely(nrelicInventoryHolder);
			this.MoveChildSafely(nrelicInventoryHolder, index);
			if (!startsShown)
			{
				CanvasItem icon = nrelicInventoryHolder.Relic.Icon;
				Color modulate = nrelicInventoryHolder.Relic.Icon.Modulate;
				modulate.A = 0f;
				icon.Modulate = modulate;
				this.UpdateNavigation();
			}
			nrelicInventoryHolder.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.OnRelicClicked(relic);
			}), 0U);
			nrelicInventoryHolder.Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				this.OnRelicFocused(relic);
			}), 0U);
			nrelicInventoryHolder.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				NRelicInventory.OnRelicUnfocused();
			}), 0U);
			base.EmitSignal(NRelicInventory.SignalName.RelicsChanged, Array.Empty<Variant>());
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x00186EC0 File Offset: 0x001850C0
		[NullableContext(1)]
		private void Remove(RelicModel relic)
		{
			if (!LocalContext.IsMine(relic))
			{
				return;
			}
			NRelicInventoryHolder nrelicInventoryHolder = this._relicNodes.First<NRelicInventoryHolder>((NRelicInventoryHolder n) => n.Relic.Model == relic);
			this._relicNodes.Remove(nrelicInventoryHolder);
			this.RemoveChildSafely(nrelicInventoryHolder);
			this.EmitSignalRelicsChanged();
			this.UpdateNavigation();
		}

		/// <summary>
		/// Opens the Inspect Relic screen and allows the player to paginate through the rest of the top bar relics.
		/// </summary>
		// Token: 0x0600347E RID: 13438 RVA: 0x00186F20 File Offset: 0x00185120
		[NullableContext(1)]
		private void OnRelicClicked(RelicModel model)
		{
			List<RelicModel> list = new List<RelicModel>();
			foreach (NRelicInventoryHolder nrelicInventoryHolder in this._relicNodes)
			{
				list.Add(nrelicInventoryHolder.Relic.Model);
			}
			NGame.Instance.GetInspectRelicScreen().Open(list, model);
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x00186F94 File Offset: 0x00185194
		[NullableContext(1)]
		private void OnRelicFocused(RelicModel model)
		{
			RunManager.Instance.HoveredModelTracker.OnLocalRelicHovered(model);
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x00186FA6 File Offset: 0x001851A6
		private static void OnRelicUnfocused()
		{
			RunManager.Instance.HoveredModelTracker.OnLocalRelicUnhovered();
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x00186FB8 File Offset: 0x001851B8
		[NullableContext(1)]
		public void AnimateRelic(RelicModel relic, Vector2? startPosition = null, Vector2? startScale = null)
		{
			if (!LocalContext.IsMine(relic))
			{
				return;
			}
			NRelicInventoryHolder nrelicInventoryHolder = this._relicNodes.First<NRelicInventoryHolder>((NRelicInventoryHolder n) => n.Relic.Model == relic);
			TaskHelper.RunSafely(nrelicInventoryHolder.PlayNewlyAcquiredAnimation(startPosition, startScale));
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x00187006 File Offset: 0x00185206
		[NullableContext(1)]
		private void OnRelicObtained(RelicModel relic)
		{
			this.Add(relic, false, this._player.Relics.IndexOf(relic));
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00187021 File Offset: 0x00185221
		[NullableContext(1)]
		private void OnRelicRemoved(RelicModel relic)
		{
			this.Remove(relic);
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x0018702C File Offset: 0x0018522C
		public void AnimShow()
		{
			base.FocusBehaviorRecursive = 2L;
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			this._curTween = base.CreateTween();
			this._curTween.TweenProperty(this, "global_position:y", this._originalPos.Y, 0.25).SetTrans(7L).SetEase(1L);
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x0018709C File Offset: 0x0018529C
		public void AnimHide()
		{
			base.FocusBehaviorRecursive = 1L;
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			this._curTween = base.CreateTween();
			this._curTween.TweenProperty(this, "global_position:y", this._originalPos.Y - 68f * (float)base.GetLineCount() - 90f, 0.25).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x00187120 File Offset: 0x00185320
		public void ShowImmediately()
		{
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			Vector2 position = base.Position;
			position.Y = this._originalPos.Y;
			base.Position = position;
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00187160 File Offset: 0x00185360
		public void HideImmediately()
		{
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			Vector2 position = base.Position;
			position.Y = this._originalPos.Y - 68f * (float)base.GetLineCount() - 90f;
			base.Position = position;
		}

		/// <summary>
		/// Returns the position the relic inventory is at normally.
		/// this.Position can change if the relic inventory is hidden, so this is used when we want to position elements
		/// relative to the relic inventory.
		/// </summary>
		// Token: 0x06003488 RID: 13448 RVA: 0x001871B2 File Offset: 0x001853B2
		public Vector2 GetDefaultPosition()
		{
			return this._originalPos;
		}

		/// <summary>
		/// Used for trailers
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003489 RID: 13449 RVA: 0x001871BA File Offset: 0x001853BA
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.hideTopBar, false))
			{
				this.DebugHideTopBar();
			}
		}

		/// <summary>
		/// Used for trailers
		/// </summary>
		// Token: 0x0600348A RID: 13450 RVA: 0x001871D0 File Offset: 0x001853D0
		private void DebugHideTopBar()
		{
			if (this._isDebugHidden)
			{
				this.AnimShow();
			}
			else
			{
				this.AnimHide();
			}
			this._isDebugHidden = !this._isDebugHidden;
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x001871F8 File Offset: 0x001853F8
		private void UpdateNavigation()
		{
			for (int i = 0; i < this.RelicNodes.Count; i++)
			{
				NRelicInventoryHolder nrelicInventoryHolder = this.RelicNodes[i];
				Control control = nrelicInventoryHolder;
				NodePath nodePath;
				if (i <= 0)
				{
					IReadOnlyList<NRelicInventoryHolder> relicNodes = this.RelicNodes;
					nodePath = relicNodes[relicNodes.Count - 1].GetPath();
				}
				else
				{
					nodePath = this.RelicNodes[i - 1].GetPath();
				}
				control.FocusNeighborLeft = nodePath;
				nrelicInventoryHolder.FocusNeighborRight = ((i < this.RelicNodes.Count - 1) ? this.RelicNodes[i + 1].GetPath() : this.RelicNodes[0].GetPath());
				Control firstPotionControl = NRun.Instance.GlobalUi.TopBar.PotionContainer.FirstPotionControl;
				nrelicInventoryHolder.FocusNeighborTop = ((firstPotionControl != null && GodotObject.IsInstanceValid(firstPotionControl)) ? firstPotionControl.GetPath() : nrelicInventoryHolder.GetPath());
				NMultiplayerPlayerStateContainer multiplayerPlayerContainer = NRun.Instance.GlobalUi.MultiplayerPlayerContainer;
				if (multiplayerPlayerContainer.GetChildCount(false) > 0)
				{
					NMultiplayerPlayerState firstPlayerState = multiplayerPlayerContainer.FirstPlayerState;
					Control control2 = ((firstPlayerState != null) ? firstPlayerState.Hitbox : null);
					nrelicInventoryHolder.FocusNeighborBottom = ((control2 != null && GodotObject.IsInstanceValid(control2)) ? control2.GetPath() : nrelicInventoryHolder.GetPath());
				}
				else
				{
					nrelicInventoryHolder.FocusNeighborBottom = NRun.Instance.GlobalUi.TopBar.ActiveScreenProxy.GetPath();
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600348C RID: 13452 RVA: 0x0018734C File Offset: 0x0018554C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NRelicInventory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.ConnectPlayerEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.DisconnectPlayerEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.OnRelicUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NRelicInventory.MethodName.AnimShow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.AnimHide, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.ShowImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.HideImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.GetDefaultPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NRelicInventory.MethodName.DebugHideTopBar, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventory.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600348D RID: 13453 RVA: 0x00187628 File Offset: 0x00185828
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicInventory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.ConnectPlayerEvents && args.Count == 0)
			{
				this.ConnectPlayerEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.DisconnectPlayerEvents && args.Count == 0)
			{
				this.DisconnectPlayerEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.OnRelicUnfocused && args.Count == 0)
			{
				NRelicInventory.OnRelicUnfocused();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.AnimShow && args.Count == 0)
			{
				this.AnimShow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.AnimHide && args.Count == 0)
			{
				this.AnimHide();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.ShowImmediately && args.Count == 0)
			{
				this.ShowImmediately();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.HideImmediately && args.Count == 0)
			{
				this.HideImmediately();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.GetDefaultPosition && args.Count == 0)
			{
				Vector2 defaultPosition = this.GetDefaultPosition();
				ret = VariantUtils.CreateFrom<Vector2>(ref defaultPosition);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.DebugHideTopBar && args.Count == 0)
			{
				this.DebugHideTopBar();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventory.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x00187858 File Offset: 0x00185A58
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicInventory.MethodName.OnRelicUnfocused && args.Count == 0)
			{
				NRelicInventory.OnRelicUnfocused();
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600348F RID: 13455 RVA: 0x00187888 File Offset: 0x00185A88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicInventory.MethodName._Ready || (ref method) == NRelicInventory.MethodName._EnterTree || (ref method) == NRelicInventory.MethodName._ExitTree || (ref method) == NRelicInventory.MethodName.ConnectPlayerEvents || (ref method) == NRelicInventory.MethodName.DisconnectPlayerEvents || (ref method) == NRelicInventory.MethodName.OnRelicUnfocused || (ref method) == NRelicInventory.MethodName.AnimShow || (ref method) == NRelicInventory.MethodName.AnimHide || (ref method) == NRelicInventory.MethodName.ShowImmediately || (ref method) == NRelicInventory.MethodName.HideImmediately || (ref method) == NRelicInventory.MethodName.GetDefaultPosition || (ref method) == NRelicInventory.MethodName._Input || (ref method) == NRelicInventory.MethodName.DebugHideTopBar || (ref method) == NRelicInventory.MethodName.UpdateNavigation || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003490 RID: 13456 RVA: 0x00187970 File Offset: 0x00185B70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicInventory.PropertyName._originalPos)
			{
				this._originalPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventory.PropertyName._curTween)
			{
				this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventory.PropertyName._debugHideTween)
			{
				this._debugHideTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventory.PropertyName._isDebugHidden)
			{
				this._isDebugHidden = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003491 RID: 13457 RVA: 0x001879F4 File Offset: 0x00185BF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicInventory.PropertyName._originalPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalPos);
				return true;
			}
			if ((ref name) == NRelicInventory.PropertyName._curTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
				return true;
			}
			if ((ref name) == NRelicInventory.PropertyName._debugHideTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._debugHideTween);
				return true;
			}
			if ((ref name) == NRelicInventory.PropertyName._isDebugHidden)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDebugHidden);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003492 RID: 13458 RVA: 0x00187A8C File Offset: 0x00185C8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NRelicInventory.PropertyName._originalPos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicInventory.PropertyName._curTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicInventory.PropertyName._debugHideTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRelicInventory.PropertyName._isDebugHidden, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003493 RID: 13459 RVA: 0x00187B24 File Offset: 0x00185D24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRelicInventory.PropertyName._originalPos, Variant.From<Vector2>(ref this._originalPos));
			info.AddProperty(NRelicInventory.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
			info.AddProperty(NRelicInventory.PropertyName._debugHideTween, Variant.From<Tween>(ref this._debugHideTween));
			info.AddProperty(NRelicInventory.PropertyName._isDebugHidden, Variant.From<bool>(ref this._isDebugHidden));
			info.AddSignalEventDelegate(NRelicInventory.SignalName.RelicsChanged, this.backing_RelicsChanged);
		}

		/// <inheritdoc />
		// Token: 0x06003494 RID: 13460 RVA: 0x00187BA4 File Offset: 0x00185DA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicInventory.PropertyName._originalPos, ref variant))
			{
				this._originalPos = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicInventory.PropertyName._curTween, ref variant2))
			{
				this._curTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelicInventory.PropertyName._debugHideTween, ref variant3))
			{
				this._debugHideTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRelicInventory.PropertyName._isDebugHidden, ref variant4))
			{
				this._isDebugHidden = variant4.As<bool>();
			}
			NRelicInventory.RelicsChangedEventHandler relicsChangedEventHandler;
			if (info.TryGetSignalEventDelegate<NRelicInventory.RelicsChangedEventHandler>(NRelicInventory.SignalName.RelicsChanged, ref relicsChangedEventHandler))
			{
				this.backing_RelicsChanged = relicsChangedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003495 RID: 13461 RVA: 0x00187C40 File Offset: 0x00185E40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NRelicInventory.SignalName.RelicsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Relics.NRelicInventory.RelicsChangedEventHandler" />
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06003496 RID: 13462 RVA: 0x00187C84 File Offset: 0x00185E84
		// (remove) Token: 0x06003497 RID: 13463 RVA: 0x00187C9D File Offset: 0x00185E9D
		public event NRelicInventory.RelicsChangedEventHandler RelicsChanged
		{
			add
			{
				this.backing_RelicsChanged = (NRelicInventory.RelicsChangedEventHandler)Delegate.Combine(this.backing_RelicsChanged, value);
			}
			remove
			{
				this.backing_RelicsChanged = (NRelicInventory.RelicsChangedEventHandler)Delegate.Remove(this.backing_RelicsChanged, value);
			}
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x00187CB6 File Offset: 0x00185EB6
		protected void EmitSignalRelicsChanged()
		{
			base.EmitSignal(NRelicInventory.SignalName.RelicsChanged, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06003499 RID: 13465 RVA: 0x00187CC9 File Offset: 0x00185EC9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NRelicInventory.SignalName.RelicsChanged) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NRelicInventory.RelicsChangedEventHandler relicsChangedEventHandler = this.backing_RelicsChanged;
			if (relicsChangedEventHandler == null)
			{
				return;
			}
			relicsChangedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x0600349A RID: 13466 RVA: 0x00187CFA File Offset: 0x00185EFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NRelicInventory.SignalName.RelicsChanged || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x0400145E RID: 5214
		[Nullable(2)]
		private Player _player;

		// Token: 0x0400145F RID: 5215
		[Nullable(1)]
		private readonly List<NRelicInventoryHolder> _relicNodes = new List<NRelicInventoryHolder>();

		// Token: 0x04001460 RID: 5216
		private Vector2 _originalPos;

		// Token: 0x04001461 RID: 5217
		[Nullable(2)]
		private Tween _curTween;

		// Token: 0x04001462 RID: 5218
		[Nullable(2)]
		private Tween _debugHideTween;

		// Token: 0x04001463 RID: 5219
		private bool _isDebugHidden;

		// Token: 0x04001464 RID: 5220
		private NRelicInventory.RelicsChangedEventHandler backing_RelicsChanged;

		// Token: 0x020016CB RID: 5835
		// (Invoke) Token: 0x0600A579 RID: 42361
		[Signal]
		public delegate void RelicsChangedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016CC RID: 5836
		public class MethodName : FlowContainer.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040058D5 RID: 22741
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040058D6 RID: 22742
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040058D7 RID: 22743
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ConnectPlayerEvents' method.
			/// </summary>
			// Token: 0x040058D8 RID: 22744
			public static readonly StringName ConnectPlayerEvents = "ConnectPlayerEvents";

			/// <summary>
			/// Cached name for the 'DisconnectPlayerEvents' method.
			/// </summary>
			// Token: 0x040058D9 RID: 22745
			public static readonly StringName DisconnectPlayerEvents = "DisconnectPlayerEvents";

			/// <summary>
			/// Cached name for the 'OnRelicUnfocused' method.
			/// </summary>
			// Token: 0x040058DA RID: 22746
			public static readonly StringName OnRelicUnfocused = "OnRelicUnfocused";

			/// <summary>
			/// Cached name for the 'AnimShow' method.
			/// </summary>
			// Token: 0x040058DB RID: 22747
			public static readonly StringName AnimShow = "AnimShow";

			/// <summary>
			/// Cached name for the 'AnimHide' method.
			/// </summary>
			// Token: 0x040058DC RID: 22748
			public static readonly StringName AnimHide = "AnimHide";

			/// <summary>
			/// Cached name for the 'ShowImmediately' method.
			/// </summary>
			// Token: 0x040058DD RID: 22749
			public static readonly StringName ShowImmediately = "ShowImmediately";

			/// <summary>
			/// Cached name for the 'HideImmediately' method.
			/// </summary>
			// Token: 0x040058DE RID: 22750
			public static readonly StringName HideImmediately = "HideImmediately";

			/// <summary>
			/// Cached name for the 'GetDefaultPosition' method.
			/// </summary>
			// Token: 0x040058DF RID: 22751
			public static readonly StringName GetDefaultPosition = "GetDefaultPosition";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x040058E0 RID: 22752
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'DebugHideTopBar' method.
			/// </summary>
			// Token: 0x040058E1 RID: 22753
			public static readonly StringName DebugHideTopBar = "DebugHideTopBar";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x040058E2 RID: 22754
			public static readonly StringName UpdateNavigation = "UpdateNavigation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016CD RID: 5837
		public class PropertyName : FlowContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the '_originalPos' field.
			/// </summary>
			// Token: 0x040058E3 RID: 22755
			public static readonly StringName _originalPos = "_originalPos";

			/// <summary>
			/// Cached name for the '_curTween' field.
			/// </summary>
			// Token: 0x040058E4 RID: 22756
			public static readonly StringName _curTween = "_curTween";

			/// <summary>
			/// Cached name for the '_debugHideTween' field.
			/// </summary>
			// Token: 0x040058E5 RID: 22757
			public static readonly StringName _debugHideTween = "_debugHideTween";

			/// <summary>
			/// Cached name for the '_isDebugHidden' field.
			/// </summary>
			// Token: 0x040058E6 RID: 22758
			public static readonly StringName _isDebugHidden = "_isDebugHidden";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016CE RID: 5838
		public class SignalName : FlowContainer.SignalName
		{
			/// <summary>
			/// Cached name for the 'RelicsChanged' signal.
			/// </summary>
			// Token: 0x040058E7 RID: 22759
			public static readonly StringName RelicsChanged = "RelicsChanged";
		}
	}
}
