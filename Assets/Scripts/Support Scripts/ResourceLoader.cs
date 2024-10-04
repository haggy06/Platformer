using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public static class ResourceLoader<T> where T : UnityEngine.Object
{
    private static Dictionary<Tuple<FolderName, string>, T> resourceCache = new Dictionary<Tuple<FolderName, string>, T>(); // �ҷ��� ���ҽ�  
    public static T ResourceLoad(FolderName folder, string resourceName)
    {
        T value;
        Tuple<FolderName, string> key = new Tuple<FolderName, string>(folder, resourceName);

        if (!resourceCache.TryGetValue(key, out value)) // ĳ�̵� ���ҽ��� ���� ��� �ҷ���
        { // ĳ�̵� ���ҽ��� ���� ���
            value = Resources.Load<T>(Path.Combine(typeof(T).Name, Path.Combine(folder.ToString(), resourceName))); // ���ҽ� �ε�

            resourceCache.Add(key, value); // �ҷ��� ���ҽ� ĳ��
        }

        if (value == null)
            Debug.LogError(resourceName + " �ε� ����");

        return value;
    }
}

public enum FolderName
{
    Player,

    BGM,
    Death,

    Singleton,

    Ect
}