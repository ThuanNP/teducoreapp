namespace TeduCoreApp.infrastructure.Interfaces
{
    public interface IHasSeoMetadata
    {
        string SeoPageTitle { get; set; }
        string SeoAlias { get; set; }
        string Seokeywords { get; set; }
        string SeoDecription { get; set; }
    }
}