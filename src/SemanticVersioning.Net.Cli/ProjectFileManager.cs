using System.Xml;

namespace SemanticVersioning.Net;

public class ProjectFileManager
{
    private readonly ProjectLookupService _projectLookupService;
    private XmlDocument? _xmlDocument;
    
    public ProjectFileManager(ProjectLookupService projectLookupService)
    {
        _projectLookupService = projectLookupService;
    }

    public string? GetVersionValue(int projectIndex)
    {
        EnsureCsProjXmlDocumentLoaded(projectIndex);
        
        string? version = null;
        
        XmlNode? versionNode = _xmlDocument?.SelectSingleNode("/Project/PropertyGroup/Version");
        
        if (versionNode != null && versionNode.ChildNodes.Count > 0)
            version = versionNode.FirstChild?.Value;

        return version;
    }

    public void TryUpdateVersion(int projectIndex, SemanticVersion version)
    {
        EnsureCsProjXmlDocumentLoaded(projectIndex);
        
        XmlNode? versionNode = _xmlDocument?.SelectSingleNode("/Project/PropertyGroup/Version");

        if (versionNode == null)
            return;

        string previousVersion = versionNode.InnerText;
        versionNode.InnerText = version;

        XmlWriterSettings settings = new();
        settings.Indent = true;
        settings.OmitXmlDeclaration = true;

        try
        {
            FileInfo projectFileInfo = _projectLookupService.ProjectPathArray[projectIndex];
            
            using XmlWriter writer = XmlWriter.Create(projectFileInfo.FullName, settings);
            _xmlDocument?.Save(writer);

            Console.WriteLine($"The project [{projectFileInfo.Name}] version settings successfully done. ({previousVersion} => {version})");
        }
        catch (Exception)
        {
            Console.WriteLine("Version setting process failed.");
        }
    }

    private void EnsureCsProjXmlDocumentLoaded(int projectIndex)
    {
        if (_xmlDocument != null)
            return;
        
        _xmlDocument = new XmlDocument
        {
            PreserveWhitespace = true
        };
        _xmlDocument.Load(_projectLookupService.ProjectPathArray[projectIndex].FullName);
    }
}