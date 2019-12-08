using System.Collections.Generic;
using AutoMapper;
using Mimir.API.DTO;
using Mimir.API.DTO.Comment.Result;
using Mimir.Core.Models;

namespace Mimir.API.Mapper
{
    public class KanbaMappings : Profile
    {
        public KanbaMappings()
        {

            CreateMap<KanbanItem, KanbanItemResultDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.ID))
                .ForMember(dest => dest.Index, opt => opt.MapFrom(x => x.Index))
                .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(x => x.Assignee.Name))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name));

            CreateMap<KanbanColumn, KanbanColumnResultDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.ID))
                .ForMember(dest => dest.Index, opt => opt.MapFrom(x => x.Index))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(x => x.Items));

            CreateMap<KanbanBoard, KanbanBoardResultDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(x => x.Timestamp))
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(x => x.Columns))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AppUser, AppUserBasicResultDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name));

            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.ID))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(x => x.Content))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(x => x.AuthorId))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(x => x.Author.Name))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn))
                .ForMember(dest => dest.EditedOn, opt => opt.MapFrom(x => x.EditedOn))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<KanbanItem, KanbanItemDetailsResultDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(x => x.Comments))
                .ForMember(dest => dest.Assignee, opt => opt.MapFrom(x => x.Assignee));

        }
    }
}
