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
	// Token: 0x02000AD1 RID: 2769
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Snakebite : CardModel
	{
		// Token: 0x06007764 RID: 30564 RVA: 0x0029792C File Offset: 0x00295B2C
		public Snakebite()
			: base(2, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002062 RID: 8290
		// (get) Token: 0x06007765 RID: 30565 RVA: 0x00297939 File Offset: 0x00295B39
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(7m));
			}
		}

		// Token: 0x17002063 RID: 8291
		// (get) Token: 0x06007766 RID: 30566 RVA: 0x0029794C File Offset: 0x00295B4C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x17002064 RID: 8292
		// (get) Token: 0x06007767 RID: 30567 RVA: 0x0029796C File Offset: 0x00295B6C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x06007768 RID: 30568 RVA: 0x00297974 File Offset: 0x00295B74
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			VfxCmd.PlayOnCreatureCenter(cardPlay.Target, "vfx/vfx_bite");
			await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, base.DynamicVars.Poison.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x002979C7 File Offset: 0x00295BC7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Poison.UpgradeValueBy(3m);
		}
	}
}
