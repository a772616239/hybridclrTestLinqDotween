using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Assets.HotFix
{


    public class TestBezier : MonoBehaviour
    {
        private float _times = 3f;
        private static float _pointCount = 5f;
        public GameObject obj;
        public GameObject startObj;
        public GameObject controlObj;
        public GameObject endObj;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                DoAnim();
            }
        }
        private void DoAnim()
        {
            
        }
        //获取二阶贝塞尔曲线路径数组
        public static Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos)
        {
            Vector3[] path = new Vector3[(int)_pointCount];
            for (int i = 1; i <= _pointCount; i++)
            {
                float t = i / _pointCount;
                path[i - 1] = Bezier2(startPos, controlPos, endPos, t);
            }
            return path;
        }
        // 2阶贝塞尔曲线
        public static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t)
        {
            return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
        }

        // 3阶贝塞尔曲线
        public static Vector3 Bezier3(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos, float t)
        {
            float t2 = 1 - t;
            return t2 * t2 * t2 * startPos
                   + 3 * t * t2 * t2 * controlPos1
                   + 3 * t * t * t2 * controlPos2
                   + t * t * t * endPos;
        }
    }


}
