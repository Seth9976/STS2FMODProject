using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000927 RID: 2343
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bolas : CardModel
	{
		// Token: 0x06006E7A RID: 28282 RVA: 0x00285715 File Offset: 0x00283915
		public Bolas()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C99 RID: 7321
		// (get) Token: 0x06006E7B RID: 28283 RVA: 0x00285722 File Offset: 0x00283922
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Move));
			}
		}

		// Token: 0x17001C9A RID: 7322
		// (get) Token: 0x06006E7C RID: 28284 RVA: 0x00285735 File Offset: 0x00283935
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NBolasVfx.AssetPaths;
			}
		}

		// Token: 0x06006E7D RID: 28285 RVA: 0x0028573C File Offset: 0x0028393C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(NBolasVfx.Create(base.Owner.Creature, cardPlay.Target));
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06006E7E RID: 28286 RVA: 0x00285790 File Offset: 0x00283990
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner)
			{
				bool flag = CombatManager.Instance.History.CardPlaysFinished.Any<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.HappenedLastPlayerTurn(base.Owner) && e.CardPlay.Card == this);
				if (flag)
				{
					CardPile pile = base.Pile;
					if (pile == null || pile.Type != PileType.Hand)
					{
						await CardPileCmd.Add(this, PileType.Hand, CardPilePosition.Bottom, null, false);
					}
				}
			}
		}

		// Token: 0x06006E7F RID: 28287 RVA: 0x002857DB File Offset: 0x002839DB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
		}
	}
}
