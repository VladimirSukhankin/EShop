using System.Globalization;

namespace EShopFanerum.Auth.WebApi.Helpers;

public class WbApiException: Exception
{
        public WbApiException() : base() {}

        public WbApiException(string message) : base(message) { }

        public WbApiException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
}