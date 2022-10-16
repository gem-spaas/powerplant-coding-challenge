from rest_framework import serializers


class FuelSerializer(serializers.Serializer):
    def __init__(self, *args, **kwargs):
        super(FuelSerializer, self).__init__(*args, **kwargs)
        self.fields["gas(euro/MWh)"] = serializers.DecimalField(decimal_places=1, max_digits=5)
        self.fields["kerosine(euro/MWh)"] = serializers.DecimalField(decimal_places=1, max_digits=5)
        self.fields["co2(euro/ton)"] = serializers.DecimalField(decimal_places=1, max_digits=5)
        self.fields["wind(%)"] = serializers.DecimalField(decimal_places=1, max_digits=5, max_value=100, min_value=0)


class PowerPlantSerializer(serializers.Serializer):
    name = serializers.CharField()
    type = serializers.CharField()
    efficiency = serializers.DecimalField(decimal_places=2, max_digits=3)
    pmin = serializers.IntegerField()
    pmax = serializers.IntegerField()


class ProductionPlanSerializer(serializers.Serializer):
    load = serializers.IntegerField()
    fuels = FuelSerializer()
    powerplants = PowerPlantSerializer(many=True, required=True)
