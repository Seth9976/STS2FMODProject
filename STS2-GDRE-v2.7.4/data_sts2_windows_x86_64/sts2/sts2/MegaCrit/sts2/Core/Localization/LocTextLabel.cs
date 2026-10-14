using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Localization
{
	// Token: 0x02000BC0 RID: 3008
	[NullableContext(2)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Localization/LocTextLabel.cs")]
	public class LocTextLabel : RichTextLabel
	{
		// Token: 0x170022A8 RID: 8872
		// (get) Token: 0x06007D72 RID: 32114 RVA: 0x002ABB06 File Offset: 0x002A9D06
		// (set) Token: 0x06007D73 RID: 32115 RVA: 0x002ABB0E File Offset: 0x002A9D0E
		public string LocalizationTable
		{
			get
			{
				return this._localizationTable;
			}
			set
			{
				if (this._localizationTable == value)
				{
					return;
				}
				this._localizationTable = value;
				this._locString = null;
				this.UpdateLocalization();
			}
		}

		// Token: 0x170022A9 RID: 8873
		// (get) Token: 0x06007D74 RID: 32116 RVA: 0x002ABB33 File Offset: 0x002A9D33
		// (set) Token: 0x06007D75 RID: 32117 RVA: 0x002ABB3B File Offset: 0x002A9D3B
		public string LocalizationKey
		{
			get
			{
				return this._localizationKey;
			}
			set
			{
				if (this._localizationKey == value)
				{
					return;
				}
				this._localizationKey = value;
				this._locString = null;
				this.UpdateLocalization();
			}
		}

		// Token: 0x06007D76 RID: 32118 RVA: 0x002ABB60 File Offset: 0x002A9D60
		private void UpdateLocalization()
		{
			if (this._localizationTable == null)
			{
				throw new InvalidOperationException("_localizationTable is null.");
			}
			if (this._localizationKey == null)
			{
				throw new InvalidOperationException("_localizationKey is null.");
			}
			if (this._locString == null)
			{
				this._locString = new LocString(this._localizationTable, this._localizationKey);
			}
			base.Text = this._locString.GetFormattedText();
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x002ABBC3 File Offset: 0x002A9DC3
		public override void _Ready()
		{
			this.UpdateLocalization();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06007D78 RID: 32120 RVA: 0x002ABBCC File Offset: 0x002A9DCC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(LocTextLabel.MethodName.UpdateLocalization, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(LocTextLabel.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06007D79 RID: 32121 RVA: 0x002ABC40 File Offset: 0x002A9E40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == LocTextLabel.MethodName.UpdateLocalization && args.Count == 0)
			{
				this.UpdateLocalization();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == LocTextLabel.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06007D7A RID: 32122 RVA: 0x002ABCA0 File Offset: 0x002A9EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == LocTextLabel.MethodName.UpdateLocalization || (ref method) == LocTextLabel.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06007D7B RID: 32123 RVA: 0x002ABCC8 File Offset: 0x002A9EC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == LocTextLabel.PropertyName.LocalizationTable)
			{
				this.LocalizationTable = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == LocTextLabel.PropertyName.LocalizationKey)
			{
				this.LocalizationKey = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == LocTextLabel.PropertyName._localizationTable)
			{
				this._localizationTable = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == LocTextLabel.PropertyName._localizationKey)
			{
				this._localizationKey = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06007D7C RID: 32124 RVA: 0x002ABD4C File Offset: 0x002A9F4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == LocTextLabel.PropertyName.LocalizationTable)
			{
				string text = this.LocalizationTable;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == LocTextLabel.PropertyName.LocalizationKey)
			{
				string text = this.LocalizationKey;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == LocTextLabel.PropertyName._localizationTable)
			{
				value = VariantUtils.CreateFrom<string>(ref this._localizationTable);
				return true;
			}
			if ((ref name) == LocTextLabel.PropertyName._localizationKey)
			{
				value = VariantUtils.CreateFrom<string>(ref this._localizationKey);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06007D7D RID: 32125 RVA: 0x002ABDE8 File Offset: 0x002A9FE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, LocTextLabel.PropertyName._localizationTable, 0L, "", 4102L, true),
				new PropertyInfo(4L, LocTextLabel.PropertyName._localizationKey, 0L, "", 4102L, true),
				new PropertyInfo(4L, LocTextLabel.PropertyName.LocalizationTable, 0L, "", 4096L, false),
				new PropertyInfo(4L, LocTextLabel.PropertyName.LocalizationKey, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06007D7E RID: 32126 RVA: 0x002ABE7C File Offset: 0x002AA07C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName localizationTable = LocTextLabel.PropertyName.LocalizationTable;
			string text = this.LocalizationTable;
			info.AddProperty(localizationTable, Variant.From<string>(ref text));
			StringName localizationKey = LocTextLabel.PropertyName.LocalizationKey;
			text = this.LocalizationKey;
			info.AddProperty(localizationKey, Variant.From<string>(ref text));
			info.AddProperty(LocTextLabel.PropertyName._localizationTable, Variant.From<string>(ref this._localizationTable));
			info.AddProperty(LocTextLabel.PropertyName._localizationKey, Variant.From<string>(ref this._localizationKey));
		}

		/// <inheritdoc />
		// Token: 0x06007D7F RID: 32127 RVA: 0x002ABEF0 File Offset: 0x002AA0F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(LocTextLabel.PropertyName.LocalizationTable, ref variant))
			{
				this.LocalizationTable = variant.As<string>();
			}
			Variant variant2;
			if (info.TryGetProperty(LocTextLabel.PropertyName.LocalizationKey, ref variant2))
			{
				this.LocalizationKey = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(LocTextLabel.PropertyName._localizationTable, ref variant3))
			{
				this._localizationTable = variant3.As<string>();
			}
			Variant variant4;
			if (info.TryGetProperty(LocTextLabel.PropertyName._localizationKey, ref variant4))
			{
				this._localizationKey = variant4.As<string>();
			}
		}

		// Token: 0x040029EF RID: 10735
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private string _localizationTable;

		// Token: 0x040029F0 RID: 10736
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private string _localizationKey;

		// Token: 0x040029F1 RID: 10737
		private LocString _locString;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020022F5 RID: 8949
		[NullableContext(0)]
		public class MethodName : RichTextLabel.MethodName
		{
			/// <summary>
			/// Cached name for the 'UpdateLocalization' method.
			/// </summary>
			// Token: 0x040092DD RID: 37597
			public static readonly StringName UpdateLocalization = "UpdateLocalization";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040092DE RID: 37598
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020022F6 RID: 8950
		[NullableContext(0)]
		public class PropertyName : RichTextLabel.PropertyName
		{
			/// <summary>
			/// Cached name for the 'LocalizationTable' property.
			/// </summary>
			// Token: 0x040092DF RID: 37599
			public static readonly StringName LocalizationTable = "LocalizationTable";

			/// <summary>
			/// Cached name for the 'LocalizationKey' property.
			/// </summary>
			// Token: 0x040092E0 RID: 37600
			public static readonly StringName LocalizationKey = "LocalizationKey";

			/// <summary>
			/// Cached name for the '_localizationTable' field.
			/// </summary>
			// Token: 0x040092E1 RID: 37601
			public static readonly StringName _localizationTable = "_localizationTable";

			/// <summary>
			/// Cached name for the '_localizationKey' field.
			/// </summary>
			// Token: 0x040092E2 RID: 37602
			public static readonly StringName _localizationKey = "_localizationKey";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020022F7 RID: 8951
		[NullableContext(0)]
		public class SignalName : RichTextLabel.SignalName
		{
		}
	}
}
