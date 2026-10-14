using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CEF RID: 3311
	[NullableContext(1)]
	[Nullable(0)]
	public class EventConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024C3 RID: 9411
		// (get) Token: 0x060085E9 RID: 34281 RVA: 0x002CF111 File Offset: 0x002CD311
		public override string CmdName
		{
			get
			{
				return "event";
			}
		}

		// Token: 0x170024C4 RID: 9412
		// (get) Token: 0x060085EA RID: 34282 RVA: 0x002CF118 File Offset: 0x002CD318
		public override string Args
		{
			get
			{
				return "<id:string>";
			}
		}

		// Token: 0x170024C5 RID: 9413
		// (get) Token: 0x060085EB RID: 34283 RVA: 0x002CF11F File Offset: 0x002CD31F
		public override string Description
		{
			get
			{
				return "Jumps a player to a specific event.";
			}
		}

		// Token: 0x170024C6 RID: 9414
		// (get) Token: 0x060085EC RID: 34284 RVA: 0x002CF126 File Offset: 0x002CD326
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085ED RID: 34285 RVA: 0x002CF12C File Offset: 0x002CD32C
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "No event name specified.");
			}
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run is currently not in progress!");
			}
			string eventName = args[0].ToUpperInvariant();
			EventModel eventModel = EventConsoleCmd.Events.FirstOrDefault<EventModel>((EventModel c) => c.Id.Entry == eventName);
			if (eventModel == null)
			{
				return new CmdResult(false, "Event '" + eventName + "' not found");
			}
			MapPointType mapPointType = ((eventModel is AncientEventModel) ? MapPointType.Ancient : MapPointType.Unknown);
			issuingPlayer.RunState.AppendToMapPointHistory(mapPointType, RoomType.Event, eventModel.Id);
			Task task = RunManager.Instance.EnterRoom(new EventRoom(eventModel));
			return new CmdResult(task, true, "Jumped to event: '" + eventModel.Id.Entry + "'");
		}

		// Token: 0x060085EE RID: 34286 RVA: 0x002CF200 File Offset: 0x002CD400
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = EventConsoleCmd.Events.Select<EventModel, string>((EventModel e) => e.Id.Entry).ToList<string>();
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}

		// Token: 0x170024C7 RID: 9415
		// (get) Token: 0x060085EF RID: 34287 RVA: 0x002CF278 File Offset: 0x002CD478
		private static IEnumerable<EventModel> Events
		{
			get
			{
				return ModelDb.AllEvents.Concat<EventModel>(ModelDb.AllAncients);
			}
		}
	}
}
