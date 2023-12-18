using Haskap.DddBase.Presentation.CustomAuthorization;

namespace Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;

public class RecipePermissionProvider : PermissionProvider
{
    public override void Define()
    {
        AddPermission(nameof(Permissions.Dashboard), Permissions.Dashboard.Read);
        AddPermission(nameof(Permissions.Dashboard), Permissions.Dashboard.Create);
        AddPermission(nameof(Permissions.Dashboard), Permissions.Dashboard.Update);

        AddPermission(nameof(Permissions.Diagnositcs), Permissions.Diagnositcs.Read);
        AddPermission(nameof(Permissions.Diagnositcs), Permissions.Diagnositcs.Create);

        AddPermission(nameof(Permissions.Tenants), Permissions.Tenants.Host);
        AddPermission(nameof(Permissions.Tenants), Permissions.Tenants.Admin);
    }
}