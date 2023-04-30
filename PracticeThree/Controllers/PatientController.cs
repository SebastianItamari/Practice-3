using Microsoft.AspNetCore.Mvc;
using UPB.CoreLogic.Managers;
using UPB.CoreLogic.Models;
using Serilog;

namespace UPB.PracticeThree.Controllers;

[ApiController]
[Route("patients")]
public class PatientController : ControllerBase
{
    private readonly PatientManager _patientManager;
    private readonly ILogger<PatientController> _logger;
    public PatientController(PatientManager patientManager, ILogger<PatientController> logger)
    {
        _patientManager = patientManager;
        _logger = logger;
    }

    [HttpPost]
    public Patient Post([FromBody]Patient patientToCreate)
    {
        _logger.LogInformation("Execute HTTP POST");
        return _patientManager.Create(patientToCreate.Name, patientToCreate.LastName, patientToCreate.CI);
    }

    [HttpPut]
    [Route("{ci}")]
    public Patient Put([FromRoute] int ci, [FromBody]Patient patientToUpdate)
    {
        _logger.LogInformation("Execute HTTP PUT");
        return _patientManager.Update(ci, patientToUpdate.Name, patientToUpdate.LastName);
    }

    [HttpDelete]
    [Route("{ci}")]
    public Patient Delete([FromRoute] int ci)
    {
        _logger.LogInformation("Execute HTTP DELETE");
        return _patientManager.Delete(ci);
    }

    [HttpGet]
    public List<Patient> Get()
    {
        _logger.LogInformation("Execute HTTP GET");
        return _patientManager.GetAll();  
    }

    [HttpGet]
    [Route("{ci}")]
    public Patient GetById([FromRoute] int ci)
    {
        _logger.LogInformation("Execute HTTP GET by CI");
        return _patientManager.GetById(ci);  
    }
}
