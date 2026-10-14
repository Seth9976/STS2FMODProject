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
	// Token: 0x0200013C RID: 316
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextOrange.cs")]
	public class RichTextOrange : AbstractMegaRichTextEffect
	{
		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0005E60C File Offset: 0x0005C80C
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0005E614 File Offset: 0x0005C814
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			charFx.Color = StsColors.orange;
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F37 RID: 3895 RVA: 0x0005E624 File Offset: 0x0005C824
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextOrange.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F38 RID: 3896 RVA: 0x0005E698 File Offset: 0x0005C898
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextOrange.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F39 RID: 3897 RVA: 0x0005E6E8 File Offset: 0x0005C8E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextOrange.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F3A RID: 3898 RVA: 0x0005E700 File Offset: 0x0005C900
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextOrange.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F3B RID: 3899 RVA: 0x0005E728 File Offset: 0x0005C928
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextOrange.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextOrange.PropertyName.bbcode)
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
		// Token: 0x06000F3C RID: 3900 RVA: 0x0005E780 File Offset: 0x0005C980
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextOrange.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextOrange.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F3D RID: 3901 RVA: 0x0005E7D4 File Offset: 0x0005C9D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextOrange.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F3E RID: 3902 RVA: 0x0005E7F4 File Offset: 0x0005C9F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextOrange.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x04000573 RID: 1395
		[Nullable(1)]
		public new string bbcode = "orange";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F16 RID: 3862
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C1A RID: 15386
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F17 RID: 3863
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C1B RID: 15387
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C1C RID: 15388
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F18 RID: 3864
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
