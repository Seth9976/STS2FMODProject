using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Localization
{
	// Token: 0x02000BC3 RID: 3011
	[NullableContext(1)]
	[Nullable(0)]
	public struct SerializableDynamicVar : IPacketSerializable
	{
		// Token: 0x06007D94 RID: 32148 RVA: 0x002AC218 File Offset: 0x002AA418
		public object ToDynamicVar(string name)
		{
			object obj;
			switch (this.type)
			{
			case DynamicVarType.BaseDynamic:
				obj = new DynamicVar(name, this.decimalValue);
				break;
			case DynamicVarType.DynamicString:
				obj = new StringVar(name, this.stringValue);
				break;
			case DynamicVarType.Decimal:
				obj = this.decimalValue;
				break;
			case DynamicVarType.String:
				obj = this.stringValue;
				break;
			case DynamicVarType.Bool:
				obj = this.boolValue;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to convert SerializableDynamicVar with invalid type ");
				defaultInterpolatedStringHandler.AppendFormatted<DynamicVarType>(this.type);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return obj;
		}

		// Token: 0x06007D95 RID: 32149 RVA: 0x002AC2C0 File Offset: 0x002AA4C0
		public static SerializableDynamicVar? FromDynamicVar(object var)
		{
			StringVar stringVar = var as StringVar;
			SerializableDynamicVar? serializableDynamicVar;
			if (stringVar == null)
			{
				DynamicVar dynamicVar = var as DynamicVar;
				if (dynamicVar == null)
				{
					string text = var as string;
					if (text == null)
					{
						if (var is decimal)
						{
							decimal num = (decimal)var;
							serializableDynamicVar = new SerializableDynamicVar?(new SerializableDynamicVar
							{
								type = DynamicVarType.Decimal,
								decimalValue = num
							});
						}
						else if (var is bool)
						{
							bool flag = (bool)var;
							serializableDynamicVar = new SerializableDynamicVar?(new SerializableDynamicVar
							{
								type = DynamicVarType.Bool,
								boolValue = flag
							});
						}
						else
						{
							serializableDynamicVar = null;
						}
					}
					else
					{
						serializableDynamicVar = new SerializableDynamicVar?(new SerializableDynamicVar
						{
							type = DynamicVarType.String,
							stringValue = text
						});
					}
				}
				else
				{
					serializableDynamicVar = new SerializableDynamicVar?(new SerializableDynamicVar
					{
						type = DynamicVarType.BaseDynamic,
						decimalValue = dynamicVar.BaseValue
					});
				}
			}
			else
			{
				serializableDynamicVar = new SerializableDynamicVar?(new SerializableDynamicVar
				{
					type = DynamicVarType.DynamicString,
					stringValue = stringVar.StringValue
				});
			}
			return serializableDynamicVar;
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x002AC3E4 File Offset: 0x002AA5E4
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<DynamicVarType>(this.type);
			switch (this.type)
			{
			case DynamicVarType.BaseDynamic:
			case DynamicVarType.Decimal:
				writer.WriteFloat((float)this.decimalValue, null);
				return;
			case DynamicVarType.DynamicString:
			case DynamicVarType.String:
				writer.WriteString(this.stringValue);
				return;
			case DynamicVarType.Bool:
				writer.WriteBool(this.boolValue);
				return;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to convert SerializableDynamicVar with invalid type ");
				defaultInterpolatedStringHandler.AppendFormatted<DynamicVarType>(this.type);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007D97 RID: 32151 RVA: 0x002AC488 File Offset: 0x002AA688
		public void Deserialize(PacketReader reader)
		{
			this.type = reader.ReadEnum<DynamicVarType>();
			switch (this.type)
			{
			case DynamicVarType.BaseDynamic:
			case DynamicVarType.Decimal:
				this.decimalValue = (decimal)reader.ReadFloat(null);
				return;
			case DynamicVarType.DynamicString:
			case DynamicVarType.String:
				this.stringValue = reader.ReadString();
				return;
			case DynamicVarType.Bool:
				this.boolValue = reader.ReadBool();
				return;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to convert SerializableDynamicVar with invalid type ");
				defaultInterpolatedStringHandler.AppendFormatted<DynamicVarType>(this.type);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x040029F5 RID: 10741
		[JsonPropertyName("type")]
		public DynamicVarType type;

		// Token: 0x040029F6 RID: 10742
		[JsonPropertyName("decimal_value")]
		public decimal decimalValue;

		// Token: 0x040029F7 RID: 10743
		[JsonPropertyName("bool_value")]
		public bool boolValue;

		// Token: 0x040029F8 RID: 10744
		[Nullable(2)]
		[JsonPropertyName("string_value")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotTypeDefault)]
		public string stringValue;
	}
}
