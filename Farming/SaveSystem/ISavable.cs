using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.SaveSystem
{
    public interface ISavable 
    {
        public int SaveID { get;}
        string GetData();
        void RestoreData(string data);
    }
}
