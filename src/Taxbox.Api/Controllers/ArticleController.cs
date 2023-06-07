using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.Articles;
using Taxbox.Application.Features.Articles.CreateArticle;
using Taxbox.Application.Features.Articles.DeleteArticle;
using Taxbox.Application.Features.Articles.GetAllArticles;
using Taxbox.Application.Features.Articles.GetArticleById;
using Taxbox.Application.Features.Articles.UpdateArticle;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArticleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetArticleResponse>> GetById(ArticleId id)
    {
        return Ok(await _mediator.Send(new GetArticleByIdRequest(id)));
    }

    [HttpGet]
    [Route("public/{id}")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetArticleResponse>> GetByIdPublic(ArticleId id)
    {
        return Ok(await _mediator.Send(new GetArticleByIdRequest(Id: id, IsPublic: true)));
    }

    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetArticleResponse>>> GetList(
        [FromQuery] GetAllArticlesRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpGet]
    [Route("public/list")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetArticleResponse>>> GetListPublic([FromQuery] GetAllArticlesPublicRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetArticleResponse>> Create(
        [FromForm] CreateArticleRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetArticleResponse>> Update(
        ArticleId id,
        [FromBody] UpdateArticleRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(ArticleId id)
    {
        return Ok(await _mediator.Send(new DeleteArticleRequest(Id: id)));
    }
}