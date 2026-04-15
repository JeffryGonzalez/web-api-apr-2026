using System.ComponentModel.DataAnnotations;
using Software.Api.Vendors.Models;

namespace Software.Tests.Vendors;

[Trait("Category", "Vendors")]
[Trait("Category", "Unit")]
public class ValidationTests
{
    // ---------------------------------------------------------------
    // Helper
    // ---------------------------------------------------------------

    /// <summary>
    /// Runs both attribute validation AND IValidatableObject.Validate()
    /// (IValidatableObject is only invoked when all attribute checks pass).
    /// </summary>
    private static IList<ValidationResult> Validate(Vendor vendor)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(vendor);
        Validator.TryValidateObject(vendor, context, results, validateAllProperties: true);
        return results;
    }

    private static Vendor ValidVendor(string name = "Acme Corp",
                                      string site = "https://acme.com",
                                      string email = "bob@acme.com") =>
        new()
        {
            Name = name,
            Site = site,
            PointOfContact = new VendorPointOfContact { Name = "Bob", Email = email }
        };

    // ---------------------------------------------------------------
    // Name – [MinLength(3)]
    // ---------------------------------------------------------------

    [Fact]
    public void Name_BelowMinLength_FailsValidation()
    {
        var results = Validate(ValidVendor(name: "AB"));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Vendor.Name)));
    }

    [Fact]
    public void Name_AtMinLength_PassesValidation()
    {
        var results = Validate(ValidVendor(name: "ABC"));
        Assert.DoesNotContain(results, r => r.MemberNames.Contains(nameof(Vendor.Name)));
    }

    // ---------------------------------------------------------------
    // Name – [MaxLength(20)]
    // ---------------------------------------------------------------

    [Fact]
    public void Name_AboveMaxLength_FailsValidation()
    {
        var results = Validate(ValidVendor(name: new string('X', 21)));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Vendor.Name)));
    }

    [Fact]
    public void Name_AtMaxLength_PassesValidation()
    {
        var results = Validate(ValidVendor(name: new string('X', 20)));
        Assert.DoesNotContain(results, r => r.MemberNames.Contains(nameof(Vendor.Name)));
    }

    // ---------------------------------------------------------------
    // Site – [Url]
    // ---------------------------------------------------------------

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("acme.com")]         // missing scheme
    [InlineData("//acme.com")]       // protocol-relative – not accepted
    public void Site_InvalidUrl_FailsValidation(string site)
    {
        var results = Validate(ValidVendor(site: site));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Vendor.Site)));
    }

    [Theory]
    [InlineData("https://acme.com")]
    [InlineData("http://acme.com")]
    [InlineData("https://www.acme.com")]
    public void Site_ValidUrl_PassesValidation(string site)
    {
        // email must still match the domain
        var uri = new Uri(site);
        var domain = uri.Host.Replace("www.", "");
        var results = Validate(ValidVendor(site: site, email: $"bob@{domain}"));
        Assert.DoesNotContain(results, r => r.MemberNames.Contains(nameof(Vendor.Site)));
    }

    // ---------------------------------------------------------------
    // IValidatableObject – email domain must match site domain
    // ---------------------------------------------------------------

    [Fact]
    public void EmailDomain_MatchesSiteDomain_NoErrors()
    {
        var results = Validate(ValidVendor(site: "https://acme.com", email: "bob@acme.com"));
        Assert.Empty(results);
    }

    [Fact]
    public void EmailDomain_SiteHasWwwPrefix_StillMatchesDomain_NoErrors()
    {
        var results = Validate(ValidVendor(site: "https://www.acme.com", email: "bob@acme.com"));
        Assert.Empty(results);
    }

    [Fact]
    public void EmailDomain_DomainComparison_IsCaseInsensitive_NoErrors()
    {
        // Site host is stored in mixed case; both sides are lowercased before comparison.
        var results = Validate(ValidVendor(site: "https://ACME.COM", email: "bob@acme.com"));
        Assert.Empty(results);
    }

    [Fact]
    public void EmailDomain_DoesNotMatchSiteDomain_FailsValidation()
    {
        var results = Validate(ValidVendor(site: "https://acme.com", email: "bob@other.com"));
        Assert.Contains(results,
            r => r.MemberNames.Contains(nameof(Vendor.PointOfContact)));
    }

    [Fact]
    public void EmailDomain_DoesNotMatchSiteDomain_ErrorMessageContainsBothDomains()
    {
        var results = Validate(ValidVendor(site: "https://acme.com", email: "bob@other.com"));
        var message = results.Single(r => r.MemberNames.Contains(nameof(Vendor.PointOfContact))).ErrorMessage;
        Assert.Contains("other.com", message);
        Assert.Contains("acme.com", message);
    }
}