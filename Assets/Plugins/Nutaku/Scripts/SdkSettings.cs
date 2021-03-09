namespace Nutaku.Unity
{
    /// <summary>
    /// SDK Settings
    /// </summary>
    public class SdkSettings
    {
        /// <summary>
        /// Nutaku App ID
        /// </summary>
        public string appId { get; private set; }

        /// <summary>
        /// Game name
        /// </summary>
        public string gameName { get; private set; }

        /// <summary>
        /// Environment
        /// </summary>
        public string environment { get; private set; }

        /// <summary>
        /// Is Development mode?
        /// </summary>
        public bool isDevelopmentMode { get; private set; }

        /// <summary>
		/// Version Code
        /// </summary>
        public int versionCode { get; private set; }

        /// <summary>
        /// Is an APK update available
        /// </summary>
        public bool canUpdate { get; private set; }

        /// <summary>
        /// Consumer Key for OAuth
        /// </summary>
        public string consumerKey { get; private set; }

        /// <summary>
        /// Consumer Key for OAuth
        /// </summary>
        public string consumerSecret { get; private set; }

        /// <summary>
		/// Returns true if the development mode is sandbox
        /// </summary>
        public bool IsSandbox()
        {
            return environment.ToLower().Contains("sandbox");
        }

        /// <summary>
		/// The value of the OAUTH_SIGNATURE_PUBLICKEY parameter to be set in the REST API when the makeRequest API is executed
        /// </summary>
        public string oauthSignaturePublicKey { get; private set; }

        public SdkSettings(
            string appId,
            string gameName,
            string environment,
            bool isDevelopmentMode,
            int versionCode,
            bool canUpdate,
            string consumerKey,
            string consumerSecret)
        {
            this.appId = appId;
            this.gameName = gameName;
            this.environment = environment.ToLower();
            this.isDevelopmentMode = isDevelopmentMode;
            this.versionCode = versionCode;
            this.canUpdate = canUpdate;
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        public SdkSettings(
            string appId,
            string gameName,
            string environment,
            bool isDevelopmentMode,
            int versionCode,
            bool canUpdate,
            string consumerKey,
            string consumerSecret,
            string makeRequestOauthSignaturePublicKey)
            : this(appId, gameName, environment, isDevelopmentMode, versionCode, canUpdate, consumerKey, consumerSecret)
        {
            this.oauthSignaturePublicKey = makeRequestOauthSignaturePublicKey;
        }

        public SdkSettings() { }
    }
}
