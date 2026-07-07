using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using MediatR;

namespace Coreapi.Application.Features.Landing.Documents
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string JalaliDate { get; set; }
        public DateTime Date { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string FileSize { get; set; }
        public int DownloadCount { get; set; }
        public List<string> Tags { get; set; }
        public string FileUrl { get; set; }
        public bool Featured { get; set; }

        public static DocumentDto From(Document d) => new()
        {
            Id = d.Id, Title = d.Title, Category = d.Category, JalaliDate = d.JalaliDate, Date = d.Date,
            Version = d.Version, Description = d.Description, FileSize = d.FileSize,
            DownloadCount = d.DownloadCount, FileUrl = d.FileUrl, Featured = d.Featured,
            Tags = string.IsNullOrWhiteSpace(d.Tags)
                ? new List<string>()
                : d.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
        };
    }

    public class DocumentCategoryDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class GetAllDocumentsQuery : IRequest<IEnumerable<DocumentDto>>
    {
        public string Category { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
    }

    public class GetDocumentCategoriesQuery : IRequest<IEnumerable<DocumentCategoryDto>> { }

    public class IncrementDownloadCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
    }

    public class DocumentHandlers(ILandingRepository repo) :
        IRequestHandler<GetAllDocumentsQuery, IEnumerable<DocumentDto>>,
        IRequestHandler<GetDocumentCategoriesQuery, IEnumerable<DocumentCategoryDto>>,
        IRequestHandler<IncrementDownloadCommand, int?>
    {
        public async Task<IEnumerable<DocumentDto>> Handle(GetAllDocumentsQuery r, CancellationToken ct) =>
            (await repo.GetDocuments(r.Category, r.Search, r.SortBy)).Select(DocumentDto.From);

        public async Task<IEnumerable<DocumentCategoryDto>> Handle(GetDocumentCategoriesQuery r, CancellationToken ct) =>
            (await repo.GetDocumentCategories()).Select(kv => new DocumentCategoryDto { Name = kv.Key, Count = kv.Value });

        public async Task<int?> Handle(IncrementDownloadCommand r, CancellationToken ct) =>
            await repo.IncrementDownload(r.Id);
    }
}
