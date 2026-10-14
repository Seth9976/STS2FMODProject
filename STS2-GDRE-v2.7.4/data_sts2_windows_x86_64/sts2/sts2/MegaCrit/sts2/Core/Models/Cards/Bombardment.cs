using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000928 RID: 2344
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bombardment : CardModel
	{
		// Token: 0x06006E81 RID: 28289 RVA: 0x00285812 File Offset: 0x00283A12
		public Bombardment()
			: base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C9B RID: 7323
		// (get) Token: 0x06006E82 RID: 28290 RVA: 0x0028581F File Offset: 0x00283A1F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001C9C RID: 7324
		// (get) Token: 0x06006E83 RID: 28291 RVA: 0x00285827 File Offset: 0x00283A27
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(18m, ValueProp.Move));
			}
		}

		// Token: 0x06006E84 RID: 28292 RVA: 0x0028583C File Offset: 0x00283A3C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(cardPlay.Target) : null);
			if (ncreature != null)
			{
				NLargeMagicMissileVfx nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(ncreature.GetBottomOfHitbox(), new Color("50b598"));
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nlargeMagicMissileVfx);
				await Cmd.Wait(nlargeMagicMissileVfx.WaitTime, false);
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		/// <remarks>
		/// We use Early here to prevent it from double-triggering if another auto-pre-play phase effect (like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Powers.MayhemPower" />) also causes it to Exhaust.
		/// </remarks>
		// Token: 0x06006E85 RID: 28293 RVA: 0x00285890 File Offset: 0x00283A90
		public override async Task AfterAutoPrePlayPhaseEnteredEarly(PlayerChoiceContext choiceContext, Player player)
		{
			CardPile pile = base.Pile;
			if (pile != null && pile.Type == PileType.Exhaust)
			{
				if (player == base.Owner)
				{
					await CardCmd.AutoPlay(choiceContext, this, null, AutoPlayType.Default, false, false);
				}
			}
		}

		// Token: 0x06006E86 RID: 28294 RVA: 0x002858E3 File Offset: 0x00283AE3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(6m);
		}
	}
}
