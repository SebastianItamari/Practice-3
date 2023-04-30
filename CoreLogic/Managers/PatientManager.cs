using UPB.CoreLogic.Models;

namespace UPB.CoreLogic.Managers;

public class PatientManager
{
    private List<Patient> _patients;

    public PatientManager()
    {
        _patients = new List<Patient>();
    }
}