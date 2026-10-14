using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C8E RID: 3214
	public static class NetScreenTypeExtensions
	{
		// Token: 0x0600838F RID: 33679 RVA: 0x002C7030 File Offset: 0x002C5230
		[NullableContext(2)]
		public static Texture2D GetLocationIcon(this NetScreenType screenType)
		{
			string text;
			switch (screenType)
			{
			case NetScreenType.None:
				text = null;
				break;
			case NetScreenType.Room:
				text = null;
				break;
			case NetScreenType.Map:
				text = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_map.tres";
				break;
			case NetScreenType.Settings:
				text = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_settings.tres";
				break;
			case NetScreenType.Compendium:
				text = "res://images/atlases/ui_atlas.sprites/compendium.tres";
				break;
			case NetScreenType.DeckView:
				text = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_deck.tres";
				break;
			case NetScreenType.CardPile:
				text = "res://images/packed/combat_ui/discard_pile.png";
				break;
			case NetScreenType.SimpleCardsView:
				text = "res://images/ui/reward_screen/reward_icon_card.png";
				break;
			case NetScreenType.CardSelection:
				text = "res://images/ui/reward_screen/reward_icon_card.png";
				break;
			case NetScreenType.GameOver:
				text = null;
				break;
			case NetScreenType.PauseMenu:
				text = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_settings.tres";
				break;
			case NetScreenType.Rewards:
				text = "res://images/ui/reward_screen/reward_icon_money.png";
				break;
			case NetScreenType.Feedback:
				text = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_settings.tres";
				break;
			case NetScreenType.SharedRelicPicking:
				text = "res://images/ui/reward_screen/reward_icon_shared_relic.png";
				break;
			case NetScreenType.RemotePlayerExpandedState:
				text = null;
				break;
			default:
				throw new ArgumentOutOfRangeException("screenType", screenType, null);
			}
			string text2 = text;
			if (text2 == null)
			{
				return null;
			}
			return PreloadManager.Cache.GetTexture2D(text2);
		}
	}
}
