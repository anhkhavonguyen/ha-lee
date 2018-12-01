using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class FieldModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FieldType Type { get; set; }
        public string DefaultValue { get; set; }
    }
}
