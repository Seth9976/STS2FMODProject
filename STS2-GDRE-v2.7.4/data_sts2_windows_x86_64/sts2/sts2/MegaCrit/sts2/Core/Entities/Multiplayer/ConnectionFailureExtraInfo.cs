using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	/// <summary>
	/// Class for holding extra info to display in multiplayer errors if the error is a ConnectionFailureReason.
	/// </summary>
	// Token: 0x02000C81 RID: 3201
	[NullableContext(2)]
	[Nullable(0)]
	public class ConnectionFailureExtraInfo : IEquatable<ConnectionFailureExtraInfo>
	{
		// Token: 0x170023E4 RID: 9188
		// (get) Token: 0x06008350 RID: 33616 RVA: 0x002C4623 File Offset: 0x002C2823
		[Nullable(1)]
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(ConnectionFailureExtraInfo);
			}
		}

		// Token: 0x06008351 RID: 33617 RVA: 0x002C4630 File Offset: 0x002C2830
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ConnectionFailureExtraInfo");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06008352 RID: 33618 RVA: 0x002C467C File Offset: 0x002C287C
		[NullableContext(1)]
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("missingModsOnLocal = ");
			builder.Append(this.missingModsOnLocal);
			builder.Append(", missingModsOnHost = ");
			builder.Append(this.missingModsOnHost);
			builder.Append(", hostVersion = ");
			builder.Append(this.hostVersion);
			builder.Append(", hostBranch = ");
			builder.Append(this.hostBranch.ToString());
			builder.Append(", hostHash = ");
			builder.Append(this.hostHash.ToString());
			builder.Append(", localVersion = ");
			builder.Append(this.localVersion);
			builder.Append(", localBranch = ");
			builder.Append(this.localBranch.ToString());
			builder.Append(", localHash = ");
			builder.Append(this.localHash.ToString());
			return true;
		}

		// Token: 0x06008353 RID: 33619 RVA: 0x002C4783 File Offset: 0x002C2983
		[CompilerGenerated]
		public static bool operator !=(ConnectionFailureExtraInfo left, ConnectionFailureExtraInfo right)
		{
			return !(left == right);
		}

		// Token: 0x06008354 RID: 33620 RVA: 0x002C478F File Offset: 0x002C298F
		[CompilerGenerated]
		public static bool operator ==(ConnectionFailureExtraInfo left, ConnectionFailureExtraInfo right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06008355 RID: 33621 RVA: 0x002C47A4 File Offset: 0x002C29A4
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.missingModsOnLocal)) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.missingModsOnHost)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.hostVersion)) * -1521134295 + EqualityComparer<PlatformBranch?>.Default.GetHashCode(this.hostBranch)) * -1521134295 + EqualityComparer<ulong?>.Default.GetHashCode(this.hostHash)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.localVersion)) * -1521134295 + EqualityComparer<PlatformBranch?>.Default.GetHashCode(this.localBranch)) * -1521134295 + EqualityComparer<ulong?>.Default.GetHashCode(this.localHash);
		}

		// Token: 0x06008356 RID: 33622 RVA: 0x002C4879 File Offset: 0x002C2A79
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ConnectionFailureExtraInfo);
		}

		// Token: 0x06008357 RID: 33623 RVA: 0x002C4888 File Offset: 0x002C2A88
		[CompilerGenerated]
		public virtual bool Equals(ConnectionFailureExtraInfo other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<List<string>>.Default.Equals(this.missingModsOnLocal, other.missingModsOnLocal) && EqualityComparer<List<string>>.Default.Equals(this.missingModsOnHost, other.missingModsOnHost) && EqualityComparer<string>.Default.Equals(this.hostVersion, other.hostVersion) && EqualityComparer<PlatformBranch?>.Default.Equals(this.hostBranch, other.hostBranch) && EqualityComparer<ulong?>.Default.Equals(this.hostHash, other.hostHash) && EqualityComparer<string>.Default.Equals(this.localVersion, other.localVersion) && EqualityComparer<PlatformBranch?>.Default.Equals(this.localBranch, other.localBranch) && EqualityComparer<ulong?>.Default.Equals(this.localHash, other.localHash));
		}

		// Token: 0x06008359 RID: 33625 RVA: 0x002C4988 File Offset: 0x002C2B88
		[CompilerGenerated]
		protected ConnectionFailureExtraInfo([Nullable(1)] ConnectionFailureExtraInfo original)
		{
			this.missingModsOnLocal = original.missingModsOnLocal;
			this.missingModsOnHost = original.missingModsOnHost;
			this.hostVersion = original.hostVersion;
			this.hostBranch = original.hostBranch;
			this.hostHash = original.hostHash;
			this.localVersion = original.localVersion;
			this.localBranch = original.localBranch;
			this.localHash = original.localHash;
		}

		// Token: 0x0600835A RID: 33626 RVA: 0x002C49FB File Offset: 0x002C2BFB
		public ConnectionFailureExtraInfo()
		{
		}

		/// <summary>
		/// The mods that the host has that we are missing on our end.
		/// Only set on ConnectionFailureReason.ModMismatch.
		/// </summary>
		// Token: 0x04002C75 RID: 11381
		[Nullable(new byte[] { 2, 1 })]
		public List<string> missingModsOnLocal;

		/// <summary>
		/// The mods that we have that are missing on the host's end.
		/// Only set on ConnectionFailureReason.ModMismatch.
		/// </summary>
		// Token: 0x04002C76 RID: 11382
		[Nullable(new byte[] { 2, 1 })]
		public List<string> missingModsOnHost;

		/// <summary>
		/// The version of the game the host is playing on.
		/// </summary>
		// Token: 0x04002C77 RID: 11383
		public string hostVersion;

		/// <summary>
		/// The steam branch the host is playing on.
		/// </summary>
		// Token: 0x04002C78 RID: 11384
		public PlatformBranch? hostBranch;

		/// <summary>
		/// The ModelDb hash reported by the host.
		/// </summary>
		// Token: 0x04002C79 RID: 11385
		public ulong? hostHash;

		/// <summary>
		/// The version of the game that we are playing on.
		/// </summary>
		// Token: 0x04002C7A RID: 11386
		public string localVersion;

		/// <summary>
		/// The steam branch that we are playing on.
		/// </summary>
		// Token: 0x04002C7B RID: 11387
		public PlatformBranch? localBranch;

		/// <summary>
		/// The ModelDb hash reported by us.
		/// </summary>
		// Token: 0x04002C7C RID: 11388
		public ulong? localHash;
	}
}
