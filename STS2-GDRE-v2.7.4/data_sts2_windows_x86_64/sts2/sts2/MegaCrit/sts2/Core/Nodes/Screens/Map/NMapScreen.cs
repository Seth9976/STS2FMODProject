using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002F8 RID: 760
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapScreen.cs")]
	public class NMapScreen : Control, IScreenContext, INetCursorPositionTranslator
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600291B RID: 10523 RVA: 0x0011AE16 File Offset: 0x00119016
		[Nullable(2)]
		public static NMapScreen Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.GlobalUi.MapScreen;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600291C RID: 10524 RVA: 0x0011AE2D File Offset: 0x0011902D
		// (set) Token: 0x0600291D RID: 10525 RVA: 0x0011AE35 File Offset: 0x00119035
		public bool IsOpen { get; private set; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x0011AE3E File Offset: 0x0011903E
		// (set) Token: 0x0600291F RID: 10527 RVA: 0x0011AE46 File Offset: 0x00119046
		public bool IsTravelEnabled { get; private set; }

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x0011AE4F File Offset: 0x0011904F
		// (set) Token: 0x06002921 RID: 10529 RVA: 0x0011AE57 File Offset: 0x00119057
		public bool IsDebugTravelEnabled { get; private set; }

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x0011AE60 File Offset: 0x00119060
		private float MapLegendX
		{
			get
			{
				return base.Size.X * 0.8f;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x0011AE73 File Offset: 0x00119073
		// (set) Token: 0x06002924 RID: 10532 RVA: 0x0011AE7B File Offset: 0x0011907B
		public bool IsTraveling { get; set; }

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06002925 RID: 10533 RVA: 0x0011AE84 File Offset: 0x00119084
		// (remove) Token: 0x06002926 RID: 10534 RVA: 0x0011AEBC File Offset: 0x001190BC
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<MapPointType> PointTypeHighlighted;

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002927 RID: 10535 RVA: 0x0011AEF1 File Offset: 0x001190F1
		public Dictionary<Player, MapCoord?> PlayerVoteDictionary { get; } = new Dictionary<Player, MapCoord?>();

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x0011AEF9 File Offset: 0x001190F9
		// (set) Token: 0x06002929 RID: 10537 RVA: 0x0011AF01 File Offset: 0x00119101
		public NMapDrawings Drawings { get; private set; }

		// Token: 0x0600292A RID: 10538 RVA: 0x0011AF0C File Offset: 0x0011910C
		public override void _Ready()
		{
			base.GetNode<MegaLabel>("MapLegend/Header").SetTextAutoSize(new LocString("map", "LEGEND_HEADER").GetFormattedText());
			this._mapContainer = base.GetNode<Control>("TheMap");
			this._mapBgContainer = base.GetNode<NMapBg>("%MapBg");
			this._pathsContainer = base.GetNode<Control>("TheMap/Paths");
			this._points = base.GetNode<Control>("TheMap/Points");
			this._marker = base.GetNode<NMapMarker>("TheMap/MapMarker");
			this.Drawings = base.GetNode<NMapDrawings>("TheMap/Drawings");
			this._backButton = base.GetNode<NBackButton>("Back");
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackButtonPressed)), 0U);
			this._backButton.Disable();
			this._mapLegend = base.GetNode<Control>("MapLegend");
			this._legendItems = base.GetNode<Control>("MapLegend/LegendItems");
			this._legendHotkeyIcon = base.GetNode<TextureRect>("MapLegend/LegendHotkeyIcon");
			this._drawingToolsHotkeyIcon = base.GetNode<TextureRect>("DrawingToolsHotkey");
			this._backstop = base.GetNode<Control>("%Backstop");
			this._drawingTools = base.GetNode<Control>("%DrawingTools");
			this._mapDrawingButton = base.GetNode<NMapDrawButton>("%DrawButton");
			this._mapDrawingButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMapDrawingButtonPressed)), 0U);
			this._mapErasingButton = base.GetNode<NMapEraseButton>("%EraseButton");
			this._mapErasingButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMapErasingButtonPressed)), 0U);
			this._mapClearButton = base.GetNode<NMapClearButton>("%ClearButton");
			this._mapClearButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnClearMapDrawingButtonPressed)), 0U);
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged += this.OnPlayerVoteChanged;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled += this.OnPlayerVoteCancelled;
			base.ProcessMode = (base.Visible ? 0L : 4L);
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChanged)), 0U);
			Callable.From<Error>(() => NCapstoneContainer.Instance.Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.OnCapstoneChanged)), 0U)).CallDeferred(Array.Empty<Variant>());
			List<NMapLegendItem> list = this._legendItems.GetChildren(false).OfType<NMapLegendItem>().ToList<NMapLegendItem>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].FocusNeighborTop = ((i > 0) ? list[i - 1].GetPath() : list[i].GetPath());
				list[i].FocusNeighborBottom = ((i < list.Count - 1) ? list[i + 1].GetPath() : list[i].GetPath());
				list[i].FocusNeighborRight = list[i].GetPath();
			}
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
			this.UpdateHotkeyDisplay();
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0011B2CD File Offset: 0x001194CD
		public override void _ExitTree()
		{
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged -= this.OnPlayerVoteChanged;
			RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled -= this.OnPlayerVoteCancelled;
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0011B308 File Offset: 0x00119508
		public void Initialize(RunState runState)
		{
			this._runState = runState;
			this.Drawings.Initialize(RunManager.Instance.NetService, this._runState, RunManager.Instance.InputSynchronizer);
			this._marker.Initialize(LocalContext.GetMe(this._runState));
			this._mapBgContainer.Initialize(this._runState);
		}

		/// <summary>
		/// This is to be called by the backend so we can setup our map screen.
		/// </summary>
		/// <param name="map">The new map to set.</param>
		/// <param name="seed">The seed to use for the visual jitter on the map node positions.</param>
		/// <param name="clearDrawings">True if map drawings should be cleared.</param>
		// Token: 0x0600292D RID: 10541 RVA: 0x0011B368 File Offset: 0x00119568
		public void SetMap(ActMap map, ulong seed, bool clearDrawings)
		{
			this._map = map;
			this._mapPointDictionary.Clear();
			this._paths.Clear();
			this.RemoveAllMapPointsAndPaths();
			this._marker.ResetMapPoint();
			if (clearDrawings)
			{
				this.Drawings.ClearAllLines();
			}
			this._hasPlayedAnimation = false;
			int rowCount = map.GetRowCount();
			int columnCount = map.GetColumnCount();
			float num = ((map.SecondBossMapPoint != null) ? 0.9f : 1f);
			this._distY = 2325f / (float)(rowCount - 1) * num;
			this._distX = 1050f / (float)columnCount;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("map_jitter_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._runState.CurrentActIndex);
			Rng rng = new Rng(seed, defaultInterpolatedStringHandler.ToStringAndClear());
			Vector2 vector;
			vector..ctor(-500f, 740f);
			Vector2 vector2;
			vector2..ctor(this._distX, -this._distY);
			foreach (MapPoint mapPoint in map.GetAllMapPoints())
			{
				NNormalMapPoint nnormalMapPoint = NNormalMapPoint.Create(mapPoint, this, this._runState);
				nnormalMapPoint.Position = new Vector2((float)mapPoint.coord.col, (float)mapPoint.coord.row) * vector2 + vector;
				float num2 = rng.NextFloat(-21f, 21f);
				float num3 = rng.NextFloat(-25f, 25f);
				nnormalMapPoint.Position += new Vector2(num2, num3);
				this._mapPointDictionary.Add(mapPoint.coord, nnormalMapPoint);
				this._points.AddChildSafely(nnormalMapPoint);
				nnormalMapPoint.SetAngle(Rng.Chaotic.NextGaussianFloat(0f, 8f, 0f, 1f));
			}
			this._bossPointNode = NBossMapPoint.Create(map.BossMapPoint, this, this._runState);
			this._bossPointNode.Position = new Vector2(-200f, -1980f * num);
			this._points.AddChildSafely(this._bossPointNode);
			this._mapPointDictionary[map.BossMapPoint.coord] = this._bossPointNode;
			if (map.SecondBossMapPoint != null)
			{
				this._bossPointNode.Scale = new Vector2(0.75f, 0.75f);
				this._secondBossPointNode = NBossMapPoint.Create(map.SecondBossMapPoint, this, this._runState);
				this._secondBossPointNode.Position = new Vector2(-200f, -2280f * num);
				this._secondBossPointNode.Scale = new Vector2(0.75f, 0.75f);
				this._points.AddChildSafely(this._secondBossPointNode);
				this._mapPointDictionary[map.SecondBossMapPoint.coord] = this._secondBossPointNode;
			}
			if (map.StartingMapPoint.PointType == MapPointType.Ancient)
			{
				this._startingPointNode = NAncientMapPoint.Create(map.StartingMapPoint, this, this._runState);
				this._startingPointNode.Position = new Vector2(-80f, (float)map.StartingMapPoint.coord.row * -this._distY + 720f);
			}
			else
			{
				this._startingPointNode = NNormalMapPoint.Create(map.StartingMapPoint, this, this._runState);
				this._startingPointNode.Position = new Vector2(-80f, (float)map.StartingMapPoint.coord.row * -this._distY + 800f);
			}
			this._points.AddChildSafely(this._startingPointNode);
			this._mapPointDictionary[map.StartingMapPoint.coord] = this._startingPointNode;
			foreach (MapPoint mapPoint2 in map.GetAllMapPoints())
			{
				this.DrawPaths(this._mapPointDictionary[mapPoint2.coord], mapPoint2);
			}
			this.DrawPaths(this._startingPointNode, map.StartingMapPoint);
			this.DrawPaths(this._bossPointNode, map.BossMapPoint);
			IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
			for (int i = 0; i < visitedMapCoords.Count - 1; i++)
			{
				IReadOnlyList<TextureRect> readOnlyList;
				if (this._paths.TryGetValue(new ValueTuple<MapCoord, MapCoord>(visitedMapCoords[i], visitedMapCoords[i + 1]), out readOnlyList))
				{
					foreach (TextureRect textureRect in readOnlyList)
					{
						textureRect.Modulate = this._runState.Act.MapTraveledColor;
						textureRect.Scale = NMapScreen._tickTraveledScale;
					}
				}
			}
			this.InitMapVotes();
			this.RefreshAllMapPointVotes();
			for (int j = 0; j < map.GetRowCount(); j++)
			{
				IEnumerable<MapPoint> pointsInRow = map.GetPointsInRow(j);
				List<NMapPoint> list = pointsInRow.Select<MapPoint, NMapPoint>((MapPoint p) => this._mapPointDictionary[p.coord]).ToList<NMapPoint>();
				for (int k = 0; k < list.Count; k++)
				{
					list[k].FocusNeighborLeft = ((k > 0) ? list[k - 1].GetPath() : list[k].GetPath());
					list[k].FocusNeighborRight = ((k < list.Count - 1) ? list[k + 1].GetPath() : list[k].GetPath());
					list[k].FocusNeighborTop = list[k].GetPath();
					list[k].FocusNeighborBottom = list[k].GetPath();
				}
			}
			this._startingPointNode.FocusNeighborLeft = this._startingPointNode.GetPath();
			this._startingPointNode.FocusNeighborRight = this._startingPointNode.GetPath();
			this._startingPointNode.FocusNeighborTop = this._startingPointNode.GetPath();
			this._startingPointNode.FocusNeighborBottom = this._startingPointNode.GetPath();
			this._bossPointNode.FocusNeighborLeft = this._bossPointNode.GetPath();
			this._bossPointNode.FocusNeighborRight = this._bossPointNode.GetPath();
			this._bossPointNode.FocusNeighborBottom = this._bossPointNode.GetPath();
			if (this._secondBossPointNode != null)
			{
				this._bossPointNode.FocusNeighborTop = this._secondBossPointNode.GetPath();
				this._secondBossPointNode.FocusNeighborBottom = this._bossPointNode.GetPath();
				this._secondBossPointNode.FocusNeighborLeft = this._secondBossPointNode.GetPath();
				this._secondBossPointNode.FocusNeighborRight = this._secondBossPointNode.GetPath();
				this._secondBossPointNode.FocusNeighborTop = this._secondBossPointNode.GetPath();
			}
			else
			{
				this._bossPointNode.FocusNeighborTop = this._bossPointNode.GetPath();
			}
			if (base.IsVisible())
			{
				this.RecalculateTravelability();
				this.RefreshAllPointVisuals();
			}
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x0011BAA8 File Offset: 0x00119CA8
		private void DrawPaths(NMapPoint mapPointNode, MapPoint mapPoint)
		{
			foreach (MapPoint mapPoint2 in mapPoint.Children)
			{
				NMapPoint nmapPoint;
				if (!this._mapPointDictionary.TryGetValue(mapPoint2.coord, out nmapPoint))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Map point child with coord ");
					defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(mapPoint2.coord);
					defaultInterpolatedStringHandler.AppendLiteral(" is not in the map point dictionary!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Vector2 lineEndpoint = this.GetLineEndpoint(mapPointNode);
				Vector2 lineEndpoint2 = this.GetLineEndpoint(nmapPoint);
				IReadOnlyList<TextureRect> readOnlyList = this.CreatePath(lineEndpoint, lineEndpoint2);
				this._paths.Add(new ValueTuple<MapCoord, MapCoord>(mapPoint.coord, mapPoint2.coord), readOnlyList);
			}
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x0011BB88 File Offset: 0x00119D88
		private Vector2 GetLineEndpoint(NMapPoint point)
		{
			if (point is NNormalMapPoint)
			{
				return point.Position;
			}
			return point.Position + point.Size * 0.5f;
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0011BBB4 File Offset: 0x00119DB4
		private void RecalculateTravelability()
		{
			if (this._runState.VisitedMapCoords.Any<MapCoord>())
			{
				foreach (NMapPoint nmapPoint in this._mapPointDictionary.Values)
				{
					nmapPoint.State = MapPointState.Untravelable;
				}
				foreach (MapCoord mapCoord in this._runState.VisitedMapCoords)
				{
					NMapPoint nmapPoint2;
					if (this._mapPointDictionary.TryGetValue(mapCoord, out nmapPoint2))
					{
						nmapPoint2.State = MapPointState.Traveled;
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 1);
						defaultInterpolatedStringHandler.AppendLiteral("VisitedMapCoord ");
						defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(mapCoord);
						defaultInterpolatedStringHandler.AppendLiteral(" not found in map point dictionary, map may have been regenerated");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
				IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
				MapCoord mapCoord2 = visitedMapCoords[visitedMapCoords.Count - 1];
				if (this._secondBossPointNode != null && mapCoord2 == this._bossPointNode.Point.coord)
				{
					this._secondBossPointNode.State = MapPointState.Travelable;
					return;
				}
				if (mapCoord2.row == this._map.GetRowCount() - 1)
				{
					this._bossPointNode.State = MapPointState.Travelable;
					return;
				}
				NMapPoint nmapPoint3;
				if (!this._mapPointDictionary.TryGetValue(mapCoord2, out nmapPoint3))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Last visited coord ");
					defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(mapCoord2);
					defaultInterpolatedStringHandler.AppendLiteral(" not found in map, falling back to starting point");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this._startingPointNode.State = MapPointState.Travelable;
					return;
				}
				IEnumerable<MapPoint> travelablePointsFrom = MapTravel.GetTravelablePointsFrom(this._runState, nmapPoint3.Point);
				using (IEnumerator<MapPoint> enumerator3 = travelablePointsFrom.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						MapPoint mapPoint = enumerator3.Current;
						this._mapPointDictionary[mapPoint.coord].State = MapPointState.Travelable;
					}
					return;
				}
			}
			this._startingPointNode.State = MapPointState.Travelable;
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x0011BDE0 File Offset: 0x00119FE0
		private void InitMapVotes()
		{
			foreach (Player player in this._runState.Players)
			{
				MapVote? mapVote;
				MapCoord? mapCoord = ((RunManager.Instance.MapSelectionSynchronizer.GetVote(player) != null) ? new MapCoord?(mapVote.GetValueOrDefault().coord) : null);
				if (mapCoord != null)
				{
					this.OnPlayerVoteChangedInternal(player, null, new MapCoord?(mapCoord.Value));
				}
			}
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x0011BE8C File Offset: 0x0011A08C
		public void OnMapPointSelectedLocally(NMapPoint point)
		{
			Player me = LocalContext.GetMe(this._runState);
			MapCoord? mapCoord;
			if (!this.PlayerVoteDictionary.TryGetValue(me, out mapCoord) || mapCoord != point.Point.coord)
			{
				MapVote? mapVote;
				this.OnPlayerVoteChangedInternal(me, (RunManager.Instance.MapSelectionSynchronizer.GetVote(me) != null) ? new MapCoord?(mapVote.GetValueOrDefault().coord) : null, new MapCoord?(point.Point.coord));
				MapLocation mapLocation = new MapLocation(this._runState.CurrentMapCoord, this._runState.CurrentActIndex);
				MapVote mapVote2 = new MapVote
				{
					coord = point.Point.coord,
					mapGenerationCount = RunManager.Instance.MapSelectionSynchronizer.MapGenerationCount
				};
				VoteForMapCoordAction voteForMapCoordAction = new VoteForMapCoordAction(LocalContext.GetMe(this._runState), mapLocation, new MapVote?(mapVote2));
				RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(voteForMapCoordAction);
				return;
			}
			if (this._runState.Players.Count <= 1)
			{
				return;
			}
			RunManager.Instance.FlavorSynchronizer.SendMapPing(point.Point.coord);
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x0011BFDC File Offset: 0x0011A1DC
		private void OnPlayerVoteChanged(Player player, MapVote? oldLocation, MapVote? newLocation)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Player vote changed for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<MapVote?>(oldLocation);
			defaultInterpolatedStringHandler.AppendLiteral("->");
			defaultInterpolatedStringHandler.AppendFormatted<MapVote?>(newLocation);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (LocalContext.IsMe(player))
			{
				return;
			}
			this.OnPlayerVoteChangedInternal(player, (oldLocation != null) ? new MapCoord?(oldLocation.GetValueOrDefault().coord) : null, (newLocation != null) ? new MapCoord?(newLocation.GetValueOrDefault().coord) : null);
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0011C09C File Offset: 0x0011A29C
		private void OnPlayerVoteCancelled(Player player)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player vote cancelled for ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this.OnPlayerVoteChangedInternal(player, this.PlayerVoteDictionary[player], null);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0011C0F8 File Offset: 0x0011A2F8
		private void OnPlayerVoteChangedInternal(Player player, MapCoord? oldCoord, MapCoord? newCoord)
		{
			if (this._runState.Players.Count <= 1)
			{
				return;
			}
			this.PlayerVoteDictionary[player] = newCoord;
			MapLocation mapLocation;
			if (oldCoord != null)
			{
				NMapPoint nmapPoint = this._mapPointDictionary[oldCoord.Value];
				nmapPoint.VoteContainer.RefreshPlayerVotes(true);
			}
			else
			{
				mapLocation = this._runState.MapLocation;
				if (mapLocation.coord != null)
				{
					Dictionary<MapCoord, NMapPoint> mapPointDictionary = this._mapPointDictionary;
					mapLocation = this._runState.MapLocation;
					NMapPoint nmapPoint2 = mapPointDictionary[mapLocation.coord.Value];
					nmapPoint2.VoteContainer.RefreshPlayerVotes(true);
				}
			}
			if (newCoord != null)
			{
				NMapPoint nmapPoint3 = this._mapPointDictionary[newCoord.Value];
				nmapPoint3.VoteContainer.RefreshPlayerVotes(true);
				return;
			}
			mapLocation = this._runState.MapLocation;
			if (mapLocation.coord != null)
			{
				Dictionary<MapCoord, NMapPoint> mapPointDictionary2 = this._mapPointDictionary;
				mapLocation = this._runState.MapLocation;
				NMapPoint nmapPoint4 = mapPointDictionary2[mapLocation.coord.Value];
				nmapPoint4.VoteContainer.RefreshPlayerVotes(true);
			}
		}

		/// <summary>
		/// Sets the marker to be at the specified coordinate.
		/// Since the player starts in the Neow room in a new run, TravelToMapCoord isn't called so we need to init the marker
		/// externally.
		/// </summary>
		/// <param name="coord"></param>
		// Token: 0x06002936 RID: 10550 RVA: 0x0011C210 File Offset: 0x0011A410
		public void InitMarker(MapCoord coord)
		{
			NMapPoint nmapPoint = this._mapPointDictionary[coord];
			this._marker.SetMapPoint(nmapPoint);
		}

		/// <summary>
		/// Animation and logic which occurs when a coord is finalized for travel.
		/// In multiplayer, this only occurs when all votes have been received and a map point has been selected.
		/// </summary>
		/// <param name="coord">The coordinate to travel to</param>
		// Token: 0x06002937 RID: 10551 RVA: 0x0011C238 File Offset: 0x0011A438
		public async Task TravelToMapCoord(MapCoord coord)
		{
			this.IsTraveling = true;
			this.RecalculateTravelability();
			if (NCapstoneContainer.Instance.CurrentCapstoneScreen is NDeckViewScreen)
			{
				NCapstoneContainer.Instance.Close();
			}
			this._marker.HideMapPoint();
			this.IsTravelEnabled = false;
			MapSplitVoteAnimation mapSplitVoteAnimation = new MapSplitVoteAnimation(this, this._runState, this._mapPointDictionary);
			await mapSplitVoteAnimation.TryPlay(coord);
			NMapPoint node = this._mapPointDictionary[coord];
			node.OnSelected();
			float num = 1f;
			if (node is NAncientMapPoint)
			{
				num = 1.5f;
			}
			else if (node is NBossMapPoint)
			{
				num = 2f;
			}
			NMapNodeSelectVfx nmapNodeSelectVfx = NMapNodeSelectVfx.Create(num);
			SfxCmd.Play("event:/sfx/ui/map/map_select", 1f);
			node.AddChildSafely(nmapNodeSelectVfx);
			nmapNodeSelectVfx.Position += node.PivotOffset;
			node.MoveChildSafely(nmapNodeSelectVfx, node.VoteContainer.GetIndex(false));
			IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
			SfxCmd.Play("event:/sfx/ui/wipe_map", 1f);
			Task fadeOutTask = TaskHelper.RunSafely(RunManager.Instance.FadeOut());
			if (visitedMapCoords.Any<MapCoord>())
			{
				Dictionary<ValueTuple<MapCoord, MapCoord>, IReadOnlyList<TextureRect>> paths = this._paths;
				IReadOnlyList<MapCoord> readOnlyList = visitedMapCoords;
				IReadOnlyList<TextureRect> readOnlyList2;
				if (paths.TryGetValue(new ValueTuple<MapCoord, MapCoord>(readOnlyList[readOnlyList.Count - 1], node.Point.coord), out readOnlyList2))
				{
					float num2;
					switch (SaveManager.Instance.PrefsSave.FastMode)
					{
					case FastModeType.Normal:
						num2 = 0.8f;
						goto IL_023A;
					case FastModeType.Fast:
						num2 = 0.3f;
						goto IL_023A;
					}
					num2 = 0f;
					IL_023A:
					float waitPerTick = num2 / (float)readOnlyList2.Count;
					foreach (TextureRect tick in readOnlyList2)
					{
						await Cmd.Wait(waitPerTick, false);
						tick.Modulate = StsColors.pathDotTraveled;
						base.CreateTween().TweenProperty(tick, "scale", NMapScreen._tickTraveledScale, 0.4).From(Vector2.One * 1.7f)
							.SetEase(1L)
							.SetTrans(7L);
						tick = null;
					}
					IEnumerator<TextureRect> enumerator = null;
				}
			}
			this._marker.SetMapPoint(node);
			await fadeOutTask;
			await RunManager.Instance.EnterMapCoord(coord);
			TaskHelper.RunSafely(RunManager.Instance.FadeIn(true));
			this.RefreshAllPointVisuals();
			this.PlayerVoteDictionary.Clear();
			this.RefreshAllMapPointVotes();
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x0011C284 File Offset: 0x0011A484
		public void RefreshAllMapPointVotes()
		{
			foreach (NMapPoint nmapPoint in this._mapPointDictionary.Values)
			{
				nmapPoint.VoteContainer.RefreshPlayerVotes(true);
			}
		}

		/// <summary>
		/// Just a helper class to make Init() easier to parse, don't mind me.
		/// </summary>
		// Token: 0x06002939 RID: 10553 RVA: 0x0011C2E4 File Offset: 0x0011A4E4
		private void RemoveAllMapPointsAndPaths()
		{
			this._points.FreeChildren();
			this._pathsContainer.FreeChildren();
			NBossMapPoint bossPointNode = this._bossPointNode;
			if (bossPointNode != null)
			{
				bossPointNode.QueueFreeSafely();
			}
			NBossMapPoint secondBossPointNode = this._secondBossPointNode;
			if (secondBossPointNode != null)
			{
				secondBossPointNode.QueueFreeSafely();
			}
			NMapPoint startingPointNode = this._startingPointNode;
			if (startingPointNode == null)
			{
				return;
			}
			startingPointNode.QueueFreeSafely();
		}

		/// <summary>
		/// Creates a bunch of dots to form a path from start to end
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		// Token: 0x0600293A RID: 10554 RVA: 0x0011C33C File Offset: 0x0011A53C
		private IReadOnlyList<TextureRect> CreatePath(Vector2 start, Vector2 end)
		{
			List<TextureRect> list = new List<TextureRect>();
			Vector2 vector = (end - start).Normalized();
			float num = vector.Angle() + 1.5707964f;
			float num2 = start.DistanceTo(end);
			int num3 = (int)(num2 / 22f) + 1;
			for (int i = 1; i < num3; i++)
			{
				float num4 = (float)i * 22f;
				TextureRect textureRect = PreloadManager.Cache.GetScene("res://scenes/ui/map_dot.tscn").Instantiate<TextureRect>(0L);
				textureRect.Position = start + vector * num4;
				textureRect.Position -= new Vector2(base.Size.X * 0.5f - 20f, base.Size.Y * 0.5f - 20f);
				textureRect.Position += new Vector2(Rng.Chaotic.NextFloat(-3f, 3f), Rng.Chaotic.NextFloat(-3f, 3f));
				textureRect.FlipH = Rng.Chaotic.NextBool();
				textureRect.Rotation = num + Rng.Chaotic.NextGaussianFloat(0f, 0.1f, 0f, 1f);
				textureRect.Modulate = this._runState.Act.MapUntraveledColor;
				this._pathsContainer.AddChildSafely(textureRect);
				list.Add(textureRect);
			}
			return list;
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x0600293B RID: 10555 RVA: 0x0011C4BB File Offset: 0x0011A6BB
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return NMapDrawings.AssetPaths.Append("res://scenes/ui/map_dot.tscn");
			}
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0011C4CC File Offset: 0x0011A6CC
		public override void _Process(double delta)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (this._actAnimTween != null && this._actAnimTween.IsRunning())
			{
				return;
			}
			this.UpdateScrollPosition(delta);
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x0011C4F4 File Offset: 0x0011A6F4
		private void UpdateScrollPosition(double delta)
		{
			if (this._mapContainer.Position != this._targetDragPos)
			{
				float num = (float)Mathf.Sign(this._mapContainer.Position.Y - this._targetDragPos.Y);
				this._mapContainer.Position = this._mapContainer.Position.Lerp(this._targetDragPos, (float)delta * 15f);
				float num2 = (float)Mathf.Sign(this._mapContainer.Position.Y - this._targetDragPos.Y);
				if (Math.Abs(this._mapContainer.Position.Y - this._targetDragPos.Y) < 0.5f || !Mathf.IsEqualApprox(num, num2))
				{
					this._mapContainer.Position = this._targetDragPos;
				}
			}
			if (!this._isDragging)
			{
				if (this._targetDragPos.Y < -600f)
				{
					this._targetDragPos = this._targetDragPos.Lerp(new Vector2(0f, -600f), (float)delta * 12f);
				}
				else if (this._targetDragPos.Y > 1800f)
				{
					this._targetDragPos = this._targetDragPos.Lerp(new Vector2(0f, 1800f), (float)delta * 12f);
				}
			}
			NGame.Instance.RemoteCursorContainer.ForceUpdateAllCursors();
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x0011C65D File Offset: 0x0011A85D
		public override void _GuiInput(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			this.ProcessMouseEvent(inputEvent);
			this.ProcessScrollEvent(inputEvent);
		}

		/// <summary>
		/// Detects mouse click up/down and updates our scroll target accordingly
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x0600293F RID: 10559 RVA: 0x0011C678 File Offset: 0x0011A878
		private void ProcessMouseEvent(InputEvent inputEvent)
		{
			this.ProcessMouseDrawingEvent(inputEvent);
			if (this._drawingInput == null)
			{
				if (this._isDragging)
				{
					InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
					if (inputEventMouseMotion != null)
					{
						this._targetDragPos += new Vector2(0f, inputEventMouseMotion.Relative.Y);
						goto IL_00B4;
					}
				}
				InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
				if (inputEventMouseButton != null)
				{
					if (inputEventMouseButton.ButtonIndex == 1L)
					{
						if (inputEventMouseButton.Pressed && this.CanScroll())
						{
							this._isDragging = true;
							this._startDragPos = this._mapContainer.Position;
							this._targetDragPos = this._startDragPos;
							this.TryCancelStartOfActAnim();
						}
						else
						{
							this._isDragging = false;
						}
					}
					else if (!inputEventMouseButton.Pressed)
					{
						this._isDragging = false;
					}
				}
				IL_00B4:
				InputEventMouseMotion inputEventMouseMotion2 = inputEvent as InputEventMouseMotion;
				if (inputEventMouseMotion2 != null && this.Drawings.IsLocalDrawing())
				{
					this.Drawings.UpdateCurrentLinePositionLocal(this.Drawings.GetGlobalTransform().Inverse() * inputEventMouseMotion2.GlobalPosition);
				}
			}
		}

		/// <summary>
		/// Detects mouse click up/down and does drawing
		/// </summary>
		// Token: 0x06002940 RID: 10560 RVA: 0x0011C77C File Offset: 0x0011A97C
		private void ProcessMouseDrawingEvent(InputEvent inputEvent)
		{
			if (this._isInputDisabled || (this._actAnimTween != null && this._actAnimTween.IsRunning()))
			{
				return;
			}
			if (this._drawingInput != null)
			{
				return;
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.Pressed)
			{
				if (inputEventMouseButton.ButtonIndex == 2L)
				{
					this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Drawing, true);
				}
				else if (inputEventMouseButton.ButtonIndex == 3L)
				{
					this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Erasing, true);
				}
				NMapDrawingInput drawingInput = this._drawingInput;
				if (drawingInput != null)
				{
					drawingInput.Connect(NMapDrawingInput.SignalName.Finished, Callable.From(delegate
					{
						this._drawingInput = null;
						this.UpdateDrawingButtonStates();
					}), 0U);
				}
				this.AddChildSafely(this._drawingInput);
			}
		}

		/// <summary>
		/// Detects mouse wheel up/down and updates our scroll target accordingly
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06002941 RID: 10561 RVA: 0x0011C834 File Offset: 0x0011AA34
		private void ProcessScrollEvent(InputEvent inputEvent)
		{
			if (!this.CanScroll())
			{
				return;
			}
			this._targetDragPos += new Vector2(0f, ScrollHelper.GetDragForScrollEvent(inputEvent));
			bool flag = inputEvent is InputEventMouseButton || inputEvent is InputEventPanGesture;
			if (flag)
			{
				this.TryCancelStartOfActAnim();
			}
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x0011C88C File Offset: 0x0011AA8C
		private void ProcessControllerEvent(InputEvent inputEvent)
		{
			if (inputEvent.IsActionPressed(MegaInput.up, false, false) && this.CanScroll())
			{
				this._targetDragPos += new Vector2(0f, this._controllerScrollAmount);
				this.TryCancelStartOfActAnim();
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.down, false, false) && this.CanScroll())
			{
				this._targetDragPos += new Vector2(0f, -this._controllerScrollAmount);
				this.TryCancelStartOfActAnim();
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.right, false, false) || inputEvent.IsActionPressed(MegaInput.left, false, false) || inputEvent.IsActionPressed(MegaInput.select, false, false))
			{
				if (this._runState.ActFloor == 0)
				{
					this._targetDragPos = new Vector2(0f, -600f);
					return;
				}
				MapCoord? mapCoord;
				int num = ((this._runState.CurrentMapCoord != null) ? mapCoord.GetValueOrDefault().row : 0);
				this._targetDragPos = new Vector2(0f, -600f + (float)num * this._distY);
			}
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x0011C9AC File Offset: 0x0011ABAC
		public void SetTravelEnabled(bool enabled)
		{
			this.IsTravelEnabled = enabled && Hook.ShouldProceedToNextMapPoint(this._runState);
			this.RefreshAllPointVisuals();
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x0011C9CB File Offset: 0x0011ABCB
		public void SetDebugTravelEnabled(bool enabled)
		{
			this.IsDebugTravelEnabled = enabled;
			this.RefreshAllPointVisuals();
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x0011C9DC File Offset: 0x0011ABDC
		public void RefreshAllPointVisuals()
		{
			foreach (NMapPoint nmapPoint in this._mapPointDictionary.Values)
			{
				nmapPoint.RefreshVisualsInstantly();
			}
			NMapPoint nmapPoint2 = this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((NMapPoint n) => n.IsEnabled);
			if (nmapPoint2 == null)
			{
				return;
			}
			nmapPoint2.TryGrabFocus();
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x0011CA6C File Offset: 0x0011AC6C
		private void PlayStartOfActAnimation()
		{
			if (this._hasPlayedAnimation)
			{
				Log.Warn("Tried to play start of act animation twice! Ignoring second try", 2);
				return;
			}
			this._hasPlayedAnimation = true;
			NActBanner nactBanner = NActBanner.Create(this._runState.Act, this._runState.CurrentActIndex);
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.GlobalUi.MapScreen.AddChildSafely(nactBanner);
			}
			TaskHelper.RunSafely(this.StartOfActAnim());
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x0011CAD8 File Offset: 0x0011ACD8
		private async Task StartOfActAnim()
		{
			this._mapContainer.Position = new Vector2(0f, 1800f);
			Tween actAnimTween = this._actAnimTween;
			if (actAnimTween != null)
			{
				actAnimTween.Kill();
			}
			this._actAnimTween = base.CreateTween().SetParallel(true);
			this._actAnimTween.TweenInterval(this._mapAnimStartDelay);
			this._actAnimTween.Chain();
			Vector2 vector;
			vector..ctor(0f, -600f);
			this._actAnimTween.TweenProperty(this._mapContainer, "position:y", -600f, this._mapAnimDuration).SetEase(2L).SetTrans(5L);
			this._actAnimTween.TweenCallback(Callable.From(new Action(this.SetInterruptable))).SetDelay(this._mapAnimDuration * 0.25);
			this._targetDragPos = vector;
			TaskAwaiter<bool> taskAwaiter = this._actAnimTween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this._actAnimTween = null;
				this.InitMapPrompt();
			}
		}

		/// <summary>
		/// Initializes and begins to animate the "Select a Starting Room" message.
		/// This only appears in the very first map screen you enter per Act.
		/// </summary>
		// Token: 0x06002948 RID: 10568 RVA: 0x0011CB1B File Offset: 0x0011AD1B
		private void InitMapPrompt()
		{
			if (TestMode.IsOn || SaveManager.Instance.SeenFtue("map_select_ftue"))
			{
				return;
			}
			TaskHelper.RunSafely(this.MapFtueCheck());
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x0011CB44 File Offset: 0x0011AD44
		private async Task MapFtueCheck()
		{
			await Task.Delay(100);
			NMapSelectFtue nmapSelectFtue = NMapSelectFtue.Create(this._startingPointNode);
			NModalContainer.Instance.Add(nmapSelectFtue, true);
			SaveManager.Instance.MarkFtueAsComplete("map_select_ftue");
			await nmapSelectFtue.WaitForPlayerToConfirm();
		}

		/// <summary>
		/// Makes the map anim interruptable via click or scroll
		/// </summary>
		// Token: 0x0600294A RID: 10570 RVA: 0x0011CB87 File Offset: 0x0011AD87
		private void SetInterruptable()
		{
			this._canInterruptAnim = true;
		}

		/// <summary>
		/// During the first half of the map scroll animation, moving the map is disabled.
		/// </summary>
		// Token: 0x0600294B RID: 10571 RVA: 0x0011CB90 File Offset: 0x0011AD90
		private bool CanScroll()
		{
			return (this._actAnimTween == null || this._canInterruptAnim) && !this._isInputDisabled;
		}

		/// <summary>
		/// Allows the player to abort (aka cancel) the start of act animation early.
		/// </summary>
		// Token: 0x0600294C RID: 10572 RVA: 0x0011CBB0 File Offset: 0x0011ADB0
		private void TryCancelStartOfActAnim()
		{
			if (this._actAnimTween != null && this._canInterruptAnim)
			{
				Tween actAnimTween = this._actAnimTween;
				if (actAnimTween != null)
				{
					actAnimTween.Kill();
				}
				this._actAnimTween = null;
				this._canInterruptAnim = false;
				this._isDragging = false;
				this._targetDragPos = new Vector2(0f, -600f);
				TaskHelper.RunSafely(this.DisableInputVeryBriefly());
			}
		}

		/// <summary>
		/// This is used to disable scrolling and clicking on the map screen for 1/5 of a second
		/// for when the player interrupts the map animation. Otherwise it's possible to double click
		/// and stop the map at a strange position.
		/// </summary>
		// Token: 0x0600294D RID: 10573 RVA: 0x0011CC14 File Offset: 0x0011AE14
		private async Task DisableInputVeryBriefly()
		{
			this._isInputDisabled = true;
			NMapDrawingInput drawingInput = this._drawingInput;
			if (drawingInput != null)
			{
				drawingInput.StopDrawing();
			}
			await Task.Delay(200);
			this._isInputDisabled = false;
			this.InitMapPrompt();
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0011CC58 File Offset: 0x0011AE58
		private void OnVisibilityChanged()
		{
			if (base.Visible)
			{
				RunManager.Instance.InputSynchronizer.StartOverridingCursorPositioning(this);
				return;
			}
			this._isDragging = false;
			RunManager.Instance.InputSynchronizer.StopOverridingCursorPositioning();
			this._backButton.Disable();
			this.Drawings.StopLineLocal();
			this.Drawings.SetDrawingModeLocal(DrawingMode.None);
			NMapDrawingInput drawingInput = this._drawingInput;
			if (drawingInput != null)
			{
				drawingInput.StopDrawing();
			}
			this.UpdateDrawingButtonStates();
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0011CCD0 File Offset: 0x0011AED0
		private void OnCapstoneChanged()
		{
			CanvasItem backstop = this._backstop;
			NCapstoneContainer instance = NCapstoneContainer.Instance;
			backstop.Visible = instance == null || !instance.InUse;
			if (base.Visible)
			{
				if (!this._backstop.Visible)
				{
					NRun.Instance.GlobalUi.TopBar.Map.StopOscillation();
					return;
				}
				NRun.Instance.GlobalUi.TopBar.Map.StartOscillation();
			}
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0011CD44 File Offset: 0x0011AF44
		public void Close(bool animateOut = true)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			base.FocusMode = 0L;
			NRun.Instance.GlobalUi.TopBar.Map.StopOscillation();
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.accept, new Action(this.OnLegendHotkeyPressed));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.viewExhaustPileAndTabRight, new Action(this.OnDrawingToolsHotkeyPressed));
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				CombatManager.Instance.Unpause();
			}
			this._backButton.Disable();
			ActiveScreenContext.Instance.Update();
			this.EmitSignalClosed();
			if (animateOut)
			{
				TaskHelper.RunSafely(this.AnimClose());
				SfxCmd.Play("event:/sfx/ui/map/map_close", 1f);
				return;
			}
			base.Visible = false;
			base.ProcessMode = 4L;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0011CE24 File Offset: 0x0011B024
		private async Task AnimClose()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._backstop, "modulate:a", 0f, 0.15);
			this._tween.TweenProperty(this._points, "modulate:a", 0f, 0.15);
			this._tween.TweenProperty(this._mapContainer, "modulate", StsColors.transparentBlack, 0.25).SetDelay(0.1);
			this._tween.TweenProperty(this._mapContainer, "position:y", this._mapContainer.Position.Y + 200f, 0.25).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._mapLegend, "modulate:a", 0f, 0.15);
			this._tween.TweenProperty(this._mapLegend, "position:x", this.MapLegendX + 120f, 0.25).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._drawingTools, "modulate:a", 0f, 0.15);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				base.Visible = false;
				base.ProcessMode = 4L;
			}
		}

		/// <summary>
		/// Opens the map screen.
		/// </summary>
		/// <param name="isOpenedFromTopBar">If this is true, then we'll skip the full map animation. Should be true if
		/// the player has opened the map manually instead of proceeding at the end of a room.</param>
		/// <returns>The map screen that was just opened.</returns>
		// Token: 0x06002952 RID: 10578 RVA: 0x0011CE68 File Offset: 0x0011B068
		public NMapScreen Open(bool isOpenedFromTopBar = false)
		{
			if (this.IsOpen)
			{
				return this;
			}
			this.IsOpen = true;
			base.Visible = true;
			this._backButton.MoveToHidePosition();
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.accept, new Action(this.OnLegendHotkeyPressed));
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.viewExhaustPileAndTabRight, new Action(this.OnDrawingToolsHotkeyPressed));
			if (this._runState.ActFloor > 0)
			{
				this._backButton.Enable();
			}
			base.ProcessMode = 0L;
			NRun.Instance.GlobalUi.TopBar.Map.StartOscillation();
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				CombatManager.Instance.Pause();
			}
			bool flag = ((this._runState.CurrentActIndex == 0 && this._runState.ExtraFields.StartedWithNeow) ? (this._runState.ActFloor == 1) : (this._runState.ActFloor == 0));
			Color color;
			if (flag && !this._hasPlayedAnimation)
			{
				bool flag2 = !isOpenedFromTopBar && (SaveManager.Instance.PrefsSave.FastMode < FastModeType.Fast || !SaveManager.Instance.SeenFtue("map_select_ftue"));
				if (flag2)
				{
					this.PlayStartOfActAnimation();
				}
				else
				{
					this._hasPlayedAnimation = true;
					Control mapContainer = this._mapContainer;
					Vector2 position = this._mapContainer.Position;
					position.Y = -600f;
					mapContainer.Position = position;
					this._targetDragPos = new Vector2(0f, -600f);
					NActBanner nactBanner = NActBanner.Create(this._runState.Act, this._runState.CurrentActIndex);
					NRun.Instance.GlobalUi.MapScreen.AddChildSafely(nactBanner);
				}
			}
			else
			{
				MapCoord? mapCoord;
				int num = ((this._runState.CurrentMapCoord != null) ? mapCoord.GetValueOrDefault().row : 0);
				this._targetDragPos = new Vector2(0f, -600f + (float)num * this._distY);
				this._mapContainer.Position = new Vector2(0f, -600f + (float)num * this._distY);
				CanvasItem points = this._points;
				color = this._points.Modulate;
				color.A = 0f;
				points.Modulate = color;
				CanvasItem backstop = this._backstop;
				color = this._backstop.Modulate;
				color.A = 0f;
				backstop.Modulate = color;
				this._mapLegend.Modulate = StsColors.transparentBlack;
				this._drawingTools.Modulate = StsColors.transparentBlack;
			}
			CanvasItem mapLegend = this._mapLegend;
			color = this._mapLegend.Modulate;
			color.A = 0f;
			mapLegend.Modulate = color;
			CanvasItem drawingTools = this._drawingTools;
			color = this._drawingTools.Modulate;
			color.A = 0f;
			drawingTools.Modulate = color;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._backstop, "modulate:a", 0.85f, 0.25);
			this._tween.TweenProperty(this._mapContainer, "modulate", Colors.White, 0.25).From(StsColors.transparentBlack);
			this._tween.TweenProperty(this._mapLegend, "modulate", Colors.White, 0.25).SetDelay(0.1);
			this._tween.TweenProperty(this._mapLegend, "position:x", this.MapLegendX, 0.25).From(this.MapLegendX + 120f).SetEase(1L)
				.SetTrans(10L)
				.SetDelay(0.1);
			this._tween.TweenProperty(this._drawingTools, "modulate", Colors.White, 0.25).SetDelay(0.2);
			this._tween.TweenProperty(this._points, "modulate:a", 1f, 0.25).SetDelay(0.1);
			this.RecalculateTravelability();
			if (this._runState.VisitedMapCoords.Count != 0)
			{
				IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
				MapCoord mapCoord2 = visitedMapCoords[visitedMapCoords.Count - 1];
				if (this._bossPointNode.Point.coord.row != mapCoord2.row && this._startingPointNode.Point.coord.row != mapCoord2.row)
				{
					NMapPoint nmapPoint;
					if (this._mapPointDictionary.TryGetValue(mapCoord2, out nmapPoint))
					{
						this._marker.SetMapPoint(nmapPoint);
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Last visited coord ");
						defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(mapCoord2);
						defaultInterpolatedStringHandler.AppendLiteral(" not found in map, marker not placed");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
			}
			SfxCmd.Play("event:/sfx/ui/map/map_open", 1f);
			ActiveScreenContext.Instance.Update();
			this.EmitSignalOpened();
			if (this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((NMapPoint n) => n.IsEnabled) == null)
			{
				base.FocusMode = 2L;
			}
			return this;
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0011D41D File Offset: 0x0011B61D
		private void OnBackButtonPressed(NButton _)
		{
			this.Close(true);
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0011D428 File Offset: 0x0011B628
		public override void _Input(InputEvent inputEvent)
		{
			if (!(base.GetViewport().GuiGetFocusOwner() is NMapPoint) && !base.HasFocus())
			{
				return;
			}
			if (!ActiveScreenContext.Instance.IsCurrent(this))
			{
				return;
			}
			if (inputEvent.IsActionReleased(DebugHotkey.unlockCharacters, false))
			{
				this._mapLegend.Visible = !this._mapLegend.Visible;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(this._mapLegend.Visible ? "Show Legend" : "Hide Legend"));
			}
			if (!base.IsVisibleInTree())
			{
				return;
			}
			this.ProcessControllerEvent(inputEvent);
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0011D4C0 File Offset: 0x0011B6C0
		private void OnMapDrawingButtonPressed(NButton _)
		{
			NMapDrawingInput drawingInput = this._drawingInput;
			if (drawingInput != null && drawingInput.DrawingMode == DrawingMode.Drawing)
			{
				NMapDrawingInput drawingInput2 = this._drawingInput;
				if (drawingInput2 != null)
				{
					drawingInput2.StopDrawing();
				}
			}
			else
			{
				NMapDrawingInput drawingInput3 = this._drawingInput;
				if (drawingInput3 != null)
				{
					drawingInput3.StopDrawing();
				}
				this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Drawing, false);
				this._drawingInput.Connect(NMapDrawingInput.SignalName.Finished, Callable.From(delegate
				{
					this._drawingInput = null;
					this.UpdateDrawingButtonStates();
				}), 0U);
				this.AddChildSafely(this._drawingInput);
			}
			this.UpdateDrawingButtonStates();
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x0011D54C File Offset: 0x0011B74C
		private void OnMapErasingButtonPressed(NButton _)
		{
			NMapDrawingInput drawingInput = this._drawingInput;
			if (drawingInput != null && drawingInput.DrawingMode == DrawingMode.Erasing)
			{
				NMapDrawingInput drawingInput2 = this._drawingInput;
				if (drawingInput2 != null)
				{
					drawingInput2.StopDrawing();
				}
			}
			else
			{
				NMapDrawingInput drawingInput3 = this._drawingInput;
				if (drawingInput3 != null)
				{
					drawingInput3.StopDrawing();
				}
				this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Erasing, false);
				this._drawingInput.Connect(NMapDrawingInput.SignalName.Finished, Callable.From(delegate
				{
					this._drawingInput = null;
					this.UpdateDrawingButtonStates();
				}), 0U);
				this.AddChildSafely(this._drawingInput);
			}
			this.UpdateDrawingButtonStates();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0011D5D8 File Offset: 0x0011B7D8
		private void UpdateDrawingButtonStates()
		{
			this._mapDrawingButton.SetIsDrawing(this.Drawings.GetLocalDrawingMode(true) == DrawingMode.Drawing);
			this._mapErasingButton.SetIsErasing(this.Drawings.GetLocalDrawingMode(true) == DrawingMode.Erasing);
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x0011D60E File Offset: 0x0011B80E
		private void OnClearMapDrawingButtonPressed(NButton _)
		{
			this.Drawings.ClearDrawnLinesLocal();
			SfxCmd.Play("event:/sfx/ui/map/map_erase", 1f);
			this.UpdateDrawingButtonStates();
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x0011D630 File Offset: 0x0011B830
		public void HighlightPointType(MapPointType pointType)
		{
			Action<MapPointType> pointTypeHighlighted = this.PointTypeHighlighted;
			if (pointTypeHighlighted == null)
			{
				return;
			}
			pointTypeHighlighted(pointType);
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x0600295A RID: 10586 RVA: 0x0011D644 File Offset: 0x0011B844
		public Control DefaultFocusedControl
		{
			get
			{
				NMapPoint nmapPoint = this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((NMapPoint n) => n.IsEnabled);
				if (nmapPoint != null)
				{
					return nmapPoint;
				}
				return this;
			}
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x0011D688 File Offset: 0x0011B888
		public void PingMapCoord(MapCoord coord, Player player)
		{
			NMapPoint nmapPoint;
			if (!this._mapPointDictionary.TryGetValue(coord, out nmapPoint))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Someone tried to ping map coord ");
				defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(coord);
				defaultInterpolatedStringHandler.AppendLiteral(" that doesn't exist!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			NMapPingVfx nmapPingVfx = NMapPingVfx.Create();
			nmapPingVfx.Modulate = player.Character.MapDrawingColor;
			nmapPoint.AddChildSafely(nmapPingVfx);
			nmapPoint.MoveChildSafely(nmapPingVfx, 0);
			nmapPingVfx.Position = Vector2.Zero;
			nmapPingVfx.Size *= nmapPoint.Size.X * 0.015625f;
			nmapPingVfx.PivotOffset = nmapPingVfx.Size * 0.5f;
			NRun.Instance.GlobalUi.MultiplayerPlayerContainer.FlashPlayerReady(player);
			NDebugAudioManager.Instance.Play("map_ping.mp3", 1f, PitchVariance.Medium);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0011D774 File Offset: 0x0011B974
		private void OnLegendHotkeyPressed()
		{
			List<NMapLegendItem> list = this._legendItems.GetChildren(false).OfType<NMapLegendItem>().ToList<NMapLegendItem>();
			if (!list.Any<NMapLegendItem>((NMapLegendItem c) => base.GetViewport().GuiGetFocusOwner() == c))
			{
				NMapPoint nmapPoint = this._mapPointDictionary.Values.LastOrDefault<NMapPoint>((NMapPoint n) => n.IsEnabled);
				if (nmapPoint != null)
				{
					foreach (NMapLegendItem nmapLegendItem in list)
					{
						if (nmapPoint != null)
						{
							nmapLegendItem.FocusNeighborLeft = nmapPoint.GetPath();
						}
						else
						{
							nmapLegendItem.FocusNeighborLeft = base.GetPath();
						}
					}
				}
				list[0].TryGrabFocus();
				return;
			}
			NMapPoint nmapPoint2 = this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((NMapPoint n) => n.IsEnabled);
			if (nmapPoint2 == null)
			{
				return;
			}
			nmapPoint2.TryGrabFocus();
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0011D87C File Offset: 0x0011BA7C
		private void OnDrawingToolsHotkeyPressed()
		{
			NMapDrawingInput drawingInput = this._drawingInput;
			if (drawingInput != null && drawingInput.DrawingMode == DrawingMode.Erasing)
			{
				this._mapErasingButton.TryGrabFocus();
				return;
			}
			this._mapDrawingButton.TryGrabFocus();
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0011D8B4 File Offset: 0x0011BAB4
		public Vector2 GetNetPositionFromScreenPosition(Vector2 screenPosition)
		{
			Vector2 vector = this._mapBgContainer.GetGlobalTransformWithCanvas().Inverse() * screenPosition;
			Vector2 vector2 = this._mapBgContainer.Size * 0.5f;
			Vector2 vector3;
			vector3..ctor(960f, vector2.Y);
			return (vector - vector2) / vector3;
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x0011D914 File Offset: 0x0011BB14
		private Vector2 GetMapPositionFromNetPosition(Vector2 netPosition)
		{
			Vector2 vector = this._mapBgContainer.Size * 0.5f;
			Vector2 vector2;
			vector2..ctor(960f, vector.Y);
			return netPosition * vector2 + vector;
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x0011D958 File Offset: 0x0011BB58
		public Vector2 GetScreenPositionFromNetPosition(Vector2 netPosition)
		{
			Vector2 mapPositionFromNetPosition = this.GetMapPositionFromNetPosition(netPosition);
			return this._mapBgContainer.GetGlobalTransformWithCanvas() * mapPositionFromNetPosition;
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x0011D980 File Offset: 0x0011BB80
		public bool IsNodeOnScreen(NMapPoint mapPoint)
		{
			float y = mapPoint.GlobalPosition.Y;
			return y > 0f && y < base.Size.Y;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x0011D9B1 File Offset: 0x0011BBB1
		public void CleanUp()
		{
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				CombatManager.Instance.Unpause();
			}
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x0011D9CC File Offset: 0x0011BBCC
		private void UpdateHotkeyDisplay()
		{
			this._legendHotkeyIcon.Visible = NControllerManager.Instance.IsUsingController;
			this._legendHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.accept);
			this._drawingToolsHotkeyIcon.Visible = NControllerManager.Instance.IsUsingController;
			this._drawingToolsHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.viewExhaustPileAndTabRight);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002964 RID: 10596 RVA: 0x0011DA44 File Offset: 0x0011BC44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(42)
			{
				new MethodInfo(NMapScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.GetLineEndpoint, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "point", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.RecalculateTravelability, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.InitMapVotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.OnMapPointSelectedLocally, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "point", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.RefreshAllMapPointVotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.RemoveAllMapPointsAndPaths, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.UpdateScrollPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.ProcessMouseEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.ProcessMouseDrawingEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.ProcessScrollEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.ProcessControllerEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.SetTravelEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "enabled", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.SetDebugTravelEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "enabled", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.RefreshAllPointVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.PlayStartOfActAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.InitMapPrompt, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.SetInterruptable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.CanScroll, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.TryCancelStartOfActAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.OnVisibilityChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.OnCapstoneChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "animateOut", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.Open, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isOpenedFromTopBar", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.OnBackButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.OnMapDrawingButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.OnMapErasingButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.UpdateDrawingButtonStates, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.OnClearMapDrawingButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.HighlightPointType, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pointType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.OnLegendHotkeyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.OnDrawingToolsHotkeyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.GetNetPositionFromScreenPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "screenPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.GetMapPositionFromNetPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "netPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.GetScreenPositionFromNetPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "netPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.IsNodeOnScreen, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "mapPoint", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapScreen.MethodName.CleanUp, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.MethodName.UpdateHotkeyDisplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002965 RID: 10597 RVA: 0x0011E600 File Offset: 0x0011C800
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.GetLineEndpoint && args.Count == 1)
			{
				Vector2 lineEndpoint = this.GetLineEndpoint(VariantUtils.ConvertTo<NMapPoint>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref lineEndpoint);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.RecalculateTravelability && args.Count == 0)
			{
				this.RecalculateTravelability();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.InitMapVotes && args.Count == 0)
			{
				this.InitMapVotes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnMapPointSelectedLocally && args.Count == 1)
			{
				this.OnMapPointSelectedLocally(VariantUtils.ConvertTo<NMapPoint>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.RefreshAllMapPointVotes && args.Count == 0)
			{
				this.RefreshAllMapPointVotes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.RemoveAllMapPointsAndPaths && args.Count == 0)
			{
				this.RemoveAllMapPointsAndPaths();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.UpdateScrollPosition && args.Count == 1)
			{
				this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.ProcessMouseEvent && args.Count == 1)
			{
				this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.ProcessMouseDrawingEvent && args.Count == 1)
			{
				this.ProcessMouseDrawingEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.ProcessScrollEvent && args.Count == 1)
			{
				this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.ProcessControllerEvent && args.Count == 1)
			{
				this.ProcessControllerEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.SetTravelEnabled && args.Count == 1)
			{
				this.SetTravelEnabled(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.SetDebugTravelEnabled && args.Count == 1)
			{
				this.SetDebugTravelEnabled(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.RefreshAllPointVisuals && args.Count == 0)
			{
				this.RefreshAllPointVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.PlayStartOfActAnimation && args.Count == 0)
			{
				this.PlayStartOfActAnimation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.InitMapPrompt && args.Count == 0)
			{
				this.InitMapPrompt();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.SetInterruptable && args.Count == 0)
			{
				this.SetInterruptable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.CanScroll && args.Count == 0)
			{
				bool flag = this.CanScroll();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.TryCancelStartOfActAnim && args.Count == 0)
			{
				this.TryCancelStartOfActAnim();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnVisibilityChanged && args.Count == 0)
			{
				this.OnVisibilityChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnCapstoneChanged && args.Count == 0)
			{
				this.OnCapstoneChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.Close && args.Count == 1)
			{
				this.Close(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.Open && args.Count == 1)
			{
				NMapScreen nmapScreen = this.Open(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<NMapScreen>(ref nmapScreen);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnBackButtonPressed && args.Count == 1)
			{
				this.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnMapDrawingButtonPressed && args.Count == 1)
			{
				this.OnMapDrawingButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnMapErasingButtonPressed && args.Count == 1)
			{
				this.OnMapErasingButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.UpdateDrawingButtonStates && args.Count == 0)
			{
				this.UpdateDrawingButtonStates();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnClearMapDrawingButtonPressed && args.Count == 1)
			{
				this.OnClearMapDrawingButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.HighlightPointType && args.Count == 1)
			{
				this.HighlightPointType(VariantUtils.ConvertTo<MapPointType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnLegendHotkeyPressed && args.Count == 0)
			{
				this.OnLegendHotkeyPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.OnDrawingToolsHotkeyPressed && args.Count == 0)
			{
				this.OnDrawingToolsHotkeyPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.GetNetPositionFromScreenPosition && args.Count == 1)
			{
				Vector2 netPositionFromScreenPosition = this.GetNetPositionFromScreenPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref netPositionFromScreenPosition);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.GetMapPositionFromNetPosition && args.Count == 1)
			{
				Vector2 mapPositionFromNetPosition = this.GetMapPositionFromNetPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref mapPositionFromNetPosition);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.GetScreenPositionFromNetPosition && args.Count == 1)
			{
				Vector2 screenPositionFromNetPosition = this.GetScreenPositionFromNetPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref screenPositionFromNetPosition);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.IsNodeOnScreen && args.Count == 1)
			{
				bool flag2 = this.IsNodeOnScreen(VariantUtils.ConvertTo<NMapPoint>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.CleanUp && args.Count == 0)
			{
				this.CleanUp();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapScreen.MethodName.UpdateHotkeyDisplay && args.Count == 0)
			{
				this.UpdateHotkeyDisplay();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002966 RID: 10598 RVA: 0x0011EDA0 File Offset: 0x0011CFA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapScreen.MethodName._Ready || (ref method) == NMapScreen.MethodName._ExitTree || (ref method) == NMapScreen.MethodName.GetLineEndpoint || (ref method) == NMapScreen.MethodName.RecalculateTravelability || (ref method) == NMapScreen.MethodName.InitMapVotes || (ref method) == NMapScreen.MethodName.OnMapPointSelectedLocally || (ref method) == NMapScreen.MethodName.RefreshAllMapPointVotes || (ref method) == NMapScreen.MethodName.RemoveAllMapPointsAndPaths || (ref method) == NMapScreen.MethodName._Process || (ref method) == NMapScreen.MethodName.UpdateScrollPosition || (ref method) == NMapScreen.MethodName._GuiInput || (ref method) == NMapScreen.MethodName.ProcessMouseEvent || (ref method) == NMapScreen.MethodName.ProcessMouseDrawingEvent || (ref method) == NMapScreen.MethodName.ProcessScrollEvent || (ref method) == NMapScreen.MethodName.ProcessControllerEvent || (ref method) == NMapScreen.MethodName.SetTravelEnabled || (ref method) == NMapScreen.MethodName.SetDebugTravelEnabled || (ref method) == NMapScreen.MethodName.RefreshAllPointVisuals || (ref method) == NMapScreen.MethodName.PlayStartOfActAnimation || (ref method) == NMapScreen.MethodName.InitMapPrompt || (ref method) == NMapScreen.MethodName.SetInterruptable || (ref method) == NMapScreen.MethodName.CanScroll || (ref method) == NMapScreen.MethodName.TryCancelStartOfActAnim || (ref method) == NMapScreen.MethodName.OnVisibilityChanged || (ref method) == NMapScreen.MethodName.OnCapstoneChanged || (ref method) == NMapScreen.MethodName.Close || (ref method) == NMapScreen.MethodName.Open || (ref method) == NMapScreen.MethodName.OnBackButtonPressed || (ref method) == NMapScreen.MethodName._Input || (ref method) == NMapScreen.MethodName.OnMapDrawingButtonPressed || (ref method) == NMapScreen.MethodName.OnMapErasingButtonPressed || (ref method) == NMapScreen.MethodName.UpdateDrawingButtonStates || (ref method) == NMapScreen.MethodName.OnClearMapDrawingButtonPressed || (ref method) == NMapScreen.MethodName.HighlightPointType || (ref method) == NMapScreen.MethodName.OnLegendHotkeyPressed || (ref method) == NMapScreen.MethodName.OnDrawingToolsHotkeyPressed || (ref method) == NMapScreen.MethodName.GetNetPositionFromScreenPosition || (ref method) == NMapScreen.MethodName.GetMapPositionFromNetPosition || (ref method) == NMapScreen.MethodName.GetScreenPositionFromNetPosition || (ref method) == NMapScreen.MethodName.IsNodeOnScreen || (ref method) == NMapScreen.MethodName.CleanUp || (ref method) == NMapScreen.MethodName.UpdateHotkeyDisplay || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002967 RID: 10599 RVA: 0x0011F02C File Offset: 0x0011D22C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapScreen.PropertyName.IsOpen)
			{
				this.IsOpen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.IsTravelEnabled)
			{
				this.IsTravelEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.IsDebugTravelEnabled)
			{
				this.IsDebugTravelEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.IsTraveling)
			{
				this.IsTraveling = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.Drawings)
			{
				this.Drawings = VariantUtils.ConvertTo<NMapDrawings>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapContainer)
			{
				this._mapContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._pathsContainer)
			{
				this._pathsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._points)
			{
				this._points = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._bossPointNode)
			{
				this._bossPointNode = VariantUtils.ConvertTo<NBossMapPoint>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._secondBossPointNode)
			{
				this._secondBossPointNode = VariantUtils.ConvertTo<NBossMapPoint>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._startingPointNode)
			{
				this._startingPointNode = VariantUtils.ConvertTo<NMapPoint>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapBgContainer)
			{
				this._mapBgContainer = VariantUtils.ConvertTo<NMapBg>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._marker)
			{
				this._marker = VariantUtils.ConvertTo<NMapMarker>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._drawingToolsHotkeyIcon)
			{
				this._drawingToolsHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._drawingTools)
			{
				this._drawingTools = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapDrawingButton)
			{
				this._mapDrawingButton = VariantUtils.ConvertTo<NMapDrawButton>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapErasingButton)
			{
				this._mapErasingButton = VariantUtils.ConvertTo<NMapEraseButton>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapClearButton)
			{
				this._mapClearButton = VariantUtils.ConvertTo<NMapClearButton>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapLegend)
			{
				this._mapLegend = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._legendItems)
			{
				this._legendItems = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._legendHotkeyIcon)
			{
				this._legendHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._startDragPos)
			{
				this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._targetDragPos)
			{
				this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._hasPlayedAnimation)
			{
				this._hasPlayedAnimation = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._controllerScrollAmount)
			{
				this._controllerScrollAmount = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._distX)
			{
				this._distX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._distY)
			{
				this._distY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._actAnimTween)
			{
				this._actAnimTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapScrollAnimTimer)
			{
				this._mapScrollAnimTimer = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._canInterruptAnim)
			{
				this._canInterruptAnim = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._isInputDisabled)
			{
				this._isInputDisabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._promptTween)
			{
				this._promptTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._drawingInput)
			{
				this._drawingInput = VariantUtils.ConvertTo<NMapDrawingInput>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002968 RID: 10600 RVA: 0x0011F428 File Offset: 0x0011D628
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapScreen.PropertyName.IsOpen)
			{
				bool flag = this.IsOpen;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.IsTravelEnabled)
			{
				bool flag = this.IsTravelEnabled;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.IsDebugTravelEnabled)
			{
				bool flag = this.IsDebugTravelEnabled;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.MapLegendX)
			{
				float mapLegendX = this.MapLegendX;
				value = VariantUtils.CreateFrom<float>(ref mapLegendX);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.IsTraveling)
			{
				bool flag = this.IsTraveling;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.Drawings)
			{
				NMapDrawings drawings = this.Drawings;
				value = VariantUtils.CreateFrom<NMapDrawings>(ref drawings);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._mapContainer);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._pathsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._pathsContainer);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._points)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._points);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._bossPointNode)
			{
				value = VariantUtils.CreateFrom<NBossMapPoint>(ref this._bossPointNode);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._secondBossPointNode)
			{
				value = VariantUtils.CreateFrom<NBossMapPoint>(ref this._secondBossPointNode);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._startingPointNode)
			{
				value = VariantUtils.CreateFrom<NMapPoint>(ref this._startingPointNode);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapBgContainer)
			{
				value = VariantUtils.CreateFrom<NMapBg>(ref this._mapBgContainer);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._marker)
			{
				value = VariantUtils.CreateFrom<NMapMarker>(ref this._marker);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._drawingToolsHotkeyIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._drawingToolsHotkeyIcon);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._drawingTools)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._drawingTools);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapDrawingButton)
			{
				value = VariantUtils.CreateFrom<NMapDrawButton>(ref this._mapDrawingButton);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapErasingButton)
			{
				value = VariantUtils.CreateFrom<NMapEraseButton>(ref this._mapErasingButton);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapClearButton)
			{
				value = VariantUtils.CreateFrom<NMapClearButton>(ref this._mapClearButton);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapLegend)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._mapLegend);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._legendItems)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._legendItems);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._legendHotkeyIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._legendHotkeyIcon);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._backstop);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._startDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._targetDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._isDragging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._hasPlayedAnimation)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._hasPlayedAnimation);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._controllerScrollAmount)
			{
				value = VariantUtils.CreateFrom<float>(ref this._controllerScrollAmount);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._distX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._distX);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._distY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._distY);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._actAnimTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._actAnimTween);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapScrollAnimTimer)
			{
				value = VariantUtils.CreateFrom<float>(ref this._mapScrollAnimTimer);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapAnimStartDelay)
			{
				value = VariantUtils.CreateFrom<double>(ref this._mapAnimStartDelay);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._mapAnimDuration)
			{
				value = VariantUtils.CreateFrom<double>(ref this._mapAnimDuration);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._canInterruptAnim)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._canInterruptAnim);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._isInputDisabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isInputDisabled);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._promptTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._promptTween);
				return true;
			}
			if ((ref name) == NMapScreen.PropertyName._drawingInput)
			{
				value = VariantUtils.CreateFrom<NMapDrawingInput>(ref this._drawingInput);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002969 RID: 10601 RVA: 0x0011F974 File Offset: 0x0011DB74
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NMapScreen.PropertyName.IsOpen, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapScreen.PropertyName.IsTravelEnabled, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapScreen.PropertyName.IsDebugTravelEnabled, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._mapContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._pathsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._points, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._bossPointNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._secondBossPointNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._startingPointNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._mapBgContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._marker, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._drawingToolsHotkeyIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._drawingTools, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._mapDrawingButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._mapErasingButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._mapClearButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._mapLegend, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._legendItems, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._legendHotkeyIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMapScreen.PropertyName._startDragPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMapScreen.PropertyName._targetDragPos, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapScreen.PropertyName._isDragging, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapScreen.PropertyName._hasPlayedAnimation, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapScreen.PropertyName.MapLegendX, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapScreen.PropertyName.IsTraveling, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapScreen.PropertyName._controllerScrollAmount, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapScreen.PropertyName._distX, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapScreen.PropertyName._distY, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._actAnimTween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapScreen.PropertyName._mapScrollAnimTimer, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapScreen.PropertyName._mapAnimStartDelay, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapScreen.PropertyName._mapAnimDuration, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapScreen.PropertyName._canInterruptAnim, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapScreen.PropertyName._isInputDisabled, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._promptTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName.Drawings, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName._drawingInput, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600296A RID: 10602 RVA: 0x0011FEC0 File Offset: 0x0011E0C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isOpen = NMapScreen.PropertyName.IsOpen;
			bool flag = this.IsOpen;
			info.AddProperty(isOpen, Variant.From<bool>(ref flag));
			StringName isTravelEnabled = NMapScreen.PropertyName.IsTravelEnabled;
			flag = this.IsTravelEnabled;
			info.AddProperty(isTravelEnabled, Variant.From<bool>(ref flag));
			StringName isDebugTravelEnabled = NMapScreen.PropertyName.IsDebugTravelEnabled;
			flag = this.IsDebugTravelEnabled;
			info.AddProperty(isDebugTravelEnabled, Variant.From<bool>(ref flag));
			StringName isTraveling = NMapScreen.PropertyName.IsTraveling;
			flag = this.IsTraveling;
			info.AddProperty(isTraveling, Variant.From<bool>(ref flag));
			StringName drawings = NMapScreen.PropertyName.Drawings;
			NMapDrawings drawings2 = this.Drawings;
			info.AddProperty(drawings, Variant.From<NMapDrawings>(ref drawings2));
			info.AddProperty(NMapScreen.PropertyName._mapContainer, Variant.From<Control>(ref this._mapContainer));
			info.AddProperty(NMapScreen.PropertyName._pathsContainer, Variant.From<Control>(ref this._pathsContainer));
			info.AddProperty(NMapScreen.PropertyName._points, Variant.From<Control>(ref this._points));
			info.AddProperty(NMapScreen.PropertyName._bossPointNode, Variant.From<NBossMapPoint>(ref this._bossPointNode));
			info.AddProperty(NMapScreen.PropertyName._secondBossPointNode, Variant.From<NBossMapPoint>(ref this._secondBossPointNode));
			info.AddProperty(NMapScreen.PropertyName._startingPointNode, Variant.From<NMapPoint>(ref this._startingPointNode));
			info.AddProperty(NMapScreen.PropertyName._mapBgContainer, Variant.From<NMapBg>(ref this._mapBgContainer));
			info.AddProperty(NMapScreen.PropertyName._marker, Variant.From<NMapMarker>(ref this._marker));
			info.AddProperty(NMapScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NMapScreen.PropertyName._drawingToolsHotkeyIcon, Variant.From<TextureRect>(ref this._drawingToolsHotkeyIcon));
			info.AddProperty(NMapScreen.PropertyName._drawingTools, Variant.From<Control>(ref this._drawingTools));
			info.AddProperty(NMapScreen.PropertyName._mapDrawingButton, Variant.From<NMapDrawButton>(ref this._mapDrawingButton));
			info.AddProperty(NMapScreen.PropertyName._mapErasingButton, Variant.From<NMapEraseButton>(ref this._mapErasingButton));
			info.AddProperty(NMapScreen.PropertyName._mapClearButton, Variant.From<NMapClearButton>(ref this._mapClearButton));
			info.AddProperty(NMapScreen.PropertyName._mapLegend, Variant.From<Control>(ref this._mapLegend));
			info.AddProperty(NMapScreen.PropertyName._legendItems, Variant.From<Control>(ref this._legendItems));
			info.AddProperty(NMapScreen.PropertyName._legendHotkeyIcon, Variant.From<TextureRect>(ref this._legendHotkeyIcon));
			info.AddProperty(NMapScreen.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
			info.AddProperty(NMapScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NMapScreen.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
			info.AddProperty(NMapScreen.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
			info.AddProperty(NMapScreen.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
			info.AddProperty(NMapScreen.PropertyName._hasPlayedAnimation, Variant.From<bool>(ref this._hasPlayedAnimation));
			info.AddProperty(NMapScreen.PropertyName._controllerScrollAmount, Variant.From<float>(ref this._controllerScrollAmount));
			info.AddProperty(NMapScreen.PropertyName._distX, Variant.From<float>(ref this._distX));
			info.AddProperty(NMapScreen.PropertyName._distY, Variant.From<float>(ref this._distY));
			info.AddProperty(NMapScreen.PropertyName._actAnimTween, Variant.From<Tween>(ref this._actAnimTween));
			info.AddProperty(NMapScreen.PropertyName._mapScrollAnimTimer, Variant.From<float>(ref this._mapScrollAnimTimer));
			info.AddProperty(NMapScreen.PropertyName._canInterruptAnim, Variant.From<bool>(ref this._canInterruptAnim));
			info.AddProperty(NMapScreen.PropertyName._isInputDisabled, Variant.From<bool>(ref this._isInputDisabled));
			info.AddProperty(NMapScreen.PropertyName._promptTween, Variant.From<Tween>(ref this._promptTween));
			info.AddProperty(NMapScreen.PropertyName._drawingInput, Variant.From<NMapDrawingInput>(ref this._drawingInput));
			info.AddSignalEventDelegate(NMapScreen.SignalName.Opened, this.backing_Opened);
			info.AddSignalEventDelegate(NMapScreen.SignalName.Closed, this.backing_Closed);
		}

		/// <inheritdoc />
		// Token: 0x0600296B RID: 10603 RVA: 0x00120234 File Offset: 0x0011E434
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapScreen.PropertyName.IsOpen, ref variant))
			{
				this.IsOpen = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapScreen.PropertyName.IsTravelEnabled, ref variant2))
			{
				this.IsTravelEnabled = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapScreen.PropertyName.IsDebugTravelEnabled, ref variant3))
			{
				this.IsDebugTravelEnabled = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapScreen.PropertyName.IsTraveling, ref variant4))
			{
				this.IsTraveling = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMapScreen.PropertyName.Drawings, ref variant5))
			{
				this.Drawings = variant5.As<NMapDrawings>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMapScreen.PropertyName._mapContainer, ref variant6))
			{
				this._mapContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMapScreen.PropertyName._pathsContainer, ref variant7))
			{
				this._pathsContainer = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMapScreen.PropertyName._points, ref variant8))
			{
				this._points = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMapScreen.PropertyName._bossPointNode, ref variant9))
			{
				this._bossPointNode = variant9.As<NBossMapPoint>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMapScreen.PropertyName._secondBossPointNode, ref variant10))
			{
				this._secondBossPointNode = variant10.As<NBossMapPoint>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMapScreen.PropertyName._startingPointNode, ref variant11))
			{
				this._startingPointNode = variant11.As<NMapPoint>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMapScreen.PropertyName._mapBgContainer, ref variant12))
			{
				this._mapBgContainer = variant12.As<NMapBg>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMapScreen.PropertyName._marker, ref variant13))
			{
				this._marker = variant13.As<NMapMarker>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMapScreen.PropertyName._backButton, ref variant14))
			{
				this._backButton = variant14.As<NBackButton>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMapScreen.PropertyName._drawingToolsHotkeyIcon, ref variant15))
			{
				this._drawingToolsHotkeyIcon = variant15.As<TextureRect>();
			}
			Variant variant16;
			if (info.TryGetProperty(NMapScreen.PropertyName._drawingTools, ref variant16))
			{
				this._drawingTools = variant16.As<Control>();
			}
			Variant variant17;
			if (info.TryGetProperty(NMapScreen.PropertyName._mapDrawingButton, ref variant17))
			{
				this._mapDrawingButton = variant17.As<NMapDrawButton>();
			}
			Variant variant18;
			if (info.TryGetProperty(NMapScreen.PropertyName._mapErasingButton, ref variant18))
			{
				this._mapErasingButton = variant18.As<NMapEraseButton>();
			}
			Variant variant19;
			if (info.TryGetProperty(NMapScreen.PropertyName._mapClearButton, ref variant19))
			{
				this._mapClearButton = variant19.As<NMapClearButton>();
			}
			Variant variant20;
			if (info.TryGetProperty(NMapScreen.PropertyName._mapLegend, ref variant20))
			{
				this._mapLegend = variant20.As<Control>();
			}
			Variant variant21;
			if (info.TryGetProperty(NMapScreen.PropertyName._legendItems, ref variant21))
			{
				this._legendItems = variant21.As<Control>();
			}
			Variant variant22;
			if (info.TryGetProperty(NMapScreen.PropertyName._legendHotkeyIcon, ref variant22))
			{
				this._legendHotkeyIcon = variant22.As<TextureRect>();
			}
			Variant variant23;
			if (info.TryGetProperty(NMapScreen.PropertyName._backstop, ref variant23))
			{
				this._backstop = variant23.As<Control>();
			}
			Variant variant24;
			if (info.TryGetProperty(NMapScreen.PropertyName._tween, ref variant24))
			{
				this._tween = variant24.As<Tween>();
			}
			Variant variant25;
			if (info.TryGetProperty(NMapScreen.PropertyName._startDragPos, ref variant25))
			{
				this._startDragPos = variant25.As<Vector2>();
			}
			Variant variant26;
			if (info.TryGetProperty(NMapScreen.PropertyName._targetDragPos, ref variant26))
			{
				this._targetDragPos = variant26.As<Vector2>();
			}
			Variant variant27;
			if (info.TryGetProperty(NMapScreen.PropertyName._isDragging, ref variant27))
			{
				this._isDragging = variant27.As<bool>();
			}
			Variant variant28;
			if (info.TryGetProperty(NMapScreen.PropertyName._hasPlayedAnimation, ref variant28))
			{
				this._hasPlayedAnimation = variant28.As<bool>();
			}
			Variant variant29;
			if (info.TryGetProperty(NMapScreen.PropertyName._controllerScrollAmount, ref variant29))
			{
				this._controllerScrollAmount = variant29.As<float>();
			}
			Variant variant30;
			if (info.TryGetProperty(NMapScreen.PropertyName._distX, ref variant30))
			{
				this._distX = variant30.As<float>();
			}
			Variant variant31;
			if (info.TryGetProperty(NMapScreen.PropertyName._distY, ref variant31))
			{
				this._distY = variant31.As<float>();
			}
			Variant variant32;
			if (info.TryGetProperty(NMapScreen.PropertyName._actAnimTween, ref variant32))
			{
				this._actAnimTween = variant32.As<Tween>();
			}
			Variant variant33;
			if (info.TryGetProperty(NMapScreen.PropertyName._mapScrollAnimTimer, ref variant33))
			{
				this._mapScrollAnimTimer = variant33.As<float>();
			}
			Variant variant34;
			if (info.TryGetProperty(NMapScreen.PropertyName._canInterruptAnim, ref variant34))
			{
				this._canInterruptAnim = variant34.As<bool>();
			}
			Variant variant35;
			if (info.TryGetProperty(NMapScreen.PropertyName._isInputDisabled, ref variant35))
			{
				this._isInputDisabled = variant35.As<bool>();
			}
			Variant variant36;
			if (info.TryGetProperty(NMapScreen.PropertyName._promptTween, ref variant36))
			{
				this._promptTween = variant36.As<Tween>();
			}
			Variant variant37;
			if (info.TryGetProperty(NMapScreen.PropertyName._drawingInput, ref variant37))
			{
				this._drawingInput = variant37.As<NMapDrawingInput>();
			}
			NMapScreen.OpenedEventHandler openedEventHandler;
			if (info.TryGetSignalEventDelegate<NMapScreen.OpenedEventHandler>(NMapScreen.SignalName.Opened, ref openedEventHandler))
			{
				this.backing_Opened = openedEventHandler;
			}
			NMapScreen.ClosedEventHandler closedEventHandler;
			if (info.TryGetSignalEventDelegate<NMapScreen.ClosedEventHandler>(NMapScreen.SignalName.Closed, ref closedEventHandler))
			{
				this.backing_Closed = closedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600296C RID: 10604 RVA: 0x00120684 File Offset: 0x0011E884
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMapScreen.SignalName.Opened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapScreen.SignalName.Closed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapScreen.OpenedEventHandler" />
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600296D RID: 10605 RVA: 0x001206F7 File Offset: 0x0011E8F7
		// (remove) Token: 0x0600296E RID: 10606 RVA: 0x00120710 File Offset: 0x0011E910
		[Nullable(0)]
		public event NMapScreen.OpenedEventHandler Opened
		{
			[NullableContext(0)]
			add
			{
				this.backing_Opened = (NMapScreen.OpenedEventHandler)Delegate.Combine(this.backing_Opened, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_Opened = (NMapScreen.OpenedEventHandler)Delegate.Remove(this.backing_Opened, value);
			}
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x00120729 File Offset: 0x0011E929
		protected void EmitSignalOpened()
		{
			base.EmitSignal(NMapScreen.SignalName.Opened, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapScreen.ClosedEventHandler" />
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06002970 RID: 10608 RVA: 0x0012073C File Offset: 0x0011E93C
		// (remove) Token: 0x06002971 RID: 10609 RVA: 0x00120755 File Offset: 0x0011E955
		[Nullable(0)]
		public event NMapScreen.ClosedEventHandler Closed
		{
			[NullableContext(0)]
			add
			{
				this.backing_Closed = (NMapScreen.ClosedEventHandler)Delegate.Combine(this.backing_Closed, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_Closed = (NMapScreen.ClosedEventHandler)Delegate.Remove(this.backing_Closed, value);
			}
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x0012076E File Offset: 0x0011E96E
		protected void EmitSignalClosed()
		{
			base.EmitSignal(NMapScreen.SignalName.Closed, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06002973 RID: 10611 RVA: 0x00120784 File Offset: 0x0011E984
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NMapScreen.SignalName.Opened && args.Count == 0)
			{
				NMapScreen.OpenedEventHandler openedEventHandler = this.backing_Opened;
				if (openedEventHandler == null)
				{
					return;
				}
				openedEventHandler();
				return;
			}
			else
			{
				if (!((ref signal) == NMapScreen.SignalName.Closed) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NMapScreen.ClosedEventHandler closedEventHandler = this.backing_Closed;
				if (closedEventHandler == null)
				{
					return;
				}
				closedEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06002974 RID: 10612 RVA: 0x001207E7 File Offset: 0x0011E9E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NMapScreen.SignalName.Opened || (ref signal) == NMapScreen.SignalName.Closed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000F56 RID: 3926
		private ActMap _map = NullActMap.Instance;

		// Token: 0x04000F57 RID: 3927
		private Control _mapContainer;

		// Token: 0x04000F58 RID: 3928
		private Control _pathsContainer;

		// Token: 0x04000F59 RID: 3929
		private Control _points;

		// Token: 0x04000F5A RID: 3930
		[Nullable(2)]
		private NBossMapPoint _bossPointNode;

		// Token: 0x04000F5B RID: 3931
		[Nullable(2)]
		private NBossMapPoint _secondBossPointNode;

		// Token: 0x04000F5C RID: 3932
		[Nullable(2)]
		private NMapPoint _startingPointNode;

		// Token: 0x04000F5D RID: 3933
		private NMapBg _mapBgContainer;

		// Token: 0x04000F5E RID: 3934
		private NMapMarker _marker;

		// Token: 0x04000F5F RID: 3935
		private NBackButton _backButton;

		// Token: 0x04000F60 RID: 3936
		private TextureRect _drawingToolsHotkeyIcon;

		// Token: 0x04000F61 RID: 3937
		private Control _drawingTools;

		// Token: 0x04000F62 RID: 3938
		private NMapDrawButton _mapDrawingButton;

		// Token: 0x04000F63 RID: 3939
		private NMapEraseButton _mapErasingButton;

		// Token: 0x04000F64 RID: 3940
		private NMapClearButton _mapClearButton;

		// Token: 0x04000F65 RID: 3941
		private Control _mapLegend;

		// Token: 0x04000F66 RID: 3942
		private Control _legendItems;

		// Token: 0x04000F67 RID: 3943
		private TextureRect _legendHotkeyIcon;

		// Token: 0x04000F68 RID: 3944
		private Control _backstop;

		// Token: 0x04000F69 RID: 3945
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000F6A RID: 3946
		private Vector2 _startDragPos;

		// Token: 0x04000F6B RID: 3947
		private Vector2 _targetDragPos;

		// Token: 0x04000F6C RID: 3948
		private bool _isDragging;

		// Token: 0x04000F6D RID: 3949
		private bool _hasPlayedAnimation;

		// Token: 0x04000F6F RID: 3951
		private readonly Dictionary<MapCoord, NMapPoint> _mapPointDictionary = new Dictionary<MapCoord, NMapPoint>();

		// Token: 0x04000F70 RID: 3952
		[Nullable(new byte[] { 1, 0, 1, 1 })]
		private readonly Dictionary<ValueTuple<MapCoord, MapCoord>, IReadOnlyList<TextureRect>> _paths = new Dictionary<ValueTuple<MapCoord, MapCoord>, IReadOnlyList<TextureRect>>();

		// Token: 0x04000F72 RID: 3954
		private float _controllerScrollAmount = 400f;

		// Token: 0x04000F73 RID: 3955
		private const float _scrollLimitTop = 1800f;

		// Token: 0x04000F74 RID: 3956
		private const float _scrollLimitBottom = -600f;

		// Token: 0x04000F75 RID: 3957
		private const float _totalHeight = 2325f;

		// Token: 0x04000F76 RID: 3958
		private const float _totalWidth = 1050f;

		// Token: 0x04000F77 RID: 3959
		private float _distX;

		// Token: 0x04000F78 RID: 3960
		private float _distY;

		// Token: 0x04000F79 RID: 3961
		private const float _pointJitterX = 21f;

		// Token: 0x04000F7A RID: 3962
		private const float _pointJitterY = 25f;

		// Token: 0x04000F7B RID: 3963
		private const float _tickDist = 22f;

		// Token: 0x04000F7C RID: 3964
		private const float _pathPosJitter = 3f;

		// Token: 0x04000F7D RID: 3965
		private const float _pathAngleJitter = 0.1f;

		// Token: 0x04000F7E RID: 3966
		private static readonly Vector2 _tickTraveledScale = Vector2.One * 1.2f;

		// Token: 0x04000F7F RID: 3967
		[Nullable(2)]
		private Tween _actAnimTween;

		// Token: 0x04000F80 RID: 3968
		private float _mapScrollAnimTimer;

		// Token: 0x04000F81 RID: 3969
		private const string _mapTickScenePath = "res://scenes/ui/map_dot.tscn";

		// Token: 0x04000F82 RID: 3970
		private readonly double _mapAnimStartDelay = ((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.5 : 1.0);

		// Token: 0x04000F83 RID: 3971
		private readonly double _mapAnimDuration = ((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 1.5 : 3.0);

		// Token: 0x04000F84 RID: 3972
		private bool _canInterruptAnim;

		// Token: 0x04000F85 RID: 3973
		private bool _isInputDisabled;

		// Token: 0x04000F86 RID: 3974
		private RunState _runState;

		// Token: 0x04000F87 RID: 3975
		[Nullable(2)]
		private Tween _promptTween;

		// Token: 0x04000F8A RID: 3978
		[Nullable(2)]
		private NMapDrawingInput _drawingInput;

		// Token: 0x04000F8B RID: 3979
		[Nullable(0)]
		private NMapScreen.OpenedEventHandler backing_Opened;

		// Token: 0x04000F8C RID: 3980
		[Nullable(0)]
		private NMapScreen.ClosedEventHandler backing_Closed;

		// Token: 0x020014C0 RID: 5312
		// (Invoke) Token: 0x0600A163 RID: 41315
		[NullableContext(0)]
		[Signal]
		public delegate void OpenedEventHandler();

		// Token: 0x020014C1 RID: 5313
		// (Invoke) Token: 0x0600A167 RID: 41319
		[NullableContext(0)]
		[Signal]
		public delegate void ClosedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014C2 RID: 5314
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004EC0 RID: 20160
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004EC1 RID: 20161
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'GetLineEndpoint' method.
			/// </summary>
			// Token: 0x04004EC2 RID: 20162
			public static readonly StringName GetLineEndpoint = "GetLineEndpoint";

			/// <summary>
			/// Cached name for the 'RecalculateTravelability' method.
			/// </summary>
			// Token: 0x04004EC3 RID: 20163
			public static readonly StringName RecalculateTravelability = "RecalculateTravelability";

			/// <summary>
			/// Cached name for the 'InitMapVotes' method.
			/// </summary>
			// Token: 0x04004EC4 RID: 20164
			public static readonly StringName InitMapVotes = "InitMapVotes";

			/// <summary>
			/// Cached name for the 'OnMapPointSelectedLocally' method.
			/// </summary>
			// Token: 0x04004EC5 RID: 20165
			public static readonly StringName OnMapPointSelectedLocally = "OnMapPointSelectedLocally";

			/// <summary>
			/// Cached name for the 'RefreshAllMapPointVotes' method.
			/// </summary>
			// Token: 0x04004EC6 RID: 20166
			public static readonly StringName RefreshAllMapPointVotes = "RefreshAllMapPointVotes";

			/// <summary>
			/// Cached name for the 'RemoveAllMapPointsAndPaths' method.
			/// </summary>
			// Token: 0x04004EC7 RID: 20167
			public static readonly StringName RemoveAllMapPointsAndPaths = "RemoveAllMapPointsAndPaths";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004EC8 RID: 20168
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'UpdateScrollPosition' method.
			/// </summary>
			// Token: 0x04004EC9 RID: 20169
			public static readonly StringName UpdateScrollPosition = "UpdateScrollPosition";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04004ECA RID: 20170
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'ProcessMouseEvent' method.
			/// </summary>
			// Token: 0x04004ECB RID: 20171
			public static readonly StringName ProcessMouseEvent = "ProcessMouseEvent";

			/// <summary>
			/// Cached name for the 'ProcessMouseDrawingEvent' method.
			/// </summary>
			// Token: 0x04004ECC RID: 20172
			public static readonly StringName ProcessMouseDrawingEvent = "ProcessMouseDrawingEvent";

			/// <summary>
			/// Cached name for the 'ProcessScrollEvent' method.
			/// </summary>
			// Token: 0x04004ECD RID: 20173
			public static readonly StringName ProcessScrollEvent = "ProcessScrollEvent";

			/// <summary>
			/// Cached name for the 'ProcessControllerEvent' method.
			/// </summary>
			// Token: 0x04004ECE RID: 20174
			public static readonly StringName ProcessControllerEvent = "ProcessControllerEvent";

			/// <summary>
			/// Cached name for the 'SetTravelEnabled' method.
			/// </summary>
			// Token: 0x04004ECF RID: 20175
			public static readonly StringName SetTravelEnabled = "SetTravelEnabled";

			/// <summary>
			/// Cached name for the 'SetDebugTravelEnabled' method.
			/// </summary>
			// Token: 0x04004ED0 RID: 20176
			public static readonly StringName SetDebugTravelEnabled = "SetDebugTravelEnabled";

			/// <summary>
			/// Cached name for the 'RefreshAllPointVisuals' method.
			/// </summary>
			// Token: 0x04004ED1 RID: 20177
			public static readonly StringName RefreshAllPointVisuals = "RefreshAllPointVisuals";

			/// <summary>
			/// Cached name for the 'PlayStartOfActAnimation' method.
			/// </summary>
			// Token: 0x04004ED2 RID: 20178
			public static readonly StringName PlayStartOfActAnimation = "PlayStartOfActAnimation";

			/// <summary>
			/// Cached name for the 'InitMapPrompt' method.
			/// </summary>
			// Token: 0x04004ED3 RID: 20179
			public static readonly StringName InitMapPrompt = "InitMapPrompt";

			/// <summary>
			/// Cached name for the 'SetInterruptable' method.
			/// </summary>
			// Token: 0x04004ED4 RID: 20180
			public static readonly StringName SetInterruptable = "SetInterruptable";

			/// <summary>
			/// Cached name for the 'CanScroll' method.
			/// </summary>
			// Token: 0x04004ED5 RID: 20181
			public static readonly StringName CanScroll = "CanScroll";

			/// <summary>
			/// Cached name for the 'TryCancelStartOfActAnim' method.
			/// </summary>
			// Token: 0x04004ED6 RID: 20182
			public static readonly StringName TryCancelStartOfActAnim = "TryCancelStartOfActAnim";

			/// <summary>
			/// Cached name for the 'OnVisibilityChanged' method.
			/// </summary>
			// Token: 0x04004ED7 RID: 20183
			public static readonly StringName OnVisibilityChanged = "OnVisibilityChanged";

			/// <summary>
			/// Cached name for the 'OnCapstoneChanged' method.
			/// </summary>
			// Token: 0x04004ED8 RID: 20184
			public static readonly StringName OnCapstoneChanged = "OnCapstoneChanged";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x04004ED9 RID: 20185
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x04004EDA RID: 20186
			public static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'OnBackButtonPressed' method.
			/// </summary>
			// Token: 0x04004EDB RID: 20187
			public static readonly StringName OnBackButtonPressed = "OnBackButtonPressed";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004EDC RID: 20188
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'OnMapDrawingButtonPressed' method.
			/// </summary>
			// Token: 0x04004EDD RID: 20189
			public static readonly StringName OnMapDrawingButtonPressed = "OnMapDrawingButtonPressed";

			/// <summary>
			/// Cached name for the 'OnMapErasingButtonPressed' method.
			/// </summary>
			// Token: 0x04004EDE RID: 20190
			public static readonly StringName OnMapErasingButtonPressed = "OnMapErasingButtonPressed";

			/// <summary>
			/// Cached name for the 'UpdateDrawingButtonStates' method.
			/// </summary>
			// Token: 0x04004EDF RID: 20191
			public static readonly StringName UpdateDrawingButtonStates = "UpdateDrawingButtonStates";

			/// <summary>
			/// Cached name for the 'OnClearMapDrawingButtonPressed' method.
			/// </summary>
			// Token: 0x04004EE0 RID: 20192
			public static readonly StringName OnClearMapDrawingButtonPressed = "OnClearMapDrawingButtonPressed";

			/// <summary>
			/// Cached name for the 'HighlightPointType' method.
			/// </summary>
			// Token: 0x04004EE1 RID: 20193
			public static readonly StringName HighlightPointType = "HighlightPointType";

			/// <summary>
			/// Cached name for the 'OnLegendHotkeyPressed' method.
			/// </summary>
			// Token: 0x04004EE2 RID: 20194
			public static readonly StringName OnLegendHotkeyPressed = "OnLegendHotkeyPressed";

			/// <summary>
			/// Cached name for the 'OnDrawingToolsHotkeyPressed' method.
			/// </summary>
			// Token: 0x04004EE3 RID: 20195
			public static readonly StringName OnDrawingToolsHotkeyPressed = "OnDrawingToolsHotkeyPressed";

			/// <summary>
			/// Cached name for the 'GetNetPositionFromScreenPosition' method.
			/// </summary>
			// Token: 0x04004EE4 RID: 20196
			public static readonly StringName GetNetPositionFromScreenPosition = "GetNetPositionFromScreenPosition";

			/// <summary>
			/// Cached name for the 'GetMapPositionFromNetPosition' method.
			/// </summary>
			// Token: 0x04004EE5 RID: 20197
			public static readonly StringName GetMapPositionFromNetPosition = "GetMapPositionFromNetPosition";

			/// <summary>
			/// Cached name for the 'GetScreenPositionFromNetPosition' method.
			/// </summary>
			// Token: 0x04004EE6 RID: 20198
			public static readonly StringName GetScreenPositionFromNetPosition = "GetScreenPositionFromNetPosition";

			/// <summary>
			/// Cached name for the 'IsNodeOnScreen' method.
			/// </summary>
			// Token: 0x04004EE7 RID: 20199
			public static readonly StringName IsNodeOnScreen = "IsNodeOnScreen";

			/// <summary>
			/// Cached name for the 'CleanUp' method.
			/// </summary>
			// Token: 0x04004EE8 RID: 20200
			public static readonly StringName CleanUp = "CleanUp";

			/// <summary>
			/// Cached name for the 'UpdateHotkeyDisplay' method.
			/// </summary>
			// Token: 0x04004EE9 RID: 20201
			public static readonly StringName UpdateHotkeyDisplay = "UpdateHotkeyDisplay";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014C3 RID: 5315
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsOpen' property.
			/// </summary>
			// Token: 0x04004EEA RID: 20202
			public static readonly StringName IsOpen = "IsOpen";

			/// <summary>
			/// Cached name for the 'IsTravelEnabled' property.
			/// </summary>
			// Token: 0x04004EEB RID: 20203
			public static readonly StringName IsTravelEnabled = "IsTravelEnabled";

			/// <summary>
			/// Cached name for the 'IsDebugTravelEnabled' property.
			/// </summary>
			// Token: 0x04004EEC RID: 20204
			public static readonly StringName IsDebugTravelEnabled = "IsDebugTravelEnabled";

			/// <summary>
			/// Cached name for the 'MapLegendX' property.
			/// </summary>
			// Token: 0x04004EED RID: 20205
			public static readonly StringName MapLegendX = "MapLegendX";

			/// <summary>
			/// Cached name for the 'IsTraveling' property.
			/// </summary>
			// Token: 0x04004EEE RID: 20206
			public static readonly StringName IsTraveling = "IsTraveling";

			/// <summary>
			/// Cached name for the 'Drawings' property.
			/// </summary>
			// Token: 0x04004EEF RID: 20207
			public static readonly StringName Drawings = "Drawings";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004EF0 RID: 20208
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_mapContainer' field.
			/// </summary>
			// Token: 0x04004EF1 RID: 20209
			public static readonly StringName _mapContainer = "_mapContainer";

			/// <summary>
			/// Cached name for the '_pathsContainer' field.
			/// </summary>
			// Token: 0x04004EF2 RID: 20210
			public static readonly StringName _pathsContainer = "_pathsContainer";

			/// <summary>
			/// Cached name for the '_points' field.
			/// </summary>
			// Token: 0x04004EF3 RID: 20211
			public static readonly StringName _points = "_points";

			/// <summary>
			/// Cached name for the '_bossPointNode' field.
			/// </summary>
			// Token: 0x04004EF4 RID: 20212
			public static readonly StringName _bossPointNode = "_bossPointNode";

			/// <summary>
			/// Cached name for the '_secondBossPointNode' field.
			/// </summary>
			// Token: 0x04004EF5 RID: 20213
			public static readonly StringName _secondBossPointNode = "_secondBossPointNode";

			/// <summary>
			/// Cached name for the '_startingPointNode' field.
			/// </summary>
			// Token: 0x04004EF6 RID: 20214
			public static readonly StringName _startingPointNode = "_startingPointNode";

			/// <summary>
			/// Cached name for the '_mapBgContainer' field.
			/// </summary>
			// Token: 0x04004EF7 RID: 20215
			public static readonly StringName _mapBgContainer = "_mapBgContainer";

			/// <summary>
			/// Cached name for the '_marker' field.
			/// </summary>
			// Token: 0x04004EF8 RID: 20216
			public static readonly StringName _marker = "_marker";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04004EF9 RID: 20217
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_drawingToolsHotkeyIcon' field.
			/// </summary>
			// Token: 0x04004EFA RID: 20218
			public static readonly StringName _drawingToolsHotkeyIcon = "_drawingToolsHotkeyIcon";

			/// <summary>
			/// Cached name for the '_drawingTools' field.
			/// </summary>
			// Token: 0x04004EFB RID: 20219
			public static readonly StringName _drawingTools = "_drawingTools";

			/// <summary>
			/// Cached name for the '_mapDrawingButton' field.
			/// </summary>
			// Token: 0x04004EFC RID: 20220
			public static readonly StringName _mapDrawingButton = "_mapDrawingButton";

			/// <summary>
			/// Cached name for the '_mapErasingButton' field.
			/// </summary>
			// Token: 0x04004EFD RID: 20221
			public static readonly StringName _mapErasingButton = "_mapErasingButton";

			/// <summary>
			/// Cached name for the '_mapClearButton' field.
			/// </summary>
			// Token: 0x04004EFE RID: 20222
			public static readonly StringName _mapClearButton = "_mapClearButton";

			/// <summary>
			/// Cached name for the '_mapLegend' field.
			/// </summary>
			// Token: 0x04004EFF RID: 20223
			public static readonly StringName _mapLegend = "_mapLegend";

			/// <summary>
			/// Cached name for the '_legendItems' field.
			/// </summary>
			// Token: 0x04004F00 RID: 20224
			public static readonly StringName _legendItems = "_legendItems";

			/// <summary>
			/// Cached name for the '_legendHotkeyIcon' field.
			/// </summary>
			// Token: 0x04004F01 RID: 20225
			public static readonly StringName _legendHotkeyIcon = "_legendHotkeyIcon";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x04004F02 RID: 20226
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004F03 RID: 20227
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_startDragPos' field.
			/// </summary>
			// Token: 0x04004F04 RID: 20228
			public static readonly StringName _startDragPos = "_startDragPos";

			/// <summary>
			/// Cached name for the '_targetDragPos' field.
			/// </summary>
			// Token: 0x04004F05 RID: 20229
			public static readonly StringName _targetDragPos = "_targetDragPos";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x04004F06 RID: 20230
			public static readonly StringName _isDragging = "_isDragging";

			/// <summary>
			/// Cached name for the '_hasPlayedAnimation' field.
			/// </summary>
			// Token: 0x04004F07 RID: 20231
			public static readonly StringName _hasPlayedAnimation = "_hasPlayedAnimation";

			/// <summary>
			/// Cached name for the '_controllerScrollAmount' field.
			/// </summary>
			// Token: 0x04004F08 RID: 20232
			public static readonly StringName _controllerScrollAmount = "_controllerScrollAmount";

			/// <summary>
			/// Cached name for the '_distX' field.
			/// </summary>
			// Token: 0x04004F09 RID: 20233
			public static readonly StringName _distX = "_distX";

			/// <summary>
			/// Cached name for the '_distY' field.
			/// </summary>
			// Token: 0x04004F0A RID: 20234
			public static readonly StringName _distY = "_distY";

			/// <summary>
			/// Cached name for the '_actAnimTween' field.
			/// </summary>
			// Token: 0x04004F0B RID: 20235
			public static readonly StringName _actAnimTween = "_actAnimTween";

			/// <summary>
			/// Cached name for the '_mapScrollAnimTimer' field.
			/// </summary>
			// Token: 0x04004F0C RID: 20236
			public static readonly StringName _mapScrollAnimTimer = "_mapScrollAnimTimer";

			/// <summary>
			/// Cached name for the '_mapAnimStartDelay' field.
			/// </summary>
			// Token: 0x04004F0D RID: 20237
			public static readonly StringName _mapAnimStartDelay = "_mapAnimStartDelay";

			/// <summary>
			/// Cached name for the '_mapAnimDuration' field.
			/// </summary>
			// Token: 0x04004F0E RID: 20238
			public static readonly StringName _mapAnimDuration = "_mapAnimDuration";

			/// <summary>
			/// Cached name for the '_canInterruptAnim' field.
			/// </summary>
			// Token: 0x04004F0F RID: 20239
			public static readonly StringName _canInterruptAnim = "_canInterruptAnim";

			/// <summary>
			/// Cached name for the '_isInputDisabled' field.
			/// </summary>
			// Token: 0x04004F10 RID: 20240
			public static readonly StringName _isInputDisabled = "_isInputDisabled";

			/// <summary>
			/// Cached name for the '_promptTween' field.
			/// </summary>
			// Token: 0x04004F11 RID: 20241
			public static readonly StringName _promptTween = "_promptTween";

			/// <summary>
			/// Cached name for the '_drawingInput' field.
			/// </summary>
			// Token: 0x04004F12 RID: 20242
			public static readonly StringName _drawingInput = "_drawingInput";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014C4 RID: 5316
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Opened' signal.
			/// </summary>
			// Token: 0x04004F13 RID: 20243
			public static readonly StringName Opened = "Opened";

			/// <summary>
			/// Cached name for the 'Closed' signal.
			/// </summary>
			// Token: 0x04004F14 RID: 20244
			public static readonly StringName Closed = "Closed";
		}
	}
}
