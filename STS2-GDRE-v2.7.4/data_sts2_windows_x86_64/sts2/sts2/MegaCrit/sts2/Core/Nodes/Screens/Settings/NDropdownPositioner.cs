using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// Helper script to allow a Node to become anchored to this Node.
	/// Used specifically for positioning the DropdownContainers (they're tricky due to how they're layered)
	/// </summary>
	// Token: 0x02000290 RID: 656
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NDropdownPositioner.cs")]
	public class NDropdownPositioner : Control
	{
		// Token: 0x060022E1 RID: 8929 RVA: 0x000EDF98 File Offset: 0x000EC198
		public override void _Ready()
		{
			base.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this._dropdownNode.TryGrabFocus();
			}), 0U);
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
			this.OnVisibilityChange();
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000EDFE8 File Offset: 0x000EC1E8
		private void OnVisibilityChange()
		{
			if (base.Visible)
			{
				this._dropdownNode.FocusNeighborBottom = base.FocusNeighborBottom;
				this._dropdownNode.FocusNeighborTop = base.FocusNeighborTop;
				this._dropdownNode.FocusNeighborLeft = base.FocusNeighborLeft;
				this._dropdownNode.FocusNeighborRight = base.FocusNeighborRight;
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000EE041 File Offset: 0x000EC241
		public override void _Process(double delta)
		{
			this._dropdownNode.GlobalPosition = base.GlobalPosition;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022E4 RID: 8932 RVA: 0x000EE054 File Offset: 0x000EC254
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NDropdownPositioner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownPositioner.MethodName.OnVisibilityChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownPositioner.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022E5 RID: 8933 RVA: 0x000EE11C File Offset: 0x000EC31C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDropdownPositioner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownPositioner.MethodName.OnVisibilityChange && args.Count == 0)
			{
				this.OnVisibilityChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownPositioner.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060022E6 RID: 8934 RVA: 0x000EE1AF File Offset: 0x000EC3AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDropdownPositioner.MethodName._Ready || (ref method) == NDropdownPositioner.MethodName.OnVisibilityChange || (ref method) == NDropdownPositioner.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060022E7 RID: 8935 RVA: 0x000EE1E5 File Offset: 0x000EC3E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDropdownPositioner.PropertyName._dropdownNode)
			{
				this._dropdownNode = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060022E8 RID: 8936 RVA: 0x000EE20A File Offset: 0x000EC40A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDropdownPositioner.PropertyName._dropdownNode)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._dropdownNode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022E9 RID: 8937 RVA: 0x000EE234 File Offset: 0x000EC434
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDropdownPositioner.PropertyName._dropdownNode, 34L, "Control", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022EA RID: 8938 RVA: 0x000EE26A File Offset: 0x000EC46A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDropdownPositioner.PropertyName._dropdownNode, Variant.From<Control>(ref this._dropdownNode));
		}

		/// <inheritdoc />
		// Token: 0x060022EB RID: 8939 RVA: 0x000EE28C File Offset: 0x000EC48C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDropdownPositioner.PropertyName._dropdownNode, ref variant))
			{
				this._dropdownNode = variant.As<Control>();
			}
		}

		// Token: 0x04000D73 RID: 3443
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Control _dropdownNode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001363 RID: 4963
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B04 RID: 19204
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnVisibilityChange' method.
			/// </summary>
			// Token: 0x04004B05 RID: 19205
			public static readonly StringName OnVisibilityChange = "OnVisibilityChange";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004B06 RID: 19206
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001364 RID: 4964
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_dropdownNode' field.
			/// </summary>
			// Token: 0x04004B07 RID: 19207
			public static readonly StringName _dropdownNode = "_dropdownNode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001365 RID: 4965
		public class SignalName : Control.SignalName
		{
		}
	}
}
