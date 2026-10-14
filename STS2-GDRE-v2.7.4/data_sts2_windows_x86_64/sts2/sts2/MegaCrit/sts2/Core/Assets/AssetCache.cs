using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// This class is responsible for preloading and caching assets. It solves a problem where we want to load whole
	/// groups of assets, but we also want to eliminate duplicate loads of the same asset. This class is thread-safe.
	/// </summary>
	// Token: 0x02000D8D RID: 3469
	[NullableContext(1)]
	[Nullable(0)]
	public class AssetCache
	{
		/// <summary>Gets the current count of missed cache assets (loaded outside preloading).</summary>
		// Token: 0x17002656 RID: 9814
		// (get) Token: 0x06008B7C RID: 35708 RVA: 0x002E8F15 File Offset: 0x002E7115
		public int MissedCacheAssetCount
		{
			get
			{
				return this._missedCacheAssets.Count;
			}
		}

		/// <summary>
		/// Gets the asset if it is cached, otherwise falls back to loading it.
		/// </summary>
		// Token: 0x06008B7D RID: 35709 RVA: 0x002E8F24 File Offset: 0x002E7124
		private Resource GetAsset(string path)
		{
			Resource resource;
			if (!this._cache.TryGetValue(path, out resource))
			{
				return this.LoadAsset(path);
			}
			if (GodotObject.IsInstanceValid(resource))
			{
				return resource;
			}
			this._cache[path] = ResourceLoader.Load<Resource>(path, null, 1L);
			return this._cache[path];
		}

		// Token: 0x06008B7E RID: 35710 RVA: 0x002E8F74 File Offset: 0x002E7174
		public TS GetAsset<[Nullable(0)] TS>(string path) where TS : Resource
		{
			return (TS)((object)this.GetAsset(path));
		}

		// Token: 0x06008B7F RID: 35711 RVA: 0x002E8F84 File Offset: 0x002E7184
		private Resource LoadAsset(string path)
		{
			if (this._failedAssets.Contains(path))
			{
				throw new AssetLoadException("Asset previously failed to load: " + path + ". The game installation may be corrupted.");
			}
			Resource resource = ResourceLoader.Load<Resource>(path, null, 1L);
			if (resource is AtlasTexture)
			{
				return resource;
			}
			this._missedCacheAssets.Add(path);
			Log.Warn("Asset not cached: " + path, 2);
			this._cache[path] = resource;
			return resource;
		}

		/// <summary>
		/// Marks an asset as failed so that synchronous fallback loading does not re-attempt it.
		/// This prevents repeated native crashes in the Godot resource parser when files are
		/// missing or corrupted.
		/// </summary>
		// Token: 0x06008B80 RID: 35712 RVA: 0x002E8FF5 File Offset: 0x002E71F5
		public void MarkAssetFailed(string path)
		{
			this._failedAssets.Add(path);
		}

		// Token: 0x06008B81 RID: 35713 RVA: 0x002E9004 File Offset: 0x002E7204
		public AssetLoadingSession CreateSession(string name, IEnumerable<string> paths)
		{
			return new AssetLoadingSession(name, paths, this._cache, this);
		}

		// Token: 0x06008B82 RID: 35714 RVA: 0x002E9014 File Offset: 0x002E7214
		public void UnloadAssets(IEnumerable<string> assetsToUnloadSet)
		{
			foreach (string text in assetsToUnloadSet)
			{
				if (!this._missedCacheAssets.Contains(text))
				{
					Resource resource = this.RemoveAndGetResource(text);
					if (resource != null && GodotObject.IsInstanceValid(resource))
					{
						Callable.From(new Action(resource.Dispose)).CallDeferred(Array.Empty<Variant>());
					}
				}
			}
		}

		/// <summary>
		/// Clears and unloads all missed cache assets. Should be called at safe boundaries
		/// like returning to the main menu to prevent unbounded memory growth.
		/// </summary>
		// Token: 0x06008B83 RID: 35715 RVA: 0x002E9098 File Offset: 0x002E7298
		public void UnloadMissedCacheAssets()
		{
			if (this._missedCacheAssets.Count == 0)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unloading ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._missedCacheAssets.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" missed cache assets");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			foreach (string text in this._missedCacheAssets)
			{
				Resource resource = this.RemoveAndGetResource(text);
				if (resource != null && GodotObject.IsInstanceValid(resource))
				{
					Callable.From(new Action(resource.Dispose)).CallDeferred(Array.Empty<Variant>());
				}
			}
			this._missedCacheAssets.Clear();
		}

		// Token: 0x06008B84 RID: 35716 RVA: 0x002E9174 File Offset: 0x002E7374
		public IReadOnlySet<string> GetLoadedCacheAssets()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (KeyValuePair<string, Resource> keyValuePair in this._cache)
			{
				if (GodotObject.IsInstanceValid(keyValuePair.Value))
				{
					hashSet.Add(keyValuePair.Key);
				}
				else
				{
					Resource resource;
					this._cache.TryRemove(keyValuePair.Key, out resource);
				}
			}
			return hashSet;
		}

		// Token: 0x06008B85 RID: 35717 RVA: 0x002E91F4 File Offset: 0x002E73F4
		public IEnumerable<string> GetCacheKeys()
		{
			return this._cache.Keys;
		}

		// Token: 0x06008B86 RID: 35718 RVA: 0x002E9204 File Offset: 0x002E7404
		[return: Nullable(2)]
		private Resource RemoveAndGetResource(string key)
		{
			Resource resource;
			if (this._cache.TryRemove(key, out resource))
			{
				return resource;
			}
			return null;
		}

		// Token: 0x06008B87 RID: 35719 RVA: 0x002E9224 File Offset: 0x002E7424
		public PackedScene GetScene(string path)
		{
			return (PackedScene)this.GetAsset(path);
		}

		// Token: 0x06008B88 RID: 35720 RVA: 0x002E9232 File Offset: 0x002E7432
		public Texture2D GetTexture2D(string path)
		{
			return (Texture2D)this.GetAsset(path);
		}

		// Token: 0x06008B89 RID: 35721 RVA: 0x002E9240 File Offset: 0x002E7440
		public Material GetMaterial(string path)
		{
			return (Material)this.GetAsset(path);
		}

		// Token: 0x06008B8A RID: 35722 RVA: 0x002E924E File Offset: 0x002E744E
		public CompressedTexture2D GetCompressedTexture2D(string path)
		{
			return (CompressedTexture2D)this.GetAsset(path);
		}

		// Token: 0x06008B8B RID: 35723 RVA: 0x002E925C File Offset: 0x002E745C
		public bool ContainsKey(string s)
		{
			return this._cache.ContainsKey(s);
		}

		// Token: 0x06008B8C RID: 35724 RVA: 0x002E926A File Offset: 0x002E746A
		public void SetAsset(string path, Resource resource)
		{
			this._cache[path] = resource;
		}

		// Token: 0x040030A8 RID: 12456
		private readonly ConcurrentDictionary<string, Resource> _cache = new ConcurrentDictionary<string, Resource>();

		// Token: 0x040030A9 RID: 12457
		private readonly HashSet<string> _missedCacheAssets = new HashSet<string>();

		// Token: 0x040030AA RID: 12458
		private readonly HashSet<string> _failedAssets = new HashSet<string>();
	}
}
