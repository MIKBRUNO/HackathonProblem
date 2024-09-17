namespace HackathonProblem.Base.Utils;

using System.Data;
using HackathonProblem.Base.Concepts;
using Microsoft.VisualBasic.FileIO;

public static class Utils
{
    /// <summary>
    /// Считывает данные о сотрудников из csv файла в формате:
    /// Id;Name
    /// </summary>
    /// <param name="filepath">Путь до файла с сотрудниками</param>
    /// <returns>Коллекция сотрудников</returns>
    public static IEnumerable<Employee> ReadEmployeesFromCSV(string filepath)
    {
        List<Employee> employees = [];
        using (TextFieldParser csvParser = new TextFieldParser(filepath))
        {
            csvParser.Delimiters = [";"];
            // skip field names
            csvParser.ReadLine();
            while (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields() ?? throw new DataException();
                employees.Add(new Employee(int.Parse(fields[0]), fields[1]));
            }
        }
        return employees;
    }
}
