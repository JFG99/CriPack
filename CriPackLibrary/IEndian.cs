namespace CriPakInterfaces
{
    public interface IEndian
    {
        bool IsLittleEndian { get; set; }
        byte[] Buffer { get; set; }
    }
}
