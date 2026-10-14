using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// An abstraction layer around how we store save data.
	/// </summary>
	// Token: 0x0200009E RID: 158
	[NullableContext(1)]
	public interface ISaveStore
	{
		// Token: 0x06000529 RID: 1321
		[return: Nullable(2)]
		string ReadFile(string path);

		// Token: 0x0600052A RID: 1322
		[return: Nullable(new byte[] { 1, 2 })]
		Task<string> ReadFileAsync(string path);

		// Token: 0x0600052B RID: 1323
		void WriteFile(string path, string content);

		// Token: 0x0600052C RID: 1324
		void WriteFile(string path, byte[] content);

		// Token: 0x0600052D RID: 1325
		Task WriteFileAsync(string path, string content);

		// Token: 0x0600052E RID: 1326
		Task WriteFileAsync(string path, byte[] content);

		// Token: 0x0600052F RID: 1327
		bool FileExists(string path);

		// Token: 0x06000530 RID: 1328
		bool DirectoryExists(string path);

		// Token: 0x06000531 RID: 1329
		void DeleteFile(string path);

		// Token: 0x06000532 RID: 1330
		void RenameFile(string sourcePath, string destinationPath);

		// Token: 0x06000533 RID: 1331
		string[] GetFilesInDirectory(string directoryPath);

		// Token: 0x06000534 RID: 1332
		string[] GetDirectoriesInDirectory(string directoryPath);

		/// <summary>
		/// Creates a directory if it doesn't already exist.
		/// Implementors must ensure that this does not throw if the directory already exists.
		/// </summary>
		// Token: 0x06000535 RID: 1333
		void CreateDirectory(string directoryPath);

		/// <summary>
		/// Deletes a directory and any remaining contents.
		/// No error is emitted if the directory doesn't already exist.
		/// </summary>
		// Token: 0x06000536 RID: 1334
		void DeleteDirectory(string directoryPath);

		// Token: 0x06000537 RID: 1335
		void DeleteTemporaryFiles(string directoryPath);

		// Token: 0x06000538 RID: 1336
		DateTimeOffset GetLastModifiedTime(string path);

		// Token: 0x06000539 RID: 1337
		int GetFileSize(string path);

		// Token: 0x0600053A RID: 1338
		void SetLastModifiedTime(string path, DateTimeOffset time);

		// Token: 0x0600053B RID: 1339
		string GetFullPath(string filename);
	}
}
