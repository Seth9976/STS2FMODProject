using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.AutoSlay.Helpers
{
	/// <summary>
	/// Async waiting utilities for AutoSlay.
	/// </summary>
	// Token: 0x02000D70 RID: 3440
	[NullableContext(1)]
	[Nullable(0)]
	public static class WaitHelper
	{
		/// <summary>
		/// Waits until a condition becomes true, with timeout.
		/// Also checks the watchdog periodically to detect stuck states.
		/// </summary>
		// Token: 0x06008B00 RID: 35584 RVA: 0x002E6AA4 File Offset: 0x002E4CA4
		public static async Task Until(Func<bool> condition, CancellationToken ct, TimeSpan? timeout = null, [Nullable(2)] string timeoutMessage = null)
		{
			TimeSpan actualTimeout = timeout ?? AutoSlayConfig.nodeWaitTimeout;
			using (CancellationTokenSource timeoutCts = new CancellationTokenSource(actualTimeout))
			{
				using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token))
				{
					try
					{
						while (!condition())
						{
							Watchdog currentWatchdog = AutoSlayer.CurrentWatchdog;
							if (currentWatchdog != null)
							{
								currentWatchdog.Check();
							}
							await Task.Delay(AutoSlayConfig.pollingInterval, linkedCts.Token);
						}
					}
					catch (OperationCanceledException obj) when (timeoutCts.IsCancellationRequested)
					{
						string text = timeoutMessage;
						if (timeoutMessage == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Condition not met after ");
							defaultInterpolatedStringHandler.AppendFormatted<double>(actualTimeout.TotalSeconds);
							defaultInterpolatedStringHandler.AppendLiteral("s");
							text = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						throw new AutoSlayTimeoutException(text);
					}
				}
			}
		}

		/// <summary>
		/// Waits for a node to exist at the given path.
		/// </summary>
		// Token: 0x06008B01 RID: 35585 RVA: 0x002E6B00 File Offset: 0x002E4D00
		public static Task<T> ForNode<[Nullable(0)] T>(Node root, string nodePath, CancellationToken ct, TimeSpan? timeout = null) where T : Node
		{
			WaitHelper.<ForNode>d__1<T> <ForNode>d__;
			<ForNode>d__.<>t__builder = AsyncTaskMethodBuilder<T>.Create();
			<ForNode>d__.root = root;
			<ForNode>d__.nodePath = nodePath;
			<ForNode>d__.ct = ct;
			<ForNode>d__.timeout = timeout;
			<ForNode>d__.<>1__state = -1;
			<ForNode>d__.<>t__builder.Start<WaitHelper.<ForNode>d__1<T>>(ref <ForNode>d__);
			return <ForNode>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Dumps scene tree context when a wait times out, to help debug what's actually there.
		/// </summary>
		// Token: 0x06008B02 RID: 35586 RVA: 0x002E6B5C File Offset: 0x002E4D5C
		private static void DumpSceneTreeContext(Node root, string nodePath, string reason)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(35, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("[AutoSlay] Scene tree dump (node ");
			appendInterpolatedStringHandler.AppendFormatted(reason);
			appendInterpolatedStringHandler.AppendLiteral("):");
			stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder4 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("  Looking for: ");
			appendInterpolatedStringHandler.AppendFormatted(nodePath);
			stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder5 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 2, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("  Root: ");
			appendInterpolatedStringHandler.AppendFormatted<StringName>(root.Name);
			appendInterpolatedStringHandler.AppendLiteral(" (");
			appendInterpolatedStringHandler.AppendFormatted(root.GetType().Name);
			appendInterpolatedStringHandler.AppendLiteral(")");
			stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			int num = nodePath.LastIndexOf('/');
			string text = ((num > 0) ? nodePath.Substring(0, num) : "");
			string text2;
			if (num <= 0)
			{
				text2 = nodePath;
			}
			else
			{
				int num2 = num + 1;
				text2 = nodePath.Substring(num2, nodePath.Length - num2);
			}
			string text3 = text2;
			Node node = (string.IsNullOrEmpty(text) ? root : root.GetNodeOrNull(text));
			if (node == null)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder6 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(32, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  Parent path '");
				appendInterpolatedStringHandler.AppendFormatted(text);
				appendInterpolatedStringHandler.AppendLiteral("' does not exist!");
				stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
				string[] array = text.Split('/', StringSplitOptions.None);
				Node node2 = root;
				string text4 = "";
				foreach (string text5 in array)
				{
					if (!string.IsNullOrEmpty(text5))
					{
						text4 = text4 + ((text4.Length > 0) ? "/" : "") + text5;
						Node nodeOrNull = node2.GetNodeOrNull(text5);
						if (nodeOrNull == null)
						{
							stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder7 = stringBuilder2;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(18, 1, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral("  Path broken at: ");
							appendInterpolatedStringHandler.AppendFormatted(text4);
							stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
							stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder8 = stringBuilder2;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 1, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral("  Children of ");
							appendInterpolatedStringHandler.AppendFormatted<StringName>(node2.Name);
							appendInterpolatedStringHandler.AppendLiteral(":");
							stringBuilder8.AppendLine(ref appendInterpolatedStringHandler);
							WaitHelper.DumpChildren(node2, stringBuilder, 2, 2, "    ");
							break;
						}
						node2 = nodeOrNull;
					}
				}
			}
			else
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder9 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(13, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  Parent: ");
				appendInterpolatedStringHandler.AppendFormatted<StringName>(node.Name);
				appendInterpolatedStringHandler.AppendLiteral(" (");
				appendInterpolatedStringHandler.AppendFormatted(node.GetType().Name);
				appendInterpolatedStringHandler.AppendLiteral(")");
				stringBuilder9.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder10 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  Looking for child: ");
				appendInterpolatedStringHandler.AppendFormatted(text3);
				stringBuilder10.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder11 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("  Children of ");
				appendInterpolatedStringHandler.AppendFormatted<StringName>(node.Name);
				appendInterpolatedStringHandler.AppendLiteral(":");
				stringBuilder11.AppendLine(ref appendInterpolatedStringHandler);
				WaitHelper.DumpChildren(node, stringBuilder, 2, 3, "    ");
			}
			AutoSlayLog.Info(stringBuilder.ToString().TrimEnd());
		}

		// Token: 0x06008B03 RID: 35587 RVA: 0x002E6EBC File Offset: 0x002E50BC
		private static void DumpChildren(Node node, StringBuilder sb, int depth, int maxDepth, string indent = "    ")
		{
			foreach (Node node2 in node.GetChildren(false))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<StringName>(node2.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(node2.GetType().Name);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				Control control = node2 as Control;
				if (control != null)
				{
					text += (control.Visible ? " [visible]" : " [hidden]");
				}
				NButton nbutton = node2 as NButton;
				if (nbutton != null)
				{
					text += (nbutton.IsEnabled ? " [enabled]" : " [disabled]");
				}
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 2, sb);
				appendInterpolatedStringHandler.AppendFormatted(indent);
				appendInterpolatedStringHandler.AppendFormatted(text);
				sb.AppendLine(ref appendInterpolatedStringHandler);
				if (depth < maxDepth)
				{
					WaitHelper.DumpChildren(node2, sb, depth + 1, maxDepth, indent + "  ");
				}
				else if (node2.GetChildCount(false) > 0)
				{
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 2, sb);
					appendInterpolatedStringHandler.AppendFormatted(indent);
					appendInterpolatedStringHandler.AppendLiteral("  ... (");
					appendInterpolatedStringHandler.AppendFormatted<int>(node2.GetChildCount(false));
					appendInterpolatedStringHandler.AppendLiteral(" children)");
					sb.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
		}

		/// <summary>
		/// Waits for a task to complete while periodically checking the watchdog.
		/// </summary>
		// Token: 0x06008B04 RID: 35588 RVA: 0x002E7044 File Offset: 0x002E5244
		public static async Task ForTask(Task task, CancellationToken ct, TimeSpan? timeout = null, [Nullable(2)] string timeoutMessage = null)
		{
			TimeSpan actualTimeout = timeout ?? AutoSlayConfig.nodeWaitTimeout;
			using (CancellationTokenSource timeoutCts = new CancellationTokenSource(actualTimeout))
			{
				using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token))
				{
					try
					{
						while (!task.IsCompleted)
						{
							Watchdog currentWatchdog = AutoSlayer.CurrentWatchdog;
							if (currentWatchdog != null)
							{
								currentWatchdog.Check();
							}
							Task task2 = Task.Delay(AutoSlayConfig.pollingInterval, linkedCts.Token);
							await Task.WhenAny(task, task2);
						}
						await task;
					}
					catch (OperationCanceledException obj) when (timeoutCts.IsCancellationRequested)
					{
						string text = timeoutMessage;
						if (timeoutMessage == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Task not completed after ");
							defaultInterpolatedStringHandler.AppendFormatted<double>(actualTimeout.TotalSeconds);
							defaultInterpolatedStringHandler.AppendLiteral("s");
							text = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						throw new AutoSlayTimeoutException(text);
					}
				}
			}
		}

		/// <summary>
		/// Runs a task with a timeout, combining with an existing cancellation token.
		/// </summary>
		// Token: 0x06008B05 RID: 35589 RVA: 0x002E70A0 File Offset: 0x002E52A0
		public static async Task WithTimeout(Func<CancellationToken, Task> action, TimeSpan timeout, CancellationToken ct)
		{
			using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
			{
				Task task = action(linkedCts.Token);
				Task task2 = Task.Delay(timeout, linkedCts.Token);
				Task task3 = await Task.WhenAny(task, task2);
				Task completedTask = task3;
				await linkedCts.CancelAsync();
				if (completedTask != task && !ct.IsCancellationRequested)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Operation timed out after ");
					defaultInterpolatedStringHandler.AppendFormatted<double>(timeout.TotalSeconds);
					defaultInterpolatedStringHandler.AppendLiteral("s");
					throw new AutoSlayTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				await task;
			}
		}
	}
}
