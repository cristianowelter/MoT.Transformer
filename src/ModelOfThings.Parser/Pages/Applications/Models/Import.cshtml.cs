using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;
using ModelOfThings.Parser.Services;
using Newtonsoft.Json;

namespace ModelOfThings.Parser.Pages.Applications.Models
{
    public class ImportModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UmlParser _umlParser;

        public ImportModel(ApplicationDbContext context, UmlParser umlParser)
        {
            _context = context;
            _umlParser = umlParser;
        }

        public MddApplication MddApplication { get; set; }

        [BindProperty]
        public UmlModel UmlModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string appId)
        {
            if (appId == null)
            {
                return NotFound();
            }

            MddApplication = await _context.MddApplications
                .FirstOrDefaultAsync(m => m.Id == appId);

            if (MddApplication == null)
            {
                return NotFound();
            }

            UmlModel = new UmlModel
            {
                MddApplicationId = MddApplication.Id
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UmlModel.UmlModelFile == null || UmlModel.UmlModelFile.Length <= 0)
            {
                return Page();
            }
                
            var file = UmlModel.UmlModelFile;

            var fileName = UmlModel.MddApplicationId.ToString();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "_xmi", fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileInfo = new FileInfo(file.FileName);

            var filePath = Path.Combine(path, $"{fileName}.{fileInfo.Extension}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _context.UmlModels.Add(UmlModel);
            await _context.SaveChangesAsync();

            await _umlParser.XmiParserAsync(UmlModel, filePath);

            return RedirectToPage("./Index");
        }

        //private async Task XmiParserAsync(UmlModel model, string filePath)
        //{
        //    var xmiMetaModel = new XmlDocument();
        //    xmiMetaModel.Load(filePath);

        //    var stereotypes = _umlParser.GetMddStereotypes(xmiMetaModel.DocumentElement);
        //    var useCases = _umlParser.GetUmlUseCase(xmiMetaModel);
        //    var actors = _umlParser.GetUmlActor(xmiMetaModel);

        //    foreach (var useCase in useCases)
        //    {
        //        useCase.MddApplicationId = model.MddApplicationId;
        //        useCase.MddComponents = new List<MddComponent>();

        //        var stereotypesUsed = stereotypes.Where(a => a.ElementId == useCase.Id);
        //        foreach (var stereotype in stereotypesUsed)
        //        {
        //            if (_context.MddComponents.Any(x => x.MddApplicationId == model.MddApplicationId && x.UmlElementId == stereotype.ElementId && x.Stereotype == stereotype.Name))
        //            {
        //                break;
        //            }

        //            var filePathJson = Path.Combine(Directory.GetCurrentDirectory(), "_json", $"{stereotype.Name.ToLower()}.json");

        //            var component = new MddComponent();

        //            if (System.IO.File.Exists(filePathJson))
        //            {
        //                var file = System.IO.File.ReadAllText(filePathJson);
        //                component = JsonConvert.DeserializeObject<MddComponent>(file);
        //            }

        //            //component.Id = Guid.NewGuid().ToString();
        //            //component.MddApplicationId = model.MddApplicationId;
        //            //component.Stereotype = stereotype.Name;
        //            //component.Name = useCase.Name;
        //            //component.UmlElementId = useCase.Id;
        //            //component.UmlUseCaseId = useCase.Id;

        //            if (component.MddComponents.Any())
        //            {
        //                foreach (var comp in component.MddComponents)
        //                {
        //                    var filePathJsonChild = Path.Combine(Directory.GetCurrentDirectory(), "_json", $"{comp.Stereotype}.json");

        //                    var childComponent = new MddComponent();

        //                    if (System.IO.File.Exists(filePathJsonChild))
        //                    {
        //                        var file = System.IO.File.ReadAllText(filePathJsonChild);
        //                        childComponent = JsonConvert.DeserializeObject<MddComponent>(file);
        //                    }

        //                    comp.Id = !string.IsNullOrEmpty(comp.Id) ? $"{useCase.Id}-{comp.Id}" : Guid.NewGuid().ToString();
        //                    comp.MddProperties = childComponent.MddProperties;
        //                    comp.ParentId = useCase.Id;
        //                    comp.MddApplicationId = model.MddApplicationId;
        //                    comp.UmlUseCaseId = useCase.Id;

        //                    if (!string.IsNullOrEmpty(comp.Connections))
        //                    {
        //                        var conn = comp.Connections.Split(" ");

        //                        for (var i = 0; i < conn.Length; i++)
        //                        {
        //                            conn[i] = $"{useCase.Id}-{conn[i]}";
        //                        }

        //                        comp.Connections = string.Join(' ', conn);
        //                    }

        //                    useCase.MddComponents.Add(comp);
        //                    //_context.Add(comp);
        //                    //await _context.SaveChangesAsync();
        //                }
        //            }
        //        }

        //        _context.Add(useCase);
        //        await _context.SaveChangesAsync();

        //    }
        //}
    }
}