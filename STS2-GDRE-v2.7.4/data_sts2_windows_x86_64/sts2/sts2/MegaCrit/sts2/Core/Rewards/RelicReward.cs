using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x02000149 RID: 329
	[NullableContext(1)]
	[Nullable(0)]
	public class RelicReward : Reward
	{
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x000604B7 File Offset: 0x0005E6B7
		protected override RewardType RewardType
		{
			get
			{
				return RewardType.Relic;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x000604BA File Offset: 0x0005E6BA
		public override int RewardsSetIndex
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x000604BD File Offset: 0x0005E6BD
		public RelicRarity Rarity
		{
			get
			{
				return this._rarity;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x000604C5 File Offset: 0x0005E6C5
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x000604CD File Offset: 0x0005E6CD
		[Nullable(2)]
		public RelicModel ClaimedRelic
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x000604D6 File Offset: 0x0005E6D6
		[Nullable(2)]
		public RelicModel Relic
		{
			[NullableContext(2)]
			get
			{
				return this._relic;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x000604DE File Offset: 0x0005E6DE
		public override LocString Description
		{
			get
			{
				return this._relic.Title;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x000604EB File Offset: 0x0005E6EB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return this._relic.HoverTips;
			}
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x000604F8 File Offset: 0x0005E6F8
		public RelicReward(Player player)
			: base(player)
		{
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00060501 File Offset: 0x0005E701
		public RelicReward(RelicModel relic, Player player)
			: base(player)
		{
			relic.AssertMutable();
			this._predeterminedRelic = relic;
			this._relic = relic;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0006051E File Offset: 0x0005E71E
		public RelicReward(RelicRarity rarity, Player player)
			: base(player)
		{
			this._rarity = rarity;
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0006052E File Offset: 0x0005E72E
		public override bool IsPopulated
		{
			get
			{
				return this._relic != null;
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0006053C File Offset: 0x0005E73C
		public override void Populate()
		{
			if (this._relic != null)
			{
				return;
			}
			if (this._rarity != RelicRarity.None)
			{
				this._relic = RelicFactory.PullNextRelicFromFront(base.Player, this._rarity).ToMutable();
				return;
			}
			if (this._rngOverride != null)
			{
				this._relic = RelicFactory.PullNextRelicFromFront(base.Player, this._rngOverride).ToMutable();
				return;
			}
			this._relic = RelicFactory.PullNextRelicFromFront(base.Player).ToMutable();
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x000605B4 File Offset: 0x0005E7B4
		[PreserveBaseOverrides]
		public new virtual TextureRect CreateIcon()
		{
			TextureRect textureRect = new TextureRect();
			textureRect.Texture = this._relic.BigIcon;
			textureRect.Material = (ShaderMaterial)PreloadManager.Cache.GetMaterial("res://materials/ui/relic_mat.tres").Duplicate(true);
			this._relic.UpdateTexture(textureRect);
			textureRect.SetAnchorsPreset(15L, false);
			textureRect.ExpandMode = 1L;
			return textureRect;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00060618 File Offset: 0x0005E818
		protected override async Task<bool> OnSelect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(base.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this._relic.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" from relic reward");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			RelicModel relicModel = await RelicCmd.Obtain(this._relic, base.Player, -1);
			this.ClaimedRelic = relicModel;
			this._wasTaken = true;
			return true;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0006065C File Offset: 0x0005E85C
		public override void OnSkipped()
		{
			if (this._wasTaken)
			{
				return;
			}
			base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).RelicChoices.Add(new ModelChoiceHistoryEntry(this._relic.Id, false));
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000606AD File Offset: 0x0005E8AD
		public override void MarkContentAsSeen()
		{
			SaveManager.Instance.MarkRelicAsSeen(this._relic);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x000606C0 File Offset: 0x0005E8C0
		public override SerializableReward ToSerializable()
		{
			SerializableReward serializableReward = base.ToSerializable();
			if (this._predeterminedRelic != null)
			{
				serializableReward.PredeterminedModelId = this._predeterminedRelic.Id;
			}
			return serializableReward;
		}

		// Token: 0x0400059A RID: 1434
		private readonly RelicRarity _rarity;

		/// <summary>
		/// The relic that this reward gives, if it's predetermined.
		/// For randomly-rolled relic rewards (like from Elite combats), this is null.
		/// For predetermined relic rewards (like from <see cref="T:MegaCrit.Sts2.Core.Models.Events.FakeMerchant" />), this is set to the predetermined relic.
		/// </summary>
		// Token: 0x0400059B RID: 1435
		[Nullable(2)]
		private RelicModel _predeterminedRelic;

		/// <summary>
		/// The relic that this reward gives.
		/// For randomly-rolled relic rewards (like from Elite combats), this starts out null, and is populated by
		/// <see cref="M:MegaCrit.Sts2.Core.Rewards.RelicReward.Populate" />.
		/// For predetermined relic rewards (like from <see cref="T:MegaCrit.Sts2.Core.Models.Events.FakeMerchant" />), this is set to the predetermined relic.
		/// </summary>
		// Token: 0x0400059C RID: 1436
		[Nullable(2)]
		private RelicModel _relic;

		// Token: 0x0400059D RID: 1437
		private bool _wasTaken;
	}
}
