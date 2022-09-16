#-----------------------------------
# Imports
#-----------------------------------

from math import gcd
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

    __power_plant_physic_factors__: list[IPhysicFactor]
    """These are power plant's physic factors."""

    __power_plant_activation__: float
    """This is the power plant's activation state."""

    __power_plant_computed_price_rate__: Optional[float] 
    """This is the computed price rate of the power plant."""

    __power_plant_computed_power__: Optional[float]
    """This is the computed output power of the power plant."""

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
        self.__power_plant_physic_factors__ = []
        self.__power_plant_activation__ = 0.0
        self.__power_plant_computed_price_rate__ = None
        self.__power_plant_computed_power__ = None

        if physic_factors:

            for physic_factor in physic_factors :
                
                if physic_factor :

                    self.__power_plant_physic_factors__.append(physic_factor)
    
    # -------------
    # Methods
    # -------------
    
    def compute_price_rate (self) -> float:
        """This method is used to compute and retrieve the price production of the power plant."""
        
        if self.__power_plant_computed_price_rate__ is None:
            self.__power_plant_computed_price_rate__ = self.__power_plant_price__ * self.__power_plant_efficiency__

        return self.__power_plant_computed_price_rate__

    def compute_output_power (self) -> float:
        """This method is used to compute and retrieve the output power of the power plant."""
        
        if self.__power_plant_computed_power__ is None:

            power: float = 0.0

            if not (self.__power_plant_activation__ == 0):

                power = self.__power_plant_p_min__ + (self.__power_plant_activation__ * (self.__power_plant_p_max__ - self.__power_plant_p_min__))

                for pf in self.__power_plant_physic_factors__:
                    power = pf.compute_power(power)

            self.__power_plant_computed_power__ = power
        
        return self.__power_plant_computed_power__
    
    def compute_activation (self, load_aimed) -> float:
        """This method is used to compute and set the activation of the power plant in fonction of the loaded aimed. Returns the output power of the power plant."""

        efficiency_factor = 1.0
        for physical_factor in self.__power_plant_physic_factors__ :
            efficiency_factor *= physical_factor.get_efficiency()

        if self.__power_plant_all_or_nothing_production__ :
            
            if load_aimed > (self.__power_plant_p_max__ * efficiency_factor) :
                self.set_activation(1)
            
            else :
                self.set_activation(0)

        else :

            if load_aimed > (self.__power_plant_p_max__ * efficiency_factor) :
                self.set_activation(1)

            elif load_aimed < (self.__power_plant_p_min__ * efficiency_factor) :
                self.set_activation(0)
            
            else :
                self.set_activation(((load_aimed - self.__power_plant_p_min__) / (self.__power_plant_p_max__ - self.__power_plant_p_min__)) / efficiency_factor)

        return self.compute_output_power()

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

        self.__power_plant_computed_power__ = None


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

    def get_activation (self) -> float:
        """This method is used to retrieve the activation of the power plant."""
        return self.__power_plant_activation__

    def is_all_or_nothing (self) -> bool:
        """This method is used to retrieve the production mode. True : all or nothing | False : linear mode."""
        return self.__power_plant_all_or_nothing_production__