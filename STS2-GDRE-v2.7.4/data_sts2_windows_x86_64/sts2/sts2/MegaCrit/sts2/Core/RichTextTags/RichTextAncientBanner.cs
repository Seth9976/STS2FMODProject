using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.RichTextTags
{
	// Token: 0x02000134 RID: 308
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextAncientBanner.cs")]
	public class RichTextAncientBanner : AbstractMegaRichTextEffect
	{
		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0005CDAA File Offset: 0x0005AFAA
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x0005CDB2 File Offset: 0x0005AFB2
		public float Rotation { get; set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0005CDBB File Offset: 0x0005AFBB
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x0005CDC3 File Offset: 0x0005AFC3
		public float Spacing { get; set; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0005CDCC File Offset: 0x0005AFCC
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0005CDD4 File Offset: 0x0005AFD4
		public float CenterCharacter { get; set; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0005CDDD File Offset: 0x0005AFDD
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0005CDE8 File Offset: 0x0005AFE8
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			if (base.ShouldTransformText())
			{
				float num = (float)charFx.RelativeIndex + 0.5f - this.CenterCharacter;
				Transform2D transform2D = charFx.Transform;
				Vector2 vector = charFx.Transform.X;
				vector.X = this.Rotation;
				transform2D.X = vector;
				charFx.Transform = transform2D;
				transform2D = charFx.Transform;
				vector = charFx.Transform.Origin;
				vector.X = charFx.Transform.Origin.X + num * this.Spacing;
				transform2D.Origin = vector;
				charFx.Transform = transform2D;
			}
			else
			{
				double num2 = charFx.ElapsedTime * 3.0 - (double)((float)charFx.RelativeIndex * 0.015f);
				Color color = charFx.Color;
				color.A = Mathf.Clamp((float)num2, 0f, 1f);
				charFx.Color = color;
			}
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000EDD RID: 3805 RVA: 0x0005CED4 File Offset: 0x0005B0D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextAncientBanner.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000EDE RID: 3806 RVA: 0x0005CF48 File Offset: 0x0005B148
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextAncientBanner.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000EDF RID: 3807 RVA: 0x0005CF98 File Offset: 0x0005B198
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextAncientBanner.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000EE0 RID: 3808 RVA: 0x0005CFB0 File Offset: 0x0005B1B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextAncientBanner.PropertyName.Rotation)
			{
				this.Rotation = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == RichTextAncientBanner.PropertyName.Spacing)
			{
				this.Spacing = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == RichTextAncientBanner.PropertyName.CenterCharacter)
			{
				this.CenterCharacter = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == RichTextAncientBanner.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000EE1 RID: 3809 RVA: 0x0005D034 File Offset: 0x0005B234
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextAncientBanner.PropertyName.Rotation)
			{
				float num = this.Rotation;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == RichTextAncientBanner.PropertyName.Spacing)
			{
				float num = this.Spacing;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == RichTextAncientBanner.PropertyName.CenterCharacter)
			{
				float num = this.CenterCharacter;
				value = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			if ((ref name) == RichTextAncientBanner.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextAncientBanner.PropertyName.bbcode)
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
		// Token: 0x06000EE2 RID: 3810 RVA: 0x0005D0F8 File Offset: 0x0005B2F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, RichTextAncientBanner.PropertyName.Rotation, 0L, "", 4096L, false),
				new PropertyInfo(3L, RichTextAncientBanner.PropertyName.Spacing, 0L, "", 4096L, false),
				new PropertyInfo(3L, RichTextAncientBanner.PropertyName.CenterCharacter, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextAncientBanner.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextAncientBanner.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000EE3 RID: 3811 RVA: 0x0005D1AC File Offset: 0x0005B3AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName rotation = RichTextAncientBanner.PropertyName.Rotation;
			float num = this.Rotation;
			info.AddProperty(rotation, Variant.From<float>(ref num));
			StringName spacing = RichTextAncientBanner.PropertyName.Spacing;
			num = this.Spacing;
			info.AddProperty(spacing, Variant.From<float>(ref num));
			StringName centerCharacter = RichTextAncientBanner.PropertyName.CenterCharacter;
			num = this.CenterCharacter;
			info.AddProperty(centerCharacter, Variant.From<float>(ref num));
			info.AddProperty(RichTextAncientBanner.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000EE4 RID: 3812 RVA: 0x0005D224 File Offset: 0x0005B424
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextAncientBanner.PropertyName.Rotation, ref variant))
			{
				this.Rotation = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(RichTextAncientBanner.PropertyName.Spacing, ref variant2))
			{
				this.Spacing = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(RichTextAncientBanner.PropertyName.CenterCharacter, ref variant3))
			{
				this.CenterCharacter = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(RichTextAncientBanner.PropertyName.bbcode, ref variant4))
			{
				this.bbcode = variant4.As<string>();
			}
		}

		// Token: 0x04000564 RID: 1380
		[Nullable(1)]
		public new string bbcode = "ancient_banner";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000EFE RID: 3838
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003BFE RID: 15358
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000EFF RID: 3839
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Rotation' property.
			/// </summary>
			// Token: 0x04003BFF RID: 15359
			public static readonly StringName Rotation = "Rotation";

			/// <summary>
			/// Cached name for the 'Spacing' property.
			/// </summary>
			// Token: 0x04003C00 RID: 15360
			public static readonly StringName Spacing = "Spacing";

			/// <summary>
			/// Cached name for the 'CenterCharacter' property.
			/// </summary>
			// Token: 0x04003C01 RID: 15361
			public static readonly StringName CenterCharacter = "CenterCharacter";

			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C02 RID: 15362
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C03 RID: 15363
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F00 RID: 3840
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
