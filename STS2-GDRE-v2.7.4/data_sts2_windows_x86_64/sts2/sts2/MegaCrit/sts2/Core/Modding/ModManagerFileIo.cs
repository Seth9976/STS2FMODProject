using System;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B98 RID: 2968
	[NullableContext(1)]
	[Nullable(0)]
	public class ModManagerFileIo : IModManagerFileIo
	{
		// Token: 0x06007BF0 RID: 31728 RVA: 0x002A415F File Offset: 0x002A235F
		public string[] GetFilesAt(string path)
		{
			return DirAccess.GetFilesAt(path);
		}

		// Token: 0x06007BF1 RID: 31729 RVA: 0x002A4167 File Offset: 0x002A2367
		public string[] GetDirectoriesAt(string path)
		{
			return DirAccess.GetDirectoriesAt(path);
		}

		// Token: 0x06007BF2 RID: 31730 RVA: 0x002A416F File Offset: 0x002A236F
		public bool FileExists(string path)
		{
			return FileAccess.FileExists(path);
		}

		// Token: 0x06007BF3 RID: 31731 RVA: 0x002A4177 File Offset: 0x002A2377
		public bool DirectoryExists(string path)
		{
			return DirAccess.DirExistsAbsolute(path);
		}

		// Token: 0x06007BF4 RID: 31732 RVA: 0x002A417F File Offset: 0x002A237F
		public Stream OpenStream(string path, FileAccess.ModeFlags mode)
		{
			return new FileAccessStream(path, mode);
		}
	}
}
