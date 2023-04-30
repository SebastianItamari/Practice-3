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
        if(ci < 0)
        {
            throw new Exception("CI invalid");
        }

        Patient createdPatient = new Patient(){ Name = name, LastName = lastName, CI = ci, Group = GenerateGruop() };
        _patients.Add(createdPatient);
        return createdPatient;
    }

    public Patient Update(int ci, String name, String lastName)
    {
        if(ci < 0)
        {
            throw new Exception("CI invalid");
        }

        Patient patientFound = _patients.Find(patient => patient.CI == ci);

        if(patientFound == null)
        {
            throw new Exception("Patient not found");
        } 

        patientFound.Name = name;
        patientFound.LastName = lastName;
        return patientFound;
    }

    public Patient Delete(int ci)
    {
        int patientToDeleteIndex = _patients.FindIndex(patient => patient.CI == ci);

        if(patientToDeleteIndex == -1)
        {
            throw new Exception("Patient not found");
        }

        Patient patientToDelete = _patients[patientToDeleteIndex];
        _patients.RemoveAt(patientToDeleteIndex); 
        return patientToDelete;
    }

    public List<Patient> GetAll()
    {
        return _patients;
    }

    public Patient GetById(int ci)
    {
        Patient patientFound = _patients.Find(patient => patient.CI == ci);

        if(patientFound == null)
        {
            throw new Exception("Patient not found");
        }   

        return patientFound;
    }
}