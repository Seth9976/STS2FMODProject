using System;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.AutoSlay.Helpers
{
	/// <summary>
	/// Watchdog that detects when AutoSlay gets stuck and dumps diagnostic state.
	/// </summary>
	// Token: 0x02000D71 RID: 3441
	[NullableContext(1)]
	[Nullable(0)]
	public class Watchdog
	{
		// Token: 0x06008B06 RID: 35590 RVA: 0x002E70F3 File Offset: 0x002E52F3
		public Watchdog()
		{
			this.Reset("Initialized");
		}

		/// <summary>
		/// Resets the watchdog timer. Call this whenever meaningful progress is made.
		/// </summary>
		// Token: 0x06008B07 RID: 35591 RVA: 0x002E7111 File Offset: 0x002E5311
		public void Reset(string activity)
		{
			this._lastProgressTime = DateTime.UtcNow;
			this._lastActivity = activity;
		}

		/// <summary>
		/// Checks if the watchdog has timed out. Call this periodically (e.g., in WaitHelper).
		/// Throws TimeoutException if no progress for too long.
		/// </summary>
		// Token: 0x06008B08 RID: 35592 RVA: 0x002E7128 File Offset: 0x002E5328
		public void Check()
		{
			TimeSpan timeSpan = DateTime.UtcNow - this._lastProgressTime;
			if (timeSpan > AutoSlayConfig.watchdogLogInterval && DateTime.UtcNow - this._lastLogTime > AutoSlayConfig.watchdogLogInterval)
			{
				this._lastLogTime = DateTime.UtcNow;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Watchdog] No progress for ");
				defaultInterpolatedStringHandler.AppendFormatted<double>(timeSpan.TotalSeconds, "F1");
				defaultInterpolatedStringHandler.AppendLiteral("s (last: ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lastActivity);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (timeSpan > AutoSlayConfig.watchdogTimeout)
			{
				string text = Watchdog.DumpState();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
				defaultInterpolatedStringHandler.AppendLiteral("[Watchdog] Stuck detected! No progress for ");
				defaultInterpolatedStringHandler.AppendFormatted<double>(timeSpan.TotalSeconds, "F1");
				defaultInterpolatedStringHandler.AppendLiteral("s\n");
				defaultInterpolatedStringHandler.AppendLiteral("Last activity: ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lastActivity);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				AutoSlayLog.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Watchdog timeout: No progress for ");
				defaultInterpolatedStringHandler.AppendFormatted<double>(timeSpan.TotalSeconds, "F1");
				defaultInterpolatedStringHandler.AppendLiteral("s. Last activity: ");
				defaultInterpolatedStringHandler.AppendFormatted(this._lastActivity);
				throw new AutoSlayTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		/// <summary>
		/// Dumps the current game state for debugging.
		/// </summary>
		// Token: 0x06008B09 RID: 35593 RVA: 0x002E72AC File Offset: 0x002E54AC
		public static string DumpState()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=== AutoSlay State Dump ===");
			try
			{
				RunState runState = RunManager.Instance.DebugOnlyGetState();
				if (runState != null)
				{
					stringBuilder.AppendLine("Run State:");
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(24, 3, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("  Floor: ");
					appendInterpolatedStringHandler.AppendFormatted<int>(runState.TotalFloor);
					appendInterpolatedStringHandler.AppendLiteral(" (Act ");
					appendInterpolatedStringHandler.AppendFormatted<int>(runState.CurrentActIndex + 1);
					appendInterpolatedStringHandler.AppendLiteral(", Floor ");
					appendInterpolatedStringHandler.AppendFormatted<int>(runState.ActFloor);
					appendInterpolatedStringHandler.AppendLiteral(")");
					stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder4 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(8, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("  Room: ");
					AbstractRoom currentRoom = runState.CurrentRoom;
					appendInterpolatedStringHandler.AppendFormatted(((currentRoom != null) ? currentRoom.RoomType.ToString() : null) ?? "null");
					stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
				}
				else
				{
					stringBuilder.AppendLine("Run State: Not initialized");
				}
				if (NOverlayStack.Instance != null)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder5 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("Overlay Stack: ");
					appendInterpolatedStringHandler.AppendFormatted<int>(NOverlayStack.Instance.ScreenCount);
					appendInterpolatedStringHandler.AppendLiteral(" screens");
					stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
					IOverlayScreen overlayScreen = NOverlayStack.Instance.Peek();
					if (overlayScreen != null)
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder6 = stringBuilder2;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
						appendInterpolatedStringHandler.AppendLiteral("  Current: ");
						appendInterpolatedStringHandler.AppendFormatted(overlayScreen.GetType().Name);
						stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
					}
				}
				Node instance = NGame.Instance;
				if (instance != null)
				{
					stringBuilder.AppendLine("Scene Container:");
					Node nodeOrNull = instance.GetNodeOrNull("RootSceneContainer");
					if (nodeOrNull != null)
					{
						foreach (Node node in nodeOrNull.GetChildren(false))
						{
							Control control = node as Control;
							string text = ((control != null) ? (control.Visible ? "visible" : "hidden") : "");
							StringBuilder stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder7 = stringBuilder2;
							StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(10, 3, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral("  - ");
							appendInterpolatedStringHandler.AppendFormatted<StringName>(node.Name);
							appendInterpolatedStringHandler.AppendLiteral(" (");
							appendInterpolatedStringHandler.AppendFormatted(node.GetType().Name);
							appendInterpolatedStringHandler.AppendLiteral(") [");
							appendInterpolatedStringHandler.AppendFormatted(text);
							appendInterpolatedStringHandler.AppendLiteral("]");
							stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
						}
					}
				}
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder8 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Error dumping state: ");
				appendInterpolatedStringHandler.AppendFormatted(ex.Message);
				stringBuilder8.AppendLine(ref appendInterpolatedStringHandler);
			}
			stringBuilder.AppendLine("=== End State Dump ===");
			return stringBuilder.ToString();
		}

		// Token: 0x0400303E RID: 12350
		private DateTime _lastProgressTime;

		// Token: 0x0400303F RID: 12351
		private DateTime _lastLogTime;

		// Token: 0x04003040 RID: 12352
		private string _lastActivity = "Starting";
	}
}
