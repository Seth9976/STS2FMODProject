using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000308 RID: 776
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenuBg.cs")]
	public class NMainMenuBg : Control
	{
		// Token: 0x06002AC0 RID: 10944 RVA: 0x0012C590 File Offset: 0x0012A790
		public override void _Ready()
		{
			this._bg = base.GetNode<Control>("BgContainer");
			this._logo = base.GetNode<Node2D>("%Logo");
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x0012C5FD File Offset: 0x0012A7FD
		private void OnWindowChange()
		{
			this.ScaleBgIfNarrow((float)this._window.Size.X / (float)this._window.Size.Y);
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x0012C628 File Offset: 0x0012A828
		private void ScaleBgIfNarrow(float ratio)
		{
			if (ratio < 1.5f)
			{
				this._bg.Scale = Vector2.One * 1.04f;
				return;
			}
			this._bg.Scale = NMainMenuBg._defaultBgScale;
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x0012C660 File Offset: 0x0012A860
		public void HideLogo()
		{
			Tween logoTween = this._logoTween;
			if (logoTween != null)
			{
				logoTween.Kill();
			}
			this._logoTween = base.CreateTween();
			this._logoTween.TweenProperty(this._logo, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x0012C6C8 File Offset: 0x0012A8C8
		public void ShowLogo()
		{
			Tween logoTween = this._logoTween;
			if (logoTween != null)
			{
				logoTween.Kill();
			}
			this._logoTween = base.CreateTween();
			this._logoTween.TweenProperty(this._logo, "modulate:a", 1f, 1.0).SetEase(1L).SetTrans(7L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002AC5 RID: 10949 RVA: 0x0012C730 File Offset: 0x0012A930
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMainMenuBg.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuBg.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuBg.MethodName.ScaleBgIfNarrow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "ratio", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMainMenuBg.MethodName.HideLogo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuBg.MethodName.ShowLogo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AC6 RID: 10950 RVA: 0x0012C858 File Offset: 0x0012AA58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMainMenuBg.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuBg.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuBg.MethodName.ScaleBgIfNarrow && args.Count == 1)
			{
				this.ScaleBgIfNarrow(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuBg.MethodName.HideLogo && args.Count == 0)
			{
				this.HideLogo();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuBg.MethodName.ShowLogo && args.Count == 0)
			{
				this.ShowLogo();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002AC7 RID: 10951 RVA: 0x0012C938 File Offset: 0x0012AB38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMainMenuBg.MethodName._Ready || (ref method) == NMainMenuBg.MethodName.OnWindowChange || (ref method) == NMainMenuBg.MethodName.ScaleBgIfNarrow || (ref method) == NMainMenuBg.MethodName.HideLogo || (ref method) == NMainMenuBg.MethodName.ShowLogo || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002AC8 RID: 10952 RVA: 0x0012C998 File Offset: 0x0012AB98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMainMenuBg.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuBg.PropertyName._bg)
			{
				this._bg = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuBg.PropertyName._logo)
			{
				this._logo = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NMainMenuBg.PropertyName._logoTween)
			{
				this._logoTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002AC9 RID: 10953 RVA: 0x0012CA1C File Offset: 0x0012AC1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMainMenuBg.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			if ((ref name) == NMainMenuBg.PropertyName._bg)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bg);
				return true;
			}
			if ((ref name) == NMainMenuBg.PropertyName._logo)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._logo);
				return true;
			}
			if ((ref name) == NMainMenuBg.PropertyName._logoTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._logoTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002ACA RID: 10954 RVA: 0x0012CAB4 File Offset: 0x0012ACB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMainMenuBg.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuBg.PropertyName._bg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuBg.PropertyName._logo, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenuBg.PropertyName._logoTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002ACB RID: 10955 RVA: 0x0012CB4C File Offset: 0x0012AD4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMainMenuBg.PropertyName._window, Variant.From<Window>(ref this._window));
			info.AddProperty(NMainMenuBg.PropertyName._bg, Variant.From<Control>(ref this._bg));
			info.AddProperty(NMainMenuBg.PropertyName._logo, Variant.From<Node2D>(ref this._logo));
			info.AddProperty(NMainMenuBg.PropertyName._logoTween, Variant.From<Tween>(ref this._logoTween));
		}

		/// <inheritdoc />
		// Token: 0x06002ACC RID: 10956 RVA: 0x0012CBB8 File Offset: 0x0012ADB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMainMenuBg.PropertyName._window, ref variant))
			{
				this._window = variant.As<Window>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMainMenuBg.PropertyName._bg, ref variant2))
			{
				this._bg = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMainMenuBg.PropertyName._logo, ref variant3))
			{
				this._logo = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMainMenuBg.PropertyName._logoTween, ref variant4))
			{
				this._logoTween = variant4.As<Tween>();
			}
		}

		// Token: 0x04001001 RID: 4097
		[Nullable(1)]
		private Window _window;

		// Token: 0x04001002 RID: 4098
		[Nullable(1)]
		private Control _bg;

		// Token: 0x04001003 RID: 4099
		[Nullable(1)]
		private Node2D _logo;

		// Token: 0x04001004 RID: 4100
		[Nullable(2)]
		private Tween _logoTween;

		// Token: 0x04001005 RID: 4101
		private static readonly Vector2 _defaultBgScale = Vector2.One * 1.01f;

		// Token: 0x04001006 RID: 4102
		private const float _bgScaleRatioThreshold = 1.5f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200150D RID: 5389
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005051 RID: 20561
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04005052 RID: 20562
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'ScaleBgIfNarrow' method.
			/// </summary>
			// Token: 0x04005053 RID: 20563
			public static readonly StringName ScaleBgIfNarrow = "ScaleBgIfNarrow";

			/// <summary>
			/// Cached name for the 'HideLogo' method.
			/// </summary>
			// Token: 0x04005054 RID: 20564
			public static readonly StringName HideLogo = "HideLogo";

			/// <summary>
			/// Cached name for the 'ShowLogo' method.
			/// </summary>
			// Token: 0x04005055 RID: 20565
			public static readonly StringName ShowLogo = "ShowLogo";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200150E RID: 5390
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x04005056 RID: 20566
			public static readonly StringName _window = "_window";

			/// <summary>
			/// Cached name for the '_bg' field.
			/// </summary>
			// Token: 0x04005057 RID: 20567
			public static readonly StringName _bg = "_bg";

			/// <summary>
			/// Cached name for the '_logo' field.
			/// </summary>
			// Token: 0x04005058 RID: 20568
			public static readonly StringName _logo = "_logo";

			/// <summary>
			/// Cached name for the '_logoTween' field.
			/// </summary>
			// Token: 0x04005059 RID: 20569
			public static readonly StringName _logoTween = "_logoTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200150F RID: 5391
		public class SignalName : Control.SignalName
		{
		}
	}
}
