using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000932 RID: 2354
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BubbleBubble : CardModel
	{
		// Token: 0x06006EB7 RID: 28343 RVA: 0x00285F83 File Offset: 0x00284183
		public BubbleBubble()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CB2 RID: 7346
		// (get) Token: 0x06006EB8 RID: 28344 RVA: 0x00285F90 File Offset: 0x00284190
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				ICombatState combatState = base.CombatState;
				if (combatState == null)
				{
					return false;
				}
				return combatState.HittableEnemies.Any<Creature>((Creature e) => e.HasPower<PoisonPower>());
			}
		}

		// Token: 0x17001CB3 RID: 7347
		// (get) Token: 0x06006EB9 RID: 28345 RVA: 0x00285FC8 File Offset: 0x002841C8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x17001CB4 RID: 7348
		// (get) Token: 0x06006EBA RID: 28346 RVA: 0x00285FE8 File Offset: 0x002841E8
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NSmokePuffVfx.AssetPaths;
			}
		}

		// Token: 0x17001CB5 RID: 7349
		// (get) Token: 0x06006EBB RID: 28347 RVA: 0x00285FEF File Offset: 0x002841EF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(9m));
			}
		}

		// Token: 0x06006EBC RID: 28348 RVA: 0x00286004 File Offset: 0x00284204
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(cardPlay.Target) : null);
			if (ncreature != null)
			{
				NGaseousImpactVfx ngaseousImpactVfx = NGaseousImpactVfx.Create(ncreature.VfxSpawnPosition, new Color("83eb85"));
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(ngaseousImpactVfx);
			}
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			if (cardPlay.Target.HasPower<PoisonPower>())
			{
				await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, base.DynamicVars.Poison.BaseValue, base.Owner.Creature, this, false);
			}
		}

		// Token: 0x06006EBD RID: 28349 RVA: 0x00286057 File Offset: 0x00284257
		protected override void OnUpgrade()
		{
			base.DynamicVars.Poison.UpgradeValueBy(3m);
		}
	}
}
