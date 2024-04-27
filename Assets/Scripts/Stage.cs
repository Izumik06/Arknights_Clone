using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class Stage : MonoBehaviour
    {
        public Vector3 cameraPos;
        public Vector3 cameraRot;
        public int sizeX, sizeY;
        public string stageName;
        public int initialCost;
        public float costSpeed;
        public int targetHP;
        public int enemyCount;
        public int deploymentSlots;

        [Header("맵 클리어시 필요한 이성")]
        public int useSane;
        [Header("실패시 차감될 이성")]
        public int deductionSane;
    }
}
