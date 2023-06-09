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
        Directory.CreateDirectory(Path.GetDirectoryName(_path));
        if(!File.Exists(_path))
        {
            StreamWriter aux = File.CreateText(_path);
            aux.Close();
        }
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

        StreamReader reader = new StreamReader(_path);
        String? line;
        while((line = reader.ReadLine()) != null)
        {
            String[] data = line.Split(',');

            if(data[0] == ci.ToString())
            {
                reader.Close();
                throw new Exception("The entered CI has already been registered");
            }
        }

        reader.Close();
        Patient createdPatient = new Patient(ci,name,lastName,GenerateGruop());
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
            return new Patient(ci,name,lastName,group);
        }
    }

    public Patient Delete(int ci)
    {
        if(ci < 0)
        {
            throw new Exception("CI invalid");
        }

        String tmp = "../Patients/temp.txt";
        String name = "";
        String lastName = "";
        String group = "";
        bool finded = false;
        StreamReader reader = new StreamReader(_path);
        StreamWriter writer = new StreamWriter(tmp, true);
        String? line;

        while((line = reader.ReadLine()) != null)
        {
            String[] data = line.Split(',');

            if(data[0] == ci.ToString())
            {
                finded = true;
                name = data[1];
                lastName = data[2];
                group = data[3];
            }
            else
            {
                writer.WriteLine(line);
            }
        }

        writer.Close();
        reader.Close();

        File.Delete(_path);
        File.Move(tmp, _path);

        if(finded == false)
        {
            throw new Exception("Patient not found");
        }
        else
        {
            return new Patient(ci,name,lastName,group);
        }
    }

    public List<Patient> GetAll()
    {
        StreamReader reader = new StreamReader(_path);
        String? line;
        while((line = reader.ReadLine()) != null)
        {
            String[] data = line.Split(',');
            Patient aux = new Patient(int.Parse(data[0]),data[1],data[2],data[3]);
            _patients.Add(aux);
        }
        reader.Close();
        return _patients;
    }

    public Patient GetById(int ci)
    {
        if(ci < 0)
        {
            throw new Exception("CI invalid");
        }
        
        StreamReader reader = new StreamReader(_path);
        String? line;
        while((line = reader.ReadLine()) != null)
        {
            String[] data = line.Split(',');

            if(data[0] == ci.ToString())
            {
                reader.Close();
                return new Patient(ci,data[1],data[2],data[3]);
            }
        }
        
        reader.Close();
        throw new Exception("Patient not found");
    }
}