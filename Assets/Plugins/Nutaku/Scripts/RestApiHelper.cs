using System;
using System.Collections.Generic;
using UnityEngine;
using Nutaku.Unity.Utils;

namespace Nutaku.Unity
{
    public static class RestApiHelper
    {
        /// <summary>
        /// Timeout, in seconds. Default: 15s
        /// </summary>
        public static double timeoutSeconds { get; set; }

        /// <summary>
        /// Batch API (2-legged requests)
        /// </summary>
        public static class Batch
        {
            /// <summary>
            /// Retrieve the profile of the specified user.
            /// </summary>
            /// <param name="userId">ID of the user to acquire</param>
            /// <param name="queryParams"></param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetProfile(
                string userId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetProfile(userId, queryParams, OAuthKind.TwoLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the profile list of followers of a specified user.
            /// </summary>
            /// <param name="userId">The user who's follower list we are requesting</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for pagination and specifying the profile fields to be retrieved</param>
            public static void GetFriends(
                string userId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetFriends(userId, queryParams, OAuthKind.TwoLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the profile of a specific follower following the specified user, or null if there is no such relationship
            /// </summary>
            /// <param name="userId">ID of user being followed</param>
            /// <param name="friendUserId">ID of follower</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetFriend(
                string userId,
                string friendUserId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetFriend(userId, friendUserId, queryParams, OAuthKind.TwoLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the Payment object for a payment that has been registered on the server
            /// </summary>
            /// <param name="userId">ID of the user who made the payment</param>
            /// <param name="paymentId">ID of the payment</param>
            /// <param name="queryParams">Not relevant for this function</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetPayment(
                string userId,
                string paymentId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PaymentQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetPayment(userId, paymentId, queryParams, OAuthKind.TwoLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            public static void GetInspection(
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                params string[] textIds)
            {
                RestApiHelper.GetInspection(textIds, null as InspectionQueryParameterBuilder, OAuthKind.TwoLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            public static void PostMakeRequest(
                string callbackUrl,
                IEnumerable<KeyValuePair<string, string>> postData,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
            {
                RestApiHelper.PostMakeRequest(callbackUrl, postData, OAuthKind.TwoLegged, myMonoBehaviour, callbackFunctionDelegate);
            }
        }

        /// <summary>
        /// Request API (3-legged requests)
        /// </summary>
        public static class Request
        {
            /// <summary>
            /// Retrieve the profile of the specified user.
            /// </summary>
            /// <param name="userId">ID of the user to acquire</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetProfile(
                string userId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetProfile(userId, queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the profile list of followers of a specified user.
            /// </summary>
            /// <param name="userId">ID of user being followed</param>
            /// <param name="queryParams">Optional for pagination and specifying the profile fields to be retrieved</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetFriends(
                string userId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetFriends(userId, queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the profile of a specific follower following the specified user, or null if there is no such relationship
            /// </summary>
            /// <param name="userId">ID of user being followed by friendUserId</param>
            /// <param name="friendUserId">ID of user that is following userId</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetFriend(
                string userId,
                string friendUserId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetFriend(userId, friendUserId, queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the profile of the user currently playing the game
            /// </summary>
            public static void GetMyProfile(
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetMyProfile(queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the profile list of followers of the current user
            /// </summary>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetMyFriends(
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetMyFriends(queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Get the profile of a specific follower of the current user, or Null if that user is not following the current user
            /// </summary>
            /// <param name="friendUserId">ID of user that is following the current player</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void GetMyFriend(
                string friendUserId,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PeopleQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.GetMyFriend(friendUserId, queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            /// <summary>
            /// Initiates the payment creation process. Use the result object to continue with the payment flow.
            /// </summary>
            /// <param name="payment">The Payment object that contains all the payment creation information</param>
            /// <param name="queryParams">Not relevant for this function</param>
            /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
            /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
            /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
            public static void PostPayment(
                Payment payment,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                PaymentQueryParameterBuilder queryParams = null)
            {
                RestApiHelper.PostPayment(payment, queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            public static void GetInspection(
                IEnumerable<string> textIds,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                InspectionQueryParameterBuilder queryParams = null)
            {
                
                RestApiHelper.GetInspection(textIds, queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            public static void PutInspection(
                string textId,
                string text,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
            {
                RestApiHelper.PutInspection(textId, text, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            public static void PostInspection(
                string text,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
                InspectionQueryParameterBuilder queryParams = null)
            {

                RestApiHelper.PostInspection(text, queryParams, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

            public static void DeleteInspection(IEnumerable<string> textIds,
                MonoBehaviour myMonoBehaviour,
                UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
            {
                RestApiHelper.DeleteInspection(textIds, OAuthKind.ThreeLegged, myMonoBehaviour, callbackFunctionDelegate);
            }

        }

        /// <summary>
        /// Retrieve the profile of the specified user.
        /// </summary>
        static void GetProfile(
            string userId,
            PeopleQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            RestApi.GetPeople(
               userId,
               RestApi.Selector.Self,
               null,
               queryParams != null ? queryParams.Build() : null,
               oauthKind,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the profile list of followers of the specified user
        /// </summary>
        static void GetFriends(
            string userId,
            PeopleQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            RestApi.GetPeople(
               userId,
               RestApi.Selector.Friends,
               null,
               queryParams != null ? queryParams.Build() : null,
               oauthKind,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the profile of a specific follower of the specified user, or Null if that user is not following the specified user
        /// </summary>
        static void GetFriend(
            string userId,
            string friendUserId,
            PeopleQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            RestApi.GetPeople(
                userId,
                RestApi.Selector.Friends,
                friendUserId,
                queryParams != null ? queryParams.Build() : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        /// <summary>
        /// Retrieve the profile of the current user
        /// </summary>
        static void GetMyProfile(
            PeopleQueryParameterBuilder queryParams,
            OAuthKind oauthKind, MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            GetProfile(RestApi.Guid.Me, queryParams, oauthKind, myMonoBehaviour, callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the profile list of followers of the current user
        /// </summary>
        static void GetMyFriends(
            PeopleQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            GetFriends(RestApi.Guid.Me, queryParams, oauthKind, myMonoBehaviour, callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the profile of a specific follower of the current user, or Null if that user is not following the current user
        /// </summary>
        /// <param name="friendUserId"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        static void GetMyFriend(
            string friendUserId,
            PeopleQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            GetFriend(RestApi.Guid.Me, friendUserId, queryParams, oauthKind, myMonoBehaviour, callbackFunctionDelegate);
        }

        static void GetPayment(
            string userId,
            string paymentId,
            PaymentQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            RestApi.GetPayment(
            userId,
            RestApi.Selector.Self,
            RestApi.AppId.App,
            paymentId,
            queryParams != null ? queryParams.Build() : null,
            oauthKind,
            myMonoBehaviour,
            callbackFunctionDelegate);
        }

        static void PostPayment(
            Payment payment,
            PaymentQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            RestApi.PostPayment(
                RestApi.Guid.Me,
                RestApi.Selector.Self,
                RestApi.AppId.App,
                null,
                queryParams != null ? queryParams.Build() : null,
                payment,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        static void GetInspection(
            IEnumerable<string> textIds,
            InspectionQueryParameterBuilder queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (textIds == null)
                throw new ArgumentNullException("textIds");
            if (textIds.IEnumCount() == 0)
                throw new ArgumentException("textIds count must be > 0");
            RestApi.GetInspection(
                RestApi.AppId.App,
                string.Join(",", textIds.IEnumToArray()),
                queryParams != null ? queryParams.Build() : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        static void PutInspection(
            string textId,
            string text,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text must not be Null or Empty");

            RestApi.PutInspection(
                RestApi.AppId.App,
                textId,
                null,
                new { data = text },
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        static void PostInspection(
            string text,
            InspectionQueryParameterBuilder queryParams,
            OAuthKind oauthKind, MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text must not be Null or Empty");

            RestApi.PostInspection(
                RestApi.AppId.App,
                null,
                queryParams != null ? queryParams.Build() : null,
                new { data = text },
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        static void DeleteInspection(
            IEnumerable<string> textIds,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (textIds == null)
                throw new ArgumentNullException("textIds");
            if (textIds.IEnumCount() == 0)
                throw new ArgumentException("textIds count must be > 0");

            RestApi.DeleteInspection(
                RestApi.AppId.App,
                string.Join(",", textIds.IEnumToArray()),
                null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        static void PostMakeRequest(
            string callbackUrl,
            IEnumerable<KeyValuePair<string, string>> postData,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            PostMakeRequest("", callbackUrl, postData, oauthKind, myMonoBehaviour, callbackFunctionDelegate);
        }

        static void PostMakeRequest(
        string name,
        string callbackUrl,
        IEnumerable<KeyValuePair<string, string>> postData,
        OAuthKind oauthKind, MonoBehaviour myMonoBehaviour,
        UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            RestApi.PostMakeRequest(
               name,
               callbackUrl,
               postData,
               null,
               oauthKind,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        static RestApiHelper()
        {
            timeoutSeconds = 15.0;
        }
    }
}
