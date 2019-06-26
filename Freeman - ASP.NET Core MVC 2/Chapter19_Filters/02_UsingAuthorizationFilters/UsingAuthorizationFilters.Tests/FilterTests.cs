using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using UsingAuthorizationFilters.Infrastructure;
using Xunit;

namespace UsingAuthorizationFilters.Tests
{
    public class FilterTests
    {
        [Fact]
        public void TestHttpsFilter()
        {
            // Arrange
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetupSequence(m => m.IsHttps)
                       .Returns(true)
                       .Returns(false);

            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(m => m.Request).Returns(httpRequest.Object);

            var actionContext = new ActionContext(
                httpContext.Object, new RouteData(), new ActionDescriptor());

            var filters = Enumerable.Empty<IFilterMetadata>().ToList();
            var authContext = new AuthorizationFilterContext(actionContext, filters);

            var filter = new HttpsOnlyAttribute();

            // Act
            filter.OnAuthorization(authContext);
            Assert.Null(authContext.Result);

            // Assert
            filter.OnAuthorization(authContext);
            Assert.IsType(typeof(StatusCodeResult), authContext.Result);
            Assert.Equal(StatusCodes.Status403Forbidden, (authContext.Result as StatusCodeResult).StatusCode);
        }
    }
}
