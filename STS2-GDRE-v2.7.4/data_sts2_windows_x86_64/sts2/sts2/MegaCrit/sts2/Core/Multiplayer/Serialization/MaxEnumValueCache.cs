using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x0200044A RID: 1098
	public static class MaxEnumValueCache
	{
		// Token: 0x060044DE RID: 17630 RVA: 0x002137D8 File Offset: 0x002119D8
		public static int Get<T>() where T : struct, Enum
		{
			int num;
			if (!MaxEnumValueCache._maxEnumValues.TryGetValue(typeof(T), out num))
			{
				if (!typeof(int).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Trying to get max value for enum type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
					defaultInterpolatedStringHandler.AppendLiteral(" that is not assignable to int!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				num = (from v in Enum.GetValues<T>()
					select Convert.ToInt32(v)).Max();
				MaxEnumValueCache._maxEnumValues[typeof(T)] = num;
			}
			return num;
		}

		// Token: 0x04001AC3 RID: 6851
		[Nullable(1)]
		private static readonly Dictionary<Type, int> _maxEnumValues = new Dictionary<Type, int>();
	}
}
