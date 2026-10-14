using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002F7 RID: 759
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapPoint.cs")]
	public abstract class NMapPoint : NButton
	{
		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060028F9 RID: 10489
		protected abstract Color TraveledColor { get; }

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060028FA RID: 10490
		protected abstract Color UntravelableColor { get; }

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060028FB RID: 10491
		protected abstract Color HoveredColor { get; }

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060028FC RID: 10492
		protected abstract Vector2 HoverScale { get; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060028FD RID: 10493
		protected abstract Vector2 DownScale { get; }

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060028FE RID: 10494 RVA: 0x00119F95 File Offset: 0x00118195
		protected override bool AllowFocusWhileDisabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x00119F98 File Offset: 0x00118198
		// (set) Token: 0x06002900 RID: 10496 RVA: 0x00119FA0 File Offset: 0x001181A0
		public NMultiplayerVoteContainer VoteContainer { get; set; }

		/// <summary>
		/// Indicates whether a node is an adjacent node above the current node (you're able to travel to
		/// it IF you have completed the room).
		/// If you want logic to trigger based on the button being clickable, use _isEnabled instead.
		/// </summary>
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002901 RID: 10497 RVA: 0x00119FAC File Offset: 0x001181AC
		protected bool IsTravelable
		{
			get
			{
				NMapScreen screen = this._screen;
				return (screen != null && screen.IsDebugTravelEnabled && !screen.IsTraveling) || (this._screen.IsTravelEnabled && this.State == MapPointState.Travelable);
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002902 RID: 10498 RVA: 0x00119FED File Offset: 0x001181ED
		// (set) Token: 0x06002903 RID: 10499 RVA: 0x00119FF5 File Offset: 0x001181F5
		public MapPoint Point { get; protected set; }

		/// <summary>
		/// This map point's current state. Affects how it looks and its hoverability/clickability.
		/// </summary>
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002904 RID: 10500 RVA: 0x00119FFE File Offset: 0x001181FE
		// (set) Token: 0x06002905 RID: 10501 RVA: 0x0011A006 File Offset: 0x00118206
		public MapPointState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (value == this._state)
				{
					return;
				}
				this._state = value;
				this.RefreshVisualsInstantly();
			}
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x0011A020 File Offset: 0x00118220
		public override void _Ready()
		{
			if (base.GetType() != typeof(NMapPoint))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x0011A078 File Offset: 0x00118278
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this.VoteContainer = base.GetNode<NMultiplayerVoteContainer>("%MapPointVoteContainer");
			this._controllerSelectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this.VoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.ShouldDisplayPlayerVote), this._runState.Players);
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x0011A0D9 File Offset: 0x001182D9
		protected bool IsInputAllowed()
		{
			return !this._screen.IsTraveling && this._screen.Drawings.GetLocalDrawingMode(true) == DrawingMode.None;
		}

		/// <summary>
		/// Returns true when a player's icon should be displayed to indicate that they've voted for this map point.
		/// </summary>
		// Token: 0x06002909 RID: 10505 RVA: 0x0011A100 File Offset: 0x00118300
		private bool ShouldDisplayPlayerVote(Player player)
		{
			MapCoord? mapCoord;
			if (this._screen.PlayerVoteDictionary.TryGetValue(player, out mapCoord) && mapCoord != null)
			{
				return mapCoord == this.Point.coord;
			}
			return this._runState.MapLocation.coord == this.Point.coord;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x0011A187 File Offset: 0x00118387
		public void RefreshVisualsInstantly()
		{
			this._controllerSelectionReticle.OnDeselect();
			this.RefreshColorInstantly();
			this.RefreshState();
		}

		/// <summary>
		/// Override this to add extra behavior when a map node selection.
		/// Usually for visuals.
		/// </summary>
		// Token: 0x0600290B RID: 10507 RVA: 0x0011A1A0 File Offset: 0x001183A0
		public virtual void OnSelected()
		{
		}

		/// <summary>
		/// The base class handles the logic for selecting a map point, so we don't allow overriding it. Instead, override
		/// OnSelected to run logic before selecting the map point.
		/// </summary>
		// Token: 0x0600290C RID: 10508 RVA: 0x0011A1A4 File Offset: 0x001183A4
		protected sealed override void OnRelease()
		{
			if (!this.IsTravelable)
			{
				return;
			}
			if (this.Point.coord.row == 0 && TestMode.IsOff && !SaveManager.Instance.SeenFtue("map_select_ftue"))
			{
				return;
			}
			if (this._screen.Drawings.GetLocalDrawingMode(true) != DrawingMode.None)
			{
				return;
			}
			if (!this._screen.IsNodeOnScreen(this) && NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			this._screen.OnMapPointSelectedLocally(this);
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x0011A220 File Offset: 0x00118420
		protected virtual void RefreshColorInstantly()
		{
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x0011A222 File Offset: 0x00118422
		protected virtual void RefreshState()
		{
			if (this.IsTravelable)
			{
				base.Enable();
				return;
			}
			base.Disable();
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600290F RID: 10511 RVA: 0x0011A23C File Offset: 0x0011843C
		protected Color TargetColor
		{
			get
			{
				MapPointState state = this.State;
				bool flag = state - MapPointState.Travelable <= 1;
				if (flag)
				{
					return this.TraveledColor;
				}
				return this.UntravelableColor;
			}
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x0011A26C File Offset: 0x0011846C
		protected override void OnFocus()
		{
			if (!this.IsInputAllowed())
			{
				return;
			}
			if (this._isEnabled && NControllerManager.Instance.IsUsingController)
			{
				this._controllerSelectionReticle.OnSelect();
			}
			if (this._state == MapPointState.Traveled && this._runState.MapLocation.coord != this.Point.coord && !NControllerManager.Instance.IsUsingController)
			{
				MapPointHistoryEntry historyEntryFor = this._runState.GetHistoryEntryFor(new MapLocation(new MapCoord?(this.Point.coord), this._runState.CurrentActIndex));
				if (historyEntryFor != null)
				{
					int num = this.Point.coord.row + 1;
					for (int i = 0; i < this._runState.MapPointHistory.Count - 1; i++)
					{
						num += this._runState.MapPointHistory[i].Count;
					}
					NHoverTipSet tip = NHoverTipSet.CreateAndShowMapPointHistory(this, NMapPointHistoryHoverTip.Create(num, LocalContext.NetId.Value, historyEntryFor));
					Callable.From(delegate
					{
						tip.SetAlignment(this, HoverTip.GetHoverTipAlignment(this, 0.75f));
					}).CallDeferred(Array.Empty<Variant>());
				}
			}
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x0011A3C8 File Offset: 0x001185C8
		protected override void OnUnfocus()
		{
			this._controllerSelectionReticle.OnDeselect();
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002912 RID: 10514 RVA: 0x0011A3DC File Offset: 0x001185DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NMapPoint.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.IsInputAllowed, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.RefreshVisualsInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.OnSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.RefreshState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPoint.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002913 RID: 10515 RVA: 0x0011A5C8 File Offset: 0x001187C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapPoint.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.IsInputAllowed && args.Count == 0)
			{
				bool flag = this.IsInputAllowed();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.RefreshVisualsInstantly && args.Count == 0)
			{
				this.RefreshVisualsInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.OnSelected && args.Count == 0)
			{
				this.OnSelected();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.RefreshColorInstantly && args.Count == 0)
			{
				this.RefreshColorInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.RefreshState && args.Count == 0)
			{
				this.RefreshState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPoint.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002914 RID: 10516 RVA: 0x0011A758 File Offset: 0x00118958
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapPoint.MethodName._Ready || (ref method) == NMapPoint.MethodName.ConnectSignals || (ref method) == NMapPoint.MethodName.IsInputAllowed || (ref method) == NMapPoint.MethodName.RefreshVisualsInstantly || (ref method) == NMapPoint.MethodName.OnSelected || (ref method) == NMapPoint.MethodName.OnRelease || (ref method) == NMapPoint.MethodName.RefreshColorInstantly || (ref method) == NMapPoint.MethodName.RefreshState || (ref method) == NMapPoint.MethodName.OnFocus || (ref method) == NMapPoint.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002915 RID: 10517 RVA: 0x0011A804 File Offset: 0x00118A04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapPoint.PropertyName.VoteContainer)
			{
				this.VoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.State)
			{
				this.State = VariantUtils.ConvertTo<MapPointState>(ref value);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._state)
			{
				this._state = VariantUtils.ConvertTo<MapPointState>(ref value);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._outlineColor)
			{
				this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._controllerSelectionReticle)
			{
				this._controllerSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._screen)
			{
				this._screen = VariantUtils.ConvertTo<NMapScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002916 RID: 10518 RVA: 0x0011A8BC File Offset: 0x00118ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapPoint.PropertyName.TraveledColor)
			{
				Color color = this.TraveledColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.UntravelableColor)
			{
				Color color = this.UntravelableColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.HoveredColor)
			{
				Color color = this.HoveredColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.HoverScale)
			{
				Vector2 vector = this.HoverScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.DownScale)
			{
				Vector2 vector = this.DownScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.AllowFocusWhileDisabled)
			{
				bool flag = this.AllowFocusWhileDisabled;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.VoteContainer)
			{
				NMultiplayerVoteContainer voteContainer = this.VoteContainer;
				value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref voteContainer);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.IsTravelable)
			{
				bool flag = this.IsTravelable;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.State)
			{
				MapPointState state = this.State;
				value = VariantUtils.CreateFrom<MapPointState>(ref state);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName.TargetColor)
			{
				Color color = this.TargetColor;
				value = VariantUtils.CreateFrom<Color>(ref color);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._state)
			{
				value = VariantUtils.CreateFrom<MapPointState>(ref this._state);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._outlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._controllerSelectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerSelectionReticle);
				return true;
			}
			if ((ref name) == NMapPoint.PropertyName._screen)
			{
				value = VariantUtils.CreateFrom<NMapScreen>(ref this._screen);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002917 RID: 10519 RVA: 0x0011AAB0 File Offset: 0x00118CB0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NMapPoint.PropertyName._state, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMapPoint.PropertyName.TraveledColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMapPoint.PropertyName.UntravelableColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMapPoint.PropertyName.HoveredColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMapPoint.PropertyName._outlineColor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMapPoint.PropertyName.HoverScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMapPoint.PropertyName.DownScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPoint.PropertyName._controllerSelectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapPoint.PropertyName.AllowFocusWhileDisabled, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPoint.PropertyName._screen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPoint.PropertyName.VoteContainer, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapPoint.PropertyName.IsTravelable, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMapPoint.PropertyName.State, 0L, "", 4096L, false),
				new PropertyInfo(20L, NMapPoint.PropertyName.TargetColor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002918 RID: 10520 RVA: 0x0011AC8C File Offset: 0x00118E8C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName voteContainer = NMapPoint.PropertyName.VoteContainer;
			NMultiplayerVoteContainer voteContainer2 = this.VoteContainer;
			info.AddProperty(voteContainer, Variant.From<NMultiplayerVoteContainer>(ref voteContainer2));
			StringName state = NMapPoint.PropertyName.State;
			MapPointState state2 = this.State;
			info.AddProperty(state, Variant.From<MapPointState>(ref state2));
			info.AddProperty(NMapPoint.PropertyName._state, Variant.From<MapPointState>(ref this._state));
			info.AddProperty(NMapPoint.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
			info.AddProperty(NMapPoint.PropertyName._controllerSelectionReticle, Variant.From<NSelectionReticle>(ref this._controllerSelectionReticle));
			info.AddProperty(NMapPoint.PropertyName._screen, Variant.From<NMapScreen>(ref this._screen));
		}

		/// <inheritdoc />
		// Token: 0x06002919 RID: 10521 RVA: 0x0011AD2C File Offset: 0x00118F2C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapPoint.PropertyName.VoteContainer, ref variant))
			{
				this.VoteContainer = variant.As<NMultiplayerVoteContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapPoint.PropertyName.State, ref variant2))
			{
				this.State = variant2.As<MapPointState>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapPoint.PropertyName._state, ref variant3))
			{
				this._state = variant3.As<MapPointState>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapPoint.PropertyName._outlineColor, ref variant4))
			{
				this._outlineColor = variant4.As<Color>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMapPoint.PropertyName._controllerSelectionReticle, ref variant5))
			{
				this._controllerSelectionReticle = variant5.As<NSelectionReticle>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMapPoint.PropertyName._screen, ref variant6))
			{
				this._screen = variant6.As<NMapScreen>();
			}
		}

		// Token: 0x04000F4A RID: 3914
		private MapPointState _state = MapPointState.Untravelable;

		// Token: 0x04000F4B RID: 3915
		protected IRunState _runState;

		// Token: 0x04000F4C RID: 3916
		protected Color _outlineColor = new Color(1f, 1f, 1f, 0.75f);

		// Token: 0x04000F4D RID: 3917
		protected const double _pressDownDur = 0.3;

		// Token: 0x04000F4E RID: 3918
		protected const double _unhoverAnimDur = 0.5;

		// Token: 0x04000F4F RID: 3919
		protected NSelectionReticle _controllerSelectionReticle;

		// Token: 0x04000F50 RID: 3920
		protected NMapScreen _screen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014BC RID: 5308
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004EA6 RID: 20134
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004EA7 RID: 20135
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'IsInputAllowed' method.
			/// </summary>
			// Token: 0x04004EA8 RID: 20136
			public static readonly StringName IsInputAllowed = "IsInputAllowed";

			/// <summary>
			/// Cached name for the 'RefreshVisualsInstantly' method.
			/// </summary>
			// Token: 0x04004EA9 RID: 20137
			public static readonly StringName RefreshVisualsInstantly = "RefreshVisualsInstantly";

			/// <summary>
			/// Cached name for the 'OnSelected' method.
			/// </summary>
			// Token: 0x04004EAA RID: 20138
			public static readonly StringName OnSelected = "OnSelected";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004EAB RID: 20139
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'RefreshColorInstantly' method.
			/// </summary>
			// Token: 0x04004EAC RID: 20140
			public static readonly StringName RefreshColorInstantly = "RefreshColorInstantly";

			/// <summary>
			/// Cached name for the 'RefreshState' method.
			/// </summary>
			// Token: 0x04004EAD RID: 20141
			public static readonly StringName RefreshState = "RefreshState";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004EAE RID: 20142
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004EAF RID: 20143
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014BD RID: 5309
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'TraveledColor' property.
			/// </summary>
			// Token: 0x04004EB0 RID: 20144
			public static readonly StringName TraveledColor = "TraveledColor";

			/// <summary>
			/// Cached name for the 'UntravelableColor' property.
			/// </summary>
			// Token: 0x04004EB1 RID: 20145
			public static readonly StringName UntravelableColor = "UntravelableColor";

			/// <summary>
			/// Cached name for the 'HoveredColor' property.
			/// </summary>
			// Token: 0x04004EB2 RID: 20146
			public static readonly StringName HoveredColor = "HoveredColor";

			/// <summary>
			/// Cached name for the 'HoverScale' property.
			/// </summary>
			// Token: 0x04004EB3 RID: 20147
			public static readonly StringName HoverScale = "HoverScale";

			/// <summary>
			/// Cached name for the 'DownScale' property.
			/// </summary>
			// Token: 0x04004EB4 RID: 20148
			public static readonly StringName DownScale = "DownScale";

			/// <summary>
			/// Cached name for the 'AllowFocusWhileDisabled' property.
			/// </summary>
			// Token: 0x04004EB5 RID: 20149
			public new static readonly StringName AllowFocusWhileDisabled = "AllowFocusWhileDisabled";

			/// <summary>
			/// Cached name for the 'VoteContainer' property.
			/// </summary>
			// Token: 0x04004EB6 RID: 20150
			public static readonly StringName VoteContainer = "VoteContainer";

			/// <summary>
			/// Cached name for the 'IsTravelable' property.
			/// </summary>
			// Token: 0x04004EB7 RID: 20151
			public static readonly StringName IsTravelable = "IsTravelable";

			/// <summary>
			/// Cached name for the 'State' property.
			/// </summary>
			// Token: 0x04004EB8 RID: 20152
			public static readonly StringName State = "State";

			/// <summary>
			/// Cached name for the 'TargetColor' property.
			/// </summary>
			// Token: 0x04004EB9 RID: 20153
			public static readonly StringName TargetColor = "TargetColor";

			/// <summary>
			/// Cached name for the '_state' field.
			/// </summary>
			// Token: 0x04004EBA RID: 20154
			public static readonly StringName _state = "_state";

			/// <summary>
			/// Cached name for the '_outlineColor' field.
			/// </summary>
			// Token: 0x04004EBB RID: 20155
			public static readonly StringName _outlineColor = "_outlineColor";

			/// <summary>
			/// Cached name for the '_controllerSelectionReticle' field.
			/// </summary>
			// Token: 0x04004EBC RID: 20156
			public static readonly StringName _controllerSelectionReticle = "_controllerSelectionReticle";

			/// <summary>
			/// Cached name for the '_screen' field.
			/// </summary>
			// Token: 0x04004EBD RID: 20157
			public static readonly StringName _screen = "_screen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014BE RID: 5310
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
