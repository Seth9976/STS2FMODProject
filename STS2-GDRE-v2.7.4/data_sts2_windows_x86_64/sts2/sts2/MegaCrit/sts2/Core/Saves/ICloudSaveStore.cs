using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// An interface used by cloud save stores, providing additional functionality on top of ISaveStore.
	/// </summary>
	// Token: 0x0200009B RID: 155
	[NullableContext(1)]
	public interface ICloudSaveStore : ISaveStore
	{
		/// <summary>
		/// Returns true if any files are stored in the cloud.
		/// </summary>
		// Token: 0x06000520 RID: 1312
		bool HasCloudFiles();

		/// <summary>
		/// Removes a file from remote storage, but keeps it in the local cache.
		///
		/// </summary>
		/// <remarks>
		/// For implementors: Functionally, what this should do is remove the file from the cloud, freeing up cloud storage,
		/// but the file should still exist when we call ReadFile on it. WriteFile will re-upload the file to the cloud.
		/// This implementation is modeled after SteamRemoteStorage. If this proves to be problematic, feel free to re-evaluate
		/// the strategy.
		/// </remarks>
		// Token: 0x06000521 RID: 1313
		void ForgetFile(string path);

		/// <summary>
		/// Returns true if the file at the path is in remote storage.
		/// The file can also be in local storage, but this is disregarded.
		/// </summary>
		// Token: 0x06000522 RID: 1314
		bool IsFilePersisted(string path);

		/// <summary>
		/// Signals the cloud store that a batch of file writes is about to begin.
		/// On Steam, this calls BeginFileWriteBatch so that cloud sync is deferred until the batch ends,
		/// preventing partial sync on Steam Deck suspend.
		/// </summary>
		// Token: 0x06000523 RID: 1315
		void BeginSaveBatch();

		/// <summary>
		/// Signals the cloud store that a batch of file writes has ended.
		/// On Steam, this calls EndFileWriteBatch so that deferred cloud sync can proceed.
		/// </summary>
		// Token: 0x06000524 RID: 1316
		void EndSaveBatch();

		/// <summary>
		/// Returns true if the user has enabled cloud sync.
		/// DO NOT use this to check if you should be writing files to the cloud.
		/// DO use this to check if you should be syncing files from the cloud to this machine.
		/// See <see cref="T:MegaCrit.Sts2.Core.Platform.Steam.SteamRemoteSaveStore" /> for why.
		/// </summary>
		// Token: 0x06000525 RID: 1317
		bool HasUserEnabledCloudSync();
	}
}
