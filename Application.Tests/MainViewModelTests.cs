using Core;
using Moq;
using NUnit.Framework;
using SampleApplication;
using SampleApplication.Commands;
using SampleApplication.Models;
using SampleApplication.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        private Mock<IRepository> _repoMock;
        private Mock<IUpdateProviderLocationsCommand> _updateLocationsMock;
        private MainViewModel _viewModel;

        [Test]
        public async Task Initialize_Call_Pass()
        {
            //Arrange
            FetchModelCollectionResult<Appointment> fetchResult = new FetchModelCollectionResult<Appointment>();
            fetchResult.ModelCollection = new List<Appointment> { new Appointment { Name = "Item 1" } };
            _repoMock.Setup(x => x.FetchAppointmentsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new FetchModelCollectionResult<Appointment>());

            //Act
            await _viewModel.InitializeAsync(null);

            //Assert
            _repoMock.VerifyAll();
            Assert.NotNull(_viewModel.Appointments, "Expected collection not to be null");
            Assert.IsTrue(_viewModel.Appointments.Count > 0, "Expected some sample items");
            Assert.IsTrue(_viewModel.Appointments.Contains(fetchResult.ModelCollection[0]), "Expected SampleItems to contain the correct values");
        }

        [SetUp]
        public void TestSetup()
        {
            _repoMock = new Mock<IRepository>();
            _updateLocationsMock = new Mock<IUpdateProviderLocationsCommand>();
            _viewModel = new MainViewModel(_repoMock.Object, _updateLocationsMock.Object);
        }
    }
}