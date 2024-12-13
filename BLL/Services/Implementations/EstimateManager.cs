using BLL.DTO;
using BLL.DTO.Common;
using BLL.DTO.EstimateLabors;
using BLL.DTO.EstimateMaterials;
using BLL.DTO.Estimates;
using BLL.DTO.EstimateSections;
using BLL.DTO.SectionItems;
using DAL.Models;
using DAL.Repositories;
using System.Text.RegularExpressions;
namespace BLL.Services.Implementations
{
    public class EstimateManager : IEstimateManager
    {
        private readonly IEstimateRepository _estimateReopsitory;
        private readonly ITemplatesRepository _templatesRepository;
        private readonly ISectionItemRepository _sectionItemRepository;
        private readonly ITradeRepository _tradeRepository;
        private readonly IEstimateSectionRepository _estimateSectionRepository;
        public EstimateManager(IEstimateRepository estimateReopsitory, ITemplatesRepository templatesRepository, ISectionItemRepository sectionItemRepository,
            ITradeRepository tradeRepository, IEstimateSectionRepository estimateSectionRepository)
        {
            _estimateReopsitory = estimateReopsitory;
            _templatesRepository = templatesRepository;
            _sectionItemRepository = sectionItemRepository;
            _tradeRepository = tradeRepository;
            _estimateSectionRepository = estimateSectionRepository;
        }
        public async Task<ServiceResult> EditCreateEstimate(EstimatesInputDTO input)
        {
            if (input.IsNewRecord)
            {
                Estimate estimate = new()
                {
                    Name = input.Name,
                    UserId = input.UserId,
                    ContactId = input.ContactId,
                    Notes = input.Notes,
                    CreatedDateTime = DateTime.UtcNow,
                    OwnerId = input.OwnerId,
                    StatusId = input.StatusId,
                    CalculateTypeId = input.CalculateTypeId,
                    Square = input.Square,
                    WorkTypeId = input.WorkTypeId,
                    JobCategoryId = input.JobCategoryId,
                    PriceLabor = input.PriceLabor,
                    PriceMaterial = input.PriceMaterial,
                    TotalPrice = input.PriceMaterial + input.PriceLabor,
                    Text = input.Text,

                };

                await _estimateReopsitory.Save(estimate);
                /*
                if (input.TradeTemplateIds.Any())
                {
                    List<EstimateTamplate> estimateTamplates = new();
                    input.TradeTemplateIds.ForEach(tradeTempId =>
                    {
                        EstimateTamplate estimateTamplate = new()
                        {
                            EstimateId = estimate.Id,
                            TradeTemplateId = tradeTempId
                        };
                        estimateTamplates.Add(estimateTamplate);
                    });
                    await _estimateTemplate.SaveRange(estimateTamplates);
                }*/
                return new ServiceResult(ServiceResultStatus.Success, "Estimate added successfully.");
            }
            var existingEstimate = await _estimateReopsitory.GetById(input.Id ?? Guid.Empty);

            if (existingEstimate != null)
            {
                existingEstimate.Name = input.Name;
                existingEstimate.UserId = input.UserId;
                existingEstimate.ContactId = input.ContactId;
                existingEstimate.Notes = input.Notes;
                existingEstimate.OwnerId = input.OwnerId;
                existingEstimate.StatusId = input.StatusId;
                existingEstimate.CalculateTypeId = input.CalculateTypeId;
                existingEstimate.Square = input.Square;
                existingEstimate.WorkTypeId = input.WorkTypeId;
                existingEstimate.JobCategoryId = input.JobCategoryId;
                existingEstimate.PriceLabor = input.PriceLabor;
                existingEstimate.PriceMaterial = input.PriceMaterial;
                existingEstimate.TotalPrice = input.PriceMaterial + input.PriceLabor;
                existingEstimate.Text = input.Text;
                await _estimateReopsitory.Update(existingEstimate);
                return new ServiceResult(ServiceResultStatus.Success, "Estimate updated successfully.");
            }
            return new ServiceResult(ServiceResultStatus.Error, "Estimate not created.");
        }
        public async Task<List<EstimateSummaryDTO>> GetEstimatesList()
        {
            var estimateList = await _estimateReopsitory.GetAll();
            List<EstimateSummaryDTO> estimates = new();
            if (estimateList.Any())
            {
                estimateList.ForEach(input =>
                {
                    estimates.Add(
                    new EstimateSummaryDTO()
                    {
                        Name = input.Name,
                        ContactId = input.ContactId,
                        Notes = input.Notes,
                        CalculateTypeId = input.CalculateTypeId,
                        Square = input.Square,
                        WorkTypeId = input.WorkTypeId,
                        JobCategoryId = input.JobCategoryId,
                        PriceLabor = input.PriceLabor,
                        PriceMaterial = input.PriceMaterial,
                        TotalPrice = input.PriceMaterial + input.PriceLabor,
                        Text = input.Text,
                        StartDate = input.CreatedDateTime
                    });
                });
            }
            return estimates;
        }
        public async Task<EstimatesInputDTO> GetEstimateById(Guid id)
        {
            var estimate = await _estimateReopsitory.GetById(id);
            EstimatesInputDTO estimateSummary = new();
            if (estimate != null)
            {
                estimateSummary.Id = id;
                estimateSummary.Name = estimate.Name;
                estimateSummary.UserId = estimate.UserId;
                estimateSummary.ContactId = estimate.ContactId;
                estimateSummary.Notes = estimate.Notes;
                estimateSummary.OwnerId = estimate.OwnerId;
                estimateSummary.StatusId = estimate.StatusId;
                estimateSummary.CalculateTypeId = estimate.CalculateTypeId;
                estimateSummary.Square = estimate.Square;
                estimateSummary.WorkTypeId = estimate.WorkTypeId;
                estimateSummary.JobCategoryId = estimate.JobCategoryId;
                estimateSummary.PriceLabor = estimate.PriceLabor;
                estimateSummary.PriceMaterial = estimate.PriceMaterial;
                estimateSummary.TotalPrice = estimate.PriceMaterial + estimate.PriceLabor;
                estimateSummary.Text = estimate.Text;
            }
            return estimateSummary;
        }
        public async Task<ServiceResult> DeleteEstimate(Guid id)
        {
            Estimate estimate = await _estimateReopsitory.GetById(id);
            if (estimate == null)
            {
                return new ServiceResult(ServiceResultStatus.NotFound, "Estimate not found.");
            }
            int appointmentDeleted = await _estimateReopsitory.Delete(estimate);
            if (appointmentDeleted > 0)
            {
                return new ServiceResult(ServiceResultStatus.Success);
            }
            return new ServiceResult(ServiceResultStatus.Error, "An error occured while deleting estimate, please try after some time.");
        }

        public async Task<EstimateSummaryDTO> CreateEstimateSection(EstimateTradesInputDTO input)
        {
            EstimateSummaryDTO summary = new();
            var tradeTemplates = await _templatesRepository.GetAllByIds(input.TradeTemplateIds);
            var tradeId = tradeTemplates.Select(x => x.TradeId).FirstOrDefault();
            var trade = await _tradeRepository.GetById(tradeId);
            EstimateSection section = new();
            section.EstimateId = input.EstimateId;
            section.Name = trade.Name;
            await _estimateReopsitory.CreateEstimateSection(section);
            if (tradeTemplates.Any())
            {


                List<SectionsItem> sectionItems = new();
                tradeTemplates.ForEach(tradeTemp =>
                {
                    SectionsItem estimateTamplate = new()
                    {
                        EstimateSectionId = section.Id,
                        TradeTemplateId = tradeTemp.Id,
                        Name = tradeTemp.Name,
                    };
                    sectionItems.Add(estimateTamplate);
                });
                await _sectionItemRepository.SaveRange(sectionItems);

            }
            summary.Id = input.EstimateId;
            summary.SectionId = section.Id;
            //ToDo add another required fields

            return summary;
        }
        public async Task<EstimateSectionInputDTO> GetEstimateSectionBySectionId(Guid sectionId)
        {
            EstimateSectionInputDTO estimateSection = new();
            var section = await _estimateSectionRepository.GetById(sectionId);
            if (section != null)
            {
                estimateSection.SectionName = section.Name;
                estimateSection.SectionId = sectionId;
                List<Guid> templateGuids = new();
                if (section.SectionsItems.Any())
                {
                    foreach (var item in section.SectionsItems)
                    {

                        templateGuids.Add(item.TradeTemplateId ?? Guid.Empty);
                    }
                }
                List<TradeTemplate> tradeTemplates = new();
                if (templateGuids.Any())
                {
                    tradeTemplates = await _templatesRepository.GetAllByIds(templateGuids);
                }
                if (tradeTemplates.Any())
                {

                    var extractedData = (from sectionItem in section.SectionsItems
                                         join template in tradeTemplates on sectionItem.TradeTemplateId equals template.Id
                                         select new SectionItemsSummaryDTO
                                         {
                                             ItemId = sectionItem.Id,
                                             ItemName = sectionItem.Name,
                                             TemplateText = RemoveHtmlTags(template.Text),
                                             EstimateLaborList = GetLaborList(),//TODO --for now just getting dummy data
                                             EstimateMaterialList = GetMaterialList(),//TODO --for now just getting dummy data
                                         }).ToList();
                    estimateSection.SectionItems = extractedData;
                }
            }
            return estimateSection;
        }
        #region private methods
        private List<EstimateMaterialSummaryDTO> GetMaterialList()
        {
            List<EstimateMaterialSummaryDTO> materialList = new();
            for (int i = 0; i < 5; i++)
            {
                materialList.Add(new EstimateMaterialSummaryDTO()
                {
                    Id = new Guid(),
                    Name = "Material " + (i + 1),
                    Quantity = new Decimal(i + 1),
                    Cost = new Decimal(i + 100),
                });
            }
            // Calculating total cost 
            foreach (var material in materialList)
            {
                material.CalculateTotalCost();
            }
            return materialList;
        }
        private List<EstimateLaborSummaryDTO> GetLaborList()
        {
            List<EstimateLaborSummaryDTO> laborList = new();
            for (int i = 0; i < 5; i++)
            {
                laborList.Add(new EstimateLaborSummaryDTO()
                {
                    Id = new Guid(),
                    Name = "Material " + (i + 1),
                    Quantity = new Decimal(i + 1),
                    CostUnit = new Decimal(i * 10 + 1),
                });
            }
            foreach (var labor in laborList)
            {
                labor.CalculateTotalCost();
            }
            return laborList;
        }
        private string RemoveHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", "");
        }
        #endregion
    }
}
