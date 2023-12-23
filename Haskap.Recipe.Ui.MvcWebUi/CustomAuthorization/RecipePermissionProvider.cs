using Haskap.DddBase.Presentation.CustomAuthorization;

namespace Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;

public class RecipePermissionProvider : PermissionProvider
{
    public override void Define()
    {
        AddPermission(nameof(Permissions.Recipe), Permissions.Recipe.Editor);
        AddPermission(nameof(Permissions.Recipe), Permissions.Recipe.Admin);
    }
}