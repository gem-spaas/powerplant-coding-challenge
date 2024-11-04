import unittest


class TestIntegrationRunner(unittest.TestCase):

    def test_integration_testing(self):
        # initialize the test suite
        suite = unittest.TestSuite()

        # add tests to the test suite

        # initialize a runner, pass it your suite and run it
        runner = unittest.TextTestRunner(verbosity=3)
        result = runner.run(suite)

        if not result.wasSuccessful():
            exit(1)


if __name__ == "__main__":
    unittest.main()
