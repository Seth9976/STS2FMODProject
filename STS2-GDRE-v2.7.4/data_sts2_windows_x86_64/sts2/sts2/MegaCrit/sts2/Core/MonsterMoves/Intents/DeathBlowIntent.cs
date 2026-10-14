using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004C7 RID: 1223
	[NullableContext(1)]
	[Nullable(0)]
	public class DeathBlowIntent : SingleAttackIntent
	{
		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06004931 RID: 18737 RVA: 0x00225DA0 File Offset: 0x00223FA0
		protected override string IntentPrefix
		{
			get
			{
				return "DEATH_BLOW";
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06004932 RID: 18738 RVA: 0x00225DA7 File Offset: 0x00223FA7
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_death_blow.tres";
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06004933 RID: 18739 RVA: 0x00225DAE File Offset: 0x00223FAE
		public override IntentType IntentType
		{
			get
			{
				return IntentType.DeathBlow;
			}
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x00225DB4 File Offset: 0x00223FB4
		public override Texture2D GetTexture(IEnumerable<Creature> targets, Creature owner)
		{
			string imagePath = ImageHelper.GetImagePath(this.SpritePath);
			return PreloadManager.Cache.GetTexture2D(imagePath);
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00225DD8 File Offset: 0x00223FD8
		public override string GetAnimation(IEnumerable<Creature> targets, Creature owner)
		{
			string text;
			if ((text = this._cachedAnimationName) == null)
			{
				text = (this._cachedAnimationName = this.IntentPrefix.ToLowerInvariant());
			}
			return text;
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00225E03 File Offset: 0x00224003
		public DeathBlowIntent(Func<decimal> damageCalc)
			: base(damageCalc)
		{
		}
	}
}
