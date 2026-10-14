using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200085B RID: 2139
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheArchitect : EventModel
	{
		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x06006920 RID: 26912 RVA: 0x0027AF33 File Offset: 0x00279133
		public override EventLayoutType LayoutType
		{
			get
			{
				return EventLayoutType.Combat;
			}
		}

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x06006921 RID: 26913 RVA: 0x0027AF36 File Offset: 0x00279136
		public override EncounterModel CanonicalEncounter
		{
			get
			{
				return ModelDb.Encounter<TheArchitectEventEncounter>();
			}
		}

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x06006922 RID: 26914 RVA: 0x0027AF3D File Offset: 0x0027913D
		public override string LocTable
		{
			get
			{
				return "ancients";
			}
		}

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x06006923 RID: 26915 RVA: 0x0027AF44 File Offset: 0x00279144
		// (set) Token: 0x06006924 RID: 26916 RVA: 0x0027AF4C File Offset: 0x0027914C
		[Nullable(2)]
		private AncientDialogue Dialogue
		{
			[NullableContext(2)]
			get
			{
				return this._dialogue;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._dialogue = value;
			}
		}

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x06006925 RID: 26917 RVA: 0x0027AF5B File Offset: 0x0027915B
		// (set) Token: 0x06006926 RID: 26918 RVA: 0x0027AF63 File Offset: 0x00279163
		private int CurrentLineIndex
		{
			get
			{
				return this._currentLineIndex;
			}
			set
			{
				base.AssertMutable();
				this._currentLineIndex = value;
			}
		}

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x06006927 RID: 26919 RVA: 0x0027AF72 File Offset: 0x00279172
		// (set) Token: 0x06006928 RID: 26920 RVA: 0x0027AF7A File Offset: 0x0027917A
		[Nullable(2)]
		private NSpeechBubbleVfx SpeechBubble
		{
			[NullableContext(2)]
			get
			{
				return this._speechBubble;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._speechBubble = value;
			}
		}

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x06006929 RID: 26921 RVA: 0x0027AF89 File Offset: 0x00279189
		// (set) Token: 0x0600692A RID: 26922 RVA: 0x0027AF91 File Offset: 0x00279191
		[Nullable(2)]
		private Creature ArchitectCreature
		{
			[NullableContext(2)]
			get
			{
				return this._architectCreature;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._architectCreature = value;
			}
		}

		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x0600692B RID: 26923 RVA: 0x0027AFA0 File Offset: 0x002791A0
		// (set) Token: 0x0600692C RID: 26924 RVA: 0x0027AFA8 File Offset: 0x002791A8
		private int Score
		{
			get
			{
				return this._score;
			}
			set
			{
				base.AssertMutable();
				this._score = value;
			}
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x0600692D RID: 26925 RVA: 0x0027AFB7 File Offset: 0x002791B7
		public override IEnumerable<LocString> GameInfoOptions
		{
			get
			{
				return Array.Empty<LocString>();
			}
		}

		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x0600692E RID: 26926 RVA: 0x0027AFBE File Offset: 0x002791BE
		private bool IsOnLastLine
		{
			get
			{
				return this.Dialogue == null || this.CurrentLineIndex >= this.Dialogue.Lines.Count - 1;
			}
		}

		// Token: 0x0600692F RID: 26927 RVA: 0x0027AFE7 File Offset: 0x002791E7
		protected override void SetInitialEventState(bool isPreFinished)
		{
			this.SetEventState(TheArchitect._emptyLocString, this.GenerateInitialOptionsWrapper());
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x0027AFFA File Offset: 0x002791FA
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			this.LoadDialogue();
			if (this.Dialogue == null || this.Dialogue.Lines.Count == 0)
			{
				return new <>z__ReadOnlySingleElementList<EventOption>(this.CreateProceedOption());
			}
			this.CurrentLineIndex = 0;
			return new <>z__ReadOnlySingleElementList<EventOption>(this.CreateOptionForCurrentLine());
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x0027B03C File Offset: 0x0027923C
		public override void OnRoomEnter()
		{
			StatsManager.RefreshGlobalStats();
			NCombatRoom instance = NCombatRoom.Instance;
			Creature creature;
			if (instance == null)
			{
				creature = null;
			}
			else
			{
				NCreature ncreature = instance.CreatureNodes.FirstOrDefault<NCreature>((NCreature n) => n.Entity.Side == CombatSide.Enemy);
				creature = ((ncreature != null) ? ncreature.Entity : null);
			}
			this.ArchitectCreature = creature;
			this.Score = ScoreUtility.CalculateScore(base.Owner.RunState, true);
			if (LocalContext.IsMe(base.Owner))
			{
				if (this.ArchitectCreature != null)
				{
					MegaAnimationState architectAnimationState = this.GetArchitectAnimationState();
					if (architectAnimationState != null)
					{
						architectAnimationState.SetAnimation("_tracks/head_reading", true, 1);
					}
				}
				AncientDialogue dialogue = this.Dialogue;
				if (dialogue != null)
				{
					IReadOnlyList<AncientDialogueLine> lines = dialogue.Lines;
					if (lines != null && lines.Count > 0)
					{
						base.ClearCurrentOptions();
					}
				}
			}
			TaskHelper.RunSafely(this.PlayCurrentLine());
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x0027B108 File Offset: 0x00279308
		public void TriggerVictory()
		{
			if (LocalContext.IsMe(base.Owner))
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance == null)
				{
					return;
				}
				instance.SetWaitingForOtherPlayersOverlayVisible(false);
			}
		}

		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x06006933 RID: 26931 RVA: 0x0027B127 File Offset: 0x00279327
		public AncientDialogueSet DialogueSet
		{
			get
			{
				if (this._dialogueSet == null)
				{
					this._dialogueSet = TheArchitect.DefineDialogues();
					this._dialogueSet.PopulateLocKeys(base.Id.Entry);
				}
				return this._dialogueSet;
			}
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x0027B158 File Offset: 0x00279358
		private static string CharKey<[Nullable(0)] T>() where T : CharacterModel
		{
			return ModelDb.Character<T>().Id.Entry;
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x0027B170 File Offset: 0x00279370
		private static AncientDialogueSet DefineDialogues()
		{
			AncientDialogueSet ancientDialogueSet = new AncientDialogueSet();
			ancientDialogueSet.FirstVisitEverDialogue = null;
			AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet;
			Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
			string text = TheArchitect.CharKey<Ironclad>();
			dictionary[text] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "" })
				{
					VisitIndex = new int?(0),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(1),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(2),
					EndAttackers = ArchitectAttackers.Both
				}
			});
			string text2 = TheArchitect.CharKey<Silent>();
			dictionary[text2] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(0),
					StartAttackers = ArchitectAttackers.Player,
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(1),
					StartAttackers = ArchitectAttackers.Player,
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(2),
					StartAttackers = ArchitectAttackers.Player,
					EndAttackers = ArchitectAttackers.Architect
				},
				new AncientDialogue(new string[] { "" })
				{
					VisitIndex = new int?(3),
					StartAttackers = ArchitectAttackers.Player,
					EndAttackers = ArchitectAttackers.Architect
				}
			});
			string text3 = TheArchitect.CharKey<Defect>();
			dictionary[text3] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(0),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(1),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(2),
					EndAttackers = ArchitectAttackers.Both
				}
			});
			string text4 = TheArchitect.CharKey<Necrobinder>();
			dictionary[text4] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "" })
				{
					VisitIndex = new int?(0),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "" })
				{
					VisitIndex = new int?(1),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "" })
				{
					VisitIndex = new int?(2),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(3),
					EndAttackers = ArchitectAttackers.Both
				}
			});
			string text5 = TheArchitect.CharKey<Regent>();
			dictionary[text5] = new <>z__ReadOnlyArray<AncientDialogue>(new AncientDialogue[]
			{
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(0),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(1),
					EndAttackers = ArchitectAttackers.Both
				},
				new AncientDialogue(new string[] { "", "", "" })
				{
					VisitIndex = new int?(2),
					EndAttackers = ArchitectAttackers.Both
				}
			});
			ancientDialogueSet2.CharacterDialogues = dictionary;
			ancientDialogueSet.AgnosticDialogues = Array.Empty<AncientDialogue>();
			return ancientDialogueSet;
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x0027B5A8 File Offset: 0x002797A8
		private void LoadDialogue()
		{
			CharacterStats statsForCharacter = SaveManager.Instance.Progress.GetStatsForCharacter(base.Owner.Character.Id);
			int num = ((statsForCharacter != null) ? statsForCharacter.TotalWins : 0);
			int wins = SaveManager.Instance.Progress.Wins;
			List<AncientDialogue> list = this.DialogueSet.GetValidDialogues(base.Owner.Character.Id, num, wins, false).ToList<AncientDialogue>();
			this.Dialogue = base.Rng.NextItem<AncientDialogue>(list);
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x0027B628 File Offset: 0x00279828
		private EventOption CreateOptionForCurrentLine()
		{
			AncientDialogueLine ancientDialogueLine = this.Dialogue.Lines[this.CurrentLineIndex];
			EventOption eventOption;
			if (this.IsOnLastLine)
			{
				eventOption = this.CreateProceedOption();
			}
			else
			{
				LocString locString;
				if (ancientDialogueLine.NextButtonText != null)
				{
					locString = ancientDialogueLine.NextButtonText;
				}
				else if (ancientDialogueLine.Speaker == AncientDialogueSpeaker.Ancient)
				{
					locString = TheArchitect._respondLocString;
				}
				else
				{
					locString = TheArchitect._continueLocString;
				}
				Func<Task> func = new Func<Task>(this.AdvanceDialogue);
				LocString locString2 = locString;
				LocString emptyLocString = TheArchitect._emptyLocString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(".dialogue.");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurrentLineIndex);
				eventOption = new EventOption(this, func, locString2, emptyLocString, defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<IHoverTip>()).ThatWontSaveToChoiceHistory();
			}
			return eventOption;
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x0027B6E8 File Offset: 0x002798E8
		private EventOption CreateProceedOption()
		{
			return new EventOption(this, new Func<Task>(this.WinRun), "PROCEED", false, false, Array.Empty<IHoverTip>()).ThatWontSaveToChoiceHistory();
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x0027B710 File Offset: 0x00279910
		private async Task AdvanceDialogue()
		{
			int currentLineIndex = this.CurrentLineIndex;
			this.CurrentLineIndex = currentLineIndex + 1;
			IEnumerable<EventOption> enumerable;
			if (this.CurrentLineIndex < this.Dialogue.Lines.Count)
			{
				await this.PlayCurrentLine();
				enumerable = new <>z__ReadOnlySingleElementList<EventOption>(this.CreateOptionForCurrentLine());
			}
			else
			{
				enumerable = new <>z__ReadOnlySingleElementList<EventOption>(this.CreateProceedOption());
			}
			this.SetEventState(TheArchitect._emptyLocString, enumerable);
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x0027B754 File Offset: 0x00279954
		private async Task WinRun()
		{
			if (LocalContext.IsMe(base.Owner))
			{
				await this.AnimPlayerAttackIfNecessary(this.Dialogue.EndAttackers);
				await this.AnimArchitectAttackIfNecessary(this.Dialogue.EndAttackers);
				if (base.Owner.RunState.Players.Count > 1)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						instance.SetWaitingForOtherPlayersOverlayVisible(true);
					}
				}
				RunManager.Instance.ActChangeSynchronizer.SetLocalPlayerReady();
			}
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x0027B798 File Offset: 0x00279998
		private async Task<bool> AnimPlayerAttackIfNecessary(ArchitectAttackers attackers)
		{
			bool flag;
			if (attackers != ArchitectAttackers.Player && attackers != ArchitectAttackers.Both)
			{
				flag = false;
			}
			else if (this.ArchitectCreature == null)
			{
				flag = false;
			}
			else
			{
				List<string> shuffledVfx = base.Owner.Character.GetArchitectAttackVfx();
				base.Rng.Shuffle<string>(shuffledVfx);
				int[] damageParts = TheArchitect.DivideWildly(this.Score, shuffledVfx.Count, base.Rng);
				NCombatRoom instance = NCombatRoom.Instance;
				Control vfxContainer = ((instance != null) ? instance.CombatVfxContainer : null);
				for (int i = 0; i < shuffledVfx.Count; i++)
				{
					bool isFinalHit = i == shuffledVfx.Count - 1;
					await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", 0.1f);
					Control control = vfxContainer;
					if (control != null)
					{
						control.AddChildSafely(NDamageNumVfx.Create(this.ArchitectCreature, damageParts[i], false));
					}
					Control control2 = vfxContainer;
					if (control2 != null)
					{
						control2.AddChildSafely(NHitSparkVfx.Create(this.ArchitectCreature, false));
					}
					VfxCmd.PlayOnCreatureCenter(this.ArchitectCreature, shuffledVfx[i]);
					await CreatureCmd.TriggerAnim(this.ArchitectCreature, "Hit", 0f);
					if (isFinalHit)
					{
						NGame instance2 = NGame.Instance;
						if (instance2 != null)
						{
							instance2.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, -1f);
						}
					}
					else
					{
						NGame instance3 = NGame.Instance;
						if (instance3 != null)
						{
							instance3.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, -1f);
						}
					}
					if (!isFinalHit)
					{
						await Cmd.Wait(0.1f, false);
					}
				}
				await Cmd.Wait(2f, false);
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x0027B7E4 File Offset: 0x002799E4
		private static int[] DivideWildly(int total, int parts, Rng rng)
		{
			if (parts <= 0)
			{
				return Array.Empty<int>();
			}
			if (parts == 1)
			{
				return new int[] { total };
			}
			double[] array = new double[parts];
			int num = rng.NextInt(parts);
			int num2;
			do
			{
				num2 = rng.NextInt(parts);
			}
			while (num2 == num);
			for (int i = 0; i < parts; i++)
			{
				if (i == num)
				{
					array[i] = (double)rng.NextFloat(2f, 3f);
				}
				else if (i == num2)
				{
					array[i] = (double)rng.NextFloat(0.1f, 0.5f);
				}
				else
				{
					array[i] = (double)rng.NextFloat(0.7f, 1.3f);
				}
			}
			double num3 = array.Sum();
			int[] array2 = new int[parts];
			int num4 = 0;
			for (int j = 0; j < parts - 1; j++)
			{
				array2[j] = Math.Max(1, (int)((double)total * array[j] / num3));
				num4 += array2[j];
			}
			array2[parts - 1] = Math.Max(1, total - num4);
			return array2;
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x0027B8D8 File Offset: 0x00279AD8
		private async Task AnimArchitectAttackIfNecessary(ArchitectAttackers attackers)
		{
			if (attackers - ArchitectAttackers.Architect <= 1)
			{
				if (this.ArchitectCreature != null)
				{
					await CreatureCmd.TriggerAnim(this.ArchitectCreature, "Attack", 0.5f);
					VfxCmd.PlayOnCreature(base.Owner.Creature, "vfx/vfx_attack_lightning");
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						instance.CombatVfxContainer.AddChildSafely(NFireBurstVfx.Create(base.Owner.Creature, 1f));
					}
					await Cmd.Wait(0.5f, false);
				}
			}
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x0027B924 File Offset: 0x00279B24
		private async Task PlayCurrentLine()
		{
			if (LocalContext.IsMe(base.Owner))
			{
				if (this.SpeechBubble != null)
				{
					TaskHelper.RunSafely(this.SpeechBubble.AnimOut());
					this.SpeechBubble = null;
				}
				if (this.Dialogue != null && this.CurrentLineIndex < this.Dialogue.Lines.Count)
				{
					AncientDialogueLine line = this.Dialogue.Lines[this.CurrentLineIndex];
					if (line.LineText != null)
					{
						Creature speaker = this.GetSpeaker(line.Speaker);
						if (speaker != null)
						{
							if (this.CurrentLineIndex == 0)
							{
								if (this.Dialogue.StartAttackers != ArchitectAttackers.None || this.Dialogue.EndAttackers != ArchitectAttackers.None)
								{
									await Cmd.Wait(0.75f, false);
								}
								bool flag = await this.AnimPlayerAttackIfNecessary(this.Dialogue.StartAttackers);
								if (this.ArchitectCreature != null)
								{
									if (!flag)
									{
										await Cmd.Wait(1f, false);
									}
									MegaAnimationState state = this.GetArchitectAnimationState();
									MegaAnimationState megaAnimationState = state;
									if (megaAnimationState != null)
									{
										megaAnimationState.SetAnimation("_tracks/head_stop_reading", false, 1);
									}
									await Cmd.Wait(0.5f, false);
									MegaAnimationState megaAnimationState2 = state;
									if (megaAnimationState2 != null)
									{
										megaAnimationState2.SetAnimation("_tracks/head_normal", true, 1);
									}
									state = null;
								}
								await this.AnimArchitectAttackIfNecessary(this.Dialogue.StartAttackers);
							}
							this.ShowSpeechBubble(line, speaker);
							if (this.CurrentLineIndex == 0)
							{
								this.SetEventState(TheArchitect._emptyLocString, new <>z__ReadOnlySingleElementList<EventOption>(this.CreateOptionForCurrentLine()));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x0027B967 File Offset: 0x00279B67
		private void ShowSpeechBubble(AncientDialogueLine line, Creature speaker)
		{
			this.SpeechBubble = TalkCmd.Play(line.LineText, speaker, (line.Speaker == AncientDialogueSpeaker.Ancient) ? VfxColor.DarkGray : base.Owner.Character.SpeechBubbleColor, VfxDuration.Forever);
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x0027B99C File Offset: 0x00279B9C
		[NullableContext(2)]
		private Creature GetSpeaker(AncientDialogueSpeaker speaker)
		{
			Creature creature;
			if (speaker != AncientDialogueSpeaker.Ancient)
			{
				if (speaker != AncientDialogueSpeaker.Character)
				{
					creature = null;
				}
				else
				{
					creature = base.Owner.Creature;
				}
			}
			else
			{
				creature = this.ArchitectCreature;
			}
			return creature;
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x0027B9D0 File Offset: 0x00279BD0
		[NullableContext(2)]
		private MegaAnimationState GetArchitectAnimationState()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return null;
			}
			NCreature creatureNode = instance.GetCreatureNode(this.ArchitectCreature);
			if (creatureNode == null)
			{
				return null;
			}
			return creatureNode.SpineAnimation.GetAnimationState();
		}

		// Token: 0x040027CF RID: 10191
		private const string _locTable = "ancients";

		// Token: 0x040027D0 RID: 10192
		private static readonly LocString _emptyLocString = new LocString("ancients", "PROCEED.description");

		// Token: 0x040027D1 RID: 10193
		private static readonly LocString _continueLocString = new LocString("ancients", "THE_ARCHITECT.CONTINUE");

		// Token: 0x040027D2 RID: 10194
		private static readonly LocString _respondLocString = new LocString("ancients", "THE_ARCHITECT.RESPOND");

		// Token: 0x040027D3 RID: 10195
		[Nullable(2)]
		private AncientDialogue _dialogue;

		// Token: 0x040027D4 RID: 10196
		private int _currentLineIndex;

		// Token: 0x040027D5 RID: 10197
		[Nullable(2)]
		private NSpeechBubbleVfx _speechBubble;

		// Token: 0x040027D6 RID: 10198
		[Nullable(2)]
		private Creature _architectCreature;

		// Token: 0x040027D7 RID: 10199
		private int _score;

		// Token: 0x040027D8 RID: 10200
		[Nullable(2)]
		private AncientDialogueSet _dialogueSet;
	}
}
