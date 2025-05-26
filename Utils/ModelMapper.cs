using AutoMapper;
using Domain.DTOs.BiometricProfile;
using Domain.DTOs.Customer;
using Domain.DTOs.Kyc;
using Domain.DTOs.KycAccess;
using Domain.DTOs.ManualAccess;
using Domain.DTOs.Partner;
using Domain.Entities;
using System.Linq;

namespace API.Utils
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<Partner, PartnerDto>().ReverseMap();

            CreateMap<ParentPartner, ParentPartnerDto>().ReverseMap();

            CreateMap<KycOperation, KycOperationsDto>()
                .ForMember(dest => dest.IsSimilaritySuccess, opt => opt.MapFrom(src => src.SimilarityStatus))
                .ForMember(dest => dest.IsLivenessSuccess, opt => opt.MapFrom(src => src.LivenessStatus))
                .ForPath(dest => dest.PartnerName, opt => opt.MapFrom(src => src.Partner.Name))
                .ReverseMap()
                .ForMember(dest => dest.SimilarityStatus, opt => opt.MapFrom(src => src.IsSimilaritySuccess))
                .ForMember(dest => dest.LivenessStatus, opt => opt.MapFrom(src => src.IsLivenessSuccess));

            CreateMap<KycOperation, KycOperationsExportDto>().ReverseMap();

            CreateMap<KycOperation, KycOperationDto>().ReverseMap();

            CreateMap<Customer, CustomersDto>().ReverseMap()
                .ForPath(dest => dest.Partner.Name, opt => opt.MapFrom(src => src.PartnerName));

            CreateMap<Customer, CustomerDto>().ReverseMap();

            CreateMap<BiometricAccessOperation, KycAccessOperationsDto>()
                .ForPath(dest => dest.PartnerName, opt => opt.MapFrom(src => src.Partner.Name))
                .ForMember(dest => dest.SimilarityScore, opt => opt.MapFrom(src => src.Similarity))
                .ForMember(dest => dest.IsLivenessSuccess, opt => opt.MapFrom(src => src.LivenessStatus))
                .ForMember(dest => dest.IsSimilaritySuccess, opt => opt.MapFrom(src => src.SimilarityStatus));

            CreateMap<BiometricAccessOperation, KycAccessOperationDto>().ForMember(dest => dest.SimilarityScore, opt => opt.MapFrom(src => src.Similarity));

            CreateMap<BiometricAccessOperation, KycAccessOperationsExportDto>().ReverseMap();

            CreateMap<ManualAccess, ManualAccessByIdDto>().ReverseMap();

            CreateMap<ManualAccess, ManualAccessDto>().ReverseMap();

            CreateMap<DocumentType, DocumentTypeDto>().ReverseMap();

            CreateMap<BiometricProfileHistory, BiometricProfileHistoryDto>()
                 .ForMember(dest => dest.Pin, opt => opt.MapFrom(src => src.BiometricProfile.Pin))
                 .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => src.BiometricProfile.IsBlocked))
                 .ReverseMap();

            CreateMap<BiometricProfile, BiometricProfileDto>()
                .ForMember(dest => dest.BiometricProfileInfo, opt => opt.MapFrom(src => src.BiometricProfileHistories.Select(history => new BiometricProfileInfo
                {
                    BiometricProfileHistoryId = history.Id,
                    DocumentTypeName = history.DocumentType.Name,
                    DocumentTypeDescription = history.DocumentType.Description,
                }).ToList()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BiometricProfileHistories.First().Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.BiometricProfileHistories.First().Surname))
                .ForMember(dest => dest.Citizenship, opt => opt.MapFrom(src => src.BiometricProfileHistories.First().Citizenship))
                .ReverseMap();
        }
    }
}
