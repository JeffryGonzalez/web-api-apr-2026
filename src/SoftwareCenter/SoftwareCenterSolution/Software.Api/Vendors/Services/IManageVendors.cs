using Software.Api.Vendors.Models;

namespace Software.Api.Vendors.Services;

public interface IManageVendors
{
    Task<VendorCreateResponse> AddVendorAsync(Vendor request);
    Task<VendorCreateResponse?> GetVendorByIdAsync(Guid id, CancellationToken token);
    Task<IReadOnlyList<VendorSummaryItem>> GetVendorSummariesAsync(CancellationToken token);
}