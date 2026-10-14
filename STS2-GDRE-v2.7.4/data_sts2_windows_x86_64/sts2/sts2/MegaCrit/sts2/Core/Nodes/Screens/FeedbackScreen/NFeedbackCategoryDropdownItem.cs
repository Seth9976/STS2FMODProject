using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	// Token: 0x02000322 RID: 802
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NFeedbackCategoryDropdownItem.cs")]
	public class NFeedbackCategoryDropdownItem : NDropdownItem
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002CC0 RID: 11456 RVA: 0x0013FEBD File Offset: 0x0013E0BD
		// (set) Token: 0x06002CC1 RID: 11457 RVA: 0x0013FEC5 File Offset: 0x0013E0C5
		public int CategoryIndex { get; private set; }

		// Token: 0x06002CC2 RID: 11458 RVA: 0x0013FECE File Offset: 0x0013E0CE
		[NullableContext(1)]
		public void Init(int categoryIndex, string localizedCategory)
		{
			this.CategoryIndex = categoryIndex;
			base.Text = localizedCategory;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CC3 RID: 11459 RVA: 0x0013FEE0 File Offset: 0x0013E0E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NFeedbackCategoryDropdownItem.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "categoryIndex", 0L, "", 6L, false),
					new PropertyInfo(4L, "localizedCategory", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CC4 RID: 11460 RVA: 0x0013FF6C File Offset: 0x0013E16C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFeedbackCategoryDropdownItem.MethodName.Init && args.Count == 2)
			{
				this.Init(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002CC5 RID: 11461 RVA: 0x0013FFC2 File Offset: 0x0013E1C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFeedbackCategoryDropdownItem.MethodName.Init || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002CC6 RID: 11462 RVA: 0x0013FFDA File Offset: 0x0013E1DA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex)
			{
				this.CategoryIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002CC7 RID: 11463 RVA: 0x00140000 File Offset: 0x0013E200
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex)
			{
				int categoryIndex = this.CategoryIndex;
				value = VariantUtils.CreateFrom<int>(ref categoryIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CC8 RID: 11464 RVA: 0x00140038 File Offset: 0x0013E238
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CC9 RID: 11465 RVA: 0x0014006C File Offset: 0x0013E26C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName categoryIndex = NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex;
			int categoryIndex2 = this.CategoryIndex;
			info.AddProperty(categoryIndex, Variant.From<int>(ref categoryIndex2));
		}

		/// <inheritdoc />
		// Token: 0x06002CCA RID: 11466 RVA: 0x0014009C File Offset: 0x0013E29C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex, ref variant))
			{
				this.CategoryIndex = variant.As<int>();
			}
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200156E RID: 5486
		public new class MethodName : NDropdownItem.MethodName
		{
			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x0400522A RID: 21034
			public static readonly StringName Init = "Init";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200156F RID: 5487
		public new class PropertyName : NDropdownItem.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CategoryIndex' property.
			/// </summary>
			// Token: 0x0400522B RID: 21035
			public static readonly StringName CategoryIndex = "CategoryIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001570 RID: 5488
		public new class SignalName : NDropdownItem.SignalName
		{
		}
	}
}
