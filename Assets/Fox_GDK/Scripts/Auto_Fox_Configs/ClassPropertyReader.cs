using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

public class ClassPropertyReader : MonoBehaviour
{
    public GameplayData outPut;
    // Start is called before the first frame update
    void Start()
    {
        //ReadProperty(nT);
        //SetProperty(outPut);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadData(outPut);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SaveData(outPut);
        }
    }

    public static void SaveData(object _object, string objectName = "")
    {
        if (_object == null)
        {
            Debug.LogError("Object is null");
            return;
        }
        objectName += "___" + _object.GetType();
        //Debug.LogError("============> Object Saving: " + _object.GetType());
        TypeInfo tpi = _object.GetType().GetTypeInfo();
        FieldInfo[] finfos = tpi.GetFields();
        
        //Debug.LogError("Child Count: " + finfos.Length);
        for (int i = 0; i < finfos.Length; i++)
        {
            FieldInfo fi = finfos[i];


            string keyName = objectName + "_" + fi.Name;
            //Debug.LogError("Name: " + keyName + " = " + fi.GetValue(_object) + "\tData Type: " + Type.GetTypeCode(fi.FieldType).ToString() + "\tType: " + fi.FieldType.ToString());
            switch (Type.GetTypeCode(fi.FieldType))
            {
                case TypeCode.Boolean:
                    PlayerPrefsX.SetBool(keyName, (bool)fi.GetValue(_object));
                    break;
                case TypeCode.Byte:
                    PlayerPrefs.SetInt(keyName, (byte)fi.GetValue(_object));
                    break;
                case TypeCode.Char:
                    PlayerPrefs.SetString(keyName, (string)fi.GetValue(_object));
                    break;
                case TypeCode.String:
                    PlayerPrefs.SetString(keyName, (string)fi.GetValue(_object));
                    break;
                case TypeCode.DateTime:
                    PlayerPrefs.SetString(keyName, (string)fi.GetValue(_object));
                    break;
                case TypeCode.Double:
                    PlayerPrefsX.SetLong(keyName, (long)fi.GetValue(_object));
                    break;
                case TypeCode.Int64:
                    PlayerPrefsX.SetLong(keyName, (Int64)fi.GetValue(_object));
                    break;
                case TypeCode.Int32:
                    PlayerPrefs.SetInt(keyName, (Int32)fi.GetValue(_object));
                    break;
                case TypeCode.Decimal:
                    PlayerPrefs.SetInt(keyName, (Int32)fi.GetValue(_object));
                    break;
                case TypeCode.Int16:
                    PlayerPrefs.SetInt(keyName, (Int16)fi.GetValue(_object));
                    break;
                case TypeCode.SByte:
                    PlayerPrefs.SetInt(keyName, (Int32)fi.GetValue(_object));
                    break;
                case TypeCode.Single:
                    PlayerPrefs.SetFloat(keyName, (float)fi.GetValue(_object));
                    break;
                case TypeCode.UInt16:
                    PlayerPrefs.SetInt(keyName, (UInt16)fi.GetValue(_object));
                    break;
                case TypeCode.UInt32:
                    PlayerPrefsX.SetLong(keyName, (long)fi.GetValue(_object));
                    break;
                case TypeCode.UInt64:
                    PlayerPrefsX.SetuLong(keyName, (ulong)fi.GetValue(_object));
                    break;
                case TypeCode.Object:
                    object obj = fi.GetValue(_object);
                    if (obj == null)
                    {
                        ObjectCreateMethod inv = new ObjectCreateMethod(fi.FieldType);
                        obj = inv.CreateInstance();
                    }
                    if (obj is Vector3)
                    {
                        KV_Vector apv = new KV_Vector();
                        apv.x = ((Vector3)fi.GetValue(_object)).x;
                        apv.y = ((Vector3)fi.GetValue(_object)).y;
                        apv.z = ((Vector3)fi.GetValue(_object)).z;
                        obj = apv;
                    }
                    else if (obj is Quaternion)
                    {
                        KV_Quaternion apq = new KV_Quaternion();
                        apq.x = ((Quaternion)fi.GetValue(_object)).x;
                        apq.y = ((Quaternion)fi.GetValue(_object)).y;
                        apq.z = ((Quaternion)fi.GetValue(_object)).z;
                        apq.w = ((Quaternion)fi.GetValue(_object)).w;
                        obj = apq;
                    }
                    SaveData(obj, keyName);
                    break;
            }
        }

    }

    public static void LoadData(object _object, string objectName = "")
    {
        if (_object == null)
        {
            Debug.LogError("Object is null");
            return;
        }
        objectName += "___" + _object.GetType();
        //Debug.LogError("============> Object Loading: " + _object.GetType());
        TypeInfo tpi = _object.GetType().GetTypeInfo();

        FieldInfo[] finfos = tpi.GetFields();
        
        for (int i = 0; i < finfos.Length; i++)
        {
            FieldInfo fi = finfos[i];


            string keyName = objectName + "_" + fi.Name;
            //Debug.LogError("Name: " + keyName + " = " + fi.GetValue(_object) + "\tData Type: " + Type.GetTypeCode(fi.FieldType).ToString() + "\tType: " + fi.FieldType.ToString());
            switch (Type.GetTypeCode(fi.FieldType))
            {
                case TypeCode.Boolean:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefsX.GetBool(keyName, false));
                    break;
                case TypeCode.Byte:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetInt(keyName, (int)fi.GetValue(_object)));
                    break;
                case TypeCode.Char:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetString(keyName, (string)fi.GetValue(_object)));                    
                    break;
                case TypeCode.String:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetString(keyName, (string)fi.GetValue(_object)));
                    break;
                case TypeCode.DateTime:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetString(keyName, (string)fi.GetValue(_object)));                    
                    break;
                case TypeCode.Double:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefsX.GetLong(keyName, (long)fi.GetValue(_object)));
                    break;
                case TypeCode.Int64:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefsX.GetLong(keyName, (Int64)fi.GetValue(_object)));                    
                    break;
                case TypeCode.Int32:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetInt(keyName, (Int32)fi.GetValue(_object)));                    
                    break;
                case TypeCode.Decimal:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetInt(keyName, (Int32)fi.GetValue(_object)));                    
                    break;
                case TypeCode.Int16:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetInt(keyName, (Int16)fi.GetValue(_object)));                    
                    break;
                case TypeCode.SByte:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetInt(keyName, (Int32)fi.GetValue(_object)));                    
                    break;
                case TypeCode.Single:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetFloat(keyName, (float)fi.GetValue(_object)));                    
                    break;
                case TypeCode.UInt16:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefs.GetInt(keyName, (UInt16)fi.GetValue(_object)));                    
                    break;
                case TypeCode.UInt32:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefsX.GetLong(keyName, (long)fi.GetValue(_object)));                    
                    break;
                case TypeCode.UInt64:
                    _object.GetType().GetField(fi.Name).SetValue(_object, PlayerPrefsX.GetuLong(keyName, (ulong)fi.GetValue(_object)));                    
                    break;
                case TypeCode.Object:
                    object obj = fi.GetValue(_object);
                    
                    if (obj == null)
                    {
                        ObjectCreateMethod ocm = new ObjectCreateMethod(fi.FieldType);
                        obj = ocm.CreateInstance();
                    }
                    if (obj is Vector3)
                    {
                        KV_Vector apv = new KV_Vector();
                        LoadData(apv, keyName);
                        Vector3 val;
                        val.x = apv.x;
                        val.y = apv.y;
                        val.z = apv.z;
                        _object.GetType().GetField(fi.Name).SetValue(_object, val);                        
                    }
                    else if (obj is Quaternion)
                    {
                        KV_Quaternion apq = new KV_Quaternion();
                        LoadData(apq, keyName);
                        Quaternion val;
                        val.x = apq.x;
                        val.y = apq.y;
                        val.z = apq.z;
                        val.w = apq.w;
                        _object.GetType().GetField(fi.Name).SetValue(_object, val);
                    }
                    else
                    {
                        LoadData(obj, keyName);
                    }
                    break;
            }
            //Debug.LogError("Name: " + keyName + " = " + fi.GetValue(_object) + "\tData Type: " + Type.GetTypeCode(fi.FieldType).ToString() + "\tType: " + fi.FieldType.ToString());
        }
    }
}

public class KV_Vector
{
    public float x, y, z;
}

public class KV_Quaternion
{
    public float x, y, z, w;     
}

[Serializable]
public class NT
{
    public Vector3 position;
    public Quaternion rotation;
    public float testFlot = 8;
    public GameplayData gameplayData;
    public int testInt = 7;
}
public class ObjectCreateMethod
{
    delegate object MethodInvoker();
    MethodInvoker methodHandler = null;

    public ObjectCreateMethod(Type type)
    {
        CreateMethod(type.GetConstructor(Type.EmptyTypes));
    }

    public ObjectCreateMethod(ConstructorInfo target)
    {
        CreateMethod(target);
    }

    void CreateMethod(ConstructorInfo target)
    {
        DynamicMethod dynamic = new DynamicMethod(string.Empty,
                    typeof(object),
                    new Type[0],
                    target.DeclaringType);
        ILGenerator il = dynamic.GetILGenerator();
        il.DeclareLocal(target.DeclaringType);
        il.Emit(OpCodes.Newobj, target);
        il.Emit(OpCodes.Stloc_0);
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ret);

        methodHandler = (MethodInvoker)dynamic.CreateDelegate(typeof(MethodInvoker));
    }

    public object CreateInstance()
    {
        return methodHandler();
    }
}

