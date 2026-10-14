using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.RichTextTags
{
	// Token: 0x02000133 RID: 307
	[ScriptPath("res://src/Core/RichTextTags/AbstractMegaRichTextEffect.cs")]
	public abstract class AbstractMegaRichTextEffect : RichTextEffect
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0005CC1B File Offset: 0x0005AE1B
		[Nullable(1)]
		public string bbcode
		{
			[NullableContext(1)]
			get
			{
				return this.Bbcode;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06000ECB RID: 3787
		[Nullable(1)]
		protected abstract string Bbcode
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0005CC23 File Offset: 0x0005AE23
		protected bool ShouldTransformText()
		{
			return Engine.IsEditorHint() || SaveManager.Instance.PrefsSave.TextEffectsEnabled;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000ECD RID: 3789 RVA: 0x0005CC40 File Offset: 0x0005AE40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(AbstractMegaRichTextEffect.MethodName.ShouldTransformText, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000ECE RID: 3790 RVA: 0x0005CC84 File Offset: 0x0005AE84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == AbstractMegaRichTextEffect.MethodName.ShouldTransformText && args.Count == 0)
			{
				bool flag = this.ShouldTransformText();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06000ECF RID: 3791 RVA: 0x0005CCC6 File Offset: 0x0005AEC6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == AbstractMegaRichTextEffect.MethodName.ShouldTransformText || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06000ED0 RID: 3792 RVA: 0x0005CCE0 File Offset: 0x0005AEE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == AbstractMegaRichTextEffect.PropertyName.bbcode)
			{
				string text = this.bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == AbstractMegaRichTextEffect.PropertyName.Bbcode)
			{
				string text = this.Bbcode;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000ED1 RID: 3793 RVA: 0x0005CD3C File Offset: 0x0005AF3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, AbstractMegaRichTextEffect.PropertyName.bbcode, 0L, "", 4096L, false),
				new PropertyInfo(4L, AbstractMegaRichTextEffect.PropertyName.Bbcode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000ED2 RID: 3794 RVA: 0x0005CD90 File Offset: 0x0005AF90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06000ED3 RID: 3795 RVA: 0x0005CD99 File Offset: 0x0005AF99
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000EFB RID: 3835
		public class MethodName : RichTextEffect.MethodName
		{
			/// <summary>
			/// Cached name for the 'ShouldTransformText' method.
			/// </summary>
			// Token: 0x04003BFB RID: 15355
			public static readonly StringName ShouldTransformText = "ShouldTransformText";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000EFC RID: 3836
		public class PropertyName : RichTextEffect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'bbcode' property.
			/// </summary>
			// Token: 0x04003BFC RID: 15356
			public static readonly StringName bbcode = "bbcode";

			/// <summary>
			/// Cached name for the 'Bbcode' property.
			/// </summary>
			// Token: 0x04003BFD RID: 15357
			public static readonly StringName Bbcode = "Bbcode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000EFD RID: 3837
		public class SignalName : RichTextEffect.SignalName
		{
		}
	}
}
