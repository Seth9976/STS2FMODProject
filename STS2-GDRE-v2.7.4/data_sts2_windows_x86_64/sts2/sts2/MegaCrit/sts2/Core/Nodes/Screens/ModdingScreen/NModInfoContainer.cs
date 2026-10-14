using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen
{
	// Token: 0x020002E6 RID: 742
	[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NModInfoContainer.cs")]
	public class NModInfoContainer : Control
	{
		// Token: 0x060027D7 RID: 10199 RVA: 0x0011083C File Offset: 0x0010EA3C
		public override void _Ready()
		{
			this._title = base.GetNode<MegaRichTextLabel>("ModTitle");
			this._image = base.GetNode<TextureRect>("ModImage");
			this._description = base.GetNode<MegaRichTextLabel>("ModDescription");
			this._title.Text = "";
			this._image.Texture = null;
			this._description.Text = "";
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x001108B7 File Offset: 0x0010EAB7
		public void Clear()
		{
			this._image.Texture = null;
			this._title.Text = "";
			this._description.Text = "";
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x001108E8 File Offset: 0x0010EAE8
		[NullableContext(1)]
		public void Fill(Mod mod)
		{
			MegaRichTextLabel title = this._title;
			ModManifest manifest = mod.manifest;
			title.Text = ((manifest != null) ? manifest.name : null) ?? "<No Name>";
			string text = "res://";
			ModManifest manifest2 = mod.manifest;
			string text2 = text + ((manifest2 != null) ? manifest2.id : null) + "/mod_image.png";
			if (ResourceLoader.Exists(text2, ""))
			{
				this._image.Texture = PreloadManager.Cache.GetAsset<Texture2D>(text2);
			}
			else
			{
				this._image.Texture = null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("[gold]Author[/gold]: ");
			ModManifest manifest3 = mod.manifest;
			appendInterpolatedStringHandler.AppendFormatted(((manifest3 != null) ? manifest3.author : null) ?? "unknown");
			stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder4 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(22, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("[gold]Version[/gold]: ");
			ModManifest manifest4 = mod.manifest;
			appendInterpolatedStringHandler.AppendFormatted(((manifest4 != null) ? manifest4.version : null) ?? "unknown");
			stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder.AppendLine();
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder5 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
			ModManifest manifest5 = mod.manifest;
			appendInterpolatedStringHandler.AppendFormatted(((manifest5 != null) ? manifest5.description : null) ?? "No description");
			stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			List<LocString> errors = mod.errors;
			if (errors != null && errors.Count > 0)
			{
				stringBuilder.AppendLine();
				foreach (LocString locString in mod.errors)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder6 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("[red]");
					appendInterpolatedStringHandler.AppendFormatted(locString.GetFormattedText());
					appendInterpolatedStringHandler.AppendLiteral("[/red]");
					stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			this._description.Text = stringBuilder.ToString();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027DA RID: 10202 RVA: 0x00110AE8 File Offset: 0x0010ECE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NModInfoContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModInfoContainer.MethodName.Clear, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027DB RID: 10203 RVA: 0x00110B5C File Offset: 0x0010ED5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NModInfoContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModInfoContainer.MethodName.Clear && args.Count == 0)
			{
				this.Clear();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060027DC RID: 10204 RVA: 0x00110BBC File Offset: 0x0010EDBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NModInfoContainer.MethodName._Ready || (ref method) == NModInfoContainer.MethodName.Clear || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060027DD RID: 10205 RVA: 0x00110BE4 File Offset: 0x0010EDE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NModInfoContainer.PropertyName._title)
			{
				this._title = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NModInfoContainer.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NModInfoContainer.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060027DE RID: 10206 RVA: 0x00110C4C File Offset: 0x0010EE4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NModInfoContainer.PropertyName._title)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._title);
				return true;
			}
			if ((ref name) == NModInfoContainer.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NModInfoContainer.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027DF RID: 10207 RVA: 0x00110CC4 File Offset: 0x0010EEC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NModInfoContainer.PropertyName._title, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModInfoContainer.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModInfoContainer.PropertyName._description, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027E0 RID: 10208 RVA: 0x00110D3C File Offset: 0x0010EF3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NModInfoContainer.PropertyName._title, Variant.From<MegaRichTextLabel>(ref this._title));
			info.AddProperty(NModInfoContainer.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NModInfoContainer.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
		}

		/// <inheritdoc />
		// Token: 0x060027E1 RID: 10209 RVA: 0x00110D94 File Offset: 0x0010EF94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NModInfoContainer.PropertyName._title, ref variant))
			{
				this._title = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NModInfoContainer.PropertyName._image, ref variant2))
			{
				this._image = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NModInfoContainer.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x04000ECB RID: 3787
		[Nullable(1)]
		private MegaRichTextLabel _title;

		// Token: 0x04000ECC RID: 3788
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000ECD RID: 3789
		[Nullable(1)]
		private MegaRichTextLabel _description;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001487 RID: 5255
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004DE6 RID: 19942
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Clear' method.
			/// </summary>
			// Token: 0x04004DE7 RID: 19943
			public static readonly StringName Clear = "Clear";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001488 RID: 5256
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_title' field.
			/// </summary>
			// Token: 0x04004DE8 RID: 19944
			public static readonly StringName _title = "_title";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004DE9 RID: 19945
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04004DEA RID: 19946
			public static readonly StringName _description = "_description";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001489 RID: 5257
		public class SignalName : Control.SignalName
		{
		}
	}
}
