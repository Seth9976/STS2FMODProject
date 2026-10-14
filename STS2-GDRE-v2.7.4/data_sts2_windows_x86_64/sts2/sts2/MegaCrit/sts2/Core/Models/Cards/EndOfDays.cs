using System;
using System.Collections.Generic;
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A5 RID: 2469
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EndOfDays : CardModel
	{
		// Token: 0x0600710D RID: 28941 RVA: 0x0028A815 File Offset: 0x00288A15
		public EndOfDays()
			: base(3, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001DB7 RID: 7607
		// (get) Token: 0x0600710E RID: 28942 RVA: 0x0028A822 File Offset: 0x00288A22
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DoomPower>(29m));
			}
		}

		// Token: 0x17001DB8 RID: 7608
		// (get) Token: 0x0600710F RID: 28943 RVA: 0x0028A838 File Offset: 0x00288A38
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06007110 RID: 28944 RVA: 0x0028A858 File Offset: 0x00288A58
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			Vector2? sideCenterFloor = VfxCmd.GetSideCenterFloor(CombatSide.Enemy, base.CombatState);
			if (sideCenterFloor != null)
			{
				NLargeMagicMissileVfx nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(sideCenterFloor.Value, new Color("8c2447"));
				if (nlargeMagicMissileVfx != null)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						instance.CombatVfxContainer.AddChildSafely(nlargeMagicMissileVfx);
					}
					await Cmd.Wait(nlargeMagicMissileVfx.WaitTime, false);
				}
			}
			foreach (Creature creature in base.CombatState.HittableEnemies)
			{
				await PowerCmd.Apply<DoomPower>(choiceContext, creature, base.DynamicVars.Doom.BaseValue, base.Owner.Creature, this, false);
			}
			IEnumerator<Creature> enumerator = null;
			await DoomPower.DoomKill(DoomPower.GetDoomedCreatures(base.CombatState.HittableEnemies));
		}

		// Token: 0x06007111 RID: 28945 RVA: 0x0028A8A3 File Offset: 0x00288AA3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Doom.UpgradeValueBy(8m);
		}

		// Token: 0x04002865 RID: 10341
		public const int doomAmount = 29;
	}
}
