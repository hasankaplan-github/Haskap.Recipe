using AutoMapper;
using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Application.Dtos.IngredientGroups;
using Haskap.Recipe.Application.Dtos.Recipes;
using Haskap.Recipe.Application.Dtos.Units;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.IngredientGroupAggregate;
using Haskap.Recipe.Domain.RecipeAggregate;
using Haskap.Recipe.Domain.UnitAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Mappings;
public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe.Domain.RecipeAggregate.Recipe, RecipeOutputDto>()
            .ForMember(x => x.Rating, x => x.MapFrom<RatingValueResolver>());
    }
}

public class RatingValueResolver : IValueResolver<Domain.RecipeAggregate.Recipe, RecipeOutputDto, short>
{
    private readonly long _maxViewCount;

    public RatingValueResolver(IRecipeDbContext recipeDbContext)
    {
        _maxViewCount = recipeDbContext.Recipe
            .Max(x => x.ViewCount);
    }

    public short Resolve(Domain.RecipeAggregate.Recipe source, RecipeOutputDto destination, short destMember, ResolutionContext context)
    {
        var rating = (short)Math.Round((decimal)source.ViewCount * 5 / _maxViewCount, MidpointRounding.AwayFromZero);

        return rating;
    }
}

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientOutputDto>();
    }
}

public class IngredientGroupProfile : Profile
{
    public IngredientGroupProfile()
    {
        CreateMap<IngredientGroup, IngredientGroupOutputDto>();
    }
}

public class RecipeCategoryProfile : Profile
{
    public RecipeCategoryProfile()
    {
        CreateMap<RecipeCategory, RecipeCategoryOutputDto>();
    }
}

public class AmountProfile : Profile
{
    public AmountProfile()
    {
        CreateMap<Amount, AmountOutputDto>();
    }
}

public class SlugProfile : Profile
{
    public SlugProfile()
    {
        CreateMap<Slug, SlugOutputDto>();
    }
}

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<Unit, UnitOutputDto>();
    }
}

public class StepProfile : Profile
{
    public StepProfile()
    {
        CreateMap<Step, StepOutputDto>();
    }
}

public class StepPictureProfile : Profile
{
    public StepPictureProfile()
    {
        CreateMap<StepPicture, StepPictureOutputDto>()
            .ForMember(x => x.Extension, x => x.MapFrom(y => y.File.Extension))
            .ForMember(x => x.OriginalName, x => x.MapFrom(y => y.File.OriginalName))
            .ForMember(x => x.NewName, x => x.MapFrom(y => y.File.NewName));
    }
}
