using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MegaCrit.Sts2.Core.Saves.Validation
{
	/// <summary>
	/// A single validation error found during save deserialization.
	/// </summary>
	/// <param name="Severity">Whether this error is fatal or a warning</param>
	/// <param name="Path">Dot-separated field path where the error occurred (e.g. "Players[0].Deck[3].Id")</param>
	/// <param name="Message">Human-readable description of the error</param>
	// Token: 0x020000BA RID: 186
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ValidationError : IEquatable<ValidationError>
	{
		/// <summary>
		/// A single validation error found during save deserialization.
		/// </summary>
		/// <param name="Severity">Whether this error is fatal or a warning</param>
		/// <param name="Path">Dot-separated field path where the error occurred (e.g. "Players[0].Deck[3].Id")</param>
		/// <param name="Message">Human-readable description of the error</param>
		// Token: 0x060008F3 RID: 2291 RVA: 0x00041BCC File Offset: 0x0003FDCC
		public ValidationError(ValidationSeverity Severity, string Path, string Message)
		{
			this.Severity = Severity;
			this.Path = Path;
			this.Message = Message;
			base..ctor();
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00041BE9 File Offset: 0x0003FDE9
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ValidationError);
			}
		}

		/// <summary>Whether this error is fatal or a warning</summary>
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00041BF5 File Offset: 0x0003FDF5
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00041BFD File Offset: 0x0003FDFD
		public ValidationSeverity Severity { get; set; }

		/// <summary>Dot-separated field path where the error occurred (e.g. "Players[0].Deck[3].Id")</summary>
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00041C06 File Offset: 0x0003FE06
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00041C0E File Offset: 0x0003FE0E
		public string Path { get; set; }

		/// <summary>Human-readable description of the error</summary>
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00041C17 File Offset: 0x0003FE17
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x00041C1F File Offset: 0x0003FE1F
		public string Message { get; set; }

		/// <summary>
		/// Whether this error is fatal (the save cannot be loaded).
		/// </summary>
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x00041C28 File Offset: 0x0003FE28
		public bool IsFatal
		{
			get
			{
				return this.Severity == ValidationSeverity.Fatal;
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00041C34 File Offset: 0x0003FE34
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ValidationError");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00041C80 File Offset: 0x0003FE80
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Severity = ");
			builder.Append(this.Severity.ToString());
			builder.Append(", Path = ");
			builder.Append(this.Path);
			builder.Append(", Message = ");
			builder.Append(this.Message);
			builder.Append(", IsFatal = ");
			builder.Append(this.IsFatal.ToString());
			return true;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00041D13 File Offset: 0x0003FF13
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ValidationError left, ValidationError right)
		{
			return !(left == right);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00041D1F File Offset: 0x0003FF1F
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ValidationError left, ValidationError right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00041D34 File Offset: 0x0003FF34
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ValidationSeverity>.Default.GetHashCode(this.<Severity>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Path>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Message>k__BackingField);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00041D96 File Offset: 0x0003FF96
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ValidationError);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00041DA4 File Offset: 0x0003FFA4
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ValidationError other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ValidationSeverity>.Default.Equals(this.<Severity>k__BackingField, other.<Severity>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Path>k__BackingField, other.<Path>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Message>k__BackingField, other.<Message>k__BackingField));
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00041E1D File Offset: 0x0004001D
		[CompilerGenerated]
		private ValidationError(ValidationError original)
		{
			this.Severity = original.<Severity>k__BackingField;
			this.Path = original.<Path>k__BackingField;
			this.Message = original.<Message>k__BackingField;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00041E49 File Offset: 0x00040049
		[CompilerGenerated]
		public void Deconstruct(out ValidationSeverity Severity, out string Path, out string Message)
		{
			Severity = this.Severity;
			Path = this.Path;
			Message = this.Message;
		}
	}
}
