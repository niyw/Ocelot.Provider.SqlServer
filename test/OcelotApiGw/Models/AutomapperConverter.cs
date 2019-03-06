//using AutoMapper;
//using Ocelot.Configuration.File;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OcelotApiGw.Models {
//    public class AutomapperConverter {
//    }

//    public class CustomConverter : ITypeConverter<RouteRule, FileReRoute> {

//        public FileReRoute Convert(RouteRule source, FileReRoute destination, ResolutionContext context) {

//            return destination;
//        }
//    }

//    public class SourceProfile : Profile {
        
//        protected override void Configure() {
//            //Source->Destination
//            CreateMap<RouteRule, FileReRoute>()
//                .ConvertUsing<CustomConverter>();
//        }
//    }
//}
