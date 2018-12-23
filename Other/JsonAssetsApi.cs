namespace MoreMachines.Other
{
    public interface JsonAssetsApi
    {
        void LoadAssets(string path);
        
        int GetBigCraftableId(string name);
    }
}
