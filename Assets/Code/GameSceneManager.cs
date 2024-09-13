using Metric;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameSceneManager : MonoBehaviour
    {

        public static GameSceneManager instance;

        [SerializeField] private n_Player.Player player;

        [SerializeField] private MenuManager menuManager;

        [SerializeField] private TimeKeeper timeKeeper;

        [SerializeField] private StrawberryManager strawberryManager;

        private bool IsShowMenu;
        
        public CutScene cutScene;

        public void Start()
        {

            menuManager.Init();

            strawberryManager.Init();

            IsShowMenu = false;

            GameSceneMusicManager.instance.bgm.Play();

            timeKeeper.enabled = true;

            if(Metric.SceneOnloadVarible.GameScene.CurrentSave == null)
            {

                timeKeeper.Init(new System.TimeSpan(), new System.TimeSpan());

            }
            else
            {

                timeKeeper.Init(Metric.SceneOnloadVarible.GameScene.CurrentSave.GetTimeSpent(), new System.TimeSpan());

            }

            timeKeeper.Open();

            StartCoroutine(InputListener());

        }
        
        public void SetMenuVisible(bool visible)
        {

            IsShowMenu = visible;

            if (IsShowMenu == true)
            {

                Time.timeScale = 0;
                menuManager.Open();
                GameSceneMusicManager.instance.GamePause();
                strawberryManager.Generate();

            }
            else
            {

                Time.timeScale = 1;
                GameSceneMusicManager.instance.GameContinue();
                menuManager.Close();

            }

        }

        public bool TryPlayerDead()
        {

            return player.TryDead();

        }

        private IEnumerator InputListener()
        {

            bool[] MenuControlButton = new bool[2] { false, false };

            while (true)
            {

                yield return new WaitUntil(

                    () =>
                    {

                        MenuControlButton[1] = Input.GetKeyDown(MenuControl.MenuControlButton) == true;

                        if (MenuControlButton[0] == false && MenuControlButton[1] == true)
                        {

                            MenuControlButton[0] = MenuControlButton[1];

                            return true;

                        }
                        else
                        {

                            MenuControlButton[0] = MenuControlButton[1];

                            return false;

                        }

                    }

                );

                if (player.IsControlable == true)
                {

                    SetMenuVisible(!IsShowMenu);

                }

            }

        }

        public void ReturnMap()
        {

            Metric.SceneOnloadVarible.GameScene.CurrentSide.SideRecord.TimeSpent += timeKeeper.AddingTime;

            Metric.SceneOnloadVarible.GameScene.CurrentSide.SideRecord.DeathNum += player.DeathNum;

            strawberryManager.Save();

            Archive.Save();

            GameSceneMusicManager.instance.FadeOut();

            cutScene.ToggleChange(
                true,
                CutScene.Kind.Lvl1,
                2,
                delegate ()
                {

                    Time.timeScale = 1;

                    SceneManager.LoadScene("Main");

                }, 
                true
            );

        }

        public void Awake()
        {

            instance = this;

        }

    }

}

