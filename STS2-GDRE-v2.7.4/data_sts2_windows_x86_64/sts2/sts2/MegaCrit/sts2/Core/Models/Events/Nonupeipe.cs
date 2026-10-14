using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000844 RID: 2116
	[NullableContext(1)]
	[Nullable(0)]
	public class Nonupeipe : AncientEventModel
	{
		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x06006849 RID: 26697 RVA: 0x00276AAB File Offset: 0x00274CAB
		public override Color ButtonColor
		{
			get
			{
				return new Color(0f, 0.1f, 0.16f, 0.75f);
			}
		}

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x0600684A RID: 26698 RVA: 0x00276AC6 File Offset: 0x00274CC6
		public override Color DialogueColor
		{
			get
			{
				return new Color("0A494D");
			}
		}

		// Token: 0x0600684B RID: 26699 RVA: 0x00276AD4 File Offset: 0x00274CD4
		protected override AncientDialogueSet DefineDialogues()
		{
			AncientDialogueSet ancientDialogueSet = new AncientDialogueSet();
			ancientDialogueSet.FirstVisitEverDialogue = new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" });
			AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet;
			Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
			string text = AncientEventModel.CharKey<Ironclad>();
			dictionary[text] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" })
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
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out", "", "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_giggle" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_eeked", "", "event:/sfx/npcs/nonupeipe/nonupeipe_eeked" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text3 = AncientEventModel.CharKey<Defect>();
			dictionary[text3] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_giggle", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text4 = AncientEventModel.CharKey<Necrobinder>();
			dictionary[text4] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "event:/sfx/npcs/nonupeipe/nonupeipe_eeked" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text5 = AncientEventModel.CharKey<Regent>();
			dictionary[text5] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "event:/sfx/npcs/nonupeipe/nonupeipe_eeked" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			ancientDialogueSet2.CharacterDialogues = dictionary;
			ancientDialogueSet.AgnosticDialogues = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_welcome" }),
				new AncientDialogue(new string[] { "event:/sfx/npcs/nonupeipe/nonupeipe_eeked" })
			});
			return ancientDialogueSet;
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x0600684C RID: 26700 RVA: 0x00276E0F File Offset: 0x0027500F
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				return this.OptionPool.Concat<EventOption>(new <>z__ReadOnlySingleElementList<EventOption>(this.BeautifulBraceletEventOption));
			}
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x0600684D RID: 26701 RVA: 0x00276E27 File Offset: 0x00275027
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x0600684E RID: 26702 RVA: 0x00276E30 File Offset: 0x00275030
		private IEnumerable<EventOption> OptionPool
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<BlessedAntler>("INITIAL", null),
					base.RelicOption<BrilliantScarf>("INITIAL", null),
					base.RelicOption<DelicateFrond>("INITIAL", null),
					base.RelicOption<DiamondDiadem>("INITIAL", null),
					base.RelicOption<FurCoat>("INITIAL", null),
					base.RelicOption<Glitter>("INITIAL", null),
					base.RelicOption<JewelryBox>("INITIAL", null),
					base.RelicOption<LoomingFruit>("INITIAL", null),
					base.RelicOption<SignetRing>("INITIAL", null)
				});
			}
		}

		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x0600684F RID: 26703 RVA: 0x00276ED0 File Offset: 0x002750D0
		private EventOption BeautifulBraceletEventOption
		{
			get
			{
				return base.RelicOption<BeautifulBracelet>("INITIAL", null);
			}
		}

		// Token: 0x06006850 RID: 26704 RVA: 0x00276EE0 File Offset: 0x002750E0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = this.OptionPool.ToList<EventOption>();
			if (base.Owner.Deck.Cards.Count<CardModel>(new Func<CardModel, bool>(ModelDb.Enchantment<Swift>().CanEnchant)) >= 4)
			{
				list.Add(this.BeautifulBraceletEventOption);
			}
			list.UnstableShuffle<EventOption>(base.Rng);
			return list.Take<EventOption>(3).ToList<EventOption>();
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x06006851 RID: 26705 RVA: 0x00276F47 File Offset: 0x00275147
		protected override Color EventButtonColor
		{
			get
			{
				return new Color("000000BF");
			}
		}

		// Token: 0x04002796 RID: 10134
		private const string _sfxEeked = "event:/sfx/npcs/nonupeipe/nonupeipe_eeked";

		// Token: 0x04002797 RID: 10135
		private const string _sfxWelcome = "event:/sfx/npcs/nonupeipe/nonupeipe_welcome";

		// Token: 0x04002798 RID: 10136
		private const string _sfxGrossedOut = "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out";

		// Token: 0x04002799 RID: 10137
		private const string _sfxGiggle = "event:/sfx/npcs/nonupeipe/nonupeipe_giggle";
	}
}
