import pandas as pd


C02_per_tonn = 0.3


# My implementation of merit order:
def mo_alg(load, fuels, powerplants):
    # Here I adjust the costs
    def cost(row):
        if row['type'] == 'gasfired':
            cost_modification = fuels['gas(euro/MWh)'] / row['efficiency'] + fuels['co2(euro/ton)'] * C02_per_tonn
        elif row['type'] == 'turbojet':
            cost_modification = fuels['kerosine(euro/MWh)'] / row['efficiency']
        elif row['type'] == 'windturbine':
            cost_modification = 0
        else:
            # This will raise Unknown fuel type error later
            cost_modification = -1
        return cost_modification
    try:

        df = pd.DataFrame(powerplants)

        # Taking wind into account for windturbine
        if 'wind(%)' in fuels:
            wind = fuels['wind(%)']/100
            df['pmax'] = df.apply(lambda x: round(x['pmax'] * wind, 2) if x['type'] == 'windturbine' else x['pmax'],
                                  axis=1)

        df['cost'] = df.apply(lambda x: cost(x), axis=1)
        df = df.sort_values('cost')
        # Now the DF is ready as a table of power stations sorted by their cost efficiency
        # print(df)

        # Check if we knew all fuel types
        known_fuel = True
        if len(df[df['cost'] < 0].index) > 0:
            known_fuel = False

        result = []
        TOTAL_SUM = load
        check_sum = 0

        if TOTAL_SUM < df['pmax'].sum() and known_fuel:
            log = 'All good'
            # Calculate load for each power station
            for i, row in df.iterrows():
                if TOTAL_SUM > row['pmax']:
                    pp_dict = {'name': row['name'], 'p': row['pmax']}
                    result.append(pp_dict)
                    TOTAL_SUM = TOTAL_SUM - row['pmax']
                    check_sum = check_sum + row['pmax']
                elif row['pmax'] > TOTAL_SUM > row['pmin']:
                    pp_dict = {'name': row['name'], 'p': TOTAL_SUM}
                    result.append(pp_dict)
                    check_sum = check_sum + TOTAL_SUM
                    TOTAL_SUM = 0
                else:
                    pp_dict = {'name': row['name'], 'p': 0}
                    result.append(pp_dict)
            if check_sum != load:
                # print(check_sum, load)
                # print(result)
                result = []
                # Mostly caused by power plants with unsuitable minimum power
                log = 'The set of power plants cannot match the required power'
        else:
            result = []
            log = 'Not enough power, build more power plants'
            if not known_fuel:
                log = 'Unknown fuel type'
    except Exception as err:
        # Handles all other errors
        result = []
        log = 'Error with ' + str(err)

    return result, log
