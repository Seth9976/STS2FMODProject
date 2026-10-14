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
	// Token: 0x02000138 RID: 312
	[GlobalClass]
	[Tool]
	[ScriptPath("res://src/Core/RichTextTags/RichTextFlyIn.cs")]
	public class RichTextFlyIn : AbstractMegaRichTextEffect
	{
		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x0005DA29 File Offset: 0x0005BC29
		[Nullable(1)]
		protected override string Bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.bbcode;
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0005DA34 File Offset: 0x0005BC34
		[NullableContext(1)]
		public override bool _ProcessCustomFX(CharFXTransform charFx)
		{
			if (Engine.IsEditorHint())
			{
				return false;
			}
			Dictionary env = charFx.Env;
			Vector2 zero = Vector2.Zero;
			Variant variant;
			if (env.TryGetValue(RichTextFlyIn._xOffsetKey, ref variant))
			{
				zero.X = (float)variant.AsDouble();
			}
			Variant variant2;
			if (env.TryGetValue(RichTextFlyIn._yOffsetKey, ref variant2))
			{
				zero.Y = (float)variant2.AsDouble();
			}
			double num = charFx.ElapsedTime * 3.0 - (double)((float)charFx.RelativeIndex * 0.015f);
			Color color = charFx.Color;
			color.A = Mathf.Clamp((float)num, 0f, 1f);
			charFx.Color = color;
			if (base.ShouldTransformText())
			{
				Vector2 vector;
				vector..ctor(charFx.Transform.X.X, charFx.Transform.Y.Y);
				Vector2 vector2 = zero.Lerp(vector, Ease.QuadOut(color.A));
				Vector2 vector3 = vector2 - vector;
				Transform2D transform2D = charFx.Transform;
				charFx.Transform = transform2D.TranslatedLocal(vector3);
				transform2D = charFx.Transform;
				charFx.Transform = transform2D.RotatedLocal(Ease.QuadOut(1f - color.A) * Mathf.DegToRad(20f) * ((vector3.X < 0f) ? 1f : (-1f)));
			}
			return true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000F0A RID: 3850 RVA: 0x0005DB98 File Offset: 0x0005BD98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(RichTextFlyIn.MethodName._ProcessCustomFX, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "charFx", 0L, "", 6L, new StringName("CharFXTransform"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F0B RID: 3851 RVA: 0x0005DC0C File Offset: 0x0005BE0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == RichTextFlyIn.MethodName._ProcessCustomFX && args.Count == 1)
			{
				bool flag = this._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000F0C RID: 3852 RVA: 0x0005DC5C File Offset: 0x0005BE5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == RichTextFlyIn.MethodName._ProcessCustomFX || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000F0D RID: 3853 RVA: 0x0005DC74 File Offset: 0x0005BE74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == RichTextFlyIn.PropertyName.bbcode)
			{
				this.bbcode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06000F0E RID: 3854 RVA: 0x0005DC9C File Offset: 0x0005BE9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == RichTextFlyIn.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == RichTextFlyIn.PropertyName.bbcode)
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
		// Token: 0x06000F0F RID: 3855 RVA: 0x0005DCF4 File Offset: 0x0005BEF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, RichTextFlyIn.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, RichTextFlyIn.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000F10 RID: 3856 RVA: 0x0005DD48 File Offset: 0x0005BF48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(RichTextFlyIn.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
		}

		/// <inheritdoc />
		// Token: 0x06000F11 RID: 3857 RVA: 0x0005DD68 File Offset: 0x0005BF68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(RichTextFlyIn.PropertyName.bbcode, ref variant))
			{
				this.bbcode = variant.As<string>();
			}
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0005DDAC File Offset: 0x0005BFAC
		// Note: this type is marked as 'beforefieldinit'.
		static RichTextFlyIn()
		{
			string text = "offset_x";
			RichTextFlyIn._xOffsetKey = Variant.From<string>(ref text);
			text = "offset_y";
			RichTextFlyIn._yOffsetKey = Variant.From<string>(ref text);
		}

		// Token: 0x0400056A RID: 1386
		private static readonly Variant _xOffsetKey;

		// Token: 0x0400056B RID: 1387
		private static readonly Variant _yOffsetKey;

		// Token: 0x0400056C RID: 1388
		[Nullable(1)]
		public new string bbcode = "fly_in";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F0A RID: 3850
		public new class MethodName : AbstractMegaRichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the '_ProcessCustomFX' method.
			/// </summary>
			// Token: 0x04003C0D RID: 15373
			public static readonly StringName _ProcessCustomFX = "_ProcessCustomFX";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F0B RID: 3851
		public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003C0E RID: 15374
			public new static readonly StringName Bbcode = "Bbcode";

			/// <summary>
			/// Cached name for the 'bbcode' field.
			/// </summary>
			// Token: 0x04003C0F RID: 15375
			public new static readonly StringName bbcode = "bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F0C RID: 3852
		public new class SignalName : AbstractMegaRichTextEffect.SignalName
		{
		}
	}
}
