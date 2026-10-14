using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200099C RID: 2460
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DramaticEntrance : CardModel
	{
		// Token: 0x060070DD RID: 28893 RVA: 0x0028A280 File Offset: 0x00288480
		public DramaticEntrance()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001DA3 RID: 7587
		// (get) Token: 0x060070DE RID: 28894 RVA: 0x0028A28D File Offset: 0x0028848D
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("vfx/vfx_dramatic_entrance_fullscreen"));
			}
		}

		// Token: 0x17001DA4 RID: 7588
		// (get) Token: 0x060070DF RID: 28895 RVA: 0x0028A29E File Offset: 0x0028849E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x17001DA5 RID: 7589
		// (get) Token: 0x060070E0 RID: 28896 RVA: 0x0028A2B2 File Offset: 0x002884B2
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Exhaust,
					CardKeyword.Innate
				});
			}
		}

		// Token: 0x060070E1 RID: 28897 RVA: 0x0028A2C7 File Offset: 0x002884C7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}

		// Token: 0x060070E2 RID: 28898 RVA: 0x0028A2E0 File Offset: 0x002884E0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", base.Owner.Character.AttackAnimDelay);
			VfxCmd.PlayFullScreenInCombat("vfx/vfx_dramatic_entrance_fullscreen", base.Owner.Creature);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x04002862 RID: 10338
		public const string vfxPath = "vfx/vfx_dramatic_entrance_fullscreen";
	}
}
