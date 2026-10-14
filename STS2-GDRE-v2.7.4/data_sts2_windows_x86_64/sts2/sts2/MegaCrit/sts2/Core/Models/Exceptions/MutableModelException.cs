using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models.Exceptions
{
	// Token: 0x02000827 RID: 2087
	public class MutableModelException : Exception
	{
		// Token: 0x0600675E RID: 26462 RVA: 0x00272678 File Offset: 0x00270878
		[NullableContext(1)]
		public MutableModelException(Type t)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Mutable model of type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(t);
			defaultInterpolatedStringHandler.AppendLiteral(" used in incorrect place.");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
