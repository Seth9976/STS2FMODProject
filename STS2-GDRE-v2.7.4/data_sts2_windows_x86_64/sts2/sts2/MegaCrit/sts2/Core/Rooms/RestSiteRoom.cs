using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x0200012E RID: 302
	[NullableContext(1)]
	[Nullable(0)]
	public class RestSiteRoom : AbstractRoom
	{
		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0005C4E6 File Offset: 0x0005A6E6
		public override RoomType RoomType
		{
			get
			{
				return RoomType.RestSite;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0005C4E9 File Offset: 0x0005A6E9
		[Nullable(2)]
		public override ModelId ModelId
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0005C4EC File Offset: 0x0005A6EC
		public IReadOnlyList<RestSiteOption> Options
		{
			get
			{
				RestSiteSynchronizer synchronizer = this._synchronizer;
				return ((synchronizer != null) ? synchronizer.GetLocalOptions() : null) ?? Array.Empty<RestSiteOption>();
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0005C50C File Offset: 0x0005A70C
		public override async Task EnterInternal([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase)
		{
			if (isRestoringRoomStackBase)
			{
				throw new InvalidOperationException("RestSiteRoom does not support room stack reconstruction.");
			}
			this._synchronizer = RunManager.Instance.RestSiteSynchronizer;
			this._synchronizer.BeginRestSite();
			if (runState != null)
			{
				await PreloadManager.LoadRoomRestSite(runState.Act, this.Options);
				this.ShowRoomNode(runState);
				await Hook.AfterRoomEntered(runState, this);
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0005C560 File Offset: 0x0005A760
		public override async Task Exit([Nullable(2)] IRunState runState)
		{
			RunManager.Instance.RestSiteSynchronizer.BeforeLocalRestSiteExited();
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			if (instance != null)
			{
				instance.BeforeExitingRoom();
			}
			await RunManager.Instance.RestSiteSynchronizer.AfterAllRestSitesCompleted();
			RunManager.Instance.ChecksumTracker.GenerateChecksum("Exiting rest site room", null);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0005C59B File Offset: 0x0005A79B
		public override Task Resume(AbstractRoom _, [Nullable(2)] IRunState runState)
		{
			this.ShowRoomNode(runState);
			return Task.CompletedTask;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0005C5A9 File Offset: 0x0005A7A9
		private void ShowRoomNode(IRunState runState)
		{
			NRun instance = NRun.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetCurrentRoom(NRestSiteRoom.Create(this, runState));
		}

		// Token: 0x0400054B RID: 1355
		[Nullable(2)]
		private RestSiteSynchronizer _synchronizer;
	}
}
