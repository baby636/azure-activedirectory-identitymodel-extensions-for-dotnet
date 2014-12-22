﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
// Apache License 2.0
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Xunit;

namespace System.IdentityModel.Test
{
    public class CreateAndValidateTokens
    {
        public class CreateAndValidateParams
        {
            public JwtSecurityToken CompareTo { get; set; }
            public Type ExceptionType { get; set; }
            public SigningCredentials SigningCredentials { get; set; }
            public SecurityKey SigningKey { get; set; }
            public TokenValidationParameters TokenValidationParameters { get; set; }
            public IEnumerable<Claim> Claims { get; set; }
            public string Case { get; set; }
            public string Issuer { get; set; }
        }

        private static string _roleClaimTypeForDelegate = "RoleClaimTypeForDelegate";
        private static string _nameClaimTypeForDelegate = "NameClaimTypeForDelegate";


        [Fact (DisplayName = "CreateAndValidateTokens: CreateAndValidateTokens_MultipleX5C")]
        public void CreateAndValidateTokens_MultipleX5C()
        {
            List<string> errors = new List<string>();
            var handler = new JwtSecurityTokenHandler();
            var payload = new JwtPayload();
            var header = new JwtHeader();

            payload.AddClaims(ClaimSets.MultipleAudiences(IdentityUtilities.DefaultIssuer, IdentityUtilities.DefaultIssuer));
            List<string> x5cs = new List<string> { "x5c1", "x5c2" };
            header.Add(JwtHeaderParameterNames.X5c, x5cs);
            var jwtToken = new JwtSecurityToken(header, payload);
            var jwt = handler.WriteToken(jwtToken);

            var validationParameters =
                new TokenValidationParameters
                {
                    RequireExpirationTime = false,
                    RequireSignedTokens = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                };

            SecurityToken validatedSecurityToken = null;
            var cp = handler.ValidateToken(jwt, validationParameters, out validatedSecurityToken);

            JwtSecurityToken validatedJwt = validatedSecurityToken as JwtSecurityToken;
            object x5csInHeader = validatedJwt.Header[JwtHeaderParameterNames.X5c];
            if (x5csInHeader == null)
            {
                errors.Add("1: validatedJwt.Header[JwtHeaderParameterNames.X5c]");
            }
            else
            {
                var list = x5csInHeader as IEnumerable<object>;
                if (list == null)
                {
                    errors.Add("2: var list = x5csInHeader as IEnumerable<object>; is NULL.");
                }

                int num = 0;
                foreach (var str in list)
                {
                    num++;
                    if (!(str is string))
                    {
                        errors.Add("3: str is not string, is:" + str.ToString());
                    }
                }

                if (num != x5cs.Count)
                {
                    errors.Add("4: num != x5cs.Count. num: " + num.ToString() + "x5cs.Count: " + x5cs.Count.ToString());
                }
            }

            // make sure we can still validate with existing logic.
            header = new JwtHeader(KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2);
            header.Add(JwtHeaderParameterNames.X5c, x5cs);
            jwtToken = new JwtSecurityToken(header, payload);
            jwt = handler.WriteToken(jwtToken);

            validationParameters.IssuerSigningKey = KeyingMaterial.DefaultX509Key_2048;
            validationParameters.RequireSignedTokens = true;
            validatedSecurityToken = null;
            cp = handler.ValidateToken(jwt, validationParameters, out validatedSecurityToken);

            TestUtilities.AssertFailIfErrors("CreateAndValidateTokens_MultipleX5C", errors);
        }

        [Fact(DisplayName = "CreateAndValidateTokens: EmptyToken, serialize and deserialze an empyt JWT")]
        public void EmptyToken()
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string jwt = handler.WriteToken(new JwtSecurityToken("", ""));
            JwtSecurityToken token = new JwtSecurityToken(jwt);
            Assert.True(IdentityComparer.AreEqual<JwtSecurityToken>(token, new JwtSecurityToken("", "")));
        }

        [Fact(DisplayName = "CreateAndValidateTokens: RoundTripTokens, serialize and deserialize using different claimsets")]
        public void RoundTripTokens()
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            CreateAndValidateParams createAndValidateParams;
            string issuer = "issuer";
            string originalIssuer = "originalIssuer";

            createAndValidateParams = new CreateAndValidateParams
            {
                Case = "ClaimSets.DuplicateTypes",
                Claims = ClaimSets.DuplicateTypes(issuer, originalIssuer),
                CompareTo = IdentityUtilities.CreateJwtSecurityToken(issuer, originalIssuer, ClaimSets.DuplicateTypes(issuer, originalIssuer), null),
                ExceptionType = null,
                TokenValidationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                }
            };

            RunRoundTrip(createAndValidateParams, handler);

            createAndValidateParams = new CreateAndValidateParams
            {
                Case = "ClaimSets.Simple_simpleSigned_Asymmetric",
                Claims = ClaimSets.Simple(issuer, originalIssuer),
                CompareTo = IdentityUtilities.CreateJwtSecurityToken(issuer, originalIssuer, ClaimSets.Simple(issuer, originalIssuer), KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2),
                ExceptionType = null,
                SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
                SigningKey = KeyingMaterial.DefaultX509Key_2048,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    IssuerSigningKey = new X509SecurityKey(KeyingMaterial.DefaultCert_2048),
                    ValidIssuer = issuer,
                }
            };

            RunRoundTrip(createAndValidateParams, handler);

#if SymmetricKeySuport
            createAndValidateParams = new CreateAndValidateParams
            {
                Case = "ClaimSets.Simple_simpleSigned_Symmetric",
                Claims = ClaimSets.Simple(issuer, originalIssuer),
                CompareTo = IdentityUtilities.CreateJwtSecurityToken(issuer, originalIssuer, ClaimSets.Simple(issuer, originalIssuer), KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2),
                ExceptionType = null,
                SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                SigningKey = KeyingMaterial.DefaultSymmetricSecurityKey_256,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    IssuerSigningKey = KeyingMaterial.DefaultSymmetricSecurityKey_256,
                    ValidIssuer = issuer,
                }
            };

            RunRoundTrip(createAndValidateParams, handler);
#endif
        }

        private void RunRoundTrip(CreateAndValidateParams jwtParams, JwtSecurityTokenHandler handler)
        {
            SecurityToken validatedToken;

            string jwt = handler.WriteToken(jwtParams.CompareTo);
            ClaimsPrincipal principal = handler.ValidateToken(jwt, jwtParams.TokenValidationParameters, out validatedToken);

            // create from security descriptor
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.SigningCredentials = jwtParams.SigningCredentials;
            tokenDescriptor.NotBefore = jwtParams.CompareTo.ValidFrom;
            tokenDescriptor.Expires    = jwtParams.CompareTo.ValidTo;
            tokenDescriptor.Claims     = jwtParams.Claims;
            tokenDescriptor.Issuer = jwtParams.CompareTo.Issuer;
            foreach (string str in jwtParams.CompareTo.Audiences)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    tokenDescriptor.Audience = str;
                }
            }


            JwtSecurityToken token = handler.CreateToken(
                issuer: tokenDescriptor.Issuer,
                audience: tokenDescriptor.Audience,
                expires: tokenDescriptor.Expires,
                notBefore: tokenDescriptor.NotBefore,
                subject: new ClaimsIdentity(tokenDescriptor.Claims),
                signingCredentials: tokenDescriptor.SigningCredentials ) as JwtSecurityToken;

            Assert.True(IdentityComparer.AreEqual(token, jwtParams.CompareTo), "!IdentityComparer.AreEqual( token, jwtParams.CompareTo )");

        }

        [Fact(DisplayName = "CreateAndValidateTokens: DuplicateClaims - roundtrips with duplicate claims")]
        public void CreateAndValidateTokens_DuplicateClaims()
        {
            SecurityToken validatedToken;
            string encodedJwt = IdentityUtilities.CreateJwtSecurityToken(
                new SecurityTokenDescriptor
                { 
                    Audience = IdentityUtilities.DefaultAudience,
                    SigningCredentials = IdentityUtilities.DefaultAsymmetricSigningCredentials,
                    Claims = ClaimSets.DuplicateTypes(IdentityUtilities.DefaultIssuer, IdentityUtilities.DefaultIssuer),
                    Issuer = IdentityUtilities.DefaultIssuer,
                });

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityTokenHandler.InboundClaimFilter.Add("aud");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("exp");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("iat");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("iss");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("nbf");

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(encodedJwt, IdentityUtilities.DefaultAsymmetricTokenValidationParameters, out validatedToken);

            var context = new CompareContext { IgnoreProperties = true, IgnoreSubject = true };
            if (!IdentityComparer.AreEqual<IEnumerable<Claim>>(claimsPrincipal.Claims, ClaimSets.DuplicateTypes(IdentityUtilities.DefaultIssuer, IdentityUtilities.DefaultIssuer), context))
                TestUtilities.AssertFailIfErrors("CreateAndValidateTokens: DuplicateClaims - roundtrips with duplicate claims", context.Diffs);

            JwtSecurityTokenHandler.InboundClaimFilter.Clear();
        }

        [Fact(DisplayName = "CreateAndValidateTokens: JsonClaims - claims values are objects serailized as json, can be recognized and reconstituted.")]
        public void CreateAndValidateTokens_JsonClaims()
        {
            List<string> errors = new List<string>();

            string issuer = "http://www.GotJWT.com";
            string claimSources = "_claim_sources";
            string claimNames = "_claim_names";

            JwtPayload jwtPayloadClaimSources = new JwtPayload();
            jwtPayloadClaimSources.Add(claimSources, JsonClaims.ClaimSources);
            jwtPayloadClaimSources.Add(claimNames, JsonClaims.ClaimNames);

            JwtSecurityToken jwtClaimSources = 
                new JwtSecurityToken(
                    new JwtHeader(),
                    jwtPayloadClaimSources);

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            string encodedJwt = jwtHandler.WriteToken(jwtClaimSources);
            var validationParameters =
                new TokenValidationParameters
                {
                    IssuerValidator = (s, st, tvp) => { return issuer;},
                    RequireSignedTokens = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };

            SecurityToken validatedJwt = null;
            var claimsPrincipal = jwtHandler.ValidateToken(encodedJwt, validationParameters, out validatedJwt);
            var context = CompareContext.Default;
            context.Title = "1";
            if (!IdentityComparer.AreEqual
                (claimsPrincipal.Identity as ClaimsIdentity,
                 JsonClaims.ClaimsIdentityDistributedClaims(issuer, TokenValidationParameters.DefaultAuthenticationType, JsonClaims.ClaimSources, JsonClaims.ClaimNames),
                 context))
            {
                errors.Add("JsonClaims.ClaimSources, JsonClaims.ClaimNames: test failed");
                errors.AddRange(context.Diffs);
            };

            Claim c = claimsPrincipal.FindFirst(claimSources);
            if (!c.Properties.ContainsKey(JwtSecurityTokenHandler.JsonClaimTypeProperty))
            {
                errors.Add(claimSources + " claim, did not have json property: " + JwtSecurityTokenHandler.JsonClaimTypeProperty);
            }
            else
            {
                //TODO - brentschmaltz, breaking this property change from commented to current
                //if (!string.Equals(c.Properties[JwtSecurityTokenHandler.JsonClaimTypeProperty], typeof(IDictionary<string, object>).ToString(), StringComparison.Ordinal))
                if (!string.Equals(c.Properties[JwtSecurityTokenHandler.JsonClaimTypeProperty], "Newtonsoft.Json.Linq.JProperty", StringComparison.Ordinal))
                {
                    errors.Add("!string.Equals(c.Properties[JwtSecurityTokenHandler.JsonClaimTypeProperty], typeof(IDictionary<string, object>).ToString(), StringComparison.Ordinal)" +
                        "value is: " + c.Properties[JwtSecurityTokenHandler.JsonClaimTypeProperty]);
                }
            }

            JwtSecurityToken jwtWithEntity =
                new JwtSecurityToken(
                    new JwtHeader(),
                    new JwtPayload(claims: ClaimSets.EntityAsJsonClaim(issuer, issuer)));

            encodedJwt = jwtHandler.WriteToken(jwtWithEntity);
            JwtSecurityToken jwtRead = jwtHandler.ReadToken(encodedJwt) as JwtSecurityToken;

            SecurityToken validatedToken;
            var cp = jwtHandler.ValidateToken(jwtRead.RawData, validationParameters, out validatedToken);
            Claim jsonClaim = cp.FindFirst(typeof(Entity).ToString());
            if (jsonClaim == null)
            {
                errors.Add("Did not find Jsonclaims. Looking for claim of type: '" + typeof(Entity).ToString() + "'");
            };

            string jsString = JsonExtensions.SerializeToJson(Entity.Default);

            if (!string.Equals(jsString, jsonClaim.Value, StringComparison.Ordinal))
            {
                errors.Add(string.Format(CultureInfo.InvariantCulture, "Find Jsonclaims of type: '{0}', but they weren't equal.\nExpecting:\n'{1}'.\nReceived:\n'{2}'", typeof(Entity).ToString(), jsString, jsonClaim.Value));
            }

            TestUtilities.AssertFailIfErrors("CreateAndValidateTokens_JsonClaims", errors);
        }

        [Fact(DisplayName = "CreateAndValidateTokens: SubClaim - is used the identity, when ClaimsIdentity.Name is called.")]
        public void CreateAndValidateTokens_SubClaim()
        {
        }

        private static string NameClaimTypeDelegate(SecurityToken jwt, string issuer)
        {
            return _nameClaimTypeForDelegate;
        }

        private static string RoleClaimTypeDelegate(SecurityToken jwt, string issuer)
        {
            return _roleClaimTypeForDelegate;
        }

        // TODO - brentsch, move to TokenValidationParameter tests.
        [Fact(DisplayName = "CreateAndValidateTokens: NameAndRoleClaimDelegates - name and role type delegates.")]
        public void CreateAndValidateTokens_NameAndRoleClaimDelegates()
        {
            string defaultName = "defaultName";
            string defaultRole = "defaultRole";
            string delegateName = "delegateName";
            string delegateRole = "delegateRole";
            string validationParameterName = "validationParameterName";
            string validationParameterRole = "validationParameterRole";
            string validationParametersNameClaimType = "validationParametersNameClaimType";
            string validationParametersRoleClaimType = "validationParametersRoleClaimType";

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = KeyingMaterial.DefaultX509Key_2048,
                NameClaimType = validationParametersNameClaimType,
                RoleClaimType = validationParametersRoleClaimType,
                ValidateAudience = false,
                ValidateIssuer = false,
            };

            ClaimsIdentity subject =
                new ClaimsIdentity(
                    new List<Claim> 
                    {   new Claim(_nameClaimTypeForDelegate, delegateName), 
                        new Claim(validationParametersNameClaimType, validationParameterName), 
                        new Claim(ClaimsIdentity.DefaultNameClaimType, defaultName), 
                        new Claim(_roleClaimTypeForDelegate, delegateRole),
                        new Claim(validationParametersRoleClaimType, validationParameterRole), 
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, defaultRole), 
                    });

            JwtSecurityToken jwt = handler.CreateToken(issuer: "https://gotjwt.com", signingCredentials: KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2, subject: subject) as JwtSecurityToken;

            // Delegates should override any other settings
            validationParameters.NameClaimTypeRetriever = NameClaimTypeDelegate;
            validationParameters.RoleClaimTypeRetriever = RoleClaimTypeDelegate;

            SecurityToken validatedToken;
            ClaimsPrincipal principal = handler.ValidateToken(jwt.RawData, validationParameters, out validatedToken);
            CheckNamesAndRole(new string[] { delegateName, defaultName, validationParameterName }, new string[] { delegateRole, defaultRole, validationParameterRole }, principal, _nameClaimTypeForDelegate, _roleClaimTypeForDelegate);

            // Set delegates to null will use TVP values
            validationParameters.NameClaimTypeRetriever = null;
            validationParameters.RoleClaimTypeRetriever = null;
            principal = handler.ValidateToken(jwt.RawData, validationParameters, out validatedToken);
            CheckNamesAndRole(new string[] { validationParameterName, defaultName, delegateName }, new string[] { validationParameterRole, defaultRole, delegateRole }, principal, validationParametersNameClaimType, validationParametersRoleClaimType);

            // check for defaults
            validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = KeyingMaterial.DefaultX509Key_2048,
                ValidateAudience = false,
                ValidateIssuer = false,
            };

            principal = handler.ValidateToken(jwt.RawData, validationParameters, out validatedToken);
            CheckNamesAndRole(new string[] { defaultName, validationParameterName, delegateName }, new string[] { defaultRole, validationParameterRole, delegateRole }, principal);
        }

        /// <summary>
        /// First string is expected, others are not.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="roles"></param>
        private void CheckNamesAndRole(string[] names, string[] roles, ClaimsPrincipal principal, string expectedNameClaimType = ClaimsIdentity.DefaultNameClaimType, string expectedRoleClaimType = ClaimsIdentity.DefaultRoleClaimType)
        {
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            Assert.Equal(identity.NameClaimType, expectedNameClaimType);
            Assert.Equal(identity.RoleClaimType, expectedRoleClaimType);
            Assert.True(principal.IsInRole(roles[0]));
            for (int i = 1; i < roles.Length; i++)
            {
                Assert.False(principal.IsInRole(roles[i]));
            }

            Assert.Equal(identity.Name, names[0]);
            for (int i = 1; i < names.Length; i++)
            {
                Assert.NotEqual(identity.Name, names[i]);
            }
        }
    }
}