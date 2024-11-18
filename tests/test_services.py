import unittest
from app.services import calculate_power_distribution


class TestCalculatePowerDistribution(unittest.TestCase):
    """Test case for the calculate_power_distribution function.

    Args:
        unittest (TestCase): The base test case class.
    """

    def test_valid_distribution(self):
        """Test a valid power distribution scenario."""

        load = 480
        fuels = {"gas(euro/MWh)": 13.4, "kerosine(euro/MWh)": 50.8, "wind(%)": 60}
        powerplants = [
            {
                "name": "gasfiredbig1",
                "type": "gasfired",
                "efficiency": 0.53,
                "pmin": 100,
                "pmax": 460,
            },
            {
                "name": "gasfiredbig2",
                "type": "gasfired",
                "efficiency": 0.53,
                "pmin": 100,
                "pmax": 460,
            },
            {
                "name": "gasfiredsomewhatsmall",
                "type": "gasfired",
                "efficiency": 0.37,
                "pmin": 40,
                "pmax": 210,
            },
            {
                "name": "tj1",
                "type": "turbojet",
                "efficiency": 0.3,
                "pmin": 0,
                "pmax": 16,
            },
            {
                "name": "windpark1",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 0,
                "pmax": 150,
            },
            {
                "name": "windpark2",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 0,
                "pmax": 36,
            },
        ]
        expected_output = [
            {"name": "windpark1", "p": 90.0},
            {"name": "windpark2", "p": 21.6},
            {"name": "gasfiredbig1", "p": 368.4},
            {"name": "gasfiredbig2", "p": 0.0},
            {"name": "gasfiredsomewhatsmall", "p": 0.0},
            {"name": "tj1", "p": 0.0},
        ]
        result = calculate_power_distribution(load, fuels, powerplants)
        self.assertEqual(result, expected_output)

    def test_invalid_powerplant_type(self):
        """Test an invalid powerplant type."""

        load = 480
        fuels = {"gas(euro/MWh)": 13.4, "kerosine(euro/MWh)": 50.8, "wind(%)": 60}
        powerplants = [
            {
                "name": "some_plant",
                "type": "any_type",
                "efficiency": 0.53,
                "pmin": 100,
                "pmax": 460,
            }
        ]
        with self.assertRaises(ValueError):
            calculate_power_distribution(load, fuels, powerplants)

    def test_pmin_exceeds_pmax(self):
        """Test a powerplant with pmin exceeding pmax."""

        load = 480
        fuels = {"gas(euro/MWh)": 13.4, "kerosine(euro/MWh)": 50.8, "wind(%)": 60}
        powerplants = [
            {
                "name": "invalid_plant",
                "type": "gasfired",
                "efficiency": 0.53,
                "pmin": 500,
                "pmax": 460,
            }
        ]
        with self.assertRaises(ValueError):
            calculate_power_distribution(load, fuels, powerplants)

    def test_unable_to_satisfy_load(self):
        """Test a scenario where it is impossible to satisfy the load."""
        load = 1000  # Set a load that cannot be satisfied by the given powerplants
        fuels = {"gas(euro/MWh)": 13.4, "kerosine(euro/MWh)": 50.8, "wind(%)": 60}
        powerplants = [
            {
                "name": "gasfiredbig1",
                "type": "gasfired",
                "efficiency": 0.53,
                "pmin": 100,
                "pmax": 460,
            },
            {
                "name": "tj1",
                "type": "turbojet",
                "efficiency": 0.3,
                "pmin": 0,
                "pmax": 16,
            },
            {
                "name": "windpark1",
                "type": "windturbine",
                "efficiency": 1,
                "pmin": 0,
                "pmax": 150,
            },
        ]
        with self.assertRaises(ValueError):
            calculate_power_distribution(load, fuels, powerplants)


if __name__ == "__main__":
    unittest.main()
