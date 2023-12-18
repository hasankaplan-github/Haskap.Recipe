using AutoMapper;
using Haskap.Recipe.Application.Dtos.Recipies;
using Haskap.Recipe.Domain.RecipeAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Mappings;
public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe.Domain.RecipeAggregate.Recipe, RecipeOutputDto>();
    }
}

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientOutputDto>();
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
            .ForMember(x => x.Extension, x => x.MapFrom(y => y.Picture.Extension))
            .ForMember(x => x.OriginalName, x => x.MapFrom(y => y.Picture.OriginalName))
            .ForMember(x => x.NewName, x => x.MapFrom(y => y.Picture.NewName));
    }
}
