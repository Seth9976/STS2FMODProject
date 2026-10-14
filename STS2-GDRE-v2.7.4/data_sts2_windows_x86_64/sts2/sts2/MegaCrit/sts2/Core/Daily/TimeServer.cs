using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Daily
{
	/// <summary>
	/// Static class for interfacing with MegaCrit's time server.
	/// </summary>
	// Token: 0x02000D1A RID: 3354
	[NullableContext(1)]
	[Nullable(0)]
	public static class TimeServer
	{
		// Token: 0x17002542 RID: 9538
		// (get) Token: 0x06008758 RID: 34648 RVA: 0x002D88A0 File Offset: 0x002D6AA0
		// (set) Token: 0x06008759 RID: 34649 RVA: 0x002D88A7 File Offset: 0x002D6AA7
		[Nullable(2)]
		public static Task<TimeServerResult?> RequestTimeTask
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x0600875A RID: 34650 RVA: 0x002D88B0 File Offset: 0x002D6AB0
		private static async Task<TimeServerResult?> RequestTime(string url)
		{
			TimeServerResult? timeServerResult;
			using (HttpClient client = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(5L)
			})
			{
				Exception exception = null;
				int retries = 0;
				while (retries < 2)
				{
					long? num2;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					try
					{
						long? num = await TimeServer.RequestTimeInternal(client, url);
						num2 = num;
					}
					catch (Exception ex)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Caught exception while requesting server time of type ");
						defaultInterpolatedStringHandler.AppendFormatted<Type>(ex.GetType());
						Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						exception = ex;
						num2 = null;
					}
					if (num2 != null)
					{
						return new TimeServerResult?(new TimeServerResult
						{
							serverTime = DateTimeOffset.FromUnixTimeSeconds(num2.Value),
							localReceivedTime = DateTimeOffset.UtcNow
						});
					}
					retries++;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Retries: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(retries);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(2);
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					if (retries < 2)
					{
						await Task.Delay(TimeSpan.FromSeconds(Math.Min((long)Math.Pow(2.0, (double)retries), 4L)));
					}
				}
				TimeServer._logger.Warn("Gave up trying to retrieve server time", 1);
				if (exception != null)
				{
					ExceptionDispatchInfo.Capture(exception).Throw();
				}
				timeServerResult = null;
			}
			return timeServerResult;
		}

		// Token: 0x0600875B RID: 34651 RVA: 0x002D88F4 File Offset: 0x002D6AF4
		private static async Task<long?> RequestTimeInternal(HttpClient client, string url)
		{
			HttpResponseMessage httpResponseMessage = await client.GetAsync(url);
			HttpResponseMessage response = httpResponseMessage;
			string text = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
			{
				TimeServer._logger.Info("Successfully queried time server. Response: " + text, 1);
				long num;
				if (long.TryParse(text, out num))
				{
					return new long?(num);
				}
			}
			else
			{
				Logger logger = TimeServer._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to retrieve server time. Status: ");
				defaultInterpolatedStringHandler.AppendFormatted<HttpStatusCode>(response.StatusCode);
				defaultInterpolatedStringHandler.AppendLiteral(" Response: ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			return null;
		}

		/// <summary>
		/// Begins fetching the time from the time server. After this, RequestTimeTask is non-null.
		/// </summary>
		// Token: 0x0600875C RID: 34652 RVA: 0x002D893F File Offset: 0x002D6B3F
		public static Task<TimeServerResult?> FetchDailyTime()
		{
			TimeServer.RequestTimeTask = TimeServer.RequestTime("https://time.megacrit.com");
			return TimeServer.RequestTimeTask;
		}

		// Token: 0x04002EF4 RID: 12020
		private const string _timeServerUrl = "https://time.megacrit.com";

		// Token: 0x04002EF5 RID: 12021
		private const int _maxRetry = 2;

		// Token: 0x04002EF6 RID: 12022
		private const int _waitTimeCap = 4;

		// Token: 0x04002EF7 RID: 12023
		private static readonly Logger _logger = new Logger("TimeServer", LogType.Network);
	}
}
