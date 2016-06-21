namespace GeekLearning.AspNetCore.FlashMessage
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.DataProtection;
    using System.Collections.Generic;
    using System.Linq;

    public class FlashMessageMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IServiceProvider serviceProvider;
        private readonly IDataProtector dataProtector;

        public FlashMessageMiddleware(RequestDelegate next, IServiceProvider serviceProvider, IDataProtectionProvider dataProtectionProvider)
        {
            this.next = next;
            this.serviceProvider = serviceProvider;
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
            var flashMessageManager = this.serviceProvider.GetService<IFlashMessageManager>();

            var requestFlashMessages = this.Read(context);
            flashMessageManager.Init(requestFlashMessages);

            await this.next.Invoke(context);

            var responseFlashMessages = flashMessageManager.Get();
            this.Write(context, responseFlashMessages);
        }

        private IEnumerable<FlashMessageModel> Read(HttpContext context)
        {
            var cookie = context.Request.Cookies[this.CookieName];
            if (cookie == null)
            {
                return Enumerable.Empty<FlashMessageModel>();
            }

            var serializedMessages = this.dataProtector.Unprotect(Convert.FromBase64String(cookie));
            return FlashMessage.Deserialize(serializedMessages);
        }

        private void Write(HttpContext context, IEnumerable<FlashMessageModel> flashMessages)
        {
            var serializedMessages = FlashMessage.Serialize(flashMessages);
            var data = Convert.ToBase64String(this.dataProtector.Protect(serializedMessages));

            if (data.Length > this.CookieSizeLimit && this.CookieSizeLimit > 0)
            {
                throw new InvalidOperationException("The flash messages cookie size limit exceeded the limit value. Queue less messages.");
            }

            context.Response.Cookies.Append(this.CookieName, data);
        }
    }
}
