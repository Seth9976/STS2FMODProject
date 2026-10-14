using System;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	[Nullable(0)]
	public class FileAccessStream : Stream
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00017102 File Offset: 0x00015302
		public override bool CanRead
		{
			get
			{
				return this._file.IsOpen() && this._flags.HasFlag(1L);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001712A File Offset: 0x0001532A
		public override bool CanSeek
		{
			get
			{
				return this._file.IsOpen();
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00017137 File Offset: 0x00015337
		public override bool CanWrite
		{
			get
			{
				return this._file.IsOpen() && this._flags.HasFlag(2L);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001715F File Offset: 0x0001535F
		public override long Length
		{
			get
			{
				return (long)this._file.GetLength();
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001716C File Offset: 0x0001536C
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00017179 File Offset: 0x00015379
		public override long Position
		{
			get
			{
				return (long)this._file.GetPosition();
			}
			set
			{
				this._file.Seek((ulong)value);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00017188 File Offset: 0x00015388
		public FileAccessStream(string filePath, FileAccess.ModeFlags flags)
		{
			this._flags = flags;
			this._filePath = filePath;
			FileAccess fileAccess = FileAccess.Open(filePath, flags);
			FileAccess fileAccess2 = fileAccess;
			if (fileAccess2 == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Opening file ");
				defaultInterpolatedStringHandler.AppendFormatted(filePath);
				defaultInterpolatedStringHandler.AppendLiteral(" with flags ");
				defaultInterpolatedStringHandler.AppendFormatted<FileAccess.ModeFlags>(flags);
				defaultInterpolatedStringHandler.AppendLiteral(" failed: ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(FileAccess.GetOpenError());
				throw new IOException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._file = fileAccess2;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00017213 File Offset: 0x00015413
		public override void Flush()
		{
			this._file.Flush();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00017220 File Offset: 0x00015420
		public override int Read(byte[] buffer, int offset, int count)
		{
			byte[] buffer2 = this._file.GetBuffer((long)count);
			int num = Math.Min(count, buffer2.Length);
			Array.Copy(buffer2, 0, buffer, offset, num);
			return num;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00017250 File Offset: 0x00015450
		public override void Write(byte[] buffer, int offset, int count)
		{
			int num = Math.Min(buffer.Length - offset, count);
			bool flag;
			if (offset == 0 && buffer.Length <= count)
			{
				flag = this._file.StoreBuffer(buffer);
			}
			else
			{
				byte[] array = new byte[num];
				Array.Copy(buffer, offset, array, 0, num);
				flag = this._file.StoreBuffer(array);
			}
			if (!flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to write ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes to file ");
				defaultInterpolatedStringHandler.AppendFormatted(this._filePath);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(this._file.GetError());
				throw new IOException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00017304 File Offset: 0x00015504
		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				this._file.Seek((ulong)offset);
				break;
			case SeekOrigin.Current:
				this._file.Seek(this._file.GetPosition() + (ulong)offset);
				break;
			case SeekOrigin.End:
				this._file.Seek(this._file.GetLength() - (ulong)offset);
				break;
			}
			return (long)this._file.GetPosition();
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00017370 File Offset: 0x00015570
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00017377 File Offset: 0x00015577
		protected override void Dispose(bool disposing)
		{
			this._file.Close();
		}

		// Token: 0x040001A4 RID: 420
		private readonly FileAccess _file;

		// Token: 0x040001A5 RID: 421
		private readonly FileAccess.ModeFlags _flags;

		// Token: 0x040001A6 RID: 422
		private readonly string _filePath;
	}
}
