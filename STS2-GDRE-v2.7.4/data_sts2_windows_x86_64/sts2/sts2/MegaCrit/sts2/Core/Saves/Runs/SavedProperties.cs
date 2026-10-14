using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class SavedProperties : IPacketSerializable
	{
		/// <summary>
		/// For manual instantiation at the start of a run or in tests.
		/// </summary>
		/// <param name="model">Abstract model to grab saved properties from.</param>
		// Token: 0x06000930 RID: 2352 RVA: 0x000428AD File Offset: 0x00040AAD
		[return: Nullable(2)]
		public static SavedProperties From(AbstractModel model)
		{
			return SavedProperties.FromInternal(model, model.Id);
		}

		/// <summary>
		/// This should only be called internally or from tests.
		/// </summary>
		/// <param name="model">The object to serialize properties from.</param>
		/// <param name="id">Id of the AbstractModel passed, or null if it is not an abstract model.</param>
		// Token: 0x06000931 RID: 2353 RVA: 0x000428BC File Offset: 0x00040ABC
		[NullableContext(2)]
		public static SavedProperties FromInternal([Nullable(1)] object model, ModelId id)
		{
			SavedProperties savedProperties = new SavedProperties();
			foreach (PropertyInfo propertyInfo in (ModelIdSerializationCache.GetJsonPropertiesForType(model.GetType()) ?? new List<PropertyInfo>()))
			{
				string name = propertyInfo.Name;
				object value = propertyInfo.GetValue(model);
				SavedPropertyAttribute customAttribute = propertyInfo.GetCustomAttribute<SavedPropertyAttribute>();
				if (customAttribute.defaultBehaviour.ShouldSerialize(value, propertyInfo))
				{
					if (value == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Property ");
						defaultInterpolatedStringHandler.AppendFormatted(name);
						defaultInterpolatedStringHandler.AppendLiteral(" on ");
						defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
						defaultInterpolatedStringHandler.AppendLiteral(" is null, which is not a valid SavedProperty");
						Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
					else if (value is int)
					{
						int num = (int)value;
						SavedProperties savedProperties2 = savedProperties;
						if (savedProperties2.ints == null)
						{
							savedProperties2.ints = new List<SavedProperties.SavedProperty<int>>();
						}
						savedProperties.ints.Add(new SavedProperties.SavedProperty<int>(name, num));
					}
					else
					{
						int[] array = value as int[];
						if (array == null)
						{
							Enum @enum = value as Enum;
							if (@enum == null)
							{
								Enum[] array2 = value as Enum[];
								if (array2 == null)
								{
									ModelId modelId = value as ModelId;
									if (modelId == null)
									{
										if (value is bool)
										{
											bool flag = (bool)value;
											SavedProperties savedProperties2 = savedProperties;
											if (savedProperties2.bools == null)
											{
												savedProperties2.bools = new List<SavedProperties.SavedProperty<bool>>();
											}
											savedProperties.bools.Add(new SavedProperties.SavedProperty<bool>(name, flag));
										}
										else
										{
											string text = value as string;
											if (text == null)
											{
												SerializableCard serializableCard = value as SerializableCard;
												if (serializableCard == null)
												{
													List<SerializableCard> list = value as List<SerializableCard>;
													if (list == null)
													{
														DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 3);
														defaultInterpolatedStringHandler.AppendLiteral("Property ");
														defaultInterpolatedStringHandler.AppendFormatted(name);
														defaultInterpolatedStringHandler.AppendLiteral(" on ");
														defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
														defaultInterpolatedStringHandler.AppendLiteral(" is not a valid type for [SavedProperty] (type ");
														defaultInterpolatedStringHandler.AppendFormatted<Type>(value.GetType());
														defaultInterpolatedStringHandler.AppendLiteral(").");
														throw new JsonException(defaultInterpolatedStringHandler.ToStringAndClear());
													}
													SavedProperties savedProperties2 = savedProperties;
													if (savedProperties2.cardArrays == null)
													{
														savedProperties2.cardArrays = new List<SavedProperties.SavedProperty<SerializableCard[]>>();
													}
													savedProperties.cardArrays.Add(new SavedProperties.SavedProperty<SerializableCard[]>(name, list.ToArray()));
												}
												else
												{
													SavedProperties savedProperties2 = savedProperties;
													if (savedProperties2.cards == null)
													{
														savedProperties2.cards = new List<SavedProperties.SavedProperty<SerializableCard>>();
													}
													savedProperties.cards.Add(new SavedProperties.SavedProperty<SerializableCard>(name, serializableCard));
												}
											}
											else
											{
												SavedProperties savedProperties2 = savedProperties;
												if (savedProperties2.strings == null)
												{
													savedProperties2.strings = new List<SavedProperties.SavedProperty<string>>();
												}
												savedProperties.strings.Add(new SavedProperties.SavedProperty<string>(name, text));
											}
										}
									}
									else
									{
										SavedProperties savedProperties2 = savedProperties;
										if (savedProperties2.modelIds == null)
										{
											savedProperties2.modelIds = new List<SavedProperties.SavedProperty<ModelId>>();
										}
										savedProperties.modelIds.Add(new SavedProperties.SavedProperty<ModelId>(name, modelId));
									}
								}
								else
								{
									SavedProperties savedProperties2 = savedProperties;
									if (savedProperties2.intArrays == null)
									{
										savedProperties2.intArrays = new List<SavedProperties.SavedProperty<int[]>>();
									}
									List<SavedProperties.SavedProperty<int[]>> list2 = savedProperties.intArrays;
									string text2 = name;
									IEnumerable<Enum> enumerable = array2;
									Func<Enum, int> func;
									if ((func = SavedProperties.<>O.<0>__ToInt32) == null)
									{
										func = (SavedProperties.<>O.<0>__ToInt32 = new Func<Enum, int>(Convert.ToInt32));
									}
									list2.Add(new SavedProperties.SavedProperty<int[]>(text2, enumerable.Select<Enum, int>(func).ToArray<int>()));
								}
							}
							else
							{
								SavedProperties savedProperties2 = savedProperties;
								if (savedProperties2.ints == null)
								{
									savedProperties2.ints = new List<SavedProperties.SavedProperty<int>>();
								}
								savedProperties.ints.Add(new SavedProperties.SavedProperty<int>(name, Convert.ToInt32(@enum)));
							}
						}
						else
						{
							SavedProperties savedProperties2 = savedProperties;
							if (savedProperties2.intArrays == null)
							{
								savedProperties2.intArrays = new List<SavedProperties.SavedProperty<int[]>>();
							}
							savedProperties.intArrays.Add(new SavedProperties.SavedProperty<int[]>(name, array));
						}
					}
				}
			}
			if (!savedProperties.Any())
			{
				return null;
			}
			return savedProperties;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00042CA0 File Offset: 0x00040EA0
		private bool Any()
		{
			if (this.ints != null)
			{
				List<SavedProperties.SavedProperty<int>> list = this.ints;
				if (list == null || list.Count != 0)
				{
					return true;
				}
			}
			if (this.bools != null)
			{
				List<SavedProperties.SavedProperty<bool>> list2 = this.bools;
				if (list2 == null || list2.Count != 0)
				{
					return true;
				}
			}
			if (this.intArrays != null)
			{
				List<SavedProperties.SavedProperty<int[]>> list3 = this.intArrays;
				if (list3 == null || list3.Count != 0)
				{
					return true;
				}
			}
			if (this.strings != null)
			{
				List<SavedProperties.SavedProperty<string>> list4 = this.strings;
				if (list4 == null || list4.Count != 0)
				{
					return true;
				}
			}
			if (this.modelIds != null)
			{
				List<SavedProperties.SavedProperty<ModelId>> list5 = this.modelIds;
				if (list5 == null || list5.Count != 0)
				{
					return true;
				}
			}
			if (this.cards != null)
			{
				List<SavedProperties.SavedProperty<SerializableCard>> list6 = this.cards;
				if (list6 == null || list6.Count != 0)
				{
					return true;
				}
			}
			if (this.cardArrays != null)
			{
				List<SavedProperties.SavedProperty<SerializableCard[]>> list7 = this.cardArrays;
				return list7 == null || list7.Count != 0;
			}
			return false;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00042D8D File Offset: 0x00040F8D
		public void Fill(AbstractModel model)
		{
			this.FillInternal(model);
		}

		/// <summary>
		/// Only for use internally or in tests!
		/// </summary>
		/// <param name="model">Object to dump properties into.</param>
		// Token: 0x06000934 RID: 2356 RVA: 0x00042D98 File Offset: 0x00040F98
		[UnconditionalSuppressMessage("AOT", "IL3050", Justification = "We only create array types that are referenced by SavedProperties that already exist in code.")]
		public void FillInternal(object model)
		{
			Type type = model.GetType();
			if (this.ints != null)
			{
				foreach (SavedProperties.SavedProperty<int> savedProperty in this.ints)
				{
					PropertyInfo property = type.GetProperty(savedProperty.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property != null)
					{
						property.SetValue(model, property.PropertyType.IsEnum ? Enum.ToObject(property.PropertyType, savedProperty.value) : savedProperty.value);
					}
				}
			}
			if (this.intArrays != null)
			{
				foreach (SavedProperties.SavedProperty<int[]> savedProperty2 in this.intArrays)
				{
					PropertyInfo property2 = type.GetProperty(savedProperty2.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (!(property2 == null))
					{
						Type elementType = property2.PropertyType.GetElementType();
						if (elementType.IsEnum)
						{
							Array array = Array.CreateInstance(elementType, savedProperty2.value.Length);
							for (int i = 0; i < savedProperty2.value.Length; i++)
							{
								array.SetValue(Enum.ToObject(elementType, savedProperty2.value[i]), i);
							}
							property2.SetValue(model, array);
						}
						else
						{
							property2.SetValue(model, savedProperty2.value);
						}
					}
				}
			}
			if (this.bools != null)
			{
				foreach (SavedProperties.SavedProperty<bool> savedProperty3 in this.bools)
				{
					PropertyInfo property3 = type.GetProperty(savedProperty3.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property3 != null)
					{
						property3.SetValue(model, savedProperty3.value);
					}
				}
			}
			if (this.modelIds != null)
			{
				foreach (SavedProperties.SavedProperty<ModelId> savedProperty4 in this.modelIds)
				{
					PropertyInfo property4 = type.GetProperty(savedProperty4.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property4 != null)
					{
						property4.SetValue(model, savedProperty4.value);
					}
				}
			}
			if (this.cards != null)
			{
				foreach (SavedProperties.SavedProperty<SerializableCard> savedProperty5 in this.cards)
				{
					PropertyInfo property5 = type.GetProperty(savedProperty5.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property5 != null)
					{
						property5.SetValue(model, savedProperty5.value);
					}
				}
			}
			if (this.cardArrays != null)
			{
				foreach (SavedProperties.SavedProperty<SerializableCard[]> savedProperty6 in this.cardArrays)
				{
					PropertyInfo property6 = type.GetProperty(savedProperty6.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property6 != null)
					{
						property6.SetValue(model, savedProperty6.value.ToList<SerializableCard>());
					}
				}
			}
			if (this.strings != null)
			{
				foreach (SavedProperties.SavedProperty<string> savedProperty7 in this.strings)
				{
					PropertyInfo property7 = type.GetProperty(savedProperty7.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property7 != null)
					{
						property7.SetValue(model, savedProperty7.value);
					}
				}
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00043124 File Offset: 0x00041324
		private static void WritePropertyName(PacketWriter writer, string propertyName)
		{
			writer.WriteInt(ModelIdSerializationCache.GetNetIdForPropertyName(propertyName), ModelIdSerializationCache.PropertyIdBitSize);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00043138 File Offset: 0x00041338
		private static string ReadPropertyName(PacketReader reader)
		{
			int num = reader.ReadInt(ModelIdSerializationCache.PropertyIdBitSize);
			return ModelIdSerializationCache.GetPropertyNameForNetId(num);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00043158 File Offset: 0x00041358
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.ints != null);
			if (this.ints != null)
			{
				writer.WriteInt(this.ints.Count, 8);
				foreach (SavedProperties.SavedProperty<int> savedProperty in this.ints)
				{
					SavedProperties.WritePropertyName(writer, savedProperty.name);
					writer.WriteInt(savedProperty.value, 32);
				}
			}
			writer.WriteBool(this.intArrays != null);
			if (this.intArrays != null)
			{
				writer.WriteInt(this.intArrays.Count, 8);
				foreach (SavedProperties.SavedProperty<int[]> savedProperty2 in this.intArrays)
				{
					SavedProperties.WritePropertyName(writer, savedProperty2.name);
					writer.WriteInt(savedProperty2.value.Length, 32);
					foreach (int num in savedProperty2.value)
					{
						writer.WriteInt(num, 32);
					}
				}
			}
			writer.WriteBool(this.bools != null);
			if (this.bools != null)
			{
				writer.WriteInt(this.bools.Count, 8);
				foreach (SavedProperties.SavedProperty<bool> savedProperty3 in this.bools)
				{
					SavedProperties.WritePropertyName(writer, savedProperty3.name);
					writer.WriteBool(savedProperty3.value);
				}
			}
			writer.WriteBool(this.modelIds != null);
			if (this.modelIds != null)
			{
				writer.WriteInt(this.modelIds.Count, 8);
				foreach (SavedProperties.SavedProperty<ModelId> savedProperty4 in this.modelIds)
				{
					SavedProperties.WritePropertyName(writer, savedProperty4.name);
					writer.WriteFullModelId(savedProperty4.value);
				}
			}
			writer.WriteBool(this.cards != null);
			if (this.cards != null)
			{
				writer.WriteInt(this.cards.Count, 8);
				foreach (SavedProperties.SavedProperty<SerializableCard> savedProperty5 in this.cards)
				{
					SavedProperties.WritePropertyName(writer, savedProperty5.name);
					writer.Write<SerializableCard>(savedProperty5.value);
				}
			}
			writer.WriteBool(this.cardArrays != null);
			if (this.cardArrays != null)
			{
				writer.WriteInt(this.cardArrays.Count, 8);
				foreach (SavedProperties.SavedProperty<SerializableCard[]> savedProperty6 in this.cardArrays)
				{
					SavedProperties.WritePropertyName(writer, savedProperty6.name);
					writer.WriteInt(savedProperty6.value.Length, 32);
					foreach (SerializableCard serializableCard in savedProperty6.value)
					{
						writer.Write<SerializableCard>(serializableCard);
					}
				}
			}
			writer.WriteBool(this.strings != null);
			if (this.strings != null)
			{
				writer.WriteInt(this.strings.Count, 8);
				foreach (SavedProperties.SavedProperty<string> savedProperty7 in this.strings)
				{
					SavedProperties.WritePropertyName(writer, savedProperty7.name);
					writer.WriteString(savedProperty7.value);
				}
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00043548 File Offset: 0x00041748
		public void Deserialize(PacketReader reader)
		{
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.ints = new List<SavedProperties.SavedProperty<int>>();
				int num = reader.ReadInt(8);
				for (int i = 0; i < num; i++)
				{
					this.ints.Add(new SavedProperties.SavedProperty<int>(SavedProperties.ReadPropertyName(reader), reader.ReadInt(32)));
				}
			}
			bool flag2 = reader.ReadBool();
			if (flag2)
			{
				this.intArrays = new List<SavedProperties.SavedProperty<int[]>>();
				int num2 = reader.ReadInt(8);
				for (int j = 0; j < num2; j++)
				{
					string text = SavedProperties.ReadPropertyName(reader);
					int num3 = reader.ReadInt(32);
					int[] array = new int[num3];
					for (int k = 0; k < num3; k++)
					{
						array[k] = reader.ReadInt(32);
					}
					this.intArrays.Add(new SavedProperties.SavedProperty<int[]>(text, array));
				}
			}
			bool flag3 = reader.ReadBool();
			if (flag3)
			{
				this.bools = new List<SavedProperties.SavedProperty<bool>>();
				int num4 = reader.ReadInt(8);
				for (int l = 0; l < num4; l++)
				{
					this.bools.Add(new SavedProperties.SavedProperty<bool>(SavedProperties.ReadPropertyName(reader), reader.ReadBool()));
				}
			}
			bool flag4 = reader.ReadBool();
			if (flag4)
			{
				this.modelIds = new List<SavedProperties.SavedProperty<ModelId>>();
				int num5 = reader.ReadInt(8);
				for (int m = 0; m < num5; m++)
				{
					this.modelIds.Add(new SavedProperties.SavedProperty<ModelId>(SavedProperties.ReadPropertyName(reader), reader.ReadFullModelId()));
				}
			}
			bool flag5 = reader.ReadBool();
			if (flag5)
			{
				this.cards = new List<SavedProperties.SavedProperty<SerializableCard>>();
				int num6 = reader.ReadInt(8);
				for (int n = 0; n < num6; n++)
				{
					this.cards.Add(new SavedProperties.SavedProperty<SerializableCard>(SavedProperties.ReadPropertyName(reader), reader.Read<SerializableCard>()));
				}
			}
			bool flag6 = reader.ReadBool();
			if (flag6)
			{
				this.cardArrays = new List<SavedProperties.SavedProperty<SerializableCard[]>>();
				int num7 = reader.ReadInt(8);
				for (int num8 = 0; num8 < num7; num8++)
				{
					string text2 = SavedProperties.ReadPropertyName(reader);
					int num9 = reader.ReadInt(32);
					SerializableCard[] array2 = new SerializableCard[num9];
					for (int num10 = 0; num10 < num9; num10++)
					{
						array2[num10] = reader.Read<SerializableCard>();
					}
					this.cardArrays.Add(new SavedProperties.SavedProperty<SerializableCard[]>(text2, array2));
				}
			}
			bool flag7 = reader.ReadBool();
			if (flag7)
			{
				this.strings = new List<SavedProperties.SavedProperty<string>>();
				int num11 = reader.ReadInt(8);
				for (int num12 = 0; num12 < num11; num12++)
				{
					this.strings.Add(new SavedProperties.SavedProperty<string>(SavedProperties.ReadPropertyName(reader), reader.ReadString()));
				}
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x000437CC File Offset: 0x000419CC
		public override string ToString()
		{
			if (!this.Any())
			{
				return "<none>";
			}
			List<string> list = new List<string>();
			if (this.ints != null)
			{
				foreach (SavedProperties.SavedProperty<int> savedProperty in this.ints)
				{
					List<string> list2 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(savedProperty.name);
					defaultInterpolatedStringHandler.AppendLiteral("=");
					defaultInterpolatedStringHandler.AppendFormatted<int>(savedProperty.value);
					list2.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (this.bools != null)
			{
				foreach (SavedProperties.SavedProperty<bool> savedProperty2 in this.bools)
				{
					List<string> list3 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(savedProperty2.name);
					defaultInterpolatedStringHandler.AppendLiteral("=");
					defaultInterpolatedStringHandler.AppendFormatted<bool>(savedProperty2.value);
					list3.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (this.strings != null)
			{
				foreach (SavedProperties.SavedProperty<string> savedProperty3 in this.strings)
				{
					list.Add(savedProperty3.name + "=" + savedProperty3.value);
				}
			}
			if (this.intArrays != null)
			{
				foreach (SavedProperties.SavedProperty<int[]> savedProperty4 in this.intArrays)
				{
					list.Add(savedProperty4.name + "=" + string.Join<int>(",", savedProperty4.value));
				}
			}
			if (this.modelIds != null)
			{
				foreach (SavedProperties.SavedProperty<ModelId> savedProperty5 in this.modelIds)
				{
					List<string> list4 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(savedProperty5.name);
					defaultInterpolatedStringHandler.AppendLiteral("=");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(savedProperty5.value);
					list4.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (this.cards != null)
			{
				foreach (SavedProperties.SavedProperty<SerializableCard> savedProperty6 in this.cards)
				{
					List<string> list5 = list;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(savedProperty6.name);
					defaultInterpolatedStringHandler.AppendLiteral("=");
					defaultInterpolatedStringHandler.AppendFormatted<SerializableCard>(savedProperty6.value);
					list5.Add(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (this.cardArrays != null)
			{
				foreach (SavedProperties.SavedProperty<SerializableCard[]> savedProperty7 in this.cardArrays)
				{
					list.Add(savedProperty7.name + "=" + string.Join(",", savedProperty7.value.Select<SerializableCard, string>((SerializableCard c) => c.Id.Entry)));
				}
			}
			return string.Join(",", list);
		}

		// Token: 0x04000320 RID: 800
		[Nullable(new byte[] { 2, 0 })]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonPropertyName("ints")]
		public List<SavedProperties.SavedProperty<int>> ints;

		// Token: 0x04000321 RID: 801
		[Nullable(new byte[] { 2, 0 })]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonPropertyName("bools")]
		public List<SavedProperties.SavedProperty<bool>> bools;

		// Token: 0x04000322 RID: 802
		[Nullable(new byte[] { 2, 0, 1 })]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonPropertyName("strings")]
		public List<SavedProperties.SavedProperty<string>> strings;

		// Token: 0x04000323 RID: 803
		[Nullable(new byte[] { 2, 0, 1 })]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonPropertyName("int_arrays")]
		public List<SavedProperties.SavedProperty<int[]>> intArrays;

		// Token: 0x04000324 RID: 804
		[Nullable(new byte[] { 2, 0, 1 })]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonPropertyName("model_ids")]
		public List<SavedProperties.SavedProperty<ModelId>> modelIds;

		// Token: 0x04000325 RID: 805
		[Nullable(new byte[] { 2, 0, 1 })]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonPropertyName("cards")]
		public List<SavedProperties.SavedProperty<SerializableCard>> cards;

		// Token: 0x04000326 RID: 806
		[Nullable(new byte[] { 2, 0, 1, 1 })]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonPropertyName("card_arrays")]
		public List<SavedProperties.SavedProperty<SerializableCard[]>> cardArrays;

		// Token: 0x02000E97 RID: 3735
		[Nullable(0)]
		public struct SavedProperty<[Nullable(2)] T>
		{
			// Token: 0x060094E6 RID: 38118 RVA: 0x002FC8A2 File Offset: 0x002FAAA2
			public SavedProperty(string name, T value)
			{
				this.name = name;
				this.value = value;
			}

			// Token: 0x04003986 RID: 14726
			public string name;

			// Token: 0x04003987 RID: 14727
			public T value;
		}

		// Token: 0x02000E98 RID: 3736
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003988 RID: 14728
			[Nullable(0)]
			public static Func<Enum, int> <0>__ToInt32;
		}
	}
}
