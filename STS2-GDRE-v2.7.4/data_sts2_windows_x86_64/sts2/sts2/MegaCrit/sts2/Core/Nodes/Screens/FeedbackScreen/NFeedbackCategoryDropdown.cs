using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	/// <summary>
	/// The language dropdown in the OptionsScreen.
	/// </summary>
	// Token: 0x02000321 RID: 801
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NFeedbackCategoryDropdown.cs")]
	public class NFeedbackCategoryDropdown : NDropdown
	{
		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x0013F5D8 File Offset: 0x0013D7D8
		public string CurrentCategory
		{
			get
			{
				return this._categories[this._currentCategoryIndex];
			}
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x0013F5E8 File Offset: 0x0013D7E8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._currentOptionHighlight = base.GetNode<Panel>("%Highlight");
			this._currentOptionLabel = base.GetNode<MegaLabel>("%Label");
			this.PopulateOptions();
			this._currentOptionLabel.SetTextAutoSize(this._categoryLoc[this._currentCategoryIndex].GetFormattedText());
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x0013F660 File Offset: 0x0013D860
		protected override void OnFocus()
		{
			this._currentOptionHighlight.Modulate = new Color("afcdde");
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x0013F68E File Offset: 0x0013D88E
		protected override void OnUnfocus()
		{
			this._selectionReticle.OnDeselect();
			this._currentOptionHighlight.Modulate = Colors.White;
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x0013F6AC File Offset: 0x0013D8AC
		private void PopulateOptions()
		{
			if (LocManager.Instance.Language != "eng")
			{
				string[] baseCategories = NFeedbackCategoryDropdown._baseCategories;
				int num = 0;
				string[] array = new string[1 + baseCategories.Length];
				ReadOnlySpan<string> readOnlySpan = new ReadOnlySpan<string>(baseCategories);
				readOnlySpan.CopyTo(new Span<string>(array).Slice(num, readOnlySpan.Length));
				num += readOnlySpan.Length;
				array[num] = "translation";
				this._categories = array;
				LocString[] baseCategoryLoc = NFeedbackCategoryDropdown._baseCategoryLoc;
				num = 0;
				LocString[] array2 = new LocString[1 + baseCategoryLoc.Length];
				ReadOnlySpan<LocString> readOnlySpan2 = new ReadOnlySpan<LocString>(baseCategoryLoc);
				readOnlySpan2.CopyTo(new Span<LocString>(array2).Slice(num, readOnlySpan2.Length));
				num += readOnlySpan2.Length;
				array2[num] = new LocString("settings_ui", "FEEDBACK_CATEGORY.translation");
				this._categoryLoc = array2;
			}
			else
			{
				this._categories = NFeedbackCategoryDropdown._baseCategories;
				this._categoryLoc = NFeedbackCategoryDropdown._baseCategoryLoc;
			}
			this._currentCategoryIndex = this._categories.IndexOf("feedback");
			Control node = base.GetNode<Control>("DropdownContainer/VBoxContainer");
			foreach (Node node2 in node.GetChildren(false))
			{
				node.RemoveChildSafely(node2);
				node2.QueueFreeSafely();
			}
			for (int i = 0; i < this._categories.Length; i++)
			{
				NFeedbackCategoryDropdownItem nfeedbackCategoryDropdownItem = this._dropdownItemScene.Instantiate<NFeedbackCategoryDropdownItem>(0L);
				node.AddChildSafely(nfeedbackCategoryDropdownItem);
				nfeedbackCategoryDropdownItem.Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
				nfeedbackCategoryDropdownItem.Init(i, this._categoryLoc[i].GetFormattedText());
			}
			node.GetParent<NDropdownContainer>().RefreshLayout();
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x0013F884 File Offset: 0x0013DA84
		private void OnDropdownItemSelected(NDropdownItem item)
		{
			NFeedbackCategoryDropdownItem nfeedbackCategoryDropdownItem = (NFeedbackCategoryDropdownItem)item;
			if (nfeedbackCategoryDropdownItem.CategoryIndex == this._currentCategoryIndex)
			{
				return;
			}
			base.CloseDropdown();
			this._currentCategoryIndex = nfeedbackCategoryDropdownItem.CategoryIndex;
			this._currentOptionLabel.SetTextAutoSize(this._categoryLoc[this._currentCategoryIndex].GetFormattedText());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CB6 RID: 11446 RVA: 0x0013F8D8 File Offset: 0x0013DAD8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NFeedbackCategoryDropdown.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFeedbackCategoryDropdown.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFeedbackCategoryDropdown.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFeedbackCategoryDropdown.MethodName.PopulateOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFeedbackCategoryDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "item", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CB7 RID: 11447 RVA: 0x0013FA08 File Offset: 0x0013DC08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFeedbackCategoryDropdown.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFeedbackCategoryDropdown.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFeedbackCategoryDropdown.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFeedbackCategoryDropdown.MethodName.PopulateOptions && args.Count == 0)
			{
				this.PopulateOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFeedbackCategoryDropdown.MethodName.OnDropdownItemSelected && args.Count == 1)
			{
				this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002CB8 RID: 11448 RVA: 0x0013FAE8 File Offset: 0x0013DCE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFeedbackCategoryDropdown.MethodName._Ready || (ref method) == NFeedbackCategoryDropdown.MethodName.OnFocus || (ref method) == NFeedbackCategoryDropdown.MethodName.OnUnfocus || (ref method) == NFeedbackCategoryDropdown.MethodName.PopulateOptions || (ref method) == NFeedbackCategoryDropdown.MethodName.OnDropdownItemSelected || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002CB9 RID: 11449 RVA: 0x0013FB48 File Offset: 0x0013DD48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._dropdownItemScene)
			{
				this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._categories)
			{
				this._categories = VariantUtils.ConvertTo<string[]>(ref value);
				return true;
			}
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex)
			{
				this._currentCategoryIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002CBA RID: 11450 RVA: 0x0013FBCC File Offset: 0x0013DDCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName.CurrentCategory)
			{
				string currentCategory = this.CurrentCategory;
				value = VariantUtils.CreateFrom<string>(ref currentCategory);
				return true;
			}
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._dropdownItemScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
				return true;
			}
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._categories)
			{
				value = VariantUtils.CreateFrom<string[]>(ref this._categories);
				return true;
			}
			if ((ref name) == NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentCategoryIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CBB RID: 11451 RVA: 0x0013FC84 File Offset: 0x0013DE84
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFeedbackCategoryDropdown.PropertyName._dropdownItemScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NFeedbackCategoryDropdown.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(34L, NFeedbackCategoryDropdown.PropertyName._categories, 0L, "", 4096L, false),
				new PropertyInfo(2L, NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex, 0L, "", 4096L, false),
				new PropertyInfo(4L, NFeedbackCategoryDropdown.PropertyName.CurrentCategory, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CBC RID: 11452 RVA: 0x0013FD3C File Offset: 0x0013DF3C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFeedbackCategoryDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
			info.AddProperty(NFeedbackCategoryDropdown.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NFeedbackCategoryDropdown.PropertyName._categories, Variant.From<string[]>(ref this._categories));
			info.AddProperty(NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex, Variant.From<int>(ref this._currentCategoryIndex));
		}

		/// <inheritdoc />
		// Token: 0x06002CBD RID: 11453 RVA: 0x0013FDA8 File Offset: 0x0013DFA8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._dropdownItemScene, ref variant))
			{
				this._dropdownItemScene = variant.As<PackedScene>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._selectionReticle, ref variant2))
			{
				this._selectionReticle = variant2.As<NSelectionReticle>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._categories, ref variant3))
			{
				this._categories = variant3.As<string[]>();
			}
			Variant variant4;
			if (info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex, ref variant4))
			{
				this._currentCategoryIndex = variant4.As<int>();
			}
		}

		// Token: 0x040010FE RID: 4350
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _dropdownItemScene;

		// Token: 0x040010FF RID: 4351
		private NSelectionReticle _selectionReticle;

		// Token: 0x04001100 RID: 4352
		private static readonly string[] _baseCategories = new string[] { "bug", "balance", "feedback" };

		// Token: 0x04001101 RID: 4353
		private static readonly LocString[] _baseCategoryLoc = new LocString[]
		{
			new LocString("settings_ui", "FEEDBACK_CATEGORY.bug"),
			new LocString("settings_ui", "FEEDBACK_CATEGORY.balance"),
			new LocString("settings_ui", "FEEDBACK_CATEGORY.feedback")
		};

		// Token: 0x04001102 RID: 4354
		private string[] _categories = Array.Empty<string>();

		// Token: 0x04001103 RID: 4355
		private LocString[] _categoryLoc = Array.Empty<LocString>();

		// Token: 0x04001104 RID: 4356
		private int _currentCategoryIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200156B RID: 5483
		[NullableContext(0)]
		public new class MethodName : NDropdown.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005220 RID: 21024
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005221 RID: 21025
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005222 RID: 21026
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'PopulateOptions' method.
			/// </summary>
			// Token: 0x04005223 RID: 21027
			public static readonly StringName PopulateOptions = "PopulateOptions";

			/// <summary>
			/// Cached name for the 'OnDropdownItemSelected' method.
			/// </summary>
			// Token: 0x04005224 RID: 21028
			public static readonly StringName OnDropdownItemSelected = "OnDropdownItemSelected";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200156C RID: 5484
		[NullableContext(0)]
		public new class PropertyName : NDropdown.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CurrentCategory' property.
			/// </summary>
			// Token: 0x04005225 RID: 21029
			public static readonly StringName CurrentCategory = "CurrentCategory";

			/// <summary>
			/// Cached name for the '_dropdownItemScene' field.
			/// </summary>
			// Token: 0x04005226 RID: 21030
			public static readonly StringName _dropdownItemScene = "_dropdownItemScene";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005227 RID: 21031
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_categories' field.
			/// </summary>
			// Token: 0x04005228 RID: 21032
			public static readonly StringName _categories = "_categories";

			/// <summary>
			/// Cached name for the '_currentCategoryIndex' field.
			/// </summary>
			// Token: 0x04005229 RID: 21033
			public static readonly StringName _currentCategoryIndex = "_currentCategoryIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200156D RID: 5485
		[NullableContext(0)]
		public new class SignalName : NDropdown.SignalName
		{
		}
	}
}
