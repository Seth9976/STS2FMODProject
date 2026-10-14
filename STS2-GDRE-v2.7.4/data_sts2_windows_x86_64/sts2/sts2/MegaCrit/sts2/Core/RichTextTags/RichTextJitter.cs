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
	// Token: 0x0200013B RID: 315
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextJitter.cs")]
	public class RichTextJitter : AbstractMegaRichTextEffect
	{
		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0005E247 File Offset: 0x0005C447
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0005E250 File Offset: 0x0005C450
		public RichTextJitter()
		{
			this._fastNoise = new FastNoiseLite();
			this._fastNoise.NoiseType = 3L;
			this._fastNoise.FractalOctaves = 8;
			this._fastNoise.FractalGain = 0.8f;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0005E2A4 File Offset: 0x0005C4A4
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			Dictionary env = charFx.Env;
			this._fastNoise.Seed = (charFx.RelativeIndex + 1) * 131;
			float noise1D = this._fastNoise.GetNoise1D((float)charFx.ElapsedTime * 600f);
			this._fastNoise.Seed = (charFx.RelativeIndex + 1) * 737;
			float noise1D2 = this._fastNoise.GetNoise1D((float)charFx.ElapsedTime * 600f);
			charFx.Offset += new Vector2(noise1D, noise1D2) * 3f;
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
		// Token: 0x06000F2D RID: 3885 RVA: 0x0005E380 File Offset: 0x0005C580
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextJitter.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F2E RID: 3886 RVA: 0x0005E3F4 File Offset: 0x0005C5F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextJitter.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F2F RID: 3887 RVA: 0x0005E444 File Offset: 0x0005C644
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextJitter.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F30 RID: 3888 RVA: 0x0005E45C File Offset: 0x0005C65C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextJitter.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == RichTextJitter.PropertyName._fastNoise)
			{
				this._fastNoise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F31 RID: 3889 RVA: 0x0005E49C File Offset: 0x0005C69C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextJitter.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextJitter.PropertyName.bbcode)
			{
				value = VariantUtils.CreateFrom<string>(ref this.bbcode);
				return true;
			}
			if ((ref name) == RichTextJitter.PropertyName._fastNoise)
			{
				value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._fastNoise);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F32 RID: 3890 RVA: 0x0005E514 File Offset: 0x0005C714
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextJitter.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextJitter.PropertyName.Bbcode, 0L, "", 4096L, false),
				new PropertyInfo(24L, RichTextJitter.PropertyName._fastNoise, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F33 RID: 3891 RVA: 0x0005E589 File Offset: 0x0005C789
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextJitter.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
			info.AddProperty(RichTextJitter.PropertyName._fastNoise, Variant.From<FastNoiseLite>(ref this._fastNoise));
		}

		/// <inheritdoc />
		// Token: 0x06000F34 RID: 3892 RVA: 0x0005E5C0 File Offset: 0x0005C7C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextJitter.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
			Variant variant2;
			if (info.TryGetProperty(RichTextJitter.PropertyName._fastNoise, ref variant2))
			{
				this._fastNoise = variant2.As<FastNoiseLite>();
			}
		}

		// Token: 0x0400056F RID: 1391
		private const float _amplitude = 3f;

		// Token: 0x04000570 RID: 1392
		private const float _speed = 600f;

		// Token: 0x04000571 RID: 1393
		[Nullable(1)]
		public new string bbcode = "jitter";

		// Token: 0x04000572 RID: 1394
		[Nullable(1)]
		private FastNoiseLite _fastNoise;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F13 RID: 3859
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C16 RID: 15382
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F14 RID: 3860
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C17 RID: 15383
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C18 RID: 15384
			public new static readonly StringName bbcode = "bbcode";

			/// <summary>
			/// Cached name for the '_fastNoise' field.
			/// </summary>
			// Token: 0x04003C19 RID: 15385
			public static readonly StringName _fastNoise = "_fastNoise";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F15 RID: 3861
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
