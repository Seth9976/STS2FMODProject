using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Entities.Ascension
{
	/// <summary>
	/// Manages ascension logic for runs.
	/// Responsible for tracking ascension levels and applying ascension effects.
	/// </summary>
	// Token: 0x02000CCF RID: 3279
	public class AscensionManager
	{
		/// <summary>
		/// Create a new AscensionManager with the specified ascension level.
		/// </summary>
		/// <param name="level">The ascension level for this run.</param>
		// Token: 0x06008512 RID: 34066 RVA: 0x002CB889 File Offset: 0x002C9A89
		public AscensionManager(int level)
		{
			this._level = level;
		}

		/// <summary>
		/// Create a new AscensionManager with the specified ascension level.
		/// </summary>
		/// <param name="level">The ascension level enum value for this run.</param>
		// Token: 0x06008513 RID: 34067 RVA: 0x002CB898 File Offset: 0x002C9A98
		public AscensionManager(AscensionLevel level)
		{
			this._level = (int)level;
		}

		/// <summary>
		/// Check if a given ascension level is active in the current run.
		/// </summary>
		/// <param name="level">The ascension level to check.</param>
		/// <returns>True if the specified level is active.</returns>
		// Token: 0x06008514 RID: 34068 RVA: 0x002CB8A7 File Offset: 0x002C9AA7
		public bool HasLevel(AscensionLevel level)
		{
			return this._level >= (int)level;
		}

		/// <summary>
		/// Apply ascension effects to a player based on the current ascension level.
		/// </summary>
		/// <param name="player">The player to apply effects to.</param>
		// Token: 0x06008515 RID: 34069 RVA: 0x002CB8B8 File Offset: 0x002C9AB8
		[NullableContext(1)]
		public void ApplyEffectsTo(Player player)
		{
			if (this.HasLevel(AscensionLevel.TightBelt))
			{
				player.SubtractFromMaxPotionCount(1);
			}
			if (this.HasLevel(AscensionLevel.AscendersBane))
			{
				AscendersBane ascendersBane = player.RunState.CreateCard<AscendersBane>(player);
				ascendersBane.FloorAddedToDeck = new int?(1);
				player.Deck.AddInternal(ascendersBane, -1, true);
			}
		}

		/// <summary>
		/// The maximum ascension level allowed in the game.
		/// </summary>
		// Token: 0x04002E25 RID: 11813
		public const int maxAscensionAllowed = 10;

		/// <summary>
		/// The current ascension level for the run.
		/// </summary>
		// Token: 0x04002E26 RID: 11814
		private readonly int _level;
	}
}
