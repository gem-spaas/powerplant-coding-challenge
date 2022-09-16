#-----------------------------------
# Imports
#-----------------------------------

#-----------------------------------
# Class
#-----------------------------------

class IPhysicFactor :
    """Is the common pseudo interface that is used to affect the power plants with physic factors."""

    # -------------
    # Constructors
    # -------------

    def __init__ (self) :
        ...

    # -------------
    # Methods
    # -------------

    def compute_power (self, power_input: float) -> float :
        """This method is used to compute the output power of the power plant by affecting the physic factor."""

        return 0.0