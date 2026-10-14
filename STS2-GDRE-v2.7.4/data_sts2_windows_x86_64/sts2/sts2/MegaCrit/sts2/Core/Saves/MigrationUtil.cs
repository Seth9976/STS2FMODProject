using System;
using System.IO;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Contains utilities for moving files around during a migration of the save directory location.
	/// </summary>
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(0)]
	public static class MigrationUtil
	{
		// Token: 0x060006FE RID: 1790 RVA: 0x0003BCAC File Offset: 0x00039EAC
		public static bool MigrateDirectory(string directoryName, string legacyBasePath, string newBasePath)
		{
			string text = Path.Combine(legacyBasePath, directoryName);
			string text2 = Path.Combine(newBasePath, directoryName);
			if (!Directory.Exists(text))
			{
				Log.Info("No legacy " + directoryName + " directory found, skipping", 2);
				return false;
			}
			if (Directory.Exists(text2))
			{
				Log.Info("Skipping migration because " + text2 + " already exists", 2);
				return false;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Migrating ");
			defaultInterpolatedStringHandler.AppendFormatted(directoryName);
			defaultInterpolatedStringHandler.AppendLiteral(" directory from ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" to ");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			bool flag;
			try
			{
				Directory.CreateDirectory(text2);
				MigrationUtil.CopyDirectoryRecursively(text, text2);
				Log.Info("Successfully migrated " + directoryName + " directory", 2);
				flag = true;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to migrate " + directoryName + " directory: " + ex.Message, 2);
				flag = false;
			}
			return flag;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0003BDB8 File Offset: 0x00039FB8
		public static bool MigrateFile(string fileName, string legacyBasePath, string newBasePath)
		{
			string text = Path.Combine(legacyBasePath, fileName);
			string text2 = Path.Combine(newBasePath, fileName);
			if (!File.Exists(text))
			{
				Log.Info("No legacy " + fileName + " file found, skipping", 2);
				return false;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Migrating ");
			defaultInterpolatedStringHandler.AppendFormatted(fileName);
			defaultInterpolatedStringHandler.AppendLiteral(" from ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" to ");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			bool flag;
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(text2));
				File.Copy(text, text2, true);
				Log.Info("Successfully migrated " + fileName, 2);
				flag = true;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to migrate " + fileName + ": " + ex.Message, 2);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0003BEA8 File Offset: 0x0003A0A8
		public static void CopyDirectoryRecursively(string sourceDir, string targetDir)
		{
			Directory.CreateDirectory(targetDir);
			foreach (string text in Directory.GetFiles(sourceDir))
			{
				string fileName = Path.GetFileName(text);
				string text2 = Path.Combine(targetDir, fileName);
				File.Copy(text, text2, true);
			}
			foreach (string text3 in Directory.GetDirectories(sourceDir))
			{
				string fileName2 = Path.GetFileName(text3);
				string text4 = Path.Combine(targetDir, fileName2);
				MigrationUtil.CopyDirectoryRecursively(text3, text4);
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0003BF2C File Offset: 0x0003A12C
		public static void ArchiveLegacyDirectory(string directoryPath, string archivePath)
		{
			if (!Directory.Exists(directoryPath))
			{
				return;
			}
			try
			{
				string fileName = Path.GetFileName(directoryPath);
				string text = Path.Combine(archivePath, fileName);
				Directory.Move(directoryPath, text);
				Log.Info("Archived legacy directory: " + directoryPath + " -> " + text, 2);
			}
			catch (Exception ex)
			{
				Log.Warn("Could not archive legacy directory " + directoryPath + ": " + ex.Message, 2);
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0003BFA4 File Offset: 0x0003A1A4
		public static void ArchiveLegacyFile(string filePath, string archivePath)
		{
			if (!File.Exists(filePath))
			{
				return;
			}
			try
			{
				string fileName = Path.GetFileName(filePath);
				string text = Path.Combine(archivePath, fileName);
				File.Move(filePath, text);
				Log.Info("Archived legacy file: " + filePath + " -> " + text, 2);
			}
			catch (Exception ex)
			{
				Log.Warn("Could not archive legacy file " + filePath + ": " + ex.Message, 2);
			}
		}
	}
}
