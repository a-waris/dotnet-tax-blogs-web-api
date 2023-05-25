using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Taxbox.Application.Common.Responses;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taxbox.Application.Common;
using Taxbox.Application.Features.Articles;
using Taxbox.Application.Features.Articles.GetAllArticles;
using Taxbox.Domain.Entities;
using Taxbox.Infrastructure.ElasticSearch;
using ISession = Taxbox.Domain.Auth.Interfaces.ISession;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArticleController : ControllerBase
{
    private readonly ISession _session;
    private readonly IMediator _mediator;
    private readonly IElasticSearchService<Article> _client;

    public ArticleController(ISession session, IMediator mediator, IElasticSearchService<Article> client)
    {
        _session = session;
        _mediator = mediator;
        _client = client;
    }

    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<PaginatedList<GetArticleResponse>>> GetList(
        [FromQuery] GetAllArticlesRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
}