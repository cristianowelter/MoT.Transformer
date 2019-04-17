using System;
using System.Collections.Generic;

namespace ModelOfThings.Parser.Models
{
    public class MddComponent
    {
        public string Id { get; set; }
        public string MddApplicationId { get; set; }
        public string UmlUseCaseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Stereotype { get; set; }
        public string Type { get; set; }
        public bool Configurable { get; set; }
        public string UmlElementId { get; set; }
        public string Connections { get; set; }
        public string ParentId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool ConnectionIn { get; set; }
        public bool ConnectionOut { get; set; }

        public virtual ICollection<MddComponent> MddComponents { get; set; }
        public virtual UmlUseCase UmlUseCase { get; set; }
        public virtual ICollection<MddPropertie> MddProperties { get; set; }
        public virtual MddApplication MddApplication { get; set; }
    }
}