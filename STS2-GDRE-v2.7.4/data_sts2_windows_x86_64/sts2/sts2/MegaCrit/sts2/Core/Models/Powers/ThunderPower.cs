using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200071F RID: 1823
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThunderPower : PowerModel
	{
		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06005C47 RID: 23623 RVA: 0x00256006 File Offset: 0x00254206
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06005C48 RID: 23624 RVA: 0x00256009 File Offset: 0x00254209
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x06005C49 RID: 23625 RVA: 0x0025600C File Offset: 0x0025420C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Evoke, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x00256030 File Offset: 0x00254230
		public override async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
		{
			if (orb.Owner == base.Owner.Player)
			{
				if (orb is LightningOrb)
				{
					List<Creature> livingTargets = targets.Where<Creature>((Creature c) => c.IsAlive).ToList<Creature>();
					base.Flash();
					SfxCmd.Play("slash_attack.mp3", 1f);
					VfxCmd.PlayOnCreatureCenters(livingTargets, "vfx/vfx_attack_slash");
					await CreatureCmd.TriggerAnim(orb.Owner.Creature, "Attack", base.Owner.Player.Character.AttackAnimDelay);
					await CreatureCmd.Damage(choiceContext, livingTargets, base.Amount, ValueProp.Unpowered, base.Owner, null, null);
				}
			}
		}
	}
}
