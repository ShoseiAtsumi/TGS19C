using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class GoResult : MonoBehaviour
{
    [SerializeField]
    private Button goResultButton; 

    private void Start()
    {
        goResultButton.OnClickAsObservable()
            .Subscribe(_ => SceneManager.LoadScene("Title"))
            .AddTo(gameObject);
    }
}
