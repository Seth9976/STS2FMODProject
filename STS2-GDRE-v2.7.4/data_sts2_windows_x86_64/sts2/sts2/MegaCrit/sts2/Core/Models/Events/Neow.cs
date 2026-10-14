using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000843 RID: 2115
	[NullableContext(1)]
	[Nullable(0)]
	public class Neow : AncientEventModel
	{
		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x06006835 RID: 26677 RVA: 0x00275FBF File Offset: 0x002741BF
		public override string AmbientBgm
		{
			get
			{
				return "event:/sfx/ambience/act1_neow";
			}
		}

		// Token: 0x1700198B RID: 6539
		// (get) Token: 0x06006836 RID: 26678 RVA: 0x00275FC6 File Offset: 0x002741C6
		public override Color ButtonColor
		{
			get
			{
				return new Color(0f, 0.1f, 0.2f, 0.5f);
			}
		}

		// Token: 0x1700198C RID: 6540
		// (get) Token: 0x06006837 RID: 26679 RVA: 0x00275FE1 File Offset: 0x002741E1
		public override Color DialogueColor
		{
			get
			{
				return new Color("28454F");
			}
		}

		// Token: 0x1700198D RID: 6541
		// (get) Token: 0x06006838 RID: 26680 RVA: 0x00275FF0 File Offset: 0x002741F0
		public override LocString InitialDescription
		{
			get
			{
				Player owner = base.Owner;
				if (owner != null && owner.RunState.Modifiers.Count <= 0)
				{
					return base.InitialDescription;
				}
				return base.L10NLookup(base.Id.Entry + ".EVENT.description");
			}
		}

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x06006839 RID: 26681 RVA: 0x00276044 File Offset: 0x00274244
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				List<EventOption> list = new List<EventOption>();
				list.AddRange(this.CurseOptions);
				list.AddRange(this.PositiveOptions);
				list.Add(this.LavaRockOption);
				list.Add(this.NeowsTalismanOption);
				list.Add(this.NutritiousOysterOption);
				list.Add(this.PomanderOption);
				list.Add(this.SmallCapsuleOption);
				list.Add(this.StoneHumidifierOption);
				return new <>z__ReadOnlyList<EventOption>(list);
			}
		}

		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x0600683A RID: 26682 RVA: 0x002760BC File Offset: 0x002742BC
		private IEnumerable<EventOption> PositiveOptions
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<ArcaneScroll>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<BoomingConch>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<FishingRod>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<GoldenPearl>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<Kaleidoscope>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<LeadPaperweight>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<LostCoffer>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<MassiveScroll>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<NeowsTorment>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<NewLeaf>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<PhialHolster>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<PreciseScissors>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<ScrollBoxes>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<WingedBoots>("INITIAL", "NEOW.pages.DONE.POSITIVE.description")
				});
			}
		}

		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x0600683B RID: 26683 RVA: 0x002761E4 File Offset: 0x002743E4
		private EventOption LavaRockOption
		{
			get
			{
				return base.RelicOption<LavaRock>("INITIAL", "NEOW.pages.DONE.POSITIVE.description");
			}
		}

		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x0600683C RID: 26684 RVA: 0x002761F6 File Offset: 0x002743F6
		private EventOption NeowsTalismanOption
		{
			get
			{
				return base.RelicOption<NeowsTalisman>("INITIAL", "NEOW.pages.DONE.POSITIVE.description");
			}
		}

		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x0600683D RID: 26685 RVA: 0x00276208 File Offset: 0x00274408
		private EventOption NutritiousOysterOption
		{
			get
			{
				return base.RelicOption<NutritiousOyster>("INITIAL", "NEOW.pages.DONE.POSITIVE.description");
			}
		}

		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x0600683E RID: 26686 RVA: 0x0027621A File Offset: 0x0027441A
		private EventOption PomanderOption
		{
			get
			{
				return base.RelicOption<Pomander>("INITIAL", "NEOW.pages.DONE.POSITIVE.description");
			}
		}

		// Token: 0x17001994 RID: 6548
		// (get) Token: 0x0600683F RID: 26687 RVA: 0x0027622C File Offset: 0x0027442C
		private EventOption SmallCapsuleOption
		{
			get
			{
				return base.RelicOption<SmallCapsule>("INITIAL", "NEOW.pages.DONE.POSITIVE.description");
			}
		}

		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x06006840 RID: 26688 RVA: 0x0027623E File Offset: 0x0027443E
		private EventOption StoneHumidifierOption
		{
			get
			{
				return base.RelicOption<StoneHumidifier>("INITIAL", "NEOW.pages.DONE.POSITIVE.description");
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x06006841 RID: 26689 RVA: 0x00276250 File Offset: 0x00274450
		private IEnumerable<EventOption> CurseOptions
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<CursedPearl>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<DowsingRod>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<HeftyTablet>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<LargeCapsule>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<LeafyPoultice>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<NeowsBones>("INITIAL", "NEOW.pages.DONE.POSITIVE.description"),
					base.RelicOption<NeowsSacrifice>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<PrecariousShears>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<SilkenTress>("INITIAL", "NEOW.pages.DONE.CURSED.description"),
					base.RelicOption<SilverCrucible>("INITIAL", "NEOW.pages.DONE.CURSED.description")
				});
			}
		}

		// Token: 0x06006842 RID: 26690 RVA: 0x00276328 File Offset: 0x00274528
		protected override AncientDialogueSet DefineDialogues()
		{
			AncientDialogueSet ancientDialogueSet = new AncientDialogueSet();
			ancientDialogueSet.FirstVisitEverDialogue = new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_welcome" });
			AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet;
			Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
			string text = AncientEventModel.CharKey<Ironclad>();
			dictionary[text] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_welcome" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_curious" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_sleepy", "event:/sfx/npcs/neow/neow_sleepy", "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text2 = AncientEventModel.CharKey<Silent>();
			dictionary[text2] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_curious" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_sleepy", "", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text3 = AncientEventModel.CharKey<Defect>();
			dictionary[text3] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_curious" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_sleepy", "", "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text4 = AncientEventModel.CharKey<Necrobinder>();
			dictionary[text4] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_welcome", "", "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "", "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text5 = AncientEventModel.CharKey<Regent>();
			dictionary[text5] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "event:/sfx/npcs/neow/neow_sleepy" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_curious" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "", "event:/sfx/npcs/neow/neow_sleepy", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			ancientDialogueSet2.CharacterDialogues = dictionary;
			ancientDialogueSet.AgnosticDialogues = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_welcome" }),
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_welcome" }),
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_welcome" }),
				new AncientDialogue(new string[] { "event:/sfx/npcs/neow/neow_welcome" }),
				new AncientDialogue(new string[] { "" })
			});
			return ancientDialogueSet;
		}

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x06006843 RID: 26691 RVA: 0x002766BD File Offset: 0x002748BD
		private List<EventOption> ModifierOptions
		{
			get
			{
				base.AssertMutable();
				if (this._modifierOptions == null)
				{
					this._modifierOptions = new List<EventOption>();
				}
				return this._modifierOptions;
			}
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x002766E0 File Offset: 0x002748E0
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			if (base.Owner.RunState.Modifiers.Count <= 0)
			{
				List<EventOption> list = this.CurseOptions.ToList<EventOption>();
				list.RemoveAll(delegate(EventOption r)
				{
					RelicModel relic = r.Relic;
					return relic != null && !relic.IsAllowedAtNeow(base.Owner);
				});
				EventOption eventOption = base.Rng.NextItem<EventOption>(list);
				List<EventOption> list2 = this.PositiveOptions.ToList<EventOption>();
				if (eventOption.Relic is CursedPearl)
				{
					list2.RemoveAll((EventOption o) => o.Relic is GoldenPearl);
				}
				if (eventOption.Relic is HeftyTablet)
				{
					list2.RemoveAll((EventOption o) => o.Relic is ArcaneScroll);
				}
				if (eventOption.Relic is LeafyPoultice)
				{
					list2.RemoveAll((EventOption o) => o.Relic is NewLeaf);
				}
				if (eventOption.Relic is PrecariousShears)
				{
					list2.RemoveAll((EventOption o) => o.Relic is PreciseScissors);
				}
				if (eventOption.Relic is NeowsSacrifice)
				{
					list2.RemoveAll((EventOption o) => o.Relic is PhialHolster);
					list2.RemoveAll((EventOption o) => o.Relic is LostCoffer);
				}
				if (!(eventOption.Relic is LargeCapsule))
				{
					if (base.Rng.NextBool())
					{
						list2.Add(this.LavaRockOption);
					}
					else
					{
						list2.Add(this.SmallCapsuleOption);
					}
				}
				if (base.Rng.NextBool())
				{
					list2.Add(this.NutritiousOysterOption);
				}
				else
				{
					list2.Add(this.StoneHumidifierOption);
				}
				if (base.Rng.NextBool())
				{
					list2.Add(this.NeowsTalismanOption);
				}
				else
				{
					list2.Add(this.PomanderOption);
				}
				list2.RemoveAll(delegate(EventOption r)
				{
					RelicModel relic2 = r.Relic;
					return relic2 != null && !relic2.IsAllowedAtNeow(base.Owner);
				});
				List<EventOption> list3 = new List<EventOption>();
				list3.AddRange(list2.ToList<EventOption>().UnstableShuffle<EventOption>(base.Rng).Take<EventOption>(2));
				list3.Add(eventOption);
				return new <>z__ReadOnlyList<EventOption>(list3);
			}
			foreach (ModifierModel modifierModel in base.Owner.RunState.Modifiers)
			{
				Func<Task> neowOption = modifierModel.GenerateNeowOption(this);
				if (neowOption != null)
				{
					int optionIndex = this.ModifierOptions.Count;
					this.ModifierOptions.Add(new EventOption(this, () => this.OnModifierOptionSelected(neowOption, optionIndex), modifierModel.NeowOptionTitle, modifierModel.NeowOptionDescription, modifierModel.Id.Entry, modifierModel.HoverTips.ToArray<IHoverTip>()));
				}
			}
			if (this.ModifierOptions.Count > 0)
			{
				return new <>z__ReadOnlySingleElementList<EventOption>(this.ModifierOptions[0]);
			}
			return Array.Empty<EventOption>();
		}

		// Token: 0x06006845 RID: 26693 RVA: 0x00276A18 File Offset: 0x00274C18
		private async Task OnModifierOptionSelected(Func<Task> modifierFunc, int index)
		{
			await modifierFunc();
			if (index + 1 >= this.ModifierOptions.Count)
			{
				base.SetEventFinished(base.L10NLookup(base.Id.Entry + ".pages.DONE.description"));
			}
			else
			{
				this.SetEventState(this.InitialDescription, new <>z__ReadOnlySingleElementList<EventOption>(this.ModifierOptions[index + 1]));
			}
		}

		// Token: 0x04002790 RID: 10128
		private const string _cursedChoiceDoneDescriptionOverride = "NEOW.pages.DONE.CURSED.description";

		// Token: 0x04002791 RID: 10129
		private const string _positiveChoiceDoneDescriptionOverride = "NEOW.pages.DONE.POSITIVE.description";

		// Token: 0x04002792 RID: 10130
		private const string _sfxSleepy = "event:/sfx/npcs/neow/neow_sleepy";

		// Token: 0x04002793 RID: 10131
		private const string _sfxWelcome = "event:/sfx/npcs/neow/neow_welcome";

		// Token: 0x04002794 RID: 10132
		private const string _sfxCurious = "event:/sfx/npcs/neow/neow_curious";

		// Token: 0x04002795 RID: 10133
		[Nullable(new byte[] { 2, 1 })]
		private List<EventOption> _modifierOptions;
	}
}
