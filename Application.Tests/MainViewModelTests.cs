using Core;
using Moq;
using NUnit.Framework;
using SampleApplication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        private Mock<IRepository> _repoMock;
        private MainViewModel _viewModel;

        [Test]
        public async Task Initialize_Call_Pass()
        {
            //Arrange
            FetchModelCollectionResult<SampleItem> fetchResult = new FetchModelCollectionResult<SampleItem>();
            fetchResult.ModelCollection = new List<SampleItem> { new SampleItem { Name = "Item 1" } };
            _repoMock.Setup(x => x.FetchSampleItemsAsync()).ReturnsAsync(new FetchModelCollectionResult<SampleItem>());

            //Act
            await _viewModel.InitializeAsync(null);

            //Assert
            _repoMock.VerifyAll();
            Assert.NotNull(_viewModel.SampleItems, "Expected collection not to be null");
            Assert.IsTrue(_viewModel.SampleItems.Count > 0, "Expected some sample items");
            Assert.IsTrue(_viewModel.SampleItems.Contains(fetchResult.ModelCollection[0]), "Expected SampleItems to contain the correct values");
        }

        [SetUp]
        public void TestSetup()
        {
            _repoMock = new Mock<IRepository>();
            _viewModel = new MainViewModel(_repoMock.Object);
        }
    }
}