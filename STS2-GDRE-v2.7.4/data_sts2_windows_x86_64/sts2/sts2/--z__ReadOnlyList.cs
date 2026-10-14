using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000DA8 RID: 3496
[CompilerGenerated]
internal sealed class <>z__ReadOnlyList<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x06008C8A RID: 35978 RVA: 0x002EEE04 File Offset: 0x002ED004
	public <>z__ReadOnlyList(List<T> items)
	{
		this._items = items;
	}

	// Token: 0x06008C8B RID: 35979 RVA: 0x002EEE13 File Offset: 0x002ED013
	[return: Nullable(1)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)this._items).GetEnumerator();
	}

	// Token: 0x1700268D RID: 9869
	// (get) Token: 0x06008C8C RID: 35980 RVA: 0x002EEE20 File Offset: 0x002ED020
	int ICollection.Count
	{
		get
		{
			return this._items.Count;
		}
	}

	// Token: 0x1700268E RID: 9870
	// (get) Token: 0x06008C8D RID: 35981 RVA: 0x002EEE2D File Offset: 0x002ED02D
	bool ICollection.IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700268F RID: 9871
	// (get) Token: 0x06008C8E RID: 35982 RVA: 0x002EEE30 File Offset: 0x002ED030
	object ICollection.SyncRoot
	{
		[return: Nullable(1)]
		get
		{
			return this;
		}
	}

	// Token: 0x06008C8F RID: 35983 RVA: 0x002EEE33 File Offset: 0x002ED033
	void ICollection.CopyTo([Nullable(1)] Array array, int index)
	{
		((ICollection)this._items).CopyTo(array, index);
	}

	// Token: 0x17002690 RID: 9872
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

	// Token: 0x17002691 RID: 9873
	// (get) Token: 0x06008C92 RID: 35986 RVA: 0x002EEE5C File Offset: 0x002ED05C
	bool IList.IsFixedSize
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17002692 RID: 9874
	// (get) Token: 0x06008C93 RID: 35987 RVA: 0x002EEE5F File Offset: 0x002ED05F
	bool IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06008C94 RID: 35988 RVA: 0x002EEE62 File Offset: 0x002ED062
	int IList.Add([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C95 RID: 35989 RVA: 0x002EEE69 File Offset: 0x002ED069
	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C96 RID: 35990 RVA: 0x002EEE70 File Offset: 0x002ED070
	bool IList.Contains([Nullable(2)] object value)
	{
		return ((IList)this._items).Contains(value);
	}

	// Token: 0x06008C97 RID: 35991 RVA: 0x002EEE7E File Offset: 0x002ED07E
	int IList.IndexOf([Nullable(2)] object value)
	{
		return ((IList)this._items).IndexOf(value);
	}

	// Token: 0x06008C98 RID: 35992 RVA: 0x002EEE8C File Offset: 0x002ED08C
	void IList.Insert(int index, [Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C99 RID: 35993 RVA: 0x002EEE93 File Offset: 0x002ED093
	void IList.Remove([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C9A RID: 35994 RVA: 0x002EEE9A File Offset: 0x002ED09A
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008C9B RID: 35995 RVA: 0x002EEEA1 File Offset: 0x002ED0A1
	[return: Nullable(new byte[] { 1, 0 })]
	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return ((IEnumerable<T>)this._items).GetEnumerator();
	}

	// Token: 0x17002693 RID: 9875
	// (get) Token: 0x06008C9C RID: 35996 RVA: 0x002EEEAE File Offset: 0x002ED0AE
	int IReadOnlyCollection<T>.Count
	{
		get
		{
			return this._items.Count;
		}
	}

	// Token: 0x17002694 RID: 9876
	T IReadOnlyList<T>.this[int index]
	{
		get
		{
			return this._items[index];
		}
	}

	// Token: 0x17002695 RID: 9877
	// (get) Token: 0x06008C9E RID: 35998 RVA: 0x002EEEC9 File Offset: 0x002ED0C9
	int ICollection<T>.Count
	{
		get
		{
			return this._items.Count;
		}
	}

	// Token: 0x17002696 RID: 9878
	// (get) Token: 0x06008C9F RID: 35999 RVA: 0x002EEED6 File Offset: 0x002ED0D6
	bool ICollection<T>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06008CA0 RID: 36000 RVA: 0x002EEED9 File Offset: 0x002ED0D9
	void ICollection<T>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CA1 RID: 36001 RVA: 0x002EEEE0 File Offset: 0x002ED0E0
	void ICollection<T>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CA2 RID: 36002 RVA: 0x002EEEE7 File Offset: 0x002ED0E7
	bool ICollection<T>.Contains(T item)
	{
		return this._items.Contains(item);
	}

	// Token: 0x06008CA3 RID: 36003 RVA: 0x002EEEF5 File Offset: 0x002ED0F5
	void ICollection<T>.CopyTo([Nullable(new byte[] { 1, 0 })] T[] array, int arrayIndex)
	{
		this._items.CopyTo(array, arrayIndex);
	}

	// Token: 0x06008CA4 RID: 36004 RVA: 0x002EEF04 File Offset: 0x002ED104
	bool ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x17002697 RID: 9879
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

	// Token: 0x06008CA7 RID: 36007 RVA: 0x002EEF20 File Offset: 0x002ED120
	int IList<T>.IndexOf(T item)
	{
		return this._items.IndexOf(item);
	}

	// Token: 0x06008CA8 RID: 36008 RVA: 0x002EEF2E File Offset: 0x002ED12E
	void IList<T>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CA9 RID: 36009 RVA: 0x002EEF35 File Offset: 0x002ED135
	void IList<T>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x04003140 RID: 12608
	[CompilerGenerated]
	private readonly List<T> _items;
}
