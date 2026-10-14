using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009F6 RID: 2550
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Haze : CardModel
	{
		// Token: 0x060072CD RID: 29389 RVA: 0x0028E08A File Offset: 0x0028C28A
		public Haze()
			: base(3, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001E7B RID: 7803
		// (get) Token: 0x060072CE RID: 29390 RVA: 0x0028E097 File Offset: 0x0028C297
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
			}
		}

		// Token: 0x17001E7C RID: 7804
		// (get) Token: 0x060072CF RID: 29391 RVA: 0x0028E09F File Offset: 0x0028C29F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(4m));
			}
		}

		// Token: 0x17001E7D RID: 7805
		// (get) Token: 0x060072D0 RID: 29392 RVA: 0x0028E0B4 File Offset: 0x0028C2B4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x060072D1 RID: 29393 RVA: 0x0028E0D4 File Offset: 0x0028C2D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			this.SpawnVfx();
			await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
			foreach (Creature creature in base.CombatState.HittableEnemies)
			{
				await PowerCmd.Apply<PoisonPower>(choiceContext, creature, base.DynamicVars.Poison.BaseValue, base.Owner.Creature, this, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060072D2 RID: 29394 RVA: 0x0028E120 File Offset: 0x0028C320
		private void SpawnVfx()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			Node node = ((instance != null) ? instance.CombatVfxContainer : null);
			if (node != null)
			{
				NSmokyVignetteVfx nsmokyVignetteVfx = NSmokyVignetteVfx.Create(new Color(0.8f, 0.8f, 0.3f, 0.66f), new Color(0f, 4f, 0f, 0.33f));
				node.AddChildSafely(nsmokyVignetteVfx);
				foreach (Creature creature in base.CombatState.HittableEnemies)
				{
					node.AddChildSafely(NSmokePuffVfx.Create(creature, NSmokePuffVfx.SmokePuffColor.Green));
				}
			}
		}

		// Token: 0x060072D3 RID: 29395 RVA: 0x0028E1CC File Offset: 0x0028C3CC
		protected override void OnUpgrade()
		{
			base.DynamicVars.Poison.UpgradeValueBy(2m);
		}
	}
}
