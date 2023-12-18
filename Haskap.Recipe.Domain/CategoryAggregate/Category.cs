using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Shared.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.CategoryAggregate;
public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public Guid? ParentCategoryId { get; private set; }

    private List<Category> _childCategories = new();
    public IReadOnlyList<Category> ChildCategories => _childCategories.AsReadOnly();



    private Category()
    {
        
    }

    public Category(Guid id, string name)
    {
        Id = id;
        SetName(name);
    }

    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.InvalidInput(name, nameof(name), x => x.Length <= CategoryConsts.MaxNameLength);

        Name = name;
    }

    public void AddChildCategory(Category childCategory)
    {
        _childCategories.Add(childCategory);
    }

    public void RemoveChildCategory(Category childCategory) 
    { 
        _childCategories.Remove(childCategory);
    }
}
