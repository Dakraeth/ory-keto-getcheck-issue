using Microsoft.AspNetCore.Mvc;
using Ory.Keto.Client.Api;
using Ory.Keto.Client.Model;

namespace DotNetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OryKetoController : ControllerBase
{
    private readonly ReadApi _ketoReadClient;
    private readonly WriteApi _ketoWriteClient;


    public OryKetoController()
    {
        _ketoReadClient = new ReadApi(new Ory.Keto.Client.Client.Configuration
        {
            BasePath = "http://localhost:4466/",
        });

        _ketoWriteClient = new WriteApi(new Ory.Keto.Client.Client.Configuration
        {
            BasePath = "http://localhost:4467/"
        });
    }
    
    [HttpPost("CreateExampleRelationTuple")]
    public async Task<IActionResult> CreateExampleRelationTuple()
    {
        await _ketoWriteClient.CreateRelationTupleAsync(new KetoRelationQuery
        {
            Namespace = "app",
            Object = "WeatherForecast",
            SubjectId = "TestUserId",
            Relation = "view"
        });

        return Ok();
    }

    [HttpGet("GetCheckAsyncSuccess")]
    public async Task<IActionResult> GetCheckAsyncSuccess()
    {
        var isAllowed = await _ketoReadClient.GetCheckAsync(
            _namespace: "app",
            _object: "WeatherForecast",
            relation: "view",
            subjectId: "TestUserId");

        return Ok(isAllowed);
    }
    
    [HttpGet("GetCheckAsyncFailure")]
    public async Task<IActionResult> GetCheckAsyncFailure()
    {
        var isAllowed = await _ketoReadClient.GetCheckAsync(
            _namespace: "app",
            _object: "WeatherForecast",
            relation: "write",
            subjectId: "TestUserId");

        return Ok(isAllowed);
    }
    
    [HttpGet("GetCheckAsyncWorkaround")]
    public async Task<IActionResult> GetCheckAsyncWorkaround()
    {
        KetoGetCheckResponse isAllowed;
        try
        {
            isAllowed = await _ketoReadClient.GetCheckAsync(
                _namespace: "app", 
                _object: "WeatherForecast", 
                relation: "view", 
                subjectId: "TestUserId");
        }
        catch (Exception e)
        {
            isAllowed = new KetoGetCheckResponse
            {
                Allowed = false
            };
        }

        return Ok(isAllowed);
    }
}