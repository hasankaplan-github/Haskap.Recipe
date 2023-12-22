using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haskap.Recipe.Application.Dtos.Recipies;
using Haskap.Recipe.Application.Dtos.Common;

namespace Haskap.Recipe.Domain.RecipeAggregate.Events;
public record StepDeletedDomainEvent(
    Guid RecipeId,
    Guid StepId,
    string WebRootPath) : DomainEvent;
