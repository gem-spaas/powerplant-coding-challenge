#-----------------------------------
# Imports
#-----------------------------------

from typing import Optional
from iphysicfactor import IPhysicFactor

#-----------------------------------
# Class
#-----------------------------------

class PowerPlant :
    """This is the class that represent a power plant."""

    # -------------
    # Fields
    # -------------

    __power_plant_index__: int
    """This is the power plant's index."""

    __power_plant_name__: str
    """This is the power plant's name."""

    __power_plant_type__: str
    """This is the power plant's type."""

    __power_plant_efficiency__: float
    """This is the power plant's efficiency."""

    __power_plant_p_min__: float
    """This is the power plant's minimal output power."""

    __power_plant_p_max__: float
    """This is the power plant's maximal output power."""

    __power_plant_price__: float
    """This is the power plant's price of production."""

    __power_plant_all_or_nothing_production__: bool
    """This is the power plant's mode of production. If it is true, the activation is 1 or 0."""

    __power_plant_physic_factors__: list[IPhysicFactor] = []
    """These are power plant's physic factors."""

    __power_plant_activation__: float = 0.0
    """This is the power plant's activation state."""

    # -------------
    # Constructors
    # -------------

    def __init__ (self, index: int, name: str, type: str, efficiency: float, pmin: float, pmax: float, price: float, aon: bool, physic_factors: Optional[list[IPhysicFactor]] = None) :
        
        self.__power_plant_index__ = index
        self.__power_plant_name__ = name
        self.__power_plant_type__ = type
        self.__power_plant_efficiency__ = efficiency
        self.__power_plant_p_min__ = pmin
        self.__power_plant_p_max__ = pmax
        self.__power_plant_price__ = price
        self.__power_plant_all_or_nothing_production__ = aon

        if not(physic_factors == None):

            for physic_factor in physic_factors :
                
                if not(physic_factor == None) :

                    self.__power_plant_physic_factors__.append(physic_factor)
    
    # -------------
    # Methods
    # -------------
    
    def compute_price_rate (self) -> float:
        """This method is used to compute and retrieve the price production of the power plant."""
        return self.__power_plant_price__ * self.__power_plant_efficiency__

    def compute_output_power (self) -> float:
        """This method is used to compute and retrieve the output power of the power plant."""
        
        power: float = 0.0

        if self.__power_plant_activation__ == 0:
            return power

        power = self.__power_plant_p_min__ + (self.__power_plant_activation__ * (self.__power_plant_p_max__ - self.__power_plant_p_min__))

        for pf in self.__power_plant_physic_factors__:
            power = pf.compute_power(power)

        return power
        

    # -------------
    # Getters
    # -------------

    def set_activation (self, activation: float) -> None:
        """This method is used to set the activation of the power plant."""
        
        if self.__power_plant_all_or_nothing_production__ :

            if activation <= 0 :
                self.__power_plant_activation__ = 0

            else :
                self.__power_plant_activation__ = 1

        else:

            if activation <= 0 :
                self.__power_plant_activation__ = 0

            elif activation >= 1 :
                self.__power_plant_activation__ = 1

            else:
                self.__power_plant_activation__ = activation

    # -------------
    # Getters
    # -------------

    def get_index (self) -> int :
        """This method is used to retrieve the index of the power plant."""
        return self.__power_plant_index__

    def get_name (self) -> str :
        """This method is used to retrieve the name of the power plant."""
        return self.__power_plant_name__

    def get_type (self) -> str :
        """This method is used to retrieve the type of the power plant."""
        return self.__power_plant_type__

    def get_efficiency (self) -> float:
        """This method is used to retrive the efficiency of the power plant."""
        return self.__power_plant_efficiency__

    def get_p_min (self) -> float:
        """This method is used to retrieve the minimum production power of the power plant."""
        return self.__power_plant_p_min__

    def get_p_max (self) -> float:
        """This method is used to retrieve the maximum production power of the power plant."""
        return self.__power_plant_p_max__

    def get_price (self) -> float:
        """This method is used to retrieve the price of the power plant."""
        return self.__power_plant_price__

    def get_production_mode (self) -> bool:
        """This method is used to retrieve the production mode. True : all or nothing | False : linear mode."""
        return self.__power_plant_all_or_nothing_production__

    def get_activation (self) -> float:
        """This method is used to retrieve the activation of the power plant."""
        return self.__power_plant_activation__