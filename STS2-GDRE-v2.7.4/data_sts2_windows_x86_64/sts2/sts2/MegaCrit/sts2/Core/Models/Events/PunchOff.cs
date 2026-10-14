using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000848 RID: 2120
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PunchOff : EventModel
	{
		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x06006871 RID: 26737 RVA: 0x00277CFF File Offset: 0x00275EFF
		public override EventLayoutType LayoutType
		{
			get
			{
				return EventLayoutType.Combat;
			}
		}

		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x06006872 RID: 26738 RVA: 0x00277D02 File Offset: 0x00275F02
		public override EncounterModel CanonicalEncounter
		{
			get
			{
				return ModelDb.Encounter<PunchOffEventEncounter>();
			}
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x06006873 RID: 26739 RVA: 0x00277D09 File Offset: 0x00275F09
		public override bool IsShared
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006874 RID: 26740 RVA: 0x00277D0C File Offset: 0x00275F0C
		public override bool IsAllowed(IRunState runState)
		{
			return runState.TotalFloor >= 6;
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x06006875 RID: 26741 RVA: 0x00277D1A File Offset: 0x00275F1A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(0));
			}
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x00277D28 File Offset: 0x00275F28
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Nab), "PUNCH_OFF.pages.INITIAL.options.NAB", HoverTipFactory.FromCardWithCardHoverTips<Injury>(false)),
				new EventOption(this, new Func<Task>(this.TakeThem), "PUNCH_OFF.pages.INITIAL.options.I_CAN_TAKE_THEM", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x00277D7F File Offset: 0x00275F7F
		public override Task AfterEventStarted()
		{
			RunManager.Instance.RoomExited += this.OnRoomExited;
			this._punchCts = new CancellationTokenSource();
			TaskHelper.RunSafely(this.PunchEachOther());
			return Task.CompletedTask;
		}

		// Token: 0x06006878 RID: 26744 RVA: 0x00277DB4 File Offset: 0x00275FB4
		private async Task PunchEachOther()
		{
			Creature leftEnemy = this._combatSynchronizer.CombatStateForLayout.Enemies[0];
			Creature rightEnemy = this._combatSynchronizer.CombatStateForLayout.Enemies[1];
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature leftEnemyNode = ((instance != null) ? instance.GetCreatureNode(leftEnemy) : null);
			if (leftEnemyNode != null)
			{
				Vector2 originalScale = leftEnemyNode.Scale;
				leftEnemyNode.Scale = new Vector2(-originalScale.X, originalScale.Y);
				NCombatRoom instance2 = NCombatRoom.Instance;
				Control vfxContainer = ((instance2 != null) ? instance2.CombatVfxContainer : null);
				while (!this._punchCts.IsCancellationRequested)
				{
					await CreatureCmd.TriggerAnim(leftEnemy, "Attack", 0f);
					await Cmd.Wait(0.1f, false);
					VfxCmd.PlayOnCreatureCenter(rightEnemy, "vfx/vfx_attack_blunt");
					Control control = vfxContainer;
					if (control != null)
					{
						control.AddChildSafely(NHitSparkVfx.Create(rightEnemy, false));
					}
					await CreatureCmd.TriggerAnim(rightEnemy, "Hit", 0f);
					await Cmd.Wait(1.2f, false);
					if (this._punchCts.IsCancellationRequested)
					{
						break;
					}
					await CreatureCmd.TriggerAnim(rightEnemy, "Attack", 0f);
					await Cmd.Wait(0.1f, false);
					VfxCmd.PlayOnCreatureCenter(leftEnemy, "vfx/vfx_attack_blunt");
					Control control2 = vfxContainer;
					if (control2 != null)
					{
						control2.AddChildSafely(NHitSparkVfx.Create(leftEnemy, false));
					}
					await CreatureCmd.TriggerAnim(leftEnemy, "Hit", 0f);
					await Cmd.Wait(1.2f, false);
				}
				this._punchCts = null;
				if (leftEnemyNode.IsValid())
				{
					leftEnemyNode.Scale = originalScale;
				}
			}
		}

		// Token: 0x06006879 RID: 26745 RVA: 0x00277DF7 File Offset: 0x00275FF7
		public override void CalculateVars()
		{
			base.DynamicVars.Gold.BaseValue = base.Rng.NextInt(91, 99);
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x00277E20 File Offset: 0x00276020
		private async Task Nab()
		{
			await CardPileCmd.AddCurseToDeck<Injury>(base.Owner);
			NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("blunt_attack.mp3", 1f, PitchVariance.None);
			}
			await Cmd.CustomScaledWait(0.25f, 0.5f, false, default(CancellationToken));
			await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1)
			{
				new RelicReward(base.Owner)
			});
			base.SetEventFinished(base.L10NLookup("PUNCH_OFF.pages.NAB.description"));
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x00277E64 File Offset: 0x00276064
		private Task TakeThem()
		{
			CancellationTokenSource punchCts = this._punchCts;
			if (punchCts != null)
			{
				punchCts.Cancel();
			}
			this.SetEventState(base.L10NLookup("PUNCH_OFF.pages.I_CAN_TAKE_THEM.description"), new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, new Func<Task>(this.Fight), "PUNCH_OFF.pages.I_CAN_TAKE_THEM.options.FIGHT", Array.Empty<IHoverTip>())));
			return Task.CompletedTask;
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x00277EBC File Offset: 0x002760BC
		private unsafe Task Fight()
		{
			int num = 2;
			List<Reward> list = new List<Reward>(num);
			CollectionsMarshal.SetCount<Reward>(list, num);
			Span<Reward> span = CollectionsMarshal.AsSpan<Reward>(list);
			int num2 = 0;
			*span[num2] = new RelicReward(base.Owner);
			num2++;
			*span[num2] = new PotionReward(base.Owner);
			List<Reward> list2 = list;
			base.EnterCombatWithoutExitingEvent<PunchOffEventEncounter>(list2, false);
			return Task.CompletedTask;
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x00277F1B File Offset: 0x0027611B
		private void OnRoomExited()
		{
			RunManager.Instance.RoomExited -= this.OnRoomExited;
			CancellationTokenSource punchCts = this._punchCts;
			if (punchCts == null)
			{
				return;
			}
			punchCts.Cancel();
		}

		/// <summary>
		/// Used to end the enemies punching each other.
		/// </summary>
		// Token: 0x0400279C RID: 10140
		[Nullable(2)]
		private CancellationTokenSource _punchCts;
	}
}
