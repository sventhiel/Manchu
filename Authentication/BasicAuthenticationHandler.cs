﻿using LiteDB;
using Manchu.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Manchu.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private ConnectionString _connectionString;
        private List<Admin> _admins;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration,
            ConnectionString connectionString) : base(options, logger, encoder, clock)
        {
            _connectionString = connectionString;
            _admins = configuration.GetSection("Admins").Get<List<Admin>>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Basic ".Length).Trim();
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');

                //
                // Admin
                var admin = _admins.Find(a => a.Name.Equals(credentials[0]));

                if (admin != null && admin.Password.Equals(credentials[1]))
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, credentials[0]),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var identity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
                }

                Response.StatusCode = 401;
                Response.Headers.Add("www-authenticate", "Basic Authorization");
                return AuthenticateResult.Fail(new UnauthorizedAccessException());
            }
            else
            {
                Response.StatusCode = 401;
                Response.Headers.Add("www-authenticate", "Basic Authorization");
                return AuthenticateResult.Fail(new UnauthorizedAccessException());

            }
        }
    }
}