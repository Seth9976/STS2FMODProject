using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Random
{
	// Token: 0x02000150 RID: 336
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerRngSet
	{
		/// <summary>
		/// Determines per-player reward generation. This means:
		/// * What cards are generated for rewards.
		/// * What potions are generated for rewards.
		/// * Whether or not the player gets a potion reward at all.
		/// </summary>
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x0006193F File Offset: 0x0005FB3F
		public Rng Rewards
		{
			get
			{
				return this.GetRng(PlayerRngType.Rewards);
			}
		}

		/// <summary>
		/// Determines what the different shops are selling.
		/// </summary>
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x00061948 File Offset: 0x0005FB48
		public Rng Shops
		{
			get
			{
				return this.GetRng(PlayerRngType.Shops);
			}
		}

		/// <summary>
		/// Determines what a transformed card will roll into,
		/// </summary>
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x00061951 File Offset: 0x0005FB51
		public Rng Transformations
		{
			get
			{
				return this.GetRng(PlayerRngType.Transformations);
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0006195A File Offset: 0x0005FB5A
		public ulong Seed { get; }

		// Token: 0x0600103F RID: 4159 RVA: 0x00061964 File Offset: 0x0005FB64
		public PlayerRngSet(ulong seed)
		{
			this.Seed = seed;
			foreach (PlayerRngType playerRngType in Enum.GetValues<PlayerRngType>())
			{
				this._rngs[playerRngType] = this.CreateRng(playerRngType);
			}
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x000619B4 File Offset: 0x0005FBB4
		private Rng CreateRng(PlayerRngType rngType)
		{
			string text = StringHelper.SnakeCase(rngType.ToString());
			return new Rng(this.Seed, text);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x000619E0 File Offset: 0x0005FBE0
		public SerializablePlayerRngSet ToSerializable()
		{
			SerializablePlayerRngSet serializablePlayerRngSet = new SerializablePlayerRngSet
			{
				Seed = this.Seed
			};
			foreach (KeyValuePair<PlayerRngType, Rng> keyValuePair in this._rngs)
			{
				PlayerRngType playerRngType;
				Rng rng;
				keyValuePair.Deconstruct(out playerRngType, out rng);
				PlayerRngType playerRngType2 = playerRngType;
				Rng rng2 = rng;
				serializablePlayerRngSet.Rngs[playerRngType2] = rng2.ToSerializable();
			}
			return serializablePlayerRngSet;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00061A64 File Offset: 0x0005FC64
		public static PlayerRngSet FromSerializable(SerializablePlayerRngSet save)
		{
			PlayerRngSet playerRngSet = new PlayerRngSet(save.Seed);
			foreach (KeyValuePair<PlayerRngType, SerializableRng> keyValuePair in save.Rngs)
			{
				PlayerRngType playerRngType;
				SerializableRng serializableRng;
				keyValuePair.Deconstruct(out playerRngType, out serializableRng);
				PlayerRngType playerRngType2 = playerRngType;
				SerializableRng serializableRng2 = serializableRng;
				playerRngSet._rngs[playerRngType2] = new Rng(serializableRng2);
			}
			return playerRngSet;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00061AE4 File Offset: 0x0005FCE4
		public void LoadFromSerializable(SerializablePlayerRngSet save)
		{
			if (this.Seed != save.Seed)
			{
				throw new NotImplementedException("RngSet seed should not change during the run!");
			}
			foreach (KeyValuePair<PlayerRngType, SerializableRng> keyValuePair in save.Rngs)
			{
				PlayerRngType playerRngType;
				SerializableRng serializableRng;
				keyValuePair.Deconstruct(out playerRngType, out serializableRng);
				PlayerRngType playerRngType2 = playerRngType;
				SerializableRng serializableRng2 = serializableRng;
				this._rngs[playerRngType2].LoadFromSerializable(serializableRng2);
			}
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00061B6C File Offset: 0x0005FD6C
		public Rng GetRng(PlayerRngType rngType)
		{
			return this._rngs[rngType];
		}

		// Token: 0x040005BE RID: 1470
		private readonly Dictionary<PlayerRngType, Rng> _rngs = new Dictionary<PlayerRngType, Rng>();
	}
}
