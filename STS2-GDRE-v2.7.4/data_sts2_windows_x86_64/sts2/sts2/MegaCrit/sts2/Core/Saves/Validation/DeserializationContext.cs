using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Validation
{
	/// <summary>
	/// Collects validation errors during save deserialization.
	/// Threaded through FromSerializable methods to accumulate structured errors with path tracking.
	/// </summary>
	// Token: 0x020000B8 RID: 184
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeserializationContext
	{
		/// <summary>
		/// Pushes a path segment onto the stack (e.g. "Players[0]", "Deck[3]", "Id").
		/// </summary>
		// Token: 0x060008E9 RID: 2281 RVA: 0x00041ABB File Offset: 0x0003FCBB
		public void PushPath(string segment)
		{
			this._pathSegments.Push(segment);
		}

		/// <summary>
		/// Pops the most recent path segment from the stack.
		/// </summary>
		// Token: 0x060008EA RID: 2282 RVA: 0x00041AC9 File Offset: 0x0003FCC9
		public void PopPath()
		{
			this._pathSegments.Pop();
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x00041AD7 File Offset: 0x0003FCD7
		private string CurrentPath
		{
			get
			{
				return string.Join(".", this._pathSegments.Reverse<string>());
			}
		}

		/// <summary>
		/// Records a warning at the current path. Warnings indicate degraded data but the save is still loadable.
		/// </summary>
		// Token: 0x060008EC RID: 2284 RVA: 0x00041AEE File Offset: 0x0003FCEE
		public void Warn(string message)
		{
			this._errors.Add(new ValidationError(ValidationSeverity.Warning, this.CurrentPath, message));
		}

		/// <summary>
		/// Records a fatal error at the current path. Fatal errors mean the save cannot be loaded.
		/// </summary>
		// Token: 0x060008ED RID: 2285 RVA: 0x00041B08 File Offset: 0x0003FD08
		public void Fatal(string message)
		{
			this._errors.Add(new ValidationError(ValidationSeverity.Fatal, this.CurrentPath, message));
		}

		/// <summary>
		/// All validation errors collected so far, in insertion order.
		/// </summary>
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x00041B22 File Offset: 0x0003FD22
		public IReadOnlyList<ValidationError> Errors
		{
			get
			{
				return this._errors;
			}
		}

		/// <summary>
		/// Whether any fatal errors have been recorded.
		/// </summary>
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x00041B2A File Offset: 0x0003FD2A
		public bool HasFatal
		{
			get
			{
				return this._errors.Any<ValidationError>((ValidationError e) => e.IsFatal);
			}
		}

		/// <summary>
		/// Number of warning-severity errors.
		/// </summary>
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00041B56 File Offset: 0x0003FD56
		public int WarningCount
		{
			get
			{
				return this._errors.Count<ValidationError>((ValidationError e) => !e.IsFatal);
			}
		}

		/// <summary>
		/// Number of fatal-severity errors.
		/// </summary>
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00041B82 File Offset: 0x0003FD82
		public int FatalCount
		{
			get
			{
				return this._errors.Count<ValidationError>((ValidationError e) => e.IsFatal);
			}
		}

		// Token: 0x0400030C RID: 780
		private readonly List<ValidationError> _errors = new List<ValidationError>();

		// Token: 0x0400030D RID: 781
		private readonly Stack<string> _pathSegments = new Stack<string>();
	}
}
