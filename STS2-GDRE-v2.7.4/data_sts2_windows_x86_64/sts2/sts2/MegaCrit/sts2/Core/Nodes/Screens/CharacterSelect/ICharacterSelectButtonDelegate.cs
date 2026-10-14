using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	// Token: 0x0200033B RID: 827
	[NullableContext(1)]
	public interface ICharacterSelectButtonDelegate
	{
		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002EC1 RID: 11969
		StartRunLobby Lobby { get; }

		// Token: 0x06002EC2 RID: 11970
		void SelectCharacter(NCharacterSelectButton charSelectButton, CharacterModel characterModel);
	}
}
