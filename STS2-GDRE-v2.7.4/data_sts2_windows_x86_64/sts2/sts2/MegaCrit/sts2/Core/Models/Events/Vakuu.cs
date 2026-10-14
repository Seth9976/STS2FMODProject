using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000864 RID: 2148
	[NullableContext(1)]
	[Nullable(0)]
	public class Vakuu : AncientEventModel
	{
		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x06006995 RID: 27029 RVA: 0x0027D223 File Offset: 0x0027B423
		public override Color ButtonColor
		{
			get
			{
				return new Color(0.05f, 0.06f, 0.12f, 0.8f);
			}
		}

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x06006996 RID: 27030 RVA: 0x0027D23E File Offset: 0x0027B43E
		public override Color DialogueColor
		{
			get
			{
				return new Color("3C1931");
			}
		}

		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x06006997 RID: 27031 RVA: 0x0027D24A File Offset: 0x0027B44A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Visits", 0m));
			}
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x0027D260 File Offset: 0x0027B460
		public override void CalculateVars()
		{
			base.CalculateVars();
			if (!LocalContext.IsMe(base.Owner))
			{
				return;
			}
			AncientStats valueOrDefault = SaveManager.Instance.Progress.AncientStats.GetValueOrDefault(base.Id);
			int num = ((valueOrDefault != null) ? valueOrDefault.GetVisitsAs(base.Owner.Character.Id) : 0);
			base.DynamicVars["Visits"].BaseValue = num + 1;
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x0027D2D8 File Offset: 0x0027B4D8
		protected override AncientDialogueSet DefineDialogues()
		{
			AncientDialogueSet ancientDialogueSet = new AncientDialogueSet();
			ancientDialogueSet.FirstVisitEverDialogue = new AncientDialogue(new string[] { "" });
			AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet;
			Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
			string text = AncientEventModel.CharKey<Ironclad>();
			dictionary[text] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
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
				new AncientDialogue(new string[] { "" })
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
				new AncientDialogue(new string[] { "" }),
				new AncientDialogue(new string[] { "" })
			});
			return ancientDialogueSet;
		}

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x0600699A RID: 27034 RVA: 0x0027D669 File Offset: 0x0027B869
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				return this.Pool1.Concat<EventOption>(this.Pool2).Concat<EventOption>(this.Pool3);
			}
		}

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x0600699B RID: 27035 RVA: 0x0027D687 File Offset: 0x0027B887
		private IEnumerable<EventOption> Pool1
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<BloodSoakedRose>("INITIAL", null),
					base.RelicOption<WhisperingEarring>("INITIAL", null),
					base.RelicOption<Fiddle>("INITIAL", null)
				});
			}
		}

		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x0600699C RID: 27036 RVA: 0x0027D6C4 File Offset: 0x0027B8C4
		private IEnumerable<EventOption> Pool2
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<PreservedFog>("INITIAL", null),
					base.RelicOption<SereTalon>("INITIAL", null).ThatDecreasesMaxHp(9m),
					base.RelicOption<DistinguishedCape>("INITIAL", null)
				});
			}
		}

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x0600699D RID: 27037 RVA: 0x0027D718 File Offset: 0x0027B918
		private IEnumerable<EventOption> Pool3
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<ChoicesParadox>("INITIAL", null),
					base.RelicOption<MusicBox>("INITIAL", null),
					base.RelicOption<LordsParasol>("INITIAL", null),
					base.RelicOption<JeweledMask>("INITIAL", null)
				});
			}
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x0027D76C File Offset: 0x0027B96C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = this.Pool1.ToList<EventOption>();
			List<EventOption> list2 = this.Pool2.ToList<EventOption>();
			List<EventOption> list3 = this.Pool3.ToList<EventOption>();
			list.UnstableShuffle<EventOption>(base.Rng);
			list2.UnstableShuffle<EventOption>(base.Rng);
			list3.UnstableShuffle<EventOption>(base.Rng);
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				list[0],
				list2[0],
				list3[0]
			});
		}

		// Token: 0x040027E6 RID: 10214
		private const string _visitsKey = "Visits";
	}
}
