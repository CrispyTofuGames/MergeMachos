namespace Nutaku.Unity
{
    public delegate void PaymentResultDelegate(WebViewEvent resultDelegate);

    /// <summary>
    /// Native platform interface for Android
    /// </summary>
    interface INativePlugin
    {
        /// <summary>
        /// Get the Login information
        /// </summary>
        LoginInfo loginInfo { get; }

        /// <summary>
        /// Get the SDK settings
        /// </summary>
        SdkSettings settings { get; }

        /// <summary>
        /// Get endpoint information for opensocial (OSAPI)
        /// </summary>
        ApiInfo socialApiInfo { get; }

        /// <summary>
        /// Get the endpoint information of the core API.
        /// </summary>
        ApiInfo coreApiInfo { get; }

        /// <summary>
        /// Open item purchase page
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        void OpenPayment(string url, PaymentResultDelegate paymentResultDelegate);

        /// <summary>
        /// Open Nutaku Top (Nutaku frontend site)
        /// </summary>
        void OpenNutakuTop();

        /// <summary>
        /// Open Gold Store (for purchasing extra gold)
        /// </summary>
        void OpenGoldStore();

        /// <summary>
        /// Open the FAQ page
        /// </summary>
        void OpenFaq();

        /// <summary>
        /// Open Game Support page
        /// </summary>
        void OpenGameSupport();

        /// <summary>
        /// Open Website Contact page
        /// </summary>
        void OpenWebsiteContact();

        /// <summary>
        /// Open Payment Contact page
        /// </summary>
        void OpenPaymentContact();


        /// <summary>
        /// Open Terms of Use page
        /// </summary>
        void OpenTermsOfUse();

        /// <summary>
        /// Open About Nutaku screen
        /// </summary>
        void OpenAboutNutaku();

        /// <summary>
        /// Open the APK update dialog
        /// </summary>
        void OpenUpdateDialog();

        /// <summary>
        /// Logout and Exit Option
        /// </summary>
        void logoutAndExit();

        /// <summary>
        /// Logout and Restart Option
        /// </summary>
        void logoutAndRestart();
    }
}
