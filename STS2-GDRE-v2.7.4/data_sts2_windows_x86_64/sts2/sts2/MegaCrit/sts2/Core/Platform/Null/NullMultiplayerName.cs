using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x0200016C RID: 364
	[NullableContext(1)]
	[Nullable(0)]
	public class NullMultiplayerName : IEquatable<NullMultiplayerName>
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x00064413 File Offset: 0x00062613
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(NullMultiplayerName);
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00064420 File Offset: 0x00062620
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("NullMultiplayerName");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0006446C File Offset: 0x0006266C
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("netId = ");
			builder.Append(this.netId.ToString());
			builder.Append(", name = ");
			builder.Append(this.name);
			return true;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000644BC File Offset: 0x000626BC
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(NullMultiplayerName left, NullMultiplayerName right)
		{
			return !(left == right);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x000644C8 File Offset: 0x000626C8
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(NullMultiplayerName left, NullMultiplayerName right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x000644DC File Offset: 0x000626DC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.netId)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.name);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0006451C File Offset: 0x0006271C
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NullMultiplayerName);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0006452C File Offset: 0x0006272C
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(NullMultiplayerName other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ulong>.Default.Equals(this.netId, other.netId) && EqualityComparer<string>.Default.Equals(this.name, other.name));
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0006458D File Offset: 0x0006278D
		[CompilerGenerated]
		protected NullMultiplayerName(NullMultiplayerName original)
		{
			this.netId = original.netId;
			this.name = original.name;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x000645AD File Offset: 0x000627AD
		public NullMultiplayerName()
		{
			this.name = "";
			base..ctor();
		}

		// Token: 0x04000611 RID: 1553
		[JsonPropertyName("net_id")]
		public ulong netId;

		// Token: 0x04000612 RID: 1554
		[JsonPropertyName("name")]
		public string name;
	}
}
