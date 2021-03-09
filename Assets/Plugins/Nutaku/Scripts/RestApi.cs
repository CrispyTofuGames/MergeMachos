using Nutaku.Unity.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Nutaku.Unity
{
    public static class RestApi
    {
        private static string makeRequest_name;
        private static string makeRequest_callbackUrl;
        private static IEnumerable<KeyValuePair<string, string>> makeRequest_postData;
        private static IEnumerable<KeyValuePair<string, string>> makeRequest_queryParams;
        private static OAuthKind makeRequest_oauthKind;
        private static MonoBehaviour makeRequest_myMonoBehaviour;
        private static UnityWebRequestUtil.callbackFunctionDelegate makeRequest_callbackFunctionDelegate;
        private static OAuthParams makeRequest_oauthParams;

        public static class Guid
        {
            /// <summary>
            /// Indicate the ID of the Nutaku user who is running the game.
            /// </summary>
            public static readonly string Me = "@me";
        }

        public static class Selector
        {
            /// <summary>
            /// Indicates that the user's own profile information specified by the guid parameter is to be acquired.
            /// </summary>
            public static readonly string Self = "@self";

            /// <summary>
            /// Indicates that the friend information of the user specified by the guid parameter is to be acquired.
            /// </summary>
            public static readonly string Friends = "@friends";

            /// <summary>
            /// Same as Friends
            /// </summary>
            public static readonly string All = "@all";
        }

        public static class AppId
        {
            /// <summary>
            /// Indicates the ID of the game being executed.
            /// </summary>
            public static readonly string App = "@app";
        }

        /// <summary>
        /// People API.
        /// </summary>
        /// <param name="guid">Required. Guid.ME or a Nutaku user ID.</param>
        /// <param name="selector">Required. Selector.SELF, Selector.FRIENDS or Selector.ALL</param>
        /// <param name="pid">Optional Nutaku user ID, specified if selector is Selector.FRIENDS or Selector.ALL.</param>
        /// <param name="queryParams">Optional. Query parameters.</param>
        /// <param name="oauthKind">OAuth type</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        public static void GetPeople(
            string guid,
            string selector,
            string pid,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentException("guid must not be Null or Empty");
            if (string.IsNullOrEmpty(selector))
                throw new ArgumentException("selector must not be Null or Empty");
            if (oauthKind == OAuthKind.None)
                throw new ArgumentException("oauthKind must not be " + OAuthKind.None);

            Request<Person>(
                "GET",
                "people",
                new List<string> { guid, selector, pid },
                queryParams,
                null,
                (oauthKind == OAuthKind.ThreeLegged || guid == Guid.Me) ? SdkPlugin.loginInfo.userId : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void GetPayment(
            string guid,
            string selector,
            string appId,
            string paymentId,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentException("guid must not be Null or Empty");
            if (guid == Guid.Me)
                throw new ArgumentException("guid must not be " + Guid.Me);
            if (selector != Selector.Self)
                throw new ArgumentException("selector must be " + Selector.Self);
            if (appId != AppId.App)
                throw new ArgumentException("appId must be " + AppId.App);
            if (string.IsNullOrEmpty(paymentId))
                throw new ArgumentException("paymentId must not be Null or Empty");
            if (oauthKind != OAuthKind.TwoLegged)
                throw new ArgumentException("oauthKind must be " + OAuthKind.TwoLegged);

            Request<Payment>(
                "GET",
                "payment",
                new List<string> { guid, selector, appId, paymentId },
                queryParams,
                null,
                (oauthKind == OAuthKind.ThreeLegged || guid == Guid.Me) ? SdkPlugin.loginInfo.userId : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void PostPayment(
        string guid,
        string selector,
        string appId,
        string paymentId,
        IEnumerable<KeyValuePair<string, string>> queryParams,
        object payment,
        OAuthKind oauthKind,
        MonoBehaviour myMonoBehaviour,
        UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (guid != Guid.Me)
                throw new ArgumentException("guid must be " + Guid.Me);
            if (selector != Selector.Self)
                throw new ArgumentException("selector must be " + Selector.Self);
            if (appId != AppId.App)
                throw new ArgumentException("appId must be " + AppId.App);
            if (payment == null)
                throw new ArgumentNullException("payment");
            if (oauthKind != OAuthKind.ThreeLegged)
                throw new ArgumentException("oauthKind must be " + OAuthKind.ThreeLegged);

            Request<Payment>(
               "POST",
               "payment",
               new List<string> { guid, selector, appId, paymentId },
               queryParams,
               payment,
               (oauthKind == OAuthKind.ThreeLegged || guid == Guid.Me) ? SdkPlugin.loginInfo.userId : null,
               oauthKind,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        public static void GetInspection(
            string appId,
            string textId,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (appId != AppId.App)
                throw new ArgumentException("appId must be " + AppId.App);
            if (string.IsNullOrEmpty(textId))
                throw new ArgumentException("textId must not be Null or Empty");
            if (oauthKind == OAuthKind.None)
                throw new ArgumentException("oauthKind must not be " + OAuthKind.None);

            Request<UserText>(
                "GET",
                "inspection",
                new List<string> { appId, textId },
                queryParams,
                null,
                oauthKind == OAuthKind.ThreeLegged ? SdkPlugin.loginInfo.userId : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void PutInspection(
           string appId,
           string textId,
           IEnumerable<KeyValuePair<string, string>> queryParams,
           object userText,
           OAuthKind oauthKind,
           MonoBehaviour myMonoBehaviour,
           UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (appId != AppId.App)
                throw new ArgumentException("appId must be " + AppId.App);
            if (string.IsNullOrEmpty(textId))
                throw new ArgumentException("textId must not be Null or Empty");
            if (userText == null)
                throw new ArgumentNullException("userText");
            if (oauthKind != OAuthKind.ThreeLegged)
                throw new ArgumentException("oauthKind must be " + OAuthKind.ThreeLegged);

            Request<UserText>(
                "PUT",
                "inspection",
                new List<string> { appId, textId },
                queryParams,
                userText,
                oauthKind == OAuthKind.ThreeLegged ? SdkPlugin.loginInfo.userId : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void PostInspection(
            string appId,
            string textId,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            object userText,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (appId != AppId.App)
                throw new ArgumentException("appId must be " + AppId.App);
            if (userText == null)
                throw new ArgumentNullException("userText");
            if (oauthKind != OAuthKind.ThreeLegged)
                throw new ArgumentException("oauthKind must be " + OAuthKind.ThreeLegged);

            Request<UserText>(
                "POST",
                "inspection",
                new List<string> { appId, textId },
                queryParams,
                userText,
                oauthKind == OAuthKind.ThreeLegged ? SdkPlugin.loginInfo.userId : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void DeleteInspection(
            string appId,
            string textId,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (appId != AppId.App)
                throw new ArgumentException("appId must be " + AppId.App);
            if (oauthKind != OAuthKind.ThreeLegged)
                throw new ArgumentException("oauthKind must be " + OAuthKind.ThreeLegged);

            Request<UserText>(
                "DELETE",
                "inspection",
                new List<string> { appId, textId },
                queryParams,
                null,
                oauthKind == OAuthKind.ThreeLegged ? SdkPlugin.loginInfo.userId : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void PostMakeRequest(
        string name,
        string callbackUrl,
        IEnumerable<KeyValuePair<string, string>> postData,
        IEnumerable<KeyValuePair<string, string>> queryParams,
        OAuthKind oauthKind,
        MonoBehaviour myMonoBehaviour,
        UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(callbackUrl))
                throw new ArgumentException("callbackUrl must not be Null or Empty");
            if (oauthKind != OAuthKind.TwoLegged)
                throw new ArgumentException("oauthKind must be " + OAuthKind.TwoLegged);

            var settings = SdkPlugin.settings;
            var loginInfo = SdkPlugin.loginInfo;
            var accessToken = loginInfo.accessToken;
            var oauthParams = new OAuthParams()
            {
                consumerKey = settings.consumerKey,
                consumerSecret = settings.consumerSecret,
                accessToken = accessToken,
            };

            var securityToken = SdkPlugin.loginInfo.securityToken;

            RestApi.makeRequest_name = name;
            RestApi.makeRequest_callbackUrl = callbackUrl;
            RestApi.makeRequest_postData = postData;
            RestApi.makeRequest_queryParams = queryParams;
            RestApi.makeRequest_oauthKind = oauthKind;
            RestApi.makeRequest_myMonoBehaviour = myMonoBehaviour;
            RestApi.makeRequest_callbackFunctionDelegate = callbackFunctionDelegate;
            RestApi.makeRequest_oauthParams = oauthParams;

            if (securityToken.token == null || securityToken.dateTime.AddMinutes(SecurityToken.ValidDurationMinutes) <= DateTime.Now)
            {
                CoreApi.UserSt(
                   loginInfo.userId,
                   settings.consumerKey,
                   settings.consumerSecret,
                   myMonoBehaviour,
                   PostMakeRequestObtainSTCallback);
            }
            else
            {
                var endpoint = new ApiInfo(
                    SdkPlugin.socialApiInfo.scheme,
                    SdkPlugin.socialApiInfo.host,
                    "/gadgets/makeRequest"
                ).getUri();

                var postDataStrings = new List<string>();
                foreach (KeyValuePair<string, string> kvp in postData)
                    if (!string.IsNullOrEmpty(kvp.Key))
                        postDataStrings.Add(kvp.Key + "=" + kvp.Value);

                var bodyParams = new Dictionary<string, string>();
                bodyParams["name"] = name;
                bodyParams["url"] = callbackUrl;
                bodyParams["httpMethod"] = "POST";
                bodyParams["headers"] = "application/x-www-form-urlencoded";
                bodyParams["postData"] = string.Join("&", postDataStrings.ToArray());
                bodyParams["authz"] = "signed";
                bodyParams["st"] = SdkPlugin.loginInfo.securityToken.token;
                bodyParams["contentType"] = "JSON";
                bodyParams["numEntries"] = "3";
                bodyParams["getSummaries"] = "false";
                bodyParams["signOwner"] = "true";
                bodyParams["signViewer"] = "true";
                bodyParams["container"] = "nutaku";
                bodyParams["bypassSpecCache"] = "";
                bodyParams["getFullHeaders"] = "false";
                bodyParams["oauthState"] = "";

                var oauthSignaturePublicKey = SdkPlugin.settings.oauthSignaturePublicKey;
                if (oauthSignaturePublicKey != null)
                {
                    bodyParams["OAUTH_SIGNATURE_PUBLICKEY"] = oauthSignaturePublicKey;
                }

                var bodyStrings = new List<string>();
                foreach (KeyValuePair<string, string> kvp in bodyParams)
                    if (kvp.Value != null)
                        bodyStrings.Add(kvp.Key + "=" +
                                        (kvp.Key == "st" ? kvp.Value : Uri.EscapeDataString(kvp.Value)));

                var body = Encoding.UTF8.GetBytes(string.Join("&", bodyStrings.ToArray()));
                UnityEngine.Debug.Log("Aqui0");

                CoreApi.RawRequest(
                    "POST",
                    endpoint,
                    queryParams,
                    new Dictionary<string, string>() { { "Content-Type", "application/x-www-form-urlencoded" } },
                    body,
                    oauthKind,
                    oauthParams,
                    null,
                    myMonoBehaviour,
                    callbackFunctionDelegate);
            }
        }

        private static void PostMakeRequestObtainSTCallback(RawResult rawResult)
        {
            var updateSt = CoreApi.HandlePostCommandCallback<UserStResult>(rawResult);

            if (updateSt.code != "ok")
                throw new WebException("Failed to update security token. error_no:" + updateSt.error_no.ToString());
            SdkPlugin.UpdateSecurityToken(new SecurityToken(updateSt.result.st, DateTime.Now));

            var endpoint = new ApiInfo(
                   SdkPlugin.socialApiInfo.scheme,
                   SdkPlugin.socialApiInfo.host,
                   "/gadgets/makeRequest"
               ).getUri();

            var postDataStrings = new List<string>();
            foreach (KeyValuePair<string, string> kvp in makeRequest_postData)
                if (!string.IsNullOrEmpty(kvp.Key))
                    postDataStrings.Add(kvp.Key + "=" + kvp.Value);

            var bodyParams = new Dictionary<string, string>();
            bodyParams["name"] = makeRequest_name;
            bodyParams["url"] = makeRequest_callbackUrl;
            bodyParams["httpMethod"] = "POST";
            bodyParams["headers"] = "application/x-www-form-urlencoded";
            bodyParams["postData"] = string.Join("&", postDataStrings.ToArray());
            bodyParams["authz"] = "signed";
            bodyParams["st"] = SdkPlugin.loginInfo.securityToken.token;
            bodyParams["contentType"] = "JSON";
            bodyParams["numEntries"] = "3";
            bodyParams["getSummaries"] = "false";
            bodyParams["signOwner"] = "true";
            bodyParams["signViewer"] = "true";
            bodyParams["container"] = "nutaku";
            bodyParams["bypassSpecCache"] = "";
            bodyParams["getFullHeaders"] = "false";
            bodyParams["oauthState"] = "";

            var oauthSignaturePublicKey = SdkPlugin.settings.oauthSignaturePublicKey;
            if (oauthSignaturePublicKey != null)
            {
                bodyParams["OAUTH_SIGNATURE_PUBLICKEY"] = oauthSignaturePublicKey;
            }

            var bodyStrings = new List<string>();
            foreach (KeyValuePair<string, string> kvp in bodyParams)
                if (kvp.Value != null)
                    bodyStrings.Add(kvp.Key + "=" +
                                    (kvp.Key == "st" ? kvp.Value : Uri.EscapeDataString(kvp.Value)));

            var body = Encoding.UTF8.GetBytes(string.Join("&", bodyStrings.ToArray()));
            UnityEngine.Debug.Log("Aqui1");

            CoreApi.RawRequest(
                "POST",
                endpoint,
                makeRequest_queryParams,
                new Dictionary<string, string>() { { "Content-Type", "application/x-www-form-urlencoded" } },
                body,
                makeRequest_oauthKind,
                makeRequest_oauthParams,
                null,
                makeRequest_myMonoBehaviour,
                makeRequest_callbackFunctionDelegate);
        }

        public static MakeRequestResult HandlePostMakeRequestCallback
               (RawResult rawResult)
        {
            const string pattern = @"throw 1; < don't be evil' >{""(?:.+?)"":(?<json>{.+})}";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(Encoding.UTF8.GetString(rawResult.body));

            if (!match.Success)
                throw new WebException("TODO");

            var json = match.Groups["json"].Value;
            var result = JsonMapper.ToObject<MakeRequestResult>(json);
            result.statusCode = rawResult.statusCode;
            return result;
        }

        /// <summary>
        /// Request API to handle Request before sending to server
        /// </summary>
        /// <typeparam name="TResult">Result Type</typeparam>
        /// <param name="method">HTTP Method</param>
        /// <param name="apiEndpoint">API Endpoint</param>
        /// <param name="templateParams">Template parameters</param>
        /// <param name="queryParams">Query parameters</param>
        /// <param name="body">Request Body</param>
        /// <param name="requestorId">UserID for the OAuth header</param>
        /// <param name="oauthKind">OAuth kind</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        public static void Request<TResult>(
            string method,
            string apiEndpoint,
            IEnumerable<string> templateParams,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            object body,
            string requestorId,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            var basePath = SdkPlugin.socialApiInfo;
            var settings = SdkPlugin.settings;
            var accessToken = SdkPlugin.loginInfo.accessToken;
            var oauthParams = new OAuthParams()
            {
                consumerKey = settings.consumerKey,
                consumerSecret = settings.consumerSecret,
                accessToken = accessToken,
            };

            var builder = new UriBuilder();
            builder.Scheme = basePath.scheme;
            builder.Host = basePath.host;

            List<string> paths = new List<string>()
            {
                basePath.endpoint,
                apiEndpoint,
            };
            if (templateParams != null)
            {
                foreach (var templateParam in templateParams)
                    if (!string.IsNullOrEmpty(templateParam))
                        paths.Add(templateParam);
            }
            builder.Path = string.Join("/", paths.ToArray()).Replace("//", "/");

            Dictionary<string, string> additionalOAuthHeaderParams = null;
            if (!string.IsNullOrEmpty(requestorId))
            {
                additionalOAuthHeaderParams = new Dictionary<string, string>();
                additionalOAuthHeaderParams.Add("xoauth_requestor_id", requestorId);
            }

            CoreApi.RawRequest(
                method,
                builder.Uri.GetLeftPart(UriPartial.Path),
                queryParams,
                body == null ? null : new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                body == null ? null : Encoding.UTF8.GetBytes(JsonMapper.ToJson(body)),
                oauthKind,
                oauthParams,
                additionalOAuthHeaderParams,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static RestApiResult<TResult> HandleRequestCallback<TResult>(RawResult apiResult)
        {
            try
            {
                var json = Encoding.UTF8.GetString(apiResult.body);
                var root = JsonMapper.ToObject(json);
                RestApiResult<TResult> result;
                if (root.Keys.Contains("entry"))
                {
                    var entry = root["entry"];
                    if (entry.IsArray)
                        result = JsonMapper.ToObject<RestApiResult<TResult>>(json);
                    else if (entry.IsObject)
                        result = JsonMapper.ToObject<RestApiSimplexResult<TResult>>(json);
                    else
                        result = new RestApiResult<TResult>();
                }
                else
                    result = new RestApiResult<TResult>();
                result.statusCode = apiResult.statusCode;
                return result;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error:" + ex.StackTrace);
                throw ex;
            }
        }
    }
}
