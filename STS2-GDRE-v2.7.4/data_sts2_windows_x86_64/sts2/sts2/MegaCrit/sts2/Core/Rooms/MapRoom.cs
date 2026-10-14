using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x0200012C RID: 300
	[NullableContext(1)]
	[Nullable(0)]
	public class MapRoom : AbstractRoom
	{
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0005C164 File Offset: 0x0005A364
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Map;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0005C167 File Offset: 0x0005A367
		[Nullable(2)]
		public override ModelId ModelId
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0005C16C File Offset: 0x0005A36C
		public override Task EnterInternal([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase)
		{
			if (isRestoringRoomStackBase)
			{
				throw new InvalidOperationException("MapRoom does not support room stack reconstruction.");
			}
			if (TestMode.IsOn)
			{
				return Task.CompletedTask;
			}
			NMapRoom nmapRoom = NMapRoom.Create(((runState != null) ? runState.Act : null) ?? ModelDb.Act<Overgrowth>(), (runState != null) ? runState.CurrentActIndex : 0);
			NRun.Instance.SetCurrentRoom(nmapRoom);
			return Task.CompletedTask;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0005C1CB File Offset: 0x0005A3CB
		public override Task Exit([Nullable(2)] IRunState runState)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0005C1D2 File Offset: 0x0005A3D2
		public override Task Resume(AbstractRoom _, [Nullable(2)] IRunState runState)
		{
			throw new NotImplementedException();
		}
	}
}
