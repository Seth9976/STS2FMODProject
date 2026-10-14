using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	/// <summary>
	/// Bidirectional map of types to unique integer IDs, for use in serialization.
	/// </summary>
	/// <typeparam name="TBase">All classes which implement this type will automatically be mapped by this class.</typeparam>
	// Token: 0x0200044D RID: 1101
	[NullableContext(1)]
	[Nullable(0)]
	public class NetTypeCache<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TBase> where TBase : class
	{
		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06004506 RID: 17670 RVA: 0x002147DD File Offset: 0x002129DD
		public int Count
		{
			get
			{
				return this._idToType.Count;
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x002147EC File Offset: 0x002129EC
		public NetTypeCache(List<Type> types)
		{
			IEnumerable<ContentSorter<string>.Item> enumerable = ContentSorter<string>.Sort(types, (Type t) => t.Name, true);
			this._idToType = enumerable.Select<ContentSorter<string>.Item, Type>((ContentSorter<string>.Item i) => i.type).ToList<Type>();
			for (int j = 0; j < this._idToType.Count; j++)
			{
				Type type = this._idToType[j];
				if (!type.GetInterfaces().Contains(typeof(TBase)))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(this._idToType[j]);
					defaultInterpolatedStringHandler.AppendLiteral(" does not implement interface ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(TBase));
					defaultInterpolatedStringHandler.AppendLiteral("!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this._typeToId[type] = j;
			}
		}

		/// <summary>
		/// Obtain the ID for a given type.
		/// </summary>
		/// <typeparam name="T">The type to map.</typeparam>
		/// <returns>The integer ID for the type T.</returns>
		/// <exception cref="T:System.InvalidOperationException">Thrown if T does not implement TBase.</exception>
		// Token: 0x06004508 RID: 17672 RVA: 0x0021490D File Offset: 0x00212B0D
		[NullableContext(0)]
		public int TypeToId<T>() where T : TBase
		{
			return this.TypeToId(typeof(T));
		}

		/// <summary>
		/// Obtain the ID for a given type.
		/// </summary>
		/// <param name="type">The type to map.</param>
		/// <returns>The integer ID for the given type.</returns>
		/// <exception cref="T:System.InvalidOperationException">Thrown if type does not implement TBase.</exception>
		// Token: 0x06004509 RID: 17673 RVA: 0x0021491F File Offset: 0x00212B1F
		public int TypeToId(Type type)
		{
			return this._typeToId[type];
		}

		/// <summary>
		/// Obtain the type for a given integer ID.
		/// </summary>
		/// <param name="id">The ID to map to a type.</param>
		/// <param name="type">The type for the given ID.</param>
		/// <returns>True if a type was found for the ID, false otherwise.</returns>
		// Token: 0x0600450A RID: 17674 RVA: 0x0021492D File Offset: 0x00212B2D
		[NullableContext(2)]
		public bool TryGetTypeFromId(int id, out Type type)
		{
			if (id < 0 || id >= this._idToType.Count)
			{
				type = null;
				return false;
			}
			type = this._idToType[id];
			return true;
		}

		// Token: 0x04001AD4 RID: 6868
		private readonly Dictionary<Type, int> _typeToId = new Dictionary<Type, int>();

		// Token: 0x04001AD5 RID: 6869
		private readonly List<Type> _idToType;
	}
}
