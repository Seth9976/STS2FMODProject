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
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000966 RID: 2406
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Corruption : CardModel
	{
		// Token: 0x06006FBD RID: 28605 RVA: 0x00288033 File Offset: 0x00286233
		public Corruption()
			: base(3, CardType.Power, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001D1F RID: 7455
		// (get) Token: 0x06006FBE RID: 28606 RVA: 0x00288040 File Offset: 0x00286240
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 1m));
			}
		}

		// Token: 0x17001D20 RID: 7456
		// (get) Token: 0x06006FBF RID: 28607 RVA: 0x00288056 File Offset: 0x00286256
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06006FC0 RID: 28608 RVA: 0x00288064 File Offset: 0x00286264
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			NPowerUpVfx.CreateNormal(base.Owner.Creature);
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<CorruptionPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Power"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FC1 RID: 28609 RVA: 0x002880AF File Offset: 0x002862AF
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x04002858 RID: 10328
		private const string _powerVarName = "Power";
	}
}
