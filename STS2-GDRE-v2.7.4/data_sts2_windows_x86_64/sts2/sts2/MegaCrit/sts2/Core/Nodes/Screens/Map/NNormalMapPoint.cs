using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002FB RID: 763
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NNormalMapPoint.cs")]
	public class NNormalMapPoint : NMapPoint
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002991 RID: 10641 RVA: 0x00120F35 File Offset: 0x0011F135
		protected override Color TraveledColor
		{
			get
			{
				return Colors.White;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002992 RID: 10642 RVA: 0x00120F3C File Offset: 0x0011F13C
		protected override Color UntravelableColor
		{
			get
			{
				return StsColors.halfTransparentWhite;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002993 RID: 10643 RVA: 0x00120F43 File Offset: 0x0011F143
		protected override Color HoveredColor
		{
			get
			{
				return Colors.White;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x00120F4A File Offset: 0x0011F14A
		protected override Vector2 HoverScale
		{
			get
			{
				return Vector2.One * 1.45f;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002995 RID: 10645 RVA: 0x00120F5B File Offset: 0x0011F15B
		protected override Vector2 DownScale
		{
			get
			{
				return Vector2.One * 0.9f;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x00120F6C File Offset: 0x0011F16C
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/ui/normal_map_point");
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002997 RID: 10647 RVA: 0x00120F78 File Offset: 0x0011F178
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NNormalMapPoint.ScenePath);
			}
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x00120F84 File Offset: 0x0011F184
		private static string IconName(MapPointType pointType)
		{
			string text;
			switch (pointType)
			{
			case MapPointType.Unassigned:
				text = "map_unknown";
				break;
			case MapPointType.Unknown:
				text = "map_unknown";
				break;
			case MapPointType.Shop:
				text = "map_shop";
				break;
			case MapPointType.Treasure:
				text = "map_chest";
				break;
			case MapPointType.RestSite:
				text = "map_rest";
				break;
			case MapPointType.Monster:
				text = "map_monster";
				break;
			case MapPointType.Elite:
				text = "map_elite";
				break;
			case MapPointType.Boss:
				text = string.Empty;
				break;
			case MapPointType.Ancient:
				text = "map_unknown";
				break;
			default:
				throw new ArgumentOutOfRangeException(pointType.ToString());
			}
			return text;
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x00121019 File Offset: 0x0011F219
		private static string IconPath(string filename)
		{
			return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/map/icons/" + filename + ".tres");
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x00121030 File Offset: 0x0011F230
		private static string OutlinePath(string filename)
		{
			return ImageHelper.GetImagePath("atlases/compressed.sprites/map/" + filename + "_outline.tres");
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x00121048 File Offset: 0x0011F248
		private static string UnknownIconPath(RoomType pointType)
		{
			string text;
			switch (pointType)
			{
			case RoomType.Monster:
				text = "unknown_monster";
				goto IL_0044;
			case RoomType.Elite:
				text = "unknown_elite";
				goto IL_0044;
			case RoomType.Treasure:
				text = "unknown_chest";
				goto IL_0044;
			case RoomType.Shop:
				text = "unknown_shop";
				goto IL_0044;
			}
			text = "unknown";
			IL_0044:
			string text2 = text;
			return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/map/icons/map_" + text2 + ".tres");
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x001210B0 File Offset: 0x0011F2B0
		private static string UnknownOutlinePath(RoomType pointType)
		{
			string text;
			switch (pointType)
			{
			case RoomType.Monster:
				text = "map_monster";
				goto IL_0044;
			case RoomType.Elite:
				text = "map_elite";
				goto IL_0044;
			case RoomType.Treasure:
				text = "map_chest";
				goto IL_0044;
			case RoomType.Shop:
				text = "map_shop";
				goto IL_0044;
			}
			text = "map_unknown";
			IL_0044:
			string text2 = text;
			return NNormalMapPoint.OutlinePath(text2);
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0012110C File Offset: 0x0011F30C
		public static NNormalMapPoint Create(MapPoint point, NMapScreen screen, IRunState runState)
		{
			NNormalMapPoint nnormalMapPoint = PreloadManager.Cache.GetScene(NNormalMapPoint.ScenePath).Instantiate<NNormalMapPoint>(0L);
			nnormalMapPoint.Point = point;
			nnormalMapPoint._screen = screen;
			nnormalMapPoint._runState = runState;
			return nnormalMapPoint;
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x00121148 File Offset: 0x0011F348
		public override void _Ready()
		{
			this.ConnectSignals();
			this._iconContainer = base.GetNode<Control>("%IconContainer");
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._outline = base.GetNode<TextureRect>("%Outline");
			this._questIcon = base.GetNode<TextureRect>("%QuestIcon");
			this.UpdateIcon();
			Color mapBgColor = this._runState.Act.MapBgColor;
			this._outline.Modulate = mapBgColor;
			ShaderMaterial shaderMaterial = (ShaderMaterial)this._icon.Material;
			shaderMaterial.SetShaderParameter(NNormalMapPoint._mapColor, mapBgColor.Lerp(Colors.Gray, 0.5f));
			this.RefreshMarkedIconVisibility();
			this.RefreshColorInstantly();
			base.Disable();
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x0012121A File Offset: 0x0011F41A
		public override void _EnterTree()
		{
			base.Point.NodeMarkedChanged += this.RefreshMarkedIconVisibility;
			NMapScreen.Instance.PointTypeHighlighted += this.OnHighlightPointType;
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x00121249 File Offset: 0x0011F449
		private void RefreshMarkedIconVisibility()
		{
			this._questIcon.Visible = base.Point.Quests.Count > 0;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x00121269 File Offset: 0x0011F469
		public override void _ExitTree()
		{
			base.Point.NodeMarkedChanged -= this.RefreshMarkedIconVisibility;
			NMapScreen.Instance.PointTypeHighlighted -= this.OnHighlightPointType;
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x00121298 File Offset: 0x0011F498
		public override void _Process(double delta)
		{
			if (this._isEnabled)
			{
				if (!base.IsFocused && base.IsInputAllowed())
				{
					this._elapsedTime += (float)delta * 4f;
					this._iconContainer.Scale = Vector2.One * (Mathf.Sin(this._elapsedTime) * 0.25f + 1.2f);
					return;
				}
				this._iconContainer.Scale = this._iconContainer.Scale.Lerp(Vector2.One, 0.5f);
			}
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x00121328 File Offset: 0x0011F528
		public override void OnSelected()
		{
			this.ShowCircleVfx(true);
			base.State = MapPointState.Traveled;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._iconContainer, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(10L);
			this._tween.TweenProperty(this._questIcon, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(10L);
			this._tween.TweenProperty(this._icon, "self_modulate", base.TargetColor, 0.3).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._outline, "modulate", this._runState.Act.MapBgColor, 0.3).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x001214A5 File Offset: 0x0011F6A5
		protected override void OnFocus()
		{
			base.OnFocus();
			if (!base.IsInputAllowed())
			{
				return;
			}
			this.AnimHover();
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x001214BC File Offset: 0x0011F6BC
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			NHoverTipSet.Remove(this);
			if (this._isEnabled)
			{
				this._elapsedTime = 3.926991f;
			}
			this.AnimUnhover();
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x001214E3 File Offset: 0x0011F6E3
		protected override void OnPress()
		{
			if (!base.IsTravelable)
			{
				return;
			}
			this.AnimPressDown();
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x001214FA File Offset: 0x0011F6FA
		public void SetAngle(float degrees)
		{
			this._iconContainer.RotationDegrees = degrees;
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x00121508 File Offset: 0x0011F708
		protected override void RefreshColorInstantly()
		{
			this._icon.SelfModulate = base.TargetColor;
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x0012151B File Offset: 0x0011F71B
		protected override void RefreshState()
		{
			base.RefreshState();
			this.UpdateIcon();
			if (base.State == MapPointState.Traveled)
			{
				this.ShowCircleVfx(false);
			}
			if (!base.IsFocused)
			{
				this._iconContainer.Scale = Vector2.One;
			}
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x00121554 File Offset: 0x0011F754
		private void UpdateIcon()
		{
			if (base.Point.PointType != MapPointType.Unknown || base.State != MapPointState.Traveled)
			{
				string text = NNormalMapPoint.IconName(base.Point.PointType);
				string text2 = NNormalMapPoint.IconPath(text);
				this._icon.Texture = ResourceLoader.Load<Texture2D>(text2, null, 1L);
				string text3 = NNormalMapPoint.OutlinePath(NNormalMapPoint.IconName(base.Point.PointType));
				this._outline.Texture = ResourceLoader.Load<Texture2D>(text3, null, 1L);
				return;
			}
			if (this._runState.MapPointHistory.Count <= this._runState.CurrentActIndex)
			{
				return;
			}
			IReadOnlyList<MapPointHistoryEntry> readOnlyList = this._runState.MapPointHistory[this._runState.CurrentActIndex];
			if (readOnlyList.Count <= base.Point.coord.row)
			{
				return;
			}
			RoomType roomType = readOnlyList[base.Point.coord.row].Rooms.First<MapPointRoomHistoryEntry>().RoomType;
			this._icon.Texture = ResourceLoader.Load<Texture2D>(NNormalMapPoint.UnknownIconPath(roomType), null, 1L);
			this._outline.Texture = ResourceLoader.Load<Texture2D>(NNormalMapPoint.UnknownOutlinePath(roomType), null, 1L);
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x00121680 File Offset: 0x0011F880
		private void ShowCircleVfx(bool playAnim)
		{
			if (this._circleVfx != null)
			{
				return;
			}
			this._circleVfx = NMapCircleVfx.Create(this._runState, base.Point.coord, playAnim);
			this.AddChildSafely(this._circleVfx);
			this._circleVfx.Position += base.PivotOffset;
			this.MoveChildSafely(this._circleVfx, base.VoteContainer.GetIndex(false));
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x001216F4 File Offset: 0x0011F8F4
		private void AnimHover()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", this.HoverScale, 0.05);
			this._tween.TweenProperty(this._questIcon, "scale", this.HoverScale, 0.05);
			this._tween.TweenProperty(this._icon, "self_modulate", this.HoveredColor, 0.05);
			if (base.IsTravelable)
			{
				this._tween.TweenProperty(this._outline, "modulate", this._outlineColor, 0.05);
			}
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x001217EC File Offset: 0x0011F9EC
		private void AnimUnhover()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._questIcon, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._icon, "self_modulate", base.TargetColor, 0.5).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._outline, "modulate", this._runState.Act.MapBgColor, 0.5).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x0012191C File Offset: 0x0011FB1C
		private void AnimPressDown()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", this.DownScale, 0.3).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._questIcon, "scale", this.DownScale, 0.3).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._outline, "modulate", this._runState.Act.MapBgColor, 0.3).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x00121A10 File Offset: 0x0011FC10
		private void OnHighlightPointType(MapPointType pointType)
		{
			if (pointType == base.Point.PointType)
			{
				this.AnimHover();
				return;
			}
			this.AnimUnhover();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060029B0 RID: 10672 RVA: 0x00121A30 File Offset: 0x0011FC30
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(23)
			{
				new MethodInfo(NNormalMapPoint.MethodName.IconName, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pointType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName.IconPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "filename", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName.OutlinePath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "filename", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName.UnknownIconPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pointType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName.UnknownOutlinePath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pointType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.RefreshMarkedIconVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName.OnSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.SetAngle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "degrees", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.RefreshState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.UpdateIcon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.ShowCircleVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "playAnim", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NNormalMapPoint.MethodName.AnimHover, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.AnimUnhover, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.AnimPressDown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNormalMapPoint.MethodName.OnHighlightPointType, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pointType", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029B1 RID: 10673 RVA: 0x00121FD4 File Offset: 0x001201D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NNormalMapPoint.MethodName.IconName && args.Count == 1)
			{
				string text = NNormalMapPoint.IconName(VariantUtils.ConvertTo<MapPointType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.IconPath && args.Count == 1)
			{
				string text2 = NNormalMapPoint.IconPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text2);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.OutlinePath && args.Count == 1)
			{
				string text3 = NNormalMapPoint.OutlinePath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text3);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.UnknownIconPath && args.Count == 1)
			{
				string text4 = NNormalMapPoint.UnknownIconPath(VariantUtils.ConvertTo<RoomType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text4);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.UnknownOutlinePath && args.Count == 1)
			{
				string text5 = NNormalMapPoint.UnknownOutlinePath(VariantUtils.ConvertTo<RoomType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text5);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.RefreshMarkedIconVisibility && args.Count == 0)
			{
				this.RefreshMarkedIconVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.OnSelected && args.Count == 0)
			{
				this.OnSelected();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.SetAngle && args.Count == 1)
			{
				this.SetAngle(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.RefreshColorInstantly && args.Count == 0)
			{
				this.RefreshColorInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.RefreshState && args.Count == 0)
			{
				this.RefreshState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.UpdateIcon && args.Count == 0)
			{
				this.UpdateIcon();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.ShowCircleVfx && args.Count == 1)
			{
				this.ShowCircleVfx(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.AnimHover && args.Count == 0)
			{
				this.AnimHover();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.AnimUnhover && args.Count == 0)
			{
				this.AnimUnhover();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.AnimPressDown && args.Count == 0)
			{
				this.AnimPressDown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.OnHighlightPointType && args.Count == 1)
			{
				this.OnHighlightPointType(VariantUtils.ConvertTo<MapPointType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x001223DC File Offset: 0x001205DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NNormalMapPoint.MethodName.IconName && args.Count == 1)
			{
				string text = NNormalMapPoint.IconName(VariantUtils.ConvertTo<MapPointType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.IconPath && args.Count == 1)
			{
				string text2 = NNormalMapPoint.IconPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text2);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.OutlinePath && args.Count == 1)
			{
				string text3 = NNormalMapPoint.OutlinePath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text3);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.UnknownIconPath && args.Count == 1)
			{
				string text4 = NNormalMapPoint.UnknownIconPath(VariantUtils.ConvertTo<RoomType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text4);
				return true;
			}
			if ((ref method) == NNormalMapPoint.MethodName.UnknownOutlinePath && args.Count == 1)
			{
				string text5 = NNormalMapPoint.UnknownOutlinePath(VariantUtils.ConvertTo<RoomType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text5);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060029B3 RID: 10675 RVA: 0x00122510 File Offset: 0x00120710
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NNormalMapPoint.MethodName.IconName || (ref method) == NNormalMapPoint.MethodName.IconPath || (ref method) == NNormalMapPoint.MethodName.OutlinePath || (ref method) == NNormalMapPoint.MethodName.UnknownIconPath || (ref method) == NNormalMapPoint.MethodName.UnknownOutlinePath || (ref method) == NNormalMapPoint.MethodName._Ready || (ref method) == NNormalMapPoint.MethodName._EnterTree || (ref method) == NNormalMapPoint.MethodName.RefreshMarkedIconVisibility || (ref method) == NNormalMapPoint.MethodName._ExitTree || (ref method) == NNormalMapPoint.MethodName._Process || (ref method) == NNormalMapPoint.MethodName.OnSelected || (ref method) == NNormalMapPoint.MethodName.OnFocus || (ref method) == NNormalMapPoint.MethodName.OnUnfocus || (ref method) == NNormalMapPoint.MethodName.OnPress || (ref method) == NNormalMapPoint.MethodName.SetAngle || (ref method) == NNormalMapPoint.MethodName.RefreshColorInstantly || (ref method) == NNormalMapPoint.MethodName.RefreshState || (ref method) == NNormalMapPoint.MethodName.UpdateIcon || (ref method) == NNormalMapPoint.MethodName.ShowCircleVfx || (ref method) == NNormalMapPoint.MethodName.AnimHover || (ref method) == NNormalMapPoint.MethodName.AnimUnhover || (ref method) == NNormalMapPoint.MethodName.AnimPressDown || (ref method) == NNormalMapPoint.MethodName.OnHighlightPointType || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060029B4 RID: 10676 RVA: 0x00122680 File Offset: 0x00120880
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NNormalMapPoint.PropertyName._iconContainer)
			{
				this._iconContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._questIcon)
			{
				this._questIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._circleVfx)
			{
				this._circleVfx = VariantUtils.ConvertTo<NMapCircleVfx>(ref value);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._pulseTween)
			{
				this._pulseTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._elapsedTime)
			{
				this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060029B5 RID: 10677 RVA: 0x00122770 File Offset: 0x00120970
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NNormalMapPoint.PropertyName.TraveledColor)
			{
				Color color = this.TraveledColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName.UntravelableColor)
			{
				Color color = this.UntravelableColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName.HoveredColor)
			{
				Color color = this.HoveredColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName.HoverScale)
			{
				Vector2 vector = this.HoverScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName.DownScale)
			{
				Vector2 vector = this.DownScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._iconContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._iconContainer);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._questIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._questIcon);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._circleVfx)
			{
				value = VariantUtils.CreateFrom<NMapCircleVfx>(ref this._circleVfx);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._pulseTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._pulseTween);
				return true;
			}
			if ((ref name) == NNormalMapPoint.PropertyName._elapsedTime)
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
		// Token: 0x060029B6 RID: 10678 RVA: 0x00122934 File Offset: 0x00120B34
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NNormalMapPoint.PropertyName._iconContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNormalMapPoint.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNormalMapPoint.PropertyName._questIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNormalMapPoint.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNormalMapPoint.PropertyName._circleVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNormalMapPoint.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(20L, NNormalMapPoint.PropertyName.TraveledColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NNormalMapPoint.PropertyName.UntravelableColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NNormalMapPoint.PropertyName.HoveredColor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NNormalMapPoint.PropertyName.HoverScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NNormalMapPoint.PropertyName.DownScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNormalMapPoint.PropertyName._pulseTween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NNormalMapPoint.PropertyName._elapsedTime, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029B7 RID: 10679 RVA: 0x00122AF4 File Offset: 0x00120CF4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NNormalMapPoint.PropertyName._iconContainer, Variant.From<Control>(ref this._iconContainer));
			info.AddProperty(NNormalMapPoint.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NNormalMapPoint.PropertyName._questIcon, Variant.From<TextureRect>(ref this._questIcon));
			info.AddProperty(NNormalMapPoint.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
			info.AddProperty(NNormalMapPoint.PropertyName._circleVfx, Variant.From<NMapCircleVfx>(ref this._circleVfx));
			info.AddProperty(NNormalMapPoint.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NNormalMapPoint.PropertyName._pulseTween, Variant.From<Tween>(ref this._pulseTween));
			info.AddProperty(NNormalMapPoint.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
		}

		/// <inheritdoc />
		// Token: 0x060029B8 RID: 10680 RVA: 0x00122BB8 File Offset: 0x00120DB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._iconContainer, ref variant))
			{
				this._iconContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._questIcon, ref variant3))
			{
				this._questIcon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._outline, ref variant4))
			{
				this._outline = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._circleVfx, ref variant5))
			{
				this._circleVfx = variant5.As<NMapCircleVfx>();
			}
			Variant variant6;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._pulseTween, ref variant7))
			{
				this._pulseTween = variant7.As<Tween>();
			}
			Variant variant8;
			if (info.TryGetProperty(NNormalMapPoint.PropertyName._elapsedTime, ref variant8))
			{
				this._elapsedTime = variant8.As<float>();
			}
		}

		// Token: 0x04000F8D RID: 3981
		private static readonly StringName _mapColor = new StringName("map_color");

		// Token: 0x04000F8E RID: 3982
		private Control _iconContainer;

		// Token: 0x04000F8F RID: 3983
		private TextureRect _icon;

		// Token: 0x04000F90 RID: 3984
		private TextureRect _questIcon;

		// Token: 0x04000F91 RID: 3985
		private TextureRect _outline;

		// Token: 0x04000F92 RID: 3986
		[Nullable(2)]
		private NMapCircleVfx _circleVfx;

		// Token: 0x04000F93 RID: 3987
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000F94 RID: 3988
		[Nullable(2)]
		private Tween _pulseTween;

		// Token: 0x04000F95 RID: 3989
		private const float _pulseSpeed = 4f;

		// Token: 0x04000F96 RID: 3990
		private const float _scaleAmount = 0.25f;

		// Token: 0x04000F97 RID: 3991
		private const float _scaleBase = 1.2f;

		// Token: 0x04000F98 RID: 3992
		private float _elapsedTime = Rng.Chaotic.NextFloat(3140f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D1 RID: 5329
		[NullableContext(0)]
		public new class MethodName : NMapPoint.MethodName
		{
			/// <summary>
			/// Cached name for the 'IconName' method.
			/// </summary>
			// Token: 0x04004F3B RID: 20283
			public static readonly StringName IconName = "IconName";

			/// <summary>
			/// Cached name for the 'IconPath' method.
			/// </summary>
			// Token: 0x04004F3C RID: 20284
			public static readonly StringName IconPath = "IconPath";

			/// <summary>
			/// Cached name for the 'OutlinePath' method.
			/// </summary>
			// Token: 0x04004F3D RID: 20285
			public static readonly StringName OutlinePath = "OutlinePath";

			/// <summary>
			/// Cached name for the 'UnknownIconPath' method.
			/// </summary>
			// Token: 0x04004F3E RID: 20286
			public static readonly StringName UnknownIconPath = "UnknownIconPath";

			/// <summary>
			/// Cached name for the 'UnknownOutlinePath' method.
			/// </summary>
			// Token: 0x04004F3F RID: 20287
			public static readonly StringName UnknownOutlinePath = "UnknownOutlinePath";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004F40 RID: 20288
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004F41 RID: 20289
			public new static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the 'RefreshMarkedIconVisibility' method.
			/// </summary>
			// Token: 0x04004F42 RID: 20290
			public static readonly StringName RefreshMarkedIconVisibility = "RefreshMarkedIconVisibility";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004F43 RID: 20291
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004F44 RID: 20292
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'OnSelected' method.
			/// </summary>
			// Token: 0x04004F45 RID: 20293
			public new static readonly StringName OnSelected = "OnSelected";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004F46 RID: 20294
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004F47 RID: 20295
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004F48 RID: 20296
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'SetAngle' method.
			/// </summary>
			// Token: 0x04004F49 RID: 20297
			public static readonly StringName SetAngle = "SetAngle";

			/// <summary>
			/// Cached name for the 'RefreshColorInstantly' method.
			/// </summary>
			// Token: 0x04004F4A RID: 20298
			public new static readonly StringName RefreshColorInstantly = "RefreshColorInstantly";

			/// <summary>
			/// Cached name for the 'RefreshState' method.
			/// </summary>
			// Token: 0x04004F4B RID: 20299
			public new static readonly StringName RefreshState = "RefreshState";

			/// <summary>
			/// Cached name for the 'UpdateIcon' method.
			/// </summary>
			// Token: 0x04004F4C RID: 20300
			public static readonly StringName UpdateIcon = "UpdateIcon";

			/// <summary>
			/// Cached name for the 'ShowCircleVfx' method.
			/// </summary>
			// Token: 0x04004F4D RID: 20301
			public static readonly StringName ShowCircleVfx = "ShowCircleVfx";

			/// <summary>
			/// Cached name for the 'AnimHover' method.
			/// </summary>
			// Token: 0x04004F4E RID: 20302
			public static readonly StringName AnimHover = "AnimHover";

			/// <summary>
			/// Cached name for the 'AnimUnhover' method.
			/// </summary>
			// Token: 0x04004F4F RID: 20303
			public static readonly StringName AnimUnhover = "AnimUnhover";

			/// <summary>
			/// Cached name for the 'AnimPressDown' method.
			/// </summary>
			// Token: 0x04004F50 RID: 20304
			public static readonly StringName AnimPressDown = "AnimPressDown";

			/// <summary>
			/// Cached name for the 'OnHighlightPointType' method.
			/// </summary>
			// Token: 0x04004F51 RID: 20305
			public static readonly StringName OnHighlightPointType = "OnHighlightPointType";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D2 RID: 5330
		[NullableContext(0)]
		public new class PropertyName : NMapPoint.PropertyName
		{
			/// <summary>
			/// Cached name for the 'TraveledColor' property.
			/// </summary>
			// Token: 0x04004F52 RID: 20306
			public new static readonly StringName TraveledColor = "TraveledColor";

			/// <summary>
			/// Cached name for the 'UntravelableColor' property.
			/// </summary>
			// Token: 0x04004F53 RID: 20307
			public new static readonly StringName UntravelableColor = "UntravelableColor";

			/// <summary>
			/// Cached name for the 'HoveredColor' property.
			/// </summary>
			// Token: 0x04004F54 RID: 20308
			public new static readonly StringName HoveredColor = "HoveredColor";

			/// <summary>
			/// Cached name for the 'HoverScale' property.
			/// </summary>
			// Token: 0x04004F55 RID: 20309
			public new static readonly StringName HoverScale = "HoverScale";

			/// <summary>
			/// Cached name for the 'DownScale' property.
			/// </summary>
			// Token: 0x04004F56 RID: 20310
			public new static readonly StringName DownScale = "DownScale";

			/// <summary>
			/// Cached name for the '_iconContainer' field.
			/// </summary>
			// Token: 0x04004F57 RID: 20311
			public static readonly StringName _iconContainer = "_iconContainer";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004F58 RID: 20312
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_questIcon' field.
			/// </summary>
			// Token: 0x04004F59 RID: 20313
			public static readonly StringName _questIcon = "_questIcon";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04004F5A RID: 20314
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_circleVfx' field.
			/// </summary>
			// Token: 0x04004F5B RID: 20315
			public static readonly StringName _circleVfx = "_circleVfx";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004F5C RID: 20316
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_pulseTween' field.
			/// </summary>
			// Token: 0x04004F5D RID: 20317
			public static readonly StringName _pulseTween = "_pulseTween";

			/// <summary>
			/// Cached name for the '_elapsedTime' field.
			/// </summary>
			// Token: 0x04004F5E RID: 20318
			public static readonly StringName _elapsedTime = "_elapsedTime";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D3 RID: 5331
		[NullableContext(0)]
		public new class SignalName : NMapPoint.SignalName
		{
		}
	}
}
