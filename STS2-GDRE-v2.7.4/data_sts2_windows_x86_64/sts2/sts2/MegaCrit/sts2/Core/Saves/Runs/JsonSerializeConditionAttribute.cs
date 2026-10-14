using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Metadata;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000BE RID: 190
	public class JsonSerializeConditionAttribute : Attribute
	{
		// Token: 0x0600092A RID: 2346 RVA: 0x000427AC File Offset: 0x000409AC
		public JsonSerializeConditionAttribute(SerializationCondition defaultBehaviour)
		{
			this.defaultBehaviour = defaultBehaviour;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000427BC File Offset: 0x000409BC
		[NullableContext(1)]
		public static void CheckJsonSerializeConditionsModifier(JsonTypeInfo typeInfo)
		{
			if (typeInfo.Kind != JsonTypeInfoKind.Object)
			{
				return;
			}
			foreach (JsonPropertyInfo jsonPropertyInfo in typeInfo.Properties)
			{
				JsonSerializeConditionAttribute.<>c__DisplayClass2_0 CS$<>8__locals1 = new JsonSerializeConditionAttribute.<>c__DisplayClass2_0();
				JsonSerializeConditionAttribute.<>c__DisplayClass2_0 CS$<>8__locals2 = CS$<>8__locals1;
				ICustomAttributeProvider attributeProvider = jsonPropertyInfo.AttributeProvider;
				CS$<>8__locals2.attr = ((attributeProvider != null) ? attributeProvider.GetCustomAttributes(true).OfType<JsonSerializeConditionAttribute>().FirstOrDefault<JsonSerializeConditionAttribute>() : null);
				if (CS$<>8__locals1.attr != null)
				{
					ICustomAttributeProvider attributeProvider2 = jsonPropertyInfo.AttributeProvider;
					CS$<>8__locals1.memberInfo = attributeProvider2 as MemberInfo;
					if (CS$<>8__locals1.memberInfo != null)
					{
						jsonPropertyInfo.ShouldSerialize = (object _, [Nullable(2)] object c) => CS$<>8__locals1.attr.defaultBehaviour.ShouldSerialize(c, CS$<>8__locals1.memberInfo);
					}
				}
			}
		}

		// Token: 0x0400031F RID: 799
		public readonly SerializationCondition defaultBehaviour;
	}
}
