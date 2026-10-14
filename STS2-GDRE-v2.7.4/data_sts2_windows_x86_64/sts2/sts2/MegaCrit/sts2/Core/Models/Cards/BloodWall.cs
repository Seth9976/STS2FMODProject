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
	// Token: 0x02000922 RID: 2338
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BloodWall : CardModel
	{
		// Token: 0x06006E61 RID: 28257 RVA: 0x0028539E File Offset: 0x0028359E
		public BloodWall()
			: base(2, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001C8F RID: 7311
		// (get) Token: 0x06006E62 RID: 28258 RVA: 0x002853AB File Offset: 0x002835AB
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("vfx/vfx_blood_wall"));
			}
		}

		// Token: 0x17001C90 RID: 7312
		// (get) Token: 0x06006E63 RID: 28259 RVA: 0x002853BC File Offset: 0x002835BC
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C91 RID: 7313
		// (get) Token: 0x06006E64 RID: 28260 RVA: 0x002853BF File Offset: 0x002835BF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(2m),
					new BlockVar(16m, ValueProp.Move)
				});
			}
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x002853EC File Offset: 0x002835EC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_bloodwall", 1f);
			VfxCmd.PlayOnCreature(base.Owner.Creature, "vfx/vfx_blood_wall");
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x0028543F File Offset: 0x0028363F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(4m);
		}

		// Token: 0x04002849 RID: 10313
		private const string _bloodWallVfxPath = "vfx/vfx_blood_wall";

		// Token: 0x0400284A RID: 10314
		private const string _bloodWallSfx = "event:/sfx/characters/ironclad/ironclad_bloodwall";
	}
}
