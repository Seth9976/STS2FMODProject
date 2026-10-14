using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Helpers
{
	/// <summary>
	/// A generic collection that allows the caller to grab random elements with weighted probabilities.
	/// </summary>
	/// <example>
	/// Imagine the following code:
	///
	/// <code>
	/// GrabBag{string} bag = new GrabBag{string}();
	///
	/// bag.Add("dog", 1);
	/// bag.Add("cat", 1);
	/// bag.Add("fish", 2);
	/// bag.add("lion", 0.5);
	/// bag.add("tiger", 0.5);
	///
	/// string result = bag.Grab();
	/// </code>
	///
	/// In the above code, 5 elements are added to a grab bag with differing weights, then a random one is chosen.
	/// If the above code were run 100 times, the results should be:
	/// * "dog" approximately 20 times
	/// * "cat" approximately 20 times
	/// * "fish" approximately 40 times (twice as many as dog or cat, since it has a weight of 2 instead of 1)
	/// * "lion" approximately 10 times (half as many as dog or cat, since it has a weight of 0.5 instead of 1)
	/// * "tiger" approximately 10 times (half as many as dog or cat, since it has a weight of 0.5 instead of 1)
	/// </example>
	/// <typeparam name="T">Type of elements contained by GrabBag.</typeparam>
	// Token: 0x02000C09 RID: 3081
	[NullableContext(1)]
	[Nullable(0)]
	public class GrabBag<[Nullable(2)] T>
	{
		/// <summary>
		/// Add an element with a specified weight.
		/// </summary>
		/// <param name="element">Element to add.</param>
		/// <param name="weight">Amount to weight the element.</param>
		// Token: 0x06007FB0 RID: 32688 RVA: 0x002B55B4 File Offset: 0x002B37B4
		public void Add(T element, double weight)
		{
			this._entries.Add(new ValueTuple<T, double>(element, weight));
			this._totalWeight += weight;
		}

		/// <summary>
		/// Grab a random element from the bag, respecting weights.
		/// </summary>
		/// <param name="rng">Generator to grab the random roll from.</param>
		/// <param name="predicate">Predicate that must be matched by the returned element.</param>
		/// <returns>Randomly-chosen element. Null if the bag is empty.</returns>
		// Token: 0x06007FB1 RID: 32689 RVA: 0x002B55D8 File Offset: 0x002B37D8
		[return: Nullable(2)]
		public T Grab(Rng rng, [Nullable(new byte[] { 2, 1 })] Func<T, bool> predicate = null)
		{
			int num = this.GrabIndex(rng, predicate);
			if (num < 0)
			{
				return default(T);
			}
			return this._entries[num].Item1;
		}

		/// <summary>
		/// Grab a random element from the bag, respecting weights. Remove it after grabbing.
		/// </summary>
		/// <param name="rng">Generator to grab the random roll from.</param>
		/// <param name="predicate">Predicate that must be matched by the returned element.</param>
		/// <returns>Randomly-chosen element. Null if the bag is empty.</returns>
		// Token: 0x06007FB2 RID: 32690 RVA: 0x002B5610 File Offset: 0x002B3810
		[return: Nullable(2)]
		public T GrabAndRemove(Rng rng, [Nullable(new byte[] { 2, 1 })] Func<T, bool> predicate = null)
		{
			int num = this.GrabIndex(rng, predicate);
			if (num < 0)
			{
				return default(T);
			}
			T item = this._entries[num].Item1;
			this.Remove(num);
			return item;
		}

		// Token: 0x17002305 RID: 8965
		// (get) Token: 0x06007FB3 RID: 32691 RVA: 0x002B564E File Offset: 0x002B384E
		public int Count
		{
			get
			{
				return this._entries.Count;
			}
		}

		// Token: 0x06007FB4 RID: 32692 RVA: 0x002B565B File Offset: 0x002B385B
		public bool Any()
		{
			return this._entries.Any<ValueTuple<T, double>>();
		}

		// Token: 0x06007FB5 RID: 32693 RVA: 0x002B5668 File Offset: 0x002B3868
		private int GrabIndex(Rng rng, [Nullable(new byte[] { 2, 1 })] Func<T, bool> predicate)
		{
			if (predicate != null && !this._entries.Any<ValueTuple<T, double>>((ValueTuple<T, double> e) => predicate(e.Item1)))
			{
				return -1;
			}
			int num;
			do
			{
				num = this.GrabIndex(rng);
			}
			while (predicate != null && num >= 0 && !predicate(this._entries[num].Item1));
			return num;
		}

		// Token: 0x06007FB6 RID: 32694 RVA: 0x002B56D8 File Offset: 0x002B38D8
		private int GrabIndex(Rng rng)
		{
			double num = rng.NextDouble() * this._totalWeight;
			double num2 = 0.0;
			for (int i = 0; i < this._entries.Count; i++)
			{
				num2 += this._entries[i].Item2;
				if (num < num2)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06007FB7 RID: 32695 RVA: 0x002B572E File Offset: 0x002B392E
		private void Remove(int index)
		{
			this._totalWeight -= this._entries[index].Item2;
			this._entries.RemoveAt(index);
		}

		/// <summary>
		///
		/// </summary>
		// Token: 0x04002A84 RID: 10884
		[Nullable(new byte[] { 1, 0, 1 })]
		private readonly List<ValueTuple<T, double>> _entries = new List<ValueTuple<T, double>>();

		// Token: 0x04002A85 RID: 10885
		private double _totalWeight;
	}
}
