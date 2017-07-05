using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace CompetencyMatrix.Infrastructure.Security
{
    /// <summary>
    /// This manager can verify passwords in AD only.
    /// </summary>
    public class ActiveDirectoryUserManager : UserManager<ApplicationUser>
    {
        private readonly IOptions<DomainOptions> _domainControllersOptions;

        public ActiveDirectoryUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IOptions<DomainOptions> domainControllersOptions,
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(
                store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
                services, logger)
        {
            _domainControllersOptions = domainControllersOptions;
        }

        protected override async Task<PasswordVerificationResult> VerifyPasswordAsync(
            IUserPasswordStore<ApplicationUser> store,
            ApplicationUser user,
            string password)
        {
            var result = await base.VerifyPasswordAsync(store, user, password);
            if (result == PasswordVerificationResult.Failed)
            {
                result = await VerifyPasswordInActiveDirectoryAsync(user, password);
            }
            return result;
        }

        private async Task<PasswordVerificationResult> VerifyPasswordInActiveDirectoryAsync(ApplicationUser user,
            string password)
        {
            return await Task.Run(() =>
            {
                var result = PasswordVerificationResult.Failed;

                foreach (var ldapHost in _domainControllersOptions.Value.DomainControllers)
                {
                    var conn = new LdapConnection();
                    try
                    {
                        conn.Connect(ldapHost, LdapConnection.DEFAULT_PORT);
                        conn.StartTls();
                        // authenticate to the server
                        conn.Bind(LdapConnection.Ldap_V3, user.UserName, password);
                        conn.Disconnect();
                        result = PasswordVerificationResult.Success;
                        break;
                    }
                    catch (LdapException e)
                    {
                        Logger.LogError(e.LdapErrorMessage);
                    }
                    catch (System.IO.IOException e)
                    {
                        Logger.LogError(e.Message);
                    }
                }

                return result;
            });
        }
    }
}