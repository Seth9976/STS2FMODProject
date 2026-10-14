using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000D5 RID: 213
	[NullableContext(1)]
	[Nullable(0)]
	public static class SerializationConditionExtensions
	{
		// Token: 0x06000A36 RID: 2614 RVA: 0x0004595F File Offset: 0x00043B5F
		[return: Nullable(2)]
		private static object GetTypeDefaultValue([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type t)
		{
			if (t.IsValueType)
			{
				return Activator.CreateInstance(t);
			}
			return null;
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00045974 File Offset: 0x00043B74
		[return: Nullable(2)]
		private static object GetMemberDefaultValue(MemberInfo info)
		{
			object obj;
			if (!SerializationConditionExtensions._defaultTypeCache.TryGetValue(info.DeclaringType, out obj))
			{
				obj = Activator.CreateInstance(info.DeclaringType);
				SerializationConditionExtensions._defaultTypeCache[info.DeclaringType] = obj;
			}
			MemberTypes memberType = info.MemberType;
			if (memberType == MemberTypes.Field)
			{
				return ((FieldInfo)info).GetValue(obj);
			}
			if (memberType != MemberTypes.Property)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Input MemberInfo must be of type FieldInfo or PropertyInfo. It was ");
				defaultInterpolatedStringHandler.AppendFormatted<MemberTypes>(info.MemberType);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(info.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return ((PropertyInfo)info).GetValue(obj);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00045A34 File Offset: 0x00043C34
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		private static Type GetUnderlyingType(this MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType == MemberTypes.Field)
			{
				return ((FieldInfo)member).FieldType;
			}
			if (memberType != MemberTypes.Property)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Input MemberInfo must be of type FieldInfo or PropertyInfo. It was ");
				defaultInterpolatedStringHandler.AppendFormatted<MemberTypes>(member.MemberType);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(member.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return ((PropertyInfo)member).PropertyType;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00045AC0 File Offset: 0x00043CC0
		public static bool ShouldSerialize(this SerializationCondition condition, [Nullable(2)] object candidate, MemberInfo memberInfo)
		{
			switch (condition)
			{
			case SerializationCondition.AlwaysSave:
				return true;
			case SerializationCondition.SaveIfNotPropertyDefault:
				return !object.Equals(candidate, SerializationConditionExtensions.GetMemberDefaultValue(memberInfo));
			case SerializationCondition.SaveIfNotTypeDefault:
				return !object.Equals(candidate, SerializationConditionExtensions.GetTypeDefaultValue(memberInfo.GetUnderlyingType()));
			case SerializationCondition.SaveIfNotCollectionEmptyOrNull:
			{
				ICollection collection = candidate as ICollection;
				return collection != null && collection.Count != 0;
			}
			default:
				throw new InvalidEnumArgumentException("Invalid condition passed!");
			}
		}

		// Token: 0x0400038B RID: 907
		[Nullable(new byte[] { 1, 1, 2 })]
		private static readonly Dictionary<Type, object> _defaultTypeCache = new Dictionary<Type, object>();
	}
}
