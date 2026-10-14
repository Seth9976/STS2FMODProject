using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	/// <summary>
	/// Manages scrolling of a container.
	/// This should be put on an object that doesn't scroll, and then the content to be scrolled should be a child named
	/// Content or Mask/Content.
	/// </summary>
	// Token: 0x020003A2 RID: 930
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NScrollableContainer.cs")]
	public class NScrollableContainer : Control
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060037F2 RID: 14322 RVA: 0x001A661C File Offset: 0x001A481C
		private float ScrollViewportTop
		{
			get
			{
				if (this._content != null)
				{
					return this._content.GetParent<Control>().Position.Y;
				}
				return 0f;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060037F3 RID: 14323 RVA: 0x001A6641 File Offset: 0x001A4841
		private float ScrollViewportSize
		{
			get
			{
				if (this._content != null)
				{
					return this._content.GetParent<Control>().Size.Y;
				}
				return 0f;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060037F4 RID: 14324 RVA: 0x001A6668 File Offset: 0x001A4868
		private float ScrollLimitBottom
		{
			get
			{
				if (this._content != null)
				{
					return -(this._paddingBottom + this._paddingTop + this._content.Size.Y) + this._content.GetParent<Control>().Size.Y;
				}
				return 0f;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x001A66B8 File Offset: 0x001A48B8
		// (set) Token: 0x060037F6 RID: 14326 RVA: 0x001A66C0 File Offset: 0x001A48C0
		public NScrollbar Scrollbar { get; private set; }

		// Token: 0x060037F7 RID: 14327 RVA: 0x001A66CC File Offset: 0x001A48CC
		public override void _Ready()
		{
			this._content = base.GetNodeOrNull<Control>("Content") ?? base.GetNodeOrNull<Control>("Mask/Content");
			this.Scrollbar = base.GetNode<NScrollbar>("Scrollbar");
			this.SetContent(this._content, 0f, 0f);
			this.Scrollbar.Visible = false;
			this.Scrollbar.Connect(NScrollbar.SignalName.MousePressed, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this._scrollbarPressed = true;
			}), 0U);
			this.Scrollbar.Connect(NScrollbar.SignalName.MouseReleased, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this._scrollbarPressed = false;
			}), 0U);
		}

		/// <summary>
		/// If the content to be scrolled is dynamic (e.g. a tab in the settings menu, then you can change it using this method.
		/// </summary>
		// Token: 0x060037F8 RID: 14328 RVA: 0x001A6784 File Offset: 0x001A4984
		[NullableContext(2)]
		public void SetContent(Control content, float paddingTop = 0f, float paddingBottom = 0f)
		{
			Callable callable = Callable.From(new Action(this.UpdateScrollLimitBottom));
			if (this._content != null && this._content.IsConnected(CanvasItem.SignalName.ItemRectChanged, callable))
			{
				this._content.Disconnect(CanvasItem.SignalName.ItemRectChanged, callable);
			}
			this._content = content;
			if (this._content != null)
			{
				this._content.Connect(CanvasItem.SignalName.ItemRectChanged, Callable.From(new Action(this.UpdateScrollLimitBottom)), 0U);
				this.UpdatePadding(paddingTop, paddingBottom);
			}
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x001A6809 File Offset: 0x001A4A09
		public void UpdatePadding(float paddingTop = 0f, float paddingBottom = 0f)
		{
			this._paddingTop = paddingTop;
			this._paddingBottom = paddingBottom;
			this.UpdateScrollLimitBottom();
		}

		/// <summary>
		/// If this is called, then scrolling is disabled when the content fits entirely within the scroll container.
		/// </summary>
		// Token: 0x060037FA RID: 14330 RVA: 0x001A681F File Offset: 0x001A4A1F
		public void DisableScrollingIfContentFits()
		{
			this._disableScrollingIfContentFits = true;
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x001A6828 File Offset: 0x001A4A28
		public override void _EnterTree()
		{
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x001A684D File Offset: 0x001A4A4D
		public override void _ExitTree()
		{
			base.GetViewport().Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x001A6870 File Offset: 0x001A4A70
		private void UpdateScrollLimitBottom()
		{
			if (this._content == null)
			{
				return;
			}
			this.Scrollbar.Visible = this._content.Size.Y + this._paddingTop + this._paddingBottom > this.ScrollViewportSize;
			this.Scrollbar.MouseFilter = (this.Scrollbar.Visible ? 0L : 2L);
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x001A68D5 File Offset: 0x001A4AD5
		public override void _GuiInput(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			this.ProcessMouseEvent(inputEvent);
			this.ProcessScrollEvent(inputEvent);
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x001A68F0 File Offset: 0x001A4AF0
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			Viewport viewport = base.GetViewport();
			if (viewport != null && viewport.GuiGetFocusOwner() != null)
			{
				return;
			}
			this.ProcessControllerEvent(inputEvent);
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x001A6920 File Offset: 0x001A4B20
		private void ProcessControllerEvent(InputEvent inputEvent)
		{
			if (inputEvent.IsActionPressed(MegaInput.up, false, false))
			{
				this._targetDragPosY += this._controllerScrollAmount;
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.down, false, false))
			{
				this._targetDragPosY += -this._controllerScrollAmount;
			}
		}

		/// <summary>
		/// Detects mouse click up/down and updates our scroll target accordingly.
		/// </summary>
		// Token: 0x06003801 RID: 14337 RVA: 0x001A6974 File Offset: 0x001A4B74
		private void ProcessMouseEvent(InputEvent inputEvent)
		{
			if (this._content == null)
			{
				return;
			}
			InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
			if (inputEventMouseMotion == null)
			{
				InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
				if (inputEventMouseButton == null)
				{
					return;
				}
				if (inputEventMouseButton.ButtonIndex == 1L)
				{
					this._isDragging = inputEventMouseButton.Pressed;
					if (inputEventMouseButton.Pressed)
					{
						this._startDragPosY = this._content.Position.Y - this._paddingTop;
						this._targetDragPosY = this._startDragPosY;
						return;
					}
				}
				else if (!inputEventMouseButton.Pressed)
				{
					this._isDragging = false;
				}
			}
			else if (this._isDragging)
			{
				this._targetDragPosY += inputEventMouseMotion.Relative.Y;
				return;
			}
		}

		/// <summary>
		/// Detects mouse wheel up/down and updates our scroll target accordingly
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003802 RID: 14338 RVA: 0x001A6A19 File Offset: 0x001A4C19
		private void ProcessScrollEvent(InputEvent inputEvent)
		{
			this._targetDragPosY += ScrollHelper.GetDragForScrollEvent(inputEvent);
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x001A6A2E File Offset: 0x001A4C2E
		public override void _Process(double delta)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (this._disableScrollingIfContentFits && !this.Scrollbar.Visible)
			{
				return;
			}
			this.UpdateScrollPosition(delta);
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x001A6A58 File Offset: 0x001A4C58
		public void InstantlyScrollToTop()
		{
			if (this._content == null)
			{
				throw new InvalidOperationException("No content to scroll!");
			}
			this._targetDragPosY = 0f;
			Control content = this._content;
			Vector2 position = this._content.Position;
			position.Y = this._paddingTop;
			content.Position = position;
			this.Scrollbar.SetValueWithoutAnimation(0.0);
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x001A6ABC File Offset: 0x001A4CBC
		private void ProcessGuiFocus(Control focusedControl)
		{
			if (this._content == null)
			{
				return;
			}
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			if (focusedControl is NDropdownItem)
			{
				return;
			}
			if (this._content.IsAncestorOf(focusedControl))
			{
				float num = this._content.GlobalPosition.Y - focusedControl.GlobalPosition.Y;
				float num2 = num + this.ScrollViewportSize * 0.5f;
				float num3 = Mathf.Max(this.ScrollLimitBottom, 0f);
				float num4 = Mathf.Min(this.ScrollLimitBottom, 0f);
				num2 = Mathf.Clamp(num2, num4, num3);
				this._targetDragPosY = num2;
			}
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x001A6B60 File Offset: 0x001A4D60
		private void UpdateScrollPosition(double delta)
		{
			if (this._content == null)
			{
				return;
			}
			float num = this._paddingTop + this._targetDragPosY;
			if (!Mathf.IsEqualApprox(this._content.Position.Y, num))
			{
				float num2 = (float)Mathf.Sign(this._content.Position.Y - num);
				float num3 = Mathf.Lerp(this._content.Position.Y, num, (float)delta * 15f);
				float num4 = (float)Mathf.Sign(num3 - num);
				if (Math.Abs(num3 - num) < 0.5f || !Mathf.IsEqualApprox(num2, num4))
				{
					num3 = num;
				}
				Control content = this._content;
				Vector2 position = this._content.Position;
				position.Y = num3;
				content.Position = position;
				if (!this._scrollbarPressed && this.ScrollLimitBottom < 0f)
				{
					this.Scrollbar.SetValueWithoutAnimation((double)(Mathf.Clamp((this._content.Position.Y - this._paddingTop) / this.ScrollLimitBottom, 0f, 1f) * 100f));
				}
			}
			if (this._scrollbarPressed)
			{
				this._targetDragPosY = Mathf.Lerp(0f, this.ScrollLimitBottom, (float)this.Scrollbar.Value * 0.01f);
			}
			if (!this._isDragging)
			{
				if (this._targetDragPosY < Mathf.Min(this.ScrollLimitBottom, 0f))
				{
					this._targetDragPosY = Mathf.Lerp(this._targetDragPosY, this.ScrollLimitBottom, (float)delta * 12f);
					return;
				}
				if (this._targetDragPosY > Mathf.Max(this.ScrollLimitBottom, 0f))
				{
					this._targetDragPosY = Mathf.Lerp(this._targetDragPosY, 0f, (float)delta * 12f);
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003807 RID: 14343 RVA: 0x001A6D18 File Offset: 0x001A4F18
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(16)
			{
				new MethodInfo(NScrollableContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollableContainer.MethodName.SetContent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "content", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(3L, "paddingTop", 0L, "", 6L, false),
					new PropertyInfo(3L, "paddingBottom", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName.UpdatePadding, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "paddingTop", 0L, "", 6L, false),
					new PropertyInfo(3L, "paddingBottom", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName.DisableScrollingIfContentFits, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollableContainer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollableContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollableContainer.MethodName.UpdateScrollLimitBottom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollableContainer.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName.ProcessControllerEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName.ProcessMouseEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName.ProcessScrollEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName.InstantlyScrollToTop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollableContainer.MethodName.ProcessGuiFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focusedControl", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NScrollableContainer.MethodName.UpdateScrollPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003808 RID: 14344 RVA: 0x001A7240 File Offset: 0x001A5440
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScrollableContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.SetContent && args.Count == 3)
			{
				this.SetContent(VariantUtils.ConvertTo<Control>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<float>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.UpdatePadding && args.Count == 2)
			{
				this.UpdatePadding(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.DisableScrollingIfContentFits && args.Count == 0)
			{
				this.DisableScrollingIfContentFits();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.UpdateScrollLimitBottom && args.Count == 0)
			{
				this.UpdateScrollLimitBottom();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.ProcessControllerEvent && args.Count == 1)
			{
				this.ProcessControllerEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.ProcessMouseEvent && args.Count == 1)
			{
				this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.ProcessScrollEvent && args.Count == 1)
			{
				this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.InstantlyScrollToTop && args.Count == 0)
			{
				this.InstantlyScrollToTop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.ProcessGuiFocus && args.Count == 1)
			{
				this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollableContainer.MethodName.UpdateScrollPosition && args.Count == 1)
			{
				this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003809 RID: 14345 RVA: 0x001A755C File Offset: 0x001A575C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScrollableContainer.MethodName._Ready || (ref method) == NScrollableContainer.MethodName.SetContent || (ref method) == NScrollableContainer.MethodName.UpdatePadding || (ref method) == NScrollableContainer.MethodName.DisableScrollingIfContentFits || (ref method) == NScrollableContainer.MethodName._EnterTree || (ref method) == NScrollableContainer.MethodName._ExitTree || (ref method) == NScrollableContainer.MethodName.UpdateScrollLimitBottom || (ref method) == NScrollableContainer.MethodName._GuiInput || (ref method) == NScrollableContainer.MethodName._Input || (ref method) == NScrollableContainer.MethodName.ProcessControllerEvent || (ref method) == NScrollableContainer.MethodName.ProcessMouseEvent || (ref method) == NScrollableContainer.MethodName.ProcessScrollEvent || (ref method) == NScrollableContainer.MethodName._Process || (ref method) == NScrollableContainer.MethodName.InstantlyScrollToTop || (ref method) == NScrollableContainer.MethodName.ProcessGuiFocus || (ref method) == NScrollableContainer.MethodName.UpdateScrollPosition || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600380A RID: 14346 RVA: 0x001A7660 File Offset: 0x001A5860
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NScrollableContainer.PropertyName.Scrollbar)
			{
				this.Scrollbar = VariantUtils.ConvertTo<NScrollbar>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._controllerScrollAmount)
			{
				this._controllerScrollAmount = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._startDragPosY)
			{
				this._startDragPosY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._targetDragPosY)
			{
				this._targetDragPosY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._paddingTop)
			{
				this._paddingTop = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._paddingBottom)
			{
				this._paddingBottom = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._content)
			{
				this._content = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._scrollbarPressed)
			{
				this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._disableScrollingIfContentFits)
			{
				this._disableScrollingIfContentFits = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600380B RID: 14347 RVA: 0x001A7784 File Offset: 0x001A5984
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NScrollableContainer.PropertyName.ScrollViewportTop)
			{
				float num = this.ScrollViewportTop;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName.ScrollViewportSize)
			{
				float num = this.ScrollViewportSize;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName.ScrollLimitBottom)
			{
				float num = this.ScrollLimitBottom;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName.Scrollbar)
			{
				NScrollbar scrollbar = this.Scrollbar;
				value = VariantUtils.CreateFrom<NScrollbar>(ref scrollbar);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._controllerScrollAmount)
			{
				value = VariantUtils.CreateFrom<float>(ref this._controllerScrollAmount);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._startDragPosY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._startDragPosY);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._targetDragPosY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._targetDragPosY);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._isDragging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._paddingTop)
			{
				value = VariantUtils.CreateFrom<float>(ref this._paddingTop);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._paddingBottom)
			{
				value = VariantUtils.CreateFrom<float>(ref this._paddingBottom);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._content)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._content);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._scrollbarPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
				return true;
			}
			if ((ref name) == NScrollableContainer.PropertyName._disableScrollingIfContentFits)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._disableScrollingIfContentFits);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600380C RID: 14348 RVA: 0x001A7948 File Offset: 0x001A5B48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NScrollableContainer.PropertyName._controllerScrollAmount, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollableContainer.PropertyName._startDragPosY, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollableContainer.PropertyName._targetDragPosY, 0L, "", 4096L, false),
				new PropertyInfo(1L, NScrollableContainer.PropertyName._isDragging, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollableContainer.PropertyName.ScrollViewportTop, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollableContainer.PropertyName.ScrollViewportSize, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollableContainer.PropertyName.ScrollLimitBottom, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollableContainer.PropertyName._paddingTop, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollableContainer.PropertyName._paddingBottom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NScrollableContainer.PropertyName._content, 0L, "", 4096L, false),
				new PropertyInfo(1L, NScrollableContainer.PropertyName._scrollbarPressed, 0L, "", 4096L, false),
				new PropertyInfo(1L, NScrollableContainer.PropertyName._disableScrollingIfContentFits, 0L, "", 4096L, false),
				new PropertyInfo(24L, NScrollableContainer.PropertyName.Scrollbar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600380D RID: 14349 RVA: 0x001A7B00 File Offset: 0x001A5D00
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName scrollbar = NScrollableContainer.PropertyName.Scrollbar;
			NScrollbar scrollbar2 = this.Scrollbar;
			info.AddProperty(scrollbar, Variant.From<NScrollbar>(ref scrollbar2));
			info.AddProperty(NScrollableContainer.PropertyName._controllerScrollAmount, Variant.From<float>(ref this._controllerScrollAmount));
			info.AddProperty(NScrollableContainer.PropertyName._startDragPosY, Variant.From<float>(ref this._startDragPosY));
			info.AddProperty(NScrollableContainer.PropertyName._targetDragPosY, Variant.From<float>(ref this._targetDragPosY));
			info.AddProperty(NScrollableContainer.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
			info.AddProperty(NScrollableContainer.PropertyName._paddingTop, Variant.From<float>(ref this._paddingTop));
			info.AddProperty(NScrollableContainer.PropertyName._paddingBottom, Variant.From<float>(ref this._paddingBottom));
			info.AddProperty(NScrollableContainer.PropertyName._content, Variant.From<Control>(ref this._content));
			info.AddProperty(NScrollableContainer.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
			info.AddProperty(NScrollableContainer.PropertyName._disableScrollingIfContentFits, Variant.From<bool>(ref this._disableScrollingIfContentFits));
		}

		/// <inheritdoc />
		// Token: 0x0600380E RID: 14350 RVA: 0x001A7BF4 File Offset: 0x001A5DF4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NScrollableContainer.PropertyName.Scrollbar, ref variant))
			{
				this.Scrollbar = variant.As<NScrollbar>();
			}
			Variant variant2;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._controllerScrollAmount, ref variant2))
			{
				this._controllerScrollAmount = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._startDragPosY, ref variant3))
			{
				this._startDragPosY = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._targetDragPosY, ref variant4))
			{
				this._targetDragPosY = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._isDragging, ref variant5))
			{
				this._isDragging = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._paddingTop, ref variant6))
			{
				this._paddingTop = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._paddingBottom, ref variant7))
			{
				this._paddingBottom = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._content, ref variant8))
			{
				this._content = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._scrollbarPressed, ref variant9))
			{
				this._scrollbarPressed = variant9.As<bool>();
			}
			Variant variant10;
			if (info.TryGetProperty(NScrollableContainer.PropertyName._disableScrollingIfContentFits, ref variant10))
			{
				this._disableScrollingIfContentFits = variant10.As<bool>();
			}
		}

		// Token: 0x040015B8 RID: 5560
		private float _controllerScrollAmount = 400f;

		// Token: 0x040015B9 RID: 5561
		private float _startDragPosY;

		// Token: 0x040015BA RID: 5562
		private float _targetDragPosY;

		// Token: 0x040015BB RID: 5563
		private bool _isDragging;

		// Token: 0x040015BC RID: 5564
		private float _paddingTop;

		// Token: 0x040015BD RID: 5565
		private float _paddingBottom;

		// Token: 0x040015BE RID: 5566
		[Nullable(2)]
		private Control _content;

		// Token: 0x040015BF RID: 5567
		private bool _scrollbarPressed;

		// Token: 0x040015C0 RID: 5568
		private bool _disableScrollingIfContentFits;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200177C RID: 6012
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BA8 RID: 23464
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetContent' method.
			/// </summary>
			// Token: 0x04005BA9 RID: 23465
			public static readonly StringName SetContent = "SetContent";

			/// <summary>
			/// Cached name for the 'UpdatePadding' method.
			/// </summary>
			// Token: 0x04005BAA RID: 23466
			public static readonly StringName UpdatePadding = "UpdatePadding";

			/// <summary>
			/// Cached name for the 'DisableScrollingIfContentFits' method.
			/// </summary>
			// Token: 0x04005BAB RID: 23467
			public static readonly StringName DisableScrollingIfContentFits = "DisableScrollingIfContentFits";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005BAC RID: 23468
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005BAD RID: 23469
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateScrollLimitBottom' method.
			/// </summary>
			// Token: 0x04005BAE RID: 23470
			public static readonly StringName UpdateScrollLimitBottom = "UpdateScrollLimitBottom";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005BAF RID: 23471
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005BB0 RID: 23472
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ProcessControllerEvent' method.
			/// </summary>
			// Token: 0x04005BB1 RID: 23473
			public static readonly StringName ProcessControllerEvent = "ProcessControllerEvent";

			/// <summary>
			/// Cached name for the 'ProcessMouseEvent' method.
			/// </summary>
			// Token: 0x04005BB2 RID: 23474
			public static readonly StringName ProcessMouseEvent = "ProcessMouseEvent";

			/// <summary>
			/// Cached name for the 'ProcessScrollEvent' method.
			/// </summary>
			// Token: 0x04005BB3 RID: 23475
			public static readonly StringName ProcessScrollEvent = "ProcessScrollEvent";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005BB4 RID: 23476
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'InstantlyScrollToTop' method.
			/// </summary>
			// Token: 0x04005BB5 RID: 23477
			public static readonly StringName InstantlyScrollToTop = "InstantlyScrollToTop";

			/// <summary>
			/// Cached name for the 'ProcessGuiFocus' method.
			/// </summary>
			// Token: 0x04005BB6 RID: 23478
			public static readonly StringName ProcessGuiFocus = "ProcessGuiFocus";

			/// <summary>
			/// Cached name for the 'UpdateScrollPosition' method.
			/// </summary>
			// Token: 0x04005BB7 RID: 23479
			public static readonly StringName UpdateScrollPosition = "UpdateScrollPosition";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200177D RID: 6013
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScrollViewportTop' property.
			/// </summary>
			// Token: 0x04005BB8 RID: 23480
			public static readonly StringName ScrollViewportTop = "ScrollViewportTop";

			/// <summary>
			/// Cached name for the 'ScrollViewportSize' property.
			/// </summary>
			// Token: 0x04005BB9 RID: 23481
			public static readonly StringName ScrollViewportSize = "ScrollViewportSize";

			/// <summary>
			/// Cached name for the 'ScrollLimitBottom' property.
			/// </summary>
			// Token: 0x04005BBA RID: 23482
			public static readonly StringName ScrollLimitBottom = "ScrollLimitBottom";

			/// <summary>
			/// Cached name for the 'Scrollbar' property.
			/// </summary>
			// Token: 0x04005BBB RID: 23483
			public static readonly StringName Scrollbar = "Scrollbar";

			/// <summary>
			/// Cached name for the '_controllerScrollAmount' field.
			/// </summary>
			// Token: 0x04005BBC RID: 23484
			public static readonly StringName _controllerScrollAmount = "_controllerScrollAmount";

			/// <summary>
			/// Cached name for the '_startDragPosY' field.
			/// </summary>
			// Token: 0x04005BBD RID: 23485
			public static readonly StringName _startDragPosY = "_startDragPosY";

			/// <summary>
			/// Cached name for the '_targetDragPosY' field.
			/// </summary>
			// Token: 0x04005BBE RID: 23486
			public static readonly StringName _targetDragPosY = "_targetDragPosY";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x04005BBF RID: 23487
			public static readonly StringName _isDragging = "_isDragging";

			/// <summary>
			/// Cached name for the '_paddingTop' field.
			/// </summary>
			// Token: 0x04005BC0 RID: 23488
			public static readonly StringName _paddingTop = "_paddingTop";

			/// <summary>
			/// Cached name for the '_paddingBottom' field.
			/// </summary>
			// Token: 0x04005BC1 RID: 23489
			public static readonly StringName _paddingBottom = "_paddingBottom";

			/// <summary>
			/// Cached name for the '_content' field.
			/// </summary>
			// Token: 0x04005BC2 RID: 23490
			public static readonly StringName _content = "_content";

			/// <summary>
			/// Cached name for the '_scrollbarPressed' field.
			/// </summary>
			// Token: 0x04005BC3 RID: 23491
			public static readonly StringName _scrollbarPressed = "_scrollbarPressed";

			/// <summary>
			/// Cached name for the '_disableScrollingIfContentFits' field.
			/// </summary>
			// Token: 0x04005BC4 RID: 23492
			public static readonly StringName _disableScrollingIfContentFits = "_disableScrollingIfContentFits";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200177E RID: 6014
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
