# powerplant-coding-challenge

*François Le Roy*
## How to use

```bash
cd powerplant-coding-challenge-implementation
dotnet run powerplant-coding-challenge-implementation.csproj
```
## How to test
```bash
curl -X POST https://localhost:8888/productionplan -H "Content-Type: application/json" -d @../example_payloads/payload1.json
```
## My choices for the challenge
* I did choose C# to do the challenge as I am more confortable with it.  I did use dotnet 6 as it is LTS version.
* I did use the difference between production and comsuption rate in order to sort the powerplant with the minimum comsuption first.
* I tried to do a code as clean as possible but I did not have the time to do what I wanted, and so the actual version is more like a draft. I did however tried to separate logics using two services (one to order the plants, one to assign the load) and injecting them with DI.

