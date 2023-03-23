using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace JTM.IntegrationTests.Helpers
{
    public static class HttpCookieHelper
    {
        public static IRequestCookieCollection MockRequestCookieCollection(string key, string value)
        {
            var requestFeature = new HttpRequestFeature();
            var featureCollection = new FeatureCollection();

            requestFeature.Headers = new HeaderDictionary
            {
                { HeaderNames.Cookie, new StringValues(key + "=" + value) }
            };

            featureCollection.Set<IHttpRequestFeature>(requestFeature);

            var cookiesFeature = new RequestCookiesFeature(featureCollection);

            return cookiesFeature.Cookies;
        }
    }
}
