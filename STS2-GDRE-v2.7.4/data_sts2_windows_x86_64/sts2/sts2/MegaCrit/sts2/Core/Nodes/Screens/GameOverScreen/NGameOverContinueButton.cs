using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen
{
	// Token: 0x0200031A RID: 794
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NGameOverContinueButton.cs")]
	public class NGameOverContinueButton : NButton
	{
		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002C26 RID: 11302 RVA: 0x00139246 File Offset: 0x00137446
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.select };
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x0013925C File Offset: 0x0013745C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._hsv = (ShaderMaterial)base.GetNode<TextureRect>("Image").Material;
			this._showPosition = base.GetPosition();
			base.Modulate = StsColors.transparentBlack;
			base.GetNode<MegaLabel>("Label").SetTextAutoSize(new LocString("game_over_screen", "BUTTON.continue").GetFormattedText());
		}

		/// <summary>
		/// Call when we want this button to animate in.
		/// </summary>
		// Token: 0x06002C28 RID: 11304 RVA: 0x001392D0 File Offset: 0x001374D0
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Visible = true;
			this._isEnabled = true;
			base.Position = new Vector2(base.Position.X, this._showPosition.Y + 190f);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "position:y", this._showPosition.Y, 0.5).SetEase(1L).SetTrans(3L);
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.5);
		}

		/// <summary>
		/// Call when we want this button to hide this button (Disables clickability/hotkeys)
		/// </summary>
		// Token: 0x06002C29 RID: 11305 RVA: 0x001393A4 File Offset: 0x001375A4
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Visible = false;
			this._isEnabled = false;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "position:y", this._showPosition.Y + 190f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x0013942C File Offset: 0x0013762C
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One * 1.1f, 0.05);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NGameOverContinueButton._s), 1.2f, 0.05).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NGameOverContinueButton._v), 1.4f, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x00139534 File Offset: 0x00137734
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 1.0).SetTrans(5L).SetEase(1L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NGameOverContinueButton._s), 1f, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NGameOverContinueButton._v), 1f, 1.0).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x00139640 File Offset: 0x00137840
		protected override void OnPress()
		{
			base.OnPress();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 1.0).SetTrans(5L).SetEase(1L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NGameOverContinueButton._s), 1f, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NGameOverContinueButton._v), 1f, 1.0).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x0013974A File Offset: 0x0013794A
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NGameOverContinueButton._s, value);
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x00139762 File Offset: 0x00137962
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NGameOverContinueButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C2F RID: 11311 RVA: 0x0013977C File Offset: 0x0013797C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NGameOverContinueButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverContinueButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverContinueButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverContinueButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverContinueButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverContinueButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverContinueButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGameOverContinueButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C30 RID: 11312 RVA: 0x00139954 File Offset: 0x00137B54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGameOverContinueButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverContinueButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverContinueButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverContinueButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverContinueButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverContinueButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverContinueButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverContinueButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002C31 RID: 11313 RVA: 0x00139AB0 File Offset: 0x00137CB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGameOverContinueButton.MethodName._Ready || (ref method) == NGameOverContinueButton.MethodName.OnEnable || (ref method) == NGameOverContinueButton.MethodName.OnDisable || (ref method) == NGameOverContinueButton.MethodName.OnFocus || (ref method) == NGameOverContinueButton.MethodName.OnUnfocus || (ref method) == NGameOverContinueButton.MethodName.OnPress || (ref method) == NGameOverContinueButton.MethodName.UpdateShaderS || (ref method) == NGameOverContinueButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002C32 RID: 11314 RVA: 0x00139B3C File Offset: 0x00137D3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGameOverContinueButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NGameOverContinueButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NGameOverContinueButton.PropertyName._showPosition)
			{
				this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NGameOverContinueButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002C33 RID: 11315 RVA: 0x00139BC0 File Offset: 0x00137DC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGameOverContinueButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NGameOverContinueButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NGameOverContinueButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NGameOverContinueButton.PropertyName._showPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
				return true;
			}
			if ((ref name) == NGameOverContinueButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C34 RID: 11316 RVA: 0x00139C78 File Offset: 0x00137E78
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NGameOverContinueButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverContinueButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverContinueButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NGameOverContinueButton.PropertyName._showPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverContinueButton.PropertyName._hsv, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C35 RID: 11317 RVA: 0x00139D30 File Offset: 0x00137F30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGameOverContinueButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NGameOverContinueButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NGameOverContinueButton.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
			info.AddProperty(NGameOverContinueButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x06002C36 RID: 11318 RVA: 0x00139D9C File Offset: 0x00137F9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGameOverContinueButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGameOverContinueButton.PropertyName._hoverTween, ref variant2))
			{
				this._hoverTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGameOverContinueButton.PropertyName._showPosition, ref variant3))
			{
				this._showPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGameOverContinueButton.PropertyName._hsv, ref variant4))
			{
				this._hsv = variant4.As<ShaderMaterial>();
			}
		}

		// Token: 0x0400109D RID: 4253
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400109E RID: 4254
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x0400109F RID: 4255
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040010A0 RID: 4256
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x040010A1 RID: 4257
		private Vector2 _showPosition;

		// Token: 0x040010A2 RID: 4258
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x040010A3 RID: 4259
		private const float _hoverS = 1.2f;

		// Token: 0x040010A4 RID: 4260
		private const float _hoverV = 1.4f;

		// Token: 0x040010A5 RID: 4261
		private const float _unhoverS = 1f;

		// Token: 0x040010A6 RID: 4262
		private const float _unhoverV = 1f;

		// Token: 0x040010A7 RID: 4263
		private const float _pressDownS = 1f;

		// Token: 0x040010A8 RID: 4264
		private const float _pressDownV = 1f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200154A RID: 5450
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005168 RID: 20840
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005169 RID: 20841
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x0400516A RID: 20842
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400516B RID: 20843
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400516C RID: 20844
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400516D RID: 20845
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x0400516E RID: 20846
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x0400516F RID: 20847
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200154B RID: 5451
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005170 RID: 20848
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005171 RID: 20849
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04005172 RID: 20850
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_showPosition' field.
			/// </summary>
			// Token: 0x04005173 RID: 20851
			public static readonly StringName _showPosition = "_showPosition";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005174 RID: 20852
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200154C RID: 5452
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
