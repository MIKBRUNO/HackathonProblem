using Csv;

namespace HackathonProblem.Implementations;

/// <summary>
/// Reads employees from csv file:
/// Id;Name
/// 1;Employee1
/// </summary>
public class CSVEmployeeProvider : IEmployeeProvider
{
    private readonly IEnumerable<IEmployee> employees;
    
    public CSVEmployeeProvider(string filepath)
    {
        var stream = File.OpenRead(filepath);
        var options = new CsvOptions
        {
            Separator = ';',
            HeaderMode = HeaderMode.HeaderPresent
        };
        List<IEmployee> list = [];
        foreach (var line in CsvReader.ReadFromStream(stream, options))
        {
            list.Add(new Employee(int.Parse(line["Id"]), line["Name"]));
        }
        employees = list;
    }

    public IEnumerable<IEmployee> GetEmployees()
    {
        return employees;
    }
}
