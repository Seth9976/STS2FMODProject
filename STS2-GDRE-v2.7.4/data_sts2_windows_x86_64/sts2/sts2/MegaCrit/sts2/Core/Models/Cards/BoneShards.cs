using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000929 RID: 2345
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BoneShards : CardModel
	{
		// Token: 0x06006E87 RID: 28295 RVA: 0x002858FB File Offset: 0x00283AFB
		public BoneShards()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001C9D RID: 7325
		// (get) Token: 0x06006E88 RID: 28296 RVA: 0x00285908 File Offset: 0x00283B08
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001C9E RID: 7326
		// (get) Token: 0x06006E89 RID: 28297 RVA: 0x00285915 File Offset: 0x00283B15
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C9F RID: 7327
		// (get) Token: 0x06006E8A RID: 28298 RVA: 0x00285918 File Offset: 0x00283B18
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001CA0 RID: 7328
		// (get) Token: 0x06006E8B RID: 28299 RVA: 0x00285927 File Offset: 0x00283B27
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new OstyDamageVar(9m, ValueProp.Move),
					new BlockVar(9m, ValueProp.Move)
				});
			}
		}

		// Token: 0x06006E8C RID: 28300 RVA: 0x00285954 File Offset: 0x00283B54
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).TargetingAllOpponents(base.CombatState)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
				if (base.Owner.IsOstyAlive)
				{
					await CreatureCmd.Kill(base.Owner.Osty, false);
				}
			}
		}

		// Token: 0x06006E8D RID: 28301 RVA: 0x002859A7 File Offset: 0x00283BA7
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(3m);
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
