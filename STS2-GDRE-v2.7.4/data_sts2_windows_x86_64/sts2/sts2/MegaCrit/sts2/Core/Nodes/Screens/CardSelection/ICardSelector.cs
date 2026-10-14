using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// An interface used to define any screen that allows for cards to be chosen
	/// (ie NCardSelectionScreen, NChooseACardScreen)
	/// </summary>
	// Token: 0x02000343 RID: 835
	[NullableContext(1)]
	public interface ICardSelector
	{
		// Token: 0x06002F99 RID: 12185
		Task<IEnumerable<CardModel>> CardsSelected();
	}
}
