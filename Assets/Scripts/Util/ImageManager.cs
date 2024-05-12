using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Izumik
{
    public class ImageManager : MonoBehaviour
    {
        [SerializeField] SpriteAtlas classIcon;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public Sprite getClassIcon(ImageType type, int index)
        {
            Sprite returnSprite = null;
            switch (type)
            {
                case ImageType.ClassIcon:
                    returnSprite = classIcon.GetSprite($"classMark_{index}");
                    break;
            }
            return returnSprite;
        }
    }
    public enum ImageType
    {
        ClassIcon
    }
}
