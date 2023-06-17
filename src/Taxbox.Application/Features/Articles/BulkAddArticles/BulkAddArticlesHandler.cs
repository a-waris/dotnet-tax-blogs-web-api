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
            // check if file is excel file 
            var fileExtension = Path.GetExtension(requestFile.FileName);
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                throw new Exception("Invalid file type");
            }


            // fetch taxbox author by name "Taxbox" from elastic search
            var taxboxAuthorResp = await _esServiceAuthor.Index("authors").Query(
                new SearchRequestDescriptor<Author>()
                    .Query(q => q.RawJson("{\"term\": {\"name.raw\": {\"value\": \"Taxbox\"}}}"))
                    .Size(1));

            var author = taxboxAuthorResp?.Documents.FirstOrDefault();

            ConcurrentBag<Article> articles = new();
            Dictionary<string, Article> addedArticles = new Dictionary<string, Article>();

            using var package = new ExcelPackage(requestFile.OpenReadStream());
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            worksheet.TrimLastEmptyRows();

            int rowCount = worksheet.Dimension.Rows;

            bulkAddArticlesResponse.TotalRows = rowCount - 1; // -1 for header row

            object lockObject = new object();
            Parallel.ForEach(Partitioner.Create(2, rowCount), range =>
            {
                for (int row = range.Item1; row <= range.Item2; row++) // Assuming the data starts from row 2
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[row, 1].Value.ToString())
                        || string.IsNullOrEmpty(worksheet.Cells[row, 2].Value.ToString())
                       )
                    {
                        bulkAddArticlesResponse.InvalidRows += 1;
                        continue;
                    }

                    var article = new Article
                    {
                        Title = worksheet.Cells[row, 1].Value.ToString()!,
                        Content = worksheet.Cells[row, 2].Value?.ToString(),
                        HtmlContent = worksheet.Cells[row, 3].Value?.ToString(),
                        Metadata = new Metadata
                        {
                            Category = worksheet.Cells[row, 4].Value?.ToString(),
                            Language = worksheet.Cells[row, 5].Value?.ToString(),
                            Views = Convert.ToInt32(worksheet.Cells[row, 6].Value)
                        },
                        AuthorIds =
                            new List<string> { worksheet.Cells[row, 7].Value?.ToString() ?? author?.Id.ToString()! },
                        PublishedAt = worksheet.Cells[row, 8].Value?.ToString() != null
                            ? Convert.ToDateTime(worksheet.Cells[row, 8].Value)
                            : DateTime.Now.Date,
                        Tags = worksheet.Cells[row, 9].Value?.ToString()?.Split(',').ToList(),
                        IsDraft =
                            worksheet.Cells[row, 10].Value?.ToString()?.ToLower() == "true" ||
                            worksheet.Cells[row, 10].Value?.ToString() == "1",
                        IsPublic =
                            worksheet.Cells[row, 11].Value?.ToString()?.ToLower() == "true" ||
                            worksheet.Cells[row, 11].Value?.ToString() == "1",
                        IsPublished = worksheet.Cells[row, 12].Value?.ToString()?.ToLower() == "true" ||
                                      worksheet.Cells[row, 12].Value?.ToString() == "1",
                        CoverImage = worksheet.Cells[row, 13].Value?.ToString(),
                        ThumbnailImage = worksheet.Cells[row, 14].Value?.ToString(),
                        // Do not add attachments for now
                        // Attachments = new List<ArticleAttachment>
                        // {
                        //     new()
                        //     {
                        //         File = worksheet.Cells[row, 14].Value?.ToString() ?? "",
                        //         Type = worksheet.Cells[row, 15].Value?.ToString() ?? ""
                        //     }
                        // },
                        CreatedAt = DateTime.Now.Date,
                        UpdatedAt = DateTime.Now.Date,
                    };

                    bool isAdded = false;

                    lock (lockObject)
                    {
                        if (!addedArticles.ContainsKey(article.Title))
                        {
                            addedArticles.Add(article.Title, article);
                            isAdded = true;
                        }
                    }

                    if (isAdded)
                    {
                        articles.Add(article);
                        bulkAddArticlesResponse.SuccessfulRows += 1;
                    }
                    else
                    {
                        // bulkAddArticlesResponse.DuplicateRows += 1;
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
}