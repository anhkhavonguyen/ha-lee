using AutoMapper;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static Harvey.PIM.Application.Infrastructure.Models.FieldTemplateModel;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class FieldTemplateModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public FieldTemplateType Type { get; set; } = FieldTemplateType.WithVariant;
        public List<Section> Sections { get; set; }

        public class Section
        {
            public string Name { get; set; }
            public bool IsVariantField { get; set; }
            public int OrderSection { get; set; }
            public List<Field_FieldTemplateModel> Fields { get; set; }
        }

        public class Field_FieldTemplateModel
        {
            public Guid Id { get; set; }
            public FieldModel Field { get; set; }
            [Required]
            public ItemActionType Action { get; set; } = ItemActionType.Get;
        }
    }

    public class FieldTemplateModelProfile : Profile
    {
        public FieldTemplateModelProfile()
        {
            CreateMap<FieldTemplateModel, FieldTemplate>()
                .ForMember(dest => dest.Id,
                            opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description,
                            opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type,
                            opt => opt.MapFrom(src => src.Type));

            CreateMap<FieldTemplateModel, List<Field_FieldTemplate>>()
                .ConstructUsing(src =>
                {
                    var result = new List<Field_FieldTemplate>();
                    src.Sections.ForEach(section =>
                    {
                        section.Fields.ForEach(field =>
                        {
                            var data = new Field_FieldTemplate()
                            {
                                Id = field.Id,
                                FieldId = field.Field.Id
                            };
                            data.FieldTemplateId = src.Id;
                            data.OrderSection = section.OrderSection;
                            data.IsVariantField = section.IsVariantField;
                            data.Section = section.Name;
                            result.Add(data);
                        });
                    });
                    return result;
                });

            CreateMap<FieldTemplate, FieldTemplateModel>()
              .ConstructUsing(src =>
              {
                  var result = new FieldTemplateModel()
                  {
                      Id = src.Id,
                      Name = src.Name,
                      Description = src.Description,
                      Type = src.Type
                  };

                  result.Sections = src.Field_FieldTemplates
                  .GroupBy(x => new
                  {
                      x.Section,
                      x.IsVariantField,
                      x.OrderSection
                  }, (key, g) =>
                       new Section()
                       {
                           Name = key.Section,
                           IsVariantField = key.IsVariantField,
                           OrderSection = key.OrderSection,
                           Fields = g.Select(x => new Field_FieldTemplateModel()
                           {
                               Id = x.Id,
                               Field = new FieldModel()
                               {
                                   Id = x.Field.Id,
                                   Name = x.Field.Name,
                                   Description = x.Field.Description,
                                   Type = x.Field.Type,
                                   DefaultValue = x.Field.DefaultValue
                               }
                           }).ToList()
                       }
                  ).ToList();
                  return result;
              });
        }
    }
}
