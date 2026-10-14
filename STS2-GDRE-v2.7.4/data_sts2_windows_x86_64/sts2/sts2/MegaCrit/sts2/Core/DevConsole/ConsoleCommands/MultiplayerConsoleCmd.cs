using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CF9 RID: 3321
	[NullableContext(1)]
	[Nullable(0)]
	public class MultiplayerConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024EE RID: 9454
		// (get) Token: 0x0600863E RID: 34366 RVA: 0x002D0F69 File Offset: 0x002CF169
		public override string CmdName
		{
			get
			{
				return "multiplayer";
			}
		}

		// Token: 0x170024EF RID: 9455
		// (get) Token: 0x0600863F RID: 34367 RVA: 0x002D0F70 File Offset: 0x002CF170
		public override string Args
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170024F0 RID: 9456
		// (get) Token: 0x06008640 RID: 34368 RVA: 0x002D0F77 File Offset: 0x002CF177
		public override string Description
		{
			get
			{
				return "Opens the multiplayer menu, or the test scene if test is the first argument";
			}
		}

		// Token: 0x170024F1 RID: 9457
		// (get) Token: 0x06008641 RID: 34369 RVA: 0x002D0F7E File Offset: 0x002CF17E
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008642 RID: 34370 RVA: 0x002D0F84 File Offset: 0x002CF184
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length != 0 && args[0] == "test")
			{
				NMultiplayerTest nmultiplayerTest = SceneHelper.Instantiate<NMultiplayerTest>("debug/multiplayer_test");
				NGame.Instance.RootSceneContainer.SetCurrentScene(nmultiplayerTest);
				TaskHelper.RunSafely(NGame.Instance.Transition.FadeIn(0.8f, "res://materials/transitions/fade_transition_mat.tres", null));
				return new CmdResult(true, "Opened multiplayer test scene");
			}
			NGame.Instance.MainMenu.OpenMultiplayerSubmenu(null);
			return new CmdResult(true, "Opened multiplayer submenu");
		}
	}
}
