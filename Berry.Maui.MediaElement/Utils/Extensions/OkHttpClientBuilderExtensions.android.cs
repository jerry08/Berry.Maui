using System;
using Java.Security;
using Java.Security.Cert;
using Javax.Net.Ssl;
using Square.OkHttp3;

namespace Berry.Maui.Utils.Extensions;

internal class NaiveTrustManager : Java.Lang.Object, IX509TrustManager
{
    public void CheckClientTrusted(X509Certificate[]? chain, string? authType) { }

    public void CheckServerTrusted(X509Certificate[]? chain, string? authType) { }

    public X509Certificate[]? GetAcceptedIssuers() => Array.Empty<X509Certificate>();
}

// https://stackoverflow.com/questions/54419649/still-getting-trust-anchor-for-certification-path-not-found-after-getting-immedi
// https://stackoverflow.com/questions/52618113/exoplayer-how-to-validate-server-certificate
// https://medium.com/@kibotu/handling-custom-ssl-certificates-on-android-and-fixing-sslhandshakeexception-65ffb9dc612e
// https://github.com/brahmkshatriya/NiceHttp/blob/main/lib/src/main/kotlin/dev/brahmkshatriya/nicehttp/Utils.kt
internal static class OkHttpClientBuilderExtensions
{
    public static OkHttpClient.Builder IgnoreAllSSLErrors(this OkHttpClient.Builder builder)
    {
        var naiveTrustManager = new NaiveTrustManager();

        var insecureSocketFactory = SSLContext.GetInstance("SSL")!;
        var trustAllCerts = new ITrustManager[] { naiveTrustManager };
        insecureSocketFactory.Init(null, trustAllCerts, new SecureRandom());

        builder.SslSocketFactory(insecureSocketFactory.SocketFactory, naiveTrustManager);
        builder.HostnameVerifier((_, _) => true);

        return builder;
    }
}
