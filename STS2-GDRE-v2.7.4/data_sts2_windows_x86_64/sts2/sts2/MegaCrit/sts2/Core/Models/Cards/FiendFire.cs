using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009C0 RID: 2496
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FiendFire : CardModel
	{
		// Token: 0x060071A6 RID: 29094 RVA: 0x0028BB37 File Offset: 0x00289D37
		public FiendFire()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DFE RID: 7678
		// (get) Token: 0x060071A7 RID: 29095 RVA: 0x0028BB44 File Offset: 0x00289D44
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x17001DFF RID: 7679
		// (get) Token: 0x060071A8 RID: 29096 RVA: 0x0028BB57 File Offset: 0x00289D57
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001E00 RID: 7680
		// (get) Token: 0x060071A9 RID: 29097 RVA: 0x0028BB5F File Offset: 0x00289D5F
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NGroundFireVfx.AssetPaths;
			}
		}

		// Token: 0x060071AA RID: 29098 RVA: 0x0028BB68 File Offset: 0x00289D68
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			List<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards.ToList<CardModel>();
			int cardCount = list.Count;
			foreach (CardModel cardModel in list)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			float scale = 0.8f;
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(cardCount).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.BeforeDamage(delegate
				{
					NGroundFireVfx ngroundFireVfx = NGroundFireVfx.Create(cardPlay.Target, VfxColor.Red);
					if (ngroundFireVfx == null)
					{
						return Task.CompletedTask;
					}
					SfxCmd.Play("event:/sfx/characters/attack_fire", 1f);
					ngroundFireVfx.Scale = Vector2.One * scale;
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						instance.CombatVfxContainer.AddChildSafely(ngroundFireVfx);
					}
					scale += 0.1f;
					return Task.CompletedTask;
				})
				.Execute(choiceContext);
		}

		// Token: 0x060071AB RID: 29099 RVA: 0x0028BBBB File Offset: 0x00289DBB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
