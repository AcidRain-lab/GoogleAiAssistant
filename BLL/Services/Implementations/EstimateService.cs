using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using BLL.DTO.Estimates;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.Implementations
{
    public class EstimateService
    {
        private readonly CrmContext _context;

        public EstimateService(CrmContext context)
        {
            _context = context;
        }

        public List<EstimateDTO> GetAllEstimates()
        {

            var estimates = _context.Estimates.Select(e => new EstimateDTO
            {
                Id = e.Id,
                Name = e.Name,
                Notes = e.Notes,
                CreatedDateTime = e.CreatedDateTime,
                Square = e.Square,
                PriceMaterial = e.PriceMaterial,
                PriceLabor = e.PriceLabor,
                TotalPrice = e.TotalPrice,
                //  EstimateSections

                //
            }).ToList();

            return estimates;
        }

        public EstimateDTO GetEstimateDetails(Guid estimateId)
        {
            var estimate = _context.Estimates.Where(e => e.Id == estimateId)
                .Select(e => new EstimateDTO
                {
                    // Fill
                }).FirstOrDefault();

            return estimate;
        }

        public List<EstimateDTO> GetEstimatesByContactId(Guid contactId)
        {
            var estimates = _context.Estimates
                .Where(e => e.ContactId == contactId)
                .Select(e => new EstimateDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Notes = e.Notes,
                    CreatedDateTime = e.CreatedDateTime,
                    Square = e.Square,
                    PriceMaterial = e.PriceMaterial,
                    PriceLabor = e.PriceLabor,
                    TotalPrice = e.TotalPrice,
                    EstimateSections = e.EstimateSections.Select(es => new EstimateSectionDTO
                    {
                        Id = es.Id,
                        Name = es.Name,
                        SectionItems = es.SectionsItems.Select(si => new SectionItemDTO
                        {
                            Id = si.Id,
                            Name = si.Name,
                            Materials = si.EstimateMaterials.Select(em => new EstimateMaterialDTO
                            {
                                Id = em.Id,
                                Name = em.Name,
                                Quantity = em.Quantity,
                                Cost = em.Cost
                            }).ToList(),
                            Labors = si.EstimateLabors.Select(el => new EstimateLaborDTO
                            {
                                Id = el.Id,
                                Name = el.Name,
                                Quantity = el.Quantity,
                                CostUnit = el.CostUnit
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();

            return estimates;
        }



        public EstimateDTO CreateEstimateForContact(Guid contactId, Guid userId)
        {
            var defaultCalculateTypeId = _context.EstimateCalculateTypes
                                         .Select(ect => ect.Id)
                                         .FirstOrDefault(); 

            if (defaultCalculateTypeId == default(int))
            {
                
                throw new InvalidOperationException("Not found EstimateCalculateType.");
            }


            var defaultWorkTypeId = _context.WorkTypes
                                    .Select(wt => wt.Id)
                                    .FirstOrDefault(); 

            if (defaultWorkTypeId == default(int))
            {
               
                throw new InvalidOperationException("Not found  WorkTypes.");
            }


            var defaultJobCategoryId = _context.JobCategories
                                        .Select(jc => jc.Id)
                                        .FirstOrDefault(); 

            if (defaultJobCategoryId == default(int))
            {
               
                throw new InvalidOperationException("Not found  JobCategories.");
            }


            var estimate = new Estimate
            {
                Id = Guid.NewGuid(),
                ContactId = contactId,
                Name = "...",
                Square = 0,
                Notes = "...",
                PriceMaterial = 0,
                PriceLabor = 0,
                TotalPrice = 0,
                CalculateTypeId = defaultCalculateTypeId,
                WorkTypeId = defaultWorkTypeId,
                JobCategoryId = defaultJobCategoryId,
                UserId = userId

        };

            _context.Estimates.Add(estimate);
            _context.SaveChanges();

            return new EstimateDTO
            {
                Id = estimate.Id,
                Name = estimate.Name,
                Notes = estimate.Notes,
                CreatedDateTime = estimate.CreatedDateTime,
                Square = estimate.Square,
                PriceMaterial = estimate.PriceMaterial,
                PriceLabor = estimate.PriceLabor,
                TotalPrice = estimate.TotalPrice,
                ContactId = contactId,
                // todo sections create
            };
        }


    }


}
