using System;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Modding
{
	/// <summary>
	/// A file IO abstraction layer for use with ModManager.
	/// It's a little different than ISaveStore, which is scoped only to a specific directory. This class allows global access
	/// to the filesystem.
	/// </summary>
	// Token: 0x02000B8F RID: 2959
	[NullableContext(1)]
	public interface IModManagerFileIo
	{
		// Token: 0x06007BB2 RID: 31666
		string[] GetFilesAt(string path);

		// Token: 0x06007BB3 RID: 31667
		string[] GetDirectoriesAt(string path);

		// Token: 0x06007BB4 RID: 31668
		bool FileExists(string path);

		// Token: 0x06007BB5 RID: 31669
		bool DirectoryExists(string path);

		// Token: 0x06007BB6 RID: 31670
		Stream OpenStream(string path, FileAccess.ModeFlags mode);
	}
}
