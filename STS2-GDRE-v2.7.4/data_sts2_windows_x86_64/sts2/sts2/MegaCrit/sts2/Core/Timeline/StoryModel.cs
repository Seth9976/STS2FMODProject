using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.Timeline
{
	// Token: 0x0200003A RID: 58
	[NullableContext(1)]
	[Nullable(0)]
	[GenerateSubtypes(DynamicallyAccessedMemberTypes = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicProperties)]
	public abstract class StoryModel
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000268 RID: 616
		protected abstract string Id { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000269 RID: 617
		public abstract EpochModel[] Epochs { get; }

		// Token: 0x0600026A RID: 618 RVA: 0x00012774 File Offset: 0x00010974
		static StoryModel()
		{
			for (int i = 0; i < StoryModelSubtypes.Count; i++)
			{
				Type type = StoryModelSubtypes.Get(i);
				StoryModel storyModel = (StoryModel)Activator.CreateInstance(type);
				StoryModel._storyTypeDictionary[storyModel.Id] = type;
			}
		}

		/// <summary>
		/// Given an Epoch, identifies the story and grabs the previous chapter if possible.
		/// Returns null if out of bounds or if there's no previous chapter.
		/// Will skip over chapters you haven't revealed.
		/// </summary>
		// Token: 0x0600026B RID: 619 RVA: 0x000127C0 File Offset: 0x000109C0
		[return: Nullable(2)]
		public static EpochModel PrevChapter(EpochModel model)
		{
			if (model.StoryId == null)
			{
				return null;
			}
			StoryModel storyModel = StoryModel.Get(StringHelper.Slugify(model.StoryId));
			for (int i = 0; i < storyModel.Epochs.Length; i++)
			{
				if (storyModel.Epochs[i].Id == model.Id)
				{
					for (i--; i >= 0; i--)
					{
						EpochModel epochModel = storyModel.Epochs[i];
						if (SaveManager.Instance.IsEpochRevealed(epochModel.Id))
						{
							return epochModel;
						}
					}
					return null;
				}
			}
			Log.Error("Epoch: " + model.Id + " was not found in " + storyModel.Id, 2);
			return null;
		}

		/// <summary>
		/// Grabs the next chapter in the provided Epoch's story.
		/// Returns null if out of bounds or if there's not a next chapter.
		/// Will skip over unrevealed chapters in a story.
		/// </summary>
		// Token: 0x0600026C RID: 620 RVA: 0x00012864 File Offset: 0x00010A64
		[return: Nullable(2)]
		public static EpochModel NextChapter(EpochModel model)
		{
			if (model.StoryId == null)
			{
				return null;
			}
			StoryModel storyModel = StoryModel.Get(StringHelper.Slugify(model.StoryId));
			for (int i = 0; i < storyModel.Epochs.Length; i++)
			{
				if (storyModel.Epochs[i].Id == model.Id)
				{
					for (i++; i < storyModel.Epochs.Length; i++)
					{
						EpochModel epochModel = storyModel.Epochs[i];
						if (SaveManager.Instance.IsEpochRevealed(epochModel.Id))
						{
							return epochModel;
						}
					}
					return null;
				}
			}
			Log.Error("Epoch: " + model.Id + " was not found in " + storyModel.Id, 2);
			return null;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00012910 File Offset: 0x00010B10
		public static StoryModel Get(string id)
		{
			Type type;
			if (StoryModel._storyTypeDictionary.TryGetValue(id, out type))
			{
				return (StoryModel)Activator.CreateInstance(type);
			}
			throw new ArgumentException("Story with id '" + id + "' does not exist.");
		}

		// Token: 0x04000112 RID: 274
		private static readonly Dictionary<string, Type> _storyTypeDictionary = new Dictionary<string, Type>();
	}
}
