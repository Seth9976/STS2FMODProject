using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000CE4 RID: 3300
	[NullableContext(1)]
	[Nullable(0)]
	public class ArtConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002495 RID: 9365
		// (get) Token: 0x0600859D RID: 34205 RVA: 0x002CDE88 File Offset: 0x002CC088
		public override string CmdName
		{
			get
			{
				return "art";
			}
		}

		// Token: 0x17002496 RID: 9366
		// (get) Token: 0x0600859E RID: 34206 RVA: 0x002CDE8F File Offset: 0x002CC08F
		public override string Args
		{
			get
			{
				return "<type:string>";
			}
		}

		// Token: 0x17002497 RID: 9367
		// (get) Token: 0x0600859F RID: 34207 RVA: 0x002CDE96 File Offset: 0x002CC096
		public override string Description
		{
			get
			{
				return "Lists all the content of the specified type that is missing art. " + ArtConsoleCmd.TypesHint;
			}
		}

		// Token: 0x17002498 RID: 9368
		// (get) Token: 0x060085A0 RID: 34208 RVA: 0x002CDEA7 File Offset: 0x002CC0A7
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002499 RID: 9369
		// (get) Token: 0x060085A1 RID: 34209 RVA: 0x002CDEAA File Offset: 0x002CC0AA
		private static string TypesHint
		{
			get
			{
				return "Types: " + string.Join(", ", ArtConsoleCmd._types) + ".";
			}
		}

		// Token: 0x060085A2 RID: 34210 RVA: 0x002CDECC File Offset: 0x002CC0CC
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1)
			{
				return new CmdResult(false, this.CmdName + " requires a type. " + ArtConsoleCmd.TypesHint);
			}
			string text = args[0].ToLowerInvariant();
			if (text.EndsWith('s'))
			{
				string text2 = text;
				text = text2.Substring(0, text2.Length - 1);
			}
			List<ArtConsoleCmd.MissingArt> list = new List<ArtConsoleCmd.MissingArt>();
			if (!(text == "affliction"))
			{
				if (!(text == "card"))
				{
					if (text == "enchantment")
					{
						goto IL_01B1;
					}
					if (text == "power")
					{
						goto IL_0220;
					}
					if (!(text == "relic"))
					{
						goto IL_0313;
					}
					goto IL_029B;
				}
			}
			else
			{
				using (IEnumerator<AfflictionModel> enumerator = ModelDb.DebugAfflictions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AfflictionModel afflictionModel = enumerator.Current;
						if (!afflictionModel.HasOverlay)
						{
							list.Add(new ArtConsoleCmd.MissingArt(afflictionModel, afflictionModel.OverlayPath, afflictionModel.DynamicDescription.GetRawText()));
						}
					}
					goto IL_0331;
				}
			}
			using (IEnumerator<CardModel> enumerator2 = ModelDb.AllCards.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CardModel cardModel = enumerator2.Current;
					if (!(cardModel is DeprecatedCard))
					{
						foreach (string text3 in cardModel.AllPortraitPaths)
						{
							ValueTuple<string, string> valueTuple = AtlasResourceLoader.ParsePath(text3);
							string item = valueTuple.Item1;
							string item2 = valueTuple.Item2;
							if (item == null || !AtlasManager.HasSprite(item, item2))
							{
								list.Add(new ArtConsoleCmd.MissingArt(cardModel, cardModel.PortraitPath, cardModel.Description.GetRawText()));
							}
						}
					}
				}
				goto IL_0331;
			}
			IL_01B1:
			using (IEnumerator<EnchantmentModel> enumerator4 = ModelDb.DebugEnchantments.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					EnchantmentModel enchantmentModel = enumerator4.Current;
					if (!(enchantmentModel is DeprecatedEnchantment) && !(enchantmentModel.IconPath == enchantmentModel.IntendedIconPath))
					{
						list.Add(new ArtConsoleCmd.MissingArt(enchantmentModel, enchantmentModel.IntendedIconPath, enchantmentModel.DynamicDescription.GetRawText()));
					}
				}
				goto IL_0331;
			}
			IL_0220:
			using (IEnumerator<PowerModel> enumerator5 = ModelDb.AllPowers.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					PowerModel powerModel = enumerator5.Current;
					ValueTuple<string, string> valueTuple2 = AtlasResourceLoader.ParsePath(powerModel.PackedIconPath);
					string item3 = valueTuple2.Item1;
					string item4 = valueTuple2.Item2;
					if (item3 == null || !AtlasManager.HasSprite(item3, item4))
					{
						list.Add(new ArtConsoleCmd.MissingArt(powerModel, powerModel.PackedIconPath, powerModel.Description.GetRawText()));
					}
				}
				goto IL_0331;
			}
			IL_029B:
			using (IEnumerator<RelicModel> enumerator6 = ModelDb.AllRelics.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					RelicModel relicModel = enumerator6.Current;
					ValueTuple<string, string> valueTuple3 = AtlasResourceLoader.ParsePath(relicModel.PackedIconPath);
					string item5 = valueTuple3.Item1;
					string item6 = valueTuple3.Item2;
					if (item5 == null || !AtlasManager.HasSprite(item5, item6))
					{
						list.Add(new ArtConsoleCmd.MissingArt(relicModel, relicModel.PackedIconPath, relicModel.DynamicDescription.GetRawText()));
					}
				}
				goto IL_0331;
			}
			IL_0313:
			return new CmdResult(false, "'" + args[0] + "' is not a valid type. " + ArtConsoleCmd.TypesHint);
			IL_0331:
			string text4 = ((list.Count == 1) ? "" : "s");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 3);
			defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendFormatted(text4);
			defaultInterpolatedStringHandler.AppendLiteral(" with missing art:\n");
			StringBuilder stringBuilder = new StringBuilder(defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (ArtConsoleCmd.MissingArt missingArt in list.OrderBy<ArtConsoleCmd.MissingArt, string>((ArtConsoleCmd.MissingArt m) => m.Model.Id.Entry))
			{
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(missingArt.Model.Id.Entry);
				stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("* Intended Path: ");
				appendInterpolatedStringHandler.AppendFormatted(missingArt.IntendedPath);
				stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder5 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("* Description: \"");
				appendInterpolatedStringHandler.AppendFormatted(missingArt.Description);
				appendInterpolatedStringHandler.AppendLiteral("\"");
				stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			}
			return new CmdResult(true, stringBuilder.ToString());
		}

		// Token: 0x060085A3 RID: 34211 RVA: 0x002CE3C8 File Offset: 0x002CC5C8
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				List<string> list = ArtConsoleCmd._types.ToList<string>();
				return base.CompleteArgument(list, Array.Empty<string>(), args.FirstOrDefault<string>() ?? "", CompletionType.Argument, null);
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}

		// Token: 0x04002E63 RID: 11875
		private static readonly string[] _types = new string[] { "affliction", "card", "enchantment", "power", "relic" };

		// Token: 0x020023E4 RID: 9188
		[Nullable(0)]
		private struct MissingArt
		{
			// Token: 0x170026D5 RID: 9941
			// (get) Token: 0x0600C227 RID: 49703 RVA: 0x00424BB2 File Offset: 0x00422DB2
			public readonly AbstractModel Model { get; }

			// Token: 0x170026D6 RID: 9942
			// (get) Token: 0x0600C228 RID: 49704 RVA: 0x00424BBA File Offset: 0x00422DBA
			public readonly string IntendedPath { get; }

			// Token: 0x170026D7 RID: 9943
			// (get) Token: 0x0600C229 RID: 49705 RVA: 0x00424BC2 File Offset: 0x00422DC2
			public readonly string Description { get; }

			// Token: 0x0600C22A RID: 49706 RVA: 0x00424BCA File Offset: 0x00422DCA
			public MissingArt(AbstractModel model, string intendedPath, string description)
			{
				this.Model = model;
				this.IntendedPath = intendedPath;
				this.Description = description;
			}
		}
	}
}
