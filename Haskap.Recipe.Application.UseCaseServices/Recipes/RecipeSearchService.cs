using AutoMapper;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using Haskap.Recipe.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class RecipeSearchService : IRecipeSearchService
{
    private readonly IRecipeDbContext _recipeDbContext;
    private readonly IMapper _mapper;

    public RecipeSearchService(
        IRecipeDbContext recipeDbContext,
        IMapper mapper)
    {
        _recipeDbContext = recipeDbContext;
        _mapper = mapper;
    }

    


    public async Task<SearchOutputDto> SearchAsync(SearchInputDto inputDto, CancellationToken cancellationToken)
    {
        var searchQuery = _recipeDbContext.Recipe
            .AsNoTracking()
            .AsQueryable();

        if (string.IsNullOrWhiteSpace(inputDto.SearchName) == false)
        {
            searchQuery = searchQuery.Where(x => x.Name.Contains(inputDto.SearchName));
        }

        if (string.IsNullOrWhiteSpace(inputDto.SearchIngredients) == false)
        {
            var searchIngredients = inputDto.SearchIngredients.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (searchIngredients.Any())
            {
                searchQuery = searchQuery.Include(x => x.Ingredients);

                var recipeQueries = new List<IQueryable<Domain.RecipeAggregate.Recipe>>();
                foreach (var ingredient in searchIngredients)
                {
                    recipeQueries.Add(searchQuery.Where(x => x.Ingredients.Any(y => y.Name.ToLower().Contains(ingredient.ToLower()))));
                }

                IQueryable<Domain.RecipeAggregate.Recipe> query = recipeQueries.FirstOrDefault();
                foreach (var recipeQuery in recipeQueries)
                {
                    query = query.Union(recipeQuery);
                }

                searchQuery = searchQuery.Intersect(query);

                //recipesQuery = recipesQuery.Where(x => x.Ingredients.Any(y => inputDto.SearchIngredients.ToLower().Contains(y.Name.ToLower())));
            }
        }

        if (inputDto.CategoryId.HasValue)
        {
            searchQuery = searchQuery.Include(x => x.Categories)
                .Where(x => x.Categories.Any(y => y.CategoryId == inputDto.CategoryId));
        }

        var filteredCount = await searchQuery.CountAsync();

        var recipes = await searchQuery
            .OrderBy(x => EF.Functions.Random())
            .Skip((inputDto.CurrentPageIndex - 1) * inputDto.PageSize)
            .Take(inputDto.PageSize)
            //.Select(x => new SearchRecipeOutputDto
            //{
            //    RecipeCreatedOn = x.CreatedOn,
            //    RecipeId = x.Id,
            //    RecipeName = x.Name,
            //    RecipePicture = new FileOutputDto
            //    {
            //        Extension = x.Picture.Extension,
            //        NewName = x.Picture.NewName,
            //        OriginalName = x.Picture.OriginalName
            //    }
            //})
            .ToListAsync();

        var recipesOutput = _mapper.Map<List<RecipeOutputDto>>(recipes);

        //recipesOutput = (from recipeOutput in recipesOutput
        //                 join user in _recipeDbContext.User on recipeOutput.OwnerUserId equals user.Id
        //                 select new RecipeOutputDto
        //                 {
        //                     CreatedOn = recipeOutput.CreatedOn,
        //                     Categories = recipeOutput.Categories,
        //                     CreatedUserId = recipeOutput.CreatedUserId,
        //                     Description = recipeOutput.Description,
        //                     Id = recipeOutput.Id,
        //                     Ingredients = recipeOutput.Ingredients,
        //                     IsDraft = recipeOutput.IsDraft,
        //                     ModifiedOn = recipeOutput.ModifiedOn,
        //                     ModifiedUserId = recipeOutput.ModifiedUserId,
        //                     Name = recipeOutput.Name,
        //                     OwnerUserId = recipeOutput.OwnerUserId,
        //                     OwnerUserUsername = user.Credentials.UserName,
        //                     Picture = recipeOutput.Picture,
        //                     Rating = recipeOutput.Rating,
        //                     Slug = recipeOutput.Slug,
        //                     Steps = recipeOutput.Steps,
        //                     ViewCount = recipeOutput.ViewCount
        //                 })
        //                 .ToList();

        var searchOutput = new SearchOutputDto
        {
            Recipes = recipesOutput,
            FilteredCount = filteredCount
        };

        return searchOutput;
    }

}
