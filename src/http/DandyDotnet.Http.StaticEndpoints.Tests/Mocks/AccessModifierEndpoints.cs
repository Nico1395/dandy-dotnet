using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

public class PublicEndpointContainer
{
    public class PublicEndpoints
    {
        [HttpGet("/access/public/public/public")]
        public static string Public() => "/access/public/public/public";

        [HttpGet("/access/public/public/internal")]
        internal static string Internal() => "/access/public/public/internal";

        [HttpGet("/access/public/public/private")]
        private static string Private() => "/access/public/public/private";

        [HttpGet("/access/public/public/protected")]
        protected static string Protected() => "/access/public/public/protected";

        [HttpGet("/access/public/public/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/public/public/protected-internal";

        [HttpGet("/access/public/public/private-protected")]
        private protected static string PrivateProtected() => "/access/public/public/private-protected";

    }
    internal class InternalEndpoints
    {
        [HttpGet("/access/public/internal/public")]
        public static string Public() => "/access/public/internal/public";

        [HttpGet("/access/public/internal/internal")]
        internal static string Internal() => "/access/public/internal/internal";

        [HttpGet("/access/public/internal/private")]
        private static string Private() => "/access/public/internal/private";

        [HttpGet("/access/public/internal/protected")]
        protected static string Protected() => "/access/public/internal/protected";

        [HttpGet("/access/public/internal/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/public/internal/protected-internal";

        [HttpGet("/access/public/internal/private-protected")]
        private protected static string PrivateProtected() => "/access/public/internal/private-protected";

    }
    private class PrivateEndpoints
    {
        [HttpGet("/access/public/private/public")]
        public static string Public() => "/access/public/private/public";

        [HttpGet("/access/public/private/internal")]
        internal static string Internal() => "/access/public/private/internal";

        [HttpGet("/access/public/private/private")]
        private static string Private() => "/access/public/private/private";

        [HttpGet("/access/public/private/protected")]
        protected static string Protected() => "/access/public/private/protected";

        [HttpGet("/access/public/private/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/public/private/protected-internal";

        [HttpGet("/access/public/private/private-protected")]
        private protected static string PrivateProtected() => "/access/public/private/private-protected";

    }
    protected class ProtectedEndpoints
    {
        [HttpGet("/access/public/protected/public")]
        public static string Public() => "/access/public/protected/public";

        [HttpGet("/access/public/protected/internal")]
        internal static string Internal() => "/access/public/protected/internal";

        [HttpGet("/access/public/protected/private")]
        private static string Private() => "/access/public/protected/private";

        [HttpGet("/access/public/protected/protected")]
        protected static string Protected() => "/access/public/protected/protected";

        [HttpGet("/access/public/protected/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/public/protected/protected-internal";

        [HttpGet("/access/public/protected/private-protected")]
        private protected static string PrivateProtected() => "/access/public/protected/private-protected";

    }
    protected internal class ProtectedInternalEndpoints
    {
        [HttpGet("/access/public/protected-internal/public")]
        public static string Public() => "/access/public/protected-internal/public";

        [HttpGet("/access/public/protected-internal/internal")]
        internal static string Internal() => "/access/public/protected-internal/internal";

        [HttpGet("/access/public/protected-internal/private")]
        private static string Private() => "/access/public/protected-internal/private";

        [HttpGet("/access/public/protected-internal/protected")]
        protected static string Protected() => "/access/public/protected-internal/protected";

        [HttpGet("/access/public/protected-internal/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/public/protected-internal/protected-internal";

        [HttpGet("/access/public/protected-internal/private-protected")]
        private protected static string PrivateProtected() => "/access/public/protected-internal/private-protected";

    }
    private protected class PrivateProtectedEndpoints
    {
        [HttpGet("/access/public/private-protected/public")]
        public static string Public() => "/access/public/private-protected/public";

        [HttpGet("/access/public/private-protected/internal")]
        internal static string Internal() => "/access/public/private-protected/internal";

        [HttpGet("/access/public/private-protected/private")]
        private static string Private() => "/access/public/private-protected/private";

        [HttpGet("/access/public/private-protected/protected")]
        protected static string Protected() => "/access/public/private-protected/protected";

        [HttpGet("/access/public/private-protected/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/public/private-protected/protected-internal";

        [HttpGet("/access/public/private-protected/private-protected")]
        private protected static string PrivateProtected() => "/access/public/private-protected/private-protected";

    }
    [HttpGet("/access/public/top-level/public")]
    public static string Public() => "/access/public/top-level/public";
    [HttpGet("/access/public/top-level/internal")]
    internal static string Internal() => "/access/public/top-level/internal";
    [HttpGet("/access/public/top-level/private")]
    private static string Private() => "/access/public/top-level/private";
    [HttpGet("/access/public/top-level/protected")]
    protected static string Protected() => "/access/public/top-level/protected";
    [HttpGet("/access/public/top-level/protected-internal")]
    protected internal static string ProtectedInternal() => "/access/public/top-level/protected-internal";
    [HttpGet("/access/public/top-level/private-protected")]
    private protected static string PrivateProtected() => "/access/public/top-level/private-protected";
}

internal class InternalEndpointContainer
{
    public class PublicEndpoints
    {
        [HttpGet("/access/internal/public/public")]
        public static string Public() => "/access/internal/public/public";

        [HttpGet("/access/internal/public/internal")]
        internal static string Internal() => "/access/internal/public/internal";

        [HttpGet("/access/internal/public/private")]
        private static string Private() => "/access/internal/public/private";

        [HttpGet("/access/internal/public/protected")]
        protected static string Protected() => "/access/internal/public/protected";

        [HttpGet("/access/internal/public/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/internal/public/protected-internal";

        [HttpGet("/access/internal/public/private-protected")]
        private protected static string PrivateProtected() => "/access/internal/public/private-protected";

    }
    internal class InternalEndpoints
    {
        [HttpGet("/access/internal/internal/public")]
        public static string Public() => "/access/internal/internal/public";

        [HttpGet("/access/internal/internal/internal")]
        internal static string Internal() => "/access/internal/internal/internal";

        [HttpGet("/access/internal/internal/private")]
        private static string Private() => "/access/internal/internal/private";

        [HttpGet("/access/internal/internal/protected")]
        protected static string Protected() => "/access/internal/internal/protected";

        [HttpGet("/access/internal/internal/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/internal/internal/protected-internal";

        [HttpGet("/access/internal/internal/private-protected")]
        private protected static string PrivateProtected() => "/access/internal/internal/private-protected";

    }
    private class PrivateEndpoints
    {
        [HttpGet("/access/internal/private/public")]
        public static string Public() => "/access/internal/private/public";

        [HttpGet("/access/internal/private/internal")]
        internal static string Internal() => "/access/internal/private/internal";

        [HttpGet("/access/internal/private/private")]
        private static string Private() => "/access/internal/private/private";

        [HttpGet("/access/internal/private/protected")]
        protected static string Protected() => "/access/internal/private/protected";

        [HttpGet("/access/internal/private/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/internal/private/protected-internal";

        [HttpGet("/access/internal/private/private-protected")]
        private protected static string PrivateProtected() => "/access/internal/private/private-protected";

    }
    protected class ProtectedEndpoints
    {
        [HttpGet("/access/internal/protected/public")]
        public static string Public() => "/access/internal/protected/public";

        [HttpGet("/access/internal/protected/internal")]
        internal static string Internal() => "/access/internal/protected/internal";

        [HttpGet("/access/internal/protected/private")]
        private static string Private() => "/access/internal/protected/private";

        [HttpGet("/access/internal/protected/protected")]
        protected static string Protected() => "/access/internal/protected/protected";

        [HttpGet("/access/internal/protected/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/internal/protected/protected-internal";

        [HttpGet("/access/internal/protected/private-protected")]
        private protected static string PrivateProtected() => "/access/internal/protected/private-protected";

    }
    protected internal class ProtectedInternalEndpoints
    {
        [HttpGet("/access/internal/protected-internal/public")]
        public static string Public() => "/access/internal/protected-internal/public";

        [HttpGet("/access/internal/protected-internal/internal")]
        internal static string Internal() => "/access/internal/protected-internal/internal";

        [HttpGet("/access/internal/protected-internal/private")]
        private static string Private() => "/access/internal/protected-internal/private";

        [HttpGet("/access/internal/protected-internal/protected")]
        protected static string Protected() => "/access/internal/protected-internal/protected";

        [HttpGet("/access/internal/protected-internal/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/internal/protected-internal/protected-internal";

        [HttpGet("/access/internal/protected-internal/private-protected")]
        private protected static string PrivateProtected() => "/access/internal/protected-internal/private-protected";

    }
    private protected class PrivateProtectedEndpoints
    {
        [HttpGet("/access/internal/private-protected/public")]
        public static string Public() => "/access/internal/private-protected/public";

        [HttpGet("/access/internal/private-protected/internal")]
        internal static string Internal() => "/access/internal/private-protected/internal";

        [HttpGet("/access/internal/private-protected/private")]
        private static string Private() => "/access/internal/private-protected/private";

        [HttpGet("/access/internal/private-protected/protected")]
        protected static string Protected() => "/access/internal/private-protected/protected";

        [HttpGet("/access/internal/private-protected/protected-internal")]
        protected internal static string ProtectedInternal() => "/access/internal/private-protected/protected-internal";

        [HttpGet("/access/internal/private-protected/private-protected")]
        private protected static string PrivateProtected() => "/access/internal/private-protected/private-protected";

    }
    [HttpGet("/access/internal/top-level/public")]
    public static string Public() => "/access/internal/top-level/public";
    [HttpGet("/access/internal/top-level/internal")]
    internal static string Internal() => "/access/internal/top-level/internal";
    [HttpGet("/access/internal/top-level/private")]
    private static string Private() => "/access/internal/top-level/private";
    [HttpGet("/access/internal/top-level/protected")]
    protected static string Protected() => "/access/internal/top-level/protected";
    [HttpGet("/access/internal/top-level/protected-internal")]
    protected internal static string ProtectedInternal() => "/access/internal/top-level/protected-internal";
    [HttpGet("/access/internal/top-level/private-protected")]
    private protected static string PrivateProtected() => "/access/internal/top-level/private-protected";
}

file static class FileEndpoints
{
    [HttpGet("/access/file")]
    internal static string Execute() => "file";
}

file class FileRegularEndpoints
{
    [HttpGet("/file-access/regular/public")]
    public static string Public() => "/file-access/regular/public";
    [HttpGet("/file-access/regular/internal")]
    internal static string Internal() => "/file-access/regular/internal";
    [HttpGet("/file-access/regular/private")]
    private static string Private() => "/file-access/regular/private";
    [HttpGet("/file-access/regular/protected")]
    protected static string Protected() => "/file-access/regular/protected";
    [HttpGet("/file-access/regular/protected-internal")]
    protected internal static string ProtectedInternal() => "/file-access/regular/protected-internal";
    [HttpGet("/file-access/regular/private-protected")]
    private protected static string PrivateProtected() => "/file-access/regular/private-protected";
}

file static class FileStaticEndpoints
{
    [HttpGet("/file-access/static/public")]
    public static string Public() => "/file-access/static/public";
    [HttpGet("/file-access/static/internal")]
    internal static string Internal() => "/file-access/static/internal";
    [HttpGet("/file-access/static/private")]
    private static string Private() => "/file-access/static/private";
}
