using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	/// <summary>
	/// The special roughly 200x200 (in 1080p) map icon for the Ancient room.
	/// TODO: Bump up asset to ~266x266 px (as we're targeting 1440p)
	/// NOTE: Outline is created as a 2px stroke for the 1080p asset and named the same as the icon with _outline appended.
	/// </summary>
	// Token: 0x020002EB RID: 747
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NAncientMapPoint.cs")]
	public class NAncientMapPoint : NMapPoint
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x00112928 File Offset: 0x00110B28
		protected override Color TraveledColor
		{
			get
			{
				return StsColors.pathDotTraveled;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x0011292F File Offset: 0x00110B2F
		protected override Color UntravelableColor
		{
			get
			{
				return StsColors.bossNodeUntraveled;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x00112936 File Offset: 0x00110B36
		protected override Color HoveredColor
		{
			get
			{
				return StsColors.pathDotTraveled;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x0011293D File Offset: 0x00110B3D
		protected override Vector2 HoverScale
		{
			get
			{
				return Vector2.One * 1.1f;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x0011294E File Offset: 0x00110B4E
		protected override Vector2 DownScale
		{
			get
			{
				return Vector2.One * 0.9f;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x0011295F File Offset: 0x00110B5F
		private static string UntravelableMaterialPath
		{
			get
			{
				return "res://materials/boss_map_point_unavailable.tres";
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x00112966 File Offset: 0x00110B66
		private static string AncientMapPointPath
		{
			get
			{
				return SceneHelper.GetScenePath("ui/ancient_map_point");
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x00112972 File Offset: 0x00110B72
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NAncientMapPoint.UntravelableMaterialPath,
					NAncientMapPoint.AncientMapPointPath
				});
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002819 RID: 10265 RVA: 0x0011298F File Offset: 0x00110B8F
		[Nullable(2)]
		private Material TargetMaterial
		{
			[NullableContext(2)]
			get
			{
				if (base.IsTravelable || base.State == MapPointState.Traveled)
				{
					return null;
				}
				return PreloadManager.Cache.GetMaterial(NAncientMapPoint.UntravelableMaterialPath);
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x001129B4 File Offset: 0x00110BB4
		public static NAncientMapPoint Create(MapPoint point, NMapScreen screen, IRunState runState)
		{
			NAncientMapPoint nancientMapPoint = PreloadManager.Cache.GetScene(NAncientMapPoint.AncientMapPointPath).Instantiate<NAncientMapPoint>(0L);
			nancientMapPoint.Point = point;
			nancientMapPoint._screen = screen;
			nancientMapPoint._runState = runState;
			return nancientMapPoint;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x001129F0 File Offset: 0x00110BF0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._icon = base.GetNode<TextureRect>("Icon");
			this._icon.Texture = this._runState.Act.Ancient.MapIcon;
			this._outline = base.GetNode<TextureRect>("Icon/Outline");
			this._outline.Texture = this._runState.Act.Ancient.MapIconOutline;
			this._outline.Modulate = this._runState.Act.MapBgColor;
			this.RefreshColorInstantly();
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x00112A90 File Offset: 0x00110C90
		public override void _Process(double delta)
		{
			if (this._isEnabled)
			{
				if (!base.IsFocused && base.IsInputAllowed())
				{
					this._elapsedTime += (float)delta * 4f;
					base.Scale = Vector2.One * (Mathf.Sin(this._elapsedTime) * 0.05f + 1f);
					return;
				}
				base.Scale = base.Scale.Lerp(Vector2.One, 0.5f);
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x00112B10 File Offset: 0x00110D10
		protected override void OnFocus()
		{
			base.OnFocus();
			if (!base.IsInputAllowed())
			{
				return;
			}
			this.AnimHover();
			if (NControllerManager.Instance.IsUsingController)
			{
				this._controllerSelectionReticle.OnSelect();
			}
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x00112B3E File Offset: 0x00110D3E
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this.AnimUnhover();
			this._controllerSelectionReticle.OnDeselect();
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x00112B57 File Offset: 0x00110D57
		protected override void OnPress()
		{
			if (!base.IsTravelable)
			{
				return;
			}
			this.AnimPressDown();
			this._controllerSelectionReticle.OnDeselect();
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x00112B74 File Offset: 0x00110D74
		public override void OnSelected()
		{
			base.State = MapPointState.Traveled;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(10L);
			this._tween.TweenProperty(this._icon, "self_modulate", base.TargetColor, 0.3).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._outline, "modulate", this._runState.Act.MapBgColor, 0.3).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x00112C6F File Offset: 0x00110E6F
		protected override void RefreshColorInstantly()
		{
			this._icon.SelfModulate = base.TargetColor;
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x00112C84 File Offset: 0x00110E84
		private void AnimHover()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", this.HoverScale, 0.05);
			this._tween.TweenProperty(this._icon, "self_modulate", this.HoveredColor, 0.05);
			if (base.IsTravelable)
			{
				this._tween.TweenProperty(this._outline, "modulate", this._outlineColor, 0.05);
			}
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x00112D4C File Offset: 0x00110F4C
		private void AnimUnhover()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._icon, "self_modulate", base.TargetColor, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._outline, "modulate", this._runState.Act.MapBgColor, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x00112E40 File Offset: 0x00111040
		private void AnimPressDown()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", this.DownScale, 0.3).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._outline, "modulate", this._runState.Act.MapBgColor, 0.5).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002825 RID: 10277 RVA: 0x00112EF8 File Offset: 0x001110F8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NAncientMapPoint.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAncientMapPoint.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName.OnSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName.AnimHover, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName.AnimUnhover, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientMapPoint.MethodName.AnimPressDown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002826 RID: 10278 RVA: 0x0011310C File Offset: 0x0011130C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAncientMapPoint.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.OnSelected && args.Count == 0)
			{
				this.OnSelected();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.RefreshColorInstantly && args.Count == 0)
			{
				this.RefreshColorInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.AnimHover && args.Count == 0)
			{
				this.AnimHover();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.AnimUnhover && args.Count == 0)
			{
				this.AnimUnhover();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientMapPoint.MethodName.AnimPressDown && args.Count == 0)
			{
				this.AnimPressDown();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002827 RID: 10279 RVA: 0x001132A4 File Offset: 0x001114A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAncientMapPoint.MethodName._Ready || (ref method) == NAncientMapPoint.MethodName._Process || (ref method) == NAncientMapPoint.MethodName.OnFocus || (ref method) == NAncientMapPoint.MethodName.OnUnfocus || (ref method) == NAncientMapPoint.MethodName.OnPress || (ref method) == NAncientMapPoint.MethodName.OnSelected || (ref method) == NAncientMapPoint.MethodName.RefreshColorInstantly || (ref method) == NAncientMapPoint.MethodName.AnimHover || (ref method) == NAncientMapPoint.MethodName.AnimUnhover || (ref method) == NAncientMapPoint.MethodName.AnimPressDown || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002828 RID: 10280 RVA: 0x00113350 File Offset: 0x00111550
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAncientMapPoint.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName._elapsedTime)
			{
				this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002829 RID: 10281 RVA: 0x001133D4 File Offset: 0x001115D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAncientMapPoint.PropertyName.TraveledColor)
			{
				Color color = this.TraveledColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName.UntravelableColor)
			{
				Color color = this.UntravelableColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName.HoveredColor)
			{
				Color color = this.HoveredColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName.HoverScale)
			{
				Vector2 vector = this.HoverScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName.DownScale)
			{
				Vector2 vector = this.DownScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName.TargetMaterial)
			{
				Material targetMaterial = this.TargetMaterial;
				value = VariantUtils.CreateFrom<Material>(ref targetMaterial);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NAncientMapPoint.PropertyName._elapsedTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600282A RID: 10282 RVA: 0x0011353C File Offset: 0x0011173C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAncientMapPoint.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientMapPoint.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientMapPoint.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(20L, NAncientMapPoint.PropertyName.TraveledColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NAncientMapPoint.PropertyName.UntravelableColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NAncientMapPoint.PropertyName.HoveredColor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientMapPoint.PropertyName.HoverScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientMapPoint.PropertyName.DownScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NAncientMapPoint.PropertyName._elapsedTime, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientMapPoint.PropertyName.TargetMaterial, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600282B RID: 10283 RVA: 0x00113698 File Offset: 0x00111898
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAncientMapPoint.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NAncientMapPoint.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
			info.AddProperty(NAncientMapPoint.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NAncientMapPoint.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
		}

		/// <inheritdoc />
		// Token: 0x0600282C RID: 10284 RVA: 0x00113704 File Offset: 0x00111904
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAncientMapPoint.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAncientMapPoint.PropertyName._outline, ref variant2))
			{
				this._outline = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAncientMapPoint.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAncientMapPoint.PropertyName._elapsedTime, ref variant4))
			{
				this._elapsedTime = variant4.As<float>();
			}
		}

		// Token: 0x04000EE8 RID: 3816
		private TextureRect _icon;

		// Token: 0x04000EE9 RID: 3817
		private TextureRect _outline;

		// Token: 0x04000EEA RID: 3818
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000EEB RID: 3819
		private const float _pulseSpeed = 4f;

		// Token: 0x04000EEC RID: 3820
		private const float _scaleAmount = 0.05f;

		// Token: 0x04000EED RID: 3821
		private const float _scaleBase = 1f;

		// Token: 0x04000EEE RID: 3822
		private float _elapsedTime = Rng.Chaotic.NextFloat(3140f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001492 RID: 5266
		[NullableContext(0)]
		public new class MethodName : NMapPoint.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E0F RID: 19983
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004E10 RID: 19984
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004E11 RID: 19985
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004E12 RID: 19986
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004E13 RID: 19987
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnSelected' method.
			/// </summary>
			// Token: 0x04004E14 RID: 19988
			public new static readonly StringName OnSelected = "OnSelected";

			/// <summary>
			/// Cached name for the 'RefreshColorInstantly' method.
			/// </summary>
			// Token: 0x04004E15 RID: 19989
			public new static readonly StringName RefreshColorInstantly = "RefreshColorInstantly";

			/// <summary>
			/// Cached name for the 'AnimHover' method.
			/// </summary>
			// Token: 0x04004E16 RID: 19990
			public static readonly StringName AnimHover = "AnimHover";

			/// <summary>
			/// Cached name for the 'AnimUnhover' method.
			/// </summary>
			// Token: 0x04004E17 RID: 19991
			public static readonly StringName AnimUnhover = "AnimUnhover";

			/// <summary>
			/// Cached name for the 'AnimPressDown' method.
			/// </summary>
			// Token: 0x04004E18 RID: 19992
			public static readonly StringName AnimPressDown = "AnimPressDown";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001493 RID: 5267
		[NullableContext(0)]
		public new class PropertyName : NMapPoint.PropertyName
		{
			/// <summary>
			/// Cached name for the 'TraveledColor' property.
			/// </summary>
			// Token: 0x04004E19 RID: 19993
			public new static readonly StringName TraveledColor = "TraveledColor";

			/// <summary>
			/// Cached name for the 'UntravelableColor' property.
			/// </summary>
			// Token: 0x04004E1A RID: 19994
			public new static readonly StringName UntravelableColor = "UntravelableColor";

			/// <summary>
			/// Cached name for the 'HoveredColor' property.
			/// </summary>
			// Token: 0x04004E1B RID: 19995
			public new static readonly StringName HoveredColor = "HoveredColor";

			/// <summary>
			/// Cached name for the 'HoverScale' property.
			/// </summary>
			// Token: 0x04004E1C RID: 19996
			public new static readonly StringName HoverScale = "HoverScale";

			/// <summary>
			/// Cached name for the 'DownScale' property.
			/// </summary>
			// Token: 0x04004E1D RID: 19997
			public new static readonly StringName DownScale = "DownScale";

			/// <summary>
			/// Cached name for the 'TargetMaterial' property.
			/// </summary>
			// Token: 0x04004E1E RID: 19998
			public static readonly StringName TargetMaterial = "TargetMaterial";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004E1F RID: 19999
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04004E20 RID: 20000
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004E21 RID: 20001
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_elapsedTime' field.
			/// </summary>
			// Token: 0x04004E22 RID: 20002
			public static readonly StringName _elapsedTime = "_elapsedTime";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001494 RID: 5268
		[NullableContext(0)]
		public new class SignalName : NMapPoint.SignalName
		{
		}
	}
}
