using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;

namespace MovieApiTests.ControllerTests
{
    public static class Helpers
    {
        public static void SetupMockProblemDetails(this Mock<ProblemDetailsFactory> mockProblemFactory)
        {
            mockProblemFactory
                .Setup(_ => _.CreateProblemDetails(
                    It.IsAny<HttpContext>(),
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
                )
                .Returns(new ProblemDetails())
                .Verifiable();
        }

        public static void SetupMockValidationProblemDetails(this Mock<ProblemDetailsFactory> mockProblemFactory)
        {
            mockProblemFactory
                .Setup(_ => _.CreateValidationProblemDetails(
                    It.IsAny<HttpContext>(),
                    It.IsAny<ModelStateDictionary>(),
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
                )
                .Returns(new ValidationProblemDetails())
                .Verifiable();
        }
    }
}
