namespace Dapper.Logging
{
    internal class Empty
    {
        public static readonly Empty Object = new Empty();
        public override string ToString() => "Empty";
    }
}