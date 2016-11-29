using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;

namespace $safeprojectname$.TestHelp
{
    public class Gen : AutoDataAttribute
    {
        public Gen(string databaseConnection = null) : base(new Fixture().Customize(
            new CompositeCustomization(
                new DatabaseConfiguredCustomization(databaseConnection),
                new AutoMoqCustomization(),
                new WebApiCustomization())))
        { }
    }
}
