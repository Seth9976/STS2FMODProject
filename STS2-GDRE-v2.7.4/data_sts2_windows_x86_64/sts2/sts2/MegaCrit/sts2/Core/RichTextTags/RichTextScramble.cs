using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.RichTextTags
{
	/// <summary>
	/// Scrambles the text to be one of the replacement characters over time.
	/// Must use a monospace font (or have the tag [code]) for the spacing to be correct.
	/// </summary>
	// Token: 0x02000140 RID: 320
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextScramble.cs")]
	public class RichTextScramble : AbstractMegaRichTextEffect
	{
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0005EEBB File Offset: 0x0005D0BB
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0005EEC4 File Offset: 0x0005D0C4
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			if (!base.ShouldTransformText())
			{
				return false;
			}
			Dictionary env = charFx.Env;
			uint num = GlyphConverter.CharToGlyphIdx(charFx.Font, ' ');
			if (charFx.GlyphIndex != num)
			{
				double num2 = charFx.ElapsedTime + charFx.GlyphIndex * 10.2 + (double)(charFx.Range.X * 2);
				num2 *= 4.0;
				Variant variant;
				if (charFx.Env.TryGetValue("speed", ref variant))
				{
					num2 *= (double)variant.AsSingle();
				}
				if (Mathf.Sin(num2) <= 0.0 || !charFx.Env.ContainsKey("showBaseChar"))
				{
					char c = "<!0{541#00>"[(int)num2 % "<!0{541#00>".Length];
					charFx.SetGlyphIndex(GlyphConverter.CharToGlyphIdx(charFx.Font, c));
				}
			}
			Variant variant2;
			if (env.TryGetValue(RichTextUtil.colorKey, ref variant2))
			{
				charFx.Color = (Color)variant2;
			}
			charFx.Visible = !env.ContainsKey(RichTextUtil.visibleKey) || (bool)env[RichTextUtil.visibleKey];
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F63 RID: 3939 RVA: 0x0005EFEC File Offset: 0x0005D1EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextScramble.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F64 RID: 3940 RVA: 0x0005F060 File Offset: 0x0005D260
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextScramble.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F65 RID: 3941 RVA: 0x0005F0B0 File Offset: 0x0005D2B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextScramble.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F66 RID: 3942 RVA: 0x0005F0C8 File Offset: 0x0005D2C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextScramble.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F67 RID: 3943 RVA: 0x0005F0F0 File Offset: 0x0005D2F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextScramble.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextScramble.PropertyName.bbcode)
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
		// Token: 0x06000F68 RID: 3944 RVA: 0x0005F148 File Offset: 0x0005D348
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextScramble.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextScramble.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F69 RID: 3945 RVA: 0x0005F19C File Offset: 0x0005D39C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextScramble.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F6A RID: 3946 RVA: 0x0005F1BC File Offset: 0x0005D3BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextScramble.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x04000577 RID: 1399
		[Nullable(1)]
		public new string bbcode = "scramble";

		// Token: 0x04000578 RID: 1400
		[Nullable(1)]
		private const string _replacementCharacters = "<!0{541#00>";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F22 RID: 3874
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C26 RID: 15398
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F23 RID: 3875
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C27 RID: 15399
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C28 RID: 15400
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F24 RID: 3876
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
