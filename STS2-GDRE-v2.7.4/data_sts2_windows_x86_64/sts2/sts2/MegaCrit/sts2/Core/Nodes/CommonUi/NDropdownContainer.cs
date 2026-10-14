using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Special component used for creating dropdown containers dynamically. Supports scrolling
	/// </summary>
	// Token: 0x020003DA RID: 986
	[ScriptPath("res://src/Core/Nodes/CommonUi/NDropdownContainer.cs")]
	public class NDropdownContainer : Control
	{
		// Token: 0x06003BD4 RID: 15316 RVA: 0x001C9BF0 File Offset: 0x001C7DF0
		public override void _Ready()
		{
			this._scrollbar = base.GetNode<NDropdownScrollbar>("Scrollbar");
			this._scrollbarTrain = base.GetNode<Control>("Scrollbar/Train");
			this._dropdownItems = base.GetNode<VBoxContainer>("VBoxContainer");
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
			this._maxHeight = base.Size.Y;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x001C9C6E File Offset: 0x001C7E6E
		public override void _EnterTree()
		{
			base._EnterTree();
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x001C9C99 File Offset: 0x001C7E99
		public override void _ExitTree()
		{
			base._ExitTree();
			base.GetViewport().Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x001C9CC2 File Offset: 0x001C7EC2
		private void OnVisibilityChange()
		{
			if (base.Visible)
			{
				this._isDragging = false;
			}
		}

		/// <summary>
		/// This is where called whenever this container's layout is modified.
		/// </summary>
		// Token: 0x06003BD8 RID: 15320 RVA: 0x001C9CD3 File Offset: 0x001C7ED3
		public void RefreshLayout()
		{
			this._scrollbar.Visible = this.IsScrollbarNeeded();
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x001C9CE8 File Offset: 0x001C7EE8
		private bool IsScrollbarNeeded()
		{
			this._contentHeight = 0f;
			foreach (Node node in this._dropdownItems.GetChildren(false))
			{
				Control control = node as Control;
				if (control != null)
				{
					this._contentHeight += control.Size.Y;
				}
			}
			this._scrollLimitBottom = -this._contentHeight + this._maxHeight;
			if (this._contentHeight > this._maxHeight)
			{
				base.Size = new Vector2(base.Size.X, this._maxHeight);
				this._scrollbar.RefreshTrainBounds();
				return true;
			}
			base.Size = new Vector2(base.Size.X, this._contentHeight);
			return false;
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x001C9DCC File Offset: 0x001C7FCC
		public override void _Process(double delta)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			this.UpdateScrollPosition(delta);
			this.UpdateScrollbar();
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x001C9DE4 File Offset: 0x001C7FE4
		[NullableContext(1)]
		private void ProcessGuiFocus(Control focusedControl)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!this._scrollbar.Visible)
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			if (this._dropdownItems.IsAncestorOf(focusedControl))
			{
				float num = this._dropdownItems.GlobalPosition.Y - focusedControl.GlobalPosition.Y;
				float num2 = num + base.Size.Y * 0.5f;
				num2 = Mathf.Clamp(num2, this._scrollLimitBottom, 0f);
				this._targetDragPos = new Vector2(this._targetDragPos.X, num2);
			}
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x001C9E80 File Offset: 0x001C8080
		private void UpdateScrollPosition(double delta)
		{
			if (!this._scrollbar.Visible)
			{
				return;
			}
			if (this._dropdownItems.Position != this._targetDragPos)
			{
				float num = (float)Mathf.Sign(this._dropdownItems.Position.Y - this._targetDragPos.Y);
				this._dropdownItems.Position = this._dropdownItems.Position.Lerp(this._targetDragPos, (float)delta * 15f);
				float num2 = (float)Mathf.Sign(this._dropdownItems.Position.Y - this._targetDragPos.Y);
				if (Math.Abs(this._dropdownItems.Position.Y - this._targetDragPos.Y) < 0.5f || !Mathf.IsEqualApprox(num, num2))
				{
					this._dropdownItems.Position = this._targetDragPos;
				}
			}
			if (!this._isDragging)
			{
				if (this._targetDragPos.Y < this._scrollLimitBottom)
				{
					this._targetDragPos = this._targetDragPos.Lerp(new Vector2(0f, this._scrollLimitBottom), (float)delta * 12f);
					return;
				}
				if (this._targetDragPos.Y > 0f)
				{
					this._targetDragPos = this._targetDragPos.Lerp(new Vector2(0f, 0f), (float)delta * 12f);
				}
			}
		}

		/// <summary>
		/// Gets the current content's position and remap + clamps it to position the train so they're in sync.
		/// </summary>
		// Token: 0x06003BDD RID: 15325 RVA: 0x001C9FEC File Offset: 0x001C81EC
		private void UpdateScrollbar()
		{
			if (!this._scrollbar.Visible || this._scrollbar.hasControl)
			{
				return;
			}
			float num = (this._dropdownItems.Position.Y - this._scrollLimitBottom) / (0f - this._scrollLimitBottom);
			this._scrollbar.SetTrainPositionFromPercentage(Mathf.Clamp(1f - num, 0f, 1f));
		}

		/// <summary>
		/// Given the train's progress from 0 - 1f, sets the targetDragPosition to match
		/// so the positions are in sync.
		/// </summary>
		// Token: 0x06003BDE RID: 15326 RVA: 0x001CA05A File Offset: 0x001C825A
		public void UpdatePositionBasedOnTrain(float trainPosition)
		{
			this._targetDragPos = new Vector2(this._targetDragPos.X, this._scrollLimitBottom + trainPosition * (0f - this._scrollLimitBottom));
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x001CA087 File Offset: 0x001C8287
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			this.ProcessMouseEvent(inputEvent);
			this.ProcessScrollEvent(inputEvent);
		}

		/// <summary>
		/// Detects mouse click up/down and updates our scroll target accordingly
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003BE0 RID: 15328 RVA: 0x001CA098 File Offset: 0x001C8298
		[NullableContext(1)]
		private void ProcessMouseEvent(InputEvent inputEvent)
		{
			if (this._isDragging)
			{
				InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
				if (inputEventMouseMotion != null)
				{
					this._targetDragPos += new Vector2(0f, inputEventMouseMotion.Relative.Y);
					return;
				}
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				if (inputEventMouseButton.ButtonIndex == 1L)
				{
					if (inputEventMouseButton.Pressed)
					{
						this._isDragging = true;
						this._startDragPos = this._dropdownItems.Position;
						this._targetDragPos = this._startDragPos;
						return;
					}
					this._isDragging = false;
					return;
				}
				else if (!inputEventMouseButton.Pressed)
				{
					this._isDragging = false;
				}
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x001CA136 File Offset: 0x001C8336
		[NullableContext(1)]
		private void ProcessScrollEvent(InputEvent inputEvent)
		{
			this._targetDragPos += new Vector2(0f, ScrollHelper.GetDragForScrollEvent(inputEvent));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003BE2 RID: 15330 RVA: 0x001CA15C File Offset: 0x001C835C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NDropdownContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownContainer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownContainer.MethodName.OnVisibilityChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownContainer.MethodName.RefreshLayout, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownContainer.MethodName.IsScrollbarNeeded, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownContainer.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDropdownContainer.MethodName.ProcessGuiFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focusedControl", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDropdownContainer.MethodName.UpdateScrollPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDropdownContainer.MethodName.UpdateScrollbar, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownContainer.MethodName.UpdatePositionBasedOnTrain, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "trainPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDropdownContainer.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NDropdownContainer.MethodName.ProcessMouseEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NDropdownContainer.MethodName.ProcessScrollEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BE3 RID: 15331 RVA: 0x001CA534 File Offset: 0x001C8734
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDropdownContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.OnVisibilityChange && args.Count == 0)
			{
				this.OnVisibilityChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.RefreshLayout && args.Count == 0)
			{
				this.RefreshLayout();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.IsScrollbarNeeded && args.Count == 0)
			{
				bool flag = this.IsScrollbarNeeded();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.ProcessGuiFocus && args.Count == 1)
			{
				this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.UpdateScrollPosition && args.Count == 1)
			{
				this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.UpdateScrollbar && args.Count == 0)
			{
				this.UpdateScrollbar();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.UpdatePositionBasedOnTrain && args.Count == 1)
			{
				this.UpdatePositionBasedOnTrain(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.ProcessMouseEvent && args.Count == 1)
			{
				this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownContainer.MethodName.ProcessScrollEvent && args.Count == 1)
			{
				this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003BE4 RID: 15332 RVA: 0x001CA7BC File Offset: 0x001C89BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDropdownContainer.MethodName._Ready || (ref method) == NDropdownContainer.MethodName._EnterTree || (ref method) == NDropdownContainer.MethodName._ExitTree || (ref method) == NDropdownContainer.MethodName.OnVisibilityChange || (ref method) == NDropdownContainer.MethodName.RefreshLayout || (ref method) == NDropdownContainer.MethodName.IsScrollbarNeeded || (ref method) == NDropdownContainer.MethodName._Process || (ref method) == NDropdownContainer.MethodName.ProcessGuiFocus || (ref method) == NDropdownContainer.MethodName.UpdateScrollPosition || (ref method) == NDropdownContainer.MethodName.UpdateScrollbar || (ref method) == NDropdownContainer.MethodName.UpdatePositionBasedOnTrain || (ref method) == NDropdownContainer.MethodName._GuiInput || (ref method) == NDropdownContainer.MethodName.ProcessMouseEvent || (ref method) == NDropdownContainer.MethodName.ProcessScrollEvent || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003BE5 RID: 15333 RVA: 0x001CA8A4 File Offset: 0x001C8AA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDropdownContainer.PropertyName._scrollbar)
			{
				this._scrollbar = VariantUtils.ConvertTo<NDropdownScrollbar>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._scrollbarTrain)
			{
				this._scrollbarTrain = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._dropdownItems)
			{
				this._dropdownItems = VariantUtils.ConvertTo<VBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._maxHeight)
			{
				this._maxHeight = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._contentHeight)
			{
				this._contentHeight = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._startDragPos)
			{
				this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._targetDragPos)
			{
				this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._scrollLimitBottom)
			{
				this._scrollLimitBottom = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003BE6 RID: 15334 RVA: 0x001CA9AC File Offset: 0x001C8BAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDropdownContainer.PropertyName._scrollbar)
			{
				value = VariantUtils.CreateFrom<NDropdownScrollbar>(ref this._scrollbar);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._scrollbarTrain)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._scrollbarTrain);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._dropdownItems)
			{
				value = VariantUtils.CreateFrom<VBoxContainer>(ref this._dropdownItems);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._maxHeight)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxHeight);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._contentHeight)
			{
				value = VariantUtils.CreateFrom<float>(ref this._contentHeight);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._startDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._targetDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._scrollLimitBottom)
			{
				value = VariantUtils.CreateFrom<float>(ref this._scrollLimitBottom);
				return true;
			}
			if ((ref name) == NDropdownContainer.PropertyName._isDragging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003BE7 RID: 15335 RVA: 0x001CAAE4 File Offset: 0x001C8CE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDropdownContainer.PropertyName._scrollbar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdownContainer.PropertyName._scrollbarTrain, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdownContainer.PropertyName._dropdownItems, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDropdownContainer.PropertyName._maxHeight, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDropdownContainer.PropertyName._contentHeight, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDropdownContainer.PropertyName._startDragPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDropdownContainer.PropertyName._targetDragPos, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDropdownContainer.PropertyName._scrollLimitBottom, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDropdownContainer.PropertyName._isDragging, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BE8 RID: 15336 RVA: 0x001CAC1C File Offset: 0x001C8E1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDropdownContainer.PropertyName._scrollbar, Variant.From<NDropdownScrollbar>(ref this._scrollbar));
			info.AddProperty(NDropdownContainer.PropertyName._scrollbarTrain, Variant.From<Control>(ref this._scrollbarTrain));
			info.AddProperty(NDropdownContainer.PropertyName._dropdownItems, Variant.From<VBoxContainer>(ref this._dropdownItems));
			info.AddProperty(NDropdownContainer.PropertyName._maxHeight, Variant.From<float>(ref this._maxHeight));
			info.AddProperty(NDropdownContainer.PropertyName._contentHeight, Variant.From<float>(ref this._contentHeight));
			info.AddProperty(NDropdownContainer.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
			info.AddProperty(NDropdownContainer.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
			info.AddProperty(NDropdownContainer.PropertyName._scrollLimitBottom, Variant.From<float>(ref this._scrollLimitBottom));
			info.AddProperty(NDropdownContainer.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
		}

		/// <inheritdoc />
		// Token: 0x06003BE9 RID: 15337 RVA: 0x001CACF8 File Offset: 0x001C8EF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._scrollbar, ref variant))
			{
				this._scrollbar = variant.As<NDropdownScrollbar>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._scrollbarTrain, ref variant2))
			{
				this._scrollbarTrain = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._dropdownItems, ref variant3))
			{
				this._dropdownItems = variant3.As<VBoxContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._maxHeight, ref variant4))
			{
				this._maxHeight = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._contentHeight, ref variant5))
			{
				this._contentHeight = variant5.As<float>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._startDragPos, ref variant6))
			{
				this._startDragPos = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._targetDragPos, ref variant7))
			{
				this._targetDragPos = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._scrollLimitBottom, ref variant8))
			{
				this._scrollLimitBottom = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDropdownContainer.PropertyName._isDragging, ref variant9))
			{
				this._isDragging = variant9.As<bool>();
			}
		}

		// Token: 0x04001765 RID: 5989
		[Nullable(1)]
		private NDropdownScrollbar _scrollbar;

		// Token: 0x04001766 RID: 5990
		[Nullable(1)]
		private Control _scrollbarTrain;

		// Token: 0x04001767 RID: 5991
		[Nullable(1)]
		private VBoxContainer _dropdownItems;

		// Token: 0x04001768 RID: 5992
		private float _maxHeight;

		// Token: 0x04001769 RID: 5993
		private float _contentHeight;

		// Token: 0x0400176A RID: 5994
		private Vector2 _startDragPos;

		// Token: 0x0400176B RID: 5995
		private Vector2 _targetDragPos = Vector2.Zero;

		// Token: 0x0400176C RID: 5996
		private const float _scrollLimitTop = 0f;

		// Token: 0x0400176D RID: 5997
		private float _scrollLimitBottom;

		// Token: 0x0400176E RID: 5998
		private bool _isDragging;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200183C RID: 6204
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005EDA RID: 24282
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005EDB RID: 24283
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005EDC RID: 24284
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnVisibilityChange' method.
			/// </summary>
			// Token: 0x04005EDD RID: 24285
			public static readonly StringName OnVisibilityChange = "OnVisibilityChange";

			/// <summary>
			/// Cached name for the 'RefreshLayout' method.
			/// </summary>
			// Token: 0x04005EDE RID: 24286
			public static readonly StringName RefreshLayout = "RefreshLayout";

			/// <summary>
			/// Cached name for the 'IsScrollbarNeeded' method.
			/// </summary>
			// Token: 0x04005EDF RID: 24287
			public static readonly StringName IsScrollbarNeeded = "IsScrollbarNeeded";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005EE0 RID: 24288
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'ProcessGuiFocus' method.
			/// </summary>
			// Token: 0x04005EE1 RID: 24289
			public static readonly StringName ProcessGuiFocus = "ProcessGuiFocus";

			/// <summary>
			/// Cached name for the 'UpdateScrollPosition' method.
			/// </summary>
			// Token: 0x04005EE2 RID: 24290
			public static readonly StringName UpdateScrollPosition = "UpdateScrollPosition";

			/// <summary>
			/// Cached name for the 'UpdateScrollbar' method.
			/// </summary>
			// Token: 0x04005EE3 RID: 24291
			public static readonly StringName UpdateScrollbar = "UpdateScrollbar";

			/// <summary>
			/// Cached name for the 'UpdatePositionBasedOnTrain' method.
			/// </summary>
			// Token: 0x04005EE4 RID: 24292
			public static readonly StringName UpdatePositionBasedOnTrain = "UpdatePositionBasedOnTrain";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005EE5 RID: 24293
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'ProcessMouseEvent' method.
			/// </summary>
			// Token: 0x04005EE6 RID: 24294
			public static readonly StringName ProcessMouseEvent = "ProcessMouseEvent";

			/// <summary>
			/// Cached name for the 'ProcessScrollEvent' method.
			/// </summary>
			// Token: 0x04005EE7 RID: 24295
			public static readonly StringName ProcessScrollEvent = "ProcessScrollEvent";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200183D RID: 6205
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_scrollbar' field.
			/// </summary>
			// Token: 0x04005EE8 RID: 24296
			public static readonly StringName _scrollbar = "_scrollbar";

			/// <summary>
			/// Cached name for the '_scrollbarTrain' field.
			/// </summary>
			// Token: 0x04005EE9 RID: 24297
			public static readonly StringName _scrollbarTrain = "_scrollbarTrain";

			/// <summary>
			/// Cached name for the '_dropdownItems' field.
			/// </summary>
			// Token: 0x04005EEA RID: 24298
			public static readonly StringName _dropdownItems = "_dropdownItems";

			/// <summary>
			/// Cached name for the '_maxHeight' field.
			/// </summary>
			// Token: 0x04005EEB RID: 24299
			public static readonly StringName _maxHeight = "_maxHeight";

			/// <summary>
			/// Cached name for the '_contentHeight' field.
			/// </summary>
			// Token: 0x04005EEC RID: 24300
			public static readonly StringName _contentHeight = "_contentHeight";

			/// <summary>
			/// Cached name for the '_startDragPos' field.
			/// </summary>
			// Token: 0x04005EED RID: 24301
			public static readonly StringName _startDragPos = "_startDragPos";

			/// <summary>
			/// Cached name for the '_targetDragPos' field.
			/// </summary>
			// Token: 0x04005EEE RID: 24302
			public static readonly StringName _targetDragPos = "_targetDragPos";

			/// <summary>
			/// Cached name for the '_scrollLimitBottom' field.
			/// </summary>
			// Token: 0x04005EEF RID: 24303
			public static readonly StringName _scrollLimitBottom = "_scrollLimitBottom";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x04005EF0 RID: 24304
			public static readonly StringName _isDragging = "_isDragging";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200183E RID: 6206
		public class SignalName : Control.SignalName
		{
		}
	}
}
