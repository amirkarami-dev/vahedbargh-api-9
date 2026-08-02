using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.LandingAgg;
using FluentValidation;
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

    // ---------- Admin write commands ----------

    public class UpsertDocumentCommand : IRequest<DocumentDto>
    {
        public Guid? Id { get; set; } // null = create, set = update
        public string Title { get; set; }
        public string Category { get; set; }
        public string JalaliDate { get; set; }
        public DateTime Date { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string FileSize { get; set; }
        public List<string> Tags { get; set; } = new();
        public string FileUrl { get; set; }
        public bool Featured { get; set; }
    }

    public class DeleteDocumentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class UpsertDocumentCommandValidator : AbstractValidator<UpsertDocumentCommand>
    {
        public UpsertDocumentCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
            RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        }
    }

    public class DocumentCommandHandlers(ILandingRepository repo) :
        IRequestHandler<UpsertDocumentCommand, DocumentDto>,
        IRequestHandler<DeleteDocumentCommand, bool>
    {
        public async Task<DocumentDto> Handle(UpsertDocumentCommand r, CancellationToken ct)
        {
            var tags = string.Join(",", (r.Tags ?? new List<string>())
                .Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()));

            if (r.Id is Guid id)
            {
                var updated = await repo.UpdateDocument(new Document
                {
                    Id = id, Title = r.Title, Category = r.Category, JalaliDate = r.JalaliDate, Date = r.Date,
                    Version = r.Version, Description = r.Description, FileSize = r.FileSize, Tags = tags,
                    FileUrl = r.FileUrl, Featured = r.Featured,
                });
                return updated is null ? null : DocumentDto.From(updated);
            }

            var created = await repo.AddDocument(new Document
            {
                Id = Guid.NewGuid(), Title = r.Title, Category = r.Category, JalaliDate = r.JalaliDate, Date = r.Date,
                Version = r.Version, Description = r.Description, FileSize = r.FileSize, DownloadCount = 0,
                Tags = tags, FileUrl = r.FileUrl, Featured = r.Featured,
            });
            return DocumentDto.From(created);
        }

        public async Task<bool> Handle(DeleteDocumentCommand r, CancellationToken ct) =>
            await repo.DeleteDocument(r.Id);
    }
}
