using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	/// <summary>
	/// The slots container intercepts inputs and transforms its child node "WhatsMoved" we do this because if this Node is moved too much we can't intercept GUI inputs and such.
	/// We could probably bypass this by using _Input oops. Whatever.
	/// </summary>
	// Token: 0x02000263 RID: 611
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NSlotsContainer.cs")]
	public class NSlotsContainer : Control
	{
		// Token: 0x06001FF5 RID: 8181 RVA: 0x000D792C File Offset: 0x000D5B2C
		public override void _Ready()
		{
			this._whatsMoved = base.GetNode<Control>("%WhatsMoved");
			this._epochSlots = base.GetNode<Control>("%EpochSlots");
			this._targetPosition = this._whatsMoved.Position;
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnToggleVisibility)), 0U);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000D7994 File Offset: 0x000D5B94
		public override void _EnterTree()
		{
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000D79B9 File Offset: 0x000D5BB9
		public override void _ExitTree()
		{
			base.GetViewport().Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000D79DC File Offset: 0x000D5BDC
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			this.ProcessPanEvent(inputEvent);
			this.ProcessScrollEvent(inputEvent);
		}

		/// <summary>
		/// Processes clicking and dragging the screen horizontally.
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06001FF9 RID: 8185 RVA: 0x000D79EC File Offset: 0x000D5BEC
		[NullableContext(1)]
		private void ProcessPanEvent(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton == null || inputEventMouseButton.ButtonIndex != 1L)
			{
				InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
				if (inputEventMouseMotion != null && this._isDragging)
				{
					Vector2 vector = inputEventMouseMotion.Position - this._dragStartPosition;
					this._targetPosition += new Vector2(vector.X, 0f);
					this._dragStartPosition = inputEventMouseMotion.Position;
				}
				return;
			}
			if (inputEventMouseButton.Pressed)
			{
				this._isDragging = true;
				this._dragStartPosition = inputEventMouseButton.Position;
				return;
			}
			this._isDragging = false;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000D7A84 File Offset: 0x000D5C84
		[NullableContext(1)]
		private void ProcessGuiFocus(Control focusedControl)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			if (base.IsAncestorOf(focusedControl))
			{
				float num = this._whatsMoved.GlobalPosition.X - focusedControl.GetParent<Control>().GlobalPosition.X;
				this._targetPosition = new Vector2(num, this._targetPosition.Y);
			}
		}

		/// <summary>
		/// Processes mouse wheel to zoom in and out of the Timeline screen.
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06001FFB RID: 8187 RVA: 0x000D7AEC File Offset: 0x000D5CEC
		[NullableContext(1)]
		private void ProcessScrollEvent(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				if (inputEventMouseButton.ButtonIndex == 4L)
				{
					this._targetPosition -= new Vector2(50f, 0f);
					return;
				}
				if (inputEventMouseButton.ButtonIndex == 5L)
				{
					this._targetPosition += new Vector2(50f, 0f);
					return;
				}
				if (inputEventMouseButton.ButtonIndex == 7L)
				{
					this._targetPosition -= new Vector2(50f, 0f);
					return;
				}
				if (inputEventMouseButton.ButtonIndex == 6L)
				{
					this._targetPosition += new Vector2(50f, 0f);
					return;
				}
			}
			else
			{
				InputEventPanGesture inputEventPanGesture = inputEvent as InputEventPanGesture;
				if (inputEventPanGesture != null)
				{
					this._targetPosition += new Vector2(-inputEventPanGesture.Delta.X * 20f, 0f);
				}
			}
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000D7BEC File Offset: 0x000D5DEC
		public override void _Process(double delta)
		{
			float num = (float)Mathf.Sign(this._whatsMoved.Position.X - this._targetPosition.X);
			this._whatsMoved.Position = this._whatsMoved.Position.Lerp(this._targetPosition, (float)delta * 20f);
			float num2 = (float)Mathf.Sign(this._whatsMoved.Position.X - this._targetPosition.X);
			if (Math.Abs(this._whatsMoved.Position.X - this._targetPosition.X) < 0.5f || !Mathf.IsEqualApprox(num, num2))
			{
				this._whatsMoved.Position = this._targetPosition;
			}
			if (!this._isDragging)
			{
				float num3 = this._targetPosition.X;
				float num4 = this._epochSlots.Position.X - this._whatsMoved.Size.X;
				float num5 = this._epochSlots.Position.X + this._epochSlots.Size.X - this._whatsMoved.Size.X;
				if (num3 < num4)
				{
					num3 = Mathf.Lerp(num3, num4, (float)delta * 36f);
				}
				else if (num3 > num5)
				{
					num3 = Mathf.Lerp(num3, num5, (float)delta * 36f);
				}
				this._targetPosition = new Vector2(num3, this._targetPosition.Y);
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000D7D60 File Offset: 0x000D5F60
		private void OnToggleVisibility()
		{
			if (base.Visible)
			{
				this._targetPosition = this._whatsMoved.Position;
				this._dragStartPosition = Vector2.Zero;
			}
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000D7D86 File Offset: 0x000D5F86
		public void Reset()
		{
			this._whatsMoved.Position = new Vector2(-960f, this._whatsMoved.Position.Y);
		}

		/// <summary>
		/// When the Timeline is opened, if there are Epochs the game will automatically center to the closest one.
		/// </summary>
		// Token: 0x06001FFF RID: 8191 RVA: 0x000D7DB0 File Offset: 0x000D5FB0
		[NullableContext(1)]
		public async Task LerpToSlot(float slotPositionX)
		{
			float num = this._whatsMoved.GlobalPosition.X - slotPositionX + 960f - 96f + (base.Size.X - 1920f) * 0.5f;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this._whatsMoved, "global_position:x", num, 2.5).SetEase(2L).SetTrans(7L);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this._targetPosition = this._whatsMoved.Position;
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x000D7DFB File Offset: 0x000D5FFB
		public void SetEnabled(bool enabled)
		{
			base.FocusBehaviorRecursive = (enabled ? 2L : 1L);
			this._isDragging = false;
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x000D7E13 File Offset: 0x000D6013
		public float GetInitX
		{
			get
			{
				return this._whatsMoved.GlobalPosition.X;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002002 RID: 8194 RVA: 0x000D7E28 File Offset: 0x000D6028
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NSlotsContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlotsContainer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlotsContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlotsContainer.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NSlotsContainer.MethodName.ProcessPanEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NSlotsContainer.MethodName.ProcessGuiFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focusedControl", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSlotsContainer.MethodName.ProcessScrollEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NSlotsContainer.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSlotsContainer.MethodName.OnToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlotsContainer.MethodName.Reset, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlotsContainer.MethodName.SetEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "enabled", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002003 RID: 8195 RVA: 0x000D8150 File Offset: 0x000D6350
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSlotsContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName.ProcessPanEvent && args.Count == 1)
			{
				this.ProcessPanEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName.ProcessGuiFocus && args.Count == 1)
			{
				this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName.ProcessScrollEvent && args.Count == 1)
			{
				this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName.OnToggleVisibility && args.Count == 0)
			{
				this.OnToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName.Reset && args.Count == 0)
			{
				this.Reset();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlotsContainer.MethodName.SetEnabled && args.Count == 1)
			{
				this.SetEnabled(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002004 RID: 8196 RVA: 0x000D8354 File Offset: 0x000D6554
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSlotsContainer.MethodName._Ready || (ref method) == NSlotsContainer.MethodName._EnterTree || (ref method) == NSlotsContainer.MethodName._ExitTree || (ref method) == NSlotsContainer.MethodName._GuiInput || (ref method) == NSlotsContainer.MethodName.ProcessPanEvent || (ref method) == NSlotsContainer.MethodName.ProcessGuiFocus || (ref method) == NSlotsContainer.MethodName.ProcessScrollEvent || (ref method) == NSlotsContainer.MethodName._Process || (ref method) == NSlotsContainer.MethodName.OnToggleVisibility || (ref method) == NSlotsContainer.MethodName.Reset || (ref method) == NSlotsContainer.MethodName.SetEnabled || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002005 RID: 8197 RVA: 0x000D8410 File Offset: 0x000D6610
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSlotsContainer.PropertyName._whatsMoved)
			{
				this._whatsMoved = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._dragStartPosition)
			{
				this._dragStartPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._targetPosition)
			{
				this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._epochSlots)
			{
				this._epochSlots = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002006 RID: 8198 RVA: 0x000D84C8 File Offset: 0x000D66C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSlotsContainer.PropertyName.GetInitX)
			{
				float getInitX = this.GetInitX;
				value = VariantUtils.CreateFrom<float>(ref getInitX);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._whatsMoved)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._whatsMoved);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._dragStartPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._dragStartPosition);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._targetPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._isDragging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NSlotsContainer.PropertyName._epochSlots)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._epochSlots);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002007 RID: 8199 RVA: 0x000D85C0 File Offset: 0x000D67C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSlotsContainer.PropertyName._whatsMoved, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSlotsContainer.PropertyName._dragStartPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSlotsContainer.PropertyName._targetPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSlotsContainer.PropertyName._isDragging, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSlotsContainer.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSlotsContainer.PropertyName._epochSlots, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSlotsContainer.PropertyName.GetInitX, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002008 RID: 8200 RVA: 0x000D86B8 File Offset: 0x000D68B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSlotsContainer.PropertyName._whatsMoved, Variant.From<Control>(ref this._whatsMoved));
			info.AddProperty(NSlotsContainer.PropertyName._dragStartPosition, Variant.From<Vector2>(ref this._dragStartPosition));
			info.AddProperty(NSlotsContainer.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
			info.AddProperty(NSlotsContainer.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
			info.AddProperty(NSlotsContainer.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NSlotsContainer.PropertyName._epochSlots, Variant.From<Control>(ref this._epochSlots));
		}

		/// <inheritdoc />
		// Token: 0x06002009 RID: 8201 RVA: 0x000D8750 File Offset: 0x000D6950
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSlotsContainer.PropertyName._whatsMoved, ref variant))
			{
				this._whatsMoved = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSlotsContainer.PropertyName._dragStartPosition, ref variant2))
			{
				this._dragStartPosition = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSlotsContainer.PropertyName._targetPosition, ref variant3))
			{
				this._targetPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSlotsContainer.PropertyName._isDragging, ref variant4))
			{
				this._isDragging = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSlotsContainer.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSlotsContainer.PropertyName._epochSlots, ref variant6))
			{
				this._epochSlots = variant6.As<Control>();
			}
		}

		// Token: 0x04000C80 RID: 3200
		[Nullable(1)]
		private Control _whatsMoved;

		// Token: 0x04000C81 RID: 3201
		private Vector2 _dragStartPosition;

		// Token: 0x04000C82 RID: 3202
		private Vector2 _targetPosition;

		// Token: 0x04000C83 RID: 3203
		private bool _isDragging;

		// Token: 0x04000C84 RID: 3204
		private const float _scrollSpeed = 50f;

		// Token: 0x04000C85 RID: 3205
		private const float _trackpadScrollSpeed = 20f;

		// Token: 0x04000C86 RID: 3206
		private const float _bounceBackStrength = 36f;

		// Token: 0x04000C87 RID: 3207
		private const float _lerpSmoothness = 20f;

		// Token: 0x04000C88 RID: 3208
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000C89 RID: 3209
		[Nullable(1)]
		private Control _epochSlots;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012BA RID: 4794
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040048EA RID: 18666
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040048EB RID: 18667
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040048EC RID: 18668
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x040048ED RID: 18669
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'ProcessPanEvent' method.
			/// </summary>
			// Token: 0x040048EE RID: 18670
			public static readonly StringName ProcessPanEvent = "ProcessPanEvent";

			/// <summary>
			/// Cached name for the 'ProcessGuiFocus' method.
			/// </summary>
			// Token: 0x040048EF RID: 18671
			public static readonly StringName ProcessGuiFocus = "ProcessGuiFocus";

			/// <summary>
			/// Cached name for the 'ProcessScrollEvent' method.
			/// </summary>
			// Token: 0x040048F0 RID: 18672
			public static readonly StringName ProcessScrollEvent = "ProcessScrollEvent";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040048F1 RID: 18673
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'OnToggleVisibility' method.
			/// </summary>
			// Token: 0x040048F2 RID: 18674
			public static readonly StringName OnToggleVisibility = "OnToggleVisibility";

			/// <summary>
			/// Cached name for the 'Reset' method.
			/// </summary>
			// Token: 0x040048F3 RID: 18675
			public static readonly StringName Reset = "Reset";

			/// <summary>
			/// Cached name for the 'SetEnabled' method.
			/// </summary>
			// Token: 0x040048F4 RID: 18676
			public static readonly StringName SetEnabled = "SetEnabled";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012BB RID: 4795
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'GetInitX' property.
			/// </summary>
			// Token: 0x040048F5 RID: 18677
			public static readonly StringName GetInitX = "GetInitX";

			/// <summary>
			/// Cached name for the '_whatsMoved' field.
			/// </summary>
			// Token: 0x040048F6 RID: 18678
			public static readonly StringName _whatsMoved = "_whatsMoved";

			/// <summary>
			/// Cached name for the '_dragStartPosition' field.
			/// </summary>
			// Token: 0x040048F7 RID: 18679
			public static readonly StringName _dragStartPosition = "_dragStartPosition";

			/// <summary>
			/// Cached name for the '_targetPosition' field.
			/// </summary>
			// Token: 0x040048F8 RID: 18680
			public static readonly StringName _targetPosition = "_targetPosition";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x040048F9 RID: 18681
			public static readonly StringName _isDragging = "_isDragging";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040048FA RID: 18682
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_epochSlots' field.
			/// </summary>
			// Token: 0x040048FB RID: 18683
			public static readonly StringName _epochSlots = "_epochSlots";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012BC RID: 4796
		public class SignalName : Control.SignalName
		{
		}
	}
}
