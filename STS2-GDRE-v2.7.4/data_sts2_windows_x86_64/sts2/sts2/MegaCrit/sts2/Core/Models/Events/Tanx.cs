using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000858 RID: 2136
	[NullableContext(1)]
	[Nullable(0)]
	public class Tanx : AncientEventModel
	{
		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x06006906 RID: 26886 RVA: 0x0027A19F File Offset: 0x0027839F
		public override Color ButtonColor
		{
			get
			{
				return new Color(0.05f, 0.02f, 0f, 0.5f);
			}
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x06006907 RID: 26887 RVA: 0x0027A1BA File Offset: 0x002783BA
		public override Color DialogueColor
		{
			get
			{
				return new Color("731717");
			}
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x0027A1C8 File Offset: 0x002783C8
		protected override AncientDialogueSet DefineDialogues()
		{
			AncientDialogueSet ancientDialogueSet = new AncientDialogueSet();
			ancientDialogueSet.FirstVisitEverDialogue = new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_laugh" });
			AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet;
			Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
			string text = AncientEventModel.CharKey<Ironclad>();
			dictionary[text] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_laugh" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar", "", "event:/sfx/npcs/tanx/tanx_laugh" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text2 = AncientEventModel.CharKey<Silent>();
			dictionary[text2] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_curiosity" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar", "", "event:/sfx/npcs/tanx/tanx_curiosity" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text3 = AncientEventModel.CharKey<Defect>();
			dictionary[text3] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_curiosity" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_laugh" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar", "", "event:/sfx/npcs/tanx/tanx_curiosity" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text4 = AncientEventModel.CharKey<Necrobinder>();
			dictionary[text4] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar", "", "event:/sfx/npcs/tanx/tanx_curiosity" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar", "", "event:/sfx/npcs/tanx/tanx_laugh" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text5 = AncientEventModel.CharKey<Regent>();
			dictionary[text5] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_curiosity", "", "event:/sfx/npcs/tanx/tanx_laugh" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar", "", "event:/sfx/npcs/tanx/tanx_roar" })
				{
					VisitIndex = new int?(4)
				}
			});
			ancientDialogueSet2.CharacterDialogues = dictionary;
			ancientDialogueSet.AgnosticDialogues = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_roar" }),
				new AncientDialogue(new string[] { "event:/sfx/npcs/tanx/tanx_laugh" })
			});
			return ancientDialogueSet;
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x06006909 RID: 26889 RVA: 0x0027A52B File Offset: 0x0027872B
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				return this.BaseOptionPool.Append(this.TriBoomerangOption);
			}
		}

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x0600690A RID: 26890 RVA: 0x0027A540 File Offset: 0x00278740
		private IEnumerable<EventOption> BaseOptionPool
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<Claws>("INITIAL", null),
					base.RelicOption<Crossbow>("INITIAL", null),
					base.RelicOption<IronClub>("INITIAL", null),
					base.RelicOption<MeatCleaver>("INITIAL", null),
					base.RelicOption<Sai>("INITIAL", null),
					base.RelicOption<SpikedGauntlets>("INITIAL", null),
					base.RelicOption<TanxsWhistle>("INITIAL", null),
					base.RelicOption<ThrowingAxe>("INITIAL", null),
					base.RelicOption<WarHammer>("INITIAL", null)
				});
			}
		}

		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x0600690B RID: 26891 RVA: 0x0027A5E0 File Offset: 0x002787E0
		private EventOption TriBoomerangOption
		{
			get
			{
				return base.RelicOption<TriBoomerang>("INITIAL", null);
			}
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x0027A5F0 File Offset: 0x002787F0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = this.BaseOptionPool.ToList<EventOption>();
			if (base.Owner.Deck.Cards.Count<CardModel>((CardModel c) => ModelDb.Enchantment<Instinct>().CanEnchant(c)) >= 3)
			{
				list.Add(this.TriBoomerangOption);
			}
			return list.UnstableShuffle<EventOption>(base.Rng).Take<EventOption>(3).ToList<EventOption>();
		}

		// Token: 0x040027C7 RID: 10183
		private const string _sfxLaugh = "event:/sfx/npcs/tanx/tanx_laugh";

		// Token: 0x040027C8 RID: 10184
		private const string _sfxRoar = "event:/sfx/npcs/tanx/tanx_roar";

		// Token: 0x040027C9 RID: 10185
		private const string _sfxCuriosity = "event:/sfx/npcs/tanx/tanx_curiosity";

		// Token: 0x040027CA RID: 10186
		private const int _triBoomerangCount = 3;
	}
}
