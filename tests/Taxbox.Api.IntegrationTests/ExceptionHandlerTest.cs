using Taxbox.Api.IntegrationTests.Common;
using FluentAssertions;
using System.Linq;
using System.Net;
using Taxbox.Application.Features.Articles;

namespace Taxbox.Api.IntegrationTests;

public class ExceptionHandlerTest : BaseTest
{
    public ExceptionHandlerTest(CustomWebApplicationFactory apiFactory) : base(apiFactory)
    {
    }


    // [Fact]
    // public async Task Forced_Error_Should_Return_InternalServerError()
    // {
    //     // // Since this get request input is not validated, should cause a error
    //     // // act
    //     // var response = await GetAsync("/api/Article", new GetArticleResponse()
    //     // {
    //     //     Name = string.Join("", Enumerable.Repeat("a", 5000))
    //     // });  
    //     //
    //     // // assert
    //     // response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    // }
}