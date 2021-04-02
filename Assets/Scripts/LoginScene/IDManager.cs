using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager: MonoBehaviour
{
    [SerializeField] private static IDManager instance = null;
    public static IDManager Instance => instance;

    private string email;
    private char parsing;
    private string[] _id;

    private string id;

    public string name;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string Nickname()
    {
        if (AuthManager.Instance.newUser.Email != null)
        {
            email = AuthManager.Instance.newUser.Email;
            parsing = '@';
            _id = email.Split(parsing);
            id = _id[0];
        }
        return id;
    }
}
