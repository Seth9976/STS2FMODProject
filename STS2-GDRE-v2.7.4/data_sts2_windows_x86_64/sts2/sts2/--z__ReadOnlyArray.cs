using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000DA7 RID: 3495
[CompilerGenerated]
internal sealed class <>z__ReadOnlyArray<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x06008C6A RID: 35946 RVA: 0x002EECD5 File Offset: 0x002ECED5
	public <>z__ReadOnlyArray(T[] items)
	{
		this._items = items;
	}

	// Token: 0x06008C6B RID: 35947 RVA: 0x002EECE4 File Offset: 0x002ECEE4
	[return: Nullable(1)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x17002682 RID: 9858
	// (get) Token: 0x06008C6C RID: 35948 RVA: 0x002EECF1 File Offset: 0x002ECEF1
	int ICollection.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17002683 RID: 9859
	// (get) Token: 0x06008C6D RID: 35949 RVA: 0x002EECFB File Offset: 0x002ECEFB
	bool ICollection.IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17002684 RID: 9860
	// (get) Token: 0x06008C6E RID: 35950 RVA: 0x002EECFE File Offset: 0x002ECEFE
	object ICollection.SyncRoot
	{
		[return: Nullable(1)]
		get
		{
			return this;
		}
	}

	// Token: 0x06008C6F RID: 35951 RVA: 0x002EED01 File Offset: 0x002ECF01
	void ICollection.CopyTo([Nullable(1)] Array array, int index)
	{
		this._items.CopyTo(array, index);
	}

	// Token: 0x17002685 RID: 9861
	object IList.this[int index]
	{
		[return: Nullable(2)]
		get
		{
			return this._items[index];
		}
		[param: Nullable(2)]
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x17002686 RID: 9862
	// (get) Token: 0x06008C72 RID: 35954 RVA: 0x002EED2A File Offset: 0x002ECF2A
	bool IList.IsFixedSize
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17002687 RID: 9863
	// (get) Token: 0x06008C73 RID: 35955 RVA: 0x002EED2D File Offset: 0x002ECF2D
	bool IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06008C74 RID: 35956 RVA: 0x002EED30 File Offset: 0x002ECF30
	int IList.Add([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C75 RID: 35957 RVA: 0x002EED37 File Offset: 0x002ECF37
	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C76 RID: 35958 RVA: 0x002EED3E File Offset: 0x002ECF3E
	bool IList.Contains([Nullable(2)] object value)
	{
		return this._items.Contains(value);
	}

	// Token: 0x06008C77 RID: 35959 RVA: 0x002EED4C File Offset: 0x002ECF4C
	int IList.IndexOf([Nullable(2)] object value)
	{
		return this._items.IndexOf(value);
	}

	// Token: 0x06008C78 RID: 35960 RVA: 0x002EED5A File Offset: 0x002ECF5A
	void IList.Insert(int index, [Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C79 RID: 35961 RVA: 0x002EED61 File Offset: 0x002ECF61
	void IList.Remove([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C7A RID: 35962 RVA: 0x002EED68 File Offset: 0x002ECF68
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C7B RID: 35963 RVA: 0x002EED6F File Offset: 0x002ECF6F
	[return: Nullable(new byte[] { 1, 0 })]
	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x17002688 RID: 9864
	// (get) Token: 0x06008C7C RID: 35964 RVA: 0x002EED7C File Offset: 0x002ECF7C
	int IReadOnlyCollection<T>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17002689 RID: 9865
	T IReadOnlyList<T>.this[int index]
	{
		get
		{
			return this._items[index];
		}
	}

	// Token: 0x1700268A RID: 9866
	// (get) Token: 0x06008C7E RID: 35966 RVA: 0x002EED94 File Offset: 0x002ECF94
	int ICollection<T>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x1700268B RID: 9867
	// (get) Token: 0x06008C7F RID: 35967 RVA: 0x002EED9E File Offset: 0x002ECF9E
	bool ICollection<T>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06008C80 RID: 35968 RVA: 0x002EEDA1 File Offset: 0x002ECFA1
	void ICollection<T>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C81 RID: 35969 RVA: 0x002EEDA8 File Offset: 0x002ECFA8
	void ICollection<T>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C82 RID: 35970 RVA: 0x002EEDAF File Offset: 0x002ECFAF
	bool ICollection<T>.Contains(T item)
	{
		return this._items.Contains(item);
	}

	// Token: 0x06008C83 RID: 35971 RVA: 0x002EEDBD File Offset: 0x002ECFBD
	void ICollection<T>.CopyTo([Nullable(new byte[] { 1, 0 })] T[] array, int arrayIndex)
	{
		this._items.CopyTo(array, arrayIndex);
	}

	// Token: 0x06008C84 RID: 35972 RVA: 0x002EEDCC File Offset: 0x002ECFCC
	bool ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x1700268C RID: 9868
	T IList<T>.this[int index]
	{
		get
		{
			return this._items[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x06008C87 RID: 35975 RVA: 0x002EEDE8 File Offset: 0x002ECFE8
	int IList<T>.IndexOf(T item)
	{
		return this._items.IndexOf(item);
	}

	// Token: 0x06008C88 RID: 35976 RVA: 0x002EEDF6 File Offset: 0x002ECFF6
	void IList<T>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C89 RID: 35977 RVA: 0x002EEDFD File Offset: 0x002ECFFD
	void IList<T>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0400313F RID: 12607
	[CompilerGenerated]
	private readonly T[] _items;
}
