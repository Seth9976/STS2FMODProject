using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ABC RID: 2748
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Severance : CardModel
	{
		// Token: 0x060076F0 RID: 30448 RVA: 0x00296B21 File Offset: 0x00294D21
		public Severance()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x1700202E RID: 8238
		// (get) Token: 0x060076F1 RID: 30449 RVA: 0x00296B2E File Offset: 0x00294D2E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(13m, ValueProp.Move));
			}
		}

		// Token: 0x1700202F RID: 8239
		// (get) Token: 0x060076F2 RID: 30450 RVA: 0x00296B42 File Offset: 0x00294D42
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x060076F3 RID: 30451 RVA: 0x00296B50 File Offset: 0x00294D50
		protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			Severance.<OnPlay>d__5 <OnPlay>d__;
			<OnPlay>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnPlay>d__.<>4__this = this;
			<OnPlay>d__.choiceContext = choiceContext;
			<OnPlay>d__.cardPlay = cardPlay;
			<OnPlay>d__.<>1__state = -1;
			<OnPlay>d__.<>t__builder.Start<Severance.<OnPlay>d__5>(ref <OnPlay>d__);
			return <OnPlay>d__.<>t__builder.Task;
		}

		// Token: 0x060076F4 RID: 30452 RVA: 0x00296BA3 File Offset: 0x00294DA3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
