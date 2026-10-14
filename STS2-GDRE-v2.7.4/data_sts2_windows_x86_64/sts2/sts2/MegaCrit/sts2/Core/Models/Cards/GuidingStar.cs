using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009EC RID: 2540
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GuidingStar : CardModel
	{
		// Token: 0x06007299 RID: 29337 RVA: 0x0028DA6F File Offset: 0x0028BC6F
		public GuidingStar()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E65 RID: 7781
		// (get) Token: 0x0600729A RID: 29338 RVA: 0x0028DA7C File Offset: 0x0028BC7C
		public override int CanonicalStarCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001E66 RID: 7782
		// (get) Token: 0x0600729B RID: 29339 RVA: 0x0028DA7F File Offset: 0x0028BC7F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(12m, ValueProp.Move),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x0600729C RID: 29340 RVA: 0x0028DAA8 File Offset: 0x0028BCA8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(cardPlay.Target) : null);
			if (ncreature != null)
			{
				SfxCmd.Play("event:/sfx/characters/regent/regent_guiding_star", 1f);
				NSmallMagicMissileVfx nsmallMagicMissileVfx = NSmallMagicMissileVfx.Create(ncreature.GetBottomOfHitbox(), new Color("50b598"));
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nsmallMagicMissileVfx);
				await Cmd.Wait(nsmallMagicMissileVfx.WaitTime, false);
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithNoAttackerAnim().FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.Execute(choiceContext);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x0600729D RID: 29341 RVA: 0x0028DAFB File Offset: 0x0028BCFB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}

		// Token: 0x04002879 RID: 10361
		private const string _guidingStarSfx = "event:/sfx/characters/regent/regent_guiding_star";
	}
}
