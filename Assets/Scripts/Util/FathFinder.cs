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

        // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
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
        //최종 선택된 최단경로
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

            //시작과 끝 노드, 연린 리스트와 닫힌 리스트, 마지막리스트 초기화
            StartNode = NodeArray[startNode.x, startNode.y];
            TargetNode = NodeArray[targetNode.x, targetNode.y];
            
            OpenList = new List<Node>() { StartNode};
            ClosedList = new List<Node>();
            FinalNodeList = new List<Node>();

            while (OpenList.Count > 0)
            {
                //열린리스트중 가장 F가 작고 F가 같다면 H가 가장 작은 걸 현재 노드로 하고 열린리스트에서 닫힌 리스트로 옮기기
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

                //마지막
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
            //상하좌우 범위 벗어나지 X, 이동 가능, 닫
            if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY && NodeArray[checkX, checkY].canMove && !ClosedList.Contains(NodeArray[checkX, checkY]))
            {
                Debug.Log(1);
                //대각선 이동 허용시 벽 사이로 통과 방지
                if (!NodeArray[CurNode.x, checkY].canMove && !NodeArray[checkX, CurNode.y].canMove) return;
                //코너 가로질러 가기 방지, 이동 중 수직수평 장애물이 있으면 안됨
                if (!NodeArray[CurNode.x, checkY].canMove || !NodeArray[checkX, CurNode.y].canMove) return;

                //이웃노드에 넣기, 이동비용 직선 : 10, 대각선 : 14
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
            //NodeArray의 크기 정해주고, isWall, x,y 대입
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
