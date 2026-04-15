namespace Software.Api.Catalog;

public static class Extensions
{
    extension(IEndpointRouteBuilder builder)
    {
       public IEndpointRouteBuilder MapCatalog()
        {

            var group = builder.MapGroup("catalog");

            // GET /catalog
            group.MapGet("", () => Results.Ok(new[] { "item1", "item2", "item3" }));
            group.MapGet("{id}", (int id) => Results.Ok($"You requested item {id}"));

            group.MapPost("", (string item) => Results.Ok($"You posted {item}"));
            return builder;
        }
    }
}
