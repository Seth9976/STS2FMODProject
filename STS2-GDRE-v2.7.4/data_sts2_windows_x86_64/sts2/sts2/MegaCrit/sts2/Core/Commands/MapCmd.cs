using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D30 RID: 3376
	public static class MapCmd
	{
		// Token: 0x06008842 RID: 34882 RVA: 0x002DE4F0 File Offset: 0x002DC6F0
		[NullableContext(1)]
		public static void SetBossEncounter(IRunState runState, EncounterModel boss)
		{
			runState.Act.SetBossEncounter(boss);
			if (TestMode.IsOff)
			{
				NRun.Instance.GlobalUi.TopBar.BossIcon.RefreshBossIcon();
				NMapScreen instance = NMapScreen.Instance;
				if (instance == null)
				{
					return;
				}
				instance.SetMap(runState.Map, runState.Rng.Seed, false);
			}
		}
	}
}
