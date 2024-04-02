using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Izumik
{
    [Serializable]
    public class Node
    {
        public bool canMove;
        public Node ParentNode;

        // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
        public int x, y, G, H;
        public Node(bool _canMove, int _x, int _y)
        {
            canMove = _canMove;
            x = _x;
            y = _y;
        }
        public Node(int _x, int _y)
        {
            canMove = true;
            x = _x;
            y = _y;
        }
        public int F { get { return G + H; } }
    }
    public class FathFinder : MonoBehaviour
    {
        public GameObject path;
        public Node test;
        public Vector2Int bottomLeft, topRight, startPos, targetPos;
        //���� ���õ� �ִܰ��
        public List<Node> FinalNodeList;
        public bool allowDiagonal, dontCrossCorner;
        
        int sizeX, sizeY;
        Node[,] NodeArray;
        public Node StartNode, TargetNode, CurNode;
        public List<Node> OpenList, ClosedList;

        public GameObject curMap;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PathFinding(new Node(8, 2), new Node(0, 3));
                ShowFath();
            }
                
        }
        public void PathFinding(Node startNode, Node targetNode)
        {
            GetMap();

            //���۰� �� ���, ���� ����Ʈ�� ���� ����Ʈ, ����������Ʈ �ʱ�ȭ
            StartNode = NodeArray[startNode.x, startNode.y];
            TargetNode = NodeArray[targetNode.x, targetNode.y];
            
            OpenList = new List<Node>() { StartNode};
            ClosedList = new List<Node>();
            FinalNodeList = new List<Node>();

            while (OpenList.Count > 0)
            {
                //��������Ʈ�� ���� F�� �۰� F�� ���ٸ� H�� ���� ���� �� ���� ���� �ϰ� ��������Ʈ���� ���� ����Ʈ�� �ű��
                CurNode = OpenList[0];
                for (int i = 1; i < OpenList.Count; i++)
                {
                    if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                    {
                        CurNode = OpenList[i];
                    }
                }
                OpenList.Remove(CurNode);
                ClosedList.Add(CurNode);

                //������
                if (CurNode == TargetNode)
                {
                    Node TargetCurNode = TargetNode;
                    while (TargetCurNode != StartNode)
                    {
                        FinalNodeList.Add(TargetCurNode);
                        TargetCurNode = TargetCurNode.ParentNode;
                    }
                    FinalNodeList.Add(StartNode);
                    FinalNodeList.Reverse();
                    return;
                }
                OpenListAdd(CurNode.x + 1, CurNode.y);
                OpenListAdd(CurNode.x - 1, CurNode.y);
                OpenListAdd(CurNode.x, CurNode.y + 1);
                OpenListAdd(CurNode.x, CurNode.y - 1);


                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);

            }
        }

        void OpenListAdd(int checkX, int checkY)
        {
            //�����¿� ���� ����� X, �̵� ����, ��
            if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY && NodeArray[checkX, checkY].canMove && !ClosedList.Contains(NodeArray[checkX, checkY]))
            {
                Debug.Log(1);
                //�밢�� �̵� ���� �� ���̷� ��� ����
                if (!NodeArray[CurNode.x, checkY].canMove && !NodeArray[checkX, CurNode.y].canMove) return;
                //�ڳ� �������� ���� ����, �̵� �� �������� ��ֹ��� ������ �ȵ�
                if (!NodeArray[CurNode.x, checkY].canMove || !NodeArray[checkX, CurNode.y].canMove) return;

                //�̿���忡 �ֱ�, �̵���� ���� : 10, �밢�� : 14
                Node neighborNode = NodeArray[checkX, checkY];
                int moveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

                if(moveCost < neighborNode.G || !OpenList.Contains(neighborNode))
                {
                    Debug.Log(1);
                    neighborNode.G = moveCost;
                    neighborNode.H = (Mathf.Abs(neighborNode.x - TargetNode.x) + Mathf.Abs(neighborNode.y - TargetNode.y));
                    neighborNode.ParentNode = CurNode;

                    OpenList.Add(neighborNode);
                }
            }
        }
        public void GetMap()
        {
            curMap = StageManager.Instance.curStageObj;
            //NodeArray�� ũ�� �����ְ�, isWall, x,y ����
            sizeX = curMap.GetComponent<Stage>().sizeX;
            sizeY = curMap.GetComponent<Stage>().sizeY;
            NodeArray = new Node[sizeX, sizeY];

            bottomLeft = new Vector2Int(0, 0);
            topRight = new Vector2Int(sizeX - 1, sizeY - 1);

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)//
                {

                    bool canMove = true;
                    if(curMap.transform.GetChild(i).GetChild(j).GetComponent<NodeObj>().isWall == true || curMap.transform.GetChild(i).GetChild(j).GetComponent<NodeObj>().isNone == true)
                    {
                        canMove = false;
                    }
                    NodeArray[j, i] = new Node(canMove, curMap.transform.GetChild(i).GetChild(j).GetComponent<NodeObj>().x, curMap.transform.GetChild(i).GetChild(j).GetComponent<NodeObj>().y);
                }
            }
            test = NodeArray[2, 2];
        }
        void ShowFath()
        {
            Debug.Log(FinalNodeList.Count);
            for(int i = 0; i < FinalNodeList.Count; i++)
            {
                GameObject check = Instantiate(path);
                check.transform.position = new Vector3(FinalNodeList[i].x, 0, FinalNodeList[i].y);
            }

        }
        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}
