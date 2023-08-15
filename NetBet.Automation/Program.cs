

//



using NetBet.Automation.Scenario;

var baseUrl = "https://localhost:7138/api";


var staticDataScenario = new CarsStaticDataScenario(baseUrl);
await staticDataScenario.StartRunScenario();

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();

var rentCarScenario = new RentCarScenario(staticDataScenario.CarRequests,baseUrl);
await rentCarScenario.StartRunScenario();


Console.WriteLine();
Console.WriteLine();
Console.WriteLine();


var sanityScenario = new SanityScenario(staticDataScenario.CarRequests, baseUrl);
await sanityScenario.StartRunScenario();


Console.WriteLine("All test passed successfully!!!!!");
Console.WriteLine("All test passed successfully!!!!!");
Console.WriteLine("All test passed successfully!!!!!");
Console.WriteLine("All test passed successfully!!!!!");
Console.WriteLine("All test passed successfully!!!!!");
Console.WriteLine("All test passed successfully!!!!!");


Console.ReadLine();

