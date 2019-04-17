namespace ModelOfThings.Parser.Models
{
    public class UmlAssociation
    {
        public string Id { get; set; }

        public string Source { get; set; }
        public string Destiny { get; set; }
        
        public AssociationType Type { get; set; }

        public string UmlUseCaseId { get; set; }

        public string UmlElementId { get; set; }

        public UmlUseCase UmlUseCase { get; set; }
    }

    public enum AssociationType
    {
        Include,
        Extend,
        ExtensionPoint
    }
}
