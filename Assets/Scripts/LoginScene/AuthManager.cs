using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager
{
    private static AuthManager instance = null;

    private FirebaseAuth auth;
    public FirebaseUser newUser;
    public FirebaseUser user;

    private string displayName;
    private string emailAddress;
    private Uri photoUrl;

    public string info;

    public static AuthManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new AuthManager();
            }

            return instance;
        }
    }

    public string UserId => user?.UserId ?? string.Empty;
    public string DisplayName => displayName;
    public string EmailAddress => emailAddress;
    public Uri PhotoUrl => photoUrl;

    public Action<bool> OnChangedLoginState;

    public void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnAuthStateChanged;

        OnAuthStateChanged(this, null);
    }

    public void CreateUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                info = "아이디 생성이 취소되었습니다";
                return;
            }

            if (task.IsFaulted)
            {
                info = "아이디 생성을 실패했습니다";

                int errorCode = GetFirebaseErrorCode(task.Exception);
                switch (errorCode)
                {
                    case (int)AuthError.EmailAlreadyInUse:
                        info = "이메일이 이미 있습니다";
                        break;
                    case (int)AuthError.InvalidEmail:
                        info = "올바른 이메일이 아닙니다";
                        break;
                    case (int)AuthError.WeakPassword:
                        info = "보안에 취약한 비밀번호입니다";
                        break;
                }

                return;
            }

            newUser = task.Result;
            info = "아이디를 성공적으로 만들었습니다";
        });
    }

    public void LogIn(string email, string password)
    {

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                info = "로그인이 취소되었습니다";
                return;
            }

            if (task.IsFaulted)
            {
                info = "로그인에 실패했습니다";

                int errorCode = GetFirebaseErrorCode(task.Exception);
                switch (errorCode)
                {
                    case (int)AuthError.WrongPassword:
                        info = "올바른 비밀번호가 아닙니다";
                        break;
                    case (int)AuthError.UnverifiedEmail:
                        info = "이메일이 없습니다";
                        break;
                    case (int)AuthError.InvalidEmail:
                        info = "올바른 이메일이 아닙니다";
                        break;
                }

                return;
            }

            newUser = task.Result;
            SceneManager.LoadScene("Lobby");
        });
    }

    private int GetFirebaseErrorCode(AggregateException exception)
    {
        FirebaseException firebaseException = null;
        foreach(Exception e in exception.Flatten().InnerExceptions)
        {
            firebaseException =  e as FirebaseException;

            if(firebaseException != null)
            {
                break;
            }
        }

        return firebaseException?.ErrorCode ?? 0;
    }

    void OnAuthStateChanged(object sender, EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = (user != auth.CurrentUser && auth.CurrentUser != null);
            if(!signedIn && user != null)
            {
                OnChangedLoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signedIn)
            {
                displayName = user.DisplayName ?? string.Empty;
                emailAddress = user.Email ?? string.Empty;
                photoUrl = user.PhotoUrl ?? null;

                OnChangedLoginState?.Invoke(true);
            }
        }
    }
}
