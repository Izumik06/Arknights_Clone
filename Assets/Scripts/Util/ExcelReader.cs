using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Izumik
{
    public class ExcelReader : MonoBehaviour
    {
        string[,] table;
        int lineSize, rowSize;
        public string[,] Decoding(TextAsset txt)
        {
            string currentText = txt.text.Substring(0, txt.text.Length - 1);
            string[] line = currentText.Split('\n');
            lineSize = line.Length;
            rowSize = line[0].Split("\t").Length;
            table = new string[lineSize, rowSize];

            for (int i = 0; i < lineSize; i++)
            {
                string[] row = line[i].Split("\t");
                for (int j = 0; j < rowSize; j++) table[i, j] = row[j];
            }

            return table;
        }
        //public List<SpawnData> TextToSpawnData(string[,] table)
        //{
        //    List<SpawnData> dispatchLog = new List<SpawnData>();
        //    for (int i = 1; i < table.Length; i++)
        //    {
        //        dispatchLog.Add(new SpawnData(table[i, 0], int.Parse(table[i, 1], float.Parse(table[i,2]), float.Parse(table[i,3]), ))
        //    }
        //}
        //public List<Coo> StringToCoo(string text)
        //{
        //    List<Coo> cooList = new List<Coo>();
        
        //    for(int i = 0; i < text.Split(" ").Length; i++)
        //    {
        //        cooList.Add(new Coo(text.Split(" ")[i]))
        //    }
        //}
    }//asdf
    public class SpawnData
    {
        public string type;
        public int count;
        public float spawnTime;
        public float spawnDelay;
        public List<Coo> root;
        public Coo defensePoint;
        public bool useAstar;

        public SpawnData(string type, int count, float spawnTime, float spawnDelay, List<Coo> root, Coo defensePoint, bool useAstar)
        {
            this.type = type;
            this.count = count;
            this.spawnTime = spawnTime;
            this.spawnDelay = spawnDelay;
            this.root = root;
            this.defensePoint = defensePoint;
            this.useAstar = useAstar;
        }
    }
    public class Coo
    {
        public int x;
        public int y;
        

        public Coo(string text)
        {
            x = int.Parse(text.Split(".")[1]);
            //y = in
        }
    }
}

