using Ardalis.Result;
using Elastic.Clients.Elasticsearch;
using MediatR;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Extensions;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;
using static System.Int32;

namespace Taxbox.Application.Features.Articles.BulkAddArticles;

public class BulkAddArticlesHandler : IRequestHandler<BulkAddArticlesRequest, Result<BulkAddArticlesResponse>>
{
    private readonly IElasticSearchService<Article> _esService;
    private readonly IElasticSearchService<Author> _esServiceAuthor;

    public BulkAddArticlesHandler(IElasticSearchService<Article> esService,
        IElasticSearchService<Author> esServiceAuthor)
    {
        _esService = esService;
        _esServiceAuthor = esServiceAuthor;
    }


    public async Task<Result<BulkAddArticlesResponse>> Handle(BulkAddArticlesRequest request,
        CancellationToken cancellationToken)
    {
        var resp = new BulkAddArticlesResponse();
        var articles = await ConvertExcelToArticles(request.File, resp);
        var bulkResponse = await _esService.AddBulk(articles);
        resp.SuccessfulRows = bulkResponse.Items.Count;
        return resp;
    }

    private async Task<IList<Article>> ConvertExcelToArticles(IFormFile requestFile,
        BulkAddArticlesResponse bulkAddArticlesResponse)
    {
        try
        {
            var taxboxAuthorResp = await _esServiceAuthor.Index("authors").Query(
                new SearchRequestDescriptor<Author>()
                    .Query(q => q.RawJson("{\"term\": {\"name.raw\": {\"value\": \"Taxbox\"}}}"))
                    .Size(1));

            var author = taxboxAuthorResp?.Documents.FirstOrDefault();
            if (author == null)
            {
                throw new Exception("Taxbox author not found");
            }

            ConcurrentBag<Article> articles = new();
            Dictionary<string, Article> addedArticles = new();

            using var package = new ExcelPackage(requestFile.OpenReadStream());
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            worksheet.TrimLastEmptyRows();

            int rowCount = worksheet.Dimension.Rows;
            bulkAddArticlesResponse.TotalRows = rowCount - 1;
            switch (rowCount)
            {
                case < 2:
                    throw new Exception("No data found in excel");
                case > 1000:
                    throw new Exception("Maximum 1000 rows are allowed");
                case 2:
                    {
                        var article = ProcessRow(worksheet, 2, author, bulkAddArticlesResponse);
                        if (article != null)
                        {
                            articles.Add(article);
                        }

                        return articles.ToList();
                    }
            }

            Parallel.For(2, rowCount + 1, row =>
            {
                var article = ProcessRow(worksheet, row, author, bulkAddArticlesResponse);
                if (article == null)
                {
                    return;
                }

                lock (addedArticles)
                {
                    if (addedArticles.ContainsKey(article.Title))
                    {
                        return;
                    }

                    addedArticles.Add(article.Title, article);
                    articles.Add(article);
                    lock (bulkAddArticlesResponse)
                    {
                        bulkAddArticlesResponse.SuccessfulRows++;
                    }
                }
            });

            return articles.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private Article? ProcessRow(ExcelWorksheet worksheet, int rowIndex, Author author,
        BulkAddArticlesResponse bulkAddArticlesResponse)
    {
        if (string.IsNullOrEmpty(worksheet.Cells[rowIndex, 1]?.Value.ToString())
            || string.IsNullOrEmpty(worksheet.Cells[rowIndex, 2]?.Value.ToString()))
        {
            lock (bulkAddArticlesResponse)
            {
                bulkAddArticlesResponse.InvalidRows++;
            }

            return null;
        }

        if (!TryParse(worksheet.Cells[rowIndex, 6].Value?.ToString(), out int views))
        {
            views = 0;
        }

        var article = new Article
        {
            Title = worksheet.Cells[rowIndex, 1].Value.ToString()!,
            Content = worksheet.Cells[rowIndex, 2].Value?.ToString(),
            HtmlContent = worksheet.Cells[rowIndex, 3].Value?.ToString(),
            Metadata = new Metadata
            {
                Category = worksheet.Cells[rowIndex, 4].Value?.ToString(),
                Language = worksheet.Cells[rowIndex, 5].Value?.ToString(),
                Views = views
            },
            AuthorIds = new List<string> { worksheet.Cells[rowIndex, 7].Value?.ToString() ?? author.Id.ToString() },
            PublishedAt = worksheet.Cells[rowIndex, 8].Value?.ToString() != null
                ? Convert.ToDateTime(worksheet.Cells[rowIndex, 8].Value)
                : DateTime.Now.Date,
            Tags = worksheet.Cells[rowIndex, 9].Value?.ToString()?.Split(',').ToList(),
            IsDraft = worksheet.Cells[rowIndex, 10].Value?.ToString()?.ToLower() == "true"
                      || worksheet.Cells[rowIndex, 10].Value?.ToString() == "1",
            IsPublic = worksheet.Cells[rowIndex, 11].Value?.ToString()?.ToLower() == "true"
                       || worksheet.Cells[rowIndex, 11].Value?.ToString() == "1",
            IsPublished = worksheet.Cells[rowIndex, 12].Value?.ToString()?.ToLower() == "true"
                          || worksheet.Cells[rowIndex, 12].Value?.ToString() == "1",
            CoverImage = worksheet.Cells[rowIndex, 13].Value?.ToString(),
            ThumbnailImage = worksheet.Cells[rowIndex, 14].Value?.ToString(),
            CreatedAt = DateTime.Now.Date,
            UpdatedAt = DateTime.Now.Date
        };

        return article;
    }
}