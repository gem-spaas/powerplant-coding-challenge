  
The Production Controller.cs takes care of the post and get.
This takes the post of the payload calculates the result and saves it to JSON, whilst checking for errors.
The Payload.cs contains classes that correspond to the fuel, powerplant and load. unfourtunatly there are problems with parsing the fuel this is unreliable. I developed this on ubuntu with no intelisense or debugging availabillity as I no longer have acces to a windows enviroment with VS, so i wasn't able to find the root cause.
The Output.cs contains the model for the output result and for the calculated data on each powerstation.
The PowerComparison calculates the result, first it calculate the realative properties of each power station and converts from input types to string and double for processing. This data is storred in al ist for each power source. These lists are then ordered and joined by cost. Finally this list is looped through to give the relative contribution of each powerplant to the total grid power. The result from this is then written to json, in the post method in the production controller. The Get Method in the Production Controller then transmits the JSON on Request.

         
