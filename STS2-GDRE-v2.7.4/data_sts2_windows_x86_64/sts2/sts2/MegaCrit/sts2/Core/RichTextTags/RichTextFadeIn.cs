using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.RichTextTags
{
	// Token: 0x02000137 RID: 311
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextFadeIn.cs")]
	public class RichTextFadeIn : AbstractMegaRichTextEffect
	{
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x0005D713 File Offset: 0x0005B913
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0005D71C File Offset: 0x0005B91C
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			Dictionary env = charFx.Env;
			charFx.Offset = Vector2.Zero;
			Variant variant;
			double num;
			if (env.TryGetValue(RichTextFadeIn._speedKey, ref variant))
			{
				num = variant.AsDouble();
			}
			else
			{
				num = 4.0;
			}
			Variant variant2;
			double num2;
			if (env.TryGetValue(RichTextFadeIn._tickKey, ref variant2))
			{
				num2 = variant2.AsDouble();
			}
			else
			{
				num2 = 0.009999999776482582;
			}
			double num3 = charFx.ElapsedTime * num - (double)charFx.RelativeIndex * num2;
			Color color = charFx.Color;
			color.A = Mathf.Clamp((float)num3, 0f, 1f);
			charFx.Color = color;
			Variant variant3;
			if (env.TryGetValue(RichTextUtil.visibleKey, ref variant3))
			{
				charFx.Visible = variant3.AsBool();
			}
			else
			{
				charFx.Visible = true;
			}
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000EFE RID: 3838 RVA: 0x0005D7E4 File Offset: 0x0005B9E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextFadeIn.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000EFF RID: 3839 RVA: 0x0005D858 File Offset: 0x0005BA58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextFadeIn.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F00 RID: 3840 RVA: 0x0005D8A8 File Offset: 0x0005BAA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextFadeIn.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F01 RID: 3841 RVA: 0x0005D8C0 File Offset: 0x0005BAC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextFadeIn.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F02 RID: 3842 RVA: 0x0005D8E8 File Offset: 0x0005BAE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextFadeIn.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextFadeIn.PropertyName.bbcode)
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
		// Token: 0x06000F03 RID: 3843 RVA: 0x0005D940 File Offset: 0x0005BB40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextFadeIn.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextFadeIn.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F04 RID: 3844 RVA: 0x0005D994 File Offset: 0x0005BB94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextFadeIn.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F05 RID: 3845 RVA: 0x0005D9B4 File Offset: 0x0005BBB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextFadeIn.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0005D9F8 File Offset: 0x0005BBF8
		// Note: this type is marked as 'beforefieldinit'.
		static RichTextFadeIn()
		{
			string text = "speed";
			RichTextFadeIn._speedKey = Variant.From<string>(ref text);
			text = "tick";
			RichTextFadeIn._tickKey = Variant.From<string>(ref text);
		}

		// Token: 0x04000567 RID: 1383
		private static readonly Variant _speedKey;

		// Token: 0x04000568 RID: 1384
		private static readonly Variant _tickKey;

		// Token: 0x04000569 RID: 1385
		[Nullable(1)]
		public new string bbcode = "fade_in";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F07 RID: 3847
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C0A RID: 15370
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F08 RID: 3848
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C0B RID: 15371
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C0C RID: 15372
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F09 RID: 3849
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
