using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000551 RID: 1361
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FurCoat : RelicModel
	{
		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x060050FE RID: 20734 RVA: 0x002400B0 File Offset: 0x0023E2B0
		public static LocString HistoryEntry
		{
			get
			{
				FurCoat furCoat = ModelDb.Relic<FurCoat>();
				LocString locString = new LocString("relics", furCoat.Id.Entry + ".historyEntry");
				locString.Add("Title", furCoat.Title);
				return locString;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x060050FF RID: 20735 RVA: 0x002400F5 File Offset: 0x0023E2F5
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005100 RID: 20736 RVA: 0x002400F8 File Offset: 0x0023E2F8
		// (set) Token: 0x06005101 RID: 20737 RVA: 0x00240100 File Offset: 0x0023E300
		[SavedProperty]
		public int FurCoatActIndex
		{
			get
			{
				return this._furCoatActIndex;
			}
			set
			{
				base.AssertMutable();
				this._furCoatActIndex = value;
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06005102 RID: 20738 RVA: 0x0024010F File Offset: 0x0023E30F
		// (set) Token: 0x06005103 RID: 20739 RVA: 0x00240117 File Offset: 0x0023E317
		[SavedProperty]
		private int[] FurCoatCoordCols { get; set; } = Array.Empty<int>();

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06005104 RID: 20740 RVA: 0x00240120 File Offset: 0x0023E320
		// (set) Token: 0x06005105 RID: 20741 RVA: 0x00240128 File Offset: 0x0023E328
		[SavedProperty]
		private int[] FurCoatCoordRows { get; set; } = Array.Empty<int>();

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06005106 RID: 20742 RVA: 0x00240131 File Offset: 0x0023E331
		// (set) Token: 0x06005107 RID: 20743 RVA: 0x00240139 File Offset: 0x0023E339
		[SavedProperty]
		private bool FurCoatCoordsSet { get; set; }

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06005108 RID: 20744 RVA: 0x00240142 File Offset: 0x0023E342
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", 7m));
			}
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x00240159 File Offset: 0x0023E359
		public override Task AfterObtained()
		{
			this.FurCoatActIndex = base.Owner.RunState.CurrentActIndex;
			this.AddMarkedRooms(base.Owner.RunState.Map);
			return Task.CompletedTask;
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x0024018D File Offset: 0x0023E38D
		public override ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
		{
			return this.AddMarkedRooms(map);
		}

		// Token: 0x0600510B RID: 20747 RVA: 0x00240198 File Offset: 0x0023E398
		private ActMap AddMarkedRooms(ActMap map)
		{
			if (base.Owner.RunState.CurrentActIndex != this.FurCoatActIndex)
			{
				return map;
			}
			List<MapCoord> markedCoords = this.GetMarkedCoords();
			bool flag = markedCoords == null;
			if (!flag)
			{
				flag = !markedCoords.TrueForAll((MapCoord c) => map.HasPoint(c) && (map.GetPoint(c).PointType == MapPointType.Monster || map.GetPoint(c).PointType == MapPointType.Elite));
			}
			if (flag)
			{
				Rng rng = new Rng(base.Owner, base.Id, 0UL);
				List<MapPoint> list = map.GetAllMapPoints().Where<MapPoint>(delegate(MapPoint p)
				{
					MapPointType pointType = p.PointType;
					bool flag2 = pointType - MapPointType.Monster <= 1;
					if (flag2)
					{
						return !p.Quests.Any<AbstractModel>((AbstractModel q) => q is FurCoat);
					}
					return false;
				}).ToList<MapPoint>();
				list.UnstableShuffle<MapPoint>(rng);
				int intValue = base.DynamicVars["Combats"].IntValue;
				List<MapPoint> list2 = list.Take<MapPoint>(intValue).ToList<MapPoint>();
				this.FurCoatCoordCols = new int[list2.Count];
				this.FurCoatCoordRows = new int[list2.Count];
				for (int i = 0; i < list2.Count; i++)
				{
					this.FurCoatCoordCols[i] = list2[i].coord.col;
					this.FurCoatCoordRows[i] = list2[i].coord.row;
				}
				this.FurCoatCoordsSet = true;
				using (List<MapPoint>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MapPoint mapPoint = enumerator.Current;
						mapPoint.AddQuest(this);
					}
					goto IL_020D;
				}
			}
			foreach (MapCoord mapCoord in markedCoords)
			{
				MapPoint point = map.GetPoint(mapCoord);
				if (point == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(95, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Loaded a fur coat map with coordinate ");
					defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(mapCoord);
					defaultInterpolatedStringHandler.AppendLiteral(", but the generated map does not ");
					defaultInterpolatedStringHandler.AppendLiteral("contain that coordinate!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				point.AddQuest(this);
			}
			IL_020D:
			return map;
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x002403D4 File Offset: 0x0023E5D4
		public override async Task BeforeCombatStart()
		{
			List<MapCoord> markedCoords = this.GetMarkedCoords();
			if (markedCoords != null && markedCoords.Contains(base.Owner.RunState.CurrentMapPoint.coord))
			{
				MapPointHistoryEntry currentMapPointHistoryEntry = base.Owner.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(base.Owner.NetId).IsAffectedByFurCoat = true;
				}
				base.Flash();
				IReadOnlyList<Creature> hittableEnemies = base.Owner.Creature.CombatState.HittableEnemies;
				VfxCmd.PlayOnCreatureCenters(hittableEnemies, "vfx/vfx_bite");
				foreach (Creature creature in hittableEnemies)
				{
					await CreatureCmd.SetCurrentHp(creature, 1m);
				}
				IEnumerator<Creature> enumerator = null;
			}
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x00240418 File Offset: 0x0023E618
		public override async Task AfterCreatureAddedToCombat(Creature creature)
		{
			if (creature.Side == CombatSide.Enemy)
			{
				List<MapCoord> markedCoords = this.GetMarkedCoords();
				if (markedCoords != null && markedCoords.Contains(base.Owner.RunState.CurrentMapPoint.coord))
				{
					base.Flash();
					VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_bite");
					await CreatureCmd.SetCurrentHp(creature, 1m);
				}
			}
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x00240464 File Offset: 0x0023E664
		[NullableContext(2)]
		public List<MapCoord> GetMarkedCoords()
		{
			if (!this.FurCoatCoordsSet)
			{
				return null;
			}
			List<MapCoord> list = new List<MapCoord>();
			for (int i = 0; i < this.FurCoatCoordCols.Length; i++)
			{
				list.Add(new MapCoord
				{
					col = this.FurCoatCoordCols[i],
					row = this.FurCoatCoordRows[i]
				});
			}
			return list;
		}

		// Token: 0x0400244D RID: 9293
		private const string _combatsKey = "Combats";

		// Token: 0x0400244E RID: 9294
		private int _furCoatActIndex = -1;
	}
}
