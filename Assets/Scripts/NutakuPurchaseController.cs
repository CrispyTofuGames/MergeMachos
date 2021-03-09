using Nutaku.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

public class NutakuPurchaseController : MonoBehaviour
{
    string futureURL;
    bool loading = false;
    Payment currentPayment;
    int _gemsToBuy;
    RewardManager _rewardManager;

    private void Awake()
    {
        _rewardManager = FindObjectOfType<RewardManager>();
        SdkPlugin.Initialize();
    }

    public void RequestPayment(int gemPackIndex, int price, int gemsQuantity)
    {

        PlayerPrefs.SetInt("LastPurchasePrice", price);
        Payment requestPayment = new Payment();
            requestPayment.callbackUrl = "http://185.129.248.170/Migue/Nutaku/NutakuResponse.php";
        requestPayment.finishPageUrl = "https_//example.com/finish";
        requestPayment.message = "Gem Pack. "+gemsQuantity+ " gems.";
        _gemsToBuy = gemsQuantity;
        PaymentItem item1 = new PaymentItem();
        item1.itemId = "gemPack" + gemPackIndex;
        item1.itemName = gemsQuantity + " Gems Pack ";
        item1.unitPrice = price;
        item1.quantity = 1;
        item1.imageUrl = "https://www.crispytofugames.com/Nutaku/images/" + gemPackIndex+ ".png";
        item1.description = "A package full of shiny gems";
        requestPayment.paymentItems.Add(item1);
        try
        {
            RestApiHelper.Request.PostPayment(requestPayment, this, this.TestRequestPostPaymentCallback);
        }
        catch (Exception ex)
        {
            print("Errorcito");
        }
        
    }
    void TestRequestPostPaymentCallback(RawResult rawResult)
    {
        var result = RestApi.HandleRequestCallback<Payment>(rawResult);

        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                result = RestApi.HandleRequestCallback<Payment>(rawResult);
                currentPayment = result.GetFirstEntry(); //this is the Payment object which you should keep to be used for payment completion later
                    Debug.Log("Payment ID: " + currentPayment.paymentId);
                    Debug.Log("Status: " + currentPayment.status);
                    Debug.Log("Ordered Date and time: " + currentPayment.orderedTime);
                    Debug.Log("Response: " + currentPayment.transactionUrl);
                print("USER " + currentPayment.userId);
                futureURL = currentPayment.transactionUrl;
                print("Solicitud de pago completada, mostramos URL " + futureURL);

                //StartCoroutine(GetRequest(futureURL));
                try
                {
#if UNITY_EDITOR
                    _rewardManager.EarnHardCoin(_gemsToBuy);
#endif
#if !UNITY_EDITOR
                    SdkPlugin.OpenPaymentView(currentPayment, MyPaymentDelegate); // payment is the object obtained from PostPayment or GetPayment. MyPaymentDelegate is your payment delegate function
#endif
                }
                catch (Exception ex)
                {
                    Debug.Log("Error al pedir el pago: " + (int)rawResult.statusCode);
                }
            }
            else
            {
                // error handling
                //Debug.Log("Http Status Code: " + (int)rawResult.statusCode);
                //Debug.Log("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            // error handling
        }
        finally
        {
            //try
            //{
            //    RestApiHelper.Batch.GetPayment(currentPayment.userId, currentPayment.paymentId, this, TestBatchGetPaymentCallback);
            //    print("1");
            //    Payment responsePayment = result.GetFirstEntry();
            //    print("2");
            //    Debug.Log("Payment ID: " + responsePayment.paymentId);
            //    Debug.Log("Status: " + responsePayment.status);
            //    Debug.Log("Ordered Date and time: " + responsePayment.orderedTime);
            //    // more fields are available here. For example: responsePayment.paymentItems, which is List<PaymentItem>          
            //}
            //catch (Exception ex)
            //{
            //    print("urtimo error");
            //}
        }
    }

    public IEnumerator GetRequest(string uri)
    {
        print("ENTRO EN CORUTINA");
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    void MyPaymentDelegate(WebViewEvent result)
    {
        try
        {
            Debug.Log("Event: " + result.kind);
            Debug.Log("Result: " + result.value);
            Debug.Log("Message: " + result.message);
            switch (result.kind)
            {
                case WebViewEventKind.Succeeded:
                    // Call RestApiHelper.Batch.GetPayment(), and if the status is completed, award the user with the items
                    StartCoroutine(CrRequestAddPurchase());
                    break;

                case WebViewEventKind.Failed:
                    Debug.Log("Error during purchase");
                    break;

                case WebViewEventKind.Cancelled:
                    Debug.Log("User cancelled the purhcase");
                    break;
            }
        }
        catch (Exception ex)
        {

        }
    }

    IEnumerator CrRequestAddPurchase()
    {
        string nutakuID = PlayerPrefs.GetString("NutakuID");
        int price = PlayerPrefs.GetInt("LastPurchasePrice");
        UnityWebRequest www = UnityWebRequest.Get("http://s852714066.mialojamiento.es/tavernofsins/addpurchase.php?nutakuuserid="+nutakuID+ "&goldamount="+price);
        yield return www.SendWebRequest();
        _rewardManager.EarnHardCoin(_gemsToBuy);

    }
}
