using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002B7 RID: 695
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsGradientMask.cs")]
	public class NSettingsGradientMask : TextureRect
	{
		// Token: 0x060024BE RID: 9406 RVA: 0x000F8CD8 File Offset: 0x000F6ED8
		public override void _Ready()
		{
			NSettingsScreen ancestorOfType = this.GetAncestorOfType<NSettingsScreen>();
			this._tabContainer = ancestorOfType.GetNode<NSettingsTabManager>("%SettingsTabManager");
			this._texture = (GradientTexture2D)base.Texture;
			base.Connect(Control.SignalName.Resized, Callable.From(new Action(this.OnResized)), 0U);
			this.OnResized();
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000F8D38 File Offset: 0x000F6F38
		private void OnResized()
		{
			float num = 1f - (this._tabContainer.Position.Y + this._tabContainer.Size.Y + -8f + 16f) / base.Size.Y;
			float num2 = num + 16f / base.Size.Y;
			this._texture.Gradient.SetOffset(2, num);
			this._texture.Gradient.SetOffset(3, num2);
			this._texture.Gradient.SetColor(2, Colors.White);
			this._texture.Gradient.SetColor(3, StsColors.transparentWhite);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024C0 RID: 9408 RVA: 0x000F8DEC File Offset: 0x000F6FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSettingsGradientMask.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsGradientMask.MethodName.OnResized, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024C1 RID: 9409 RVA: 0x000F8E60 File Offset: 0x000F7060
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsGradientMask.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsGradientMask.MethodName.OnResized && args.Count == 0)
			{
				this.OnResized();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060024C2 RID: 9410 RVA: 0x000F8EC0 File Offset: 0x000F70C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsGradientMask.MethodName._Ready || (ref method) == NSettingsGradientMask.MethodName.OnResized || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060024C3 RID: 9411 RVA: 0x000F8EE7 File Offset: 0x000F70E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsGradientMask.PropertyName._tabContainer)
			{
				this._tabContainer = VariantUtils.ConvertTo<NSettingsTabManager>(ref value);
				return true;
			}
			if ((ref name) == NSettingsGradientMask.PropertyName._texture)
			{
				this._texture = VariantUtils.ConvertTo<GradientTexture2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060024C4 RID: 9412 RVA: 0x000F8F28 File Offset: 0x000F7128
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsGradientMask.PropertyName._tabContainer)
			{
				value = VariantUtils.CreateFrom<NSettingsTabManager>(ref this._tabContainer);
				return true;
			}
			if ((ref name) == NSettingsGradientMask.PropertyName._texture)
			{
				value = VariantUtils.CreateFrom<GradientTexture2D>(ref this._texture);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024C5 RID: 9413 RVA: 0x000F8F80 File Offset: 0x000F7180
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsGradientMask.PropertyName._tabContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsGradientMask.PropertyName._texture, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024C6 RID: 9414 RVA: 0x000F8FD6 File Offset: 0x000F71D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsGradientMask.PropertyName._tabContainer, Variant.From<NSettingsTabManager>(ref this._tabContainer));
			info.AddProperty(NSettingsGradientMask.PropertyName._texture, Variant.From<GradientTexture2D>(ref this._texture));
		}

		/// <inheritdoc />
		// Token: 0x060024C7 RID: 9415 RVA: 0x000F900C File Offset: 0x000F720C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsGradientMask.PropertyName._tabContainer, ref variant))
			{
				this._tabContainer = variant.As<NSettingsTabManager>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsGradientMask.PropertyName._texture, ref variant2))
			{
				this._texture = variant2.As<GradientTexture2D>();
			}
		}

		// Token: 0x04000DC1 RID: 3521
		private const float _fadeOffset = -8f;

		// Token: 0x04000DC2 RID: 3522
		private const float _fadeSize = 16f;

		// Token: 0x04000DC3 RID: 3523
		[Nullable(1)]
		private NSettingsTabManager _tabContainer;

		// Token: 0x04000DC4 RID: 3524
		[Nullable(1)]
		private GradientTexture2D _texture;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013DD RID: 5085
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BE2 RID: 19426
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnResized' method.
			/// </summary>
			// Token: 0x04004BE3 RID: 19427
			public static readonly StringName OnResized = "OnResized";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013DE RID: 5086
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tabContainer' field.
			/// </summary>
			// Token: 0x04004BE4 RID: 19428
			public static readonly StringName _tabContainer = "_tabContainer";

			/// <summary>
			/// Cached name for the '_texture' field.
			/// </summary>
			// Token: 0x04004BE5 RID: 19429
			public static readonly StringName _texture = "_texture";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013DF RID: 5087
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
