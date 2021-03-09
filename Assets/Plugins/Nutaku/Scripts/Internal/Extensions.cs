using System;
using System.Collections.Generic;

namespace Nutaku.Unity.Utils
{
    /// <summary>
    /// Method extensions used in the library
    /// </summary>
    static class Extensions
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> enumerable, Action<TSource> action)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            foreach (var elem in enumerable)
            {
                action(elem);
            }
        }

        /// <summary>
		/// Alternative to Average float on iOS + AOT
        /// </summary>
        public static float FloatAverage(this IEnumerable<float> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            var sum = 0f;
            var count = 0;
            foreach (var elem in enumerable)
            {
                sum += elem;
                ++count;
            }
            return sum / count;
        }

		public static int IEnumCount<TSource>(this IEnumerable<TSource> enumerable)
		{
			if (enumerable == null)
				return 0;

			int result = 0;

			foreach (var elem in enumerable)
				result++;
			
			return result;
		}

		public static TSource[] IEnumToArray<TSource>(this IEnumerable<TSource> enumerable)
		{
			if (enumerable == null)
				return null;

			List<TSource> res = new List<TSource> ();

			foreach (var elem in enumerable)
				res.Add(elem);

			return res.ToArray();
		}
    }
}
