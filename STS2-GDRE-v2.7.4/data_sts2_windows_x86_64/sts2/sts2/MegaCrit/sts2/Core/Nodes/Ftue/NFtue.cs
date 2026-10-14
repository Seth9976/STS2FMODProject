using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003AD RID: 941
	[ScriptPath("res://src/Core/Nodes/Ftue/NFtue.cs")]
	public class NFtue : Control, IScreenContext
	{
		// Token: 0x060038A7 RID: 14503 RVA: 0x001AC750 File Offset: 0x001AA950
		public override void _EnterTree()
		{
			base.FocusMode = 2L;
			base.FocusNeighborBottom = base.GetPath();
			base.FocusNeighborTop = base.GetPath();
			base.FocusNeighborLeft = base.GetPath();
			base.FocusNeighborRight = base.GetPath();
			NHotkeyManager.Instance.AddBlockingScreen(this);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x001AC7A0 File Offset: 0x001AA9A0
		public override void _ExitTree()
		{
			NHotkeyManager.Instance.RemoveBlockingScreen(this);
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x060038A9 RID: 14505 RVA: 0x001AC7AD File Offset: 0x001AA9AD
		[Nullable(1)]
		public Control DefaultFocusedControl
		{
			[NullableContext(1)]
			get
			{
				return this;
			}
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x001AC7B0 File Offset: 0x001AA9B0
		protected void CloseFtue()
		{
			NModalContainer.Instance.Clear();
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038AB RID: 14507 RVA: 0x001AC7C4 File Offset: 0x001AA9C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NFtue.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFtue.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038AC RID: 14508 RVA: 0x001AC868 File Offset: 0x001AAA68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFtue.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFtue.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFtue.MethodName.CloseFtue && args.Count == 0)
			{
				this.CloseFtue();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060038AD RID: 14509 RVA: 0x001AC8ED File Offset: 0x001AAAED
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFtue.MethodName._EnterTree || (ref method) == NFtue.MethodName._ExitTree || (ref method) == NFtue.MethodName.CloseFtue || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060038AE RID: 14510 RVA: 0x001AC924 File Offset: 0x001AAB24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFtue.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038AF RID: 14511 RVA: 0x001AC95C File Offset: 0x001AAB5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFtue.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038B0 RID: 14512 RVA: 0x001AC991 File Offset: 0x001AAB91
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060038B1 RID: 14513 RVA: 0x001AC99A File Offset: 0x001AAB9A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A2 RID: 6050
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005C23 RID: 23587
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005C24 RID: 23588
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C25 RID: 23589
			public static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A3 RID: 6051
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005C26 RID: 23590
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A4 RID: 6052
		public class SignalName : Control.SignalName
		{
		}
	}
}
