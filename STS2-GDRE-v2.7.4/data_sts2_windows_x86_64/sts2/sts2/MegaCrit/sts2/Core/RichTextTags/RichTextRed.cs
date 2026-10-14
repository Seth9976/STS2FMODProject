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
	// Token: 0x0200013F RID: 319
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextRed.cs")]
	public class RichTextRed : AbstractMegaRichTextEffect
	{
		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0005EC8F File Offset: 0x0005CE8F
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0005EC97 File Offset: 0x0005CE97
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			charFx.Color = StsColors.red;
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F58 RID: 3928 RVA: 0x0005ECA8 File Offset: 0x0005CEA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextRed.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F59 RID: 3929 RVA: 0x0005ED1C File Offset: 0x0005CF1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextRed.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F5A RID: 3930 RVA: 0x0005ED6C File Offset: 0x0005CF6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextRed.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F5B RID: 3931 RVA: 0x0005ED84 File Offset: 0x0005CF84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextRed.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F5C RID: 3932 RVA: 0x0005EDAC File Offset: 0x0005CFAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextRed.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextRed.PropertyName.bbcode)
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
		// Token: 0x06000F5D RID: 3933 RVA: 0x0005EE04 File Offset: 0x0005D004
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextRed.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextRed.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F5E RID: 3934 RVA: 0x0005EE58 File Offset: 0x0005D058
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextRed.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F5F RID: 3935 RVA: 0x0005EE78 File Offset: 0x0005D078
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextRed.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x04000576 RID: 1398
		[Nullable(1)]
		public new string bbcode = "red";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F1F RID: 3871
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C23 RID: 15395
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F20 RID: 3872
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C24 RID: 15396
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C25 RID: 15397
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F21 RID: 3873
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
