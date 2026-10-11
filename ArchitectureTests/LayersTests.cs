using NetArchTest.Rules;

namespace ArchitectureTests
{
    public class LayersTests
    {
        [Fact]
        public void DomainShouldNotReferenceOtherLayers()
        {
            //Arrange & Act
            var result = Types
                .InAssembly(Assemblies.DomainAssembly)
                .ShouldNot()
                .OnlyHaveDependenciesOn("SharedKernel")
                .GetResult();

            //Assert

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ApplicationShouldOnlyReferenceDomainLayer()
        {
            //Arrange & Act
            var result = Types
                .InAssembly(Assemblies.ApplicationAssembly)
                .ShouldNot()
                .HaveDependencyOn("ProductionPlanHost")
                .GetResult();

            ///Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void CommandHandlerMustEndWithHandler()
        {
            //Arrange & Act
            var result = Types
                .InAssembly(Assemblies.ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(MediatR.IRequestHandler<,>))
                .Should()
                .HaveNameEndingWith("Handler")
                .GetResult();
            //Assert
            Assert.True(result.IsSuccessful);

        }
    }
}