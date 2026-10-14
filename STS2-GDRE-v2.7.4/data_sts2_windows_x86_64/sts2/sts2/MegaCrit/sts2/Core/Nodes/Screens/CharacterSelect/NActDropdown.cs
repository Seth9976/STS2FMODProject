using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	/// <summary>
	/// Dropdown for selecting an act. Only used in development.
	/// If this ends up being used in production, make it less messy.
	/// </summary>
	// Token: 0x0200033C RID: 828
	[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NActDropdown.cs")]
	public class NActDropdown : NDropdown
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x00152B95 File Offset: 0x00150D95
		[Nullable(1)]
		public string CurrentOption
		{
			[NullableContext(1)]
			get
			{
				return NActDropdown._options[this._currentOptionIndex];
			}
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00152BA3 File Offset: 0x00150DA3
		public override void _Ready()
		{
			this.ConnectSignals();
			this.PopulateOptions();
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x00152BB1 File Offset: 0x00150DB1
		protected override void OnFocus()
		{
			this._currentOptionHighlight.Modulate = new Color("afcdde");
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x00152BC8 File Offset: 0x00150DC8
		protected override void OnUnfocus()
		{
			this._currentOptionHighlight.Modulate = Colors.White;
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x00152BDC File Offset: 0x00150DDC
		private void PopulateOptions()
		{
			List<NDropdownItem> list = this.GetDropdownItems().ToList<NDropdownItem>();
			for (int i = 0; i < NActDropdown._options.Length; i++)
			{
				NDropdownItem ndropdownItem = list[i];
				string text = NActDropdown._options[i];
				ndropdownItem.Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
				NDropdownItem ndropdownItem2 = ndropdownItem;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
				defaultInterpolatedStringHandler.AppendFormatted<char>(char.ToUpperInvariant(text[0]));
				string text2 = text;
				defaultInterpolatedStringHandler.AppendFormatted(text2.Substring(1, text2.Length - 1));
				ndropdownItem2.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this.GetDropdownContainer().GetParent<NDropdownContainer>().RefreshLayout();
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x00152C89 File Offset: 0x00150E89
		[NullableContext(1)]
		private void OnDropdownItemSelected(NDropdownItem item)
		{
			base.CloseDropdown();
			this._currentOptionIndex = this.GetDropdownItems().ToList<NDropdownItem>().IndexOf(item);
			this._currentOptionLabel.SetTextAutoSize(item.Text);
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x00152CB9 File Offset: 0x00150EB9
		[NullableContext(1)]
		private Control GetDropdownContainer()
		{
			return base.GetNode<Control>("DropdownContainer/VBoxContainer");
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x00152CCB File Offset: 0x00150ECB
		[NullableContext(1)]
		private IEnumerable<NDropdownItem> GetDropdownItems()
		{
			return this.GetDropdownContainer().GetChildren(false).OfType<NDropdownItem>();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002ECB RID: 11979 RVA: 0x00152CE0 File Offset: 0x00150EE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NActDropdown.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NActDropdown.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NActDropdown.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NActDropdown.MethodName.PopulateOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NActDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "item", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NActDropdown.MethodName.GetDropdownContainer, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002ECC RID: 11980 RVA: 0x00152E4C File Offset: 0x0015104C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NActDropdown.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NActDropdown.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NActDropdown.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NActDropdown.MethodName.PopulateOptions && args.Count == 0)
			{
				this.PopulateOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NActDropdown.MethodName.OnDropdownItemSelected && args.Count == 1)
			{
				this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NActDropdown.MethodName.GetDropdownContainer && args.Count == 0)
			{
				Control dropdownContainer = this.GetDropdownContainer();
				ret = VariantUtils.CreateFrom<Control>(ref dropdownContainer);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002ECD RID: 11981 RVA: 0x00152F58 File Offset: 0x00151158
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NActDropdown.MethodName._Ready || (ref method) == NActDropdown.MethodName.OnFocus || (ref method) == NActDropdown.MethodName.OnUnfocus || (ref method) == NActDropdown.MethodName.PopulateOptions || (ref method) == NActDropdown.MethodName.OnDropdownItemSelected || (ref method) == NActDropdown.MethodName.GetDropdownContainer || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002ECE RID: 11982 RVA: 0x00152FC6 File Offset: 0x001511C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NActDropdown.PropertyName._currentOptionIndex)
			{
				this._currentOptionIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002ECF RID: 11983 RVA: 0x00152FEC File Offset: 0x001511EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NActDropdown.PropertyName.CurrentOption)
			{
				string currentOption = this.CurrentOption;
				value = VariantUtils.CreateFrom<string>(ref currentOption);
				return true;
			}
			if ((ref name) == NActDropdown.PropertyName._currentOptionIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentOptionIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002ED0 RID: 11984 RVA: 0x00153044 File Offset: 0x00151244
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NActDropdown.PropertyName._currentOptionIndex, 0L, "", 4096L, false),
				new PropertyInfo(4L, NActDropdown.PropertyName.CurrentOption, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002ED1 RID: 11985 RVA: 0x00153098 File Offset: 0x00151298
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NActDropdown.PropertyName._currentOptionIndex, Variant.From<int>(ref this._currentOptionIndex));
		}

		/// <inheritdoc />
		// Token: 0x06002ED2 RID: 11986 RVA: 0x001530B8 File Offset: 0x001512B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NActDropdown.PropertyName._currentOptionIndex, ref variant))
			{
				this._currentOptionIndex = variant.As<int>();
			}
		}

		// Token: 0x040011D7 RID: 4567
		[Nullable(1)]
		private static readonly string[] _options = new string[] { "random", "overgrowth", "underdocks" };

		// Token: 0x040011D8 RID: 4568
		private int _currentOptionIndex = NActDropdown._options.IndexOf("random");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015D2 RID: 5586
		public new class MethodName : NDropdown.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400541E RID: 21534
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400541F RID: 21535
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005420 RID: 21536
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'PopulateOptions' method.
			/// </summary>
			// Token: 0x04005421 RID: 21537
			public static readonly StringName PopulateOptions = "PopulateOptions";

			/// <summary>
			/// Cached name for the 'OnDropdownItemSelected' method.
			/// </summary>
			// Token: 0x04005422 RID: 21538
			public static readonly StringName OnDropdownItemSelected = "OnDropdownItemSelected";

			/// <summary>
			/// Cached name for the 'GetDropdownContainer' method.
			/// </summary>
			// Token: 0x04005423 RID: 21539
			public static readonly StringName GetDropdownContainer = "GetDropdownContainer";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015D3 RID: 5587
		public new class PropertyName : NDropdown.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CurrentOption' property.
			/// </summary>
			// Token: 0x04005424 RID: 21540
			public static readonly StringName CurrentOption = "CurrentOption";

			/// <summary>
			/// Cached name for the '_currentOptionIndex' field.
			/// </summary>
			// Token: 0x04005425 RID: 21541
			public static readonly StringName _currentOptionIndex = "_currentOptionIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015D4 RID: 5588
		public new class SignalName : NDropdown.SignalName
		{
		}
	}
}
