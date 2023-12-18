namespace Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;

public static class Permissions
{
    public static class Dashboard
    {
        public const string Read = "Permissions.Dashboard.Read";
        public const string Create = "Permissions.Dashboard.Create";
        public const string Update = "Permissions.Dashboard.Update";
    }

    public static class Diagnositcs
    {
        public const string Read = "Permissions.Diagnostics.Read";
        public const string Create = "Permissions.Diagnostics.Create";
    }

    public static class Tenants
    {
        public const string Host = "Permissions.Tenants.Host";
        public const string Admin = "Permissions.Tenants.Admin";
    }
}
