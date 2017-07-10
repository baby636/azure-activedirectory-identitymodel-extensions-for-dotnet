﻿//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Tests
{
    public class XmlEement
    {
        public XmlEement(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public object Value { get; }

        public static string Generate(XmlEement dataElement)
        {
            var stringBuilder = new StringBuilder($"<{dataElement.Name}>");
            if (dataElement.Value is string str)
                stringBuilder.Append(str);
            else if (dataElement.Value is XmlEement element)
                stringBuilder.Append(Generate(element));
            else if (dataElement.Value is List<XmlEement> elements)
                foreach (var ele in elements)
                    stringBuilder.Append(Generate(ele));
            else
                throw new TestException($"dataElement.Value must be of type: '{typeof(string)}' or '{typeof(XmlEement)} or '{typeof(List<XmlEement>)}' was: {dataElement.Value.GetType()}.");

            stringBuilder.Append($"</{dataElement.Name}>");
            return stringBuilder.ToString();
        }
    }

    public static class XmlGenerator
    {
        public static string ClaimTypesOffered
        {
            get => "<fed:ClaimTypesOffered><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Name</auth:DisplayName><auth:Description>The mutable display name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Subject</auth:DisplayName><auth:Description>An immutable, globally unique, non-reusable identifier of the user that is unique to the application for which a token is issued.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Given Name</auth:DisplayName><auth:Description>First name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Surname</auth:DisplayName><auth:Description>Last name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/displayname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Display Name</auth:DisplayName><auth:Description>Display name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/nickname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Nick Name</auth:DisplayName><auth:Description>Nick name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Authentication Instant</auth:DisplayName><auth:Description>The time (UTC) when the user is authenticated to Windows Azure Active Directory.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Authentication Method</auth:DisplayName><auth:Description>The method that Windows Azure Active Directory uses to authenticate users.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/objectidentifier\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>ObjectIdentifier</auth:DisplayName><auth:Description>Primary identifier for the user in the directory. Immutable, globally unique, non-reusable.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/tenantid\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>TenantId</auth:DisplayName><auth:Description>Identifier for the user's tenant.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/identityprovider\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>IdentityProvider</auth:DisplayName><auth:Description>Identity provider for the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Email</auth:DisplayName><auth:Description>Email address of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/groups\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Groups</auth:DisplayName><auth:Description>Groups of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/accesstoken\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>External Access Token</auth:DisplayName><auth:Description>Access token issued by external identity provider.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>External Access Token Expiration</auth:DisplayName><auth:Description>UTC expiration time of access token issued by external identity provider.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/openid2_id\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>External OpenID 2.0 Identifier</auth:DisplayName><auth:Description>OpenID 2.0 identifier issued by external identity provider.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/claims/groups.link\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>GroupsOverageClaim</auth:DisplayName><auth:Description>Issued when number of user's group claims exceeds return limit.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/role\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Role Claim</auth:DisplayName><auth:Description>Roles that the user or Service Principal is attached to</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/wids\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>RoleTemplate Id Claim</auth:DisplayName><auth:Description>Role template id of the Built-in Directory Roles that the user is a member of</auth:Description></auth:ClaimType></fed:ClaimTypesOffered>";
        }

        public static string IDPSSODescriptorTemplate
        {
            get => "<IDPSSODescriptor protocolSupportEnumeration=\"urn:oasis:names:tc:SAML:2.0:protocol\">{0}</IDPSSODescriptor>";
        }

        public static string EntityDescriptorTemplate
        {
            get => "<?xml version=\"1.0\" encoding=\"utf-8\"?><EntityDescriptor ID=\"{0}\" entityID=\"{1}\" xmlns=\"{2}\">{3}</EntityDescriptor>";
        }

        public static string KeyDescriptorTemplate
        {
            get => "<KeyDescriptor use=\"{0}\">{1}</KeyDescriptor>";
        }

        public static string KeyDescriptorXml(KeyInfo keyInfo)
        {
            return string.Format(KeyDescriptorTemplate, XmlSignatureConstants.Namespace, Generate(keyInfo));
        }

        public static string KeyDescriptorXml(string @namespace, string keyInfo)
        {
            return string.Format(KeyDescriptorTemplate, @namespace, keyInfo);
        }

        public static string KeyInfoTemplate
        {
            get => "<KeyInfo xmlns=\"{0}\">{1}</KeyInfo>";
        }

        public static string KeyInfoXml(string @namespace, XmlEement x509Data)
        {
            return string.Format(KeyInfoTemplate, @namespace, XmlEement.Generate(x509Data));
        }

        public static string NotBeforeXml(string notBefore)
        {
            if (string.IsNullOrEmpty(notBefore))
                return string.Empty;

            return $"NotBefore = \"{notBefore}\"";
        }

        public static string NotOnOrAfterXml(string notOnOrAfter)
        {
            if (string.IsNullOrEmpty(notOnOrAfter))
                return string.Empty;

            return $"NotOnOrAfter = \"{notOnOrAfter}\"";
        }


        public static string SamlActionTemplate
        {
            get => "<Action Namespace=\"{0}\" xmlns=\"{1}\">{2}</Action>";
        }

        public static string SamlActionXml(string @namespace, string actionNamespace, string action)
        {
            return string.Format(SamlActionTemplate, actionNamespace, @namespace, action);
        }

        public static string SamlAttributeValueTemplate
        {
            get => "<AttributeValue xmlns=\"{0}\">{1}</AttributeValue>";
        }

        public static string SamlAttributeValueXml(string @namespace, string value)
        {
            return string.Format(SamlAttributeValueTemplate, @namespace, value);
        }

        public static string SamlAttributeTemplate
        {
            get => "<Attribute AttributeName=\"{0}\" AttributeNamespace=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}</Attribute>";
        }

        public static string SamlAttributeXml(string name, string attributeNs, IEnumerable<string> attributes)
        {
            if (string.IsNullOrEmpty(name))
                name = string.Empty;

            if (string.IsNullOrEmpty(attributeNs))
                attributeNs = string.Empty;

            return string.Format(SamlAttributeTemplate, name, attributeNs, (attributes == null) ? string.Empty : string.Concat(attributes));
        }

        public static string SamlAttributeStatementTemplate
        {
            get => "<AttributeStatement xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0} {1}</AttributeStatement>";
        }

        public static string SamlAttributeStatementXml(string subject, string attributes)
        {
            return string.Format(SamlAttributeStatementTemplate, subject, attributes);
        }

        public static string SamlAudienceTemplate
        {
            get => "<Audience xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</Audience>";
        }

        public static string SamlAudienceXml(string audience)
        {
            return string.Format(SamlAudienceTemplate, audience);
        }

        public static string SamlAudienceRestrictionConditionTemplate
        {
            get => "<AudienceRestrictionCondition xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</AudienceRestrictionCondition>";
        }

        public static string SamlAudienceRestrictionConditionXml(IEnumerable<string> audiences)
        {
            return string.Format(SamlAudienceRestrictionConditionTemplate, string.Concat(audiences));
        }

        public static string SamlSubjectLocalityTemplate
        {
            get => "<SubjectLocality IPAddress=\"{0}\" DNSAddress=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\"/>";
        }

        public static string SamlSubjectLocalityXml(string ipAddress, string dnsAddress)
        {
            return string.Format(SamlSubjectLocalityTemplate, ipAddress, dnsAddress);
        }

        public static string SamlAuthorityBindingTemplate
        {
            get => "<AuthorityBinding AuthorityKind=\"{0}\" Location=\"{1}\" Binding=\"{2}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\"/>";
        }

        public static string SamlAuthorityBindingXml(string authorityKind, string location, string binding)
        {
            return string.Format(SamlAuthorityBindingTemplate, authorityKind, location, binding);
        }

        public static string SamlAuthenticationStatementTemplate
        {
            get => "<AuthenticationStatement AuthenticationMethod=\"{0}\" AuthenticationInstant=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}{3}{4}</AuthenticationStatement>";
        }

        public static string SamlAuthenticationStatementXml(string method, string instant, string subject, string subjectLocality, string binding)
        {
            return string.Format(SamlAuthenticationStatementTemplate, method, instant, subject, subjectLocality, binding);
        }

        public static string SamlConditionsTemplate
        {
            get => "<Conditions {0} {1} xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}</Conditions>";
        }

        public static string SamlConditionsXml(string notBefore, string notOnOrAfter, IEnumerable<string> conditions)
        {
            return string.Format(SamlConditionsTemplate, NotBeforeXml(notBefore), NotOnOrAfterXml(notOnOrAfter), (conditions == null) ? string.Empty : string.Concat(conditions));
        }

        public static string SamlNameIdentifierTemplate
        {
            get => "<NameIdentifier NameQualifier=\"{0}\" Format=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}</NameIdentifier>";
        }

        public static string SamlNameIdentifierXml(string nameQualifier, string format, string name)
        {
            return string.Format(SamlNameIdentifierTemplate, nameQualifier, format, name);
        }

        public static string SamlConfirmationMethodTemplate
        {
            get => "<ConfirmationMethod xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</ConfirmationMethod>";
        }

        public static string SamlConfirmationMethodXml(string confirmationMethod)
        {
            return string.Format(SamlConfirmationMethodTemplate, confirmationMethod);
        }

        public static string SamlAdviceTemplate
        {
            get => "<Advice xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}{1}</Advice>";
        }

        public static string SamlAdviceXml(string assertionIDRef, string assertion)
        {
            return string.Format(SamlAdviceTemplate, assertionIDRef, assertion);
        }

        public static string SamlAssertionTemplate
        {
            get => "<Assertion MajorVersion=\"{0}\" MinorVersion=\"{1}\" AssertionID=\"{2}\" Issuer=\"{3}\" IssueInstant=\"{4}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{5}{6}{7}{8}</Assertion>";
        }

        public static string SamlAssertionXml(string majorVersion, string minorVersion, string assertionId, string issuer, string issueInstant, string conditions, string advices, string statements, string signatures)
        {
            return string.Format(SamlAssertionTemplate, majorVersion, minorVersion, assertionId, issuer, issueInstant, conditions, advices, statements, signatures);
        }

        public static string SamlAssertionIDRefTemplate
        {
            get => "<AssertionIDReference xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</AssertionIDReference>";
        }

        public static string SamlAssertionIDRefXml(string assertionID)
        {
            return string.Format(SamlAssertionIDRefTemplate, assertionID);
        }
        public static string SamlAuthorizationDecisionStatementTemplate
        {
            get => "<AuthorizationDecisionStatement Resource=\"{0}\" Decision=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}{3}{4}</AuthorizationDecisionStatement>";
        }

        public static string SamlAuthorizationDecisionStatementXml(string resource, string decision, string subject, string action, string evidence)
        {
            return string.Format(SamlAuthorizationDecisionStatementTemplate, resource, decision, subject, action, evidence);
        }

        public static string SamlEvidenceTemplate
        {
            get => "<Evidence xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}{1}</Evidence>";
        }

        public static string SamlEvidenceXml(string assertionIdRef, string assertion)
        {
            return string.Format(SamlEvidenceTemplate, assertionIdRef, assertion);
        }

        public static string SamlSubjectConfirmationTemplate
        {
            get => "<SubjectConfirmation xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}<SubjectConfirmationData>\"{1}\"</SubjectConfirmationData></SubjectConfirmation>";
        }

        public static string SamlSubjectConfirmationXml(List<string> confirmations, string confirmationData)
        {

            return string.Format(SamlSubjectConfirmationTemplate, (confirmations == null) ? string.Empty : string.Concat(confirmations), confirmationData);
        }

        public static string SamlSubjectTemplate
        {
            get => "<Subject xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0} {1}</Subject>";
        }

        public static string SamlSubjectXml(string nameIdentifier, string confirmation)
        {
            return string.Format(SamlSubjectTemplate, nameIdentifier, confirmation);
        }

        public static string Generate(KeyInfo keyInfo)
        {
            var elements = new List<XmlEement>();

            if (!string.IsNullOrEmpty(keyInfo.CertificateData))
                elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509Certificate, keyInfo.CertificateData));

            if (!string.IsNullOrEmpty(keyInfo.IssuerName))
                elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509IssuerName, keyInfo.IssuerName));

            if (!string.IsNullOrEmpty(keyInfo.SerialNumber))
                elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509SerialNumber, keyInfo.SerialNumber));

            if (!string.IsNullOrEmpty(keyInfo.SKI))
                elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509SKI, keyInfo.SKI));

            if (!string.IsNullOrEmpty(keyInfo.SubjectName))
                elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509SubjectName, keyInfo.SubjectName));

            var str = string.Format(KeyInfoTemplate, XmlSignatureConstants.Namespace, XmlEement.Generate(new XmlEement(XmlSignatureConstants.Elements.X509Data, elements)));

            return string.Format(KeyInfoTemplate, XmlSignatureConstants.Namespace, XmlEement.Generate(new XmlEement(XmlSignatureConstants.Elements.X509Data, elements)));
        }

        public static string Generate(Signature signature)
        {
            return string.Format(SignatureTemplate, XmlSignatureConstants.Namespace, Generate(signature.SignedInfo), signature.SignatureValue, Generate(signature.KeyInfo));
        }

        public static string Generate(SignedInfo signedInfo)
        {
            return string.Format(SignedInfoTemplate, XmlSignatureConstants.Namespace, signedInfo.CanonicalizationMethod, signedInfo.SignatureAlgorithm, ReferenceXml(signedInfo.Reference));
        }

        public static string ReferenceTemplate
        {
            get => "<Reference URI=\"{0}\"><Transforms><Transform Algorithm=\"{1}\" /><Transform Algorithm=\"{2}\" /></Transforms><DigestMethod Algorithm=\"{3}\" /><DigestValue>{4}</DigestValue></Reference>";
        }

        public static string ReferenceXml(string referenceUri, string envelopingAlgorithm, string c14nAlgorithm, string digestAlgorithm, string digestValue)
        {
            return string.Format(ReferenceTemplate, referenceUri, envelopingAlgorithm, c14nAlgorithm, digestAlgorithm, digestValue);
        }

        // Always assumes two transforms
        public static string ReferenceXml(Reference reference)
        {
            return string.Format(ReferenceTemplate, reference.Uri, reference.TransformChain[0].Algorithm, reference.TransformChain[1].Algorithm, reference.DigestAlgorithm, reference.DigestText);
        }

        public static string RoleDescriptorTemplate
        {
            get => "<RoleDescriptor xsi:type=\"{0}\" protocolSupportEnumeration=\"{1}\" xmlns:xsi=\"{2}\" xmlns:fed=\"{3}\">{4}{5}{6}{7}{8}{9}{10}</RoleDescriptor>";
        }

        public static string SignatureTemplate
        {
            get => "<Signature xmlns=\"{0}\">{1}<SignatureValue>{2}</SignatureValue>{3}</Signature>";
        }

        public static string SignatureXml(string @namespace, string signatureValue, string signedInfo, string keyInfo)
        {
            return string.Format(SignedInfoTemplate, @namespace, signatureValue, signedInfo, keyInfo);
        }

        public static string SignedInfoTemplate
        {
            get => "<SignedInfo xmlns=\"{0}\"><CanonicalizationMethod Algorithm=\"{1}\" /><SignatureMethod Algorithm=\"{2}\" />{3}</SignedInfo>";
        }

        public static string SignedInfoXml(string @namespace, string c14nAlgorithm, string signatureAlgorithm, string reference)
        {
            return string.Format(SignedInfoTemplate, @namespace, c14nAlgorithm, signatureAlgorithm, reference);
        }

        public static string SingleLogoutServiceTemplate
        {
            get => "<SingleLogoutService Binding=\"urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect\" Location=\"https://login.microsoftonline.com/common/saml2\" />";
        }

        public static string SingleSignOnServiceTemplate
        {
            get => "<SingleSignOnService Binding=\"urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST\" Location=\"https://login.microsoftonline.com/common/saml2\" />";
        }
    }
}
