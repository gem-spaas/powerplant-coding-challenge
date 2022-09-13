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
/// <summary>
/// This si the method used to read the production data, compute the activation of the prodctuors and export the data.
/// </summary>
String ProductionplanMethod (String fileName)
{
    // FCTN 01 - read data
    if (!File.Exists(fileName))
    {
        return "Input file not found";
    }
    String fileDataReaded = File.ReadAllText(fileName);
    JToken? jsonReaded = null;
    try
    {
        jsonReaded = JValue.Parse(fileDataReaded);
    }
    catch (Exception ex)
    {
        return "Exception while parsing file : " + ex.Message;
    }

    if (jsonReaded == null)
    {
        return "Impossible to parse input file";
    }

    // FCTN 02 - parsing data
    // Fuels
    JToken fuelsData = jsonReaded["fuels"];

    if (fuelsData == null)
    {
        return "Fuels not found in input file";
    }

    JValue gas = (JValue) fuelsData["gas(euro/MWh)"];
    JValue kerosine = (JValue) fuelsData["kerosine(euro/MWh)"];
    JValue co2 = (JValue) fuelsData["co2(euro/ton)"];
    JValue wind = (JValue) fuelsData["wind(%)"];

    InputFuels inputFuels = new InputFuels(
        Convert.ToDouble(gas.Value),
        Convert.ToDouble(kerosine.Value),
        Convert.ToDouble(co2.Value),
        Convert.ToDouble(wind.Value)
    );

    // Power plants
    JToken powerPlantsData = jsonReaded["powerplants"];
    IList<InputProductor> inputProductors = new List<InputProductor>();

    if (powerPlantsData == null)
    {
        return "Power plants not found in input file";
    }

    foreach (var powerPlant in powerPlantsData)
    {
        JValue nameValue = (JValue)(powerPlant["name"]);
        JValue typeValue = (JValue)(powerPlant["type"]);
        JValue efficiencyValue = (JValue)(powerPlant["efficiency"]);
        JValue pminValue = (JValue)(powerPlant["pmin"]);
        JValue pmaxValue = (JValue)(powerPlant["pmax"]);

        inputProductors.Add(new InputProductor(
            nameValue.ToString(),
            typeValue.ToString(),
            Convert.ToDouble(efficiencyValue.Value),
            Convert.ToDouble(pminValue.Value),
            Convert.ToDouble(pmaxValue.Value)
        ));
    }

    Input inputData = new Input(Convert.ToDouble(((JValue)jsonReaded["load"]).Value), inputFuels, inputProductors);

    // FCTN 03 - activate productors
    Double loadLeft = inputData.Load;

    // FCTN 03.01 - Create productors list
    Int32 index = 0;
    List<Productor> productors = new List<Productor>();
    List<IPhysicFactor> windFactor = new List<IPhysicFactor>()
    {
        new WindFactor(inputFuels.WindPercent / 100)
    };

    foreach (InputProductor inputProductor in inputProductors)
    {
        index += 1;
        if (inputProductor.Type == "gasfired")
        {
            productors.Add(new Productor(index, inputProductor.Name, inputProductor.Type, inputProductor.Efficiency, inputProductor.PMin, inputProductor.PMax, inputFuels.GasEuroMWH, false));
        }
        else if (inputProductor.Type == "turbojet")
        {
            productors.Add(new Productor(index, inputProductor.Name, inputProductor.Type, inputProductor.Efficiency, inputProductor.PMin, inputProductor.PMax, inputFuels.KersosinEuroMWH, false));
        }
        else if (inputProductor.Type == "windturbine")
        {
            productors.Add(new Productor(index, inputProductor.Name, inputProductor.Type, inputProductor.Efficiency, inputProductor.PMin, inputProductor.PMax, 0, true, windFactor));
        }
    }

    // FCTN 03.02 - Sort list by lower prices.
    productors.Sort(delegate(Productor p1, Productor p2)
    {
        if (p1.PriceRate == p2.PriceRate)
        {
            return p1.Index - p2.Index;
        }
        else
        {
            return Convert.ToInt32(p1.PriceRate - p2.PriceRate);
        }
    });

    // FCTN 03.03 - Activate the lower prices first.
    foreach (Productor productor in productors)
    {
        if (productor.PriceRate == 0)
        {
            if (productor.AON)
            {
                productor.Activation = 1;
                loadLeft -= productor.OutputPower;
                if (loadLeft < 0)
                {
                    loadLeft += productor.OutputPower;
                    productor.Activation = 0;
                }
            }
            else
            {
                if (loadLeft > productor.PMax)
                {
                    productor.Activation = 1;
                }
                else if (loadLeft < productor.PMin)
                {
                    productor.Activation = 0;
                }
                else
                {
                    productor.Activation = loadLeft - productor.PMin / (productor.PMax - productor.PMin);
                }
                loadLeft -= productor.OutputPower;
            }
        }
        else
        {
            break;
        }
    }

    // FCTN 03.04 - If power left activate higher price
    if (loadLeft > 0)
    {
        foreach (Productor productor in productors)
        {
            if (loadLeft == 0)
            {
                break;
            }

            if (productor.PriceRate == 0)
            {
                continue;
            }
            
            if (productor.AON)
            {
                productor.Activation = 1;
                loadLeft -= productor.OutputPower;
                if (loadLeft < 0)
                {
                    loadLeft += productor.OutputPower;
                    productor.Activation = 0;
                }
            }
            else
            {
                if (loadLeft > productor.PMax)
                {
                    productor.Activation = 1;
                }
                else if (loadLeft < productor.PMin)
                {
                    productor.Activation = 0;
                }
                else
                {
                    productor.Activation = (loadLeft - productor.PMin) / (productor.PMax - productor.PMin);
                }
                loadLeft -= productor.OutputPower;
            }
        }
    }

    // FCTN 03.05 - If impossible due to pmin, deactivate last AON in list and activate low price linear producer.
    while (loadLeft > 0)
    {
        for (int i = productors.Count - 1; i >= 0; i--)
        {
            if (productors[i].AON && productors[i].Activation == 1)
            {
                loadLeft += productors[i].OutputPower;
                productors[i].Activation = 0;
            }
        }

        foreach (Productor productor in productors)
        {
            if (productor.AON)
            {
                continue;
            }
            else
            {
                if (loadLeft > productor.PMax)
                {
                    productor.Activation = 1;
                }
                else if (loadLeft < productor.PMin)
                {
                    productor.Activation = 0;
                }
                else
                {
                    productor.Activation = (loadLeft - productor.PMin) / (productor.PMax - productor.PMin);
                }
                loadLeft -= productor.OutputPower;
            }
        }
    }

    // FCTN 03.06 - Sort list by index.
    productors.Sort(delegate(Productor p1, Productor p2)
    {
        return p1.Index - p2.Index;
    });

    // FCTN 04 - jsonify data
    List<OutputProductor> outputProductors = new List<OutputProductor>();

    foreach (Productor productor in productors)
    {
        outputProductors.Add(new OutputProductor(productor.Name, productor.OutputPower));
    }

    String jsonExport = JValue.FromObject(outputProductors).ToString();

    // FCTN 05 - write data
    try
    {
        File.WriteAllText(OUTPUT_FILE_NAME, jsonExport);
    }
    catch (Exception ex)
    {
        return "Exception while writing file : " + ex.Message;
    }

    return "SUCCESS";
}

/// <summary>
/// This is the method used to test the classes and the functionnalities.
/// </summary>
String UnitTest ( )
{
    // TEST 01
    Productor p1 = new Productor(0, "gasfiredbig1", "gasfired", 0.53, 100, 460, 10);
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

    Productor p2 = new Productor(1, "windpark1", "windturbine", 1, 0, 150, 0, true, windFactor);
    p2.Activation = 1;

    Double expectedP2Power = 90;
    Double actualP2Power = p2.OutputPower;

    Double errorP2 = expectedP2Power - actualP2Power;

    if (errorP2 != 0)
    {
        return "The power computed for p2 is different than the expected one !";
    }


    return "Tests passed !";
}

//-----------------------------------
// Routes
//-----------------------------------

/// <summary>
/// This is the required first route.
/// </summary>
app.MapGet("/productionplan", () => ProductionplanMethod(INPUT_FILE_NAME));

/// <summary>
/// This is the route used to test the classes created.
/// </summary>
app.MapGet("/test", () => UnitTest());

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
