namespace UPB.CoreLogic.Models;

public class Patient
{
    public int CI { get; set; }
    public String Name { get; set; }
    public String LastName { get; set; }
    public String Group { get; set; }

    public Patient(int ci, String name, String lastName, String group)
    {
        CI = ci;
        Name = name;
        LastName = lastName;
        Group = group;
    }
}