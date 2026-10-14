using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// A simple button to skip choosing a card on some select screens (like the choose 1 of 3 card reward screen).
	/// </summary>
	// Token: 0x02000347 RID: 839
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NChoiceSelectionSkipButton.cs")]
	public class NChoiceSelectionSkipButton : NButton
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002FEB RID: 12267 RVA: 0x0015E9F5 File Offset: 0x0015CBF5
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.cancel };
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x0015EA0C File Offset: 0x0015CC0C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<TextureRect>("Image");
			this._label = base.GetNode<MegaLabel>("Label");
			this._hsv = (ShaderMaterial)this._image.Material;
			this._showPosition = base.Position;
			if (this._optionName != null)
			{
				this._label.SetTextAutoSize(this._optionName);
			}
			this._controllerHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(this.Hotkeys.First<string>());
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x0015EAA8 File Offset: 0x0015CCA8
		public void AnimateIn()
		{
			Tween animInTween = this._animInTween;
			if (animInTween != null)
			{
				animInTween.Kill();
			}
			this._animInTween = base.CreateTween().SetParallel(true);
			this._animInTween.TweenProperty(this, "modulate:a", 1f, 0.4).SetEase(1L).SetTrans(5L)
				.From(0f);
			this._animInTween.TweenProperty(this, "position", this._showPosition, 0.4).SetEase(1L).SetTrans(10L)
				.From(this._showPosition + NChoiceSelectionSkipButton._animOffsetPosition);
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x0015EB74 File Offset: 0x0015CD74
		protected override void OnPress()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsvHover, this._hsvDown, 0.20000000298023224).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this, "scale", NChoiceSelectionSkipButton._downScale, 0.20000000298023224).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x0015EC1C File Offset: 0x0015CE1C
		protected override void OnFocus()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			base.Scale = NChoiceSelectionSkipButton._hoverScale;
			this._hsv.SetShaderParameter(NChoiceSelectionSkipButton._v, this._hsvHover);
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x0015EC50 File Offset: 0x0015CE50
		protected override void OnUnfocus()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NChoiceSelectionSkipButton._v), this._hsvDefault, 0.5).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this, "scale", NChoiceSelectionSkipButton._defaultScale, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x0015ED02 File Offset: 0x0015CF02
		private void UpdateShaderParam(float value)
		{
			this._hsv.SetShaderParameter(NChoiceSelectionSkipButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FF2 RID: 12274 RVA: 0x0015ED1C File Offset: 0x0015CF1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NChoiceSelectionSkipButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChoiceSelectionSkipButton.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChoiceSelectionSkipButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChoiceSelectionSkipButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChoiceSelectionSkipButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChoiceSelectionSkipButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FF3 RID: 12275 RVA: 0x0015EE70 File Offset: 0x0015D070
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NChoiceSelectionSkipButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChoiceSelectionSkipButton.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChoiceSelectionSkipButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChoiceSelectionSkipButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChoiceSelectionSkipButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChoiceSelectionSkipButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002FF4 RID: 12276 RVA: 0x0015EF74 File Offset: 0x0015D174
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NChoiceSelectionSkipButton.MethodName._Ready || (ref method) == NChoiceSelectionSkipButton.MethodName.AnimateIn || (ref method) == NChoiceSelectionSkipButton.MethodName.OnPress || (ref method) == NChoiceSelectionSkipButton.MethodName.OnFocus || (ref method) == NChoiceSelectionSkipButton.MethodName.OnUnfocus || (ref method) == NChoiceSelectionSkipButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002FF5 RID: 12277 RVA: 0x0015EFE4 File Offset: 0x0015D1E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._optionName)
			{
				this._optionName = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._animInTween)
			{
				this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._showPosition)
			{
				this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._currentTween)
			{
				this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsvDefault)
			{
				this._hsvDefault = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsvHover)
			{
				this._hsvHover = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsvDown)
			{
				this._hsvDown = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002FF6 RID: 12278 RVA: 0x0015F108 File Offset: 0x0015D308
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._optionName)
			{
				value = VariantUtils.CreateFrom<string>(ref this._optionName);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._animInTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._showPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._currentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsvDefault)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvDefault);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsvHover)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvHover);
				return true;
			}
			if ((ref name) == NChoiceSelectionSkipButton.PropertyName._hsvDown)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvDown);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FF7 RID: 12279 RVA: 0x0015F280 File Offset: 0x0015D480
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NChoiceSelectionSkipButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChoiceSelectionSkipButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(4L, NChoiceSelectionSkipButton.PropertyName._optionName, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChoiceSelectionSkipButton.PropertyName._animInTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NChoiceSelectionSkipButton.PropertyName._showPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChoiceSelectionSkipButton.PropertyName._currentTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChoiceSelectionSkipButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(0L, NChoiceSelectionSkipButton.PropertyName._hsvDefault, 0L, "", 4096L, false),
				new PropertyInfo(0L, NChoiceSelectionSkipButton.PropertyName._hsvHover, 0L, "", 4096L, false),
				new PropertyInfo(0L, NChoiceSelectionSkipButton.PropertyName._hsvDown, 0L, "", 4096L, false),
				new PropertyInfo(34L, NChoiceSelectionSkipButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FF8 RID: 12280 RVA: 0x0015F3FC File Offset: 0x0015D5FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._optionName, Variant.From<string>(ref this._optionName));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsvDefault, Variant.From<Variant>(ref this._hsvDefault));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsvHover, Variant.From<Variant>(ref this._hsvHover));
			info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsvDown, Variant.From<Variant>(ref this._hsvDown));
		}

		/// <inheritdoc />
		// Token: 0x06002FF9 RID: 12281 RVA: 0x0015F4EC File Offset: 0x0015D6EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._optionName, ref variant3))
			{
				this._optionName = variant3.As<string>();
			}
			Variant variant4;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._animInTween, ref variant4))
			{
				this._animInTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._showPosition, ref variant5))
			{
				this._showPosition = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._currentTween, ref variant6))
			{
				this._currentTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsvDefault, ref variant8))
			{
				this._hsvDefault = variant8.As<Variant>();
			}
			Variant variant9;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsvHover, ref variant9))
			{
				this._hsvHover = variant9.As<Variant>();
			}
			Variant variant10;
			if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsvDown, ref variant10))
			{
				this._hsvDown = variant10.As<Variant>();
			}
		}

		// Token: 0x0400126F RID: 4719
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001270 RID: 4720
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04001271 RID: 4721
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04001272 RID: 4722
		[Nullable(2)]
		private string _optionName;

		// Token: 0x04001273 RID: 4723
		[Nullable(2)]
		private Tween _animInTween;

		// Token: 0x04001274 RID: 4724
		private Vector2 _showPosition;

		// Token: 0x04001275 RID: 4725
		private static readonly Vector2 _animOffsetPosition = new Vector2(0f, -50f);

		// Token: 0x04001276 RID: 4726
		[Nullable(2)]
		private Tween _currentTween;

		// Token: 0x04001277 RID: 4727
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04001278 RID: 4728
		private Variant _hsvDefault = 0.9;

		// Token: 0x04001279 RID: 4729
		private Variant _hsvHover = 1.1;

		// Token: 0x0400127A RID: 4730
		private Variant _hsvDown = 0.7;

		// Token: 0x0400127B RID: 4731
		private static readonly Vector2 _defaultScale = Vector2.One;

		// Token: 0x0400127C RID: 4732
		private static readonly Vector2 _hoverScale = Vector2.One * 1.05f;

		// Token: 0x0400127D RID: 4733
		private static readonly Vector2 _downScale = Vector2.One * 0.95f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001602 RID: 5634
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400554B RID: 21835
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x0400554C RID: 21836
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400554D RID: 21837
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400554E RID: 21838
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400554F RID: 21839
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04005550 RID: 21840
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001603 RID: 5635
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005551 RID: 21841
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005552 RID: 21842
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005553 RID: 21843
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_optionName' field.
			/// </summary>
			// Token: 0x04005554 RID: 21844
			public static readonly StringName _optionName = "_optionName";

			/// <summary>
			/// Cached name for the '_animInTween' field.
			/// </summary>
			// Token: 0x04005555 RID: 21845
			public static readonly StringName _animInTween = "_animInTween";

			/// <summary>
			/// Cached name for the '_showPosition' field.
			/// </summary>
			// Token: 0x04005556 RID: 21846
			public static readonly StringName _showPosition = "_showPosition";

			/// <summary>
			/// Cached name for the '_currentTween' field.
			/// </summary>
			// Token: 0x04005557 RID: 21847
			public static readonly StringName _currentTween = "_currentTween";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005558 RID: 21848
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_hsvDefault' field.
			/// </summary>
			// Token: 0x04005559 RID: 21849
			public static readonly StringName _hsvDefault = "_hsvDefault";

			/// <summary>
			/// Cached name for the '_hsvHover' field.
			/// </summary>
			// Token: 0x0400555A RID: 21850
			public static readonly StringName _hsvHover = "_hsvHover";

			/// <summary>
			/// Cached name for the '_hsvDown' field.
			/// </summary>
			// Token: 0x0400555B RID: 21851
			public static readonly StringName _hsvDown = "_hsvDown";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001604 RID: 5636
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
