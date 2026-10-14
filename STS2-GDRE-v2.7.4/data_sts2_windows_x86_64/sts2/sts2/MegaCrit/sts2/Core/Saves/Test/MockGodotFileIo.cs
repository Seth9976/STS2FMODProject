using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Platform.Steam;

namespace MegaCrit.Sts2.Core.Saves.Test
{
	/// <summary>
	/// A minimalist mock implementation of ISaveStore for testing.
	/// Instead of relying on actual file system operations like the original implementation,
	/// this mock keeps everything in memory and tracks method calls for verification.
	/// This approach:
	/// 1. Avoids duplicating the real implementation's logic
	/// 2. Makes tests more predictable by not hitting the actual file system
	/// 3. Allows verifying interactions through the Calls collection
	/// 4. Provides customization points only where needed for specific tests
	/// </summary>
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	public class MockGodotFileIo : ISaveStore
	{
		/// <summary>
		/// Tracks all method calls for verification in tests
		/// </summary>
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00041F15 File Offset: 0x00040115
		[TupleElementNames(new string[] { "Method", "Args" })]
		[Nullable(new byte[] { 1, 0, 1, 1, 1 })]
		public List<ValueTuple<string, object[]>> Calls
		{
			[return: TupleElementNames(new string[] { "Method", "Args" })]
			[return: Nullable(new byte[] { 1, 0, 1, 1, 1 })]
			get;
		} = new List<ValueTuple<string, object[]>>();

		/// <summary>
		/// Custom callback for RenameFile operation to allow tests to intercept and verify behavior
		/// </summary>
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00041F1D File Offset: 0x0004011D
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00041F25 File Offset: 0x00040125
		[Nullable(new byte[] { 2, 1, 1 })]
		public Action<string, string> RenameFileAction
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		/// <summary>
		/// Creates a new instance of the mock save store with the specified base directory
		/// </summary>
		/// <param name="saveDir">The base directory for file operations</param>
		// Token: 0x06000910 RID: 2320 RVA: 0x00041F30 File Offset: 0x00040130
		public MockGodotFileIo(string saveDir)
		{
			this.CanonicalizePath(ref saveDir, false);
			this._saveDir = saveDir;
			this.CreateDirectory(this._saveDir);
		}

		/// <summary>
		/// Makes GetLastModifiedTime for the given path return the guarded conversion of a raw Unix-seconds value,
		/// mirroring how the real stores read timestamps from Steam/the OS.
		/// </summary>
		// Token: 0x06000911 RID: 2321 RVA: 0x00041F8B File Offset: 0x0004018B
		public void SetRawTimestampSeconds(string path, long seconds)
		{
			this.CanonicalizePath(ref path, true);
			this._rawTimestampSeconds[path] = seconds;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00041FA4 File Offset: 0x000401A4
		public DateTimeOffset GetLastModifiedTime(string path)
		{
			this.CanonicalizePath(ref path, true);
			long num;
			if (this._rawTimestampSeconds.TryGetValue(path, out num))
			{
				return SaveTimestamps.FromUnixTimeSecondsOrEpoch(num, path);
			}
			MockGodotFileIo.File file;
			if (!this._files.TryGetValue(path, out file))
			{
				throw new InvalidOperationException("No file at " + path + "!");
			}
			if (file.lastModifiedTime == null)
			{
				throw new InvalidOperationException("getCurrentTime was not set when file " + path + " was created!");
			}
			return file.lastModifiedTime.Value;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00042028 File Offset: 0x00040228
		public int GetFileSize(string path)
		{
			this.CanonicalizePath(ref path, true);
			MockGodotFileIo.File file;
			if (!this._files.TryGetValue(path, out file))
			{
				throw new InvalidOperationException("No file at " + path + "!");
			}
			return Encoding.UTF8.GetByteCount(file.content);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00042074 File Offset: 0x00040274
		public void SetLastModifiedTime(string path, DateTimeOffset time)
		{
			this.CanonicalizePath(ref path, true);
			if (this.ShouldFailTimestampSync)
			{
				throw new IOException("Simulated timestamp sync failure for " + path);
			}
			MockGodotFileIo.File file;
			if (!this._files.TryGetValue(path, out file))
			{
				throw new InvalidOperationException("No file at " + path + "!");
			}
			file.lastModifiedTime = new DateTimeOffset?(time);
		}

		/// <summary>
		/// Gets the full path for a filename relative to the base directory
		/// </summary>
		// Token: 0x06000915 RID: 2325 RVA: 0x000420D5 File Offset: 0x000402D5
		public string GetFullPath(string filename)
		{
			this.Calls.Add(new ValueTuple<string, object[]>("GetFullPath", new object[] { filename }));
			this.CanonicalizePath(ref filename, true);
			return filename;
		}

		/// <summary>
		/// Directly sets the content of a file in the virtual file system without going through WriteFile.
		/// Used to simulate corrupt files (e.g. empty content from zeroed-out saves).
		/// </summary>
		// Token: 0x06000916 RID: 2326 RVA: 0x00042100 File Offset: 0x00040300
		public void SetFileContent(string path, string content)
		{
			this.CanonicalizePath(ref path, true);
			MockGodotFileIo.File file;
			if (!this._files.TryGetValue(path, out file))
			{
				throw new InvalidOperationException("Cannot set content: no file at " + path + ". Write the file first.");
			}
			file.content = content;
		}

		/// <summary>
		/// Reads a file from the virtual file system
		/// </summary>
		// Token: 0x06000917 RID: 2327 RVA: 0x00042144 File Offset: 0x00040344
		[return: Nullable(2)]
		public string ReadFile(string path)
		{
			this.CanonicalizePath(ref path, true);
			this.Calls.Add(new ValueTuple<string, object[]>("ReadFile", new object[] { path }));
			MockGodotFileIo.File file;
			if (!this._files.TryGetValue(path, out file))
			{
				return null;
			}
			return file.content;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00042194 File Offset: 0x00040394
		[return: Nullable(new byte[] { 1, 2 })]
		public Task<string> ReadFileAsync(string path)
		{
			if (this.DoSteamSpecificError)
			{
				throw new SteamRemoteSaveStoreException("Simulating Steam Error", 9);
			}
			this.CanonicalizePath(ref path, true);
			this.Calls.Add(new ValueTuple<string, object[]>("ReadFileAsync", new object[] { path }));
			MockGodotFileIo.File file;
			return Task.FromResult<string>(this._files.TryGetValue(path, out file) ? file.content : null);
		}

		/// <summary>
		/// Writes a file to the virtual file system
		/// </summary>
		// Token: 0x06000919 RID: 2329 RVA: 0x000421FC File Offset: 0x000403FC
		public void WriteFile(string path, string content)
		{
			this.CanonicalizePath(ref path, true);
			this.Calls.Add(new ValueTuple<string, object[]>("WriteFile", new object[] { path, content }));
			if (this.ShouldFailWrites)
			{
				throw new InvalidOperationException("Simulated write failure");
			}
			string text = path + ".backup";
			MockGodotFileIo.File file;
			if (this._files.TryGetValue(path, out file))
			{
				this._files[text] = file;
			}
			MockGodotFileIo.File file2 = new MockGodotFileIo.File();
			file2.content = content;
			Func<DateTimeOffset> func = this.getCurrentTime;
			file2.lastModifiedTime = ((func != null) ? new DateTimeOffset?(func()) : null);
			MockGodotFileIo.File file3 = file2;
			this._files[path] = file3;
		}

		/// <summary>
		/// Writes a file to the virtual file system
		/// </summary>
		// Token: 0x0600091A RID: 2330 RVA: 0x000422B0 File Offset: 0x000404B0
		public void WriteFile(string path, byte[] bytes)
		{
			this.WriteFile(path, Encoding.UTF8.GetString(bytes));
		}

		/// <summary>
		/// Asynchronously writes a file to the virtual file system
		/// </summary>
		// Token: 0x0600091B RID: 2331 RVA: 0x000422C4 File Offset: 0x000404C4
		public Task WriteFileAsync(string path, string content)
		{
			this.WriteFile(path, content);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Asynchronously writes a file to the virtual file system
		/// </summary>
		// Token: 0x0600091C RID: 2332 RVA: 0x000422D3 File Offset: 0x000404D3
		public Task WriteFileAsync(string path, byte[] bytes)
		{
			this.WriteFile(path, Encoding.UTF8.GetString(bytes));
			return Task.CompletedTask;
		}

		/// <summary>
		/// Checks if a file exists in the virtual file system
		/// </summary>
		// Token: 0x0600091D RID: 2333 RVA: 0x000422EC File Offset: 0x000404EC
		public bool FileExists(string path)
		{
			this.CanonicalizePath(ref path, true);
			this.Calls.Add(new ValueTuple<string, object[]>("FileExists", new object[] { path }));
			return this._files.ContainsKey(path);
		}

		/// <summary>
		/// Checks if a directory exists in the virtual file system
		/// </summary>
		// Token: 0x0600091E RID: 2334 RVA: 0x00042322 File Offset: 0x00040522
		public bool DirectoryExists(string path)
		{
			return true;
		}

		/// <summary>
		/// Deletes a file from the virtual file system
		/// </summary>
		// Token: 0x0600091F RID: 2335 RVA: 0x00042328 File Offset: 0x00040528
		public void DeleteFile(string path)
		{
			this.CanonicalizePath(ref path, true);
			this.Calls.Add(new ValueTuple<string, object[]>("DeleteFile", new object[] { path }));
			MockGodotFileIo.File file;
			this._files.Remove(path, out file);
		}

		/// <summary>
		/// Renames a file in the virtual file system.
		/// If RenameFileAction is set, it will be called instead of performing the default behavior.
		/// </summary>
		// Token: 0x06000920 RID: 2336 RVA: 0x0004236C File Offset: 0x0004056C
		public void RenameFile(string sourcePath, string destinationPath)
		{
			this.Calls.Add(new ValueTuple<string, object[]>("RenameFile", new object[] { sourcePath, destinationPath }));
			if (this.RenameFileAction != null)
			{
				this.CanonicalizePath(ref sourcePath, false);
				this.CanonicalizePath(ref destinationPath, false);
				this.RenameFileAction(sourcePath, destinationPath);
				return;
			}
			this.CanonicalizePath(ref sourcePath, true);
			this.CanonicalizePath(ref destinationPath, true);
			MockGodotFileIo.File file;
			if (this._files.Remove(sourcePath, out file))
			{
				this._files[destinationPath] = file;
			}
		}

		/// <summary>
		/// Gets all files in a directory from the virtual file system
		/// </summary>
		// Token: 0x06000921 RID: 2337 RVA: 0x000423F4 File Offset: 0x000405F4
		public string[] GetFilesInDirectory(string directoryPath)
		{
			this.CanonicalizePath(ref directoryPath, true);
			this.Calls.Add(new ValueTuple<string, object[]>("GetFilesInDirectory", new object[] { directoryPath }));
			string prefix = (directoryPath.EndsWith('/') ? directoryPath : (directoryPath + "/"));
			return (from path in this._files.Keys
				where path.StartsWith(prefix)
				select Path.GetFileName(path)).ToArray<string>();
		}

		/// <summary>
		/// Gets all directories in a directory from the virtual file system
		/// </summary>
		// Token: 0x06000922 RID: 2338 RVA: 0x00042494 File Offset: 0x00040694
		public string[] GetDirectoriesInDirectory(string directoryPath)
		{
			this.CanonicalizePath(ref directoryPath, true);
			this.Calls.Add(new ValueTuple<string, object[]>("GetDirectoriesInDirectory", new object[] { directoryPath }));
			string prefix = (directoryPath.EndsWith('/') ? directoryPath : (directoryPath + "/"));
			return (from path in this._files.Keys.Where<string>((string path) => path.StartsWith(prefix)).Select<string, string>(delegate(string path)
				{
					int num = prefix.Length + 1;
					return path.Substring(num, path.Length - num);
				})
				select new DirectoryInfo(path).Root.Name).ToArray<string>();
		}

		/// <summary>
		/// Creates a directory in the virtual file system
		/// </summary>
		// Token: 0x06000923 RID: 2339 RVA: 0x00042544 File Offset: 0x00040744
		public void CreateDirectory(string directoryPath)
		{
			this.CanonicalizePath(ref directoryPath, true);
			this.Calls.Add(new ValueTuple<string, object[]>("CreateDirectory", new object[] { directoryPath }));
			if (!this._directories.ContainsKey(directoryPath))
			{
				this._directories[directoryPath] = new List<string>();
			}
		}

		/// <summary>
		/// Deletes a directory and any remaining contents from the virtual file system.
		/// </summary>
		// Token: 0x06000924 RID: 2340 RVA: 0x00042598 File Offset: 0x00040798
		public void DeleteDirectory(string directoryPath)
		{
			this.CanonicalizePath(ref directoryPath, true);
			this.Calls.Add(new ValueTuple<string, object[]>("DeleteDirectory", new object[] { directoryPath }));
		}

		/// <summary>
		/// Deletes temporary files (ending with .tmp) from a directory in the virtual file system
		/// </summary>
		// Token: 0x06000925 RID: 2341 RVA: 0x000425C4 File Offset: 0x000407C4
		public void DeleteTemporaryFiles(string directoryPath)
		{
			this.CanonicalizePath(ref directoryPath, true);
			this.Calls.Add(new ValueTuple<string, object[]>("DeleteTemporaryFiles", new object[] { directoryPath }));
			string prefix = (directoryPath.EndsWith('/') ? directoryPath : (directoryPath + "/"));
			List<string> list = this._files.Keys.Where<string>((string path) => path.StartsWith(prefix) && path.EndsWith(".tmp")).ToList<string>();
			foreach (string text in list)
			{
				MockGodotFileIo.File file;
				this._files.Remove(text, out file);
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00042688 File Offset: 0x00040888
		protected void CanonicalizePath(ref string path, bool getFullPath = true)
		{
			path = path.Replace('\\', '/');
			if (getFullPath)
			{
				path = this._saveDir + "/" + path;
			}
		}

		/// <summary>
		/// In-memory storage of file contents, with path as the key and content as the value
		/// </summary>
		// Token: 0x04000315 RID: 789
		protected readonly ConcurrentDictionary<string, MockGodotFileIo.File> _files = new ConcurrentDictionary<string, MockGodotFileIo.File>();

		/// <summary>
		/// In-memory representation of directory structure
		/// </summary>
		// Token: 0x04000316 RID: 790
		protected readonly ConcurrentDictionary<string, List<string>> _directories = new ConcurrentDictionary<string, List<string>>();

		/// <summary>
		/// Base directory for GetFullPath operations
		/// </summary>
		// Token: 0x04000319 RID: 793
		protected readonly string _saveDir;

		// Token: 0x0400031A RID: 794
		[Nullable(2)]
		public Func<DateTimeOffset> getCurrentTime;

		// Token: 0x0400031B RID: 795
		public bool ShouldFailWrites;

		// Token: 0x0400031C RID: 796
		public bool ShouldFailTimestampSync;

		// Token: 0x0400031D RID: 797
		public bool DoSteamSpecificError;

		/// <summary>
		/// Per-file raw Unix-seconds timestamps. When set for a path, GetLastModifiedTime routes the raw value through
		/// the real <see cref="T:MegaCrit.Sts2.Core.Saves.SaveTimestamps" /> guard, exactly as SteamRemoteSaveStore/GodotFileIo do, so tests can
		/// exercise the out-of-range path (PRG-7045) end to end. Set via <see cref="M:MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo.SetRawTimestampSeconds(System.String,System.Int64)" />.
		/// </summary>
		// Token: 0x0400031E RID: 798
		protected readonly ConcurrentDictionary<string, long> _rawTimestampSeconds = new ConcurrentDictionary<string, long>();

		/// <summary>
		/// Constants for method names to use when tracking or comparing method calls
		/// </summary>
		// Token: 0x02000E90 RID: 3728
		[Nullable(0)]
		public static class Methods
		{
			// Token: 0x0400396D RID: 14701
			public const string writeFile = "WriteFile";

			// Token: 0x0400396E RID: 14702
			public const string writeFileAsync = "WriteFileAsync";

			// Token: 0x0400396F RID: 14703
			public const string readFile = "ReadFile";

			// Token: 0x04003970 RID: 14704
			public const string readFileAsync = "ReadFileAsync";

			// Token: 0x04003971 RID: 14705
			public const string fileExists = "FileExists";

			// Token: 0x04003972 RID: 14706
			public const string renameFile = "RenameFile";

			// Token: 0x04003973 RID: 14707
			public const string deleteFile = "DeleteFile";

			// Token: 0x04003974 RID: 14708
			public const string getFullPath = "GetFullPath";

			// Token: 0x04003975 RID: 14709
			public const string getDirectoriesInDirectory = "GetDirectoriesInDirectory";

			// Token: 0x04003976 RID: 14710
			public const string getFilesInDirectory = "GetFilesInDirectory";

			// Token: 0x04003977 RID: 14711
			public const string createDirectory = "CreateDirectory";

			// Token: 0x04003978 RID: 14712
			public const string deleteDirectory = "DeleteDirectory";

			// Token: 0x04003979 RID: 14713
			public const string deleteTemporaryFiles = "DeleteTemporaryFiles";

			// Token: 0x0400397A RID: 14714
			public const string getLastModifiedTime = "GetLastModifiedTime";
		}

		// Token: 0x02000E91 RID: 3729
		[NullableContext(0)]
		[RequiredMember]
		protected class File
		{
			// Token: 0x060094D8 RID: 38104 RVA: 0x002FC7CD File Offset: 0x002FA9CD
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public File()
			{
			}

			// Token: 0x0400397B RID: 14715
			[Nullable(1)]
			[RequiredMember]
			public string content;

			// Token: 0x0400397C RID: 14716
			public DateTimeOffset? lastModifiedTime;

			// Token: 0x0400397D RID: 14717
			public bool forgotten;
		}
	}
}
