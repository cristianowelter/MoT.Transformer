using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ModelOfThings.Parser.Services
{
    public class UmlParser
    {
        private readonly string _profileNamespace = @"https://cristianowelter.visualstudio.com/ModelOfThings";
        private readonly string _xmiNamespace = @"http://www.omg.org/spec/XMI/20131001";

        private readonly ApplicationDbContext _context;

        public UmlParser(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task XmiParserAsync(UmlModel model, string filePath)
        {
            var xmiMetaModel = new XmlDocument();
            xmiMetaModel.Load(filePath);

            var stereotypes = GetMddStereotypes(xmiMetaModel.DocumentElement);
            var useCases = GetUmlUseCase(xmiMetaModel);
            var actors = GetUmlActor(xmiMetaModel);

            foreach (var useCase in useCases)
            {
                useCase.MddApplicationId = model.MddApplicationId;

                var stereotypesUsed = stereotypes.Where(a => a.ElementId == useCase.UmlElementId);

                if (stereotypesUsed.Any())
                {
                    useCase.MddComponents = new List<MddComponent>();

                    foreach (var stereotype in stereotypesUsed)
                    {
                        if (_context.MddComponents.Any(x => x.MddApplicationId == model.MddApplicationId && x.UmlElementId == stereotype.ElementId && x.Stereotype == stereotype.Name))
                        {
                            break;
                        }

                        var filePathJson = Path.Combine(Directory.GetCurrentDirectory(), "_json", $"{stereotype.Name.ToLower()}.json");

                        var component = new MddComponent();

                        if (File.Exists(filePathJson))
                        {
                            var file = File.ReadAllText(filePathJson);
                            component = JsonConvert.DeserializeObject<MddComponent>(file);
                        }

                        if (component.MddComponents != null && component.MddComponents.Any())
                        {
                            foreach (var comp in component.MddComponents)
                            {

                                comp.Id = $"{useCase.Id}-{comp.Id}";
                                comp.ParentId = useCase.Id;
                                comp.MddApplicationId = model.MddApplicationId;
                                comp.UmlUseCaseId = useCase.Id;


                                if (!string.IsNullOrEmpty(comp.Stereotype))
                                {
                                    var filePathJsonChild = Path.Combine(Directory.GetCurrentDirectory(), "_json", $"{comp.Stereotype}.json");

                                    var childComponent = new MddComponent();

                                    if (File.Exists(filePathJsonChild))
                                    {
                                        var file = File.ReadAllText(filePathJsonChild);
                                        childComponent = JsonConvert.DeserializeObject<MddComponent>(file);
                                    }

                                    comp.Type = childComponent.Type;
                                    comp.Description = childComponent.Description;
                                    comp.Configurable = childComponent.Configurable;
                                    comp.MddProperties = childComponent.MddProperties;
                                }

                                if (!string.IsNullOrEmpty(comp.Connections))
                                {
                                    var connections = comp.Connections.Split(' ').ToArray();

                                    for (var i = 0; i < connections.Length; i++)
                                    {
                                        connections[i] = $"{useCase.Id}-{connections[i]}";
                                    }
                                    comp.Connections = string.Join(' ', connections);
                                }

                                useCase.MddComponents.Add(comp);
                            }
                        }
                        else
                        {
                            component.Id = Guid.NewGuid().ToString();
                            component.MddApplicationId = model.MddApplicationId;
                            component.Stereotype = stereotype.Name;
                            component.ParentId = useCase.Id;
                            component.UmlElementId = useCase.Id;
                            component.UmlUseCaseId = useCase.Id;

                            useCase.MddComponents.Add(component);
                        }
                    }
                }

                _context.Add(useCase);
                await _context.SaveChangesAsync();
            }
        }

        private IEnumerable<UmlStereotype> GetMddStereotypes(XmlElement xmiMetaModel)
        {
            var stereotypes = new List<UmlStereotype>();

            var elemList = xmiMetaModel.GetElementsByTagName("*", _profileNamespace);

            for (var i = 0; i < elemList.Count; i++)
            {
                var current = elemList[i];
                var stereotype = new UmlStereotype
                {
                    Id = current.Attributes["id", _xmiNamespace].Value,
                    Name = current.LocalName,
                    ElementId = current.Attributes["base_UseCase"].Value
                };

                stereotypes.Add(stereotype);
            }

            return stereotypes;
        }

        private IEnumerable<UmlActor> GetUmlActor(XmlDocument xmiMetaModel)
        {
            var elemList =
                xmiMetaModel.GetElementsByTagName("packagedElement").Cast<XmlNode>()
                .Where(e => e.Attributes["xmi:type"].Value == "uml:Actor");

            return elemList.Select(el => new UmlActor
            {
                Id = el.Attributes["id", _xmiNamespace].Value,
                Name = el.Attributes["name"].Value
            })
                .ToList();
        }

        private IEnumerable<UmlUseCase> GetUmlUseCase(XmlDocument xmiMetaModel)
        {
            var useCases = new List<UmlUseCase>();

            var elemList =
                xmiMetaModel.GetElementsByTagName("packagedElement").Cast<XmlNode>()
                .Where(e => e.Attributes["xmi:type"].Value == "uml:UseCase");

            foreach (var elem in elemList)
            {
                var useCase = new UmlUseCase
                {
                    Id = Guid.NewGuid().ToString(),
                    UmlElementId = elem.Attributes["id", _xmiNamespace].Value,
                    Name = elem.Attributes["name"].Value
                };

                if (elem.HasChildNodes)
                {
                    useCase.Associations = new List<UmlAssociation>();

                    foreach (var nodeChild in elem.ChildNodes.Cast<XmlNode>().Where(e => e.Attributes["xmi:type"].Value == "uml:Include"))
                    {
                        var association = new UmlAssociation
                        {
                            Id = Guid.NewGuid().ToString(),
                            UmlUseCaseId = useCase.Id,
                            Type = AssociationType.Include,
                            UmlElementId = nodeChild.Attributes["id", _xmiNamespace].Value,
                            Source = useCase.UmlElementId,
                            Destiny = nodeChild.Attributes["addition"].Value
                        };

                        useCase.Associations.Add(association);
                    }

                    foreach (var nodeChild in elem.ChildNodes.Cast<XmlNode>().Where(e => e.Attributes["xmi:type"].Value == "uml:Extend"))
                    {
                        var association = new UmlAssociation
                        {
                            Id = Guid.NewGuid().ToString(),
                            UmlUseCaseId = useCase.Id,
                            Type = AssociationType.Extend,
                            UmlElementId = nodeChild.Attributes["id", _xmiNamespace].Value,
                            Source = useCase.UmlElementId,
                            Destiny = $"{nodeChild.Attributes["extendedCase"].Value}{nodeChild.Attributes["extensionLocation"].Value}"
                        };

                        useCase.Associations.Add(association);
                    }

                    foreach (var nodeChild in elem.ChildNodes.Cast<XmlNode>().Where(e => e.Attributes["xmi:type"].Value == "uml:ExtensionPoint"))
                    {
                        var association = new UmlAssociation
                        {
                            Id = Guid.NewGuid().ToString(),
                            UmlUseCaseId = useCase.Id,
                            Type = AssociationType.ExtensionPoint,
                            UmlElementId = nodeChild.Attributes["id", _xmiNamespace].Value,
                            Source = useCase.UmlElementId
                        };

                        useCase.Associations.Add(association);
                    }
                }

                useCases.Add(useCase);
            }

            return useCases;
        }

        private IEnumerable<UmlAssociation> GetUmlAssociation(XmlDocument xmiMetaModel)
        {
            var associations = new List<UmlAssociation>();

            var elemList = xmiMetaModel.GetElementsByTagName("packagedElement");

            //for (var i = 0; i < elemList.Count; i++)
            //{
            //    var current = elemList[i];
            //    if (current.Attributes["xmi:type"].Value == "uml:Association")
            //    {

            //        var owneds = ((XmlElement)current).GetElementsByTagName("ownedEnd").Cast<XmlNode>();

            //        var members = current.Attributes["memberEnd"].Value.Split(' ');

            //        var association = new UmlAssociation
            //        {
            //            MemberOwned = owneds.FirstOrDefault(n => n.Attributes["xmi:id"].Value == members[1])
            //                .Attributes["type"].Value,
            //            MemberEnd = owneds.FirstOrDefault(n => n.Attributes["xmi:id"].Value == members[0])
            //                .Attributes["type"].Value

            //        };
            //        associations.Add(association);
            //    }
            //}
            return associations;
        }
    }
}
