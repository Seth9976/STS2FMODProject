using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CFB RID: 3323
	[NullableContext(1)]
	[Nullable(0)]
	public class PotionConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024F7 RID: 9463
		// (get) Token: 0x0600864D RID: 34381 RVA: 0x002D124F File Offset: 0x002CF44F
		public override string CmdName
		{
			get
			{
				return "potion";
			}
		}

		// Token: 0x170024F8 RID: 9464
		// (get) Token: 0x0600864E RID: 34382 RVA: 0x002D1256 File Offset: 0x002CF456
		public override string Args
		{
			get
			{
				return "<id:string>";
			}
		}

		// Token: 0x170024F9 RID: 9465
		// (get) Token: 0x0600864F RID: 34383 RVA: 0x002D125D File Offset: 0x002CF45D
		public override string Description
		{
			get
			{
				return "Adds potion to belt. Screaming snake case ('ENTROPIC_BREW', not 'Entropic Brew').";
			}
		}

		// Token: 0x170024FA RID: 9466
		// (get) Token: 0x06008650 RID: 34384 RVA: 0x002D1264 File Offset: 0x002CF464
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008651 RID: 34385 RVA: 0x002D1268 File Offset: 0x002CF468
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1)
			{
				return new CmdResult(false, this.CmdName + " requires a potion name");
			}
			if (!RunManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "A run is not in progress.");
			}
			string potionId = args[0].ToUpperInvariant();
			PotionModel potionModel = ModelDb.AllPotions.FirstOrDefault<PotionModel>((PotionModel p) => p.Id.Entry == potionId);
			if (potionModel == null)
			{
				return new CmdResult(false, "Potion '" + potionId + "' not found");
			}
			PotionModel potionModel2 = potionModel.ToMutable();
			Task task = PotionCmd.TryToProcure(potionModel2, issuingPlayer, -1);
			return new CmdResult(task, true, "Added potion " + potionModel.Id.Entry);
		}

		// Token: 0x06008652 RID: 34386 RVA: 0x002D1320 File Offset: 0x002CF520
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ModelDb.AllPotions.Select<PotionModel, string>((PotionModel p) => p.Id.Entry).ToList<string>();
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
