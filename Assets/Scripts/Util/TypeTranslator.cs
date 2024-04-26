using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class TypeTranslator : MonoBehaviour
    {
        private static TypeTranslator instance;
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
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
            Debug.Log(text.Split(" ").Length);
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
        public static TypeTranslator Instance
        {
            get
            {
                if(instance == null)
                {
                    return null;
                }
                return instance;
            }
        }
    }
}

