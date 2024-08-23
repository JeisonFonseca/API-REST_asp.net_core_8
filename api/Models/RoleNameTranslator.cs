using Npgsql;

namespace api.Models;

public class RoleNameTranslator : INpgsqlNameTranslator
{
    public string TranslateMemberName(string clrName) => clrName;

    public string TranslateTypeName(string clrName) => "userrole";
}