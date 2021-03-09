#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using System;

namespace Nutaku.Unity
{
    /// <summary>
	/// Stub of the NativePlugin for running the SDK in Unity Editor.
    /// </summary>
    class NativePluginStub : INativePlugin
    {
        const int VersionCode = 1;
        const bool CanUpdate = false;
        const string CoreApiEndpoint = "https://sbox-mobileapi.nutaku.com/";
        const string SocialApiEndpoint = "https://sbox-osapi.nutaku.com/social_android/rest/";

        SdkSettings _sdkSettings;
        public LoginInfo _loginInfo;
        ApiInfo _socialApiInfo;
        ApiInfo _coreApiInfo;
        bool _initialized = false;

        internal static NativePluginStub _instance = new NativePluginStub();

        /// <summary>
		/// Get a NativePluginStub instance.
        /// </summary>
        internal static NativePluginStub instance
        {
            get
            {
                return _instance;
            }
        }

        internal static void Initialize()
        {
            _instance.ReadSdkSettings();
            _instance._loginInfo = SandboxLoginView.Login();
			_instance._initialized = true;
        }

        public LoginInfo loginInfo
        {
            get
            {
                if (!_initialized)
                {
                    throw new InitializationException("Initialization has not been completed yet");
                }
                return _loginInfo;
            }
        }

        public SdkSettings settings
        {
            get
            {
                if (_sdkSettings == null)
                {
                    ReadSdkSettings();
                }

                return _sdkSettings;
            }
        }

        public ApiInfo socialApiInfo
        {
            get
            {
                if (_socialApiInfo == null)
                {
                    var uri = new Uri(SocialApiEndpoint);
                    _socialApiInfo = new ApiInfo(uri.Scheme, uri.Host, uri.AbsolutePath);
                }
                return _socialApiInfo;
            }
        }

        public ApiInfo coreApiInfo
        {
            get
            {
                if (_coreApiInfo == null)
                {
                    var uri = new Uri(CoreApiEndpoint);
                    _coreApiInfo = new ApiInfo(uri.Scheme, uri.Host, uri.AbsolutePath);
                }
                return _coreApiInfo;
            }
        }

		public void OpenPayment(string url, PaymentResultDelegate paymentDelegate)
        {
			const string message = "OpenPayment is not available in Unity Editor";
			Debug.LogError(message);
			var result = new WebViewEvent()
			{
				kind = WebViewEventKind.Failed,
				message = message,
			};
			paymentDelegate (result);
        }

        public void OpenNutakuTop()
        {
			Debug.LogError("OpenNutakuTop is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenGoldStore()
        {
			Debug.LogError("OpenGoldStore is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenFaq()
        {
			Debug.LogError("OpenFaq is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenGameSupport()
        {
			Debug.LogError("OpenGameSupport is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenWebsiteContact()
        {
			Debug.LogError("OpenWebsiteContact is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenPaymentContact()
        {
			Debug.LogError("OpenPaymentContact is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenTermsOfUse()
        {
			Debug.LogError("OpenTermsOfUse is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenAboutNutaku()
        {
			Debug.LogError("OpenAboutNutaku is not available in Unity Editor. You need to be running on a device.");
        }

        public void OpenUpdateDialog()
        {
			Debug.LogError("OpenUpdateDialog is not available in Unity Editor. You need to be running on a device.");
        }

		public void logoutAndExit()
		{
			Debug.LogError("LogoutAndExit is not available in Unity Editor. You need to be running on a device.");
		}

		public void logoutAndRestart()
		{
			Debug.LogError("LogoutAndRestart is not available in Unity Editor. You need to be running on a device.");
		}

        void ReadSdkSettings()
        {
            try
            {
                var configFilePath = Application.dataPath + "/Plugins/Android/res/xml/nutaku_game_configuration.xml";

                using (var stream = new FileStream(configFilePath, FileMode.Open))
                {
					TextReader tr = new StreamReader(stream);
					var xDocument = XDocument.Load(tr);
					tr.Dispose();

                    var properties = xDocument.Elements("properties");
                    if (properties.Count() != 1)
                    {
                        throw new InitializationException("nutaku_game_configuration.xml must have one 'properties' element");
                    }

                    var appId = properties.First().Elements("appId");
                    if (appId.Count() != 1)
                    {
                        throw new InitializationException(
                            "'properties' element must have one 'appId' element");
                    }

                    var gameName = properties.First().Elements("gameName");
                    if (gameName.Count() != 1)
                    {
                        throw new InitializationException(
							"'properties' element must have one 'gameName' element");
                    }

                    var environment = properties.First().Elements("environment");
                    if (environment.Count() != 1)
                    {
                        throw new InitializationException(
							"'properties' element must have one 'environment' element");
                    }

                    var consumerKey = properties.First().Elements("consumerKey");
                    if (consumerKey.Count() != 1)
                    {
                        throw new InitializationException(
							"'properties' element must have one 'consumerKey' element");
                    }

                    var consumerSecret = properties.First().Elements("consumerSecret");
                    if (consumerSecret.Count() != 1)
                    {
                        throw new InitializationException(
							"'properties' element must have one 'consumerSecret' element");
                    }

					string oauthSignaturePublicKey = null;
                    var oauthSignaturePublicKeyElement = properties.First().Elements("oauthSignaturePublicKey");
                    if (oauthSignaturePublicKeyElement.Count() == 1)
                    {
                        oauthSignaturePublicKey = oauthSignaturePublicKeyElement.First().Value;
                    }

                    var settings = new SdkSettings(
                                       appId.First().Value,
                                       gameName.First().Value,
                                       environment.First().Value,
                                       isDevelopmentMode(environment.First().Value),
                                       VersionCode,
                                       CanUpdate,
                                       consumerKey.First().Value,
                                       consumerSecret.First().Value,
                                       oauthSignaturePublicKey);

                    if (!settings.IsSandbox())
                    {
                        throw new InitializationException(
                            "It is possible to run on Unity Editor only in the sandbox environment");
                    }

                    _sdkSettings = settings;
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new InitializationException("nutaku_game_configuration.xml not found", ex);
            }
        }

        bool isDevelopmentMode(string environment)
        {
			return environment.ToLower () != "release";
        }
    }
}
#endif
