using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;

namespace MegaCrit.Sts2.Core.Extensions
{
	// Token: 0x02000C48 RID: 3144
	public static class SignalAwaiterExtensions
	{
		// Token: 0x06008197 RID: 33175 RVA: 0x002BF240 File Offset: 0x002BD440
		[NullableContext(1)]
		public static async Task ToTask(this SignalAwaiter awaiter)
		{
			await awaiter;
		}
	}
}
