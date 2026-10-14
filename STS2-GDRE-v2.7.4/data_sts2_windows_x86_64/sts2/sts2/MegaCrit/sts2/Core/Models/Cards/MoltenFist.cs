using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A48 RID: 2632
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MoltenFist : CardModel
	{
		// Token: 0x06007492 RID: 29842 RVA: 0x00291CAA File Offset: 0x0028FEAA
		public MoltenFist()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F34 RID: 7988
		// (get) Token: 0x06007493 RID: 29843 RVA: 0x00291CB7 File Offset: 0x0028FEB7
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("vfx/vfx_molten_fist"));
			}
		}

		// Token: 0x17001F35 RID: 7989
		// (get) Token: 0x06007494 RID: 29844 RVA: 0x00291CC8 File Offset: 0x0028FEC8
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F36 RID: 7990
		// (get) Token: 0x06007495 RID: 29845 RVA: 0x00291CD0 File Offset: 0x0028FED0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x17001F37 RID: 7991
		// (get) Token: 0x06007496 RID: 29846 RVA: 0x00291CE4 File Offset: 0x0028FEE4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x00291D04 File Offset: 0x0028FF04
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_molten_fist", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			int num = (cardPlay.Target.IsAlive ? cardPlay.Target.GetPowerAmount<VulnerablePower>() : 0);
			if (num > 0)
			{
				await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, num, base.Owner.Creature, this, false);
			}
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x00291D57 File Offset: 0x0028FF57
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}

		// Token: 0x040028A3 RID: 10403
		private const string _moltenFistVfxPath = "vfx/vfx_molten_fist";
	}
}
