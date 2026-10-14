using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// Manages the loading and access of release information.
	/// </summary>
	// Token: 0x02000D15 RID: 3349
	[NullableContext(2)]
	[Nullable(0)]
	public class ReleaseInfoManager
	{
		/// <summary>
		/// Gets the singleton instance of ReleaseInfoManager.
		/// </summary>
		// Token: 0x17002536 RID: 9526
		// (get) Token: 0x06008723 RID: 34595 RVA: 0x002D6EED File Offset: 0x002D50ED
		[Nullable(1)]
		public static ReleaseInfoManager Instance
		{
			[NullableContext(1)]
			get
			{
				ReleaseInfoManager releaseInfoManager;
				if ((releaseInfoManager = ReleaseInfoManager._instance) == null)
				{
					releaseInfoManager = (ReleaseInfoManager._instance = new ReleaseInfoManager());
				}
				return releaseInfoManager;
			}
		}

		/// <summary>
		/// Gets the loaded release information.
		/// </summary>
		// Token: 0x17002537 RID: 9527
		// (get) Token: 0x06008724 RID: 34596 RVA: 0x002D6F03 File Offset: 0x002D5103
		public ReleaseInfo ReleaseInfo { get; }

		/// <summary>
		/// Gets the game version as a semantic version, parsed from ReleaseInfo.version.
		/// </summary>
		// Token: 0x17002538 RID: 9528
		// (get) Token: 0x06008725 RID: 34597 RVA: 0x002D6F0B File Offset: 0x002D510B
		public SemanticVersion SemVer { get; }

		// Token: 0x06008726 RID: 34598 RVA: 0x002D6F14 File Offset: 0x002D5114
		private ReleaseInfoManager()
		{
			this.ReleaseInfo = this.LoadConfig();
			ReleaseInfo releaseInfo = this.ReleaseInfo;
			if (((releaseInfo != null) ? releaseInfo.Version : null) != null)
			{
				SemanticVersion semanticVersion;
				SemanticVersion.TryFromString(this.ReleaseInfo.Version, out semanticVersion);
				this.SemVer = semanticVersion;
			}
		}

		/// <summary>
		/// Loads the release configuration.
		/// </summary>
		// Token: 0x06008727 RID: 34599 RVA: 0x002D6F64 File Offset: 0x002D5164
		private ReleaseInfo LoadConfig()
		{
			string[] possibleReleaseInfoPaths = ReleaseInfoManager.GetPossibleReleaseInfoPaths();
			foreach (string text in possibleReleaseInfoPaths)
			{
				string text2 = ProjectSettings.GlobalizePath(text);
				if (FileAccess.FileExists(text2))
				{
					Log.Info("Found release_info.json at: " + text2, 2);
					using (FileAccess fileAccess = FileAccess.Open(text2, 1L))
					{
						if (fileAccess == null)
						{
							Log.Error("Failed to open file: " + text2, 2);
						}
						else
						{
							try
							{
								string asText = fileAccess.GetAsText(false);
								return JsonSerializer.Deserialize<ReleaseInfo>(asText, ReleaseInfoJsonSerializerContext.Default.ReleaseInfo);
							}
							catch (JsonException ex)
							{
								Log.Error("Failed to deserialize release_info.json: " + ex.Message, 2);
							}
							catch (Exception ex2)
							{
								Log.Error("Unexpected error reading release_info.json: " + ex2.Message, 2);
							}
						}
					}
				}
			}
			Log.Info("File `release_info.json` not found in any of the expected locations.", 2);
			return null;
		}

		/// <summary>
		/// Gets possible paths for the release info file.
		/// </summary>
		// Token: 0x06008728 RID: 34600 RVA: 0x002D7074 File Offset: 0x002D5274
		[NullableContext(1)]
		private static string[] GetPossibleReleaseInfoPaths()
		{
			string executablePath = OS.GetExecutablePath();
			string text = Path.GetDirectoryName(executablePath) ?? string.Empty;
			if (OS.GetName() == "macOS")
			{
				string text2 = Path.Combine(text, "..", "Resources");
				return new string[]
				{
					Path.Combine(text2, "release_info.json"),
					Path.Combine(text, "release_info.json")
				};
			}
			return new string[] { Path.Combine(text, "release_info.json") };
		}

		// Token: 0x04002ED3 RID: 11987
		private static ReleaseInfoManager _instance;

		// Token: 0x04002ED4 RID: 11988
		[Nullable(1)]
		private const string _releaseInfoFileName = "release_info.json";
	}
}
