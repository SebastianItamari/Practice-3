using UPB.CoreLogic.Models;
using Microsoft.Extensions.Configuration;

namespace UPB.CoreLogic.Managers;

public class PatientManager
{
    private List<Patient> _patients;
    private readonly String _path;

    public PatientManager(IConfiguration config)
    {
        _patients = new List<Patient>();
        _path = config.GetSection("PathPatients").Value;
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
        StreamWriter writer = new StreamWriter(_path, true);
        writer.WriteLine(createdPatient.CI + "," + createdPatient.Name + "," + createdPatient.LastName + "," + createdPatient.Group);
        writer.Close();

        return createdPatient;
    }

    public Patient Update(int ci, String name, String lastName)
    {
        if(ci < 0)
        {
            throw new Exception("CI invalid");
        }

        String[] lines = File.ReadAllLines(_path);
        String CI = ci.ToString();
        int index = -1;
        String group = "";

        for(int i = 0; i < lines.Length; i++)
        {
            String[] fields = lines[i].Split(',');
            if(fields[0] == CI)
            {
                index = i;
                group = fields[3];
                break;
            }
        }

        if(index == -1)
        {
            throw new Exception("Patient not found");
        } 
        else
        {
            String updatedPatient = (CI + "," + name + "," + lastName + "," + group);
            lines[index] = updatedPatient;
            File.WriteAllLines(_path, lines);
            Patient uPatient = new Patient(){ Name = name, LastName = lastName, CI = ci, Group = group };
            return uPatient;
        }
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
        StreamReader reader = new StreamReader(_path);
        while(!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] data = line.Split(',');
            Patient aux = new Patient(){ Name = data[1], LastName = data[2], CI = int.Parse(data[0]), Group = data[3] };
            _patients.Add(aux);
        }
        reader.Close();
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