using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006BB RID: 1723
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NoxiousFumesPower : PowerModel
	{
		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x060059CB RID: 22987 RVA: 0x0025113F File Offset: 0x0024F33F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x060059CC RID: 22988 RVA: 0x00251142 File Offset: 0x0024F342
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x060059CD RID: 22989 RVA: 0x00251148 File Offset: 0x0024F348
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x00251168 File Offset: 0x0024F368
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
				foreach (Creature creature in base.CombatState.HittableEnemies)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
					if (ncreature != null)
					{
						NGaseousImpactVfx ngaseousImpactVfx = NGaseousImpactVfx.Create(ncreature.VfxSpawnPosition, new Color("83eb85"));
						NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(ngaseousImpactVfx);
					}
				}
				await PowerCmd.Apply<PoisonPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.Amount, base.Owner, null, false);
			}
		}
	}
}
