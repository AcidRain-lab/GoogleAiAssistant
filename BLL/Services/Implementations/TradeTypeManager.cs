using BLL.DTO.TradeTypes;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class TradeTypeManager : ITradeTypeManager
    {
        private readonly ITradeTypeRepository _tradeTypeRepository;
        public TradeTypeManager(ITradeTypeRepository tradeTypeRepository)
        {
            _tradeTypeRepository = tradeTypeRepository;
        }

        public async Task<List<TradeTypeDTO>> GetTradeTypeList()
        {
            var tradeTypeList = await _tradeTypeRepository.GetTradeTypeList();
            List<TradeTypeDTO> tradeTypes = new();
            if (tradeTypeList.Any())
            {
                tradeTypeList.ForEach(tradeType =>
                {
                    TradeTypeDTO tradeTypeDTO = new()
                    {
                        Id = tradeType.Id,
                        Name = tradeType.Name ?? ""
                    };
                    tradeTypes.Add(tradeTypeDTO);
                });
            }
            return tradeTypes;
        }
    }
}
