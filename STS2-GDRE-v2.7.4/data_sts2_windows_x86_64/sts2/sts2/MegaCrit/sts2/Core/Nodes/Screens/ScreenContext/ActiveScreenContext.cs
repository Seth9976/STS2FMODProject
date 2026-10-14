using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext
{
	/// <summary>
	/// Manages the screen contexts that is currently active (the top-most screen that the player is interacting with).
	/// This is used to span between all of our different screen systems (overlays, capstones, submenus, even rooms).
	/// </summary>
	// Token: 0x020002C9 RID: 713
	[NullableContext(2)]
	[Nullable(0)]
	public class ActiveScreenContext
	{
		/// <summary>
		/// Event used to let listeners know that the Current Active Screen may have changed
		/// Useful for screens that needs to hide/show certain UX depending on if it is the Current Screen or not.
		/// </summary>
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060025C3 RID: 9667 RVA: 0x000FF44C File Offset: 0x000FD64C
		// (remove) Token: 0x060025C4 RID: 9668 RVA: 0x000FF484 File Offset: 0x000FD684
		public event Action Updated;

		// Token: 0x060025C5 RID: 9669 RVA: 0x000FF4B9 File Offset: 0x000FD6B9
		public void Update()
		{
			Action updated = this.Updated;
			if (updated == null)
			{
				return;
			}
			updated();
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x060025C6 RID: 9670 RVA: 0x000FF4CB File Offset: 0x000FD6CB
		[Nullable(1)]
		public static ActiveScreenContext Instance
		{
			[NullableContext(1)]
			get
			{
				ActiveScreenContext activeScreenContext;
				if ((activeScreenContext = ActiveScreenContext._instance) == null)
				{
					activeScreenContext = (ActiveScreenContext._instance = new ActiveScreenContext());
				}
				return activeScreenContext;
			}
		}

		/// <summary>
		/// Looks through all of our screen systems in order of priority to determine what the active screen context currently
		/// is.
		/// </summary>
		/// <returns>The screen context that is currently active.</returns>
		// Token: 0x060025C7 RID: 9671 RVA: 0x000FF4E4 File Offset: 0x000FD6E4
		public IScreenContext GetCurrentScreen()
		{
			NGame instance = NGame.Instance;
			if (((instance != null) ? instance.FeedbackScreen : null) != null && NGame.Instance.FeedbackScreen.Visible)
			{
				return NGame.Instance.FeedbackScreen;
			}
			NModalContainer instance2 = NModalContainer.Instance;
			if (((instance2 != null) ? instance2.OpenModal : null) != null)
			{
				return NModalContainer.Instance.OpenModal;
			}
			NGame instance3 = NGame.Instance;
			if (((instance3 != null) ? instance3.InspectCardScreen : null) != null && NGame.Instance.InspectCardScreen.Visible)
			{
				return NGame.Instance.InspectCardScreen;
			}
			NGame instance4 = NGame.Instance;
			if (((instance4 != null) ? instance4.InspectRelicScreen : null) != null && NGame.Instance.InspectRelicScreen.Visible)
			{
				return NGame.Instance.InspectRelicScreen;
			}
			NGame instance5 = NGame.Instance;
			if (((instance5 != null) ? instance5.LogoAnimation : null) != null)
			{
				return NGame.Instance.LogoAnimation;
			}
			NGame instance6 = NGame.Instance;
			if (((instance6 != null) ? instance6.MainMenu : null) == null)
			{
				if (NRun.Instance != null)
				{
					NRun instance7 = NRun.Instance;
					if (NCapstoneContainer.Instance.CurrentCapstoneScreen != null)
					{
						return NCapstoneContainer.Instance.CurrentCapstoneScreen;
					}
					if (NMapScreen.Instance.IsOpen)
					{
						return NMapScreen.Instance;
					}
					if (NOverlayStack.Instance.ScreenCount > 0)
					{
						return NOverlayStack.Instance.Peek();
					}
					if (instance7.EventRoom != null)
					{
						if (instance7.EventRoom.CustomEventNode != null)
						{
							return instance7.EventRoom.CustomEventNode.CurrentScreenContext;
						}
						NCombatEventLayout ncombatEventLayout = instance7.EventRoom.Layout as NCombatEventLayout;
						if (ncombatEventLayout == null)
						{
							return instance7.EventRoom;
						}
						if (!ncombatEventLayout.HasCombatStarted)
						{
							return instance7.EventRoom;
						}
						return instance7.EventRoom.EmbeddedCombatRoom;
					}
					else
					{
						if (instance7.CombatRoom != null)
						{
							return instance7.CombatRoom;
						}
						if (instance7.TreasureRoom != null)
						{
							return instance7.TreasureRoom;
						}
						if (instance7.RestSiteRoom != null)
						{
							return instance7.RestSiteRoom;
						}
						if (instance7.MapRoom != null)
						{
							return instance7.MapRoom;
						}
						if (instance7.MerchantRoom != null)
						{
							NMerchantRoom merchantRoom = instance7.MerchantRoom;
							if (merchantRoom.Inventory.IsOpen)
							{
								return merchantRoom.Inventory;
							}
							return merchantRoom;
						}
					}
				}
				return null;
			}
			NMainMenu mainMenu = NGame.Instance.MainMenu;
			if (mainMenu.PatchNotesScreen.IsOpen)
			{
				return mainMenu.PatchNotesScreen;
			}
			NMainMenuSubmenuStack submenuStack = mainMenu.SubmenuStack;
			if (!submenuStack.SubmenusOpen)
			{
				return NGame.Instance.MainMenu;
			}
			NTimelineScreen ntimelineScreen = submenuStack.Peek() as NTimelineScreen;
			if (ntimelineScreen != null && ntimelineScreen.CurrentUnlockScreen != null)
			{
				return ntimelineScreen.CurrentUnlockScreen;
			}
			return submenuStack.Peek();
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000FF749 File Offset: 0x000FD949
		[NullableContext(1)]
		public bool IsCurrent(IScreenContext screen)
		{
			return screen == this.GetCurrentScreen();
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000FF754 File Offset: 0x000FD954
		public void FocusOnDefaultControl()
		{
			IScreenContext currentScreen = this.GetCurrentScreen();
			Control control = ((currentScreen != null) ? currentScreen.DefaultFocusedControl : null);
			if (control != null)
			{
				control.TryGrabFocus();
				return;
			}
			Viewport viewport = NGame.Instance.GetViewport();
			if (viewport == null)
			{
				return;
			}
			viewport.GuiReleaseFocus();
		}

		// Token: 0x04000DF8 RID: 3576
		private static ActiveScreenContext _instance;
	}
}
