using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000DA9 RID: 3497
[CompilerGenerated]
internal sealed class <>z__ReadOnlySingleElementList<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x06008CAA RID: 36010 RVA: 0x002EEF3C File Offset: 0x002ED13C
	public <>z__ReadOnlySingleElementList(T item)
	{
		this._item = item;
	}

	// Token: 0x06008CAB RID: 36011 RVA: 0x002EEF4B File Offset: 0x002ED14B
	[return: Nullable(1)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new <>z__ReadOnlySingleElementList<T>.Enumerator(this._item);
	}

	// Token: 0x17002698 RID: 9880
	// (get) Token: 0x06008CAC RID: 36012 RVA: 0x002EEF58 File Offset: 0x002ED158
	int ICollection.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17002699 RID: 9881
	// (get) Token: 0x06008CAD RID: 36013 RVA: 0x002EEF5B File Offset: 0x002ED15B
	bool ICollection.IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700269A RID: 9882
	// (get) Token: 0x06008CAE RID: 36014 RVA: 0x002EEF5E File Offset: 0x002ED15E
	object ICollection.SyncRoot
	{
		[return: Nullable(1)]
		get
		{
			return this;
		}
	}

	// Token: 0x06008CAF RID: 36015 RVA: 0x002EEF61 File Offset: 0x002ED161
	void ICollection.CopyTo([Nullable(1)] Array array, int index)
	{
		array.SetValue(this._item, index);
	}

	// Token: 0x1700269B RID: 9883
	object IList.this[int index]
	{
		[return: Nullable(2)]
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this._item;
		}
		[param: Nullable(2)]
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x1700269C RID: 9884
	// (get) Token: 0x06008CB2 RID: 36018 RVA: 0x002EEF92 File Offset: 0x002ED192
	bool IList.IsFixedSize
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700269D RID: 9885
	// (get) Token: 0x06008CB3 RID: 36019 RVA: 0x002EEF95 File Offset: 0x002ED195
	bool IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06008CB4 RID: 36020 RVA: 0x002EEF98 File Offset: 0x002ED198
	int IList.Add([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CB5 RID: 36021 RVA: 0x002EEF9F File Offset: 0x002ED19F
	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CB6 RID: 36022 RVA: 0x002EEFA6 File Offset: 0x002ED1A6
	bool IList.Contains([Nullable(2)] object value)
	{
		return EqualityComparer<T>.Default.Equals(this._item, (T)((object)value));
	}

	// Token: 0x06008CB7 RID: 36023 RVA: 0x002EEFBE File Offset: 0x002ED1BE
	int IList.IndexOf([Nullable(2)] object value)
	{
		if (!EqualityComparer<T>.Default.Equals(this._item, (T)((object)value)))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06008CB8 RID: 36024 RVA: 0x002EEFDB File Offset: 0x002ED1DB
	void IList.Insert(int index, [Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CB9 RID: 36025 RVA: 0x002EEFE2 File Offset: 0x002ED1E2
	void IList.Remove([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CBA RID: 36026 RVA: 0x002EEFE9 File Offset: 0x002ED1E9
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CBB RID: 36027 RVA: 0x002EEFF0 File Offset: 0x002ED1F0
	[return: Nullable(new byte[] { 1, 0 })]
	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new <>z__ReadOnlySingleElementList<T>.Enumerator(this._item);
	}

	// Token: 0x1700269E RID: 9886
	// (get) Token: 0x06008CBC RID: 36028 RVA: 0x002EEFFD File Offset: 0x002ED1FD
	int IReadOnlyCollection<T>.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700269F RID: 9887
	T IReadOnlyList<T>.this[int index]
	{
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this._item;
		}
	}

	// Token: 0x170026A0 RID: 9888
	// (get) Token: 0x06008CBE RID: 36030 RVA: 0x002EF011 File Offset: 0x002ED211
	int ICollection<T>.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170026A1 RID: 9889
	// (get) Token: 0x06008CBF RID: 36031 RVA: 0x002EF014 File Offset: 0x002ED214
	bool ICollection<T>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06008CC0 RID: 36032 RVA: 0x002EF017 File Offset: 0x002ED217
	void ICollection<T>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CC1 RID: 36033 RVA: 0x002EF01E File Offset: 0x002ED21E
	void ICollection<T>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CC2 RID: 36034 RVA: 0x002EF025 File Offset: 0x002ED225
	bool ICollection<T>.Contains(T item)
	{
		return EqualityComparer<T>.Default.Equals(this._item, item);
	}

	// Token: 0x06008CC3 RID: 36035 RVA: 0x002EF038 File Offset: 0x002ED238
	void ICollection<T>.CopyTo([Nullable(new byte[] { 1, 0 })] T[] array, int arrayIndex)
	{
		array[arrayIndex] = this._item;
	}

	// Token: 0x06008CC4 RID: 36036 RVA: 0x002EF047 File Offset: 0x002ED247
	bool ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x170026A2 RID: 9890
	T IList<T>.this[int index]
	{
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this._item;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x06008CC7 RID: 36039 RVA: 0x002EF066 File Offset: 0x002ED266
	int IList<T>.IndexOf(T item)
	{
		if (!EqualityComparer<T>.Default.Equals(this._item, item))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06008CC8 RID: 36040 RVA: 0x002EF07E File Offset: 0x002ED27E
	void IList<T>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06008CC9 RID: 36041 RVA: 0x002EF085 File Offset: 0x002ED285
	void IList<T>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x04003141 RID: 12609
	[CompilerGenerated]
	private readonly T _item;

	// Token: 0x0200258B RID: 9611
	private sealed class Enumerator : IDisposable, IEnumerator, IEnumerator<T>
	{
		// Token: 0x0600C66B RID: 50795 RVA: 0x0044F186 File Offset: 0x0044D386
		public Enumerator(T item)
		{
			this.System.Collections.Generic.IEnumerator<T>.Current = item;
		}

		// Token: 0x170026F4 RID: 9972
		// (get) Token: 0x0600C66C RID: 50796 RVA: 0x0044F195 File Offset: 0x0044D395
		object IEnumerator.Current
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x170026F5 RID: 9973
		// (get) Token: 0x0600C66D RID: 50797 RVA: 0x0044F1A2 File Offset: 0x0044D3A2
		T IEnumerator<T>.Current
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x0600C66E RID: 50798 RVA: 0x0044F1AC File Offset: 0x0044D3AC
		bool IEnumerator.MoveNext()
		{
			return !this._moveNextCalled && (this._moveNextCalled = true);
		}

		// Token: 0x0600C66F RID: 50799 RVA: 0x0044F1CD File Offset: 0x0044D3CD
		void IEnumerator.Reset()
		{
			this._moveNextCalled = false;
		}

		// Token: 0x0600C670 RID: 50800 RVA: 0x0044F1D6 File Offset: 0x0044D3D6
		void IDisposable.Dispose()
		{
		}

		// Token: 0x0400A093 RID: 41107
		[CompilerGenerated]
		private readonly T _item;

		// Token: 0x0400A094 RID: 41108
		[CompilerGenerated]
		private bool _moveNextCalled;
	}
}
