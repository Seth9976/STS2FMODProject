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
	// Token: 0x02000020 RID: 32
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarRoomIcon.cs")]
	public class NTopBarRoomIcon : NClickableControl
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00008D44 File Offset: 0x00006F44
		public override void _Ready()
		{
			this._roomIcon = base.GetNode<TextureRect>("Icon");
			this._roomIconOutline = base.GetNode<TextureRect>("Icon/Outline");
			this.ConnectSignals();
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008D78 File Offset: 0x00006F78
		public override void _EnterTree()
		{
			RunManager.Instance.RoomEntered += this.UpdateIcon;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008D90 File Offset: 0x00006F90
		public override void _ExitTree()
		{
			RunManager.Instance.RoomEntered -= this.UpdateIcon;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008DA8 File Offset: 0x00006FA8
		[NullableContext(1)]
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
			this.UpdateIcon();
		}

		/// <summary>
		/// Override the map point type for debugging purposes.
		/// </summary>
		/// <param name="mapPointType">MapPointType to use instead of the real one.</param>
		// Token: 0x0600011C RID: 284 RVA: 0x00008DB7 File Offset: 0x00006FB7
		public void DebugSetMapPointTypeOverride(MapPointType mapPointType)
		{
			if (mapPointType == MapPointType.Unassigned)
			{
				return;
			}
			this._debugMapPointTypeOverride = mapPointType;
		}

		/// <summary>
		/// Clear the map point type override.
		/// </summary>
		// Token: 0x0600011D RID: 285 RVA: 0x00008DC4 File Offset: 0x00006FC4
		public void DebugClearMapPointTypeOverride()
		{
			this._debugMapPointTypeOverride = MapPointType.Unassigned;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008DD0 File Offset: 0x00006FD0
		protected override void OnFocus()
		{
			string hoverTipPrefixForRoomType = this.GetHoverTipPrefixForRoomType();
			HoverTip hoverTip = new HoverTip(new LocString("static_hover_tips", hoverTipPrefixForRoomType + ".title"), new LocString("static_hover_tips", hoverTipPrefixForRoomType + ".description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this._roomIcon, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(this._roomIcon.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008E60 File Offset: 0x00007060
		[NullableContext(1)]
		private string GetHoverTipPrefixForRoomType()
		{
			string text;
			switch (this.GetCurrentMapPointType())
			{
			case MapPointType.Unassigned:
				text = "ROOM_MAP";
				break;
			case MapPointType.Unknown:
				text = this.GetHoverTipPrefixForUnknownRoomType();
				break;
			case MapPointType.Shop:
				text = "ROOM_MERCHANT";
				break;
			case MapPointType.Treasure:
				text = "ROOM_TREASURE";
				break;
			case MapPointType.RestSite:
				text = "ROOM_REST";
				break;
			case MapPointType.Monster:
				text = "ROOM_ENEMY";
				break;
			case MapPointType.Elite:
				text = "ROOM_ELITE";
				break;
			case MapPointType.Boss:
				text = "ROOM_BOSS";
				break;
			case MapPointType.Ancient:
				text = "ROOM_ANCIENT";
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return text;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008EF0 File Offset: 0x000070F0
		[NullableContext(1)]
		private string GetHoverTipPrefixForUnknownRoomType()
		{
			AbstractRoom baseRoom = this._runState.BaseRoom;
			switch (baseRoom.RoomType)
			{
			case RoomType.Monster:
				return "ROOM_UNKNOWN_ENEMY";
			case RoomType.Treasure:
				return "ROOM_UNKNOWN_TREASURE";
			case RoomType.Shop:
				return "ROOM_UNKNOWN_MERCHANT";
			case RoomType.Event:
				return "ROOM_UNKNOWN_EVENT";
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008F59 File Offset: 0x00007159
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this._roomIcon);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008F68 File Offset: 0x00007168
		private void UpdateIcon()
		{
			if (this._runState.CurrentRoom == null)
			{
				return;
			}
			AbstractRoom baseRoom = this._runState.BaseRoom;
			ActModel act = this._runState.Act;
			MapPointType currentMapPointType = this.GetCurrentMapPointType();
			ModelId modelId = null;
			if (currentMapPointType == MapPointType.Boss)
			{
				modelId = ((this._runState.CurrentMapPoint == this._runState.Map.SecondBossMapPoint) ? act.SecondBossEncounter.Id : act.BossEncounter.Id);
			}
			else if (currentMapPointType == MapPointType.Ancient)
			{
				modelId = act.Ancient.Id;
			}
			string roomIconPath = ImageHelper.GetRoomIconPath(currentMapPointType, baseRoom.RoomType, modelId);
			if (roomIconPath != null)
			{
				this._roomIcon.Visible = true;
				this._roomIcon.Texture = PreloadManager.Cache.GetCompressedTexture2D(roomIconPath);
			}
			else
			{
				this._roomIcon.Visible = false;
			}
			string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(currentMapPointType, baseRoom.RoomType, modelId);
			if (roomIconOutlinePath != null)
			{
				this._roomIconOutline.Visible = true;
				this._roomIconOutline.Texture = PreloadManager.Cache.GetCompressedTexture2D(roomIconOutlinePath);
			}
			else
			{
				this._roomIconOutline.Visible = false;
			}
			if (baseRoom.IsVictoryRoom)
			{
				this._roomIcon.Visible = false;
				this._roomIconOutline.Visible = false;
				base.FocusMode = 0L;
				base.MouseFilter = 2L;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000090A8 File Offset: 0x000072A8
		private MapPointType GetCurrentMapPointType()
		{
			if (this._debugMapPointTypeOverride != MapPointType.Unassigned)
			{
				return this._debugMapPointTypeOverride;
			}
			MapPoint currentMapPoint = this._runState.CurrentMapPoint;
			if (currentMapPoint == null)
			{
				return MapPointType.Unassigned;
			}
			return currentMapPoint.PointType;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000124 RID: 292 RVA: 0x000090D0 File Offset: 0x000072D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NTopBarRoomIcon.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.DebugSetMapPointTypeOverride, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "mapPointType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.DebugClearMapPointTypeOverride, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.GetHoverTipPrefixForRoomType, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.GetHoverTipPrefixForUnknownRoomType, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.UpdateIcon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarRoomIcon.MethodName.GetCurrentMapPointType, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000125 RID: 293 RVA: 0x00009310 File Offset: 0x00007510
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarRoomIcon.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.DebugSetMapPointTypeOverride && args.Count == 1)
			{
				this.DebugSetMapPointTypeOverride(VariantUtils.ConvertTo<MapPointType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.DebugClearMapPointTypeOverride && args.Count == 0)
			{
				this.DebugClearMapPointTypeOverride();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.GetHoverTipPrefixForRoomType && args.Count == 0)
			{
				string hoverTipPrefixForRoomType = this.GetHoverTipPrefixForRoomType();
				ret = VariantUtils.CreateFrom<string>(ref hoverTipPrefixForRoomType);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.GetHoverTipPrefixForUnknownRoomType && args.Count == 0)
			{
				string hoverTipPrefixForUnknownRoomType = this.GetHoverTipPrefixForUnknownRoomType();
				ret = VariantUtils.CreateFrom<string>(ref hoverTipPrefixForUnknownRoomType);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.UpdateIcon && args.Count == 0)
			{
				this.UpdateIcon();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarRoomIcon.MethodName.GetCurrentMapPointType && args.Count == 0)
			{
				MapPointType currentMapPointType = this.GetCurrentMapPointType();
				ret = VariantUtils.CreateFrom<MapPointType>(ref currentMapPointType);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000126 RID: 294 RVA: 0x000094E0 File Offset: 0x000076E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarRoomIcon.MethodName._Ready || (ref method) == NTopBarRoomIcon.MethodName._EnterTree || (ref method) == NTopBarRoomIcon.MethodName._ExitTree || (ref method) == NTopBarRoomIcon.MethodName.DebugSetMapPointTypeOverride || (ref method) == NTopBarRoomIcon.MethodName.DebugClearMapPointTypeOverride || (ref method) == NTopBarRoomIcon.MethodName.OnFocus || (ref method) == NTopBarRoomIcon.MethodName.GetHoverTipPrefixForRoomType || (ref method) == NTopBarRoomIcon.MethodName.GetHoverTipPrefixForUnknownRoomType || (ref method) == NTopBarRoomIcon.MethodName.OnUnfocus || (ref method) == NTopBarRoomIcon.MethodName.UpdateIcon || (ref method) == NTopBarRoomIcon.MethodName.GetCurrentMapPointType || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000127 RID: 295 RVA: 0x0000959C File Offset: 0x0000779C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarRoomIcon.PropertyName._roomIcon)
			{
				this._roomIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTopBarRoomIcon.PropertyName._roomIconOutline)
			{
				this._roomIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride)
			{
				this._debugMapPointTypeOverride = VariantUtils.ConvertTo<MapPointType>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06000128 RID: 296 RVA: 0x00009604 File Offset: 0x00007804
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarRoomIcon.PropertyName._roomIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._roomIcon);
				return true;
			}
			if ((ref name) == NTopBarRoomIcon.PropertyName._roomIconOutline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._roomIconOutline);
				return true;
			}
			if ((ref name) == NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride)
			{
				value = VariantUtils.CreateFrom<MapPointType>(ref this._debugMapPointTypeOverride);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000129 RID: 297 RVA: 0x0000967C File Offset: 0x0000787C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTopBarRoomIcon.PropertyName._roomIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarRoomIcon.PropertyName._roomIconOutline, 0L, "", 4096L, false),
				new PropertyInfo(2L, NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600012A RID: 298 RVA: 0x000096F4 File Offset: 0x000078F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarRoomIcon.PropertyName._roomIcon, Variant.From<TextureRect>(ref this._roomIcon));
			info.AddProperty(NTopBarRoomIcon.PropertyName._roomIconOutline, Variant.From<TextureRect>(ref this._roomIconOutline));
			info.AddProperty(NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride, Variant.From<MapPointType>(ref this._debugMapPointTypeOverride));
		}

		/// <inheritdoc />
		// Token: 0x0600012B RID: 299 RVA: 0x0000974C File Offset: 0x0000794C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarRoomIcon.PropertyName._roomIcon, ref variant))
			{
				this._roomIcon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTopBarRoomIcon.PropertyName._roomIconOutline, ref variant2))
			{
				this._roomIconOutline = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride, ref variant3))
			{
				this._debugMapPointTypeOverride = variant3.As<MapPointType>();
			}
		}

		// Token: 0x04000063 RID: 99
		[Nullable(1)]
		private IRunState _runState;

		// Token: 0x04000064 RID: 100
		[Nullable(1)]
		private TextureRect _roomIcon;

		// Token: 0x04000065 RID: 101
		[Nullable(1)]
		private TextureRect _roomIconOutline;

		// Token: 0x04000066 RID: 102
		private MapPointType _debugMapPointTypeOverride;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DF0 RID: 3568
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040031DB RID: 12763
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040031DC RID: 12764
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040031DD RID: 12765
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'DebugSetMapPointTypeOverride' method.
			/// </summary>
			// Token: 0x040031DE RID: 12766
			public static readonly StringName DebugSetMapPointTypeOverride = "DebugSetMapPointTypeOverride";

			/// <summary>
			/// Cached name for the 'DebugClearMapPointTypeOverride' method.
			/// </summary>
			// Token: 0x040031DF RID: 12767
			public static readonly StringName DebugClearMapPointTypeOverride = "DebugClearMapPointTypeOverride";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040031E0 RID: 12768
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'GetHoverTipPrefixForRoomType' method.
			/// </summary>
			// Token: 0x040031E1 RID: 12769
			public static readonly StringName GetHoverTipPrefixForRoomType = "GetHoverTipPrefixForRoomType";

			/// <summary>
			/// Cached name for the 'GetHoverTipPrefixForUnknownRoomType' method.
			/// </summary>
			// Token: 0x040031E2 RID: 12770
			public static readonly StringName GetHoverTipPrefixForUnknownRoomType = "GetHoverTipPrefixForUnknownRoomType";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040031E3 RID: 12771
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateIcon' method.
			/// </summary>
			// Token: 0x040031E4 RID: 12772
			public static readonly StringName UpdateIcon = "UpdateIcon";

			/// <summary>
			/// Cached name for the 'GetCurrentMapPointType' method.
			/// </summary>
			// Token: 0x040031E5 RID: 12773
			public static readonly StringName GetCurrentMapPointType = "GetCurrentMapPointType";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DF1 RID: 3569
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_roomIcon' field.
			/// </summary>
			// Token: 0x040031E6 RID: 12774
			public static readonly StringName _roomIcon = "_roomIcon";

			/// <summary>
			/// Cached name for the '_roomIconOutline' field.
			/// </summary>
			// Token: 0x040031E7 RID: 12775
			public static readonly StringName _roomIconOutline = "_roomIconOutline";

			/// <summary>
			/// Cached name for the '_debugMapPointTypeOverride' field.
			/// </summary>
			// Token: 0x040031E8 RID: 12776
			public static readonly StringName _debugMapPointTypeOverride = "_debugMapPointTypeOverride";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DF2 RID: 3570
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
