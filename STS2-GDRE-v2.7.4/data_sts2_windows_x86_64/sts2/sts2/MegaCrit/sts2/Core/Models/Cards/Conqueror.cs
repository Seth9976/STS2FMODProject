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
	// Token: 0x0200095E RID: 2398
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Conqueror : CardModel
	{
		// Token: 0x06006F91 RID: 28561 RVA: 0x00287AE2 File Offset: 0x00285CE2
		public Conqueror()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D0B RID: 7435
		// (get) Token: 0x06006F92 RID: 28562 RVA: 0x00287AEF File Offset: 0x00285CEF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(3));
			}
		}

		// Token: 0x17001D0C RID: 7436
		// (get) Token: 0x06006F93 RID: 28563 RVA: 0x00287AFC File Offset: 0x00285CFC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x06006F94 RID: 28564 RVA: 0x00287B04 File Offset: 0x00285D04
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
			await PowerCmd.Apply<ConquerorPower>(choiceContext, cardPlay.Target, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F95 RID: 28565 RVA: 0x00287B57 File Offset: 0x00285D57
		protected override void OnUpgrade()
		{
			base.DynamicVars.Forge.UpgradeValueBy(2m);
		}
	}
}
