using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    string url = "https://gist.github.com/ShoseiAtsumi/1423d5548fd69c26c1cb009a4e727b65";
    public void Openurl()
    {
        Application.OpenURL(url);
    }
}
