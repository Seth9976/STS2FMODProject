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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	// Token: 0x0200036A RID: 874
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Rooms/NCombatRoom.cs")]
	public class NCombatRoom : Control, IScreenContext, IRoomWithProceedButton
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x0017A1E8 File Offset: 0x001783E8
		[Nullable(2)]
		public static NCombatRoom Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.CombatRoom;
			}
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060032CF RID: 13007 RVA: 0x0017A1FC File Offset: 0x001783FC
		// (remove) Token: 0x060032D0 RID: 13008 RVA: 0x0017A234 File Offset: 0x00178434
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action ProceedButtonPressed;

		/// <summary>
		/// This is used by NControllerCardPlay to try to
		/// initially hover over the last creature that was targeted
		/// </summary>
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x0017A269 File Offset: 0x00178469
		// (set) Token: 0x060032D2 RID: 13010 RVA: 0x0017A271 File Offset: 0x00178471
		[Nullable(2)]
		public Creature LastTargetedCreature
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060032D3 RID: 13011 RVA: 0x0017A27A File Offset: 0x0017847A
		// (set) Token: 0x060032D4 RID: 13012 RVA: 0x0017A282 File Offset: 0x00178482
		public NCombatUi Ui { get; private set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x0017A28B File Offset: 0x0017848B
		public IEnumerable<NCreature> CreatureNodes
		{
			get
			{
				return this._creatureNodes;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x0017A293 File Offset: 0x00178493
		public IEnumerable<NCreature> RemovingCreatureNodes
		{
			get
			{
				IEnumerable<NCreature> removingCreatureNodes = this._removingCreatureNodes;
				Func<NCreature, bool> func;
				if ((func = NCombatRoom.<>O.<0>__IsInstanceValid) == null)
				{
					func = (NCombatRoom.<>O.<0>__IsInstanceValid = new Func<NCreature, bool>(GodotObject.IsInstanceValid));
				}
				return removingCreatureNodes.Where<NCreature>(func);
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060032D7 RID: 13015 RVA: 0x0017A2BB File Offset: 0x001784BB
		// (set) Token: 0x060032D8 RID: 13016 RVA: 0x0017A2C3 File Offset: 0x001784C3
		public Control SceneContainer { get; private set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x0017A2CC File Offset: 0x001784CC
		// (set) Token: 0x060032DA RID: 13018 RVA: 0x0017A2D4 File Offset: 0x001784D4
		private Control BgContainer { get; set; }

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x0017A2DD File Offset: 0x001784DD
		// (set) Token: 0x060032DC RID: 13020 RVA: 0x0017A2E5 File Offset: 0x001784E5
		public NCombatBackground Background { get; private set; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x0017A2EE File Offset: 0x001784EE
		public NProceedButton ProceedButton
		{
			get
			{
				return this._proceedButton;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060032DE RID: 13022 RVA: 0x0017A2F6 File Offset: 0x001784F6
		// (set) Token: 0x060032DF RID: 13023 RVA: 0x0017A2FE File Offset: 0x001784FE
		public Control BackCombatVfxContainer { get; private set; }

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x0017A307 File Offset: 0x00178507
		// (set) Token: 0x060032E1 RID: 13025 RVA: 0x0017A30F File Offset: 0x0017850F
		public Control CombatVfxContainer { get; private set; }

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x0017A318 File Offset: 0x00178518
		// (set) Token: 0x060032E3 RID: 13027 RVA: 0x0017A320 File Offset: 0x00178520
		public ulong CreatedMsec { get; private set; }

		/// <summary>
		/// The mode that this combat room is currently in.
		/// See <see cref="T:MegaCrit.Sts2.Core.Rooms.CombatRoomMode" /> docs for more info.
		/// </summary>
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x0017A329 File Offset: 0x00178529
		// (set) Token: 0x060032E5 RID: 13029 RVA: 0x0017A331 File Offset: 0x00178531
		public CombatRoomMode Mode { get; private set; }

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060032E6 RID: 13030 RVA: 0x0017A33A File Offset: 0x0017853A
		// (set) Token: 0x060032E7 RID: 13031 RVA: 0x0017A342 File Offset: 0x00178542
		[Nullable(2)]
		private Control EncounterSlots
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060032E8 RID: 13032 RVA: 0x0017A34B File Offset: 0x0017854B
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/rooms/combat_room.tscn");
			}
		}

		/// <summary>
		/// Create a new combat room node.
		/// </summary>
		/// <param name="visuals">Data used to determine what visuals should appear in the combat room.</param>
		/// <param name="mode">The mode that the combat room should start in.</param>
		// Token: 0x060032E9 RID: 13033 RVA: 0x0017A358 File Offset: 0x00178558
		[return: Nullable(2)]
		public static NCombatRoom Create(ICombatRoomVisuals visuals, CombatRoomMode mode)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom ncombatRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/combat_room.tscn").Instantiate<NCombatRoom>(0L);
			ncombatRoom._visuals = visuals;
			ncombatRoom.Mode = mode;
			return ncombatRoom;
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x0017A394 File Offset: 0x00178594
		public override void _Ready()
		{
			this.Ui = base.GetNode<NCombatUi>("%CombatUi");
			this.Ui.Deactivate();
			this.SceneContainer = base.GetNode<Control>("%CombatSceneContainer");
			this._allyContainer = base.GetNode<Control>("%AllyContainer");
			this._enemyContainer = base.GetNode<Control>("%EnemyContainer");
			this.BackCombatVfxContainer = base.GetNode<Control>("%BackCombatVfxContainer");
			this.CombatVfxContainer = base.GetNode<Control>("%CombatVfxContainer");
			this._radialBlur = base.GetNode<NRadialBlurVfx>("RadialBlur");
			this._waitingForOtherPlayersOverlay = base.GetNode<Control>("%WaitingForOtherPlayers");
			this._waitingForOtherPlayersOverlay.GetNode<MegaLabel>("Label").SetTextAutoSize(NCombatRoom._waitingLoc.GetRawText());
			this._proceedButton = base.GetNode<NProceedButton>("%ProceedButton");
			this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
			if (this.Mode == CombatRoomMode.VisualOnly && NEventRoom.Instance == null)
			{
				this._proceedButton.Visible = true;
				this._proceedButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
				this._proceedButton.Enable();
			}
			this.BgContainer = base.GetNode<Control>("%BgContainer");
			this.CreatedMsec = Time.GetTicksMsec();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Creating NCombatRoom with mode=");
			defaultInterpolatedStringHandler.AppendFormatted<CombatRoomMode>(this.Mode);
			defaultInterpolatedStringHandler.AppendLiteral(" encounter=");
			defaultInterpolatedStringHandler.AppendFormatted(this._visuals.Encounter.Id.Entry);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this.CreateAllyNodes();
			if (this.Mode != CombatRoomMode.FinishedCombat)
			{
				this.CreateEnemyNodes();
			}
			if (this.Mode != CombatRoomMode.ActiveCombat)
			{
				foreach (NCreature ncreature in this._creatureNodes)
				{
					ncreature.Hitbox.FocusMode = 0L;
					this.SetCreatureIsInteractable(ncreature.Entity, false);
				}
			}
			this.SceneContainer.Scale = Vector2.One * this._visuals.Encounter.GetCameraScaling();
			this.SceneContainer.Position += this._visuals.Encounter.GetCameraOffset();
			this.SceneContainer.ZIndex = -10;
			this.CombatVfxContainer.ZIndex = -9;
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.AdjustCreatureScaleForAspectRatio)), 0U);
			this.AdjustCreatureScaleForAspectRatio();
			NGame.Instance.SetScreenShakeTarget(this.SceneContainer);
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x0017A69C File Offset: 0x0017889C
		public static Rng GenerateBackgroundRngForCurrentPoint(IRunState state)
		{
			uint num = 0U;
			if (state.CurrentMapCoord != null)
			{
				num = (uint)(state.CurrentMapCoord.Value.row + state.CurrentMapCoord.Value.col * 747);
			}
			return new Rng(state.Rng.Seed + (ulong)num);
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x0017A6FC File Offset: 0x001788FC
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenUpdated;
			if (this.Mode == CombatRoomMode.ActiveCombat)
			{
				this.SubscribeToCombatEvents();
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x0017A724 File Offset: 0x00178924
		public override void _ExitTree()
		{
			NGame.Instance.ClearScreenShakeTarget();
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenUpdated;
			CombatManager.Instance.CombatSetUp -= this.OnCombatSetUp;
			CombatManager.Instance.CombatEnded -= this.RestrictControllerNavigation;
			CombatManager.Instance.CombatWon -= this.RestrictControllerNavigation;
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x0017A794 File Offset: 0x00178994
		private void SubscribeToCombatEvents()
		{
			CombatManager.Instance.CombatSetUp += this.OnCombatSetUp;
			CombatManager.Instance.CombatEnded += this.RestrictControllerNavigation;
			CombatManager.Instance.CombatWon += this.RestrictControllerNavigation;
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x0017A7E3 File Offset: 0x001789E3
		private void OnCombatSetUp(CombatState state)
		{
			this.Ui.Activate(((CombatRoom)this._visuals).CombatState);
			if (this.Background == null)
			{
				this.SetUpBackground(state.RunState);
			}
		}

		/// <summary>
		/// For typical combat, this is called as part of combat setup.
		/// If there is no combat setup (e.g. in pre-finished rooms), then this must be called directly, otherwise the
		/// background of the combat will not be shown.
		/// </summary>
		// Token: 0x060032F0 RID: 13040 RVA: 0x0017A814 File Offset: 0x00178A14
		public void SetUpBackground(IRunState state)
		{
			if (this.Background != null)
			{
				Log.Warn("Tried to set up background twice!", 2);
			}
			this.Background = this._visuals.Encounter.CreateBackground(this._visuals.Act, NCombatRoom.GenerateBackgroundRngForCurrentPoint(state));
			this.BgContainer.AddChildSafely(this.Background);
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x0017A86C File Offset: 0x00178A6C
		private void AdjustCreatureScaleForAspectRatio()
		{
			this._allyContainer.Scale = Vector2.One;
			this._enemyContainer.Scale = Vector2.One;
			this._enemyContainer.Position = this.SceneContainer.Size * 0.5f;
			float num = 0f;
			foreach (NCreature ncreature in this._creatureNodes)
			{
				num = Math.Max(ncreature.GlobalPosition.X + ncreature.Visuals.Bounds.Size.X * 0.5f * this.SceneContainer.Scale.X, num);
			}
			num += 15f;
			if (num > base.Size.X)
			{
				float num2 = base.Size.X / num;
				this._allyContainer.Scale = Vector2.One * num2;
				this._enemyContainer.Scale = Vector2.One * num2;
				this._enemyContainer.Position += Vector2.Left * (num - base.Size.X) * num2;
			}
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x0017A9C0 File Offset: 0x00178BC0
		private void CreateAllyNodes()
		{
			List<Creature> allies = this._visuals.Allies.ToList<Creature>();
			foreach (Creature creature in allies)
			{
				this.AddCreature(creature);
			}
			NCombatRoom.PositionPlayersAndPets(this._creatureNodes.Where<NCreature>((NCreature c) => allies.Contains(c.Entity)).ToList<NCreature>(), this._visuals.Encounter.GetCameraScaling(), this._visuals.Encounter.FullyCenterPlayers);
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x0017AA70 File Offset: 0x00178C70
		private void CreateEnemyNodes()
		{
			if (this._visuals.Encounter.HasScene)
			{
				this.EncounterSlots = this._visuals.Encounter.CreateScene();
			}
			if (this.EncounterSlots != null)
			{
				this._enemyContainer.AddChildSafely(this.EncounterSlots);
				this.EncounterSlots.Position -= new Vector2(1920f, 1080f) * 0.5f;
			}
			List<Creature> enemies = this._visuals.Enemies.ToList<Creature>();
			foreach (Creature creature in enemies)
			{
				this.AddCreature(creature);
			}
			List<NCreature> list = this._creatureNodes.Where<NCreature>((NCreature c) => enemies.Contains(c.Entity)).ToList<NCreature>();
			if (this.EncounterSlots != null)
			{
				this.PositionCreaturesWithSlots(list);
			}
			else
			{
				this.PositionEnemies(list, this._visuals.Encounter.GetCameraScaling());
			}
			this.RandomizeEnemyScalesAndHues();
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x0017AB9C File Offset: 0x00178D9C
		private void PositionCreaturesWithSlots(List<NCreature> creatures)
		{
			foreach (NCreature ncreature in creatures)
			{
				string slotName = ncreature.Entity.SlotName;
				ncreature.GlobalPosition = this.EncounterSlots.GetNode<Marker2D>(slotName).GlobalPosition;
			}
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0017AC0C File Offset: 0x00178E0C
		private void PositionEnemies(List<NCreature> creatures, float scaling)
		{
			float num = 960f / scaling;
			float num2 = 70f;
			float num3 = creatures.Sum<NCreature>((NCreature n) => n.Visuals.Bounds.Size.X);
			float num4 = num3 + (float)(creatures.Count - 1) * num2;
			float num5 = (num - num4) * 0.5f;
			num5 = Math.Max(num5, 150f);
			float num6 = 0f;
			if (num5 + num4 > num)
			{
				num2 = Math.Max((num - 150f - num3) / (float)(creatures.Count - 1), 5f);
				num4 = num3 + (float)(creatures.Count - 1) * num2;
				num5 = (num - num4) * 0.5f;
				if (num2 < 30f)
				{
					num6 = float.Lerp(60f, 40f, (num2 - 5f) / 25f);
				}
				if (num5 + num4 > num)
				{
					Log.Warn("Creatures for current encounter (" + this._visuals.Encounter.Title.GetFormattedText() + ") are being displayed off-screen because they are too wide!", 2);
				}
			}
			for (int i = 0; i < creatures.Count; i++)
			{
				NCreature ncreature = creatures[i];
				ncreature.Position = new Vector2(num5 + ncreature.Visuals.Bounds.Size.X * 0.5f, 200f - ((i % 2 != 0) ? num6 : 0f));
				num5 += ncreature.Visuals.Bounds.Size.X + num2;
			}
		}

		/// <summary>
		/// Places player creatures and pets in the combat room.
		/// It places players in an offset grid. Row/column count is determined by the nearest square root of the player
		/// count.
		/// Grid rows above the first one are offset successively, and we attempt to fit all the players in the available
		/// space, leaving enough padding in the center for card plays (usually, see <paramref name="fullyCenterPlayers" />).
		/// Pets are placed around players' feet.
		/// Technically we only support four players, but I figured I'd make it easy for modders to expand the number of players!
		/// </summary>
		/// <param name="creatureNodes">All player and pet nodes.</param>
		/// <param name="scaling">Scaling added by the camera.</param>
		/// <param name="fullyCenterPlayers">Should the players be fully centered in the scene?
		/// Usually false (to leave enough padding in the center for card plays), but true in certain situations like
		/// "surrounded" combats.
		/// </param>
		// Token: 0x060032F6 RID: 13046 RVA: 0x0017AD90 File Offset: 0x00178F90
		public static void PositionPlayersAndPets(List<NCreature> creatureNodes, float scaling, bool fullyCenterPlayers)
		{
			List<NCombatRoom.PlayerAndPets> list = new List<NCombatRoom.PlayerAndPets>();
			foreach (NCreature ncreature in creatureNodes)
			{
				if (ncreature.Entity.IsPlayer)
				{
					NCombatRoom.PlayerAndPets playerAndPets = new NCombatRoom.PlayerAndPets
					{
						player = ncreature,
						pets = new List<NCreature>()
					};
					if (LocalContext.IsMe(ncreature.Entity))
					{
						list.Insert(0, playerAndPets);
					}
					else
					{
						list.Add(playerAndPets);
					}
				}
			}
			using (List<NCreature>.Enumerator enumerator2 = creatureNodes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					NCreature creature = enumerator2.Current;
					if (!creature.Entity.IsPlayer)
					{
						list.First<NCombatRoom.PlayerAndPets>((NCombatRoom.PlayerAndPets p) => p.player.Entity.Player == creature.Entity.PetOwner).pets.Add(creature);
					}
				}
			}
			float num = 960f / scaling;
			float num2 = 70f;
			int num3 = (int)Math.Ceiling(Math.Sqrt((double)list.Count));
			int num4 = (int)Math.Ceiling((double)list.Count / (double)num3);
			float num5 = creatureNodes.Take<NCreature>(num3).Sum<NCreature>((NCreature n) => n.Visuals.Bounds.Size.X);
			float num6 = num5 + (float)(num3 - 1) * num2;
			float num7 = num5 * 0.33f;
			float num8 = ((num4 > 1) ? (num7 / (float)(num4 - 1)) : 0f);
			float num9 = ((num4 > 1) ? (120f / (float)(num4 - 1)) : 0f);
			float num10;
			if (fullyCenterPlayers)
			{
				num10 = creatureNodes.First<NCreature>((NCreature c) => c.Entity.IsPlayer).Visuals.Bounds.Size.X * -0.5f;
			}
			else
			{
				num10 = (num - num6) * 0.5f;
				num10 = Math.Max(num10, 150f);
				if (list.Count >= num3 * 2)
				{
					num5 += num7;
				}
				if (num10 + num6 > num)
				{
					num2 = (num - 150f - num5) / (float)(num3 - 1);
					num6 = num5 + (float)(num3 - 1) * num2;
					num10 = (num - num6) * 0.5f;
				}
			}
			for (int i = 0; i < num3; i++)
			{
				float num11 = num10 + num8 * (float)i;
				for (int j = 0; j < num3; j++)
				{
					int num12 = i * num3 + j;
					if (num12 >= list.Count)
					{
						break;
					}
					NCombatRoom.PlayerAndPets playerAndPets2 = list[num12];
					NCreature player = playerAndPets2.player;
					List<NCreature> pets = playerAndPets2.pets;
					player.Position = new Vector2(-num11 - player.Visuals.Bounds.Size.X * 0.5f, 200f - num9 * (float)i);
					if (LocalContext.IsMe(player.Entity) && player.Entity.Player.Character is Necrobinder)
					{
						NCreature ncreature2 = null;
						for (int k = 0; k < pets.Count; k++)
						{
							NCreature ncreature3 = pets[k];
							if (ncreature3.Entity.Monster is Osty)
							{
								ncreature2 = ncreature3;
								pets.RemoveAt(k);
								break;
							}
						}
						NCombatRoom.PositionLocalPlayerOsty(ref num11, player.Position.Y, player, ncreature2);
					}
					float num13 = ((pets.Count > 1) ? (player.Visuals.Bounds.Size.X / (float)(pets.Count - 1)) : 0f);
					for (int l = 0; l < pets.Count; l++)
					{
						NCreature ncreature4 = pets[l];
						ncreature4.Position = new Vector2(-num11 + 20f - (float)l * num13 - ncreature4.Visuals.Bounds.Size.X * 0.5f, player.Position.Y + 10f);
					}
					if (i > 0)
					{
						playerAndPets2.player.Visuals.Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
						foreach (NCreature ncreature5 in pets)
						{
							ncreature5.Visuals.Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
						}
					}
					num11 += playerAndPets2.player.Visuals.Bounds.Size.X + num2;
				}
			}
			foreach (NCombatRoom.PlayerAndPets playerAndPets3 in list)
			{
				playerAndPets3.player.GetParent().MoveChildSafely(playerAndPets3.player, 0);
				for (int m = 0; m < playerAndPets3.pets.Count; m++)
				{
					NCreature ncreature6 = playerAndPets3.pets[m];
					ncreature6.GetParent().MoveChildSafely(ncreature6, m + 1);
					if (!LocalContext.IsMe(playerAndPets3.player.Entity))
					{
						ncreature6.Visuals.Bounds.Visible = false;
					}
				}
			}
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x0017B31C File Offset: 0x0017951C
		private static void PositionLocalPlayerOsty(ref float targetXPos, float playerYPosition, NCreature player, [Nullable(2)] NCreature osty)
		{
			Vector2 position = player.Position;
			position.X = player.Position.X - 150f;
			player.Position = position;
			if (osty != null)
			{
				osty.Position = new Vector2(-targetXPos, playerYPosition) + NCreature.GetOstyOffsetFromPlayer(osty.Entity);
			}
			targetXPos += 100f;
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x0017B37C File Offset: 0x0017957C
		[NullableContext(2)]
		public NCreature GetCreatureNode(Creature creature)
		{
			if (creature != null)
			{
				return this.CreatureNodes.FirstOrDefault<NCreature>((NCreature c) => c.Entity == creature);
			}
			return null;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x0017B3B8 File Offset: 0x001795B8
		public void RemoveCreatureNode(NCreature node)
		{
			this._creatureNodes.Remove(node);
			this._removingCreatureNodes.Add(node);
			this.UpdateCreatureNavigation();
			if (base.GetViewport().GuiGetFocusOwner() == node.Hitbox)
			{
				this._creatureNodes[0].Hitbox.TryGrabFocus();
			}
			TaskHelper.RunSafely(this.RemoveCreatureWhenGone(node));
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x0017B41C File Offset: 0x0017961C
		private async Task RemoveCreatureWhenGone(NCreature node)
		{
			if (node.DeathAnimationTask != null)
			{
				await node.DeathAnimationTask;
			}
			if (this.IsValid())
			{
				this._removingCreatureNodes.Remove(node);
			}
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x0017B468 File Offset: 0x00179668
		public void AddCreature(Creature creature)
		{
			NCreature ncreature = NCreature.Create(creature);
			this._creatureNodes.Add(ncreature);
			if (creature.IsPlayer || creature.PetOwner != null)
			{
				this._allyContainer.AddChildSafely(ncreature);
			}
			else
			{
				this._enemyContainer.AddChildSafely(ncreature);
			}
			if (creature.SlotName != null)
			{
				if (this.EncounterSlots == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Creature ");
					defaultInterpolatedStringHandler.AppendFormatted<Creature>(creature);
					defaultInterpolatedStringHandler.AppendLiteral(" has slot name '");
					defaultInterpolatedStringHandler.AppendFormatted(creature.SlotName);
					defaultInterpolatedStringHandler.AppendLiteral("' but NCombatRoom.EncounterSlots is null.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ncreature.GlobalPosition = this.EncounterSlots.GetNode<Marker2D>(creature.SlotName).GlobalPosition;
			}
			this.UpdateCreatureNavigation();
			if (creature.PetOwner != null)
			{
				NCreature creatureNode = this.GetCreatureNode(creature.PetOwner.Creature);
				Player player = creatureNode.Entity.Player;
				List<NCreature> list = this._creatureNodes.Where<NCreature>((NCreature c) => c.Entity.PetOwner == player && (!(c.Entity.Monster is Osty) || !LocalContext.IsMe(player))).ToList<NCreature>();
				ncreature.GetParent().MoveChildSafely(ncreature, creatureNode.GetIndex(false) + 1);
				if (creature.Monster is Osty && LocalContext.IsMe(player))
				{
					ncreature.OstyScaleToSize((float)creature.MaxHp, 0.0);
					ncreature.GetParent().MoveChildSafely(ncreature, creatureNode.GetIndex(false));
					return;
				}
				float num = ((list.Count > 1) ? (creatureNode.Visuals.Bounds.Size.X / (float)(list.Count - 1)) : 0f);
				for (int i = 0; i < list.Count; i++)
				{
					NCreature ncreature2 = list[i];
					ncreature2.Position = new Vector2(creatureNode.Position.X - 20f + (float)i * num + ncreature2.Visuals.Bounds.Size.X * 0.5f, creatureNode.Position.Y + 10f);
					ncreature2.ToggleIsInteractable(false);
				}
				if (creatureNode.Position.Y < 199f)
				{
					ncreature.Visuals.Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
				}
			}
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x0017B6C8 File Offset: 0x001798C8
		[NullableContext(2)]
		public void SetCreatureIsInteractable(Creature creature, bool on)
		{
			NCreature creatureNode = this.GetCreatureNode(creature);
			if (creatureNode != null)
			{
				creatureNode.ToggleIsInteractable(on);
			}
			this.UpdateCreatureNavigation();
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x0017B6E4 File Offset: 0x001798E4
		private void UpdateCreatureNavigation()
		{
			List<NCreature> list = (from c in this._creatureNodes
				where c.IsInteractable
				select c into n
				orderby n.GlobalPosition.X
				select n).ToList<NCreature>();
			for (int i = 0; i < list.Count; i++)
			{
				Control hitbox = list[i].Hitbox;
				NodePath nodePath;
				if (i <= 0)
				{
					List<NCreature> list2 = list;
					nodePath = list2[list2.Count - 1].Hitbox.GetPath();
				}
				else
				{
					nodePath = list[i - 1].Hitbox.GetPath();
				}
				hitbox.FocusNeighborLeft = nodePath;
				list[i].Hitbox.FocusNeighborRight = ((i < list.Count - 1) ? list[i + 1].Hitbox.GetPath() : list[0].Hitbox.GetPath());
				list[i].Hitbox.FocusNeighborBottom = this.Ui.Hand.CardHolderContainer.GetPath();
				list[i].Hitbox.FocusNeighborTop = list[i].Hitbox.GetPath();
				list[i].UpdateNavigation();
			}
			Control cardHolderContainer = this.Ui.Hand.CardHolderContainer;
			NCreature ncreature = NCombatRoom.Instance.CreatureNodes.FirstOrDefault<NCreature>();
			cardHolderContainer.FocusNeighborTop = ((ncreature != null) ? ncreature.Hitbox.GetPath() : null);
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060032FE RID: 13054 RVA: 0x0017B869 File Offset: 0x00179A69
		public Control DefaultFocusedControl
		{
			get
			{
				return this.Ui.Hand.CardHolderContainer;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x0017B87C File Offset: 0x00179A7C
		public Control FocusedControlFromTopBar
		{
			get
			{
				NCreature ncreature = this._creatureNodes.FirstOrDefault<NCreature>(delegate(NCreature c)
				{
					if (c != null && c.IsInteractable)
					{
						Control hitbox = c.Hitbox;
						if (hitbox != null)
						{
							return hitbox.FocusMode == 2L;
						}
					}
					return false;
				});
				return ((ncreature != null) ? ncreature.Hitbox : null) ?? this.DefaultFocusedControl;
			}
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x0017B8C9 File Offset: 0x00179AC9
		private void OnActiveScreenUpdated()
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			this.UpdateControllerNavEnabled<NCombatRoom>();
			if (ActiveScreenContext.Instance.IsCurrent(this))
			{
				this.Ui.Enable();
				return;
			}
			this.Ui.Disable();
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x0017B902 File Offset: 0x00179B02
		private void RestrictControllerNavigation(CombatRoom _)
		{
			this.RestrictControllerNavigation(Array.Empty<Control>());
		}

		/// <summary>
		/// Enables controller navigation for all control nodes on the whitelist (atm we assume these are creature hitboxes).
		/// Disables controller navigation for un-whitelisted creature hitboxes and cards in hand.
		/// Use case: restrict navigation to just monsters when we are targeting them with a card.
		/// </summary>
		/// <param name="whitelist">Controls that we still want to allow input navigation for</param>
		// Token: 0x06003302 RID: 13058 RVA: 0x0017B910 File Offset: 0x00179B10
		public void RestrictControllerNavigation(IEnumerable<Control> whitelist)
		{
			foreach (NCreature ncreature in this._creatureNodes)
			{
				Control hitbox = ncreature.Hitbox;
				bool flag = whitelist.Contains(hitbox);
				hitbox.FocusMode = (flag ? 2L : 0L);
				hitbox.FocusNeighborBottom = ncreature.Hitbox.GetPath();
				NOrbManager orbManager = ncreature.OrbManager;
				if (orbManager != null)
				{
					orbManager.SetFocusBehaviorRecursive(flag ? 0L : 1L);
				}
			}
			this.Ui.Hand.DisableControllerNavigation();
		}

		/// <summary>
		/// Enables controller navigation for creatures and cards in hand.
		/// </summary>
		// Token: 0x06003303 RID: 13059 RVA: 0x0017B9B8 File Offset: 0x00179BB8
		public void EnableControllerNavigation()
		{
			if (!base.IsInsideTree())
			{
				return;
			}
			foreach (NCreature ncreature in this._creatureNodes)
			{
				if (!ncreature.Entity.IsDead)
				{
					ncreature.Hitbox.FocusMode = 2L;
					ncreature.Hitbox.FocusNeighborBottom = this.Ui.Hand.CardHolderContainer.GetPath();
					NOrbManager orbManager = ncreature.OrbManager;
					if (orbManager != null)
					{
						orbManager.SetFocusBehaviorRecursive(0L);
					}
				}
			}
			this.UpdateCreatureNavigation();
			this.Ui.Hand.EnableControllerNavigation();
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x0017BA70 File Offset: 0x00179C70
		private void RandomizeEnemyScalesAndHues()
		{
			Dictionary<Type, List<NCreature>> dictionary = new Dictionary<Type, List<NCreature>>();
			foreach (NCreature ncreature in this._creatureNodes)
			{
				if (ncreature.Entity.Side != CombatSide.Player && ncreature.Entity.Monster != null)
				{
					Type type = ncreature.Entity.Monster.GetType();
					List<NCreature> list;
					if (!dictionary.TryGetValue(type, out list))
					{
						list = new List<NCreature>();
						dictionary[type] = list;
					}
					list.Add(ncreature);
				}
			}
			foreach (KeyValuePair<Type, List<NCreature>> keyValuePair in dictionary)
			{
				if (keyValuePair.Value.Count != 1)
				{
					foreach (NCreature ncreature2 in keyValuePair.Value)
					{
						MonsterModel monster = ncreature2.Entity.Monster;
						int value = monster.Creature.MonsterMaxHpBeforeModification.Value;
						float num;
						if (monster.MaxInitialHp == monster.MinInitialHp)
						{
							num = 0f;
						}
						else
						{
							num = ((float)(value - monster.MinInitialHp) / (float)(monster.MaxInitialHp - monster.MinInitialHp) - 0.5f) * 2f;
						}
						num = Mathf.Clamp(num, 0f, 1f);
						float num2 = Math.Max(ncreature2.Visuals.Bounds.Size.X, ncreature2.Visuals.Bounds.Size.Y);
						float num3 = Mathf.Clamp(Mathf.InverseLerp(250f, 100f, num2), 0f, 1f);
						float num4 = float.Lerp(0.1f, 0.15f, num3);
						ncreature2.SetScaleAndHue(1f + num * num4, Rng.Chaotic.NextFloat(0.05f));
					}
				}
			}
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x0017BCD0 File Offset: 0x00179ED0
		public void RadialBlur(VfxPosition vfxPosition = VfxPosition.Center)
		{
			this._radialBlur.Activate(vfxPosition);
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x0017BCE0 File Offset: 0x00179EE0
		public void ShakeOstyIfDead(Player owner)
		{
			NCreature ncreature = this._creatureNodes.FirstOrDefault<NCreature>((NCreature c) => c.Entity.Monster is Osty && c.Entity.PetOwner == owner);
			if (ncreature != null)
			{
				ncreature.AnimShake();
			}
		}

		/// <summary>
		/// Plays the splash VFX and adds the liquid overlay on the targeted creature.
		/// </summary>
		// Token: 0x06003307 RID: 13063 RVA: 0x0017BD1C File Offset: 0x00179F1C
		public void PlaySplashVfx(Creature target, Color tint)
		{
			NCreature creatureNode = this.GetCreatureNode(target);
			if (creatureNode == null)
			{
				return;
			}
			Control combatVfxContainer = this.CombatVfxContainer;
			combatVfxContainer.AddChildSafely(NSplashVfx.Create(creatureNode.GetBottomOfHitbox(), tint));
			combatVfxContainer.AddChildSafely(NLiquidOverlayVfx.Create(target, tint));
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x0017BD5B File Offset: 0x00179F5B
		public void SetWaitingForOtherPlayersOverlayVisible(bool visible)
		{
			this._waitingForOtherPlayersOverlay.Visible = visible;
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x0017BD69 File Offset: 0x00179F69
		private void OnProceedButtonPressed(NButton button)
		{
			this._proceedButton.Disable();
			Action proceedButtonPressed = this.ProceedButtonPressed;
			if (proceedButtonPressed == null)
			{
				return;
			}
			proceedButtonPressed();
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x0017BD88 File Offset: 0x00179F88
		public void TransitionToActiveCombat(CombatRoom combatRoom)
		{
			if (this.Mode != CombatRoomMode.VisualOnly)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot transition to ");
				defaultInterpolatedStringHandler.AppendFormatted("ActiveCombat");
				defaultInterpolatedStringHandler.AppendLiteral(" from ");
				defaultInterpolatedStringHandler.AppendFormatted<CombatRoomMode>(this.Mode);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Mode = CombatRoomMode.ActiveCombat;
			this._visuals = combatRoom;
			foreach (NCreature ncreature in this._creatureNodes)
			{
				ncreature.Hitbox.FocusMode = 2L;
				this.SetCreatureIsInteractable(ncreature.Entity, true);
			}
			this.SubscribeToCombatEvents();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600330B RID: 13067 RVA: 0x0017BE60 File Offset: 0x0017A060
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(15)
			{
				new MethodInfo(NCombatRoom.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.SubscribeToCombatEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.AdjustCreatureScaleForAspectRatio, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.CreateAllyNodes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.CreateEnemyNodes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.RemoveCreatureNode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCombatRoom.MethodName.UpdateCreatureNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.EnableControllerNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.RandomizeEnemyScalesAndHues, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRoom.MethodName.RadialBlur, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "vfxPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCombatRoom.MethodName.SetWaitingForOtherPlayersOverlayVisible, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "visible", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCombatRoom.MethodName.OnProceedButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600330C RID: 13068 RVA: 0x0017C1E4 File Offset: 0x0017A3E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatRoom.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.SubscribeToCombatEvents && args.Count == 0)
			{
				this.SubscribeToCombatEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.AdjustCreatureScaleForAspectRatio && args.Count == 0)
			{
				this.AdjustCreatureScaleForAspectRatio();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.CreateAllyNodes && args.Count == 0)
			{
				this.CreateAllyNodes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.CreateEnemyNodes && args.Count == 0)
			{
				this.CreateEnemyNodes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.RemoveCreatureNode && args.Count == 1)
			{
				this.RemoveCreatureNode(VariantUtils.ConvertTo<NCreature>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.UpdateCreatureNavigation && args.Count == 0)
			{
				this.UpdateCreatureNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.OnActiveScreenUpdated && args.Count == 0)
			{
				this.OnActiveScreenUpdated();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.EnableControllerNavigation && args.Count == 0)
			{
				this.EnableControllerNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.RandomizeEnemyScalesAndHues && args.Count == 0)
			{
				this.RandomizeEnemyScalesAndHues();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.RadialBlur && args.Count == 1)
			{
				this.RadialBlur(VariantUtils.ConvertTo<VfxPosition>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.SetWaitingForOtherPlayersOverlayVisible && args.Count == 1)
			{
				this.SetWaitingForOtherPlayersOverlayVisible(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRoom.MethodName.OnProceedButtonPressed && args.Count == 1)
			{
				this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600330D RID: 13069 RVA: 0x0017C460 File Offset: 0x0017A660
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatRoom.MethodName._Ready || (ref method) == NCombatRoom.MethodName._EnterTree || (ref method) == NCombatRoom.MethodName._ExitTree || (ref method) == NCombatRoom.MethodName.SubscribeToCombatEvents || (ref method) == NCombatRoom.MethodName.AdjustCreatureScaleForAspectRatio || (ref method) == NCombatRoom.MethodName.CreateAllyNodes || (ref method) == NCombatRoom.MethodName.CreateEnemyNodes || (ref method) == NCombatRoom.MethodName.RemoveCreatureNode || (ref method) == NCombatRoom.MethodName.UpdateCreatureNavigation || (ref method) == NCombatRoom.MethodName.OnActiveScreenUpdated || (ref method) == NCombatRoom.MethodName.EnableControllerNavigation || (ref method) == NCombatRoom.MethodName.RandomizeEnemyScalesAndHues || (ref method) == NCombatRoom.MethodName.RadialBlur || (ref method) == NCombatRoom.MethodName.SetWaitingForOtherPlayersOverlayVisible || (ref method) == NCombatRoom.MethodName.OnProceedButtonPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600330E RID: 13070 RVA: 0x0017C558 File Offset: 0x0017A758
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatRoom.PropertyName.Ui)
			{
				this.Ui = VariantUtils.ConvertTo<NCombatUi>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.SceneContainer)
			{
				this.SceneContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.BgContainer)
			{
				this.BgContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.Background)
			{
				this.Background = VariantUtils.ConvertTo<NCombatBackground>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.BackCombatVfxContainer)
			{
				this.BackCombatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.CombatVfxContainer)
			{
				this.CombatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.CreatedMsec)
			{
				this.CreatedMsec = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.Mode)
			{
				this.Mode = VariantUtils.ConvertTo<CombatRoomMode>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.EncounterSlots)
			{
				this.EncounterSlots = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._allyContainer)
			{
				this._allyContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._enemyContainer)
			{
				this._enemyContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._radialBlur)
			{
				this._radialBlur = VariantUtils.ConvertTo<NRadialBlurVfx>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._proceedButton)
			{
				this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._waitingForOtherPlayersOverlay)
			{
				this._waitingForOtherPlayersOverlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600330F RID: 13071 RVA: 0x0017C704 File Offset: 0x0017A904
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatRoom.PropertyName.Ui)
			{
				NCombatUi ui = this.Ui;
				value = VariantUtils.CreateFrom<NCombatUi>(ref ui);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.SceneContainer)
			{
				Control control = this.SceneContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.BgContainer)
			{
				Control control = this.BgContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.Background)
			{
				NCombatBackground background = this.Background;
				value = VariantUtils.CreateFrom<NCombatBackground>(ref background);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.ProceedButton)
			{
				NProceedButton proceedButton = this.ProceedButton;
				value = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.BackCombatVfxContainer)
			{
				Control control = this.BackCombatVfxContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.CombatVfxContainer)
			{
				Control control = this.CombatVfxContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.CreatedMsec)
			{
				ulong createdMsec = this.CreatedMsec;
				value = VariantUtils.CreateFrom<ulong>(ref createdMsec);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.Mode)
			{
				CombatRoomMode mode = this.Mode;
				value = VariantUtils.CreateFrom<CombatRoomMode>(ref mode);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.EncounterSlots)
			{
				Control control = this.EncounterSlots;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._allyContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._allyContainer);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._enemyContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._enemyContainer);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._radialBlur)
			{
				value = VariantUtils.CreateFrom<NRadialBlurVfx>(ref this._radialBlur);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._proceedButton)
			{
				value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._waitingForOtherPlayersOverlay)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._waitingForOtherPlayersOverlay);
				return true;
			}
			if ((ref name) == NCombatRoom.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003310 RID: 13072 RVA: 0x0017C980 File Offset: 0x0017AB80
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatRoom.PropertyName.Ui, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName._allyContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName._enemyContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.SceneContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.BgContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.Background, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName._radialBlur, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName._proceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.ProceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName._waitingForOtherPlayersOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.BackCombatVfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.CombatVfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCombatRoom.PropertyName.CreatedMsec, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCombatRoom.PropertyName.Mode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.EncounterSlots, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRoom.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003311 RID: 13073 RVA: 0x0017CBE4 File Offset: 0x0017ADE4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName ui = NCombatRoom.PropertyName.Ui;
			NCombatUi ui2 = this.Ui;
			info.AddProperty(ui, Variant.From<NCombatUi>(ref ui2));
			StringName sceneContainer = NCombatRoom.PropertyName.SceneContainer;
			Control control = this.SceneContainer;
			info.AddProperty(sceneContainer, Variant.From<Control>(ref control));
			StringName bgContainer = NCombatRoom.PropertyName.BgContainer;
			control = this.BgContainer;
			info.AddProperty(bgContainer, Variant.From<Control>(ref control));
			StringName background = NCombatRoom.PropertyName.Background;
			NCombatBackground background2 = this.Background;
			info.AddProperty(background, Variant.From<NCombatBackground>(ref background2));
			StringName backCombatVfxContainer = NCombatRoom.PropertyName.BackCombatVfxContainer;
			control = this.BackCombatVfxContainer;
			info.AddProperty(backCombatVfxContainer, Variant.From<Control>(ref control));
			StringName combatVfxContainer = NCombatRoom.PropertyName.CombatVfxContainer;
			control = this.CombatVfxContainer;
			info.AddProperty(combatVfxContainer, Variant.From<Control>(ref control));
			StringName createdMsec = NCombatRoom.PropertyName.CreatedMsec;
			ulong createdMsec2 = this.CreatedMsec;
			info.AddProperty(createdMsec, Variant.From<ulong>(ref createdMsec2));
			StringName mode = NCombatRoom.PropertyName.Mode;
			CombatRoomMode mode2 = this.Mode;
			info.AddProperty(mode, Variant.From<CombatRoomMode>(ref mode2));
			StringName encounterSlots = NCombatRoom.PropertyName.EncounterSlots;
			control = this.EncounterSlots;
			info.AddProperty(encounterSlots, Variant.From<Control>(ref control));
			info.AddProperty(NCombatRoom.PropertyName._allyContainer, Variant.From<Control>(ref this._allyContainer));
			info.AddProperty(NCombatRoom.PropertyName._enemyContainer, Variant.From<Control>(ref this._enemyContainer));
			info.AddProperty(NCombatRoom.PropertyName._radialBlur, Variant.From<NRadialBlurVfx>(ref this._radialBlur));
			info.AddProperty(NCombatRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
			info.AddProperty(NCombatRoom.PropertyName._waitingForOtherPlayersOverlay, Variant.From<Control>(ref this._waitingForOtherPlayersOverlay));
			info.AddProperty(NCombatRoom.PropertyName._window, Variant.From<Window>(ref this._window));
		}

		/// <inheritdoc />
		// Token: 0x06003312 RID: 13074 RVA: 0x0017CD60 File Offset: 0x0017AF60
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatRoom.PropertyName.Ui, ref variant))
			{
				this.Ui = variant.As<NCombatUi>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatRoom.PropertyName.SceneContainer, ref variant2))
			{
				this.SceneContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatRoom.PropertyName.BgContainer, ref variant3))
			{
				this.BgContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCombatRoom.PropertyName.Background, ref variant4))
			{
				this.Background = variant4.As<NCombatBackground>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCombatRoom.PropertyName.BackCombatVfxContainer, ref variant5))
			{
				this.BackCombatVfxContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCombatRoom.PropertyName.CombatVfxContainer, ref variant6))
			{
				this.CombatVfxContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCombatRoom.PropertyName.CreatedMsec, ref variant7))
			{
				this.CreatedMsec = variant7.As<ulong>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCombatRoom.PropertyName.Mode, ref variant8))
			{
				this.Mode = variant8.As<CombatRoomMode>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCombatRoom.PropertyName.EncounterSlots, ref variant9))
			{
				this.EncounterSlots = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCombatRoom.PropertyName._allyContainer, ref variant10))
			{
				this._allyContainer = variant10.As<Control>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCombatRoom.PropertyName._enemyContainer, ref variant11))
			{
				this._enemyContainer = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCombatRoom.PropertyName._radialBlur, ref variant12))
			{
				this._radialBlur = variant12.As<NRadialBlurVfx>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCombatRoom.PropertyName._proceedButton, ref variant13))
			{
				this._proceedButton = variant13.As<NProceedButton>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCombatRoom.PropertyName._waitingForOtherPlayersOverlay, ref variant14))
			{
				this._waitingForOtherPlayersOverlay = variant14.As<Control>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCombatRoom.PropertyName._window, ref variant15))
			{
				this._window = variant15.As<Window>();
			}
		}

		// Token: 0x040013BB RID: 5051
		private const float _centerSafeZone = 150f;

		// Token: 0x040013BC RID: 5052
		private const float _defaultPadding = 70f;

		// Token: 0x040013BD RID: 5053
		private const float _minimumAutoPadding = 5f;

		// Token: 0x040013BE RID: 5054
		private const float _minAlternatingYPos = 40f;

		// Token: 0x040013BF RID: 5055
		private const float _maxAlternatingYPos = 60f;

		// Token: 0x040013C0 RID: 5056
		private const float _alternateYPosBeginPadding = 30f;

		// Token: 0x040013C1 RID: 5057
		private const float _yPos = 200f;

		// Token: 0x040013C2 RID: 5058
		private static readonly LocString _waitingLoc = new LocString("gameplay_ui", "MULTIPLAYER_WAITING");

		// Token: 0x040013C4 RID: 5060
		private readonly List<NCreature> _creatureNodes = new List<NCreature>();

		// Token: 0x040013C5 RID: 5061
		private readonly List<NCreature> _removingCreatureNodes = new List<NCreature>();

		// Token: 0x040013C8 RID: 5064
		private Control _allyContainer;

		// Token: 0x040013C9 RID: 5065
		private Control _enemyContainer;

		// Token: 0x040013CA RID: 5066
		private const string _scenePath = "res://scenes/rooms/combat_room.tscn";

		// Token: 0x040013CE RID: 5070
		private NRadialBlurVfx _radialBlur;

		/// <summary>
		/// TEMPORARY!
		/// Used when showing a <see cref="F:MegaCrit.Sts2.Core.Rooms.CombatRoomMode.VisualOnly" /> combat room, to allow proceeding to the next room.
		/// We'll probably remove this once we have more event-like buttons set up for this.
		/// </summary>
		// Token: 0x040013CF RID: 5071
		private NProceedButton _proceedButton;

		// Token: 0x040013D0 RID: 5072
		private Control _waitingForOtherPlayersOverlay;

		// Token: 0x040013D5 RID: 5077
		private ICombatRoomVisuals _visuals;

		// Token: 0x040013D7 RID: 5079
		private Window _window;

		// Token: 0x0200167D RID: 5757
		[Nullable(0)]
		private struct PlayerAndPets
		{
			// Token: 0x04005797 RID: 22423
			public NCreature player;

			// Token: 0x04005798 RID: 22424
			public List<NCreature> pets;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200167E RID: 5758
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005799 RID: 22425
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400579A RID: 22426
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400579B RID: 22427
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SubscribeToCombatEvents' method.
			/// </summary>
			// Token: 0x0400579C RID: 22428
			public static readonly StringName SubscribeToCombatEvents = "SubscribeToCombatEvents";

			/// <summary>
			/// Cached name for the 'AdjustCreatureScaleForAspectRatio' method.
			/// </summary>
			// Token: 0x0400579D RID: 22429
			public static readonly StringName AdjustCreatureScaleForAspectRatio = "AdjustCreatureScaleForAspectRatio";

			/// <summary>
			/// Cached name for the 'CreateAllyNodes' method.
			/// </summary>
			// Token: 0x0400579E RID: 22430
			public static readonly StringName CreateAllyNodes = "CreateAllyNodes";

			/// <summary>
			/// Cached name for the 'CreateEnemyNodes' method.
			/// </summary>
			// Token: 0x0400579F RID: 22431
			public static readonly StringName CreateEnemyNodes = "CreateEnemyNodes";

			/// <summary>
			/// Cached name for the 'RemoveCreatureNode' method.
			/// </summary>
			// Token: 0x040057A0 RID: 22432
			public static readonly StringName RemoveCreatureNode = "RemoveCreatureNode";

			/// <summary>
			/// Cached name for the 'UpdateCreatureNavigation' method.
			/// </summary>
			// Token: 0x040057A1 RID: 22433
			public static readonly StringName UpdateCreatureNavigation = "UpdateCreatureNavigation";

			/// <summary>
			/// Cached name for the 'OnActiveScreenUpdated' method.
			/// </summary>
			// Token: 0x040057A2 RID: 22434
			public static readonly StringName OnActiveScreenUpdated = "OnActiveScreenUpdated";

			/// <summary>
			/// Cached name for the 'EnableControllerNavigation' method.
			/// </summary>
			// Token: 0x040057A3 RID: 22435
			public static readonly StringName EnableControllerNavigation = "EnableControllerNavigation";

			/// <summary>
			/// Cached name for the 'RandomizeEnemyScalesAndHues' method.
			/// </summary>
			// Token: 0x040057A4 RID: 22436
			public static readonly StringName RandomizeEnemyScalesAndHues = "RandomizeEnemyScalesAndHues";

			/// <summary>
			/// Cached name for the 'RadialBlur' method.
			/// </summary>
			// Token: 0x040057A5 RID: 22437
			public static readonly StringName RadialBlur = "RadialBlur";

			/// <summary>
			/// Cached name for the 'SetWaitingForOtherPlayersOverlayVisible' method.
			/// </summary>
			// Token: 0x040057A6 RID: 22438
			public static readonly StringName SetWaitingForOtherPlayersOverlayVisible = "SetWaitingForOtherPlayersOverlayVisible";

			/// <summary>
			/// Cached name for the 'OnProceedButtonPressed' method.
			/// </summary>
			// Token: 0x040057A7 RID: 22439
			public static readonly StringName OnProceedButtonPressed = "OnProceedButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200167F RID: 5759
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Ui' property.
			/// </summary>
			// Token: 0x040057A8 RID: 22440
			public static readonly StringName Ui = "Ui";

			/// <summary>
			/// Cached name for the 'SceneContainer' property.
			/// </summary>
			// Token: 0x040057A9 RID: 22441
			public static readonly StringName SceneContainer = "SceneContainer";

			/// <summary>
			/// Cached name for the 'BgContainer' property.
			/// </summary>
			// Token: 0x040057AA RID: 22442
			public static readonly StringName BgContainer = "BgContainer";

			/// <summary>
			/// Cached name for the 'Background' property.
			/// </summary>
			// Token: 0x040057AB RID: 22443
			public static readonly StringName Background = "Background";

			/// <summary>
			/// Cached name for the 'ProceedButton' property.
			/// </summary>
			// Token: 0x040057AC RID: 22444
			public static readonly StringName ProceedButton = "ProceedButton";

			/// <summary>
			/// Cached name for the 'BackCombatVfxContainer' property.
			/// </summary>
			// Token: 0x040057AD RID: 22445
			public static readonly StringName BackCombatVfxContainer = "BackCombatVfxContainer";

			/// <summary>
			/// Cached name for the 'CombatVfxContainer' property.
			/// </summary>
			// Token: 0x040057AE RID: 22446
			public static readonly StringName CombatVfxContainer = "CombatVfxContainer";

			/// <summary>
			/// Cached name for the 'CreatedMsec' property.
			/// </summary>
			// Token: 0x040057AF RID: 22447
			public static readonly StringName CreatedMsec = "CreatedMsec";

			/// <summary>
			/// Cached name for the 'Mode' property.
			/// </summary>
			// Token: 0x040057B0 RID: 22448
			public static readonly StringName Mode = "Mode";

			/// <summary>
			/// Cached name for the 'EncounterSlots' property.
			/// </summary>
			// Token: 0x040057B1 RID: 22449
			public static readonly StringName EncounterSlots = "EncounterSlots";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040057B2 RID: 22450
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x040057B3 RID: 22451
			public static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_allyContainer' field.
			/// </summary>
			// Token: 0x040057B4 RID: 22452
			public static readonly StringName _allyContainer = "_allyContainer";

			/// <summary>
			/// Cached name for the '_enemyContainer' field.
			/// </summary>
			// Token: 0x040057B5 RID: 22453
			public static readonly StringName _enemyContainer = "_enemyContainer";

			/// <summary>
			/// Cached name for the '_radialBlur' field.
			/// </summary>
			// Token: 0x040057B6 RID: 22454
			public static readonly StringName _radialBlur = "_radialBlur";

			/// <summary>
			/// Cached name for the '_proceedButton' field.
			/// </summary>
			// Token: 0x040057B7 RID: 22455
			public static readonly StringName _proceedButton = "_proceedButton";

			/// <summary>
			/// Cached name for the '_waitingForOtherPlayersOverlay' field.
			/// </summary>
			// Token: 0x040057B8 RID: 22456
			public static readonly StringName _waitingForOtherPlayersOverlay = "_waitingForOtherPlayersOverlay";

			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x040057B9 RID: 22457
			public static readonly StringName _window = "_window";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001680 RID: 5760
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}

		// Token: 0x02001681 RID: 5761
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040057BA RID: 22458
			[Nullable(0)]
			public static Func<NCreature, bool> <0>__IsInstanceValid;
		}
	}
}
