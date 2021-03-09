using System;

namespace Nutaku.Unity
{
    /// <summary>
	/// Structure that represents security token
    /// </summary>
    public struct SecurityToken
    {
        /// <summary>
		/// Security token validity period (minutes)
        /// </summary>
        public static readonly double ValidDurationMinutes = 30.0;

        /// <summary>
        /// Security token value
        /// </summary>
        public string token;

        /// <summary>
        /// DateTime of acquisition
        /// </summary>
        public DateTime dateTime;

        public SecurityToken(string token, DateTime dateTime)
        {
            this.token = token;
            this.dateTime = dateTime;
        }
    }
}
