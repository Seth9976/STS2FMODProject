using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves.Migrations
{
	/// <summary>
	/// A mutable JSON representation of the data being migrated. It serves as a loosely typed mutable blob we can
	/// deserialize into and modify with our migration classes until finally parsing into the latest structured save schema.
	/// This implementation uses System.Text.Json's JsonNode instead of a custom implementation.
	/// </summary>
	// Token: 0x020000D8 RID: 216
	[NullableContext(1)]
	[Nullable(0)]
	public class MigratingData
	{
		/// <summary>
		/// Creates a JSON object from a JsonDocument.
		/// </summary>
		/// <param name="document">The JsonDocument to convert</param>
		// Token: 0x06000A3F RID: 2623 RVA: 0x00045B3A File Offset: 0x00043D3A
		public MigratingData(JsonDocument document)
		{
			this._data = document.Deserialize<JsonObject>(JsonSerializationUtility.GetTypeInfo<JsonObject>());
		}

		/// <summary>
		/// Creates a JSON object from a JSON string.
		/// </summary>
		/// <param name="json">The JSON string</param>
		// Token: 0x06000A40 RID: 2624 RVA: 0x00045B54 File Offset: 0x00043D54
		public MigratingData(string json)
		{
			this._data = (JsonObject)JsonNode.Parse(json, null, default(JsonDocumentOptions));
		}

		/// <summary>
		/// Creates a JSON object from a JsonObject.
		/// </summary>
		/// <param name="jsonObject">The JsonObject</param>
		// Token: 0x06000A41 RID: 2625 RVA: 0x00045B8A File Offset: 0x00043D8A
		public MigratingData(JsonObject jsonObject)
		{
			this._data = jsonObject;
		}

		/// <summary>
		/// Parses this JsonObject to a strongly typed object.
		/// </summary>
		/// <typeparam name="T">The type to convert to</typeparam>
		/// <returns>An instance of the specified type</returns>
		// Token: 0x06000A42 RID: 2626 RVA: 0x00045B9C File Offset: 0x00043D9C
		public T ToObject<[Nullable(2)] T>() where T : new()
		{
			string text = this._data.ToJsonString(null);
			T t = JsonSerializer.Deserialize<T>(text, JsonSerializationUtility.GetTypeInfo<T>());
			if (t == null)
			{
				return new T();
			}
			return t;
		}

		/// <summary>
		/// Gets a property in the JSON object.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>The property value</returns>
		// Token: 0x170003BF RID: 959
		[Nullable(2)]
		public object this[string key]
		{
			[return: Nullable(2)]
			get
			{
				JsonNode jsonNode;
				if (!this._data.TryGetPropertyValue(key, out jsonNode))
				{
					return null;
				}
				return MigratingData.ConvertJsonNodeToObject(jsonNode);
			}
		}

		/// <summary>
		/// Removes a property from the JSON object.
		/// </summary>
		/// <param name="key">The property name to remove</param>
		// Token: 0x06000A44 RID: 2628 RVA: 0x00045BF9 File Offset: 0x00043DF9
		public void Remove(string key)
		{
			if (this._data.ContainsKey(key))
			{
				this._data.Remove(key);
			}
		}

		/// <summary>
		/// Renames a property in the JSON object.
		/// </summary>
		/// <param name="oldKey">The current property name</param>
		/// <param name="newKey">The new property name</param>
		// Token: 0x06000A45 RID: 2629 RVA: 0x00045C18 File Offset: 0x00043E18
		public void Rename(string oldKey, string newKey)
		{
			JsonNode jsonNode;
			if (this._data.TryGetPropertyValue(oldKey, out jsonNode))
			{
				this._data[newKey] = ((jsonNode != null) ? jsonNode.DeepClone() : null);
				this._data.Remove(oldKey);
				return;
			}
			throw new MigrationException("Cannot rename a key that doesn't exist. Key=" + oldKey);
		}

		/// <summary>
		/// Checks if the JSON object has a property.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>True if the property exists</returns>
		// Token: 0x06000A46 RID: 2630 RVA: 0x00045C6B File Offset: 0x00043E6B
		public bool Has(string key)
		{
			return this._data.ContainsKey(key);
		}

		/// <summary>
		/// Helper method to convert JsonNode to .NET objects
		/// </summary>
		// Token: 0x06000A47 RID: 2631 RVA: 0x00045C7C File Offset: 0x00043E7C
		[NullableContext(2)]
		private static object ConvertJsonNodeToObject(JsonNode node)
		{
			object obj;
			if (node != null)
			{
				if (!(node is JsonObject))
				{
					if (!(node is JsonArray))
					{
						JsonValue jsonValue = node as JsonValue;
						if (jsonValue == null)
						{
							obj = null;
						}
						else
						{
							JsonValue jsonValue2 = jsonValue;
							string text;
							if (jsonValue2.TryGetValue<string>(out text))
							{
								obj = text;
							}
							else
							{
								JsonValue jsonValue3 = jsonValue;
								int num;
								if (jsonValue3.TryGetValue<int>(out num))
								{
									obj = num;
								}
								else
								{
									JsonValue jsonValue4 = jsonValue;
									long num2;
									if (jsonValue4.TryGetValue<long>(out num2))
									{
										obj = num2;
									}
									else
									{
										JsonValue jsonValue5 = jsonValue;
										float num3;
										if (jsonValue5.TryGetValue<float>(out num3))
										{
											obj = num3;
										}
										else
										{
											JsonValue jsonValue6 = jsonValue;
											double num4;
											if (jsonValue6.TryGetValue<double>(out num4))
											{
												obj = num4;
											}
											else
											{
												JsonValue jsonValue7 = jsonValue;
												bool flag;
												if (jsonValue7.TryGetValue<bool>(out flag))
												{
													obj = flag;
												}
												else
												{
													JsonValue jsonValue8 = jsonValue;
													obj = jsonValue8.ToString();
												}
											}
										}
									}
								}
							}
						}
					}
					else
					{
						obj = node.Deserialize<List<JsonNode>>(JsonSerializationUtility.GetTypeInfo<List<JsonNode>>());
					}
				}
				else
				{
					obj = node.Deserialize<Dictionary<string, object>>(JsonSerializationUtility.GetTypeInfo<Dictionary<string, object>>());
				}
			}
			else
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00045D88 File Offset: 0x00043F88
		[NullableContext(0)]
		public T? GetAsOrNull<T>([Nullable(1)] string key) where T : struct
		{
			if (!this.Has(key))
			{
				return null;
			}
			return new T?(this.GetAs<T>(key));
		}

		/// <summary>
		/// Gets a property as a specific type.
		/// </summary>
		/// <typeparam name="T">The type to convert to</typeparam>
		/// <param name="key">The property name</param>
		/// <returns>The property value as the specified type</returns>
		// Token: 0x06000A49 RID: 2633 RVA: 0x00045DB4 File Offset: 0x00043FB4
		public T GetAs<[Nullable(2)] T>(string key)
		{
			JsonNode jsonNode;
			if (!this._data.TryGetPropertyValue(key, out jsonNode) || jsonNode == null)
			{
				throw new MigrationException("Cannot get value of key=" + key);
			}
			T t;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle == typeof(MigratingData))
				{
					JsonObject jsonObject = jsonNode as JsonObject;
					if (jsonObject != null)
					{
						return (T)((object)new MigratingData(jsonObject));
					}
				}
				if (typeFromHandle == typeof(List<MigratingData>))
				{
					JsonArray jsonArray = jsonNode as JsonArray;
					if (jsonArray != null)
					{
						List<MigratingData> list = new List<MigratingData>();
						foreach (JsonNode jsonNode2 in jsonArray)
						{
							JsonObject jsonObject2 = jsonNode2 as JsonObject;
							if (jsonObject2 == null)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
								defaultInterpolatedStringHandler.AppendLiteral("Cannot convert array item to MigratingData: ");
								defaultInterpolatedStringHandler.AppendFormatted<JsonNode>(jsonNode2);
								throw new MigrationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							list.Add(new MigratingData(jsonObject2));
						}
						return (T)((object)list);
					}
				}
				if (typeFromHandle == typeof(ModelId))
				{
					string value = jsonNode.GetValue<string>();
					if (string.IsNullOrEmpty(value))
					{
						t = (T)((object)ModelId.none);
					}
					else
					{
						t = (T)((object)ModelId.Deserialize(value));
					}
				}
				else if (typeFromHandle == typeof(string))
				{
					t = (T)((object)jsonNode.GetValue<string>());
				}
				else if (typeFromHandle == typeof(int))
				{
					t = (T)((object)jsonNode.GetValue<int>());
				}
				else if (typeFromHandle == typeof(long))
				{
					t = (T)((object)jsonNode.GetValue<long>());
				}
				else if (typeFromHandle == typeof(double))
				{
					t = (T)((object)jsonNode.GetValue<double>());
				}
				else if (typeFromHandle == typeof(float))
				{
					t = (T)((object)jsonNode.GetValue<float>());
				}
				else if (typeFromHandle == typeof(bool))
				{
					t = (T)((object)jsonNode.GetValue<bool>());
				}
				else if (typeFromHandle == typeof(DateTime))
				{
					t = (T)((object)jsonNode.GetValue<DateTime>());
				}
				else
				{
					T t2 = jsonNode.Deserialize<T>(JsonSerializationUtility.GetTypeInfo<T>());
					if (t2 == null)
					{
						throw new MigrationException("Unable to convert " + key + " to " + typeof(T).Name);
					}
					t = t2;
				}
			}
			catch (Exception ex) when (!(ex is MigrationException))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot convert value of key=");
				defaultInterpolatedStringHandler.AppendFormatted(key);
				defaultInterpolatedStringHandler.AppendLiteral(" to ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
				throw new MigrationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return t;
		}

		/// <summary>
		/// Gets a string value from the JSON object.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>The string value</returns>
		// Token: 0x06000A4A RID: 2634 RVA: 0x00046120 File Offset: 0x00044320
		public string GetString(string key)
		{
			return this.GetAs<string>(key);
		}

		/// <summary>
		/// Gets a boolean value from the JSON object.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>The boolean value</returns>
		// Token: 0x06000A4B RID: 2635 RVA: 0x00046129 File Offset: 0x00044329
		public bool GetBool(string key)
		{
			return this.GetAs<bool>(key);
		}

		/// <summary>
		/// Gets an integer value from the JSON object.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>The integer value</returns>
		// Token: 0x06000A4C RID: 2636 RVA: 0x00046132 File Offset: 0x00044332
		public int GetInt(string key)
		{
			return this.GetAs<int>(key);
		}

		/// <summary>
		/// Gets a nested JSON object from the JSON object.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>The nested JSON object</returns>
		// Token: 0x06000A4D RID: 2637 RVA: 0x0004613B File Offset: 0x0004433B
		public MigratingData GetObject(string key)
		{
			return this.GetAs<MigratingData>(key);
		}

		/// <summary>
		/// Sets a value in the JSON object.
		/// </summary>
		/// <param name="key">The key to set.</param>
		/// <param name="value">The value to set.</param>
		// Token: 0x06000A4E RID: 2638 RVA: 0x00046144 File Offset: 0x00044344
		public void Set<[Nullable(2)] T>(string key, T value)
		{
			List<MigratingData> list = value as List<MigratingData>;
			if (list != null)
			{
				this.SetList<MigratingData>(key, list);
			}
			if (value is MigratingData)
			{
				throw new NotImplementedException();
			}
			this._data[key] = ((value != null) ? JsonValue.Create<T>(value, JsonSerializationUtility.GetTypeInfo<T>(), null) : null);
		}

		/// <summary>
		/// Sets a nested object property in the JSON object.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <param name="value">The MigratingData object to set</param>
		// Token: 0x06000A4F RID: 2639 RVA: 0x000461A6 File Offset: 0x000443A6
		public void SetObject(string key, MigratingData value)
		{
			this._data[key] = value._data.DeepClone();
		}

		/// <summary>
		/// Gets a list of objects from the JSON array.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>The list of objects</returns>
		// Token: 0x06000A50 RID: 2640 RVA: 0x000461BF File Offset: 0x000443BF
		public List<MigratingData> GetList(string key)
		{
			return this.GetAs<List<MigratingData>>(key);
		}

		/// <summary>
		/// Gets the raw JsonNode for a property, useful for direct manipulation in migrations.
		/// </summary>
		/// <param name="key">The property name</param>
		/// <returns>The JsonNode or null if not found</returns>
		// Token: 0x06000A51 RID: 2641 RVA: 0x000461C8 File Offset: 0x000443C8
		[return: Nullable(2)]
		public JsonNode GetRawNode(string key)
		{
			JsonNode jsonNode;
			if (!this._data.TryGetPropertyValue(key, out jsonNode))
			{
				return null;
			}
			return jsonNode;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000461E8 File Offset: 0x000443E8
		public JsonObject GetRawNode()
		{
			return this._data;
		}

		/// <summary>
		/// Sets an array property in the JSON object.
		/// </summary>
		/// <typeparam name="T">The type of items in the array</typeparam>
		/// <param name="key">The property name</param>
		/// <param name="items">The collection of items to set</param>
		// Token: 0x06000A53 RID: 2643 RVA: 0x000461F0 File Offset: 0x000443F0
		public void SetList<[Nullable(2)] T>(string key, IEnumerable<T> items)
		{
			if (typeof(T) == typeof(MigratingData))
			{
				JsonArray jsonArray = new JsonArray(null);
				foreach (T t in items)
				{
					MigratingData migratingData = t as MigratingData;
					if (migratingData != null)
					{
						jsonArray.Add(migratingData._data.DeepClone());
					}
				}
				this._data[key] = jsonArray;
				return;
			}
			string text = JsonSerializer.Serialize(items, JsonSerializationUtility.GetTypeInfo<List<T>>());
			this._data[key] = JsonNode.Parse(text, null, default(JsonDocumentOptions));
		}

		// Token: 0x0400038C RID: 908
		private readonly JsonObject _data;
	}
}
