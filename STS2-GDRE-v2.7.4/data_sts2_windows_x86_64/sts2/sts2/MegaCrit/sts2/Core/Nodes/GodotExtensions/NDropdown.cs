using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	/// <summary>
	/// A custom Dropdown component class.
	/// </summary>
	// Token: 0x0200039E RID: 926
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NDropdown.cs")]
	public class NDropdown : NClickableControl
	{
		// Token: 0x060037BC RID: 14268 RVA: 0x001A4E02 File Offset: 0x001A3002
		public override void _Ready()
		{
			if (base.GetType() != typeof(NDropdown))
			{
				throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead");
			}
			this.ConnectSignals();
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x001A4E2C File Offset: 0x001A302C
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._currentOptionHighlight = base.GetNode<Control>("%Highlight");
			this._currentOptionLabel = base.GetNode<MegaLabel>("%Label");
			this._dropdownContainer = base.GetNode<Control>("%DropdownContainer");
			this._dropdownItems = this._dropdownContainer.GetNode<Control>("VBoxContainer");
			this._dismisser = base.GetNode<NButton>("%Dismisser");
			this._dismisser.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDismisserClicked)), 0U);
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x001A4EF3 File Offset: 0x001A30F3
		private void OnVisibilityChange()
		{
			if (!base.IsVisibleInTree() && this._isOpen)
			{
				this.CloseDropdown();
			}
		}

		/// <summary>
		/// Removes any junk that's already there
		/// </summary>
		// Token: 0x060037BF RID: 14271 RVA: 0x001A4F0C File Offset: 0x001A310C
		protected void ClearDropdownItems()
		{
			foreach (Node node in this._dropdownItems.GetChildren(false))
			{
				this._dropdownItems.RemoveChildSafely(node);
				node.QueueFreeSafely();
			}
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x001A4F6C File Offset: 0x001A316C
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!this._isEnabled)
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			Viewport viewport = base.GetViewport();
			if (viewport == null)
			{
				return;
			}
			Control control = viewport.GuiGetFocusOwner();
			bool flag = control is TextEdit || control is LineEdit;
			if (flag)
			{
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.cancel, false, false) && this._isOpen)
			{
				this.CloseDropdown();
				viewport.SetInputAsHandled();
			}
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x001A4FE2 File Offset: 0x001A31E2
		[NullableContext(1)]
		private void OnDismisserClicked(NButton obj)
		{
			this.CloseDropdown();
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x001A4FEA File Offset: 0x001A31EA
		protected override void OnRelease()
		{
			base.OnRelease();
			if (this._isOpen)
			{
				Log.Info("Closing dropdown because you clicked on the main dropdown button.", 2);
				this.CloseDropdown();
				return;
			}
			this.OpenDropdown();
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x001A5014 File Offset: 0x001A3214
		private void OpenDropdown()
		{
			this._dropdownContainer.Visible = true;
			this._dismisser.Visible = true;
			this._isOpen = true;
			base.GetParent().MoveChildSafely(this, base.GetParent().GetChildCount(false));
			List<NDropdownItem> list = this._dropdownItems.GetChildren(false).OfType<NDropdownItem>().ToList<NDropdownItem>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].UnhoverSelection();
				list[i].FocusNeighborLeft = list[i].GetPath();
				list[i].FocusNeighborRight = list[i].GetPath();
				list[i].FocusNeighborTop = ((i > 0) ? list[i - 1].GetPath() : list[i].GetPath());
				list[i].FocusNeighborBottom = ((i < list.Count - 1) ? list[i + 1].GetPath() : list[i].GetPath());
				list[i].FocusMode = 2L;
			}
			NDropdownItem ndropdownItem = list.FirstOrDefault<NDropdownItem>();
			if (ndropdownItem == null)
			{
				return;
			}
			ndropdownItem.TryGrabFocus();
		}

		/// <summary>
		/// Closes this dropdown. The name is verbose because it sounds generic without it.
		/// Called when we select an option or click outside the bounds of the dropdown.
		/// </summary>
		// Token: 0x060037C4 RID: 14276 RVA: 0x001A513F File Offset: 0x001A333F
		protected void CloseDropdown()
		{
			this._dismisser.Visible = false;
			this._dropdownContainer.Visible = false;
			this._isOpen = false;
			this.TryGrabFocus();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060037C5 RID: 14277 RVA: 0x001A5168 File Offset: 0x001A3368
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NDropdown.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdown.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdown.MethodName.OnVisibilityChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdown.MethodName.ClearDropdownItems, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdown.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NDropdown.MethodName.OnDismisserClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "obj", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDropdown.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdown.MethodName.OpenDropdown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdown.MethodName.CloseDropdown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060037C6 RID: 14278 RVA: 0x001A5388 File Offset: 0x001A3588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDropdown.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName.OnVisibilityChange && args.Count == 0)
			{
				this.OnVisibilityChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName.ClearDropdownItems && args.Count == 0)
			{
				this.ClearDropdownItems();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName.OnDismisserClicked && args.Count == 1)
			{
				this.OnDismisserClicked(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName.OpenDropdown && args.Count == 0)
			{
				this.OpenDropdown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdown.MethodName.CloseDropdown && args.Count == 0)
			{
				this.CloseDropdown();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060037C7 RID: 14279 RVA: 0x001A5508 File Offset: 0x001A3708
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDropdown.MethodName._Ready || (ref method) == NDropdown.MethodName.ConnectSignals || (ref method) == NDropdown.MethodName.OnVisibilityChange || (ref method) == NDropdown.MethodName.ClearDropdownItems || (ref method) == NDropdown.MethodName._Input || (ref method) == NDropdown.MethodName.OnDismisserClicked || (ref method) == NDropdown.MethodName.OnRelease || (ref method) == NDropdown.MethodName.OpenDropdown || (ref method) == NDropdown.MethodName.CloseDropdown || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060037C8 RID: 14280 RVA: 0x001A55A4 File Offset: 0x001A37A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDropdown.PropertyName._dropdownContainer)
			{
				this._dropdownContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._dropdownItems)
			{
				this._dropdownItems = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._dismisser)
			{
				this._dismisser = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._currentOptionLabel)
			{
				this._currentOptionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._currentOptionHighlight)
			{
				this._currentOptionHighlight = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._isHovered)
			{
				this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._isOpen)
			{
				this._isOpen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060037C9 RID: 14281 RVA: 0x001A5678 File Offset: 0x001A3878
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDropdown.PropertyName._dropdownContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._dropdownContainer);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._dropdownItems)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._dropdownItems);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._dismisser)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._dismisser);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._currentOptionLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._currentOptionLabel);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._currentOptionHighlight)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._currentOptionHighlight);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._isHovered)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
				return true;
			}
			if ((ref name) == NDropdown.PropertyName._isOpen)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isOpen);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060037CA RID: 14282 RVA: 0x001A5770 File Offset: 0x001A3970
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDropdown.PropertyName._dropdownContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdown.PropertyName._dropdownItems, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdown.PropertyName._dismisser, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdown.PropertyName._currentOptionLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdown.PropertyName._currentOptionHighlight, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDropdown.PropertyName._isHovered, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDropdown.PropertyName._isOpen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060037CB RID: 14283 RVA: 0x001A586C File Offset: 0x001A3A6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDropdown.PropertyName._dropdownContainer, Variant.From<Control>(ref this._dropdownContainer));
			info.AddProperty(NDropdown.PropertyName._dropdownItems, Variant.From<Control>(ref this._dropdownItems));
			info.AddProperty(NDropdown.PropertyName._dismisser, Variant.From<NButton>(ref this._dismisser));
			info.AddProperty(NDropdown.PropertyName._currentOptionLabel, Variant.From<MegaLabel>(ref this._currentOptionLabel));
			info.AddProperty(NDropdown.PropertyName._currentOptionHighlight, Variant.From<Control>(ref this._currentOptionHighlight));
			info.AddProperty(NDropdown.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
			info.AddProperty(NDropdown.PropertyName._isOpen, Variant.From<bool>(ref this._isOpen));
		}

		/// <inheritdoc />
		// Token: 0x060037CC RID: 14284 RVA: 0x001A591C File Offset: 0x001A3B1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDropdown.PropertyName._dropdownContainer, ref variant))
			{
				this._dropdownContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDropdown.PropertyName._dropdownItems, ref variant2))
			{
				this._dropdownItems = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDropdown.PropertyName._dismisser, ref variant3))
			{
				this._dismisser = variant3.As<NButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDropdown.PropertyName._currentOptionLabel, ref variant4))
			{
				this._currentOptionLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDropdown.PropertyName._currentOptionHighlight, ref variant5))
			{
				this._currentOptionHighlight = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDropdown.PropertyName._isHovered, ref variant6))
			{
				this._isHovered = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDropdown.PropertyName._isOpen, ref variant7))
			{
				this._isOpen = variant7.As<bool>();
			}
		}

		// Token: 0x040015AF RID: 5551
		[Nullable(1)]
		private Control _dropdownContainer;

		// Token: 0x040015B0 RID: 5552
		[Nullable(1)]
		protected Control _dropdownItems;

		// Token: 0x040015B1 RID: 5553
		[Nullable(1)]
		private NButton _dismisser;

		// Token: 0x040015B2 RID: 5554
		[Nullable(1)]
		protected MegaLabel _currentOptionLabel;

		// Token: 0x040015B3 RID: 5555
		[Nullable(1)]
		protected Control _currentOptionHighlight;

		// Token: 0x040015B4 RID: 5556
		private bool _isHovered;

		// Token: 0x040015B5 RID: 5557
		private bool _isOpen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200176D RID: 5997
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005B6B RID: 23403
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04005B6C RID: 23404
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnVisibilityChange' method.
			/// </summary>
			// Token: 0x04005B6D RID: 23405
			public static readonly StringName OnVisibilityChange = "OnVisibilityChange";

			/// <summary>
			/// Cached name for the 'ClearDropdownItems' method.
			/// </summary>
			// Token: 0x04005B6E RID: 23406
			public static readonly StringName ClearDropdownItems = "ClearDropdownItems";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005B6F RID: 23407
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'OnDismisserClicked' method.
			/// </summary>
			// Token: 0x04005B70 RID: 23408
			public static readonly StringName OnDismisserClicked = "OnDismisserClicked";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005B71 RID: 23409
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OpenDropdown' method.
			/// </summary>
			// Token: 0x04005B72 RID: 23410
			public static readonly StringName OpenDropdown = "OpenDropdown";

			/// <summary>
			/// Cached name for the 'CloseDropdown' method.
			/// </summary>
			// Token: 0x04005B73 RID: 23411
			public static readonly StringName CloseDropdown = "CloseDropdown";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200176E RID: 5998
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_dropdownContainer' field.
			/// </summary>
			// Token: 0x04005B74 RID: 23412
			public static readonly StringName _dropdownContainer = "_dropdownContainer";

			/// <summary>
			/// Cached name for the '_dropdownItems' field.
			/// </summary>
			// Token: 0x04005B75 RID: 23413
			public static readonly StringName _dropdownItems = "_dropdownItems";

			/// <summary>
			/// Cached name for the '_dismisser' field.
			/// </summary>
			// Token: 0x04005B76 RID: 23414
			public static readonly StringName _dismisser = "_dismisser";

			/// <summary>
			/// Cached name for the '_currentOptionLabel' field.
			/// </summary>
			// Token: 0x04005B77 RID: 23415
			public static readonly StringName _currentOptionLabel = "_currentOptionLabel";

			/// <summary>
			/// Cached name for the '_currentOptionHighlight' field.
			/// </summary>
			// Token: 0x04005B78 RID: 23416
			public static readonly StringName _currentOptionHighlight = "_currentOptionHighlight";

			/// <summary>
			/// Cached name for the '_isHovered' field.
			/// </summary>
			// Token: 0x04005B79 RID: 23417
			public new static readonly StringName _isHovered = "_isHovered";

			/// <summary>
			/// Cached name for the '_isOpen' field.
			/// </summary>
			// Token: 0x04005B7A RID: 23418
			public static readonly StringName _isOpen = "_isOpen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200176F RID: 5999
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
