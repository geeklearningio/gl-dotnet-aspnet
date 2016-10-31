namespace GeekLearning.AspNetCore.Semver.Internal
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Primitives;
    using System.Threading.Tasks;

    public class SemverMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IOptions<SemverOptions> options;

        public SemverMiddleware(RequestDelegate next, IOptions<SemverOptions> options)
        {
            this.next = next;
            this.options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(
                state =>
                {
                    var httpContext = (HttpContext)state;
                    httpContext.Response.Headers.Add("X-SemVer", new StringValues(this.options.Value.SemVer));
                    return Task.FromResult(0);
                },
                context);

            await this.next.Invoke(context);
        }
    }
}
