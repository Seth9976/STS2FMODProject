using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007A4 RID: 1956
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Byrdpip : MonsterModel
	{
		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x06005FE8 RID: 24552 RVA: 0x0025C528 File Offset: 0x0025A728
		public override int MinInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x06005FE9 RID: 24553 RVA: 0x0025C52F File Offset: 0x0025A72F
		public override int MaxInitialHp
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x06005FEA RID: 24554 RVA: 0x0025C536 File Offset: 0x0025A736
		public override bool IsHealthBarVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x0025C53C File Offset: 0x0025A73C
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			string text;
			if (base.IsMutable)
			{
				text = base.Creature.PetOwner.GetRelic<Byrdpip>().Skin;
			}
			else
			{
				text = Byrdpip.SkinOptions[0];
			}
			MegaSkeletonDataResource data = skeleton.GetData();
			skeleton.SetSkin(data.FindSkin(text));
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x0025C58C File Offset: 0x0025A78C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, Array.Empty<AbstractIntent>());
			moveState.FollowUpState = moveState;
			return new MonsterMoveStateMachine(new <>z__ReadOnlySingleElementList<MonsterState>(moveState), moveState);
		}
	}
}
