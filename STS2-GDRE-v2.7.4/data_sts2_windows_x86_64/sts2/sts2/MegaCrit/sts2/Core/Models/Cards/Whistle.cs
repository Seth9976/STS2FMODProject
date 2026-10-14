using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B3A RID: 2874
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Whistle : CardModel
	{
		// Token: 0x060079A0 RID: 31136 RVA: 0x0029C263 File Offset: 0x0029A463
		public Whistle()
			: base(3, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002145 RID: 8517
		// (get) Token: 0x060079A1 RID: 31137 RVA: 0x0029C270 File Offset: 0x0029A470
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(StunIntent.GetStaticHoverTip());
			}
		}

		// Token: 0x17002146 RID: 8518
		// (get) Token: 0x060079A2 RID: 31138 RVA: 0x0029C281 File Offset: 0x0029A481
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(33m, ValueProp.Move));
			}
		}

		// Token: 0x17002147 RID: 8519
		// (get) Token: 0x060079A3 RID: 31139 RVA: 0x0029C295 File Offset: 0x0029A495
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x060079A4 RID: 31140 RVA: 0x0029C2A0 File Offset: 0x0029A4A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await CreatureCmd.Stun(cardPlay.Target, null);
		}

		// Token: 0x060079A5 RID: 31141 RVA: 0x0029C2F3 File Offset: 0x0029A4F3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(11m);
		}
	}
}
