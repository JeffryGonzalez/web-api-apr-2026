namespace Software.Api.Vendors.Services;

public class HttpContextCallingUserProvider(IHttpContextAccessor httpContextAccessor) : IProvideTheCallingUser
{
    public string GetSubClaim()
    {
        if(httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException("Not to be used outside of an HTTP request context");
        }

        // The below is how a lot of folks do it - Microsoft maps JWT claims to claims that it uses and has used since before JWTs, this is how you'd
        // do it if you turned that off

        //return httpContextAccessor.HttpContext.User.Claims.SingleOrDefault( c => c.Type == "sub")?.Value
        //    ?? throw new InvalidOperationException("No sub claim found for the calling user");

        // This is the default - if you aren't telling it to map claims.
        return httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    }
}
