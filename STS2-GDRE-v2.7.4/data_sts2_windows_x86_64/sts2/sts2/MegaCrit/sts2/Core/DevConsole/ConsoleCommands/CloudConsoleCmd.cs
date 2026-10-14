using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE8 RID: 3304
	[NullableContext(1)]
	[Nullable(0)]
	public class CloudConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024A6 RID: 9382
		// (get) Token: 0x060085BA RID: 34234 RVA: 0x002CE91A File Offset: 0x002CCB1A
		public override string CmdName
		{
			get
			{
				return "cloud";
			}
		}

		// Token: 0x170024A7 RID: 9383
		// (get) Token: 0x060085BB RID: 34235 RVA: 0x002CE921 File Offset: 0x002CCB21
		public override string Args
		{
			get
			{
				return "delete";
			}
		}

		// Token: 0x170024A8 RID: 9384
		// (get) Token: 0x060085BC RID: 34236 RVA: 0x002CE928 File Offset: 0x002CCB28
		public override string Description
		{
			get
			{
				return "Deletes all save files from Steam Cloud, if you are running on Steam";
			}
		}

		// Token: 0x170024A9 RID: 9385
		// (get) Token: 0x060085BD RID: 34237 RVA: 0x002CE92F File Offset: 0x002CCB2F
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170024AA RID: 9386
		// (get) Token: 0x060085BE RID: 34238 RVA: 0x002CE932 File Offset: 0x002CCB32
		public override bool DebugOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060085BF RID: 34239 RVA: 0x002CE938 File Offset: 0x002CCB38
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1 || args[0] != "delete")
			{
				return new CmdResult(false, "No argument specified.\n" + this.Args);
			}
			if (!CloudConsoleCmd._confirmed)
			{
				CloudConsoleCmd._confirmed = true;
				return new CmdResult(false, "Run this command again to confirm you want to delete all your Steam cloud saves. The game will quit.");
			}
			CloudConsoleCmd.DeleteCloudSaves();
			return new CmdResult(true, "Steam cloud saves deleted.");
		}

		// Token: 0x060085C0 RID: 34240 RVA: 0x002CE99C File Offset: 0x002CCB9C
		public static void DeleteCloudSaves()
		{
			if (!SteamInitializer.Initialized)
			{
				throw new InvalidOperationException("Steam not initialized");
			}
			int fileCount = SteamRemoteStorage.GetFileCount();
			for (int i = fileCount - 1; i >= 0; i--)
			{
				int num;
				string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(i, ref num);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Deleting ");
				defaultInterpolatedStringHandler.AppendFormatted(fileNameAndSize);
				defaultInterpolatedStringHandler.AppendLiteral(" from Steam cloud (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes)");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SteamRemoteStorage.FileDelete(fileNameAndSize);
			}
			((SceneTree)Engine.GetMainLoop()).Quit(0);
		}

		// Token: 0x04002E64 RID: 11876
		private static bool _confirmed;
	}
}
