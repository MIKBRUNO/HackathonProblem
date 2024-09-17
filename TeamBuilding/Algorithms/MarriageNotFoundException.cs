namespace TeamBuilding.Algorithms;

public class MarriageNotFoundException : Exception
{
    public MarriageNotFoundException() : base() {  }
    public MarriageNotFoundException(string? message) : base(message) {  }
    public MarriageNotFoundException(string? message, Exception? inner) : base(message, inner) {  }
}
