using System.Xml;

namespace SemanticVersioning.Net;

public class ProjectFileManager
{
    private readonly ProjectLookupService _projectLookupService;

    public ProjectFileManager(ProjectLookupService projectLookupService)
    {
        _projectLookupService = projectLookupService;
    }

    public string? GetVersionValue(int projectIndex)
    {
        XmlDocument xmlDocument = ReadCsProjXmlDocumentLoaded(projectIndex);

        XmlNode? versionNode = xmlDocument?.SelectSingleNode("/Project/PropertyGroup/Version");

        return versionNode?.FirstChild?.Value;
    }

    public void TryUpdateVersion(int projectIndex, SemanticVersion version, bool createNodeIfNotExists = false)
    {
        XmlDocument xmlDocument = ReadCsProjXmlDocumentLoaded(projectIndex);

        XmlNode? versionNode = xmlDocument?.SelectSingleNode("/Project/PropertyGroup/Version");

        if (versionNode == null)
        {
            if (createNodeIfNotExists == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Error.WriteLine(
                    $"The project [{_projectLookupService.ProjectPathArray[projectIndex].Name}] has not been versioned before." +
                    "\r\nTry to set the version using the <set> command.");
                Console.ResetColor();

                return;
            }
            
            //TODO: What happens if there is no any Project/PropertyGroup at all?
            XmlNode? selectSingleNode = xmlDocument?.SelectSingleNode("/Project/PropertyGroup");
            
            versionNode = xmlDocument?.CreateNode(XmlNodeType.Element, "Version", string.Empty);
            string firstWhitespace = selectSingleNode?.FirstChild?.InnerText ?? "\r\n";
            
            XmlNode? lastChildElement = selectSingleNode?.ChildNodes
                .Cast<XmlNode>()
                .Last(x => x.NodeType == XmlNodeType.Element);
            
            XmlNode? whitespaceAfterLast = selectSingleNode?.InsertAfter(xmlDocument!.CreateSignificantWhitespace(firstWhitespace), lastChildElement);
            selectSingleNode!.InsertAfter(versionNode!, whitespaceAfterLast);
        }

        string previousVersion = string.IsNullOrEmpty(versionNode!.InnerText) == false ? versionNode!.InnerText : "-no version-";
        versionNode!.InnerText = version;

        XmlWriterSettings settings = new()
        {
            Indent = true,
            OmitXmlDeclaration = true
        };

        try
        {
            FileInfo projectFileInfo = _projectLookupService.ProjectPathArray[projectIndex];

            using XmlWriter writer = XmlWriter.Create(projectFileInfo.FullName, settings);
            xmlDocument?.Save(writer);

            Console.WriteLine(
                $"The project [{projectFileInfo.Name}] version settings successfully done. ({previousVersion} => {version})");
        }
        catch (Exception)
        {
            Console.WriteLine("Version setting process failed.");
        }
    }

    private XmlDocument ReadCsProjXmlDocumentLoaded(int projectIndex)
    {
        XmlDocument xmlDocument = new()
        {
            PreserveWhitespace = true
        };
        
        xmlDocument.Load(_projectLookupService.ProjectPathArray[projectIndex].FullName);

        return xmlDocument;
    }
}