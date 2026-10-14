using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.sts2.Core.Nodes.TopBar
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarBossIcon.cs")]
	public class NTopBarBossIcon : NClickableControl
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x000067DA File Offset: 0x000049DA
		public override void _Ready()
		{
			this._bossIcon = base.GetNode<TextureRect>("Icon");
			this._bossIconOutline = base.GetNode<TextureRect>("Icon/Outline");
			this.ConnectSignals();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000680E File Offset: 0x00004A0E
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
			this.OnActEntered();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000681D File Offset: 0x00004A1D
		public override void _EnterTree()
		{
			RunManager.Instance.ActEntered += this.OnActEntered;
			RunManager.Instance.RoomEntered += this.OnRoomEntered;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000684B File Offset: 0x00004A4B
		public override void _ExitTree()
		{
			RunManager.Instance.ActEntered -= this.OnActEntered;
			RunManager.Instance.RoomEntered -= this.OnRoomEntered;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000687C File Offset: 0x00004A7C
		private void OnRoomEntered()
		{
			if (this._runState.CurrentRoom == null)
			{
				return;
			}
			AbstractRoom baseRoom = this._runState.BaseRoom;
			base.Visible = baseRoom.RoomType != RoomType.Boss || this.ShouldOnlyShowSecondBossIcon;
			base.FocusMode = ((baseRoom.RoomType != RoomType.Boss || this.ShouldOnlyShowSecondBossIcon) ? 2L : 0L);
			if (this.ShouldOnlyShowSecondBossIcon)
			{
				this.RefreshBossIcon();
			}
			this.RefreshSecondBossIconColor();
			if (this._runState.CurrentRoom.IsVictoryRoom)
			{
				this._bossIcon.SetVisible(false);
				this._bossIconOutline.SetVisible(false);
				TextureRect secondBossIcon = this._secondBossIcon;
				if (secondBossIcon != null)
				{
					secondBossIcon.SetVisible(false);
				}
				TextureRect secondBossIconOutline = this._secondBossIconOutline;
				if (secondBossIconOutline != null)
				{
					secondBossIconOutline.SetVisible(false);
				}
				base.FocusMode = 0L;
				base.MouseFilter = 2L;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000694A File Offset: 0x00004B4A
		private bool ShouldOnlyShowSecondBossIcon
		{
			get
			{
				return this._runState.Map.SecondBossMapPoint != null && this._runState.CurrentMapPoint == this._runState.Map.BossMapPoint;
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000697D File Offset: 0x00004B7D
		private void OnActEntered()
		{
			this.RefreshBossIcon();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006988 File Offset: 0x00004B88
		public void RefreshBossIcon()
		{
			EncounterModel encounterModel = (this.ShouldOnlyShowSecondBossIcon ? this._runState.Act.SecondBossEncounter : this._runState.Act.BossEncounter);
			string roomIconPath = ImageHelper.GetRoomIconPath(MapPointType.Boss, RoomType.Boss, encounterModel.Id);
			string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(MapPointType.Boss, RoomType.Boss, encounterModel.Id);
			this._bossIcon.Texture = PreloadManager.Cache.GetTexture2D(roomIconPath);
			this._bossIconOutline.Texture = PreloadManager.Cache.GetTexture2D(roomIconOutlinePath);
			EncounterModel secondBossEncounter = this._runState.Act.SecondBossEncounter;
			if (secondBossEncounter != null && !this.ShouldOnlyShowSecondBossIcon)
			{
				if (this._secondBossIcon == null)
				{
					PackedScene packedScene = GD.Load<PackedScene>("res://scenes/ui/top_bar/second_boss_icon.tscn");
					this._secondBossIcon = packedScene.Instantiate<TextureRect>(0L);
					this._secondBossIconOutline = this._secondBossIcon.GetNode<TextureRect>("%Outline");
					this._secondBossIcon.MouseFilter = 1L;
					this._secondBossIconOutline.MouseFilter = 1L;
					this._bossIcon.AddChildSafely(this._secondBossIcon);
					this._secondBossIcon.Position = new Vector2(30f, 22f);
				}
				string roomIconPath2 = ImageHelper.GetRoomIconPath(MapPointType.Boss, RoomType.Boss, secondBossEncounter.Id);
				string roomIconOutlinePath2 = ImageHelper.GetRoomIconOutlinePath(MapPointType.Boss, RoomType.Boss, secondBossEncounter.Id);
				this._secondBossIcon.Texture = PreloadManager.Cache.GetTexture2D(roomIconPath2);
				this._secondBossIconOutline.Texture = PreloadManager.Cache.GetTexture2D(roomIconOutlinePath2);
				this._secondBossIcon.Visible = true;
				this.RefreshSecondBossIconColor();
				return;
			}
			TextureRect secondBossIcon = this._secondBossIcon;
			if (secondBossIcon != null)
			{
				secondBossIcon.SetVisible(false);
			}
			TextureRect secondBossIconOutline = this._secondBossIconOutline;
			if (secondBossIconOutline == null)
			{
				return;
			}
			secondBossIconOutline.SetVisible(false);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006B30 File Offset: 0x00004D30
		private void RefreshSecondBossIconColor()
		{
			TextureRect secondBossIcon = this._secondBossIcon;
			ShaderMaterial shaderMaterial = ((secondBossIcon != null) ? secondBossIcon.Material : null) as ShaderMaterial;
			if (shaderMaterial == null)
			{
				return;
			}
			TextureRect secondBossIconOutline = this._secondBossIconOutline;
			ShaderMaterial shaderMaterial2 = ((secondBossIconOutline != null) ? secondBossIconOutline.Material : null) as ShaderMaterial;
			if (shaderMaterial2 == null)
			{
				return;
			}
			ActModel act = this._runState.Act;
			MapPoint currentMapPoint = this._runState.CurrentMapPoint;
			MapPoint bossMapPoint = this._runState.Map.BossMapPoint;
			MapPoint secondBossMapPoint = this._runState.Map.SecondBossMapPoint;
			Color color = ((currentMapPoint == bossMapPoint || currentMapPoint == secondBossMapPoint) ? act.MapTraveledColor : act.MapUntraveledColor);
			shaderMaterial.SetShaderParameter(NTopBarBossIcon._tintColor, new Vector3(color.R, color.G, color.B));
			shaderMaterial2.SetShaderParameter(NTopBarBossIcon._tintColor, new Vector3(color.R, color.G, color.B));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006C28 File Offset: 0x00004E28
		protected override void OnFocus()
		{
			EncounterModel bossEncounter = this._runState.Act.BossEncounter;
			EncounterModel secondBossEncounter = this._runState.Act.SecondBossEncounter;
			HoverTip hoverTip;
			if (secondBossEncounter != null && !this.ShouldOnlyShowSecondBossIcon)
			{
				NTopBarBossIcon._doubleBossHoverTipTitle.Add("BossName1", bossEncounter.Title);
				NTopBarBossIcon._doubleBossHoverTipTitle.Add("BossName2", secondBossEncounter.Title);
				NTopBarBossIcon._doubleBossHoverTipDescription.Add("BossName1", bossEncounter.Title);
				NTopBarBossIcon._doubleBossHoverTipDescription.Add("BossName2", secondBossEncounter.Title);
				hoverTip = new HoverTip(NTopBarBossIcon._doubleBossHoverTipTitle, NTopBarBossIcon._doubleBossHoverTipDescription, null);
			}
			else
			{
				NTopBarBossIcon._bossHoverTipTitle.Add("BossName", this.ShouldOnlyShowSecondBossIcon ? secondBossEncounter.Title : bossEncounter.Title);
				NTopBarBossIcon._bossHoverTipDescription.Add("BossName", this.ShouldOnlyShowSecondBossIcon ? secondBossEncounter.Title : bossEncounter.Title);
				hoverTip = new HoverTip(NTopBarBossIcon._bossHoverTipTitle, NTopBarBossIcon._bossHoverTipDescription, null);
			}
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(this._bossIcon.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006D69 File Offset: 0x00004F69
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000BC RID: 188 RVA: 0x00006D74 File Offset: 0x00004F74
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NTopBarBossIcon.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName.OnRoomEntered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName.OnActEntered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName.RefreshBossIcon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName.RefreshSecondBossIconColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarBossIcon.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000BD RID: 189 RVA: 0x00006F34 File Offset: 0x00005134
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarBossIcon.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName.OnRoomEntered && args.Count == 0)
			{
				this.OnRoomEntered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName.OnActEntered && args.Count == 0)
			{
				this.OnActEntered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName.RefreshBossIcon && args.Count == 0)
			{
				this.RefreshBossIcon();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName.RefreshSecondBossIconColor && args.Count == 0)
			{
				this.RefreshSecondBossIconColor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarBossIcon.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060000BE RID: 190 RVA: 0x00007098 File Offset: 0x00005298
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarBossIcon.MethodName._Ready || (ref method) == NTopBarBossIcon.MethodName._EnterTree || (ref method) == NTopBarBossIcon.MethodName._ExitTree || (ref method) == NTopBarBossIcon.MethodName.OnRoomEntered || (ref method) == NTopBarBossIcon.MethodName.OnActEntered || (ref method) == NTopBarBossIcon.MethodName.RefreshBossIcon || (ref method) == NTopBarBossIcon.MethodName.RefreshSecondBossIconColor || (ref method) == NTopBarBossIcon.MethodName.OnFocus || (ref method) == NTopBarBossIcon.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060000BF RID: 191 RVA: 0x00007134 File Offset: 0x00005334
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarBossIcon.PropertyName._bossIcon)
			{
				this._bossIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTopBarBossIcon.PropertyName._bossIconOutline)
			{
				this._bossIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTopBarBossIcon.PropertyName._secondBossIcon)
			{
				this._secondBossIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTopBarBossIcon.PropertyName._secondBossIconOutline)
			{
				this._secondBossIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060000C0 RID: 192 RVA: 0x000071B8 File Offset: 0x000053B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarBossIcon.PropertyName.ShouldOnlyShowSecondBossIcon)
			{
				bool shouldOnlyShowSecondBossIcon = this.ShouldOnlyShowSecondBossIcon;
				value = VariantUtils.CreateFrom<bool>(ref shouldOnlyShowSecondBossIcon);
				return true;
			}
			if ((ref name) == NTopBarBossIcon.PropertyName._bossIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._bossIcon);
				return true;
			}
			if ((ref name) == NTopBarBossIcon.PropertyName._bossIconOutline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._bossIconOutline);
				return true;
			}
			if ((ref name) == NTopBarBossIcon.PropertyName._secondBossIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._secondBossIcon);
				return true;
			}
			if ((ref name) == NTopBarBossIcon.PropertyName._secondBossIconOutline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._secondBossIconOutline);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000C1 RID: 193 RVA: 0x00007270 File Offset: 0x00005470
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTopBarBossIcon.PropertyName._bossIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarBossIcon.PropertyName._bossIconOutline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarBossIcon.PropertyName._secondBossIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarBossIcon.PropertyName._secondBossIconOutline, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTopBarBossIcon.PropertyName.ShouldOnlyShowSecondBossIcon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000C2 RID: 194 RVA: 0x00007328 File Offset: 0x00005528
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarBossIcon.PropertyName._bossIcon, Variant.From<TextureRect>(ref this._bossIcon));
			info.AddProperty(NTopBarBossIcon.PropertyName._bossIconOutline, Variant.From<TextureRect>(ref this._bossIconOutline));
			info.AddProperty(NTopBarBossIcon.PropertyName._secondBossIcon, Variant.From<TextureRect>(ref this._secondBossIcon));
			info.AddProperty(NTopBarBossIcon.PropertyName._secondBossIconOutline, Variant.From<TextureRect>(ref this._secondBossIconOutline));
		}

		/// <inheritdoc />
		// Token: 0x060000C3 RID: 195 RVA: 0x00007394 File Offset: 0x00005594
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarBossIcon.PropertyName._bossIcon, ref variant))
			{
				this._bossIcon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTopBarBossIcon.PropertyName._bossIconOutline, ref variant2))
			{
				this._bossIconOutline = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTopBarBossIcon.PropertyName._secondBossIcon, ref variant3))
			{
				this._secondBossIcon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTopBarBossIcon.PropertyName._secondBossIconOutline, ref variant4))
			{
				this._secondBossIconOutline = variant4.As<TextureRect>();
			}
		}

		// Token: 0x04000047 RID: 71
		private static readonly LocString _bossHoverTipTitle = new LocString("static_hover_tips", "BOSS.title");

		// Token: 0x04000048 RID: 72
		private static readonly LocString _bossHoverTipDescription = new LocString("static_hover_tips", "BOSS.description");

		// Token: 0x04000049 RID: 73
		private static readonly LocString _doubleBossHoverTipTitle = new LocString("static_hover_tips", "DOUBLE_BOSS.title");

		// Token: 0x0400004A RID: 74
		private static readonly LocString _doubleBossHoverTipDescription = new LocString("static_hover_tips", "DOUBLE_BOSS.description");

		// Token: 0x0400004B RID: 75
		private TextureRect _bossIcon;

		// Token: 0x0400004C RID: 76
		private TextureRect _bossIconOutline;

		// Token: 0x0400004D RID: 77
		[Nullable(2)]
		private TextureRect _secondBossIcon;

		// Token: 0x0400004E RID: 78
		[Nullable(2)]
		private TextureRect _secondBossIconOutline;

		// Token: 0x0400004F RID: 79
		private static readonly StringName _tintColor = new StringName("tint_color");

		// Token: 0x04000050 RID: 80
		private const string _secondBossIconScenePath = "res://scenes/ui/top_bar/second_boss_icon.tscn";

		// Token: 0x04000051 RID: 81
		private IRunState _runState;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DD9 RID: 3545
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040031A4 RID: 12708
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040031A5 RID: 12709
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040031A6 RID: 12710
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnRoomEntered' method.
			/// </summary>
			// Token: 0x040031A7 RID: 12711
			public static readonly StringName OnRoomEntered = "OnRoomEntered";

			/// <summary>
			/// Cached name for the 'OnActEntered' method.
			/// </summary>
			// Token: 0x040031A8 RID: 12712
			public static readonly StringName OnActEntered = "OnActEntered";

			/// <summary>
			/// Cached name for the 'RefreshBossIcon' method.
			/// </summary>
			// Token: 0x040031A9 RID: 12713
			public static readonly StringName RefreshBossIcon = "RefreshBossIcon";

			/// <summary>
			/// Cached name for the 'RefreshSecondBossIconColor' method.
			/// </summary>
			// Token: 0x040031AA RID: 12714
			public static readonly StringName RefreshSecondBossIconColor = "RefreshSecondBossIconColor";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040031AB RID: 12715
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040031AC RID: 12716
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DDA RID: 3546
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ShouldOnlyShowSecondBossIcon' property.
			/// </summary>
			// Token: 0x040031AD RID: 12717
			public static readonly StringName ShouldOnlyShowSecondBossIcon = "ShouldOnlyShowSecondBossIcon";

			/// <summary>
			/// Cached name for the '_bossIcon' field.
			/// </summary>
			// Token: 0x040031AE RID: 12718
			public static readonly StringName _bossIcon = "_bossIcon";

			/// <summary>
			/// Cached name for the '_bossIconOutline' field.
			/// </summary>
			// Token: 0x040031AF RID: 12719
			public static readonly StringName _bossIconOutline = "_bossIconOutline";

			/// <summary>
			/// Cached name for the '_secondBossIcon' field.
			/// </summary>
			// Token: 0x040031B0 RID: 12720
			public static readonly StringName _secondBossIcon = "_secondBossIcon";

			/// <summary>
			/// Cached name for the '_secondBossIconOutline' field.
			/// </summary>
			// Token: 0x040031B1 RID: 12721
			public static readonly StringName _secondBossIconOutline = "_secondBossIconOutline";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DDB RID: 3547
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
