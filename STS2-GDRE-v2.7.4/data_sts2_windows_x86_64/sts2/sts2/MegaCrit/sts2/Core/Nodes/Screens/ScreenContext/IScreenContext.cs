using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext
{
	/// <summary>
	/// An interface that is shared by all screens in sts2. Used to bridge the gap between
	/// all of our various screen systems (Overlays, Capstones, Rooms, Submenus)
	/// </summary>
	// Token: 0x020002CA RID: 714
	[NullableContext(2)]
	public interface IScreenContext
	{
		/// <summary>
		/// The default control to focus on when it becomes the ActiveScreen while the player is using controller
		/// </summary>
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060025CB RID: 9675
		Control DefaultFocusedControl { get; }

		/// <summary>
		/// The default control to focus on when navigating from the top bar (typically from your relics)
		/// Most of the time just equal to the DefaultFocusedControl
		/// </summary>
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060025CC RID: 9676 RVA: 0x000FF79A File Offset: 0x000FD99A
		Control FocusedControlFromTopBar
		{
			get
			{
				return this.DefaultFocusedControl;
			}
		}
	}
}
