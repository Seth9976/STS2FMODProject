using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E8 RID: 1256
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class OrbModel : AbstractModel
	{
		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06004D91 RID: 19857
		public abstract decimal PassiveVal { get; }

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06004D92 RID: 19858
		public abstract decimal EvokeVal { get; }

		// Token: 0x06004D93 RID: 19859 RVA: 0x0022EB89 File Offset: 0x0022CD89
		public static OrbModel GetRandomOrb(Rng rng)
		{
			return ModelDb.GetById<OrbModel>(rng.NextItem<ModelId>(OrbModel._validOrbs));
		}

		/// <summary>
		/// Set to true when this is removed from a player's orb queue.
		/// </summary>
		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06004D94 RID: 19860 RVA: 0x0022EB9B File Offset: 0x0022CD9B
		// (set) Token: 0x06004D95 RID: 19861 RVA: 0x0022EBA3 File Offset: 0x0022CDA3
		public bool HasBeenRemovedFromState { get; private set; }

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06004D96 RID: 19862 RVA: 0x0022EBAC File Offset: 0x0022CDAC
		public LocString Title
		{
			get
			{
				return new LocString("orbs", base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06004D97 RID: 19863 RVA: 0x0022EBCD File Offset: 0x0022CDCD
		public LocString Description
		{
			get
			{
				return new LocString("orbs", base.Id.Entry + ".description");
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x0022EBEE File Offset: 0x0022CDEE
		public bool HasSmartDescription
		{
			get
			{
				return LocString.Exists("orbs", this.SmartDescriptionLocKey);
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06004D99 RID: 19865 RVA: 0x0022EC00 File Offset: 0x0022CE00
		private string SmartDescriptionLocKey
		{
			get
			{
				return base.Id.Entry + ".smartDescription";
			}
		}

		/// <summary>
		/// NOTE: Unlike other models' DynamicDescription, this doesn't include any variables. Someone should probably do that
		/// but it's a bit larger of a refactor than I want to do right now
		/// </summary>
		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06004D9A RID: 19866 RVA: 0x0022EC17 File Offset: 0x0022CE17
		public LocString SmartDescription
		{
			get
			{
				if (!this.HasSmartDescription)
				{
					return this.Description;
				}
				return new LocString("orbs", base.Id.Entry + ".smartDescription");
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x0022EC47 File Offset: 0x0022CE47
		private string DebugPassiveSfx
		{
			get
			{
				return base.Id.Entry.ToLowerInvariant() + "_passive.mp3";
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x0022EC63 File Offset: 0x0022CE63
		private string DebugEvokeSfx
		{
			get
			{
				return base.Id.Entry.ToLowerInvariant() + "_evoke.mp3";
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x0022EC7F File Offset: 0x0022CE7F
		private string DebugChannelSfx
		{
			get
			{
				return base.Id.Entry.ToLowerInvariant() + "_channel.mp3";
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x0022EC9B File Offset: 0x0022CE9B
		protected virtual string PassiveSfx
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x0022ECA2 File Offset: 0x0022CEA2
		protected virtual string EvokeSfx
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x0022ECA9 File Offset: 0x0022CEA9
		protected virtual string ChannelSfx
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x0022ECB0 File Offset: 0x0022CEB0
		protected void PlayPassiveSfx()
		{
			if (this.PassiveSfx != "")
			{
				SfxCmd.Play(this.PassiveSfx, 1f);
				return;
			}
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Play(this.DebugPassiveSfx, 1f, PitchVariance.None);
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x0022ECFC File Offset: 0x0022CEFC
		protected void PlayEvokeSfx()
		{
			if (this.EvokeSfx != "")
			{
				SfxCmd.Play(this.EvokeSfx, 1f);
				return;
			}
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Play(this.DebugEvokeSfx, 1f, PitchVariance.None);
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x0022ED48 File Offset: 0x0022CF48
		public void PlayChannelSfx()
		{
			if (this.ChannelSfx != "")
			{
				SfxCmd.Play(this.ChannelSfx, 1f);
				return;
			}
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Play(this.DebugChannelSfx, 1f, PitchVariance.None);
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06004DA4 RID: 19876 RVA: 0x0022ED94 File Offset: 0x0022CF94
		public static HoverTip EmptySlotHoverTipHoverTip
		{
			get
			{
				return new HoverTip(new LocString("orbs", "EMPTY_SLOT.title"), new LocString("orbs", "EMPTY_SLOT.description"), null);
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06004DA5 RID: 19877 RVA: 0x0022EDBA File Offset: 0x0022CFBA
		public HoverTip DumbHoverTip
		{
			get
			{
				return new HoverTip(this, this.Description);
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06004DA6 RID: 19878 RVA: 0x0022EDC8 File Offset: 0x0022CFC8
		protected virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x0022EDD0 File Offset: 0x0022CFD0
		public IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				List<IHoverTip> list = this.ExtraHoverTips.ToList<IHoverTip>();
				if (this.HasSmartDescription && base.IsMutable)
				{
					LocString smartDescription = this.SmartDescription;
					smartDescription.Add("energyPrefix", this.Owner.Character.CardPool.Title);
					smartDescription.Add("Passive", this.PassiveVal);
					smartDescription.Add("Evoke", this.EvokeVal);
					list.Add(new HoverTip(this, smartDescription));
				}
				else
				{
					list.Add(this.DumbHoverTip);
				}
				return list;
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06004DA8 RID: 19880 RVA: 0x0022EE68 File Offset: 0x0022D068
		private string IconPath
		{
			get
			{
				return ImageHelper.GetImagePath("orbs/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x0022EE8E File Offset: 0x0022D08E
		private string SpritePath
		{
			get
			{
				return SceneHelper.GetScenePath("orbs/orb_visuals/" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06004DAA RID: 19882 RVA: 0x0022EEAF File Offset: 0x0022D0AF
		public CompressedTexture2D Icon
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.IconPath);
			}
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x0022EEC1 File Offset: 0x0022D0C1
		public Node2D CreateSprite()
		{
			return PreloadManager.Cache.GetScene(this.SpritePath).Instantiate<Node2D>(0L);
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06004DAC RID: 19884
		public abstract Color DarkenedColor { get; }

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x0022EEDA File Offset: 0x0022D0DA
		// (set) Token: 0x06004DAE RID: 19886 RVA: 0x0022EEEC File Offset: 0x0022D0EC
		private OrbModel CanonicalInstance
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

		// Token: 0x06004DAF RID: 19887 RVA: 0x0022EEFC File Offset: 0x0022D0FC
		public OrbModel ToMutable(int initialAmount = 0)
		{
			base.AssertCanonical();
			OrbModel orbModel = (OrbModel)base.MutableClone();
			orbModel.CanonicalInstance = this;
			return orbModel;
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x0022EF23 File Offset: 0x0022D123
		// (set) Token: 0x06004DB1 RID: 19889 RVA: 0x0022EF34 File Offset: 0x0022D134
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
				if (this._owner != null && value != null && value != this._owner)
				{
					throw new InvalidOperationException("Card " + base.Id.Entry + " already has an owner.");
				}
				this._owner = value;
			}
		}

		/// <summary>
		/// Get the CombatState of the orb's owner.
		/// Will never be null, since orbs are combat-only.
		/// </summary>
		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x0022EF82 File Offset: 0x0022D182
		public ICombatState CombatState
		{
			get
			{
				return this.Owner.Creature.CombatState;
			}
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x0022EF94 File Offset: 0x0022D194
		protected void ActivatePassive()
		{
			Action passiveActivated = this.PassiveActivated;
			if (passiveActivated == null)
			{
				return;
			}
			passiveActivated();
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06004DB4 RID: 19892 RVA: 0x0022EFA8 File Offset: 0x0022D1A8
		// (remove) Token: 0x06004DB5 RID: 19893 RVA: 0x0022EFE0 File Offset: 0x0022D1E0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action PassiveActivated;

		// Token: 0x06004DB6 RID: 19894 RVA: 0x0022F015 File Offset: 0x0022D215
		public void ActivateEvoke(Creature[] targets)
		{
			Action<Creature[]> evokeActivated = this.EvokeActivated;
			if (evokeActivated == null)
			{
				return;
			}
			evokeActivated(targets);
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06004DB7 RID: 19895 RVA: 0x0022F028 File Offset: 0x0022D228
		// (remove) Token: 0x06004DB8 RID: 19896 RVA: 0x0022F060 File Offset: 0x0022D260
		[Nullable(new byte[] { 2, 1, 1 })]
		[field: Nullable(new byte[] { 2, 1, 1 })]
		public event Action<Creature[]> EvokeActivated;

		// Token: 0x06004DB9 RID: 19897 RVA: 0x0022F095 File Offset: 0x0022D295
		public virtual Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x0022F09C File Offset: 0x0022D29C
		public virtual Task AfterTurnStartOrbTrigger(PlayerChoiceContext choiceContext)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Calculates the number of times an Orb's passive should trigger and performs it.
		/// </summary>
		// Token: 0x06004DBB RID: 19899 RVA: 0x0022F0A4 File Offset: 0x0022D2A4
		public async Task TriggerPassive(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			List<AbstractModel> list;
			int triggerCount = Hook.ModifyOrbPassiveTriggerCount(this.Owner.Creature.CombatState, this, 1, out list);
			await Hook.AfterModifyingOrbPassiveTriggerCount(this.Owner.Creature.CombatState, this, list);
			for (int i = 0; i < triggerCount; i++)
			{
				await this.Passive(choiceContext, target);
				if (LocalContext.IsMe(this.Owner))
				{
					await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
				}
				else
				{
					await Cmd.Wait(0.05f, false);
				}
			}
		}

		/// <summary>
		/// CAREFUL: Should only be used if in special cases, since this skips the ModifyOrbPassiveTrigger.
		/// </summary>
		// Token: 0x06004DBC RID: 19900 RVA: 0x0022F0F7 File Offset: 0x0022D2F7
		public virtual Task Passive(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x0022F0FE File Offset: 0x0022D2FE
		public virtual Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
		{
			return Task.FromResult<IEnumerable<Creature>>(Array.Empty<Creature>());
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x0022F10A File Offset: 0x0022D30A
		protected decimal ModifyOrbValue(decimal result)
		{
			return Hook.ModifyOrbValue(this.Owner.Creature.CombatState, this, result);
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x0022F123 File Offset: 0x0022D323
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this.EvokeActivated = null;
			this.PassiveActivated = null;
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x0022F139 File Offset: 0x0022D339
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06004DC1 RID: 19905 RVA: 0x0022F13C File Offset: 0x0022D33C
		public IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { this.IconPath, this.SpritePath });
			}
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x0022F15B File Offset: 0x0022D35B
		public void RemoveInternal()
		{
			this.HasBeenRemovedFromState = true;
		}

		// Token: 0x04001D6A RID: 7530
		public const string locTable = "orbs";

		// Token: 0x04001D6B RID: 7531
		private static readonly ModelId[] _validOrbs = new ModelId[]
		{
			ModelDb.GetId<LightningOrb>(),
			ModelDb.GetId<FrostOrb>(),
			ModelDb.GetId<DarkOrb>(),
			ModelDb.GetId<PlasmaOrb>(),
			ModelDb.GetId<GlassOrb>()
		};

		// Token: 0x04001D6D RID: 7533
		private OrbModel _canonicalInstance;

		// Token: 0x04001D6E RID: 7534
		[Nullable(2)]
		private Player _owner;
	}
}
