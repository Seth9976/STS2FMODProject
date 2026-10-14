using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000978 RID: 2424
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeadlyPoison : CardModel
	{
		// Token: 0x06007012 RID: 28690 RVA: 0x00288B4E File Offset: 0x00286D4E
		public DeadlyPoison()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D40 RID: 7488
		// (get) Token: 0x06007013 RID: 28691 RVA: 0x00288B5B File Offset: 0x00286D5B
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NSmokePuffVfx.AssetPaths;
			}
		}

		// Token: 0x17001D41 RID: 7489
		// (get) Token: 0x06007014 RID: 28692 RVA: 0x00288B62 File Offset: 0x00286D62
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(5m));
			}
		}

		// Token: 0x17001D42 RID: 7490
		// (get) Token: 0x06007015 RID: 28693 RVA: 0x00288B74 File Offset: 0x00286D74
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06007016 RID: 28694 RVA: 0x00288B94 File Offset: 0x00286D94
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			NPoisonImpactVfx npoisonImpactVfx = NPoisonImpactVfx.Create(cardPlay.Target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(npoisonImpactVfx);
			}
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, base.DynamicVars.Poison.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007017 RID: 28695 RVA: 0x00288BE7 File Offset: 0x00286DE7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Poison.UpgradeValueBy(2m);
		}
	}
}
