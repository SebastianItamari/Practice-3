using Microsoft.AspNetCore.Mvc;
using UPB.CoreLogic.Managers;
using UPB.CoreLogic.Models;

namespace UPB.PracticeThree.Controllers;

[ApiController]
[Route("patients")]
public class PatientController : ControllerBase
{
    private readonly PatientManager _patientManager;
    public PatientController(PatientManager patientManager)
    {
        _patientManager = patientManager;
    }

    [HttpPost]
    public Patient Post([FromBody]Patient patientToCreate)
    {
        return _patientManager.Create(patientToCreate.Name, patientToCreate.LastName, patientToCreate.CI);
    }

    [HttpPut]
    [Route("{ci}")]
    public Patient Put([FromRoute] int ci, [FromBody]Patient patientToUpdate)
    {
        return _patientManager.Update(ci, patientToUpdate.Name, patientToUpdate.LastName);
    }
}
