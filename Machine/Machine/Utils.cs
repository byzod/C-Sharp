using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	/// <summary>
	/// Provides static helper methods or extension methods
	/// </summary>
	static class Utils
	{
		/*	
		 * ElementAtMin & ElementAtMax are from LinqLib (http://linqlib.codeplex.com/)
		 */

		/// <summary>
		/// Returns the element that yields the minimum value when processed by the selector function. 
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
		/// <typeparam name="TSelector">The type of the element returned by the selector of the source sequence. TSelector must implement the IComparable interface.</typeparam>
		/// <param name="source">Source sequence.</param>
		/// <param name="selector">A function that return a comparable value used to evaluate the minimum sequence value.</param>
		/// <returns>The element that yields the minimum value when processed by the selector function.</returns>
		/// <exception cref="System.ArgumentNullException">source is null.</exception>    
		/// <exception cref="System.ArgumentNullException">selector is null.</exception>    
		/// <exception cref="System.ArgumentException">source must have one or more elements.</exception>    
		public static TSource ElementAtMin<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector) where TSelector : IComparable
		{
			if (source == null) throw (new ArgumentNullException("source"));
			if (selector == null) throw (new ArgumentNullException("selector"));
			if (!source.Any()) throw (new ArgumentException("source is empty"));

			bool first = true;
			IComparable temp = null;
			TSource minElement = default(TSource);

			foreach (TSource item in source)
			{
				if (first)
				{
					temp = selector(item);
					minElement = item;
					first = false;
					continue;
				}

				IComparable temp2 = selector(item);
				if (temp.CompareTo(temp2) > 0)
				{
					temp = temp2;
					minElement = item;
				}
			}

			return minElement;
		}


		/// <summary>
		/// Returns the element that yields the maximum value when processed by the selector function. 
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
		/// <typeparam name="TSelector">The type of the element returned by the selector of the source sequence. TSelector must implement the IComparable interface.</typeparam>
		/// <param name="source">Source sequence.</param>
		/// <param name="selector">A function that return a comparable value used to evaluate the maximum sequence value.</param>
		/// <returns>The element that yields the maximum value when processed by the selector function.</returns>
		/// <exception cref="System.ArgumentNullException">source is null.</exception>    
		/// <exception cref="System.ArgumentNullException">selector is null.</exception>    
		/// <exception cref="System.ArgumentException">source must have one or more elements.</exception>    
		public static TSource ElementAtMax<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector) where TSelector : IComparable
		{
			if (source == null) throw (new ArgumentNullException("source"));
			if (selector == null) throw (new ArgumentNullException("selector"));
			if (!source.Any()) throw (new ArgumentException("source is empty"));

			bool first = true;
			IComparable temp = null;
			TSource maxElement = default(TSource);

			foreach (TSource item in source)
			{
				if (first)
				{
					temp = selector(item);
					maxElement = item;
					first = false;
					continue;
				}

				IComparable temp2 = selector(item);
				if (temp.CompareTo(temp2) <= 0)
				{
					temp = temp2;
					maxElement = item;
				}
			}

			return maxElement;
		}

	}
}
