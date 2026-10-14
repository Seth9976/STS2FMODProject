using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003B4 RID: 948
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Ftue/NRestSiteFtue.cs")]
	public class NRestSiteFtue : NFtue
	{
		// Token: 0x0600390A RID: 14602 RVA: 0x001AF7B8 File Offset: 0x001AD9B8
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "REST_SITE_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "REST_SITE_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			this._defaultZIndex = this._restSiteOptionContainer.ZIndex;
			Control restSiteOptionContainer = this._restSiteOptionContainer;
			int zindex = restSiteOptionContainer.ZIndex;
			restSiteOptionContainer.ZIndex = zindex + 1;
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x001AF890 File Offset: 0x001ADA90
		[return: Nullable(2)]
		public static NRestSiteFtue Create(Control restSiteOptionContainer)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRestSiteFtue nrestSiteFtue = PreloadManager.Cache.GetScene(NRestSiteFtue._scenePath).Instantiate<NRestSiteFtue>(0L);
			nrestSiteFtue._restSiteOptionContainer = restSiteOptionContainer;
			return nrestSiteFtue;
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x001AF8C5 File Offset: 0x001ADAC5
		private void CloseFtue(NButton _)
		{
			this._restSiteOptionContainer.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600390D RID: 14605 RVA: 0x001AF8E0 File Offset: 0x001ADAE0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NRestSiteFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "restSiteOptionContainer", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRestSiteFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600390E RID: 14606 RVA: 0x001AF9F0 File Offset: 0x001ADBF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSiteFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteFtue.MethodName.Create && args.Count == 1)
			{
				NRestSiteFtue nrestSiteFtue = NRestSiteFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NRestSiteFtue>(ref nrestSiteFtue);
				return true;
			}
			if ((ref method) == NRestSiteFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x001AFA98 File Offset: 0x001ADC98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSiteFtue.MethodName.Create && args.Count == 1)
			{
				NRestSiteFtue nrestSiteFtue = NRestSiteFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NRestSiteFtue>(ref nrestSiteFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003910 RID: 14608 RVA: 0x001AFAE6 File Offset: 0x001ADCE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRestSiteFtue.MethodName._Ready || (ref method) == NRestSiteFtue.MethodName.Create || (ref method) == NRestSiteFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003911 RID: 14609 RVA: 0x001AFB1C File Offset: 0x001ADD1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRestSiteFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._arrow)
			{
				this._arrow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._restSiteOptionContainer)
			{
				this._restSiteOptionContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003912 RID: 14610 RVA: 0x001AFBD4 File Offset: 0x001ADDD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRestSiteFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._arrow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._arrow);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._defaultZIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._defaultZIndex);
				return true;
			}
			if ((ref name) == NRestSiteFtue.PropertyName._restSiteOptionContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._restSiteOptionContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003913 RID: 14611 RVA: 0x001AFCAC File Offset: 0x001ADEAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRestSiteFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteFtue.PropertyName._arrow, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRestSiteFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteFtue.PropertyName._restSiteOptionContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003914 RID: 14612 RVA: 0x001AFD88 File Offset: 0x001ADF88
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRestSiteFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NRestSiteFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NRestSiteFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NRestSiteFtue.PropertyName._arrow, Variant.From<Control>(ref this._arrow));
			info.AddProperty(NRestSiteFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
			info.AddProperty(NRestSiteFtue.PropertyName._restSiteOptionContainer, Variant.From<Control>(ref this._restSiteOptionContainer));
		}

		/// <inheritdoc />
		// Token: 0x06003915 RID: 14613 RVA: 0x001AFE20 File Offset: 0x001AE020
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRestSiteFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRestSiteFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRestSiteFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRestSiteFtue.PropertyName._arrow, ref variant4))
			{
				this._arrow = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRestSiteFtue.PropertyName._defaultZIndex, ref variant5))
			{
				this._defaultZIndex = variant5.As<int>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRestSiteFtue.PropertyName._restSiteOptionContainer, ref variant6))
			{
				this._restSiteOptionContainer = variant6.As<Control>();
			}
		}

		// Token: 0x04001627 RID: 5671
		public const string id = "rest_site_ftue";

		// Token: 0x04001628 RID: 5672
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/rest_site_ftue");

		// Token: 0x04001629 RID: 5673
		private NButton _confirmButton;

		// Token: 0x0400162A RID: 5674
		private MegaLabel _header;

		// Token: 0x0400162B RID: 5675
		private MegaRichTextLabel _description;

		// Token: 0x0400162C RID: 5676
		private Control _arrow;

		// Token: 0x0400162D RID: 5677
		private int _defaultZIndex;

		// Token: 0x0400162E RID: 5678
		private Control _restSiteOptionContainer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B8 RID: 6072
		[NullableContext(0)]
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C61 RID: 23649
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C62 RID: 23650
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C63 RID: 23651
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B9 RID: 6073
		[NullableContext(0)]
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C64 RID: 23652
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C65 RID: 23653
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C66 RID: 23654
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_arrow' field.
			/// </summary>
			// Token: 0x04005C67 RID: 23655
			public static readonly StringName _arrow = "_arrow";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C68 RID: 23656
			public static readonly StringName _defaultZIndex = "_defaultZIndex";

			/// <summary>
			/// Cached name for the '_restSiteOptionContainer' field.
			/// </summary>
			// Token: 0x04005C69 RID: 23657
			public static readonly StringName _restSiteOptionContainer = "_restSiteOptionContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017BA RID: 6074
		[NullableContext(0)]
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
