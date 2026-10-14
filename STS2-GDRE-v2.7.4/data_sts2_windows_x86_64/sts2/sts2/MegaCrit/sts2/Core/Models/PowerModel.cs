using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004EB RID: 1259
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PowerModel : AbstractModel
	{
		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06004DFE RID: 19966 RVA: 0x0022F84F File Offset: 0x0022DA4F
		public virtual LocString Title
		{
			get
			{
				return new LocString("powers", base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06004DFF RID: 19967 RVA: 0x0022F870 File Offset: 0x0022DA70
		public virtual LocString Description
		{
			get
			{
				return new LocString("powers", base.Id.Entry + ".description");
			}
		}

		/// <summary>
		/// NOTE: Unlike other models' DynamicDescription, this doesn't include any variables. Someone should probably do that
		/// but it's a bit larger of a refactor than I want to do right now
		/// </summary>
		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06004E00 RID: 19968 RVA: 0x0022F891 File Offset: 0x0022DA91
		public LocString SmartDescription
		{
			get
			{
				if (!this.HasSmartDescription)
				{
					return this.Description;
				}
				return new LocString("powers", this.SmartDescriptionLocKey);
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06004E01 RID: 19969 RVA: 0x0022F8B2 File Offset: 0x0022DAB2
		public bool HasSmartDescription
		{
			get
			{
				return LocString.Exists("powers", this.SmartDescriptionLocKey);
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06004E02 RID: 19970 RVA: 0x0022F8C4 File Offset: 0x0022DAC4
		public LocString RemoteDescription
		{
			get
			{
				if (!this.HasRemoteDescription)
				{
					return this.Description;
				}
				return new LocString("powers", this.RemoteDescriptionLocKey);
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06004E03 RID: 19971 RVA: 0x0022F8E5 File Offset: 0x0022DAE5
		public bool HasRemoteDescription
		{
			get
			{
				return LocString.Exists("powers", this.RemoteDescriptionLocKey);
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06004E04 RID: 19972 RVA: 0x0022F8F7 File Offset: 0x0022DAF7
		protected virtual string RemoteDescriptionLocKey
		{
			get
			{
				return base.Id.Entry + ".remoteDescription";
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06004E05 RID: 19973 RVA: 0x0022F90E File Offset: 0x0022DB0E
		protected virtual string SmartDescriptionLocKey
		{
			get
			{
				return base.Id.Entry + ".smartDescription";
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06004E06 RID: 19974 RVA: 0x0022F928 File Offset: 0x0022DB28
		protected LocString SelectionScreenPrompt
		{
			get
			{
				LocString locString = new LocString("powers", base.Id.Entry + ".selectionScreenPrompt");
				if (!locString.Exists())
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
					defaultInterpolatedStringHandler.AppendLiteral("No selection screen prompt for ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(base.Id);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.DynamicVars.AddTo(locString);
				locString.Add("Amount", this.Amount);
				return locString;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06004E07 RID: 19975 RVA: 0x0022F9BC File Offset: 0x0022DBBC
		public string PackedIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/power_atlas.sprites/" + base.Id.Entry.ToLowerInvariant() + ".tres");
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06004E08 RID: 19976 RVA: 0x0022F9E2 File Offset: 0x0022DBE2
		private string BigIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("powers/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06004E09 RID: 19977 RVA: 0x0022FA08 File Offset: 0x0022DC08
		private string BigBetaIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("powers/beta/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06004E0A RID: 19978 RVA: 0x0022FA2E File Offset: 0x0022DC2E
		private static string MissingIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("powers/missing_power.png");
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06004E0B RID: 19979 RVA: 0x0022FA3A File Offset: 0x0022DC3A
		public string IconPath
		{
			get
			{
				return this.PackedIconPath;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06004E0C RID: 19980 RVA: 0x0022FA42 File Offset: 0x0022DC42
		public Texture2D Icon
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.PackedIconPath, null, 1L);
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06004E0D RID: 19981 RVA: 0x0022FA52 File Offset: 0x0022DC52
		public Texture2D BigIcon
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.ResolvedBigIconPath);
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06004E0E RID: 19982 RVA: 0x0022FA64 File Offset: 0x0022DC64
		public string ResolvedBigIconPath
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
					this._resolvedBigIconPath = PowerModel.MissingIconPath;
				}
				return this._resolvedBigIconPath;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06004E0F RID: 19983
		public abstract PowerType Type { get; }

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06004E10 RID: 19984 RVA: 0x0022FAD1 File Offset: 0x0022DCD1
		public virtual PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.None;
			}
		}

		/// <summary>
		/// Should this power be visible?
		/// Don't make this virtual, use <see cref="P:MegaCrit.Sts2.Core.Models.PowerModel.IsVisibleInternal" /> instead if you need to override.
		/// </summary>
		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06004E11 RID: 19985 RVA: 0x0022FAD4 File Offset: 0x0022DCD4
		public bool IsVisible
		{
			get
			{
				return (this.Target == null || LocalContext.IsMe(this.Target) || this.Target.IsEnemy) && this.IsVisibleInternal;
			}
		}

		/// <summary>
		/// Should this power be visible?
		/// Usually true, but overridden to false for powers that want to stay hidden and perform extra logic behind the
		/// scenes.
		/// </summary>
		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06004E12 RID: 19986 RVA: 0x0022FB00 File Offset: 0x0022DD00
		protected virtual bool IsVisibleInternal
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Should this power play VFX when applied/removed/flashed?
		/// Usually true for visible powers, but overridden to false for powers that don't want to be so "loud", like Osty's
		/// Die For You power.
		/// </summary>
		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x0022FB04 File Offset: 0x0022DD04
		public virtual bool ShouldPlayVfx
		{
			get
			{
				Creature owner = this.Owner;
				return owner != null && owner.IsAlive && CombatManager.Instance.IsInProgress && this.IsVisible;
			}
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x0022FB37 File Offset: 0x0022DD37
		public void StartPulsing()
		{
			Action pulsingStarted = this.PulsingStarted;
			if (pulsingStarted == null)
			{
				return;
			}
			pulsingStarted();
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x0022FB49 File Offset: 0x0022DD49
		public void StopPulsing()
		{
			Action pulsingStopped = this.PulsingStopped;
			if (pulsingStopped == null)
			{
				return;
			}
			pulsingStopped();
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06004E16 RID: 19990 RVA: 0x0022FB5C File Offset: 0x0022DD5C
		// (remove) Token: 0x06004E17 RID: 19991 RVA: 0x0022FB94 File Offset: 0x0022DD94
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action PulsingStarted;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06004E18 RID: 19992 RVA: 0x0022FBCC File Offset: 0x0022DDCC
		// (remove) Token: 0x06004E19 RID: 19993 RVA: 0x0022FC04 File Offset: 0x0022DE04
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action PulsingStopped;

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06004E1A RID: 19994 RVA: 0x0022FC39 File Offset: 0x0022DE39
		// (set) Token: 0x06004E1B RID: 19995 RVA: 0x0022FC41 File Offset: 0x0022DE41
		public int Amount
		{
			get
			{
				return this._amount;
			}
			private set
			{
				this.SetAmount(value, false);
			}
		}

		/// <summary>
		/// The amount this power had at the beginning of the current turn.
		/// Updated at the very start of each turn before any hooks run.
		/// Useful for powers that should only trigger if they were present at turn start,
		/// preventing same-turn activation when applied mid-turn (e.g., via auto-play effects).
		/// </summary>
		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06004E1C RID: 19996 RVA: 0x0022FC4B File Offset: 0x0022DE4B
		// (set) Token: 0x06004E1D RID: 19997 RVA: 0x0022FC53 File Offset: 0x0022DE53
		public int AmountOnTurnStart
		{
			get
			{
				return this._amountOnTurnStart;
			}
			set
			{
				base.AssertMutable();
				this._amountOnTurnStart = value;
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06004E1E RID: 19998 RVA: 0x0022FC62 File Offset: 0x0022DE62
		public virtual int DisplayAmount
		{
			get
			{
				return this.Amount;
			}
		}

		/// <summary>
		/// The color to use for this power's Amount label.
		/// Usually red for debuffs and cream for all other powers, but can be overridden for special cases.
		/// </summary>
		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06004E1F RID: 19999 RVA: 0x0022FC6A File Offset: 0x0022DE6A
		public virtual Color AmountLabelColor
		{
			get
			{
				if (this.GetTypeForAmount(this.Amount) != PowerType.Debuff)
				{
					return PowerModel._normalAmountLabelColor;
				}
				return PowerModel._debuffAmountLabelColor;
			}
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x0022FC8B File Offset: 0x0022DE8B
		protected void Flash()
		{
			Action<PowerModel> flashed = this.Flashed;
			if (flashed == null)
			{
				return;
			}
			flashed(this);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x0022FC9E File Offset: 0x0022DE9E
		protected void InvokeDisplayAmountChanged()
		{
			Action displayAmountChanged = this.DisplayAmountChanged;
			if (displayAmountChanged == null)
			{
				return;
			}
			displayAmountChanged();
		}

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06004E22 RID: 20002 RVA: 0x0022FCB0 File Offset: 0x0022DEB0
		// (remove) Token: 0x06004E23 RID: 20003 RVA: 0x0022FCE8 File Offset: 0x0022DEE8
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PowerModel> Flashed;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06004E24 RID: 20004 RVA: 0x0022FD20 File Offset: 0x0022DF20
		// (remove) Token: 0x06004E25 RID: 20005 RVA: 0x0022FD58 File Offset: 0x0022DF58
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action DisplayAmountChanged;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06004E26 RID: 20006 RVA: 0x0022FD90 File Offset: 0x0022DF90
		// (remove) Token: 0x06004E27 RID: 20007 RVA: 0x0022FDC8 File Offset: 0x0022DFC8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Removed;

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06004E28 RID: 20008
		public abstract PowerStackType StackType { get; }

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06004E29 RID: 20009 RVA: 0x0022FDFD File Offset: 0x0022DFFD
		public virtual bool AllowNegative
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06004E2A RID: 20010 RVA: 0x0022FE00 File Offset: 0x0022E000
		public PowerType TypeForCurrentAmount
		{
			get
			{
				return this.GetTypeForAmount(this.Amount);
			}
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0022FE14 File Offset: 0x0022E014
		public PowerType GetTypeForAmount(decimal customAmount)
		{
			if (this.StackType.Equals(PowerStackType.Counter) && this.AllowNegative && customAmount < 0m)
			{
				return PowerType.Debuff;
			}
			if (!this.AllowNegative && this.Type.Equals(PowerType.Debuff) && customAmount < 0m)
			{
				return PowerType.Buff;
			}
			return this.Type;
		}

		/// <summary>
		/// Should this power be removed from its owner due to an amount change?
		/// Most powers are removed when they go below 0.
		/// Powers that allow negative amounts (Strength, DexterityPower, etc.) should be removed when they reach exactly 0.
		/// </summary>
		// Token: 0x06004E2C RID: 20012 RVA: 0x0022FE8D File Offset: 0x0022E08D
		public bool ShouldRemoveDueToAmount()
		{
			return (!this.AllowNegative && this.Amount <= 0) || (this.AllowNegative && this.Amount == 0);
		}

		/// <summary>
		/// This enables the behavior of duration-type powers (Vulnerable, Weak, etc.) ticking down at the end of the
		/// monster side turn, but skipping the first tick if a monster applied the power to the player.
		/// </summary>
		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06004E2D RID: 20013 RVA: 0x0022FEB5 File Offset: 0x0022E0B5
		// (set) Token: 0x06004E2E RID: 20014 RVA: 0x0022FEBD File Offset: 0x0022E0BD
		public bool SkipNextDurationTick
		{
			get
			{
				return this._skipNextDurationTick;
			}
			set
			{
				base.AssertMutable();
				this._skipNextDurationTick = value;
			}
		}

		/// <summary>
		/// Get the Creature that this power is on.
		/// Will technically be null on a canonical power model, but we should never be checking that, so we leave this as
		/// non-nullable for convenience.
		/// </summary>
		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06004E2F RID: 20015 RVA: 0x0022FECC File Offset: 0x0022E0CC
		// (set) Token: 0x06004E30 RID: 20016 RVA: 0x0022FEDA File Offset: 0x0022E0DA
		public Creature Owner
		{
			get
			{
				base.AssertMutable();
				return this._owner;
			}
			private set
			{
				base.AssertMutable();
				if (this._owner != null && this._owner != value)
				{
					throw new InvalidOperationException("Cannot move power " + base.Id.Entry + " from one owner to another");
				}
				this._owner = value;
			}
		}

		/// <summary>
		/// The CombatState that this power's owner exists in.
		/// Will technically be null on a canonical power model, but we should never be checking that, so we leave this as
		/// non-nullable for convenience.
		/// </summary>
		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06004E31 RID: 20017 RVA: 0x0022FF1A File Offset: 0x0022E11A
		public ICombatState CombatState
		{
			get
			{
				return this.Owner.CombatState;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06004E32 RID: 20018 RVA: 0x0022FF27 File Offset: 0x0022E127
		// (set) Token: 0x06004E33 RID: 20019 RVA: 0x0022FF2F File Offset: 0x0022E12F
		[Nullable(2)]
		public Creature Applier
		{
			[NullableContext(2)]
			get
			{
				return this._applier;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._applier = value;
			}
		}

		/// <summary>
		/// Get the Creature that this power targets.
		/// For most powers, this is null. It is used on instanced powers that each target a player in multiplayer.
		/// </summary>
		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x0022FF3E File Offset: 0x0022E13E
		// (set) Token: 0x06004E35 RID: 20021 RVA: 0x0022FF46 File Offset: 0x0022E146
		[Nullable(2)]
		public Creature Target
		{
			[NullableContext(2)]
			get
			{
				return this._target;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._target = value;
			}
		}

		/// <summary>
		/// "Public" data about this power.
		/// These data are visible everywhere else in the game. They can be displayed in localization entries, read/written
		/// by other callers, and if you clone this power, all these vars will be cloned along with it.
		/// If you have "private" data that should be invisible to the rest of the game, use <see cref="F:MegaCrit.Sts2.Core.Models.PowerModel._internalData" />.
		/// </summary>
		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06004E36 RID: 20022 RVA: 0x0022FF55 File Offset: 0x0022E155
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

		/// <summary>
		/// If set to true, this power's amount will automatically be multiplied by the player count when applied.
		/// </summary>
		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06004E37 RID: 20023 RVA: 0x0022FF89 File Offset: 0x0022E189
		public virtual bool ShouldScaleInMultiplayer
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// If ShouldScaleInMultiplayer is true, this will be called before the power is applied to any enemies.
		/// Unless overridden, it applies a default scaling based on the number of players and
		/// <see cref="M:MegaCrit.Sts2.Core.Models.Singleton.MultiplayerScalingModel.GetMultiplayerScaling(MegaCrit.Sts2.Core.Models.EncounterModel,System.Int32)" />.
		/// Be sure to use the passed combat state rather than CombatState, as it will not be set in time.
		/// </summary>
		/// <param name="combatState">The combat state which will own the power.</param>
		/// <param name="applier">The creature applying the power.</param>
		/// <param name="amount">The amount of the power that is being applied.</param>
		/// <param name="target">The target receiving the power.</param>
		/// <param name="cardSource">The card applying the power, if any.</param>
		/// <returns>The modified amount to apply.</returns>
		// Token: 0x06004E38 RID: 20024 RVA: 0x0022FF8C File Offset: 0x0022E18C
		public virtual decimal GetScaledAmountForMultiplayer(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource)
		{
			return amount * combatState.Players.Count * MultiplayerScalingModel.GetMultiplayerScaling(combatState.Encounter, combatState.RunState.CurrentActIndex);
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06004E39 RID: 20025 RVA: 0x0022FFBF File Offset: 0x0022E1BF
		protected virtual IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		/// <summary>
		/// Initialize any internal data used by this power.
		/// </summary>
		/// <returns></returns>
		// Token: 0x06004E3A RID: 20026 RVA: 0x0022FFC6 File Offset: 0x0022E1C6
		[NullableContext(2)]
		protected virtual object InitInternalData()
		{
			return null;
		}

		/// <summary>
		/// "Private" data that lives behind-the-scenes to make this power work as expected.
		/// These data are isolated to this instance of the power. They cannot be displayed in localization entries, and
		/// they will be reset on clones of this power.
		/// If you want to set data that is meant to be cloned and displayed in localization, use <see cref="P:MegaCrit.Sts2.Core.Models.PowerModel.DynamicVars" />.
		/// </summary>
		// Token: 0x06004E3B RID: 20027 RVA: 0x0022FFC9 File Offset: 0x0022E1C9
		protected T GetInternalData<[Nullable(2)] T>()
		{
			return (T)((object)this._internalData);
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06004E3C RID: 20028 RVA: 0x0022FFD8 File Offset: 0x0022E1D8
		public HoverTip DumbHoverTip
		{
			get
			{
				return this.GetDumbHoverTip(null);
			}
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x0022FFF4 File Offset: 0x0022E1F4
		public HoverTip GetDumbHoverTip(int? amountOverride = null)
		{
			LocString description = this.Description;
			this.AddDumbVariablesToDescription(description, amountOverride);
			return new HoverTip(this, description.GetFormattedText(), false);
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x0023001D File Offset: 0x0022E21D
		protected virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06004E3F RID: 20031 RVA: 0x00230024 File Offset: 0x0022E224
		public IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				if (!this.IsVisible)
				{
					return list;
				}
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = this.HasSmartDescription && base.IsMutable;
				if (flag)
				{
					LocString locString = this.SmartDescription;
					if (this.Applier != null && !LocalContext.IsMe(this.Applier) && this.HasRemoteDescription)
					{
						locString = this.RemoteDescription;
					}
					locString.Add("Amount", this.Amount);
					locString.Add("OnPlayer", this.Owner.IsPlayer);
					locString.Add("IsMultiplayer", this.Owner.CombatState.Players.Count > 1);
					locString.Add("PlayerCount", this.Owner.CombatState.Players.Count);
					locString.Add("OwnerName", this.Owner.IsPlayer ? this.Owner.Player.Character.Title : this.Owner.Monster.Title);
					if (this.Applier != null)
					{
						string text;
						if (this.Applier.Monster != null)
						{
							text = this.Applier.Monster.Title.GetFormattedText();
						}
						else if (this.Applier.Player != null)
						{
							text = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this.Applier.Player.NetId);
						}
						else
						{
							text = "";
						}
						locString.Add("ApplierName", text);
					}
					if (this.Target != null)
					{
						if (this.Target.IsMonster)
						{
							locString.Add("TargetName", this.Target.Monster.Title);
						}
						else
						{
							locString.Add("TargetName", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this.Target.Player.NetId));
						}
					}
					this.AddDumbVariablesToDescription(locString, null);
					this.DynamicVars.AddTo(locString);
					stringBuilder.Append(locString.GetFormattedText());
				}
				else
				{
					LocString description = this.Description;
					this.AddDumbVariablesToDescription(description, null);
					stringBuilder.Append(description.GetFormattedText());
				}
				list.Add(new HoverTip(this, stringBuilder.ToString(), flag));
				list.AddRange(this.ExtraHoverTips);
				return list;
			}
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x00230290 File Offset: 0x0022E490
		private void AddDumbVariablesToDescription(LocString description, int? amountOverride = null)
		{
			description.Add("Amount", amountOverride ?? this.Amount);
			description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
			description.Add("energyPrefix", EnergyIconHelper.GetPrefix(this));
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06004E41 RID: 20033 RVA: 0x002302E8 File Offset: 0x0022E4E8
		// (set) Token: 0x06004E42 RID: 20034 RVA: 0x002302FA File Offset: 0x0022E4FA
		private PowerModel CanonicalInstance
		{
			get
			{
				if (!base.IsMutable)
				{
					return this;
				}
				return this._canonicalInstance;
			}
			set
			{
				base.AssertMutable();
				this._canonicalInstance = value;
			}
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x0023030C File Offset: 0x0022E50C
		public void SetAmount(int amount, bool silent = false)
		{
			base.AssertMutable();
			amount = Math.Clamp(amount, -999999999, 999999999);
			int num = amount - this._amount;
			if (num == 0)
			{
				return;
			}
			this._amount = amount;
			Action displayAmountChanged = this.DisplayAmountChanged;
			if (displayAmountChanged != null)
			{
				displayAmountChanged();
			}
			this.Owner.InvokePowerModified(this, num, silent);
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x00230364 File Offset: 0x0022E564
		public PowerModel ToMutable(int initialAmount = 0)
		{
			base.AssertCanonical();
			PowerModel powerModel = (PowerModel)base.MutableClone();
			powerModel.CanonicalInstance = this;
			powerModel.Amount = initialAmount;
			return powerModel;
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x00230392 File Offset: 0x0022E592
		public void ApplyInternal(Creature owner, decimal amount, bool silent = false)
		{
			if (amount == 0m)
			{
				return;
			}
			base.AssertMutable();
			this.Owner = owner;
			this.SetAmount((int)amount, silent);
			this.Owner.ApplyPowerInternal(this);
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x002303C8 File Offset: 0x0022E5C8
		public void RemoveInternal()
		{
			base.AssertMutable();
			Action removed = this.Removed;
			if (removed != null)
			{
				removed();
			}
			this.Owner.RemovePowerInternal(this);
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x002303ED File Offset: 0x0022E5ED
		protected override void DeepCloneFields()
		{
			base.DeepCloneFields();
			this._dynamicVars = this.DynamicVars.Clone(this);
			this._internalData = this.InitInternalData();
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x00230413 File Offset: 0x0022E613
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this.Flashed = null;
			this.DisplayAmountChanged = null;
			this.Removed = null;
			this.PulsingStarted = null;
			this.PulsingStopped = null;
			this._owner = null;
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x00230445 File Offset: 0x0022E645
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Hook that runs just before this power is first applied to its owner.
		/// Does not run if the owner already has this power, and its amount was just changed.
		/// </summary>
		/// <param name="target">The creature to which this power is about to be applied.</param>
		/// <param name="amount">The amount of the power that is about to be applied.</param>
		/// <param name="applier">The creature that applied the power, if any.</param>
		/// <param name="cardSource">The card that applied the power, if any.</param>
		// Token: 0x06004E4A RID: 20042 RVA: 0x00230448 File Offset: 0x0022E648
		public virtual Task BeforeApplied(Creature target, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Hook that runs after this power is first applied to its owner.
		/// Does not run if the owner already has this power, and its amount was just changed.
		/// </summary>
		/// <param name="applier"></param>
		/// <param name="cardSource"></param>
		// Token: 0x06004E4B RID: 20043 RVA: 0x0023044F File Offset: 0x0022E64F
		[NullableContext(2)]
		[return: Nullable(1)]
		public virtual Task AfterApplied(Creature applier, CardModel cardSource)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Hook that runs after this power is removed from its owner.
		/// <param name="oldOwner">The owner of this power before it was removed.</param>
		/// </summary>
		// Token: 0x06004E4C RID: 20044 RVA: 0x00230456 File Offset: 0x0022E656
		public virtual Task AfterRemoved(Creature oldOwner)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Should this power be removed after its owner dies?
		/// Usually true, but false for powers that do things like revive their owner.
		/// </summary>
		// Token: 0x06004E4D RID: 20045 RVA: 0x0023045D File Offset: 0x0022E65D
		public virtual bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return true;
		}

		/// <summary>
		/// Should this power's owner's death trigger effects with the Fatal keyword like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Feed" />?
		/// Usually true, but false for <see cref="T:MegaCrit.Sts2.Core.Models.Powers.MinionPower" /> and a few other powers.
		/// </summary>
		// Token: 0x06004E4E RID: 20046 RVA: 0x00230460 File Offset: 0x0022E660
		public virtual bool ShouldOwnerDeathTriggerFatal()
		{
			return true;
		}

		/// <summary>
		/// Hook that allows this power to classify the enemy as primary or secondary.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Creatures.Creature.IsPrimaryEnemy" /> for detailed definitions.
		/// </summary>
		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06004E4F RID: 20047 RVA: 0x00230463 File Offset: 0x0022E663
		public virtual bool OwnerIsSecondaryEnemy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001D7A RID: 7546
		public const string locTable = "powers";

		// Token: 0x04001D7B RID: 7547
		protected static readonly Color _normalAmountLabelColor = StsColors.cream;

		// Token: 0x04001D7C RID: 7548
		protected static readonly Color _debuffAmountLabelColor = StsColors.red;

		// Token: 0x04001D7D RID: 7549
		[Nullable(2)]
		private string _resolvedBigIconPath;

		// Token: 0x04001D80 RID: 7552
		private int _amount;

		// Token: 0x04001D81 RID: 7553
		private int _amountOnTurnStart;

		// Token: 0x04001D85 RID: 7557
		private bool _skipNextDurationTick;

		// Token: 0x04001D86 RID: 7558
		[Nullable(2)]
		private Creature _owner;

		// Token: 0x04001D87 RID: 7559
		[Nullable(2)]
		private Creature _applier;

		// Token: 0x04001D88 RID: 7560
		[Nullable(2)]
		private Creature _target;

		// Token: 0x04001D89 RID: 7561
		[Nullable(2)]
		private DynamicVarSet _dynamicVars;

		// Token: 0x04001D8A RID: 7562
		[Nullable(2)]
		private object _internalData;

		// Token: 0x04001D8B RID: 7563
		private PowerModel _canonicalInstance;
	}
}
