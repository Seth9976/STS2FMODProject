using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MegaCrit.Sts2.Core.Localization
{
	/// <summary>
	/// Represents a validation error found in a localization override file.
	/// </summary>
	// Token: 0x02000BC1 RID: 3009
	[NullableContext(1)]
	[Nullable(0)]
	public class LocValidationError : IEquatable<LocValidationError>
	{
		/// <summary>
		/// Represents a validation error found in a localization override file.
		/// </summary>
		// Token: 0x06007D81 RID: 32129 RVA: 0x002ABF7C File Offset: 0x002AA17C
		public LocValidationError(string FilePath, string Key, string ErrorMessage)
		{
			this.FilePath = FilePath;
			this.Key = Key;
			this.ErrorMessage = ErrorMessage;
			base..ctor();
		}

		// Token: 0x170022AA RID: 8874
		// (get) Token: 0x06007D82 RID: 32130 RVA: 0x002ABF99 File Offset: 0x002AA199
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(LocValidationError);
			}
		}

		// Token: 0x170022AB RID: 8875
		// (get) Token: 0x06007D83 RID: 32131 RVA: 0x002ABFA5 File Offset: 0x002AA1A5
		// (set) Token: 0x06007D84 RID: 32132 RVA: 0x002ABFAD File Offset: 0x002AA1AD
		public string FilePath { get; set; }

		// Token: 0x170022AC RID: 8876
		// (get) Token: 0x06007D85 RID: 32133 RVA: 0x002ABFB6 File Offset: 0x002AA1B6
		// (set) Token: 0x06007D86 RID: 32134 RVA: 0x002ABFBE File Offset: 0x002AA1BE
		public string Key { get; set; }

		// Token: 0x170022AD RID: 8877
		// (get) Token: 0x06007D87 RID: 32135 RVA: 0x002ABFC7 File Offset: 0x002AA1C7
		// (set) Token: 0x06007D88 RID: 32136 RVA: 0x002ABFCF File Offset: 0x002AA1CF
		public string ErrorMessage { get; set; }

		// Token: 0x06007D89 RID: 32137 RVA: 0x002ABFD8 File Offset: 0x002AA1D8
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("LocValidationError");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x002AC024 File Offset: 0x002AA224
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("FilePath = ");
			builder.Append(this.FilePath);
			builder.Append(", Key = ");
			builder.Append(this.Key);
			builder.Append(", ErrorMessage = ");
			builder.Append(this.ErrorMessage);
			return true;
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x002AC082 File Offset: 0x002AA282
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(LocValidationError left, LocValidationError right)
		{
			return !(left == right);
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x002AC08E File Offset: 0x002AA28E
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(LocValidationError left, LocValidationError right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x002AC0A4 File Offset: 0x002AA2A4
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<FilePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Key>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ErrorMessage>k__BackingField);
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x002AC106 File Offset: 0x002AA306
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as LocValidationError);
		}

		// Token: 0x06007D8F RID: 32143 RVA: 0x002AC114 File Offset: 0x002AA314
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(LocValidationError other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<FilePath>k__BackingField, other.<FilePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Key>k__BackingField, other.<Key>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ErrorMessage>k__BackingField, other.<ErrorMessage>k__BackingField));
		}

		// Token: 0x06007D91 RID: 32145 RVA: 0x002AC18D File Offset: 0x002AA38D
		[CompilerGenerated]
		protected LocValidationError(LocValidationError original)
		{
			this.FilePath = original.<FilePath>k__BackingField;
			this.Key = original.<Key>k__BackingField;
			this.ErrorMessage = original.<ErrorMessage>k__BackingField;
		}

		// Token: 0x06007D92 RID: 32146 RVA: 0x002AC1B9 File Offset: 0x002AA3B9
		[CompilerGenerated]
		public void Deconstruct(out string FilePath, out string Key, out string ErrorMessage)
		{
			FilePath = this.FilePath;
			Key = this.Key;
			ErrorMessage = this.ErrorMessage;
		}
	}
}
