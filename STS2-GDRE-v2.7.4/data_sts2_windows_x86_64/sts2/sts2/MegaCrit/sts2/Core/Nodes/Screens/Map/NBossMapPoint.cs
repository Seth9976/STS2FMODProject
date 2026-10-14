using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002ED RID: 749
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NBossMapPoint.cs")]
	public class NBossMapPoint : NMapPoint
	{
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x001137BF File Offset: 0x001119BF
		protected override Color TraveledColor
		{
			get
			{
				return StsColors.pathDotTraveled;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x001137C6 File Offset: 0x001119C6
		protected override Color UntravelableColor
		{
			get
			{
				return StsColors.red;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x001137CD File Offset: 0x001119CD
		protected override Color HoveredColor
		{
			get
			{
				return StsColors.pathDotTraveled;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x001137D4 File Offset: 0x001119D4
		protected override Vector2 HoverScale
		{
			get
			{
				return Vector2.One * 1.05f;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002835 RID: 10293 RVA: 0x001137E5 File Offset: 0x001119E5
		protected override Vector2 DownScale
		{
			get
			{
				return Vector2.One * 1.02f;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002836 RID: 10294 RVA: 0x001137F6 File Offset: 0x001119F6
		private static string BossMapPointPath
		{
			get
			{
				return SceneHelper.GetScenePath("ui/boss_map_point");
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002837 RID: 10295 RVA: 0x00113802 File Offset: 0x00111A02
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NBossMapPoint.BossMapPointPath);
			}
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x00113810 File Offset: 0x00111A10
		public static NBossMapPoint Create(MapPoint point, NMapScreen screen, IRunState runState)
		{
			NBossMapPoint nbossMapPoint = PreloadManager.Cache.GetScene(NBossMapPoint.BossMapPointPath).Instantiate<NBossMapPoint>(0L);
			nbossMapPoint.Point = point;
			nbossMapPoint._screen = screen;
			nbossMapPoint._runState = runState;
			nbossMapPoint._act = runState.Act;
			return nbossMapPoint;
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x00113858 File Offset: 0x00111A58
		public override void _Ready()
		{
			this.ConnectSignals();
			base.Disable();
			this._spriteContainer = base.GetNode<Node2D>("%SpriteContainer");
			this._spineSprite = base.GetNode<Node2D>("%SpineSprite");
			this._animController = new MegaSprite(this._spineSprite);
			EncounterModel encounterModel = ((base.Point == this._runState.Map.SecondBossMapPoint) ? this._runState.Act.SecondBossEncounter : this._runState.Act.BossEncounter);
			if (encounterModel.BossNodeSpineResource != null)
			{
				this._usesSpine = true;
				this._spineSprite.Visible = true;
				this._animController.SetSkeletonDataRes(encounterModel.BossNodeSpineResource);
				this._animController.GetAnimationState().AddAnimation("animation", 0f, true, 0);
				this._material = (ShaderMaterial)this._animController.GetNormalMaterial();
			}
			else
			{
				this._usesSpine = false;
				this._spineSprite.Visible = false;
				this._placeholderImage = base.GetNode<TextureRect>("%PlaceholderImage");
				this._placeholderOutline = base.GetNode<TextureRect>("%PlaceholderOutline");
				this._placeholderImage.Visible = true;
				this._placeholderImage.Texture = PreloadManager.Cache.GetAsset<Texture2D>(encounterModel.BossNodePath + ".png");
				this._placeholderOutline.Texture = PreloadManager.Cache.GetAsset<Texture2D>(encounterModel.BossNodePath + "_outline.png");
				this._placeholderImage.SelfModulate = this._act.MapTraveledColor;
				this._placeholderOutline.SelfModulate = this._act.MapBgColor;
			}
			this.RefreshColorInstantly();
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x00113A1C File Offset: 0x00111C1C
		protected override void OnFocus()
		{
			base.OnFocus();
			if (!base.IsInputAllowed())
			{
				return;
			}
			if (!base.IsTravelable)
			{
				return;
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._spriteContainer, "scale", this.HoverScale, 0.05);
			bool usesSpine = this._usesSpine;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x00113A9C File Offset: 0x00111C9C
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._spriteContainer, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(7L);
			bool usesSpine = this._usesSpine;
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x00113B18 File Offset: 0x00111D18
		protected override void OnPress()
		{
			if (!base.IsTravelable)
			{
				return;
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._spriteContainer, "scale", this.DownScale, 0.3).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00113B8F File Offset: 0x00111D8F
		public override void OnSelected()
		{
			base.State = MapPointState.Traveled;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x00113B98 File Offset: 0x00111D98
		protected override void RefreshColorInstantly()
		{
			MapPointState mapPointState;
			bool flag;
			if (this._usesSpine)
			{
				mapPointState = base.State;
				flag = mapPointState - MapPointState.Travelable <= 1;
				if (flag)
				{
					this._material.SetShaderParameter(NBossMapPoint._blackLayerColor, this._act.MapTraveledColor);
				}
				else
				{
					this._material.SetShaderParameter(NBossMapPoint._blackLayerColor, this._act.MapUntraveledColor);
				}
				this._material.SetShaderParameter(NBossMapPoint._mapColor, this._act.MapBgColor);
				return;
			}
			mapPointState = base.State;
			flag = mapPointState - MapPointState.Travelable <= 1;
			Color color = (flag ? this._act.MapTraveledColor : this._act.MapUntraveledColor);
			this._placeholderImage.SelfModulate = color;
			this._placeholderOutline.SelfModulate = this._act.MapBgColor;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600283F RID: 10303 RVA: 0x00113C78 File Offset: 0x00111E78
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NBossMapPoint.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBossMapPoint.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBossMapPoint.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBossMapPoint.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBossMapPoint.MethodName.OnSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBossMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002840 RID: 10304 RVA: 0x00113DA8 File Offset: 0x00111FA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBossMapPoint.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBossMapPoint.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBossMapPoint.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBossMapPoint.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBossMapPoint.MethodName.OnSelected && args.Count == 0)
			{
				this.OnSelected();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBossMapPoint.MethodName.RefreshColorInstantly && args.Count == 0)
			{
				this.RefreshColorInstantly();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002841 RID: 10305 RVA: 0x00113E9C File Offset: 0x0011209C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBossMapPoint.MethodName._Ready || (ref method) == NBossMapPoint.MethodName.OnFocus || (ref method) == NBossMapPoint.MethodName.OnUnfocus || (ref method) == NBossMapPoint.MethodName.OnPress || (ref method) == NBossMapPoint.MethodName.OnSelected || (ref method) == NBossMapPoint.MethodName.RefreshColorInstantly || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002842 RID: 10306 RVA: 0x00113F0C File Offset: 0x0011210C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBossMapPoint.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._usesSpine)
			{
				this._usesSpine = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._spriteContainer)
			{
				this._spriteContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._spineSprite)
			{
				this._spineSprite = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._material)
			{
				this._material = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._placeholderImage)
			{
				this._placeholderImage = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._placeholderOutline)
			{
				this._placeholderOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002843 RID: 10307 RVA: 0x00113FE0 File Offset: 0x001121E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBossMapPoint.PropertyName.TraveledColor)
			{
				Color color = this.TraveledColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName.UntravelableColor)
			{
				Color color = this.UntravelableColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName.HoveredColor)
			{
				Color color = this.HoveredColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName.HoverScale)
			{
				Vector2 vector = this.HoverScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName.DownScale)
			{
				Vector2 vector = this.DownScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._usesSpine)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._usesSpine);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._spriteContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._spriteContainer);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._spineSprite)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._spineSprite);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._material)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._material);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._placeholderImage)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._placeholderImage);
				return true;
			}
			if ((ref name) == NBossMapPoint.PropertyName._placeholderOutline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._placeholderOutline);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002844 RID: 10308 RVA: 0x00114184 File Offset: 0x00112384
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(20L, NBossMapPoint.PropertyName.TraveledColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBossMapPoint.PropertyName.UntravelableColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBossMapPoint.PropertyName.HoveredColor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBossMapPoint.PropertyName.HoverScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBossMapPoint.PropertyName.DownScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBossMapPoint.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBossMapPoint.PropertyName._usesSpine, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBossMapPoint.PropertyName._spriteContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBossMapPoint.PropertyName._spineSprite, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBossMapPoint.PropertyName._material, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBossMapPoint.PropertyName._placeholderImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBossMapPoint.PropertyName._placeholderOutline, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002845 RID: 10309 RVA: 0x00114324 File Offset: 0x00112524
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBossMapPoint.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NBossMapPoint.PropertyName._usesSpine, Variant.From<bool>(ref this._usesSpine));
			info.AddProperty(NBossMapPoint.PropertyName._spriteContainer, Variant.From<Node2D>(ref this._spriteContainer));
			info.AddProperty(NBossMapPoint.PropertyName._spineSprite, Variant.From<Node2D>(ref this._spineSprite));
			info.AddProperty(NBossMapPoint.PropertyName._material, Variant.From<ShaderMaterial>(ref this._material));
			info.AddProperty(NBossMapPoint.PropertyName._placeholderImage, Variant.From<TextureRect>(ref this._placeholderImage));
			info.AddProperty(NBossMapPoint.PropertyName._placeholderOutline, Variant.From<TextureRect>(ref this._placeholderOutline));
		}

		/// <inheritdoc />
		// Token: 0x06002846 RID: 10310 RVA: 0x001143D4 File Offset: 0x001125D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBossMapPoint.PropertyName._hoverTween, ref variant))
			{
				this._hoverTween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBossMapPoint.PropertyName._usesSpine, ref variant2))
			{
				this._usesSpine = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBossMapPoint.PropertyName._spriteContainer, ref variant3))
			{
				this._spriteContainer = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBossMapPoint.PropertyName._spineSprite, ref variant4))
			{
				this._spineSprite = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBossMapPoint.PropertyName._material, ref variant5))
			{
				this._material = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NBossMapPoint.PropertyName._placeholderImage, ref variant6))
			{
				this._placeholderImage = variant6.As<TextureRect>();
			}
			Variant variant7;
			if (info.TryGetProperty(NBossMapPoint.PropertyName._placeholderOutline, ref variant7))
			{
				this._placeholderOutline = variant7.As<TextureRect>();
			}
		}

		// Token: 0x04000EEF RID: 3823
		private static readonly StringName _mapColor = new StringName("map_color");

		// Token: 0x04000EF0 RID: 3824
		private static readonly StringName _blackLayerColor = new StringName("black_layer_color");

		// Token: 0x04000EF1 RID: 3825
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04000EF2 RID: 3826
		private ActModel _act;

		// Token: 0x04000EF3 RID: 3827
		private bool _usesSpine;

		// Token: 0x04000EF4 RID: 3828
		private Node2D _spriteContainer;

		// Token: 0x04000EF5 RID: 3829
		private Node2D _spineSprite;

		// Token: 0x04000EF6 RID: 3830
		private MegaSprite _animController;

		// Token: 0x04000EF7 RID: 3831
		private ShaderMaterial _material;

		// Token: 0x04000EF8 RID: 3832
		private TextureRect _placeholderImage;

		// Token: 0x04000EF9 RID: 3833
		private TextureRect _placeholderOutline;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001498 RID: 5272
		[NullableContext(0)]
		public new class MethodName : NMapPoint.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E23 RID: 20003
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004E24 RID: 20004
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004E25 RID: 20005
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004E26 RID: 20006
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnSelected' method.
			/// </summary>
			// Token: 0x04004E27 RID: 20007
			public new static readonly StringName OnSelected = "OnSelected";

			/// <summary>
			/// Cached name for the 'RefreshColorInstantly' method.
			/// </summary>
			// Token: 0x04004E28 RID: 20008
			public new static readonly StringName RefreshColorInstantly = "RefreshColorInstantly";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001499 RID: 5273
		[NullableContext(0)]
		public new class PropertyName : NMapPoint.PropertyName
		{
			/// <summary>
			/// Cached name for the 'TraveledColor' property.
			/// </summary>
			// Token: 0x04004E29 RID: 20009
			public new static readonly StringName TraveledColor = "TraveledColor";

			/// <summary>
			/// Cached name for the 'UntravelableColor' property.
			/// </summary>
			// Token: 0x04004E2A RID: 20010
			public new static readonly StringName UntravelableColor = "UntravelableColor";

			/// <summary>
			/// Cached name for the 'HoveredColor' property.
			/// </summary>
			// Token: 0x04004E2B RID: 20011
			public new static readonly StringName HoveredColor = "HoveredColor";

			/// <summary>
			/// Cached name for the 'HoverScale' property.
			/// </summary>
			// Token: 0x04004E2C RID: 20012
			public new static readonly StringName HoverScale = "HoverScale";

			/// <summary>
			/// Cached name for the 'DownScale' property.
			/// </summary>
			// Token: 0x04004E2D RID: 20013
			public new static readonly StringName DownScale = "DownScale";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04004E2E RID: 20014
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_usesSpine' field.
			/// </summary>
			// Token: 0x04004E2F RID: 20015
			public static readonly StringName _usesSpine = "_usesSpine";

			/// <summary>
			/// Cached name for the '_spriteContainer' field.
			/// </summary>
			// Token: 0x04004E30 RID: 20016
			public static readonly StringName _spriteContainer = "_spriteContainer";

			/// <summary>
			/// Cached name for the '_spineSprite' field.
			/// </summary>
			// Token: 0x04004E31 RID: 20017
			public static readonly StringName _spineSprite = "_spineSprite";

			/// <summary>
			/// Cached name for the '_material' field.
			/// </summary>
			// Token: 0x04004E32 RID: 20018
			public static readonly StringName _material = "_material";

			/// <summary>
			/// Cached name for the '_placeholderImage' field.
			/// </summary>
			// Token: 0x04004E33 RID: 20019
			public static readonly StringName _placeholderImage = "_placeholderImage";

			/// <summary>
			/// Cached name for the '_placeholderOutline' field.
			/// </summary>
			// Token: 0x04004E34 RID: 20020
			public static readonly StringName _placeholderOutline = "_placeholderOutline";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200149A RID: 5274
		[NullableContext(0)]
		public new class SignalName : NMapPoint.SignalName
		{
		}
	}
}
