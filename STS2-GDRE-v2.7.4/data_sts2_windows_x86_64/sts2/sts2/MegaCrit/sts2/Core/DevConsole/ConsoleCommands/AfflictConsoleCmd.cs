using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE1 RID: 3297
	[NullableContext(1)]
	[Nullable(0)]
	public class AfflictConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002488 RID: 9352
		// (get) Token: 0x06008587 RID: 34183 RVA: 0x002CD70F File Offset: 0x002CB90F
		public override string CmdName
		{
			get
			{
				return "afflict";
			}
		}

		// Token: 0x17002489 RID: 9353
		// (get) Token: 0x06008588 RID: 34184 RVA: 0x002CD716 File Offset: 0x002CB916
		public override string Args
		{
			get
			{
				return "<id:string> [amount:int] [hand-index:int]";
			}
		}

		// Token: 0x1700248A RID: 9354
		// (get) Token: 0x06008589 RID: 34185 RVA: 0x002CD71D File Offset: 0x002CB91D
		public override string Description
		{
			get
			{
				return "Apply the specified affliction to a card in the player's hand";
			}
		}

		// Token: 0x1700248B RID: 9355
		// (get) Token: 0x0600858A RID: 34186 RVA: 0x002CD724 File Offset: 0x002CB924
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600858B RID: 34187 RVA: 0x002CD728 File Offset: 0x002CB928
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "Must specify an affliction ID!");
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "Combat is not currently in progress!");
			}
			ModelId modelId = new ModelId(ModelId.SlugifyCategory<AfflictionModel>(), args[0].ToUpperInvariant());
			AfflictionModel afflictionModel;
			try
			{
				afflictionModel = ModelDb.GetById<AfflictionModel>(modelId).ToMutable();
			}
			catch (ModelNotFoundException)
			{
				return new CmdResult(false, "Affliction '" + modelId.Entry + "' not found");
			}
			int num = 0;
			int num2 = 0;
			if (args.Length > 1 && !int.TryParse(args[1], out num))
			{
				return new CmdResult(false, "Arg 2 must be the affliction count (int), got '" + args[1] + "'.");
			}
			if (args.Length > 2 && !int.TryParse(args[2], out num2))
			{
				return new CmdResult(false, "Arg 3 must be the hand index (int), got '" + args[2] + "'.");
			}
			CardPile pile = PileType.Hand.GetPile(issuingPlayer);
			IReadOnlyList<CardModel> cards = pile.Cards;
			int count = cards.Count;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num2 < 0 || num2 >= count)
			{
				bool flag = false;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid hand index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(". Valid range: 0-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count - 1);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return new CmdResult(flag, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			CardModel cardModel = pile.Cards[num2];
			Task task = CardCmd.Afflict(afflictionModel, cardModel, num);
			Task task2 = task;
			bool flag2 = true;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Afflicted card ");
			defaultInterpolatedStringHandler.AppendFormatted(cardModel.Title);
			defaultInterpolatedStringHandler.AppendLiteral(" with ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(afflictionModel.Title.GetFormattedText());
			return new CmdResult(task2, flag2, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600858C RID: 34188 RVA: 0x002CD908 File Offset: 0x002CBB08
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ModelDb.DebugAfflictions.Select<AfflictionModel, string>((AfflictionModel affliction) => affliction.Id.Entry).ToList<string>();
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
