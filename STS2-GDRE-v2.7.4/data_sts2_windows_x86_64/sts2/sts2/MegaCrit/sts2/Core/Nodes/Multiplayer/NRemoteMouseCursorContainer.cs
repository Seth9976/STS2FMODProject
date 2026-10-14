using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// Displays mouse cursors for remote players and syncs the local mouse cursor with remote players.
	/// </summary>
	// Token: 0x02000398 RID: 920
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteMouseCursorContainer.cs")]
	public class NRemoteMouseCursorContainer : Control
	{
		// Token: 0x06003716 RID: 14102 RVA: 0x0019DFC8 File Offset: 0x0019C1C8
		[NullableContext(1)]
		public void Initialize(PeerInputSynchronizer synchronizer, IEnumerable<ulong> connectedPlayerIds)
		{
			if (this._synchronizer != null)
			{
				this.Deinitialize();
			}
			this._synchronizer = synchronizer;
			this._synchronizer.StateAdded += this.OnInputStateAdded;
			this._synchronizer.StateChanged += this.OnInputStateChanged;
			this._synchronizer.StateRemoved += this.OnInputStateRemoved;
			this._synchronizer.NetService.Disconnected += this.NetServiceDisconnected;
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x0019E04B File Offset: 0x0019C24B
		private void NetServiceDisconnected(NetErrorInfo _)
		{
			this.Deinitialize();
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x0019E054 File Offset: 0x0019C254
		public void Deinitialize()
		{
			if (this._synchronizer != null)
			{
				this._synchronizer.StateAdded -= this.OnInputStateAdded;
				this._synchronizer.StateChanged -= this.OnInputStateChanged;
				this._synchronizer.StateRemoved -= this.OnInputStateRemoved;
				this._synchronizer.NetService.Disconnected -= this.NetServiceDisconnected;
				this._synchronizer.Dispose();
				this._synchronizer = null;
			}
			foreach (NRemoteMouseCursor nremoteMouseCursor in this._cursors)
			{
				nremoteMouseCursor.QueueFreeSafely();
			}
			this._cursors.Clear();
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x0019E12C File Offset: 0x0019C32C
		public override void _Ready()
		{
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.OnGuiFocusChanged)), 0U);
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x0019E151 File Offset: 0x0019C351
		public override void _ExitTree()
		{
			this.Deinitialize();
		}

		/// <summary>
		/// Forces all cursors to update their positions, even if they haven't moved.
		/// When cursor positioning is overridden using Synchronizer.StartOverridingCursorPositioning, call this method if
		/// the cursor's screen-space position has been affected, even though the cursor has not moved. For instance, in a
		/// scrollable screen like the map screen, this should be called when the map is scrolled by the local player.
		/// </summary>
		// Token: 0x0600371B RID: 14107 RVA: 0x0019E15C File Offset: 0x0019C35C
		public void ForceUpdateAllCursors()
		{
			foreach (NRemoteMouseCursor nremoteMouseCursor in this._cursors)
			{
				this.OnInputStateChanged(nremoteMouseCursor.PlayerId);
			}
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x0019E1B4 File Offset: 0x0019C3B4
		public Vector2 GetCursorPosition(ulong playerId)
		{
			return this.GetCursor(playerId).Position;
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x0019E1C2 File Offset: 0x0019C3C2
		private void OnInputStateAdded(ulong playerId)
		{
			this.AddCursor(playerId);
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x0019E1CB File Offset: 0x0019C3CB
		private void OnInputStateRemoved(ulong playerId)
		{
			this.RemoveCursor(playerId);
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x0019E1D4 File Offset: 0x0019C3D4
		private void AddCursor(ulong playerId)
		{
			ulong playerId2 = playerId;
			PeerInputSynchronizer synchronizer = this._synchronizer;
			ulong? num = ((synchronizer != null) ? new ulong?(synchronizer.NetService.NetId) : null);
			if ((playerId2 == num.GetValueOrDefault()) & (num != null))
			{
				return;
			}
			if (this._cursors.Any<NRemoteMouseCursor>((NRemoteMouseCursor c) => c.PlayerId == playerId))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to add cursor for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" twice!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			NRemoteMouseCursor nremoteMouseCursor = NRemoteMouseCursor.Create(playerId);
			this._cursors.Add(nremoteMouseCursor);
			this.AddChildSafely(nremoteMouseCursor);
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0019E2A4 File Offset: 0x0019C4A4
		private void OnInputStateChanged(ulong playerId)
		{
			PeerInputSynchronizer synchronizer = this._synchronizer;
			ulong? num = ((synchronizer != null) ? new ulong?(synchronizer.NetService.NetId) : null);
			if ((playerId == num.GetValueOrDefault()) & (num != null))
			{
				this.UpdateCursorVisibility();
				return;
			}
			Vector2 controlSpaceFocusPosition = this._synchronizer.GetControlSpaceFocusPosition(playerId, this);
			NRemoteMouseCursor cursor = this.GetCursor(playerId);
			cursor.SetNextPosition(controlSpaceFocusPosition);
			cursor.UpdateImage(this._synchronizer.GetMouseDown(playerId), NRemoteMouseCursorContainer.GetDrawingMode(playerId));
			this.UpdateCursorVisibility();
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x0019E32C File Offset: 0x0019C52C
		public void DrawingCursorStateChanged(ulong playerId)
		{
			NRemoteMouseCursor cursor = this.GetCursor(playerId);
			if (cursor != null)
			{
				cursor.UpdateImage(this._synchronizer.GetMouseDown(playerId), NRemoteMouseCursorContainer.GetDrawingMode(playerId));
			}
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x0019E35C File Offset: 0x0019C55C
		private static DrawingMode GetDrawingMode(ulong playerId)
		{
			if (NRun.Instance != null)
			{
				return NRun.Instance.GlobalUi.MapScreen.Drawings.GetDrawingMode(playerId);
			}
			return DrawingMode.None;
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x0019E384 File Offset: 0x0019C584
		[NullableContext(2)]
		private NRemoteMouseCursor GetCursor(ulong playerId)
		{
			return this._cursors.FirstOrDefault<NRemoteMouseCursor>((NRemoteMouseCursor c) => c.PlayerId == playerId);
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x0019E3B8 File Offset: 0x0019C5B8
		private void RemoveCursor(ulong playerId)
		{
			NRemoteMouseCursor cursor = this.GetCursor(playerId);
			if (cursor != null)
			{
				cursor.QueueFreeSafely();
				this._cursors.Remove(cursor);
			}
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0019E3E4 File Offset: 0x0019C5E4
		private void UpdateCursorVisibility()
		{
			NetScreenType screenType = this._synchronizer.GetScreenType(this._synchronizer.NetService.NetId);
			foreach (NRemoteMouseCursor nremoteMouseCursor in this._cursors)
			{
				NetScreenType screenType2 = this._synchronizer.GetScreenType(nremoteMouseCursor.PlayerId);
				bool flag = screenType == screenType2;
				bool flag2 = screenType2 - NetScreenType.DeckView <= 3 || screenType2 == NetScreenType.RemotePlayerExpandedState;
				bool flag3 = flag2;
				bool flag4 = screenType2 == NetScreenType.SharedRelicPicking;
				nremoteMouseCursor.Visible = flag && !flag3 && !flag4;
			}
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x0019E498 File Offset: 0x0019C698
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (this._synchronizer == null)
			{
				return;
			}
			if (inputEvent.IsActionReleased(DebugHotkey.hideMpCursors, false))
			{
				NRemoteMouseCursorContainer._isDebugUiVisible = !NRemoteMouseCursorContainer._isDebugUiVisible;
				this.ApplyDebugUiVisibility();
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NRemoteMouseCursorContainer._isDebugUiVisible ? "Show MP Cursors" : "Hide MP Cursors"));
			}
			InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
			if (inputEventMouseMotion != null)
			{
				this._synchronizer.SyncLocalIsUsingController(false);
				if (!NGame.Instance.ReactionWheel.Visible)
				{
					this._synchronizer.SyncLocalMousePos(inputEventMouseMotion.Position, this);
					return;
				}
			}
			else
			{
				InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
				if (inputEventMouseButton != null)
				{
					this._synchronizer.SyncLocalIsUsingController(false);
					if (inputEventMouseButton.ButtonIndex == 1L)
					{
						this._synchronizer.SyncLocalMouseDown(inputEventMouseButton.Pressed);
					}
				}
			}
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x0019E55C File Offset: 0x0019C75C
		[NullableContext(1)]
		private void OnGuiFocusChanged(Control focused)
		{
			if (this._synchronizer == null)
			{
				return;
			}
			NControllerManager instance = NControllerManager.Instance;
			if (instance == null || !instance.IsUsingController)
			{
				return;
			}
			this._synchronizer.SyncLocalIsUsingController(true);
			this._synchronizer.SyncLocalControllerFocus(focused.GlobalPosition + focused.Size * 0.5f, this);
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x0019E5B9 File Offset: 0x0019C7B9
		private void ApplyDebugUiVisibility()
		{
			base.Visible = NRemoteMouseCursorContainer._isDebugUiVisible;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003729 RID: 14121 RVA: 0x0019E5C8 File Offset: 0x0019C7C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(17)
			{
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.Deinitialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.ForceUpdateAllCursors, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.GetCursorPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnInputStateAdded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnInputStateRemoved, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.AddCursor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnInputStateChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.DrawingCursorStateChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.GetDrawingMode, new PropertyInfo(2L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.GetCursor, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.RemoveCursor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.UpdateCursorVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.OnGuiFocusChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focused", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRemoteMouseCursorContainer.MethodName.ApplyDebugUiVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600372A RID: 14122 RVA: 0x0019EAB8 File Offset: 0x0019CCB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.Deinitialize && args.Count == 0)
			{
				this.Deinitialize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.ForceUpdateAllCursors && args.Count == 0)
			{
				this.ForceUpdateAllCursors();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.GetCursorPosition && args.Count == 1)
			{
				Vector2 cursorPosition = this.GetCursorPosition(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref cursorPosition);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.OnInputStateAdded && args.Count == 1)
			{
				this.OnInputStateAdded(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.OnInputStateRemoved && args.Count == 1)
			{
				this.OnInputStateRemoved(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.AddCursor && args.Count == 1)
			{
				this.AddCursor(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.OnInputStateChanged && args.Count == 1)
			{
				this.OnInputStateChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.DrawingCursorStateChanged && args.Count == 1)
			{
				this.DrawingCursorStateChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.GetDrawingMode && args.Count == 1)
			{
				DrawingMode drawingMode = NRemoteMouseCursorContainer.GetDrawingMode(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.GetCursor && args.Count == 1)
			{
				NRemoteMouseCursor cursor = this.GetCursor(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<NRemoteMouseCursor>(ref cursor);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.RemoveCursor && args.Count == 1)
			{
				this.RemoveCursor(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.UpdateCursorVisibility && args.Count == 0)
			{
				this.UpdateCursorVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.OnGuiFocusChanged && args.Count == 1)
			{
				this.OnGuiFocusChanged(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.ApplyDebugUiVisibility && args.Count == 0)
			{
				this.ApplyDebugUiVisibility();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x0019EDF4 File Offset: 0x0019CFF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteMouseCursorContainer.MethodName.GetDrawingMode && args.Count == 1)
			{
				DrawingMode drawingMode = NRemoteMouseCursorContainer.GetDrawingMode(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600372C RID: 14124 RVA: 0x0019EE44 File Offset: 0x0019D044
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRemoteMouseCursorContainer.MethodName.Deinitialize || (ref method) == NRemoteMouseCursorContainer.MethodName._Ready || (ref method) == NRemoteMouseCursorContainer.MethodName._ExitTree || (ref method) == NRemoteMouseCursorContainer.MethodName.ForceUpdateAllCursors || (ref method) == NRemoteMouseCursorContainer.MethodName.GetCursorPosition || (ref method) == NRemoteMouseCursorContainer.MethodName.OnInputStateAdded || (ref method) == NRemoteMouseCursorContainer.MethodName.OnInputStateRemoved || (ref method) == NRemoteMouseCursorContainer.MethodName.AddCursor || (ref method) == NRemoteMouseCursorContainer.MethodName.OnInputStateChanged || (ref method) == NRemoteMouseCursorContainer.MethodName.DrawingCursorStateChanged || (ref method) == NRemoteMouseCursorContainer.MethodName.GetDrawingMode || (ref method) == NRemoteMouseCursorContainer.MethodName.GetCursor || (ref method) == NRemoteMouseCursorContainer.MethodName.RemoveCursor || (ref method) == NRemoteMouseCursorContainer.MethodName.UpdateCursorVisibility || (ref method) == NRemoteMouseCursorContainer.MethodName._Input || (ref method) == NRemoteMouseCursorContainer.MethodName.OnGuiFocusChanged || (ref method) == NRemoteMouseCursorContainer.MethodName.ApplyDebugUiVisibility || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600372D RID: 14125 RVA: 0x0019EF57 File Offset: 0x0019D157
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600372E RID: 14126 RVA: 0x0019EF60 File Offset: 0x0019D160
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04001563 RID: 5475
		private static bool _isDebugUiVisible = true;

		// Token: 0x04001564 RID: 5476
		[Nullable(2)]
		private PeerInputSynchronizer _synchronizer;

		// Token: 0x04001565 RID: 5477
		[Nullable(1)]
		private readonly List<NRemoteMouseCursor> _cursors = new List<NRemoteMouseCursor>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001751 RID: 5969
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Deinitialize' method.
			/// </summary>
			// Token: 0x04005AFB RID: 23291
			public static readonly StringName Deinitialize = "Deinitialize";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005AFC RID: 23292
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005AFD RID: 23293
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ForceUpdateAllCursors' method.
			/// </summary>
			// Token: 0x04005AFE RID: 23294
			public static readonly StringName ForceUpdateAllCursors = "ForceUpdateAllCursors";

			/// <summary>
			/// Cached name for the 'GetCursorPosition' method.
			/// </summary>
			// Token: 0x04005AFF RID: 23295
			public static readonly StringName GetCursorPosition = "GetCursorPosition";

			/// <summary>
			/// Cached name for the 'OnInputStateAdded' method.
			/// </summary>
			// Token: 0x04005B00 RID: 23296
			public static readonly StringName OnInputStateAdded = "OnInputStateAdded";

			/// <summary>
			/// Cached name for the 'OnInputStateRemoved' method.
			/// </summary>
			// Token: 0x04005B01 RID: 23297
			public static readonly StringName OnInputStateRemoved = "OnInputStateRemoved";

			/// <summary>
			/// Cached name for the 'AddCursor' method.
			/// </summary>
			// Token: 0x04005B02 RID: 23298
			public static readonly StringName AddCursor = "AddCursor";

			/// <summary>
			/// Cached name for the 'OnInputStateChanged' method.
			/// </summary>
			// Token: 0x04005B03 RID: 23299
			public static readonly StringName OnInputStateChanged = "OnInputStateChanged";

			/// <summary>
			/// Cached name for the 'DrawingCursorStateChanged' method.
			/// </summary>
			// Token: 0x04005B04 RID: 23300
			public static readonly StringName DrawingCursorStateChanged = "DrawingCursorStateChanged";

			/// <summary>
			/// Cached name for the 'GetDrawingMode' method.
			/// </summary>
			// Token: 0x04005B05 RID: 23301
			public static readonly StringName GetDrawingMode = "GetDrawingMode";

			/// <summary>
			/// Cached name for the 'GetCursor' method.
			/// </summary>
			// Token: 0x04005B06 RID: 23302
			public static readonly StringName GetCursor = "GetCursor";

			/// <summary>
			/// Cached name for the 'RemoveCursor' method.
			/// </summary>
			// Token: 0x04005B07 RID: 23303
			public static readonly StringName RemoveCursor = "RemoveCursor";

			/// <summary>
			/// Cached name for the 'UpdateCursorVisibility' method.
			/// </summary>
			// Token: 0x04005B08 RID: 23304
			public static readonly StringName UpdateCursorVisibility = "UpdateCursorVisibility";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005B09 RID: 23305
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'OnGuiFocusChanged' method.
			/// </summary>
			// Token: 0x04005B0A RID: 23306
			public static readonly StringName OnGuiFocusChanged = "OnGuiFocusChanged";

			/// <summary>
			/// Cached name for the 'ApplyDebugUiVisibility' method.
			/// </summary>
			// Token: 0x04005B0B RID: 23307
			public static readonly StringName ApplyDebugUiVisibility = "ApplyDebugUiVisibility";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001752 RID: 5970
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001753 RID: 5971
		public class SignalName : Control.SignalName
		{
		}
	}
}
