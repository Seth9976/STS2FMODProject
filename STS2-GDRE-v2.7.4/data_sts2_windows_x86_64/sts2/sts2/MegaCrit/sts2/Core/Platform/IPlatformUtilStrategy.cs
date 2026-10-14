using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Multiplayer.Game;

namespace MegaCrit.Sts2.Core.Platform
{
	// Token: 0x02000154 RID: 340
	[NullableContext(1)]
	internal interface IPlatformUtilStrategy
	{
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600106B RID: 4203
		bool SupportsInviteDialog { get; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x0600106C RID: 4204
		bool IsPlatformOverlayOpen { get; }

		// Token: 0x0600106D RID: 4205
		string GetPlayerName(ulong playerId);

		// Token: 0x0600106E RID: 4206
		ulong GetLocalPlayerId();

		// Token: 0x0600106F RID: 4207
		Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies();

		// Token: 0x06001070 RID: 4208
		void OpenInviteDialog(INetGameService gameService);

		// Token: 0x06001071 RID: 4209
		void OpenUrl(string url);

		// Token: 0x06001072 RID: 4210
		void OpenVirtualKeyboard();

		// Token: 0x06001073 RID: 4211
		void CloseVirtualKeyboard();

		// Token: 0x06001074 RID: 4212
		PlatformBranch GetPlatformBranch();

		// Token: 0x06001075 RID: 4213
		[NullableContext(2)]
		string GetThreeLetterLanguageCode();

		// Token: 0x06001076 RID: 4214
		string GetRawLanguage();

		// Token: 0x06001077 RID: 4215
		void SetRichPresence(string token, [Nullable(2)] string playerGroup, int? groupSize);

		// Token: 0x06001078 RID: 4216
		void SetRichPresenceValue(string key, [Nullable(2)] string value);

		// Token: 0x06001079 RID: 4217
		void ClearRichPresence();

		// Token: 0x0600107A RID: 4218
		SupportedWindowMode GetSupportedWindowMode();
	}
}
