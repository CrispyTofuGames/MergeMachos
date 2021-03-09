﻿namespace Nutaku.Unity
{
    /// <summary>
    /// 'Login'の結果を格納する構造体.
    /// </summary>
    public struct LoginResult
    {
        public string code;
        public int error_no;
        public string error_message;
        public Result result;


        public struct Result
        {
            public string session_id;
            public string autologin_token;
            public string oauth_token;
            public string oauth_token_secret;
            public string user_id;
        }
    }
}
