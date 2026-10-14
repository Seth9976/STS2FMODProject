using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004C4 RID: 1220
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class AttackIntent : AbstractIntent
	{
		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x0600491C RID: 18716 RVA: 0x00225B2B File Offset: 0x00223D2B
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Attack;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x0600491D RID: 18717 RVA: 0x00225B2E File Offset: 0x00223D2E
		protected override string IntentPrefix
		{
			get
			{
				return "ATTACK";
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x0600491E RID: 18718 RVA: 0x00225B35 File Offset: 0x00223D35
		// (set) Token: 0x0600491F RID: 18719 RVA: 0x00225B3D File Offset: 0x00223D3D
		[Nullable(2)]
		public Func<decimal> DamageCalc
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			protected set;
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06004920 RID: 18720 RVA: 0x00225B46 File Offset: 0x00223D46
		public virtual int Repeats
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06004921 RID: 18721 RVA: 0x00225B49 File Offset: 0x00223D49
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_attack.tres";
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06004922 RID: 18722 RVA: 0x00225B50 File Offset: 0x00223D50
		public override IEnumerable<string> AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				for (int i = 1; i <= 5; i++)
				{
					List<string> list2 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
					defaultInterpolatedStringHandler.AppendLiteral("atlases/intent_atlas.sprites/attack/intent_attack_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(".tres");
					list2.Add(ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear()));
				}
				return list;
			}
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x00225BAC File Offset: 0x00223DAC
		public override Texture2D GetTexture(IEnumerable<Creature> targets, Creature owner)
		{
			int totalDamage = this.GetTotalDamage(targets, owner);
			string text = "";
			if (totalDamage < 5)
			{
				text += "1";
			}
			else if (totalDamage < 10)
			{
				text += "2";
			}
			else if (totalDamage < 20)
			{
				text += "3";
			}
			else if (totalDamage < 40)
			{
				text += "4";
			}
			else
			{
				text += "5";
			}
			string imagePath = ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_" + text + ".tres");
			return PreloadManager.Cache.GetTexture2D(imagePath);
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00225C40 File Offset: 0x00223E40
		public override string GetAnimation(IEnumerable<Creature> targets, Creature owner)
		{
			string text = base.GetAnimation(targets, owner);
			int totalDamage = this.GetTotalDamage(targets, owner);
			if (totalDamage < 5)
			{
				text += "_1";
			}
			else if (totalDamage < 10)
			{
				text += "_2";
			}
			else if (totalDamage < 20)
			{
				text += "_3";
			}
			else if (totalDamage < 40)
			{
				text += "_4";
			}
			else
			{
				text += "_5";
			}
			return text;
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00225CB8 File Offset: 0x00223EB8
		protected override LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
		{
			LocString intentDescription = base.GetIntentDescription(targets, owner);
			intentDescription.Add("Damage", this.GetSingleDamage(targets, owner));
			intentDescription.Add("Repeat", this.Repeats);
			return intentDescription;
		}

		// Token: 0x06004926 RID: 18726
		public abstract int GetTotalDamage(IEnumerable<Creature> targets, Creature owner);

		// Token: 0x06004927 RID: 18727 RVA: 0x00225D00 File Offset: 0x00223F00
		public int GetSingleDamage(IEnumerable<Creature> targets, Creature owner)
		{
			decimal num = this.DamageCalc();
			Player me = LocalContext.GetMe(owner.CombatState);
			if (me != null)
			{
				IEnumerable<AbstractModel> enumerable;
				num = Hook.ModifyDamage(me.RunState, me.Creature.CombatState, me.Creature, owner, this.DamageCalc(), ValueProp.Move, null, null, ModifyDamageHookType.All, CardPreviewMode.None, out enumerable);
			}
			return Math.Max(0, (int)num);
		}
	}
}
