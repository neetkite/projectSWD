using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Helper
{
    public class ConfigAutomapper : Profile
    {
        public ConfigAutomapper()
        {
            CreateMap<DTOs.Request.RequestRequest, Models.Request>();
            CreateMap<Models.Request, DTOs.Request.RequestRequest>();

            CreateMap<DTOs.Request.RequestResponse, Models.Request>();
            CreateMap<Models.Request ,DTOs.Request.RequestResponse>();

        }
    }
}
