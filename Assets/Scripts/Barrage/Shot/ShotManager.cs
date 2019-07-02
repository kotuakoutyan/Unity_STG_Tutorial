
using Barrage.Bullet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Barrage.Shot
{
    public class ShotManager
    {
        private ShotManager() { Initialize(); }
        private static ShotManager instance;
        public static ShotManager Instance
        {
            get
            {
                if (instance == null) instance = new ShotManager();
                return instance;
            }
        }

        public NormalShot NormalShot = new NormalShot();
        public AllRangeShot AllRangeShot = new AllRangeShot();
        public GatherShot GatherShot = new GatherShot();
        
        
        //AllRangeShotのテンプレートを取得するメソッド
        public Vector3[] GetShotShape(AllRangeShotShape shape, int bulletNum = default, float randomNum = default)
        {
            switch (shape)
            {
                case AllRangeShotShape.Plane:
                    return OrthogonalUnitVectors(bulletNum, randomNum);
                case AllRangeShotShape.Tetrahedron:
                    return Tetrahedron;
                case AllRangeShotShape.Hexahedron:
                    return Hexahedron;
                case AllRangeShotShape.Octahedron:
                    return Octahedron;
                case AllRangeShotShape.Dodecahedron:
                    return Dodecahedron;
                case AllRangeShotShape.Icosahedron:
                    return Icosahedron;
                case AllRangeShotShape.HighDensity10:
                    return HighDensity10;
                case AllRangeShotShape.HighDensity20:
                    return HighDensity20;
                case AllRangeShotShape.HighDensity30:
                    return HighDensity30;
                case AllRangeShotShape.HighDensity40:
                    return HighDensity40;
                case AllRangeShotShape.HighDensity50:
                    return HighDensity50;
                default:
                    return default;
            }
        }


        //AllRangeShotのテンプレートは他のところでも使うのでここで管理する
        private Vector3[] Tetrahedron = new Vector3[]
        {
            new Vector3(1,1,1).normalized, new Vector3(-1,1,-1).normalized,
            new Vector3(-1,-1,1).normalized, new Vector3(1,-1,-1).normalized
        };
        private Vector3[] Hexahedron = new Vector3[]
        {
            new Vector3(1,1,1).normalized, new Vector3(1,1,-1).normalized, new Vector3(1,-1,1).normalized, new Vector3(1,-1,-1).normalized,
            new Vector3(-1,1,1).normalized, new Vector3(-1,1,-1).normalized, new Vector3(-1,-1,1).normalized, new Vector3(-1,-1,-1).normalized
        };
        private Vector3[] Octahedron = new Vector3[]
        {
            new Vector3(0,0,1), new Vector3(0,0,-1), new Vector3(0,1,0),
            new Vector3(0,-1,0), new Vector3(1,0,0), new Vector3(-1,0,0)
        };
        
        private Vector3[] Dodecahedron = null;
        private Vector3[] Icosahedron = null;
        private Vector3[] HighDensity10 = null;
        private Vector3[] HighDensity20 = null;
        private Vector3[] HighDensity30 = null;
        private Vector3[] HighDensity40 = null;
        private Vector3[] HighDensity50 = null;


        public Vector3[] RandomVectors(int bulletNum)
        {
            var directions = new Vector3[bulletNum];
            for (int i = 0; i < bulletNum; i++) directions[i] = Random.onUnitSphere;
            return directions;
        }

        private Vector3[] OrthogonalUnitVectors(int bulletNum, float randomNum)
        {
            var directions = new Vector3[bulletNum];
            var randomAngle = randomNum * 360;
            for (int i = 0; i < bulletNum; i++) directions[i] = Quaternion.Euler(0, 0, 360f / bulletNum * i + randomAngle) * Vector3.right;
            return directions;
        }


        //コンストラクタ
        public void Initialize()
        {
            if (Icosahedron == null)
            {
                Icosahedron = new Vector3[]
                {
                    Secret(Octahedron[0], Octahedron[4]), Secret(Octahedron[0], Octahedron[5]), Secret(Octahedron[1], Octahedron[4]),
                    Secret(Octahedron[1], Octahedron[5]), Secret(Octahedron[2], Octahedron[0]), Secret(Octahedron[2], Octahedron[1]),
                    Secret(Octahedron[3], Octahedron[0]), Secret(Octahedron[3], Octahedron[1]), Secret(Octahedron[4], Octahedron[2]),
                    Secret(Octahedron[4], Octahedron[3]), Secret(Octahedron[5], Octahedron[2]), Secret(Octahedron[5], Octahedron[3])
                };
            }

            if (Dodecahedron == null)
            {
                Dodecahedron = new Vector3[20];
                float length = 0.0f;
                var count = 0;
                for (int i = 0; i < Icosahedron.Length; i++)
                {
                    for (int j = i + 1; j < Icosahedron.Length; j++)
                    {
                        for (int k = j + 1; k < Icosahedron.Length; k++)
                        {
                            {
                                if (Vector3.Distance(Icosahedron[i], Icosahedron[j]) == Vector3.Distance(Icosahedron[j], Icosahedron[k]) &&
                                    Vector3.Distance(Icosahedron[j], Icosahedron[k]) == Vector3.Distance(Icosahedron[k], Icosahedron[i]))
                                {
                                    if (length == 0) length = Vector3.Distance(Icosahedron[i], Icosahedron[j]);
                                    if (length == Vector3.Distance(Icosahedron[i], Icosahedron[j]))
                                    {
                                        Dodecahedron[count] = ((Icosahedron[i] + Icosahedron[j] + Icosahedron[k]) / 3).normalized;
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                }

                if (HighDensity10 == null) HighDensity10 = GetHighDensity(10);
                if (HighDensity20 == null) HighDensity20 = GetHighDensity(20);
                if (HighDensity30 == null) HighDensity30 = GetHighDensity(30);
                if (HighDensity40 == null) HighDensity40 = GetHighDensity(40);
                if (HighDensity50 == null) HighDensity50 = GetHighDensity(50);
            }
        }

        private Vector3[] GetHighDensity(int baseNum)
        {
            var list = new List<Vector3>();

            //弾幕間の緯線上における距離
            float dis = 2 * Mathf.PI / baseNum;
            //弾幕間の経線における距離
            float width = dis * Mathf.Sqrt(3) / 2;
            bool isUseOffset = false;
            for (float phi = -Mathf.PI / 2; phi < Mathf.PI / 2; phi += width)
            {
                isUseOffset = !isUseOffset;
                //その緯線上における弾幕の数
                int num = (int)(Mathf.Cos(phi) * baseNum);
                float angle = num == 0 ? 2 * Mathf.PI : 2 * Mathf.PI / num;
                for (float theta = isUseOffset ? angle / 2 : 0; theta < 2 * Mathf.PI; theta += angle)
                {
                    list.Add(new Vector3(Mathf.Cos(phi) * Mathf.Sin(theta), Mathf.Sin(phi), Mathf.Cos(phi) * Mathf.Cos(theta)));
                }
            }
            return list.ToArray();
        }

        //内分点を返す
        private Vector3 Secret(Vector3 vec1, Vector3 vec2) => ((vec1 + vec2 * (1 + Mathf.Sqrt(5)) / 2) / (3 + Mathf.Sqrt(5))).normalized;
    }
}