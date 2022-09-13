//-----------------------------------
// Imports
//-----------------------------------

using Newtonsoft.Json.Linq;

using powerplant_coding_challenge;

//-----------------------------------
// Fields
//-----------------------------------

/// <summary>
/// This is the builder that allows to instanciate the application launhcer.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// This is the application.
/// </summary>
var app = builder.Build();

/// <summary>
/// This is the input file used as an input.
/// </summary>
const String INPUT_FILE_NAME = "payload1.json";

/// <summary>
/// This is the output file used as an output.
/// </summary>
const String OUTPUT_FILE_NAME = "example_response.json";

//-----------------------------------
// Functions
//-----------------------------------

String ProductionplanMethod(String fileName)
{
    // TEST 01
    Productor p1 = new Productor("gasfiredbig1", "gasfired", 0.53, 100, 460);
    p1.Activation = 1;

    Double expectedP1Power = 243.8;
    Double actualP1Power = p1.OutputPower;

    Double errorP1 = expectedP1Power - actualP1Power;

    if (errorP1 != 0)
    {
        return "The power computed for p1 is different than the expected one !";
    }

    // TEST 02
    List<IPhysicFactor> windFactor = new List<IPhysicFactor>()
    {
        new WindFactor(0.60)
    };

    Productor p2 = new Productor("windpark1", "windturbine", 1, 0, 150, windFactor);
    p2.Activation = 1;

    Double expectedP2Power = 90;
    Double actualP2Power = p2.OutputPower;

    Double errorP2 = expectedP2Power - actualP2Power;

    if (errorP2 != 0)
    {
        return "The power computed for p2 is different than the expected one !";
    }


    return "Hello World !";
}

//-----------------------------------
// Routes
//-----------------------------------

/// <summary>
/// This is the required first route.
/// </summary>
app.MapGet("/productionplan", () => ProductionplanMethod(INPUT_FILE_NAME));

/// <summary>
/// This is the route in order to know that everything work.
/// </summary>
app.MapGet("/", () => "Hello World !");

//-----------------------------------
// Main
//-----------------------------------

/// <summary>
/// This is the launching of the application.
/// </summary>
app.Run();
