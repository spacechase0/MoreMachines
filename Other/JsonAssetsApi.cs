using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreMachines.Other
{
    public interface JsonAssetsApi
    {
        void LoadAssets(string path);
        
        int GetBigCraftableId(string name);
    }
}
