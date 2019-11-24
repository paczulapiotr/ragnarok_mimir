using AutoMapper;
using Mimir.API.DTO;
using Mimir.Core.Models;

namespace Mimir.API.Mapper
{
    public class KanbaMappings: Profile
    {
        public KanbaMappings()
        {

            CreateMap<KanbanItem, KanbanItemResultDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.ID))
                .ForMember(dest => dest.Index, opt => opt.MapFrom(x => x.Index))
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
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(x => x.Columns));


        }
    }
}
