using Marten;
using Software.Api.Vendors.Data;
using Software.Api.Vendors.Models;
using static System.Collections.Specialized.BitVector32;

namespace Software.Api.Vendors.Services;

public class PostgresMartenVendorData(IDocumentSession session) : IManageVendors
{
    public async Task<IReadOnlyList<VendorSummaryItem>> GetVendorSummariesAsync(CancellationToken token)
    {
        
        return await session.Query<VendorEntity>()
             .Select(e => new VendorSummaryItem
             {
                 Id = e.Id,
                 Name = e.Name
             })
             .ToListAsync(token);
    }

    public async Task<VendorCreateResponse> AddVendorAsync(Vendor request)
    {
        var entity = new VendorEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTimeOffset.UtcNow,

            CreatedBy = "TODO: Get the user from the auth context",
            Name = request.Name,
            Site = request.Site,
            PointOfContact = request.PointOfContact
        };

        // save that entity into the database
        // Martin Fowler "Transaction Script" pattern: https://martinfowler.com/eaaCatalog/transactionScript.html
        session.Store(entity);
        // add another thing to another table,
        // also update this other table on this database 
        await session.SaveChangesAsync(); // Engage! Make it so!

        var response = new VendorCreateResponse
        {
            Id = entity.Id,
            Created = entity.Created,
            Name = entity.Name,
            Site = entity.Site,
            PointOfContact = entity.PointOfContact
        };
        return response;
    }

    public async Task<VendorCreateResponse?> GetVendorByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await session.LoadAsync<VendorEntity>(id, token);

        if (entity is null)
        {
            return null;
        }
        return new VendorCreateResponse
        {
            Id = entity.Id,
            Created = entity.Created,
            Name = entity.Name,
            Site = entity.Site,
            PointOfContact = entity.PointOfContact
        };
    }
}

