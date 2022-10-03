using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Emitters
{
    public class EndGameCanvas : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene("Main Menu Scene");
            }
        }
    }
}