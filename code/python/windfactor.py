#-----------------------------------
# Imports
#-----------------------------------

from iphysicfactor import IPhysicFactor

#-----------------------------------
# Class
#-----------------------------------

class WindFactor (IPhysicFactor) :
    """This is the wind physic factor."""

    # -------------
    # Fields
    # -------------

    __wind_factor__: float = 0.0
    """This is the wind factor to apply to the power plants."""

    # -------------
    # Constructors
    # -------------

    def __init__ (self, wind_factor: float) :
        self.__wind_factor__ = wind_factor

    # -------------
    # Methods
    # -------------

    def compute_power (self, power_input: float) -> float :
        return power_input * self.__wind_factor__

    def get_efficiency (self) -> float :
        return self.__wind_factor__

    # -------------
    # Getters
    # -------------

    def get_wind_factor (self) -> float :
        """This method allows to retrieve the wind factor. Returns a float representing the wind factor."""
        return self.__wind_factor__

