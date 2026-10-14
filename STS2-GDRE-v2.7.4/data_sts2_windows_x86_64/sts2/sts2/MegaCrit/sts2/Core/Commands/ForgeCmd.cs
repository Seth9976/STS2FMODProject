using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D2F RID: 3375
	[NullableContext(1)]
	[Nullable(0)]
	public static class ForgeCmd
	{
		/// <summary>
		/// Applies Forge to the player. Adds Sovereign Blade to their hand if they haven't
		/// forged this combat, and adds the amount of damage to it.
		/// </summary>
		/// <param name="amount">Amount of damage to add to Forge.</param>
		/// <param name="player">Player to give the stars to.</param>
		/// <param name="source">Model that caused the forging</param>
		/// <returns>All of the player's un-Exhausted Sovereign Blade cards.</returns>
		// Token: 0x0600883D RID: 34877 RVA: 0x002DE208 File Offset: 0x002DC408
		public static async Task<IEnumerable<SovereignBlade>> Forge(decimal amount, Player player, [Nullable(2)] AbstractModel source)
		{
			IEnumerable<SovereignBlade> enumerable;
			if (CombatManager.Instance.IsOverOrEnding)
			{
				enumerable = Array.Empty<SovereignBlade>();
			}
			else
			{
				List<SovereignBlade> blades = ForgeCmd.GetSovereignBlades(player, false).ToList<SovereignBlade>();
				if (blades.Count == 0)
				{
					SovereignBlade sovereignBlade = player.Creature.CombatState.CreateCard<SovereignBlade>(player);
					sovereignBlade.CreatedThroughForge = true;
					await CardPileCmd.AddGeneratedCardToCombat(sovereignBlade, PileType.Hand, player, CardPilePosition.Bottom);
					blades.Add(sovereignBlade);
					sovereignBlade = null;
				}
				ForgeCmd.IncreaseSovereignBladeDamage(amount, player);
				await Hook.AfterForge(player.Creature.CombatState, amount, player, source);
				enumerable = blades;
			}
			return enumerable;
		}

		// Token: 0x0600883E RID: 34878 RVA: 0x002DE25C File Offset: 0x002DC45C
		private static void IncreaseSovereignBladeDamage(decimal amount, Player player)
		{
			List<SovereignBlade> list = ForgeCmd.GetSovereignBlades(player, true).ToList<SovereignBlade>();
			foreach (SovereignBlade sovereignBlade in list)
			{
				sovereignBlade.AddDamage(amount);
				sovereignBlade.AfterForged();
				ForgeCmd.PlayCombatRoomForgeVfx(player, sovereignBlade);
			}
			ForgeCmd.PreviewSovereignBlade(list);
		}

		// Token: 0x0600883F RID: 34879 RVA: 0x002DE2CC File Offset: 0x002DC4CC
		private static IEnumerable<SovereignBlade> GetSovereignBlades(Player player, bool includeExhausted)
		{
			return player.PlayerCombatState.AllCards.Where<CardModel>(delegate(CardModel c)
			{
				if (c.IsDupe)
				{
					return false;
				}
				if (!includeExhausted)
				{
					CardPile pile = c.Pile;
					return pile == null || pile.Type != PileType.Exhaust;
				}
				return true;
			}).OfType<SovereignBlade>();
		}

		// Token: 0x06008840 RID: 34880 RVA: 0x002DE308 File Offset: 0x002DC508
		private static void PreviewSovereignBlade(IReadOnlyCollection<SovereignBlade> blades)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (!LocalContext.IsMine(blades.First<SovereignBlade>()))
			{
				return;
			}
			List<SovereignBlade> list = blades.Where<SovereignBlade>((SovereignBlade c) => c.Pile.Type == PileType.Hand).ToList<SovereignBlade>();
			List<SovereignBlade> list2 = blades.Where<SovereignBlade>((SovereignBlade c) => c.Pile.Type != PileType.Hand).ToList<SovereignBlade>();
			foreach (SovereignBlade sovereignBlade in list)
			{
				NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create(NCombatRoom.Instance.Ui.Hand.GetCard(sovereignBlade), false);
				NRun.Instance.GlobalUi.AboveTopBarVfxContainer.AddChildSafely(ncardSmithVfx);
			}
			if (list2.Count != 0)
			{
				NCardSmithVfx ncardSmithVfx2 = NCardSmithVfx.Create(list2, false);
				NRun.Instance.GlobalUi.CardPreviewContainer.AddChildSafely(ncardSmithVfx2);
			}
		}

		// Token: 0x06008841 RID: 34881 RVA: 0x002DE418 File Offset: 0x002DC618
		public static void PlayCombatRoomForgeVfx(Player player, CardModel card)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(player.Creature) : null);
			if (ncreature == null)
			{
				return;
			}
			NSovereignBladeVfx nsovereignBladeVfx = SovereignBlade.GetVfxNode(player, card);
			bool flag = nsovereignBladeVfx == null;
			if (flag)
			{
				nsovereignBladeVfx = NSovereignBladeVfx.Create(card);
				ncreature.AddChildSafely(nsovereignBladeVfx);
				nsovereignBladeVfx.Position = Vector2.Zero;
				SfxCmd.Play("event:/sfx/characters/regent/regent_forge", 1f);
			}
			else
			{
				SfxCmd.Play("event:/sfx/characters/regent/regent_refine", 1f);
			}
			nsovereignBladeVfx.Forge((float)card.DynamicVars.Damage.IntValue, flag);
			if (flag)
			{
				List<SovereignBlade> list = ForgeCmd.GetSovereignBlades(player, false).ToList<SovereignBlade>();
				for (int i = 0; i < list.Count; i++)
				{
					NSovereignBladeVfx vfxNode = SovereignBlade.GetVfxNode(player, list[i]);
					if (vfxNode != null)
					{
						vfxNode.OrbitProgress = (double)((float)i / (float)list.Count);
					}
				}
			}
		}

		// Token: 0x04002F5B RID: 12123
		private const string _forgeInitialSfx = "event:/sfx/characters/regent/regent_forge";

		// Token: 0x04002F5C RID: 12124
		private const string _forgeRefineSfx = "event:/sfx/characters/regent/regent_refine";
	}
}
