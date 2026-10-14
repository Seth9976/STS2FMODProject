using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Test
{
	/// <summary>
	/// A minimalist mock implementation of ICloudSaveStore for testing.
	/// </summary>
	// Token: 0x020000BB RID: 187
	public class MockCloudGodotFileIo : MockGodotFileIo, ICloudSaveStore, ISaveStore
	{
		// Token: 0x06000906 RID: 2310 RVA: 0x00041E63 File Offset: 0x00040063
		[NullableContext(1)]
		public MockCloudGodotFileIo(string saveDir)
			: base(saveDir)
		{
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00041E73 File Offset: 0x00040073
		public bool HasCloudFiles()
		{
			return this._files.Any<KeyValuePair<string, MockGodotFileIo.File>>();
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00041E80 File Offset: 0x00040080
		[NullableContext(1)]
		public void ForgetFile(string path)
		{
			base.CanonicalizePath(ref path, true);
			MockGodotFileIo.File file;
			if (!this._files.TryGetValue(path, out file))
			{
				throw new InvalidOperationException("No file at " + path + "!");
			}
			file.forgotten = true;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00041EC4 File Offset: 0x000400C4
		[NullableContext(1)]
		public bool IsFilePersisted(string path)
		{
			base.CanonicalizePath(ref path, true);
			MockGodotFileIo.File file;
			if (!this._files.TryGetValue(path, out file))
			{
				throw new InvalidOperationException("No file at " + path + "!");
			}
			return !file.forgotten;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00041F09 File Offset: 0x00040109
		public void BeginSaveBatch()
		{
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00041F0B File Offset: 0x0004010B
		public void EndSaveBatch()
		{
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00041F0D File Offset: 0x0004010D
		public bool HasUserEnabledCloudSync()
		{
			return this.hasUserEnabledCloudSync;
		}

		// Token: 0x04000314 RID: 788
		public bool hasUserEnabledCloudSync = true;
	}
}
