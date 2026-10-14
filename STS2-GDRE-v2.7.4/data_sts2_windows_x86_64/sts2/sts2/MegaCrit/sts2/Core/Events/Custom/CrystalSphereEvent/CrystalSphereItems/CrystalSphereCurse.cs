using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems
{
	// Token: 0x02000C54 RID: 3156
	[NullableContext(1)]
	[Nullable(0)]
	public class CrystalSphereCurse : CrystalSphereItem
	{
		// Token: 0x17002385 RID: 9093
		// (get) Token: 0x0600821B RID: 33307 RVA: 0x002C0866 File Offset: 0x002BEA66
		public override Vector2I Size
		{
			get
			{
				return new Vector2I(2, 2);
			}
		}

		// Token: 0x17002386 RID: 9094
		// (get) Token: 0x0600821C RID: 33308 RVA: 0x002C086F File Offset: 0x002BEA6F
		public override bool IsGood
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600821D RID: 33309 RVA: 0x002C0874 File Offset: 0x002BEA74
		public override async Task RevealItem(Player owner)
		{
			await base.RevealItem(owner);
			CardModel cardModel = await CardPileCmd.AddCurseToDeck<Doubt>(owner);
			if (cardModel != null)
			{
				RunManager.Instance.RewardSynchronizer.SyncLocalObtainedCard(cardModel);
			}
		}

		// Token: 0x0600821E RID: 33310 RVA: 0x002C08BF File Offset: 0x002BEABF
		public override SerializableCrystalSphereItem ToSerializable()
		{
			return new SerializableCrystalSphereItem
			{
				type = CrystalSphereItemType.Curse
			};
		}
	}
}
