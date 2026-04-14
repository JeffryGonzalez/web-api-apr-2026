using Software.Api.Vendors.Models;

namespace Software.Api.Vendors.Services;

public class SqlPolecatVendorData : IManageVendors
{
    public Task<VendorCreateResponse> AddVendorAsync(Vendor request)
    {
        throw new NotImplementedException();
    }

    public Task<VendorCreateResponse?> GetVendorByIdAsync(Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<VendorSummaryItem>> GetVendorSummariesAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
