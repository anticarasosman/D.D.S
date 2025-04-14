namespace Shin_Megami_Tensei_Model;

public class InvalidTeamException : Exception
{
    // Constructor con mensaje opcional
    public InvalidTeamException(string message) : base(message) { }
}