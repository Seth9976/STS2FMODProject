using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004EC RID: 1260
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class RelicModel : AbstractModel
	{
		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06004E52 RID: 20050 RVA: 0x00230484 File Offset: 0x0022E684
		public virtual LocString Title
		{
			get
			{
				LocString locString = new LocString("relics", base.Id.Entry + ".title");
				if (this.IsWax)
				{
					LocString waxRelicPrefix = ToyBox.WaxRelicPrefix;
					waxRelicPrefix.Add("Title", locString);
					locString = waxRelicPrefix;
				}
				return locString;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06004E53 RID: 20051 RVA: 0x002304CE File Offset: 0x0022E6CE
		public LocString Flavor
		{
			get
			{
				return new LocString("relics", base.Id.Entry + ".flavor");
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06004E54 RID: 20052 RVA: 0x002304EF File Offset: 0x0022E6EF
		protected LocString EventDescription
		{
			get
			{
				return LocString.GetIfExists("relics", base.Id.Entry + ".eventDescription") ?? this.Description;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06004E55 RID: 20053 RVA: 0x0023051A File Offset: 0x0022E71A
		private LocString Description
		{
			get
			{
				return new LocString("relics", base.Id.Entry + ".description");
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06004E56 RID: 20054 RVA: 0x0023053C File Offset: 0x0022E73C
		protected LocString SelectionScreenPrompt
		{
			get
			{
				LocString locString = new LocString("relics", base.Id.Entry + ".selectionScreenPrompt");
				this.DynamicVars.AddTo(locString);
				return locString;
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06004E57 RID: 20055 RVA: 0x00230578 File Offset: 0x0022E778
		public LocString DynamicEventDescription
		{
			get
			{
				LocString eventDescription = this.EventDescription;
				this.DynamicVars.AddTo(eventDescription);
				eventDescription.Add("energyPrefix", EnergyIconHelper.GetPrefix(this));
				eventDescription.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
				return eventDescription;
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06004E58 RID: 20056 RVA: 0x002305BC File Offset: 0x0022E7BC
		public LocString DynamicDescription
		{
			get
			{
				LocString description = this.Description;
				this.DynamicVars.AddTo(description);
				string prefix = EnergyIconHelper.GetPrefix(this);
				description.Add("energyPrefix", prefix);
				description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
				foreach (KeyValuePair<string, object> keyValuePair in description.Variables)
				{
					EnergyVar energyVar = keyValuePair.Value as EnergyVar;
					if (energyVar != null)
					{
						energyVar.ColorPrefix = prefix;
					}
				}
				return description;
			}
		}

		/// <summary>
		/// Get the additional text that this relic should add to the "heal" option at the rest site.
		/// Returns null for relics that add no additional text.
		/// </summary>
		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06004E59 RID: 20057 RVA: 0x00230654 File Offset: 0x0022E854
		[Nullable(2)]
		protected LocString AdditionalRestSiteHealText
		{
			[NullableContext(2)]
			get
			{
				LocString ifExists = LocString.GetIfExists("relics", base.Id.Entry + ".additionalRestSiteHealText");
				if (ifExists != null)
				{
					this.DynamicVars.AddTo(ifExists);
				}
				return ifExists;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06004E5A RID: 20058 RVA: 0x00230691 File Offset: 0x0022E891
		protected virtual string IconBaseName
		{
			get
			{
				return base.Id.Entry.ToLowerInvariant();
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06004E5B RID: 20059 RVA: 0x002306A3 File Offset: 0x0022E8A3
		public virtual string PackedIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/relic_atlas.sprites/" + this.IconBaseName + ".tres");
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06004E5C RID: 20060 RVA: 0x002306BF File Offset: 0x0022E8BF
		protected virtual string PackedIconOutlinePath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/relic_outline_atlas.sprites/" + this.IconBaseName + ".tres");
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06004E5D RID: 20061 RVA: 0x002306DB File Offset: 0x0022E8DB
		protected virtual string BigIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("relics/" + this.IconBaseName + ".png");
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06004E5E RID: 20062 RVA: 0x002306F7 File Offset: 0x0022E8F7
		private string BigBetaIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("relics/beta/" + this.IconBaseName + ".png");
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06004E5F RID: 20063 RVA: 0x00230713 File Offset: 0x0022E913
		private static string MissingIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("powers/missing_power.png");
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06004E60 RID: 20064 RVA: 0x0023071F File Offset: 0x0022E91F
		public string IconPath
		{
			get
			{
				return this.PackedIconPath;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06004E61 RID: 20065 RVA: 0x00230727 File Offset: 0x0022E927
		public Texture2D Icon
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.PackedIconPath, null, 1L);
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06004E62 RID: 20066 RVA: 0x00230737 File Offset: 0x0022E937
		public Texture2D IconOutline
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.PackedIconOutlinePath, null, 1L);
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x00230747 File Offset: 0x0022E947
		public Texture2D BigIcon
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.ResolvedBigIconPath);
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x0023075C File Offset: 0x0022E95C
		private string ResolvedBigIconPath
		{
			get
			{
				if (this._resolvedBigIconPath != null)
				{
					return this._resolvedBigIconPath;
				}
				if (ResourceLoader.Exists(this.BigIconPath, ""))
				{
					this._resolvedBigIconPath = this.BigIconPath;
				}
				else if (ResourceLoader.Exists(this.BigBetaIconPath, ""))
				{
					this._resolvedBigIconPath = this.BigBetaIconPath;
				}
				else
				{
					this._resolvedBigIconPath = RelicModel.MissingIconPath;
				}
				return this._resolvedBigIconPath;
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06004E65 RID: 20069
		public abstract RelicRarity Rarity { get; }

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06004E66 RID: 20070 RVA: 0x002307C9 File Offset: 0x0022E9C9
		public RelicPoolModel Pool
		{
			get
			{
				return ModelDb.AllRelicPools.First<RelicPoolModel>((RelicPoolModel p) => p.AllRelicIds.Contains(base.Id));
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06004E67 RID: 20071 RVA: 0x002307E4 File Offset: 0x0022E9E4
		public bool IsTradable
		{
			get
			{
				if (this.IsUsedUp)
				{
					return false;
				}
				if (this.HasUponPickupEffect)
				{
					return false;
				}
				if (this.IsMelted)
				{
					return false;
				}
				if (this.SpawnsPets)
				{
					return false;
				}
				RelicRarity rarity = this.Rarity;
				bool flag = rarity == RelicRarity.Starter || rarity - RelicRarity.Event <= 1;
				return !flag;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06004E68 RID: 20072 RVA: 0x00230836 File Offset: 0x0022EA36
		public virtual bool IsAllowedInShops
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Get the player that owns this relic.
		/// Will technically be null on a canonical relic model, but we should never be checking that, so we leave this as
		/// non-nullable for convenience.
		/// </summary>
		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06004E69 RID: 20073 RVA: 0x00230839 File Offset: 0x0022EA39
		// (set) Token: 0x06004E6A RID: 20074 RVA: 0x00230847 File Offset: 0x0022EA47
		public Player Owner
		{
			get
			{
				base.AssertMutable();
				return this._owner;
			}
			set
			{
				base.AssertMutable();
				if (this._owner != null && this._owner != value)
				{
					throw new InvalidOperationException("Cannot move relic from " + base.Id.Entry + " one owner to another");
				}
				this._owner = value;
			}
		}

		/// <summary>
		/// Is this a relic with a limited number of uses that have all been used up?
		///
		/// Good example: Matryoshka after opening 2 chests.
		/// Bad example: Strawberry (no "uses", just used immediately upon pickup).
		/// </summary>
		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x00230887 File Offset: 0x0022EA87
		public virtual bool IsUsedUp
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Does this relic have an "upon pickup" effect? Example: Strawberry.
		/// </summary>
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06004E6C RID: 20076 RVA: 0x0023088A File Offset: 0x0022EA8A
		public virtual bool HasUponPickupEffect
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Does this relic spawn pets? Example: Byrdpip.
		/// </summary>
		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06004E6D RID: 20077 RVA: 0x0023088D File Offset: 0x0022EA8D
		public virtual bool SpawnsPets
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Can multiple instances of this relic be stacked? Example: Circlet.
		/// </summary>
		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06004E6E RID: 20078 RVA: 0x00230890 File Offset: 0x0022EA90
		public virtual bool IsStackable
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// See the Wax Choker relic for more info on this.
		/// </summary>
		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x00230893 File Offset: 0x0022EA93
		// (set) Token: 0x06004E70 RID: 20080 RVA: 0x0023089B File Offset: 0x0022EA9B
		[SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
		public bool IsWax
		{
			get
			{
				return this._isWax;
			}
			set
			{
				base.AssertMutable();
				this._isWax = value;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06004E71 RID: 20081 RVA: 0x002308AA File Offset: 0x0022EAAA
		// (set) Token: 0x06004E72 RID: 20082 RVA: 0x002308B2 File Offset: 0x0022EAB2
		[SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
		public bool IsMelted
		{
			get
			{
				return this._isMelted;
			}
			set
			{
				base.AssertMutable();
				this._isMelted = value;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06004E73 RID: 20083 RVA: 0x002308C1 File Offset: 0x0022EAC1
		public virtual bool AddsPet
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x002308C4 File Offset: 0x0022EAC4
		// (set) Token: 0x06004E75 RID: 20085 RVA: 0x002308CC File Offset: 0x0022EACC
		public int StackCount { get; private set; } = 1;

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x002308D5 File Offset: 0x0022EAD5
		public DynamicVarSet DynamicVars
		{
			get
			{
				if (this._dynamicVars != null)
				{
					return this._dynamicVars;
				}
				this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
				this._dynamicVars.InitializeWithOwner(this);
				return this._dynamicVars;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06004E77 RID: 20087 RVA: 0x00230909 File Offset: 0x0022EB09
		protected virtual IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06004E78 RID: 20088 RVA: 0x00230910 File Offset: 0x0022EB10
		public virtual int MerchantCost
		{
			get
			{
				int num;
				switch (this.Rarity)
				{
				case RelicRarity.None:
					num = 1;
					break;
				case RelicRarity.Starter:
					num = 999999999;
					break;
				case RelicRarity.Common:
					num = 175;
					break;
				case RelicRarity.Uncommon:
					num = 225;
					break;
				case RelicRarity.Rare:
					num = 275;
					break;
				case RelicRarity.Shop:
					num = 200;
					break;
				case RelicRarity.Event:
					num = 999999999;
					break;
				case RelicRarity.Ancient:
					num = 999999999;
					break;
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Relic ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(base.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" has invalid merchant rarity ");
					defaultInterpolatedStringHandler.AppendFormatted<RelicRarity>(this.Rarity);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				return num;
			}
		}

		/// <summary>
		/// Whether or not this relic is allowed to be rolled based on the current game state.
		/// This will usually just return true, but some relics require special conditions.
		/// </summary>
		// Token: 0x06004E79 RID: 20089 RVA: 0x002309E4 File Offset: 0x0022EBE4
		public virtual bool IsAllowed(IRunState runState)
		{
			return true;
		}

		/// <summary>
		/// Whether or not this is allowed at Neow. Only applies to Neow relics.
		/// Doing this because Kaledioscope needs the potential owning player.
		/// </summary>
		// Token: 0x06004E7A RID: 20090 RVA: 0x002309E7 File Offset: 0x0022EBE7
		public virtual bool IsAllowedAtNeow(Player player)
		{
			return this.IsAllowed(player.RunState);
		}

		/// <summary>
		/// Helper for relics that should stop spawning at the Act 3 treasure chest or later.
		/// These relics provide diminishing value late in a run.
		/// </summary>
		// Token: 0x06004E7B RID: 20091 RVA: 0x002309F8 File Offset: 0x0022EBF8
		protected static bool IsBeforeAct3TreasureChest(IRunState runState)
		{
			int num = ((runState.Players.Count > 1) ? 38 : 41);
			return runState.TotalFloor < num;
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06004E7C RID: 20092 RVA: 0x00230A23 File Offset: 0x0022EC23
		// (set) Token: 0x06004E7D RID: 20093 RVA: 0x00230A2B File Offset: 0x0022EC2B
		public int FloorAddedToDeck
		{
			get
			{
				return this._floorAddedToDeck;
			}
			set
			{
				base.AssertMutable();
				this._floorAddedToDeck = value;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004E7E RID: 20094 RVA: 0x00230A3A File Offset: 0x0022EC3A
		// (set) Token: 0x06004E7F RID: 20095 RVA: 0x00230A42 File Offset: 0x0022EC42
		public RelicStatus Status
		{
			get
			{
				return this._status;
			}
			set
			{
				base.AssertMutable();
				if (this._status == value)
				{
					return;
				}
				this._status = value;
				Action statusChanged = this.StatusChanged;
				if (statusChanged == null)
				{
					return;
				}
				statusChanged();
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06004E80 RID: 20096 RVA: 0x00230A6B File Offset: 0x0022EC6B
		public virtual bool ShowCounter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06004E81 RID: 20097 RVA: 0x00230A6E File Offset: 0x0022EC6E
		public virtual int DisplayAmount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x00230A71 File Offset: 0x0022EC71
		public void Flash()
		{
			this.Flash(new <>z__ReadOnlySingleElementList<Creature>(this.Owner.Creature));
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x00230A89 File Offset: 0x0022EC89
		public void Flash(IEnumerable<Creature> targets)
		{
			Action<RelicModel, IEnumerable<Creature>> flashed = this.Flashed;
			if (flashed == null)
			{
				return;
			}
			flashed(this, targets);
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06004E84 RID: 20100 RVA: 0x00230A9D File Offset: 0x0022EC9D
		public virtual string FlashSfx
		{
			get
			{
				return "event:/sfx/ui/relic_activate_general";
			}
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06004E85 RID: 20101 RVA: 0x00230AA4 File Offset: 0x0022ECA4
		// (remove) Token: 0x06004E86 RID: 20102 RVA: 0x00230ADC File Offset: 0x0022ECDC
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		[field: Nullable(new byte[] { 2, 1, 1, 1 })]
		public event Action<RelicModel, IEnumerable<Creature>> Flashed;

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004E87 RID: 20103 RVA: 0x00230B11 File Offset: 0x0022ED11
		public virtual bool ShouldFlashOnPlayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x00230B14 File Offset: 0x0022ED14
		protected void InvokeDisplayAmountChanged()
		{
			Action displayAmountChanged = this.DisplayAmountChanged;
			if (displayAmountChanged == null)
			{
				return;
			}
			displayAmountChanged();
		}

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06004E89 RID: 20105 RVA: 0x00230B28 File Offset: 0x0022ED28
		// (remove) Token: 0x06004E8A RID: 20106 RVA: 0x00230B60 File Offset: 0x0022ED60
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action DisplayAmountChanged;

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06004E8B RID: 20107 RVA: 0x00230B98 File Offset: 0x0022ED98
		// (remove) Token: 0x06004E8C RID: 20108 RVA: 0x00230BD0 File Offset: 0x0022EDD0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action StatusChanged;

		// Token: 0x06004E8D RID: 20109 RVA: 0x00230C08 File Offset: 0x0022EE08
		public void UpdateTexture(TextureRect texture)
		{
			ShaderMaterial shaderMaterial = (ShaderMaterial)texture.Material;
			shaderMaterial.SetShaderParameter(RelicModel._isWaxStr, (this.IsWax > false) ? 1 : 0);
			if (this.IsMelted)
			{
				texture.SelfModulate = Colors.DarkRed;
			}
			if (!RunManager.Instance.IsInProgress || this.IsMelted)
			{
				shaderMaterial.SetShaderParameter(RelicModel._pulse, 0);
				shaderMaterial.SetShaderParameter(RelicModel._isUsed, 0);
				return;
			}
			switch (this.Status)
			{
			case RelicStatus.Normal:
				shaderMaterial.SetShaderParameter(RelicModel._pulse, 0);
				shaderMaterial.SetShaderParameter(RelicModel._isUsed, 0);
				return;
			case RelicStatus.Active:
				shaderMaterial.SetShaderParameter(RelicModel._pulse, 1);
				shaderMaterial.SetShaderParameter(RelicModel._isUsed, 0);
				return;
			case RelicStatus.Disabled:
				shaderMaterial.SetShaderParameter(RelicModel._pulse, 0);
				shaderMaterial.SetShaderParameter(RelicModel._isUsed, 1);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06004E8E RID: 20110 RVA: 0x00230D10 File Offset: 0x0022EF10
		public HoverTip HoverTip
		{
			get
			{
				LocString locString = this.DynamicDescription;
				if (this.IsMelted)
				{
					locString = new LocString("gameplay_ui", "RELIC_IS_MELTED");
					locString.Add("description", this.DynamicDescription);
				}
				else if (this.IsUsedUp && base.IsMutable)
				{
					locString = new LocString("gameplay_ui", "RELIC_USED_UP");
					locString.Add("description", this.DynamicDescription);
				}
				HoverTip hoverTip = new HoverTip(this.Title, locString, null);
				hoverTip.SetCanonicalModel(this.CanonicalInstance);
				return hoverTip;
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06004E8F RID: 20111 RVA: 0x00230D9D File Offset: 0x0022EF9D
		protected virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		/// <summary>
		/// Returns all extra hovertips, excluding the HoverTip for the relic itself.
		/// Useful for event options which replicate the relic description exactly in the option description.
		/// </summary>
		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06004E90 RID: 20112 RVA: 0x00230DA4 File Offset: 0x0022EFA4
		public IEnumerable<IHoverTip> HoverTipsExcludingRelic
		{
			get
			{
				return this.ExtraHoverTips;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06004E91 RID: 20113 RVA: 0x00230DAC File Offset: 0x0022EFAC
		public unsafe IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				int num = 1;
				List<IHoverTip> list = new List<IHoverTip>(num);
				CollectionsMarshal.SetCount<IHoverTip>(list, num);
				Span<IHoverTip> span = CollectionsMarshal.AsSpan<IHoverTip>(list);
				int num2 = 0;
				*span[num2] = this.HoverTip;
				List<IHoverTip> list2 = list;
				list2.AddRange(this.ExtraHoverTips);
				return list2;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06004E92 RID: 20114 RVA: 0x00230DF3 File Offset: 0x0022EFF3
		// (set) Token: 0x06004E93 RID: 20115 RVA: 0x00230E05 File Offset: 0x0022F005
		public RelicModel CanonicalInstance
		{
			get
			{
				if (!base.IsMutable)
				{
					return this;
				}
				return this._canonicalInstance;
			}
			private set
			{
				base.AssertMutable();
				this._canonicalInstance = value;
			}
		}

		/// <summary>
		/// Set to true when this relic is removed from a player's relic inventory (happens very rarely).
		/// </summary>
		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06004E94 RID: 20116 RVA: 0x00230E14 File Offset: 0x0022F014
		// (set) Token: 0x06004E95 RID: 20117 RVA: 0x00230E1C File Offset: 0x0022F01C
		public bool HasBeenRemovedFromState { get; private set; }

		// Token: 0x06004E96 RID: 20118 RVA: 0x00230E28 File Offset: 0x0022F028
		public RelicModel ToMutable()
		{
			base.AssertCanonical();
			return (RelicModel)base.MutableClone();
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x00230E48 File Offset: 0x0022F048
		protected override void DeepCloneFields()
		{
			base.DeepCloneFields();
			this._dynamicVars = this.DynamicVars.Clone(this);
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x00230E62 File Offset: 0x0022F062
		protected override void AfterCloned()
		{
			base.AfterCloned();
			if (this._canonicalInstance == null)
			{
				this.CanonicalInstance = ModelDb.GetById<RelicModel>(base.Id);
			}
			this.HasBeenRemovedFromState = false;
			this.Flashed = null;
			this.DisplayAmountChanged = null;
			this.StatusChanged = null;
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x00230E9F File Offset: 0x0022F09F
		public void RemoveInternal()
		{
			this.HasBeenRemovedFromState = true;
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x00230EA8 File Offset: 0x0022F0A8
		public void IncrementStackCount()
		{
			base.AssertMutable();
			if (!this.IsStackable)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot increment stack count on ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(base.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" because it is not a stackable relic.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int stackCount = this.StackCount;
			this.StackCount = stackCount + 1;
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x00230F0F File Offset: 0x0022F10F
		public virtual Task AfterObtained()
		{
			return Task.CompletedTask;
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x00230F16 File Offset: 0x0022F116
		public virtual Task AfterRemoved()
		{
			return Task.CompletedTask;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x00230F1D File Offset: 0x0022F11D
		public SerializableRelic ToSerializable()
		{
			base.AssertMutable();
			return new SerializableRelic
			{
				Id = base.Id,
				Props = SavedProperties.From(this),
				FloorAddedToDeck = new int?(this.FloorAddedToDeck)
			};
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x00230F54 File Offset: 0x0022F154
		public static RelicModel FromSerializable(SerializableRelic save)
		{
			RelicModel relicModel = SaveUtil.RelicOrDeprecated(save.Id).ToMutable();
			SavedProperties props = save.Props;
			if (props != null)
			{
				props.Fill(relicModel);
			}
			if (save.FloorAddedToDeck != null)
			{
				relicModel.FloorAddedToDeck = save.FloorAddedToDeck.Value;
			}
			return relicModel;
		}

		/// <summary>
		/// This should be called when the relic icon changes so the icon paths are uncached.
		/// </summary>
		// Token: 0x06004E9F RID: 20127 RVA: 0x00230FA9 File Offset: 0x0022F1A9
		protected void RelicIconChanged()
		{
			this._resolvedBigIconPath = null;
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06004EA0 RID: 20128 RVA: 0x00230FB2 File Offset: 0x0022F1B2
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x00230FB5 File Offset: 0x0022F1B5
		protected static LocString L10NLookup(string entryName)
		{
			return new LocString("relics", entryName);
		}

		// Token: 0x04001D8C RID: 7564
		private static readonly StringName _isUsed = new StringName("is_used");

		// Token: 0x04001D8D RID: 7565
		private static readonly StringName _pulse = new StringName("pulse");

		// Token: 0x04001D8E RID: 7566
		private static readonly StringName _isWaxStr = new StringName("is_wax");

		// Token: 0x04001D8F RID: 7567
		protected const string _locTable = "relics";

		// Token: 0x04001D90 RID: 7568
		[Nullable(2)]
		private string _resolvedBigIconPath;

		// Token: 0x04001D91 RID: 7569
		[Nullable(2)]
		private Player _owner;

		// Token: 0x04001D92 RID: 7570
		private bool _isWax;

		// Token: 0x04001D93 RID: 7571
		private bool _isMelted;

		// Token: 0x04001D95 RID: 7573
		[Nullable(2)]
		private DynamicVarSet _dynamicVars;

		// Token: 0x04001D96 RID: 7574
		private int _floorAddedToDeck;

		// Token: 0x04001D97 RID: 7575
		private RelicStatus _status;

		// Token: 0x04001D9B RID: 7579
		[Nullable(2)]
		private RelicModel _canonicalInstance;
	}
}
