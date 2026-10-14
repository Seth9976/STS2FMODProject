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
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AFC RID: 2812
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Suppress : CardModel
	{
		// Token: 0x06007854 RID: 30804 RVA: 0x00299982 File Offset: 0x00297B82
		public Suppress()
			: base(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020BE RID: 8382
		// (get) Token: 0x06007855 RID: 30805 RVA: 0x00299990 File Offset: 0x00297B90
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x170020BF RID: 8383
		// (get) Token: 0x06007856 RID: 30806 RVA: 0x002999B0 File Offset: 0x00297BB0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Innate);
			}
		}

		// Token: 0x170020C0 RID: 8384
		// (get) Token: 0x06007857 RID: 30807 RVA: 0x002999B8 File Offset: 0x00297BB8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(11m, ValueProp.Move),
					new PowerVar<WeakPower>(3m)
				});
			}
		}

		// Token: 0x06007858 RID: 30808 RVA: 0x002999E4 File Offset: 0x00297BE4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(NThinSliceVfx.Create(cardPlay.Target, VfxColor.Cyan));
			}
			float num = base.Owner.Character.AttackAnimDelay;
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Normal)
			{
				num += 0.2f;
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithAttackerAnim("Attack", num, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007859 RID: 30809 RVA: 0x00299A37 File Offset: 0x00297C37
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(6m);
			base.DynamicVars.Weak.UpgradeValueBy(2m);
		}
	}
}
