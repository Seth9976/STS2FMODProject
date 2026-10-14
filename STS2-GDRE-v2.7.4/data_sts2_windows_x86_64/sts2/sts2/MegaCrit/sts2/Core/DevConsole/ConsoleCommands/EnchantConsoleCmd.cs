using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CED RID: 3309
	[NullableContext(1)]
	[Nullable(0)]
	public class EnchantConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x170024BB RID: 9403
		// (get) Token: 0x060085DC RID: 34268 RVA: 0x002CEDCF File Offset: 0x002CCFCF
		public override string CmdName
		{
			get
			{
				return "enchant";
			}
		}

		// Token: 0x170024BC RID: 9404
		// (get) Token: 0x060085DD RID: 34269 RVA: 0x002CEDD6 File Offset: 0x002CCFD6
		public override string Args
		{
			get
			{
				return "<id:string> [amount:int] [hand-index:int]";
			}
		}

		// Token: 0x170024BD RID: 9405
		// (get) Token: 0x060085DE RID: 34270 RVA: 0x002CEDDD File Offset: 0x002CCFDD
		public override string Description
		{
			get
			{
				return "Enchants a card in the player's hand with the specified enchantment.";
			}
		}

		// Token: 0x170024BE RID: 9406
		// (get) Token: 0x060085DF RID: 34271 RVA: 0x002CEDE4 File Offset: 0x002CCFE4
		public override bool IsNetworked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085E0 RID: 34272 RVA: 0x002CEDE8 File Offset: 0x002CCFE8
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length == 0)
			{
				return new CmdResult(false, "Must specify an enchantment ID!");
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return new CmdResult(false, "Combat is not currently in progress!");
			}
			ModelId modelId = new ModelId(ModelId.SlugifyCategory<EnchantmentModel>(), args[0].ToUpperInvariant());
			EnchantmentModel enchantmentModel;
			try
			{
				enchantmentModel = ModelDb.GetById<EnchantmentModel>(modelId).ToMutable();
			}
			catch (ModelNotFoundException)
			{
				return new CmdResult(false, "Enchantment '" + modelId.Entry + "' not found");
			}
			int num = 1;
			int num2 = 0;
			if (args.Length > 1 && !int.TryParse(args[1], out num))
			{
				return new CmdResult(false, "Arg 2 must be the enchantment amount (int), got '" + args[1] + "'.");
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
			CardCmd.Enchant(enchantmentModel, cardModel, num);
			bool flag2 = true;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Enchanted card ");
			defaultInterpolatedStringHandler.AppendFormatted(cardModel.Title);
			defaultInterpolatedStringHandler.AppendLiteral(" with ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(enchantmentModel.Title.GetFormattedText());
			return new CmdResult(flag2, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060085E1 RID: 34273 RVA: 0x002CEFC4 File Offset: 0x002CD1C4
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ModelDb.DebugEnchantments.Select<EnchantmentModel, string>((EnchantmentModel e) => e.Id.Entry).ToList<string>();
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
