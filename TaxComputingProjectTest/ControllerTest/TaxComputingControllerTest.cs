using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaxComputingProject.Controllers;
using TaxComputingProject.Model;
using TaxComputingProject.Services;

namespace TaxComputingProjectTest.ControllerTest;

public class TaxComputingControllerTest
{
    
    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(12)]
    public void Should_Return_Ok_When_Get_MonthOfTax_Correctly(int month)
    {
        var mockService = new Mock<ITaxComputingService>();
        mockService.Setup(user => user.GetTaxOfMonth(1, It.IsIn(1, 6, 12))).Returns(It.IsAny<double>());
        var taxComputingController = new TaxComputingController(mockService.Object);
        var result = taxComputingController.GetMonthOfTax(month);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Should_Return_BadRequest_When_Request_Failed()
    {
        var mockService = new Mock<ITaxComputingService>();
        var taxComputingController = new TaxComputingController(mockService.Object);
        List<MonthSalary> monthSalaries = new List<MonthSalary>();
        var result = taxComputingController.SaveTaxByAccumulatedSalary(monthSalaries);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Should_Return_Ok_When_Request_SaveTaxByAccumulatedSalary_Successfully()
    {
        var mockService = new Mock<ITaxComputingService>();
        var taxComputingController = new TaxComputingController(mockService.Object);
        List<MonthSalary> monthSalaries = new List<MonthSalary>
        {
            new()
            {
                Month = 1,
                Salary = 41000
            }
        };
        var result = taxComputingController.SaveTaxByAccumulatedSalary(monthSalaries);
        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(13)]
    public void Should_Return_BadRequest_When_Request_Month_Not_Valid(int month)
    {
        var mockService = new Mock<ITaxComputingService>();
        var taxComputingController = new TaxComputingController(mockService.Object);
        var result = taxComputingController.GetMonthOfTax(month);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Should_Return_BadRequest_When_No_User_Tax_Record()
    {
        var mockService = new Mock<TaxComputingServiceImpl>();
        mockService.Setup(taxComputingService =>
                taxComputingService.GetTaxOfMonth(It.IsAny<int>(), It.IsAny<int>()))
            .Throws<BadHttpRequestException>(() =>
                throw new BadHttpRequestException("The user has no tax record"));
        var taxComputingController = new TaxComputingController(mockService.Object);
        var result = taxComputingController.GetMonthOfTax(1);
        Assert.IsType<BadRequestObjectResult>(result);
        var objectResult = result as BadRequestObjectResult;
        Assert.Equal("{ errorMessage = The user has no tax record }", objectResult?.Value?.ToString());
    }

    [Fact]
    public void Should_Return_OK_When_Get_AnnualTaxRecords_Successfully()
    {
        var mockService = new Mock<ITaxComputingService>();
        AnnualTaxRecords annualTaxRecords = new AnnualTaxRecords();
        mockService.Setup(tax => tax.GetAnnualTaxRecords(It.IsAny<int>())).Returns(annualTaxRecords);
        var taxComputingController = new TaxComputingController(mockService.Object);
        var result = taxComputingController.GetAnnualTaxRecords();
        Assert.IsType<OkObjectResult>(result);
    }
}