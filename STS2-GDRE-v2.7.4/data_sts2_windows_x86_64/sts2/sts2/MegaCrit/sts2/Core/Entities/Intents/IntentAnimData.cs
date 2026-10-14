using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Entities.Intents
{
	// Token: 0x02000C9C RID: 3228
	[NullableContext(1)]
	[Nullable(0)]
	public static class IntentAnimData
	{
		// Token: 0x17002407 RID: 9223
		// (get) Token: 0x060083F5 RID: 33781 RVA: 0x002C8667 File Offset: 0x002C6867
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return IntentAnimData._data.Values.SelectMany<IntentAnimData.InternalData, string>((IntentAnimData.InternalData v) => v.frames);
			}
		}

		// Token: 0x060083F6 RID: 33782 RVA: 0x002C8697 File Offset: 0x002C6897
		public static string GetAnimationFrame(string animation, int frame)
		{
			return IntentAnimData._data[animation].frames[frame];
		}

		// Token: 0x060083F7 RID: 33783 RVA: 0x002C86AB File Offset: 0x002C68AB
		public static int GetAnimationFrameCount(string animation)
		{
			return IntentAnimData._data[animation].frames.Length;
		}

		// Token: 0x04002D15 RID: 11541
		public const string attack1 = "attack_1";

		// Token: 0x04002D16 RID: 11542
		public const string attack2 = "attack_2";

		// Token: 0x04002D17 RID: 11543
		public const string attack3 = "attack_3";

		// Token: 0x04002D18 RID: 11544
		public const string attack4 = "attack_4";

		// Token: 0x04002D19 RID: 11545
		public const string attack5 = "attack_5";

		// Token: 0x04002D1A RID: 11546
		public const string buff = "buff";

		// Token: 0x04002D1B RID: 11547
		public const string cardDebuff = "card_debuff";

		// Token: 0x04002D1C RID: 11548
		public const string deathBlow = "death_blow";

		// Token: 0x04002D1D RID: 11549
		public const string debuff = "debuff";

		// Token: 0x04002D1E RID: 11550
		public const string defend = "defend";

		// Token: 0x04002D1F RID: 11551
		public const string escape = "escape";

		// Token: 0x04002D20 RID: 11552
		public const string heal = "heal";

		// Token: 0x04002D21 RID: 11553
		public const string hidden = "hidden";

		// Token: 0x04002D22 RID: 11554
		public const string sleep = "sleep";

		// Token: 0x04002D23 RID: 11555
		public const string status = "status";

		// Token: 0x04002D24 RID: 11556
		public const string stun = "stun";

		// Token: 0x04002D25 RID: 11557
		public const string summon = "summon";

		// Token: 0x04002D26 RID: 11558
		public const string unknown = "unknown";

		// Token: 0x04002D27 RID: 11559
		private static readonly Dictionary<string, IntentAnimData.InternalData> _data = new Dictionary<string, IntentAnimData.InternalData>
		{
			{
				"attack_1",
				new IntentAnimData.InternalData("attack/intent_attack_1")
			},
			{
				"attack_2",
				new IntentAnimData.InternalData("attack/intent_attack_2")
			},
			{
				"attack_3",
				new IntentAnimData.InternalData("attack/intent_attack_3")
			},
			{
				"attack_4",
				new IntentAnimData.InternalData("attack/intent_attack_4")
			},
			{
				"attack_5",
				new IntentAnimData.InternalData("attack/intent_attack_5")
			},
			{
				"buff",
				new IntentAnimData.InternalData("buff/intent_buff", 30)
			},
			{
				"card_debuff",
				new IntentAnimData.InternalData("card_debuff/intent_carddebuff", 15)
			},
			{
				"death_blow",
				new IntentAnimData.InternalData("intent_death_blow")
			},
			{
				"debuff",
				new IntentAnimData.InternalData("debuff/intent_megadebuff", 11)
			},
			{
				"defend",
				new IntentAnimData.InternalData("defend/intent_defend", 45)
			},
			{
				"escape",
				new IntentAnimData.InternalData("escape/intent_escape", 40)
			},
			{
				"heal",
				new IntentAnimData.InternalData("heal/intent_heal", 45)
			},
			{
				"hidden",
				new IntentAnimData.InternalData("intent_hidden")
			},
			{
				"sleep",
				new IntentAnimData.InternalData("sleep/intent_sleep", 16)
			},
			{
				"status",
				new IntentAnimData.InternalData("status/intent_statuscard", 19)
			},
			{
				"stun",
				new IntentAnimData.InternalData("stun/intent_stunned", 16)
			},
			{
				"summon",
				new IntentAnimData.InternalData("summon/intent_summon", 25)
			},
			{
				"unknown",
				new IntentAnimData.InternalData("unknown/intent_unknown", 30)
			}
		};

		// Token: 0x020023BE RID: 9150
		[Nullable(0)]
		private struct InternalData
		{
			// Token: 0x0600C1B4 RID: 49588 RVA: 0x00423D84 File Offset: 0x00421F84
			public InternalData(string prefix, int frameCount)
			{
				this.frames = new string[frameCount];
				for (int i = 0; i < frameCount; i++)
				{
					string[] array = this.frames;
					int num = i;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
					defaultInterpolatedStringHandler.AppendLiteral("atlases/intent_atlas.sprites/");
					defaultInterpolatedStringHandler.AppendFormatted(prefix);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted(i.ToString().PadLeft(2, '0'));
					defaultInterpolatedStringHandler.AppendLiteral(".tres");
					array[num] = ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}

			// Token: 0x0600C1B5 RID: 49589 RVA: 0x00423E09 File Offset: 0x00422009
			public InternalData(string singleFrameName)
			{
				this.frames = new string[1];
				this.frames[0] = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/" + singleFrameName + ".tres");
			}

			// Token: 0x0400976C RID: 38764
			public string[] frames;
		}
	}
}
