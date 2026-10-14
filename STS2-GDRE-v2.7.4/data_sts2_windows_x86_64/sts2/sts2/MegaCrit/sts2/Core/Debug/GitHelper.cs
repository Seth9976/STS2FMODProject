using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D0C RID: 3340
	[NullableContext(2)]
	[Nullable(0)]
	public static class GitHelper
	{
		// Token: 0x17002527 RID: 9511
		// (get) Token: 0x060086BB RID: 34491 RVA: 0x002D366F File Offset: 0x002D186F
		// (set) Token: 0x060086BC RID: 34492 RVA: 0x002D3676 File Offset: 0x002D1876
		public static Task<string> ShortCommitIdTask { get; private set; }

		// Token: 0x17002528 RID: 9512
		// (get) Token: 0x060086BD RID: 34493 RVA: 0x002D367E File Offset: 0x002D187E
		// (set) Token: 0x060086BE RID: 34494 RVA: 0x002D3685 File Offset: 0x002D1885
		public static string ShortCommitId { get; private set; }

		// Token: 0x060086BF RID: 34495 RVA: 0x002D3690 File Offset: 0x002D1890
		[NullableContext(1)]
		public static async Task Initialize()
		{
			Func<string> func;
			if ((func = GitHelper.<>O.<0>__GetCommitId) == null)
			{
				func = (GitHelper.<>O.<0>__GetCommitId = new Func<string>(GitHelper.GetCommitId));
			}
			GitHelper.ShortCommitIdTask = Task.Run<string>(func);
			string text = await GitHelper.ShortCommitIdTask;
			GitHelper.ShortCommitId = text;
		}

		// Token: 0x060086C0 RID: 34496 RVA: 0x002D36CC File Offset: 0x002D18CC
		private static string GetCommitId()
		{
			if (!OS.HasFeature("editor"))
			{
				return null;
			}
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				FileName = "git",
				Arguments = "rev-parse --short HEAD",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};
			string text;
			using (Process process = Process.Start(processStartInfo))
			{
				if (process == null)
				{
					Log.Error("Error: Unable to start git process to get the git commit id.", 2);
					text = null;
				}
				else
				{
					string text2 = process.StandardOutput.ReadToEnd();
					process.WaitForExit();
					text = text2.Trim();
				}
			}
			return text;
		}

		// Token: 0x0200240D RID: 9229
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04009857 RID: 38999
			[Nullable(0)]
			public static Func<string> <0>__GetCommitId;
		}
	}
}
