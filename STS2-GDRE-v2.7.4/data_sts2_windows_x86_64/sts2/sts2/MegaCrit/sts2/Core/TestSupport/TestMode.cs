using System;

namespace MegaCrit.Sts2.Core.TestSupport
{
	// Token: 0x02000084 RID: 132
	public static class TestMode
	{
		/// <summary>
		/// Whether the game is running in test mode.
		/// True when we're running unit tests, true when we're running the normal game.
		/// </summary>
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00015C19 File Offset: 0x00013E19
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x00015C20 File Offset: 0x00013E20
		public static bool IsOn { get; set; }

		/// <summary>
		/// Whether the game iS NOT running in test mode.
		/// True when we're running the normal game, false when we're running unit tests.
		/// </summary>
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00015C28 File Offset: 0x00013E28
		public static bool IsOff
		{
			get
			{
				return !TestMode.IsOn;
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00015C32 File Offset: 0x00013E32
		public static void AssertOn()
		{
			if (TestMode.IsOn)
			{
				return;
			}
			throw new TestModeOffException();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00015C41 File Offset: 0x00013E41
		public static void AssertOff()
		{
			if (TestMode.IsOff)
			{
				return;
			}
			throw new TestModeOnException();
		}

		/// <summary>
		/// NEVER CALL THIS. Only calls should be in NetCoreRunner and CiCoreRunner.
		/// </summary>
		// Token: 0x06000468 RID: 1128 RVA: 0x00015C50 File Offset: 0x00013E50
		public static void TurnOnInternal()
		{
			TestMode.IsOn = true;
		}
	}
}
