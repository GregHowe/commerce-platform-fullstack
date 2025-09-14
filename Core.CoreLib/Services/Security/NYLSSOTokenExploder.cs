
using System.Web;
using System.Text;
using Core.CoreLib.Models.SAML;
using System.Xml.Linq;

namespace Core.CoreLib.Services.Security
{
    public class NYLSSOTokenExploderService : ITokenExploder
    {
        public const string SampleToken =
            @"<samlp:Response xmlns:samlp=""urn:oasis:names:tc:SAML:2.0:protocol"" Version=""2.0"" ID=""Rj.IAwo741ljKmp7X481pDlfKLf"" IssueInstant=""2023-02-10T19:10:58.616Z"">
            <saml:Issuer xmlns:saml=""urn:oasis:names:tc:SAML:2.0:assertion"">https://www.newyorklife.com</saml:Issuer>
            <samlp:Status>
            <samlp:StatusCode Value=""urn:oasis:names:tc:SAML:2.0:status:Success""></samlp:StatusCode>
            </samlp:Status>
            <saml:Assertion xmlns:saml=""urn:oasis:names:tc:SAML:2.0:assertion"" ID=""t.yxgdLFbYLhctCsSWwJeaKNcfQ"" IssueInstant=""2023-02-10T19:10:58.676Z"" Version=""2.0"">
            <saml:Issuer>https://www.newyorklife.com</saml:Issuer>
            <ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"">
            <ds:SignedInfo>
            <ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""></ds:CanonicalizationMethod>
            <ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256""></ds:SignatureMethod>
            <ds:Reference URI=""#t.yxgdLFbYLhctCsSWwJeaKNcfQ"">
            <ds:Transforms>
            <ds:Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature""></ds:Transform>
            <ds:Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""></ds:Transform>
            </ds:Transforms>
            <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""></ds:DigestMethod>
            <ds:DigestValue>jcAtHYShvUVCtX4ehi9hvYVyc5bhs9+3n+tf53dmIxA=</ds:DigestValue>
            </ds:Reference>
            </ds:SignedInfo>
            <ds:SignatureValue>
            d8zuT6GXf42yLFiJWLcDmbOb0YYkR5B9AWZaxpzYoj7hwhGI+P2Ho3bmwUVoUox5iALOwA6bFWPYgU/wvcJybC6SyaOlOJR9zDsOfxhN2xJecG1X4irL4aGL1/vFXdWaXRByFb0VwmjS3wo
            /GgYeKVo8/CalaVDl0VNWRAxiNG43JzWothI+C0+97btOmEAEaO4wBQCeEpIHsGslrbJu2xXRiVQuAaScTxMngV0D91vK+oh+C2Kx//UQmc7SejE2yaJnN+qZzzswThuJ+yooRimHjk7oNhoBhn
            /Ca46YZ5FFtKObzkPPo0hVB8RE3B+tztT4QQUC84djUk6MHyvdADNuASi7HvBH2M/0vuAkXwtgveucnT4x2WDzEkrYcaInDZc5dSM3GQPcyzP943GgL8u3xw95ozNQKlHRI7h6sNjUFnfmYFizH+
            pmbLOjSP5aJdgrtaoU9+DWzmyuGkIBBiJKgFstPVt9KE+zhJliAzDL4HqKo7wpf3s3Yz6rEyFmhT01dfFFdkpoCV63UguSpccOHCpEWVd/bSj3Xj1NFL5oO1Wg4lKvazdwCNOR2n4Pqi9DjMy4AgZxEAR
            jrL3rYLEeG8pdSkxPEhrIxDuOjtodZ397myMcKastsqo1a4Z/soqeRxlcEnRNY4RTZBDfR4126S4vklP33Jar1lCbbG4=
            </ds:SignatureValue>
            </ds:Signature>
            <saml:Subject>
            <saml:NameID Format=""urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified"">ap00p060dpr2t55p</saml:NameID>
            <saml:SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"">
            <saml:SubjectConfirmationData Recipient=""https://nyl-integration.fusion92core.com/auth/success"" NotOnOrAfter=""2023-02-10T19:15:58.676Z""></saml:SubjectConfirmationData>
            </saml:SubjectConfirmation>
            </saml:Subject>
            <saml:Conditions NotBefore=""2023-02-10T19:05:58.676Z"" NotOnOrAfter=""2023-02-10T19:15:58.676Z"">
            <saml:AudienceRestriction>
            <saml:Audience>https://nyl-integration.fusion92core.com</saml:Audience>
            </saml:AudienceRestriction>
            </saml:Conditions>
            <saml:AuthnStatement SessionIndex=""t.yxgdLFbYLhctCsSWwJeaKNcfQ"" AuthnInstant=""2023-02-10T19:10:58.639Z"">
            <saml:AuthnContext>
            <saml:AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:unspecified</saml:AuthnContextClassRef>
            </saml:AuthnContext>
            </saml:AuthnStatement>
            <saml:AttributeStatement>
            <saml:Attribute Name=""nylRole"" NameFormat=""urn:oasis:names:tc:SAML:2.0:attrname-format:basic"">
            <saml:AttributeValue xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:type=""xs:string"">Agent Staff</saml:AttributeValue>
            </saml:Attribute>
            <saml:Attribute Name=""principal:nylid"" NameFormat=""urn:oasis:names:tc:SAML:2.0:attrname-format:basic"">
            <saml:AttributeValue xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:type=""xs:string"">ap00p060dpr2t55p</saml:AttributeValue>
            </saml:Attribute>
            <saml:Attribute Name=""groups"" NameFormat=""urn:oasis:names:tc:SAML:2.0:attrname-format:basic"">
            <saml:AttributeValue xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:type=""xs:string"">NoMatchingGroupFound</saml:AttributeValue>
            </saml:Attribute>
            <saml:Attribute Name=""email"" NameFormat=""urn:oasis:names:tc:SAML:2.0:attrname-format:basic"">
            <saml:AttributeValue xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:type=""xs:string"">marykennedy@ft.newyorklife.com</saml:AttributeValue>
            </saml:Attribute>
            <saml:Attribute Name=""onbehalfof:nylid"" NameFormat=""urn:oasis:names:tc:SAML:2.0:attrname-format:basic"">
            <saml:AttributeValue xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:type=""xs:string"">ap00p060dbs98lff</saml:AttributeValue>
            </saml:Attribute>
            </saml:AttributeStatement>
            </saml:Assertion></samlp:Response>";

        public const string EncodedNONOBOSampleToken =
            "JTNjc2FtbHAlM2FSZXNwb25zZSt4bWxucyUzYXNhbWxwJTNkJTIydXJuJTNhb2FzaXMlM2FuYW1lcyUzYXRjJTNhU0FNTCUzYTIuMCUzYXByb3RvY29sJTIyK1ZlcnNpb24lM2QlMjIyLjAlMjIrSUQlM2QlMjJSai5JQXdvNzQxbGpLbXA3WDQ4MXBEbGZLTGYlMjIrSXNzdWVJbnN0YW50JTNkJTIyMjAyMy0wMi0xMFQxOSUzYTEwJTNhNTguNjE2WiUyMiUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FJc3N1ZXIreG1sbnMlM2FzYW1sJTNkJTIydXJuJTNhb2FzaXMlM2FuYW1lcyUzYXRjJTNhU0FNTCUzYTIuMCUzYWFzc2VydGlvbiUyMiUzZWh0dHBzJTNhJTJmJTJmd3d3Lm5ld3lvcmtsaWZlLmNvbSUzYyUyZnNhbWwlM2FJc3N1ZXIlM2UlMGQlMGErKysrKysrKysrKyslM2NzYW1scCUzYVN0YXR1cyUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWxwJTNhU3RhdHVzQ29kZStWYWx1ZSUzZCUyMnVybiUzYW9hc2lzJTNhbmFtZXMlM2F0YyUzYVNBTUwlM2EyLjAlM2FzdGF0dXMlM2FTdWNjZXNzJTIyJTNlJTNjJTJmc2FtbHAlM2FTdGF0dXNDb2RlJTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmc2FtbHAlM2FTdGF0dXMlM2UlMGQlMGErKysrKysrKysrKyslM2NzYW1sJTNhQXNzZXJ0aW9uK3htbG5zJTNhc2FtbCUzZCUyMnVybiUzYW9hc2lzJTNhbmFtZXMlM2F0YyUzYVNBTUwlM2EyLjAlM2Fhc3NlcnRpb24lMjIrSUQlM2QlMjJ0Lnl4Z2RMRmJZTGhjdENzU1d3SmVhS05jZlElMjIrSXNzdWVJbnN0YW50JTNkJTIyMjAyMy0wMi0xMFQxOSUzYTEwJTNhNTguNjc2WiUyMitWZXJzaW9uJTNkJTIyMi4wJTIyJTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYUlzc3VlciUzZWh0dHBzJTNhJTJmJTJmd3d3Lm5ld3lvcmtsaWZlLmNvbSUzYyUyZnNhbWwlM2FJc3N1ZXIlM2UlMGQlMGErKysrKysrKysrKyslM2NkcyUzYVNpZ25hdHVyZSt4bWxucyUzYWRzJTNkJTIyaHR0cCUzYSUyZiUyZnd3dy53My5vcmclMmYyMDAwJTJmMDklMmZ4bWxkc2lnJTIzJTIyJTNlJTBkJTBhKysrKysrKysrKysrJTNjZHMlM2FTaWduZWRJbmZvJTNlJTBkJTBhKysrKysrKysrKysrJTNjZHMlM2FDYW5vbmljYWxpemF0aW9uTWV0aG9kK0FsZ29yaXRobSUzZCUyMmh0dHAlM2ElMmYlMmZ3d3cudzMub3JnJTJmMjAwMSUyZjEwJTJmeG1sLWV4Yy1jMTRuJTIzJTIyJTNlJTNjJTJmZHMlM2FDYW5vbmljYWxpemF0aW9uTWV0aG9kJTNlJTBkJTBhKysrKysrKysrKysrJTNjZHMlM2FTaWduYXR1cmVNZXRob2QrQWxnb3JpdGhtJTNkJTIyaHR0cCUzYSUyZiUyZnd3dy53My5vcmclMmYyMDAxJTJmMDQlMmZ4bWxkc2lnLW1vcmUlMjNyc2Etc2hhMjU2JTIyJTNlJTNjJTJmZHMlM2FTaWduYXR1cmVNZXRob2QlM2UlMGQlMGErKysrKysrKysrKyslM2NkcyUzYVJlZmVyZW5jZStVUkklM2QlMjIlMjN0Lnl4Z2RMRmJZTGhjdENzU1d3SmVhS05jZlElMjIlM2UlMGQlMGErKysrKysrKysrKyslM2NkcyUzYVRyYW5zZm9ybXMlM2UlMGQlMGErKysrKysrKysrKyslM2NkcyUzYVRyYW5zZm9ybStBbGdvcml0aG0lM2QlMjJodHRwJTNhJTJmJTJmd3d3LnczLm9yZyUyZjIwMDAlMmYwOSUyZnhtbGRzaWclMjNlbnZlbG9wZWQtc2lnbmF0dXJlJTIyJTNlJTNjJTJmZHMlM2FUcmFuc2Zvcm0lM2UlMGQlMGErKysrKysrKysrKyslM2NkcyUzYVRyYW5zZm9ybStBbGdvcml0aG0lM2QlMjJodHRwJTNhJTJmJTJmd3d3LnczLm9yZyUyZjIwMDElMmYxMCUyZnhtbC1leGMtYzE0biUyMyUyMiUzZSUzYyUyZmRzJTNhVHJhbnNmb3JtJTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmZHMlM2FUcmFuc2Zvcm1zJTNlJTBkJTBhKysrKysrKysrKysrJTNjZHMlM2FEaWdlc3RNZXRob2QrQWxnb3JpdGhtJTNkJTIyaHR0cCUzYSUyZiUyZnd3dy53My5vcmclMmYyMDAxJTJmMDQlMmZ4bWxlbmMlMjNzaGEyNTYlMjIlM2UlM2MlMmZkcyUzYURpZ2VzdE1ldGhvZCUzZSUwZCUwYSsrKysrKysrKysrKyUzY2RzJTNhRGlnZXN0VmFsdWUlM2VqY0F0SFlTaHZVVkN0WDRlaGk5aHZZVnljNWJoczklMmIzbiUyYnRmNTNkbUl4QSUzZCUzYyUyZmRzJTNhRGlnZXN0VmFsdWUlM2UlMGQlMGErKysrKysrKysrKyslM2MlMmZkcyUzYVJlZmVyZW5jZSUzZSUwZCUwYSsrKysrKysrKysrKyUzYyUyZmRzJTNhU2lnbmVkSW5mbyUzZSUwZCUwYSsrKysrKysrKysrKyUzY2RzJTNhU2lnbmF0dXJlVmFsdWUlM2UlMGQlMGErKysrKysrKysrKytkOHp1VDZHWGY0MnlMRmlKV0xjRG1iT2IwWVlrUjVCOUFXWmF4cHpZb2o3aHdoR0klMmJQMkhvM2Jtd1VWb1VveDVpQUxPd0E2YkZXUFlnVSUyZnd2Y0p5YkM2U3lhT2xPSlI5ekRzT2Z4aE4yeEplY0cxWDRpckw0YUdMMSUyZnZGWGRXYVhSQnlGYjBWd21qUzN3byUwZCUwYSsrKysrKysrKysrKyUyZkdnWWVLVm84JTJmQ2FsYVZEbDBWTldSQXhpTkc0M0p6V290aEklMmJDMCUyYjk3YnRPbUVBRWFPNHdCUUNlRXBJSHNHc2xyYkp1MnhYUmlWUXVBYVNjVHhNbmdWMEQ5MXZLJTJib2glMmJDMkt4JTJmJTJmVVFtYzdTZWpFMnlhSm5OJTJicVp6enN3VGh1SiUyYnlvb1JpbUhqazdvTmhvQmhuJTBkJTBhKysrKysrKysrKysrJTJmQ2E0NllaNUZGdEtPYnprUFBvMGhWQjhSRTNCJTJidHp0VDRRUVVDODRkalVrNk1IeXZkQUROdUFTaTdIdkJIMk0lMmYwdnVBa1h3dGd2ZXVjblQ0eDJXRHpFa3JZY2FJbkRaYzVkU00zR1FQY3l6UDk0M0dnTDh1M3h3OTVvek5RS2xIUkk3aDZzTmpVRm5mbVlGaXpIJTJiJTBkJTBhKysrKysrKysrKysrcG1iTE9qU1A1YUpkZ3J0YW9VOSUyYkRXem15dUdrSUJCaUpLZ0ZzdFBWdDlLRSUyYnpoSmxpQXpETDRIcUtvN3dwZjNzM1l6NnJFeUZtaFQwMWRmRkZka3BvQ1Y2M1VndVNwY2NPSENwRVdWZCUyZmJTajNYajFORkw1b08xV2c0bEt2YXpkd0NOT1IybjRQcWk5RGpNeTRBZ1p4RUFSJTBkJTBhKysrKysrKysrKysranJMM3JZTEVlRzhwZFNreFBFaHJJeER1T2p0b2RaMzk3bXlNY0thc3RzcW8xYTRaJTJmc29xZVJ4bGNFblJOWTRSVFpCRGZSNDEyNlM0dmtsUDMzSmFyMWxDYmJHNCUzZCUwZCUwYSsrKysrKysrKysrKyUzYyUyZmRzJTNhU2lnbmF0dXJlVmFsdWUlM2UlMGQlMGErKysrKysrKysrKyslM2MlMmZkcyUzYVNpZ25hdHVyZSUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FTdWJqZWN0JTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYU5hbWVJRCtGb3JtYXQlM2QlMjJ1cm4lM2FvYXNpcyUzYW5hbWVzJTNhdGMlM2FTQU1MJTNhMS4xJTNhbmFtZWlkLWZvcm1hdCUzYXVuc3BlY2lmaWVkJTIyJTNlYXAwMHAwNjBkcHIydDU1cCUzYyUyZnNhbWwlM2FOYW1lSUQlM2UlMGQlMGErKysrKysrKysrKyslM2NzYW1sJTNhU3ViamVjdENvbmZpcm1hdGlvbitNZXRob2QlM2QlMjJ1cm4lM2FvYXNpcyUzYW5hbWVzJTNhdGMlM2FTQU1MJTNhMi4wJTNhY20lM2FiZWFyZXIlMjIlM2UlMGQlMGErKysrKysrKysrKyslM2NzYW1sJTNhU3ViamVjdENvbmZpcm1hdGlvbkRhdGErUmVjaXBpZW50JTNkJTIyaHR0cHMlM2ElMmYlMmZueWwtaW50ZWdyYXRpb24uZnVzaW9uOTJjb3JlLmNvbSUyZmF1dGglMmZzdWNjZXNzJTIyK05vdE9uT3JBZnRlciUzZCUyMjIwMjMtMDItMTBUMTklM2ExNSUzYTU4LjY3NlolMjIlM2UlM2MlMmZzYW1sJTNhU3ViamVjdENvbmZpcm1hdGlvbkRhdGElM2UlMGQlMGErKysrKysrKysrKyslM2MlMmZzYW1sJTNhU3ViamVjdENvbmZpcm1hdGlvbiUzZSUwZCUwYSsrKysrKysrKysrKyUzYyUyZnNhbWwlM2FTdWJqZWN0JTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYUNvbmRpdGlvbnMrTm90QmVmb3JlJTNkJTIyMjAyMy0wMi0xMFQxOSUzYTA1JTNhNTguNjc2WiUyMitOb3RPbk9yQWZ0ZXIlM2QlMjIyMDIzLTAyLTEwVDE5JTNhMTUlM2E1OC42NzZaJTIyJTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYUF1ZGllbmNlUmVzdHJpY3Rpb24lM2UlMGQlMGErKysrKysrKysrKyslM2NzYW1sJTNhQXVkaWVuY2UlM2VodHRwcyUzYSUyZiUyZm55bC1pbnRlZ3JhdGlvbi5mdXNpb245MmNvcmUuY29tJTNjJTJmc2FtbCUzYUF1ZGllbmNlJTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmc2FtbCUzYUF1ZGllbmNlUmVzdHJpY3Rpb24lM2UlMGQlMGErKysrKysrKysrKyslM2MlMmZzYW1sJTNhQ29uZGl0aW9ucyUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdXRoblN0YXRlbWVudCtTZXNzaW9uSW5kZXglM2QlMjJ0Lnl4Z2RMRmJZTGhjdENzU1d3SmVhS05jZlElMjIrQXV0aG5JbnN0YW50JTNkJTIyMjAyMy0wMi0xMFQxOSUzYTEwJTNhNTguNjM5WiUyMiUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdXRobkNvbnRleHQlM2UlMGQlMGErKysrKysrKysrKyslM2NzYW1sJTNhQXV0aG5Db250ZXh0Q2xhc3NSZWYlM2V1cm4lM2FvYXNpcyUzYW5hbWVzJTNhdGMlM2FTQU1MJTNhMi4wJTNhYWMlM2FjbGFzc2VzJTNhdW5zcGVjaWZpZWQlM2MlMmZzYW1sJTNhQXV0aG5Db250ZXh0Q2xhc3NSZWYlM2UlMGQlMGErKysrKysrKysrKyslM2MlMmZzYW1sJTNhQXV0aG5Db250ZXh0JTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmc2FtbCUzYUF1dGhuU3RhdGVtZW50JTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYUF0dHJpYnV0ZVN0YXRlbWVudCUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdHRyaWJ1dGUrTmFtZSUzZCUyMm55bFJvbGUlMjIrTmFtZUZvcm1hdCUzZCUyMnVybiUzYW9hc2lzJTNhbmFtZXMlM2F0YyUzYVNBTUwlM2EyLjAlM2FhdHRybmFtZS1mb3JtYXQlM2FiYXNpYyUyMiUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdHRyaWJ1dGVWYWx1ZSt4bWxucyUzYXhzJTNkJTIyaHR0cCUzYSUyZiUyZnd3dy53My5vcmclMmYyMDAxJTJmWE1MU2NoZW1hJTIyK3htbG5zJTNheHNpJTNkJTIyaHR0cCUzYSUyZiUyZnd3dy53My5vcmclMmYyMDAxJTJmWE1MU2NoZW1hLWluc3RhbmNlJTIyK3hzaSUzYXR5cGUlM2QlMjJ4cyUzYXN0cmluZyUyMiUzZUFnZW50K1N0YWZmJTNjJTJmc2FtbCUzYUF0dHJpYnV0ZVZhbHVlJTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmc2FtbCUzYUF0dHJpYnV0ZSUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdHRyaWJ1dGUrTmFtZSUzZCUyMnByaW5jaXBhbCUzYW55bGlkJTIyK05hbWVGb3JtYXQlM2QlMjJ1cm4lM2FvYXNpcyUzYW5hbWVzJTNhdGMlM2FTQU1MJTNhMi4wJTNhYXR0cm5hbWUtZm9ybWF0JTNhYmFzaWMlMjIlM2UlMGQlMGErKysrKysrKysrKyslM2NzYW1sJTNhQXR0cmlidXRlVmFsdWUreG1sbnMlM2F4cyUzZCUyMmh0dHAlM2ElMmYlMmZ3d3cudzMub3JnJTJmMjAwMSUyZlhNTFNjaGVtYSUyMit4bWxucyUzYXhzaSUzZCUyMmh0dHAlM2ElMmYlMmZ3d3cudzMub3JnJTJmMjAwMSUyZlhNTFNjaGVtYS1pbnN0YW5jZSUyMit4c2klM2F0eXBlJTNkJTIyeHMlM2FzdHJpbmclMjIlM2VhcDAwcDA2MGRwcjJ0NTVwJTNjJTJmc2FtbCUzYUF0dHJpYnV0ZVZhbHVlJTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmc2FtbCUzYUF0dHJpYnV0ZSUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdHRyaWJ1dGUrTmFtZSUzZCUyMmdyb3VwcyUyMitOYW1lRm9ybWF0JTNkJTIydXJuJTNhb2FzaXMlM2FuYW1lcyUzYXRjJTNhU0FNTCUzYTIuMCUzYWF0dHJuYW1lLWZvcm1hdCUzYWJhc2ljJTIyJTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYUF0dHJpYnV0ZVZhbHVlK3htbG5zJTNheHMlM2QlMjJodHRwJTNhJTJmJTJmd3d3LnczLm9yZyUyZjIwMDElMmZYTUxTY2hlbWElMjIreG1sbnMlM2F4c2klM2QlMjJodHRwJTNhJTJmJTJmd3d3LnczLm9yZyUyZjIwMDElMmZYTUxTY2hlbWEtaW5zdGFuY2UlMjIreHNpJTNhdHlwZSUzZCUyMnhzJTNhc3RyaW5nJTIyJTNlTm9NYXRjaGluZ0dyb3VwRm91bmQlM2MlMmZzYW1sJTNhQXR0cmlidXRlVmFsdWUlM2UlMGQlMGErKysrKysrKysrKyslM2MlMmZzYW1sJTNhQXR0cmlidXRlJTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYUF0dHJpYnV0ZStOYW1lJTNkJTIyZW1haWwlMjIrTmFtZUZvcm1hdCUzZCUyMnVybiUzYW9hc2lzJTNhbmFtZXMlM2F0YyUzYVNBTUwlM2EyLjAlM2FhdHRybmFtZS1mb3JtYXQlM2FiYXNpYyUyMiUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdHRyaWJ1dGVWYWx1ZSt4bWxucyUzYXhzJTNkJTIyaHR0cCUzYSUyZiUyZnd3dy53My5vcmclMmYyMDAxJTJmWE1MU2NoZW1hJTIyK3htbG5zJTNheHNpJTNkJTIyaHR0cCUzYSUyZiUyZnd3dy53My5vcmclMmYyMDAxJTJmWE1MU2NoZW1hLWluc3RhbmNlJTIyK3hzaSUzYXR5cGUlM2QlMjJ4cyUzYXN0cmluZyUyMiUzZW1hcnlrZW5uZWR5JTQwZnQubmV3eW9ya2xpZmUuY29tJTNjJTJmc2FtbCUzYUF0dHJpYnV0ZVZhbHVlJTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmc2FtbCUzYUF0dHJpYnV0ZSUzZSUwZCUwYSsrKysrKysrKysrKyUzY3NhbWwlM2FBdHRyaWJ1dGUrTmFtZSUzZCUyMm9uYmVoYWxmb2YlM2FueWxpZCUyMitOYW1lRm9ybWF0JTNkJTIydXJuJTNhb2FzaXMlM2FuYW1lcyUzYXRjJTNhU0FNTCUzYTIuMCUzYWF0dHJuYW1lLWZvcm1hdCUzYWJhc2ljJTIyJTNlJTBkJTBhKysrKysrKysrKysrJTNjc2FtbCUzYUF0dHJpYnV0ZVZhbHVlK3htbG5zJTNheHMlM2QlMjJodHRwJTNhJTJmJTJmd3d3LnczLm9yZyUyZjIwMDElMmZYTUxTY2hlbWElMjIreG1sbnMlM2F4c2klM2QlMjJodHRwJTNhJTJmJTJmd3d3LnczLm9yZyUyZjIwMDElMmZYTUxTY2hlbWEtaW5zdGFuY2UlMjIreHNpJTNhdHlwZSUzZCUyMnhzJTNhc3RyaW5nJTIyJTNlYXAwMHAwNjBkYnM5OGxmZiUzYyUyZnNhbWwlM2FBdHRyaWJ1dGVWYWx1ZSUzZSUwZCUwYSsrKysrKysrKysrKyUzYyUyZnNhbWwlM2FBdHRyaWJ1dGUlM2UlMGQlMGErKysrKysrKysrKyslM2MlMmZzYW1sJTNhQXR0cmlidXRlU3RhdGVtZW50JTNlJTBkJTBhKysrKysrKysrKysrJTNjJTJmc2FtbCUzYUFzc2VydGlvbiUzZSUzYyUyZnNhbWxwJTNhUmVzcG9uc2UlM2U=";

        public const string EncodedSampleToken =
            "PHNhbWxwOlJlc3BvbnNlIFZlcnNpb249IjIuMCIgSUQ9Ik1PdE44bFFzTHZkUEJjOGE3RXZUcmxXUEZMSCIgSXNzdWVJbnN0YW50PSIyMDIyLTEyLTA5VDE3OjAwOjI2LjM1OFoiIHhtbG5zOnNhbWxwPSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6cHJvdG9jb2wiPjxzYW1sOklzc3VlciB4bWxuczpzYW1sPSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXNzZXJ0aW9uIj5odHRwczovL3d3dy5uZXd5b3JrbGlmZS5jb208L3NhbWw6SXNzdWVyPjxzYW1scDpTdGF0dXM+PHNhbWxwOlN0YXR1c0NvZGUgVmFsdWU9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDpzdGF0dXM6U3VjY2VzcyIvPjwvc2FtbHA6U3RhdHVzPjxzYW1sOkFzc2VydGlvbiBJRD0iWTZjMWdvQmpFV3pQWk9XYXZzaUVwZFhHVHJwIiBJc3N1ZUluc3RhbnQ9IjIwMjItMTItMDlUMTc6MDA6MjYuOTg2WiIgVmVyc2lvbj0iMi4wIiB4bWxuczpzYW1sPSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXNzZXJ0aW9uIj48c2FtbDpJc3N1ZXI+aHR0cHM6Ly93d3cubmV3eW9ya2xpZmUuY29tPC9zYW1sOklzc3Vlcj48ZHM6U2lnbmF0dXJlIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIj4KPGRzOlNpZ25lZEluZm8+CjxkczpDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8xMC94bWwtZXhjLWMxNG4jIi8+CjxkczpTaWduYXR1cmVNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2Ii8+CjxkczpSZWZlcmVuY2UgVVJJPSIjWTZjMWdvQmpFV3pQWk9XYXZzaUVwZFhHVHJwIj4KPGRzOlRyYW5zZm9ybXM+CjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjZW52ZWxvcGVkLXNpZ25hdHVyZSIvPgo8ZHM6VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8xMC94bWwtZXhjLWMxNG4jIi8+CjwvZHM6VHJhbnNmb3Jtcz4KPGRzOkRpZ2VzdE1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMDQveG1sZW5jI3NoYTI1NiIvPgo8ZHM6RGlnZXN0VmFsdWU+N2crVXpPVDFva0NZaUN1Z3hWdEhGemh6S3FUMHBsSklSRHg4MjM5ZVhmTT08L2RzOkRpZ2VzdFZhbHVlPgo8L2RzOlJlZmVyZW5jZT4KPC9kczpTaWduZWRJbmZvPgo8ZHM6U2lnbmF0dXJlVmFsdWU+CllPaW1sNnNDNGgzKzNCeUMzOWNsSStqTjcyK3NWc1ZhMmFyQyt3RFFUMFdiNzZwVnQxSHA0bFVoM1lqWHowc3NHQlBpSkZrSTNWMHgmIzEzOwptNGU5TVVRNzRNNDB3UThGNWsyUXpuNW9YNDZyMkZKNFpaeW5xVlducmdwZW1iNE5lYnVjZEd4OU1hQ0hScWc0Wk9tZFlZYjM0ZktzJiMxMzsKSy9BdHRzNERLbXVoWnZjZHRvSzJOQm9KRVZ0ZEFTcWdudkQwSzBOUEJlb2t6VGJOZG96U2Y3WlZIWG5jdmVnMTBmQmpyNkZUVTJGZSYjMTM7ClNqZEVRaXB6dE0yT0FnZzFMdGhIaGR5SXZxYUw1TGVjc2QwZGNjV1NvT1lod2NjWEp3SHRLL3lJRTB2OGNmZWdoVVlHNUI1dU81Y1QmIzEzOwpmaDlaQTE2SUZ2ckg4SGxYNGxOM2xtUktYQmJTNnlMUDhBekdGVTRDS00zVi9adEhNY2QzSUJMNUpBalE2YUNjVDR5Z3p2V254NStGJiMxMzsKbU1Ialp4RDZTNFRXUjh6RlhJcGR0NzZzb3hHNG9INlkzOTREMnlOQi9SblJSOGYyQVcrZGZ1RTR0TmY4YUVBZy9oSUJOOEFXQzFxcCYjMTM7CkZUcWlOTlJvMG5zbDUxUXBEL0c4dEk4NjBWT3R3VzVPNVhhUXovcFNJczhJNXlrdmhpbTR3SmZOTkM3eGhycWxINXFEV2JUK3pBNmgmIzEzOwpXSEZaQk0rZVByZWp2aXFoM2VTVXpvWVF1WWxIc25oR1RtaXlMWXN5Y2ppQ2ZqQ240ZVgyNXNpdVpoMEl0NFFuSjk4c214Tk1YbDJiJiMxMzsKZnYveXdxaWF1NkdINDkxQ0hXZGJZQ3BMc3dvYWk1eU1aN3VjSGNINWs5ZmlNelFGUjhFWHlNTStpOTlvajYrZHRscVJaQkRteENzPQo8L2RzOlNpZ25hdHVyZVZhbHVlPgo8L2RzOlNpZ25hdHVyZT48c2FtbDpTdWJqZWN0PjxzYW1sOk5hbWVJRCBGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjEuMTpuYW1laWQtZm9ybWF0OnVuc3BlY2lmaWVkIj5hcDAwcDA1MGRiczh5ODAyPC9zYW1sOk5hbWVJRD48c2FtbDpTdWJqZWN0Q29uZmlybWF0aW9uIE1ldGhvZD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmNtOmJlYXJlciI+PHNhbWw6U3ViamVjdENvbmZpcm1hdGlvbkRhdGEgUmVjaXBpZW50PSJodHRwczovL255bC1pbnRlZ3JhdGlvbi5mdXNpb245MmNvcmUuY29tL2F1dGgvc3VjY2VzcyIgTm90T25PckFmdGVyPSIyMDIyLTEyLTA5VDE3OjA1OjI2Ljk4N1oiLz48L3NhbWw6U3ViamVjdENvbmZpcm1hdGlvbj48L3NhbWw6U3ViamVjdD48c2FtbDpDb25kaXRpb25zIE5vdEJlZm9yZT0iMjAyMi0xMi0wOVQxNjo1NToyNi45ODdaIiBOb3RPbk9yQWZ0ZXI9IjIwMjItMTItMDlUMTc6MDU6MjYuOTg3WiI+PHNhbWw6QXVkaWVuY2VSZXN0cmljdGlvbj48c2FtbDpBdWRpZW5jZT5odHRwczovL255bC1pbnRlZ3JhdGlvbi5mdXNpb245MmNvcmUuY29tPC9zYW1sOkF1ZGllbmNlPjwvc2FtbDpBdWRpZW5jZVJlc3RyaWN0aW9uPjwvc2FtbDpDb25kaXRpb25zPjxzYW1sOkF1dGhuU3RhdGVtZW50IFNlc3Npb25JbmRleD0iWTZjMWdvQmpFV3pQWk9XYXZzaUVwZFhHVHJwIiBBdXRobkluc3RhbnQ9IjIwMjItMTItMDlUMTc6MDA6MjYuMzgxWiI+PHNhbWw6QXV0aG5Db250ZXh0PjxzYW1sOkF1dGhuQ29udGV4dENsYXNzUmVmPnVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphYzpjbGFzc2VzOnVuc3BlY2lmaWVkPC9zYW1sOkF1dGhuQ29udGV4dENsYXNzUmVmPjwvc2FtbDpBdXRobkNvbnRleHQ+PC9zYW1sOkF1dGhuU3RhdGVtZW50PjxzYW1sOkF0dHJpYnV0ZVN0YXRlbWVudD48c2FtbDpBdHRyaWJ1dGUgTmFtZT0ibnlsUm9sZSIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyIgeG1sbnM6eHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIj5BZ2VudDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSBOYW1lPSJwcmluY2lwYWw6bnlsaWQiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSI+YXAwMHAwNTBkYnM4eTgwMjwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSBOYW1lPSJncm91cHMiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSI+Tm9NYXRjaGluZ0dyb3VwRm91bmQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgTmFtZT0iZW1haWwiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSI+ZWFzaHdvcnRoQGZ0Lm5ld3lvcmtsaWZlLmNvbTwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjwvc2FtbDpBdHRyaWJ1dGVTdGF0ZW1lbnQ+PC9zYW1sOkFzc2VydGlvbj48L3NhbWxwOlJlc3BvbnNlPg==";

        public SAMLPayload ExplodeToken(string token)
        {
            var result = new SAMLPayload();

            var decodedToken =
                DecodeToken(token) ?? string.Empty;

            var xmlDoc = XDocument.Parse(decodedToken);
            var attributes =
                xmlDoc?
                .Descendants()?
                .Where(w => 
                    w.Name.LocalName.Equals(SAMLAttributeKey.AttributeNode))?
                .ToList() ?? new List<XElement>();

            // SSOId
            var ssoId =
                FindInXMLAttributes(attributes, SAMLAttributeKey.NYLId);
                
            if (!string.IsNullOrWhiteSpace(ssoId))
                result.SSOId = ssoId;
            else
                throw new Exception("Token returned from SSO not valid (SSOId)");

            // OBO
            var oboId =
                FindInXMLAttributes(attributes, SAMLAttributeKey.OBONYLId);
            
            // Not required, don't throw exception if missing
            if (!string.IsNullOrWhiteSpace(oboId))
            {
                result.OBOSSOId = oboId;
                result.OBOSession = true;
            }

            // User email
            var userEmail =
                FindInXMLAttributes(attributes, SAMLAttributeKey.Email);
            
            if (!string.IsNullOrWhiteSpace(userEmail))
                result.EmailAddress = userEmail;
            else
                throw new Exception("Token returned from SSO not valid (email)");

            return result;
        }

        private string FindInXMLAttributes(List<XElement> attributes, string name) =>
            attributes?.Where(w => w.FirstAttribute.Value == name)?.FirstOrDefault()?.Value ?? string.Empty;

        private string EncodeToken(string token)
        {
            // UrlEncode, convert to bytes, to 64 bit string
            var encoded = HttpUtility.UrlEncode(token);
            var bytes = Encoding.UTF8.GetBytes(encoded);
            var result = Convert.ToBase64String(bytes);

            return result;
        }

        private string DecodeToken(string token)
        {
            // From 64 bit string to bytes, urldecode
            var bytes = Convert.FromBase64String(token);
            var encoded = Encoding.UTF8.GetString(bytes);
            var decoded = HttpUtility.UrlDecode(encoded);

            return
                decoded
                .Replace("\r\n", string.Empty)
                .Replace("            ", "");
        }
    }

    internal class SAMLAttributeKey
    {
        public const string AttributeNode = "Attribute";
        public const string NYLId = "principal:nylid";
        public const string Email = "email";
        public const string OBONYLId = "onbehalfof:nylid";
    }
}