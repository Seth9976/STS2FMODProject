using System;
using System.Buffers.Binary;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// Hashes assemblies to aid in mod detection.
	/// </summary>
	// Token: 0x02000D07 RID: 3335
	public static class AssemblyHasher
	{
		// Token: 0x060086AB RID: 34475 RVA: 0x002D3118 File Offset: 0x002D1318
		public static int GetMainAssemblyHash()
		{
			if (AssemblyHasher._mainAssemblyHash != null)
			{
				return AssemblyHasher._mainAssemblyHash.Value;
			}
			if (OS.HasFeature("editor"))
			{
				Log.Info("Assembly hashing disabled in editor", 2);
				AssemblyHasher._mainAssemblyHash = new int?(0);
				return 0;
			}
			Assembly assembly = typeof(AssemblyHasher).Assembly;
			try
			{
				using (FileStream fileStream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					byte[] array = SHA1.HashData(fileStream);
					AssemblyHasher._mainAssemblyHash = new int?(BinaryPrimitives.ReadInt32LittleEndian(array));
				}
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Could not read main assembly ");
				defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
				defaultInterpolatedStringHandler.AppendLiteral(" from ");
				defaultInterpolatedStringHandler.AppendFormatted(assembly.Location);
				defaultInterpolatedStringHandler.AppendLiteral(". Exception: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				AssemblyHasher._mainAssemblyHash = new int?(0);
			}
			return AssemblyHasher._mainAssemblyHash.Value;
		}

		// Token: 0x04002E98 RID: 11928
		private static int? _mainAssemblyHash;
	}
}
