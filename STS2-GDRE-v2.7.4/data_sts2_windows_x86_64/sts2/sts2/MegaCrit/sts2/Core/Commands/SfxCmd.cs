using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D39 RID: 3385
	[NullableContext(1)]
	[Nullable(0)]
	public static class SfxCmd
	{
		// Token: 0x06008873 RID: 34931 RVA: 0x002DF40B File Offset: 0x002DD60B
		public static void Play(string sfx, float volume = 1f)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			NAudioManager.Instance.PlayOneShot(sfx, volume);
		}

		// Token: 0x06008874 RID: 34932 RVA: 0x002DF42E File Offset: 0x002DD62E
		public static void Play(string sfx, string param, float val, float volume = 1f)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			NAudioManager.Instance.PlayOneShot(sfx, new Dictionary<string, float> { { param, val } }, volume);
		}

		// Token: 0x06008875 RID: 34933 RVA: 0x002DF45E File Offset: 0x002DD65E
		public static void PlayLoop(string sfx, bool usesLoopParam = true)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NAudioManager.Instance.PlayLoop(sfx, usesLoopParam);
		}

		// Token: 0x06008876 RID: 34934 RVA: 0x002DF474 File Offset: 0x002DD674
		public static void PlayLoop(Creature creature, string sfx)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NCreature creatureNode = creature.GetCreatureNode();
			if (creatureNode != null)
			{
				creatureNode.StartSfxLoop(sfx);
			}
		}

		// Token: 0x06008877 RID: 34935 RVA: 0x002DF49C File Offset: 0x002DD69C
		public static void PlayLoop(Creature creature, string sfx, string loopParam, float loopStopValue)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NCreature creatureNode = creature.GetCreatureNode();
			if (creatureNode != null)
			{
				creatureNode.StartSfxLoop(sfx, loopParam, loopStopValue);
			}
		}

		// Token: 0x06008878 RID: 34936 RVA: 0x002DF4C4 File Offset: 0x002DD6C4
		public static void StopLoop(string sfx)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NAudioManager.Instance.StopLoop(sfx);
		}

		// Token: 0x06008879 RID: 34937 RVA: 0x002DF4DC File Offset: 0x002DD6DC
		public static void StopLoop(Creature creature, string sfx)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NCreature creatureNode = creature.GetCreatureNode();
			if (creatureNode != null)
			{
				creatureNode.StopSfxLoop(sfx);
			}
		}

		// Token: 0x0600887A RID: 34938 RVA: 0x002DF502 File Offset: 0x002DD702
		public static void SetParam(string sfx, string param, float value)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NAudioManager.Instance.SetParam(sfx, param, value);
		}

		// Token: 0x0600887B RID: 34939 RVA: 0x002DF51C File Offset: 0x002DD71C
		[NullableContext(2)]
		public static void PlayDamage(MonsterModel monster, int damageAmount)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			if (monster == null)
			{
				return;
			}
			NAudioManager.Instance.PlayOneShot(monster.TakeDamageSfx, new Dictionary<string, float> { { "EnemyImpact_Intensity", 2f } }, 1f);
		}

		// Token: 0x0600887C RID: 34940 RVA: 0x002DF56C File Offset: 0x002DD76C
		[NullableContext(2)]
		public static void PlayDeath(MonsterModel monster)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (monster == null)
			{
				return;
			}
			NAudioManager.Instance.PlayOneShot(monster.DeathSfx, 1f);
		}

		// Token: 0x0600887D RID: 34941 RVA: 0x002DF58F File Offset: 0x002DD78F
		public static void PlayDeath(Player player)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NAudioManager.Instance.PlayOneShot(player.Character.DeathSfx, 1f);
		}

		/// <summary>
		/// Plays the correct swoosh sfx depending on the pile the carf came from and is going to
		/// </summary>
		// Token: 0x0600887E RID: 34942 RVA: 0x002DF5B4 File Offset: 0x002DD7B4
		public static void PlayCardSwooshSfx(CardPile currentPile, [Nullable(2)] CardPile prevPile = null)
		{
			if (currentPile.Type == PileType.Draw)
			{
				SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_into_draw", 1f);
				return;
			}
			if (currentPile.Type != PileType.Discard)
			{
				SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_into_deck", 1f);
				return;
			}
			if (prevPile != null && prevPile.Type == PileType.Play)
			{
				SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_play_into_discard", 1f);
				return;
			}
			SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_into_discard", 1f);
		}
	}
}
