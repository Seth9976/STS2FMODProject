using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000833 RID: 2099
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DenseVegetation : EventModel
	{
		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x060067B4 RID: 26548 RVA: 0x00273F74 File Offset: 0x00272174
		public override bool IsShared
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x060067B5 RID: 26549 RVA: 0x00273F77 File Offset: 0x00272177
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new GoldVar(0),
					new HealVar(0m),
					new HpLossVar(8m)
				});
			}
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x00273FA8 File Offset: 0x002721A8
		public override void CalculateVars()
		{
			base.DynamicVars.Gold.BaseValue = base.Rng.NextInt(61, 100);
			base.DynamicVars.Heal.BaseValue = ((base.Owner != null) ? HealRestSiteOption.GetHealAmount(base.Owner) : 0m);
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x00274004 File Offset: 0x00272204
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.Players.Count == 1)
			{
				return true;
			}
			foreach (Player player in runState.Players)
			{
				if (player.Creature.CurrentHp <= base.DynamicVars.HpLoss.BaseValue)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060067B8 RID: 26552 RVA: 0x00274088 File Offset: 0x00272288
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.TrudgeOn), "DENSE_VEGETATION.pages.INITIAL.options.TRUDGE_ON", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars.HpLoss.BaseValue),
				new EventOption(this, new Func<Task>(this.Rest), "DENSE_VEGETATION.pages.INITIAL.options.REST", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x002740F4 File Offset: 0x002722F4
		private async Task TrudgeOn()
		{
			NEventRoom instance = NEventRoom.Instance;
			Control container = ((instance != null) ? instance.VfxContainer : null);
			if (LocalContext.IsMe(base.Owner) && container != null)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 vector;
					vector..ctor(container.Size.X * 0.25f, container.Size.Y * 0.6f);
					Vector2 vector2;
					vector2..ctor(Rng.Chaotic.NextFloat(-100f, 100f), Rng.Chaotic.NextFloat(-200f, 200f));
					Node2D node2D = VfxCmd.PlayNonCombatVfx(container, vector + vector2, "vfx/vfx_attack_slash");
					Node2D node2D2 = VfxCmd.PlayNonCombatVfx(container, vector + vector2, "vfx/events/dense_vegetation_slice_vfx");
					NDebugAudioManager.Instance.Play("slash_attack.mp3", 0.8f, PitchVariance.Medium);
					node2D.RotationDegrees = -Rng.Chaotic.NextFloat(1f) * 180f;
					node2D2.RotationDegrees = -Rng.Chaotic.NextFloat(1f) * 180f;
					await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
				}
			}
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
			base.SetEventFinished(base.L10NLookup("DENSE_VEGETATION.pages.TRUDGE_ON.description"));
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x00274138 File Offset: 0x00272338
		private async Task Rest()
		{
			await PlayerCmd.MimicRestSiteHeal(base.Owner, false);
			if (LocalContext.IsMe(base.Owner))
			{
				int restHandle = NDebugAudioManager.Instance.Play("sleep.tres", 0.8f, PitchVariance.None);
				await Cmd.CustomScaledWait(0.7f, 1.5f, false, default(CancellationToken));
				NDebugAudioManager.Instance.Stop(restHandle, 0.5f);
				NDebugAudioManager.Instance.Play("hiss.mp3", 0.8f, PitchVariance.Large);
				NGame.Instance.ScreenRumble(ShakeStrength.Medium, ShakeDuration.Normal, RumbleStyle.Rumble);
			}
			this.SetEventState(base.L10NLookup("DENSE_VEGETATION.pages.REST.description"), new <>z__ReadOnlySingleElementList<EventOption>(new EventOption(this, new Func<Task>(this.Fight), "DENSE_VEGETATION.pages.REST.options.FIGHT", Array.Empty<IHoverTip>())));
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x0027417B File Offset: 0x0027237B
		private Task Fight()
		{
			base.EnterCombatWithoutExitingEvent<DenseVegetationEventEncounter>(Array.Empty<Reward>(), false);
			return Task.CompletedTask;
		}
	}
}
