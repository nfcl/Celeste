using Managers;
using Metric;
using ObjectPools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public delegate void OptionClick();

    private class Option
    {

        public string Name;
        public Option Parent;
        public bool IsParent;
        public List<Option> SubOption;
        public OptionClick callBack;

    }

    private static OptionItem OptionPrefab;

    [SerializeField] private Transform OptionContainer;

    private Option _root;

    private Option CurrentOption;

    private int CurrentIndex;

    private List<OptionItem> items;

    private Coroutine InputEnumerator;

    public void Init()
    {

        _root = new Option
        {

            Name = "root",
            Parent = null,
            IsParent = true,
            SubOption = new List<Option>
            {
                new Option
                {
                    Name = "返回游戏",
                    Parent =null,
                    IsParent = false,
                    SubOption = null,
                    callBack = delegate { GameSceneManager.instance.SetMenuVisible(false); }
                },
                new Option
                {
                    Name = "重试",
                    Parent = null,
                    IsParent = false,
                    SubOption = null,
                    callBack = delegate{ if(GameSceneManager.instance.TryPlayerDead()){ GameSceneManager.instance.SetMenuVisible(false); } }
                },
                new Option
                {
                    Name = "退出",
                    Parent =null,
                    IsParent = false,
                    SubOption = null,
                    callBack = delegate{ GameSceneManager.instance.ReturnMap(); }
                }
            },
            callBack = null

        };

        CurrentOption = _root;

        items = new List<OptionItem>();

        InputEnumerator = null;

    }

    public void DeleteMenu()
    {

        if (items != null)
        {

            for (int i = 0; i < items.Count; ++i)
            {

                MenuOptionPool.instance.ReturnInstance(items[i]);

            }

            items.Clear();

        }

        Transform child;

        OptionItem childItem;

        while (OptionContainer.childCount > 0)
        {

            child = OptionContainer.GetChild(0);

            childItem = child.GetComponent<OptionItem>();

            if (childItem != null)
            {

                MenuOptionPool.instance.ReturnInstance(childItem);

            }
            else
            {

                DestroyImmediate(child.gameObject);

            }

        }

    }

    public void Open()
    {

        gameObject.SetActive(true);
        CurrentOption = _root;
        CurrentIndex = 0;
        ShowMenu();

    }

    public void Close()
    {

        if (InputEnumerator != null)
        {

            StopCoroutine(InputEnumerator);

        }

        gameObject.SetActive(false);

    }

    private void ShowMenu()
    {

        if(InputEnumerator != null)
        {

            StopCoroutine(InputEnumerator);

        }

        DeleteMenu();

        OptionItem clone = null;

        if (CurrentOption.IsParent == false || CurrentOption.SubOption == null)
        {

            throw new System.Exception("");

        }

        float ItemHeight = 40f;
        float ItemDistance = 30f;
        float Y = (CurrentOption.SubOption.Count - 1) * (ItemHeight + ItemDistance) / 2;

        for(int i = 0; i < CurrentOption.SubOption.Count; ++i)
        {

            clone = MenuOptionPool.instance.GetInstance();

            clone.transform.SetParent(OptionContainer);

            clone.transform.localPosition = new Vector2(0, Y);

            clone.OriginY = Y;

            clone.Name = CurrentOption.SubOption[i].Name;

            clone.IsSelected = false;

            Y -= ItemHeight + ItemDistance;

            items.Add(clone);

        }

        CurrentIndex = 0;

        SelectOption();

        InputEnumerator = StartCoroutine(InputListener());

    }

    private void SelectOption()
    {

        for (int i = 0; i < CurrentOption.SubOption.Count; ++i)
        {

            items[i].IsSelected = false;

        }

        items[CurrentIndex].IsSelected = true;

    }

    private IEnumerator InputListener()
    {

        while (true)
        {

            yield return new WaitUntil(
                () =>
                {
                    return
                           Input.GetKeyDown(MenuControl.ConfirmButton) == true
                        || Input.GetKeyDown(MenuControl.CancelButton) == true
                        || Input.GetKeyDown(MenuControl.UpButton) == true
                        || Input.GetKeyDown(MenuControl.DownButton) == true
                        || Input.GetKeyDown(MenuControl.LeftButton) == true
                        || Input.GetKeyDown(MenuControl.RightButton) == true;
                }
            );

            if (Input.GetKeyDown(MenuControl.ConfirmButton) == true)
            {

                if (CurrentOption.SubOption[CurrentIndex].IsParent == true)
                {

                    CurrentOption = CurrentOption.SubOption[CurrentIndex];

                    ShowMenu();

                }

                CurrentOption.SubOption[CurrentIndex]?.callBack();

            }
            else if (Input.GetKeyDown(MenuControl.CancelButton) == true)
            {

                if(CurrentOption.Parent != null)
                {

                    CurrentOption = CurrentOption.Parent;

                    ShowMenu();

                }

            }
            else if (Input.GetKeyDown(MenuControl.UpButton) == true)
            {

                if(CurrentIndex > 0)
                {

                    CurrentIndex -= 1;

                    SelectOption();

                }

            }
            else if (Input.GetKeyDown(MenuControl.DownButton) == true)
            {

                if (CurrentIndex < CurrentOption.SubOption.Count - 1)
                {

                    CurrentIndex += 1;

                    SelectOption();

                }

            }

        }

    }

    public void Awake()
    {

        OptionPrefab = Resources.Load<OptionItem>("Menu/MenuOption");

    }

}
