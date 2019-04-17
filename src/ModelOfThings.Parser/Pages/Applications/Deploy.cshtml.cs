using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;
using ModelOfThings.Parser.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModelOfThings.Parser.Pages.Applications
{
    public class DeployModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UmlParser _umlParser;
        private readonly AmazonS3Service _amazonS3Service;

        public DeployModel(ApplicationDbContext context, UmlParser umlParser, AmazonS3Service amazonS3Service)
        {
            _context = context;
            _umlParser = umlParser;
            _amazonS3Service = amazonS3Service;
        }

        public async Task<IActionResult> OnGetAsync(string appId)
        {
            if (appId == null)
            {
                return NotFound();
            }

            var application = await _context.MddApplications
                .Include(a => a.UmlUseCases)
                    .ThenInclude(b => b.Associations)
                .Include(a => a.UmlUseCases)
                    .ThenInclude(b => b.MddComponents)
                        .ThenInclude(c => c.MddProperties)
                .Include(a => a.CloudProviders)
                .SingleOrDefaultAsync(m => m.Id == appId);

            if (application == null)
            {
                return NotFound();
            }

            var json = new List<object>();

            foreach (var useCase in application.UmlUseCases)
            {
                var tab = new
                {
                    id = useCase.Id,
                    type = "tab",
                    label = useCase.Name
                };
                json.Add(tab);

                var associationsOutput = new List<string>();

                if (useCase.Associations.Any())
                {
                    foreach (var association in useCase.Associations)
                    {
                        var idOut = $"{association.Id}-out";
                        var idIn = $"{association.Id}-in";

                        associationsOutput.Add(idOut);

                        var tabDest = application.UmlUseCases.FirstOrDefault(x => x.UmlElementId == association.Destiny);

                        if (association.Type == AssociationType.Include)
                        {
                            var components = tabDest.MddComponents.Where(x => x.ConnectionIn);

                            var componentsIn = new string[] { };

                            if (components.Any())
                            {
                                componentsIn = components.Select(x => x.Id).ToArray();
                            }

                            var linkOutNode = new
                            {
                                id = idOut,
                                type = "link out",
                                z = useCase.Id,
                                name = "Include",
                                links = new[] { new[] { idIn }},
                                x = 700,
                                y = 100,
                                wires = new string[] { }
                        };

                            var linkInNode = new
                            {
                                id = idIn,
                                type = "link in",
                                z = tabDest.Id,
                                name = "Include",
                                links = new [] { new[] { idOut }},
                                x = 100,
                                y = 100,
                                wires = new[] { componentsIn }
                            };

                            json.Add(linkOutNode);
                            json.Add(linkInNode);
                        }

                        if (association.Type == AssociationType.Extend)
                        {
                            var components = useCase.MddComponents.Where(x => x.ConnectionIn);

                            var componentsIn = new string[] { };

                            if (components.Any())
                            {
                                componentsIn = components.Select(x => x.Id).ToArray();
                            }

                            var linkInNode = new
                            {
                                id = $"{association.Destiny}-in",
                                type = "link in",
                                z = useCase.Id,
                                name = "Extend",
                                links = new [] { new[] { $"{association.Destiny}-out" }},
                                x = 100,
                                y = 100,
                                wires = new[] { componentsIn }
                            };

                            json.Add(linkInNode);
                        }

                        if (association.Type == AssociationType.ExtensionPoint)
                        {
                            var idOutAux = Guid.NewGuid().ToString();

                            var switchNode = new
                            {
                                id = idOut,
                                type = "switch",
                                z = useCase.Id,
                                name = "Extend",
                                outputs = 1,
                                x = 500,
                                y = 100,
                                wires = new[] { new[] { $"{useCase.UmlElementId}{association.UmlElementId}-out" }}
                            };

                            var linkOutNode = new
                            {
                                id = $"{useCase.UmlElementId}{association.UmlElementId}-out",
                                type = "link out",
                                z = useCase.Id,
                                name = "Extend",
                                links = new[] { new[] { $"{useCase.UmlElementId}{association.UmlElementId}-in" }},
                                x = 700,
                                y = 300,
                                wires = new string[] { }
                            };

                            json.Add(switchNode);
                            json.Add(linkOutNode);
                        }
                    }
                }

                if (useCase.MddComponents.Any())
                {
                    foreach (var component in useCase.MddComponents)
                    {
                        var wires = !string.IsNullOrEmpty(component.Connections) ? component.Connections.Split(' ').ToList() : new List<string>();

                        if (component.ConnectionOut)
                        {
                            wires.AddRange(associationsOutput);
                        }

                        var componentJson = new
                        {
                            z = useCase.Id,
                            x = component.PositionX,
                            y = component.PositionY,
                            id = component.Id,
                            type = component.Type,
                            name = component.Name,
                            wires = new[] { wires.ToArray() }
                        };

                        var jo = JObject.FromObject(componentJson);

                        foreach (var property in component.MddProperties)
                        {
                            switch (property.Type)
                            {
                                case "bool":
                                    jo.Add(property.Name, Convert.ToBoolean(property.Value));
                                    break;
                                case "array":
                                    var arr = JsonConvert.DeserializeObject(property.Value);
                                    jo.Add(property.Name, JToken.Parse(arr.ToString()));
                                    break;
                                default:
                                    jo.Add(property.Name, property.Value);
                                    break;
                            }
                        }

                        json.Add(jo);
                    }
                }

                //var associations = _context.UmlAssociations.Where(a => a.MemberEnd == useCase.Id);

                //if (associations.Any())
                //{
                //    var linkIn = new
                //    {
                //        id = $"{useCase.Id}-linkin",
                //        type = "link in",
                //        z = useCase.Id,
                //        name = "Include",
                //        links = associations.Select(x => $"{x.MemberOwned}-linkout").ToArray(),
                //        x = 50,
                //        y = 200,
                //        wires = new string[0]
                //    };

                //    json.Add(linkIn);
                //}

                //if (component.Type == "tab")
                //{
                //    var comment = new
                //    {
                //        type = "comment",
                //        z = component.Id,
                //        name = "Model Of Things",
                //        info = "MDD tool",
                //        x = 150,
                //        y = 50
                //    };

                //    json.Add(comment);
                //}

                //var componentJson = new
                //{
                //    z = component.ParentId,
                //    x = component.PositionX,
                //    y = component.PositionY,
                //    id = component.Id,
                //    type = component.Type,
                //    name = component.Name,
                //    label = component.Name,
                //    //wires = new string[] { !string.IsNullOrEmpty(component.Connections) ? component.Connections.Split(' ') : null }
                //};

                //var jo = JObject.FromObject(componentJson);

                //foreach (var property in component.MddProperties)
                //{
                //    switch (property.Type)
                //    {
                //        case "bool":
                //            jo.Add(property.Name, Convert.ToBoolean(property.Value));
                //            break;
                //        case "array":
                //            var arr = JsonConvert.DeserializeObject(property.Value);

                //            jo.Add(property.Name, JToken.Parse(arr.ToString()));
                //            break;
                //        default:
                //            jo.Add(property.Name, property.Value);
                //            break;
                //    }
                //}


                //json.Add(jo);
            }
       
            //return new JsonResult(json);

            var jsonString = JsonConvert.SerializeObject(json);


            var byteArray = Encoding.UTF8.GetBytes(jsonString);
            var stream = new MemoryStream(byteArray);

            await _amazonS3Service.CreateBucketAsync(application.Id);

            await _amazonS3Service.UploadFileAsync(application.Id, stream, application.NormalizedName);

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "serverless.zip");

            var pathPackage = Path.Combine(
                Directory.GetCurrentDirectory(), "_serverless", "package.json");

            var pathCloudFormation = Path.Combine(
                Directory.GetCurrentDirectory(), "_serverless", "cloudformation.yaml");

            var pathSettings = Path.Combine(
                Directory.GetCurrentDirectory(), "_serverless", "settings.js");

            var pathSimpleProxyApi = Path.Combine(
                Directory.GetCurrentDirectory(), "_serverless", "simple-proxy-api.yaml");

            using (var zipToOpen = new FileStream(path, FileMode.Open))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    using (var streamReader = new StreamReader(pathPackage))
                    {
                        var content = await streamReader.ReadToEndAsync();

                        content = content
                            .Replace("YOUR_ACCOUNT_ID", application.CloudProviders.FirstOrDefault()?.AccountId)
                            .Replace("YOUR_UNIQUE_BUCKET_NAME", application.Id)
                            .Replace("YOUR_AWS_REGION", "us-east-2")
                            .Replace("YOUR_STACK_NAME", application.NormalizedName)
                            .Replace("YOUR_LAMBDA_FUNCTION_NAME", application.NormalizedName)
                            .Replace("YOUR_SERVERLESS_EXPRESS_LAMBDA_FUNCTION_NAME", application.NormalizedName);

                        var packEntry = archive.GetEntry("package.json");
                        packEntry?.Delete();

                        packEntry = archive.CreateEntry("package.json");
                        using (var writer = new StreamWriter(packEntry.Open()))
                        {
                            await writer.WriteAsync(content);
                        }
                    }

                    using (var streamReader = new StreamReader(pathCloudFormation))
                    {
                        var content = await streamReader.ReadToEndAsync();

                        content = content
                            .Replace("YOUR_ACCOUNT_ID", application.CloudProviders.FirstOrDefault()?.AccountId)
                            .Replace("YOUR_UNIQUE_BUCKET_NAME", application.Id)
                            .Replace("YOUR_AWS_REGION", "us-east-2")
                            .Replace("YOUR_STACK_NAME", application.NormalizedName)
                            .Replace("YOUR_LAMBDA_FUNCTION_NAME", application.NormalizedName)
                            .Replace("YOUR_SERVERLESS_EXPRESS_LAMBDA_FUNCTION_NAME", application.NormalizedName);

                        var cloudFormationEntry = archive.GetEntry("cloudformation.yaml");
                        cloudFormationEntry?.Delete();

                        cloudFormationEntry = archive.CreateEntry("cloudformation.yaml");
                        using (var writer = new StreamWriter(cloudFormationEntry.Open()))
                        {
                            await writer.WriteAsync(content);
                        }
                    }

                    using (var streamReader = new StreamReader(pathSettings))
                    {
                        var content = await streamReader.ReadToEndAsync();

                        content = content
                            .Replace("YOUR_ACCOUNT_ID", application.CloudProviders.FirstOrDefault()?.AccountId)
                            .Replace("YOUR_UNIQUE_BUCKET_NAME", application.Id)
                            .Replace("YOUR_AWS_REGION", "us-east-2")
                            .Replace("YOUR_STACK_NAME", application.NormalizedName)
                            .Replace("YOUR_LAMBDA_FUNCTION_NAME", application.NormalizedName)
                            .Replace("YOUR_SERVERLESS_EXPRESS_LAMBDA_FUNCTION_NAME", application.NormalizedName);

                        var settingsEntry = archive.GetEntry("settings.js");
                        settingsEntry?.Delete();

                        settingsEntry = archive.CreateEntry("settings.js");

                        using (var writer = new StreamWriter(settingsEntry.Open()))
                        {
                            await writer.WriteAsync(content);
                        }
                    }

                    using (var streamReader = new StreamReader(pathSimpleProxyApi))
                    {
                        var content = await streamReader.ReadToEndAsync();

                        content = content
                            .Replace("YOUR_ACCOUNT_ID", application.CloudProviders.FirstOrDefault()?.AccountId)
                            .Replace("YOUR_UNIQUE_BUCKET_NAME", application.Id)
                            .Replace("YOUR_AWS_REGION", "us-east-2")
                            .Replace("YOUR_STACK_NAME", application.NormalizedName)
                            .Replace("YOUR_LAMBDA_FUNCTION_NAME", application.NormalizedName)
                            .Replace("YOUR_SERVERLESS_EXPRESS_LAMBDA_FUNCTION_NAME", application.NormalizedName);


                        var spaEntry = archive.GetEntry("simple-proxy-api.yaml");
                        spaEntry?.Delete();

                        spaEntry = archive.CreateEntry("simple-proxy-api.yaml");

                        using (var writer = new StreamWriter(spaEntry.Open()))
                        {
                            await writer.WriteAsync(content);
                        }
                    }
                }
            }

            return File("/serverless.zip", "application/zip", $"{application.Name}-serverless.zip");
        }
    }
}