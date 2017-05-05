using System;
using System.Collections.Specialized;
using R2D4Web.Enums;

namespace R2D4Web.Models
{
    public class Hub
    {
        private readonly NameValueCollection _query;
        public string VerifyToken => _query["hub.verify_token"];
        public HubMode Mode
        {
            get
            {
                HubMode mode;
                if (Enum.TryParse(_query["hub.mode"], out mode))
                {
                    return mode;
                }
                return HubMode.None;
            }
        }
        public int Challenge
        {
            get
            {
                int challenge;
                if (int.TryParse(_query["hub.challenge"], out challenge))
                {
                    return challenge;
                }
                return 0;
            }
        }

        public Hub(NameValueCollection query)
        {
            _query = query;
        }
    }
}