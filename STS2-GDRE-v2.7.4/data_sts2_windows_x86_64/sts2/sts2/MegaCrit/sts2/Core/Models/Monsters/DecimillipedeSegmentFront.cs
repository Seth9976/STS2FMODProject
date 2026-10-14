using System;
using MegaCrit.Sts2.Core.Nodes.Animation;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	/// <summary>
	/// Monster class exists to connect this to the correct monster visual scene.
	/// All logic lives in the <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.DecimillipedeSegment" />.
	/// </summary>
	// Token: 0x020007AF RID: 1967
	public sealed class DecimillipedeSegmentFront : DecimillipedeSegment
	{
		// Token: 0x060060D2 RID: 24786 RVA: 0x0025ECE8 File Offset: 0x0025CEE8
		public override void SegmentAttack()
		{
			NCreature creatureNode = base.Creature.GetCreatureNode();
			if (creatureNode != null)
			{
				NDecimillipedeSegmentDriver specialNode = creatureNode.GetSpecialNode<NDecimillipedeSegmentDriver>("%Visuals/SegmentDriver");
				if (specialNode == null)
				{
					return;
				}
				specialNode.AttackShake();
			}
		}
	}
}
