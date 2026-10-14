using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Steamworks;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam
{
	// Token: 0x02000436 RID: 1078
	public class SteamCallResult<T> : IDisposable where T : struct
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06004476 RID: 17526 RVA: 0x00210F8A File Offset: 0x0020F18A
		[Nullable(new byte[] { 1, 0 })]
		public Task<T> Task
		{
			[return: Nullable(new byte[] { 1, 0 })]
			get
			{
				return this._completionSource.Task;
			}
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x00210F98 File Offset: 0x0020F198
		public SteamCallResult(SteamAPICall_t call, CancellationToken cancelToken = default(CancellationToken))
		{
			this._callResult = CallResult<T>.Create(new CallResult<T>.APIDispatchDelegate(this.OnCallResult));
			this._callResult.Set(call, null);
			if (cancelToken.CanBeCanceled)
			{
				this._cancelTokenRegistration = new CancellationTokenRegistration?(cancelToken.Register(new Action(this.Cancel)));
			}
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x00211004 File Offset: 0x0020F204
		public void Cancel()
		{
			this._callResult.Cancel();
			this._completionSource.TrySetCanceled();
			if (this._cancelTokenRegistration == null)
			{
				return;
			}
			this._cancelTokenRegistration.GetValueOrDefault().Dispose();
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x00211048 File Offset: 0x0020F248
		private void OnCallResult(T result, bool ioError)
		{
			if (ioError)
			{
				TaskCompletionSource<T> completionSource = this._completionSource;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Got IO failure from CallResult of type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral("!");
				completionSource.SetException(new IOException(defaultInterpolatedStringHandler.ToStringAndClear()));
			}
			else
			{
				this._completionSource.SetResult(result);
			}
			if (this._cancelTokenRegistration == null)
			{
				return;
			}
			this._cancelTokenRegistration.GetValueOrDefault().Dispose();
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x002110D0 File Offset: 0x0020F2D0
		public void Dispose()
		{
			this._callResult.Dispose();
			if (this._cancelTokenRegistration == null)
			{
				return;
			}
			this._cancelTokenRegistration.GetValueOrDefault().Dispose();
		}

		// Token: 0x04001A89 RID: 6793
		[Nullable(new byte[] { 1, 0 })]
		private readonly TaskCompletionSource<T> _completionSource = new TaskCompletionSource<T>();

		// Token: 0x04001A8A RID: 6794
		[Nullable(new byte[] { 1, 0 })]
		private readonly CallResult<T> _callResult;

		// Token: 0x04001A8B RID: 6795
		private readonly CancellationTokenRegistration? _cancelTokenRegistration;
	}
}
