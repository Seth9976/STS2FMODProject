using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.RichTextTags
{
	// Token: 0x0200013A RID: 314
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextGreen.cs")]
	public class RichTextGreen : AbstractMegaRichTextEffect
	{
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0005E01B File Offset: 0x0005C21B
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0005E023 File Offset: 0x0005C223
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			charFx.Color = StsColors.green;
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F21 RID: 3873 RVA: 0x0005E034 File Offset: 0x0005C234
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextGreen.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F22 RID: 3874 RVA: 0x0005E0A8 File Offset: 0x0005C2A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextGreen.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F23 RID: 3875 RVA: 0x0005E0F8 File Offset: 0x0005C2F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextGreen.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F24 RID: 3876 RVA: 0x0005E110 File Offset: 0x0005C310
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextGreen.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F25 RID: 3877 RVA: 0x0005E138 File Offset: 0x0005C338
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextGreen.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextGreen.PropertyName.bbcode)
			{
				value = VariantUtils.CreateFrom<string>(ref this.bbcode);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F26 RID: 3878 RVA: 0x0005E190 File Offset: 0x0005C390
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextGreen.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextGreen.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F27 RID: 3879 RVA: 0x0005E1E4 File Offset: 0x0005C3E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextGreen.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F28 RID: 3880 RVA: 0x0005E204 File Offset: 0x0005C404
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextGreen.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x0400056E RID: 1390
		[Nullable(1)]
		public new string bbcode = "green";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F10 RID: 3856
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C13 RID: 15379
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F11 RID: 3857
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C14 RID: 15380
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C15 RID: 15381
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F12 RID: 3858
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
