using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Represents a single item within a DropdownList.
	/// Must be extended, see NLanguageDropdownItem for an example.
	/// </summary>
	// Token: 0x020003DB RID: 987
	[ScriptPath("res://src/Core/Nodes/CommonUi/NDropdownItem.cs")]
	public class NDropdownItem : NButton
	{
		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x001CAE1B File Offset: 0x001C901B
		// (set) Token: 0x06003BEC RID: 15340 RVA: 0x001CAE28 File Offset: 0x001C9028
		[Nullable(1)]
		public string Text
		{
			[NullableContext(1)]
			get
			{
				return this._label.Text;
			}
			[NullableContext(1)]
			set
			{
				this._label.SetTextAutoSize(value);
			}
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x001CAE38 File Offset: 0x001C9038
		public override void _Ready()
		{
			this.ConnectSignals();
			this._highlight = base.GetNode<ColorRect>("Highlight");
			this._label = base.GetNodeOrNull<MegaLabel>("Label");
			this._richLabel = base.GetNodeOrNull<MegaRichTextLabel>("RichLabel");
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x001CAE8D File Offset: 0x001C908D
		protected override void OnFocus()
		{
			this._highlight.Visible = true;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x001CAE9B File Offset: 0x001C909B
		protected override void OnUnfocus()
		{
			this._highlight.Visible = false;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x001CAEA9 File Offset: 0x001C90A9
		protected override void OnPress()
		{
			this._highlight.Visible = false;
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x001CAEB7 File Offset: 0x001C90B7
		protected sealed override void OnRelease()
		{
			this._highlight.Visible = true;
			base.EmitSignal(NDropdownItem.SignalName.Selected, new Variant[] { this });
		}

		/// <summary>
		/// Helper class to access protected method OnUnhover()
		/// </summary>
		// Token: 0x06003BF2 RID: 15346 RVA: 0x001CAEE4 File Offset: 0x001C90E4
		public void UnhoverSelection()
		{
			this.OnUnfocus();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003BF3 RID: 15347 RVA: 0x001CAEEC File Offset: 0x001C90EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NDropdownItem.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownItem.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownItem.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownItem.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownItem.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownItem.MethodName.UnhoverSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BF4 RID: 15348 RVA: 0x001CB01C File Offset: 0x001C921C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDropdownItem.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownItem.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownItem.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownItem.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownItem.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownItem.MethodName.UnhoverSelection && args.Count == 0)
			{
				this.UnhoverSelection();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003BF5 RID: 15349 RVA: 0x001CB110 File Offset: 0x001C9310
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDropdownItem.MethodName._Ready || (ref method) == NDropdownItem.MethodName.OnFocus || (ref method) == NDropdownItem.MethodName.OnUnfocus || (ref method) == NDropdownItem.MethodName.OnPress || (ref method) == NDropdownItem.MethodName.OnRelease || (ref method) == NDropdownItem.MethodName.UnhoverSelection || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003BF6 RID: 15350 RVA: 0x001CB180 File Offset: 0x001C9380
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDropdownItem.PropertyName.Text)
			{
				this.Text = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NDropdownItem.PropertyName._highlight)
			{
				this._highlight = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NDropdownItem.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDropdownItem.PropertyName._richLabel)
			{
				this._richLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003BF7 RID: 15351 RVA: 0x001CB204 File Offset: 0x001C9404
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDropdownItem.PropertyName.Text)
			{
				string text = this.Text;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NDropdownItem.PropertyName._highlight)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._highlight);
				return true;
			}
			if ((ref name) == NDropdownItem.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NDropdownItem.PropertyName._richLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._richLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003BF8 RID: 15352 RVA: 0x001CB29C File Offset: 0x001C949C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDropdownItem.PropertyName._highlight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdownItem.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdownItem.PropertyName._richLabel, 0L, "", 4096L, false),
				new PropertyInfo(4L, NDropdownItem.PropertyName.Text, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BF9 RID: 15353 RVA: 0x001CB334 File Offset: 0x001C9534
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName text = NDropdownItem.PropertyName.Text;
			string text2 = this.Text;
			info.AddProperty(text, Variant.From<string>(ref text2));
			info.AddProperty(NDropdownItem.PropertyName._highlight, Variant.From<ColorRect>(ref this._highlight));
			info.AddProperty(NDropdownItem.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NDropdownItem.PropertyName._richLabel, Variant.From<MegaRichTextLabel>(ref this._richLabel));
			info.AddSignalEventDelegate(NDropdownItem.SignalName.Selected, this.backing_Selected);
		}

		/// <inheritdoc />
		// Token: 0x06003BFA RID: 15354 RVA: 0x001CB3B4 File Offset: 0x001C95B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDropdownItem.PropertyName.Text, ref variant))
			{
				this.Text = variant.As<string>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDropdownItem.PropertyName._highlight, ref variant2))
			{
				this._highlight = variant2.As<ColorRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDropdownItem.PropertyName._label, ref variant3))
			{
				this._label = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDropdownItem.PropertyName._richLabel, ref variant4))
			{
				this._richLabel = variant4.As<MegaRichTextLabel>();
			}
			NDropdownItem.SelectedEventHandler selectedEventHandler;
			if (info.TryGetSignalEventDelegate<NDropdownItem.SelectedEventHandler>(NDropdownItem.SignalName.Selected, ref selectedEventHandler))
			{
				this.backing_Selected = selectedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003BFB RID: 15355 RVA: 0x001CB450 File Offset: 0x001C9650
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NDropdownItem.SignalName.Selected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NDropdownItem.SelectedEventHandler" />
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06003BFC RID: 15356 RVA: 0x001CB4C4 File Offset: 0x001C96C4
		// (remove) Token: 0x06003BFD RID: 15357 RVA: 0x001CB4DD File Offset: 0x001C96DD
		public event NDropdownItem.SelectedEventHandler Selected
		{
			add
			{
				this.backing_Selected = (NDropdownItem.SelectedEventHandler)Delegate.Combine(this.backing_Selected, value);
			}
			remove
			{
				this.backing_Selected = (NDropdownItem.SelectedEventHandler)Delegate.Remove(this.backing_Selected, value);
			}
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x001CB4F6 File Offset: 0x001C96F6
		protected void EmitSignalSelected(NDropdownItem cardHolder)
		{
			base.EmitSignal(NDropdownItem.SignalName.Selected, new Variant[] { cardHolder });
		}

		/// <inheritdoc />
		// Token: 0x06003BFF RID: 15359 RVA: 0x001CB517 File Offset: 0x001C9717
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NDropdownItem.SignalName.Selected) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NDropdownItem.SelectedEventHandler selectedEventHandler = this.backing_Selected;
			if (selectedEventHandler == null)
			{
				return;
			}
			selectedEventHandler(VariantUtils.ConvertTo<NDropdownItem>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06003C00 RID: 15360 RVA: 0x001CB556 File Offset: 0x001C9756
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NDropdownItem.SignalName.Selected || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x0400176F RID: 5999
		[Nullable(1)]
		private ColorRect _highlight;

		// Token: 0x04001770 RID: 6000
		[Nullable(1)]
		protected MegaLabel _label;

		// Token: 0x04001771 RID: 6001
		[Nullable(2)]
		protected MegaRichTextLabel _richLabel;

		// Token: 0x04001772 RID: 6002
		private NDropdownItem.SelectedEventHandler backing_Selected;

		// Token: 0x0200183F RID: 6207
		// (Invoke) Token: 0x0600A851 RID: 43089
		[Signal]
		public delegate void SelectedEventHandler(NDropdownItem cardHolder);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001840 RID: 6208
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005EF1 RID: 24305
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005EF2 RID: 24306
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005EF3 RID: 24307
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005EF4 RID: 24308
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005EF5 RID: 24309
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'UnhoverSelection' method.
			/// </summary>
			// Token: 0x04005EF6 RID: 24310
			public static readonly StringName UnhoverSelection = "UnhoverSelection";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001841 RID: 6209
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Text' property.
			/// </summary>
			// Token: 0x04005EF7 RID: 24311
			public static readonly StringName Text = "Text";

			/// <summary>
			/// Cached name for the '_highlight' field.
			/// </summary>
			// Token: 0x04005EF8 RID: 24312
			public static readonly StringName _highlight = "_highlight";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005EF9 RID: 24313
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_richLabel' field.
			/// </summary>
			// Token: 0x04005EFA RID: 24314
			public static readonly StringName _richLabel = "_richLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001842 RID: 6210
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'Selected' signal.
			/// </summary>
			// Token: 0x04005EFB RID: 24315
			public static readonly StringName Selected = "Selected";
		}
	}
}
