using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Combat
{
	// Token: 0x02000D45 RID: 3397
	[NullableContext(1)]
	[Nullable(0)]
	public class NullCombatState : ICombatState
	{
		// Token: 0x170025B3 RID: 9651
		// (get) Token: 0x0600899C RID: 35228 RVA: 0x002E2F83 File Offset: 0x002E1183
		public static NullCombatState Instance { get; } = new NullCombatState();

		// Token: 0x170025B4 RID: 9652
		// (get) Token: 0x0600899D RID: 35229 RVA: 0x002E2F8A File Offset: 0x002E118A
		public IRunState RunState
		{
			get
			{
				return NullRunState.Instance;
			}
		}

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x0600899E RID: 35230 RVA: 0x002E2F94 File Offset: 0x002E1194
		// (remove) Token: 0x0600899F RID: 35231 RVA: 0x002E2FCC File Offset: 0x002E11CC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<ICombatState> CreaturesChanged;

		// Token: 0x170025B5 RID: 9653
		// (get) Token: 0x060089A0 RID: 35232 RVA: 0x002E3001 File Offset: 0x002E1201
		public IReadOnlyList<Creature> Allies { get; } = Array.Empty<Creature>();

		// Token: 0x170025B6 RID: 9654
		// (get) Token: 0x060089A1 RID: 35233 RVA: 0x002E3009 File Offset: 0x002E1209
		public IReadOnlyList<Creature> Enemies { get; } = Array.Empty<Creature>();

		// Token: 0x170025B7 RID: 9655
		// (get) Token: 0x060089A2 RID: 35234 RVA: 0x002E3011 File Offset: 0x002E1211
		public IReadOnlyList<Creature> Creatures { get; } = Array.Empty<Creature>();

		// Token: 0x170025B8 RID: 9656
		// (get) Token: 0x060089A3 RID: 35235 RVA: 0x002E3019 File Offset: 0x002E1219
		public IReadOnlyList<Creature> PlayerCreatures { get; } = Array.Empty<Creature>();

		// Token: 0x170025B9 RID: 9657
		// (get) Token: 0x060089A4 RID: 35236 RVA: 0x002E3021 File Offset: 0x002E1221
		public IReadOnlyList<Player> Players { get; } = Array.Empty<Player>();

		// Token: 0x170025BA RID: 9658
		// (get) Token: 0x060089A5 RID: 35237 RVA: 0x002E3029 File Offset: 0x002E1229
		public IReadOnlyList<ModifierModel> Modifiers { get; } = Array.Empty<ModifierModel>();

		// Token: 0x170025BB RID: 9659
		// (get) Token: 0x060089A6 RID: 35238 RVA: 0x002E3031 File Offset: 0x002E1231
		[Nullable(2)]
		public MultiplayerScalingModel MultiplayerScalingModel
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170025BC RID: 9660
		// (get) Token: 0x060089A7 RID: 35239 RVA: 0x002E3034 File Offset: 0x002E1234
		// (set) Token: 0x060089A8 RID: 35240 RVA: 0x002E303C File Offset: 0x002E123C
		public int RoundNumber { get; set; } = 1;

		// Token: 0x170025BD RID: 9661
		// (get) Token: 0x060089A9 RID: 35241 RVA: 0x002E3045 File Offset: 0x002E1245
		// (set) Token: 0x060089AA RID: 35242 RVA: 0x002E304D File Offset: 0x002E124D
		public CombatSide CurrentSide { get; set; }

		// Token: 0x170025BE RID: 9662
		// (get) Token: 0x060089AB RID: 35243 RVA: 0x002E3056 File Offset: 0x002E1256
		[Nullable(2)]
		public EncounterModel Encounter
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170025BF RID: 9663
		// (get) Token: 0x060089AC RID: 35244 RVA: 0x002E3059 File Offset: 0x002E1259
		public IReadOnlyList<Creature> EscapedCreatures
		{
			get
			{
				return Array.Empty<Creature>();
			}
		}

		// Token: 0x060089AD RID: 35245 RVA: 0x002E3060 File Offset: 0x002E1260
		public T CreateCard<[Nullable(0)] T>(Player owner) where T : CardModel
		{
			throw new NotImplementedException();
		}

		// Token: 0x060089AE RID: 35246 RVA: 0x002E3072 File Offset: 0x002E1272
		public CardModel CreateCard(CardModel canonicalCard, Player owner)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060089AF RID: 35247 RVA: 0x002E3079 File Offset: 0x002E1279
		public CardModel CloneCard(CardModel mutableCard)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060089B0 RID: 35248 RVA: 0x002E3080 File Offset: 0x002E1280
		public void AddCard(CardModel card, Player owner)
		{
		}

		// Token: 0x060089B1 RID: 35249 RVA: 0x002E3082 File Offset: 0x002E1282
		public void RemoveCard(CardModel card)
		{
		}

		// Token: 0x060089B2 RID: 35250 RVA: 0x002E3084 File Offset: 0x002E1284
		public bool ContainsCard(CardModel card)
		{
			return false;
		}

		// Token: 0x060089B3 RID: 35251 RVA: 0x002E3087 File Offset: 0x002E1287
		public void AddPlayer(Player player)
		{
		}

		// Token: 0x060089B4 RID: 35252 RVA: 0x002E3089 File Offset: 0x002E1289
		public Creature CreateCreature(MonsterModel monster, CombatSide side, [Nullable(2)] string slot)
		{
			return new Creature(monster, side, slot);
		}

		// Token: 0x060089B5 RID: 35253 RVA: 0x002E3093 File Offset: 0x002E1293
		public void CreatureEscaped(Creature creature)
		{
		}

		// Token: 0x060089B6 RID: 35254 RVA: 0x002E3095 File Offset: 0x002E1295
		public void RemoveCreature(Creature creature, bool unattach = true)
		{
		}

		// Token: 0x060089B7 RID: 35255 RVA: 0x002E3097 File Offset: 0x002E1297
		public bool ContainsCreature(Creature creature)
		{
			return false;
		}

		// Token: 0x060089B8 RID: 35256 RVA: 0x002E309A File Offset: 0x002E129A
		[NullableContext(0)]
		public bool ContainsMonster<T>() where T : MonsterModel
		{
			return false;
		}

		// Token: 0x060089B9 RID: 35257 RVA: 0x002E309D File Offset: 0x002E129D
		[NullableContext(2)]
		public Creature GetCreature(uint? combatId)
		{
			return null;
		}

		// Token: 0x060089BA RID: 35258 RVA: 0x002E30A0 File Offset: 0x002E12A0
		[return: Nullable(new byte[] { 1, 2 })]
		public Task<Creature> GetCreatureAsync(uint? combatId, double timeoutSec)
		{
			return Task.FromResult<Creature>(null);
		}

		// Token: 0x170025C0 RID: 9664
		// (get) Token: 0x060089BB RID: 35259 RVA: 0x002E30A8 File Offset: 0x002E12A8
		public IReadOnlyList<Creature> CreaturesOnCurrentSide { get; } = Array.Empty<Creature>();

		// Token: 0x170025C1 RID: 9665
		// (get) Token: 0x060089BC RID: 35260 RVA: 0x002E30B0 File Offset: 0x002E12B0
		public IReadOnlyList<Creature> HittableEnemies { get; } = Array.Empty<Creature>();

		// Token: 0x060089BD RID: 35261 RVA: 0x002E30B8 File Offset: 0x002E12B8
		public IReadOnlyList<Creature> GetCreaturesOnSide(CombatSide side)
		{
			return this.CreaturesOnCurrentSide;
		}

		// Token: 0x060089BE RID: 35262 RVA: 0x002E30C0 File Offset: 0x002E12C0
		public IReadOnlyList<Creature> GetOpponentsOf(Creature creature)
		{
			return Array.Empty<Creature>();
		}

		// Token: 0x060089BF RID: 35263 RVA: 0x002E30C7 File Offset: 0x002E12C7
		public IReadOnlyList<Creature> GetTeammatesOf(Creature creature)
		{
			return Array.Empty<Creature>();
		}

		// Token: 0x060089C0 RID: 35264 RVA: 0x002E30CE File Offset: 0x002E12CE
		[NullableContext(2)]
		public Player GetPlayer(ulong playerId)
		{
			return null;
		}

		// Token: 0x060089C1 RID: 35265 RVA: 0x002E30D1 File Offset: 0x002E12D1
		public IEnumerable<AbstractModel> IterateHookListeners()
		{
			return Array.Empty<AbstractModel>();
		}

		// Token: 0x060089C2 RID: 35266 RVA: 0x002E30D8 File Offset: 0x002E12D8
		public void SortEnemiesBySlotName()
		{
		}

		// Token: 0x060089C3 RID: 35267 RVA: 0x002E30DA File Offset: 0x002E12DA
		public void SetEnemyIndex(Creature creature, int index)
		{
		}

		// Token: 0x060089C4 RID: 35268 RVA: 0x002E30DC File Offset: 0x002E12DC
		public void AddCreature(Creature creature)
		{
		}

		// Token: 0x060089C5 RID: 35269 RVA: 0x002E30DE File Offset: 0x002E12DE
		public bool IsLiveCombat()
		{
			return false;
		}
	}
}
