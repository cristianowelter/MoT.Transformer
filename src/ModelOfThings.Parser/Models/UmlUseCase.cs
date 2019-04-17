using System.Collections.Generic;

namespace ModelOfThings.Parser.Models
{
    public class UmlUseCase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MddApplicationId { get; set; }
        public string UmlElementId { get; set; }

        public MddApplication MddApplication { get; set; }
        public virtual ICollection<UmlAssociation> Associations { get; set; }
        public virtual ICollection<MddComponent> MddComponents { get; set; }
    }
}
