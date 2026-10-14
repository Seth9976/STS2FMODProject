using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MegaCrit.Sts2.Core.Entities.Text
{
	// Token: 0x02000C60 RID: 3168
	[NullableContext(2)]
	[Nullable(0)]
	public class BbcodeObject : IEquatable<BbcodeObject>
	{
		// Token: 0x17002391 RID: 9105
		// (get) Token: 0x0600823A RID: 33338 RVA: 0x002C0C6D File Offset: 0x002BEE6D
		[Nullable(1)]
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(BbcodeObject);
			}
		}

		// Token: 0x0600823B RID: 33339 RVA: 0x002C0C7C File Offset: 0x002BEE7C
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BbcodeObject");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600823C RID: 33340 RVA: 0x002C0CC8 File Offset: 0x002BEEC8
		[NullableContext(1)]
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("type = ");
			builder.Append(this.type.ToString());
			builder.Append(", text = ");
			builder.Append(this.text);
			builder.Append(", tag = ");
			builder.Append(this.tag);
			return true;
		}

		// Token: 0x0600823D RID: 33341 RVA: 0x002C0D31 File Offset: 0x002BEF31
		[CompilerGenerated]
		public static bool operator !=(BbcodeObject left, BbcodeObject right)
		{
			return !(left == right);
		}

		// Token: 0x0600823E RID: 33342 RVA: 0x002C0D3D File Offset: 0x002BEF3D
		[CompilerGenerated]
		public static bool operator ==(BbcodeObject left, BbcodeObject right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x0600823F RID: 33343 RVA: 0x002C0D54 File Offset: 0x002BEF54
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<BbcodeObjectType>.Default.GetHashCode(this.type)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.text)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.tag);
		}

		// Token: 0x06008240 RID: 33344 RVA: 0x002C0DB6 File Offset: 0x002BEFB6
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as BbcodeObject);
		}

		// Token: 0x06008241 RID: 33345 RVA: 0x002C0DC4 File Offset: 0x002BEFC4
		[CompilerGenerated]
		public virtual bool Equals(BbcodeObject other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<BbcodeObjectType>.Default.Equals(this.type, other.type) && EqualityComparer<string>.Default.Equals(this.text, other.text) && EqualityComparer<string>.Default.Equals(this.tag, other.tag));
		}

		// Token: 0x06008243 RID: 33347 RVA: 0x002C0E3D File Offset: 0x002BF03D
		[CompilerGenerated]
		protected BbcodeObject([Nullable(1)] BbcodeObject original)
		{
			this.type = original.type;
			this.text = original.text;
			this.tag = original.tag;
		}

		// Token: 0x06008244 RID: 33348 RVA: 0x002C0E69 File Offset: 0x002BF069
		public BbcodeObject()
		{
		}

		// Token: 0x04002BC6 RID: 11206
		public BbcodeObjectType type;

		// Token: 0x04002BC7 RID: 11207
		public string text;

		// Token: 0x04002BC8 RID: 11208
		public string tag;
	}
}
