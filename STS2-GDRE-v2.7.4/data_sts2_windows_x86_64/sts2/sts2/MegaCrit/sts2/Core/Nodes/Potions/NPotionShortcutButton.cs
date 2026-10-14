using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Potions
{
	// Token: 0x02000383 RID: 899
	[ScriptPath("res://src/Core/Nodes/Potions/NPotionShortcutButton.cs")]
	public class NPotionShortcutButton : NButton
	{
		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06003589 RID: 13705 RVA: 0x0018FAC4 File Offset: 0x0018DCC4
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.topPanel };
			}
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x0018FAD9 File Offset: 0x0018DCD9
		public override void _Ready()
		{
			this.ConnectSignals();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600358B RID: 13707 RVA: 0x0018FAE4 File Offset: 0x0018DCE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NPotionShortcutButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600358C RID: 13708 RVA: 0x0018FB28 File Offset: 0x0018DD28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionShortcutButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600358D RID: 13709 RVA: 0x0018FB58 File Offset: 0x0018DD58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionShortcutButton.MethodName._Ready || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600358E RID: 13710 RVA: 0x0018FB70 File Offset: 0x0018DD70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionShortcutButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600358F RID: 13711 RVA: 0x0018FBA8 File Offset: 0x0018DDA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NPotionShortcutButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003590 RID: 13712 RVA: 0x0018FBDD File Offset: 0x0018DDDD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003591 RID: 13713 RVA: 0x0018FBE6 File Offset: 0x0018DDE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001703 RID: 5891
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040059BC RID: 22972
			public new static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001704 RID: 5892
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040059BD RID: 22973
			public new static readonly StringName Hotkeys = "Hotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001705 RID: 5893
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
