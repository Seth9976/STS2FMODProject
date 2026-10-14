using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C69 RID: 3177
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HealRestSiteOption : RestSiteOption
	{
		// Token: 0x17002399 RID: 9113
		// (get) Token: 0x0600825E RID: 33374 RVA: 0x002C1291 File Offset: 0x002BF491
		public override string OptionId
		{
			get
			{
				return "HEAL";
			}
		}

		// Token: 0x1700239A RID: 9114
		// (get) Token: 0x0600825F RID: 33375 RVA: 0x002C1298 File Offset: 0x002BF498
		public override IEnumerable<string> AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.AddRange(base.AssetPaths);
				list.AddRange(NRestSmokeVfx.AssetPaths);
				list.AddRange(NDesaturateTransitionVfx.AssetPaths);
				return new <>z__ReadOnlyList<string>(list);
			}
		}

		// Token: 0x06008260 RID: 33376 RVA: 0x002C12C6 File Offset: 0x002BF4C6
		public static decimal GetHealAmount(Player player)
		{
			return Hook.ModifyRestSiteHealAmount(player.RunState, player.Creature, HealRestSiteOption.GetBaseHealAmount(player.Creature));
		}

		// Token: 0x1700239B RID: 9115
		// (get) Token: 0x06008261 RID: 33377 RVA: 0x002C12E4 File Offset: 0x002BF4E4
		public override LocString Description
		{
			get
			{
				LocString description = base.Description;
				HealVar healVar = new HealVar(HealRestSiteOption.GetBaseHealAmount(base.Owner.Creature))
				{
					PreviewValue = HealRestSiteOption.GetHealAmount(base.Owner)
				};
				description.Add("Character", base.Owner.Character.Id.Entry);
				description.Add(healVar);
				IReadOnlyList<LocString> readOnlyList = Hook.ModifyExtraRestSiteHealText(base.Owner.RunState, base.Owner, Array.Empty<LocString>());
				if (readOnlyList.Any<LocString>())
				{
					description.Add("ExtraText", "\n" + string.Join("\n", readOnlyList.Select<LocString, string>((LocString s) => s.GetFormattedText())));
				}
				else
				{
					description.Add("ExtraText", string.Empty);
				}
				return description;
			}
		}

		// Token: 0x06008262 RID: 33378 RVA: 0x002C13C1 File Offset: 0x002BF5C1
		public HealRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x06008263 RID: 33379 RVA: 0x002C13CC File Offset: 0x002BF5CC
		public override async Task<bool> OnSelect()
		{
			await HealRestSiteOption.ExecuteRestSiteHeal(base.Owner, false);
			return true;
		}

		// Token: 0x06008264 RID: 33380 RVA: 0x002C1410 File Offset: 0x002BF610
		public override async Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			HealRestSiteOption.PlayRestSiteHealSfx();
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			if (instance != null)
			{
				instance.AddChildSafely(NRestSmokeVfx.Create());
			}
			NRestSiteRoom instance2 = NRestSiteRoom.Instance;
			if (instance2 != null)
			{
				instance2.AddChildSafely(NDesaturateTransitionVfx.Create());
			}
			await Cmd.CustomScaledWait(1.5f, 2.5f, false, ct);
		}

		// Token: 0x06008265 RID: 33381 RVA: 0x002C1453 File Offset: 0x002BF653
		public override Task DoRemotePostSelectVfx()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("SOTE_SFX_SleepBlanket_v1.mp3", 0.5f, PitchVariance.Small);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06008266 RID: 33382 RVA: 0x002C1476 File Offset: 0x002BF676
		public static decimal GetBaseHealAmount(Creature creature)
		{
			return creature.MaxHp * 0.3m;
		}

		// Token: 0x06008267 RID: 33383 RVA: 0x002C1492 File Offset: 0x002BF692
		public static void PlayRestSiteHealSfx()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("sleep.tres", 1f, PitchVariance.None);
			}
			NDebugAudioManager instance2 = NDebugAudioManager.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.Play("SOTE_SFX_SleepBlanket_v1.mp3", 1f, PitchVariance.Small);
		}

		/// <summary>
		/// Heals the player's creature and offers rewards if applicable.
		/// This is public because <see cref="M:MegaCrit.Sts2.Core.Commands.PlayerCmd.MimicRestSiteHeal(MegaCrit.Sts2.Core.Entities.Players.Player,System.Boolean)" /> calls it for mimicking rest site heals
		/// (for example, for <see cref="T:MegaCrit.Sts2.Core.Models.Events.DenseVegetation" />'s Rest option).
		/// </summary>
		// Token: 0x06008268 RID: 33384 RVA: 0x002C14CC File Offset: 0x002BF6CC
		public static async Task ExecuteRestSiteHeal(Player player, bool isMimicked)
		{
			await CreatureCmd.Heal(player.Creature, HealRestSiteOption.GetHealAmount(player), true);
			await Hook.AfterRestSiteHeal(player.RunState, player, isMimicked);
			List<Reward> list = new List<Reward>();
			Hook.ModifyRestSiteHealRewards(player.RunState, player, list, isMimicked);
			await RewardsCmd.OfferCustom(player, list);
		}
	}
}
