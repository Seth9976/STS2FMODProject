using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE2 RID: 3298
	[NullableContext(1)]
	[Nullable(0)]
	public class AncientConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x1700248C RID: 9356
		// (get) Token: 0x0600858E RID: 34190 RVA: 0x002CD988 File Offset: 0x002CBB88
		public override string CmdName
		{
			get
			{
				return "ancient";
			}
		}

		// Token: 0x1700248D RID: 9357
		// (get) Token: 0x0600858F RID: 34191 RVA: 0x002CD98F File Offset: 0x002CBB8F
		public override string Args
		{
			get
			{
				return "<id:string> <choice:string>";
			}
		}

		// Token: 0x1700248E RID: 9358
		// (get) Token: 0x06008590 RID: 34192 RVA: 0x002CD996 File Offset: 0x002CBB96
		public override string Description
		{
			get
			{
				return "Opens an ancient event with the selected choice";
			}
		}

		// Token: 0x1700248F RID: 9359
		// (get) Token: 0x06008591 RID: 34193 RVA: 0x002CD99D File Offset: 0x002CBB9D
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008592 RID: 34194 RVA: 0x002CD9A0 File Offset: 0x002CBBA0
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			AncientConsoleCmd.<>c__DisplayClass8_0 CS$<>8__locals1 = new AncientConsoleCmd.<>c__DisplayClass8_0();
			if (args.Length == 0)
			{
				return new CmdResult(false, "No ancient ID specified.");
			}
			ModelId modelId = new ModelId(ModelDb.GetCategory(typeof(EventModel)), args[0].ToUpperInvariant());
			EventModel byIdOrNull = ModelDb.GetByIdOrNull<EventModel>(modelId);
			AncientEventModel ancientEventModel = byIdOrNull as AncientEventModel;
			if (ancientEventModel == null)
			{
				return new CmdResult(false, "Invalid ancient ID.");
			}
			CS$<>8__locals1.choice = null;
			if (args.Length > 1)
			{
				CS$<>8__locals1.choice = args[1].ToUpperInvariant();
			}
			if (CS$<>8__locals1.choice != null && !ancientEventModel.AllPossibleOptions.Any<EventOption>((EventOption option) => option.TextKey.Contains(CS$<>8__locals1.choice)))
			{
				return new CmdResult(false, "invalid ancient choice.");
			}
			EventRoom eventRoom = new EventRoom(byIdOrNull)
			{
				OnStart = new Action<EventModel>(CS$<>8__locals1.<Process>g__SetDebugOption|1)
			};
			issuingPlayer.RunState.AppendToMapPointHistory(MapPointType.Ancient, RoomType.Event, byIdOrNull.Id);
			Task task = RunManager.Instance.EnterRoom(eventRoom);
			return new CmdResult(task, true, "Opened Ancient Event. Forced " + (CS$<>8__locals1.choice ?? "no") + " option");
		}

		// Token: 0x06008593 RID: 34195 RVA: 0x002CDAA8 File Offset: 0x002CBCA8
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ModelDb.AllAncients.Select<AncientEventModel, string>((AncientEventModel ancient) => ancient.Id.Entry).ToList<string>();
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			if (args.Length == 2)
			{
				ModelId modelId = new ModelId(ModelDb.GetCategory(typeof(EventModel)), args[0].ToUpperInvariant());
				EventModel byIdOrNull = ModelDb.GetByIdOrNull<EventModel>(modelId);
				AncientEventModel ancientEventModel = byIdOrNull as AncientEventModel;
				if (ancientEventModel != null)
				{
					List<string> list2 = ancientEventModel.AllPossibleOptions.Select<EventOption, string>((EventOption option) => option.TextKey.Split('.', StringSplitOptions.None).Last<string>()).ToList<string>();
					return base.CompleteArgument(list2, new string[] { args[0] }, args[1], CompletionType.Argument, null);
				}
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}
	}
}
