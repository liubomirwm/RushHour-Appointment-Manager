// <auto-generated />
namespace DataAccess.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class RestrictActivityNameto250chars : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RestrictActivityNameto250chars));
        
        string IMigrationMetadata.Id
        {
            get { return "201708031115288_Restrict Activity.Name to 250 chars"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
