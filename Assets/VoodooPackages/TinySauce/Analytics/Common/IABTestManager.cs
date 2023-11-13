namespace Voodoo.Tiny.Sauce.Internal.Analytics
{
    public interface IABTestManager
    {
        void Init();
        string[] GetAbTestValues();
    }
}
