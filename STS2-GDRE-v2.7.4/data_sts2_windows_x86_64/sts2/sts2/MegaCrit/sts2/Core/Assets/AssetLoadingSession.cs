using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Assets
{
	// Token: 0x02000D8F RID: 3471
	[NullableContext(1)]
	[Nullable(0)]
	public class AssetLoadingSession
	{
		// Token: 0x17002657 RID: 9815
		// (get) Token: 0x06008B90 RID: 35728 RVA: 0x002E92B5 File Offset: 0x002E74B5
		public Task<bool> Task
		{
			get
			{
				return this._completionSource.Task;
			}
		}

		// Token: 0x17002658 RID: 9816
		// (get) Token: 0x06008B91 RID: 35729 RVA: 0x002E92C2 File Offset: 0x002E74C2
		public bool IsCompleted
		{
			get
			{
				return this._completionSource.Task.IsCompleted;
			}
		}

		// Token: 0x06008B92 RID: 35730 RVA: 0x002E92D4 File Offset: 0x002E74D4
		public AssetLoadingSession(string name, IEnumerable<string> paths, ConcurrentDictionary<string, Resource> cache, [Nullable(2)] AssetCache assetCache = null)
		{
			this._cache = cache;
			this._assetCache = assetCache;
			this._name = name;
			foreach (string text in paths)
			{
				if (AssetLoadingSession.IsVfxScene(text))
				{
					this._vfxScenes.Enqueue(text);
				}
				else
				{
					this._toLoad.Enqueue(text);
				}
			}
			this._stopwatch.Start();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Preloading '");
			defaultInterpolatedStringHandler.AppendFormatted(name);
			defaultInterpolatedStringHandler.AppendLiteral("' assets... count=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._toLoad.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" vfx=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._vfxScenes.Count);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06008B93 RID: 35731 RVA: 0x002E9408 File Offset: 0x002E7608
		private AssetLoadingSession()
		{
			this._name = "EMPTY";
			this._cache = null;
			this._toLoad = new Queue<string>();
			this._vfxScenes = new Queue<string>();
			this._completionSource.SetResult(true);
		}

		// Token: 0x06008B94 RID: 35732 RVA: 0x002E9491 File Offset: 0x002E7691
		private static bool IsVfxScene(string path)
		{
			return path.EndsWith(".tscn") && path.Contains("/vfx/");
		}

		// Token: 0x06008B95 RID: 35733 RVA: 0x002E94AD File Offset: 0x002E76AD
		public static AssetLoadingSession Empty()
		{
			return new AssetLoadingSession();
		}

		// Token: 0x06008B96 RID: 35734 RVA: 0x002E94B4 File Offset: 0x002E76B4
		public void Process()
		{
			this.FinalizeLoading();
			this.ProcessLoadingQueue();
			this.CheckLoadingStatus();
			if (this._toLoad.Count == 0 && this._loading.Count == 0 && this._finalizing.Count == 0)
			{
				this.ProcessVfxQueue();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Preloading '");
			defaultInterpolatedStringHandler.AppendFormatted(this._name);
			defaultInterpolatedStringHandler.AppendLiteral("' Process: toLoad=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._toLoad.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" loading=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._loading.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" finalizing=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._finalizing.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" vfx=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._vfxScenes.Count);
			Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (this._toLoad.Count == 0 && this._loading.Count == 0 && this._finalizing.Count == 0 && this._vfxScenes.Count == 0 && !this._vfxLoading)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Preloading '");
				defaultInterpolatedStringHandler.AppendFormatted(this._name);
				defaultInterpolatedStringHandler.AppendLiteral("' Complete: assets=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._totalLoaded);
				defaultInterpolatedStringHandler.AppendLiteral(" time_elapsed=");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._stopwatch.ElapsedMilliseconds, "N0");
				defaultInterpolatedStringHandler.AppendLiteral("ms");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this._stopwatch.Stop();
				this._completionSource.TrySetResult(true);
			}
		}

		/// <summary>
		/// Load VFX scenes one at a time via the async threaded loader. Many VFX scenes share ext_resources
		/// (e.g. canvas_item_material_additive_shared.tres is used by 162 scenes), and loading them concurrently
		/// causes Godot's threaded loader to race on shared sub-resource paths.
		/// </summary>
		// Token: 0x06008B97 RID: 35735 RVA: 0x002E9684 File Offset: 0x002E7884
		private void ProcessVfxQueue()
		{
			if (!this._vfxLoading)
			{
				string text;
				while (this._vfxScenes.TryDequeue(out text))
				{
					if (!this._cache.ContainsKey(text))
					{
						if (ResourceLoader.LoadThreadedRequest(text, "", false, 1L) == null)
						{
							this._currentVfxPath = text;
							this._vfxLoading = true;
							return;
						}
						Log.Error("Error requesting VFX load for path: " + text, 2);
					}
				}
				return;
			}
			ResourceLoader.ThreadLoadStatus threadLoadStatus = ResourceLoader.LoadThreadedGetStatus(this._currentVfxPath, null);
			if (threadLoadStatus == 3L)
			{
				this.AddToCache(ResourceLoader.LoadThreadedGet(this._currentVfxPath), this._currentVfxPath);
				this._vfxLoading = false;
				return;
			}
			if (threadLoadStatus == 2L || threadLoadStatus == null)
			{
				Log.Error("Failed to load VFX scene: " + this._currentVfxPath, 2);
				this._vfxLoading = false;
			}
		}

		// Token: 0x06008B98 RID: 35736 RVA: 0x002E9740 File Offset: 0x002E7940
		private void FinalizeLoading()
		{
			while (this._finalizing.Count != 0)
			{
				string text;
				if (!this._finalizing.TryDequeue(out text))
				{
					Log.Error("Failed to dequeue finalizing asset!", 2);
				}
				else
				{
					this.AddToCache(ResourceLoader.LoadThreadedGet(text), text);
				}
			}
		}

		// Token: 0x06008B99 RID: 35737 RVA: 0x002E9785 File Offset: 0x002E7985
		private void AddToCache([Nullable(2)] Resource resource, string path)
		{
			if (resource == null)
			{
				Log.Error("Resource loaded as null for path: " + path, 2);
				return;
			}
			this._totalLoaded++;
			this._cache[path] = resource;
		}

		// Token: 0x06008B9A RID: 35738 RVA: 0x002E97B8 File Offset: 0x002E79B8
		private void ProcessLoadingQueue()
		{
			string text;
			while (this._loading.Count < 128 && this._toLoad.TryDequeue(out text))
			{
				if (!this._cache.ContainsKey(text))
				{
					if (ResourceLoader.LoadThreadedRequest(text, "", false, 1L) == null)
					{
						this._loading.Enqueue(text);
					}
					else
					{
						Log.Error("Error requesting load for path: " + text, 2);
					}
				}
			}
		}

		// Token: 0x06008B9B RID: 35739 RVA: 0x002E9828 File Offset: 0x002E7A28
		private void CheckLoadingStatus()
		{
			int count = this._loading.Count;
			int i = 0;
			while (i < count)
			{
				string text;
				if (!this._loading.TryDequeue(out text))
				{
					Log.Error("Failed to dequeue loading asset!", 2);
					return;
				}
				ResourceLoader.ThreadLoadStatus threadLoadStatus = ResourceLoader.LoadThreadedGetStatus(text, null);
				ResourceLoader.ThreadLoadStatus threadLoadStatus2 = threadLoadStatus;
				if (threadLoadStatus2 > 3L)
				{
					goto IL_0106;
				}
				switch (threadLoadStatus2)
				{
				case 0U:
				case 2U:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Threaded load status ");
					defaultInterpolatedStringHandler.AppendFormatted<ResourceLoader.ThreadLoadStatus>(threadLoadStatus);
					defaultInterpolatedStringHandler.AppendLiteral(" for ");
					defaultInterpolatedStringHandler.AppendFormatted(text);
					defaultInterpolatedStringHandler.AppendLiteral(", falling back to sync load");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					Resource resource = ResourceLoader.Load<Resource>(text, null, 1L);
					if (resource != null)
					{
						this.AddToCache(resource, text);
					}
					else
					{
						Log.Error("Failed to load resource synchronously: " + text, 2);
						AssetCache assetCache = this._assetCache;
						if (assetCache != null)
						{
							assetCache.MarkAssetFailed(text);
						}
					}
					break;
				}
				case 1U:
					this._loading.Enqueue(text);
					break;
				case 3U:
					this._finalizing.Enqueue(text);
					break;
				default:
					goto IL_0106;
				}
				IL_0117:
				i++;
				continue;
				IL_0106:
				Log.Error("Unexpected thread load status for path: " + text, 2);
				goto IL_0117;
			}
		}

		// Token: 0x06008B9C RID: 35740 RVA: 0x002E9958 File Offset: 0x002E7B58
		public void PrintStatus()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 4);
			defaultInterpolatedStringHandler.AppendLiteral("LOADING_STATUS: ToLoad=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._toLoad.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Loading=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._loading.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Finishing=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._finalizing.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" VfxScenes=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._vfxScenes.Count);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06008B9D RID: 35741 RVA: 0x002E99F4 File Offset: 0x002E7BF4
		public Task WaitForCompletion()
		{
			return this._completionSource.Task;
		}

		// Token: 0x040030AB RID: 12459
		private const int _batchSize = 128;

		// Token: 0x040030AC RID: 12460
		private readonly string _name;

		// Token: 0x040030AD RID: 12461
		private readonly ConcurrentDictionary<string, Resource> _cache;

		// Token: 0x040030AE RID: 12462
		[Nullable(2)]
		private readonly AssetCache _assetCache;

		/// <summary>
		/// The paths of all the assets that need to be loaded. This starts out containing every asset that needs to be
		/// loaded, and then is gradually emptied as ResourceLoader loads assets.
		/// </summary>
		// Token: 0x040030AF RID: 12463
		private readonly Queue<string> _toLoad = new Queue<string>();

		/// <summary>
		/// The paths of all the assets that are in the middle of being loaded.
		/// Assets are dequeued from <see cref="F:MegaCrit.Sts2.Core.Assets.AssetLoadingSession._toLoad" /> and enqueued here in <see cref="M:MegaCrit.Sts2.Core.Assets.AssetLoadingSession.ProcessLoadingQueue" />.
		/// </summary>
		// Token: 0x040030B0 RID: 12464
		private readonly Queue<string> _loading = new Queue<string>();

		/// <summary>
		/// The paths of all the assets that are finished loading.
		/// Assets are dequeued from <see cref="F:MegaCrit.Sts2.Core.Assets.AssetLoadingSession._loading" /> and enqueued here in <see cref="M:MegaCrit.Sts2.Core.Assets.AssetLoadingSession.CheckLoadingStatus" /> when
		/// ResourceLoader.LoadThreadedGetStatus finds that an asset has finished loading.
		/// Assets are dequeued from here and added to the cache in <see cref="M:MegaCrit.Sts2.Core.Assets.AssetLoadingSession.FinalizeLoading" />.
		/// </summary>
		// Token: 0x040030B1 RID: 12465
		private readonly Queue<string> _finalizing = new Queue<string>();

		/// <summary>
		/// VFX scenes are loaded one at a time after other assets finish. Loading them concurrently causes
		/// "Another resource is loaded from path" errors because many VFX scenes share the same ext_resources
		/// (materials, textures) and Godot's threaded loader races on shared sub-resource loading.
		/// </summary>
		// Token: 0x040030B2 RID: 12466
		private readonly Queue<string> _vfxScenes = new Queue<string>();

		// Token: 0x040030B3 RID: 12467
		private readonly TaskCompletionSource<bool> _completionSource = new TaskCompletionSource<bool>();

		// Token: 0x040030B4 RID: 12468
		private readonly Stopwatch _stopwatch = new Stopwatch();

		// Token: 0x040030B5 RID: 12469
		private int _totalLoaded;

		// Token: 0x040030B6 RID: 12470
		private bool _vfxLoading;

		// Token: 0x040030B7 RID: 12471
		[Nullable(2)]
		private string _currentVfxPath;
	}
}
