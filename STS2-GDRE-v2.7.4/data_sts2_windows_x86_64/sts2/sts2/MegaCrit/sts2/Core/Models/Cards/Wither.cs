using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B3E RID: 2878
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Wither : CardModel
	{
		// Token: 0x060079B4 RID: 31156 RVA: 0x0029C45C File Offset: 0x0029A65C
		public Wither()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x1700214D RID: 8525
		// (get) Token: 0x060079B5 RID: 31157 RVA: 0x0029C469 File Offset: 0x0029A669
		public string[] AllPortraitPaths
		{
			[PreserveBaseOverrides]
			get
			{
				return new string[]
				{
					this.GetPortraitPath(0),
					this.GetPortraitPath(1),
					this.GetPortraitPath(2)
				};
			}
		}

		// Token: 0x1700214E RID: 8526
		// (get) Token: 0x060079B6 RID: 31158 RVA: 0x0029C48F File Offset: 0x0029A68F
		public override string PortraitPath
		{
			get
			{
				return this.GetPortraitPath(this.FakeUpgradeLevel);
			}
		}

		// Token: 0x1700214F RID: 8527
		// (get) Token: 0x060079B7 RID: 31159 RVA: 0x0029C49D File Offset: 0x0029A69D
		protected override string PortraitPngPath
		{
			get
			{
				return this.GetPortraitPngPath(this.FakeUpgradeLevel);
			}
		}

		// Token: 0x060079B8 RID: 31160 RVA: 0x0029C4AC File Offset: 0x0029A6AC
		private string GetPortraitPath(int witherLevel)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
			defaultInterpolatedStringHandler.AppendLiteral("atlases/card_atlas.sprites/");
			defaultInterpolatedStringHandler.AppendFormatted(this.Pool.Title.ToLowerInvariant());
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted(this.GetPortraitFilename(witherLevel));
			defaultInterpolatedStringHandler.AppendLiteral(".tres");
			return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060079B9 RID: 31161 RVA: 0x0029C518 File Offset: 0x0029A718
		private string GetPortraitPngPath(int witherLevel)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
			defaultInterpolatedStringHandler.AppendLiteral("packed/card_portraits/");
			defaultInterpolatedStringHandler.AppendFormatted(this.Pool.Title.ToLowerInvariant());
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted(this.GetPortraitFilename(witherLevel));
			defaultInterpolatedStringHandler.AppendLiteral(".png");
			return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060079BA RID: 31162 RVA: 0x0029C584 File Offset: 0x0029A784
		private string GetPortraitFilename(int witherLevel)
		{
			if (witherLevel >= 2)
			{
				return "wither3";
			}
			if (witherLevel < 1)
			{
				return "wither1";
			}
			return "wither2";
		}

		// Token: 0x17002150 RID: 8528
		// (get) Token: 0x060079BB RID: 31163 RVA: 0x0029C5A4 File Offset: 0x0029A7A4
		public override string Title
		{
			get
			{
				string title = base.Title;
				if (this.FakeUpgradeLevel > 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(title);
					defaultInterpolatedStringHandler.AppendLiteral("+");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.FakeUpgradeLevel);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return title;
			}
		}

		// Token: 0x17002151 RID: 8529
		// (get) Token: 0x060079BC RID: 31164 RVA: 0x0029C5F4 File Offset: 0x0029A7F4
		// (set) Token: 0x060079BD RID: 31165 RVA: 0x0029C5FC File Offset: 0x0029A7FC
		private int FakeUpgradeLevel
		{
			get
			{
				return this._fakeUpgradeLevel;
			}
			set
			{
				base.AssertMutable();
				this._fakeUpgradeLevel = value;
			}
		}

		// Token: 0x17002152 RID: 8530
		// (get) Token: 0x060079BE RID: 31166 RVA: 0x0029C60B File Offset: 0x0029A80B
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002153 RID: 8531
		// (get) Token: 0x060079BF RID: 31167 RVA: 0x0029C60E File Offset: 0x0029A80E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Unpowered | ValueProp.Move));
			}
		}

		// Token: 0x17002154 RID: 8532
		// (get) Token: 0x060079C0 RID: 31168 RVA: 0x0029C622 File Offset: 0x0029A822
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17002155 RID: 8533
		// (get) Token: 0x060079C1 RID: 31169 RVA: 0x0029C62A File Offset: 0x0029A82A
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002156 RID: 8534
		// (get) Token: 0x060079C2 RID: 31170 RVA: 0x0029C62D File Offset: 0x0029A82D
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NGroundFireVfx.AssetPaths;
			}
		}

		// Token: 0x060079C3 RID: 31171 RVA: 0x0029C634 File Offset: 0x0029A834
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.Damage, this, null);
		}

		// Token: 0x060079C4 RID: 31172 RVA: 0x0029C680 File Offset: 0x0029A880
		public void FakeUpgrade()
		{
			int fakeUpgradeLevel = this.FakeUpgradeLevel;
			this.FakeUpgradeLevel = fakeUpgradeLevel + 1;
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}

		// Token: 0x040028DB RID: 10459
		private int _fakeUpgradeLevel;
	}
}
