using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.Categories;
using Taxbox.Application.Features.Categories.CreateCategory;
using Taxbox.Application.Features.Categories.DeleteCategory;
using Taxbox.Application.Features.Categories.GetAllCategories;
using Taxbox.Application.Features.Categories.GetCategoryById;
using Taxbox.Application.Features.Categories.UpdateCategory;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetCategoryResponse>> GetById(CategoryId id)
    {
        return Ok(await _mediator.Send(new GetCategoryByIdRequest(id)));
    }


    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetCategoryResponse>>> GetList(
        [FromQuery] GetAllCategoriesRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetCategoryResponse>> Create(
        [FromForm] CreateCategoryRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetCategoryResponse>> Update(
        CategoryId id,
        [FromBody] UpdateCategoryRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(CategoryId id)
    {
        return Ok(await _mediator.Send(new DeleteCategoryRequest(Id: id)));
    }
}