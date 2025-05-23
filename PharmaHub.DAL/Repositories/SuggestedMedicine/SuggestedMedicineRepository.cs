﻿using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.DALRepositories;

public class SuggestedMedicineRepository : GenericRepository<SuggestedMedicine>, ISuggestedMedicineRepository
{
    
    public SuggestedMedicineRepository(ApplicationDbContext context) : base(context) { }

    public async Task AddRangeSuggestedMedicineAsync(params SuggestedMedicine[] newMed)
    {
        if (newMed == null)
            throw new ArgumentNullException(nameof(newMed));
        await _context.SuggestedMedicines.AddRangeAsync(newMed);    
    }

    public async Task<IReadOnlyList<SuggestedMedicine>> GetSuggestedMedicineByName(string name)
    {
        return await _context.SuggestedMedicines
            .AsNoTracking()
            .Where(s =>EF.Functions.Like(s.Name,$"%{name}%"))
            .ToListAsync();
    }


}

