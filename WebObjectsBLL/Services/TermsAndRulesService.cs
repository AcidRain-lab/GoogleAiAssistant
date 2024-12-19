using AutoMapper;
using DAL.Models;
using WebObjectsBLL.DTO;
using Microsoft.EntityFrameworkCore;

namespace WebObjectsBLL.Services
{
    public class TermsAndRulesService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public TermsAndRulesService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TermsAndRulesDto>> GetTermsAndRulesTreeAsync()
        {
            var termsAndRules = await _context.TermsAndRules
                .Include(t => t.SubTermsAndRules)
                    .ThenInclude(st => st.NestedSubTerms)
                .OrderBy(t => t.Title)
                .ToListAsync();

            return _mapper.Map<List<TermsAndRulesDto>>(termsAndRules);
        }

        public async Task<TermsAndRulesDto> AddTermsAndRulesAsync(TermsAndRulesDto termsAndRulesDto)
        {
            var termsAndRules = _mapper.Map<TermsAndRule>(termsAndRulesDto);
            _context.TermsAndRules.Add(termsAndRules);
            await _context.SaveChangesAsync();

            return _mapper.Map<TermsAndRulesDto>(termsAndRules);
        }

        public async Task<TermsAndRulesDto?> GetTermsAndRulesByIdAsync(Guid id)
        {
            var termsAndRules = await _context.TermsAndRules
                .Include(t => t.SubTermsAndRules)
                    .ThenInclude(st => st.NestedSubTerms)
                .FirstOrDefaultAsync(t => t.Id == id);

            return termsAndRules == null ? null : _mapper.Map<TermsAndRulesDto>(termsAndRules);
        }

        public async Task UpdateTermsAndRulesAsync(TermsAndRulesDto updatedTermsAndRulesDto)
        {
            var existingTermsAndRules = await _context.TermsAndRules
                .Include(t => t.SubTermsAndRules)
                    .ThenInclude(st => st.NestedSubTerms)
                .FirstOrDefaultAsync(t => t.Id == updatedTermsAndRulesDto.Id);

            if (existingTermsAndRules != null)
            {
                _mapper.Map(updatedTermsAndRulesDto, existingTermsAndRules);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTermsAndRulesAsync(Guid id)
        {
            var termsAndRules = await _context.TermsAndRules.FirstOrDefaultAsync(t => t.Id == id);
            if (termsAndRules != null)
            {
                _context.TermsAndRules.Remove(termsAndRules);
                await _context.SaveChangesAsync();
            }
        }
    }
}
