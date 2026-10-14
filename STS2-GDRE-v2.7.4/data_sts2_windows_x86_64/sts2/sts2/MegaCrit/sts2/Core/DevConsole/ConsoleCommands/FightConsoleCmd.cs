using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CF0 RID: 3312
	[NullableContext(1)]
	[Nullable(0)]
	public class FightConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024C8 RID: 9416
		// (get) Token: 0x060085F1 RID: 34289 RVA: 0x002CF291 File Offset: 0x002CD491
		public override string CmdName
		{
			get
			{
				return "fight";
			}
		}

		// Token: 0x170024C9 RID: 9417
		// (get) Token: 0x060085F2 RID: 34290 RVA: 0x002CF298 File Offset: 0x002CD498
		public override string Args
		{
			get
			{
				return "<id:string>";
			}
		}

		// Token: 0x170024CA RID: 9418
		// (get) Token: 0x060085F3 RID: 34291 RVA: 0x002CF29F File Offset: 0x002CD49F
		public override string Description
		{
			get
			{
				return "Jumps a player to a specific encounter.";
			}
		}

		// Token: 0x170024CB RID: 9419
		// (get) Token: 0x060085F4 RID: 34292 RVA: 0x002CF2A6 File Offset: 0x002CD4A6
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085F5 RID: 34293 RVA: 0x002CF2AC File Offset: 0x002CD4AC
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "No encounter name specified.");
			}
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run is currently not in progress!");
			}
			ModelId modelId = new ModelId(ModelId.SlugifyCategory<EncounterModel>(), args[0].ToUpperInvariant());
			EncounterModel encounterModel;
			try
			{
				encounterModel = ModelDb.GetById<EncounterModel>(modelId).ToMutable();
			}
			catch (ModelNotFoundException)
			{
				return new CmdResult(false, "Encounter '" + modelId.Entry + "' not found");
			}
			encounterModel.DebugRandomizeRng();
			Task task = RunManager.Instance.EnterRoomDebug(RoomType.Monster, MapPointType.Unassigned, encounterModel, true);
			return new CmdResult(task, true, "Jumped to encounter: '" + encounterModel.Id.Entry + "'");
		}

		// Token: 0x060085F6 RID: 34294 RVA: 0x002CF36C File Offset: 0x002CD56C
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ModelDb.AllEncounters.Select<EncounterModel, string>((EncounterModel e) => e.Id.Entry).ToList<string>();
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
