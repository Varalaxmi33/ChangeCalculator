using Xunit;
using CurrencyCalculations.Controllers;
using CurrencyCalculations.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CurrencyCalculations.Tests
{
    /// <summary>
    /// Unit tests for ChangeController.Calculate
    /// </summary>
    public class ChangeControllerTests
    {
        private readonly ChangeController _controller;

        public ChangeControllerTests()
        {
            _controller = new ChangeController();
        }

        [Fact]
        public void Calculate_ReturnsBadRequest_WhenAmountGivenIsZero()
        {
            // Arrange
            var request = new ChangeRequest { AmountGiven = 0, ProductPrice = 10 };

            // Act
            var result = _controller.Calculate(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Values must be greater than zero.", badRequest.Value);
        }

        [Fact]
        public void Calculate_ReturnsBadRequest_WhenProductPriceExceedsAmountGiven()
        {
            // Arrange
            var request = new ChangeRequest { AmountGiven = 10, ProductPrice = 20 };

            // Act
            var result = _controller.Calculate(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Product price cannot exceed amount given.", badRequest.Value);
        }

        [Fact]
        public void Calculate_ReturnsOk_WithSingleDenomination()
        {
            // Arrange
            var request = new ChangeRequest { AmountGiven = 100, ProductPrice = 50 };

            // Act
            var result = _controller.Calculate(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var denominations = Assert.IsAssignableFrom<List<DenominationResult>>(result.Value);
            Assert.Contains(denominations, d => d.Denomination == "£50" && d.Count == 1);
        }

        [Fact]
        public void Calculate_ReturnsOk_WithMultipleDenominations()
        {
            // Arrange
            var request = new ChangeRequest { AmountGiven = 10, ProductPrice = 7.38m };

            // Act
            var result = _controller.Calculate(request) as OkObjectResult;

            // Assert
            var denominations = Assert.IsAssignableFrom<List<DenominationResult>>(result.Value);
            Assert.Contains(denominations, d => d.Denomination == "£2" && d.Count == 1);
            Assert.Contains(denominations, d => d.Denomination == "50p" && d.Count == 1);
            Assert.Contains(denominations, d => d.Denomination == "10p" && d.Count == 1);
            Assert.Contains(denominations, d => d.Denomination == "2p" && d.Count == 1);
        }

        [Fact]
        public void Calculate_ReturnsOk_WithExactPayment_NoChange()
        {
            // Arrange
            var request = new ChangeRequest { AmountGiven = 20, ProductPrice = 20 };

            // Act
            var result = _controller.Calculate(request) as OkObjectResult;

            // Assert
            var denominations = Assert.IsAssignableFrom<List<DenominationResult>>(result.Value);
            Assert.Empty(denominations);
        }
    }
}
