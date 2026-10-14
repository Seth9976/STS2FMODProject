using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B36 RID: 2870
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Voltaic : CardModel
	{
		// Token: 0x0600798C RID: 31116 RVA: 0x0029BFFB File Offset: 0x0029A1FB
		public Voltaic()
			: base(3, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x1700213C RID: 8508
		// (get) Token: 0x0600798D RID: 31117 RVA: 0x0029C008 File Offset: 0x0029A208
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x1700213D RID: 8509
		// (get) Token: 0x0600798E RID: 31118 RVA: 0x0029C02C File Offset: 0x0029A22C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new CalculationExtraVar(1m);
				array[2] = new CalculatedVar("CalculatedChannels").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => CombatManager.Instance.History.Entries.OfType<OrbChanneledEntry>().Count<OrbChanneledEntry>((OrbChanneledEntry e) => e.Actor.Player == card.Owner && e.Orb is LightningOrb));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x1700213E RID: 8510
		// (get) Token: 0x0600798F RID: 31119 RVA: 0x0029C08F File Offset: 0x0029A28F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007990 RID: 31120 RVA: 0x0029C098 File Offset: 0x0029A298
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int lightningChanneledCount = (int)((CalculatedVar)base.DynamicVars["CalculatedChannels"]).Calculate(cardPlay.Target);
			for (int i = 0; i < lightningChanneledCount; i++)
			{
				await OrbCmd.Channel<LightningOrb>(choiceContext, base.Owner);
			}
		}

		// Token: 0x06007991 RID: 31121 RVA: 0x0029C0EB File Offset: 0x0029A2EB
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}

		// Token: 0x040028D9 RID: 10457
		private const string _calculatedChannelsKey = "CalculatedChannels";
	}
}
