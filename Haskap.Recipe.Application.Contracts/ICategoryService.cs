using Haskap.Recipe.Application.Dtos.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Contracts;
public interface ICategoryService
{
    Task<List<CategoryOutputDto>> GetAllAsync(CancellationToken cancellationToken);
}
