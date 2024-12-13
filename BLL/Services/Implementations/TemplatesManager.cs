using BLL.DTO.Templates;
using BLL.DTO.Common;
using DAL.Repositories;
using DAL.Models;
using System.Text.RegularExpressions;
using DAL.Repositories.Implementations;
using System.Diagnostics;
namespace BLL.Services.Implementations
{
    public class TemplatesManager : ITemplatesManager
    {
        private readonly ITemplatesRepository _templatesRepository;

        public TemplatesManager(ITemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public async Task<ServiceResult> DeleteTemplate(Guid id)
        {
            var template = await _templatesRepository.GetById(id);
            if (template == null)
            {
                return new ServiceResult(ServiceResultStatus.NotFound, "Template not found.");
            }

            int templatesDeleted = await _templatesRepository.Delete(template);
            if (templatesDeleted > 0)
            {
                return new ServiceResult(ServiceResultStatus.Success);
            }

            return new ServiceResult(ServiceResultStatus.Error, "An error occurred while deleting the template, please try after some time.");
        }

        public async Task<ServiceResult> EditCreateTemplate(TemplatesInputDTO input)
        {
            if (input.IsNewRecord)
            {
                TradeTemplate tradeTemplate = new()
                {
                    Name = input.Name,
                    TradeId = input.TradeId,
                    Text = input.Text,
                    CreatedDateTime = DateTime.UtcNow,
                    OwnerId = input.UserId
                };

                await _templatesRepository.Save(tradeTemplate);
                return new ServiceResult(ServiceResultStatus.Success, "Trade template added successfully.");
            }
            var existingTemplate = await _templatesRepository.GetById(input.Id ?? Guid.Empty);

            if (existingTemplate != null)
            {
                existingTemplate.Name = input.Name;
                existingTemplate.TradeId = input.TradeId;
                existingTemplate.Text = input.Text;
                await _templatesRepository.Update(existingTemplate);
                return new ServiceResult(ServiceResultStatus.Success, "Trade template updated successfully.");
            }
            return new ServiceResult(ServiceResultStatus.Error, "Trade template not created.");
        }

        public async Task<TemplatesInputDTO> GetTemplateById(Guid id)
        {
            var templateResult = await _templatesRepository.GetById(id);
            TemplatesInputDTO template = new();
            if (templateResult != null)
            {
                string fullText = templateResult.Text;
                string searchText = "<p><br></p>";
                string newText = Regex.Replace(fullText, searchText, string.Empty);
                template.Text = newText;
                template.Id = templateResult.Id;
                template.Name = templateResult.Name;
                template.TradeId = templateResult.TradeId;
                template.TradeName = templateResult.Trade.Name;
            }
            return template;
        }

        public async Task<List<TemplateDTO>> GetTemplatesList(Guid id)
        {
            var templates = await _templatesRepository.GetAllByTrade(id);


            var templateDtos = templates.Select(t => new TemplateDTO
            {

                Id = t.Id,
                Name = t.Name,
                TradeName = t.Trade.Name ?? string.Empty,
                Text = t.Text,
            }).ToList();

            return templateDtos;
        }

        public async Task<List<TemplateDTO>> GetTemplatesList()
        {
            var templates = await _templatesRepository.GetAll();


            var templateDtos = templates.Select(t => new TemplateDTO
            {

                Id = t.Id,
                Name = t.Name,
                TradeName = t.Trade.Name ?? string.Empty,
                Text = t.Text,
            }).ToList();

            return templateDtos;
        }

        public async Task<List<TemplateDTO>> GetTemplateListByIds(List<Guid> ids)
        {
            var templateList = await _templatesRepository.GetAllByIds(ids);
            List<TemplateDTO> templatePreview = new();
            if (templateList.Any())
            {
                templateList.ForEach(template =>
                {
                    TemplateDTO templateDetail = new();
                    string fullText = template.Text;
                    string searchText = "<p><br></p>";
                    string newText = Regex.Replace(fullText, searchText, string.Empty);
                    templatePreview.Add(new TemplateDTO()
                    {
                        Id = template.Id,
                        Name = template.Name,
                        Text = newText,
                    });
                });
            }
            return templatePreview;
        }
    }
}
