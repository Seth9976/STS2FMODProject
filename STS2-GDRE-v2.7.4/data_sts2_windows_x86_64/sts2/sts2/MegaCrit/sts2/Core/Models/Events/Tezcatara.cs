using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200085A RID: 2138
	[NullableContext(1)]
	[Nullable(0)]
	public class Tezcatara : AncientEventModel
	{
		/// <summary>
		/// This ancient's "any character" dialogue is friendly, but they don't trust the <see cref="T:MegaCrit.Sts2.Core.Models.Characters.Defect" />, so we
		/// blacklist them so we don't show these friendly dialogue options for them.
		/// </summary>
		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x06006915 RID: 26901 RVA: 0x0027A913 File Offset: 0x00278B13
		public override IEnumerable<CharacterModel> AnyCharacterDialogueBlacklist
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CharacterModel>(ModelDb.Character<Defect>());
			}
		}

		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x06006916 RID: 26902 RVA: 0x0027A91F File Offset: 0x00278B1F
		public override Color ButtonColor
		{
			get
			{
				return new Color(0.08f, 0.04f, 0f, 0.75f);
			}
		}

		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x06006917 RID: 26903 RVA: 0x0027A93A File Offset: 0x00278B3A
		public override Color DialogueColor
		{
			get
			{
				return new Color("33251E");
			}
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x0027A948 File Offset: 0x00278B48
		protected override AncientDialogueSet DefineDialogues()
		{
			AncientDialogueSet ancientDialogueSet = new AncientDialogueSet();
			ancientDialogueSet.FirstVisitEverDialogue = new AncientDialogue(new string[] { "" });
			AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet;
			Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
			string text = AncientEventModel.CharKey<Ironclad>();
			dictionary[text] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text2 = AncientEventModel.CharKey<Silent>();
			dictionary[text2] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text3 = AncientEventModel.CharKey<Defect>();
			dictionary[text3] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text4 = AncientEventModel.CharKey<Necrobinder>();
			dictionary[text4] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text5 = AncientEventModel.CharKey<Regent>();
			dictionary[text5] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			ancientDialogueSet2.CharacterDialogues = dictionary;
			ancientDialogueSet.AgnosticDialogues = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "" }),
				new AncientDialogue(new string[] { "" })
			});
			return ancientDialogueSet;
		}

		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x06006919 RID: 26905 RVA: 0x0027ACB4 File Offset: 0x00278EB4
		public unsafe override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				List<EventOption>[] array = new List<EventOption>[4];
				array[0] = this.OptionPool1;
				array[1] = this.OptionPool2;
				array[2] = this.OptionPool3;
				int num = 3;
				int num2 = 1;
				List<EventOption> list = new List<EventOption>(num2);
				CollectionsMarshal.SetCount<EventOption>(list, num2);
				Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
				int num3 = 0;
				*span[num3] = this.NutritiousSoupOption;
				array[num] = list;
				return array.SelectMany<List<EventOption>, EventOption>((List<EventOption> x) => x);
			}
		}

		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x0600691A RID: 26906 RVA: 0x0027AD30 File Offset: 0x00278F30
		private EventOption NutritiousSoupOption
		{
			get
			{
				return base.RelicOption<NutritiousSoup>("INITIAL", null);
			}
		}

		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x0600691B RID: 26907 RVA: 0x0027AD40 File Offset: 0x00278F40
		private unsafe List<EventOption> OptionPool1
		{
			get
			{
				int num = 2;
				List<EventOption> list = new List<EventOption>(num);
				CollectionsMarshal.SetCount<EventOption>(list, num);
				Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
				int num2 = 0;
				*span[num2] = base.RelicOption<VeryHotCocoa>("INITIAL", null);
				num2++;
				*span[num2] = base.RelicOption<YummyCookie>("INITIAL", null);
				return list;
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x0600691C RID: 26908 RVA: 0x0027AD94 File Offset: 0x00278F94
		private unsafe List<EventOption> OptionPool2
		{
			get
			{
				int num = 3;
				List<EventOption> list = new List<EventOption>(num);
				CollectionsMarshal.SetCount<EventOption>(list, num);
				Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
				int num2 = 0;
				*span[num2] = base.RelicOption<BiiigHug>("INITIAL", null);
				num2++;
				*span[num2] = base.RelicOption<Storybook>("INITIAL", null);
				num2++;
				*span[num2] = base.RelicOption<ToastyMittens>("INITIAL", null);
				return list;
			}
		}

		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x0600691D RID: 26909 RVA: 0x0027AE00 File Offset: 0x00279000
		private unsafe List<EventOption> OptionPool3
		{
			get
			{
				int num = 4;
				List<EventOption> list = new List<EventOption>(num);
				CollectionsMarshal.SetCount<EventOption>(list, num);
				Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
				int num2 = 0;
				*span[num2] = base.RelicOption<GoldenCompass>("INITIAL", null);
				num2++;
				*span[num2] = base.RelicOption<PumpkinCandle>("INITIAL", null);
				num2++;
				*span[num2] = base.RelicOption<ToyBox>("INITIAL", null);
				num2++;
				*span[num2] = base.RelicOption<SealOfGold>("INITIAL", null);
				return list;
			}
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x0027AE88 File Offset: 0x00279088
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = this.OptionPool1.ToList<EventOption>();
			if (base.Owner.Deck.Cards.Any<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Strike) && c.Rarity == CardRarity.Basic))
			{
				list.Add(this.NutritiousSoupOption);
			}
			EventOption eventOption = base.Rng.NextItem<EventOption>(list);
			EventOption eventOption2 = base.Rng.NextItem<EventOption>(this.OptionPool2);
			EventOption eventOption3 = base.Rng.NextItem<EventOption>(this.OptionPool3);
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[] { eventOption, eventOption2, eventOption3 });
		}
	}
}
