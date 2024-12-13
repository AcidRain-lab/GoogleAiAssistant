using BLL.DTO.Common;
using BLL.DTO.Templates;
using BLL.DTO.Trades;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Implementations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL.Services.Implementations
{
    public class TradeManager : ITradeManager
    {
        private readonly ITradeRepository _tradeRepository;
        public TradeManager(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<ServiceResult> DeleteTrade(Guid id)
        {
            Trade trade = await _tradeRepository.GetById(id);
            if (trade == null)
            {
                return new ServiceResult(ServiceResultStatus.NotFound, "Trade not found.");
            }
            int tradeDeleted = await _tradeRepository.Delete(trade);
            if (tradeDeleted > 0)
            {
                return new ServiceResult(ServiceResultStatus.Success);
            }
            return new ServiceResult(ServiceResultStatus.Error, "An error occured while deleting trade, please try after some time.");
        }

        public async Task<ServiceResult> EditCreateTrade(TradesInputDTO input)
        {
            if (input.IsNewRecord)
            {
                Trade trade = new()
                {
                    Name = input.Name,
                };

                await _tradeRepository.Save(trade);
                return new ServiceResult(ServiceResultStatus.Success, "Trade added successfully.");
            }
            var existingTrade = await _tradeRepository.GetById(input.Id ?? Guid.Empty);

            if (existingTrade != null)
            {
                existingTrade.Name = input.Name;
                await _tradeRepository.Update(existingTrade);
                return new ServiceResult(ServiceResultStatus.Success, "Trade updated successfully.");
            }
            return new ServiceResult(ServiceResultStatus.Error, "Trade not created.");
        }

        public async Task<TradesInputDTO> GetTradeById(Guid id)
        {
            var tradeResult = await _tradeRepository.GetById(id);
            TradesInputDTO trade = new();
            if (tradeResult != null)
            {
                trade.Id = id;
                trade.Name = tradeResult.Name ?? string.Empty;
                if (tradeResult.TradeTemplates.Any())
                {
                    List<TemplateDTO> templates = new();
                    foreach (var item in tradeResult.TradeTemplates)
                    {
                        string fullText = item.Text;
                        string searchText = "<p><br></p>";
                        string newText = Regex.Replace(fullText, searchText, string.Empty);
                        templates.Add(new TemplateDTO()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Text = newText,
                        });
                    }
                    trade.TemplatesList = templates;
                }
            }
            return trade;
        }



        public async Task<List<TradesInputDTO>> GetTradeList()
        {
            var tradeList = await _tradeRepository.GetAll();
            List<TradesInputDTO> trades = new();
            if (tradeList.Any())
            {
                tradeList.ForEach(trade =>
                {
                    trades.Add(new TradesInputDTO()
                    {
                        Id = trade.Id,
                        Name = trade.Name ?? string.Empty
                    });
                });
            }
            return trades;
        }
        public async Task<List<TemplateDTO>> GetTradeTemplateListByTradeIds(List<Guid> ids)
        {
            var tradeList = await _tradeRepository.GetAllByIds(ids);
            List<TemplateDTO> trades = new();
            if (tradeList.Any())
            {
                tradeList.ForEach(trade =>
                {
                    TemplateDTO tradeDetail = new();
                    if (trade.TradeTemplates.Any())
                    {

                        foreach (var item in trade.TradeTemplates)
                        {
                            string fullText = item.Text;
                            string searchText = "<p><br></p>";
                            string newText = Regex.Replace(fullText, searchText, string.Empty);
                            trades.Add(new TemplateDTO()
                            {
                                Id = item.Id,
                                TradeId = trade.Id,
                                TradeName = trade.Name ?? string.Empty,
                                Name = item.Name,
                                Text = newText,
                            });
                        }

                    }
                });
            }
            return trades;
        }

    }
}
