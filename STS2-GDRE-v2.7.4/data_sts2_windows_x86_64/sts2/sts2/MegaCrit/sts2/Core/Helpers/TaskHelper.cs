using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C18 RID: 3096
	[NullableContext(1)]
	[Nullable(0)]
	public static class TaskHelper
	{
		/// <summary>
		/// Runs a task without awaiting it.
		/// Prefer using this over calling the task-returning method and then discarding the task, as that causes exceptions
		/// not to be logged.
		/// </summary>
		// Token: 0x06007FF6 RID: 32758 RVA: 0x002B736A File Offset: 0x002B556A
		public static Task RunSafely(Task task)
		{
			return TaskHelper.LogTaskExceptions(task);
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x002B7374 File Offset: 0x002B5574
		private static async Task LogTaskExceptions(Task task)
		{
			try
			{
				await task;
			}
			catch (Exception ex)
			{
				if (!(ex is OperationCanceledException))
				{
					Log.Error(ex.ToString(), 2);
					SentryService.CaptureException(ex);
				}
				throw;
			}
		}

		/// <summary>
		/// Runs all the tasks at once and returns a task whose result is equivalent to the first task completed.
		/// Prefer using this over Task.WhenAny, as that returns a Task that is always successful.
		/// </summary>
		/// <param name="tasks"></param>
		/// <returns></returns>
		// Token: 0x06007FF8 RID: 32760 RVA: 0x002B73B8 File Offset: 0x002B55B8
		public static async Task WhenAny(params Task[] tasks)
		{
			Task task = await Task.WhenAny(tasks);
			await task;
		}
	}
}
