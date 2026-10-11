using Application.Use_Cases.ProductionPlan;
using Domain.Entities;
using System.Reflection;

namespace ArchitectureTests
{
    public static class Assemblies
    {
        public static readonly Assembly ApplicationAssembly = typeof(CreateProductionPlanCommand).Assembly;

        public static readonly Assembly DomainAssembly = typeof(PowerPlant).Assembly;

    }
}
