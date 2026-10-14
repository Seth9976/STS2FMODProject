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
	/// <summary>
	/// This is specifically the main menu button in the Game Over screen.
	/// </summary>
	// Token: 0x0200031C RID: 796
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NReturnToMainMenuButton.cs")]
	public class NReturnToMainMenuButton : NButton
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x0013CE43 File Offset: 0x0013B043
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.select };
			}
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x0013CE58 File Offset: 0x0013B058
		public override void _Ready()
		{
			this.ConnectSignals();
			this._hsv = (ShaderMaterial)base.GetNode<TextureRect>("Image").Material;
			this._showPosition = base.GetPosition();
			base.Position = this._showPosition + new Vector2(-140f, 0f);
			this._showPosition = base.Position;
			base.Modulate = StsColors.transparentBlack;
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.SetTextAutoSize(NReturnToMainMenuButton._mainMenuLoc.GetFormattedText());
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x0013CEF9 File Offset: 0x0013B0F9
		public void SetLabelForUnlock()
		{
			this._label.SetTextAutoSize(new LocString("game_over_screen", "BUTTON.unlock").GetFormattedText());
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x0013CF1C File Offset: 0x0013B11C
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
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NReturnToMainMenuButton._s), 1f, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NReturnToMainMenuButton._v), 1f, 1.0).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Call when we want this button to animate in.
		/// Called when we transition from Death intermission -&gt; summary and when returning from Run History/Settings.
		/// </summary>
		// Token: 0x06002C6F RID: 11375 RVA: 0x0013D028 File Offset: 0x0013B228
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Visible = true;
			this._isEnabled = true;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "position", this._showPosition, 0.5).SetEase(1L).SetTrans(3L);
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.5).From(StsColors.transparentBlack);
		}

		/// <summary>
		/// Called to hide this button (Open Settings/Run History or something during Death summary)
		/// </summary>
		// Token: 0x06002C70 RID: 11376 RVA: 0x0013D0CD File Offset: 0x0013B2CD
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Visible = false;
			this._isEnabled = false;
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x0013D0E4 File Offset: 0x0013B2E4
		private void HideButton()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "position:y", this._showPosition.Y + 190f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x0013D158 File Offset: 0x0013B358
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
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NReturnToMainMenuButton._s), 1.2f, 0.05).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NReturnToMainMenuButton._v), 1.4f, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x0013D260 File Offset: 0x0013B460
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
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NReturnToMainMenuButton._s), 1f, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NReturnToMainMenuButton._v), 1f, 1.0).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x0013D36A File Offset: 0x0013B56A
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NReturnToMainMenuButton._s, value);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x0013D382 File Offset: 0x0013B582
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NReturnToMainMenuButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C76 RID: 11382 RVA: 0x0013D39C File Offset: 0x0013B59C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NReturnToMainMenuButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.SetLabelForUnlock, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.HideButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NReturnToMainMenuButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C77 RID: 11383 RVA: 0x0013D5D4 File Offset: 0x0013B7D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NReturnToMainMenuButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.SetLabelForUnlock && args.Count == 0)
			{
				this.SetLabelForUnlock();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.HideButton && args.Count == 0)
			{
				this.HideButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReturnToMainMenuButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002C78 RID: 11384 RVA: 0x0013D778 File Offset: 0x0013B978
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NReturnToMainMenuButton.MethodName._Ready || (ref method) == NReturnToMainMenuButton.MethodName.SetLabelForUnlock || (ref method) == NReturnToMainMenuButton.MethodName.OnPress || (ref method) == NReturnToMainMenuButton.MethodName.OnEnable || (ref method) == NReturnToMainMenuButton.MethodName.OnDisable || (ref method) == NReturnToMainMenuButton.MethodName.HideButton || (ref method) == NReturnToMainMenuButton.MethodName.OnFocus || (ref method) == NReturnToMainMenuButton.MethodName.OnUnfocus || (ref method) == NReturnToMainMenuButton.MethodName.UpdateShaderS || (ref method) == NReturnToMainMenuButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002C79 RID: 11385 RVA: 0x0013D824 File Offset: 0x0013BA24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NReturnToMainMenuButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._showPosition)
			{
				this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002C7A RID: 11386 RVA: 0x0013D8C0 File Offset: 0x0013BAC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NReturnToMainMenuButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._showPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
				return true;
			}
			if ((ref name) == NReturnToMainMenuButton.PropertyName._hsv)
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
		// Token: 0x06002C7B RID: 11387 RVA: 0x0013D998 File Offset: 0x0013BB98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NReturnToMainMenuButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReturnToMainMenuButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReturnToMainMenuButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReturnToMainMenuButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NReturnToMainMenuButton.PropertyName._showPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReturnToMainMenuButton.PropertyName._hsv, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C7C RID: 11388 RVA: 0x0013DA74 File Offset: 0x0013BC74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NReturnToMainMenuButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NReturnToMainMenuButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NReturnToMainMenuButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NReturnToMainMenuButton.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
			info.AddProperty(NReturnToMainMenuButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x06002C7D RID: 11389 RVA: 0x0013DAF8 File Offset: 0x0013BCF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NReturnToMainMenuButton.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NReturnToMainMenuButton.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NReturnToMainMenuButton.PropertyName._hoverTween, ref variant3))
			{
				this._hoverTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NReturnToMainMenuButton.PropertyName._showPosition, ref variant4))
			{
				this._showPosition = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NReturnToMainMenuButton.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
		}

		// Token: 0x040010CD RID: 4301
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040010CE RID: 4302
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040010CF RID: 4303
		[Nullable(1)]
		private static readonly LocString _mainMenuLoc = new LocString("game_over_screen", "BUTTON.mainMenu");

		// Token: 0x040010D0 RID: 4304
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x040010D1 RID: 4305
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040010D2 RID: 4306
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x040010D3 RID: 4307
		private Vector2 _showPosition;

		// Token: 0x040010D4 RID: 4308
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x040010D5 RID: 4309
		private const float _hoverS = 1.2f;

		// Token: 0x040010D6 RID: 4310
		private const float _hoverV = 1.4f;

		// Token: 0x040010D7 RID: 4311
		private const float _unhoverS = 1f;

		// Token: 0x040010D8 RID: 4312
		private const float _unhoverV = 1f;

		// Token: 0x040010D9 RID: 4313
		private const float _pressDownS = 1f;

		// Token: 0x040010DA RID: 4314
		private const float _pressDownV = 1f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200155B RID: 5467
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040051DC RID: 20956
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetLabelForUnlock' method.
			/// </summary>
			// Token: 0x040051DD RID: 20957
			public static readonly StringName SetLabelForUnlock = "SetLabelForUnlock";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x040051DE RID: 20958
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x040051DF RID: 20959
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x040051E0 RID: 20960
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'HideButton' method.
			/// </summary>
			// Token: 0x040051E1 RID: 20961
			public static readonly StringName HideButton = "HideButton";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040051E2 RID: 20962
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040051E3 RID: 20963
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x040051E4 RID: 20964
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x040051E5 RID: 20965
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200155C RID: 5468
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040051E6 RID: 20966
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040051E7 RID: 20967
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040051E8 RID: 20968
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x040051E9 RID: 20969
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_showPosition' field.
			/// </summary>
			// Token: 0x040051EA RID: 20970
			public static readonly StringName _showPosition = "_showPosition";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040051EB RID: 20971
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200155D RID: 5469
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
