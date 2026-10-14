using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	/// <summary>
	/// Used in the ModelIdSerializationCache to sort models before IDing them.
	/// When mods are installed, the Sort function ensures that the types returned are sorted in a deterministic order
	/// across all peers. It guards against several issues:
	///  - Mods can be loaded in different sort orders
	///  - Different mods can have content with the same ID
	///  - The same mod can have content with the same ID
	/// When in the base game, this is mostly for theoretical safety; ModelDb is likely to return models in the same order.
	///
	/// This class is generic so it can be used across several places.
	/// </summary>
	// Token: 0x02000447 RID: 1095
	[NullableContext(1)]
	[Nullable(0)]
	public static class ContentSorter<[Nullable(0)] TIdType> where TIdType : IComparable<TIdType>
	{
		/// <summary>
		/// Sort some content.
		/// </summary>
		/// <param name="types">The types to sort.</param>
		/// <param name="getId">The delegate which will be used to obtain the ID of each type.</param>
		/// <param name="affectsGameplayAtEnd">If true, types from mods that do not affect gameplay will be placed at the
		/// end of the resulting list. This is important so that players without those mods will still have a stable and
		/// deterministic sorting of IDs that will be sent over the network.</param>
		/// <returns></returns>
		// Token: 0x060044D6 RID: 17622 RVA: 0x00213678 File Offset: 0x00211878
		[return: Nullable(new byte[] { 1, 0, 0 })]
		public static List<ContentSorter<TIdType>.Item> Sort(IEnumerable<Type> types, Func<Type, TIdType> getId, bool affectsGameplayAtEnd = true)
		{
			if (AssemblyInfo.ModMap == null)
			{
				throw new InvalidOperationException("ContentSorter called before AssemblyInfo was initialized. This is not allowed.");
			}
			List<ContentSorter<TIdType>.Item> list = new List<ContentSorter<TIdType>.Item>();
			foreach (Type type in types)
			{
				bool flag;
				Mod mod = AssemblyInfo.ModForType(type, out flag);
				if (mod == null && !flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(210, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Attempting to register type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
					defaultInterpolatedStringHandler.AppendLiteral(" in assembly ");
					defaultInterpolatedStringHandler.AppendFormatted<Assembly>(type.Assembly);
					defaultInterpolatedStringHandler.AppendLiteral(", but it is not associated with any mod! You may need to call ");
					defaultInterpolatedStringHandler.AppendFormatted("ModManager");
					defaultInterpolatedStringHandler.AppendLiteral(".");
					defaultInterpolatedStringHandler.AppendFormatted("AssociateAssemblyWithMod");
					defaultInterpolatedStringHandler.AppendLiteral(" to manually register the assembly. Type sorting may break because of this, causing errors in multiplayer.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				list.Add(new ContentSorter<TIdType>.Item
				{
					type = type,
					id = getId(type),
					mod = mod
				});
			}
			ContentSorter<TIdType>.Sort(list, affectsGameplayAtEnd);
			return list;
		}

		/// <summary>
		/// This is public for testing.
		/// </summary>
		// Token: 0x060044D7 RID: 17623 RVA: 0x002137A8 File Offset: 0x002119A8
		[return: Nullable(new byte[] { 1, 0, 0 })]
		public static List<ContentSorter<TIdType>.Item> Sort([Nullable(new byte[] { 1, 0, 0 })] List<ContentSorter<TIdType>.Item> items, bool affectsGameplayAtEnd = true)
		{
			items.Sort(delegate(ContentSorter<TIdType>.Item p1, ContentSorter<TIdType>.Item p2)
			{
				if (affectsGameplayAtEnd)
				{
					Mod mod = p1.mod;
					bool? flag;
					if (mod == null)
					{
						flag = null;
					}
					else
					{
						ModManifest manifest = mod.manifest;
						flag = ((manifest != null) ? new bool?(manifest.affectsGameplay) : null);
					}
					bool? flag2 = flag;
					bool valueOrDefault = flag2.GetValueOrDefault(true);
					Mod mod2 = p2.mod;
					bool? flag3;
					if (mod2 == null)
					{
						flag3 = null;
					}
					else
					{
						ModManifest manifest2 = mod2.manifest;
						flag3 = ((manifest2 != null) ? new bool?(manifest2.affectsGameplay) : null);
					}
					flag2 = flag3;
					int num = flag2.GetValueOrDefault(true).CompareTo(valueOrDefault);
					if (num != 0)
					{
						return num;
					}
				}
				int num2 = p1.id.CompareTo(p2.id);
				if (num2 != 0)
				{
					return num2;
				}
				if (p1.mod != null && p2.mod == null)
				{
					return 1;
				}
				if (p1.mod == null && p2.mod != null)
				{
					return -1;
				}
				if (p1.mod == null && p2.mod == null)
				{
					return 0;
				}
				int num3 = string.CompareOrdinal(p1.mod.manifest.id, p2.mod.manifest.id);
				if (num3 != 0)
				{
					return num3;
				}
				int num4 = string.CompareOrdinal(p1.type.FullName, p2.type.FullName);
				if (num4 != 0)
				{
					return num4;
				}
				string fullName = p1.type.Assembly.FullName;
				string fullName2 = p2.type.Assembly.FullName;
				int num5 = string.CompareOrdinal(fullName, fullName2);
				if (num5 != 0)
				{
					return num5;
				}
				return 0;
			});
			return items;
		}

		// Token: 0x020019A3 RID: 6563
		[Nullable(0)]
		public struct Item
		{
			// Token: 0x0600AB37 RID: 43831 RVA: 0x0036024C File Offset: 0x0035E44C
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(this.type);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<TIdType>(this.id);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				Mod mod = this.mod;
				string text;
				if (mod == null)
				{
					text = null;
				}
				else
				{
					ModManifest manifest = mod.manifest;
					text = ((manifest != null) ? manifest.id : null);
				}
				defaultInterpolatedStringHandler.AppendFormatted(text);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}

			// Token: 0x0400653C RID: 25916
			public Type type;

			// Token: 0x0400653D RID: 25917
			public TIdType id;

			// Token: 0x0400653E RID: 25918
			[Nullable(2)]
			public Mod mod;
		}
	}
}
