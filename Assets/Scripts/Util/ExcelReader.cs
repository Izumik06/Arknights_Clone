using System;
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
                for (int j = 0; j < rowSize; j++) { table[i, j] = row[j]; }

            }
            Debug.Log(rowSize);
            Debug.Log(lineSize);
            return table;
        }
        public List<SpawnData> TextToSpawnData(string[,] table)
        {
            List<SpawnData> dispatchLog = new List<SpawnData>();
            for (int i = 1; i < table.GetLength(0); i++)
            {
                dispatchLog.Add(new SpawnData(table[i, 0], int.Parse(table[i, 1]), float.Parse(table[i, 2]), float.Parse(table[i, 3]), new Node(int.Parse(table[i, 4].Split(".")[0]), int.Parse(table[i, 4].Split(".")[1])), StringToRoot(table[i, 5]), StringToWaitnode(table[i, 6]), StringToPosError(table[i,7]), new Node(int.Parse(table[i, 8].Split(".")[0]), int.Parse(table[i, 8].Split(".")[1])), bool.Parse(table[i, 9])));
            }
            return dispatchLog;
        }
        public List<float> StringToPosError(string text)
        {
            List<float> posErrorList = new List<float>();

            for (int i = 0; i < text.Split(" ").Length; i++)
            {
                if (text.Split(" ")[i] == "") { break; }
                posErrorList.Add(float.Parse(text.Split(" ")[i]));
            }
            return posErrorList;
        }
        public List<Node> StringToRoot(string text)
        {
            List<Node> cooList = new List<Node>();

            for (int i = 0; i < text.Split(" ").Length; i++)
            {
                string nodeCoo = text.Split(" ")[i];
                cooList.Add(new Node(int.Parse(nodeCoo.Split(".")[0]), int.Parse(nodeCoo.Split(".")[1])));
            }

            return cooList;
        }
        public List<WaitNode> StringToWaitnode(string text)
        {
            List<WaitNode> cooList = new List<WaitNode>();
            for (int i = 0; i < text.Split(" ").Length; i++)
            {
                string nodeCoo = text.Split(" ")[i];
                if (nodeCoo == "") break;
                cooList.Add(new WaitNode(int.Parse(nodeCoo.Split(".")[0]), int.Parse(nodeCoo.Split(".")[1]), int.Parse(nodeCoo.Split(".")[2])));
            }

            return cooList;
        }
    }
    [Serializable]
    public class WaitNode
    {
        public int x, y;
        public float time;
        
        public WaitNode(int _x, int _y, float _time)
        {
            this.x = _x;
            this.y = _y;
            this.time = _time;
        }
    }
    [Serializable]
    public class SpawnData
    {
        public string type;
        public int count;
        public float spawnTime;
        public float spawnDelay;
        public List<Node> root;
        public Node defensePoint;
        public bool useAstar;
        public List<WaitNode> waitNodes;
        public Node spawnPoint;
        public List<float> posError;

        public SpawnData(string type, int count, float spawnTime, float spawnDelay, Node spawnPoint, List<Node> root, List<WaitNode> _waitNodes, List<float> _PosError, Node defensePoint, bool useAstar)
        {
            this.type = type;
            this.count = count;
            this.spawnTime = spawnTime;
            this.spawnDelay = spawnDelay;
            this.root = root;
            this.defensePoint = defensePoint;
            this.useAstar = useAstar;
            this.waitNodes = _waitNodes;
            this.posError = _PosError;
            this.spawnPoint = spawnPoint;
        }
    }
}

