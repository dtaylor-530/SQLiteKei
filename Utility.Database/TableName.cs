namespace Utility.Database;

public record TableName(string Name)
{
    public static implicit operator string(TableName fileName)
    {
        return fileName.Name;
    }
}
