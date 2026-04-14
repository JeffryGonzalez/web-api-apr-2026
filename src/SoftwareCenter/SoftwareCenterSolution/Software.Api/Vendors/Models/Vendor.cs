using System.ComponentModel.DataAnnotations;

namespace Software.Api.Vendors.Models;

public record Vendor : IValidatableObject
{
    [MinLength(3), MaxLength(20)]
    public required string Name { get; init; }
    [Url]
    public required string Site { get; init; }
    public required VendorPointOfContact PointOfContact { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Extract domain from site URL
        var siteUri = new Uri(Site);
        var siteDomain = siteUri.Host.ToLowerInvariant();
        
        // Remove 'www.' prefix if present for comparison
        if (siteDomain.StartsWith("www."))
        {
            siteDomain = siteDomain.Substring(4);
        }
        
        // Extract domain from email
        var emailParts = PointOfContact.Email.Split('@');
        if (emailParts.Length == 2)
        {
            var emailDomain = emailParts[1].ToLowerInvariant();
            
            if (siteDomain != emailDomain)
            {
                yield return new ValidationResult(
                    $"Email domain '{emailDomain}' does not match the vendor's site domain '{siteDomain}'.",
                    new[] { nameof(PointOfContact) }
                );
            }
        }
    }
}
