using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x0200028F RID: 655
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NDisplayDropdownItem.cs")]
	public class NDisplayDropdownItem : NDropdownItem
	{
		// Token: 0x060022D7 RID: 8919 RVA: 0x000EDDA4 File Offset: 0x000EBFA4
		public void Init(int setIndex)
		{
			this.displayIndex = setIndex;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Monitor (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.displayIndex);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			this._label.SetTextAutoSize(text);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022D8 RID: 8920 RVA: 0x000EDDFC File Offset: 0x000EBFFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NDisplayDropdownItem.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "setIndex", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022D9 RID: 8921 RVA: 0x000EDE65 File Offset: 0x000EC065
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDisplayDropdownItem.MethodName.Init && args.Count == 1)
			{
				this.Init(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060022DA RID: 8922 RVA: 0x000EDEA3 File Offset: 0x000EC0A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDisplayDropdownItem.MethodName.Init || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060022DB RID: 8923 RVA: 0x000EDEBB File Offset: 0x000EC0BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDisplayDropdownItem.PropertyName.displayIndex)
			{
				this.displayIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060022DC RID: 8924 RVA: 0x000EDEE0 File Offset: 0x000EC0E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDisplayDropdownItem.PropertyName.displayIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this.displayIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022DD RID: 8925 RVA: 0x000EDF0C File Offset: 0x000EC10C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NDisplayDropdownItem.PropertyName.displayIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022DE RID: 8926 RVA: 0x000EDF40 File Offset: 0x000EC140
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDisplayDropdownItem.PropertyName.displayIndex, Variant.From<int>(ref this.displayIndex));
		}

		/// <inheritdoc />
		// Token: 0x060022DF RID: 8927 RVA: 0x000EDF60 File Offset: 0x000EC160
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDisplayDropdownItem.PropertyName.displayIndex, ref variant))
			{
				this.displayIndex = variant.As<int>();
			}
		}

		// Token: 0x04000D72 RID: 3442
		public int displayIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001360 RID: 4960
		public new class MethodName : NDropdownItem.MethodName
		{
			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x04004B02 RID: 19202
			public static readonly StringName Init = "Init";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001361 RID: 4961
		public new class PropertyName : NDropdownItem.PropertyName
		{
			/// <summary>
			/// Cached name for the 'displayIndex' field.
			/// </summary>
			// Token: 0x04004B03 RID: 19203
			public static readonly StringName displayIndex = "displayIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001362 RID: 4962
		public new class SignalName : NDropdownItem.SignalName
		{
		}
	}
}
