using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C0C RID: 3084
	[NullableContext(2)]
	[Nullable(0)]
	public static class ImageHelper
	{
		/// <summary>
		/// Get the path to the image with the specified "inner" path.
		/// </summary>
		/// <param name="innerPath">The image's inner path. For example, for "images/cards/bash.png", pass "cards/bash.png".</param>
		/// <returns>The full path to the image.</returns>
		// Token: 0x06007FBF RID: 32703 RVA: 0x002B6064 File Offset: 0x002B4264
		[NullableContext(1)]
		public static string GetImagePath(string innerPath)
		{
			if (innerPath.StartsWith('/'))
			{
				string text = innerPath;
				innerPath = text.Substring(1, text.Length - 1);
			}
			return "res://images/" + innerPath;
		}

		/// <summary>
		/// Get the path to the icon for the specified map point and room type.
		/// </summary>
		/// <param name="mapPointType">The type of map point that the room is in.</param>
		/// <param name="roomType">The type of room.</param>
		/// <param name="modelId">
		/// (Optional) The model ID corresponding to room. This is for bosses and ancients, which have unique icons.
		/// </param>
		/// <returns>The full path to the image</returns>
		// Token: 0x06007FC0 RID: 32704 RVA: 0x002B609C File Offset: 0x002B429C
		public static string GetRoomIconPath(MapPointType mapPointType, RoomType roomType, ModelId modelId)
		{
			if (mapPointType == MapPointType.Unassigned || roomType == RoomType.Map)
			{
				return null;
			}
			string roomIconSuffix = ImageHelper.GetRoomIconSuffix(mapPointType, roomType, modelId);
			if (roomIconSuffix == null)
			{
				return null;
			}
			return ImageHelper.GetImagePath("ui/run_history/" + roomIconSuffix + ".png");
		}

		/// <summary>
		/// Get the path to the icon outline for the specified map point and room type.
		/// </summary>
		/// <param name="mapPointType">The type of map point that the room is in.</param>
		/// <param name="roomType">The type of room.</param>
		/// <param name="modelId">
		/// (Optional) The model ID corresponding to room. This is for bosses and ancients, which have unique icons.
		/// </param>
		/// <returns>The full path to the image</returns>
		// Token: 0x06007FC1 RID: 32705 RVA: 0x002B60D8 File Offset: 0x002B42D8
		public static string GetRoomIconOutlinePath(MapPointType mapPointType, RoomType roomType, ModelId modelId)
		{
			if (mapPointType == MapPointType.Unassigned || roomType == RoomType.Map)
			{
				return null;
			}
			string roomIconSuffix = ImageHelper.GetRoomIconSuffix(mapPointType, roomType, modelId);
			if (roomIconSuffix == null)
			{
				return null;
			}
			return ImageHelper.GetImagePath("ui/run_history/" + roomIconSuffix + "_outline.png");
		}

		// Token: 0x06007FC2 RID: 32706 RVA: 0x002B6114 File Offset: 0x002B4314
		private static string GetRoomIconSuffix(MapPointType mapPointType, RoomType roomType, ModelId modelId)
		{
			if (modelId != null)
			{
				return modelId.Entry.ToLowerInvariant();
			}
			if (roomType == RoomType.Boss)
			{
				return null;
			}
			string text = StringHelper.Slugify(roomType.ToString()).ToLowerInvariant();
			if (mapPointType == MapPointType.Unknown && roomType != RoomType.Event)
			{
				return "unknown_" + text;
			}
			return text;
		}
	}
}
