using Test2;

const string filePath = "data.txt";
var data = File.ReadAllLines(filePath);

var occupants = data.Select(line =>
{
    var parts = line.Split(new[] { "\",\"" }, StringSplitOptions.None)
        .Select(p => p.Trim('"'))
        .ToArray();

    return new Occupant
    {
        FirstName = parts[0],
        LastName = parts[1],
        Address = $"{parts[2]}, {parts[3]}, {parts[4]}",
        Age = int.Parse(parts[5])
    };
}).ToList();

var groupedData = occupants.GroupBy(occupant => NormalizeAddress(occupant.Address))
    .Select(group => new
    {
        Id = group.Key,
        Count = group.Count(),
        Occupants = group.Where(o => o.Age >= 19)
            .OrderBy(o => o.LastName)
            .ThenBy(o => o.FirstName)
            .ToList()
    })
    .OrderBy(g => g.Id);

foreach (var group in groupedData)
{
    Console.WriteLine($"{group.Id} {group.Count}");
    foreach (var occupant in group.Occupants)
    {
        Console.WriteLine($"    {occupant.FirstName} {occupant.LastName} {occupant.Address} {occupant.Age}");
    }
}

return;

static string NormalizeAddress(string address) =>
    address.ToLower().Replace(" ", "").Replace(".", "").Replace(",", "");