namespace GeekLearning.AspNetCore.Identity.Localization
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Localization;
    using System.Reflection;

    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        private const string IdentityErrorResourceName = "IdentityErrorResource";
        private static AssemblyName webApplicationAssemblyName = new AssemblyName(Assembly.GetEntryAssembly().FullName);
        private static AssemblyName currentAssemblyName = new AssemblyName(typeof(LocalizedIdentityErrorDescriber).GetTypeInfo().Assembly.FullName);
        private readonly IStringLocalizer localizer;

        public LocalizedIdentityErrorDescriber(IStringLocalizerFactory factory)
        {
            var webLocalizer = factory.Create(IdentityErrorResourceName, webApplicationAssemblyName.Name);

            var firstChance = webLocalizer[nameof(DefaultError)];
            this.localizer = !firstChance.ResourceNotFound ? webLocalizer : factory.Create(IdentityErrorResourceName, currentAssemblyName.Name);
        }

        public override IdentityError DefaultError() => this.GetLocalizedIdentityError(nameof(DefaultError));

        public override IdentityError ConcurrencyFailure() => this.GetLocalizedIdentityError(nameof(ConcurrencyFailure));

        public override IdentityError PasswordMismatch() => this.GetLocalizedIdentityError(nameof(PasswordMismatch));

        public override IdentityError InvalidToken() => this.GetLocalizedIdentityError(nameof(InvalidToken));

        public override IdentityError LoginAlreadyAssociated() => this.GetLocalizedIdentityError(nameof(LoginAlreadyAssociated));

        public override IdentityError InvalidUserName(string userName) => this.GetLocalizedIdentityError(nameof(InvalidUserName), userName);

        public override IdentityError InvalidEmail(string email) => this.GetLocalizedIdentityError(nameof(InvalidEmail), email);

        public override IdentityError DuplicateUserName(string userName) => this.GetLocalizedIdentityError(nameof(DuplicateUserName), userName);

        public override IdentityError DuplicateEmail(string email) => this.GetLocalizedIdentityError(nameof(DuplicateEmail), email);

        public override IdentityError InvalidRoleName(string role) => this.GetLocalizedIdentityError(nameof(InvalidRoleName), role);

        public override IdentityError DuplicateRoleName(string role) => this.GetLocalizedIdentityError(nameof(DuplicateRoleName), role);

        public override IdentityError UserAlreadyHasPassword() => this.GetLocalizedIdentityError(nameof(UserAlreadyHasPassword));

        public override IdentityError UserLockoutNotEnabled() => this.GetLocalizedIdentityError(nameof(UserLockoutNotEnabled));

        public override IdentityError UserAlreadyInRole(string role) => this.GetLocalizedIdentityError(nameof(UserAlreadyInRole), role);

        public override IdentityError UserNotInRole(string role) => this.GetLocalizedIdentityError(nameof(UserNotInRole), role);

        public override IdentityError PasswordTooShort(int length) => this.GetLocalizedIdentityError(nameof(PasswordTooShort), length);

        public override IdentityError PasswordRequiresNonAlphanumeric() => this.GetLocalizedIdentityError(nameof(PasswordRequiresNonAlphanumeric));

        public override IdentityError PasswordRequiresDigit() => this.GetLocalizedIdentityError(nameof(PasswordRequiresDigit));

        public override IdentityError PasswordRequiresLower() => this.GetLocalizedIdentityError(nameof(PasswordRequiresLower));

        public override IdentityError PasswordRequiresUpper() => this.GetLocalizedIdentityError(nameof(PasswordRequiresUpper));

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => this.GetLocalizedIdentityError(nameof(PasswordRequiresUniqueChars), uniqueChars);

        public override IdentityError RecoveryCodeRedemptionFailed() => this.GetLocalizedIdentityError(nameof(RecoveryCodeRedemptionFailed));

        private IdentityError GetLocalizedIdentityError(string code, params object[] arguments)
            => new IdentityError
            {
                Code = code,
                Description = this.localizer[code, arguments],
            };
    }
}
