using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D7 RID: 2007
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Noisebot : MonsterModel
	{
		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x06006330 RID: 25392 RVA: 0x00266300 File Offset: 0x00264500
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
			}
		}

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x06006331 RID: 25393 RVA: 0x0026630C File Offset: 0x0026450C
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 23);
			}
		}

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x06006332 RID: 25394 RVA: 0x00266318 File Offset: 0x00264518
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x06006333 RID: 25395 RVA: 0x0026631C File Offset: 0x0026451C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			if (TestMode.IsOff)
			{
				FabricatorNormal.SetBotFallPosition(NCombatRoom.Instance.GetCreatureNode(base.Creature));
			}
		}

		// Token: 0x06006334 RID: 25396 RVA: 0x00266360 File Offset: 0x00264560
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("NOISE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NoiseMove), new AbstractIntent[]
			{
				new StatusIntent(2)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006335 RID: 25397 RVA: 0x002663B0 File Offset: 0x002645B0
		private async Task NoiseMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.6f);
			foreach (Creature creature in targets)
			{
				Player player = creature.Player ?? creature.PetOwner;
				CardPileAddResult[] statusCards = new CardPileAddResult[2];
				CardModel cardModel = base.CombatState.CreateCard<Dazed>(player);
				CardPileAddResult[] array = statusCards;
				array[0] = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, null, CardPilePosition.Bottom);
				array = null;
				CardModel cardModel2 = base.CombatState.CreateCard<Dazed>(player);
				array = statusCards;
				array[1] = await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Draw, null, CardPilePosition.Random);
				array = null;
				if (LocalContext.IsMe(player))
				{
					CardCmd.PreviewCardPileAdd(statusCards, 1.2f, CardPreviewStyle.HorizontalLayout);
					await Cmd.Wait(1f, false);
				}
				player = null;
				statusCards = null;
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x0400263B RID: 9787
		private const int _noiseStatusCount = 2;
	}
}
