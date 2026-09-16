using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

public class PublicStaticEndpointContainer
{
    public static class PublicEndpoints
    {
        [HttpGet("/static-access/public/public/public")]
        public static string Public() => "/static-access/public/public/public";

        [HttpGet("/static-access/public/public/internal")]
        internal static string Internal() => "/static-access/public/public/internal";

        [HttpGet("/static-access/public/public/private")]
        private static string Private() => "/static-access/public/public/private";

    }
    internal static class InternalEndpoints
    {
        [HttpGet("/static-access/public/internal/public")]
        public static string Public() => "/static-access/public/internal/public";

        [HttpGet("/static-access/public/internal/internal")]
        internal static string Internal() => "/static-access/public/internal/internal";

        [HttpGet("/static-access/public/internal/private")]
        private static string Private() => "/static-access/public/internal/private";

    }
    private static class PrivateEndpoints
    {
        [HttpGet("/static-access/public/private/public")]
        public static string Public() => "/static-access/public/private/public";

        [HttpGet("/static-access/public/private/internal")]
        internal static string Internal() => "/static-access/public/private/internal";

        [HttpGet("/static-access/public/private/private")]
        private static string Private() => "/static-access/public/private/private";

    }
    protected static class ProtectedEndpoints
    {
        [HttpGet("/static-access/public/protected/public")]
        public static string Public() => "/static-access/public/protected/public";

        [HttpGet("/static-access/public/protected/internal")]
        internal static string Internal() => "/static-access/public/protected/internal";

        [HttpGet("/static-access/public/protected/private")]
        private static string Private() => "/static-access/public/protected/private";

    }
    protected internal static class ProtectedInternalEndpoints
    {
        [HttpGet("/static-access/public/protected-internal/public")]
        public static string Public() => "/static-access/public/protected-internal/public";

        [HttpGet("/static-access/public/protected-internal/internal")]
        internal static string Internal() => "/static-access/public/protected-internal/internal";

        [HttpGet("/static-access/public/protected-internal/private")]
        private static string Private() => "/static-access/public/protected-internal/private";

    }
    private protected static class PrivateProtectedEndpoints
    {
        [HttpGet("/static-access/public/private-protected/public")]
        public static string Public() => "/static-access/public/private-protected/public";

        [HttpGet("/static-access/public/private-protected/internal")]
        internal static string Internal() => "/static-access/public/private-protected/internal";

        [HttpGet("/static-access/public/private-protected/private")]
        private static string Private() => "/static-access/public/private-protected/private";

    }
}

public static class PublicStaticEndpoints
{
    [HttpGet("/static-access/public/top-level/public")]
    public static string Public() => "/static-access/public/top-level/public";
    [HttpGet("/static-access/public/top-level/internal")]
    internal static string Internal() => "/static-access/public/top-level/internal";
    [HttpGet("/static-access/public/top-level/private")]
    private static string Private() => "/static-access/public/top-level/private";
}

internal class InternalStaticEndpointContainer
{
    public static class PublicEndpoints
    {
        [HttpGet("/static-access/internal/public/public")]
        public static string Public() => "/static-access/internal/public/public";

        [HttpGet("/static-access/internal/public/internal")]
        internal static string Internal() => "/static-access/internal/public/internal";

        [HttpGet("/static-access/internal/public/private")]
        private static string Private() => "/static-access/internal/public/private";

    }
    internal static class InternalEndpoints
    {
        [HttpGet("/static-access/internal/internal/public")]
        public static string Public() => "/static-access/internal/internal/public";

        [HttpGet("/static-access/internal/internal/internal")]
        internal static string Internal() => "/static-access/internal/internal/internal";

        [HttpGet("/static-access/internal/internal/private")]
        private static string Private() => "/static-access/internal/internal/private";

    }
    private static class PrivateEndpoints
    {
        [HttpGet("/static-access/internal/private/public")]
        public static string Public() => "/static-access/internal/private/public";

        [HttpGet("/static-access/internal/private/internal")]
        internal static string Internal() => "/static-access/internal/private/internal";

        [HttpGet("/static-access/internal/private/private")]
        private static string Private() => "/static-access/internal/private/private";

    }
    protected static class ProtectedEndpoints
    {
        [HttpGet("/static-access/internal/protected/public")]
        public static string Public() => "/static-access/internal/protected/public";

        [HttpGet("/static-access/internal/protected/internal")]
        internal static string Internal() => "/static-access/internal/protected/internal";

        [HttpGet("/static-access/internal/protected/private")]
        private static string Private() => "/static-access/internal/protected/private";

    }
    protected internal static class ProtectedInternalEndpoints
    {
        [HttpGet("/static-access/internal/protected-internal/public")]
        public static string Public() => "/static-access/internal/protected-internal/public";

        [HttpGet("/static-access/internal/protected-internal/internal")]
        internal static string Internal() => "/static-access/internal/protected-internal/internal";

        [HttpGet("/static-access/internal/protected-internal/private")]
        private static string Private() => "/static-access/internal/protected-internal/private";

    }
    private protected static class PrivateProtectedEndpoints
    {
        [HttpGet("/static-access/internal/private-protected/public")]
        public static string Public() => "/static-access/internal/private-protected/public";

        [HttpGet("/static-access/internal/private-protected/internal")]
        internal static string Internal() => "/static-access/internal/private-protected/internal";

        [HttpGet("/static-access/internal/private-protected/private")]
        private static string Private() => "/static-access/internal/private-protected/private";

    }
}

internal static class InternalStaticEndpoints
{
    [HttpGet("/static-access/internal/top-level/public")]
    public static string Public() => "/static-access/internal/top-level/public";
    [HttpGet("/static-access/internal/top-level/internal")]
    internal static string Internal() => "/static-access/internal/top-level/internal";
    [HttpGet("/static-access/internal/top-level/private")]
    private static string Private() => "/static-access/internal/top-level/private";
}

