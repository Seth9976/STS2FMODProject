using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models.Exceptions
{
	// Token: 0x02000824 RID: 2084
	public class CanonicalModelException : Exception
	{
		// Token: 0x0600675B RID: 26459 RVA: 0x002725E4 File Offset: 0x002707E4
		[NullableContext(1)]
		public CanonicalModelException(Type t)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Canonical model of type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(t);
			defaultInterpolatedStringHandler.AppendLiteral(" used in incorrect place.");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
