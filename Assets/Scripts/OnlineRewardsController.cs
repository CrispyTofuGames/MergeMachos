using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineRewardsController : MonoBehaviour
{
    PanelManager panelManager;

    private void Start()
    {
        panelManager = FindObjectOfType<PanelManager>();
        StartCoroutine(CheckOnlineRewards());
    }
    IEnumerator CheckOnlineRewards()
    {
        yield return new WaitForSeconds(5);
        if (UserDataController.GetBiggestDino() > 5)
        {
            if (!UserDataController._currentUserData._epicChestsRedeemed6)
            {
                UserDataController._currentUserData._epicChestsRedeemed6 = true;
                FindObjectOfType<RewardManager>().EarnLootBox(2, 2);
            }
            //string nutakuUserID = PlayerPrefs.GetString("NutakuID");
            //UnityWebRequest www = UnityWebRequest.Get("http://s852714066.mialojamiento.es/tavernofsins/getrewards.php?nutakuuserid=" + nutakuUserID);
            //yield return www.SendWebRequest();
            //string responseText = www.downloadHandler.text;
            //OnlineReward onlineReward = null;
            //if (responseText != null)
            //{
            //    if (responseText != "")
            //    {

            //        onlineReward = JsonUtility.FromJson<OnlineReward>(responseText);
            //        UnityWebRequest redeemWWW = UnityWebRequest.Get("http://s852714066.mialojamiento.es/tavernofsins/redeemreward.php?rewardID=" + onlineReward.ID);
            //        yield return redeemWWW.SendWebRequest();

            //        while (panelManager.GetPanelState())
            //        {
            //            yield return null;
            //        }
            //        if (onlineReward.epicChests > 0)
            //        {
            //            FindObjectOfType<RewardManager>().EarnLootBox(2, onlineReward.epicChests);
            //        }
            //        if (onlineReward.gems > 0)
            //        {
            //            FindObjectOfType<RewardManager>().EarnHardCoin(onlineReward.gems);
            //        }
            //    }
            //}
        }
    }

    [System.Serializable]
    public class OnlineReward
    {
        public int ID;
        public int epicChests;
        public int gems;
    }
}
