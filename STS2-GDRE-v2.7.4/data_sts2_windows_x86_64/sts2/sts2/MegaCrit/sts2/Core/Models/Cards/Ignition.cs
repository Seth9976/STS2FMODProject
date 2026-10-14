using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A0B RID: 2571
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ignition : CardModel
	{
		// Token: 0x0600733E RID: 29502 RVA: 0x0028F037 File Offset: 0x0028D237
		public Ignition()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001EA9 RID: 7849
		// (get) Token: 0x0600733F RID: 29503 RVA: 0x0028F044 File Offset: 0x0028D244
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001EAA RID: 7850
		// (get) Token: 0x06007340 RID: 29504 RVA: 0x0028F047 File Offset: 0x0028D247
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001EAB RID: 7851
		// (get) Token: 0x06007341 RID: 29505 RVA: 0x0028F04F File Offset: 0x0028D24F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<PlasmaOrb>()
				});
			}
		}

		// Token: 0x06007342 RID: 29506 RVA: 0x0028F074 File Offset: 0x0028D274
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await OrbCmd.Channel<PlasmaOrb>(choiceContext, cardPlay.Target.Player);
		}

		// Token: 0x06007343 RID: 29507 RVA: 0x0028F0C7 File Offset: 0x0028D2C7
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
