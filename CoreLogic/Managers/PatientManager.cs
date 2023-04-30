using UPB.CoreLogic.Models;

namespace UPB.CoreLogic.Managers;

public class PatientManager
{
    private List<Patient> _patients;

    public PatientManager()
    {
        _patients = new List<Patient>();
    }

    public String GenerateGruop()
    {
        Random random = new Random();
        int randomNumber = random.Next(0,8);

        switch(randomNumber)
        {
            case 0:
                return "A+";
            case 1:
                return "B+";
            case 2:
                return "AB+";
            case 3:
                return "O+";
            case 4:
                return "A-";
            case 5:
                return "B-";
            case 6:
                return "AB-";
            default:
                return "O-";
        }
    }

    public Patient Create(String name, String lastName, int ci)
    {
        Patient createdPatient = new Patient(){ Name = name, LastName = lastName, CI = ci, Group = GenerateGruop() };
        _patients.Add(createdPatient);
        return createdPatient;
    }

    //Falta ver excepciones
    public Patient Update(int ci, String name, String lastName)
    {
        Patient patientFound = _patients.Find(patient => patient.CI == ci);
        patientFound.Name = name;
        patientFound.LastName = lastName;
        return patientFound;
    }

    //Falta ver excepciones
    public Patient Delete(int ci)
    {
        int patientToDeleteIndex = _patients.FindIndex(patient => patient.CI == ci);
        Patient patientToDelete = _patients[patientToDeleteIndex];
        _patients.RemoveAt(patientToDeleteIndex); 
        return patientToDelete;
    }
}