namespace GeekLearning.AspNetCore.FlashMessage.Internal
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.DataProtection;
    using System.Collections.Generic;
    using System.Linq;

    public class FlashMessageCookieMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDataProtector dataProtector;

        public FlashMessageCookieMiddleware(RequestDelegate next, IDataProtectionProvider dataProtectionProvider)
        {
            this.next = next;
            this.dataProtector = dataProtectionProvider.CreateProtector("GeekLearning.AspNetCore.FlashMessage");

            this.CookieName = "_FlashMessage";
            this.CookieSizeLimit = 1024 * 3;
            this.Secure = true;
        }

        public string CookieName { get; }

        public int CookieSizeLimit { get; }

        public bool Secure { get; }

        public async Task Invoke(HttpContext context)
        {
            var flashMessageManager = (FlashMessageManager)context.RequestServices.GetRequiredService<IFlashMessageManager>();

            var requestFlashMessages = this.Read(context);
            flashMessageManager.Incoming(requestFlashMessages);

            context.Response.OnStarting(() =>
            {
                var responseFlashMessages = flashMessageManager.Outgoing();
                this.Write(context, responseFlashMessages);
                return Task.FromResult(0);
            });

            await this.next.Invoke(context);
        }

        private IEnumerable<FlashMessage> Read(HttpContext context)
        {
            var cookie = context.Request.Cookies[this.CookieName];
            if (cookie == null)
            {
                return Enumerable.Empty<FlashMessage>();
            }

            var serializedMessages = this.dataProtector.Unprotect(Convert.FromBase64String(cookie));
            return FlashMessageStringSerializer.Deserialize(serializedMessages);
        }

        private void Write(HttpContext context, IEnumerable<FlashMessage> flashMessages)
        {
            var serializedMessages = FlashMessageStringSerializer.Serialize(flashMessages);
            var data = Convert.ToBase64String(this.dataProtector.Protect(serializedMessages));

            if (data.Length > this.CookieSizeLimit && this.CookieSizeLimit > 0)
            {
                throw new InvalidOperationException("The flash messages cookie size limit exceeded the limit value. Queue less messages.");
            }

            context.Response.Cookies.Append(this.CookieName, data);
        }
    }
}
