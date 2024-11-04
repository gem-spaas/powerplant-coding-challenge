import unittest

from tests.unitary.api import test_api


class TestUnitRunner(unittest.TestCase):

    def test_unit_testing(self):
        # initialize the test suite
        loader = unittest.TestLoader()
        suite = unittest.TestSuite()

        # add tests to the test suite
        suite.addTests(loader.loadTestsFromModule(test_api))
        # initialize a runner, pass it your suite and run it
        runner = unittest.TextTestRunner(verbosity=3)
        result = runner.run(suite)

        if not result.wasSuccessful():
            exit(1)


if __name__ == "__main__":
    unittest.main()
