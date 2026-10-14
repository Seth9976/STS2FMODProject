using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AC4 RID: 2756
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shiv : CardModel
	{
		// Token: 0x0600771E RID: 30494 RVA: 0x0029706E File Offset: 0x0029526E
		public Shiv()
			: base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002044 RID: 8260
		// (get) Token: 0x0600771F RID: 30495 RVA: 0x0029707B File Offset: 0x0029527B
		public override TargetType TargetType
		{
			get
			{
				if (!this.HasFanOfKnives)
				{
					return TargetType.AnyEnemy;
				}
				return TargetType.AllEnemies;
			}
		}

		// Token: 0x17002045 RID: 8261
		// (get) Token: 0x06007720 RID: 30496 RVA: 0x00297088 File Offset: 0x00295288
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Shiv };
			}
		}

		// Token: 0x17002046 RID: 8262
		// (get) Token: 0x06007721 RID: 30497 RVA: 0x00297097 File Offset: 0x00295297
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(4m, ValueProp.Move));
			}
		}

		// Token: 0x17002047 RID: 8263
		// (get) Token: 0x06007722 RID: 30498 RVA: 0x002970AA File Offset: 0x002952AA
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007723 RID: 30499 RVA: 0x002970B4 File Offset: 0x002952B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			AttackCommand attackCommand = DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay);
			if (this.HasFanOfKnives)
			{
				Creature lastEnemy = base.CombatState.HittableEnemies.LastOrDefault<Creature>();
				attackCommand = attackCommand.TargetingAllOpponents(base.CombatState).WithHitVfxNode((Creature _) => NShivThrowVfx.Create(this.Owner.Creature, lastEnemy, Colors.Green));
			}
			else
			{
				ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
				attackCommand = attackCommand.Targeting(cardPlay.Target).WithHitVfxNode((Creature t) => NShivThrowVfx.Create(base.Owner.Creature, t, Colors.Green));
			}
			if (base.Owner.Character is Silent)
			{
				attackCommand.WithAttackerAnim("Shiv", 0.2f, null);
			}
			await attackCommand.Execute(choiceContext);
		}

		// Token: 0x06007724 RID: 30500 RVA: 0x00297107 File Offset: 0x00295307
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}

		// Token: 0x06007725 RID: 30501 RVA: 0x00297120 File Offset: 0x00295320
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<CardModel> CreateInHand(Player owner, ICombatState combatState, [Nullable(2)] Player creator = null)
		{
			IEnumerable<CardModel> enumerable = await Shiv.CreateInHand(owner, 1, combatState, creator);
			return enumerable.FirstOrDefault<CardModel>();
		}

		// Token: 0x06007726 RID: 30502 RVA: 0x00297174 File Offset: 0x00295374
		public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, ICombatState combatState, [Nullable(2)] Player creator = null)
		{
			IEnumerable<CardModel> enumerable;
			if (count == 0)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else if (CombatManager.Instance.IsOverOrEnding)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				List<CardModel> shivs = new List<CardModel>();
				for (int i = 0; i < count; i++)
				{
					shivs.Add(combatState.CreateCard<Shiv>(owner));
				}
				await CardPileCmd.AddGeneratedCardsToCombat(shivs, PileType.Hand, creator ?? owner, CardPilePosition.Bottom);
				enumerable = shivs;
			}
			return enumerable;
		}

		// Token: 0x17002048 RID: 8264
		// (get) Token: 0x06007727 RID: 30503 RVA: 0x002971CF File Offset: 0x002953CF
		private bool HasFanOfKnives
		{
			get
			{
				return base.IsMutable && base.Owner != null && base.Owner.Creature.HasPower<FanOfKnivesPower>();
			}
		}
	}
}
