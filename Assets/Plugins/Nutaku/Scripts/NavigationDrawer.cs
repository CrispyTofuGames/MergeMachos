using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Nutaku.Unity
{
    public class NavigationDrawer : MonoBehaviour
    {
        public RectTransform menuList;
        public Button menuButton;
        public CanvasGroup glassPane;

        public Button closeButton;
        public Button nutakuTopButton;
        public Button goldStoreButton;
        public Button faqButton;
        public Button gameSupportButton;
        public Button websiteContactButton;
        public Button paymentContactButton;
        public Button termsOfUseButton;
        public Button aboutNutakuButton;
        public Button updateButton;
        public Button logoutAndExitButton;
        public Button logoutAndRestartButton;

        void Awake()
        {
            SdkPlugin.Initialize();

#if !UNITY_ANDROID
            gameObject.SetActive(false);
            return;
#else

            menuButton.gameObject.SetActive(true);
            menuButton.onClick.AddListener(ShowDrawer);
            glassPane.GetComponent<Button>().onClick.AddListener(HideDrawer);
            closeButton.GetComponent<Button>().onClick.AddListener(HideDrawer);

            if (SdkPlugin.settings.canUpdate)
                updateButton.gameObject.SetActive(true);

            nutakuTopButton.onClick.AddListener(SdkPlugin.OpenNutakuTop);
            goldStoreButton.onClick.AddListener(SdkPlugin.OpenGoldStore);
            faqButton.onClick.AddListener(SdkPlugin.OpenFaq);
            gameSupportButton.onClick.AddListener(SdkPlugin.OpenGameSupport);
            websiteContactButton.onClick.AddListener(SdkPlugin.OpenWebsiteContact);
            paymentContactButton.onClick.AddListener(SdkPlugin.OpenPaymentContact);
            termsOfUseButton.onClick.AddListener(SdkPlugin.OpenTermsOfUse);
            aboutNutakuButton.onClick.AddListener(SdkPlugin.OpenAboutNutaku);
            updateButton.onClick.AddListener(SdkPlugin.OpenUpdateDialog);
            logoutAndExitButton.onClick.AddListener(SdkPlugin.logoutAndExit);
            logoutAndRestartButton.onClick.AddListener(SdkPlugin.logoutAndRestart);

#endif
        }

        IEnumerator Start()
        {
            while (menuList.sizeDelta.x <= float.Epsilon)
            {
                yield return null;
            }
        }

        void OnDestroy()
        {
            PlayerPrefs.Save();
        }

        public void ShowDrawer()
        {
            menuList.gameObject.SetActive(true);
            glassPane.gameObject.SetActive(true);
        }

        public void HideDrawer()
        {
            menuList.gameObject.SetActive(false);
            glassPane.gameObject.SetActive(false);
        }
    }
}
