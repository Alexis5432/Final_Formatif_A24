using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    Mock<SeatsService> seatsServiceMock;
    Mock<SeatsController> seatsControllerMock;

    public SeatsControllerTests()
    {
        seatsServiceMock = new Mock<SeatsService>();
        seatsControllerMock = new Mock<SeatsController>(seatsServiceMock.Object) { CallBase = true };

        seatsControllerMock.Setup(a => a.UserId).Returns("1111");
    }
    [TestMethod]
    public void ReserveSeat()
    {
       Seat seat = new Seat();
        seat.Id = 1;
        seat.Number = 1;

        seatsServiceMock.Setup(s=>s.ReserveSeat(It.IsAny<string>(),It.IsAny<int>())).Returns(seat);

        var actionresult = seatsControllerMock.Object.ReserveSeat(seat.Number);

        var result = actionresult.Result as OkObjectResult;
        Assert.IsNotNull(result);

    }

    [TestMethod]
    public void ReserveSeat_SeatTaken()
    {
        seatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatAlreadyTakenException());

        var actionresult = seatsControllerMock.Object.ReserveSeat(1);
        var result = actionresult.Result as UnauthorizedResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeat_SeatMaximum()
    {
        seatsServiceMock.Setup(s=>s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatOutOfBoundsException());

        var seatNumber = 1;
        var actionresult = seatsControllerMock.Object.ReserveSeat(seatNumber);
        var result = actionresult.Result as NotFoundObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Could not find " + seatNumber, result.Value);

    }

    [TestMethod]
    public void ReserveSeat_Taken()
    {
        seatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new UserAlreadySeatedException());

        var actionresult = seatsControllerMock.Object.ReserveSeat(1);
        var result = actionresult.Result as BadRequestResult;
        Assert.IsNotNull(result);
    }
}
