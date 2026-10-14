using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009B4 RID: 2484
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Expose : CardModel
	{
		// Token: 0x0600715F RID: 29023 RVA: 0x0028B17A File Offset: 0x0028937A
		public Expose()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DDD RID: 7645
		// (get) Token: 0x06007160 RID: 29024 RVA: 0x0028B187 File Offset: 0x00289387
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 2m));
			}
		}

		// Token: 0x17001DDE RID: 7646
		// (get) Token: 0x06007161 RID: 29025 RVA: 0x0028B19E File Offset: 0x0028939E
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001DDF RID: 7647
		// (get) Token: 0x06007162 RID: 29026 RVA: 0x0028B1A8 File Offset: 0x002893A8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<VulnerablePower>(null),
					HoverTipFactory.FromPower<ArtifactPower>(null),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x06007163 RID: 29027 RVA: 0x0028B1F0 File Offset: 0x002893F0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_flying_slash");
			int amount = base.DynamicVars["Power"].IntValue;
			await CreatureCmd.LoseBlock(choiceContext, cardPlay.Target, cardPlay.Target.Block, base.Owner.Creature);
			if (cardPlay.Target.HasPower<ArtifactPower>())
			{
				await PowerCmd.Remove<ArtifactPower>(cardPlay.Target);
			}
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, amount, base.Owner.Creature, this, false);
		}

		// Token: 0x06007164 RID: 29028 RVA: 0x0028B243 File Offset: 0x00289443
		protected override void OnUpgrade()
		{
			base.DynamicVars["Power"].UpgradeValueBy(1m);
		}

		// Token: 0x04002869 RID: 10345
		private const string _powerKey = "Power";
	}
}
