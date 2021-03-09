using System;

namespace Nutaku.Unity
{
    /// <summary>
    /// For use with Inspection API
    /// </summary>
    public class UserText
    {
        /// <summary>
        /// Text entry ID
        /// </summary>
        public string textId;

        /// <summary>
        /// App ID
        /// </summary>
        public string appId;

        /// <summary>
        /// Author user ID
        /// </summary>
        public string authorId;

        /// <summary>
		/// Game owner user ID (same as author)
        /// </summary>
        public string ownerId;

        /// <summary>
        /// Text data in the Text entry
        /// </summary>
        public string data;

        /// <summary>
        /// status
		/// 1: Monitoring in progress, 2: Monitored: Accepted / OK, 3: Deleted, 4: Monitored: Not acceptable / Not OK / NG
        /// </summary>
        public int? status;

        /// <summary>
        /// DateTime of creation
        /// </summary>
        public DateTime? ctime;

        /// <summary>
        /// DateTime of last modification
        /// </summary>
        public DateTime? mtime;
    }
}
