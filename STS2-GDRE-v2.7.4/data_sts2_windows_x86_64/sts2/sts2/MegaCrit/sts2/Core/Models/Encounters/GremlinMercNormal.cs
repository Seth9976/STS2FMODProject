using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200088D RID: 2189
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GremlinMercNormal : EncounterModel
	{
		// Token: 0x17001A7D RID: 6781
		// (get) Token: 0x06006AA7 RID: 27303 RVA: 0x0027FACD File Offset: 0x0027DCCD
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A7E RID: 6782
		// (get) Token: 0x06006AA8 RID: 27304 RVA: 0x0027FAD0 File Offset: 0x0027DCD0
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x06006AA9 RID: 27305 RVA: 0x0027FAD3 File Offset: 0x0027DCD3
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<GremlinMerc>(),
					ModelDb.Monster<FatGremlin>(),
					ModelDb.Monster<SneakyGremlin>()
				});
			}
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x0027FAF8 File Offset: 0x0027DCF8
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<GremlinMerc>().ToMutable(), "merc"));
		}

		/// <summary>
		/// Whether GremlinMerc stole at least some gold this combat. Set by <see cref="T:MegaCrit.Sts2.Core.Models.Powers.SurprisePower" />
		/// when the merc dies and its stolen gold is transferred to Fat Gremlin.
		/// </summary>
		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x06006AAB RID: 27307 RVA: 0x0027FB13 File Offset: 0x0027DD13
		// (set) Token: 0x06006AAC RID: 27308 RVA: 0x0027FB1B File Offset: 0x0027DD1B
		private bool GoldWasStolen
		{
			get
			{
				return this._goldWasStolen;
			}
			set
			{
				base.AssertMutable();
				this._goldWasStolen = value;
			}
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x0027FB2A File Offset: 0x0027DD2A
		public void MarkGoldStolen()
		{
			this.GoldWasStolen = true;
		}

		// Token: 0x06006AAE RID: 27310 RVA: 0x0027FB34 File Offset: 0x0027DD34
		public override float CalculateGoldProportion(CombatState combatState)
		{
			if (!combatState.EscapedCreatures.Any<Creature>((Creature c) => c.Monster is FatGremlin))
			{
				return 1f;
			}
			if (!this.GoldWasStolen)
			{
				return 0.5f;
			}
			return 0f;
		}

		// Token: 0x04002813 RID: 10259
		public const string mercSlot = "merc";

		// Token: 0x04002814 RID: 10260
		public const string sneakySlot = "sneaky";

		// Token: 0x04002815 RID: 10261
		public const string fatSlot = "fat";

		// Token: 0x04002816 RID: 10262
		private bool _goldWasStolen;
	}
}
