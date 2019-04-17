using System;

namespace ModelOfThings.Parser.Models
{
    public class MddPropertie
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
        public string Type { get; set; }

        public virtual MddComponent MddComponent { get; set; }
    }
}