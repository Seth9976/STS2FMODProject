using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	// Token: 0x0200035C RID: 860
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class BestiaryEntry
	{
		// Token: 0x060031C8 RID: 12744 RVA: 0x0017194E File Offset: 0x0016FB4E
		public static BestiaryEntry FromMonster(MonsterModel monster, EncounterModel encounter, RoomType type)
		{
			return new BestiaryEntry
			{
				monsterModel = monster,
				encounterModel = encounter,
				roomType = type
			};
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x0017196A File Offset: 0x0016FB6A
		public static BestiaryEntry FromEncounter(EncounterModel encounter, RoomType type)
		{
			return new BestiaryEntry
			{
				encounterModel = encounter,
				roomType = type
			};
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x0017197F File Offset: 0x0016FB7F
		public string GetEncounterTitle()
		{
			return this.encounterModel.Title.GetFormattedText();
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x00171991 File Offset: 0x0016FB91
		public string GetEntryTitle()
		{
			if (this.monsterModel != null)
			{
				return this.monsterModel.Title.GetFormattedText();
			}
			return this.GetEncounterTitle();
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x001719B2 File Offset: 0x0016FBB2
		[NullableContext(2)]
		public bool CanReuseLayout(NBestiaryLayout layout)
		{
			if (this.encounterModel is KaiserCrabBoss)
			{
				return layout is NBestiaryLayoutKaiserCrab;
			}
			if (this.encounterModel is DecimillipedeElite)
			{
				return layout is NBestiaryLayoutDecimillipede;
			}
			return layout is NBestiaryLayoutDefault;
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x001719EB File Offset: 0x0016FBEB
		[return: Nullable(2)]
		public NBestiaryLayout CreateLayoutNode(NBestiary bestiary)
		{
			if (this.encounterModel is KaiserCrabBoss)
			{
				return NBestiaryLayoutKaiserCrab.Create();
			}
			if (this.encounterModel is DecimillipedeElite)
			{
				return NBestiaryLayoutDecimillipede.Create(bestiary);
			}
			return NBestiaryLayoutDefault.Create();
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x00171A19 File Offset: 0x0016FC19
		public bool IsDiscovered(HashSet<ModelId> discoveredMonsterIds, HashSet<ModelId> discoveredEncounterIds)
		{
			if (this.monsterModel == null)
			{
				return discoveredEncounterIds.Contains(this.encounterModel.Id);
			}
			return discoveredMonsterIds.Contains(this.monsterModel.Id);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x00171A46 File Offset: 0x0016FC46
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public BestiaryEntry()
		{
		}

		// Token: 0x0400135A RID: 4954
		[Nullable(2)]
		public MonsterModel monsterModel;

		// Token: 0x0400135B RID: 4955
		[RequiredMember]
		public EncounterModel encounterModel;

		// Token: 0x0400135C RID: 4956
		[RequiredMember]
		public RoomType roomType;
	}
}
