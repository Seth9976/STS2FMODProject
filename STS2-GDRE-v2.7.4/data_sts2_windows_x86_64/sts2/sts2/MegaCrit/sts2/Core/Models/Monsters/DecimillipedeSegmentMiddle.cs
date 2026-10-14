using System;
using MegaCrit.Sts2.Core.Nodes.Animation;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	/// <summary>
	/// Monster class exists to connect this to the correct monster visual scene.
	/// All logic lives in the <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.DecimillipedeSegment" />.
	/// </summary>
	// Token: 0x020007B0 RID: 1968
	public sealed class DecimillipedeSegmentMiddle : DecimillipedeSegment
	{
		// Token: 0x060060D4 RID: 24788 RVA: 0x0025ED24 File Offset: 0x0025CF24
		public override void SegmentAttack()
		{
			NCreature creatureNode = base.Creature.GetCreatureNode();
			if (creatureNode != null)
			{
				NDecimillipedeSegmentDriver specialNode = creatureNode.GetSpecialNode<NDecimillipedeSegmentDriver>("%Visuals/RightSegmentDriver");
				if (specialNode != null)
				{
					specialNode.AttackShake();
				}
			}
			if (creatureNode != null)
			{
				NDecimillipedeSegmentDriver specialNode2 = creatureNode.GetSpecialNode<NDecimillipedeSegmentDriver>("%Visuals/LeftSegmentDriver");
				if (specialNode2 == null)
				{
					return;
				}
				specialNode2.AttackShake();
			}
		}
	}
}
