using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	// Token: 0x020003C5 RID: 965
	[ScriptPath("res://src/Core/Nodes/Debug/NDebugAspectRatio.cs")]
	public class NDebugAspectRatio : Control
	{
		// Token: 0x06003A5B RID: 14939 RVA: 0x001BC27C File Offset: 0x001BA47C
		public override void _Ready()
		{
			this._bg = base.GetNode<TextureRect>("EventBg");
			this._infoLabel = base.GetNode<Label>("Anchors/Label");
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x001BC2EC File Offset: 0x001BA4EC
		private void OnWindowChange()
		{
			float num = (float)this._window.Size.X / (float)this._window.Size.Y;
			string text = num.ToString("0.000");
			this.ScaleBgIfNarrow(num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num > 2.3888888f)
			{
				this._window.ContentScaleAspect = 2L;
				this._window.ContentScaleSize = new Vector2I(2580, 1080);
				Label infoLabel = this._infoLabel;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(this._window.Size);
				infoLabel.Text = defaultInterpolatedStringHandler.ToStringAndClear();
				this._infoLabel.Modulate = StsColors.red;
				return;
			}
			if (num < 1.3333334f)
			{
				this._window.ContentScaleAspect = 3L;
				this._window.ContentScaleSize = new Vector2I(1680, 1260);
				Label infoLabel2 = this._infoLabel;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(this._window.Size);
				infoLabel2.Text = defaultInterpolatedStringHandler.ToStringAndClear();
				this._infoLabel.Modulate = StsColors.red;
				return;
			}
			this._window.ContentScaleAspect = 4L;
			this._window.ContentScaleSize = new Vector2I(1680, 1080);
			Label infoLabel3 = this._infoLabel;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(this._window.Size);
			infoLabel3.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			this._infoLabel.Modulate = StsColors.cream;
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x001BC4AB File Offset: 0x001BA6AB
		private void ScaleBgIfNarrow(float ratio)
		{
			if (ratio < 1.5f)
			{
				this._bg.Scale = Vector2.One * 1.05f;
				return;
			}
			this._bg.Scale = NDebugAspectRatio._defaultBgScale;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A5E RID: 14942 RVA: 0x001BC4E0 File Offset: 0x001BA6E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NDebugAspectRatio.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugAspectRatio.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugAspectRatio.MethodName.ScaleBgIfNarrow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "ratio", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A5F RID: 14943 RVA: 0x001BC5A8 File Offset: 0x001BA7A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDebugAspectRatio.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAspectRatio.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAspectRatio.MethodName.ScaleBgIfNarrow && args.Count == 1)
			{
				this.ScaleBgIfNarrow(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003A60 RID: 14944 RVA: 0x001BC63B File Offset: 0x001BA83B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDebugAspectRatio.MethodName._Ready || (ref method) == NDebugAspectRatio.MethodName.OnWindowChange || (ref method) == NDebugAspectRatio.MethodName.ScaleBgIfNarrow || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003A61 RID: 14945 RVA: 0x001BC674 File Offset: 0x001BA874
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDebugAspectRatio.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			if ((ref name) == NDebugAspectRatio.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<Label>(ref value);
				return true;
			}
			if ((ref name) == NDebugAspectRatio.PropertyName._bg)
			{
				this._bg = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003A62 RID: 14946 RVA: 0x001BC6DC File Offset: 0x001BA8DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDebugAspectRatio.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			if ((ref name) == NDebugAspectRatio.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<Label>(ref this._infoLabel);
				return true;
			}
			if ((ref name) == NDebugAspectRatio.PropertyName._bg)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._bg);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A63 RID: 14947 RVA: 0x001BC754 File Offset: 0x001BA954
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDebugAspectRatio.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDebugAspectRatio.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDebugAspectRatio.PropertyName._bg, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A64 RID: 14948 RVA: 0x001BC7CC File Offset: 0x001BA9CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDebugAspectRatio.PropertyName._window, Variant.From<Window>(ref this._window));
			info.AddProperty(NDebugAspectRatio.PropertyName._infoLabel, Variant.From<Label>(ref this._infoLabel));
			info.AddProperty(NDebugAspectRatio.PropertyName._bg, Variant.From<TextureRect>(ref this._bg));
		}

		/// <inheritdoc />
		// Token: 0x06003A65 RID: 14949 RVA: 0x001BC824 File Offset: 0x001BAA24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDebugAspectRatio.PropertyName._window, ref variant))
			{
				this._window = variant.As<Window>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDebugAspectRatio.PropertyName._infoLabel, ref variant2))
			{
				this._infoLabel = variant2.As<Label>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDebugAspectRatio.PropertyName._bg, ref variant3))
			{
				this._bg = variant3.As<TextureRect>();
			}
		}

		// Token: 0x040016C8 RID: 5832
		[Nullable(1)]
		private Window _window;

		// Token: 0x040016C9 RID: 5833
		[Nullable(1)]
		private Label _infoLabel;

		// Token: 0x040016CA RID: 5834
		[Nullable(1)]
		private TextureRect _bg;

		// Token: 0x040016CB RID: 5835
		private const float _maxNarrowRatio = 1.3333334f;

		// Token: 0x040016CC RID: 5836
		private const float _maxWideRatio = 2.3888888f;

		// Token: 0x040016CD RID: 5837
		private static readonly Vector2 _defaultBgScale = Vector2.One * 1.01f;

		// Token: 0x040016CE RID: 5838
		private const float _bgScaleRatioThreshold = 1.5f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017F4 RID: 6132
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D86 RID: 23942
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04005D87 RID: 23943
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'ScaleBgIfNarrow' method.
			/// </summary>
			// Token: 0x04005D88 RID: 23944
			public static readonly StringName ScaleBgIfNarrow = "ScaleBgIfNarrow";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017F5 RID: 6133
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x04005D89 RID: 23945
			public static readonly StringName _window = "_window";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x04005D8A RID: 23946
			public static readonly StringName _infoLabel = "_infoLabel";

			/// <summary>
			/// Cached name for the '_bg' field.
			/// </summary>
			// Token: 0x04005D8B RID: 23947
			public static readonly StringName _bg = "_bg";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017F6 RID: 6134
		public class SignalName : Control.SignalName
		{
		}
	}
}
