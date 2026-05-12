using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public interface ICommentEngFormRepository:IRepository<CommentEngForm>
{
    Task<IEnumerable<CommentEngForm>> GetAllCommentEngForm();
    Task<IEnumerable<CommentEngForm>> GetAllCommentEngForm(Guid engId);
    Task<IEnumerable<Common.ViewModels.CommentEngForm>> GetCommentEngForm(Guid electProjectId,Guid engId);
}