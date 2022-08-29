using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HotFix;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Class { get; set; }

    public Student(int id, string name, int age, string @class)
    {
        Id = id;
        Name = name;
        Age = age;
        Class = @class;
    }

    public override string ToString()
    {
        return $"Id={Id},Name={Name},Age={Age},Class={Class}";
    }
}

class StudentKey
{
    public int Age { get; set; }
    public string Class { get; set; }
}
public class PrintHello : MonoBehaviour
{

    public string text;
    Dictionary<int,int> testList=new Dictionary<int,int>()
    {
        [0]=1,
        [1] = 2,
        [2] = 3,
        [3] = 4,
    };
    Vector3 myvalue = Vector3.zero;

    public GameObject go;
    // Start is called before the first frame update
    public void Start()
    {
        Test2();
        Debug.LogFormat("hello, H. {0}", text);

    }

    public static void Test2()
    {
        Debug.Log("--这个热更新脚本挂载在prefab上，打包成ab。通过从ab中实例化prefab成功还原");
        var list = new List<Student>();
        list.Add(new Student(1, "Cat", 9, "University1"));
        list.Add(new Student(2, "Dog", 10, "University1"));
        list.Add(new Student(3, "Pig", 10, "University2"));
        list.Add(new Student(4, "Fish", 12, "University1"));

        var groups = list.Where(t => t.Age > 9).Select(x=>x).GroupBy(m => new { m.Age, m.Class });

        foreach (var group in groups)
        {
            Debug.LogFormat("Age:{0},Class:{1}", group.Key.Age, group.Key.Class);
            foreach (var student in group)
            {
                Debug.LogFormat(student.ToString());
            }
        }
        Debug.Log("--create GameObject");
        var go = new GameObject();
        var img= go.AddComponent<Image>();
        var parent= FindObjectOfType<Canvas>().transform;
        go.transform.SetParent(parent);
        var startlolPos = go.transform.localPosition;
        var controllPos =new Vector3(50, 300);
        var wControllPos= parent.TransformPoint(controllPos);
        var wStartPos= parent.TransformPoint(startlolPos);
        var wEndPos = parent.TransformPoint(new Vector3(200, 200));
        var EcontrollPos = new Vector3(100, 250);
        var wEcontrollPos = parent.TransformPoint(EcontrollPos);
        // tw.SetEase(Ease.InOutCubic).SetRecyclable(true).SetLoops(-1);
        go.transform.DOShakeScale(1);
        Debug.Log("--create red");

        img.CrossFadeColor(Color.red, 1,true,true);
        Debug.Log("--create TestBezier");

        Vector3[] pathvec = TestBezier.Bezier2Path(wStartPos, wControllPos, wEndPos);
        var arr= DOCurve.CubicBezier.GetSegmentPointCloud(wStartPos, wControllPos, wEndPos, wEcontrollPos);
        go.transform.DOPath(arr, 2).SetEase(Ease.OutCubic).SetLoops(-1,LoopType.Yoyo);
        Debug.Log("--create new obj");

        var debug = Activator.CreateInstance<NewDebug>();
        debug.DebugNew();
    }
    // Update is called once per frame
    void Update()
    {
        if (go!=null)
        {
            go.transform.position = myvalue;
        }
    }
}
