using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// a button that sits below the card reward screen that presents alternatives to picking cards.
	/// The most common is "Skip", but some relics dynamically add other options such as Pael's Wing
	/// </summary>
	// Token: 0x02000345 RID: 837
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NCardRewardAlternativeButton.cs")]
	public class NCardRewardAlternativeButton : NButton
	{
		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002FB6 RID: 12214 RVA: 0x0015C57D File Offset: 0x0015A77D
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/ui/card_reward_alternative_button");
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x0015C589 File Offset: 0x0015A789
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardRewardAlternativeButton.ScenePath);
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x0015C595 File Offset: 0x0015A795
		protected override string[] Hotkeys
		{
			get
			{
				return this._hotkeys;
			}
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x0015C5A0 File Offset: 0x0015A7A0
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

		// Token: 0x06002FBA RID: 12218 RVA: 0x0015C63C File Offset: 0x0015A83C
		public static NCardRewardAlternativeButton Create(string optionName, string hotkey)
		{
			NCardRewardAlternativeButton ncardRewardAlternativeButton = PreloadManager.Cache.GetScene(NCardRewardAlternativeButton.ScenePath).Instantiate<NCardRewardAlternativeButton>(0L);
			ncardRewardAlternativeButton._optionName = optionName;
			ncardRewardAlternativeButton._hotkeys = new string[] { hotkey };
			return ncardRewardAlternativeButton;
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x0015C678 File Offset: 0x0015A878
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
				.From(this._showPosition + NCardRewardAlternativeButton._animOffsetPosition);
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x0015C744 File Offset: 0x0015A944
		protected override void OnPress()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsvHover, this._hsvDown, 0.20000000298023224).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this, "scale", NCardRewardAlternativeButton._downScale, 0.20000000298023224).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x0015C7EC File Offset: 0x0015A9EC
		protected override void OnFocus()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			base.Scale = NCardRewardAlternativeButton._hoverScale;
			this._hsv.SetShaderParameter(NCardRewardAlternativeButton._v, this._hsvHover);
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x0015C820 File Offset: 0x0015AA20
		protected override void OnUnfocus()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NCardRewardAlternativeButton._v), this._hsvDefault, 0.5).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this, "scale", NCardRewardAlternativeButton._defaultScale, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0015C8D2 File Offset: 0x0015AAD2
		private void UpdateShaderParam(float value)
		{
			this._hsv.SetShaderParameter(NCardRewardAlternativeButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FC0 RID: 12224 RVA: 0x0015C8EC File Offset: 0x0015AAEC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NCardRewardAlternativeButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardAlternativeButton.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "optionName", 0L, "", 6L, false),
					new PropertyInfo(4L, "hotkey", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardRewardAlternativeButton.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardAlternativeButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardAlternativeButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardAlternativeButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRewardAlternativeButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FC1 RID: 12225 RVA: 0x0015CAC4 File Offset: 0x0015ACC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRewardAlternativeButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardAlternativeButton.MethodName.Create && args.Count == 2)
			{
				NCardRewardAlternativeButton ncardRewardAlternativeButton = NCardRewardAlternativeButton.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<NCardRewardAlternativeButton>(ref ncardRewardAlternativeButton);
				return true;
			}
			if ((ref method) == NCardRewardAlternativeButton.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardAlternativeButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardAlternativeButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardAlternativeButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRewardAlternativeButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x0015CC0C File Offset: 0x0015AE0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRewardAlternativeButton.MethodName.Create && args.Count == 2)
			{
				NCardRewardAlternativeButton ncardRewardAlternativeButton = NCardRewardAlternativeButton.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<NCardRewardAlternativeButton>(ref ncardRewardAlternativeButton);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002FC3 RID: 12227 RVA: 0x0015CC68 File Offset: 0x0015AE68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardRewardAlternativeButton.MethodName._Ready || (ref method) == NCardRewardAlternativeButton.MethodName.Create || (ref method) == NCardRewardAlternativeButton.MethodName.AnimateIn || (ref method) == NCardRewardAlternativeButton.MethodName.OnPress || (ref method) == NCardRewardAlternativeButton.MethodName.OnFocus || (ref method) == NCardRewardAlternativeButton.MethodName.OnUnfocus || (ref method) == NCardRewardAlternativeButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002FC4 RID: 12228 RVA: 0x0015CCE8 File Offset: 0x0015AEE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._optionName)
			{
				this._optionName = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._animInTween)
			{
				this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._showPosition)
			{
				this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._currentTween)
			{
				this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsvDefault)
			{
				this._hsvDefault = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsvHover)
			{
				this._hsvHover = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsvDown)
			{
				this._hsvDown = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hotkeys)
			{
				this._hotkeys = VariantUtils.ConvertTo<string[]>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002FC5 RID: 12229 RVA: 0x0015CE28 File Offset: 0x0015B028
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardRewardAlternativeButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._optionName)
			{
				value = VariantUtils.CreateFrom<string>(ref this._optionName);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._animInTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._showPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._currentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsvDefault)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvDefault);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsvHover)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvHover);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hsvDown)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvDown);
				return true;
			}
			if ((ref name) == NCardRewardAlternativeButton.PropertyName._hotkeys)
			{
				value = VariantUtils.CreateFrom<string[]>(ref this._hotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FC6 RID: 12230 RVA: 0x0015CFC0 File Offset: 0x0015B1C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardRewardAlternativeButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardAlternativeButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(4L, NCardRewardAlternativeButton.PropertyName._optionName, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardAlternativeButton.PropertyName._animInTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardRewardAlternativeButton.PropertyName._showPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardAlternativeButton.PropertyName._currentTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRewardAlternativeButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(0L, NCardRewardAlternativeButton.PropertyName._hsvDefault, 0L, "", 4096L, false),
				new PropertyInfo(0L, NCardRewardAlternativeButton.PropertyName._hsvHover, 0L, "", 4096L, false),
				new PropertyInfo(0L, NCardRewardAlternativeButton.PropertyName._hsvDown, 0L, "", 4096L, false),
				new PropertyInfo(34L, NCardRewardAlternativeButton.PropertyName._hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(34L, NCardRewardAlternativeButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FC7 RID: 12231 RVA: 0x0015D15C File Offset: 0x0015B35C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._optionName, Variant.From<string>(ref this._optionName));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._hsvDefault, Variant.From<Variant>(ref this._hsvDefault));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._hsvHover, Variant.From<Variant>(ref this._hsvHover));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._hsvDown, Variant.From<Variant>(ref this._hsvDown));
			info.AddProperty(NCardRewardAlternativeButton.PropertyName._hotkeys, Variant.From<string[]>(ref this._hotkeys));
		}

		/// <inheritdoc />
		// Token: 0x06002FC8 RID: 12232 RVA: 0x0015D264 File Offset: 0x0015B464
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._optionName, ref variant3))
			{
				this._optionName = variant3.As<string>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._animInTween, ref variant4))
			{
				this._animInTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._showPosition, ref variant5))
			{
				this._showPosition = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._currentTween, ref variant6))
			{
				this._currentTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._hsvDefault, ref variant8))
			{
				this._hsvDefault = variant8.As<Variant>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._hsvHover, ref variant9))
			{
				this._hsvHover = variant9.As<Variant>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._hsvDown, ref variant10))
			{
				this._hsvDown = variant10.As<Variant>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardRewardAlternativeButton.PropertyName._hotkeys, ref variant11))
			{
				this._hotkeys = variant11.As<string[]>();
			}
		}

		// Token: 0x04001250 RID: 4688
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001251 RID: 4689
		private TextureRect _image;

		// Token: 0x04001252 RID: 4690
		private MegaLabel _label;

		// Token: 0x04001253 RID: 4691
		[Nullable(2)]
		private string _optionName;

		// Token: 0x04001254 RID: 4692
		[Nullable(2)]
		private Tween _animInTween;

		// Token: 0x04001255 RID: 4693
		private Vector2 _showPosition;

		// Token: 0x04001256 RID: 4694
		private static readonly Vector2 _animOffsetPosition = new Vector2(0f, -50f);

		// Token: 0x04001257 RID: 4695
		[Nullable(2)]
		private Tween _currentTween;

		// Token: 0x04001258 RID: 4696
		private ShaderMaterial _hsv;

		// Token: 0x04001259 RID: 4697
		private Variant _hsvDefault = 0.9;

		// Token: 0x0400125A RID: 4698
		private Variant _hsvHover = 1.1;

		// Token: 0x0400125B RID: 4699
		private Variant _hsvDown = 0.7;

		// Token: 0x0400125C RID: 4700
		private static readonly Vector2 _defaultScale = Vector2.One;

		// Token: 0x0400125D RID: 4701
		private static readonly Vector2 _hoverScale = Vector2.One * 1.05f;

		// Token: 0x0400125E RID: 4702
		private static readonly Vector2 _downScale = Vector2.One * 0.95f;

		// Token: 0x0400125F RID: 4703
		private string[] _hotkeys;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F5 RID: 5621
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005511 RID: 21777
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005512 RID: 21778
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x04005513 RID: 21779
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005514 RID: 21780
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005515 RID: 21781
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005516 RID: 21782
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04005517 RID: 21783
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F6 RID: 5622
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005518 RID: 21784
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005519 RID: 21785
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400551A RID: 21786
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_optionName' field.
			/// </summary>
			// Token: 0x0400551B RID: 21787
			public static readonly StringName _optionName = "_optionName";

			/// <summary>
			/// Cached name for the '_animInTween' field.
			/// </summary>
			// Token: 0x0400551C RID: 21788
			public static readonly StringName _animInTween = "_animInTween";

			/// <summary>
			/// Cached name for the '_showPosition' field.
			/// </summary>
			// Token: 0x0400551D RID: 21789
			public static readonly StringName _showPosition = "_showPosition";

			/// <summary>
			/// Cached name for the '_currentTween' field.
			/// </summary>
			// Token: 0x0400551E RID: 21790
			public static readonly StringName _currentTween = "_currentTween";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x0400551F RID: 21791
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_hsvDefault' field.
			/// </summary>
			// Token: 0x04005520 RID: 21792
			public static readonly StringName _hsvDefault = "_hsvDefault";

			/// <summary>
			/// Cached name for the '_hsvHover' field.
			/// </summary>
			// Token: 0x04005521 RID: 21793
			public static readonly StringName _hsvHover = "_hsvHover";

			/// <summary>
			/// Cached name for the '_hsvDown' field.
			/// </summary>
			// Token: 0x04005522 RID: 21794
			public static readonly StringName _hsvDown = "_hsvDown";

			/// <summary>
			/// Cached name for the '_hotkeys' field.
			/// </summary>
			// Token: 0x04005523 RID: 21795
			public static readonly StringName _hotkeys = "_hotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F7 RID: 5623
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
