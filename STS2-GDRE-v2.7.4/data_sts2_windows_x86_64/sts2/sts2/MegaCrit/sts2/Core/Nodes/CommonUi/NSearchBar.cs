using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003EC RID: 1004
	[ScriptPath("res://src/Core/Nodes/CommonUi/NSearchBar.cs")]
	public class NSearchBar : Control
	{
		/// <remarks>
		/// Pattern:<br />
		/// <code>[\\t\\r\\n]</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ Match a character in the set [\t\n\r].<br />
		/// </code>
		/// </remarks>
		// Token: 0x06003D3A RID: 15674 RVA: 0x001D5498 File Offset: 0x001D3698
		[NullableContext(1)]
		[GeneratedRegex("[\\t\\r\\n]")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex NonSpaceWhitespaceCharacters()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5.Instance;
		}

		/// <remarks>
		/// Pattern:<br />
		/// <code>\\s{2,}</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ Match a whitespace character atomically at least twice.<br />
		/// </code>
		/// </remarks>
		// Token: 0x06003D3B RID: 15675 RVA: 0x001D549F File Offset: 0x001D369F
		[NullableContext(1)]
		[GeneratedRegex("\\s{2,}")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex ConsecutiveSpaces()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6.Instance;
		}

		/// <remarks>
		/// Pattern:<br />
		/// <code>&lt;.*?&gt;</code><br />
		/// Explanation:<br />
		/// <code>
		/// ○ Match '&lt;'.<br />
		/// ○ Match a character other than '\n' lazily any number of times.<br />
		/// ○ Match '&gt;'.<br />
		/// </code>
		/// </remarks>
		// Token: 0x06003D3C RID: 15676 RVA: 0x001D54A6 File Offset: 0x001D36A6
		[NullableContext(1)]
		[GeneratedRegex("<.*?>")]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
		private static Regex HtmlTags()
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7.Instance;
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003D3D RID: 15677 RVA: 0x001D54AD File Offset: 0x001D36AD
		[Nullable(1)]
		public string Text
		{
			[NullableContext(1)]
			get
			{
				return this._textArea.Text;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003D3E RID: 15678 RVA: 0x001D54BA File Offset: 0x001D36BA
		[Nullable(1)]
		public LineEdit TextArea
		{
			[NullableContext(1)]
			get
			{
				return this._textArea;
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x001D54C4 File Offset: 0x001D36C4
		public override void _Ready()
		{
			this._textArea = base.GetNode<LineEdit>("TextArea");
			this._textArea.Connect(LineEdit.SignalName.TextChanged, Callable.From<string>(new Action<string>(this.TextUpdated)), 0U);
			this._textArea.Connect(LineEdit.SignalName.TextSubmitted, Callable.From<string>(new Action<string>(this.TextSubmitted)), 0U);
			this._textArea.SetPlaceholder(new LocString("card_library", "SEARCH_PLACEHOLDER").GetRawText());
			this._clearButton = base.GetNode<NButton>("ClearButton");
			this._clearButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ClearText)), 0U);
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x001D5585 File Offset: 0x001D3785
		[NullableContext(1)]
		private void TextUpdated(string _)
		{
			base.EmitSignal(NSearchBar.SignalName.QueryChanged, new Variant[] { this._textArea.Text });
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x001D55B0 File Offset: 0x001D37B0
		[NullableContext(1)]
		private void TextSubmitted(string _)
		{
			base.EmitSignal(NSearchBar.SignalName.QuerySubmitted, new Variant[] { this._textArea.Text });
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x001D55DB File Offset: 0x001D37DB
		[NullableContext(1)]
		private void ClearText(NButton _)
		{
			this.ClearText();
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x001D55E4 File Offset: 0x001D37E4
		public void ClearText()
		{
			this._textArea.TryGrabFocus();
			if (string.IsNullOrWhiteSpace(this._textArea.Text))
			{
				return;
			}
			this._textArea.Text = "";
			base.EmitSignal(NSearchBar.SignalName.QueryChanged, new Variant[] { this._textArea.Text });
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x001D5648 File Offset: 0x001D3848
		[NullableContext(1)]
		public static string Normalize(string text)
		{
			string text2 = NSearchBar.NonSpaceWhitespaceCharacters().Replace(text.Trim(), " ");
			return NSearchBar.ConsecutiveSpaces().Replace(text2, " ").ToLowerInvariant();
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x001D5680 File Offset: 0x001D3880
		[NullableContext(1)]
		public static string RemoveHtmlTags(string text)
		{
			return NSearchBar.HtmlTags().Replace(text, string.Empty);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D46 RID: 15686 RVA: 0x001D5694 File Offset: 0x001D3894
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NSearchBar.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSearchBar.MethodName.TextUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSearchBar.MethodName.TextSubmitted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSearchBar.MethodName.ClearText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSearchBar.MethodName.ClearText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSearchBar.MethodName.Normalize, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSearchBar.MethodName.RemoveHtmlTags, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D47 RID: 15687 RVA: 0x001D58B8 File Offset: 0x001D3AB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSearchBar.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSearchBar.MethodName.TextUpdated && args.Count == 1)
			{
				this.TextUpdated(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSearchBar.MethodName.TextSubmitted && args.Count == 1)
			{
				this.TextSubmitted(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSearchBar.MethodName.ClearText && args.Count == 1)
			{
				this.ClearText(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSearchBar.MethodName.ClearText && args.Count == 0)
			{
				this.ClearText();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSearchBar.MethodName.Normalize && args.Count == 1)
			{
				string text = NSearchBar.Normalize(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NSearchBar.MethodName.RemoveHtmlTags && args.Count == 1)
			{
				string text2 = NSearchBar.RemoveHtmlTags(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text2);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x001D5A24 File Offset: 0x001D3C24
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSearchBar.MethodName.Normalize && args.Count == 1)
			{
				string text = NSearchBar.Normalize(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NSearchBar.MethodName.RemoveHtmlTags && args.Count == 1)
			{
				string text2 = NSearchBar.RemoveHtmlTags(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003D49 RID: 15689 RVA: 0x001D5AAC File Offset: 0x001D3CAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSearchBar.MethodName._Ready || (ref method) == NSearchBar.MethodName.TextUpdated || (ref method) == NSearchBar.MethodName.TextSubmitted || (ref method) == NSearchBar.MethodName.ClearText || (ref method) == NSearchBar.MethodName.Normalize || (ref method) == NSearchBar.MethodName.RemoveHtmlTags || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003D4A RID: 15690 RVA: 0x001D5B1A File Offset: 0x001D3D1A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSearchBar.PropertyName._textArea)
			{
				this._textArea = VariantUtils.ConvertTo<LineEdit>(ref value);
				return true;
			}
			if ((ref name) == NSearchBar.PropertyName._clearButton)
			{
				this._clearButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003D4B RID: 15691 RVA: 0x001D5B5C File Offset: 0x001D3D5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSearchBar.PropertyName.Text)
			{
				string text = this.Text;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NSearchBar.PropertyName.TextArea)
			{
				LineEdit textArea = this.TextArea;
				value = VariantUtils.CreateFrom<LineEdit>(ref textArea);
				return true;
			}
			if ((ref name) == NSearchBar.PropertyName._textArea)
			{
				value = VariantUtils.CreateFrom<LineEdit>(ref this._textArea);
				return true;
			}
			if ((ref name) == NSearchBar.PropertyName._clearButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._clearButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D4C RID: 15692 RVA: 0x001D5BF8 File Offset: 0x001D3DF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSearchBar.PropertyName._textArea, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSearchBar.PropertyName._clearButton, 0L, "", 4096L, false),
				new PropertyInfo(4L, NSearchBar.PropertyName.Text, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSearchBar.PropertyName.TextArea, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D4D RID: 15693 RVA: 0x001D5C90 File Offset: 0x001D3E90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSearchBar.PropertyName._textArea, Variant.From<LineEdit>(ref this._textArea));
			info.AddProperty(NSearchBar.PropertyName._clearButton, Variant.From<NButton>(ref this._clearButton));
			info.AddSignalEventDelegate(NSearchBar.SignalName.QueryChanged, this.backing_QueryChanged);
			info.AddSignalEventDelegate(NSearchBar.SignalName.QuerySubmitted, this.backing_QuerySubmitted);
		}

		/// <inheritdoc />
		// Token: 0x06003D4E RID: 15694 RVA: 0x001D5CF4 File Offset: 0x001D3EF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSearchBar.PropertyName._textArea, ref variant))
			{
				this._textArea = variant.As<LineEdit>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSearchBar.PropertyName._clearButton, ref variant2))
			{
				this._clearButton = variant2.As<NButton>();
			}
			NSearchBar.QueryChangedEventHandler queryChangedEventHandler;
			if (info.TryGetSignalEventDelegate<NSearchBar.QueryChangedEventHandler>(NSearchBar.SignalName.QueryChanged, ref queryChangedEventHandler))
			{
				this.backing_QueryChanged = queryChangedEventHandler;
			}
			NSearchBar.QuerySubmittedEventHandler querySubmittedEventHandler;
			if (info.TryGetSignalEventDelegate<NSearchBar.QuerySubmittedEventHandler>(NSearchBar.SignalName.QuerySubmitted, ref querySubmittedEventHandler))
			{
				this.backing_QuerySubmitted = querySubmittedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D4F RID: 15695 RVA: 0x001D5D6C File Offset: 0x001D3F6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSearchBar.SignalName.QueryChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "query", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSearchBar.SignalName.QuerySubmitted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "query", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NSearchBar.QueryChangedEventHandler" />
		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06003D50 RID: 15696 RVA: 0x001D5E29 File Offset: 0x001D4029
		// (remove) Token: 0x06003D51 RID: 15697 RVA: 0x001D5E42 File Offset: 0x001D4042
		public event NSearchBar.QueryChangedEventHandler QueryChanged
		{
			add
			{
				this.backing_QueryChanged = (NSearchBar.QueryChangedEventHandler)Delegate.Combine(this.backing_QueryChanged, value);
			}
			remove
			{
				this.backing_QueryChanged = (NSearchBar.QueryChangedEventHandler)Delegate.Remove(this.backing_QueryChanged, value);
			}
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x001D5E5B File Offset: 0x001D405B
		protected void EmitSignalQueryChanged(string query)
		{
			base.EmitSignal(NSearchBar.SignalName.QueryChanged, new Variant[] { query });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NSearchBar.QuerySubmittedEventHandler" />
		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06003D53 RID: 15699 RVA: 0x001D5E7C File Offset: 0x001D407C
		// (remove) Token: 0x06003D54 RID: 15700 RVA: 0x001D5E95 File Offset: 0x001D4095
		public event NSearchBar.QuerySubmittedEventHandler QuerySubmitted
		{
			add
			{
				this.backing_QuerySubmitted = (NSearchBar.QuerySubmittedEventHandler)Delegate.Combine(this.backing_QuerySubmitted, value);
			}
			remove
			{
				this.backing_QuerySubmitted = (NSearchBar.QuerySubmittedEventHandler)Delegate.Remove(this.backing_QuerySubmitted, value);
			}
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x001D5EAE File Offset: 0x001D40AE
		protected void EmitSignalQuerySubmitted(string query)
		{
			base.EmitSignal(NSearchBar.SignalName.QuerySubmitted, new Variant[] { query });
		}

		/// <inheritdoc />
		// Token: 0x06003D56 RID: 15702 RVA: 0x001D5ED0 File Offset: 0x001D40D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NSearchBar.SignalName.QueryChanged && args.Count == 1)
			{
				NSearchBar.QueryChangedEventHandler queryChangedEventHandler = this.backing_QueryChanged;
				if (queryChangedEventHandler == null)
				{
					return;
				}
				queryChangedEventHandler(VariantUtils.ConvertTo<string>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NSearchBar.SignalName.QuerySubmitted) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NSearchBar.QuerySubmittedEventHandler querySubmittedEventHandler = this.backing_QuerySubmitted;
				if (querySubmittedEventHandler == null)
				{
					return;
				}
				querySubmittedEventHandler(VariantUtils.ConvertTo<string>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06003D57 RID: 15703 RVA: 0x001D5F4F File Offset: 0x001D414F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NSearchBar.SignalName.QueryChanged || (ref signal) == NSearchBar.SignalName.QuerySubmitted || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040017E2 RID: 6114
		[Nullable(1)]
		private LineEdit _textArea;

		// Token: 0x040017E3 RID: 6115
		[Nullable(1)]
		private NButton _clearButton;

		// Token: 0x040017E4 RID: 6116
		private NSearchBar.QueryChangedEventHandler backing_QueryChanged;

		// Token: 0x040017E5 RID: 6117
		private NSearchBar.QuerySubmittedEventHandler backing_QuerySubmitted;

		// Token: 0x02001884 RID: 6276
		// (Invoke) Token: 0x0600A8DB RID: 43227
		[Signal]
		public delegate void QueryChangedEventHandler(string query);

		// Token: 0x02001885 RID: 6277
		// (Invoke) Token: 0x0600A8DF RID: 43231
		[Signal]
		public delegate void QuerySubmittedEventHandler(string query);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001886 RID: 6278
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005FD6 RID: 24534
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'TextUpdated' method.
			/// </summary>
			// Token: 0x04005FD7 RID: 24535
			public static readonly StringName TextUpdated = "TextUpdated";

			/// <summary>
			/// Cached name for the 'TextSubmitted' method.
			/// </summary>
			// Token: 0x04005FD8 RID: 24536
			public static readonly StringName TextSubmitted = "TextSubmitted";

			/// <summary>
			/// Cached name for the 'ClearText' method.
			/// </summary>
			// Token: 0x04005FD9 RID: 24537
			public static readonly StringName ClearText = "ClearText";

			/// <summary>
			/// Cached name for the 'Normalize' method.
			/// </summary>
			// Token: 0x04005FDA RID: 24538
			public static readonly StringName Normalize = "Normalize";

			/// <summary>
			/// Cached name for the 'RemoveHtmlTags' method.
			/// </summary>
			// Token: 0x04005FDB RID: 24539
			public static readonly StringName RemoveHtmlTags = "RemoveHtmlTags";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001887 RID: 6279
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Text' property.
			/// </summary>
			// Token: 0x04005FDC RID: 24540
			public static readonly StringName Text = "Text";

			/// <summary>
			/// Cached name for the 'TextArea' property.
			/// </summary>
			// Token: 0x04005FDD RID: 24541
			public static readonly StringName TextArea = "TextArea";

			/// <summary>
			/// Cached name for the '_textArea' field.
			/// </summary>
			// Token: 0x04005FDE RID: 24542
			public static readonly StringName _textArea = "_textArea";

			/// <summary>
			/// Cached name for the '_clearButton' field.
			/// </summary>
			// Token: 0x04005FDF RID: 24543
			public static readonly StringName _clearButton = "_clearButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001888 RID: 6280
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'QueryChanged' signal.
			/// </summary>
			// Token: 0x04005FE0 RID: 24544
			public static readonly StringName QueryChanged = "QueryChanged";

			/// <summary>
			/// Cached name for the 'QuerySubmitted' signal.
			/// </summary>
			// Token: 0x04005FE1 RID: 24545
			public static readonly StringName QuerySubmitted = "QuerySubmitted";
		}
	}
}
