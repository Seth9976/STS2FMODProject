using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Implement this on a node under NCreature to stop the NCreature from being freed until the task returned from GetDelayTask is complete
	/// </summary>
	// Token: 0x0200017D RID: 381
	[NullableContext(1)]
	public interface IDeathDelayer
	{
		// Token: 0x0600125C RID: 4700
		Task GetDelayTask();
	}
}
