using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Models.Exceptions
{
	// Token: 0x02000826 RID: 2086
	public class ModelNotFoundException : Exception
	{
		// Token: 0x0600675D RID: 26461 RVA: 0x00272634 File Offset: 0x00270834
		[NullableContext(1)]
		public ModelNotFoundException(ModelId id)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Model id=");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
			defaultInterpolatedStringHandler.AppendLiteral(" not found");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
