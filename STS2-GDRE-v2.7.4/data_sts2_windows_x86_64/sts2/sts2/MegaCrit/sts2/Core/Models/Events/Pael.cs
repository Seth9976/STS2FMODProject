using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000846 RID: 2118
	[NullableContext(1)]
	[Nullable(0)]
	public class Pael : AncientEventModel
	{
		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x0600685E RID: 26718 RVA: 0x0027761C File Offset: 0x0027581C
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				return this.OptionPool1.Concat<EventOption>(this.OptionPool2).Concat<EventOption>(this.OptionPool3).Concat<EventOption>(new <>z__ReadOnlyArray<EventOption>(new EventOption[] { this.PaelsClawOption, this.PaelsToothOption, this.PaelsLegionOption, this.PaelsGrowthOption }));
			}
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x0027767C File Offset: 0x0027587C
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
			string text3 = AncientEventModel.CharKey<Defect>();
			dictionary[text3] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
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
			ancientDialogueSet2.CharacterDialogues = dictionary;
			ancientDialogueSet.AgnosticDialogues = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "" }),
				new AncientDialogue(new string[] { "" })
			});
			return ancientDialogueSet;
		}

		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x06006860 RID: 26720 RVA: 0x002779A7 File Offset: 0x00275BA7
		public override Color ButtonColor
		{
			get
			{
				return new Color(0.03f, 0.08f, 0f, 0.75f);
			}
		}

		// Token: 0x170019A9 RID: 6569
		// (get) Token: 0x06006861 RID: 26721 RVA: 0x002779C2 File Offset: 0x00275BC2
		public override Color DialogueColor
		{
			get
			{
				return new Color("332C29");
			}
		}

		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x06006862 RID: 26722 RVA: 0x002779CE File Offset: 0x00275BCE
		private EventOption PaelsClawOption
		{
			get
			{
				return base.RelicOption<PaelsClaw>("INITIAL", null);
			}
		}

		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x06006863 RID: 26723 RVA: 0x002779DC File Offset: 0x00275BDC
		private EventOption PaelsToothOption
		{
			get
			{
				return base.RelicOption<PaelsTooth>("INITIAL", null);
			}
		}

		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x06006864 RID: 26724 RVA: 0x002779EA File Offset: 0x00275BEA
		private EventOption PaelsGrowthOption
		{
			get
			{
				return base.RelicOption<PaelsGrowth>("INITIAL", null);
			}
		}

		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x06006865 RID: 26725 RVA: 0x002779F8 File Offset: 0x00275BF8
		private EventOption PaelsLegionOption
		{
			get
			{
				return base.RelicOption<PaelsLegion>("INITIAL", null);
			}
		}

		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x06006866 RID: 26726 RVA: 0x00277A06 File Offset: 0x00275C06
		private IEnumerable<EventOption> OptionPool1
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<PaelsFlesh>("INITIAL", null),
					base.RelicOption<PaelsHorn>("INITIAL", null),
					base.RelicOption<PaelsTears>("INITIAL", null)
				});
			}
		}

		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x06006867 RID: 26727 RVA: 0x00277A40 File Offset: 0x00275C40
		private unsafe List<EventOption> OptionPool2
		{
			get
			{
				int num = 1;
				List<EventOption> list = new List<EventOption>(num);
				CollectionsMarshal.SetCount<EventOption>(list, num);
				Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
				int num2 = 0;
				*span[num2] = base.RelicOption<PaelsWing>("INITIAL", null);
				return list;
			}
		}

		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x06006868 RID: 26728 RVA: 0x00277A7C File Offset: 0x00275C7C
		private unsafe List<EventOption> OptionPool3
		{
			get
			{
				int num = 2;
				List<EventOption> list = new List<EventOption>(num);
				CollectionsMarshal.SetCount<EventOption>(list, num);
				Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(list);
				int num2 = 0;
				*span[num2] = base.RelicOption<PaelsEye>("INITIAL", null);
				num2++;
				*span[num2] = base.RelicOption<PaelsBlood>("INITIAL", null);
				return list;
			}
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x00277AD0 File Offset: 0x00275CD0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			EventOption eventOption = base.Rng.NextItem<EventOption>(this.OptionPool1);
			List<EventOption> list = this.OptionPool2.ToList<EventOption>();
			IReadOnlyList<CardModel> cards = base.Owner.Deck.Cards;
			if (cards.Count<CardModel>((CardModel c) => ModelDb.Enchantment<Goopy>().CanEnchant(c)) >= 3)
			{
				list.Add(this.PaelsClawOption);
			}
			if (cards.Count<CardModel>((CardModel c) => c.IsRemovable) >= 5)
			{
				list.Add(this.PaelsToothOption);
			}
			list.AddRange(list);
			list.Add(this.PaelsGrowthOption);
			EventOption eventOption2 = base.Rng.NextItem<EventOption>(list);
			List<EventOption> list2 = this.OptionPool3.ToList<EventOption>();
			if (!base.Owner.HasEventPet())
			{
				list2.Add(this.PaelsLegionOption);
			}
			EventOption eventOption3 = base.Rng.NextItem<EventOption>(list2);
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[] { eventOption, eventOption2, eventOption3 });
		}
	}
}
