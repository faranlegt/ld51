using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Emitters
{
    public class MainMenu : MonoBehaviour
    {
        public int currentImage = 0;
        
        public Sprite[] spritesList = { };

        public Image actualImage;

        private void Start()
        {
            actualImage.sprite = spritesList[currentImage];
        }

        private void Update()
        {
            if (!Keyboard.current.anyKey.wasPressedThisFrame)
                return;

            currentImage++;
                
            if (currentImage >= spritesList.Length)
            {
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                actualImage.sprite = spritesList[currentImage];
            }
        }
    }
}