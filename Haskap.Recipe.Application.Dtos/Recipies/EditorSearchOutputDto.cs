﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class EditorSearchOutputDto
{
    public Guid RecipeId { get; set; }
    public string RecipeName { get; set; }
    public Guid OwnerUserId { get; set; } 
    public string OwnerUserUsername { get; set; }
}