using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x0200016D RID: 365
	[NullableContext(1)]
	[Nullable(0)]
	public class NullPlatformUtilStrategy : IPlatformUtilStrategy
	{
		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x000645C0 File Offset: 0x000627C0
		public ulong LocalPlayerId { get; } = 1UL;

		// Token: 0x06001115 RID: 4373 RVA: 0x000645C8 File Offset: 0x000627C8
		public NullPlatformUtilStrategy()
		{
			string text;
			ulong num;
			if (CommandLineHelper.TryGetValue("clientId", out text) && ulong.TryParse(text, out num))
			{
				this.LocalPlayerId = num;
			}
			GodotFileIo godotFileIo = new GodotFileIo(".");
			if (godotFileIo.FileExists("mp_names.json"))
			{
				string text2 = godotFileIo.ReadFile("mp_names.json");
				this._mpNames = JsonSerializer.Deserialize<List<NullMultiplayerName>>(text2, JsonSerializationUtility.GetTypeInfo<List<NullMultiplayerName>>());
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00064637 File Offset: 0x00062837
		public bool SupportsInviteDialog
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x0006463A File Offset: 0x0006283A
		public bool IsPlatformOverlayOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00064640 File Offset: 0x00062840
		public string GetPlayerName(ulong playerId)
		{
			if (this._mpNames != null)
			{
				foreach (NullMultiplayerName nullMultiplayerName in this._mpNames)
				{
					if (nullMultiplayerName.netId == playerId)
					{
						return nullMultiplayerName.name;
					}
				}
			}
			if (playerId <= 1000UL)
			{
				if (playerId == 1UL)
				{
					return "Test Host";
				}
				if (playerId == 1000UL)
				{
					return "Test Client 1";
				}
			}
			else
			{
				if (playerId == 2000UL)
				{
					return "Test Client 2";
				}
				if (playerId == 3000UL)
				{
					return "Test Client 3";
				}
			}
			return playerId.ToString();
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00064700 File Offset: 0x00062900
		public ulong GetLocalPlayerId()
		{
			return this.LocalPlayerId;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00064708 File Offset: 0x00062908
		public Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies()
		{
			return Task.FromResult<IEnumerable<ulong>>(Array.Empty<ulong>());
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00064714 File Offset: 0x00062914
		public void OpenInviteDialog(INetGameService netService)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0006471B File Offset: 0x0006291B
		public void OpenUrl(string url)
		{
			OS.ShellOpen(url);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00064724 File Offset: 0x00062924
		public void OpenVirtualKeyboard()
		{
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00064726 File Offset: 0x00062926
		public void CloseVirtualKeyboard()
		{
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00064728 File Offset: 0x00062928
		public void SetRichPresence(string token, [Nullable(2)] string playerGroup, int? groupSize)
		{
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0006472A File Offset: 0x0006292A
		public void SetRichPresenceValue(string key, [Nullable(2)] string value)
		{
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0006472C File Offset: 0x0006292C
		public void ClearRichPresence()
		{
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0006472E File Offset: 0x0006292E
		public PlatformBranch GetPlatformBranch()
		{
			return PlatformBranch.None;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00064734 File Offset: 0x00062934
		[NullableContext(2)]
		public string GetThreeLetterLanguageCode()
		{
			CultureInfo cultureInfo = new CultureInfo(this.GetRawLanguage());
			if (LocManager.Languages.Contains(cultureInfo.ThreeLetterISOLanguageName))
			{
				return cultureInfo.ThreeLetterISOLanguageName;
			}
			string text = cultureInfo.Name.ToLowerInvariant();
			text = text.Replace('-', '_');
			if (text.StartsWith("zh"))
			{
				if (text.StartsWith("zh_hans") || text.StartsWith("zh_cn"))
				{
					return "zhs";
				}
				if (text.StartsWith("zh_hant") || text.StartsWith("zh_tw"))
				{
					return "zht";
				}
				return "zhs";
			}
			else
			{
				if (!text.StartsWith("pt"))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
					defaultInterpolatedStringHandler.AppendLiteral("CultureInfo ");
					defaultInterpolatedStringHandler.AppendFormatted<CultureInfo>(cultureInfo);
					defaultInterpolatedStringHandler.AppendLiteral(" could not be mapped to a three-letter language code!");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					return null;
				}
				if (text.StartsWith("pt_br"))
				{
					return "ptb";
				}
				return "por";
			}
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0006482E File Offset: 0x00062A2E
		public string GetRawLanguage()
		{
			return OS.GetLocale().Replace('_', '-');
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0006483E File Offset: 0x00062A3E
		public SupportedWindowMode GetSupportedWindowMode()
		{
			return SupportedWindowMode.Any;
		}

		// Token: 0x04000613 RID: 1555
		private const string _multiplayerNamesFile = "mp_names.json";

		// Token: 0x04000614 RID: 1556
		[Nullable(new byte[] { 2, 1 })]
		private List<NullMultiplayerName> _mpNames;
	}
}
