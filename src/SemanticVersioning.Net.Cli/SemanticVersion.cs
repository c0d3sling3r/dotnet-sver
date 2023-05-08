using System.Text;
using System.Text.RegularExpressions;

namespace SemanticVersioning.Net;

public class SemanticVersion
{
    public SemanticVersion() { }

    public SemanticVersion(int major, int minor, int patch, string? preRelease, string? build)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
        PreRelease = preRelease;
        Build = build;
    }

    public SemanticVersion(string major, string minor, string patch, string? preRelease, string? build)
    {
        Major = int.Parse(major);
        Minor = int.Parse(minor);
        Patch = int.Parse(patch);
        PreRelease = preRelease;
        Build = build;
    }

    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }

    /// <summary>
    /// Follows by a hyphen(-) after the patch version, indicating the pre-release version.  
    /// </summary>
    public string? PreRelease { get; set; }

    /// <summary>
    /// Follows by a plus(+) after the last version part; Comes after the pre-release if exists.
    /// </summary>
    public string? Build { get; set; }

    public static SemanticVersion? TryParse(string versionString)
    {
        // Pattern inspired. Thanks to Gerard Lisboa.
        // https://regex101.com/library/gG8cK7?orderBy=RELEVANCE&search=semantic+version
        Match regexMatch = Regex.Match(versionString,
            @"^(?'major'0|(?:[1-9]\d*))\.(?'minor'0|(?:[1-9]\d*))\.(?'patch'0|(?:[1-9]\d*))(?:-(?'prerelease'(?:0|(?:[1-9A-Za-z-][0-9A-Za-z-]*))(?:\.(?:0|(?:[1-9A-Za-z-][0-9A-Za-z-]*)))*))?(?:\+(?'build'(?:0|(?:[1-9A-Za-z-][0-9A-Za-z-]*))(?:\.(?:0|(?:[1-9A-Za-z-][0-9A-Za-z-]*)))*))?$");

        if (regexMatch.Success == false)
            return null;

        GroupCollection groups = regexMatch.Groups;

        return new SemanticVersion(groups["major"].Value, groups["minor"].Value, groups["patch"].Value,
            groups["prerelease"].Value, groups["build"].Value);
    }

    public override string ToString()
    {
        StringBuilder versionBuilder = new($"{Major}.{Minor}.{Patch}");

        if (string.IsNullOrEmpty(PreRelease) == false)
            versionBuilder.Append($"-{PreRelease}");

        if (string.IsNullOrEmpty(Build) == false)
            versionBuilder.Append($"+{Build}");
        
        return versionBuilder.ToString();
    }

    public static implicit operator SemanticVersion(string? version) => TryParse(version ?? "0.0.0") ?? new SemanticVersion();
    public static implicit operator string(SemanticVersion version) => version.ToString();
}