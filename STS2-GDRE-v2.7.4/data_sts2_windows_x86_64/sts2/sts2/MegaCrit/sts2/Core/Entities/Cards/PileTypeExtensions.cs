using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CC5 RID: 3269
	public static class PileTypeExtensions
	{
		// Token: 0x060084E6 RID: 34022 RVA: 0x002CB10C File Offset: 0x002C930C
		[NullableContext(1)]
		public static CardPile GetPile(this PileType pileType, Player player)
		{
			ArgumentNullException.ThrowIfNull(player, "player");
			CardPile cardPile = CardPile.Get(pileType, player);
			if (cardPile == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get ");
				defaultInterpolatedStringHandler.AppendFormatted<PileType>(pileType);
				defaultInterpolatedStringHandler.AppendLiteral(" pile while out of combat.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return cardPile;
		}

		/// <summary>
		/// Is this pile a combat pile? Combat piles are:
		///
		/// Hand
		/// Draw
		/// Discard
		/// Exhaust
		/// Play
		/// </summary>
		/// <param name="pileType">Pile to check.</param>
		/// <returns>Whether or not it's a combat pile.</returns>
		// Token: 0x060084E7 RID: 34023 RVA: 0x002CB168 File Offset: 0x002C9368
		public static bool IsCombatPile(this PileType pileType)
		{
			return pileType - PileType.Draw <= 4;
		}

		// Token: 0x060084E8 RID: 34024 RVA: 0x002CB184 File Offset: 0x002C9384
		[NullableContext(2)]
		public static Vector2 GetTargetPosition(this PileType pileType, NCard node)
		{
			if (pileType.IsCombatPile() && !CombatManager.Instance.IsInProgress)
			{
				return Vector2.Zero;
			}
			Vector2 size = NGame.Instance.GetViewportRect().Size;
			Vector2 vector;
			switch (pileType)
			{
			case PileType.None:
				vector = size;
				break;
			case PileType.Draw:
				vector = NCombatRoom.Instance.Ui.DrawPile.GlobalPosition + NCombatRoom.Instance.Ui.DrawPile.Size * 0.5f;
				break;
			case PileType.Hand:
				vector = new Vector2(size.X * 0.5f - node.Size.X * 0.5f, size.Y - node.Size.Y * 0.5f);
				break;
			case PileType.Discard:
				vector = NCombatRoom.Instance.Ui.DiscardPile.GlobalPosition + NCombatRoom.Instance.Ui.DiscardPile.Size * 0.5f;
				break;
			case PileType.Exhaust:
				vector = NCombatRoom.Instance.Ui.ExhaustPile.GlobalPosition + NCombatRoom.Instance.Ui.ExhaustPile.Size * 0.5f;
				break;
			case PileType.Play:
				vector = NCombatRoom.Instance.Ui.PlayContainer.Size * 0.5f - node.Size * 0.5f + Vector2.Up * 100f;
				break;
			case PileType.Deck:
				vector = NRun.Instance.GlobalUi.TopBar.Deck.GlobalPosition + NRun.Instance.GlobalUi.TopBar.Deck.Size * 0.5f;
				break;
			default:
				throw new ArgumentOutOfRangeException("pileType", pileType, "Unknown pile type");
			}
			return vector;
		}
	}
}
