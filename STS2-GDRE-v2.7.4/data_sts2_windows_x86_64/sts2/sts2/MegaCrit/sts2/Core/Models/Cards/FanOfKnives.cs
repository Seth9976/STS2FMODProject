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
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009B8 RID: 2488
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FanOfKnives : CardModel
	{
		// Token: 0x06007176 RID: 29046 RVA: 0x0028B4A3 File Offset: 0x002896A3
		public FanOfKnives()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001DE8 RID: 7656
		// (get) Token: 0x06007177 RID: 29047 RVA: 0x0028B4B0 File Offset: 0x002896B0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar("Shivs", 4));
			}
		}

		// Token: 0x17001DE9 RID: 7657
		// (get) Token: 0x06007178 RID: 29048 RVA: 0x0028B4C2 File Offset: 0x002896C2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x17001DEA RID: 7658
		// (get) Token: 0x06007179 RID: 29049 RVA: 0x0028B4CF File Offset: 0x002896CF
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NFanOfKnivesVfx.AssetPaths;
			}
		}

		// Token: 0x0600717A RID: 29050 RVA: 0x0028B4D8 File Offset: 0x002896D8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<FanOfKnivesPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
			for (int i = 0; i < base.DynamicVars["Shivs"].IntValue; i++)
			{
				await Shiv.CreateInHand(base.Owner, base.CombatState, null);
				await Cmd.CustomScaledWait(0.1f, 0.2f, false, default(CancellationToken));
			}
		}

		// Token: 0x0600717B RID: 29051 RVA: 0x0028B524 File Offset: 0x00289724
		public override async Task OnEnqueuePlayVfx([Nullable(2)] Creature target)
		{
			Control backVfxContainer = base.Owner.Creature.GetBackVfxContainer();
			if (backVfxContainer != null)
			{
				backVfxContainer.AddChildSafely(NFanOfKnivesVfx.Create(base.Owner.Creature));
			}
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
		}

		// Token: 0x0600717C RID: 29052 RVA: 0x0028B567 File Offset: 0x00289767
		protected override void OnUpgrade()
		{
			base.DynamicVars["Shivs"].UpgradeValueBy(1m);
		}

		// Token: 0x0400286A RID: 10346
		private const string _shivsKey = "Shivs";
	}
}
