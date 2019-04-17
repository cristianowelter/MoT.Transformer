
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ModelOfThings.Parser.Models
{
    public class MddApplication
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

        [NotMapped]
        public string NormalizedName => !string.IsNullOrEmpty(Name) ? Regex.Replace(Name, "[^0-9a-zA-Z]+", "") : string.Empty;

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<UmlModel> UmlModels { get; set; }
        public virtual ICollection<UmlUseCase> UmlUseCases { get; set; }
        public virtual ICollection<CloudProvider> CloudProviders { get; set; }
    }
}
