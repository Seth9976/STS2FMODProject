using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B2D RID: 2861
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UpMySleeve : CardModel
	{
		// Token: 0x0600795F RID: 31071 RVA: 0x0029BAAF File Offset: 0x00299CAF
		public UpMySleeve()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x1700212A RID: 8490
		// (get) Token: 0x06007960 RID: 31072 RVA: 0x0029BABC File Offset: 0x00299CBC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x1700212B RID: 8491
		// (get) Token: 0x06007961 RID: 31073 RVA: 0x0029BAC9 File Offset: 0x00299CC9
		// (set) Token: 0x06007962 RID: 31074 RVA: 0x0029BAD1 File Offset: 0x00299CD1
		private int TimesPlayedThisCombat
		{
			get
			{
				return this._timesPlayedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._timesPlayedThisCombat = value;
			}
		}

		// Token: 0x1700212C RID: 8492
		// (get) Token: 0x06007963 RID: 31075 RVA: 0x0029BAE0 File Offset: 0x00299CE0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x0029BAF0 File Offset: 0x00299CF0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				await Shiv.CreateInHand(base.Owner, base.CombatState, null);
				await Cmd.Wait(0.1f, false);
			}
			this.TimesPlayedThisCombat++;
			base.EnergyCost.AddThisCombat(-1, false);
		}

		// Token: 0x06007965 RID: 31077 RVA: 0x0029BB33 File Offset: 0x00299D33
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}

		// Token: 0x040028D7 RID: 10455
		private int _timesPlayedThisCombat;
	}
}
