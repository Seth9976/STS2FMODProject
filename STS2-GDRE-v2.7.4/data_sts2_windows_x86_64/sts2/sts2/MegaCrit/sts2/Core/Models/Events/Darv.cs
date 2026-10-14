using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000832 RID: 2098
	[NullableContext(1)]
	[Nullable(0)]
	public class Darv : AncientEventModel
	{
		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x060067AA RID: 26538 RVA: 0x0027387F File Offset: 0x00271A7F
		public override Color ButtonColor
		{
			get
			{
				return new Color(0.06f, 0f, 0.08f, 0.5f);
			}
		}

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x060067AB RID: 26539 RVA: 0x0027389A File Offset: 0x00271A9A
		public override Color DialogueColor
		{
			get
			{
				return new Color("512E66");
			}
		}

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x060067AC RID: 26540 RVA: 0x002738A8 File Offset: 0x00271AA8
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				return (from r in Darv._validRelicSets.SelectMany<Darv.ValidRelicSet, RelicModel>((Darv.ValidRelicSet s) => s.relics)
					select base.RelicOption(r.ToMutable(), "INITIAL", null)).Concat<EventOption>(new <>z__ReadOnlySingleElementList<EventOption>(base.RelicOption<DustyTome>("INITIAL", null)));
			}
		}

		// Token: 0x060067AD RID: 26541 RVA: 0x00273908 File Offset: 0x00271B08
		protected override AncientDialogueSet DefineDialogues()
		{
			AncientDialogueSet ancientDialogueSet = new AncientDialogueSet();
			ancientDialogueSet.FirstVisitEverDialogue = new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_introduction" });
			AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet;
			Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
			string text = AncientEventModel.CharKey<Ironclad>();
			dictionary[text] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_introduction" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_endeared" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text2 = AncientEventModel.CharKey<Silent>();
			dictionary[text2] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_introduction" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_excited" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_pain", "", "event:/sfx/npcs/darv/darv_outta_the_way" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text3 = AncientEventModel.CharKey<Defect>();
			dictionary[text3] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_introduction", "" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_endeared" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_fear", "", "event:/sfx/npcs/darv/darv_fear" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text4 = AncientEventModel.CharKey<Necrobinder>();
			dictionary[text4] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_introduction", "", "event:/sfx/npcs/darv/darv_excited" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_endeared" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_excited", "", "event:/sfx/npcs/darv/darv_fear" })
				{
					VisitIndex = new int?(4)
				}
			});
			string text5 = AncientEventModel.CharKey<Regent>();
			dictionary[text5] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_introduction", "", "event:/sfx/npcs/darv/darv_excited" })
				{
					VisitIndex = new int?(0)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_introduction" })
				{
					VisitIndex = new int?(1)
				},
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_excited", "", "event:/sfx/npcs/darv/darv_pain" })
				{
					VisitIndex = new int?(4)
				}
			});
			ancientDialogueSet2.CharacterDialogues = dictionary;
			ancientDialogueSet.AgnosticDialogues = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_excited" }),
				new AncientDialogue(new string[] { "event:/sfx/npcs/darv/darv_outta_the_way" })
			});
			return ancientDialogueSet;
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x00273C74 File Offset: 0x00271E74
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = (from rs in Darv._validRelicSets
				where rs.filter(base.Owner)
				select base.RelicOption(base.Rng.NextItem<RelicModel>(rs.relics).ToMutable(), "INITIAL", null)).ToList<EventOption>().UnstableShuffle<EventOption>(base.Rng);
			List<EventOption> list2;
			if (base.Rng.NextBool())
			{
				list2 = list.Take<EventOption>(2).ToList<EventOption>();
				DustyTome dustyTome = (DustyTome)ModelDb.Relic<DustyTome>().ToMutable();
				if (base.Owner != null)
				{
					dustyTome.SetupForPlayer(base.Owner);
				}
				list2.Add(base.RelicOption(dustyTome, "INITIAL", null));
			}
			else
			{
				list2 = list.Take<EventOption>(3).ToList<EventOption>();
			}
			return list2;
		}

		// Token: 0x060067B0 RID: 26544 RVA: 0x00273D24 File Offset: 0x00271F24
		// Note: this type is marked as 'beforefieldinit'.
		unsafe static Darv()
		{
			int num = 11;
			List<Darv.ValidRelicSet> list = new List<Darv.ValidRelicSet>(num);
			CollectionsMarshal.SetCount<Darv.ValidRelicSet>(list, num);
			Span<Darv.ValidRelicSet> span = CollectionsMarshal.AsSpan<Darv.ValidRelicSet>(list);
			int num2 = 0;
			*span[num2] = new Darv.ValidRelicSet(new RelicModel[] { ModelDb.Relic<Astrolabe>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet(new RelicModel[] { ModelDb.Relic<BlackStar>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet(new RelicModel[] { ModelDb.Relic<CallingBell>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet(new RelicModel[] { ModelDb.Relic<EmptyCage>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet((Player owner) => !owner.RunState.Modifiers.Any<ModifierModel>((ModifierModel m) => m.ClearsPlayerDeck), new RelicModel[] { ModelDb.Relic<PandorasBox>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet(new RelicModel[] { ModelDb.Relic<RunicPyramid>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet(new RelicModel[] { ModelDb.Relic<SneckoEye>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet((Player owner) => owner.RunState.CurrentActIndex == 1, new RelicModel[] { ModelDb.Relic<Ectoplasm>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet((Player owner) => owner.RunState.CurrentActIndex == 1, new RelicModel[] { ModelDb.Relic<Sozu>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet((Player owner) => owner.RunState.CurrentActIndex >= 1, new RelicModel[] { ModelDb.Relic<PhilosophersStone>() });
			num2++;
			*span[num2] = new Darv.ValidRelicSet((Player owner) => owner.RunState.CurrentActIndex >= 1, new RelicModel[] { ModelDb.Relic<VelvetChoker>() });
			Darv._validRelicSets = list;
		}

		// Token: 0x0400276A RID: 10090
		private const string _sfxExcited = "event:/sfx/npcs/darv/darv_excited";

		// Token: 0x0400276B RID: 10091
		private const string _sfxOuttaTheWay = "event:/sfx/npcs/darv/darv_outta_the_way";

		// Token: 0x0400276C RID: 10092
		private const string _sfxFear = "event:/sfx/npcs/darv/darv_fear";

		// Token: 0x0400276D RID: 10093
		private const string _sfxPain = "event:/sfx/npcs/darv/darv_pain";

		// Token: 0x0400276E RID: 10094
		private const string _sfxEndeared = "event:/sfx/npcs/darv/darv_endeared";

		// Token: 0x0400276F RID: 10095
		private const string _sfxIntroduction = "event:/sfx/npcs/darv/darv_introduction";

		// Token: 0x04002770 RID: 10096
		private static readonly List<Darv.ValidRelicSet> _validRelicSets;

		// Token: 0x02001EFD RID: 7933
		[Nullable(0)]
		private struct ValidRelicSet
		{
			// Token: 0x0600B6BD RID: 46781 RVA: 0x003C3FA6 File Offset: 0x003C21A6
			public ValidRelicSet(Func<Player, bool> filter, RelicModel[] relics)
			{
				this.filter = filter;
				this.relics = relics;
			}

			// Token: 0x0600B6BE RID: 46782 RVA: 0x003C3FB6 File Offset: 0x003C21B6
			public ValidRelicSet(RelicModel[] relics)
			{
				this.filter = (Player _) => true;
				this.relics = relics;
			}

			// Token: 0x04007E82 RID: 32386
			public readonly Func<Player, bool> filter;

			// Token: 0x04007E83 RID: 32387
			public readonly RelicModel[] relics;
		}
	}
}
