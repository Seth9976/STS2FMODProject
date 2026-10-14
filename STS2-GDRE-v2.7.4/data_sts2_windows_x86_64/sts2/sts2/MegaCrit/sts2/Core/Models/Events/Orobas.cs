using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000845 RID: 2117
	[NullableContext(1)]
	[Nullable(0)]
	public class Orobas : AncientEventModel
	{
		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x06006853 RID: 26707 RVA: 0x00276F5B File Offset: 0x0027515B
		public override Color ButtonColor
		{
			get
			{
				return new Color(0.05f, 0f, 0.1f, 0.35f);
			}
		}

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x06006854 RID: 26708 RVA: 0x00276F76 File Offset: 0x00275176
		public override Color DialogueColor
		{
			get
			{
				return new Color("5C5F7A");
			}
		}

		// Token: 0x06006855 RID: 26709 RVA: 0x00276F84 File Offset: 0x00275184
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
				new AncientDialogue(new string[] { "", "" })
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

		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x06006856 RID: 26710 RVA: 0x002772D0 File Offset: 0x002754D0
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				return new IEnumerable<EventOption>[]
				{
					this.OptionPool1,
					this.OptionPool2,
					this.OptionPool3,
					this.SeaGlassOptions,
					new <>z__ReadOnlySingleElementList<EventOption>(this.PrismaticGemOption)
				}.SelectMany<IEnumerable<EventOption>, EventOption>((IEnumerable<EventOption> x) => x);
			}
		}

		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x06006857 RID: 26711 RVA: 0x00277339 File Offset: 0x00275539
		private IEnumerable<EventOption> OptionPool1
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<ElectricShrymp>("INITIAL", null),
					base.RelicOption<GlassEye>("INITIAL", null)
				});
			}
		}

		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x06006858 RID: 26712 RVA: 0x00277364 File Offset: 0x00275564
		private IEnumerable<EventOption> OptionPool2
		{
			get
			{
				return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					base.RelicOption<AlchemicalCoffer>("INITIAL", null),
					base.RelicOption<Driftwood>("INITIAL", null),
					base.RelicOption<RadiantPearl>("INITIAL", null),
					base.RelicOption<SandCastle>("INITIAL", null)
				});
			}
		}

		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x06006859 RID: 26713 RVA: 0x002773B8 File Offset: 0x002755B8
		private IEnumerable<EventOption> SeaGlassOptions
		{
			get
			{
				List<EventOption> list = new List<EventOption>();
				foreach (CharacterModel characterModel in ModelDb.AllCharacters)
				{
					SeaGlass seaGlass = (SeaGlass)ModelDb.Relic<SeaGlass>().ToMutable();
					seaGlass.CharacterId = characterModel.Id;
					list.Add(base.RelicOption(seaGlass, "INITIAL", null));
				}
				return list;
			}
		}

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x0600685A RID: 26714 RVA: 0x00277434 File Offset: 0x00275634
		private EventOption PrismaticGemOption
		{
			get
			{
				return base.RelicOption<PrismaticGem>("INITIAL", null);
			}
		}

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x0600685B RID: 26715 RVA: 0x00277444 File Offset: 0x00275644
		private IEnumerable<EventOption> OptionPool3
		{
			get
			{
				List<EventOption> list = new List<EventOption>();
				TouchOfOrobas touchOfOrobas = (TouchOfOrobas)ModelDb.Relic<TouchOfOrobas>().ToMutable();
				if (base.Owner != null)
				{
					if (touchOfOrobas.SetupForPlayer(base.Owner))
					{
						list.Add(base.RelicOption(touchOfOrobas, "INITIAL", null));
					}
				}
				else
				{
					list.Add(base.RelicOption(touchOfOrobas, "INITIAL", null));
				}
				ArchaicTooth archaicTooth = (ArchaicTooth)ModelDb.Relic<ArchaicTooth>().ToMutable();
				if (base.Owner != null)
				{
					if (archaicTooth.SetupForPlayer(base.Owner))
					{
						list.Add(base.RelicOption(archaicTooth, "INITIAL", null));
					}
				}
				else
				{
					list.Add(base.RelicOption(archaicTooth, "INITIAL", null));
				}
				if (list.Count == 0)
				{
					list.Add(new EventOption(this, null, "OROBAS.pages.INITIAL.options.OPTION_POOL_3_LOCKED", Array.Empty<IHoverTip>()));
				}
				return list;
			}
		}

		// Token: 0x0600685C RID: 26716 RVA: 0x00277514 File Offset: 0x00275714
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			CharacterModel character = base.Owner.Character;
			CharacterModel characterModel = base.Rng.NextItem<CharacterModel>(base.Owner.UnlockState.Characters.Where<CharacterModel>((CharacterModel c) => c.Id != character.Id));
			if (characterModel == null)
			{
				characterModel = character;
			}
			List<EventOption> list = this.OptionPool1.ToList<EventOption>();
			EventOption eventOption;
			if (base.Rng.NextFloat(1f) < 0.3333333f)
			{
				eventOption = this.PrismaticGemOption;
			}
			else
			{
				SeaGlass seaGlass = (SeaGlass)ModelDb.Relic<SeaGlass>().ToMutable();
				seaGlass.CharacterId = characterModel.Id;
				eventOption = base.RelicOption(seaGlass, "INITIAL", null);
			}
			list.Add(eventOption);
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				base.Rng.NextItem<EventOption>(list),
				base.Rng.NextItem<EventOption>(this.OptionPool2),
				base.Rng.NextItem<EventOption>(this.OptionPool3)
			});
		}

		// Token: 0x0400279A RID: 10138
		private const float _prismaticOdds = 0.3333333f;
	}
}
