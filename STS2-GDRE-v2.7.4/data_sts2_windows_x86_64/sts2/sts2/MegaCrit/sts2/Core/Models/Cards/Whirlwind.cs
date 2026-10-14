using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B39 RID: 2873
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Whirlwind : CardModel
	{
		// Token: 0x0600799B RID: 31131 RVA: 0x0029C1D3 File Offset: 0x0029A3D3
		public Whirlwind()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17002143 RID: 8515
		// (get) Token: 0x0600799C RID: 31132 RVA: 0x0029C1E0 File Offset: 0x0029A3E0
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002144 RID: 8516
		// (get) Token: 0x0600799D RID: 31133 RVA: 0x0029C1E3 File Offset: 0x0029A3E3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x0600799E RID: 31134 RVA: 0x0029C1F8 File Offset: 0x0029A3F8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num = base.ResolveEnergyXValue();
			if (num > 0)
			{
				Color color;
				color..ctor("FFFFFF80");
				double num2 = ((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.2 : 0.3);
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(color, 0.8 + (double)Mathf.Min(8, num) * num2, true));
				}
				SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_whirlwind", 1f);
				NRun instance2 = NRun.Instance;
				if (instance2 != null)
				{
					instance2.GlobalUi.AddChildSafely(NSmokyVignetteVfx.Create(color, color));
				}
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this, cardPlay)
				.TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_giant_horizontal_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600799F RID: 31135 RVA: 0x0029C24B File Offset: 0x0029A44B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}

		// Token: 0x040028DA RID: 10458
		private const string _whirlwindSfx = "event:/sfx/characters/ironclad/ironclad_whirlwind";
	}
}
