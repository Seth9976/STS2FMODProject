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
	// Token: 0x02000142 RID: 322
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextThinkyDots.cs")]
	public class RichTextThinkyDots : AbstractMegaRichTextEffect
	{
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0005F4D7 File Offset: 0x0005D6D7
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0005F4E0 File Offset: 0x0005D6E0
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			if (!base.ShouldTransformText())
			{
				return false;
			}
			Dictionary env = charFx.Env;
			charFx.Offset = Vector2.Zero;
			float num = (float)(charFx.ElapsedTime * 1.0 - (double)((float)charFx.RelativeIndex * 0.1f));
			num = Math.Max(num, 0f);
			float num2 = num % 4.4f;
			float num3;
			if (num2 < 0.4f)
			{
				num3 = 1.5f * Mathf.Sin(num2 / 0.4f * 3.1415927f);
			}
			else
			{
				num3 = 0f;
			}
			charFx.Offset += new Vector2(0f, -Mathf.Max(num3, 0f));
			Variant variant;
			if (env.TryGetValue(RichTextUtil.colorKey, ref variant))
			{
				charFx.Color = (Color)variant;
			}
			charFx.Visible = !env.ContainsKey(RichTextUtil.visibleKey) || (bool)env[RichTextUtil.visibleKey];
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F79 RID: 3961 RVA: 0x0005F5D4 File Offset: 0x0005D7D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextThinkyDots.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F7A RID: 3962 RVA: 0x0005F648 File Offset: 0x0005D848
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextThinkyDots.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F7B RID: 3963 RVA: 0x0005F698 File Offset: 0x0005D898
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextThinkyDots.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F7C RID: 3964 RVA: 0x0005F6B0 File Offset: 0x0005D8B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextThinkyDots.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F7D RID: 3965 RVA: 0x0005F6D8 File Offset: 0x0005D8D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextThinkyDots.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextThinkyDots.PropertyName.bbcode)
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
		// Token: 0x06000F7E RID: 3966 RVA: 0x0005F730 File Offset: 0x0005D930
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextThinkyDots.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextThinkyDots.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F7F RID: 3967 RVA: 0x0005F784 File Offset: 0x0005D984
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextThinkyDots.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F80 RID: 3968 RVA: 0x0005F7A4 File Offset: 0x0005D9A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextThinkyDots.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x0400057D RID: 1405
		private const float _amplitude = 1.5f;

		// Token: 0x0400057E RID: 1406
		private const float _frequency = 0.4f;

		// Token: 0x0400057F RID: 1407
		private const float _speed = 1f;

		// Token: 0x04000580 RID: 1408
		private const float _spacing = 4f;

		// Token: 0x04000581 RID: 1409
		[Nullable(1)]
		public new string bbcode = "thinky_dots";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F28 RID: 3880
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C2C RID: 15404
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F29 RID: 3881
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C2D RID: 15405
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C2E RID: 15406
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F2A RID: 3882
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
