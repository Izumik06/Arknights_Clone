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

        [Header("�� Ŭ����� �ʿ��� �̼�")]
        public int useSane;
        [Header("���н� ������ �̼�")]
        public int deductionSane;
    }
}
