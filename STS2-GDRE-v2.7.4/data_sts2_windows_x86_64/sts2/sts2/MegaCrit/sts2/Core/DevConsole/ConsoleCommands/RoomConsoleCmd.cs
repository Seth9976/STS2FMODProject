using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CFE RID: 3326
	[NullableContext(1)]
	[Nullable(0)]
	public class RoomConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002503 RID: 9475
		// (get) Token: 0x06008663 RID: 34403 RVA: 0x002D1A3B File Offset: 0x002CFC3B
		public override string CmdName
		{
			get
			{
				return "room";
			}
		}

		// Token: 0x17002504 RID: 9476
		// (get) Token: 0x06008664 RID: 34404 RVA: 0x002D1A42 File Offset: 0x002CFC42
		public override string Args
		{
			get
			{
				return "<id:string>";
			}
		}

		// Token: 0x17002505 RID: 9477
		// (get) Token: 0x06008665 RID: 34405 RVA: 0x002D1A49 File Offset: 0x002CFC49
		public override string Description
		{
			get
			{
				return "Jumps a player to a specific room.";
			}
		}

		// Token: 0x17002506 RID: 9478
		// (get) Token: 0x06008666 RID: 34406 RVA: 0x002D1A50 File Offset: 0x002CFC50
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008667 RID: 34407 RVA: 0x002D1A54 File Offset: 0x002CFC54
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "No room name specified.");
			}
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run is currently not in progress!");
			}
			string text = args[0].ToUpperInvariant();
			RoomType roomType;
			if (!AbstractConsoleCmd.TryParseEnum<RoomType>(text, out roomType))
			{
				return new CmdResult(false, "Room '" + text + "' not found");
			}
			Task task = RunManager.Instance.EnterRoomDebug(roomType, MapPointType.Unassigned, null, true);
			Task task2 = task;
			bool flag = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Jumped to room: '");
			defaultInterpolatedStringHandler.AppendFormatted<RoomType>(roomType);
			defaultInterpolatedStringHandler.AppendLiteral("'");
			return new CmdResult(task2, flag, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06008668 RID: 34408 RVA: 0x002D1AFC File Offset: 0x002CFCFC
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = (from n in Enum.GetNames(typeof(RoomType))
					where !n.Equals("Unassigned")
					select n).ToList<string>();
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}
	}
}
