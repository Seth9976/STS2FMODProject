using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	// Token: 0x020003CC RID: 972
	[NullableContext(1)]
	[GenerateSubtypes(DynamicallyAccessedMemberTypes = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
	public interface IBootstrapSettings
	{
		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06003AD3 RID: 15059
		CharacterModel Character { get; }

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06003AD4 RID: 15060
		RoomType RoomType { get; }

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06003AD5 RID: 15061
		EncounterModel Encounter { get; }

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06003AD6 RID: 15062
		EventModel Event { get; }

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06003AD7 RID: 15063
		ActModel Act { get; }

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06003AD8 RID: 15064
		int Ascension { get; }

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06003AD9 RID: 15065
		bool SaveRunHistory { get; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06003ADA RID: 15066
		[Nullable(2)]
		string Seed
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06003ADB RID: 15067
		bool DoPreloading { get; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06003ADC RID: 15068
		bool BootstrapInMultiplayer { get; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06003ADD RID: 15069
		List<ModifierModel> Modifiers { get; }

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x001C1386 File Offset: 0x001BF586
		[Nullable(2)]
		string Language
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x001C138C File Offset: 0x001BF58C
		int? ReplayPlayerIndex
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003AE0 RID: 15072
		Task Setup(Player localPlayer);

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x001C13A4 File Offset: 0x001BF5A4
		MapPointType MapPointType
		{
			get
			{
				RoomType roomType = this.RoomType;
				MapPointType mapPointType;
				switch (roomType)
				{
				case RoomType.Unassigned:
					throw new ArgumentOutOfRangeException();
				case RoomType.Monster:
					mapPointType = MapPointType.Monster;
					break;
				case RoomType.Elite:
					mapPointType = MapPointType.Elite;
					break;
				case RoomType.Boss:
					mapPointType = MapPointType.Boss;
					break;
				case RoomType.Treasure:
					mapPointType = MapPointType.Treasure;
					break;
				case RoomType.Shop:
					mapPointType = MapPointType.Shop;
					break;
				case RoomType.Event:
					mapPointType = MapPointType.Unknown;
					break;
				case RoomType.RestSite:
					mapPointType = MapPointType.RestSite;
					break;
				case RoomType.Map:
					mapPointType = MapPointType.Unknown;
					break;
				default:
					<PrivateImplementationDetails>.ThrowSwitchExpressionException(roomType);
					break;
				}
				return mapPointType;
			}
		}
	}
}
