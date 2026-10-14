using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// Manages texture atlases by parsing .tpsheet files and creating AtlasTextures on demand.
	/// Thread-safe via ConcurrentDictionary caching.
	/// </summary>
	// Token: 0x02000D91 RID: 3473
	[NullableContext(1)]
	[Nullable(0)]
	public static class AtlasManager
	{
		/// <summary>
		/// Loads all known atlases. Call during initialization.
		/// </summary>
		// Token: 0x06008BA8 RID: 35752 RVA: 0x002EA02C File Offset: 0x002E822C
		public static void LoadAllAtlases()
		{
			foreach (string text in AtlasManager._knownAtlases)
			{
				AtlasManager.LoadAtlas(text);
			}
		}

		/// <summary>
		/// Loads only atlases referenced by scene ext_resources.
		/// Must be called before any scenes load.
		/// </summary>
		// Token: 0x06008BA9 RID: 35753 RVA: 0x002EA058 File Offset: 0x002E8258
		public static void LoadEssentialAtlases()
		{
			foreach (string text in AtlasManager._essentialAtlases)
			{
				AtlasManager.LoadAtlas(text);
			}
		}

		/// <summary>
		/// Loads a single atlas by parsing its .tpsheet file and loading PNG textures.
		/// Thread-safe: uses lock to prevent duplicate loading work.
		/// </summary>
		// Token: 0x06008BAA RID: 35754 RVA: 0x002EA084 File Offset: 0x002E8284
		public static void LoadAtlas(string atlasName)
		{
			if (AtlasManager._atlases.ContainsKey(atlasName))
			{
				return;
			}
			using (AtlasManager._loadLock.EnterScope())
			{
				if (!AtlasManager._atlases.ContainsKey(atlasName))
				{
					AtlasManager.LoadAtlasInternal(atlasName);
				}
			}
		}

		// Token: 0x06008BAB RID: 35755 RVA: 0x002EA0DC File Offset: 0x002E82DC
		private static void LoadAtlasInternal(string atlasName)
		{
			string text = "res://images/atlases/" + atlasName + ".tpsheet";
			if (!FileAccess.FileExists(text))
			{
				Log.Warn("AtlasManager: tpsheet not found: " + text, 2);
				return;
			}
			using (FileAccess fileAccess = FileAccess.Open(text, 1L))
			{
				if (fileAccess == null)
				{
					Log.Warn("AtlasManager: Failed to open " + text, 2);
				}
				else
				{
					string asText = fileAccess.GetAsText(false);
					TpSheetData tpSheetData = JsonSerializer.Deserialize<TpSheetData>(asText, AtlasManager._jsonOptions);
					if (tpSheetData == null)
					{
						Log.Warn("AtlasManager: Failed to parse " + text, 2);
					}
					else
					{
						Dictionary<string, Texture2D> dictionary = new Dictionary<string, Texture2D>();
						Dictionary<string, AtlasManager.SpriteInfo> dictionary2 = new Dictionary<string, AtlasManager.SpriteInfo>();
						foreach (TpSheetTexture tpSheetTexture in tpSheetData.Textures)
						{
							string text2 = "res://images/atlases/" + tpSheetTexture.Image;
							Texture2D texture2D = ResourceLoader.Load<Texture2D>(text2, null, 1L);
							if (texture2D == null)
							{
								Log.Warn("AtlasManager: Failed to load texture: " + text2, 2);
							}
							else
							{
								dictionary[tpSheetTexture.Image] = texture2D;
								foreach (TpSheetSprite tpSheetSprite in tpSheetTexture.Sprites)
								{
									string text3 = AtlasManager.NormalizeSpriteKey(tpSheetSprite.Filename);
									dictionary2[text3] = new AtlasManager.SpriteInfo(texture2D, tpSheetSprite);
								}
							}
						}
						AtlasManager.AtlasData atlasData = new AtlasManager.AtlasData
						{
							TpSheet = tpSheetData,
							PageTextures = dictionary,
							SpriteMap = dictionary2
						};
						AtlasManager._atlases[atlasName] = atlasData;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
						defaultInterpolatedStringHandler.AppendLiteral("AtlasManager: Loaded ");
						defaultInterpolatedStringHandler.AppendFormatted(atlasName);
						defaultInterpolatedStringHandler.AppendLiteral(" with ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(dictionary2.Count);
						defaultInterpolatedStringHandler.AppendLiteral(" sprites");
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
			}
		}

		/// <summary>
		/// Gets an AtlasTexture for the specified sprite, creating and caching it if needed.
		/// </summary>
		/// <param name="atlasName">Atlas name (e.g., "relic_atlas")</param>
		/// <param name="spriteName">Sprite name without extension (e.g., "anchor_small")</param>
		/// <returns>The AtlasTexture, or null if not found</returns>
		// Token: 0x06008BAC RID: 35756 RVA: 0x002EA318 File Offset: 0x002E8518
		[return: Nullable(2)]
		public static AtlasTexture GetSprite(string atlasName, string spriteName)
		{
			AtlasManager.AtlasData atlasData;
			if (!AtlasManager._atlases.TryGetValue(atlasName, out atlasData))
			{
				return null;
			}
			string text = AtlasManager.NormalizeSpriteKey(spriteName);
			AtlasManager.SpriteInfo spriteInfo;
			if (!atlasData.SpriteMap.TryGetValue(text, out spriteInfo))
			{
				return null;
			}
			string text2 = atlasName + "/" + spriteName;
			AtlasTexture atlasTexture;
			if (AtlasManager._spriteCache.TryGetValue(text2, out atlasTexture))
			{
				if (GodotObject.IsInstanceValid(atlasTexture))
				{
					return atlasTexture;
				}
				AtlasTexture atlasTexture2;
				AtlasManager._spriteCache.TryRemove(text2, out atlasTexture2);
			}
			return AtlasManager._spriteCache.GetOrAdd(text2, (string _) => AtlasManager.CreateAtlasTexture(spriteInfo));
		}

		/// <summary>
		/// Checks if a sprite exists in the specified atlas.
		/// </summary>
		// Token: 0x06008BAD RID: 35757 RVA: 0x002EA3A8 File Offset: 0x002E85A8
		public static bool HasSprite(string atlasName, string spriteName)
		{
			AtlasManager.AtlasData atlasData;
			if (!AtlasManager._atlases.TryGetValue(atlasName, out atlasData))
			{
				return false;
			}
			string text = AtlasManager.NormalizeSpriteKey(spriteName);
			return atlasData.SpriteMap.ContainsKey(text);
		}

		/// <summary>
		/// Gets the number of sprites in a loaded atlas.
		/// </summary>
		// Token: 0x06008BAE RID: 35758 RVA: 0x002EA3DC File Offset: 0x002E85DC
		public static int GetSpriteCount(string atlasName)
		{
			AtlasManager.AtlasData atlasData;
			if (!AtlasManager._atlases.TryGetValue(atlasName, out atlasData))
			{
				return 0;
			}
			return atlasData.SpriteMap.Count;
		}

		/// <summary>
		/// Checks if an atlas has been loaded.
		/// </summary>
		// Token: 0x06008BAF RID: 35759 RVA: 0x002EA405 File Offset: 0x002E8605
		public static bool IsAtlasLoaded(string atlasName)
		{
			return AtlasManager._atlases.ContainsKey(atlasName);
		}

		/// <summary>
		/// Clears all loaded atlases and cached sprites. Useful for testing.
		/// </summary>
		// Token: 0x06008BB0 RID: 35760 RVA: 0x002EA412 File Offset: 0x002E8612
		public static void Clear()
		{
			AtlasManager._atlases.Clear();
			AtlasManager._spriteCache.Clear();
		}

		// Token: 0x06008BB1 RID: 35761 RVA: 0x002EA428 File Offset: 0x002E8628
		private static AtlasTexture CreateAtlasTexture(AtlasManager.SpriteInfo spriteInfo)
		{
			TpSheetSprite sprite = spriteInfo.Sprite;
			AtlasTexture atlasTexture = new AtlasTexture
			{
				Atlas = spriteInfo.Atlas,
				Region = new Rect2((float)sprite.Region.X, (float)sprite.Region.Y, (float)sprite.Region.W, (float)sprite.Region.H)
			};
			if (sprite.Margin.X != 0 || sprite.Margin.Y != 0 || sprite.Margin.W != 0 || sprite.Margin.H != 0)
			{
				atlasTexture.Margin = new Rect2((float)sprite.Margin.X, (float)sprite.Margin.Y, (float)sprite.Margin.W, (float)sprite.Margin.H);
			}
			return atlasTexture;
		}

		// Token: 0x06008BB2 RID: 35762 RVA: 0x002EA4FC File Offset: 0x002E86FC
		private static string NormalizeSpriteKey(string filename)
		{
			if (filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
			{
				return filename.Substring(0, filename.Length - 4);
			}
			return filename;
		}

		// Token: 0x040030BE RID: 12478
		private static readonly string[] _knownAtlases = new string[]
		{
			"ui_atlas", "compressed", "epoch_atlas", "relic_atlas", "relic_outline_atlas", "power_atlas", "card_atlas", "potion_atlas", "potion_outline_atlas", "stats_screen_atlas",
			"intent_atlas", "era_atlas"
		};

		// Token: 0x040030BF RID: 12479
		private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		// Token: 0x040030C0 RID: 12480
		private static readonly ConcurrentDictionary<string, AtlasManager.AtlasData> _atlases = new ConcurrentDictionary<string, AtlasManager.AtlasData>();

		// Token: 0x040030C1 RID: 12481
		private static readonly ConcurrentDictionary<string, AtlasTexture> _spriteCache = new ConcurrentDictionary<string, AtlasTexture>();

		// Token: 0x040030C2 RID: 12482
		private static readonly Lock _loadLock = new Lock();

		/// <summary>
		/// Atlases needed before main menu scene loads.
		/// Their .tres files are referenced by scene ext_resources and must be loaded
		/// before Godot's scene deserializer encounters them.
		/// </summary>
		// Token: 0x040030C3 RID: 12483
		private static readonly string[] _essentialAtlases = new string[] { "ui_atlas", "compressed" };

		// Token: 0x0200255E RID: 9566
		[Nullable(0)]
		[RequiredMember]
		private class AtlasData
		{
			// Token: 0x170026EE RID: 9966
			// (get) Token: 0x0600C612 RID: 50706 RVA: 0x0044D8EC File Offset: 0x0044BAEC
			// (set) Token: 0x0600C613 RID: 50707 RVA: 0x0044D8F4 File Offset: 0x0044BAF4
			[RequiredMember]
			public TpSheetData TpSheet { get; set; }

			// Token: 0x170026EF RID: 9967
			// (get) Token: 0x0600C614 RID: 50708 RVA: 0x0044D8FD File Offset: 0x0044BAFD
			// (set) Token: 0x0600C615 RID: 50709 RVA: 0x0044D905 File Offset: 0x0044BB05
			[RequiredMember]
			public Dictionary<string, Texture2D> PageTextures { get; set; }

			// Token: 0x170026F0 RID: 9968
			// (get) Token: 0x0600C616 RID: 50710 RVA: 0x0044D90E File Offset: 0x0044BB0E
			// (set) Token: 0x0600C617 RID: 50711 RVA: 0x0044D916 File Offset: 0x0044BB16
			[RequiredMember]
			public Dictionary<string, AtlasManager.SpriteInfo> SpriteMap { get; set; }

			// Token: 0x0600C618 RID: 50712 RVA: 0x0044D91F File Offset: 0x0044BB1F
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public AtlasData()
			{
			}
		}

		// Token: 0x0200255F RID: 9567
		[Nullable(0)]
		private class SpriteInfo : IEquatable<AtlasManager.SpriteInfo>
		{
			// Token: 0x0600C619 RID: 50713 RVA: 0x0044D927 File Offset: 0x0044BB27
			public SpriteInfo(Texture2D Atlas, TpSheetSprite Sprite)
			{
				this.Atlas = Atlas;
				this.Sprite = Sprite;
				base..ctor();
			}

			// Token: 0x170026F1 RID: 9969
			// (get) Token: 0x0600C61A RID: 50714 RVA: 0x0044D93D File Offset: 0x0044BB3D
			[CompilerGenerated]
			protected virtual Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(AtlasManager.SpriteInfo);
				}
			}

			// Token: 0x170026F2 RID: 9970
			// (get) Token: 0x0600C61B RID: 50715 RVA: 0x0044D949 File Offset: 0x0044BB49
			// (set) Token: 0x0600C61C RID: 50716 RVA: 0x0044D951 File Offset: 0x0044BB51
			public Texture2D Atlas { get; set; }

			// Token: 0x170026F3 RID: 9971
			// (get) Token: 0x0600C61D RID: 50717 RVA: 0x0044D95A File Offset: 0x0044BB5A
			// (set) Token: 0x0600C61E RID: 50718 RVA: 0x0044D962 File Offset: 0x0044BB62
			public TpSheetSprite Sprite { get; set; }

			// Token: 0x0600C61F RID: 50719 RVA: 0x0044D96C File Offset: 0x0044BB6C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SpriteInfo");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600C620 RID: 50720 RVA: 0x0044D9B8 File Offset: 0x0044BBB8
			[CompilerGenerated]
			protected virtual bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Atlas = ");
				builder.Append(this.Atlas);
				builder.Append(", Sprite = ");
				builder.Append(this.Sprite);
				return true;
			}

			// Token: 0x0600C621 RID: 50721 RVA: 0x0044D9F2 File Offset: 0x0044BBF2
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(AtlasManager.SpriteInfo left, AtlasManager.SpriteInfo right)
			{
				return !(left == right);
			}

			// Token: 0x0600C622 RID: 50722 RVA: 0x0044D9FE File Offset: 0x0044BBFE
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(AtlasManager.SpriteInfo left, AtlasManager.SpriteInfo right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600C623 RID: 50723 RVA: 0x0044DA12 File Offset: 0x0044BC12
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(this.<Atlas>k__BackingField)) * -1521134295 + EqualityComparer<TpSheetSprite>.Default.GetHashCode(this.<Sprite>k__BackingField);
			}

			// Token: 0x0600C624 RID: 50724 RVA: 0x0044DA52 File Offset: 0x0044BC52
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as AtlasManager.SpriteInfo);
			}

			// Token: 0x0600C625 RID: 50725 RVA: 0x0044DA60 File Offset: 0x0044BC60
			[NullableContext(2)]
			[CompilerGenerated]
			public virtual bool Equals(AtlasManager.SpriteInfo other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Texture2D>.Default.Equals(this.<Atlas>k__BackingField, other.<Atlas>k__BackingField) && EqualityComparer<TpSheetSprite>.Default.Equals(this.<Sprite>k__BackingField, other.<Sprite>k__BackingField));
			}

			// Token: 0x0600C627 RID: 50727 RVA: 0x0044DAC1 File Offset: 0x0044BCC1
			[CompilerGenerated]
			protected SpriteInfo(AtlasManager.SpriteInfo original)
			{
				this.Atlas = original.<Atlas>k__BackingField;
				this.Sprite = original.<Sprite>k__BackingField;
			}

			// Token: 0x0600C628 RID: 50728 RVA: 0x0044DAE1 File Offset: 0x0044BCE1
			[CompilerGenerated]
			public void Deconstruct(out Texture2D Atlas, out TpSheetSprite Sprite)
			{
				Atlas = this.Atlas;
				Sprite = this.Sprite;
			}
		}
	}
}
