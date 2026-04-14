using Microsoft.AspNetCore.Mvc;

namespace Software.Api;

public class PdfController : ControllerBase
{
    [HttpPost("/pdf")]
    public async Task<ActionResult> GeneratePdf([FromBody] string html, [FromServices] RemoteServiceClient remoteServiceClient)
    {

        // do your validation...
        var response = await remoteServiceClient.SendRequestAsync(new RemoteServiceClient.RemoteServiceRequest
        {
            FormType = "pdf",
            Body = html
        }, CancellationToken.None);

        return Ok(response);    
    }
}
