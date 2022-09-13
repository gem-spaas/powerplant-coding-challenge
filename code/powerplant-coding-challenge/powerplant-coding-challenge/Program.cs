//-----------------------------------
// Imports
//-----------------------------------

using Newtonsoft.Json.Linq;

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
