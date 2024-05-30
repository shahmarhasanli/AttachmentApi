namespace WarrAttachmentManagementService.Application.Common;

public static class StringManipulation
{
    public static string RemoveWhitespace(this string input)
    {
        return new string(input
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());
    }
}
