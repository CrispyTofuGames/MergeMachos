using Nutaku.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test class for Nutaku APIs
/// </summary>
public class TestUIController : MonoBehaviour
{
    public Text userIdText;
    public Text nicknameText;
    public Button clearAllLogsButton;
    public ScrollRect logViewScrollRect;
    public GameObject logViewItemPrefab;

    public CanvasGroup loadingView;

    public Button configGetButton;

    public Button batchGetProfileButton;
    public Button batchGetFriendsButton;
    public Button batchGetPaymentButton;
    public Button batchGetInspectionButton;
    public Button batchPostMakeRequestButton;

    public Button requestGetMyProfileButton;
    public Button requestGetMyFriendsButton;
    public Button requestPostPaymentButton;
    public Button requestGetInspectionButton;
    public Button requestPutInspectionButton;
    public Button requestPostInspectionButton;
    public Button requestDeleteInspectionButton;

    public Button openPaymentViewButton;

    Stack<Text> _textObjectCache = new Stack<Text>();

    Payment _payment = new Payment();
    List<Person> _myFriends = new List<Person>();
    Dictionary<string, UserText> _userTexts = new Dictionary<string, UserText>();

    void Awake()
    {
        SdkPlugin.Initialize();

        batchGetInspectionButton.interactable = true;
        requestGetInspectionButton.interactable = true;
        requestPutInspectionButton.interactable = false;
        requestDeleteInspectionButton.interactable = false;
        batchGetPaymentButton.interactable = false;
        openPaymentViewButton.interactable = false;

        clearAllLogsButton.onClick.AddListener(clearLogs);

        //Mobile APIs: Get Config
        configGetButton.onClick.AddListener(TestConfigGet);

        //Batch APIs: Get Profile
        batchGetProfileButton.onClick.AddListener(TestBatchGetProfile);

        //Batch APIs: Get Friends of current User
        batchGetFriendsButton.onClick.AddListener(TestBatchGetFriends);

        //Batch APIs: Get Payment Records
        batchGetPaymentButton.onClick.AddListener(TestBatchGetPayment);

        //Batch APIs: Get User Texts
        batchGetInspectionButton.onClick.AddListener(TestBatchGetInspection);

        //Batch APIs: Send Make Request
        batchPostMakeRequestButton.onClick.AddListener(TestBatchPostMakeRequest);

        //Request APIs: Get My Profile
        requestGetMyProfileButton.onClick.AddListener(TestRequestGetMyProfile);

        //Request APIs: Get Friends of current User 
        requestGetMyFriendsButton.onClick.AddListener(TestRequestGetMyFriends);

        //Request APIs: Post Payment
        requestPostPaymentButton.onClick.AddListener(TestRequestPostPayment);

        //Request APIs: Get User Texts
        requestGetInspectionButton.onClick.AddListener(TestRequestGetInspection);

        //Request APIs: Update User Texts
        requestPutInspectionButton.onClick.AddListener(TestRequestPutInspection);

        //Request APIs: Send User Texts
        requestPostInspectionButton.onClick.AddListener(TestRequestPostInspection);

        //Request APIs: Delete User Texts
        requestDeleteInspectionButton.onClick.AddListener(TestRequestDeleteInspection);

        //Web View: Open Payment View
        openPaymentViewButton.onClick.AddListener(TestOpenPaymentView);
    }

    void TestConfigGet()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("ConfigGet Start");
            CoreApi.GetConfig(SdkPlugin.settings.consumerKey, SdkPlugin.settings.consumerSecret, this, this.TestConfigGetCallback);
        }
        catch (Exception ex)
        {
            logException(ex);
        }
    }

    void TestConfigGetCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = CoreApi.HandlePostCommandCallback<ConfigGetResult>(rawResult);
                logMessage("ConfigGet Success");
                logMessage("code: " + result.code);
                logMessage("is_adult: " + result.result.is_adult.ToString());
                logMessage("maintenance - is_maintenance: " + result.result.maintenance.is_maintenance);
                logMessage("maintenance - message: " + result.result.maintenance.message);
                logMessage("version - code: " + result.result.version.code);
                logMessage("version - description: " + result.result.version.description);
                logMessage("version - is_force_update: " + result.result.version.is_force_update);
                logMessage("version - update_url: " + result.result.version.update_url);
            }
            else
            {
                logError("ConfigGet Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("ConfigGet Failure");
            logException(ex);
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestBatchGetProfile()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("BatchGetProfile Start");
            var userId = SdkPlugin.loginInfo.userId;
            logMessage("Get Profile of UserID: " + userId);
            RestApiHelper.Batch.GetProfile(userId, this, this.TestBatchGetProfileCallback);
        }
        catch (Exception ex)
        {
            logError("BatchGetProfile Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestBatchGetProfileCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("BatchGetProfile Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                var profile = result.GetFirstEntry();
                OutputPersonData(profile);
                userIdText.text = profile.id;
                nicknameText.text = profile.nickname;
            }
            else
            {
                logError("TestBatchGetProfile Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("TestBatchGetProfile Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestBatchGetFriends()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("BatchGetFriends Start");
            var userId = SdkPlugin.loginInfo.userId;
            logMessage("Get Friend list of UserID: " + userId);

            var queryParams = new PeopleQueryParameterBuilder();
            queryParams.count = 3;
            RestApiHelper.Batch.GetFriends(userId, this, this.TestBatchGetFriendsCallback, queryParams);
        }
        catch (Exception ex)
        {
            logError("BatchGetFriends Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestBatchGetFriendsCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("BatchGetFriends Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                _myFriends.Clear();
                foreach (var person in result.GetEntry())
                {
                    OutputPersonData(person);
                    _myFriends.Add(person);
                }
            }
            else
            {
                logError("BatchGetFriends Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("BatchGetFriends Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestBatchGetFriend()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("BatchGetFriend Start");

            var userId = SdkPlugin.loginInfo.userId;
            var friendUserId = CheckAndGetSomeFriendId();
            UnityEngine.Debug.LogFormat("Get Friend(ID:) of User(ID:)"+ userId+":"+friendUserId);
            logMessage(string.Format("Get Friend(ID:{1}) of User(ID:{0})",userId,friendUserId));
            RestApiHelper.Batch.GetFriend(userId, friendUserId, this, this.TestBatchGetFriendCallback);
        }
        catch (Exception ex)
        {
            logError("BatchGetFriend Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestBatchGetFriendCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("BatchGetFriend Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                var friend = result.GetFirstEntry();
                OutputPersonData(friend);
            }
            else
            {
                logError("BatchGetFriend Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("BatchGetFriend Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }
    
    void TestBatchGetPayment()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("BatchGetPayment Start");
            RestApiHelper.Batch.GetPayment(SdkPlugin.loginInfo.userId, CheckAndGetPayment().paymentId, this, this.TestBatchGetPaymentCallback);
        }
        catch (Exception ex)
        {
            logError("BatchGetPayment Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestBatchGetPaymentCallback(RawResult rawResult)
    {
        BeginLoading();
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Payment>(rawResult);
                logMessage("BatchGetPayment Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                var payment = result.GetFirstEntry();
                OutputPaymentData(payment);
            }
            else
            {
                logError("BatchGetPayment Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("BatchGetPayment Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestBatchGetInspection()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("BatchGetInspection Start");
            List<string> textIds = new List<string>();
            var texts = CheckAndGetUserTexts();
            foreach (UserText txt in texts)
                textIds.Add(txt.textId);
            RestApiHelper.Batch.GetInspection(this, this.TestBatchGetInspectionCallback, textIds.ToArray());
        }
        catch (Exception ex)
        {
            logError("BatchGetInspection Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestBatchGetInspectionCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<UserText>(rawResult);
                logMessage("BatchGetInspection Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                foreach (var userText in result.GetEntry())
                {
                    OutputUserTextData(userText);
                }
            }
            else
            {
                logError("BatchGetInspection Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("BatchGetInspection Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestBatchPostMakeRequest()
    {
        BeginLoading();
        const string TestName = "BatchPostMakeRequest";
        try
        {
            logMessage("");
            logMessage(TestName + " Start");
            var callbackUrl = "https://postman-echo.com/post"; //this sample URL should just echo back the details received from Nutaku's request
            var postData = new Dictionary<string, string>();
            postData.Add("login_check", "1");
            logMessage("Callback URL: " + callbackUrl);
            RestApiHelper.Batch.PostMakeRequest(callbackUrl, postData, this, this.TestBatchPostMakeRequestCallback);
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestBatchPostMakeRequestCallback(RawResult rawResult)
    {
        const string TestName = "BatchPostMakeRequest";
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandlePostMakeRequestCallback(rawResult);
                logMessage(TestName + " Success");
                logMessage("Http Status Code: " + result.statusCode);

                logMessage("rc: " + result.rc);
                logMessage("body: \n" + result.body);
                logMessage("headers:");
                foreach (var header in result.headers)
                {
                    logMessage(string.Format("\t{0}: {1}", header.Key, header.Value));
                }
            }
            else
            {
                logError("BatchPostMakeRequest Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestGetProfile()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestGetProfile Start");
            var userId = SdkPlugin.loginInfo.userId;
            logMessage("Get Profile Of UserID: " + userId);

            RestApiHelper.Request.GetProfile(userId, this, this.TestRequestGetProfileCallback);
        }
        catch (Exception ex)
        {
            logError("RequestGetProfile Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestGetProfileCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("RequestGetProfile Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                var profile = result.GetFirstEntry();
                OutputPersonData(profile);
                userIdText.text = profile.id;
                nicknameText.text = profile.nickname;
            }
            else
            {
                logError("RequestGetProfile Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestGetProfile Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestGetFriends()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestGetFriends Start");

            var userId = SdkPlugin.loginInfo.userId;
            logMessage("Get Friend list Of UserID: " + userId);

            var queryParams = new PeopleQueryParameterBuilder();
            queryParams.count = 3;
            RestApiHelper.Request.GetFriends(userId, this, this.TestRequestGetFriendsCallback, queryParams);
        }
        catch (Exception ex)
        {
            logError("RequestGetFriends Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestGetFriendsCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("RequestGetFriends Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                _myFriends.Clear();
                foreach (var person in result.GetEntry())
                {
                    OutputPersonData(person);
                    _myFriends.Add(person);
                }
            }
            else
            {
                logError("RequestGetFriends Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestGetFriends Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestGetFriend()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestGetFriend Start");
            var userId = SdkPlugin.loginInfo.userId;
            var friendUserId = CheckAndGetSomeFriendId();
            logMessage(string.Format("Get Friend(ID:{1}) of User(ID:{0})",userId,friendUserId));
            RestApiHelper.Request.GetFriend(userId, friendUserId, this, this.TestRequestGetFriendCallback);
        }
        catch (Exception ex)
        {
            logError("RequestGetFriend Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestGetFriendCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("RequestGetFriend Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);

                var friend = result.GetFirstEntry();
                OutputPersonData(friend);
            }
            else
            {
                logError("RequestGetFriend Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestGetFriend Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }


    void TestRequestGetMyProfile()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestGetMyProfile Start");
            logMessage(string.Format("Get Your(ID:{0}) Profile",SdkPlugin.loginInfo.userId));
            RestApiHelper.Request.GetMyProfile(this, this.TestRequestGetMyProfileCallback);
        }
        catch (Exception ex)
        {
            logError("RequestGetMyProfile Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestGetMyProfileCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("RequestGetMyProfile Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                var profile = result.GetFirstEntry();
                OutputPersonData(profile);
                userIdText.text = profile.id;
                nicknameText.text = profile.nickname;
            }
            else
            {
                logError("RequestGetMyProfile Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestGetMyProfile Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestGetMyFriends()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestGetMyFriends Start");
            logMessage(string.Format("Get Your(ID:{0}) Friend list", SdkPlugin.loginInfo.userId));
            RestApiHelper.Request.GetMyFriends(this, this.TestRequestGetMyFriendsCallback);
        }
        catch (Exception ex)
        {
            logError("RequestGetMyFriends Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestGetMyFriendsCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("RequestGetMyFriends Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                _myFriends.Clear();
                foreach (var person in result.GetEntry())
                {
                    OutputPersonData(person);
                    _myFriends.Add(person);
                }
            }
            else
            {
                logError("RequestGetMyFriends Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestGetFriends Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestGetMyFriend()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestGetMyFriend Start");
            var friendUserId = CheckAndGetSomeFriendId();
            logMessage(string.Format("Get Your(ID:{0}) Friend(ID:{1})",SdkPlugin.loginInfo.userId,friendUserId));
            RestApiHelper.Request.GetMyFriend(friendUserId, this, this.TestRequestGetMyFriendCallback);
        }
        catch (Exception ex)
        {
            logError("RequestGetMyFriend Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestGetMyFriendCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("RequestGetMyFriend Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);

                var friend = result.GetFirstEntry();
                OutputPersonData(friend);
            }
            else
            {
                logError("RequestGetMyFriend Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestGetMyFriend Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestPostPayment()
    {
        BeginLoading();
        try
        {
            Payment payment = new Payment();
            logMessage("");
            logMessage("RequestPostPayment Start");

            //This is just a documented shortcut available on sandbox
            payment.callbackUrl = "https://skip.payment.handler";

            payment.finishPageUrl = "http://www.nutaku.net/";
            payment.message = "Test Payment";

            PaymentItem item1 = new PaymentItem();
            item1.itemId = "1";
            item1.itemName = "Much Doge1";
            item1.unitPrice = 100;
            item1.quantity = 1;
            item1.imageUrl = "http://i.imgur.com/cBAPCQ4.png";
            item1.description = "item 001";
            payment.paymentItems.Add(item1);

            PaymentItem item2 = new PaymentItem();
            item2.itemId = "2";
            item2.itemName = "Much Doge2";
            item2.unitPrice = 500;
            item2.quantity = 2;
            item2.imageUrl = "https://dogecoin.com/imgs/dogecoin-300.png";
            item2.description = "item 002";
            payment.paymentItems.Add(item2);

            RestApiHelper.Request.PostPayment(payment, this, this.TestRequestPostPaymentCallback);
        }
        catch (Exception ex)
        {
            logError("RequestPostPayment Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestPostPaymentCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                Payment payment = new Payment();
                var result = RestApi.HandleRequestCallback<Payment>(rawResult);

                logMessage("RequestPostPayment Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);

                payment = result.GetFirstEntry();
                OutputPaymentData(payment);

                _payment = payment;
                batchGetPaymentButton.interactable = true;
                openPaymentViewButton.interactable = true;
            }
            else
            {
                logError("RequestPostPayment Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestPostPayment Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestGetInspection()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestGetInspection Start");

            var textIds = new List<string>();
            var userTexts = CheckAndGetUserTexts();
            foreach (UserText text in userTexts)
                textIds.Add(text.textId);
            RestApiHelper.Request.GetInspection(textIds, this, this.TestRequestGetInspectionCallback);
        }
        catch (Exception ex)
        {
            logError("RequestGetInspection Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestGetInspectionCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<UserText>(rawResult);

                logMessage("RequestGetInspection Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);

                foreach (var userText in result.GetEntry())
                {
                    OutputUserTextData(userText);
                }
            }
            else
            {
                logError("RequestGetInspection Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestGetInspection Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestPutInspection()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestPutInspection Start");
            RestApiHelper.Request.PutInspection(CheckAndGetSomeUserText().textId, "Changed User Texts "+Guid.NewGuid(), this, this.TestRequestPutInspectionCallback);
        }
        catch (Exception ex)
        {
            logError("RequestPutInspection Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestPutInspectionCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                logMessage("");
                logMessage("RequestPutInspection Start");

                var result = RestApi.HandleRequestCallback<UserText>(rawResult);

                logMessage("RequestPutInspection Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
            }
            else
            {
                logError("RequestPutInspection Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestPutInspection Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestPostInspection()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestPostInspection Start");

            RestApiHelper.Request.PostInspection("Test Of User Texts "+Guid.NewGuid(), this, this.TestRequestPostInspectionCallback);
        }
        catch (Exception ex)
        {
            logError("RequestPostInspection Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestPostInspectionCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<UserText>(rawResult);
                logMessage("RequestPostInspection Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);

                var userText = result.GetFirstEntry();
                OutputUserTextData(userText);

                _userTexts[userText.textId] = userText;
                requestPutInspectionButton.interactable = true;
                requestDeleteInspectionButton.interactable = true;
            }
            else
            {
                logError("RequestPostInspection Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestPostInspection Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestRequestDeleteInspection()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("RequestDeleteInspection Start");

            var userTexts = CheckAndGetUserTexts();
            var countToDelete = Math.Min(3, userTexts.Count);
            List<string> textIdsToDelete = new List<string>();

            for (var i = 0; i < countToDelete; i++)
                textIdsToDelete.Add(userTexts[i].textId);

            RestApiHelper.Request.DeleteInspection(textIdsToDelete, this, this.TestRequestDeleteInspectionCallback);
            foreach (string textId in textIdsToDelete)
                _userTexts.Remove(textId);
        }
        catch (Exception ex)
        {
            logError("RequestDeleteInspection Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestRequestDeleteInspectionCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<UserText>(rawResult);

                if (_userTexts.Count == 0)
                {
                    requestPutInspectionButton.interactable = false;
                    requestDeleteInspectionButton.interactable = false;
                }

                logMessage("RequestDeleteInspection Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
            }
            else
            {
                logError("RequestDeleteInspection Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("RequestDeleteInspection Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestOpenPaymentView()
    {
        BeginLoading();
        const string TestName = "TestOpenPaymentView";
        try
        {
            logMessage("");
            logMessage(TestName + " Start");

            SdkPlugin.OpenPaymentView(CheckAndGetPayment(), TestPaymentDelegate);
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestPaymentDelegate(WebViewEvent result)
    {
        const string TestName = "TestParsePaymentResult";
        try
        {
            logMessage(TestName + " Success");
            logMessage("Event: " + result.kind);
            logMessage("Result: " + result.value);
            logMessage("Message: " + result.message);
            if (result.kind == WebViewEventKind.Succeeded)
            {
                _payment = null;
                openPaymentViewButton.interactable = false;
            }
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void DumpError(Exception ex)
    {
        var webEx = ex as WebException;
        if (webEx != null)
        {
            var response = webEx.Response as HttpWebResponse;
            if (response != null)
            {
                logError("Http Status Code: " + (int)response.StatusCode);
            }
        }
        logException(ex);
    }

    class TestException : Exception
    {
        public TestException(string message)
            : base(message)
        {
        }

        public TestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    List<string> CheckAndGetFriendIds()
    {
        if (_myFriends.Count == 0)
        {
            throw new TestException("Friend list is not acquired or you have no friends.");
        }

        var friendIds = new List<string>();
        foreach (Person friend in _myFriends)
            friendIds.Add(friend.id);

        return friendIds;
    }

    string CheckAndGetSomeFriendId()
    {
        var personIds = CheckAndGetFriendIds();
        return personIds[personIds.Count - 1];
    }

    Payment CheckAndGetPayment()
    {
        if (_payment == null)
        {
            batchGetPaymentButton.interactable = false;
            openPaymentViewButton.interactable = false;
            throw new TestException("Payment is not registered");
        }

        return _payment;
    }

    List<UserText> CheckAndGetUserTexts()
    {
        if (_userTexts.Count == 0)
        {
            requestPutInspectionButton.interactable = false;
            requestDeleteInspectionButton.interactable = false;
            throw new TestException("No User Text is registered");
        }

        return new List<UserText>(_userTexts.Values);
    }

    UserText CheckAndGetSomeUserText()
    {
        var userTexts = CheckAndGetUserTexts();
        return userTexts[userTexts.Count - 1];
    }

    void BeginLoading()
    {
        loadingView.gameObject.SetActive(true);
    }

    void EndLoading()
    {
        loadingView.gameObject.SetActive(false);
    }

    void OutputPersonData(Person person)
    {
        logMessage("id: " + person.id);
        logMessage("nickname: " + person.nickname);
        logMessage("displayName: " + person.displayName);
        logMessage("profileUrl: " + person.profileUrl);
        logMessage("thumbnailUrl: " + person.thumbnailUrl);
        logMessage("thumbnailUrlSmall: " + person.thumbnailUrlSmall);
        logMessage("thumbnailUrlLarge: " + person.thumbnailUrlLarge);
        logMessage("thumbnailUrlHuge: " + person.thumbnailUrlHuge);
        logMessage("aboutMe: " + person.aboutMe);
        logMessage("birthday: " + person.birthday);
        logMessage("gender: " + person.gender);
        logMessage("age: " + person.age);
        logMessage("addresses (country): " + person.addresses.country);
        logMessage("hasApp: " + person.hasApp);
        logMessage("userType: " + person.userType);
        logMessage("grade: " + person.grade);
        logMessage("languagesSpoken: " + person.languagesSpoken);
    }

    void OutputUserTextData(UserText userText)
    {
        logMessage("textId: " + userText.textId);
        logMessage("appId: " + userText.appId);
        logMessage("authorId: " + userText.authorId);
        logMessage("ownerId: " + userText.ownerId);
        logMessage("data: " + userText.data);
        logMessage("status: " + userText.status);
        logMessage("ctime: " + userText.ctime);
        logMessage("mtime: " + userText.mtime);
    }

    void OutputPaymentData(Payment payment)
    {
        logMessage("paymentId: " + payment.paymentId);
        logMessage("appId: " + payment.appId);
        logMessage("userId: " + payment.userId);
        logMessage("status: " + payment.status);
        logMessage("callbackUrl: " + payment.callbackUrl);
        logMessage("finishPageUrl: " + payment.finishPageUrl);
        logMessage("transactionUrl: " + payment.transactionUrl);
        logMessage("message: " + payment.message);
        logMessage("orderedTime: " + payment.orderedTime);
        logMessage("executedTime: " + payment.executedTime);

        foreach (var items in payment.paymentItems)
        {
            logMessage("\titemId: " + items.itemId);
            logMessage("\titemName: " + items.itemName);
            logMessage("\tunitPrice: " + items.unitPrice);
            logMessage("\tquantity: " + items.quantity);
            logMessage("\timageUrl: " + items.imageUrl);
            logMessage("\tdescription: " + items.description);
        }
    }

    void clearLogs()
    {
        foreach (RectTransform child in logViewScrollRect.content)
        {
            child.gameObject.SetActive(false);
            _textObjectCache.Push(child.gameObject.GetComponent<Text>());
        }
    }

    void logMessage(string message, LogType logType = LogType.Log)
    {
        Text text;
        if (_textObjectCache.Count > 0)
        {
            text = _textObjectCache.Pop();
            text.rectTransform.SetSiblingIndex(text.rectTransform.parent.childCount - 1);
            text.gameObject.SetActive(true);
        }
        else
        {
            text = (Instantiate(logViewItemPrefab) as GameObject).GetComponent<Text>();
        }
        text.text = message;
#if UNITY_ANDROID && !UNITY_EDITOR
        text.fontSize = 2;
#endif

        switch (logType)
        {
            case LogType.Exception:
                text.color = Color.magenta;
                break;

            case LogType.Warning:
                text.color = Color.yellow;
                break;

            case LogType.Error:
                text.color = Color.red;
                break;

            case LogType.Assert:
                text.color = Color.red;
                break;

            default:
                text.color = Color.white;
                break;
        }

        text.rectTransform.SetParent(logViewScrollRect.content, false);
        logViewScrollRect.normalizedPosition = new Vector2(0f, 0f);
    }

    void logError(string message)
    {
        logMessage("Error: " + message, LogType.Error);
    }

    void logException(Exception ex)
    {
        if (ex.StackTrace == null)
            logMessage("Exception: " + ex.Message, LogType.Exception);
        else
            logMessage("Exception: " + ex.Message + ".\r\nStack trace: " + ex.StackTrace, LogType.Exception);
    }
}
