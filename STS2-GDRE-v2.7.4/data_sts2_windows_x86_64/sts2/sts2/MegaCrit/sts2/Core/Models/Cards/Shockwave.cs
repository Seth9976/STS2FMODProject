using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AC5 RID: 2757
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shockwave : CardModel
	{
		// Token: 0x06007729 RID: 30505 RVA: 0x0029720B File Offset: 0x0029540B
		public Shockwave()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17002049 RID: 8265
		// (get) Token: 0x0600772A RID: 30506 RVA: 0x00297218 File Offset: 0x00295418
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 3m));
			}
		}

		// Token: 0x1700204A RID: 8266
		// (get) Token: 0x0600772B RID: 30507 RVA: 0x0029722F File Offset: 0x0029542F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x1700204B RID: 8267
		// (get) Token: 0x0600772C RID: 30508 RVA: 0x00297238 File Offset: 0x00295438
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x0600772D RID: 30509 RVA: 0x00297274 File Offset: 0x00295474
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_flying_slash");
			int amount = base.DynamicVars["Power"].IntValue;
			foreach (Creature enemy in base.CombatState.HittableEnemies)
			{
				await PowerCmd.Apply<WeakPower>(choiceContext, enemy, amount, base.Owner.Creature, this, false);
				await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, amount, base.Owner.Creature, this, false);
				enemy = null;
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x0600772E RID: 30510 RVA: 0x002972BF File Offset: 0x002954BF
		protected override void OnUpgrade()
		{
			base.DynamicVars["Power"].UpgradeValueBy(2m);
		}

		// Token: 0x040028BE RID: 10430
		private const string _powerKey = "Power";
	}
}
