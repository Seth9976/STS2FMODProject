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
	// Token: 0x02000141 RID: 321
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextSine.cs")]
	public class RichTextSine : AbstractMegaRichTextEffect
	{
		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x0005F1FF File Offset: 0x0005D3FF
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0005F208 File Offset: 0x0005D408
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			if (!base.ShouldTransformText())
			{
				return false;
			}
			Dictionary env = charFx.Env;
			float num = (float)(charFx.ElapsedTime * 1.5 + (double)((float)charFx.RelativeIndex * 0.1f));
			float num2 = 0.8f * Mathf.Sin(num * 3.1415927f * 2f * 0.5f);
			charFx.Offset += new Vector2(0f, num2);
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
		// Token: 0x06000F6E RID: 3950 RVA: 0x0005F2C4 File Offset: 0x0005D4C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextSine.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F6F RID: 3951 RVA: 0x0005F338 File Offset: 0x0005D538
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextSine.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F70 RID: 3952 RVA: 0x0005F388 File Offset: 0x0005D588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextSine.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F71 RID: 3953 RVA: 0x0005F3A0 File Offset: 0x0005D5A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextSine.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F72 RID: 3954 RVA: 0x0005F3C8 File Offset: 0x0005D5C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextSine.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextSine.PropertyName.bbcode)
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
		// Token: 0x06000F73 RID: 3955 RVA: 0x0005F420 File Offset: 0x0005D620
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextSine.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextSine.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F74 RID: 3956 RVA: 0x0005F474 File Offset: 0x0005D674
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextSine.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F75 RID: 3957 RVA: 0x0005F494 File Offset: 0x0005D694
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextSine.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x04000579 RID: 1401
		private const float _amplitude = 0.8f;

		// Token: 0x0400057A RID: 1402
		private const float _frequency = 0.5f;

		// Token: 0x0400057B RID: 1403
		private const float _speed = 1.5f;

		// Token: 0x0400057C RID: 1404
		[Nullable(1)]
		public new string bbcode = "sine";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F25 RID: 3877
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C29 RID: 15401
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F26 RID: 3878
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C2A RID: 15402
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C2B RID: 15403
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F27 RID: 3879
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
